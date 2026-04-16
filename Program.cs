using TrackIT.Repositories;
using TrackIT.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<MockDataStore>();
builder.Services.AddSingleton<CompanyMockRepository>();
builder.Services.AddSingleton<DepartmentMockRepository>();
builder.Services.AddSingleton<EmployeeMockRepository>();
builder.Services.AddSingleton<AssetMockRepository>();
builder.Services.AddSingleton<AssetAssignmentMockRepository>();
builder.Services.AddSingleton<MaintenanceRecordMockRepository>();
builder.Services.AddSingleton<SoftwareLicenseMockRepository>();
builder.Services.AddSingleton<VendorMockRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var report = await Lab1DemoService.BuildReportAsync();
    foreach (var line in report.ToConsoleLines())
    {
        Console.WriteLine(line);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
