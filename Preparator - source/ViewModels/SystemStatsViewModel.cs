using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

public class SystemStatsViewModel : INotifyPropertyChanged
{
    private readonly PerformanceCounter _downloadCounter;
    private readonly PerformanceCounter _diskWriteCounter;

    private readonly DispatcherTimer _timer;

    private double _smoothDownload = 0;
    private double _smoothDiskWrite = 0;

    private const double SmoothFactor = 0.3;

    private const int MaxPoints = 120;
    private void AddPoint(ObservableCollection<double> list, double value)
    {
        list.Add(value);

        if (list.Count > MaxPoints)
            list.RemoveAt(0);
    }

    public ObservableCollection<double> DownloadHistory { get; } = new();
    public ObservableCollection<double> DiskWriteHistory { get; } = new();

    private double _maxDownload = 1;
    public double MaxDownload
    {
        get => _maxDownload;
        set { _maxDownload = value; OnPropertyChanged(); }
    }

    private double _maxDisk = 1;
    public double MaxDisk
    {
        get => _maxDisk;
        set { _maxDisk = value; OnPropertyChanged(); }
    }

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

    private string _diskWrite;
    public string DiskWrite
    {
        get => _diskWrite;
        set { _diskWrite = value; OnPropertyChanged(); }
    }

    public double MaxValue =>
    new[]
    {
        DownloadHistory.DefaultIfEmpty(0).Max(),
        DiskWriteHistory.DefaultIfEmpty(0).Max()
    }.Max();

    public string MaxValueFormatted => FormatBytes(MaxValue);

    private double _offset;
    public double Offset
    {
        get => _offset;
        set { _offset = value; OnPropertyChanged(); }
    }

    public SystemStatsViewModel()
    {
        var category = new PerformanceCounterCategory("Network Interface");
        var instance = category.GetInstanceNames()
            .FirstOrDefault(n => !n.ToLower().Contains("loopback"));

        _downloadCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);

        _diskWriteCounter = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(500);
        _timer.Tick += Update;
        _timer.Start();
    }

    private void Update(object sender, EventArgs e)
    {
        // NET
        var rawDownload = _downloadCounter.NextValue();

        _smoothDownload = Smooth(_smoothDownload, rawDownload);

        Download = FormatBytes(_smoothDownload);

        DownloadValue = _smoothDownload;
        OnPropertyChanged(nameof(DownloadPercent));

        AddPoint(DownloadHistory, _smoothDownload);

        // DISK
        var diskValue = _diskWriteCounter.NextValue();
        _smoothDiskWrite = Smooth(_smoothDiskWrite, diskValue);

        DiskWrite = FormatBytes(_smoothDiskWrite);

        AddPoint(DiskWriteHistory, _smoothDiskWrite);

        // SCALE
        MaxDownload = Math.Max(DownloadHistory.DefaultIfEmpty(1).Max(), 1) * 1.05;
        MaxDisk = Math.Max(DiskWriteHistory.DefaultIfEmpty(1).Max(), 1) * 1.05;

        OnPropertyChanged(nameof(DownloadHistory));
        OnPropertyChanged(nameof(DiskWriteHistory));
        OnPropertyChanged(nameof(MaxDownload));
        OnPropertyChanged(nameof(MaxDisk));
    }

    public double VisibleMaxDownload =>
    Math.Max(DownloadHistory.DefaultIfEmpty(1).Max(), 1);

    public double VisibleMaxDisk =>
    Math.Max(DiskWriteHistory.DefaultIfEmpty(1).Max(), 1);

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