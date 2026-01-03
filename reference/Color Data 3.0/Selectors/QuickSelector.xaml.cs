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
using Microsoft.Devices;
using System.Windows.Media.Imaging;
using System.Threading;
using System.IO;
using Color_Data_3._0.Classes.ColorClasses;
using Color_Data_3._0.Classes;
using Microsoft.Phone.Shell;

namespace Color_Data_3._0.Selectors {

	public partial class QuickSelector : PhoneApplicationPage {

		private PhotoCamera camera; //The device's camera
		private static ManualResetEvent pauseFramesEvent = new ManualResetEvent(true); //Used to avoid collisions between one image processing and another
		private Color theColor;

		public QuickSelector() {
			InitializeComponent();
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
			if (null == camera) {

				camera = new PhotoCamera(CameraType.Primary);

				camera.Initialized += camera_Initialized;

				previewBrush.SetSource(camera);
			}

			g_Color.Background = new SolidColorBrush(Colors.Black);
			b_accept.Visibility = System.Windows.Visibility.Collapsed;

			tb_namedColor.Text = "";
			tb_r.Text = "";
			tb_g.Text = "";
			tb_b.Text = "";

		}

		protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e) {
			camera.Dispose();
			camera = null;

			base.OnNavigatedFrom(e);
		}

		private void camera_Initialized(object sender, CameraOperationCompletedEventArgs e) {
			if (camera == null)
				return;

			if (e.Succeeded) {
				var res = from resolution in camera.AvailableResolutions
					    where resolution.Width == 640
					    select resolution;

				camera.Resolution = res.First();
			}
		}

