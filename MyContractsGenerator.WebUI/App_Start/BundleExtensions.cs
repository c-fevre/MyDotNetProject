using System.Web.Optimization;
using MyContractsGenerator.WebUI.App_Start;

namespace MyContractsGenerator.WebUI
{
    internal static class BundleExtensions
    {
        public static Bundle ForceOrdered(this Bundle sb)
        {
            sb.Orderer = new AsIsBundleOrderer();
            return sb;
        }
    }
}