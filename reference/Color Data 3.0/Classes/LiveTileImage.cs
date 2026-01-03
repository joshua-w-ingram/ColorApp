using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.Windows.Resources;
using Color_Data_3._0.Classes.ColorClasses;

namespace Color_Data_3._0.Classes {
	public static class LiveTileImage {

		public static void MakeFront(Color c) {
			Grid g = new Grid();
			g.Height = 173;
			g.Width = 173;
			g.Background = new SolidColorBrush(c);

			Image img = new Image();
			img.Source = new BitmapImage(new Uri("/Images/Background_small.png", UriKind.Relative));
			img.VerticalAlignment = VerticalAlignment.Center;
			img.HorizontalAlignment = HorizontalAlignment.Center;

			Border b = new Border();
			b.Background = new SolidColorBrush(Colors.Transparent);
			b.Width = 173;
			b.Height = 173;
			b.Child = img;
			b.Measure(new Size(173, 173));
			b.Arrange(new Rect(0, 0, 173, 173));
			b.UpdateLayout();

			g.Children.Add(b);

			//call measure, arrange and updatelayout to prepare for rendering
			g.Measure(new Size(173, 173));
			g.Arrange(new Rect(0, 0, 173, 173));
			g.UpdateLayout();

			WriteableBitmap wbm = new WriteableBitmap(173, 173);
			wbm.Render(g, null);
			wbm.Invalidate();

			//write image to isolated storage - note that the \Shared\ShellContent folder is required
			string sIsoStorePath = @"\Shared\ShellContent\liveTile.png";
			using (IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication()) {
				//ensure directory exists
				String sDirectory = System.IO.Path.GetDirectoryName(sIsoStorePath);
				if (!appStorage.DirectoryExists(sDirectory)) {
					appStorage.CreateDirectory(sDirectory);
				}

				using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(sIsoStorePath, System.IO.FileMode.Create, appStorage)) {
					wbm.SaveJpeg(stream, 173, 173, 0, 100);
				}

			}

		}

		public static void MakeBack(Color c) {
			Grid g = new Grid();
			g.Height = 173;
			g.Width = 173;
			g.Background = new SolidColorBrush(c);

			Border b = new Border();
			b.Background = new SolidColorBrush(Colors.Transparent);
			b.Width = 173;
			b.Height = 173;
			//b.Child = sp;
			b.Measure(new Size(173, 173));
			b.Arrange(new Rect(0, 0, 173, 173));
			b.UpdateLayout();

			g.Children.Add(b);

			//call measure, arrange and updatelayout to prepare for rendering
			g.Measure(new Size(173, 173));
			g.Arrange(new Rect(0, 0, 173, 173));
			g.UpdateLayout();

			WriteableBitmap wbm = new WriteableBitmap(173, 173);
			wbm.Render(g, null);
			wbm.Invalidate();

			//write image to isolated storage - note that the \Shared\ShellContent folder is required
			string sIsoStorePath = @"\Shared\ShellContent\liveTileBack.png";
			using (IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication()) {
				//ensure directory exists
				String sDirectory = System.IO.Path.GetDirectoryName(sIsoStorePath);
				if (!appStorage.DirectoryExists(sDirectory)) {
					appStorage.CreateDirectory(sDirectory);
				}

				using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(sIsoStorePath, System.IO.FileMode.Create, appStorage)) {
					wbm.SaveJpeg(stream, 173, 173, 0, 100);
				}

			}
		}


	}
}
