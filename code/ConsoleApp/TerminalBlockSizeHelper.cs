/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/7 14:30:44
 * 文件描述:  
 * 
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RQ.ConsoleApp
{
    public class TerminalBlockSizeHelper
    {
        /// <summary>
        /// 4x(1x15)
        /// ^\d+[x|\*]*\(\d*[a-zA-Z]+\d+\)
        /// </summary>
        public static string OneMode = @"^\d+[x|\*]*\(\d*[a-zA-Z]+[\d(.\d)?]+\)";
        /// <summary>
        /// 4x(1H15)
        /// </summary>
        public static string D = null;
        /// <summary>
        /// 4(1H15)
        /// </summary>
        public static string E = null;
        /// <summary>
        /// 4(1X15)
        /// </summary>
        public static string F = null;
        // 以上匹配作为统一匹配 按照 ( )拆分，取 （ 前面连续的数字  取 ）连续的数字，从遇到第一个数字开始，到不是第一个数字结束

        /// <summary>
        /// [数字] x 字母 x 数字
        /// 4x15
        /// 4H15
        /// ^\d*[a-zA-Z|\*]+\d+
        /// </summary>
        public static string TwoMode = @"^\d*[a-zA-Z|\*]+[\d(.\d)?]+";

        /// <summary>
        /// 字母 x 数字
        /// H16
        /// </summary>
        public static string TwoMode_1 = @"^[a-zA-Z|\*]+[\d(.\d)?]+";
        // 以上匹配作为统一匹配 按照中间的英语字母进行拆分 前面为空则补1。

        /// <summary>
        /// 乘法的匹配方式
        /// </summary>
        public static string Multiply = @"[x|\*]";


        public static TerminalBlockSize DoSplit(string str)
        {
            TerminalBlockSize terminalBlockSize = new TerminalBlockSize();

            List<double> sectionArea = new List<double>();

            // 先判断是否有加号，对每个加号进行分割，再别分处理
            String[] splitPlus = str.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

            if (splitPlus.Length == 1)
            {   // 只要一个部分
                string splitPlusOne = splitPlus[0];
                DoSpliSingleWithMode(splitPlusOne, sectionArea);
            }
            else if (splitPlus.Length == 2)
            {
                string splitPlusOne = splitPlus[0];
                string splitPlusTwo = splitPlus[1];
                DoSpliSingleWithMode(splitPlusOne, sectionArea);
                DoSpliSingleWithMode(splitPlusTwo, sectionArea);
            }
            else
            {
                foreach (string s in splitPlus)
                {
                    string splitPlusO = s;
                    DoSpliSingleWithMode(splitPlusO, sectionArea);
                }
            }
            // 排序
            sectionArea.Sort();
            sectionArea.Reverse();

            if (sectionArea.Count == 3)
            {
                terminalBlockSize.Type = TerminalBlockType.ThreeWireSystem;
                terminalBlockSize.FireWireA = sectionArea[0].ToString();
                terminalBlockSize.EarthWire = sectionArea[1].ToString();
                terminalBlockSize.NeutralWire = sectionArea[2].ToString();
            }
            if (sectionArea.Count == 4)
            {
                terminalBlockSize.Type = TerminalBlockType.FourWireSystem;
                terminalBlockSize.FireWireA = sectionArea[0].ToString();
                terminalBlockSize.FireWireB = sectionArea[1].ToString();
                terminalBlockSize.FireWireC = sectionArea[2].ToString();
                terminalBlockSize.EarthWire = sectionArea[3].ToString();
            }
            if (sectionArea.Count >= 5)
            {
                terminalBlockSize.Type = TerminalBlockType.FiveWireSystem;
                terminalBlockSize.FireWireA = sectionArea[0].ToString();
                terminalBlockSize.FireWireB = sectionArea[1].ToString();
                terminalBlockSize.FireWireC = sectionArea[2].ToString();
                terminalBlockSize.NeutralWire = sectionArea[3].ToString();
                terminalBlockSize.EarthWire = sectionArea[4].ToString();
            }

            return terminalBlockSize;
        }

        /// <summary>
        /// 拆分单独的一组数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <remarks>
        /// 举例：4x(1H15)
        /// 先按照 '('')'拆分成 4x 1H14 "" 强制拆分成3份
        /// 将第一份数据 4x 按照单词继续拆分成 4 x 取数字
        /// 如果只有1H14 继续拆分单词 'H'  1  14
        ///     取14作为横截面积
        /// 如果有两个 分别拆分，4作为条数，14作为横截面积
        /// </remarks>
        private static void DoSpliSingleWithMode(string str, List<double> sectionAreaList)
        {
            Match match;

            // 数字 [乘] 左括号 [数字] [乘] [字母] 数字 右括号
            match = Regex.Match(str, @"\d[x|\*]?\(\d?[x|\*]?[a-zA-Z]?\d+[.\d]*\)");
            if (match.Success)
            {
                // 先把满足的部分给截取出来, 默认取第一部分
                string strSplit = match.Value;
                String[] strSplits = strSplit.Split(new char[] { '(', ')' }, StringSplitOptions.None);

                // 第一部分
                double num = 1;    // 默认有一条线
                string oneSplit = strSplits[0];
                DoSplitEndNum(oneSplit, ref num);

                // 第二部分
                double sectionArea = 0;
                string twoSplit = strSplits[1];
                DoSplitEndNum(twoSplit, ref sectionArea);

                // 第三部分 不管
                string threeSplit = strSplits[2];

                for (int i = 0; i < num; i++)
                {
                    sectionAreaList.Add(sectionArea);
                }

                return;
            }

            // 不带括号 但是有乘号
            match = Regex.Match(str, @"\d[x|*]\d*[a-zA-Z]*\d+[.\d]*");
            if (match.Success)
            {
                string strSplit = match.Value;
                String[] strSplits = strSplit.Split(new char[] { 'x', '*' }, StringSplitOptions.None);

                double num = 1;    // 默认有一条线
                double sectionArea = 0;

                // 第一部分应该是数字
                if (int.TryParse(strSplits[0], out int onenum))
                {
                    num = onenum;
                }
                DoSplitEndNum(strSplits[1], ref sectionArea);

                for (int i = 0; i < num; i++)
                {
                    sectionAreaList.Add(sectionArea);
                }
                return;
            }

            // 没有乘
            match = Regex.Match(str, @"\d*[a-zA-Z]+\d+[.\d]*");
            if (match.Success)
            {
                double num = 1;    // 默认有一条线
                double sectionArea = 0;

                DoSplitFrontNum(str, ref num);
                DoSplitEndNum(str, ref sectionArea);

                for (int i = 0; i < num; i++)
                {
                    sectionAreaList.Add(sectionArea);
                }
                return;
            }
        }

        /// <summary>
        /// 拆分 数字 字母 数字的集合体 取后面的数字
        /// 类式于 H16
        /// </summary>
        /// <param name="splitStr"></param>
        /// <param name="num"></param>
        private static void DoSplitFrontNum(string splitStr, ref double num)
        {
            if (Regex.IsMatch(splitStr, TerminalBlockSizeHelper.TwoMode_1))
            {
                num = 1;
                return;
            }

            string[] numberSplit = Regex.Split(splitStr, @"[a-zA-Z]+");
            for (int i = 0; i >= 0; i++)
            {
                if (numberSplit[i] != string.Empty)
                {
                    if (double.TryParse(numberSplit[i], out double tNum))
                    {
                        num = tNum;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 拆分 数字 字母 数字的集合体 取后面的数字
        /// </summary>
        private static void DoSplitEndNum(string splitStr, ref double num)
        {
            string[] numberSplit = Regex.Split(splitStr, @"[a-zA-Z]+");
            for (int i = numberSplit.Length - 1; i >= 0; i--)
            {
                if (numberSplit[i] != string.Empty)
                {
                    if (double.TryParse(numberSplit[i], out double tNum))
                    {
                        num = tNum;
                        break;
                    }
                }
            }
        }
    }
}