using System.ComponentModel.DataAnnotations;

namespace Identity.Shared.Api.ViewModels
{
    public class NewMobileKeyViewModel
    {
        [Required]
        public string? Mobile { get; set; }
        

    }
}
