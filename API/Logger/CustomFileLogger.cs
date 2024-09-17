namespace API.Logger
{
    public class CustomFileLogger : ILogger
    {
        private readonly string _logFilePath;
        private readonly object _lock = new object();
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomFileLogger(string logFilePath, IHttpContextAccessor httpContextAccessor)
        {
            _logFilePath = logFilePath;
            _httpContextAccessor = httpContextAccessor;

            // Ensure the log directory exists
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Ensure the log file is created
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Dispose();
            }
        }
        public IDisposable BeginScope<TState>(TState state) => null;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // Use a lock to ensure thread safety when writing to the log file.
            lock (_lock)
            {
                Console.WriteLine("Logging: " + formatter(state, exception));
                string logLevelShortForm = GetLogLevelShortForm(logLevel);
                string currentUsername = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonymous";
                string timestamp = $"{DateTime.Now:MM-dd-yyyyTHH:mm:ss tt}";
                string requestPath = _httpContextAccessor.HttpContext?.Request?.Path ?? "N/A";
                string requestMethod = _httpContextAccessor.HttpContext?.Request?.Method ?? "N/A";
                string userIp = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "N/A";

                // Format the log entry with timestamp, log level, and the log message.
                string logEntry = $"{timestamp} [{logLevelShortForm}] User: {currentUsername} | Path: {requestPath} | Method: {requestMethod} | IP: {userIp} | {formatter(state, exception)}";
                if (exception != null)
                {
                    logEntry += Environment.NewLine + $"Exception: {exception}";
                }

                logEntry += Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine;

                // Append the log entry to the log file.
                File.AppendAllText(_logFilePath, logEntry);
            }
        }
        private string GetLogLevelShortForm(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => "TRC",
                LogLevel.Debug => "DBG",
                LogLevel.Information => "INFO",
                LogLevel.Warning => "WARN",
                LogLevel.Error => "ERR",
                LogLevel.Critical => "CRT",
                _ => logLevel.ToString().ToUpper()
            };
        }
        public void Dispose() { }
    }
}
