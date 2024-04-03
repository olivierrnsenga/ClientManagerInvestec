using System;
using System.Collections.Generic;
using System.Linq;
using ClientManager.Core.DataInterface.DeskBooker.Core.DataInterface;
using ClientManager.Core.Domain;

namespace ClientManager.Core.Processor
{
    public class ClientRequestProcessor : IClientRequestProcessor
    {
        private readonly IClientRepository _clientRepository;

        public ClientRequestProcessor(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
        }

        public ClientResponse CreateClient(ClientRequest client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            var result = new ClientResponse();
            try
            {
                _clientRepository.Add(client);
                result.IsSuccess = true;
                result.Message = "Client created successfully.";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"An error occurred: {ex.Message}";
            }

            return result;
        }

        public ClientResponse GetClient(int clientId)
        {
            var result = new ClientResponse();
            try
            {
                var client = _clientRepository.GetById(clientId);
                if (client != null)
                {
                    result.IsSuccess = true;
                    result.Message = "Client retrieved successfully.";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Client not found.";
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"An error occurred: {ex.Message}";
            }

            return result;
        }

        public ClientResponse UpdateClient(ClientRequest client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            var result = new ClientResponse();
            try
            {
                _clientRepository.Update(client);
                result.IsSuccess = true;
                result.Message = "Client updated successfully.";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"An error occurred: {ex.Message}";
            }

            return result;
        }

        public ClientResponse DeleteClient(int clientId)
        {
            var result = new ClientResponse();
            try
            {
                _clientRepository.Delete(clientId);
                result.IsSuccess = true;
                result.Message = "Client deleted successfully.";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"An error occurred: {ex.Message}";
            }

            return result;
        }

        public IEnumerable<Client> GetAllClients()
        {
            var allClients = Enumerable.Empty<Client>();

            try
            {
                allClients = _clientRepository.GetAll();

                if (allClients == null) allClients = Enumerable.Empty<Client>();
            }
            catch (Exception)
            {
                allClients = Enumerable.Empty<Client>();
            }

            return allClients;
        }

        public IEnumerable<Client> SearchClients(string searchTerm)
        {
            return _clientRepository.Search(searchTerm);
        }
    }
}