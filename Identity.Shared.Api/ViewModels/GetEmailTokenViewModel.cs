using System.ComponentModel.DataAnnotations;

namespace Identity.Shared.Api.ViewModels
{
    public class GetEmailTokenViewModel
    {
        [Required]
        public string? Email { get; set; }
        
        [Required]
        public string? VerificationKey { get; set; }

    }
}
