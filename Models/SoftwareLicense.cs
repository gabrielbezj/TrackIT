namespace TrackIT.Models;

public class SoftwareLicense
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public LicenseType LicenseType { get; set; }
    public int SeatsPurchased { get; set; }
    public int SeatsUsed { get; set; }
    public DateTime ExpiresOn { get; set; }
    public string LicenseKey { get; set; } = string.Empty;
    public bool IsAutoRenewal { get; set; }

    public int AssetId { get; set; }
    public Asset? Asset { get; set; }
}