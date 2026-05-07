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

        static string UniquePair(string[] left, string[] right, int index, string separator)
        {
            if (left.Length == 0 || right.Length == 0)
            {
                return $"Item{index + 1}";
            }

            var leftIndex = index % left.Length;
            var rightIndex = (index / left.Length) % right.Length;
            return $"{left[leftIndex]}{separator}{right[rightIndex]}";
        }

        var professorFirstNames = new[]
        {
            "Ivan", "Marija", "Tomislav", "Petra", "Dario", "Maja", "Karlo", "Lucija", "Nikola", "Ivana"
        };

        var professorLastNames = new[]
        {
            "Horvat", "Kovač", "Babić", "Radić", "Vuković", "Pavić", "Božić", "Knežević", "Perić", "Novak", "Jurić"
        };

        var studentFirstNames = new[]
        {
            "Ana", "Luka", "Sara", "Marko", "Iva", "Filip", "Ema", "Petar", "Nina", "Matej", "Lea", "Karlo"
        };

        var studentLastNames = new[]
        {
            "Novak", "Perić", "Jurić", "Kovačević", "Marić", "Barišić", "Šarić", "Knežević", "Radić", "Božić", "Vuković"
        };

        var katedraNames = new[]
        {
            "Katedra za matematiku",
            "Katedra za fiziku",
            "Katedra za informatiku",
            "Katedra za elektrotehniku",
            "Katedra za statistiku",
            "Katedra za programiranje",
            "Katedra za baze podataka",
            "Katedra za računalne mreže"
        };

        var facultyTypes = new[]
        {
            "Fakultet", "Tehnički fakultet", "Akademija", "Visoka škola"
        };

        var facultyFields = new[]
        {
            "prirodnih znanosti", "informatike", "elektrotehnike", "strojarstva", "matematike", "fizike"
        };

        var coursePrefixes = new[]
        {
            "Uvod u", "Osnove", "Napredni", "Primijenjena", "Metode"
        };

        var courseTopics = new[]
        {
            "matematiku", "fiziku", "algoritme", "baze podataka", "programiranje", "računalne mreže", "statistiku"
        };

        var ectsValues = new[] { 6, 5, 7 };

        static void LinkProfesorToKolegij(Profesor profesor, Kolegij kolegij)
        {
            if (!kolegij.Profesori.Contains(profesor))
            {
                kolegij.Profesori.Add(profesor);
            }

            if (!profesor.Kolegiji.Contains(kolegij))
            {
                profesor.Kolegiji.Add(kolegij);
            }
        }

        for (var setIndex = 1; setIndex <= sets; setIndex++)
        {
            // One fakultet per set, with richer relationships:
            // - 3 professors
            // - 3 courses
            // - 12 students (4 per course)
            var setSeedIndex = setIndex - 1;

            var fakultetName = UniquePair(facultyTypes, facultyFields, setSeedIndex, " ");
            var fakultet = new Fakultet(fakultetName);

            var setProfesori = new List<Profesor>();
            var setStudenti = new List<Student>();
            var setKolegiji = new List<Kolegij>();

            for (var i = 0; i < 3; i++)
            {
                var globalIndex = setSeedIndex * 3 + i;

                var profesorIme = professorFirstNames[globalIndex % professorFirstNames.Length];
                var profesorPrezime = professorLastNames[(globalIndex * 7) % professorLastNames.Length];
                var profesorKatedra = katedraNames[globalIndex % katedraNames.Length];
                var profesor = new Profesor(0, profesorIme, profesorPrezime, profesorKatedra)
                {
                    Fakultet = fakultet
                };

                var kolegijNaziv = UniquePair(coursePrefixes, courseTopics, globalIndex, " ");
                var kolegijEcts = ectsValues[globalIndex % ectsValues.Length];
                var kolegij = new Kolegij(0, kolegijNaziv, kolegijEcts)
                {
                    Fakultet = fakultet
                };

                fakultet.Profesori.Add(profesor);
                fakultet.Kolegiji.Add(kolegij);

                setProfesori.Add(profesor);
                setKolegiji.Add(kolegij);
            }

            for (var i = 0; i < 12; i++)
            {
                var globalIndex = setSeedIndex * 12 + i;
                var studentIme = studentFirstNames[globalIndex % studentFirstNames.Length];
                var studentPrezime = studentLastNames[(globalIndex * 5 + 1) % studentLastNames.Length];
                var studentDatumUpisa = new DateTime(2021, 10, 1).AddYears(globalIndex % 4);
                var student = new Student(0, studentIme, studentPrezime, studentDatumUpisa)
                {
                    Fakultet = fakultet
                };

                fakultet.Studenti.Add(student);
                setStudenti.Add(student);
            }

            // Connect courses to professors (2 per course).
            for (var courseIndex = 0; courseIndex < setKolegiji.Count; courseIndex++)
            {
                var kolegij = setKolegiji[courseIndex];
                var profesorA = setProfesori[courseIndex % setProfesori.Count];
                var profesorB = setProfesori[(courseIndex + 1) % setProfesori.Count];

                LinkProfesorToKolegij(profesorA, kolegij);
                LinkProfesorToKolegij(profesorB, kolegij);
            }

            // Connect courses to students (4 per course).
            for (var courseIndex = 0; courseIndex < setKolegiji.Count; courseIndex++)
            {
                var kolegij = setKolegiji[courseIndex];

                for (var s = 0; s < 4; s++)
                {
                    var student = setStudenti[courseIndex * 4 + s];
                    student.Kolegij = kolegij;
                    if (!kolegij.Studenti.Contains(student))
                    {
                        kolegij.Studenti.Add(student);
                    }
                }
            }

            fakulteti.Add(fakultet);
            profesori.AddRange(setProfesori);
            studenti.AddRange(setStudenti);
            kolegiji.AddRange(setKolegiji);
        }

        // Save principal entities first, so identity IDs are generated.
        db.Fakulteti.AddRange(fakulteti);
        db.SaveChanges();

        var ocjene = new List<Ocjena>();

        for (var setIndex = 1; setIndex <= sets; setIndex++)
        {
            var profesorOffset = (setIndex - 1) * 3;
            var studentOffset = (setIndex - 1) * 12;
            var kolegijOffset = (setIndex - 1) * 3;

            var s1 = studenti[studentOffset + 0];
            var s2 = studenti[studentOffset + 1];
            var s3 = studenti[studentOffset + 2];

            var k1 = kolegiji[kolegijOffset + 0];
            var k2 = kolegiji[kolegijOffset + 1];
            var k3 = kolegiji[kolegijOffset + 2];

            var baseDate = new DateTime(2024, 1, 15).AddMonths((setIndex - 1) * 2);

            var k1p1 = k1.Profesori.First();
            var k1p2 = k1.Profesori.Skip(1).FirstOrDefault() ?? k1p1;
            var k2p1 = k2.Profesori.First();
            var k2p2 = k2.Profesori.Skip(1).FirstOrDefault() ?? k2p1;
            var k3p1 = k3.Profesori.First();
            var k3p2 = k3.Profesori.Skip(1).FirstOrDefault() ?? k3p1;

            var o1 = new Ocjena(0, 5, "Odličan rad", baseDate, TipOcjene.Predavanje, k1p1, s1, k1);
            var o2 = new Ocjena(0, 4, "Vrlo dobar", baseDate.AddDays(10), TipOcjene.Komunikacija, k2p1, s2, k2);
            var o3 = new Ocjena(0, 3, "Dobar", baseDate.AddDays(20), TipOcjene.UkupniDojam, k3p1, s3, k3);
            var o4 = new Ocjena(0, 5, "Izvrsna predavanja", baseDate.AddDays(30), TipOcjene.Organizacija, k1p2, s2, k1);
            var o5 = new Ocjena(0, 4, "Dobri materijali", baseDate.AddDays(40), TipOcjene.Materijali, k2p2, s3, k2);
            var o6 = new Ocjena(0, 2, "Potrebno poboljšanje", baseDate.AddDays(50), TipOcjene.UkupniDojam, k3p2, s1, k3);

            k1p1.Ocjene.Add(o1);
            k2p1.Ocjene.Add(o2);
            k3p1.Ocjene.Add(o3);
            k1p2.Ocjene.Add(o4);
            k2p2.Ocjene.Add(o5);
            k3p2.Ocjene.Add(o6);

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
