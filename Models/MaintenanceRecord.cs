namespace TrackIT.Models;

public class MaintenanceRecord
{
    public int Id { get; set; }
    public DateTime PerformedOn { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public string PerformedBy { get; set; } = string.Empty;
    public bool IsPreventive { get; set; }

    public int AssetId { get; set; }
    public Asset? Asset { get; set; }
}
