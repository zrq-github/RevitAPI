using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI.Selection;
using RQ.Test.RevtDotNet.测试部件Transfrom.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace RQ.RevtDotNet.Test.PMEP.Commands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    internal class TestParallelFlangeEC : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;
            Autodesk.Revit.DB.View view = uiApp.ActiveUIDocument.ActiveView;

            FamilyInstance flange1 = doc.GetElement(new ElementId(786662)) as FamilyInstance; // 左法兰
            Pipe pipe = doc.GetElement(new ElementId(786764)) as Pipe; // 右法兰

            // reference1
            Connector mainConnector1 = GetMainConnector(flange1);
            Face face1 = GetFace(doc, flange1, mainConnector1.CoordinateSystem.BasisZ);
            Reference reference1 = face1.Reference;

            // reference2
            Reference reference2 = new Reference(pipe);

            var refArray = new ReferenceArray();
            refArray.Append(reference1);
            refArray.Append(reference2);

            // Line
            // 作为标注的线位置
            Pipe pipeEle = doc.GetElement(new ElementId(786795)) as Pipe;
            Line pipeLine = (pipeEle.Location as LocationCurve).Curve as Line;

            XYZ first = (flange1.Location as LocationPoint).Point;
            //XYZ second = (flange2.Location as LocationPoint).Point;
            XYZ direction = pipeLine.Direction;

            using (var tran = new Transaction(doc, "Test"))
            {
                tran.Start();
                Line line = Line.CreateUnbound(first, direction);
                doc.Create.NewDimension(doc.ActiveView, line, refArray);
                tran.Commit();
            }


            return Result.Succeeded;
        }

        private void ReferenceSuface(Document doc)
        {
            FamilyInstance flange1 = doc.GetElement(new ElementId(786662)) as FamilyInstance; // 左法兰
            FamilyInstance flange2 = doc.GetElement(new ElementId(786680)) as FamilyInstance; // 右法兰

            // reference1
            Connector mainConnector1 = GetMainConnector(flange1);
            Face face1 = GetFace(doc, flange1, mainConnector1.CoordinateSystem.BasisZ);
            Reference reference1 = face1.Reference;

            // reference2
            Connector mainConnector2 = GetMainConnector(flange2);
            Face face2 = GetFace(doc, flange2, mainConnector2.CoordinateSystem.BasisZ);
            Reference reference2 = face2.Reference;

            var refArray = new ReferenceArray();
            refArray.Append(reference1);
            refArray.Append(reference2);

            XYZ first = (flange1.Location as LocationPoint).Point;
            XYZ second = (flange2.Location as LocationPoint).Point;
            XYZ direction = first - second;

            using (var tran = new Transaction(doc, "Test"))
            {
                tran.Start();
                Line line = Line.CreateUnbound(first, direction);
                doc.Create.NewDimension(doc.ActiveView, line, refArray);
                tran.Commit();
            }
        }

        /// <summary>
        /// 获取管件的主连接件
        /// </summary>
        /// <param name="fit"></param>
        /// <returns></returns>
        private Connector GetMainConnector(FamilyInstance fit)
        {
            Connector mainConnector = null;
            ConnectorSet fitConnSets = fit.MEPModel.ConnectorManager.Connectors;
            ConnectorSetIterator iter = fitConnSets.ForwardIterator();
            while (iter.MoveNext())
            {
                Connector c = iter.Current as Connector;
                if (c.GetMEPConnectorInfo().IsPrimary)
                {
                    mainConnector = c;
                    break;
                }
            }
            return mainConnector;
        }

        private Face GetFace(Document doc, FamilyInstance flange1, XYZ faceDir)
        {
            Options geomOptions = new Options();
            geomOptions.ComputeReferences = true;
            geomOptions.DetailLevel = ViewDetailLevel.Medium;
            geomOptions.IncludeNonVisibleObjects = true;

            List<Tuple<Face, XYZ>> faceAndDirList = new List<Tuple<Face, XYZ>>();

            GeometryElement geoElement = flange1.get_Geometry(geomOptions);
            foreach (GeometryObject obj in geoElement)
            {
                if (obj is GeometryInstance)
                {
                    GeometryInstance geoInstance = obj as GeometryInstance;
                    if (geoInstance != null)
                    {
                        GeometryElement geoSymbol = geoInstance.GetSymbolGeometry();
                        if (geoSymbol != null)
                        {
                            foreach (GeometryObject geomObj in geoSymbol)
                            {
                                if (geomObj is Solid)
                                {
                                    Solid solid = geomObj as Solid;
                                    if (solid.Faces.Size > 0)
                                    {
                                        Transform trans = geoInstance.Transform;
                                        foreach (Face face in solid.Faces)
                                        {
                                            var dir = trans.OfVector(face.ComputeNormal(new UV(0, 0)));
                                            if (face is PlanarFace)
                                            {
                                                if (faceDir.IsAlmostEqualTo(faceDir, 0.1))
                                                {
                                                    return face;
                                                }

                                                faceAndDirList.Add(new Tuple<Face, XYZ>(face, dir));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Face aa = null;
            return aa;
        }

        public static List<Tuple<Face, XYZ>> GetSpecialFamilyReference(Document doc, FamilyInstance instance, ref Transform trans, Transform docTrans = null)
        {
            List<Tuple<Face, XYZ>> list = new List<Tuple<Face, XYZ>>();

            Options geomOptions = new Options();
            geomOptions.ComputeReferences = true;
            geomOptions.DetailLevel = ViewDetailLevel.Medium;
            geomOptions.IncludeNonVisibleObjects = true;
            trans = instance.GetTotalTransform();
            GeometryElement geoElement = instance.get_Geometry(geomOptions);

            foreach (GeometryObject obj in geoElement)
            {
                if (obj is GeometryInstance)
                {
                    GeometryInstance geoInstance = obj as GeometryInstance;
                    trans = geoInstance.Transform;
                    GeometryElement geoSymbol = geoInstance.GetSymbolGeometry();

                    if (geoSymbol == null)
                    {
                        continue;
                    }
                    foreach (GeometryObject geomObj in geoSymbol)
                    {
                        if (geomObj is Solid)
                        {
                            Solid solid = geomObj as Solid;
                            if (solid.Faces.Size > 0)
                            {
                                foreach (Face item in solid.Faces)
                                {
                                    var dir = trans.OfVector(item.ComputeNormal(new UV(0, 0)));
                                    if (docTrans != null)
                                    {
                                        dir = docTrans.OfVector(dir);
                                    }
                                    list.Add(new Tuple<Face, XYZ>(item, dir));
                                }
                            }
                        }

                    }
                }

            }
            if (list.Count == 0)
            {
                trans = null;
                foreach (GeometryObject obj in geoElement)
                {
                    if (obj is Solid)
                    {
                        Solid solid = obj as Solid;
                        if (solid.Faces.Size > 0)
                        {
                            foreach (Face item in solid.Faces)
                            {
                                var dir = item.ComputeNormal(new UV(0, 0));
                                if (docTrans != null)
                                {
                                    dir = docTrans.OfVector(dir);
                                }
                                list.Add(new Tuple<Face, XYZ>(item, dir));
                            }
                        }
                    }
                }
            }
            return list;
        }

        private void FlangeDim2(Document doc, Autodesk.Revit.DB.View view)
        {
            // 作为标注的线位置
            Pipe pipeEle = doc.GetElement(new ElementId(786475)) as Pipe;
            Line pipeLine = (pipeEle.Location as LocationCurve).Curve as Line;

            FamilyInstance flange1 = doc.GetElement(new ElementId(786126)) as FamilyInstance; // 左法兰
            FamilyInstance flange2 = doc.GetElement(new ElementId(785585)) as FamilyInstance; // 右法兰
        }

        private static void PipeDim(Document doc, Autodesk.Revit.DB.View view)
        {
            Pipe pipeEle = doc.GetElement(new ElementId(786209)) as Pipe;
            Line pipeLine = (pipeEle.Location as LocationCurve).Curve as Line;

            Pipe pipeEle1 = doc.GetElement(new ElementId(786295)) as Pipe;
            Pipe pipeEle2 = doc.GetElement(new ElementId(773738)) as Pipe;

            Reference reference1 = new Reference(pipeEle1);
            Reference reference2 = new Reference(pipeEle2);

            ReferenceArray referenceArray = new ReferenceArray();
            referenceArray.Append(reference1);
            referenceArray.Append(reference2);

            Transaction transaction = new Transaction(doc, nameof(TestParallelFlangeEEH));
            transaction.Start();
            doc.Create.NewDimension(view, pipeLine, referenceArray);
            TransactionStatus transactionStatus = transaction.Commit();
        }

        public static Reference GetSpecialFamilyReference(Document doc, FamilyInstance instance, SpecialReferenceType ReferenceType)
        {
            Reference indexReference = null;
            int index = (int)ReferenceType;

            Options geomOptions = new Options();
            geomOptions.ComputeReferences = true;
            geomOptions.DetailLevel = ViewDetailLevel.Medium;
            geomOptions.IncludeNonVisibleObjects = true;

            GeometryElement geoElement = instance.get_Geometry(geomOptions);
            foreach (GeometryObject obj in geoElement)
            {
                if (obj is GeometryInstance)
                {
                    GeometryInstance geoInstance = obj as GeometryInstance;

                    String sampleStableRef = null;

                    if (geoInstance != null)
                    {
                        GeometryElement geoSymbol = geoInstance.GetSymbolGeometry();

                        if (geoSymbol != null)
                        {
                            foreach (GeometryObject geomObj in geoSymbol)
                            {
                                if (geomObj is Solid)
                                {
                                    Solid solid = geomObj as Solid;

                                    if (solid.Faces.Size > 0)
                                    {
                                        Face face = solid.Faces.get_Item(0);

                                        sampleStableRef = face.Reference.ConvertToStableRepresentation(doc);
                                        break;
                                    }
                                }

                            }
                        }

                        if (sampleStableRef != null)
                        {
                            String[] refTokens = sampleStableRef.Split(new char[] { ':' });

                            String customStableRef = refTokens[0] + ":" + refTokens[1] + ":" + refTokens[2] + ":" + refTokens[3] + ":" + index.ToString();

                            indexReference = Reference.ParseFromStableRepresentation(doc, customStableRef);

                        }
                        break;
                    }
                    else
                    {

                    }
                }
            }

            return indexReference;
        }

        private static void FlangeDim(Document doc, Autodesk.Revit.DB.View view)
        {
            // 作为标注的线位置
            Pipe pipeEle = doc.GetElement(new ElementId(786475)) as Pipe;
            Line pipeLine = (pipeEle.Location as LocationCurve).Curve as Line;

            FamilyInstance flange1 = doc.GetElement(new ElementId(786126)) as FamilyInstance; // 左法兰
            FamilyInstance flange2 = doc.GetElement(new ElementId(785585)) as FamilyInstance; // 右法兰

            List<Reference> flange1References1 = flange1.GetReferences(FamilyInstanceReferenceType.Left).ToList();
            List<Reference> flange1References2 = flange1.GetReferences(FamilyInstanceReferenceType.CenterLeftRight).ToList();
            List<Reference> flange1References3 = flange1.GetReferences(FamilyInstanceReferenceType.Right).ToList();
            List<Reference> flange1References4 = flange1.GetReferences(FamilyInstanceReferenceType.Front).ToList();
            List<Reference> flange1References5 = flange1.GetReferences(FamilyInstanceReferenceType.CenterFrontBack).ToList();
            List<Reference> flange1References6 = flange1.GetReferences(FamilyInstanceReferenceType.Back).ToList();
            List<Reference> flange1References7 = flange1.GetReferences(FamilyInstanceReferenceType.Bottom).ToList();
            List<Reference> flange1References8 = flange1.GetReferences(FamilyInstanceReferenceType.CenterElevation).ToList();
            List<Reference> flange1References9 = flange1.GetReferences(FamilyInstanceReferenceType.Top).ToList();
            List<Reference> flange1References10 = flange1.GetReferences(FamilyInstanceReferenceType.StrongReference).ToList();
            List<Reference> flange1References11 = flange1.GetReferences(FamilyInstanceReferenceType.WeakReference).ToList();
            List<Reference> flange1References12 = flange1.GetReferences(FamilyInstanceReferenceType.NotAReference).ToList();

            Reference reference1 = new Reference(flange1);
            Reference reference2 = new Reference(flange2);


            Transaction transaction = new Transaction(doc, nameof(TestParallelFlangeEEH));
            transaction.Start();

            ReferenceArray referenceArray = new ReferenceArray();
            referenceArray.Append(reference1);
            referenceArray.Append(reference2);

            doc.Create.NewDimension(view, pipeLine, referenceArray);

            TransactionStatus transactionStatus = transaction.Commit();
        }
    }

    public class TestParallelFlangeEEH : IExternalEventHandler
    {
        public static readonly ExternalEvent FlangeAlignEvnet = ExternalEvent.Create(new TestParallelFlangeEEH());

        public void Execute(UIApplication uiApp)
        {
            Document doc = uiApp.ActiveUIDocument.Document;
            Autodesk.Revit.DB.View view = uiApp.ActiveUIDocument.ActiveView;

            // 作为标注的线位置
            Pipe pipeEle = doc.GetElement(new ElementId(786178)) as Pipe;
            Line pipeLine = (pipeEle.Location as LocationCurve).Curve as Line;

            FamilyInstance flange1 = doc.GetElement(new ElementId(786126)) as FamilyInstance; // 左法兰
            FamilyInstance flange2 = doc.GetElement(new ElementId(785585)) as FamilyInstance; // 右法兰

            List<Reference> flange1References = flange1.GetReferences(FamilyInstanceReferenceType.CenterElevation).ToList();
            List<Reference> flange2References = flange2.GetReferences(FamilyInstanceReferenceType.CenterElevation).ToList();

            Transaction transaction = new Transaction(doc, nameof(TestParallelFlangeEEH));
            transaction.Start();

            ReferenceArray referenceArray = new ReferenceArray();
            referenceArray.Append(flange1References[0]);
            referenceArray.Append(flange2References[0]);

            doc.Create.NewDimension(view, pipeLine, referenceArray);

            transaction.Commit();

        }

        public string GetName()
        {
            return nameof(TestParallelFlangeEEH);
        }
    }


}
