using System.Web.Optimization;

namespace Hasse.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;
            bundles.UseCdn = true;

            Scripts(bundles);
            Styles(bundles);
        }

        private static void Styles(BundleCollection bundles)
        {
            var styles = new StyleBundle("~/assets/css")
                .IncludeDirectory("~/assets/css", "*.css", true);

            bundles.Add(styles);
        }

        private static void Scripts(BundleCollection bundles)
        {
            // jQuery
            bundles.Add(new ScriptBundle("~/assets/jquery",
                "//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js")
                .Include("~/assets/jquery-{version}.js"));

            // ours
            bundles.Add(new ScriptBundle("~/assets/js")
                .IncludeDirectory("~/assets/js", "*.js", true));

        }
    }
}