using System.ComponentModel.DataAnnotations;

namespace Asp_projekt.Models;

public class ProfesorCreateViewModel
{
    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Ime { get; set; } = string.Empty;

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Prezime { get; set; } = string.Empty;

    [Required]
    [StringLength(120, MinimumLength = 2)]
    public string Katedra { get; set; } = string.Empty;

    public int? FakultetId { get; set; }

    public List<LookupOption> Fakulteti { get; set; } = new();
}
