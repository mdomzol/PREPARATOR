using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Preparator.Helpers
{
    public class ChartAreaConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var data = values[0] as IEnumerable<double>;
            var width = values[1] as double? ?? 300;

            if (data == null || !data.Any())
                return Geometry.Empty;

            double height = 120;

            var max = data.Max();
            if (max <= 0) max = 1;

            int count = data.Count();
            double step = width / Math.Max(count - 1, 1);

            var figure = new PathFigure();
            var points = new List<Point>();

            int i = 0;
            foreach (var v in data)
            {
                double x = i * step;
                double y = height - (v / max * height);
                points.Add(new Point(x, y));
                i++;
            }

            if (!points.Any())
                return Geometry.Empty;

            // START
            figure.StartPoint = new Point(0, height);

            // LINE TO FIRST POINT
            figure.Segments.Add(new LineSegment(points[0], true));

            // LINE THROUGH ALL POINTS
            for (int j = 1; j < points.Count; j++)
                figure.Segments.Add(new LineSegment(points[j], true));

            // CLOSE TO BOTTOM RIGHT
            figure.Segments.Add(new LineSegment(new Point(points.Last().X, height), true));

            figure.IsClosed = true;

            return new PathGeometry(new[] { figure });
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}