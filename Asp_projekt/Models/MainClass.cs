using Asp_projekt.Data;
using Microsoft.EntityFrameworkCore;

namespace Asp_projekt.Models;

public static class MainClass
{
    public static void SeedDatabase(AppDbContext db, int sets = 3)
    {
        if (sets <= 0)
        {
            return;
        }

        if (db.Profesori.AsNoTracking().Any()
            || db.Studenti.AsNoTracking().Any()
            || db.Kolegiji.AsNoTracking().Any()
            || db.Fakulteti.AsNoTracking().Any()
            || db.Ocjene.AsNoTracking().Any()
            || db.Izvjestaji.AsNoTracking().Any())
        {
            return;
        }

        var fakulteti = new List<Fakultet>();
        var profesori = new List<Profesor>();
        var studenti = new List<Student>();
        var kolegiji = new List<Kolegij>();

        var baseProfesori = new (string Ime, string Prezime, string Katedra)[]
        {
            ("Ivan", "Horvat", "Katedra za matematiku"),
            ("Marija", "Kovač", "Katedra za fiziku"),
            ("Tomislav", "Babić", "Katedra za informatiku")
        };

        var baseStudenti = new (string Ime, string Prezime, DateTime DatumUpisa)[]
        {
            ("Ana", "Novak", new DateTime(2022, 10, 1)),
            ("Luka", "Perić", new DateTime(2021, 10, 1)),
            ("Sara", "Jurić", new DateTime(2023, 10, 1))
        };

        var baseKolegiji = new (string Naziv, int Ects)[]
        {
            ("Matematika 1", 6),
            ("Fizika 1", 5),
            ("Algoritmi i strukture podataka", 7)
        };

        var baseFakulteti = new[]
        {
            "Fakultet prirodnih znanosti",
            "Tehnički fakultet",
            "Fakultet informatike"
        };

        for (var setIndex = 1; setIndex <= sets; setIndex++)
        {
            for (var i = 0; i < 3; i++)
            {
                var suffix = setIndex == 1 ? string.Empty : $" ({setIndex})";

                var fakultet = new Fakultet($"{baseFakulteti[i]}{suffix}");

                var profesorData = baseProfesori[i];
                var profesor = new Profesor(0, profesorData.Ime, profesorData.Prezime + suffix, profesorData.Katedra)
                {
                    Fakultet = fakultet
                };

                var studentData = baseStudenti[i];
                var student = new Student(0, studentData.Ime, studentData.Prezime + suffix, studentData.DatumUpisa.AddYears(setIndex - 1))
                {
                    Fakultet = fakultet
                };

                var kolegijData = baseKolegiji[i];
                var kolegij = new Kolegij(0, $"{kolegijData.Naziv}{suffix}", kolegijData.Ects)
                {
                    Fakultet = fakultet
                };

                fakultet.Profesori.Add(profesor);
                fakultet.Studenti.Add(student);
                fakultet.Kolegiji.Add(kolegij);

                kolegij.Profesori.Add(profesor);
                profesor.Kolegiji.Add(kolegij);

                student.Kolegij = kolegij;
                kolegij.Studenti.Add(student);

                fakulteti.Add(fakultet);
                profesori.Add(profesor);
                studenti.Add(student);
                kolegiji.Add(kolegij);
            }
        }

        // Save principal entities first, so identity IDs are generated.
        db.Fakulteti.AddRange(fakulteti);
        db.SaveChanges();

        var ocjene = new List<Ocjena>();

        for (var setIndex = 1; setIndex <= sets; setIndex++)
        {
            var offset = (setIndex - 1) * 3;
            var p1 = profesori[offset + 0];
            var p2 = profesori[offset + 1];
            var p3 = profesori[offset + 2];

            var s1 = studenti[offset + 0];
            var s2 = studenti[offset + 1];
            var s3 = studenti[offset + 2];

            var k1 = kolegiji[offset + 0];
            var k2 = kolegiji[offset + 1];
            var k3 = kolegiji[offset + 2];

            var baseDate = new DateTime(2024, 1, 15).AddMonths((setIndex - 1) * 2);

            var o1 = new Ocjena(0, 5, "Odličan rad", baseDate, TipOcjene.Predavanje, p1, s1, k1);
            var o2 = new Ocjena(0, 4, "Vrlo dobar", baseDate.AddDays(10), TipOcjene.Komunikacija, p2, s2, k2);
            var o3 = new Ocjena(0, 3, "Dobar", baseDate.AddDays(20), TipOcjene.UkupniDojam, p3, s3, k3);
            var o4 = new Ocjena(0, 5, "Izvrsna predavanja", baseDate.AddDays(30), TipOcjene.Organizacija, p1, s2, k1);
            var o5 = new Ocjena(0, 4, "Dobri materijali", baseDate.AddDays(40), TipOcjene.Materijali, p2, s3, k2);
            var o6 = new Ocjena(0, 2, "Potrebno poboljšanje", baseDate.AddDays(50), TipOcjene.UkupniDojam, p3, s1, k3);

            p1.Ocjene.Add(o1);
            p2.Ocjene.Add(o2);
            p3.Ocjene.Add(o3);
            p1.Ocjene.Add(o4);
            p2.Ocjene.Add(o5);
            p3.Ocjene.Add(o6);

            s1.DaneOcjene.Add(o1);
            s2.DaneOcjene.Add(o2);
            s3.DaneOcjene.Add(o3);
            s2.DaneOcjene.Add(o4);
            s3.DaneOcjene.Add(o5);
            s1.DaneOcjene.Add(o6);

            ocjene.AddRange([o1, o2, o3, o4, o5, o6]);
        }

        db.Ocjene.AddRange(ocjene);
        db.SaveChanges();

        var izvjestaji = profesori
            .Select(p => new Izvjestaj(0, p, DateTime.Today))
            .ToList();

        db.Izvjestaji.AddRange(izvjestaji);
        db.SaveChanges();
    }
}
