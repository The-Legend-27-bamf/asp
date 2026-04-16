using Asp_projekt.Models;

namespace Asp_projekt.Repositories;

public class KolegijMockRepository
{
    public List<Kolegij> GetAll()
    {
        return MockDataStore.Kolegiji.ToList();
    }

    public Kolegij? GetById(int id)
    {
        return MockDataStore.Kolegiji.FirstOrDefault(kolegij => kolegij.Id == id);
    }
}