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
using Color_Data_3._0.Classes;
using Color_Data_3._0.Classes.ColorLists;
using System.Windows.Navigation;
using Color_Data_3._0.Data;

namespace Color_Data_3._0.Controls {
	public partial class colorDatabaseList : UserControl {
		public colorDatabaseList() {
			InitializeComponent();
			Loaded += new RoutedEventHandler(colorDatabaseList_Loaded);
		}

		void colorDatabaseList_Loaded(object sender, RoutedEventArgs e) {
			tb_search.Text = "";
			search(null);
		}

		private void imageButton_Tap(object sender, GestureEventArgs e) {
			search(tb_search.Text.ToLower());
		}

		private void tb_search_KeyUp(object sender, KeyEventArgs e) {
			if (e.Key == Key.Enter) {
				search(tb_search.Text.ToLower());
				this.Focus();
			}
		}

		private void search(string searchString) {
			List<Group<namedColor>> groups = new List<Group<namedColor>>();

			if (string.IsNullOrEmpty(searchString)) {
				for (char i = 'a'; i <= 'z'; i++) {
					IEnumerable<namedColor> grp = from color in App.colorNameXml.Elements("Color")
										where color.Attribute("name").Value.Substring(0, 1).ToLower().Equals(i.ToString())
										select new namedColor {
											colorName = color.Attribute("name").Value,
											color = Color.FromArgb(255, byte.Parse(color.Attribute("R").Value), byte.Parse(color.Attribute("G").Value), byte.Parse(color.Attribute("B").Value))
										};
					groups.Add(new Group<namedColor>(i.ToString(), grp));
				}
			}
			else {
				for (char i = 'a'; i <= 'z'; i++) {
					IEnumerable<namedColor> grp = from color in App.colorNameXml.Elements("Color")
										where color.Attribute("name").Value.Substring(0, 1).ToLower().Equals(i.ToString()) && color.Attribute("name").Value.ToLower().Contains(searchString)
										select new namedColor {
											colorName = color.Attribute("name").Value,
											color = Color.FromArgb(255, byte.Parse(color.Attribute("R").Value), byte.Parse(color.Attribute("G").Value), byte.Parse(color.Attribute("B").Value))
										};
					groups.Add(new Group<namedColor>(i.ToString(), grp));
				}
			}
			this.colorsList.ItemsSource = groups;

			colorsList.ScrollTo(groups[0]);
		}

		private void colorButton_Tap(object sender, GestureEventArgs e) {
			colorButton2 cb = (colorButton2)sender;

			NamedColorData.namedColorName = cb.Content.ToString();
			NamedColorData.currentNamedColor = (Color)cb.Background.GetValue(SolidColorBrush.ColorProperty);
			((NavigationService)Tag).Navigate(new Uri("/NamedColorData", UriKind.Relative));
		}



	}
}
