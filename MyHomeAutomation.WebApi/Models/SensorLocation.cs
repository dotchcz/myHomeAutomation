using System.ComponentModel.DataAnnotations;

namespace MyHomeAutomation.WebApi.Models;

public class SensorLocation
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int SensorId { get; set; }
    public virtual Sensor Sensor { get; set; }
    
    [Required]
    public int LocationId { get; set; }
    public virtual Location Location { get; set; }
    
    [Required]
    public DateTime ValidSince { get; set; }
    public DateTime? ValidTill { get; set; }
}