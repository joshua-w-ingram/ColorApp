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

using Color_Data_3._0.Controls;
using Color_Data_3._0.Classes;
using System.Windows.Data;
using Microsoft.Phone.Shell;
using Color_Data_3._0.Support;
using System.ComponentModel;
using System.Threading;

namespace Color_Data_3._0.Data {

	public partial class SetArrangement : PhoneApplicationPage {

		public const int SNAP = 10;

		private enum AppBarState {
			None,
			Hide,
			Standard,
			SetColorSelect
		}
		private AppBarState AppBarIcons = AppBarState.None;

		public colorSquare currentSquare { get; set; }

		private ApplicationBarIconButton abib_accept;
		private ApplicationBarIconButton abib_trash;
		private ApplicationBarIconButton abib_remove;

		private bool isChanged { get; set; }

		public SetArrangement() {
			InitializeComponent();

			abib_accept = new ApplicationBarIconButton(new Uri("/Images/light/check.png", UriKind.Relative));
			abib_accept.Text = "accept";
			abib_accept.Click += new EventHandler(abib_accept_Click);

			abib_trash = new ApplicationBarIconButton(new Uri("/Images/light/trash.png", UriKind.Relative));
			abib_trash.Text = "trash";
			abib_trash.Click += new EventHandler(abib_trash_Click);

			abib_remove = new ApplicationBarIconButton(new Uri("/Images/light/removeColor.png", UriKind.Relative));
			abib_remove.Text = "remove";
			abib_remove.Click += new EventHandler(abib_remove_Click);

			this.Loaded += new RoutedEventHandler(SetArrangement_Loaded);

			if (ApplicationLicense.IsTrial) {
			}
			else {
				var parent = ad1.Parent as Grid;
				if (parent != null)
					parent.Children.Remove(ad1);
			}
		}

		void SetArrangement_Loaded(object sender, RoutedEventArgs e) {
			makeGrid();
			isChanged = false;
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
			base.OnNavigatedTo(e);

			if (App.currentSet == null)
				throw new NullReferenceException("App.currentSet was null when trying to navigate to the set arrangement page");

			setStandardAppBarIcons();

			arrangeColors();

			setOverlayImageButtonVisibility();
		}

