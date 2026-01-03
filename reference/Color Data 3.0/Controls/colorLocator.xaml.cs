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
	public partial class colorLocator : UserControl {

		public bool isFlashing;

		public colorLocator() {
			InitializeComponent();
			DataContext = this;
			isFlashing = false;
		}

		public void beginFlash() {
			VisualStateManager.GoToState(this, "Current", false);
			VisualStateManager.GoToState(this, "NoText", false);
			flash.Begin();
			isFlashing = true;
		}

		public void beginFlash(string namedColor, double xPos, double yPos) {
			VisualStateManager.GoToState(this, "Current", false);
			tb_namedColor.Text = namedColor;
			if (string.IsNullOrEmpty(namedColor))
				VisualStateManager.GoToState(this, "NoText", false);
			else {
				if (xPos > 240) {
					if (yPos < 400)
						VisualStateManager.GoToState(this, "BottomRight", false);
					else
						VisualStateManager.GoToState(this, "TopRight", false);
				}
				else {
					if (yPos < 400)
						VisualStateManager.GoToState(this, "BottomLeft", false);
					else
						VisualStateManager.GoToState(this, "TopLeft", false);
				}
			}
			flash.Begin();
			isFlashing = true;
		}

		public void endFlash() {
			VisualStateManager.GoToState(this, "Normal", false);
			flash.Stop();
			isFlashing = false;
		}


	}
}
