using TrackIT.Models;

namespace TrackIT.Services;

public class Lab1Report
{
    public int CompanyCount { get; init; }
    public int DepartmentCount { get; init; }
    public int EmployeeCount { get; init; }
    public int AssetCount { get; init; }
    public IReadOnlyList<string> OverdueAssignments { get; init; } = [];
    public IReadOnlyList<string> ExpiringLicenses { get; init; } = [];
    public IReadOnlyList<string> TopDepartmentsByAssetValue { get; init; } = [];

    public IEnumerable<string> ToConsoleLines()
    {
        yield return "=== LAB 1 DEMO (TrackIT) ===";
        yield return $"Companies: {CompanyCount}";
        yield return $"Departments: {DepartmentCount}";
        yield return $"Employees: {EmployeeCount}";
        yield return $"Assets: {AssetCount}";
        yield return string.Empty;

        yield return "[LINQ] Overdue assignments:";
        foreach (var line in OverdueAssignments)
        {
            yield return $"- {line}";
        }

        yield return string.Empty;
        yield return "[LINQ] Licenses expiring in next 60 days:";
        foreach (var line in ExpiringLicenses)
        {
            yield return $"- {line}";
        }

        yield return string.Empty;
        yield return "[LINQ] Top 3 departments by asset value:";
        foreach (var line in TopDepartmentsByAssetValue)
        {
            yield return $"- {line}";
        }
    }
}

public static class Lab1DemoService
{
    public static async Task<Lab1Report> BuildReportAsync(CancellationToken cancellationToken = default)
    {
        var companies = SeedData();

        // Async-await primjer: simulacija sinkronizacije inventara bez blokiranja glavne dretve.
        await SimulateInventorySyncAsync(companies, cancellationToken);

        var now = DateTime.UtcNow;

        var overdueAssignments = companies
            .SelectMany(c => c.Departments)
            .SelectMany(d => d.Assets)
            .SelectMany(a => a.Assignments)
            .Where(a => a.ReturnedOn is null && a.DueBackOn.HasValue && a.DueBackOn.Value < now)
            .OrderBy(a => a.DueBackOn)
            .Select(a =>
                $"{a.Employee?.FirstName} {a.Employee?.LastName} -> {a.Asset?.InventoryTag} (due {a.DueBackOn:yyyy-MM-dd})")
            .ToList();

        var expiringLicenses = companies
            .SelectMany(c => c.Departments)
            .SelectMany(d => d.Assets)
            .SelectMany(a => a.SoftwareLicenses)
            .Where(l => l.ExpiresOn <= now.AddDays(60))
            .OrderBy(l => l.ExpiresOn)
            .Select(l =>
                $"{l.ProductName} on {l.Asset?.InventoryTag} expires {l.ExpiresOn:yyyy-MM-dd} (used {l.SeatsUsed}/{l.SeatsPurchased})")
            .ToList();

        var topDepartmentsByAssetValue = companies
            .SelectMany(c => c.Departments)
            .Select(d => new
            {
                DepartmentName = d.Name,
                CompanyName = d.Company?.Name ?? "N/A",
                TotalValue = d.Assets.Sum(a => a.PurchasePrice)
            })
            .OrderByDescending(x => x.TotalValue)
            .Take(3)
            .Select(x => $"{x.CompanyName} / {x.DepartmentName}: {x.TotalValue:C}")
            .ToList();

        return new Lab1Report
        {
            CompanyCount = companies.Count,
            DepartmentCount = companies.Sum(c => c.Departments.Count),
            EmployeeCount = companies.Sum(c => c.Departments.Sum(d => d.Employees.Count)),
            AssetCount = companies.Sum(c => c.Departments.Sum(d => d.Assets.Count)),
            OverdueAssignments = overdueAssignments,
            ExpiringLicenses = expiringLicenses,
            TopDepartmentsByAssetValue = topDepartmentsByAssetValue
        };
    }

