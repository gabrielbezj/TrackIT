using TrackIT.Models;

namespace TrackIT.Repositories;

public class AssetMockRepository
{
    private readonly MockDataStore _dataStore;

    public AssetMockRepository(MockDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public IReadOnlyList<Asset> GetAll() => _dataStore.Assets;

    public Asset? GetById(int id) => _dataStore.Assets.FirstOrDefault(x => x.Id == id);
}
