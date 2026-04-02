namespace Asp_projekt.Models;

public class Student
{
    public int Id { get; set; }
    public string Ime { get; set; }
    public string Prezime { get; set; }
    public DateTime DatumUpisa { get; set; }
    public List<Ocjena> DaneOcjene { get; set; }

    public Student(int id, string ime, string prezime, DateTime datumUpisa)
    {
        Id = id;
        Ime = ime;
        Prezime = prezime;
        DatumUpisa = datumUpisa;
        DaneOcjene = new List<Ocjena>();
    }
}
