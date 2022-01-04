/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/4 15:01:02
 * 文件描述:  
 * 
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.Test.RevtDotNet.测试扩展数据
{
    internal class T2StorageData : ExtendStorageBase
    {
        public T2StorageData()
        {
            CurVersion = "1.0";
            StorageDataDescription = $"this is a {nameof(T2StorageData)} Class, it update form {nameof(T1StorageData)}";
        }
        public override string CurVersion { get; set; }
        public string StorageDataDescription { get; private set; }
        public override UpdataState UpdataState { get; set; }

        public override UpdataState UpdateData(Element ele)
        {
            UpdataState updataState = UpdataState.Fail;

            if (this.UpdataState == UpdataState.ClassUpdating)
            {
                ExtendStorageTable extendStorageTable = new ExtendStorageTable();
                T1StorageData t1StorageData = extendStorageTable.GetT1StorageData(ele);
                this.StorageDataDescription += t1StorageData.StorageDataDescription;
                updataState = UpdataState.Succeed;
            }

            if (this.UpdataState == UpdataState.Updating)
            {
                if (this.CurVersion == "1.0")
                {
                    updataState = UpdataState.Latest;
                }
            }

            return updataState;
        }
    }
}