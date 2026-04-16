using Asp_projekt.Models;

namespace Asp_projekt.Repositories;

internal static class MockDataStore
{
    private static readonly SeedData Data = CreateSeedData();

    public static List<Student> Students => Data.Students;
    public static List<Kolegij> Kolegiji => Data.Kolegiji;
    public static List<Profesor> Profesori => Data.Profesori;
    public static List<Ocjena> Ocjene => Data.Ocjene;
    public static List<Izvjestaj> Izvjestaji => Data.Izvjestaji;
    public static List<FakultetEntry> Fakulteti => Data.Fakulteti;

    private static SeedData CreateSeedData()
    {
        var profesor1 = new Profesor(1, "Ivan", "Horvat", "Katedra za matematiku");
        var profesor2 = new Profesor(2, "Marija", "Kovač", "Katedra za fiziku");
        var profesor3 = new Profesor(3, "Tomislav", "Babić", "Katedra za informatiku");

        var student1 = new Student(1, "Ana", "Novak", new DateTime(2022, 10, 1));
        var student2 = new Student(2, "Luka", "Perić", new DateTime(2021, 10, 1));
        var student3 = new Student(3, "Sara", "Jurić", new DateTime(2023, 10, 1));

        var kolegij1 = new Kolegij(1, "Matematika 1", 6);
        kolegij1.Profesori.Add(profesor1);
        kolegij1.Studenti.Add(student1);

        var kolegij2 = new Kolegij(2, "Fizika 1", 5);
        kolegij2.Profesori.Add(profesor2);
        kolegij2.Studenti.Add(student2);

        var kolegij3 = new Kolegij(3, "Algoritmi i strukture podataka", 7);
        kolegij3.Profesori.Add(profesor3);
        kolegij3.Studenti.Add(student3);

        var ocjena1 = new Ocjena(1, 5, "Odličan rad", new DateTime(2024, 1, 15), TipOcjene.Predavanje, profesor1, student1, kolegij1);
        var ocjena2 = new Ocjena(2, 4, "Vrlo dobar", new DateTime(2024, 2, 20), TipOcjene.Komunikacija, profesor2, student2, kolegij2);
        var ocjena3 = new Ocjena(3, 3, "Dobar", new DateTime(2024, 3, 10), TipOcjene.UkupniDojam, profesor3, student3, kolegij3);
        var ocjena4 = new Ocjena(4, 5, "Izvrsna predavanja", new DateTime(2024, 4, 5), TipOcjene.Organizacija, profesor1, student2, kolegij1);
        var ocjena5 = new Ocjena(5, 4, "Dobri materijali", new DateTime(2024, 5, 12), TipOcjene.Materijali, profesor2, student3, kolegij2);
        var ocjena6 = new Ocjena(6, 2, "Potrebno poboljšanje", new DateTime(2024, 6, 18), TipOcjene.UkupniDojam, profesor3, student1, kolegij3);

        profesor1.Ocjene.Add(ocjena1);
        profesor1.Ocjene.Add(ocjena4);
        profesor2.Ocjene.Add(ocjena2);
        profesor2.Ocjene.Add(ocjena5);
        profesor3.Ocjene.Add(ocjena3);
        profesor3.Ocjene.Add(ocjena6);

        student1.DaneOcjene.Add(ocjena1);
        student1.DaneOcjene.Add(ocjena6);
        student2.DaneOcjene.Add(ocjena2);
        student2.DaneOcjene.Add(ocjena4);
        student3.DaneOcjene.Add(ocjena3);
        student3.DaneOcjene.Add(ocjena5);

        profesor1.Kolegiji.Add(kolegij1);
        profesor2.Kolegiji.Add(kolegij2);
        profesor3.Kolegiji.Add(kolegij3);

        var fakultet1 = new Fakultet("Fakultet prirodnih znanosti");
        fakultet1.Profesori.Add(profesor1);
        fakultet1.Studenti.Add(student1);
        fakultet1.Kolegiji.Add(kolegij1);

        var fakultet2 = new Fakultet("Tehnički fakultet");
        fakultet2.Profesori.Add(profesor2);
        fakultet2.Studenti.Add(student2);
        fakultet2.Kolegiji.Add(kolegij2);

        var fakultet3 = new Fakultet("Fakultet informatike");
        fakultet3.Profesori.Add(profesor3);
        fakultet3.Studenti.Add(student3);
        fakultet3.Kolegiji.Add(kolegij3);

        var izvjestaj1 = new Izvjestaj(1, profesor1, DateTime.Now);
        var izvjestaj2 = new Izvjestaj(2, profesor2, DateTime.Now);
        var izvjestaj3 = new Izvjestaj(3, profesor3, DateTime.Now);

        return new SeedData
        {
            Students = [student1, student2, student3],
            Kolegiji = [kolegij1, kolegij2, kolegij3],
            Profesori = [profesor1, profesor2, profesor3],
            Ocjene = [ocjena1, ocjena2, ocjena3, ocjena4, ocjena5, ocjena6],
            Izvjestaji = [izvjestaj1, izvjestaj2, izvjestaj3],
            Fakulteti =
            [
                new FakultetEntry(1, fakultet1),
                new FakultetEntry(2, fakultet2),
                new FakultetEntry(3, fakultet3)
            ]
        };
    }

    public sealed record FakultetEntry(int Id, Fakultet Fakultet);

    private sealed class SeedData
    {
        public required List<Student> Students { get; init; }
        public required List<Kolegij> Kolegiji { get; init; }
        public required List<Profesor> Profesori { get; init; }
        public required List<Ocjena> Ocjene { get; init; }
        public required List<Izvjestaj> Izvjestaji { get; init; }
        public required List<FakultetEntry> Fakulteti { get; init; }
    }
}