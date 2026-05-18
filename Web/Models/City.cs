using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TourGuide.Web.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Region { get; set; }
        public int Population { get; set; }
        public string? History { get; set; }
        public string? CoatOfArmsUrl { get; set; }
        public string? PhotoUrl { get; set; }

        public List<Attraction> Attractions { get; set; } = new();
    }
}