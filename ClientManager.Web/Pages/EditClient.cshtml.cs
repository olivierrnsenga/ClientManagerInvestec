using ClientManager.Core.DataInterface.DeskBooker.Core.DataInterface;
using ClientManager.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientManager.Web.Pages
{
    public class EditClientModel : PageModel
    {
        private readonly IClientRepository _clientRepository;

        public EditClientModel(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [BindProperty] public ClientRequest Client { get; set; } 

        public IActionResult OnGet(int id)
        {
            var clientEntity = _clientRepository.GetById(id);

            Client = new ClientRequest
            {
                Id = clientEntity.Id,
                FirstName = clientEntity.FirstName,
                LastName = clientEntity.LastName,
                MobileNumber = clientEntity.MobileNumber,
                IdNumber = clientEntity.IdNumber,
                PhysicalAddress = clientEntity.PhysicalAddress
            };

            if (Client == null) return RedirectToPage("/ClientsList");
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            _clientRepository.Update(Client);

            return RedirectToPage("/ClientsList");
        }
    }
}