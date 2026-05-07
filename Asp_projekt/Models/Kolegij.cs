using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp_projekt.Models;

public class Kolegij
{
    [Key]
    public int Id { get; set; }

    public string Naziv { get; set; }
    public int ECTS { get; set; }

    public int? FakultetId { get; set; }

    [ForeignKey(nameof(FakultetId))]
    public virtual Fakultet? Fakultet { get; set; }

    public virtual ICollection<Profesor> Profesori { get; set; }
    public virtual ICollection<Student> Studenti { get; set; }

    protected Kolegij()
    {
        Naziv = string.Empty;
        Profesori = new List<Profesor>();
        Studenti = new List<Student>();
    }

    public Kolegij(int id, string naziv, int ects)
    {
        Id = id;
        Naziv = naziv;
        ECTS = ects;
        Profesori = new List<Profesor>();
        Studenti = new List<Student>();
    }
}
