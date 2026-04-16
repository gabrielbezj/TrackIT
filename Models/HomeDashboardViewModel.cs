namespace TrackIT.Models;

public class HomeDashboardViewModel
{
    public int CompanyCount { get; init; }
    public int DepartmentCount { get; init; }
    public int EmployeeCount { get; init; }
    public int AssetCount { get; init; }
    public int VendorCount { get; init; }
    public int InMaintenanceCount { get; init; }
    public int OverdueAssignmentsCount { get; init; }
    public int LicensesExpiringSoonCount { get; init; }
    public DateTime GeneratedAtUtc { get; init; }

    public IReadOnlyList<HomeDashboardAlertItem> Alerts { get; init; } = [];
}

public class HomeDashboardAlertItem
{
    public string Label { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Severity { get; init; } = "info";
}
