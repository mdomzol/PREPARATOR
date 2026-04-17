using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

public class SystemStatsViewModel : INotifyPropertyChanged
{
    private readonly PerformanceCounter _downloadCounter;
    private readonly PerformanceCounter _uploadCounter;
    private readonly PerformanceCounter _diskReadCounter;
    private readonly PerformanceCounter _diskWriteCounter;

    private readonly DispatcherTimer _timer;

    private double _smoothDownload = 0;
    private double _smoothUpload = 0;
    private double _smoothDiskRead = 0;
    private double _smoothDiskWrite = 0;

    private const double SmoothFactor = 0.3;

    public ObservableCollection<double> DownloadHistory { get; } = new();
    public ObservableCollection<double> DiskHistory { get; } = new();
    private const int MaxPoints = 30;

    private double _downloadValue;
    public double DownloadValue
    {
        get => _downloadValue;
        set { _downloadValue = value; OnPropertyChanged(); }
    }

    public double DownloadPercent => Math.Min(DownloadValue / (100 * 1024 * 1024) * 100, 100);

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

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Update;
        _timer.Start();
    }

    private void Update(object sender, EventArgs e)
    {
        // NET STATS
        var rawDownload = _downloadCounter.NextValue();
        var rawUpload = _uploadCounter.NextValue();

        _smoothDownload = Smooth(_smoothDownload, rawDownload);
        _smoothUpload = Smooth(_smoothUpload, rawUpload);

        Download = FormatBytes(_smoothDownload);
        Upload = FormatBytes(_smoothUpload);

        DownloadValue = _smoothDownload;
        OnPropertyChanged(nameof(DownloadPercent));

        DownloadHistory.Add(_smoothDownload);

        if (DownloadHistory.Count > MaxPoints)
            DownloadHistory.RemoveAt(0);

        // DISK STATS
        var diskRead = _diskReadCounter.NextValue();
        var diskWrite = _diskWriteCounter.NextValue();

        _smoothDiskRead = Smooth(_smoothDiskRead, diskRead);
        _smoothDiskWrite = Smooth(_smoothDiskWrite, diskWrite);

        DiskRead = FormatBytes(_smoothDiskRead);
        DiskWrite = FormatBytes(_smoothDiskWrite);

        DiskHistory.Add(_smoothDiskRead);

        if (DiskHistory.Count > MaxPoints)
            DiskHistory.RemoveAt(0);
    }

    private double Smooth(double previous, double current)
    {
        return previous + (current - previous) * SmoothFactor;
    }

    private string FormatBytes(double bytes)
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