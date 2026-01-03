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
	public partial class colorRect : Button {


		public colorRect() {
			InitializeComponent();
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
			base.OnMouseLeftButtonDown(e);

			VisualStateManager.GoToState(this, "Pressed", true);

		}


	}
}