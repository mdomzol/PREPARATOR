using System.Windows;
using Preparator.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace Preparator.Views
{
    public partial class AppsView : UserControl
    {
        public AppsView()
        {
            InitializeComponent();

        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            var vm = (AppsViewModel)DataContext;
            var stats = vm.Stats;

            var pos = e.GetPosition(ChartCanvas);

            if (stats.DownloadHistory.Count < 2)
                return;

            double ratio = pos.X / ChartCanvas.ActualWidth;

            int index = (int)(ratio * (stats.DownloadHistory.Count - 1));

            index = Math.Clamp(index, 0, stats.DownloadHistory.Count - 1);

            var net = stats.DownloadHistory[index];
            var disk = stats.DiskWriteHistory.ElementAtOrDefault(index);

            TooltipText.Text =
                $"Network: {net / 1024 / 1024:0.##} MB/s\n" +
                $"Disk: {disk / 1024 / 1024:0.##} MB/s";

            Canvas.SetLeft(Tooltip, pos.X + 12);
            Canvas.SetTop(Tooltip, pos.Y - 20);

            Tooltip.Visibility = Visibility.Visible;

            Panel.SetZIndex(Tooltip, 999); // 🔥 FIX: nie znika pod UI
        }
    }
}
