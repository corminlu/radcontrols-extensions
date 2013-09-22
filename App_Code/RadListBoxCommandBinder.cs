//
// extension methods for telerik radcontrols for asp.net ajax
//
// Copyright 2013 corminlu@gmail.com
//

using System;
using System.Collections.Generic;
using System.Globalization;
using Telerik.Web.UI;

namespace Cormin.RadControlsExtensions
{
    /// <summary>
    /// 绑定RadListBox的排序/拖动/移动/删除命令
    /// </summary>
    public class RadListBoxCommandBinder
    {
        private RadListBox sourceBox;
        private RadListBox destBox;

        /// <summary>
        /// 排序事件
        /// </summary>
        /// <param name="box"></param>
        /// <param name="index"></param>
        public delegate void OrderAction(RadListBoxItem box, int index);
        /// <summary>
        /// 操作完成事件
        /// </summary>
        public event EventHandler ActionComplete;
        /// <summary>
        /// 数据绑定完成事件
        /// </summary>
        public event Action<RadListBoxItem> DataBounded;
        /// <summary>
        /// 删除完成事件
        /// </summary>
        public event Action<RadListBoxItem> Deleted;
        /// <summary>
        /// 排序完成事件
        /// </summary>
        public event OrderAction Ordered;
        /// <summary>
        /// 禁用完成事件
        /// </summary>
        public event Action<RadListBoxItem> Disabled;

        /// <summary>
        /// 绑定RadListBox的排序/拖动/移动/删除命令
        /// </summary>
        /// <param name="sourceBox"></param>
        /// <param name="destBox"></param>
        public RadListBoxCommandBinder(RadListBox sourceBox, RadListBox destBox)
        {
            if (!sourceBox.Page.IsPostBack)
                sourceBox.TransferToID = destBox.ID;
            this.sourceBox = sourceBox;
            this.destBox = destBox;
            this.Init(sourceBox, true);
            this.Init(destBox, false);
        }

        private void Init(RadListBox box, bool isLeft)
        {
            if (!box.Page.IsPostBack)
            {
                box.AllowDelete = true;
                box.AllowReorder = true;
                if (isLeft)
                {
                    box.AllowTransfer = true;
                    box.AllowTransferOnDoubleClick = true;
                    box.AutoPostBackOnTransfer = true;
                }

                box.AutoPostBackOnDelete = true;
                box.AutoPostBackOnReorder = true;
                box.EnableDragAndDrop = true;
                box.Culture = CultureInfo.CurrentCulture;
                box.SelectionMode = ListBoxSelectionMode.Multiple;
            }

            if (isLeft) box.Transferring += new RadListBoxTransferringEventHandler(box_Transferring);

            box.ItemDataBound += new RadListBoxItemEventHandler(box_ItemDataBound);
            box.Reordered += new RadListBoxEventHandler(box_Reordered);

            box.Transferred += new RadListBoxTransferredEventHandler(box_Transferred);

            box.Deleting += new RadListBoxDeletingEventHandler(box_Deleting);
            box.Deleted += new RadListBoxEventHandler(box_Deleted);

            box.Dropping += new RadListBoxDroppingEventHandler(box_Dropping);
            box.Dropped += new RadListBoxDroppedEventHandler(box_Dropped);
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="box"></param>
        /// <param name="dataSource"></param>
        /// <param name="textField"></param>
        /// <param name="valueField"></param>
        public static void BindData(RadListBox box, object dataSource, string textField, string valueField)
        {
            box.DataSource = dataSource;
            box.DataTextField = textField;
            box.DataValueField = valueField;
            box.DataBind();
        }

        void box_ItemDataBound(object sender, RadListBoxItemEventArgs e)
        {
            if (DataBounded != null)
                DataBounded(e.Item);
        }

        void box_Reordered(object sender, RadListBoxEventArgs e)
        {
            RadListBox box = sender as RadListBox;
            ChangeItesOrder(box.Items);
        }

        bool IsTransferring
        {
            get
            {
                return sourceBox.Attributes["isTransferring"] == true.ToString();
            }
            set
            {
                sourceBox.Attributes["isTransferring"] = value.ToString();
            }
        }

        void box_Transferring(object sender, RadListBoxTransferringEventArgs e)
        {
            IsTransferring = true;
        }

        void box_Transferred(object sender, RadListBoxTransferredEventArgs e)
        {
            ChangeItemsDisabled(e.Items);
        }

        void box_Dropping(object sender, RadListBoxDroppingEventArgs e)
        {
            RadListBox box = sender as RadListBox;
            if (e.HtmlElementID != box.ClientID)
                IsTransferring = true;
        }

        void box_Dropped(object sender, RadListBoxDroppedEventArgs e)
        {
            RadListBox box = sender as RadListBox;
            if (e.HtmlElementID != box.ClientID)
            {
                ChangeItemsDisabled(e.SourceDragItems);
                IsTransferring = false;
            }
            else
                ChangeItesOrder(box.Items);
        }

        void box_Deleting(object sender, RadListBoxDeletingEventArgs e)
        {
            if (IsTransferring)
            {
                e.Cancel = true;
                IsTransferring = false;
            }
        }

        void box_Deleted(object sender, RadListBoxEventArgs e)
        {
            if (Deleted != null)
            {
                foreach (RadListBoxItem item in e.Items)
                {
                    Deleted(item);
                }
                OnActionComplete();
            }
        }

        void ChangeItesOrder(IList<RadListBoxItem> items)
        {
            if (Ordered != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    Ordered(items[i], i);
                }
                OnActionComplete();
            }
        }

        void ChangeItemsDisabled(IList<RadListBoxItem> items)
        {
            if (Disabled != null)
            {
                foreach (RadListBoxItem item in items)
                {
                    Disabled(item);
                }
                OnActionComplete();
            }
        }

        void OnActionComplete()
        {
            if (ActionComplete != null)
                ActionComplete(this, null);
        }
    }
}