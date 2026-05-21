using System.ComponentModel.DataAnnotations;

namespace Asp_projekt.Models;

public class OcjenaEditViewModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int ProfesorId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int KolegijId { get; set; }

    [Range(1, 5)]
    public int Vrijednost { get; set; }

    [Required]
    public TipOcjene Tip { get; set; }

    [Required]
    [StringLength(400)]
    public string Komentar { get; set; } = string.Empty;

    [Required]
    public DateTime DatumOcjene { get; set; }

    public List<OcjenaCreateViewModel.ProfesorOption> Profesori { get; set; } = new();
    public List<OcjenaCreateViewModel.StudentOption> Studenti { get; set; } = new();
    public List<OcjenaCreateViewModel.KolegijOption> Kolegiji { get; set; } = new();
}
