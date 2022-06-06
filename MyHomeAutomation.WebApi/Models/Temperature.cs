using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Models;

public class Temperature
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public decimal Value { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public int SensorId { get; set; }
    public Sensor Sensor { get; set; }
}