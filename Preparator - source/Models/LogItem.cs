using System.Windows.Media;
using static Preparator.ViewModels.AppsViewModel;

namespace Preparator.Models
{
    public class LogItem
    {
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public LogType Type { get; set; }
    }
}