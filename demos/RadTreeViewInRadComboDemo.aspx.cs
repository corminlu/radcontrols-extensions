using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Cormin.RadControlsExtensions
{
    public partial class demos_RadTreeViewInRadComboDemo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            m1.AddAjaxSettingsFromTwoContainer(phSource, phResult);
            if (!IsPostBack)
            {
                var data = new List<KeyValuePair<string, string>>();
                for (int i = 0; i < 20; i++)
                {
                    data.Add(new KeyValuePair<string, string>(i.ToString() + "-KeyKeyKeyKeyKey", "value" + i.ToString()));
                }
                var treeView = RadComboBox1.FindNestedTreeView();
                treeView.DataSource = data;
                treeView.DataBind();
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Literal1.Text = RadComboBox1.Text;
            Literal2.Text = RadComboBox1.SelectedValue;
        }
    }
}