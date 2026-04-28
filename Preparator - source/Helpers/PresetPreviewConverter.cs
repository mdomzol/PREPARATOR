using Preparator.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Preparator.Helpers
{
    public class PresetPreviewConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var preset = values[0] as string;
            var vm = values[1] as AppsViewModel;

            if (preset == null || vm == null)
                return null;

            return vm.GetPresetApps(preset);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}