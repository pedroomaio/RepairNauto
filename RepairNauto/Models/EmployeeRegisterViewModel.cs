using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AutoRepair.Models
{
    public class EmployeeRegisterViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        public bool IsMechanic { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
