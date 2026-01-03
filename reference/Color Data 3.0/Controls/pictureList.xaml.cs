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
using Color_Data_3._0.Classes;
using Color_Data_3._0.Classes.ColorLists;
using System.Windows.Navigation;
using Color_Data_3._0.Data;

namespace Color_Data_3._0.Controls {
	public partial class pictureList : UserControl {


		public List<setModel> sets {
			set {
				lb_setPics.ItemsSource = value;
			}
		}

		public setModel selectedSet { get; set; }

		public string Title { get; set; }

		public pictureList() {
			InitializeComponent();
		}

		private void lb_setPics_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (e.AddedItems.Count > 0)
				selectedSet = e.AddedItems[0] as setModel;
		}



	}
}
