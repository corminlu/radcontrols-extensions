using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;

namespace Cormin.RadControlsExtensions
{
    public partial class demos_RadGridExtractValueDemo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            m1.AddAjaxSetting(rg, rg);
            m1.AddAjaxSettingsFromTargetContainer(rg, phResult);
        }

        protected void rg_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var data = new List<dynamic>();
            for (int i = 0; i < 100; i++)
            {
                data.Add(new
                {
                    Id = i,
                    Text = "Text" + i.ToString(),
                    Value = "Value" + i.ToString(),
                    Other = "Other" + i.ToString(),
                    Other2 = "Other2" + i.ToString(),
                    Date = DateTime.Now
                });
            }
            rg.DataSource = data;
        }

        protected void rg_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            ShowResult(e);
        }

        protected void rg_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item ||
                e.Item.ItemType == GridItemType.AlternatingItem)
            {
                if (e.CommandName == "Command1")
                    ShowResult(e);
            }
            else if (e.Item.ItemType == GridItemType.CommandItem)
            {
                if (e.CommandName == "GetSelected")
                {
                    ShowSelected();
                }
            }
        }

        void ShowResult(GridCommandEventArgs e)
        {
            liId.Text = e.ExtractValue<int>(v => int.Parse(v)).ToString();
            liText.Text = e.ExtractValue<string>(v => v, "Text");
            liValue.Text = e.ExtractValue<string>(v => v, "Value");
            liOther.Text = e.ExtractValue<string>(v => v, "Other");
            liOther2.Text = e.ExtractValue<string>(v => v, "Other2");
            liDate.Text = e.ExtractValue<DateTime>(v => DateTime.Parse(v), "Date").ToString();
        }

        void ShowResult2(GridItem item)
        {
            liId.Text = item.ExtractValue<int>(v => int.Parse(v)).ToString();
            liText.Text = item.ExtractValue<string>(v => v, "Text");
            liValue.Text = item.ExtractValue<string>(v => v, "Value");
            liOther.Text = item.ExtractValue<string>(v => v, "Other");
            liOther2.Text = item.ExtractValue<string>(v => v, "Other2");
            liDate.Text = item.ExtractValue<DateTime>(v => DateTime.Parse(v), "Date").ToString();
        }

        void ShowSelected()
        {
            var selected = rg.ExtractSelected<int>(v => int.Parse(v));
            if (selected.Count > 0)
            {
                var builder = new StringBuilder();
                builder.Append("<fieldset><legend>selected</legend>");
                foreach (GridItem item in rg.SelectedItems)
                {
                    ShowResult2(item);
                    var sw = new StringWriter();
                    var tw = new HtmlTextWriter(sw);
                    phResult1.RenderControl(tw);
                    builder.Append(sw.ToString());
                }
                //
                builder.Append("<div style=\"border:1px solid #333;padding:5px;margin:5px;\">");
                builder.Append("Selected IDs:");
                builder.Append(string.Join(",", selected.ToArray()));
                builder.Append("</div>");
                builder.Append("</fieldset>");

                liSelected.Text = builder.ToString();
            }
        }
    }
}