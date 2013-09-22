//
// extension methods for telerik radcontrols for asp.net ajax
//
// Copyright 2013 corminlu@gmail.com
//

using System;
using Telerik.Web.UI;

namespace Cormin.RadControlsExtensions
{
    /// <summary>
    /// ListBox多级联动
    /// </summary>
    public class RadListBoxCascade
    {
        RadListBox box;
        RadListBoxItem defaultItem;
        string defaultValue;
        /// <summary>
        /// 选择变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item"></param>
        public delegate void SelectChangedEvent(RadListBoxCascade sender, RadListBoxItem item);
        SelectChangedEvent selectChanged;
        RadListBoxCascade subQuery;

        /// <summary>
        /// ListBox多级联动
        /// </summary>
        /// <param name="box"></param>
        /// <param name="selectChanged"></param>
        public RadListBoxCascade(RadListBox box, SelectChangedEvent selectChanged)
        {
            this.box = box;
            if (selectChanged != null)
            {
                this.selectChanged = selectChanged;
                box.SelectedIndexChanged += box_SelectedIndexChanged;
            }
        }

        void box_SelectedIndexChanged(object sender, EventArgs e)
        {
            RaiseSelectChange();
        }

        /// <summary>
        /// 选择变更时执行
        /// </summary>
        void RaiseSelectChange()
        {
            ResetSubList();////

            if (defaultItem == null && box.SelectedIndex > -1 || defaultItem != null && box.SelectedIndex > 0)
            {
                //执行选择变更的方法，并且递归到每个子查询
                if (selectChanged != null && subQuery != null && subQuery.box.Visible)
                {
                    selectChanged(subQuery, box.SelectedItem);
                    if (!string.IsNullOrEmpty(subQuery.defaultValue) && !box.Page.IsPostBack)
                    {//初次加载时设置值
                        subQuery.SetValue(subQuery.defaultValue);
                    }
                    subQuery.RaiseSelectChange();
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
        /// 设置默认项
        /// </summary>
        /// <param name="text"></param>
        public RadListBoxCascade DefaultItem(string text)
        {
            return DefaultItem(text, string.Empty);
        }

        /// <summary>
        /// 设置默认项
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        public RadListBoxCascade DefaultItem(string text, string value)
        {
            return DefaultItem(new RadListBoxItem(text, value));
        }

        /// <summary>
        /// 设置默认项
        /// </summary>
        /// <param name="item"></param>
        public RadListBoxCascade DefaultItem(RadListBoxItem item)
        {
            if (defaultItem == null)
                this.defaultItem = item;
            if (box.Items.Count == 0 || box.Items.Count > 0 && box.FindItem(delegate(RadListBoxItem li)
            {
                return li.Text == item.Text && li.Value == item.Value;
            }) == null)
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
        public RadListBoxCascade DefaultValue(string value)
        {
            this.defaultValue = value;
            return this;
        }

        /// <summary>
        /// 设置选中的值，并更新子控件
        /// </summary>
        /// <param name="value">值</param>
        public RadListBoxCascade SetValue(string value)
        {
            if (box.SetSelectedValue(value) != null)
                RaiseSelectChange();
            return this;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="textfld">文本字段</param>
        /// <param name="valuefld">值字段</param>
        public RadListBoxCascade BindData(object dataSource, string textfld, string valuefld)
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
        public RadListBoxCascade SetSubQuery(RadListBox box)
        {
            return SetSubQuery(box, null);
        }

        /// <summary>
        /// 关联子查询
        /// </summary>
        /// <param name="box"></param>
        /// <param name="selectChanged"></param>
        /// <returns></returns>
        public RadListBoxCascade SetSubQuery(RadListBox box, SelectChangedEvent selectChanged)
        {
            return SetSubQuery(new RadListBoxCascade(box, selectChanged));
        }

        /// <summary>
        /// 关联子查询
        /// </summary>
        /// <param name="subQuery"></param>
        /// <returns></returns>
        public RadListBoxCascade SetSubQuery(RadListBoxCascade subQuery)
        {
            if (subQuery != null)
            {
                box.AutoPostBack = true;
            }
            this.subQuery = subQuery;
            return subQuery;
        }
    }
}