namespace TrackIT.Models;

public class AssetAssignment
{
    public int Id { get; set; }
    public DateTime AssignedOn { get; set; }
    public DateTime? DueBackOn { get; set; }
    public DateTime? ReturnedOn { get; set; }
    public string Notes { get; set; } = string.Empty;

    public int AssetId { get; set; }
    public Asset? Asset { get; set; }

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}
