using System.Web.Optimization;

namespace Stardome
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jtable").Include(
                        "~/Scripts/jtable/jquery.jtable.min.js",
                        "~/Scripts/jtable/jquery.jtable.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
            //jtable
            bundles.Add(new StyleBundle("~/Content/jtable/css").Include(
                "~/Scripts/jtable/themes/metro/blue/jtable.min.css",
                "~/Scripts/jtable/themes/metro/blue/jtable.css",
                "~/Scripts/jtable/themes/basic/jtable_basic.css",
                "~/Scripts/jtable/themes/basic/jtable_basic.min.css",
                "~/Scripts/jtable/themes/jqueryui/jtable_jqueryui.css"
                ));
            
            //jstree
            bundles.Add(new ScriptBundle("~/Scripts/jsTree").Include(
                      "~/Scripts/jsTreeScript/jstree.js",                      
                      "~/Scripts/jsTreeScript/jqueryFileTree.js",
                      "~/Scripts/jsTreeScript/jquery.contextMenu.js",
                      "~/Scripts/jsTreeScript/jquery.ui.position.js",
                      "~/Scripts/jsTreeScript/jquery.easing.js",
                      "~/Scripts/jsTreeScript/fileTree.js",
                      "~/Scripts/jsTreeScript/test.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/jsTree").Include(
                        "~/Content/themes/jsTree/style.css",
                        "~/Content/themes/jsTree/jqueryFileTree.css",
                        "~/Content/themes/jsTree/jquery.contextMenu.css"));
            //bootstrap
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                        "~/Content/themes/bootstrap/css/bootstrap-theme.css",
                        "~/Content/themes/bootstrap/css/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include(
                      "~/Scripts/bootstrap/js/bootstrap.js"
                      ));

            //bootstrap
            bundles.Add(new StyleBundle("~/Content/bootstrapToggle").Include(
                        "~/Content/themes/bootstrap/css/bootstrap-toggle.css",
                        "~/Content/themes/bootstrap/css/bootstrap-toggle.min.css"
                        ));
            bundles.Add(new ScriptBundle("~/Scripts/bootstrapToggle").Include(
                      "~/Scripts/bootstrap/js/bootstrap-toggle.js",
                      "~/Scripts/bootstrap/js/bootstrap-toggle.min.js",
                      "~/Scripts/bootstrap/js/bootstrap-toggle.min.js.map"
                      ));
            
        }
    }
}