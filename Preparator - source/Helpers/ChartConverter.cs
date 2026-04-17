using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Preparator.Helpers
{
    public class ChartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var values = value as IEnumerable<double>;

            if (values == null || !values.Any())
                return null;

            var points = new PointCollection();

            double widthStep = 6;
            double max = values.Max();
            if (max == 0)
                max = 1;

            int i = 0;
            foreach (var v in values)
            {
                double x = i * widthStep;
                double y = 60 - (v / max * 60);

                points.Add(new Point(x, y));
                i++;
            }

            return points;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}