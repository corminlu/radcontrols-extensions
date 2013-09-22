//
// extension methods for telerik radcontrols for asp.net ajax
//
// Copyright 2013 corminlu@gmail.com
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Cormin.RadControlsExtensions
{
    /// <summary>
    /// RadGrid扩展方法
    /// </summary>
    public static class RadGridExtractValueExtensions
    {
        /// <summary>
        /// 提取值
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ExtractValuesFromItem(this GridItem item)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase); // 不区分大小写
            var dataItem = item as GridEditableItem;
            if (dataItem != null)
            {
                dataItem.ExtractValues(dict);
            }
            return dict;
        }

        /// <summary>
        /// 提取值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ExtractValueFromItem(this GridItem item, string key)
        {
            var dict = ExtractValuesFromItem(item);
            if (dict.ContainsKey(key))
                return dict[key];

            return null;
        }

        /// <summary>
        /// 提取值
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Hashtable ExtractValuesFromTableView(this GridItem item)
        {
            return new Hashtable(item.OwnerTableView.DataKeyValues[item.ItemIndex], StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 提取值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object ExtractValueFromTableView(this GridItem item, string key)
        {
            var dict = ExtractValuesFromTableView(item);
            if (dict.Contains(key))
                return dict[key];
            return null;
        }

        static TableCell FindCell(GridEditableItem dataItem, string key)
        {
            GridColumn[] renderColumns = dataItem.OwnerTableView.RenderColumns;
            int num = 0;
            bool flag = false;
            foreach (GridColumn column in renderColumns)
            {
                if (column.UniqueName.IgnoreCaseCompare(key) ||
                    column.SortExpression.IgnoreCaseCompare(key))
                {
                    flag = true;
                    break;
                }
                var boundCol = column as GridBoundColumn;
                if (boundCol != null && boundCol.DataField.IgnoreCaseCompare(key))
                {
                    flag = true;
                    break;
                }
                var tempCol = column as GridTemplateColumn;
                if (tempCol != null && tempCol.DataField.IgnoreCaseCompare(key))
                {
                    flag = true;
                    break;
                }
                num++;
            }
            if (flag)
                return dataItem.Cells[num];

            return null;
        }

        /// <summary>
        /// 提取值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ExtractValueFromTableCell(this GridItem item, string key)
        {
            var dataItem = item as GridEditableItem;
            var cell = FindCell(dataItem, key);
            if (cell != null)
            {
                if (!string.IsNullOrWhiteSpace(cell.Text))
                    return cell.Text;

                foreach (var ctrl in cell.Controls)
                {
                    var literal = ctrl as DataBoundLiteralControl;
                    if (literal != null)
                        return literal.Text;
                }
            }

            var textCtrl = item.FindControl(key) as ITextControl;
            if (textCtrl != null)
                return textCtrl.Text;

            return null;
        }

        /// <summary>
        /// 提取值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object ExtractValue(this GridItem item, string key = null)
        {
            var keyNames = item.OwnerTableView.DataKeyNames;
            bool hasKeys = keyNames.Length > 0;
            if (hasKeys && key == null)
                key = keyNames[0];

            object value = null;
            if (hasKeys)
                value = ExtractValueFromTableView(item, key);

            if (value == null)
            {
                var strValue = ExtractValueFromItem(item, key);
                if (string.IsNullOrEmpty(strValue))
                    strValue = ExtractValueFromTableCell(item, key);
                if (!string.IsNullOrEmpty(strValue))
                    value = strValue;
            }

            return value;
        }

        /// <summary>
        /// 提取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ExtractValue<T>(this GridItem item, Func<string, T> cast, string key = null)
        {
            var result = ExtractValue(item, key);
            if (result == null)
                return default(T);
            if (result.GetType() == typeof(string))
                return cast(result.ToString());
            return (T)result;
        }

        /// <summary>
        /// 提取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ExtractValue<T>(this GridCommandEventArgs e, Func<string, T> cast, string key = null)
        {
            return ExtractValue<T>(e.Item, cast, key);
        }

        /// <summary>
        /// 提取选中的多个值
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static List<object> ExtractSelected(this RadGrid grid, string key = null)
        {
            var items = grid.SelectedItems;
            var result = new List<object>(items.Count);

            foreach (GridDataItem item in items)
            {
                object value = ExtractValue(item, key);
                if (value != null)
                    result.Add(value);
            }

            return result;
        }

        /// <summary>
        /// 提取选中的多个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> ExtractSelected<T>(this RadGrid grid, Func<string, T> cast, string key = null)
        {
            var items = grid.SelectedItems;
            var result = new List<T>(items.Count);

            foreach (GridDataItem item in items)
            {
                T value = ExtractValue<T>(item, cast, key);
                if (value != null)
                    result.Add(value);
            }

            return result;
        }
    }
}