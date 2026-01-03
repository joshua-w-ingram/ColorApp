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
	public partial class simpleColorButton : Button {

		public static readonly DependencyProperty ColorProperty =
			DependencyProperty.Register("ColorBrush", typeof(Brush), typeof(simpleColorButton), null);

		public Brush ColorBrush {
			get {
				return (Brush)GetValue(ColorProperty);
			}
			set {
				SetValue(ColorProperty, value);
			}
		}

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(simpleColorButton), new PropertyMetadata(""));

		public string Title {
			get {
				return (string)GetValue(TitleProperty);
			}
			set {
				SetValue(TitleProperty, value);
			}
		}

		public static readonly DependencyProperty DescProperty =
			DependencyProperty.Register("Desc", typeof(string), typeof(simpleColorButton), new PropertyMetadata(""));

		public string Desc {
			get {
				return (string)GetValue(DescProperty);
			}
			set {
				SetValue(DescProperty, value);
			}
		}

		public simpleColorButton() {
			InitializeComponent();
			DataContext = this;

		}


	}
}