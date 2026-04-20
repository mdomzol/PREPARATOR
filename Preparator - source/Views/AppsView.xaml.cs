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
            var pos = e.GetPosition(ChartCanvas);

            var vm = (AppsViewModel)DataContext;
            var stats = vm.Stats;

            int index = (int)(pos.X / ChartCanvas.ActualWidth * stats.DownloadHistory.Count);

            if (index >= 0 && index < stats.DownloadHistory.Count)
            {
                var value = stats.DownloadHistory[index];

                TooltipText.Text = $"{value / 1024 / 1024:0##} MB/s";
                Canvas.SetLeft(Tooltip, pos.X + 10);
                Canvas.SetTop(Tooltip, pos.Y - 20);

                Tooltip.Visibility = Visibility.Visible;
            }
        }
    }
}
