using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.RevitUtils.ExtensibleStorageUtility
{
    /// <summary>
    /// 更新的结果
    /// </summary>
    /// <remarks>
    /// Succeed：更新成功
    /// Fail： 更新失败
    /// </remarks>
    public enum UpdataResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <remarks>更新成功</remarks>
        Succeed = 0,
        /// <summary>
        /// 失败
        /// </summary>
        /// <remarks>更新失败</remarks>
        Fail = 1,

    }

    public interface IUpdateStorage
    {
        /// <summary>
        /// 版本号
        /// </summary>
        /// <remarks>默认应该为1.0</remarks>
        string CurVersion { get; set; }

        /// <summary>
        /// 更新状态
        /// </summary>
        UpdataResult UpdataState { get; set; }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <remarks>默认返回false</remarks>
        UpdataResult UpdateData();

        /// <summary>
        /// 跨类更新
        /// </summary>
        /// <remarks>
        /// 从当前类更新到其他类型
        /// </remarks>
        object UpdataNewClass { get; set; }
    }
}
