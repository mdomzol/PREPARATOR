using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace Preparator.ViewModels
{
    public class SystemStatsViewModel : INotifyPropertyChanged
    {
        private readonly PerformanceCounter _downloadCounter;
        private readonly PerformanceCounter _uploadCounter;
        private readonly PerformanceCounter _diskReadCounter;
        private readonly PerformanceCounter _diskWriteCounter;

        private readonly Timer _timer;

        private string _download;
        public string Download
        {
            get => _download;
            set { _download = value; OnPropertyChanged(); }
        }

        private string _upload;
        public string Upload
        {
            get => _upload;
            set { _upload = value; OnPropertyChanged(); }
        }

        private string _diskRead;
        public string DiskRead
        {
            get => _diskRead;
            set { _diskRead = value; OnPropertyChanged(); }
        }

        private string _diskWrite;
        public string DiskWrite
        {
            get => _diskWrite;
            set { _diskWrite = value; OnPropertyChanged(); }
        }

        public SystemStatsViewModel()
        {
            var category = new PerformanceCounterCategory("Network Interface");
            var instance = category.GetInstanceNames()
                .FirstOrDefault(n => !n.ToLower().Contains("loopback"));

            _downloadCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
            _uploadCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);

            _diskReadCounter = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            _diskWriteCounter = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");

            _timer = new Timer(1000);
            _timer.Elapsed += Update;
            _timer.Start();
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            Download = FormatBytes(_downloadCounter.NextValue());
            Upload = FormatBytes(_uploadCounter.NextValue());
            DiskRead = FormatBytes(_diskReadCounter.NextValue());
            DiskWrite = FormatBytes(_diskWriteCounter.NextValue());
        }

        private string FormatBytes(float bytes)
        {
            string[] sizes = { "B/s", "KB/s", "MB/s", "GB/s" };
            int order = 0;
            while (bytes >= 1024 && order < sizes.Length - 1)
            {
                order++;
                bytes /= 1024;
            }
            return $"{bytes:0.##} {sizes[order]}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
