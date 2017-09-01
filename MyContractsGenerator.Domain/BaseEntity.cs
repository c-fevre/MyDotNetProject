//------------------------------------------------------------------------------
// <summary>
//    APPLICATION : MyContractsGenerator
//    Author : Clement Fevre
//    Description : Classe de base des entités métier
// </summary>
//------------------------------------------------------------------------------
namespace MyContractsGenerator.Domain
{
    /// <summary>
    /// Classe de base des entités métier
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets la clé des entités
        /// </summary>
        public int Id
        {
            get;
            set;
        }
    }
}
