//
// extension methods for telerik radcontrols for asp.net ajax
//
// Copyright 2013 corminlu@gmail.com
//

using Telerik.Web.UI;

namespace Cormin.RadControlsExtensions
{
    public static class RadListBoxExtensions
    {
        /// <summary>
        /// 设置选中的文本
        /// </summary>
        /// <param name="box"></param>
        /// <param name="text">文本</param>
        public static RadListBoxItem SetSelectedText(this RadListBox box, string text)
        {
            RadListBoxItem item = box.FindItemByText(text);
            return SetSelected(box, item);
        }

        /// <summary>
        /// 设置选中的值
        /// </summary>
        /// <param name="box"></param>
        /// <param name="value">值</param>
        public static RadListBoxItem SetSelectedValue(this RadListBox box, string value)
        {
            RadListBoxItem item = box.FindItemByValue(value);
            return SetSelected(box, item);
        }

        /// <summary>
        /// 设置选中的值
        /// </summary>
        /// <param name="box"></param>
        /// <param name="item">值</param>
        public static RadListBoxItem SetSelected(this RadListBox box, RadListBoxItem item)
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
        public static RadListBox BindData(this RadListBox box, object dataSource, string textfld, string valuefld)
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
    }
}