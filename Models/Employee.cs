namespace TrackIT.Models;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    public List<AssetAssignment> AssetAssignments { get; set; } = new();
}
