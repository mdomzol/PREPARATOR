using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Preparator.Helpers
{
    public class ChartConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var data = values[0] as IEnumerable<double>;
            var width = values[1] as double? ?? 300;
            var max = values.Length > 2 ? (values[2] as double? ?? 1) : 1;

            if (data == null || !data.Any())
                return new PointCollection();

            if (max <= 0) max = 1;

            var points = new PointCollection();

            double height = 120;

            int count = data.Count();
            double step = width / Math.Max(count - 1, 1);

            int i = 0;
            foreach (var v in data)
            {
                double x = i * step;
                double y = height - (v / max * height);

                points.Add(new Point(x, y));
                i++;
            }

            return points;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}