using Microsoft.Practices.Unity;
using MyContractsGenerator.Core.Unity;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesServices;

namespace MyContractsGenerator.Tests.Business
{
    /// <summary>
    /// Classe de base pour tests de la couche service
    /// </summary>
    public abstract class TestsServiceBase<TEntity,TService>
        where TEntity : BaseEntity
        where TService : IService<TEntity>
    {
        protected IUnityContainer unityContainer;
        protected TService service;

        /// <summary>
        /// Initialise Unity pour l'injection de dépendence
        /// </summary>
        protected virtual void Init()
        {
            unityContainer = UnityConfig.GetConfiguredContainer();
            service = unityContainer.Resolve<TService>();
        }
    }
}
