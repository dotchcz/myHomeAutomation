using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Models;

public class Sensor
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
}