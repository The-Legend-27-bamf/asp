namespace Asp_projekt.Models;

public class Administrator
{
    public string Username { get; set; }
    public string Lozinka { get; set; }

    public Administrator(string username, string lozinka)
    {
        Username = username;
        Lozinka = lozinka;
    }

    public Profesor DodajProfesora(int id, string ime, string prezime, string katedra)
    {
        return new Profesor(id, ime, prezime, katedra);
    }

    public Kolegij DodajKolegij(int id, string naziv, int ects)
    {
        return new Kolegij(id, naziv, ects);
    }

    public Izvjestaj GenerirajIzvjestaj(int id, Profesor profesor, DateTime datumGeneriranja)
    {
        return new Izvjestaj(id, profesor, datumGeneriranja);
    }
}
