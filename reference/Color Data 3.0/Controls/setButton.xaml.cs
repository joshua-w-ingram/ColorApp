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
	public partial class setButton : Button {

		public static readonly DependencyProperty TitleProperty =
	DependencyProperty.Register("Title", typeof(string), typeof(setButton), new PropertyMetadata("Set Title"));

		public string Title {
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public static readonly DependencyProperty DescriptionProperty =
			DependencyProperty.Register("Description", typeof(string), typeof(setButton), new PropertyMetadata("Set Description"));

		public string Description {
			get { return (string)GetValue(DescriptionProperty); }
			set { SetValue(DescriptionProperty, value); }
		}

		public static readonly DependencyProperty OrientationProperty =
	DependencyProperty.Register("Orientation", typeof(Orientation), typeof(setButton), new PropertyMetadata(Orientation.Horizontal));

		public Orientation Orientation {
			get { return (Orientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		public static readonly DependencyProperty SetColorPaletteProperty =
	DependencyProperty.Register("SetColorPalette", typeof(ObservableCollection<setColorModel>), typeof(setButton), null);

		public ObservableCollection<setColorModel> SetColorPalette {
			get {
				return (ObservableCollection<setColorModel>)GetValue(SetColorPaletteProperty);
			}
			set {
				SetValue(SetColorPaletteProperty, value);
			}
		}

		public setButton() {
			InitializeComponent();

		}

		public override void OnApplyTemplate() {
			base.OnApplyTemplate();

			Grid g_colorPalette = GetTemplateChild("g_colorPalette") as Grid;

			int cnt = 0;
			foreach (colorModel cm in SetColorPalette) {

				Rectangle r = new Rectangle();
				r.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
				r.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
				r.Fill = cm.ColorBrush;

				if (Orientation == System.Windows.Controls.Orientation.Horizontal) {
					ColumnDefinition rd = new ColumnDefinition();
					rd.Width = new GridLength(1, GridUnitType.Star);
					g_colorPalette.ColumnDefinitions.Add(rd);
					r.SetValue(Grid.ColumnProperty, cnt);
				}
				else {
					RowDefinition rd = new RowDefinition();
					rd.Height = new GridLength(1, GridUnitType.Star);
					g_colorPalette.RowDefinitions.Add(rd);
					r.SetValue(Grid.RowProperty, cnt);
				}

				g_colorPalette.Children.Add(r);

				cnt++;
			}



		}
	}
}
