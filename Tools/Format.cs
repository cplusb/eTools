using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace eTools.Tools
{
    class Format
    {
        /// <summary>
        /// 保留小数点后3位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double FormatDecimalThree(double value) => double.Parse(string.Format("{0:n3}", value));
        /// <summary>
        /// 数字分段显示
        /// </summary>
        /// <param name="numberStr"></param>
        /// <returns></returns>
        public static string SegmentaThree(double value) => Regex.Replace(value + " ", @"(?<=\D*)\d+?(?=(?:\d{3})+\D+)", "$0,").Trim();
    }
}
