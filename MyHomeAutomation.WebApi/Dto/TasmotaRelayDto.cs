namespace MyHomeAutomation.WebApi.Dto;

public class TasmotaRelayDto
{
    public  Status Status { get; set; }
}

public class Status
{
    public string Power { get; set; }
}