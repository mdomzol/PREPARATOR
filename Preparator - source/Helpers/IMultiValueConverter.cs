using System;
using System.Globalization;
using System.Windows.Data;

namespace Preparator.Helpers
{
    public class ActivePresetToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var activePreset = values[0] as string;
            var presetName = values[1] as string;

            if (string.IsNullOrEmpty(activePreset) || string.IsNullOrEmpty(presetName))
                return false;

            return activePreset == presetName;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}