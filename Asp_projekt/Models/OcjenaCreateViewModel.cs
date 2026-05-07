using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asp_projekt.Models;

public class OcjenaCreateViewModel
{
    [Required]
    public int ProfesorId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int KolegijId { get; set; }

    [Range(1, 5)]
    public int Vrijednost { get; set; } = 5;

    [Required]
    public TipOcjene Tip { get; set; } = TipOcjene.UkupniDojam;

    [Required]
    public string Komentar { get; set; } = string.Empty;

    public DateTime DatumOcjene { get; set; } = DateTime.Today;

    public List<SelectListItem> Profesori { get; set; } = new();
    public List<SelectListItem> Studenti { get; set; } = new();
    public List<SelectListItem> Kolegiji { get; set; } = new();
}
