using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourGuide.Web.Data;

namespace TourGuide.Web.Controllers
{
    public class CitiesController : Controller
    {
        private readonly AppDbContext _db;

        public CitiesController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var query = _db.Cities.Include(c => c.Attractions).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            var cities = await query.ToListAsync();
            return View(cities);
        }

        public async Task<IActionResult> Details(int id)
        {
            var city = await _db.Cities
                .Include(c => c.Attractions)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (city == null) return NotFound();

            return View(city);
        }
    }
}