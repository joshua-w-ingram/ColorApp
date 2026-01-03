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
	public partial class pSlider : UserControl {

		//NOTE: the height of the control is hard coded because I couldn't find a way to get the control to update the lifter height based on actual height

		public event RoutedPropertyChangedEventHandler<double> ValueChanged;

		public static readonly DependencyProperty MaxProperty =
		DependencyProperty.Register("Maximum", typeof(double), typeof(pSlider), new PropertyMetadata(100.0));

		public double Maximum {
			get { return (double)GetValue(MaxProperty); }
			set { SetValue(MaxProperty, value); }
		}

		public static readonly DependencyProperty MinProperty =
		DependencyProperty.Register("Minimum", typeof(double), typeof(pSlider), new PropertyMetadata(0.0));

		public double Minimum {
			get { return (double)GetValue(MinProperty); }
			set { SetValue(MinProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty =
		DependencyProperty.Register("Value", typeof(double), typeof(pSlider), new PropertyMetadata(50.0));

		public double Value {
			get { return (double)GetValue(ValueProperty); }
			set {
				SetValue(ValueProperty, value);
				updateSliderPosition();
			}
		}

		public pSlider() {
			InitializeComponent();
			Loaded += new RoutedEventHandler(pSlider_Loaded);
			g_container.Loaded += new RoutedEventHandler(g_container_Loaded);
		}

		void g_container_Loaded(object sender, RoutedEventArgs e) {
			updateSliderPosition();
		}

		void pSlider_Loaded(object sender, RoutedEventArgs e) {

			updateSliderPosition();

		} 

		public void updateSliderPosition() {
			if (Value > Maximum)
				Value = Maximum;
			else if (Value < Minimum)
				Value = Minimum;

			double min = 0;
			double max = g_container.Height;

			r_lifter.Height = Value / (Maximum - Minimum) * (max - min);
		}

		private Point Position;

		private void LayoutRoot_MouseMove(object sender, MouseEventArgs e) {
			double oldVal = Value;

			Point newPosition = e.GetPosition((UIElement)sender);

			double delta = newPosition.Y - Position.Y;

			double temp = r_lifter.Height - delta;

			if (temp > g_container.ActualHeight)
				r_lifter.Height = g_container.ActualHeight;
			else if (temp < 0)
				r_lifter.Height = 0;
			else
				r_lifter.Height = temp;

			double min = 0;
			double max = g_container.ActualHeight;

			Value = r_lifter.Height / (max - min) * (Maximum - Minimum);
			Value = Math.Floor(Value);
			RoutedPropertyChangedEventHandler<double> handler = ValueChanged;
			if (handler != null) {
				RoutedPropertyChangedEventArgs<double> rpcea = new RoutedPropertyChangedEventArgs<double>(oldVal, Value);
				handler(this, rpcea);
			}

			Position = e.GetPosition((UIElement)sender);
		}

		private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			Position = e.GetPosition((UIElement)sender);
		}

		private void LayoutRoot_Tap(object sender, GestureEventArgs e) {
			double oldVal = Value;

			if (Value < Maximum)
				Value += 1;

			RoutedPropertyChangedEventHandler<double> handler = ValueChanged;
			if (handler != null) {
				RoutedPropertyChangedEventArgs<double> rpcea = new RoutedPropertyChangedEventArgs<double>(oldVal, Value);
				handler(this, rpcea);
			}
		}

		private void r_lifter_Tap(object sender, GestureEventArgs e) {
			double oldVal = Value;

			if (Value > Minimum)
				Value -= 1;

			RoutedPropertyChangedEventHandler<double> handler = ValueChanged;
			if (handler != null) {
				RoutedPropertyChangedEventArgs<double> rpcea = new RoutedPropertyChangedEventArgs<double>(oldVal, Value);
				handler(this, rpcea);
			}

			e.Handled = true;
		}

	}
}