		protected void arrangeColors() {

			if (App.currentSet.Snap) {
				((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "no snap";
				toggleGrid(true);
			}
			else {
				((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "snap to grid";
				toggleGrid(false);
			}


			foreach (setColorModel scm in App.currentSet.setColorPalette) {

				colorSquare cs = new colorSquare();
				cs.Tag = scm;
				cs.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
				cs.VerticalAlignment = System.Windows.VerticalAlignment.Top;

				cs.Margin = new Thickness(scm.X, scm.Y, 0, 0);

				cs.Width = scm.IconSize;
				cs.Height = scm.IconSize;

				cs.Fill = scm.ColorBrush;
				cs.Position = scm.Position + 1;
				cs.Title = scm.Title;
				cs.PositionVisible = System.Windows.Visibility.Visible;
				cs.SetValue(Canvas.ZIndexProperty, scm.Z);

				cs.MouseLeftButtonDown += new MouseButtonEventHandler(ColorSquare_MouseLeftButtonDown);
				cs.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(ColorSquare_Tap);
				cs.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(ColorSquare_ManipulationStarted);
				cs.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(ColorSquare_ManipulationDelta);

				cs.RenderTransform = new ScaleTransform();
				var gl = GestureService.GetGestureListener(cs);
				gl.PinchStarted += new EventHandler<PinchStartedGestureEventArgs>(gl_PinchStarted);
				gl.PinchDelta += new EventHandler<PinchGestureEventArgs>(gl_PinchDelta);
				g_ContentPanel.Children.Add(cs);

			}
		}

		protected void makeGrid() {

			for (int x = 10; x < 490; x += 20) {

				LineGeometry lineGeometry1 = new LineGeometry();
				lineGeometry1.StartPoint = new Point(0, 0);
				lineGeometry1.EndPoint = new Point(0, 1);

				GeometryGroup geometry1 = new GeometryGroup();
				geometry1.Children.Add(lineGeometry1);

				Path path1 = new Path();
				path1.Stroke = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
				path1.Stretch = Stretch.Fill;
				path1.StrokeThickness = 1.0;
				path1.Margin = new Thickness(x, 0, 0, 0);
				path1.Data = geometry1;
				path1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

				g_grid.Children.Add(path1);
			}

			for (int y = 0; y < 600; y += 20) {

				LineGeometry lineGeometry1 = new LineGeometry();
				lineGeometry1.StartPoint = new Point(0, 0);
				lineGeometry1.EndPoint = new Point(1, 0);

				GeometryGroup geometry1 = new GeometryGroup();
				geometry1.Children.Add(lineGeometry1);

				Path path1 = new Path();
				path1.Stroke = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
				path1.Stretch = Stretch.Fill;
				path1.StrokeThickness = 1.0;
				path1.Margin = new Thickness(10, y, 0, 0);
				path1.Data = geometry1;
				path1.VerticalAlignment = System.Windows.VerticalAlignment.Top;

				g_grid.Children.Add(path1);

			}
		}

		private void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			foreach (UIElement iue in g_ContentPanel.Children) {
				colorSquare _cs = iue as colorSquare;
				if (_cs == null)
					continue;
				VisualStateManager.GoToState(_cs, "Unselected", true);
			}
			setStandardAppBarIcons();
		}

		private void ColorSquare_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			isChanged = true;

			currentSquare = (colorSquare)sender;
			foreach (UIElement iue in g_ContentPanel.Children) {
				colorSquare _cs = iue as colorSquare;
				if (_cs == null)
					continue;
				if (!_cs.Equals(sender))
					VisualStateManager.GoToState(_cs, "Unselected", true);
			}

			setSetColorSelectAppBarIcons();

			e.Handled = true;
		}

		private void ColorSquare_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			e.Handled = true;
		}

		private void ColorSquare_Hold(object sender, System.Windows.Input.GestureEventArgs e) {
			e.Handled = true;
		}

		double dx_temp = 0;
		double dy_temp = 0;

		private void ColorSquare_ManipulationStarted(object sender, ManipulationStartedEventArgs e) {
			isChanged = true;
			colorSquare currentColorSquare = sender as colorSquare;
			setColorModel currentSetColorModel = (setColorModel)currentColorSquare.Tag;

			App.currentSet.setColorZFirst(currentSetColorModel);

			foreach (UIElement iue in g_ContentPanel.Children) {
				colorSquare cs = iue as colorSquare;
				if (cs == null)
					continue;
				setColorModel scm = (setColorModel)cs.Tag;
				cs.SetValue(Canvas.ZIndexProperty, scm.Z);

			}

			dx_temp = currentColorSquare.Margin.Left;
			dy_temp = currentColorSquare.Margin.Top;

			e.Handled = true;
		}

		private void ColorSquare_ManipulationDelta(object sender, ManipulationDeltaEventArgs e) {
			colorSquare currentColorSquare = sender as colorSquare;

			dx_temp = dx_temp + e.DeltaManipulation.Translation.X;
			dy_temp = dy_temp + e.DeltaManipulation.Translation.Y;


			double dx;
			double dy;

			if (App.currentSet.Snap) {
				dx = Math.Round(dx_temp / SNAP, 0) * SNAP;
				dy = Math.Round(dy_temp / SNAP, 0) * SNAP;
			}
			else {
				dx = dx_temp; dy = dy_temp;
			}

			if (dx < 0)
				dx = 0;
			if (dx > g_ContentPanel.ActualWidth - currentColorSquare.Width)
				dx = g_ContentPanel.ActualWidth - currentColorSquare.Width;

			if (dy < 0)
				dy = 0;
			if (dy > g_ContentPanel.ActualHeight - currentColorSquare.Height)
				dy = g_ContentPanel.ActualHeight - currentColorSquare.Height;


			if (App.currentSet.Snap) {
				double snap = 10;
				dx = Math.Round(dx / snap, 0) * snap;
				dy = Math.Round(dy / snap, 0) * snap;
			}

			currentColorSquare.Margin = new Thickness(dx, dy, currentColorSquare.Margin.Right, currentColorSquare.Margin.Bottom);

			setColorModel scm = currentColorSquare.Tag as setColorModel;
			scm.X = dx;
			scm.Y = dy;


			App.currentSet.setColorOrder();
			foreach (UIElement iue in g_ContentPanel.Children) {
				colorSquare _cs = iue as colorSquare;
				if (_cs == null)
					continue;
				setColorModel _sc = (setColorModel)_cs.Tag;
				_cs.Position = _sc.Position + 1;
			}

			e.Handled = true;
		}

		double intialSize;
		void gl_PinchStarted(object sender, PinchStartedGestureEventArgs e) {
			isChanged = true;
			intialSize = currentSquare.Width;
		}

		void gl_PinchDelta(object sender, PinchGestureEventArgs e) {

			double val = intialSize * e.DistanceRatio;

			if (App.currentSet.Snap)
				val = Math.Round(val / SNAP, 0) * SNAP;

			if (val > 200) {
				val = 200;
			}
			if (val < 90) {
				val = 90;
			}

			currentSquare.Width = currentSquare.Height = (int)val;
			((setColorModel)currentSquare.Tag).IconSize = (int)val;
		}

		#region "appBars"

		private void setStandardAppBarIcons() {

			if (AppBarIcons == AppBarState.Standard)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_accept);

			AppBarIcons = AppBarState.Standard;
		}

