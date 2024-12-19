public class LogEntry
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string Level { get; set; } // Info, Warning, Error
    public DateTime Timestamp { get; set; }
}