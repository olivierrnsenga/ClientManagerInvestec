using System.Collections.Generic;
using System.Linq;
using ClientManager.Core.Domain;
using ClientManager.Core.Processor;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientManager.Web.Pages
{
    public class SearchClientsModel : PageModel
    {
        private readonly IClientRequestProcessor _clientProcessor;

        public SearchClientsModel(IClientRequestProcessor clientProcessor)
        {
            _clientProcessor = clientProcessor;
        }

        public IEnumerable<Client> Clients { get; set; }

        public void OnGet(string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                Clients = _clientProcessor.SearchClients(searchTerm).ToList();
            }
        }
    }
}