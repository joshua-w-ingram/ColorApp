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

namespace Color_Data_3._0.Classes {
	public class setColorComparer : System.Collections.Generic.IComparer<setColorModel> {

		public static System.Collections.Generic.IComparer<setColorModel> setColorSorter {
			get {
				return new setColorComparer();
			}
		}

		public int Compare(setColorModel a, setColorModel b) {

			if (a == null & b == null)
				return 0;
			else if (a == null)
				return -1;
			else if (b == null)
				return 1;
			else if (a.X < b.X)
				return -1;
			else if (a.X > b.X)
				return 1;
			else if (a.X == b.X) {
				if (a.Y < b.Y)
					return -1;
				else if (a.Y > b.Y)
					return 1;
				else {
					if (a.X < b.X)
						return -1;
					else if (a.X > b.X)
						return 1;
					else
						return 0;
				}
			}
			else
				return 1;

		}


	}
}
