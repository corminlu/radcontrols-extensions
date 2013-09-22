using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;

namespace Cormin.RadControlsExtensions
{
    public partial class demos_RadListBoxCascadeDemo : Page
    {
        RadListBoxCascade c;

        protected void Page_Load(object sender, EventArgs e)
        {
            m1.AddAjaxSetting(RadListBox1, RadListBox2, RadListBox3, RadListBox4, RadListBox5);
            m1.AddAjaxSetting(RadListBox2, RadListBox3, RadListBox4, RadListBox5);
            m1.AddAjaxSetting(RadListBox3, RadListBox4, RadListBox5);
            m1.AddAjaxSetting(RadListBox4, RadListBox5);
            m1.AddAjaxSetting(RadListBox5, RadListBoxDest);
            m1.AddAjaxSetting(Button1, RadListBox1, RadListBox2, RadListBox3, RadListBox4, RadListBox5, Literal1);

            c = new RadListBoxCascade(RadListBox1, Load1)/*.DefaultItem("选择1")*/;
            c.SetSubQuery(new RadListBoxCascade(RadListBox2, Load1))/*.DefaultItem("选择2").DefaultValue("2")*/
                .SetSubQuery(new RadListBoxCascade(RadListBox3, Load1))/*.DefaultItem("选择3").DefaultValue("4")*/
                .SetSubQuery(new RadListBoxCascade(RadListBox4, Load1))/*.DefaultItem("选择4").DefaultValue("8")*/
                .SetSubQuery(new RadListBoxCascade(RadListBox5, Load1))/*.DefaultItem("选择5").DefaultValue("16")*/;

            BindProperties();//////////////////
            if (!IsPostBack)
            {
                DoLoad();
            }
        }

        void DoLoad()
        {
            c.BindData(GenerateData(string.Empty), "Key", "Value").SetValue("0");
        }

        void Load1(RadListBoxCascade box, RadListBoxItem e)
        {
            box.BindData(GenerateData(e.Value), "Key", "Value");
        }

        object GenerateData(string val)
        {
            int x = 1;
            int.TryParse(val, out x);
            if (x < 1) x = 1;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < 10; i++)
            {
                dict.Add(val + "-key" + (i * x).ToString(), (i * x).ToString());
            }
            return dict;
        }

        private const string MessageTemplate = "{0} ### {1} ### {2} ### {3} ### {4}";
        protected void Button1_Click(object sender, EventArgs e)
        {
            Literal1.Text = string.Empty;

            Literal1.Text = string.Format(MessageTemplate, GetItemResult(RadListBox1), GetItemResult(RadListBox2), GetItemResult(RadListBox3), GetItemResult(RadListBox4), GetItemResult(RadListBox5));
            DoLoad();
        }

        string GetItemResult(RadListBox box)
        {
            var builder = new StringBuilder();
            builder.Append("(");
            builder.Append(box.ID);
            if (box.SelectedItem != null)
            {
                builder.Append("  Text:");
                builder.Append(box.SelectedItem.Text);
            }
            if (box.SelectedIndex > -1)
            {
                builder.Append("  Value:");
                builder.Append(box.SelectedValue);
            }
            builder.Append(")");
            return builder.ToString();
        }

        /// /////////////////////////////////////////////////////////
        void BindProperties()
        {
            RadListBoxCommandBinder boxBinder = new RadListBoxCommandBinder(RadListBox5, RadListBoxDest);
            boxBinder.ActionComplete += boxBinder_ActionComplete;
            boxBinder.DataBounded += boxBinder_DataBounded;
            boxBinder.Deleted += boxBinder_Deleted;
            boxBinder.Ordered += boxBinder_Ordered;
            boxBinder.Disabled += boxBinder_Disabled;
        }

        void boxBinder_ActionComplete(object sender, EventArgs e)
        {
            Literal1.Text += "<hr />Complete";
        }

        void boxBinder_DataBounded(RadListBoxItem obj)
        {
            Literal1.Text += "<hr />" + obj.Text + " - " + obj.Value;
        }

        void boxBinder_Deleted(RadListBoxItem obj)
        {
            Literal1.Text += "<hr />" + obj.Text + " - " + obj.Value;
        }

        void boxBinder_Ordered(RadListBoxItem obj, int index)
        {
            Literal1.Text += "<hr />" + obj.Text + " - " + obj.Value;
        }

        void boxBinder_Disabled(RadListBoxItem obj)
        {
            Literal1.Text += "<hr />" + obj.Text + " - " + obj.Value;
        }
    }
}