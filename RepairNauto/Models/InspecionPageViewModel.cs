using System;
using System.ComponentModel.DataAnnotations;

namespace RepairNauto.Models
{
    public class InspecionPageViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime InspecionDateStart { get; set; }
        public DateTime InspecionDate { get; set; }
        public string InspecionHours { get; set; }

        public double Price { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
           ? $"https://autorepairtpsi.azurewebsites.net/Img/noimage.png"
           : $"https://autorepairtpsi.blob.core.windows.net/users/{ImageId}";
    }
}
