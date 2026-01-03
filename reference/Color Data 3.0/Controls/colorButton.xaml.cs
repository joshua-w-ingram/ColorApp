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
	public partial class colorButton : Button {

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(colorButton), new PropertyMetadata(""));

		public string Title {
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public static readonly DependencyProperty DescriptionProperty =
			DependencyProperty.Register("Description", typeof(string), typeof(colorButton), new PropertyMetadata(""));

		public string Description {
			get { return (string)GetValue(DescriptionProperty); }
			set { SetValue(DescriptionProperty, value); }
		}

		public static readonly DependencyProperty ColorProperty =
			DependencyProperty.Register("ColorBrush", typeof(Brush), typeof(colorButton), new PropertyMetadata(new SolidColorBrush(Colors.Cyan)));

		public Brush ColorBrush {
			get {
				return (Brush)GetValue(ColorProperty);
			}
			set {
				SetValue(ColorProperty, value);
			}
		}

		public colorButton() {
			InitializeComponent();
			DataContext = this;
		}


	}
}