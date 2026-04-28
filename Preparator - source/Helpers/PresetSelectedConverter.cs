using System;
using System.Globalization;
using System.Windows.Data;

namespace Preparator.Helpers
{
    public class PresetSelectedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return false;

            var activePreset = values[0]?.ToString();
            var presetName = values[1]?.ToString();

            return activePreset == presetName;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }
}