namespace TrackIT.Models;

public class Vendor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SupportEmail { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int PreferredPaymentTermDays { get; set; }

    public int CompanyId { get; set; }
    public Company? Company { get; set; }

    public List<Asset> SuppliedAssets { get; set; } = new();
}
