using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitUtils.ExtensibleStorageUtils
{
    /// <summary>
    /// 更新的状态
    /// </summary>
    /// <remarks>
    /// </remarks>
    public enum UpdataState
    {
        /// <summary>
        /// 最新的(暂留)
        /// </summary>
        /// <remarks>
        /// 合理调用这个会减少序列化操作
        /// 因为判断最新的版本由另外的方式，这个不在需要
        /// </remarks>
        Latest = 0,
        /// <summary>
        /// 更新成功
        /// </summary>
        Succeed = 1,
        /// <summary>
        /// 更新失败
        /// </summary>
        Fail = 2,
        /// <summary>
        /// 正在更新
        /// </summary>
        Updating = 3,
        /// <summary>
        /// 跨类更新(预留，暂时不使用)
        /// </summary>
        ClassUpdating = 4,
    }

    public interface IUpdateStorage
    {
        /// <summary>
        /// 版本号
        /// </summary>
        int CurVersion { get; set; }

        int GetLatestVersion();

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
    }
}