		private void ib_focus_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			camera.Focus();
		}

		private void ib_add_Tap(object sender, System.Windows.Input.GestureEventArgs e) {

			Blip.Stop();
			Blip.Begin();

			// Obtain the YCbCr layout settings used by the camera buffer.
			var bufferLayout = camera.YCbCrPixelLayout;

			// Allocate the appropriately sized preview buffer.
			byte[] currentPreviewBuffer = new byte[bufferLayout.RequiredBufferSize];

			camera.GetPreviewBufferYCbCr(currentPreviewBuffer);

			List<Color> cC = new List<Color>();
			for (int u = 310; u <= 330; u++) {
				for (int v = 230; v <= 250; v++) {
					// The output parameters used in the following method.
					byte y;
					int cr;
					int cb;
					// Extract details about the pixel where the camera crosshairs meet.
					// This location is estimated to be X=320, Y=240. Adjust as desired.
					GetYCbCrFromPixel(bufferLayout, currentPreviewBuffer, 320, 240, out y, out cr, out cb);

					cC.Add(YCbCrToArgb(y, cb, cr));
				}
			}

			long r = 0;
			long g = 0;
			long b = 0;

			foreach (Color _c in cC) {
				r += _c.R;
				g += _c.G;
				b += _c.B;
			}

			Color c = Color.FromArgb(255, (byte)(r / cC.Count), (byte)(g / cC.Count), (byte)(b / cC.Count));

			theColor = c;
			g_Color.Background = new SolidColorBrush(theColor);

			HSL hsl = HSL.convertRGB(c);

			if (hsl.L < .75) {
				tb_namedColor.Foreground = new SolidColorBrush(Colors.White);
				tb_r.Foreground = new SolidColorBrush(Colors.White);
				tb_g.Foreground = new SolidColorBrush(Colors.White);
				tb_b.Foreground = new SolidColorBrush(Colors.White);
			}
			else {
				tb_namedColor.Foreground = new SolidColorBrush(Colors.Black);
				tb_r.Foreground = new SolidColorBrush(Colors.Black);
				tb_g.Foreground = new SolidColorBrush(Colors.Black);
				tb_b.Foreground = new SolidColorBrush(Colors.Black);
			}

			namedColor nC = namedColor.findNamedColors(c, namedColor.NAMEDCOLOR_DEVIATION);
			if (nC != null) {
				if (nC.deviation > 0.1)
					tb_namedColor.Text = nC.colorName.Trim() + "*";
				else
					tb_namedColor.Text = nC.colorName.Trim();
			}
			else
				tb_namedColor.Text = "";

			tb_r.Text = "R:   " + c.R;
			tb_g.Text = "G:   " + c.G;
			tb_b.Text = "B:   " + c.B;

			updateTile(theColor, tb_namedColor.Text);
			b_accept.Visibility = System.Windows.Visibility.Visible;
		}

		private void ib_accept_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			string name;
			if (string.IsNullOrEmpty(tb_namedColor.Text))
				name = RGB.ColorToHex(theColor);
			else
				name = tb_namedColor.Text;

			colorModel cm = new colorModel() {
				Id = Guid.NewGuid(),
				Title = name,
				Description = "",
				ColorBrush = new SolidColorBrush(theColor),
				IsUploaded = false
			};

			cm.saveColor(true);

			MessageBox.Show(name + " has been added to your color list");
		}

		private void GetYCbCrFromPixel(YCbCrPixelLayout layout, byte[] currentPreviewBuffer, int xFramePos, int yFramePos, out byte y, out int cr, out int cb) {
			// Find the bytes corresponding to the pixel location in the frame.
			int yBufferIndex = layout.YOffset + yFramePos * layout.YPitch + xFramePos * layout.YXPitch;
			int crBufferIndex = layout.CrOffset + (yFramePos / 2) * layout.CrPitch + (xFramePos / 2) * layout.CrXPitch;
			int cbBufferIndex = layout.CbOffset + (yFramePos / 2) * layout.CbPitch + (xFramePos / 2) * layout.CbXPitch;

			// The luminance value is always positive.
			y = currentPreviewBuffer[yBufferIndex];

			// The preview buffer contains an unsigned value between 255 and 0.
			// The buffer value is cast from a byte to an integer.
			cr = currentPreviewBuffer[crBufferIndex];

			// Convert to a signed value between 127 and -128.
			cr -= 128;

			// The preview buffer contains an unsigned value between 255 and 0.
			// The buffer value is cast from a byte to an integer.
			cb = currentPreviewBuffer[cbBufferIndex];

			// Convert to a signed value between 127 and -128.
			cb -= 128;
		}

		private Color YCbCrToArgb(byte y, int cb, int cr) {
			// Individual RGB components.
			int r, g, b;

			// Used for building a 32-bit ARGB pixel.
			uint argbPixel;

			// Assumes Cb & Cr have been converted to signed values (ranging from 127 to -128).

			// Integer-only division.
			r = y + cr + (cr >> 2) + (cr >> 3) + (cr >> 5);
			g = y - ((cb >> 2) + (cb >> 4) + (cb >> 5)) - ((cr >> 1) + (cr >> 3) + (cr >> 4) + (cr >> 5));
			b = y + cb + (cb >> 1) + (cb >> 2) + (cb >> 6);

			// Clamp values to 8-bit RGB range between 0 and 255.
			r = r <= 255 ? r : 255;
			r = r >= 0 ? r : 0;
			g = g <= 255 ? g : 255;
			g = g >= 0 ? g : 0;
			b = b <= 255 ? b : 255;
			b = b >= 0 ? b : 0;

			// Pack individual components into a single pixel.
			argbPixel = 0xff000000; // Alpha
			argbPixel |= (uint)b;
			argbPixel |= (uint)(g << 8);
			argbPixel |= (uint)(r << 16);


			return Color.FromArgb(255, (byte)r, (byte)g, (byte)b);
		}

		private void updateTile(Color c, string name) {

			ShellTile quickSelector = FindTile("QuickSelector");
			if (quickSelector != null) {

				LiveTileImage.MakeFront(c);
				LiveTileImage.MakeBack(c);

				StandardTileData secondaryTile = new StandardTileData {
					BackgroundImage = new Uri("isostore:/Shared/ShellContent/liveTile.png", UriKind.Absolute),
					Title = "Quick Color Data",
					Count = null,
					BackTitle = name,
					BackContent = "R:   " + c.R + System.Environment.NewLine + "G:   " + c.G + System.Environment.NewLine + "B:   " + c.B,
					BackBackgroundImage = new Uri("isostore:/Shared/ShellContent/liveTileBack.png", UriKind.Absolute)
				};

				quickSelector.Update(secondaryTile);
			}

		}

		private ShellTile FindTile(string partOfUri) {
			ShellTile shellTile = ShellTile.ActiveTiles.FirstOrDefault(
			    tile => tile.NavigationUri.ToString().Contains(partOfUri));

			return shellTile;
		}
	}
}