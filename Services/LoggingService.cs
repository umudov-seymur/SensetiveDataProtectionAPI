public interface ILoggingService
{
    void Log(string message, string level);
    IEnumerable<LogEntry> GetAllLogs();
}

public class LoggingService : ILoggingService
{
    private readonly AppDbContext _context;

    public LoggingService(AppDbContext context)
    {
        _context = context;
    }

    public void Log(string message, string level)
    {
        var log = new LogEntry
        {
            Message = message,
            Level = level,
            Timestamp = DateTime.UtcNow
        };

        _context.Add(log);
        _context.SaveChanges();
    }

    public IEnumerable<LogEntry> GetAllLogs()
    {
        return _context.LogEntries.ToList();
    }
}