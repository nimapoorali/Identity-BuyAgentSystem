using System.ComponentModel.DataAnnotations;

namespace Identity.Shared.Api.ViewModels
{
    public class GetMobileTokenViewModel
    {
        [Required]
        public string? Mobile { get; set; }
        
        [Required]
        public string? VerificationKey { get; set; }

    }
}
