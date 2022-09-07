using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitTest.UIViewViewport
{
    /// <summary>  
    /// 与控制台交互  
    /// </summary>  
    public class ShellUtils : IDisposable
    {
        private static ShellUtils instance;

        public static ShellUtils Inst
        {
            get
            {
                if (instance == null)
                {
#if DEVELOP
                    instance = new ShellUtils()
                    {
                        IsDevelop = true
                    };
                    ShellUtils.AllocConsole();
#else
                    instance = new ShellUtils()
                    {
                        IsDevelop = false
                    };
                    AllocConsole();
#endif
                }
                return instance;
            }
        }

        public bool IsDevelop { get; private set; }

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        public static void Error(string error)
        {
            WriteLine(ref error, ShellMessageType.ERROR);
        }

        public static void Warning(string warning)
        {
            WriteLine(ref warning, ShellMessageType.WARNING);
        }

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        public static void Notice(string notice)
        {
            WriteLine(ref notice, ShellMessageType.NOTICE);
        }

        public void Dispose()
        {
            if (null == instance)
            {
                return;
            }
            FreeConsole();
        }

        public void Error(string error, Exception ex = null)
        {
            if (null == ex)
            {
                Error($"{error}");
                return;
            }
            Error($"{error}\n{ex}");
        }

        public void Free()
        {
            FreeConsole();
            instance = null;
        }

        /// <summary>
        /// 普通的记录信息
        /// </summary>
        /// <param name="output"></param>
        public void Info(string output)
        {
            WriteLine(ref output);
        }

        public void Warning(string warning, Exception ex = null)
        {
            if (null == ex)
            {
                Warning($"{warning}");
                return;
            }
            Warning($"{warning}\n{ex}");
        }

        private static ConsoleColor GetConsoleColor(ShellMessageType shellMessageType)
        {
            if (shellMessageType == ShellMessageType.Info)
                return ConsoleColor.Gray;
            if (shellMessageType == ShellMessageType.ERROR)
                return ConsoleColor.Red;
            if (shellMessageType == ShellMessageType.WARNING)
                return ConsoleColor.Yellow;
            if (shellMessageType == ShellMessageType.NOTICE)
                return ConsoleColor.Green;

            return ConsoleColor.Gray;
        }

        private static void WriteLine(ref string output, ShellMessageType shellMessageType = ShellMessageType.Info)
        {
            Console.ForegroundColor = GetConsoleColor(shellMessageType);
            Console.WriteLine(@"[{0}]{1}", DateTimeOffset.Now, output);
        }
    }
}
