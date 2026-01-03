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
using Microsoft.Phone.Controls;
using Color_Data_3._0.Classes.ColorClasses;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Tasks;

namespace Color_Data_3._0.Selectors {
	public partial class ColorSelector : UserControl {

		public static Color StartColor { get; set; }
		public Color selectedColor { get; set; }

		public ColorSelector() {
			InitializeComponent();
			Loaded += new RoutedEventHandler(ManualSelector_Loaded);
		}

		void ManualSelector_Loaded(object sender, RoutedEventArgs e) {
			ps_rgb_R.Value = StartColor.R;
			ps_rgb_G.Value = StartColor.G;
			ps_rgb_B.Value = StartColor.B;
			tb_rgb_R_value.Text = StartColor.R.ToString();
			tb_rgb_G_value.Text = StartColor.G.ToString();
			tb_rgb_B_value.Text = StartColor.B.ToString();

		
			setBackground();
			setRed();
			setGreen();
			setBlue();
		}

		private void ps_rgb_R_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			setRed();
			setGreen();
			setBlue();
			setBackground();
		}

		private void ps_rgb_G_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			setRed();
			setGreen();
			setBlue();
			setBackground();
		}

		private void ps_rgb_B_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			setRed();
			setGreen();
			setBlue();
			setBackground();
		}

		//TODO?
		private void ps_hsl_H_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {

		}

		private void ps_hsl_S_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {

		}

		private void ps_hsl_L_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {

		}

		private void setBackground() {
			if (ps_rgb_R == null || ps_rgb_G == null || ps_rgb_B == null)
				return;

			Color c = Color.FromArgb(255, (byte)ps_rgb_R.Value, (byte)ps_rgb_G.Value, (byte)ps_rgb_B.Value);
			SolidColorBrush scb = new SolidColorBrush(c);
			ContentPanel.Background = scb;
			ps_rgb_R.Foreground = scb;
			ps_rgb_G.Foreground = scb;
			ps_rgb_B.Foreground = scb;

			tb_rgb_R_value.Text = ps_rgb_R.Value.ToString("0");
			tb_rgb_G_value.Text = ps_rgb_G.Value.ToString("0");
			tb_rgb_B_value.Text = ps_rgb_B.Value.ToString("0");

			HSL hsl = HSL.convertRGB(c);

			if (hsl.L < 0.25) {
				double tL = (0.25 - hsl.L) * 2;
				HSL rectHSLs = new HSL(0, 0, tL);
				r1.Fill = new SolidColorBrush(rectHSLs.toRGB());
				r2.Fill = new SolidColorBrush(rectHSLs.toRGB());
				r3.Fill = new SolidColorBrush(rectHSLs.toRGB());
			}
		}

		private void setRed() {
			if (ps_rgb_R == null || ps_rgb_G == null || ps_rgb_B == null)
				return;
			GradientStopCollection gsc = new GradientStopCollection();
			GradientStop gs1 = new GradientStop();
			gs1.Color = Color.FromArgb(255, 255, (byte)ps_rgb_G.Value, (byte)ps_rgb_B.Value);
			gs1.Offset = 0;
			GradientStop gs2 = new GradientStop();
			gs2.Color = Color.FromArgb(255, 0, (byte)ps_rgb_G.Value, (byte)ps_rgb_B.Value);
			gs2.Offset = 1;
			gsc.Add(gs1);
			gsc.Add(gs2);

			LinearGradientBrush lgb = new LinearGradientBrush();
			lgb.GradientStops = gsc;

			lgb.StartPoint = new Point(0, 0);
			lgb.EndPoint = new Point(0, 1);

			ps_rgb_R.Background = lgb;
		}

		private void setGreen() {
			if (ps_rgb_R == null || ps_rgb_G == null || ps_rgb_B == null)
				return;
			GradientStopCollection gsc = new GradientStopCollection();
			GradientStop gs1 = new GradientStop();
			gs1.Color = Color.FromArgb(255, (byte)ps_rgb_R.Value, 255, (byte)ps_rgb_B.Value);
			gs1.Offset = 0;
			GradientStop gs2 = new GradientStop();
			gs2.Color = Color.FromArgb(255, (byte)ps_rgb_R.Value, 0, (byte)ps_rgb_B.Value);
			gs2.Offset = 1;
			gsc.Add(gs1);
			gsc.Add(gs2);

			LinearGradientBrush lgb = new LinearGradientBrush();
			lgb.GradientStops = gsc;

			lgb.StartPoint = new Point(0, 0);
			lgb.EndPoint = new Point(0, 1);

			ps_rgb_G.Background = lgb;
		}

		private void setBlue() {
			if (ps_rgb_R == null || ps_rgb_G == null || ps_rgb_B == null)
				return;
			GradientStopCollection gsc = new GradientStopCollection();
			GradientStop gs1 = new GradientStop();
			gs1.Color = Color.FromArgb(255, (byte)ps_rgb_R.Value, (byte)ps_rgb_G.Value, 255);
			gs1.Offset = 0;
			GradientStop gs2 = new GradientStop();
			gs2.Color = Color.FromArgb(255, (byte)ps_rgb_R.Value, (byte)ps_rgb_G.Value, 0);
			gs2.Offset = 1;
			gsc.Add(gs1);
			gsc.Add(gs2);

			LinearGradientBrush lgb = new LinearGradientBrush();
			lgb.GradientStops = gsc;

			lgb.StartPoint = new Point(0, 0);
			lgb.EndPoint = new Point(0, 1);

			ps_rgb_B.Background = lgb;
		}

		private void b_accept_Click(object sender, RoutedEventArgs e) {
			CameraLibrarySelector.ManualSelectorColor = (Color)ContentPanel.Background.GetValue(SolidColorBrush.ColorProperty);
			StartColor = Colors.Black;
			Popup p = Parent as Popup;
			if (p != null) {
				p.Tag = TaskResult.OK;
				p.IsOpen = false;
			}
		}

	}
}