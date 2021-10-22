using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoRepair.Data.Entities
{
    public class Brand : IEntity
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "Brand")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }
        public string UserId { get; set; }
        public ICollection<User> Users{ get; set; }
    }
}