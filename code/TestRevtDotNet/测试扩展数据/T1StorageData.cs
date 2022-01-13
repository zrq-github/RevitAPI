/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2021/12/31 13:56:35
 * 文件描述:  
 * 
*************************************************************************************/
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.Test.RevtDotNet.测试扩展数据
{
    internal class T1StorageData : ExtendStorageBase
    {
        public static bool IsStarUpdata = false;

        public T1StorageData()
        {
            CurVersion = 1;
            StorageDataDescription = $"this is a {nameof(T1StorageData)} Class";
        }

        public override int CurVersion { get; set; }

        public string LastCommand { get; set; }
        public string StorageDataDescription { get; set; }
        public override UpdataState UpdataState { get; set; }

        public override int GetLatestVersion()
        {
            return 2;
        }

        public override UpdataState UpdateData(Element ele)
        {
            #region 测试入口
            if (!T1StorageData.IsStarUpdata)
            {
                return UpdataState.Succeed;
            }
            #endregion

            UpdataState updataState = UpdataState.Fail;
            if (this.UpdataState == UpdataState.Updating)
            {
                if (CurVersion == 1)
                {
                    // 升级数据结构
                    CurVersion = 2;
                    StorageDataDescription = $"this is a {nameof(T1StorageData)} Class, already update 2.0";
                    updataState = UpdataState.Succeed;
                }
                else if (CurVersion == 2)
                {
                    updataState = UpdataState.Latest;
                }
            }

            return updataState;
        }
    }
}