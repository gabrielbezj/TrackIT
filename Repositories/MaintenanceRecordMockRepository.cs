using TrackIT.Models;

namespace TrackIT.Repositories;

public class MaintenanceRecordMockRepository
{
    private readonly MockDataStore _dataStore;

    public MaintenanceRecordMockRepository(MockDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public IReadOnlyList<MaintenanceRecord> GetAll() => _dataStore.MaintenanceRecords;

    public MaintenanceRecord? GetById(int id) => _dataStore.MaintenanceRecords.FirstOrDefault(x => x.Id == id);
}
