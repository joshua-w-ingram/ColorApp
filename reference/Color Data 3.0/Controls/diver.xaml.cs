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

namespace Color_Data_3._0.Controls {
	public partial class diver : UserControl {

		public diver() {
			InitializeComponent();
			Loaded += new RoutedEventHandler(diver_Loaded);

			diver1.Completed += new EventHandler(diver1_Completed);
		}

		void diver_Loaded(object sender, RoutedEventArgs e) {
			diver1.Stop();
			diver2.Stop();
			diver1.Begin();
		}

		public void startDiving() {
			diver1.Begin();
		}

		void diver1_Completed(object sender, EventArgs e) {
			diver2.Begin();
		}
	}
}
