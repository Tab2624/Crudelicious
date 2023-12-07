using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Crudelicious.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Crudelicious.Controllers;

public class DishController : Controller
{
    private readonly ILogger<DishController> _logger;
    private MyContext _context;

    public DishController(ILogger<DishController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    // Dashboard
    [HttpGet("/")]
    public IActionResult Index()
    {
        List<Dish> DishesFromDb = _context.Dishes.OrderByDescending(d => d.CreatedAt).ToList();
        return View("Index", DishesFromDb);
    }

    // Render new dish form
    [HttpGet("dishes/new")]
    public IActionResult NewDish()
    {
        return View("NewDish");
    }

    // Add dish to dashboard
    [HttpPost("dishes/create")]
    public IActionResult CreateDish(Dish newDish)
    {
        if(!ModelState.IsValid)
        {
            return View("NewDish");
        }
        _context.Dishes.Add(newDish);
        //! SAVE CHANGES TO THE DB, OR IT WON'T BE PERMANENT!
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // View one
    [HttpGet("dishes/{dishId}")]
    public IActionResult ViewPost(int dishId)
    {
        Dish? OneDish = _context.Dishes.FirstOrDefault(d => d.DishID == dishId);
        if(OneDish == null)
        {
            return RedirectToAction("Index");
        }
        return View("ViewDish", OneDish);
    }

    // Delete by ID
    [HttpPost("dishes/{dishId}/delete")]
    public RedirectToActionResult DeleteDish(int dishId)
    {
        Dish? DishToDelete = _context.Dishes.SingleOrDefault(d => d.DishID == dishId);
        if(DishToDelete != null)
        {
            _context.Remove(DishToDelete);

            // Save Changes
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    // Edit a post
    [HttpGet("dishes/{dishId}/edit")]
    public IActionResult EditPost(int dishId)
    {
        Dish? OneDish = _context.Dishes.FirstOrDefault(e => e.DishID == dishId);
        if (OneDish == null)
        {
            return RedirectToAction("Index");
        }
        return View("EditDish", OneDish);
    }

    // Update the new changes
    [HttpPost("dishes/{dishId}/update")]
    public IActionResult UpdatePost(int dishId, Dish editedDish)
    {
        Dish? OldDish = _context.Dishes.FirstOrDefault(e => e.DishID == dishId);
        if (ModelState.IsValid)
        {
            OldDish.Chef = editedDish.Chef;
            OldDish.Name = editedDish.Name;
            OldDish.Description = editedDish.Description;
            OldDish.Calories = editedDish.Calories;
            OldDish.Tastiness = editedDish.Tastiness;

            OldDish.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View("EditDish", editedDish);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
