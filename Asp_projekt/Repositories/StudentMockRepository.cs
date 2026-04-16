using Asp_projekt.Models;

namespace Asp_projekt.Repositories;

public class StudentMockRepository
{
    public List<Student> GetAll()
    {
        return MockDataStore.Students.ToList();
    }

    public Student? GetById(int id)
    {
        return MockDataStore.Students.FirstOrDefault(student => student.Id == id);
    }
}