namespace TrackIT.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CostCenterCode { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public string ManagerName { get; set; } = string.Empty;

    public int CompanyId { get; set; }
    public Company? Company { get; set; }

    public List<Employee> Employees { get; set; } = new();
    public List<Asset> Assets { get; set; } = new();
}
