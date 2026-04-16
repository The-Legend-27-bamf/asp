using Asp_projekt.Models;

namespace Asp_projekt.Repositories;

public class OcjenaMockRepository
{
    public List<Ocjena> GetAll()
    {
        return MockDataStore.Ocjene.ToList();
    }

    public Ocjena? GetById(int id)
    {
        return MockDataStore.Ocjene.FirstOrDefault(ocjena => ocjena.Id == id);
    }
}
