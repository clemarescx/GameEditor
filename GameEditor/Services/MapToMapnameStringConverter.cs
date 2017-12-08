using System;
using System.Globalization;
using System.Windows.Data;
using GameEditor.Models;

namespace GameEditor.Services
{

    /// <summary>
    /// unpack an AreaMap's name from within XAML view file.
    /// ...Perhaps unnecessary.
    /// </summary>
    [ValueConversion(typeof(AreaMap), typeof(string))]
    public class MapToMapnameStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var map = (AreaMap)value;
            var mapName = value != null ? map.Name : "*";
            return mapName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
