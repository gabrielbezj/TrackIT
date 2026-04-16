using TrackIT.Models;

namespace TrackIT.Repositories;

public class DepartmentMockRepository
{
    private readonly MockDataStore _dataStore;

    public DepartmentMockRepository(MockDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public IReadOnlyList<Department> GetAll() => _dataStore.Departments;

    public Department? GetById(int id) => _dataStore.Departments.FirstOrDefault(x => x.Id == id);
}
