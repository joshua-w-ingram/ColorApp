using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Color_Data_3._0.Controls {
	public partial class imageButton : UserControl {


		public static readonly DependencyProperty IconLightProperty =
				DependencyProperty.Register("iconLight", typeof(Uri), typeof(imageButton), null);

		public Uri iconLight {
			get { return (Uri)GetValue(IconLightProperty); }
			set { SetValue(IconLightProperty, value); }
		}

		public static readonly DependencyProperty IconDarkProperty =
				DependencyProperty.Register("iconDark", typeof(Uri), typeof(imageButton), null);

		public Uri iconDark {
			get { return (Uri)GetValue(IconDarkProperty); }
			set { SetValue(IconDarkProperty, value); }
		}

		public imageButton() {
			InitializeComponent();
			DataContext = this;
		}

	}
}