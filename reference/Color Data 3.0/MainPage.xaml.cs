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
using Color_Data_3._0.Controls;
using Color_Data_3._0.Classes;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Threading;
using Color_Data_3._0.Selectors;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Text;
using Color_Data_3._0.Support;


namespace Color_Data_3._0 {
	public partial class MainPage : PhoneApplicationPage {

		List<CheckBox> checkBoxes_colors = new List<CheckBox>();
		List<CheckBox> checkBoxes_sets = new List<CheckBox>();

		private MainViewModel viewModel = null;
		public MainViewModel ViewModel {
			get {
				// Delay creation of the view model until necessary
				if (viewModel == null)
					viewModel = new MainViewModel();
				return viewModel;
			}
		}

		private enum AppBarState {
			None,
			Standard,
			ColorSelect,
			SetSelect,
			ColorSelect_Multi,
			SetSelect_Multi,
			EditColor,
			EditSet,
			SelectPicture,
			Hide
		}
		private AppBarState AppBarIcons = AppBarState.None;

		private ApplicationBarIconButton abib_pickColor;
		private ApplicationBarIconButton abib_library;
		private ApplicationBarIconButton abib_camera;
		private ApplicationBarIconButton abib_editColor;
		private ApplicationBarIconButton abib_deleteColor;
		private ApplicationBarIconButton abib_addToSet;
		private ApplicationBarIconButton abib_editSet;
		private ApplicationBarIconButton abib_mergeSet;
		private ApplicationBarIconButton abib_deleteSet;
		private ApplicationBarIconButton abib_removeColorsFromSet;
		private ApplicationBarIconButton abib_editColorAccept;
		private ApplicationBarIconButton abib_editSetAccept;
		private ApplicationBarIconButton abib_selectPictureAccept;

		BackgroundWorker backgroundWorker;
		Popup myPopup;

		public MainPage() {
			InitializeComponent();

			createAppBarIcons();
			if (ApplicationLicense.IsTrial) {
				((ApplicationBarMenuItem)ApplicationBar.MenuItems[1]).IsEnabled = false;
			}
			else {
				var parent = ad1.Parent as Grid;
				if (parent != null)
					parent.Children.Remove(ad1);
			}

			switch (StorageHelper.loadSetting<App.splashVisibilityType>("splashVisibilityOptions")) {
				case App.splashVisibilityType.show:
					myPopup = new Popup() { IsOpen = true, Child = new AnimatedSplashScreen() };
					RunBackgroundWorker();
					break;
				case App.splashVisibilityType.hide:
					ApplicationBar.IsVisible = true;

					refreshLists();
					setPanoramaItems();
					setStandardAppBarIcons();

					App.isFirstLoad = false;
					break;
			}


		}

