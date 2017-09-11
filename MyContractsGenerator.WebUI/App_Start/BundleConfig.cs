using System.Web.Optimization;

namespace MyContractsGenerator.WebUI
{
    public static class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                            "~/Scripts/jquery-{version}.js",
                            "~/Scripts/jquery-ui-1.11.4.custom/jquery-ui.js",
                            "~/Scripts/jquery.validate*",
                            "~/Scripts/bootstrap-multiselect.js",
                            "~/Scripts/bootstrap-multiselect-collapsible-groups.js",
                            "~/Scripts/placeholders.jquery.js",
                            "~/Scripts/bootstrap.js",
                            "~/Scripts/respond.js",
                            "~/Scripts/bootstrap-notify.js",
                            "~/Scripts/tagmanager.js",
                            "~/Scripts/Customs/custom-shared.js").ForceOrdered());

            bundles.Add(new StyleBundle("~/Content/css").Include(
                            "~/Content/bootstrap.css",
                            "~/Content/bootstrap-multiselect.css",
                            "~/Content/site.css",
                            "~/Content/simple-sidebar.css",
                            "~/Content/jquery-ui.css",
                            "~/Content/jquery-ui.structure",
                            "~/Content/jquery-ui.theme.css",
                            "~/Content/bootstrap-table.css",
                            "~/Content/icomoon.css").ForceOrdered());
        }
    }
}