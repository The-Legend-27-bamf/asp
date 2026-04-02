namespace Asp_projekt.Models;

public class Kolegij
{
    public int Id { get; set; }
    public string Naziv { get; set; }
    public int ECTS { get; set; }
    public List<Profesor> Profesori { get; set; }
    public List<Student> Studenti { get; set; }

    public Kolegij(int id, string naziv, int ects)
    {
        Id = id;
        Naziv = naziv;
        ECTS = ects;
        Profesori = new List<Profesor>();
        Studenti = new List<Student>();
    }
}
