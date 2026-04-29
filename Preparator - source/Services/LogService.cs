using Preparator.Models;
using System;
using static Preparator.ViewModels.AppsViewModel;

namespace Preparator.Services
{
    public class LogService
    {
        public LogItem Parse(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return Create("Empty log line","", LogType.Info);
            }

            if (line.Contains("Installing", StringComparison.OrdinalIgnoreCase))
                return Create(line, "🔧", LogType.Install);

            if (line.Contains("Downloading", StringComparison.OrdinalIgnoreCase))
                return Create(line, "⬇", LogType.Download);

            if (line.Contains("Complete", StringComparison.OrdinalIgnoreCase))
                return Create(line, "✔", LogType.Success);

            if (line.Contains("Error", StringComparison.OrdinalIgnoreCase))
                return Create(line, "✖", LogType.Error);

            return Create(line, "", LogType.Info);
        }

        private LogItem Create(string message, string icon, LogType type)
        {
            return new LogItem
            {
                Message = string.IsNullOrEmpty(icon) ? message : $"{icon} {message}",
                Timestamp = DateTime.Now,
                Type = type
            };
        }
    }
}