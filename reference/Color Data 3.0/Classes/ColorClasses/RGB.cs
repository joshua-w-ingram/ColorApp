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
using System.Linq;

namespace Color_Data_3._0.Classes.ColorClasses {
	public static class RGB {

		public static String ColorToHex(Color c) {
			String r = toHex(c.R);
			String g = toHex(c.G);
			String b = toHex(c.B);
			return "#" + r + g + b;
		}

		public static String ColorToWebsafeHex(Color c) {
			int rMod = (int)Math.Round((double)(c.R / 51.0)) * 51;
			int gMod = (int)Math.Round((double)(c.G / 51.0)) * 51;
			int bMod = (int)Math.Round((double)(c.B / 51.0)) * 51;
			String r = toHex(rMod);
			String g = toHex(gMod);
			String b = toHex(bMod);
			return "#" + r + g + b;
		}

		private static String toHex(int N) {
			if (N == 0)
				return "00";
			//nice way of setting max and min
			N = Math.Max(0, N);
			N = Math.Min(N, 255);
			String hex = "0123456789ABCDEF";
			return hex.ElementAt((N - N % 16) / 16).ToString() + hex.ElementAt(N % 16).ToString();

		}

		public static String R_text(int val) {
			return @"R: " + val.ToString() + " (0-255)";
		}

		public static String G_text(int val) {
			return @"G: " + val.ToString() + " (0-255)";
		}

		public static String B_text(int val) {
			return @"B: " + val.ToString() + " (0-255)";
		}

		public static String R_textShort(int val) {
			return @"R: " + val.ToString();
		}

		public static String G_textShort(int val) {
			return @"G: " + val.ToString();
		}

		public static String B_textShort(int val) {
			return @"B: " + val.ToString();
		}


	}
}
