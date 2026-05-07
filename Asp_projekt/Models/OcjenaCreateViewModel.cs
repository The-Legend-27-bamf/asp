using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asp_projekt.Models;

public class OcjenaCreateViewModel
{
    public sealed class ProfesorOption
    {
        public int Id { get; init; }
        public string Naziv { get; init; } = string.Empty;
        public string Katedra { get; init; } = string.Empty;
        public string? FakultetNaziv { get; init; }
    }

    public sealed class StudentOption
    {
        public int Id { get; init; }
        public string Naziv { get; init; } = string.Empty;
        public DateTime DatumUpisa { get; init; }
        public string? FakultetNaziv { get; init; }
    }

    public sealed class KolegijOption
    {
        public int Id { get; init; }
        public string Naziv { get; init; } = string.Empty;
        public int ECTS { get; init; }
        public string? FakultetNaziv { get; init; }
    }

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

    public List<ProfesorOption> Profesori { get; set; } = new();
    public List<StudentOption> Studenti { get; set; } = new();
    public List<KolegijOption> Kolegiji { get; set; } = new();
}
