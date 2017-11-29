using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using GameEditor.Models;

namespace GameEditor.Services
{
	// Converter to unpack a Tile's sprite from within XAML view file
	[ValueConversion(typeof(Tile), typeof(BitmapImage))]
	public class TileToBitmapImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var selectedTile = (Tile)value;
			return selectedTile?.TileImage;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}