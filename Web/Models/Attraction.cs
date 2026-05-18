using System.ComponentModel.DataAnnotations;

namespace TourGuide.Web.Models
{
    public class Attraction
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
        public string? History { get; set; }
        public string? PhotoUrl { get; set; }
        public string? OpeningHours { get; set; }
        public string? Price { get; set; }

        public int CityId { get; set; }
        public City? City { get; set; }
    }
}