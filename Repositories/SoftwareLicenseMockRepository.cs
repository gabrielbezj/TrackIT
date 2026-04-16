using TrackIT.Models;

namespace TrackIT.Repositories;

public class SoftwareLicenseMockRepository
{
    private readonly MockDataStore _dataStore;

    public SoftwareLicenseMockRepository(MockDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public IReadOnlyList<SoftwareLicense> GetAll() => _dataStore.SoftwareLicenses;

    public SoftwareLicense? GetById(int id) => _dataStore.SoftwareLicenses.FirstOrDefault(x => x.Id == id);
}
