using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Newtonsoft.Json;
using RQ.RevitUtils.ExtensibleStorageUtils.SchemaWrapperTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.RevitUtils.ExtensibleStorageUtils
{
    [Serializable]
    /// <summary>
    /// 扩展存储的基本类
    /// </summary>
    /// <remarks>
    /// 2021年10月13日 zrq:
    /// 所有的数据结构全部封装在StorageDictionary中
    /// 在用途上其实StorageDictionary就等于一个大型的Class类，Dictionary里面的子项就等于大类中的类，
    /// 使用Dictionary而不使用大类包小类的，主要是降低对类的负担，拆分多个小类，更加灵活一点
    /// </remarks>
    public abstract class ExtendStorageManage
    {
        #region must data

        /// <summary>
        /// 读取权限
        /// </summary>
        protected abstract AccessLevel ReadLevel { get; }

        /// <summary>
        /// 写入权限
        /// </summary>
        protected abstract AccessLevel WriteLevle { get; }

        /// <summary>
        /// 扩展表的GUID
        /// </summary>
        public abstract Guid SchemaGuid { get; }

        /// <summary>
        /// 扩展文档名字(作为版本号使用)
        /// </summary>
        /// <remarks>这里应该是能通过<see cref="double.Parse(string)"/>解析的字符串</remarks>
        protected abstract string StorageVersion { get; }

        /// <summary>
        /// 扩展数据的名字
        /// </summary>
        protected abstract string StorageName { get; }

        /// <summary>
        /// AddinID
        /// </summary>
        /// <remarks>因为不是必须的 暂时做成 虚函数</remarks>
        protected abstract string ApplicationId { get; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        protected virtual string VendorId { get; set; } = "com.hwbim.family";

        /// <summary>
        /// 扩展数据字典
        /// </summary>
        /// <remarks>
        /// 2021年10月12日 -zrq
        /// 为什么这么设计？可以解决将统一功能的扩展数据全部封装在一起
        /// </remarks>
        public IDictionary<string, string> StorageDictionary { get; set; }

        public IList<string> StorageList { get; set; }

        public string StorageString { get; set; }

        #endregion

        #region 初始化需要使用的函数
        private void NewSchema()
        {
            Schema schema = Schema.Lookup(SchemaGuid);
            if (schema != null)
            {
                Schema = schema;
            }
            else
            {
                SchemaWrapperTools.SchemaWrapper mySchemaWrapper = SchemaWrapperTools.SchemaWrapper.NewSchema(SchemaGuid, ReadLevel, WriteLevle, VendorId, ApplicationId, StorageName, StorageVersion);

                // 每个扩展数据必须需要的字段
                ConstraintAddField(mySchemaWrapper, SchemaGuid, ReadLevel, WriteLevle, VendorId, ApplicationId, StorageName, StorageVersion);

                // 各个产品写自己的不同的数据结构
                //CreateField(mySchemaWrapper, SchemaGuid, ReadLevel, WriteLevle, VendorId, ApplicationId, StorageName, StorageVersion);

                mySchemaWrapper.FinishSchema();
                schema = mySchemaWrapper.GetSchema();
                Schema = schema;
            }
        }

        private void ConstraintAddField(SchemaWrapper mySchemaWrapper, Guid schemaGuid, AccessLevel readLevel, AccessLevel writeLevle, string vendorId, string applicationId, string storageName, string storageVersion)
        {
            mySchemaWrapper.AddField<string>(nameof(StorageVersion), GetUndefinedUnitType(), mySchemaWrapper);
            mySchemaWrapper.AddField<IDictionary<string, string>>(nameof(StorageDictionary), GetUndefinedUnitType(), mySchemaWrapper);
            mySchemaWrapper.AddField<IList<string>>(nameof(StorageList), GetUndefinedUnitType(), mySchemaWrapper);
            mySchemaWrapper.AddField<string>(nameof(StorageString), GetUndefinedUnitType(), mySchemaWrapper);
        }
#if (REVIT2016 || REVIT2017 || REVIT2018 || REVIT2019 || REVIT2020 || REVIT2021)
        public static UnitType GetLengthUnitType()
        {
            return UnitType.UT_Length;
        }
        public static UnitType GetUndefinedUnitType()
        {
            return UnitType.UT_Undefined;
        }
#else
        internal static ForgeTypeId GetLengthUnitType()
        {
            return typeId("autodesk.spec.aec:length-2.0.0");
        }
        internal static ForgeTypeId GetUndefinedUnitType()
        {
            return typeId("autodesk.spec.aec:undefined-2.0.0");
        }
        private static ForgeTypeId typeId(string key)
        {
            IList<ForgeTypeId> AllMeasurableSpecs = UnitUtils.GetAllMeasurableSpecs();
            ForgeTypeId forgeTypeId = AllMeasurableSpecs.FirstOrDefault(x => x.TypeId.Equals(key));
            return forgeTypeId;
        }
#endif       
        /// <summary>
        /// 创建扩展表的例子-预留函数，目前应该用不上
        /// </summary>
        /// <remarks>
        /// 每个产品自己控制生成表格类型
        /// 这个虚函数内部写的是例子
        /// </remarks>
        protected virtual void CreateField(SchemaWrapperTools.SchemaWrapper mySchemaWrapper, Guid schemaId, AccessLevel readAccess, AccessLevel writeAccess, string vendorId, string applicationId, string storageName, string storageVersion)
        {
            #region Field names and schema guids used in sample schemas
            string int0Name = "int0Name";
            string double0Name = "double0Name";
            string bool0Name = "bool0Name";
            string string0Name = "string0Name";
            string id0Name = "id0Name";
            string point0Name = "point0Name";
            string uv0Name = "uv0Name";
            string float0Name = "float0Name";
            string short0Name = "short0Name";
            string guid0Name = "guid0Name";
            string map0Name = "map0Name";
            string array0Name = "array0Name";
            #endregion
            int s_counter = DateTime.Now.Second;
            Guid NewGuid()
            {

                byte[] guidBytes = new byte[16];
                Random randomGuidBytes = new Random(s_counter);
                randomGuidBytes.NextBytes(guidBytes);
                s_counter++;
                return new Guid(guidBytes);
            }

            Guid subEntityGuid0 = NewGuid();
            string entity0Name = "entity0Name";

            Guid subEntityGuid_Map1 = NewGuid();
            string entity1Name_Map = "entity1Name_Map";

            Guid subEntityGuid_Array2 = NewGuid();
            string entity2Name_Array = "entity2Name_Array";

            string array1Name = entity2Name_Array;
            string map1Name = entity1Name_Map;

            #region Add Fields to the SchemaWrapper
            mySchemaWrapper.AddField<int>(int0Name, GetUndefinedUnitType(), null);
            mySchemaWrapper.AddField<short>(short0Name, GetUndefinedUnitType(), null);
            mySchemaWrapper.AddField<double>(double0Name, GetUndefinedUnitType(), null);
            mySchemaWrapper.AddField<float>(float0Name, GetUndefinedUnitType(), null);
            mySchemaWrapper.AddField<bool>(bool0Name, GetUndefinedUnitType(), null);
            mySchemaWrapper.AddField<string>(string0Name, GetUndefinedUnitType(), null);
            mySchemaWrapper.AddField<ElementId>(id0Name, GetUndefinedUnitType(), null);
            mySchemaWrapper.AddField<XYZ>(point0Name, GetUndefinedUnitType(), null);
            mySchemaWrapper.AddField<UV>(uv0Name, GetUndefinedUnitType(), null);
            mySchemaWrapper.AddField<Guid>(guid0Name, GetUndefinedUnitType(), null);

            //Note that we use IDictionary<> for map types and IList<> for array types
            mySchemaWrapper.AddField<IDictionary<string, string>>(map0Name, GetUndefinedUnitType(), null);
            mySchemaWrapper.AddField<IList<bool>>(array0Name, GetUndefinedUnitType(), null);

            //Create a sample subEntity
            SchemaWrapperTools.SchemaWrapper mySubSchemaWrapper0 = SchemaWrapperTools.SchemaWrapper.NewSchema(subEntityGuid0, readAccess, writeAccess, vendorId, applicationId, entity0Name, "A sub entity");
            mySubSchemaWrapper0.AddField<int>("subInt0", GetUndefinedUnitType(), null);
            mySubSchemaWrapper0.FinishSchema();
            Entity subEnt0 = new Entity(mySubSchemaWrapper0.GetSchema());
            subEnt0.Set<int>(mySubSchemaWrapper0.GetSchema().GetField("subInt0"), 11);
            mySchemaWrapper.AddField<Entity>(entity0Name, GetUndefinedUnitType(), mySubSchemaWrapper0);

            //
            //Create a sample map of subEntities (An IDictionary<> with key type "int" and value type "Entity")
            //
            //Create a new sample schema.
            SchemaWrapperTools.SchemaWrapper mySubSchemaWrapper1_Map = SchemaWrapperTools.SchemaWrapper.NewSchema(subEntityGuid_Map1, readAccess, writeAccess, vendorId, applicationId, map1Name, "A map of int to Entity");
            mySubSchemaWrapper1_Map.AddField<int>("subInt1", GetUndefinedUnitType(), null);
            mySubSchemaWrapper1_Map.FinishSchema();
            //Create a new sample Entity.
            Entity subEnt1 = new Entity(mySubSchemaWrapper1_Map.GetSchema());
            //Set data in that entity.
            subEnt1.Set<int>(mySubSchemaWrapper1_Map.GetSchema().GetField("subInt1"), 22);
            //Add a new map field to the top-level Schema.  We will add the entity we just created after all top-level
            //fields are created.
            mySchemaWrapper.AddField<IDictionary<int, Entity>>(map1Name, GetUndefinedUnitType(), mySubSchemaWrapper1_Map);


            //
            //Create a sample array of subentities (An IList<> of type "Entity")
            //
            //Create a new sample schema
            SchemaWrapperTools.SchemaWrapper mySubSchemaWrapper2_Array = SchemaWrapperTools.SchemaWrapper.NewSchema(subEntityGuid_Array2, readAccess, writeAccess, vendorId, applicationId, array1Name, "An array of Entities");
            mySubSchemaWrapper2_Array.AddField<int>("subInt2", GetUndefinedUnitType(), null);
            mySubSchemaWrapper2_Array.FinishSchema();
            //Create a new sample Entity.
            Entity subEnt2 = new Entity(mySubSchemaWrapper2_Array.GetSchema());
            //Set the data in that Entity.
            subEnt2.Set<int>(mySubSchemaWrapper2_Array.GetSchema().GetField("subInt2"), 33);
            //Add a new array field to the top-level Schema We will add the entity we just crated after all top-level fields
            //are created.
            mySchemaWrapper.AddField<IList<Entity>>(array1Name, GetUndefinedUnitType(), mySubSchemaWrapper2_Array);

            #endregion

            #region Populate the Schema in the SchemaWrapper with data
            mySchemaWrapper.FinishSchema();
            #endregion

            //#region Create a new entity to store an instance of schema data
            //storageElementEntityWrite = null;

            //storageElementEntityWrite = new Entity(mySchemaWrapper.GetSchema());
            //#endregion

            //#region Get fields and set data in them
            //Field fieldInt0 = mySchemaWrapper.GetSchema().GetField(int0Name);
            //Field fieldShort0 = mySchemaWrapper.GetSchema().GetField(short0Name);
            //Field fieldDouble0 = mySchemaWrapper.GetSchema().GetField(double0Name);
            //Field fieldFloat0 = mySchemaWrapper.GetSchema().GetField(float0Name);

            //Field fieldBool0 = mySchemaWrapper.GetSchema().GetField(bool0Name);
            //Field fieldString0 = mySchemaWrapper.GetSchema().GetField(string0Name);

            //Field fieldId0 = mySchemaWrapper.GetSchema().GetField(id0Name);
            //Field fieldPoint0 = mySchemaWrapper.GetSchema().GetField(point0Name);
            //Field fieldUv0 = mySchemaWrapper.GetSchema().GetField(uv0Name);
            //Field fieldGuid0 = mySchemaWrapper.GetSchema().GetField(guid0Name);

            //Field fieldMap0 = mySchemaWrapper.GetSchema().GetField(map0Name);
            //Field fieldArray0 = mySchemaWrapper.GetSchema().GetField(array0Name);

            //Field fieldEntity0 = mySchemaWrapper.GetSchema().GetField(entity0Name);

            //Field fieldMap1 = mySchemaWrapper.GetSchema().GetField(map1Name);
            //Field fieldArray1 = mySchemaWrapper.GetSchema().GetField(array1Name);


            //storageElementEntityWrite.Set<int>(fieldInt0, 5);
            //storageElementEntityWrite.Set<short>(fieldShort0, 2);

            //storageElementEntityWrite.Set<double>(fieldDouble0, 7.1, DisplayUnitType.DUT_METERS);
            //storageElementEntityWrite.Set<float>(fieldFloat0, 3.1f, DisplayUnitType.DUT_METERS);


            //storageElementEntityWrite.Set(fieldBool0, false);
            //storageElementEntityWrite.Set(fieldString0, "hello");
            ////storageElementEntityWrite.Set(fieldId0, storageElement.Id);
            //storageElementEntityWrite.Set(fieldPoint0, new XYZ(1, 2, 3), DisplayUnitType.DUT_METERS);
            //storageElementEntityWrite.Set(fieldUv0, new UV(1, 2), DisplayUnitType.DUT_METERS);
            //storageElementEntityWrite.Set(fieldGuid0, new Guid("D8301329-F207-43B8-8AA1-634FD344F350"));

            ////Note that we must pass an IDictionary<>, not a Dictionary<> to Set().
            //IDictionary<string, string> myMap0 = new Dictionary<string, string>();
            //myMap0.Add("mykeystr", "myvalstr");
            //storageElementEntityWrite.Set(fieldMap0, myMap0);

            ////Note that we must pass an IList<>, not a List<> to Set().
            //IList<bool> myBoolArrayList0 = new List<bool>();
            //myBoolArrayList0.Add(true);
            //myBoolArrayList0.Add(false);
            //storageElementEntityWrite.Set(fieldArray0, myBoolArrayList0);
            //storageElementEntityWrite.Set(fieldEntity0, subEnt0);


            ////Create a map of Entities
            //IDictionary<int, Entity> myMap1 = new Dictionary<int, Entity>();
            //myMap1.Add(5, subEnt1);
            ////Set the map of Entities.
            //storageElementEntityWrite.Set(fieldMap1, myMap1);

            ////Create a list of entities
            //IList<Entity> myEntArrayList1 = new List<Entity>();
            //myEntArrayList1.Add(subEnt2);
            //myEntArrayList1.Add(subEnt2);
            ////Set the list of entities.
            //storageElementEntityWrite.Set(fieldArray1, myEntArrayList1);
            //#endregion
        }
        #endregion

        #region 

        /// <summary>
        /// 拿到字典里面的数据
        /// </summary>
        /// <remarks>
        /// 没有扩展表，将返回 default(T)
        /// 没有数据，会自动调跨类更新
        /// 有数据，会自动尝试调用更新
        /// </remarks>
        public T GetDictionary<T>(Element storageElement)
        {
            Schema schemaLookup = Schema.Lookup(SchemaGuid);
            Field fieldData = schemaLookup.GetField(nameof(StorageDictionary));
            Entity entity = storageElement.GetEntity(schemaLookup);
            if (entity.SchemaGUID != SchemaGuid)
            {
                Console.WriteLine($"{SchemaGuid} does not exist");
                return Activator.CreateInstance<T>();
            }

            T Object = Activator.CreateInstance<T>();
            IDictionary<string, string> iDictionary = entity.Get<IDictionary<string, string>>(fieldData);

            if (!iDictionary.TryGetValue(typeof(T).Name, out string json))
            {
                //TryClassUpdate(storageElement, Object);
                return Object;
            }

            Object = JsonConvert.DeserializeObject<T>(json);

            // 判断一下启动数据更新流程
            IExtendStorageBase extendStorageBase = Object as IExtendStorageBase;
            if (extendStorageBase != null)
            {
                int latestVersion = extendStorageBase.GetLatestVersion();
                if (latestVersion > extendStorageBase.CurVersion)
                {
                    extendStorageBase.UpdataState = UpdataState.Updating;
                    UpdataState updataResult = extendStorageBase.UpdateData();
                    extendStorageBase.UpdataState = updataResult;
                    if (updataResult == UpdataState.Succeed)
                    {
                        SetDictionary<T>(storageElement, Object);//再次保存一下
                    }
                }
            }

            return Object;
        }

        /// <summary>
        /// 尝试跨类更新(暂时不开放)
        /// </summary>
        private void TryClassUpdate<T>(Element storageElement, T Object)
        {
            Console.WriteLine($"{typeof(T).Name} does not exist,try to doing classUpdate");

            IExtendStorageBase extendStorageBase = Object as IExtendStorageBase;
            if (extendStorageBase != null)
            {
                extendStorageBase.UpdataState = UpdataState.ClassUpdating;
                UpdataState updataResult = extendStorageBase.UpdateData(storageElement);
                extendStorageBase.UpdataState = updataResult;

                // update succeed and save
                if (updataResult == UpdataState.Succeed)
                {
                    SetDictionary<T>(storageElement, Object);
                }
            }
        }

        /// <summary>
        /// 保存类的json数据到字典
        /// </summary>
        public void SetDictionary<T>(Element storageElement, T Object)
        {
            Schema schemaLookup = Schema.Lookup(SchemaGuid);
            Field fieldData = schemaLookup.GetField(nameof(StorageDictionary));
            Entity entity = storageElement.GetEntity(schemaLookup);
            if (entity.SchemaGUID != SchemaGuid)
            {
                entity = new Entity(SchemaGuid);
            }

            // 判断原来有没有map
            Dictionary<string, string> keyValuePairs;
            IDictionary<string, string> iDictionary = entity.Get<IDictionary<string, string>>(fieldData);
            keyValuePairs = new Dictionary<string, string>(iDictionary);

            string objectJson = JsonConvert.SerializeObject(Object);
            keyValuePairs[typeof(T).Name] = objectJson;

            string json = JsonConvert.SerializeObject(keyValuePairs);
            entity.Set<IDictionary<string, string>>(fieldData, keyValuePairs);
            storageElement.SetEntity(entity);
        }
        #endregion

        public ExtendStorageManage()
        {
            if (SchemaGuid == null)
            {
                throw new ArgumentNullException(string.Format("{0}{1}", nameof(SchemaGuid), " 没有赋值"));
            }
            NewSchema();
        }

        /// <summary>
        /// revit中的扩展信息主类
        /// </summary>
        protected Schema Schema { get; set; }
    }
}
