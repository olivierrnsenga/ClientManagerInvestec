using System.Collections.Generic;
using ClientManager.Core.Domain;

namespace ClientManager.Core.Processor
{
    public interface IClientRequestProcessor
    {
        ClientResponse CreateClient(ClientRequest client);

        ClientResponse GetClient(int clientId);

        ClientResponse UpdateClient(ClientRequest client);

        ClientResponse DeleteClient(int clientId);

        IEnumerable<Client> GetAllClients();

        IEnumerable<Client> SearchClients(string searchTerm);
    }
}