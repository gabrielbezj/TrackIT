using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrackIT.Models;
using TrackIT.Repositories;

namespace TrackIT.Controllers;

public class HomeController : Controller
{
    private readonly CompanyMockRepository _companyRepository;
    private readonly DepartmentMockRepository _departmentRepository;
    private readonly EmployeeMockRepository _employeeRepository;
    private readonly AssetMockRepository _assetRepository;
    private readonly AssetAssignmentMockRepository _assignmentRepository;
    private readonly SoftwareLicenseMockRepository _licenseRepository;
    private readonly VendorMockRepository _vendorRepository;

    public HomeController(
        CompanyMockRepository companyRepository,
        DepartmentMockRepository departmentRepository,
        EmployeeMockRepository employeeRepository,
        AssetMockRepository assetRepository,
        AssetAssignmentMockRepository assignmentRepository,
        SoftwareLicenseMockRepository licenseRepository,
        VendorMockRepository vendorRepository)
    {
        _companyRepository = companyRepository;
        _departmentRepository = departmentRepository;
        _employeeRepository = employeeRepository;
        _assetRepository = assetRepository;
        _assignmentRepository = assignmentRepository;
        _licenseRepository = licenseRepository;
        _vendorRepository = vendorRepository;
    }

    public IActionResult Index()
    {
        var now = DateTime.UtcNow;

        var assets = _assetRepository.GetAll();
        var assignments = _assignmentRepository.GetAll();
        var licenses = _licenseRepository.GetAll();

        var model = new HomeDashboardViewModel
        {
            CompanyCount = _companyRepository.GetAll().Count,
            DepartmentCount = _departmentRepository.GetAll().Count,
            EmployeeCount = _employeeRepository.GetAll().Count,
            AssetCount = assets.Count,
            VendorCount = _vendorRepository.GetAll().Count,
            InMaintenanceCount = assets.Count(x => x.Status == AssetStatus.InMaintenance),
            OverdueAssignmentsCount = assignments.Count(x => x.ReturnedOn is null && x.DueBackOn.HasValue && x.DueBackOn.Value < now),
            LicensesExpiringSoonCount = licenses.Count(x => x.ExpiresOn <= now.AddDays(30)),
            GeneratedAtUtc = now,
            Alerts = BuildAlerts(assets.Count(x => x.Status == AssetStatus.InMaintenance), assignments, licenses, now)
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private static IReadOnlyList<HomeDashboardAlertItem> BuildAlerts(
        int inMaintenanceCount,
        IReadOnlyList<AssetAssignment> assignments,
        IReadOnlyList<SoftwareLicense> licenses,
        DateTime now)
    {
        var overdueCount = assignments.Count(x => x.ReturnedOn is null && x.DueBackOn.HasValue && x.DueBackOn.Value < now);
        var expiringSoonCount = licenses.Count(x => x.ExpiresOn <= now.AddDays(30));

        return
        [
            new HomeDashboardAlertItem
            {
                Label = "Overdue assignments",
                Description = $"{overdueCount} assignment(s) are past due return date.",
                Severity = overdueCount > 0 ? "high" : "ok"
            },
            new HomeDashboardAlertItem
            {
                Label = "Licenses expiring in 30 days",
                Description = $"{expiringSoonCount} license(s) require renewal review soon.",
                Severity = expiringSoonCount > 0 ? "medium" : "ok"
            },
            new HomeDashboardAlertItem
            {
                Label = "Assets in maintenance",
                Description = $"{inMaintenanceCount} asset(s) are currently unavailable.",
                Severity = inMaintenanceCount > 0 ? "medium" : "ok"
            }
        ];
    }
}
