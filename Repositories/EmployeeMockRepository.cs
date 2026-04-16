using TrackIT.Models;

namespace TrackIT.Repositories;

public class EmployeeMockRepository
{
    private readonly MockDataStore _dataStore;

    public EmployeeMockRepository(MockDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public IReadOnlyList<Employee> GetAll() => _dataStore.Employees;

    public Employee? GetById(int id) => _dataStore.Employees.FirstOrDefault(x => x.Id == id);
}
