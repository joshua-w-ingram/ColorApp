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
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using Color_Data_3._0.Classes;

namespace Color_Data_3._0.Controls {
	public partial class EditSet : UserControl {

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(EditSet), new PropertyMetadata(""));

		public string Title {
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public static readonly DependencyProperty DescriptionProperty =
			DependencyProperty.Register("Description", typeof(string), typeof(EditSet), new PropertyMetadata(""));

		public string Description {
			get { return (string)GetValue(DescriptionProperty); }
			set { SetValue(DescriptionProperty, value); }
		}

		public static readonly DependencyProperty ColorProperty =
			DependencyProperty.Register("currentSetColorPalette", typeof(ObservableCollection<setColorModel>), typeof(EditSet), null);

		public ObservableCollection<setColorModel> currentSetColorPalette {
			get {
				return (ObservableCollection<setColorModel>)GetValue(ColorProperty);
			}
			set {
				SetValue(ColorProperty, value);
			}
		}

		public EditSet() {
			InitializeComponent();
			DataContext = this;
			Loaded += new RoutedEventHandler(EditSet_Loaded);
		}

		void EditSet_Loaded(object sender, RoutedEventArgs e) {
			if (currentSetColorPalette != null)
				sr_setColors.setColorPalette = currentSetColorPalette;
		}


	}
}
