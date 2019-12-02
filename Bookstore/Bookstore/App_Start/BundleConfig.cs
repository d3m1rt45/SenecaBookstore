using System.Web;
using System.Web.Optimization;

namespace Bookstore
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.3.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalunobstrusive").Include(
                        "~/Scripts/jquery.validate.unobstrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/external-libraries").Include(
                      "~/Content/libraries/bootstrap.css",
                      "~/Content/libraries/fontawesome/font-awesome.min.css",
                      "~/Content/libraries/hover.css",
                      "~/Content/libraries/PagedList.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/project-specific").Include(
                      "~/Content/project-specific/Shared.css",
                      "~/Content/project-specific/PageSpecific.css",
                      "~/Content/project-specific/Fonts.css"));
        }
    }
}
