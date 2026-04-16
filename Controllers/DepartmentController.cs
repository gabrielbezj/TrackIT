using Microsoft.AspNetCore.Mvc;
using TrackIT.Repositories;

namespace TrackIT.Controllers;

public class DepartmentController : Controller
{
    private readonly DepartmentMockRepository _repository;

    public DepartmentController(DepartmentMockRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var items = _repository.GetAll();
        return View(items);
    }

    public IActionResult Details(int id)
    {
        var item = _repository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        return View(item);
    }
}
