using Asp_projekt.Models;

namespace Asp_projekt.Repositories;

public class FakultetMockRepository
{
    public List<Fakultet> GetAll()
    {
        return MockDataStore.Fakulteti.Select(entry => entry.Fakultet).ToList();
    }

    public Fakultet? GetById(int id)
    {
        return MockDataStore.Fakulteti
            .FirstOrDefault(entry => entry.Id == id)
            ?.Fakultet;
    }
}
