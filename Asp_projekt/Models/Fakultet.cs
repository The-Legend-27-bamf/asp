using System.ComponentModel.DataAnnotations;

namespace Asp_projekt.Models;

public class Fakultet
{
    [Key]
    public int Id { get; set; }

    public string Naziv { get; set; }

    public virtual ICollection<Profesor> Profesori { get; set; }
    public virtual ICollection<Student> Studenti { get; set; }
    public virtual ICollection<Kolegij> Kolegiji { get; set; }

    protected Fakultet()
    {
        Naziv = string.Empty;
        Profesori = new List<Profesor>();
        Studenti = new List<Student>();
        Kolegiji = new List<Kolegij>();
    }

    public Fakultet(string naziv)
    {
        Naziv = naziv;
        Profesori = new List<Profesor>();
        Studenti = new List<Student>();
        Kolegiji = new List<Kolegij>();
    }
}
