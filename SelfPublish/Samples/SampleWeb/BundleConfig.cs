using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace SampleWeb {
    public class BundleConfig {
        public static void RegisterTemplateBundles() {
            Bundle jsBundle = new Bundle("~/bundles/WebFormsJs", new JsMinify());
            jsBundle.AddFile("~/Scripts/WebForms/WebForms.js");
            jsBundle.AddFile("~/Scripts/WebForms/MenuStandards.js");
            jsBundle.AddFile("~/Scripts/WebForms/Focus.js");
            jsBundle.AddFile("~/Scripts/WebForms/GridView.js");
            jsBundle.AddFile("~/Scripts/WebForms/DetailsView.js");
            jsBundle.AddFile("~/Scripts/WebForms/TreeView.js");
            jsBundle.AddFile("~/Scripts/WebForms/WebParts.js");
            BundleTable.Bundles.Add(jsBundle);

            // Order is very important for these files to work, they have explicit dependencies
            Bundle ajaxBundle = new Bundle("~/bundles/MsAjaxJs", new JsMinify());
            ajaxBundle.AddFile("~/Scripts/WebForms/MsAjax/MicrosoftAjax.js");
            ajaxBundle.AddFile("~/Scripts/WebForms/MsAjax/MicrosoftAjaxCore.js");
            ajaxBundle.AddFile("~/Scripts/WebForms/MsAjax/MicrosoftAjaxSerialization.js");
            ajaxBundle.AddFile("~/Scripts/WebForms/MsAjax/MicrosoftAjaxNetwork.js");
            ajaxBundle.AddFile("~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebServices.js");
            ajaxBundle.AddFile("~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js");
            ajaxBundle.AddFile("~/Scripts/WebForms/MsAjax/MicrosoftAjaxComponentModel.js");
            ajaxBundle.AddFile("~/Scripts/WebForms/MsAjax/MicrosoftAjaxGlobalization.js");
            ajaxBundle.AddFile("~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js");
            ajaxBundle.AddFile("~/Scripts/WebForms/MsAjax/MicrosoftAjaxHistory.js");
            ajaxBundle.AddFile("~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js");
            BundleTable.Bundles.Add(ajaxBundle);

            Bundle cssBundle = new Bundle("~/Content/css", new CssMinify());
            cssBundle.TryAddFile("~/Content/site.css");
            BundleTable.Bundles.Add(cssBundle);

            Bundle jqueryUiCssBundle = new Bundle("~/Content/themes/base/css", new CssMinify());
            jqueryUiCssBundle.TryAddFile("~/Content/themes/base/jquery.ui.core.css");
            jqueryUiCssBundle.TryAddFile("~/Content/themes/base/jquery.ui.resizable.css");
            jqueryUiCssBundle.TryAddFile("~/Content/themes/base/jquery.ui.selectable.css");
            jqueryUiCssBundle.TryAddFile("~/Content/themes/base/jquery.ui.accordion.css");
            jqueryUiCssBundle.TryAddFile("~/Content/themes/base/jquery.ui.autocomplete.css");
            jqueryUiCssBundle.TryAddFile("~/Content/themes/base/jquery.ui.button.css");
            jqueryUiCssBundle.TryAddFile("~/Content/themes/base/jquery.ui.dialog.css");
            jqueryUiCssBundle.TryAddFile("~/Content/themes/base/jquery.ui.slider.css");
            jqueryUiCssBundle.TryAddFile("~/Content/themes/base/jquery.ui.tabs.css");
            jqueryUiCssBundle.TryAddFile("~/Content/themes/base/jquery.ui.datepicker.css");
            jqueryUiCssBundle.TryAddFile("~/Content/themes/base/jquery.ui.progressbar.css");
            jqueryUiCssBundle.TryAddFile("~/Content/themes/base/jquery.ui.theme.css");
            BundleTable.Bundles.Add(jqueryUiCssBundle);

        }
    }
}