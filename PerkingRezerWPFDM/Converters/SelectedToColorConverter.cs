using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PerkingRezerWPFDM.Converters
{
    public class SelectedToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected)
            {
                return isSelected ? Brushes.LightBlue : Brushes.DarkGreen;
            }
            return Brushes.DarkGreen;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
