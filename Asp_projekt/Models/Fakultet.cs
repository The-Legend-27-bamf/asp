namespace Asp_projekt.Models;

public class Fakultet
{
    public string Naziv { get; set; }
    public List<Profesor> Profesori { get; set; }
    public List<Student> Studenti { get; set; }
    public List<Kolegij> Kolegiji { get; set; }

    public Fakultet(string naziv)
    {
        Naziv = naziv;
        Profesori = new List<Profesor>();
        Studenti = new List<Student>();
        Kolegiji = new List<Kolegij>();
    }
}
