using Asp_projekt.Models;

namespace Asp_projekt.Repositories;

public class IzvjestajMockRepository
{
    public List<Izvjestaj> GetAll()
    {
        return MockDataStore.Izvjestaji.ToList();
    }

    public Izvjestaj? GetById(int id)
    {
        return MockDataStore.Izvjestaji.FirstOrDefault(izvjestaj => izvjestaj.Id == id);
    }
}
