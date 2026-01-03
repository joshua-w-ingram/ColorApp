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
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;

using Color_Data_3._0.Classes;
using db = Color_Data_3._0.db;
using Color_Data_3._0.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using Color_Data_3._0.Support;

namespace Color_Data_3._0.Data {
	public partial class SetData : PhoneApplicationPage {

		private enum AppBarState {
			None,
			Hide,
			Arrange,
			Edit,
			AcceptEdit
		}

		private AppBarState AppBarIcons = AppBarState.None;

		ApplicationBarIconButton abib_arrange;
		ApplicationBarIconButton abib_edit;
		ApplicationBarIconButton abib_editAccept;

		public SetData() {
			InitializeComponent();

			abib_arrange = new ApplicationBarIconButton(new Uri("/Images/light/arrange.png", UriKind.Relative));
			abib_arrange.Text = "arrange";
			abib_arrange.Click += new EventHandler(abib_arrange_Click);

			abib_edit = new ApplicationBarIconButton(new Uri("/Images/light/edit.png", UriKind.Relative));
			abib_edit.Text = "edit";
			abib_edit.Click += new EventHandler(abib_edit_Click);

			abib_editAccept = new ApplicationBarIconButton(new Uri("/Images/light/check.png", UriKind.Relative));
			abib_editAccept.Text = "accept";
			abib_editAccept.Click += new EventHandler(abib_editAccept_Click);

			if (ApplicationLicense.IsTrial) {
			}
			else {
				var parent = ad1.Parent as Grid;
				if (parent != null)
					parent.Children.Remove(ad1);
			}
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
			if (App.returnToMain) {
				NavigationService.GoBack();
				return;
			}

			if (App.currentSet == null)
				throw new InvalidCastException("the App.currentSet was null when trying to navigate to the setData page");

			setValues();
			arrangeColors();
			loadPicture();
			if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
				setEditAppBarIcons();

			setOverlayImageButtonVisibility();
		}

		private void setValues() {

			pi_set.Header = App.currentSet.Title;
			tb_desc.Text = App.currentSet.Description;
			sc_palette.setColorPalette = App.currentSet.setColorPalette;

		}

		private void arrangeColors() {

			g_arrangement.Children.Clear();

			foreach (setColorModel scm in App.currentSet.setColorPalette) {

				colorSquare cs = new colorSquare();
				cs.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
				cs.VerticalAlignment = System.Windows.VerticalAlignment.Top;
				cs.Margin = new Thickness(scm.X, scm.Y, 0, 0);
				cs.Width = scm.IconSize;
				cs.Height = scm.IconSize;
				cs.Fill = scm.ColorBrush;
				cs.Position = scm.Position + 1;
				cs.Title = scm.Title;
				cs.PositionVisible = System.Windows.Visibility.Collapsed;
				cs.SelectMode = colorSquare.SelectType.tap;
				cs.SetValue(Canvas.ZIndexProperty, scm.Z);
				cs.Tag = scm;
				cs.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(r_Tap);
				g_arrangement.Children.Add(cs);

			}
		}

		private void loadPicture() {

			if (App.currentSet.Picture != null) {

				MemoryStream ms = new MemoryStream(App.currentSet.Picture.Img);
				BitmapImage bmp = new BitmapImage();
				bmp.SetSource(ms);
				i_picture.Source = bmp;
			}
			else
				pi_picture.Visibility = System.Windows.Visibility.Collapsed;

		}

		void r_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			colorSquare r = (colorSquare)sender;
			App.currentColor = (setColorModel)r.Tag;
			NavigationService.Navigate(new Uri("/ColorData", UriKind.Relative));
		}

		private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e) {

			PanoramaItem pi = e.AddedItems[0] as PanoramaItem;
			if (pi.Equals(pi_arrange))
				setArrangeAppBarIcons();
			else if (pi.Equals(pi_set))
				setEditAppBarIcons();
			else
				clearAppBarIcons();

		}

		#region "appBars"

		private void setArrangeAppBarIcons() {

			if (AppBarIcons == AppBarState.Arrange)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_arrange);

			AppBarIcons = AppBarState.Arrange;
		}

		private void setEditAppBarIcons() {

			if (AppBarIcons == AppBarState.Edit)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_edit);

			AppBarIcons = AppBarState.Edit;
		}

		private void setEditAcceptAppBarIcons() {

			if (AppBarIcons == AppBarState.AcceptEdit)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_editAccept);

			AppBarIcons = AppBarState.AcceptEdit;
		}

		private void clearAppBarIcons() {

			if (AppBarIcons == AppBarState.None)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			//making appBar disappear and reappear has a bad animation
			//ApplicationBar.IsVisible = false;

			AppBarIcons = AppBarState.None;

		}

		private void HideAppBar() {
			if (AppBarIcons == AppBarState.Hide)
				return;

			ApplicationBar.IsVisible = false;

			AppBarIcons = AppBarState.Hide;
		}

		#endregion

		void abib_arrange_Click(object sender, EventArgs e) {
			NavigationService.Navigate(new Uri("/Arrange", UriKind.Relative));
		}

		void abib_edit_Click(object sender, EventArgs e) {
			if (App.currentSet == null)
				throw new NullReferenceException("SetModel did not return a setModel object when appbar edit icon was pushed");

			ec_editSet.currentSetColorPalette = App.currentSet.setColorPalette;
			ec_editSet.Title = App.currentSet.Title;
			ec_editSet.Description = App.currentSet.Description;

			setEditAcceptAppBarIcons();
			pano.Visibility = System.Windows.Visibility.Collapsed;

			p_editSet.IsOpen = true;
		}

		private void abib_editAccept_Click(object sender, EventArgs e) {

			if (App.currentSet == null)
				throw new NullReferenceException("App.currentSet was null when appbar editAccept icon was pushed");

			App.currentSet.Title = ec_editSet.Title;
			App.currentSet.Description = ec_editSet.Description;

			App.currentSet.saveSet();

			p_editSet.IsOpen = false;
		}

		void edit_Closed(object sender, EventArgs e) {

			pi_set.Header = App.currentSet.Title;
			tb_desc.Text = App.currentSet.Description;

			pano.Visibility = System.Windows.Visibility.Visible;
			setEditAppBarIcons();


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

			FrameworkElement oth = pano;
			o_mainPage.objectToHide = oth;
			o_mainPage.addOverlay(new setOverlay());

			HideAppBar();

			p_overlay.IsOpen = true;
		}

		void p_overlay_Closed(object sender, EventArgs e) {
			PanoramaItem pi = pano.SelectedItem as PanoramaItem;
			if (pi.Equals(pi_arrange))
				setArrangeAppBarIcons();
			else if (pi.Equals(pi_set))
				setEditAppBarIcons();
			else
				clearAppBarIcons();
		}

		protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e) {
			if (p_editSet.IsOpen) {
				e.Cancel = true;
				p_editSet.IsOpen = false;
			}
			if (p_overlay.IsOpen) {
				e.Cancel = true;
				p_overlay.IsOpen = false;
			}
		}


	}
}