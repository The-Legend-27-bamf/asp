namespace Asp_projekt.Models;

public class Ocjena
{
    public int Id { get; set; }
    public int Vrijednost { get; set; }
    public string Komentar { get; set; }
    public DateTime DatumOcjene { get; set; }
    public TipOcjene Tip { get; set; }
    public Profesor Profesor { get; set; }
    public Student Student { get; set; }
    public Kolegij Kolegij { get; set; }

    public Ocjena(int id, int vrijednost, string komentar, DateTime datumOcjene, TipOcjene tip, Profesor profesor, Student student, Kolegij kolegij)
    {
        Id = id;
        Vrijednost = vrijednost;
        Komentar = komentar;
        DatumOcjene = datumOcjene;
        Tip = tip;
        Profesor = profesor;
        Student = student;
        Kolegij = kolegij;
    }
}