		private void setSetColorSelectAppBarIcons() {

			if (AppBarIcons == AppBarState.SetColorSelect)
				return;

			if (g_ContentPanel.Children.Count <= 1)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_accept);

			ApplicationBar.Buttons.Add(abib_trash);
			ApplicationBar.Buttons.Add(abib_remove);

			AppBarIcons = AppBarState.SetColorSelect;
		}

		private void HideAppBar() {
			if (AppBarIcons == AppBarState.Hide)
				return;

			ApplicationBar.IsVisible = false;

			AppBarIcons = AppBarState.Hide;
		}

		#endregion

		//void abib_shrink_Click(object sender, EventArgs e) {
		//      setColorModel scm = (setColorModel)currentSquare.Tag;
		//      //WARNING: fixed values
		//      currentSquare.Width = currentSquare.Height = scm.decreaseIconSize(5);
		//}

		//void abib_grow_Click(object sender, EventArgs e) {
		//      setColorModel scm = (setColorModel)currentSquare.Tag;
		//      //WARNING: fixed values
		//      currentSquare.Width = currentSquare.Height = scm.IncreaseIconSize(5, (int)g_ContentPanel.ActualWidth, (int)g_ContentPanel.ActualHeight);
		//      currentSquare.Margin = new Thickness(scm.X, scm.Y, 0, 0);
		//}

		void abib_trash_Click(object sender, EventArgs e) {
			MessageBoxResult mbr = MessageBox.Show("Are you sure you want to trash this color? This cannot be undone.", "Trash Color", MessageBoxButton.OKCancel);
			if (mbr == MessageBoxResult.Cancel)
				return;
			else {
				App.currentSet.setColorPalette.Remove((setColorModel)currentSquare.Tag);
				setColorModel.deleteSetColor((setColorModel)currentSquare.Tag);
				setColorModel.deleteColor((setColorModel)currentSquare.Tag);
				g_ContentPanel.Children.Remove(currentSquare);
				currentSquare = null;
			}
		}

		void abib_remove_Click(object sender, EventArgs e) {
			MessageBoxResult mbr = MessageBox.Show("Are you sure you want to move this color out of the set? This cannot be undone.", "Remove Color", MessageBoxButton.OKCancel);
			if (mbr == MessageBoxResult.Cancel)
				return;
			else {
				App.currentSet.setColorPalette.Remove((setColorModel)currentSquare.Tag);
				setColorModel.deleteSetColor((setColorModel)currentSquare.Tag);
				g_ContentPanel.Children.Remove(currentSquare);
				currentSquare = null;
			}
		}

