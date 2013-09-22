using System;
using System.Collections.Generic;
using System.Web.UI;
using Telerik.Web.UI;

namespace Cormin.RadControlsExtensions
{
    public partial class demos_RadComboBoxClientCascadeDemo : Page
    {
        RadComboBoxClientCascade c;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = new RadComboBoxClientCascade(RadComboBox1, Load1)/*.DefaultItem("选择1")*/;
            c.SetSubQuery(new RadComboBoxClientCascade(RadComboBox2, Load1))/*.DefaultItem("选择2").DefaultValue("2")*/
                .SetSubQuery(new RadComboBoxClientCascade(RadComboBox3, Load1))/*.DefaultItem("选择3").DefaultValue("4")*/
                .SetSubQuery(new RadComboBoxClientCascade(RadComboBox4, Load1))/*.DefaultItem("选择4").DefaultValue("8")*/
                .SetSubQuery(new RadComboBoxClientCascade(RadComboBox5, Load1))/*.DefaultItem("选择5").DefaultValue("16")*/;
            if (!IsPostBack)
            {
                DoLoad();
            }
            c.RegisterScript();
        }

        void DoLoad()
        {
            c.BindData(GenerateData(string.Empty), "Key", "Value");//.SetValue("0");
        }

        void Load1(RadComboBoxClientCascade box, RadComboBoxItemsRequestedEventArgs e)
        {
            box.BindData(GenerateData(e.Value), "Key", "Value");
            System.Diagnostics.Debug.WriteLine(box.Control.ID);
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

            Literal1.Text = string.Format(MessageTemplate, RadComboBox1.Text + " " + RadComboBox1.SelectedValue, RadComboBox2.Text + " " + RadComboBox2.SelectedValue, RadComboBox3.Text + " " + RadComboBox3.SelectedValue, RadComboBox4.Text + " " + RadComboBox4.SelectedValue, RadComboBox5.Text + " " + RadComboBox5.SelectedValue);
        }
    }
}