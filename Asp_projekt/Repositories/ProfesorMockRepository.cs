using Asp_projekt.Models;

namespace Asp_projekt.Repositories;

public class ProfesorMockRepository
{
    public List<Profesor> GetAll()
    {
        return MockDataStore.Profesori.ToList();
    }

    public Profesor? GetById(int id)
    {
        return MockDataStore.Profesori.FirstOrDefault(profesor => profesor.Id == id);
    }
}
