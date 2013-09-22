//
// extension methods for telerik radcontrols for asp.net ajax
//
// Copyright 2013 corminlu@gmail.com
//

using System;

namespace Cormin.RadControlsExtensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 忽略大小写比较
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IgnoreCaseCompare(this string a, string b)
        {
            return string.Compare(a, b, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// 分割字符串,去除空元素
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string[] SplitNonEmpty(this string s)
        {
            return s.SplitNonEmpty(new char[] { ',' });
        }

        /// <summary>
        /// 分割字符串,去除空元素
        /// </summary>
        /// <param name="s"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] SplitNonEmpty(this string s, char[] separator)
        {
            if (string.IsNullOrEmpty(s))
                return new string[0];

            return s.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}