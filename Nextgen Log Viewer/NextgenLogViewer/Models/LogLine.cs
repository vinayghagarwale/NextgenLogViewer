

namespace NextgenLogViewer.Models
{
    public enum LogType
    {
        Trace,
        Info,
        Fatal,
        Warning,
        Debug,
        Error,
        SQL,
        Other
    }
    public class LogLine
    {
        public string logdate { get; set; }
        public string logtime { get; set; }
        public LogType logType { get; set; }
        public string logmessage { get; set; }
        public double TraceTime { get; set; }
    }
}
