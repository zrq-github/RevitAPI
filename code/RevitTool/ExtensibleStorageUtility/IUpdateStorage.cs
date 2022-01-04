using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.RevitUtils.ExtensibleStorageUtility
{
    /// <summary>
    /// 更新的状态
    /// </summary>
    /// <remarks>
    /// </remarks>
    public enum UpdataState
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
        /// <summary>
        /// 跨类更新
        /// </summary>
        /// <remarks>
        /// 中间状态位
        /// </remarks>
        ClassUpdate = 2,
        /// <summary>
        /// 正在更新
        /// </summary>
        Updating = 3,
    }

    public interface IUpdateStorage
    {
        /// <summary>
        /// 版本号
        /// </summary>
        string CurVersion { get; set; }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <remarks>
        /// 非必要，请勿手动更改此状态
        /// </remarks>
        [Newtonsoft.Json.JsonIgnore]
        UpdataState UpdataState { get; set; }

        /// <summary>
        /// 更新函数
        /// </summary>
        /// <remarks>
        /// 在构造函数设置当前序列号，在更新的时候是没有任何作用
        /// 因为反序列回来的数据是存着版本号
        /// </remarks>
        UpdataState UpdateData(Element ele = null);

        /// <summary>
        /// 跨类更新
        /// </summary>
        /// <remarks>
        /// 从当前类更新到其他类型
        /// </remarks>
        object UpdataNewClass { get; set; }
    }
}
