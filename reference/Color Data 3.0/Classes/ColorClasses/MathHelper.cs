using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Color_Data_3._0.Classes.ColorClasses {

	public static class MathHelper {

		public static double minimum(double a, double b, double c) {
			return Math.Min(a, Math.Min(b, c));
		}

		public static double maximum(double a, double b, double c) {
			return Math.Max(a, Math.Max(b, c));
		}

		public static double deg2Rad(double deg) {
			return deg * 180 / Math.PI;
		}

		public static double rad2Deg(double rad) {
			return rad * Math.PI / 180;
		}



	}

}
