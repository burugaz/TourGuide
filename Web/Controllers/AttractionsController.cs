using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourGuide.Web.Data;

namespace TourGuide.Web.Controllers
{
    public class AttractionsController : Controller
    {
        private readonly AppDbContext _db;

        public AttractionsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Details(int id)
        {
            var attraction = await _db.Attractions.Include(a => a.City)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attraction == null) return NotFound();

            return View(attraction);
        }
    }
}