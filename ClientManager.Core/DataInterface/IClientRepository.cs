using ClientManager.Core.Domain;
using System.Collections.Generic;

namespace ClientManager.Core.DataInterface
{
    namespace DeskBooker.Core.DataInterface
    {
        public interface IClientRepository
        {
            void Add(ClientRequest client);

            Client GetById(int clientId);

            IEnumerable<Client> GetAll();

            void Update(ClientRequest client);

            void Delete(int clientId);
            public IEnumerable<Client> Search(string searchTerm);


        }
    }
}