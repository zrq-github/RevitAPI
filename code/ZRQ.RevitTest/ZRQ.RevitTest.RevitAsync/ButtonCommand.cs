using Autodesk.Revit.DB;
using Revit.Async;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ZRQ.RevitTest.RevitAsync
{
    public class ButtonCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        //public async void Execute(object parameter)
        //{
        //    //  直接在按钮响应中执行Revit API代码，将会得到一个异常，告诉你现在不是在Revit API的执行上下文中，无法执行Revit API
        //    //  常规做法是Raise一个包含Revit API业务逻辑的外部事件
        //    //ExternalCommand.SomeEvent.Raise();

        //    //await 是.NET 4.5 的关键字, 如果是基于.NET 4.0的，请使用ContinueWith
        //    var families = await RevitTask.RunAsync(
        //        app =>
        //        {
        //            //在这里书写Revit API代码

        //            //这里利用了Lambda表达式创建的闭包上下文,
        //            //使得我们可以访问按钮点击事件传入的参数，以及所有的局部变量
        //            //假设点击按钮传入的是个bool值，用来指示是否过滤出可编辑的族
        //            if (parameter is bool editable)
        //            {
        //                return new FilteredElementCollector(app.ActiveUIDocument.Document).OfType<Family>().OfType<Family>().Cast<Family>().Where(family => editable ? family.IsEditable : true).ToList();
        //            }

        //            return null;
        //        });

        //    MessageBox.Show($"Family count: {families?.Count ?? 0}");
        //}

        public async void Execute(object parameter)
        {
            var savePath = await RevitTask.RunAsync(
                async app =>
                {
                    try
                    {
                        var document = app.ActiveUIDocument.Document;
                        var randomFamily = await RevitTask.RunAsync(
                            () =>
                            {
                                var families = new FilteredElementCollector(document)
                                    .OfClass(typeof(Family))
                                    .Cast<Family>()
                                    .Where(family => family.IsEditable)
                                    .ToArray();
                                var random = new Random(Environment.TickCount);
                                return families[random.Next(0, families.Length)];
                            });

                    //Raise外部事件，传入参数，await这个异步任务，接收回调结果
                        return await RevitTask.RaiseGlobal<SaveFamilyToDesktopExternalEventHandler, Family, string>(randomFamily);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                });
            var saveResult = !string.IsNullOrWhiteSpace(savePath);
            MessageBox.Show($"Family {(saveResult ? "" : "not ")}saved:\n{savePath}");
            if (saveResult)
            {
                Process.Start(Path.GetDirectoryName(savePath));
            }
        }
    }
}