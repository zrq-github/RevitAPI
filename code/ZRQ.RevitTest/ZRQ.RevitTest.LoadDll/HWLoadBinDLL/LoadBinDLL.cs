using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HW.LoadBinDLL
{
    /// <summary>
    /// 2021/09/03 李马元  此dll放在产品安装目录的bin文件夹内
    /// 各个产品主动加载此dll，并传入需要加载的bin文件夹内的dll名称
    /// </summary>
    public class HWLoadBinDLL
    {
        /// <summary>
        /// 找到的bin文件夹
        /// </summary>
        private static string strBinDir { get; set; } = @"C:\ProgramData\Autodesk\Revit\Addins\2022\bin";

        /// <summary>
        /// 需要加载的dll
        /// </summary>
        private static List<string> dllNames { get; set; }

        /// <summary>
        /// 2021/08/27 李马元 动态加载与revit版本无关的dll。达到减小安装包的目的
        /// </summary>
        /// <param name="dllNames">需要加载的dll名称，必须都在bin文件夹内</param>
        /// <param name="listErrorInfo">返回日志信息</param>
        /// <returns>正常结束返回true，出现异常返回false</returns>
        public static bool LoadBinDLL(List<string> dllNames, out List<HWLoadBinDLLLog> listErrorInfo)
        {// 利用反射加载库 
            HWLoadBinDLL.dllNames = new List<string>();
            HWLoadBinDLL.dllNames.AddRange(dllNames);
            strBinDir = string.Empty;
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            var bin = Assembly.GetExecutingAssembly().Location;

            listErrorInfo = new List<HWLoadBinDLLLog>();
            try
            {
                string strTip = string.Format("LoadBinDLL 加载路径：{0}", bin);
                listErrorInfo.Add(new HWLoadBinDLLLog(strTip));
                strBinDir = Path.GetDirectoryName(bin);
                for (int i = 0; i < dllNames.Count; i++)
                {
                    var dllName = dllNames[i];
                    var path = Path.Combine(strBinDir, dllName);
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(path);
                        if (null == assembly)
                        {
                            strTip = string.Format("LoadBinDLL 加载bin文件下的 {0} ,失败！！！", dllName);
                            listErrorInfo.Add(new HWLoadBinDLLLog(strTip, new Exception("加载失败")));
                        }
                    }
                    catch (Exception ex)
                    {
                        strTip = string.Format("LoadBinDLL 加载bin文件下的 {0} ,失败！！！", dllName);
                        listErrorInfo.Add(new HWLoadBinDLLLog(strTip, ex));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                listErrorInfo.Add(new HWLoadBinDLLLog("LoadBinDLL 加载库文件出错", ex));
                return false;
            }
        }

        /// <summary>
        /// 失败再加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            string name = callingAssembly.GetName().Name;
            if (!name.Contains("RevitCC"))
            {
                return callingAssembly;
            }

            try
            {
                AssemblyName assemblyName = new AssemblyName(args.Name);
                var dllName = assemblyName.Name;
                string file = string.Format("{0}.dll", Path.Combine(strBinDir, dllName));
                if (File.Exists(file))
                { // 尝试重载
                    return Assembly.LoadFrom(file);
                }
            }
            catch (Exception ex)
            {
                string strTip = string.Format("失败再加载 OnAssemblyResolve 出错:{0}", ex.Message);
                System.Windows.Forms.MessageBox.Show(strTip, "警告");
                //移除回调
                AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
            }
            return args.RequestingAssembly;
        }
    }

    /// <summary>
    /// 日志信息
    /// </summary>
    public class HWLoadBinDLLLog
    {
        /// <summary>
        /// 只有提示
        /// </summary>
        /// <param name="strTip"></param>
        public HWLoadBinDLLLog(string strTip)
        {
            this.strTip = strTip;
            ex = null;
        }

        /// <summary>
        /// 提示+异常
        /// </summary>
        /// <param name="strTip"></param>
        /// <param name="ex"></param>
        public HWLoadBinDLLLog(string strTip, Exception ex) : this(strTip)
        {
            this.ex = ex;
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string strTip { get; set; }

        /// <summary>
        /// 相关异常
        /// </summary>
        public Exception ex { get; set; }
    }
}
