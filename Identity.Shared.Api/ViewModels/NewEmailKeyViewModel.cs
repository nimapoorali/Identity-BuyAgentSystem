using System.ComponentModel.DataAnnotations;

namespace Identity.Shared.Api.ViewModels
{
    public class NewEmailKeyViewModel
    {
        [Required]
        public string? Email { get; set; }
        

    }
}
