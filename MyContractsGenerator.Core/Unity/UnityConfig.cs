//------------------------------------------------------------------------------
// <summary>
//    APPLICATION : MyContractsGenerator
//    Author : Clement Fevre
//    Description : Configuration Unity
// </summary>
//------------------------------------------------------------------------------
using System;
using Microsoft.Practices.Unity;
using MyContractsGenerator.Business;
using MyContractsGenerator.DAL;
using MyContractsGenerator.DAL.Repositories;
using MyContractsGenerator.Interfaces.InterfacesRepo;
using MyContractsGenerator.Interfaces.InterfacesServices;

namespace MyContractsGenerator.Core.Unity
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        private static Lazy<IUnityContainer> containerLife = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container, new PerRequestLifetimeManager());
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer(bool withLifetimeManager = false)
        {
            if (!withLifetimeManager)
            {
                return container.Value;
            }
            else
            {
                return containerLife.Value;
            }
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        private static void RegisterTypes(IUnityContainer container, LifetimeManager lifetimeManager = null)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            container.RegisterType<IAdministratorRepository, AdministratorRepository>();
            container.RegisterType<ICollaboratorRepository, CollaboratorRepository>();
            container.RegisterType<IRoleRepository, RoleRepository>();
            container.RegisterType<IAnswerRepository, AnswerRepository>();
            container.RegisterType<IFormAnswerRepository, FormAnswerRepository>();
            container.RegisterType<IFormRepository, FormRepository>();
            container.RegisterType<IQuestionRepository, QuestionRepository>();
            container.RegisterType<IQuestionTypeRepository,IQuestionTypeRepository>();

            container.RegisterType<IAdministratorService, AdministratorService>();
            container.RegisterType<ICollaboratorService, CollaboratorService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IAnswerService, AnswerService>();
            container.RegisterType<IFormAnswerService, FormAnswerService>();
            container.RegisterType<IFormService, FormService>();
            container.RegisterType<IQuestionService, QuestionService>();
            container.RegisterType<IQuestionTypeService, QuestionTypeService>();
            
            container.RegisterType<IMailService, MailService>();

            if (null == lifetimeManager)
            {
                container.RegisterType<MyContractsGeneratorEntities>();
            }
            else
            {
                container.RegisterType<MyContractsGeneratorEntities>(lifetimeManager);
            }

            DAL.AutoMapping.Configure();
        }
    }
}
