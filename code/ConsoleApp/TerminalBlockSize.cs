/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/7 14:32:13
 * 文件描述:  
 * 
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.ConsoleApp
{
    public class TerminalBlockSize
    {
        public TerminalBlockType Type = TerminalBlockType.Invalid;

        private string earthWire = String.Empty;
        private string fireWireA = String.Empty;
        private string fireWireB = String.Empty;
        private string fireWireC = String.Empty;
        private string neutralWire = String.Empty;

        public string EarthWire { get => earthWire; set => earthWire = value; }
        public string FireWireA { get => fireWireA; set => fireWireA = value; }
        public string FireWireB { get => fireWireB; set => fireWireB = value; }
        public string FireWireC { get => fireWireC; set => fireWireC = value; }
        public string NeutralWire { get => neutralWire; set => neutralWire = value; }

        /// <summary>
        /// 转换成 数量x横截面积+数量x横截面积的形式
        /// </summary>
        /// <returns></returns>
        public string ToSize()
        {
            List<int> list = new List<int>();
            Dictionary<int, int> dict = new Dictionary<int, int>();

            if (!string.IsNullOrEmpty(FireWireA))
            {
                AddDic(dict, FireWireA);
            }
            if (!string.IsNullOrEmpty(FireWireB))
            {
                AddDic(dict, FireWireB);
            }
            if (!string.IsNullOrEmpty(FireWireC))
            {
                AddDic(dict, FireWireC);
            }
            if (!string.IsNullOrEmpty(EarthWire))
            {
                AddDic(dict, EarthWire);
            }
            if (!string.IsNullOrEmpty(NeutralWire))
            {
                AddDic(dict, NeutralWire);
            }

            string str = null;
            foreach (var pair in dict)
            {
                if (str == null)
                {
                    str += string.Format("{0}x{1}", pair.Value, pair.Key);
                }
                else
                {
                    str += string.Format("+{0}x{1}", pair.Value, pair.Key);
                }
            }
            return str;
        }

        public override string ToString()
        {
            return string.Format("火线A {0}\n-火线B {1}\n-火线C {2}\n-地线 {3 }\n-零线 {4}\n",
                this.FireWireA,
                this.FireWireB,
                this.FireWireC,
                this.EarthWire,
                this.NeutralWire);
        }

        private void AddDic(Dictionary<int, int> dict, string str)
        {
            int num = int.Parse(str);
            if (dict.ContainsKey(num))
            {
                dict[num]++;
            }
            else
            {
                dict.Add(num, 1);
            }
        }
    }
}