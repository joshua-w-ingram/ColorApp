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
	public partial class AnimatedSplashScreen : UserControl {
		public AnimatedSplashScreen() {
			InitializeComponent();
			Storyboard Cd_Animation = this.Resources["Cd_Animation"] as Storyboard;
			Cd_Animation.Begin();
		}
	}
}
