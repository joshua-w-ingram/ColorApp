using System;
using System.Windows;
using System.Windows.Data;

namespace Color_Data_3._0.Classes.Converters {

	public class BoolToVisibilityConverter : IValueConverter {

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if (parameter == null) {
				return ((bool)value == true) ? Visibility.Visible : Visibility.Collapsed;
			}
			else if (parameter.ToString() == "Inverse") {
				return ((bool)value == true) ? Visibility.Collapsed : Visibility.Visible;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			var visiblity = (Visibility)value;
			return visiblity == Visibility.Visible;
		}

	}
}