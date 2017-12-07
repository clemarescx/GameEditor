using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GameEditor.Services
{
    // Converter to unpack a Tile's sprite from within XAML view file
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class StringToTerrainSpriteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var filename = (string)value;
            var img = new BitmapImage();
            if(value != null)
                img = SpriteLoader.TerrainSprites[ filename ];
            else
                img = SpriteLoader.ErrorSprite;
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
