//
// extension methods for telerik radcontrols for asp.net ajax
//
// Copyright 2013 corminlu@gmail.com
//

using System.Web.UI;
using Telerik.Web.UI;

namespace Cormin.RadControlsExtensions
{
    public static class RadComboBoxExtensions
    {
        /// <summary>
        /// 设置选中的文本
        /// </summary>
        /// <param name="box"></param>
        /// <param name="text">文本</param>
        public static RadComboBoxItem SetSelectedText(this RadComboBox box, string text)
        {
            RadComboBoxItem item = box.FindItemByText(text);
            return SetSelected(box, item);
        }

        /// <summary>
        /// 设置选中的值
        /// </summary>
        /// <param name="box"></param>
        /// <param name="value">值</param>
        public static RadComboBoxItem SetSelectedValue(this RadComboBox box, string value)
        {
            RadComboBoxItem item = box.FindItemByValue(value);
            return SetSelected(box, item);
        }

        /// <summary>
        /// 设置选中的值
        /// </summary>
        /// <param name="box"></param>
        /// <param name="item">值</param>
        public static RadComboBoxItem SetSelected(this RadComboBox box, RadComboBoxItem item)
        {
            if (item != null)
            {
                box.ClearSelection();
                item.Selected = true;
            }
            return item;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="box"></param>
        /// <param name="dataSource">数据源</param>
        /// <param name="textfld">文本字段</param>
        /// <param name="valuefld">值字段</param>
        public static RadComboBox BindData(this RadComboBox box, object dataSource, string textfld, string valuefld)
        {
            if (box.SelectedIndex > -1)
                box.ClearSelection();
            if (box.Items.Count > 0)
                box.Items.Clear();
            box.DataSource = dataSource;
            box.DataTextField = textfld;
            box.DataValueField = valuefld;
            box.DataBind();
            return box;
        }

        /// <summary>
        /// 从RadComboBox中查找TreeView
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static RadTreeView FindNestedTreeView(this RadComboBox box)
        {
            RadComboBoxItem item = box.Items[0];
            RadTreeView tree = item.Controls[1] as RadTreeView;
            if (tree != null) return tree;
            foreach (Control ctrl in box.Items[0].Controls)
            {
                tree = ctrl as RadTreeView;
                if (tree != null)
                    return tree;
            }
            return null;
        }
    }
}