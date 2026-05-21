using System.ComponentModel.DataAnnotations;

namespace Asp_projekt.Models;

public class FakultetCreateViewModel
{
    [Required]
    [StringLength(120, MinimumLength = 2)]
    public string Naziv { get; set; } = string.Empty;
}
