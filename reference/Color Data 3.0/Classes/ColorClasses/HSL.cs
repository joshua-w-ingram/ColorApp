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

	public class HSL {
		public double H, S, L;

		public const double H_MAX = 360;
		public const double S_MAX = 1;
		public const double L_MAX = 1;

		public HSL(double _h, double _s, double _l) {
			H = _h;
			S = _s;
			L = _l;
		}

		public static HSL convertRGB(Color c) {
			return convertRGB(c.R, c.G, c.B);
		}

		public static HSL convertRGB(double r, double g, double b) {
			double h = 0;
			double s = 0;
			double l = 0;

			//RGB from 0 to 255
			double var_R = (r / 255);
			double var_G = (g / 255);
			double var_B = (b / 255);

			double var_Min = MathHelper.minimum(var_R, var_G, var_B);
			double var_Max = MathHelper.maximum(var_R, var_G, var_B);
			double delta = var_Max - var_Min;

			l = (var_Max + var_Min) / 2;

			//HSL results from 0 to 1
			if (delta == 0) {
				h = 0;
				s = 0;
			}
			else {
				if (l < 0.5)
					s = delta / (var_Max + var_Min);
				else
					s = delta / (2.0 - var_Max - var_Min);

				//double del_R = (((var_Max - var_R) / 6) + (del_Max / 2)) / del_Max;
				//double del_G = (((var_Max - var_G) / 6) + (del_Max / 2)) / del_Max;
				//double del_B = (((var_Max - var_B) / 6) + (del_Max / 2)) / del_Max;

				if (var_R == var_Max)
					//del_B - del_G;
					h = (var_G - var_B) / delta;
				else if (var_G == var_Max)
					//h = (1 / 3) + del_R - del_B;
					h = 2.0 + (var_B - var_R) / delta;
				else if (var_B == var_Max)
					//h = (2 / 3) + del_G - del_R;
					h = 4.0 + (var_R - var_G) / delta;

				h *= 60.0;

				if (h < 0) h += 360.0;
				if (h > 360.0) h -= 360.0;
			}


			return new HSL(h, s, l);
		}

		public Color toRGB() {
			double R, G, B;

			//HSL from 0 to 1
			if (S == 0) {
				R = L * 255;
				G = L * 255;
				B = L * 255;
			}
			else {
				double var_2;
				if (L < 0.5)
					var_2 = L * (1.0 + S);
				else
					var_2 = (L + S) - (S * L);

				double var_1 = 2.0 * L - var_2;

				double tH = H / 360.0;

				double var_3 = tH + 1 / 3.0;

				R = 255 * Hue_2_RGB(var_1, var_2, tH + (1 / 3.0));
				G = 255 * Hue_2_RGB(var_1, var_2, tH);
				B = 255 * Hue_2_RGB(var_1, var_2, tH - (1 / 3.0));
			}
			return Color.FromArgb(255, (byte)R, (byte)G, (byte)B);
		}

		private double Hue_2_RGB(double v1, double v2, double vH) {
			if (vH < 0)
				vH += 1;

			if (vH > 1)
				vH -= 1;

			if ((6.0 * vH) < 1)
				return (v1 + (v2 - v1) * 6.0 * vH);

			if ((2.0 * vH) < 1)
				return (v2);

			if ((3.0 * vH) < 2)
				return (v1 + (v2 - v1) * ((2 / 3.0) - vH) * 6.0);

			return (v1);
		}

		public HSL addH(Double _h) {
			return new HSL((H + _h) % 360, S, L);
		}

		public HSL addS(Double _s) {
			if ((S + _s) > 1.0)
				return new HSL(H, 1.0, L);
			else
				return new HSL(H, S + _s, L);
		}

		public HSL addL(Double _l) {
			if ((L + _l) > 1.0)
				return new HSL(H, S, 1.0);
			else
				return new HSL(H, S, L + _l);
		}

		public HSL withH(Double _h) {
			return new HSL(_h, S, L);
		}

		public HSL withS(Double _s) {
			return new HSL(H, _s, L);
		}

		public HSL withL(Double _l) {
			return new HSL(H, S, _l);
		}

		public static String H_text(double val) {
			return @"H: " + val.ToString("0.00") + " (0-360)";
		}

		public static String S_text(double val) {
			return @"S: " + (val * 100).ToString("0.00") + " (%)";
		}

		public static String L_text(double val) {
			return @"L: " + (val * 100).ToString("0.00") + " (%)";
		}

		public static String H_textShort(double val) {
			return @"H: " + val.ToString("0.00");
		}

		public static String S_textShort(double val) {
			return @"S: " + (val * 100).ToString("0.00");
		}

		public static String L_textShort(double val) {
			return @"L: " + (val * 100).ToString("0.00");
		}

	}

}
