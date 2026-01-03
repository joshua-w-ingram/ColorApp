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
	public partial class colorHolder : UserControl {

		public event RoutedEventHandler RemoveTap;

		public colorHolder() {
			InitializeComponent();
			ib_remove.Tap += new EventHandler<GestureEventArgs>(ib_remove_Tap);
		}

		void ib_remove_Tap(object sender, GestureEventArgs e) {
			RoutedEventHandler handler = RemoveTap;
			if (handler != null)
				handler(this, e);
		}

		public void hideRemove() {
			sb_delete.Begin();
		}


	}
}