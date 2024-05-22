namespace LocationCheck.Data.Models;

public class RequestLog
{
    public DateTimeOffset TimeStamp { get; set; }
    public IDictionary<string, string> Headers { get; set; }
    public string Method { get; set; }
    public string Host { get; set; }
    public string Path { get; set; }
    public string QueryParams { get; set; }
    public string Body { get; set; }
}