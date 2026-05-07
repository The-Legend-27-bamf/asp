using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp_projekt.Models;

public class Student
{
    [Key]
    public int Id { get; set; }

    public string Ime { get; set; }
    public string Prezime { get; set; }
    public DateTime DatumUpisa { get; set; }

    public int? FakultetId { get; set; }

    [ForeignKey(nameof(FakultetId))]
    public virtual Fakultet? Fakultet { get; set; }

    public int? KolegijId { get; set; }

    [ForeignKey(nameof(KolegijId))]
    public virtual Kolegij? Kolegij { get; set; }

    public virtual ICollection<Ocjena> DaneOcjene { get; set; }

    protected Student()
    {
        Ime = string.Empty;
        Prezime = string.Empty;
        DaneOcjene = new List<Ocjena>();
    }

    public Student(int id, string ime, string prezime, DateTime datumUpisa)
    {
        Id = id;
        Ime = ime;
        Prezime = prezime;
        DatumUpisa = datumUpisa;
        DaneOcjene = new List<Ocjena>();
    }
}
