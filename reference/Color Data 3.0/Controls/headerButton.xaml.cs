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
	public partial class headerButton : UserControl {

		public headerButton() {
			InitializeComponent();
			IsEnabledChanged += new DependencyPropertyChangedEventHandler(headerButton_IsEnabledChanged);
		}

		void headerButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) {
			if (IsEnabled)
				VisualStateManager.GoToState(this, "Normal", false);
			else
				VisualStateManager.GoToState(this, "Disabled", false);
		}




	}
}
