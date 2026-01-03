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

namespace Color_Data_3._0.Classes {

	public class namedColor {

		public static int NAMEDCOLOR_DEVIATION {
			get {
				switch (StorageHelper.loadSetting<App.namedColorDistanceType>("NamedColorDistance")) {
					case App.namedColorDistanceType.p5:
						return 5;
					case App.namedColorDistanceType.p10:
						return 10;
					case App.namedColorDistanceType.p30:
						return 30;
					case App.namedColorDistanceType.p50:
						return 50;
					case App.namedColorDistanceType.all:
						return 200;
					default:
						return 30;
				}

			}
		}

		public string colorName { get; set; }

		public Color color { get; set; }

		public Brush colorBrush {
			get {
				return new SolidColorBrush(color);
			}
		}

		public double deviation { get; set; }

		public static namedColor findNamedColors(Color c, double range) {
			if (ApplicationLicense.IsTrial)
				return null;

			var xe = from colors in App.colorNameXml.Elements("Color")
				   where dist(int.Parse(colors.Attribute("R").Value), int.Parse(colors.Attribute("G").Value), int.Parse(colors.Attribute("B").Value), c.R, c.G, c.B) < range
				   orderby dist(int.Parse(colors.Attribute("R").Value), int.Parse(colors.Attribute("G").Value), int.Parse(colors.Attribute("B").Value), c.R, c.G, c.B) ascending
				   select colors;

			var namedColor = xe.FirstOrDefault();

			if (namedColor == null)
				return null;
			else {
				Color nC = Color.FromArgb(255, byte.Parse(namedColor.Attribute("R").Value), byte.Parse(namedColor.Attribute("G").Value), byte.Parse(namedColor.Attribute("B").Value));
				return new namedColor() { colorName = namedColor.Attribute("name").Value, color = nC, deviation = dist(nC.R, nC.G, nC.B, c.R, c.G, c.B) };
			}
		}

		public static double dist(double x1, double y1, double z1, double x2, double y2, double z2) {
			return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
		}



	}
}