		private void createAppBarIcons() {

			abib_pickColor = new ApplicationBarIconButton(new Uri("/Images/light/paintbrush.png", UriKind.Relative));
			abib_pickColor.Text = "Manual";
			abib_pickColor.Click += new EventHandler(abib_pickColor_Click);

			abib_library = new ApplicationBarIconButton(new Uri("/Images/light/folder.png", UriKind.Relative));
			abib_library.Text = "Library";
			abib_library.Click += new EventHandler(abib_library_Click);

			abib_camera = new ApplicationBarIconButton(new Uri("/Images/light/camera.png", UriKind.Relative));
			abib_camera.Text = "Camera";
			abib_camera.Click += new EventHandler(abib_camera_Click);

			abib_editColor = new ApplicationBarIconButton(new Uri("/Images/light/edit.png", UriKind.Relative));
			abib_editColor.Text = "Edit Color";
			abib_editColor.Click += new EventHandler(abib_editColor_Click);

			abib_deleteColor = new ApplicationBarIconButton(new Uri("/Images/light/trash.png", UriKind.Relative));
			abib_deleteColor.Text = "Trash";
			abib_deleteColor.Click += new EventHandler(abib_deleteColor_Click);

			abib_addToSet = new ApplicationBarIconButton(new Uri("/Images/light/addSet.png", UriKind.Relative));
			abib_addToSet.Text = "Add To Set";
			abib_addToSet.Click += new EventHandler(abib_addToSet_Click);

			abib_editSet = new ApplicationBarIconButton(new Uri("/Images/light/edit.png", UriKind.Relative));
			abib_editSet.Text = "Edit Set";
			abib_editSet.Click += new EventHandler(abib_editSet_Click);

			abib_deleteSet = new ApplicationBarIconButton(new Uri("/Images/light/trash.png", UriKind.Relative));
			abib_deleteSet.Text = "Trash";
			abib_deleteSet.Click += new EventHandler(abib_deleteSet_Click);

			abib_removeColorsFromSet = new ApplicationBarIconButton(new Uri("/Images/light/removeColor.png", UriKind.Relative));
			abib_removeColorsFromSet.Text = "Remove";
			abib_removeColorsFromSet.Click += new EventHandler(abib_removeColorsFromSet_Click);

			abib_mergeSet = new ApplicationBarIconButton(new Uri("/Images/light/merge.png", UriKind.Relative));
			abib_mergeSet.Text = "Merge Sets";
			abib_mergeSet.Click += new EventHandler(abib_MergeSet_Click);

			abib_editColorAccept = new ApplicationBarIconButton(new Uri("/Images/light/check.png", UriKind.Relative));
			abib_editColorAccept.Text = "accept";
			abib_editColorAccept.Click += new EventHandler(abib_AcceptEditColor_Click);

			abib_editSetAccept = new ApplicationBarIconButton(new Uri("/Images/light/check.png", UriKind.Relative));
			abib_editSetAccept.Text = "accept";
			abib_editSetAccept.Click += new EventHandler(abib_AcceptEditSet_Click);

			abib_selectPictureAccept = new ApplicationBarIconButton(new Uri("/Images/light/check.png", UriKind.Relative));
			abib_selectPictureAccept.Text = "accept";
			abib_selectPictureAccept.Click += new EventHandler(abib_selectPictureAccept_Click);

		}

		private void RunBackgroundWorker() {
			backgroundWorker = new BackgroundWorker();

			backgroundWorker.DoWork += ((s, args) => {
				Thread.Sleep(3500);
			});

			backgroundWorker.RunWorkerCompleted += ((s, args) => {
				this.Dispatcher.BeginInvoke(() => {

					if (ApplicationLicense.IsTrial) {
						//no setup
					}
					else {
						if (string.IsNullOrEmpty(App.uid) && StorageHelper.loadSetting<App.webCommType>("webCommOptions") != App.webCommType.disabled) {
							NavigationService.Navigate(new Uri("/Setup", UriKind.Relative));
							this.myPopup.IsOpen = false;
							App.isFirstLoad = false;
							return;
						}
					}

					this.myPopup.IsOpen = false;
					ApplicationBar.IsVisible = true;

					refreshLists();

					setPanoramaItems();

					setStandardAppBarIcons();
					App.isFirstLoad = false;

				}
			  );
			});
			backgroundWorker.RunWorkerAsync();
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
			App.returnToMain = false;

			if (App.isFirstLoad)
				return;

			if (ApplicationLicense.IsTrial) {
				//no setup
			}
			else {
				if (string.IsNullOrEmpty(App.uid) && !App.disableSetup && StorageHelper.loadSetting<App.webCommType>("webCommOptions") != App.webCommType.disabled)
					NavigationService.Navigate(new Uri("/Setup", UriKind.Relative));
			}
			refreshLists();

			setPanoramaItems();

			setStandardAppBarIcons();

		}

