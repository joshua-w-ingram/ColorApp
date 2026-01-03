using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Color_Data_3._0.Classes.ColorClasses;

namespace Color_Data_3._0.Controls {
	public partial class colorSquare : UserControl {

		public enum SelectType {
			select,
			tap
		}

		public static readonly DependencyProperty TitleProperty =
	DependencyProperty.Register("Title", typeof(string), typeof(colorSquare), new PropertyMetadata("Color Title"));

		public string Title {
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}


		public static readonly DependencyProperty PositionProperty =
	DependencyProperty.Register("Position", typeof(int), typeof(colorSquare), new PropertyMetadata(0));

		public int Position {
			get { return (int)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}


		public static readonly DependencyProperty PositionVisibleProperty =
	DependencyProperty.Register("PositionVisible", typeof(Visibility), typeof(colorSquare), new PropertyMetadata(Visibility.Visible));

		public Visibility PositionVisible {
			get { return (Visibility)GetValue(PositionVisibleProperty); }
			set { SetValue(PositionVisibleProperty, value); }
		}


		public static readonly DependencyProperty SelectModeProperty =
DependencyProperty.Register("selectMode", typeof(SelectType), typeof(colorSquare), new PropertyMetadata(SelectType.select));

		public SelectType SelectMode {
			get { return (SelectType)GetValue(SelectModeProperty); }
			set { SetValue(SelectModeProperty, value); }
		}

		public static readonly DependencyProperty FillProperty =
	DependencyProperty.Register("Fill", typeof(Brush), typeof(colorSquare), new PropertyMetadata(new SolidColorBrush(Colors.Cyan)));

		public Brush Fill {
			get { return (Brush)GetValue(FillProperty); }
			set {
				SetValue(FillProperty, value);

				Color currentColor = (Color)value.GetValue(SolidColorBrush.ColorProperty);
				HSL hsl = HSL.convertRGB(currentColor);

				if (hsl.L > 0.4) {
					tb_color.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
					tb_position.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
				}
				else {
					tb_color.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
					tb_position.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
				}
			}
		}

		public colorSquare() {
			InitializeComponent();
			DataContext = this;

		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
			base.OnMouseLeftButtonDown(e);

			if (SelectMode == SelectType.select)
				VisualStateManager.GoToState(this, "Selected", true);
			else
				VisualStateManager.GoToState(this, "Tapped", true);

		}

		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e) {
			if (SelectMode == SelectType.tap)
				VisualStateManager.GoToState(this, "Unselected", true);
		}

		protected override void OnMouseLeave(MouseEventArgs e) {
			if (SelectMode == SelectType.tap)
				VisualStateManager.GoToState(this, "Unselected", true);
		}

	}
}