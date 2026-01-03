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

	public class CMYK {
		public double C, M, Y, K;

		public CMYK(double _c, double _m, double _y, double _k) {
			C = _c;
			M = _m;
			Y = _y;
			K = _k;
		}

		public static CMYK convertRGB(Color c) {
			return convertRGB(c.R, c.G, c.B);
		}

		public static CMYK convertRGB(double r, double g, double b) {
			double c, m, y, k;

			c = 1 - (r / 255.0);
			m = 1 - (g / 255.0);
			y = 1 - (b / 255.0);

			k = 1;

			if (c < k) k = c;
			if (m < k) k = m;
			if (y < k) k = y;
			if (k == 1) {
				c = 0;
				m = 0;
				y = 0;
			}
			else {
				c = (c - k) / (1 - k);
				m = (m - k) / (1 - k);
				y = (y - k) / (1 - k);
			}

			return new CMYK(c , m , y , k );
		}

		public Color toRGB() {
			C = (C * (1 - K) + K);
			M = (M * (1 - K) + K);
			Y = (Y * (1 - K) + K);

			double r = (1 - C) * 255;
			double g = (1 - M) * 255;
			double b = (1 - Y) * 255;

			return Color.FromArgb(255, (byte)r, (byte)g, (byte)b);

		}

		public static String C_text(double val) {
			return @"C: " + (val*100).ToString("0.00") + " (%)";
		}

		public static String M_text(double val) {
			return @"M: " + (val * 100).ToString("0.00") + " (%)";
		}

		public static String Y_text(double val) {
			return @"Y: " + (val * 100).ToString("0.00") + " (%)";
		}

		public static String K_text(double val) {
			return @"K: " + (val * 100).ToString("0.00") + " (%)";
		}

		public static String C_textShort(double val) {
			return @"C: " + (val * 100).ToString("0.00");
		}

		public static String M_textShort(double val) {
			return @"M: " + (val * 100).ToString("0.00");
		}

		public static String Y_textShort(double val) {
			return @"Y: " + (val * 100).ToString("0.00");
		}

		public static String K_textShort(double val) {
			return @"K: " + (val * 100).ToString("0.00");
		}
	}
}
