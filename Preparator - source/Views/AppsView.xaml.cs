using Preparator.ViewModels;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

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

        private void Preset_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement btn)
                return;

            var pos = btn.TransformToAncestor(PresetList)
                         .Transform(new Point(0, 0));

            // === EASING ===
            var ease = new CubicEase
            {
                EasingMode = EasingMode.EaseOut
            };

            // === MOVE (X) ===
            var moveAnim = new DoubleAnimationUsingKeyFrames
            {
                Duration = TimeSpan.FromMilliseconds(260)
            };

            moveAnim.KeyFrames.Add(new EasingDoubleKeyFrame(
                pos.X + 12, // overshoot
                KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(160)))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            });

            moveAnim.KeyFrames.Add(new EasingDoubleKeyFrame(
                pos.X,
                KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(260)))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            });

            HighlightTransform.BeginAnimation(TranslateTransform.XProperty, moveAnim);

            // === WIDTH ===
            var widthAnim = new DoubleAnimation
            {
                To = btn.ActualWidth,
                Duration = TimeSpan.FromMilliseconds(220),
                EasingFunction = ease
            };

            SelectionHighlight.BeginAnimation(WidthProperty, widthAnim);

            // === FADE IN ===
            var fadeAnim = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromMilliseconds(120)
            };

            SelectionHighlight.BeginAnimation(OpacityProperty, fadeAnim);

            // === SCALE POP (micro interaction) ===
            var scaleAnim = new DoubleAnimation
            {
                From = 0.95,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(180),
                EasingFunction = ease
            };

            HighlightScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            HighlightScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);

            // === MVVM COMMAND ===
            if (DataContext is AppsViewModel vm && btn.DataContext is string preset)
            {
                vm.ApplyPresetCommand.Execute(preset);
            }
        }
    }
}
