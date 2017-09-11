using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesRepo
{
    public interface ICollaboratorRepository : IBaseRepository<collaborator>
    {
        /// <summary>
        ///     Gets Administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new collaborator GetById(int id);

        /// <summary>
        ///     Gets administrator by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        collaborator GetByEmail(string email);

        IEnumerable<collaborator> GetAllActive();
    }
}