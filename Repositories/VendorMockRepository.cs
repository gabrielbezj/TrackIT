using TrackIT.Models;

namespace TrackIT.Repositories;

public class VendorMockRepository
{
    private readonly MockDataStore _dataStore;

    public VendorMockRepository(MockDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public IReadOnlyList<Vendor> GetAll() => _dataStore.Vendors;

    public Vendor? GetById(int id) => _dataStore.Vendors.FirstOrDefault(x => x.Id == id);
}