		private void setPanoramaItems() {
			if (ViewModel.setItems.Count == 0 && ViewModel.colorItems.Count == 0) {
				pi_info.Visibility = System.Windows.Visibility.Visible;
				ib_overlay.Visibility = System.Windows.Visibility.Collapsed;
			}
			else {
				pi_info.Visibility = System.Windows.Visibility.Collapsed;
				setOverlayImageButtonVisibility();
			}

			if (ViewModel.setItems.Count == 0)
				pi_sets.Visibility = System.Windows.Visibility.Collapsed;
			else
				pi_sets.Visibility = System.Windows.Visibility.Visible;
			if (ViewModel.colorItems.Count == 0)
				pi_colors.Visibility = System.Windows.Visibility.Collapsed;
			else
				pi_colors.Visibility = System.Windows.Visibility.Visible;
		}

		private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e) {

			if (lb_colors.SelectedItems.Count > 0)
				lb_colors.SelectedItems.Clear();
			if (lb_sets.SelectedItems.Count > 0)
				lb_sets.SelectedItems.Clear();

		}

		#region "color events"

		private void checkBox_colors_Loaded(object sender, RoutedEventArgs e) {
			CheckBox cb = sender as CheckBox;
			checkBoxes_colors.Add(cb);
		}

		private void cb1_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			colorButton cb = sender as colorButton;
			App.currentColor = colorModel.getColor((Guid)cb.Tag);
			NavigationService.Navigate(new Uri("/Data/ColorData.xaml", UriKind.Relative));
			e.Handled = true;
		}

		private void lb_colors_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			ListBox lb = sender as ListBox;

			if (lb.SelectedItems.Count == 0)
				setStandardAppBarIcons();
			else if (lb.SelectedItems.Count == 1)
				setColorSelectedAppBarIcons();
			else {
				if (lb.SelectedItems.OfType<colorModel>().Count() <= setModel.MAX_COLORS)
					setColorSelected_MultiAppBarIcons();
				else
					setNoneIcons();
			}
		}

		#endregion

		#region "set events"

		private void checkBox_sets_Loaded(object sender, RoutedEventArgs e) {
			//CheckBox cb = sender as CheckBox;
			//checkBoxes_sets.Add(cb);
			//VisualStateManager.GoToState(cb, "Indeterminate", false);
		}

		private void sb1_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			setButton sb = sender as setButton;
			App.currentSet = setModel.getSet((Guid)sb.Tag);
			NavigationService.Navigate(new Uri("/Data/SetData.xaml", UriKind.Relative));
			e.Handled = true;
		}

		private void lb_sets_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			ListBox lb = sender as ListBox;

			if (lb.SelectedItems.Count == 0)
				setStandardAppBarIcons();
			else if (lb.SelectedItems.Count == 1)
				setSetSelectedAppBarIcons();
			else {
				if (lb.SelectedItems.Count > 2)
					setNoneIcons();
				else {
					int cnt = 0;
					foreach (var item in lb.SelectedItems.OfType<setModel>()) {
						cnt += item.setColorPalette.Count;
					}
					if (cnt <= setModel.MAX_COLORS)
						setSetSelected_MultiAppBarIcons();
					else
						setNoneIcons();
				}
			}

		}

		#endregion

		#region "appBars"

		private void setStandardAppBarIcons() {

			if (AppBarIcons == AppBarState.Standard)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			//ApplicationBar.Buttons.Add(abib_help);
			ApplicationBar.Buttons.Add(abib_pickColor);
			ApplicationBar.Buttons.Add(abib_library);
			ApplicationBar.Buttons.Add(abib_camera);

			AppBarIcons = AppBarState.Standard;
		}

		private void setColorSelectedAppBarIcons() {

			if (AppBarIcons == AppBarState.ColorSelect)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_editColor);
			ApplicationBar.Buttons.Add(abib_addToSet);
			ApplicationBar.Buttons.Add(abib_deleteColor);

			AppBarIcons = AppBarState.ColorSelect;

		}

		private void setColorSelected_MultiAppBarIcons() {

			if (AppBarIcons == AppBarState.ColorSelect_Multi)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_addToSet);
			ApplicationBar.Buttons.Add(abib_deleteColor);

			AppBarIcons = AppBarState.ColorSelect_Multi;

		}

		private void setSetSelectedAppBarIcons() {

			if (AppBarIcons == AppBarState.SetSelect)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_editSet);
			ApplicationBar.Buttons.Add(abib_deleteSet);
			ApplicationBar.Buttons.Add(abib_removeColorsFromSet);

			AppBarIcons = AppBarState.SetSelect;

		}

		private void setSetSelected_MultiAppBarIcons() {

			if (AppBarIcons == AppBarState.SetSelect_Multi)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_mergeSet);

			AppBarIcons = AppBarState.SetSelect_Multi;

		}

		private void setEditColorIcons() {

			if (AppBarIcons == AppBarState.EditColor)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_editColorAccept);

			AppBarIcons = AppBarState.EditColor;

		}

		private void setEditSetIcons() {

			if (AppBarIcons == AppBarState.EditSet)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_editSetAccept);

			AppBarIcons = AppBarState.EditSet;

		}

		private void setSelectPictureIcons() {

			if (AppBarIcons == AppBarState.SelectPicture)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_selectPictureAccept);

			AppBarIcons = AppBarState.SelectPicture;

		}

		private void setNoneIcons() {

			if (AppBarIcons == AppBarState.None)
				return;

			ApplicationBar.Buttons.Clear();

			AppBarIcons = AppBarState.None;

		}

		private void hideAppBarIcons() {

			if (AppBarIcons == AppBarState.Hide)
				return;

			ApplicationBar.Buttons.Clear();
			ApplicationBar.IsVisible = false;

			AppBarIcons = AppBarState.Hide;
		}

		#endregion

		#region "appBars events"

		private void abib_addToSet_Click(object sender, EventArgs e) {

			w_waiting.Show();
			BackgroundWorker bw = new BackgroundWorker();

			bw.DoWork += ((s, args) => {
				Thread.Sleep(2000);
				this.Dispatcher.BeginInvoke(() => {

					setModel sm = setModel.createEmptySet();
					StringBuilder sb = new StringBuilder();
					ObservableCollection<setColorModel> palette = new ObservableCollection<setColorModel>();

					int i = 0;
					int nameCnt = 0;
					foreach (var cm in lb_colors.SelectedItems.OfType<colorModel>()) {
						if (!string.IsNullOrEmpty(cm.Title)) {
							if (string.IsNullOrEmpty(sb.ToString()))
								sb.Append(cm.Title);
							else {
								if (nameCnt > 2)
									sb.Append(" + " + cm.Title);
								else if (nameCnt == 2)
									sb.Append(" + ...");
							}
							nameCnt++;
						}

						palette.Add(setColorModel.ConvertColorModel(cm, sm, i));
						i++;

					}
					sm.Title = sb.ToString();
					sm.setColorPalette = palette;
					sm.saveSet();
				});

			});

			bw.RunWorkerCompleted += ((s, args) => {
				this.Dispatcher.BeginInvoke(() => {
					refreshLists();

					setPanoramaItems();

					w_waiting.Hide();
				});
			});

			bw.RunWorkerAsync();
		}

		private void abib_deleteColor_Click(object sender, EventArgs e) {

			MessageBoxResult mbr = MessageBox.Show("Do you want to delete the selected colors?", "Trash Colors", MessageBoxButton.OKCancel);

			if (mbr == MessageBoxResult.Cancel)
				return;

			foreach (var item in lb_colors.SelectedItems) {
				colorModel cm = item as colorModel;
				colorModel.deleteColor(cm);
			}

			refreshLists();
			setPanoramaItems();

		}

		private void abib_editColor_Click(object sender, EventArgs e) {

			colorModel cm = lb_colors.SelectedItem as colorModel;

			if (cm == null)
				throw new NullReferenceException("lb_colors.SelectedItem was not a colorModel object when appbar edit icon was pushed");

			ec_editColor.Background = cm.ColorBrush;
			ec_editColor.Title = cm.Title;
			ec_editColor.Description = cm.Description;
			ec_editColor.Tag = cm.Id;

			setEditColorIcons();

			p_pano.Visibility = System.Windows.Visibility.Collapsed;
			p_editColor.IsOpen = true;

		}

		private void abib_editSet_Click(object sender, EventArgs e) {
			setModel sm = lb_sets.SelectedItem as setModel;

			if (sm == null)
				throw new NullReferenceException("lb_sets.SelectedItem was not a setModel object when appbar edit icon was pushed");

			ec_editSet.currentSetColorPalette = sm.setColorPalette;
			ec_editSet.Title = sm.Title;
			ec_editSet.Description = sm.Description;
			ec_editSet.Tag = sm.Id;
			setEditSetIcons();

			p_pano.Visibility = System.Windows.Visibility.Collapsed;
			p_editSet.IsOpen = true;
		}

		private void abib_deleteSet_Click(object sender, EventArgs e) {

			MessageBoxResult mbr = MessageBox.Show("Do you want to delete the selected set?", "Trash Set", MessageBoxButton.OKCancel);

			if (mbr == MessageBoxResult.Cancel)
				return;

			w_waiting.Show();
			BackgroundWorker bw = new BackgroundWorker();

			bw.DoWork += ((s, args) => {
				Thread.Sleep(2000);
				this.Dispatcher.BeginInvoke(() => {
					foreach (var item in lb_sets.SelectedItems) {
						setModel sm = item as setModel;
						setModel.deleteSetAndColors(sm);
					}
				});
			});

			bw.RunWorkerCompleted += ((s, args) => {
				this.Dispatcher.BeginInvoke(() => {
					refreshLists();
					setPanoramaItems();
					w_waiting.Hide();
				});
			});

			bw.RunWorkerAsync();

		}

		private void abib_removeColorsFromSet_Click(object sender, EventArgs e) {

			MessageBoxResult mbr = MessageBox.Show("Do you want to delete the selected Set and move its colors to your color list? You will lose any picture attached to the Set.", "Trash Set/Keep Colors", MessageBoxButton.OKCancel);

			if (mbr == MessageBoxResult.Cancel)
				return;

			w_waiting.Show();
			BackgroundWorker bw = new BackgroundWorker();

			bw.DoWork += ((s, args) => {
				Thread.Sleep(2000);
				this.Dispatcher.BeginInvoke(() => {
					foreach (var item in lb_sets.SelectedItems) {
						setModel sm = item as setModel;
						setModel.deleteSet(sm);
					}
				});
			});

			bw.RunWorkerCompleted += ((s, args) => {
				this.Dispatcher.BeginInvoke(() => {
					refreshLists();
					setPanoramaItems();
					w_waiting.Hide();
				});
			});

			bw.RunWorkerAsync();

		}

		private void abib_MergeSet_Click(object sender, EventArgs e) {

			List<setModel> setPics = new List<setModel>();
			StringBuilder sb = new StringBuilder();
			foreach (setModel sm in lb_sets.SelectedItems.OfType<setModel>()) {
				if (sm.Picture != null)
					setPics.Add(sm);

				if (!string.IsNullOrEmpty(sm.Title)) {
					if (string.IsNullOrEmpty(sb.ToString()))
						sb.Append(sm.Title);
					else
						sb.Append(" + " + sm.Title);
				}
			}

			if (setPics.Count == 0)
				merge(sb.ToString(), null);
			else if (setPics.Count == 1)
				merge(sb.ToString(), setPics.First().Picture);
			else if (setPics.Count > 1) {
				pl_setMerge.sets = setPics;
				pl_setMerge.Title = sb.ToString();
				P_SetMerge.IsOpen = true;
				setSelectPictureIcons();
				p_pano.Visibility = System.Windows.Visibility.Collapsed;
			}
		}

		private void abib_selectPictureAccept_Click(object sender, EventArgs e) {

			merge(pl_setMerge.Title, pl_setMerge.selectedSet.Picture);

			P_SetMerge.IsOpen = false;
		}

		private void merge(string newTitle, pictureModel pm) {
			w_waiting.Show();
			BackgroundWorker bw = new BackgroundWorker();

			bw.DoWork += ((s, args) => {
				Thread.Sleep(2000);
				this.Dispatcher.BeginInvoke(() => {

					setModel sm_new = setModel.createEmptySet(newTitle, "");
					if (pm != null)
						sm_new.Picture = new pictureModel() { Img = pm.Img, SetId = sm_new.Id };
					sm_new.saveSet();

					int cnt = 0;
					foreach (var item in lb_sets.SelectedItems) {
						setModel sm = item as setModel;
						foreach (setColorModel scm in sm.setColorPalette) {
							scm.Set_Id = sm_new.Id;
							scm.Position = (short)(cnt++);
							scm.IconSize = setColorModel.INITIAL_RECT_SIZE;
							scm.X = Math.Floor(scm.Position / setColorModel.MAX_ROWS) * (setColorModel.INITIAL_RECT_SIZE + 10);
							scm.Y = (scm.Position % setColorModel.MAX_ROWS) * (setColorModel.INITIAL_RECT_SIZE + 10);
							scm.Z = scm.Position;
							scm.saveSetColor(true);
						}
					}

					foreach (var item in lb_sets.SelectedItems) {
						setModel sm = item as setModel;
						setModel.deleteSet(sm);
					}

				});
			});

			bw.RunWorkerCompleted += ((s, args) => {
				this.Dispatcher.BeginInvoke(() => {

					refreshLists();
					setPanoramaItems();

					w_waiting.Hide();
				});
			});

			bw.RunWorkerAsync();

		}

		private void abib_camera_Click(object sender, EventArgs e) {
			NavigationService.Navigate(new Uri("/CameraLibrary/" + CameraLibrarySelector.selectorType.camera, UriKind.Relative));
		}

		private void abib_library_Click(object sender, EventArgs e) {
			NavigationService.Navigate(new Uri("/CameraLibrary/" + CameraLibrarySelector.selectorType.library, UriKind.Relative));
		}

		private void abib_pickColor_Click(object sender, EventArgs e) {
			NavigationService.Navigate(new Uri("/CameraLibrary/" + CameraLibrarySelector.selectorType.manual, UriKind.Relative));
		}

		private void abmi_help_Click(object sender, EventArgs e) {
			NavigationService.Navigate(new Uri("/Help", UriKind.Relative));
		}

		private void abib_AcceptEditColor_Click(object sender, EventArgs e) {

			colorModel cm = colorModel.getColor((Guid)ec_editColor.Tag);

			if (cm == null)
				throw new NullReferenceException("ec_editColor.Tag was not a colorModel object when appbar edit icon was pushed");

			cm.Title = ec_editColor.Title;
			cm.Description = ec_editColor.Description;

			cm.saveColor(true);

			p_editColor.IsOpen = false;
		}

		private void abib_AcceptEditSet_Click(object sender, EventArgs e) {

			setModel sm = setModel.getSet((Guid)ec_editSet.Tag);

			if (sm == null)
				throw new NullReferenceException("ec_editSet.Tag was not a setModel object when appbar edit icon was pushed");

			sm.Title = ec_editSet.Title;
			sm.Description = ec_editSet.Description;

			sm.saveSet();

			p_editSet.IsOpen = false;
		}

		private void abmi_setting_Click(object sender, EventArgs e) {
			//NavigationService.Navigate(new Uri("/QuickSelector", UriKind.Relative));
			NavigationService.Navigate(new Uri("/Settings", UriKind.Relative));
		}

		void communicationHelper_userDownloaded(bool success) {

		}

		private void abmi_namedColors_Click(object sender, EventArgs e) {

			p_pano.Visibility = System.Windows.Visibility.Collapsed;

			hideAppBarIcons();
			cdbl_namedColors.Tag = NavigationService;
			P_namedColors.IsOpen = true;
		}

		#endregion

		#region search

		private void ib_colorSearch_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			ViewModel.LoadColorData(tb_color_search.Text.Trim());
		}

		private void tb_colorSearch_KeyUp(object sender, KeyEventArgs e) {
			if (e.Key == Key.Enter) {
				ViewModel.LoadColorData(tb_color_search.Text.Trim());
				this.Focus();
			}
		}

		private void ib_setSearch_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			ViewModel.LoadSetData(tb_set_search.Text.Trim());
		}

		private void tb_setSearch_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
			if (e.Key == Key.Enter) {
				ViewModel.LoadSetData(tb_set_search.Text.Trim());
				this.Focus();
			}
		}

		#endregion

		private void refreshLists() {
			ViewModel.LoadColorData();
			ViewModel.LoadSetData();
			DataContext = ViewModel;

			switch (StorageHelper.loadSetting<App.searchVisibilityType>("searchVisibilityOptions")) {
				case App.searchVisibilityType.auto:
					if (ViewModel.colorItems.Count < 5)
						sp_color_search.Visibility = System.Windows.Visibility.Collapsed;
					else
						sp_color_search.Visibility = System.Windows.Visibility.Visible;
					if (ViewModel.setItems.Count < 5)
						sp_set_search.Visibility = System.Windows.Visibility.Collapsed;
					else
						sp_set_search.Visibility = System.Windows.Visibility.Visible;
					break;
				case App.searchVisibilityType.hide:
					sp_color_search.Visibility = System.Windows.Visibility.Collapsed;
					sp_set_search.Visibility = System.Windows.Visibility.Collapsed;
					break;
				case App.searchVisibilityType.show:
					sp_color_search.Visibility = System.Windows.Visibility.Visible;
					sp_set_search.Visibility = System.Windows.Visibility.Visible;
					break;
			}
		}

		void edit_Closed(object sender, EventArgs e) {

			refreshLists();

			p_pano.Visibility = System.Windows.Visibility.Visible;

			if (lb_colors.SelectedItems.Count > 0)
				lb_colors.SelectedItems.Clear();
			if (lb_sets.SelectedItems.Count > 0)
				lb_sets.SelectedItems.Clear();

			setStandardAppBarIcons();


		}

		void namedColors_Closed(object sender, EventArgs e) {

			p_pano.Visibility = System.Windows.Visibility.Visible;

			setStandardAppBarIcons();
		}

		void setMergedPopup_Closed(object sender, EventArgs e) {

			//if (lb_sets.SelectedItems.Count > 0)
			//      lb_sets.SelectedItems.Clear();

			p_pano.Visibility = System.Windows.Visibility.Visible;

			setStandardAppBarIcons();
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

			FrameworkElement oth = p_pano;
			o_mainPage.objectToHide = oth;
			o_mainPage.addOverlay(new mainPageOverlay());

			hideAppBarIcons();

			p_overlay.IsOpen = true;
		}

		void p_overlay_Closed(object sender, EventArgs e) {
			setStandardAppBarIcons();
		}

		protected override void OnBackKeyPress(CancelEventArgs e) {
			if (p_editColor.IsOpen || p_editSet.IsOpen || P_namedColors.IsOpen || P_SetMerge.IsOpen || p_overlay.IsOpen) {
				e.Cancel = true;
				if (p_editColor.IsOpen)
					p_editColor.IsOpen = false;
				else if (p_editSet.IsOpen)
					p_editSet.IsOpen = false;
				else if (P_namedColors.IsOpen)
					P_namedColors.IsOpen = false;
				else if (P_SetMerge.IsOpen)
					P_SetMerge.IsOpen = false;
				else if (p_overlay.IsOpen)
					p_overlay.IsOpen = false;
			}

		}






	}
}