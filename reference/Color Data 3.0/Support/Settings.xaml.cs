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
using Color_Data_3._0.Classes;
using System.ComponentModel;
using Color_Data_3._0.db;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using Color_Data_3._0.Classes.ColorClasses;
using Microsoft.Phone.Tasks;

namespace Color_Data_3._0.Support {
	public partial class Settings : PhoneApplicationPage {

		private bool ignore = true;

		public Settings() {
			InitializeComponent();
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
			ShellTile quickSelector = FindTile("QuickSelector");

			if (ApplicationLicense.IsTrial) {
				sp_trial1.Visibility = System.Windows.Visibility.Visible;
				sp_trial2.Visibility = System.Windows.Visibility.Visible;
				sp_trial3.Visibility = System.Windows.Visibility.Visible;
			}
			else {
				sp_trial1.Visibility = System.Windows.Visibility.Collapsed;
				sp_trial2.Visibility = System.Windows.Visibility.Collapsed;
				sp_trial3.Visibility = System.Windows.Visibility.Collapsed;
			}

			if (ApplicationLicense.IsTrial) {
				ts_liveTile.Content = "Live Tile Off   ";
				ts_liveTile.IsChecked = false;
				ts_liveTile.IsEnabled = false;
			}
			else {
				if (quickSelector == null) {
					ts_liveTile.Content = "Live Tile Off   ";
					ts_liveTile.IsChecked = false;
				}
				else {
					ts_liveTile.Content = "Live Tile On   ";
					ts_liveTile.IsChecked = true;
				}
			}
			if (!string.IsNullOrEmpty(App.uid))
				tbox_webCode.Text = App.uid;


			if (ApplicationLicense.IsTrial) {
				ListPickerItem lpi = new ListPickerItem() { Content = "Not available" };
				lp_namedColorDatabase.Items.Add(lpi);
				lp_namedColorDatabase.SelectedItem = lpi;
				lp_namedColorDatabase.IsEnabled = false;
				lp_namedColorDatabase.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
			}
			else {
				switch (StorageHelper.loadSetting<App.namedColorDatabaseType>("NamedColorDatabase")) {
					case App.namedColorDatabaseType.microsoft:
						lp_namedColorDatabase.SelectedItem = lpi_namedColors_microsoft;
						break;
					case App.namedColorDatabaseType.crayon:
						lp_namedColorDatabase.SelectedItem = lpi_namedColors_crayola;
						break;
					case App.namedColorDatabaseType.w3c:
						lp_namedColorDatabase.SelectedItem = lpi_namedColors_w3c;
						break;
					case App.namedColorDatabaseType.wiki:
						lp_namedColorDatabase.SelectedItem = lpi_namedColors_wiki;
						break;
				}
			}

			if (ApplicationLicense.IsTrial) {
				ListPickerItem lpi = new ListPickerItem() { Content = "Not available" };
				lp_NamedColorDistance.Items.Add(lpi);
				lp_NamedColorDistance.SelectedItem = lpi;
				lp_NamedColorDistance.IsEnabled = false;
				lp_NamedColorDistance.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
			}
			else {
				switch (StorageHelper.loadSetting<App.namedColorDistanceType>("NamedColorDistance")) {
					case App.namedColorDistanceType.p5:
						lp_NamedColorDistance.SelectedItem = lpi_5p;
						break;
					case App.namedColorDistanceType.p10:
						lp_NamedColorDistance.SelectedItem = lpi_10p;
						break;
					case App.namedColorDistanceType.p30:
						lp_NamedColorDistance.SelectedItem = lpi_30p;
						break;
					case App.namedColorDistanceType.p50:
						lp_NamedColorDistance.SelectedItem = lpi_50p;
						break;
					case App.namedColorDistanceType.all:
						lp_NamedColorDistance.SelectedItem = lpi_all;
						break;
				}
			}

			if (ApplicationLicense.IsTrial) {
				lp_AutoName.SelectedItem = lpi_autoName_all;
				lp_AutoName.IsEnabled = false;
				lp_AutoName.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
			}
			else {
				switch (StorageHelper.loadSetting<App.autoNameOptionType>("AutoNameOptions")) {
					case App.autoNameOptionType.all:
						lp_AutoName.SelectedItem = lpi_autoName_all;
						break;
					case App.autoNameOptionType.name:
						lp_AutoName.SelectedItem = lpi_autoName_name;
						break;
					case App.autoNameOptionType.desc:
						lp_AutoName.SelectedItem = lpi_autoName_desc;
						break;
					case App.autoNameOptionType.none:
						lp_AutoName.SelectedItem = lpi_autoName_none;
						break;
				}
			}

			if (ApplicationLicense.IsTrial) {
				lp_searchBarVisibility.SelectedItem = lpi_searchBarVisibility_hide;
				lp_searchBarVisibility.IsEnabled = false;
				lp_searchBarVisibility.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
			}
			else {
				switch (StorageHelper.loadSetting<App.searchVisibilityType>("searchVisibilityOptions")) {
					case App.searchVisibilityType.hide:
						lp_searchBarVisibility.SelectedItem = lpi_searchBarVisibility_hide;
						break;
					case App.searchVisibilityType.show:
						lp_searchBarVisibility.SelectedItem = lpi_searchBarVisibility_show;
						break;
					case App.searchVisibilityType.auto:
						lp_searchBarVisibility.SelectedItem = lpi_searchBarVisibility_auto;
						break;
				}
			}

			if (ApplicationLicense.IsTrial) {
				lp_helpVisibility.SelectedItem = lpi_helpVisibility_show;
				lp_helpVisibility.IsEnabled = false;
				lp_helpVisibility.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
			}
			else {
				switch (StorageHelper.loadSetting<App.helpVisibilityType>("helpVisibilityOptions")) {
					case App.helpVisibilityType.hide:
						lp_helpVisibility.SelectedItem = lpi_helpVisibility_hide;
						break;
					case App.helpVisibilityType.show:
						lp_helpVisibility.SelectedItem = lpi_helpVisibility_show;
						break;
				}
			}

			if (ApplicationLicense.IsTrial) {
				lp_splashVisibility.SelectedItem = lpi_splashVisibility_show;
				lp_splashVisibility.IsEnabled = false;
				lp_splashVisibility.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
			}
			else {
				switch (StorageHelper.loadSetting<App.splashVisibilityType>("splashVisibilityOptions")) {
					case App.splashVisibilityType.hide:
						lp_splashVisibility.SelectedItem = lpi_splashVisibility_hide;
						break;
					case App.splashVisibilityType.show:
						lp_splashVisibility.SelectedItem = lpi_splashVisibility_show;
						break;
				}
			}

			if (ApplicationLicense.IsTrial) {
				lp_uploadSettings.SelectedItem = lpi_uploadSettings_disabled;
				lp_uploadSettings.IsEnabled = false;
				b_forceSync.IsEnabled = false;
				lp_uploadSettings.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
			}
			else {
				switch (StorageHelper.loadSetting<App.webCommType>("webCommOptions")) {
					case App.webCommType.auto:
						lp_uploadSettings.SelectedItem = lpi_uploadSettings_auto;
						b_forceSync.IsEnabled = false;
						break;
					case App.webCommType.manual:
						lp_uploadSettings.SelectedItem = lpi_uploadSettings_manual;
						b_forceSync.IsEnabled = true;
						break;
					case App.webCommType.disabled:
						lp_uploadSettings.SelectedItem = lpi_uploadSettings_disabled;
						b_forceSync.IsEnabled = false;
						break;
				}
			}
			ignore = false;
		}

		protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e) {
			ignore = true;
		}

		private void lp_namedColorDatabase_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (ignore)
				return;

			if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_namedColors_microsoft))
				StorageHelper.saveSetting<App.namedColorDatabaseType>("NamedColorDatabase", App.namedColorDatabaseType.microsoft);
			else if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_namedColors_crayola))
				StorageHelper.saveSetting<App.namedColorDatabaseType>("NamedColorDatabase", App.namedColorDatabaseType.crayon);
			else if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_namedColors_w3c))
				StorageHelper.saveSetting<App.namedColorDatabaseType>("NamedColorDatabase", App.namedColorDatabaseType.w3c);
			else if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_namedColors_wiki))
				StorageHelper.saveSetting<App.namedColorDatabaseType>("NamedColorDatabase", App.namedColorDatabaseType.wiki);
			else
				throw new Exception("NamedColorDatabase listPicker not selecting value provided by list");
		}

		private void lp_NamedColorDistance_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (ignore)
				return;

			if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_5p))
				StorageHelper.saveSetting<App.namedColorDistanceType>("NamedColorDistance", App.namedColorDistanceType.p5);
			else if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_10p))
				StorageHelper.saveSetting<App.namedColorDistanceType>("NamedColorDistance", App.namedColorDistanceType.p10);
			else if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_30p))
				StorageHelper.saveSetting<App.namedColorDistanceType>("NamedColorDistance", App.namedColorDistanceType.p30);
			else if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_50p))
				StorageHelper.saveSetting<App.namedColorDistanceType>("NamedColorDistance", App.namedColorDistanceType.p50);
			else if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_all))
				StorageHelper.saveSetting<App.namedColorDistanceType>("NamedColorDistance", App.namedColorDistanceType.all);
			else
				throw new Exception("NamedColorDistance listPicker not selecting value provided by list");
		}

		private void lp_AutoName_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (ignore)
				return;

			if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_autoName_all))
				StorageHelper.saveSetting<App.autoNameOptionType>("AutoNameOptions", App.autoNameOptionType.all);
			else if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_autoName_name))
				StorageHelper.saveSetting<App.autoNameOptionType>("AutoNameOptions", App.autoNameOptionType.name);
			else if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_autoName_desc))
				StorageHelper.saveSetting<App.autoNameOptionType>("AutoNameOptions", App.autoNameOptionType.desc);
			else if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_autoName_none))
				StorageHelper.saveSetting<App.autoNameOptionType>("AutoNameOptions", App.autoNameOptionType.none);
			else
				throw new Exception("AutoNameOptions listPicker not selecting value provided by list");
		}

		private void lp_searchBarVisibility_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (ignore)
				return;
			if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_searchBarVisibility_show))
				StorageHelper.saveSetting<App.searchVisibilityType>("searchVisibilityOptions", App.searchVisibilityType.show);
			else if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_searchBarVisibility_hide))
				StorageHelper.saveSetting<App.searchVisibilityType>("searchVisibilityOptions", App.searchVisibilityType.hide);
			else if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_searchBarVisibility_auto))
				StorageHelper.saveSetting<App.searchVisibilityType>("searchVisibilityOptions", App.searchVisibilityType.auto);
			else
				throw new Exception("searchBarVisibility listPicker not selecting value provided by list");
		}

		private void lp_helpVisibility_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (ignore)
				return;
			if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_helpVisibility_show))
				StorageHelper.saveSetting<App.helpVisibilityType>("helpVisibilityOptions", App.helpVisibilityType.show);
			else if (((ListPickerItem)e.AddedItems[0]).Equals(lpi_helpVisibility_hide))
				StorageHelper.saveSetting<App.helpVisibilityType>("helpVisibilityOptions", App.helpVisibilityType.hide);
			else
				throw new Exception("helpVisibilityOptions listPicker not selecting value provided by list");
		}

		private void lp_splashVisibility_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (ignore)
				return;
			if ((e.AddedItems[0]).Equals(lpi_splashVisibility_show))
				StorageHelper.saveSetting<App.splashVisibilityType>("splashVisibilityOptions", App.splashVisibilityType.show);
			else if ((e.AddedItems[0]).Equals(lpi_splashVisibility_hide))
				StorageHelper.saveSetting<App.splashVisibilityType>("splashVisibilityOptions", App.splashVisibilityType.hide);
			else
				throw new Exception("splashVisibilityOptions listPicker not selecting value provided by list");
		}

		private void lp_uploadSettings_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (ignore)
				return;

			if ((e.AddedItems[0]).Equals(lpi_uploadSettings_auto)) {
				StorageHelper.saveSetting<App.webCommType>("webCommOptions", App.webCommType.auto);
				b_forceSync.IsEnabled = false;
			}
			else if ((e.AddedItems[0]).Equals(lpi_uploadSettings_manual)) {
				StorageHelper.saveSetting<App.webCommType>("webCommOptions", App.webCommType.manual);
				b_forceSync.IsEnabled = true;
			}
			else if ((e.AddedItems[0]).Equals(lpi_uploadSettings_disabled)) {
				StorageHelper.saveSetting<App.webCommType>("webCommOptions", App.webCommType.disabled);
				b_forceSync.IsEnabled = false;
			}
			else
				throw new Exception("webCommOptions listPicker not selecting value provided by list");


		}

		private void forceSync_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			//TODO: need to do this in a way that works
			w_waiting.Show();

			BackgroundWorker backgroundWorker = new BackgroundWorker();


			backgroundWorker.DoWork += ((s, args) => {
				this.Dispatcher.BeginInvoke(() => {

					communicationHelper.Sync();

				});
			});

			backgroundWorker.RunWorkerCompleted += ((s, args) => {
				this.Dispatcher.BeginInvoke(() => {
					w_waiting.Hide();
				});
			});

			backgroundWorker.RunWorkerAsync();
		}

		private void ToggleSwitch_Checked(object sender, System.Windows.RoutedEventArgs e) {

			ts_liveTile.Content = "Live Tile On   ";

			ShellTile quickSelector = FindTile("QuickSelector");
			if (quickSelector == null) {

				LiveTileImage.MakeFront(Colors.Transparent);

				SolidColorBrush scb = (SolidColorBrush)Resources["PhoneAccentBrush"];
				System.Windows.Media.Color c = scb.Color;
				namedColor nC = namedColor.findNamedColors(c, namedColor.NAMEDCOLOR_DEVIATION);


				LiveTileImage.MakeBack(c);
				string name;
				if (nC == null)
					name = RGB.ColorToHex(c);
				else
					name = nC.colorName;

				StandardTileData secondaryTile = new StandardTileData {
					BackgroundImage = new Uri("/Images/Background_small.png", UriKind.Relative),
					Title = "Quick Color Data",
					Count = null,
					BackTitle = name,
					BackContent = "R:   " + c.R + System.Environment.NewLine + "G:   " + c.G + System.Environment.NewLine + "B:   " + c.B,
					BackBackgroundImage = new Uri("isostore:/Shared/ShellContent/liveTileBack.png", UriKind.Absolute)
				};

				ShellTile.Create(new Uri("/QuickSelector", UriKind.Relative), secondaryTile);
			}
		}

		private void ToggleSwitch_Unchecked(object sender, System.Windows.RoutedEventArgs e) {

			ts_liveTile.Content = "Live Tile Off   ";

			ShellTile quickSelector = FindTile("QuickSelector");

			if (quickSelector != null) {
				quickSelector.Delete();
			}

		}

		private ShellTile FindTile(string partOfUri) {
			ShellTile shellTile = ShellTile.ActiveTiles.FirstOrDefault(
			    tile => tile.NavigationUri.ToString().Contains(partOfUri));

			return shellTile;
		}

		private void purchase_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
			marketplaceDetailTask.Show();
		}





	}
}