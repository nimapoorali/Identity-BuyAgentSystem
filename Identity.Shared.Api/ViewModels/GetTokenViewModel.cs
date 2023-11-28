using System.ComponentModel.DataAnnotations;

namespace Identity.Shared.Api.ViewModels
{
    public class GetTokenViewModel
    {
        [Required]
        public string? Username { get; set; }
        
        [Required]
        public string? Password { get; set; }

    }
}
