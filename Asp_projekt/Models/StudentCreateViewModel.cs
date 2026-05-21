using System.ComponentModel.DataAnnotations;

namespace Asp_projekt.Models;

public class StudentCreateViewModel
{
    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Ime { get; set; } = string.Empty;

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Prezime { get; set; } = string.Empty;

    [Required]
    public DateTime DatumUpisa { get; set; } = DateTime.Today;

    public int? FakultetId { get; set; }
    public int? KolegijId { get; set; }

    public List<LookupOption> Fakulteti { get; set; } = new();
    public List<LookupOption> Kolegiji { get; set; } = new();
}
