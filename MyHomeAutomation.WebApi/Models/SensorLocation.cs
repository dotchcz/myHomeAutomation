using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Models;

public class SensorLocation
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int SensorId { get; set; }
    public Sensor Sensor { get; set; }
    
    [Required]
    public int LocationId { get; set; }
    public Location Location { get; set; }
    
    [Required]
    public DateTime ValidSince { get; set; }
    public DateTime? ValidTill { get; set; }
}