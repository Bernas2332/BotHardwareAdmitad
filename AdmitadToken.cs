namespace BotHardware;

public class WebmasterResult
{
    public string? name { get; set; }
    public string? status { get; set; }
    public bool validation_passed { get; set; }
    public string? site_url { get; set; }
}

public class WebmasterResponse
{
    public List<WebmasterResult>? results { get; set; }
}