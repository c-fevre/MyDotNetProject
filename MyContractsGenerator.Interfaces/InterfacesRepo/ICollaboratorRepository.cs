using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyContractsGenerator.DAL;
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
