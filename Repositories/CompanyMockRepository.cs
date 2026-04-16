using TrackIT.Models;

namespace TrackIT.Repositories;

public class CompanyMockRepository
{
    private readonly MockDataStore _dataStore;

    public CompanyMockRepository(MockDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public IReadOnlyList<Company> GetAll() => _dataStore.Companies;

    public Company? GetById(int id) => _dataStore.Companies.FirstOrDefault(x => x.Id == id);
}
