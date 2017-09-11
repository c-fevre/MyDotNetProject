using System.Collections.Generic;
using System.Web.Optimization;

namespace MyContractsGenerator.WebUI.App_Start
{
    internal class AsIsBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}