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

namespace Color_Data_3._0.Support {
	public partial class overlay : UserControl {

		public FrameworkElement objectToHide { get; set; }

		public overlay() {
			InitializeComponent();
			Loaded += new RoutedEventHandler(overlay_Loaded);
			Unloaded += new RoutedEventHandler(overlay_Unloaded);
		}

		void overlay_Unloaded(object sender, RoutedEventArgs e) {
				objectToHide.Visibility = System.Windows.Visibility.Visible;
				showContent.Stop(); 
		}

		void overlay_Loaded(object sender, RoutedEventArgs e) {
			showContent.Stop(); 
			Storyboard.SetTarget(da_overlay, b_overlay);
			showContent.Begin();
			showContent.Completed += new EventHandler(showContent_Completed);
		}

		void showContent_Completed(object sender, EventArgs e) {
				objectToHide.Visibility = System.Windows.Visibility.Collapsed;
		}

		public void addOverlay(FrameworkElement o) {
			b_overlay.Child = o;
		}

	}
}
