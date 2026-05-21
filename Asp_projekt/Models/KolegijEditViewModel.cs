using System.ComponentModel.DataAnnotations;

namespace Asp_projekt.Models;

public class KolegijEditViewModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(120, MinimumLength = 2)]
    public string Naziv { get; set; } = string.Empty;

    [Range(1, 30)]
    public int ECTS { get; set; } = 5;

    public int? FakultetId { get; set; }

    public List<LookupOption> Fakulteti { get; set; } = new();
}
