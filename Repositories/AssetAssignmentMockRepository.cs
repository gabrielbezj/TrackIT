using TrackIT.Models;

namespace TrackIT.Repositories;

public class AssetAssignmentMockRepository
{
    private readonly MockDataStore _dataStore;

    public AssetAssignmentMockRepository(MockDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public IReadOnlyList<AssetAssignment> GetAll() => _dataStore.AssetAssignments;

    public AssetAssignment? GetById(int id) => _dataStore.AssetAssignments.FirstOrDefault(x => x.Id == id);
}
