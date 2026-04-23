using System.Windows;
using Preparator.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Specialized;

namespace Preparator.Views
{
    public partial class AppsView : UserControl
    {
        public AppsView()
        {
            InitializeComponent();
            DataContext = new AppsViewModel();

        }

        private void LogsList_Loaded(object sender, RoutedEventArgs e)
        {
            var list = (ListBox)sender;

            if (list.Items is INotifyCollectionChanged collection)
            {
                collection.CollectionChanged += (s, args) =>
                {
                    if (list.Items.Count > 0)
                        list.ScrollIntoView(list.Items[list.Items.Count - 1]);
                };
            }
        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            var vm = (AppsViewModel)DataContext;
            var stats = vm.Stats;

            var pos = e.GetPosition(ChartCanvas);

            if (stats.DownloadHistory.Count < 2)
                return;

            double ratio = pos.X / ChartCanvas.ActualWidth;

            int maxCount = Math.Min(
                stats.DownloadHistory.Count,
                stats.DiskWriteHistory.Count
            );

            int index = (int)(ratio * (maxCount - 1));
            index = Math.Clamp(index, 0, maxCount - 1);

            var net = stats.DownloadHistory[index];
            var disk = stats.DiskWriteHistory.ElementAtOrDefault(index);

            string netText = FormatBytes(net);
            string diskText = FormatBytes(disk);

            TooltipText.Text =
                $"Network: {netText}\nDisk: {diskText}";

            Canvas.SetLeft(Tooltip, pos.X + 12);
            Canvas.SetTop(Tooltip, pos.Y - 20);

            Tooltip.Visibility = Visibility.Visible;

            Panel.SetZIndex(Tooltip, 999);
        }

        private void Chart_MouseLeave(object sender, MouseEventArgs e)
        {
            Tooltip.Visibility = Visibility.Collapsed;
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
    }
}
