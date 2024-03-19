using System.ComponentModel.DataAnnotations;
using MyHomeAutomation.WebApi.Enums;

namespace MyHomeAutomation.WebApi.Models;

public class Relay
{
    [Required]
    [Key]
    public int Id { get; set; }
    public bool Active { get; set; }

    public string Name { get; set; }
    
    public  DateTime LastUpdate { get; set; }

    public string Ip { get; set; }

    public RelayType Type { get; set; }

    public bool IsExtendingButton { get; set; }

    public TimeSpan? Delay { get; set; }
}