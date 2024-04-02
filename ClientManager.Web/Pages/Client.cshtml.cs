using ClientManager.Core.Domain;
using ClientManager.Core.Processor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientManager.Web.Pages
{
    public class ClientModel : PageModel
    {
        private readonly IClientRequestProcessor _clientProcessor;

        public ClientModel(IClientRequestProcessor clientProcessor)
        {
            _clientProcessor = clientProcessor;
        }

        public ClientRequest Client { get; set; }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public IActionResult OnPost(ClientRequest client)
        {
            if (!ModelState.IsValid)
                return Page();

            var result = _clientProcessor.CreateClient(client);

            IsSuccess = result.IsSuccess;
            Message = result.Message;

            if (IsSuccess)
                return RedirectToPage("/ClientsList", new { id = result.Id });

            return Page();
        }
    }
}