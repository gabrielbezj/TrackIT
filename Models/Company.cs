namespace TrackIT.Models;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string HeadquartersCity { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public DateTime FoundedOn { get; set; }
    public List<Department> Departments { get; set; } = new();
    public List<Vendor> Vendors { get; set; } = new();
}
