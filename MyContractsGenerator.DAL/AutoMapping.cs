//------------------------------------------------------------------------------
// <summary>
//    APPLICATION : MyContractsGenerator
//    Author : Clement Fevre
//    Description : Classe de Mapping entre les entités de domaine et les tables
// </summary>
//------------------------------------------------------------------------------
using System.Text.RegularExpressions;
using AutoMapper;

namespace MyContractsGenerator.DAL
{
    /// <summary>
    ///     Classe de Mapping entre les entités de domaine et les tables
    /// </summary>
    public static class AutoMapping
    {
        internal static IMapper Mapper { get; private set; }

        /// <summary>
        ///     Configuration du mapping
        /// </summary>
        public static void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
                cfg.DestinationMemberNamingConvention = new PascalCaseNamingConvention();

                /*cfg.CreateMap<collaborator, collaborator>()
                    .ForMember(x => x.Id, x => x.MapFrom(y => y.id))
                    .ForMember(x => x.Departements, x => x.MapFrom(y => y.departements));

                cfg.CreateMap<collaborator, collaborator>()
                    .ForMember(x => x.id_collaborateur, x => x.MapFrom(y => y.Id))
                    .ForMember(x => x.departements, x => x.MapFrom(y => y.Departements))
                    .ForMember(x => x.departements1, x => x.Ignore());

                cfg.CreateMap<Departement, departement>()
                    .ForMember(x => x.id_departement, x => x.MapFrom(y => y.Id))
                    .ForMember(x => x.id_departement_parent, x => x.MapFrom(y => y.Parent.Id))
                    .ForMember(x => x.id_responsable, x => x.MapFrom(y => y.Responsable.Id))
                    .ForMember(x => x.collaborateur, x => x.MapFrom(y => y.Responsable))
                    .ForMember(x => x.departement1, x => x.Ignore())
                    .ForMember(x => x.departement2, x => x.MapFrom(y => y.Parent))
                    .ForMember(x => x.collaborateurs, x => x.Ignore());

                cfg.CreateMap<departement, Departement>()
                    .ForMember(x => x.Id, x => x.MapFrom(y => y.id_departement))
                    .ForMember(x => x.Responsable, x => x.MapFrom(y => y.collaborateur))
                    .ForMember(x => x.Parent, x => x.MapFrom(y => y.departement2));*/
            });
            config.AssertConfigurationIsValid();

            Mapper = config.CreateMapper();
        }

        #region UpperUnderscoreNamingConvention

        private class UpperUnderscoreNamingConvention : INamingConvention
        {
            public Regex SplittingExpression { get; } = new Regex(@"[p{Lu}0-9]+(?=_?)");

            public string SeparatorCharacter { get; } = "_";

            public string ReplaceValue(Match match)
            {
                return match.Value.ToUpper();
            }
        }

        #endregion
    }
}