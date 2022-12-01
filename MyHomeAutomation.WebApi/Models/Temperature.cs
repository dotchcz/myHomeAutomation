using System.ComponentModel.DataAnnotations;
using MyHomeAutomation.WebApi.Models;

namespace MinimalApi.Models;

public class Temperature
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