		void abib_accept_Click(object sender, EventArgs e) {

			if (isChanged) {
				w_waiting.Show();
				BackgroundWorker bw = new BackgroundWorker();

				bw.DoWork += ((s, args) => {
					Thread.Sleep(2000);
					this.Dispatcher.BeginInvoke(() => {
						App.currentSet.saveSet();

					});
				});

				bw.RunWorkerCompleted += ((s, args) => {
					this.Dispatcher.BeginInvoke(() => {
						w_waiting.Hide();
						NavigationService.GoBack();
					});
				});

				bw.RunWorkerAsync();

			}
			else
				NavigationService.GoBack();

		}

		private void abmi_Snap_Click(object sender, EventArgs e) {
			isChanged = true;
			App.currentSet.Snap = !App.currentSet.Snap;

			if (App.currentSet.Snap) {
				((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "No Snap";
				toggleGrid(true);
			}
			else {
				((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "Snap";
				toggleGrid(false);
			}

		}

		private void abmi_Grid_Click(object sender, EventArgs e) {
			isChanged = true;
			if (g_grid.Visibility == System.Windows.Visibility.Collapsed) {
				g_grid.Visibility = System.Windows.Visibility.Visible;
				((ApplicationBarMenuItem)ApplicationBar.MenuItems[1]).Text = "Hide Grid";
			}
			else {

				g_grid.Visibility = System.Windows.Visibility.Collapsed;
				((ApplicationBarMenuItem)ApplicationBar.MenuItems[1]).Text = "Show Grid";
			}
		}

		private void toggleGrid(bool turnOn) {
			isChanged = true;
			if (turnOn) {
				g_grid.Visibility = System.Windows.Visibility.Visible;
				((ApplicationBarMenuItem)ApplicationBar.MenuItems[1]).Text = "Hide Grid";
			}
			else {
				g_grid.Visibility = System.Windows.Visibility.Collapsed;
				((ApplicationBarMenuItem)ApplicationBar.MenuItems[1]).Text = "Show Grid";
			}
		}

		private void setOverlayImageButtonVisibility() {
			switch (StorageHelper.loadSetting<App.helpVisibilityType>("helpVisibilityOptions")) {
				case App.helpVisibilityType.hide:
					ib_overlay.Visibility = System.Windows.Visibility.Collapsed;
					break;
				case App.helpVisibilityType.show:
					ib_overlay.Visibility = System.Windows.Visibility.Visible;
					break;
			}
		}

		private void ib_overlay_Tap(object sender, System.Windows.Input.GestureEventArgs e) {

			FrameworkElement oth = g_ContentPanel;
			o_mainPage.objectToHide = oth;
			o_mainPage.addOverlay(new arrangementOverlay());

			HideAppBar();

			p_overlay.IsOpen = true;
			e.Handled = true;
		}

		void p_overlay_Closed(object sender, EventArgs e) {
			setStandardAppBarIcons();
		}

		protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e) {
			if (p_overlay.IsOpen) {
				e.Cancel = true;
				p_overlay.IsOpen = false;
			}
			else if (isChanged) {
				MessageBoxResult mbr = MessageBox.Show("Do you want to discard all changes?", "Go Back", MessageBoxButton.OKCancel);

				if (mbr == MessageBoxResult.Cancel)
					e.Cancel = true;
				else
					App.currentSet.LoadSetData(App.currentSet.Id);
			}
		}

		private void abmi_autoArrange_Click(object sender, EventArgs e) {

			foreach (UIElement iue in g_ContentPanel.Children) {
				colorSquare cs = iue as colorSquare;
				if (cs == null)
					continue;

				setColorModel scm = cs.Tag as setColorModel;

				scm.IconSize = setColorModel.INITIAL_RECT_SIZE;
				scm.X = Math.Floor(scm.Position / setColorModel.MAX_ROWS) * (setColorModel.INITIAL_RECT_SIZE + 10);
				scm.Y = (scm.Position % setColorModel.MAX_ROWS) * (setColorModel.INITIAL_RECT_SIZE + 10);
				scm.Z = scm.Position;
				cs.Width = scm.IconSize;
				cs.Height = scm.IconSize;
				cs.Margin = new Thickness(scm.X, scm.Y, 0, 0);
				cs.SetValue(Canvas.ZIndexProperty, scm.Z);
			}
		}

	}
}