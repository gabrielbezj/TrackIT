using TrackIT.Models;
using TrackIT.Services;

namespace TrackIT.Repositories;

public class MockDataStore
{
    public MockDataStore()
    {
        Companies = Lab1DemoService.CreateSeedData();
        Departments = Companies.SelectMany(c => c.Departments).OrderBy(d => d.Id).ToList();
        Employees = Departments.SelectMany(d => d.Employees).OrderBy(e => e.Id).ToList();
        Vendors = Companies.SelectMany(c => c.Vendors).OrderBy(v => v.Id).ToList();
        Assets = Departments.SelectMany(d => d.Assets).OrderBy(a => a.Id).ToList();
        AssetAssignments = Assets.SelectMany(a => a.Assignments).OrderBy(x => x.Id).ToList();
        MaintenanceRecords = Assets.SelectMany(a => a.MaintenanceRecords).OrderBy(x => x.Id).ToList();
        SoftwareLicenses = Assets.SelectMany(a => a.SoftwareLicenses).OrderBy(x => x.Id).ToList();
    }

    public IReadOnlyList<Company> Companies { get; }
    public IReadOnlyList<Department> Departments { get; }
    public IReadOnlyList<Employee> Employees { get; }
    public IReadOnlyList<Vendor> Vendors { get; }
    public IReadOnlyList<Asset> Assets { get; }
    public IReadOnlyList<AssetAssignment> AssetAssignments { get; }
    public IReadOnlyList<MaintenanceRecord> MaintenanceRecords { get; }
    public IReadOnlyList<SoftwareLicense> SoftwareLicenses { get; }
}
