using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Color_Data_3._0.Classes.Converters {

	public class BoolToBrushConverter : IValueConverter {

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if (parameter == null) {
				return ((bool)value == true) ? new SolidColorBrush(Color.FromArgb(70,255,255,255)) : new SolidColorBrush(Color.FromArgb(0,255,255,255));
			}
			else if (parameter.ToString() == "Inverse") {
				return ((bool)value == true) ? new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)) : new SolidColorBrush(Color.FromArgb(70, 255, 255, 255));
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			throw new NotImplementedException();
		}

	}
}