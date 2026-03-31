namespace TrackIT.Models;

public class Asset
{
    public int Id { get; set; }
    public string InventoryTag { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public AssetType Type { get; set; }
    public AssetStatus Status { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
    public DateTime WarrantyExpiresOn { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    public int? VendorId { get; set; }
    public Vendor? Vendor { get; set; }

    public List<AssetAssignment> Assignments { get; set; } = new();
    public List<MaintenanceRecord> MaintenanceRecords { get; set; } = new();
    public List<SoftwareLicense> SoftwareLicenses { get; set; } = new();
}
