using System.Collections.Generic;
using ClientManager.Core.Domain;
using ClientManager.Core.Processor;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientManager.Web.Pages
{
    public class ClientsListModel : PageModel
    {
        private readonly IClientRequestProcessor _clientProcessor;

        public List<Client> Clients { get; set; }
        public Client ClientToUpdate { get; set; }

        public ClientsListModel(IClientRequestProcessor clientProcessor)
        {
            _clientProcessor = clientProcessor;
            Clients = new List<Client>();
            ClientToUpdate = new Client(); 

        }

        public void OnGet()
        {
            Clients = new List<Client>(_clientProcessor.GetAllClients());
        }
    }
}