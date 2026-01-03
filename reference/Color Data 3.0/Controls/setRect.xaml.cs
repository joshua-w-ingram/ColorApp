using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Color_Data_3._0.Classes;


namespace Color_Data_3._0.Controls {
	public partial class setRect : UserControl {

		public static readonly DependencyProperty OrientationProperty =
	DependencyProperty.Register("Orientation", typeof(Orientation), typeof(setRect), new PropertyMetadata(Orientation.Horizontal));

		public Orientation Orientation {
			get { return (Orientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		public static readonly DependencyProperty ColorProperty =
	DependencyProperty.Register("setColorPalette", typeof(ObservableCollection<setColorModel>), typeof(setRect), null);

		public ObservableCollection<setColorModel> setColorPalette {
			get {
				return (ObservableCollection<setColorModel>)GetValue(ColorProperty);
			}
			set {
				SetValue(ColorProperty, value);
				load();
			}
		}

		public setRect() {
			InitializeComponent();
			DataContext = this;

			this.Loaded += new RoutedEventHandler(setRect_Loaded);
		}

		void setRect_Loaded(object sender, RoutedEventArgs e) {
			load();
		}

		public void load() {
			if (setColorPalette == null)
				return;

			if (Orientation == System.Windows.Controls.Orientation.Horizontal) {
				g_palette.ColumnDefinitions.Clear();
			}
			else {
				g_palette.RowDefinitions.Clear();
			}
			g_palette.Children.Clear();

			int cnt = 0;
			foreach (setColorModel c in setColorPalette) {

				Rectangle r = new Rectangle();
				r.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
				r.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
				r.Fill = c.ColorBrush;

				if (Orientation == System.Windows.Controls.Orientation.Horizontal) {
					ColumnDefinition rd = new ColumnDefinition();
					rd.Width = new GridLength(1, GridUnitType.Star);
					g_palette.ColumnDefinitions.Add(rd);
					r.SetValue(Grid.ColumnProperty, cnt);
				}
				else {
					RowDefinition rd = new RowDefinition();
					rd.Height = new GridLength(1, GridUnitType.Star);
					g_palette.RowDefinitions.Add(rd);
					r.SetValue(Grid.RowProperty, cnt);
				}

				g_palette.Children.Add(r);

				cnt++;
			}
		}

	}
}
