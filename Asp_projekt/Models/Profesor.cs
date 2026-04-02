namespace Asp_projekt.Models;

public class Profesor
{
    public int Id { get; set; }
    public string Ime { get; set; }
    public string Prezime { get; set; }
    public string Katedra { get; set; }
    public List<Ocjena> Ocjene { get; set; }
    public List<Kolegij> Kolegiji { get; set; }

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
