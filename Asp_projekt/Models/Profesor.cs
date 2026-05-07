using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp_projekt.Models;

public class Profesor
{
    [Key]
    public int Id { get; set; }

    public string Ime { get; set; }
    public string Prezime { get; set; }
    public string Katedra { get; set; }

    public int? FakultetId { get; set; }

    [ForeignKey(nameof(FakultetId))]
    public virtual Fakultet? Fakultet { get; set; }

    public virtual ICollection<Ocjena> Ocjene { get; set; }
    public virtual ICollection<Kolegij> Kolegiji { get; set; }

    protected Profesor()
    {
        Ime = string.Empty;
        Prezime = string.Empty;
        Katedra = string.Empty;
        Ocjene = new List<Ocjena>();
        Kolegiji = new List<Kolegij>();
    }

    public Profesor(int id, string ime, string prezime, string katedra)
    {
        Id = id;
        Ime = ime;
        Prezime = prezime;
        Katedra = katedra;
        Ocjene = new List<Ocjena>();
        Kolegiji = new List<Kolegij>();
    }
}
