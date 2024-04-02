using ClientManager.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace ClientManager.Core.Domain
{
    public class Client
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [MobileNumber(ErrorMessage = "Invalid Mobile Number format.")]
        [UniqueMobileNumber(ErrorMessage = "A client with the MobileNumber '{0}' already exists.")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "ID Number is required.")]
        [SouthAfricanIdNumber(ErrorMessage = "Invalid South African ID Number.")]
        [UniqueIdNumber(ErrorMessage = "A client with the IdNumber '{0}' already exists.")]
        public string IdNumber { get; set; }

        public string PhysicalAddress { get; set; }
    }
}