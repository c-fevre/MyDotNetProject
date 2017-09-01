//------------------------------------------------------------------------------
// <summary>
//    APPLICATION : MyContractsGenerator
//    Author : Clement Fevre
//    Description : Démarrage / Arrêt du container 
// </summary>
//------------------------------------------------------------------------------
using System.Linq;
using System.Web.Mvc;
using System.Web.Http;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MyContractsGenerator.Core.Unity.UnityWebActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(MyContractsGenerator.Core.Unity.UnityWebActivator), "Shutdown")]

namespace MyContractsGenerator.Core.Unity
{
    /// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start() 
        {
            var container = UnityConfig.GetConfiguredContainer(true);

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new Microsoft.Practices.Unity.Mvc.UnityFilterAttributeFilterProvider(container));

            DependencyResolver.SetResolver(new Microsoft.Practices.Unity.Mvc.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new global::Unity.WebApi.UnityDependencyResolver(container);

            // TODO: Uncomment if you want to use PerRequestLifetimeManager
            // Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer(true);
            container.Dispose();
        }
    }
}
