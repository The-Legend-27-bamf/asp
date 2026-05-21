using System.ComponentModel.DataAnnotations;

namespace Asp_projekt.Models;

public class FakultetEditViewModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(120, MinimumLength = 2)]
    public string Naziv { get; set; } = string.Empty;
}
