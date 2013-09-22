//
// extension methods for telerik radcontrols for asp.net ajax
//
// Copyright 2013 corminlu@gmail.com
//

using System.Collections.Generic;
using Telerik.Web.UI;

namespace Cormin.RadControlsExtensions
{
    public static class RadTreeViewExtensions
    {
        /// <summary>
        /// 设置选中的值
        /// </summary>
        /// <param name="treeview"></param>
        /// <param name="values">值</param>
        public static RadTreeView SetChecked(this RadTreeView treeview, string values)
        {
            if (!string.IsNullOrEmpty(values))
            {
                string[] valueArray = values.SplitNonEmpty();
                if (values != null && values.Length > 0)
                {
                    foreach (string value in valueArray)
                    {
                        RadTreeNode node = treeview.FindNodeByValue(value);
                        if (node != null && node.Nodes.Count == 0)
                            node.Checked = true;
                    }
                }
            }
            return treeview;
        }

        /// <summary>
        /// 获取选中的值
        /// </summary>
        /// <param name="treeview"></param>
        /// <returns></returns>
        public static List<string> GetSelected(this RadTreeView treeview)
        {
            IList<RadTreeNode> nodes = treeview.CheckedNodes;
            if (nodes != null && nodes.Count > 0)
            {
                var ret = new List<string>(nodes.Count);
                foreach (RadTreeNode node in nodes)
                {
                    ret.Add(node.Value);
                }
                return ret;
            }
            return null;
        }
    }
}