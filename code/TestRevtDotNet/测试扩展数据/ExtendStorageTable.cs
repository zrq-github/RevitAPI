/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2021/12/31 13:46:33
 * 文件描述:  
 * 
*************************************************************************************/
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using RQ.RevitUtils.ExtensibleStorageUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRevtDotNet;

namespace RQ.Test.RevtDotNet.测试扩展数据
{
    internal class ExtendStorageTable : ExtendStorageManage
    {
        public override Guid SchemaGuid { get; } = new Guid("1FE5BD8A-D624-40FF-BBE0-2EF7755F7817");
        protected override AccessLevel ReadLevel { get; } = AccessLevel.Public;
        protected override AccessLevel WriteLevle { get; } = AccessLevel.Public;
        protected override string StorageVersion { get; } = "1.0";
        protected override string StorageName { get; } = nameof(ExtendStorageTable);
        protected override string ApplicationId { get; } = RevitApplication.ApplicationId;

        // 建议主动提供每个数据类的 Get/Set方法, 在里面再调用父类的Set/Get
        // 一是方便自己知道，这个表中存了那些数据
        // 二是方面通过引用，找到指定的数据类在那些地方被修改了

        internal bool SetStoragePerson(Element element, T1StorageData elevation)
        {
            SetDictionary(element, elevation);
            return true;
        }

        internal T1StorageData GetStoragePerson(Element element)
        {
            T1StorageData elevation = GetDictionary<T1StorageData>(element);
            if (elevation == null)
            {
                // 记日志
            }
            return elevation;
        }
    }
}