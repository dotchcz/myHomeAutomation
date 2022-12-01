using System.ComponentModel.DataAnnotations;
using MinimalApi.Models;

namespace MyHomeAutomation.WebApi.Models;

public class Sensor
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    public virtual ICollection<Temperature> Temperatures { get; set; }
}