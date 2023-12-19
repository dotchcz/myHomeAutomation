using System.ComponentModel.DataAnnotations;

namespace MyHomeAutomation.WebApi.Models;

public class Humidity
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public decimal Value { get; set; }

    [Required]
    public DateTime Created { get; set; }
    
    public int SensorId { get; set; }
    
    public virtual Sensor Sensor { get; set; }
}