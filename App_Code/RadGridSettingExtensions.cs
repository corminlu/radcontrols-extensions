//
// extension methods for telerik radcontrols for asp.net ajax
//
// Copyright 2013 corminlu@gmail.com
//

using System;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Cormin.RadControlsExtensions
{
    public static class RadGridSettingExtensions
    {
        /// <summary>
        /// 开启自定义分页
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static RadGrid AllowCustomPaging(this RadGrid grid)
        {
            if (!grid.AllowPaging)
            {
                grid.AllowPaging = true;
                grid.AllowCustomPaging = true;
                grid.AllowSorting = true;
                grid.AllowMultiRowSelection = true;
                grid.AutoGenerateColumns = false; // 不自动生成列
                grid.Culture = CultureInfo.CurrentCulture; // 设置文化信息，用于显示中文
                grid.PagerStyle.AlwaysVisible = true; // 总是显示分页工具条
            }
            return grid;
        }

        /// <summary>
        /// 客户端设置用于拖动、选择等
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static RadGrid AllowColumnReorder(this RadGrid grid)
        {
            var gcs = grid.ClientSettings;
            if (!gcs.AllowColumnsReorder)
            {
                gcs.AllowColumnsReorder = true;
                gcs.ReorderColumnsOnClient = true;
                gcs.Selecting.AllowRowSelect = true;
                gcs.Resizing.AllowColumnResize = true;
                //gcs.Selecting.EnableDragToSelectRows = false; // 关闭拖动选择行功能才可以选中行的文本
            }
            return grid;
        }

        /// <summary>
        /// 用于获取数据的key时使用
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="keyNames"></param>
        /// <returns></returns>
        public static RadGrid SetDataKeys(this RadGrid grid, params string[] keyNames)
        {
            var gtv = grid.MasterTableView;
            if (gtv.DataKeyNames.Length == 0 ||
                !gtv.DataKeyNames.Contains(keyNames[0]))
                gtv.DataKeyNames = keyNames.Concat(gtv.DataKeyNames).ToArray();
            return grid;
        }

        /// <summary>
        /// 用于获取数据的key时使用，比如前台的row.getDataKeyValue
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="keyNames"></param>
        /// <returns></returns>
        public static RadGrid SetClientDataKeys(this RadGrid grid, params string[] keyNames)
        {
            var gtv = grid.MasterTableView;
            if (gtv.ClientDataKeyNames.Length == 0 ||
                !gtv.ClientDataKeyNames.Contains(keyNames[0]))
                gtv.ClientDataKeyNames = keyNames.Concat(gtv.ClientDataKeyNames).ToArray();
            return grid;
        }

        /// <summary>
        /// 设置页号
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static RadGrid SetPageIndexFromQuery(this RadGrid grid, string key = "pageIndex")
        {
            int pageIndex;
            string value = grid.Page.Request.QueryString[key];
            if (!string.IsNullOrEmpty(value) &&
                int.TryParse(value, out pageIndex) &&
                pageIndex > 0)
                grid.CurrentPageIndex = pageIndex - 1;
            return grid;
        }

        /// <summary>
        /// 锁定表头
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="height">表格高度</param>
        public static RadGrid LockHeader(this RadGrid grid, Unit height)
        {
            GridScrolling gs = grid.ClientSettings.Scrolling;
            if (!gs.AllowScroll)
            {
                gs.AllowScroll = true;
                gs.UseStaticHeaders = true;
                gs.ScrollHeight = height;
                grid.PageSize = (int)Math.Floor(height.Value / 24); // 24为行高度
            }
            return grid;
        }
    }
}