using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class PointOfInterestForUpdateDto
    {
        [Required(ErrorMessage = " You should provide a name value")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(100, ErrorMessage = "Maximum length is 100")]
        public string? Description { get; set; }
    }
}
