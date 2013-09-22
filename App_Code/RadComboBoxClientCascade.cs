//
// extension methods for telerik radcontrols for asp.net ajax
//
// Copyright 2013 corminlu@gmail.com
//

using System.Text;
using Telerik.Web.UI;

namespace Cormin.RadControlsExtensions
{
    /// <summary>
    /// 客户端Combobox多级联动
    /// </summary>
    public class RadComboBoxClientCascade
    {
        RadComboBox box;
        RadComboBoxItem defaultItem;
        string defaultValue;
        /// <summary>
        /// 请求事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ItemsRequestedEvent(RadComboBoxClientCascade sender, RadComboBoxItemsRequestedEventArgs e);
        ItemsRequestedEvent itemRequested;
        RadComboBoxItemsRequestedEventArgs currentArg;
        RadComboBoxClientCascade subQuery;

        /// <summary>
        /// 客户端Combobox多级联动
        /// </summary>
        /// <param name="box"></param>
        /// <param name="itemRequested"></param>
        public RadComboBoxClientCascade(RadComboBox box, ItemsRequestedEvent itemRequested)
        {
            this.box = box;
            if (itemRequested != null)
            {
                this.itemRequested = itemRequested;
                box.ItemsRequested += box_ItemsRequested;
            }
        }

        void box_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (itemRequested != null)
                itemRequested(this, e);
        }

        /// <summary>
        /// 选择变更时执行
        /// </summary>
        void RaiseSelectChange()
        {
            ResetSubList();////

            if (defaultItem == null && (box.SelectedIndex > -1 || box.FindItemIndexByValue(box.SelectedValue) > -1) || defaultItem != null && (box.SelectedIndex > 0 || box.FindItemIndexByValue(box.SelectedValue) > -1))
            {
                //执行选择变更的方法，并且递归到每个子查询
                if (itemRequested != null && subQuery != null && subQuery.box.Visible)
                {
                    itemRequested(subQuery, currentArg);
                    if (!string.IsNullOrEmpty(subQuery.defaultValue))
                    {
                        subQuery.SetValue(subQuery.defaultValue);
                    }
                }

            }
        }

        /// <summary>
        /// 重置控件（清空所有项）
        /// </summary>
        void ResetList()
        {
            if (box.Items.Count > 0)
                box.Items.Clear();
            AddDefaultItem();
            ResetSubList();
        }

        /// <summary>
        /// 递归重置子查询的控件
        /// </summary>
        void ResetSubList()
        {
            if (subQuery != null)
                subQuery.ResetList();
        }

        /// <summary>
        /// 在页面注册脚本
        /// </summary>
        public void RegisterScript()
        {
            if (!box.Page.ClientScript.IsStartupScriptRegistered("cascadeRadCombo"))
            {
                box.Page.ClientScript.RegisterStartupScript(box.Page.GetType(), "cascadeRadCombo", BuildScript(), true);
            }
        }

        /// <summary>
        /// 生成客户端脚本
        /// </summary>
        /// <returns></returns>
        public string BuildScript()
        {
            StringBuilder builder = new StringBuilder(box.ClientID.Length * 30);
            builder.Append("Sys.Application.add_load(function() { ");
            builder.AppendFormat("var box1 = cascadeCombo(\"{0}\")", box.ClientID);
            if (defaultItem != null)
                builder.Append(".defaultItem(true)");
            builder.Append(";");
            RadComboBoxClientCascade sub = subQuery;
            if (sub != null)
            {
                builder.Append("box1");
                do
                {
                    builder.AppendFormat(".setSub(cascadeCombo(\"{0}\"))", sub.box.ClientID);
                    if (sub.defaultItem != null)
                        builder.Append(".defaultItem(true)");
                } while ((sub = sub.subQuery) != null);
                builder.Append(";");
            }
            builder.Append("});");
            return builder.ToString();
        }

        /// <summary>
        /// 设置默认项
        /// </summary>
        /// <param name="text"></param>
        public RadComboBoxClientCascade DefaultItem(string text)
        {
            return DefaultItem(text, string.Empty);
        }

        /// <summary>
        /// 设置默认项
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        public RadComboBoxClientCascade DefaultItem(string text, string value)
        {
            return DefaultItem(new RadComboBoxItem(text, value));
        }

        /// <summary>
        /// 设置默认项
        /// </summary>
        /// <param name="item"></param>
        public RadComboBoxClientCascade DefaultItem(RadComboBoxItem item)
        {
            if (defaultItem == null)
                this.defaultItem = item;
            if (box.Items.Count == 0 || box.Items.Count > 0 && !box.Items.Contains(item))
            {
                box.Items.Insert(0, item);
            }
            return this;
        }

        /// <summary>
        /// 插入默认选项
        /// </summary>
        void AddDefaultItem()
        {
            if (defaultItem != null)
                box.Items.Insert(0, defaultItem);
        }

        /// <summary>
        /// 默认选中值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public RadComboBoxClientCascade DefaultValue(string value)
        {
            this.defaultValue = value;
            return this;
        }

        /// <summary>
        /// 设置选中的值，并更新子控件
        /// </summary>
        /// <param name="value">值</param>
        public RadComboBoxClientCascade SetValue(string value)
        {
            RadComboBoxItem item = box.SetSelectedValue(value);
            if (item != null)
            {
                currentArg = new RadComboBoxItemsRequestedEventArgs();
                currentArg.Value = item.Value;
                currentArg.Text = item.Text;
                RaiseSelectChange();
            }
            return this;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="textfld">文本字段</param>
        /// <param name="valuefld">值字段</param>
        public RadComboBoxClientCascade BindData(object dataSource, string textfld, string valuefld)
        {
            box.BindData(dataSource, textfld, valuefld);
            AddDefaultItem();
            return this;
        }

        /// <summary>
        /// 关联子查询
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public RadComboBoxClientCascade SetSubQuery(RadComboBox box)
        {
            return SetSubQuery(box, null);
        }

        /// <summary>
        /// 关联子查询
        /// </summary>
        /// <param name="box"></param>
        /// <param name="itemRequested"></param>
        /// <returns></returns>
        public RadComboBoxClientCascade SetSubQuery(RadComboBox box, ItemsRequestedEvent itemRequested)
        {
            return SetSubQuery(new RadComboBoxClientCascade(box, itemRequested));
        }

        /// <summary>
        /// 关联子查询
        /// </summary>
        /// <param name="subQuery"></param>
        /// <returns></returns>
        public RadComboBoxClientCascade SetSubQuery(RadComboBoxClientCascade subQuery)
        {
            if (subQuery != null)
            {
                subQuery.box.EnableViewState = false;
            }
            this.subQuery = subQuery;
            return subQuery;
        }

        /// <summary>
        /// 控件
        /// </summary>
        public RadComboBox Control
        {
            get
            {
                return box;
            }
        }
    }
}