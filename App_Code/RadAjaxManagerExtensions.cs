//
// extension methods for telerik radcontrols for asp.net ajax
//
// Copyright 2013 corminlu@gmail.com
//

using System.Web.UI;
using Telerik.Web.UI;

namespace Cormin.RadControlsExtensions
{
    public static class RadAjaxManagerExtensions
    {
        public static void AddAjaxSettingsFromTwoContainer(this RadAjaxManager manager, Control sourceContainer, Control targetContainer)
        {
            foreach (Control source in sourceContainer.Controls)
            {
                if (!IsControlValid(source))
                    continue;

                AddAjaxSettingsFromTargetContainer(manager, source, targetContainer);
            }
        }

        public static void AddAjaxSettingsFromTargetContainer(this RadAjaxManager manager, Control sourceControl, Control targetContainer)
        {
            foreach (Control target in targetContainer.Controls)
            {
                AddAjaxSetting(manager, sourceControl, target);
            }
        }

        public static void AddAjaxSettingsFromSourceContainer(this RadAjaxManager manager, Control sourceContainer, params Control[] targets)
        {
            foreach (Control source in sourceContainer.Controls)
            {
                if (!IsControlValid(source))
                    continue;
                AddAjaxSetting(manager, source, targets);
            }
        }

        static bool IsControlValid(Control control)
        {
            return !string.IsNullOrEmpty(control.ID) &&
                !(control is LiteralControl);
        }

        public static void AddAjaxSetting(this RadAjaxManager manager, Control sourceControl, params Control[] targets)
        {
            var setting = new AjaxSetting(sourceControl.ID);
            foreach (var target in targets)
            {
                if (!IsControlValid(target))
                    continue;

                var update = new AjaxUpdatedControl();
                update.ControlID = target.ID;
                setting.UpdatedControls.Add(update);
            }
            manager.AjaxSettings.Add(setting);
        }
    }
}