    private static async Task SimulateInventorySyncAsync(List<Company> companies, CancellationToken cancellationToken)
    {
        await Task.Delay(200, cancellationToken);

        var now = DateTime.UtcNow;
        var expiredWarrantyAssets = companies
            .SelectMany(c => c.Departments)
            .SelectMany(d => d.Assets)
            .Where(a => a.WarrantyExpiresOn < now && a.Status == AssetStatus.InStock)
            .ToList();

        foreach (var asset in expiredWarrantyAssets)
        {
            asset.Status = AssetStatus.InMaintenance;
        }
    }

    private static List<Company> SeedData()
    {
        var now = DateTime.UtcNow;

        var companies = new List<Company>
        {
            new()
            {
                Id = 1,
                Name = "TrackIT Solutions",
                TaxId = "HR-90817263",
                HeadquartersCity = "Zagreb",
                ContactEmail = "info@trackit-solutions.hr",
                FoundedOn = new DateTime(2018, 3, 12)
            },
            new()
            {
                Id = 2,
                Name = "Adriatic Data Systems",
                TaxId = "HR-44018290",
                HeadquartersCity = "Split",
                ContactEmail = "office@adriatic-data.hr",
                FoundedOn = new DateTime(2016, 9, 5)
            },
            new()
            {
                Id = 3,
                Name = "Northwind IT Services",
                TaxId = "HR-55420188",
                HeadquartersCity = "Rijeka",
                ContactEmail = "contact@northwindit.hr",
                FoundedOn = new DateTime(2020, 1, 20)
            }
        };

        var departments = new List<Department>
        {
            new() { Id = 1, Name = "Engineering", CostCenterCode = "ENG-100", Budget = 300000m, ManagerName = "Ivana Horvat", CompanyId = 1, Company = companies[0] },
            new() { Id = 2, Name = "Support", CostCenterCode = "SUP-110", Budget = 120000m, ManagerName = "Luka Vidovic", CompanyId = 1, Company = companies[0] },
            new() { Id = 3, Name = "Infrastructure", CostCenterCode = "INF-200", Budget = 260000m, ManagerName = "Mia Kovacevic", CompanyId = 2, Company = companies[1] },
            new() { Id = 4, Name = "Sales", CostCenterCode = "SAL-210", Budget = 90000m, ManagerName = "Tomislav Jukic", CompanyId = 2, Company = companies[1] },
            new() { Id = 5, Name = "R&D", CostCenterCode = "RND-300", Budget = 340000m, ManagerName = "Petar Grgic", CompanyId = 3, Company = companies[2] },
            new() { Id = 6, Name = "Operations", CostCenterCode = "OPS-310", Budget = 150000m, ManagerName = "Klara Bencic", CompanyId = 3, Company = companies[2] }
        };

        foreach (var company in companies)
        {
            company.Departments = departments.Where(d => d.CompanyId == company.Id).ToList();
        }

        var vendors = new List<Vendor>
        {
            new() { Id = 1, Name = "TechSupply d.o.o.", SupportEmail = "support@techsupply.hr", Phone = "+385-1-555-100", Country = "Croatia", PreferredPaymentTermDays = 30, CompanyId = 1, Company = companies[0] },
            new() { Id = 2, Name = "Adria Hardware", SupportEmail = "help@adriahardware.hr", Phone = "+385-21-555-220", Country = "Croatia", PreferredPaymentTermDays = 45, CompanyId = 2, Company = companies[1] },
            new() { Id = 3, Name = "Nordic Devices", SupportEmail = "support@nordicdevices.eu", Phone = "+46-08-204-999", Country = "Sweden", PreferredPaymentTermDays = 60, CompanyId = 3, Company = companies[2] }
        };

        foreach (var company in companies)
        {
            company.Vendors = vendors.Where(v => v.CompanyId == company.Id).ToList();
        }

        var employees = new List<Employee>
        {
            new() { Id = 1, FirstName = "Ana", LastName = "Maric", Email = "ana.maric@trackit.hr", JobTitle = "Backend Developer", HireDate = new DateTime(2022, 2, 10), DepartmentId = 1 },
            new() { Id = 2, FirstName = "Marko", LastName = "Brajic", Email = "marko.brajic@trackit.hr", JobTitle = "QA Engineer", HireDate = new DateTime(2021, 11, 3), DepartmentId = 1 },
            new() { Id = 3, FirstName = "Nika", LastName = "Peric", Email = "nika.peric@trackit.hr", JobTitle = "Support Specialist", HireDate = new DateTime(2023, 4, 8), DepartmentId = 2 },
            new() { Id = 4, FirstName = "Ivan", LastName = "Zoric", Email = "ivan.zoric@adriatic.hr", JobTitle = "SysAdmin", HireDate = new DateTime(2020, 6, 15), DepartmentId = 3 },
            new() { Id = 5, FirstName = "Sara", LastName = "Lovric", Email = "sara.lovric@adriatic.hr", JobTitle = "Sales Engineer", HireDate = new DateTime(2019, 9, 1), DepartmentId = 4 },
            new() { Id = 6, FirstName = "Filip", LastName = "Mikic", Email = "filip.mikic@northwind.hr", JobTitle = "Research Engineer", HireDate = new DateTime(2024, 1, 12), DepartmentId = 5 },
            new() { Id = 7, FirstName = "Lea", LastName = "Novak", Email = "lea.novak@northwind.hr", JobTitle = "Ops Analyst", HireDate = new DateTime(2022, 8, 22), DepartmentId = 6 }
        };

        foreach (var dept in departments)
        {
            dept.Employees = employees.Where(e => e.DepartmentId == dept.Id).ToList();
            foreach (var employee in dept.Employees)
            {
                employee.Department = dept;
            }
        }

        var assets = new List<Asset>
        {
            new() { Id = 1, InventoryTag = "TRK-LAP-001", Name = "Dell Latitude 7440", Type = AssetType.Laptop, Status = AssetStatus.Assigned, PurchaseDate = now.AddMonths(-18), PurchasePrice = 1450m, WarrantyExpiresOn = now.AddMonths(18), DepartmentId = 1, VendorId = 1 },
            new() { Id = 2, InventoryTag = "TRK-SRV-002", Name = "HP ProLiant DL380", Type = AssetType.Server, Status = AssetStatus.InMaintenance, PurchaseDate = now.AddYears(-4), PurchasePrice = 6200m, WarrantyExpiresOn = now.AddMonths(-1), DepartmentId = 1, VendorId = 1 },
            new() { Id = 3, InventoryTag = "TRK-MOB-003", Name = "iPhone 15", Type = AssetType.MobileDevice, Status = AssetStatus.Assigned, PurchaseDate = now.AddMonths(-8), PurchasePrice = 1199m, WarrantyExpiresOn = now.AddMonths(16), DepartmentId = 2, VendorId = 1 },
            new() { Id = 4, InventoryTag = "ADS-SRV-011", Name = "Lenovo ThinkSystem SR650", Type = AssetType.Server, Status = AssetStatus.Assigned, PurchaseDate = now.AddYears(-2), PurchasePrice = 7100m, WarrantyExpiresOn = now.AddMonths(6), DepartmentId = 3, VendorId = 2 },
            new() { Id = 5, InventoryTag = "ADS-LAP-021", Name = "ThinkPad T14", Type = AssetType.Laptop, Status = AssetStatus.Assigned, PurchaseDate = now.AddMonths(-14), PurchasePrice = 1670m, WarrantyExpiresOn = now.AddMonths(10), DepartmentId = 4, VendorId = 2 },
            new() { Id = 6, InventoryTag = "NWD-DESK-101", Name = "Dell OptiPlex 7010", Type = AssetType.Desktop, Status = AssetStatus.InStock, PurchaseDate = now.AddYears(-3), PurchasePrice = 980m, WarrantyExpiresOn = now.AddMonths(-2), DepartmentId = 5, VendorId = 3 },
            new() { Id = 7, InventoryTag = "NWD-NET-111", Name = "Cisco Catalyst 9300", Type = AssetType.NetworkDevice, Status = AssetStatus.Assigned, PurchaseDate = now.AddYears(-1), PurchasePrice = 4900m, WarrantyExpiresOn = now.AddYears(2), DepartmentId = 6, VendorId = 3 }
        };

        foreach (var asset in assets)
        {
            var dept = departments.Single(d => d.Id == asset.DepartmentId);
            asset.Department = dept;
            dept.Assets.Add(asset);

            asset.Vendor = vendors.Single(v => v.Id == asset.VendorId);
            asset.Vendor.SuppliedAssets.Add(asset);
        }

        var assignments = new List<AssetAssignment>
        {
            new() { Id = 1, AssetId = 1, EmployeeId = 1, AssignedOn = now.AddDays(-80), DueBackOn = now.AddDays(40), Notes = "Primary dev machine" },
            new() { Id = 2, AssetId = 3, EmployeeId = 3, AssignedOn = now.AddDays(-120), DueBackOn = now.AddDays(-5), Notes = "On-call phone" },
            new() { Id = 3, AssetId = 4, EmployeeId = 4, AssignedOn = now.AddDays(-45), DueBackOn = now.AddDays(180), Notes = "Datacenter admin" },
            new() { Id = 4, AssetId = 5, EmployeeId = 5, AssignedOn = now.AddDays(-30), DueBackOn = now.AddDays(120), Notes = "Sales demo laptop" },
            new() { Id = 5, AssetId = 7, EmployeeId = 7, AssignedOn = now.AddDays(-200), DueBackOn = now.AddDays(-1), Notes = "Network operations" }
        };

        foreach (var assignment in assignments)
        {
            assignment.Asset = assets.Single(a => a.Id == assignment.AssetId);
            assignment.Employee = employees.Single(e => e.Id == assignment.EmployeeId);
            assignment.Asset.Assignments.Add(assignment);
            assignment.Employee.AssetAssignments.Add(assignment);
        }

        var maintenanceRecords = new List<MaintenanceRecord>
        {
            new() { Id = 1, AssetId = 2, PerformedOn = now.AddDays(-15), Description = "Power supply replacement", Cost = 380m, PerformedBy = "TechSupply d.o.o.", IsPreventive = false },
            new() { Id = 2, AssetId = 4, PerformedOn = now.AddDays(-45), Description = "Firmware update", Cost = 120m, PerformedBy = "Internal IT", IsPreventive = true },
            new() { Id = 3, AssetId = 7, PerformedOn = now.AddDays(-20), Description = "Port diagnostics", Cost = 70m, PerformedBy = "Nordic Devices", IsPreventive = true }
        };

        foreach (var record in maintenanceRecords)
        {
            record.Asset = assets.Single(a => a.Id == record.AssetId);
            record.Asset.MaintenanceRecords.Add(record);
        }

        var licenses = new List<SoftwareLicense>
        {
            new() { Id = 1, AssetId = 1, ProductName = "Visual Studio Enterprise", LicenseType = LicenseType.Subscription, SeatsPurchased = 1, SeatsUsed = 1, ExpiresOn = now.AddDays(20), LicenseKey = "VSENT-1001", IsAutoRenewal = true },
            new() { Id = 2, AssetId = 3, ProductName = "Mobile Security Suite", LicenseType = LicenseType.Subscription, SeatsPurchased = 1, SeatsUsed = 1, ExpiresOn = now.AddDays(55), LicenseKey = "MSS-2099", IsAutoRenewal = false },
            new() { Id = 3, AssetId = 4, ProductName = "Windows Server Datacenter", LicenseType = LicenseType.Perpetual, SeatsPurchased = 16, SeatsUsed = 16, ExpiresOn = now.AddYears(5), LicenseKey = "WIN-DC-7788", IsAutoRenewal = false },
            new() { Id = 4, AssetId = 5, ProductName = "Office 365", LicenseType = LicenseType.Subscription, SeatsPurchased = 1, SeatsUsed = 1, ExpiresOn = now.AddDays(10), LicenseKey = "O365-3344", IsAutoRenewal = true },
            new() { Id = 5, AssetId = 7, ProductName = "Network Monitor Pro", LicenseType = LicenseType.Subscription, SeatsPurchased = 5, SeatsUsed = 3, ExpiresOn = now.AddDays(180), LicenseKey = "NMP-8821", IsAutoRenewal = true }
        };

        foreach (var license in licenses)
        {
            license.Asset = assets.Single(a => a.Id == license.AssetId);
            license.Asset.SoftwareLicenses.Add(license);
        }

        return companies;
    }
}