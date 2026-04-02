namespace Asp_projekt.Models;

public class Izvjestaj
{
    public int Id { get; set; }
    public Profesor Profesor { get; set; }
    public double ProsjecnaOcjena { get; set; }
    public int BrojOcjena { get; set; }
    public DateTime DatumGeneriranja { get; set; }

    public Izvjestaj(int id, Profesor profesor,DateTime datumGeneriranja)
    {
        Id = id;
        Profesor = profesor;
        DatumGeneriranja = datumGeneriranja;
        ProsjecnaOcjena = IzracunProsjecneOcjene(profesor);
        BrojOcjena = DohvatiBrojOcjena(profesor);
    }

    public double IzracunProsjecneOcjene(Profesor profesor)
    {
        if (profesor.Ocjene == null || profesor.Ocjene.Count == 0)
            return 0;

        return profesor.Ocjene.Average(o => o.Vrijednost);
    }

    public int DohvatiBrojOcjena(Profesor profesor)
    {
        if (profesor.Ocjene == null)
            return 0;

        return profesor.Ocjene.Count;
    }
}
