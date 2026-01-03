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
	public partial class Waiting : UserControl {


		public Waiting() {
			InitializeComponent();
		}


		public void Show() {
			sb_progressBar.Stop();
			sb_progressBar.Begin();
			Visibility = System.Windows.Visibility.Visible;

		}

		public void Hide() {
			sb_progressBar.Stop();
			Visibility = System.Windows.Visibility.Collapsed;

		}



	}
}
