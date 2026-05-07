using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp_projekt.Models;

public class Ocjena
{
    [Key]
    public int Id { get; set; }

    public int Vrijednost { get; set; }
    public string Komentar { get; set; }
    public DateTime DatumOcjene { get; set; }
    public TipOcjene Tip { get; set; }

    public int ProfesorId { get; set; }

    [ForeignKey(nameof(ProfesorId))]
    public virtual Profesor Profesor { get; set; }

    public int StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; }

    public int KolegijId { get; set; }

    [ForeignKey(nameof(KolegijId))]
    public virtual Kolegij Kolegij { get; set; }

    protected Ocjena()
    {
        Komentar = string.Empty;
        Profesor = null!;
        Student = null!;
        Kolegij = null!;
    }

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
        ProfesorId = profesor.Id;
        StudentId = student.Id;
        KolegijId = kolegij.Id;
    }
}
