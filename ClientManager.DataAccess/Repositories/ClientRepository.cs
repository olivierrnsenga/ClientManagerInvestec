using System;
using System.Collections.Generic;
using System.Linq;
using ClientManager.Core.DataInterface.DeskBooker.Core.DataInterface;
using ClientManager.Core.Domain;

namespace ClientManager.DataAccess.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientContext _context;

        public ClientRepository(ClientContext context)
        {
            _context = context;
        }

        public void Add(ClientRequest client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public Client GetById(int clientId)
        {
            return _context.Clients.FirstOrDefault(x => x.Id == clientId);
        }

        public IEnumerable<Client> GetAll()
        {
            return _context.Clients.OrderBy(x => x.FirstName).ToList();
        }

        public void Update(ClientRequest client)
        {
            var existingClient = _context.Clients.Find(client.Id);
            if (existingClient != null)
            {
                existingClient.FirstName = client.FirstName;
                existingClient.LastName = client.LastName;
                existingClient.MobileNumber = client.MobileNumber;
                existingClient.PhysicalAddress = client.PhysicalAddress;
                existingClient.IdNumber = client.IdNumber;

                _context.Clients.Update(existingClient);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Client not found", nameof(client));
            }
        }

        public void Delete(int clientId)
        {
            var client = _context.Clients.Find(clientId);
            if (client != null)
            {
                _context.Clients.Remove(client);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Client not found", nameof(clientId));
            }
        }

        public IEnumerable<Client> Search(string searchTerm)
        {
            searchTerm = searchTerm.Trim().ToLower(); 

            return _context.Clients
                .Where(c => c.FirstName.ToLower().Contains(searchTerm)
                            || c.IdNumber.Trim() == searchTerm
                            || c.MobileNumber.Trim() == searchTerm)
                .ToList();
        }

    }
}