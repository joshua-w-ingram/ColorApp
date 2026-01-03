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
using System.Threading;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Phone.Tasks;
using Microsoft.Phone;
using Color_Data_3._0.Classes.ColorClasses;
using Color_Data_3._0.Controls;
using Color_Data_3._0.Classes;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Color_Data_3._0.Support;

namespace Color_Data_3._0.Selectors {
	public partial class CameraLibrarySelector : PhoneApplicationPage {

		public const double TweakStep = .06;

		public enum selectorType {
			camera,
			library,
			manual
		}
		private ChooserBase<PhotoResult> chooser;

		private enum sampleType {
			PointSample,
			AreaSample
		}
		private sampleType colorSample = sampleType.PointSample;

		//point coordinates of user tap
		private Point touchPoint;
		//current location of user tap (transforms are used on touchPoint, so there is no location similarity) 
		private colorLocator currentLocator;
		//used to increment name, since there is a state and state changes require object names
		private int currentLocatorInc = 0;

		//all saved colors (colorholder tag holds reference to the colorLocator that is associated with it)
		private List<colorHolder> colorCollection;

		//when the color max is reached isAddlocked toggles
		private bool isAddLocked;

		//current selected color for tweak mode
		Color currentColor;

		//used in case user presses back button on chooser
		public static bool isChooserCanceled;

		//MANUAL SELECTOR HELPERS
		public static Color ManualSelectorColor;
		public bool isManual;

		public CameraLibrarySelector() {
			InitializeComponent();
			DataContext = this;
			Loaded += new RoutedEventHandler(CameraSelector_Loaded);

			if (ApplicationLicense.IsTrial) {

			}
			else {
				var parent1 = ad1.Parent as Grid;
				if (parent1 != null)
					parent1.Children.Remove(ad1);
				var parent2 = ad2.Parent as Grid;
				if (parent2 != null)
					parent2.Children.Remove(ad2);
				var parent3 = ad3.Parent as Grid;
				if (parent3 != null)
					parent3.Children.Remove(ad3);
				var parent4 = ad4.Parent as Grid;
				if (parent4 != null)
					parent4.Children.Remove(ad4);
			}
		}

		void CameraSelector_Loaded(object sender, RoutedEventArgs e) {
			string val;
			NavigationContext.QueryString.TryGetValue("selector", out val);
			isChooserCanceled = false;
			if (val == selectorType.camera.ToString()) {
				isManual = false;
				chooser = new CameraCaptureTask();
				chooser.Completed += new EventHandler<PhotoResult>(ccT_Completed);
				chooser.Show();

			}
			else if (val == selectorType.library.ToString()) {
				isManual = false;
				chooser = new PhotoChooserTask();
				chooser.Completed += new EventHandler<PhotoResult>(ccT_Completed);
				chooser.Show();
			}
			else if (val == selectorType.manual.ToString()) {
				isManual = true;
				chooser = new colorChooserTask();
				chooser.Completed += new EventHandler<PhotoResult>(ccT_Completed);
				chooser.Show();
			}
			else
				throw new Exception("There is no query string 'selector' to determine which chooser to use");

			colorCollection = new List<colorHolder>();
			isAddLocked = false;
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
			if (isChooserCanceled) {
				isChooserCanceled = false;
				App.returnToMain = true;
				NavigationService.GoBack();
			}
			setOverlayImageButtonVisibility();
		}

		#region Tasks

		void ccT_Completed(object sender, PhotoResult e) {
			if (e.TaskResult == TaskResult.None) {
				if (isManual) {
					//this indicates that the back button was pressed while in the manual color selector
					return;
				}
			}
			else if (e.TaskResult == TaskResult.Cancel) {
				if (isManual) {
					//this indicates that the cancel button was pressed while in the manual color selector
					App.returnToMain = true;
					NavigationService.GoBack();
				}
				else {
					isChooserCanceled = true;

				}
				return;
			}
			if (e.ChosenPhoto == null) {
				currentColor = ManualSelectorColor;
				fillTweakBoard(currentColor);
				VisualStateManager.GoToState(this, "TweakMode", false);
				return;
			}

			BitmapImage bi = new BitmapImage();
			bi.SetSource(e.ChosenPhoto);

			int width = bi.PixelWidth;
			int height = bi.PixelHeight;

			double nominalWidth = 480;
			double nominalHeight = 720;

			//find which dimension is proportionally bigger, based on display area
			double wProportion = 0;
			double hProportion = 0;

			if (height > width) {
				wProportion = width / nominalWidth;
				hProportion = height / nominalHeight;
			}
			else {
				wProportion = height / nominalWidth;
				hProportion = width / nominalHeight;
			}

			double proportionReduction;
			if (wProportion > hProportion)
				proportionReduction = wProportion;
			else
				proportionReduction = hProportion;

			Stream capturedImage;

			capturedImage = RotateStream(e.ChosenPhoto, (int)(width / proportionReduction), (int)(height / proportionReduction));


			BitmapImage bmp = new BitmapImage();
			bmp.SetSource(capturedImage);

			i_picture.Source = bmp;
			VisualStateManager.GoToState(this, "SelectionMode", false);
			VisualStateManager.GoToState(this, "RemoveLocked", false);
		}

		#endregion

		#region SelectMode

		private Stream RotateStream(Stream stream, int width, int height) {
			int updatedWidth;
			int updatedHeight;

			BitmapImage bitmap = new BitmapImage();
			bitmap.SetSource(stream);
			WriteableBitmap wbSource = new WriteableBitmap(bitmap);

			WriteableBitmap wbTarget = null;
			if (height > width) {
				wbTarget = new WriteableBitmap(wbSource.PixelWidth, wbSource.PixelHeight);
				updatedWidth = width;
				updatedHeight = height;
			}
			else {
				wbTarget = new WriteableBitmap(wbSource.PixelHeight, wbSource.PixelWidth);
				updatedWidth = height;
				updatedHeight = width;
			}

			if (height > width)
				wbTarget = wbSource;
			else {
				for (int x = 0; x < wbSource.PixelWidth; x++) {
					for (int y = 0; y < wbSource.PixelHeight; y++) {
						wbTarget.Pixels[(wbSource.PixelHeight - y - 1) + x * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
					}
				}
			}

			MemoryStream targetStream = new MemoryStream();
			wbTarget.SaveJpeg(targetStream, updatedWidth, updatedHeight, 0, 100);
			return targetStream;
		}

		double initialScale;
		Point center;

		private void GestureListener_PinchStarted(object sender, PinchStartedGestureEventArgs e) {
			initialScale = i_scale.ScaleX;

			Point firstTouch = e.GetPosition(g_Picture, 0);
			Point secondTouch = e.GetPosition(g_Picture, 1);
			center = new Point(firstTouch.X + (secondTouch.X - firstTouch.X) / 2.0, firstTouch.Y + (secondTouch.Y - firstTouch.Y) / 2.0);
		}

		private void GestureListener_PinchDelta(object sender, PinchGestureEventArgs e) {

			if (initialScale * e.DistanceRatio > 20 || (initialScale != 1 && e.DistanceRatio == 1) || initialScale * e.DistanceRatio < 1)
				return;

			// if its original size then center it back
			if (e.DistanceRatio <= 1.08) {
				i_scale.CenterY = 0;
				i_scale.CenterY = 0;
				i_translate.X = 0;
				i_translate.Y = 0;
			}

			i_scale.CenterX = center.X;
			i_scale.CenterY = center.Y;

			i_scale.ScaleX = initialScale * e.DistanceRatio;
			i_scale.ScaleY = initialScale * e.DistanceRatio;
		}

		private void GestureListener_DragDelta(object sender, DragDeltaGestureEventArgs e) {
			// if is not touch enabled or the scale is different than 1 then don’t allow moving
			if (i_scale.ScaleX <= 1.1)
				return;
			double centerX = i_scale.CenterX;
			double centerY = i_scale.CenterY;
			double translateX = i_translate.X;
			double translateY = i_translate.Y;
			double scale = i_scale.ScaleX;
			double width = g_Picture.ActualWidth;
			double height = g_Picture.ActualHeight;

			// verify limits to not allow the image to get out of area
			if (centerX - scale * centerX + translateX + e.HorizontalChange < 0 &&
			centerX + scale * (width - centerX) + translateX + e.HorizontalChange > width) {
				i_translate.X += e.HorizontalChange;
			}

			if (centerY - scale * centerY + translateY + e.VerticalChange < 0 &&
			centerY + scale * (height - centerY) + translateY + e.VerticalChange > height) {
				i_translate.Y += e.VerticalChange;
			}

			return;
		}

		private void g_Picture_Tap(object sender, System.Windows.Input.GestureEventArgs e) {

			if (isAddLocked) {
				blinkContinue.Begin();
				return;
			}

			touchPoint = e.GetPosition(g_Selections);
			calculateColor(touchPoint.X, touchPoint.Y);
		}

		private void calculateColor(double mX, double mY) {
			WriteableBitmap wb = new WriteableBitmap((BitmapSource)i_picture.Source);

			int x = (int)Math.Round(mX / i_picture.ActualWidth * (double)wb.PixelWidth);
			int y = (int)Math.Round(mY / i_picture.ActualHeight * (double)wb.PixelHeight);

			GeneralTransform generalTransform = g_Selections.TransformToVisual(i_picture);
			Point coord = generalTransform.Transform(new Point(mX, mY));

			g_Selections.Children.Remove(currentLocator);
			currentLocator = new colorLocator();
			currentLocator.Name = "current" + currentLocatorInc;
			currentLocatorInc++;

			currentLocator.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
			currentLocator.VerticalAlignment = System.Windows.VerticalAlignment.Top;

			Color c;
			if (colorSample == sampleType.AreaSample) {
				int radius = 20;
				int r = 0; int g = 0; int b = 0;
				int cnt = 0;

				for (int i = -radius; i <= radius; i++) {
					//y= sqrt(r^2-x2)
					double y1 = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(i, 2));
					double y2 = -y1;
					for (int j = (int)Math.Ceiling(y2); j < y1; j++) {
						if (coord.Y + i < 0 || coord.Y + i > i_picture.ActualHeight - 1) continue;
						if (coord.X + j < 0 || coord.X + j > i_picture.ActualWidth - 1) continue;
						if (((coord.Y + i) * wb.PixelWidth + (coord.X + j)) > wb.Pixels.Count())
							continue;
						var by = BitConverter.GetBytes(wb.Pixels[((int)coord.Y + i) * wb.PixelWidth + ((int)coord.X + j)]);
						r += by[2]; g += by[1]; b += by[0];
						cnt++;

						//Saving as debug tool to see where the color calculation is done on screen
						//Rectangle rect = new Rectangle();
						//rect.Margin = new Thickness(mX + j, mY + i, 0, 0);
						//rect.Width = 1;
						//rect.Height = 1;
						//rect.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
						//rect.VerticalAlignment = System.Windows.VerticalAlignment.Top;
						//rect.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 126, 255));
						//g_Selections.Children.Add(rect);
					}
				}


				r /= cnt;
				g /= cnt;
				b /= cnt;

				c = Color.FromArgb(255, (byte)r, (byte)g, (byte)b);

				currentLocator.Margin = new Thickness(mX - radius, mY - radius, 0, 0);
				VisualStateManager.GoToState(currentLocator, "Large", false);
			}
			else {
				var bytes = BitConverter.GetBytes(wb.Pixels[(int)coord.Y * wb.PixelWidth + (int)coord.X]);
				c = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);

				int radius = 3;
				currentLocator.Margin = new Thickness(mX - radius, mY - radius, 0, 0);
				VisualStateManager.GoToState(currentLocator, "Small", false);

				GeneralTransform gT = g_Selections.TransformToVisual(i_picture);
				Point screenPos = generalTransform.Transform(new Point(mX, mY));

			}

			namedColor nC = namedColor.findNamedColors(c, namedColor.NAMEDCOLOR_DEVIATION);
			string nC_name = string.Empty;
			if (nC != null)
				nC_name = nC.colorName;
			currentLocator.beginFlash(nC_name, mX, mY);

			HSL hsl = HSL.convertRGB(c);
			if (hsl.L < 0.5)
				currentLocator.Background = new SolidColorBrush(Color.FromArgb(126, 255, 255, 255));
			else
				currentLocator.Background = new SolidColorBrush(Color.FromArgb(126, 0, 0, 0));

			g_Selections.Children.Add(currentLocator);

			tb_sampleType.Text += " r: " + c.R + " g: " + c.G + " b: " + c.B;
			if (colorSample == sampleType.PointSample)
				tb_sampleType.Text = "point sampling    (r: " + c.R + " g: " + c.G + " b: " + c.B + ")";
			else
				tb_sampleType.Text = "area sampling    (r: " + c.R + " g: " + c.G + " b: " + c.B + ")";

			g_colors.Background = new SolidColorBrush(c);
		}

		private void ib_addColor_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			if (currentLocator == null)
				return;

			colorHolder newCh = new colorHolder();
			newCh.Name = "colorHolder" + currentLocatorInc;
			newCh.Background = new SolidColorBrush((Color)g_colors.Background.GetValue(SolidColorBrush.ColorProperty));

			currentLocator.endFlash();
			newCh.Tag = currentLocator;
			currentLocator = null;
			touchPoint = new Point(0, 0);
			colorCollection.Add(newCh);
			newCh.RemoveTap += new RoutedEventHandler(colorHolder_RemoveTap);
			newCh.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(colorHolder_Tap);
			g_colors.ColumnDefinitions.Clear();
			g_colors.Children.Clear();
			g_colors.Background = new SolidColorBrush(Colors.Transparent);

			int columns = colorCollection.Count;
			double gWidth = g_colors.ActualWidth;

			int i = 0;
			foreach (colorHolder ch in colorCollection) {
				ch.SetValue(Grid.ColumnProperty, i);
				ColumnDefinition cd = new ColumnDefinition();
				cd.Width = new GridLength(1, GridUnitType.Auto);
				ch.Width = g_colors.ActualWidth / (2 * columns);
				g_colors.ColumnDefinitions.Add(cd);
				g_colors.Children.Add(ch);
				i++;
			}

			if (colorCollection.Count >= setModel.MAX_COLORS)
				lockAdd();
			else if (colorCollection.Count == 0)
				lockRemove();
			else
				gotoNormal();

			e.Handled = true;
		}

		private colorLocator clTemp;
		void colorHolder_Tap(object sender, System.Windows.Input.GestureEventArgs e) {

			if (clTemp != null) {
				colorLocator cl = (colorLocator)((colorHolder)sender).Tag;
				if (clTemp.Equals(cl)) {
					clTemp.endFlash();
					clTemp = null;
				}
				else {
					clTemp.endFlash();
					clTemp = cl;
					clTemp.beginFlash();
				}
			}
			else {
				clTemp = (colorLocator)((colorHolder)sender).Tag;
				clTemp.beginFlash();
			}

		}

		private colorHolder chTemp;
		void colorHolder_RemoveTap(object sender, RoutedEventArgs e) {
			chTemp = (colorHolder)sender;
			Storyboard sb = MakeDeleteStoryboard(chTemp);
			sb.Completed += new EventHandler(Storyboard_Completed);
			sb.Begin();
			g_colors.Background = new SolidColorBrush(Colors.Transparent);
			chTemp.hideRemove();
			colorCollection.Remove(chTemp);
			if (colorCollection.Count == 0) {
				undoRemoveMode();
			}
		}

		void Storyboard_Completed(object sender, EventArgs e) {
			g_Selections.Children.Remove((colorLocator)chTemp.Tag);
			g_colors.Children.Remove(chTemp);
		}

		private void ib_removeColor_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			gotoRemoveMode();
			e.Handled = true;
		}

		private void ib_removeBack_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			undoRemoveMode();
			e.Handled = true;
		}

		private void ib_areaCapture_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			ib_areaCapture.IsEnabled = false;
			ib_pointCapture.IsEnabled = true;
			colorSample = sampleType.AreaSample;
			tb_sampleType.Text = "area sampling";
			if (touchPoint.X != 0 && touchPoint.Y != 0)
				calculateColor(touchPoint.X, touchPoint.Y);
			e.Handled = true;
		}

		private void ib_pointCapture_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			ib_areaCapture.IsEnabled = true;
			ib_pointCapture.IsEnabled = false;
			colorSample = sampleType.PointSample;
			tb_sampleType.Text = "point sampling";
			if (touchPoint.X != 0 && touchPoint.Y != 0)
				calculateColor(touchPoint.X, touchPoint.Y);
			e.Handled = true;
		}

		private void ib_finish_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			if (colorCollection.Count() == 1) {
				currentColor = (Color)colorCollection[0].Background.GetValue(SolidColorBrush.ColorProperty);
				fillTweakBoard(currentColor);
				VisualStateManager.GoToState(this, "TweakMode", true);
			}
			else {
				setAddColorSetAppBarIcons();
				gotoAddSetMode();
			}
			e.Handled = true;
		}

		private void lockAdd() {
			VisualStateManager.GoToState(this, "AddLocked", true);
			isAddLocked = true;
		}

		private void lockRemove() {
			VisualStateManager.GoToState(this, "RemoveLocked", true);
		}

		private void gotoNormal() {
			VisualStateManager.GoToState(this, "Normal", true);
		}

		private void gotoRemoveMode() {
			g_Selections.Children.Remove(currentLocator);
			currentLocator = null;

			i_scale.CenterY = 0;
			i_scale.CenterY = 0;
			i_scale.ScaleX = 1;
			i_scale.ScaleY = 1;
			i_translate.X = 0;
			i_translate.Y = 0;

			isAddLocked = true;

			VisualStateManager.GoToState(this, "RemoveMode", true);
			Storyboard sb = MakeRemoveModeStoryboard();
			sb.Begin();
		}

		private void undoRemoveMode() {
			if (colorCollection.Count < setModel.MAX_COLORS)
				isAddLocked = false;

			if (colorCollection.Count > 0)
				VisualStateManager.GoToState(this, "Normal", true);
			else
				VisualStateManager.GoToState(this, "RemoveLocked", true);

			if (colorSample == sampleType.PointSample)
				tb_sampleType.Text = "point sampling";
			else
				tb_sampleType.Text = "area sampling";

			Storyboard sb = MakeUndoRemoveModeStoryboard();
			sb.Begin();
		}

		#endregion

		#region Tweak Mode

		private void fillTweakBoard(Color c) {

			TweakSelectionHighlight.Stop();
			Storyboard.SetTarget(TweakSelectionHighlight, C3);
			TweakSelectionHighlight.Begin();
			r_tweakOriginal.Fill = new SolidColorBrush(c);
			r_tweakCurrent.Fill = new SolidColorBrush(c);
			setTweakBoard(c);

		}

		private void Tweak_Tap(object sender, System.Windows.Input.GestureEventArgs e) {

			//clearing select rect
			foreach (System.Windows.Shapes.Path p in g_tweakColors.Children.OfType<System.Windows.Shapes.Path>()) {
				p.Stroke = null;
				p.StrokeThickness = 0;
			}

			//setting select rect to center
			System.Windows.Shapes.Path currentPath = (System.Windows.Shapes.Path)sender;
			currentPath.Stroke = new SolidColorBrush(Colors.White);
			currentPath.StrokeThickness = 2;
			TweakSelectionHighlight.Stop();
			Storyboard.SetTarget(TweakSelectionHighlight, currentPath);
			TweakSelectionHighlight.Begin();

			//setting currentColor
			currentColor = (Color)currentPath.Fill.GetValue(SolidColorBrush.ColorProperty);
			CA_CurrentColor.To = currentColor;
			updateCurrentColor.Begin();
		}

		private void updateCurrentColor_Completed(object sender, EventArgs e) {
			r_tweakCurrent.Fill = new SolidColorBrush(currentColor);
		}

		private void Tweak_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e) {
			//updating tweakboard
			System.Windows.Shapes.Path currentPath = (System.Windows.Shapes.Path)sender;
			currentColor = (Color)currentPath.Fill.GetValue(SolidColorBrush.ColorProperty);
			updateTweakBoard();
		}

		private void SB_Tweak_Completed(object sender, EventArgs e) {
			setTweakBoard(currentColor);
		}

		private void changeTweakBoard(Color c) {
			turnOffNamedColor(c);

			HSL hsl = HSL.convertRGB(c);

			SB_Tweak.Stop();

			currentColor = c;
			CA_CurrentColor.To = currentColor;
			updateCurrentColor.Begin();

			if (hsl.S + TweakStep * 2 <= HSL.S_MAX) {
				if (hsl.L - TweakStep * 2 >= 0) {
					CA_A1.SetValue(ColorAnimation.ToProperty, hsl.addL(-TweakStep * 2).addS(TweakStep * 2).toRGB());
					A1.IsHitTestVisible = true;
				}
				else {
					CA_A1.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					A1.IsHitTestVisible = false;
				}
				if (hsl.L - TweakStep >= 0) {
					CA_A2.SetValue(ColorAnimation.ToProperty, hsl.addL(-TweakStep).addS(TweakStep * 2).toRGB());
					A1.IsHitTestVisible = true;
				}
				else {
					CA_A2.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					A2.IsHitTestVisible = false;
				}

				CA_A3.SetValue(ColorAnimation.ToProperty, hsl.addL(0).addS(TweakStep * 2).toRGB());
				A3.IsHitTestVisible = true;

				if (hsl.L + TweakStep <= HSL.L_MAX) {
					CA_A4.SetValue(ColorAnimation.ToProperty, hsl.addL(TweakStep).addS(TweakStep * 2).toRGB());
					A4.IsHitTestVisible = true;
				}
				else {
					CA_A4.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					A4.IsHitTestVisible = false;
				}
				if (hsl.L + TweakStep * 2 <= HSL.L_MAX) {
					CA_A5.SetValue(ColorAnimation.ToProperty, hsl.addL(TweakStep * 2).addS(TweakStep * 2).toRGB());
					A5.IsHitTestVisible = true;
				}
				else {
					CA_A5.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					A5.IsHitTestVisible = false;
				}

				DA_TL1.SetValue(DoubleAnimation.ToProperty, 0.0);
			}
			else {
				CA_A1.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_A2.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_A3.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_A4.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_A5.SetValue(ColorAnimation.ToProperty, Colors.Transparent);

				DA_TL1.SetValue(DoubleAnimation.ToProperty, 1.0);

				A1.IsHitTestVisible = false;
				A2.IsHitTestVisible = false;
				A3.IsHitTestVisible = false;
				A4.IsHitTestVisible = false;
				A5.IsHitTestVisible = false;

			}

			if (hsl.S + TweakStep <= HSL.S_MAX) {
				if (hsl.L - TweakStep * 2 >= 0) {
					CA_B1.SetValue(ColorAnimation.ToProperty, hsl.addL(-TweakStep * 2).addS(TweakStep).toRGB());
					B1.IsHitTestVisible = true;
				}
				else {
					CA_B1.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					B1.IsHitTestVisible = false;
				}
				if (hsl.L - TweakStep >= 0) {
					CA_B2.SetValue(ColorAnimation.ToProperty, hsl.addL(-TweakStep).addS(TweakStep).toRGB());
					B2.IsHitTestVisible = true;
				}
				else {
					CA_B2.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					B2.IsHitTestVisible = false;
				}

				CA_B3.SetValue(ColorAnimation.ToProperty, hsl.addL(0).addS(TweakStep).toRGB());
				B3.IsHitTestVisible = true;

				if (hsl.L + TweakStep <= HSL.L_MAX) {
					CA_B4.SetValue(ColorAnimation.ToProperty, hsl.addL(TweakStep).addS(TweakStep).toRGB());
					B4.IsHitTestVisible = true;
				}
				else {
					CA_B4.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					B4.IsHitTestVisible = false;
				}
				if (hsl.L + TweakStep * 2 <= HSL.L_MAX) {
					CA_B5.SetValue(ColorAnimation.ToProperty, hsl.addL(TweakStep * 2).addS(TweakStep).toRGB());
					B5.IsHitTestVisible = true;
				}
				else {
					CA_B5.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					B5.IsHitTestVisible = false;
				}

				DA_TL2.SetValue(DoubleAnimation.ToProperty, 0.0);
			}
			else {
				CA_B1.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_B2.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_B3.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_B4.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_B5.SetValue(ColorAnimation.ToProperty, Colors.Transparent);

				DA_TL2.SetValue(DoubleAnimation.ToProperty, 1.0);

				B1.IsHitTestVisible = false;
				B2.IsHitTestVisible = false;
				B3.IsHitTestVisible = false;
				B4.IsHitTestVisible = false;
				B5.IsHitTestVisible = false;
			}

			if (hsl.L - TweakStep * 2 >= 0) {
				CA_C1.SetValue(ColorAnimation.ToProperty, hsl.addL(-TweakStep * 2).addS(0).toRGB());
				DA_LL1.SetValue(DoubleAnimation.ToProperty, 0.0);
				C1.IsHitTestVisible = true;
			}
			else {
				CA_C1.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				DA_LL1.SetValue(DoubleAnimation.ToProperty, 1.0);
				C1.IsHitTestVisible = false;
			}
			if (hsl.L - TweakStep >= 0) {
				CA_C2.SetValue(ColorAnimation.ToProperty, hsl.addL(-TweakStep).addS(0).toRGB());
				DA_LL2.SetValue(DoubleAnimation.ToProperty, 0.0);
				C2.IsHitTestVisible = true;
			}
			else {
				CA_C2.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				DA_LL2.SetValue(DoubleAnimation.ToProperty, 1.0);
				C2.IsHitTestVisible = false;
			}

			CA_C3.SetValue(ColorAnimation.ToProperty, c);

			if (hsl.L + TweakStep <= HSL.L_MAX) {
				CA_C4.SetValue(ColorAnimation.ToProperty, hsl.addL(TweakStep).addS(0).toRGB());
				DA_RL2.SetValue(DoubleAnimation.ToProperty, 0.0);
				C4.IsHitTestVisible = true;
			}
			else {
				CA_C4.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				DA_RL2.SetValue(DoubleAnimation.ToProperty, 1.0);
				C4.IsHitTestVisible = false;
			}
			if (hsl.L + TweakStep * 2 <= HSL.L_MAX) {
				CA_C5.SetValue(ColorAnimation.ToProperty, hsl.addL(TweakStep * 2).addS(0).toRGB());
				DA_RL1.SetValue(DoubleAnimation.ToProperty, 0.0);
				C5.IsHitTestVisible = true;
			}
			else {
				CA_C5.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				DA_RL1.SetValue(DoubleAnimation.ToProperty, 1.0);
				C5.IsHitTestVisible = false;
			}


			if (hsl.S - TweakStep >= 0) {
				if (hsl.L - TweakStep * 2 >= 0) {
					CA_D1.SetValue(ColorAnimation.ToProperty, hsl.addL(-TweakStep * 2).addS(-TweakStep).toRGB());
					D1.IsHitTestVisible = true;
				}
				else {
					CA_D1.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					D1.IsHitTestVisible = false;
				}
				if (hsl.L - TweakStep >= 0) {
					CA_D2.SetValue(ColorAnimation.ToProperty, hsl.addL(-TweakStep).addS(-TweakStep).toRGB());
					D2.IsHitTestVisible = true;
				}
				else {
					CA_D2.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					D2.IsHitTestVisible = false;
				}

				CA_D3.SetValue(ColorAnimation.ToProperty, hsl.addL(0).addS(-TweakStep).toRGB());
				D3.IsHitTestVisible = true;

				if (hsl.L + TweakStep <= HSL.L_MAX) {
					CA_D4.SetValue(ColorAnimation.ToProperty, hsl.addL(TweakStep).addS(-TweakStep).toRGB());
					D4.IsHitTestVisible = true;
				}
				else {
					CA_D4.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					D4.IsHitTestVisible = false;
				}
				if (hsl.L + TweakStep * 2 <= HSL.L_MAX) {
					CA_D5.SetValue(ColorAnimation.ToProperty, hsl.addL(TweakStep * 2).addS(-TweakStep).toRGB());
					D5.IsHitTestVisible = true;
				}
				else {
					CA_D5.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					D5.IsHitTestVisible = false;
				}

				DA_BL2.SetValue(DoubleAnimation.ToProperty, 0.0);
			}
			else {
				CA_D1.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_D2.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_D3.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_D4.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_D5.SetValue(ColorAnimation.ToProperty, Colors.Transparent);

				DA_BL2.SetValue(DoubleAnimation.ToProperty, 1.0);

				D1.IsHitTestVisible = false;
				D2.IsHitTestVisible = false;
				D3.IsHitTestVisible = false;
				D4.IsHitTestVisible = false;
				D5.IsHitTestVisible = false;
			}

			if (hsl.S - TweakStep * 2 >= 0) {
				if (hsl.L - TweakStep * 2 >= 0) {
					CA_E1.SetValue(ColorAnimation.ToProperty, hsl.addL(-TweakStep * 2).addS(-TweakStep * 2).toRGB());
					E1.IsHitTestVisible = true;
				}
				else {
					CA_E1.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					E1.IsHitTestVisible = false;
				}
				if (hsl.L - TweakStep >= 0) {
					CA_E2.SetValue(ColorAnimation.ToProperty, hsl.addL(-TweakStep).addS(-TweakStep * 2).toRGB());
					E2.IsHitTestVisible = true;
				}
				else {
					CA_E2.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					E2.IsHitTestVisible = false;
				}

				CA_E3.SetValue(ColorAnimation.ToProperty, hsl.addL(0).addS(-TweakStep * 2).toRGB());
				E3.IsHitTestVisible = true;

				if (hsl.L + TweakStep <= HSL.L_MAX) {
					CA_E4.SetValue(ColorAnimation.ToProperty, hsl.addL(TweakStep).addS(-TweakStep * 2).toRGB());
					E4.IsHitTestVisible = true;
				}
				else {
					CA_E4.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					E4.IsHitTestVisible = false;
				}
				if (hsl.L + TweakStep * 2 <= HSL.L_MAX) {
					CA_E5.SetValue(ColorAnimation.ToProperty, hsl.addL(TweakStep * 2).addS(-TweakStep * 2).toRGB());
					E5.IsHitTestVisible = true;
				}
				else {
					CA_E5.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
					E5.IsHitTestVisible = false;
				}

				DA_BL1.SetValue(DoubleAnimation.ToProperty, 0.0);
			}
			else {
				CA_E1.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_E2.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_E3.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_E4.SetValue(ColorAnimation.ToProperty, Colors.Transparent);
				CA_E5.SetValue(ColorAnimation.ToProperty, Colors.Transparent);

				DA_BL1.SetValue(DoubleAnimation.ToProperty, 1.0);

				E1.IsHitTestVisible = false;
				E2.IsHitTestVisible = false;
				E3.IsHitTestVisible = false;
				E4.IsHitTestVisible = false;
				E5.IsHitTestVisible = false;

			}

			SB_Tweak.Begin();

		}

		private void setTweakBoard(Color c) {
			turnOnNamedColor(c);

			HSL hsl = HSL.convertRGB(c);

			if (hsl.S + TweakStep * 2 <= HSL.S_MAX) {
				if (hsl.L - TweakStep * 2 >= 0) {
					A1.Fill = new SolidColorBrush(hsl.addL(-TweakStep * 2).addS(TweakStep * 2).toRGB());
				}
				else {
					A1.Fill = new SolidColorBrush(Colors.Transparent);
				}
				if (hsl.L - TweakStep >= 0) {
					A2.Fill = new SolidColorBrush(hsl.addL(-TweakStep).addS(TweakStep * 2).toRGB());
				}
				else {
					A2.Fill = new SolidColorBrush(Colors.Transparent);
				}

				A3.Fill = new SolidColorBrush(hsl.addL(0).addS(TweakStep * 2).toRGB());

				if (hsl.L + TweakStep <= HSL.L_MAX) {
					A4.Fill = new SolidColorBrush(hsl.addL(TweakStep).addS(TweakStep * 2).toRGB());
				}
				else {
					A4.Fill = new SolidColorBrush(Colors.Transparent);
				}
				if (hsl.L + TweakStep * 2 <= HSL.L_MAX) {
					A5.Fill = new SolidColorBrush(hsl.addL(TweakStep * 2).addS(TweakStep * 2).toRGB());
				}
				else {
					A5.Fill = new SolidColorBrush(Colors.Transparent);
				}

				p_top_L1.Opacity = 0.0;
			}
			else {
				A1.Fill = new SolidColorBrush(Colors.Transparent);
				A2.Fill = new SolidColorBrush(Colors.Transparent);
				A3.Fill = new SolidColorBrush(Colors.Transparent);
				A4.Fill = new SolidColorBrush(Colors.Transparent);
				A5.Fill = new SolidColorBrush(Colors.Transparent);

				p_top_L1.Opacity = 1.0;
			}

			if (hsl.S + TweakStep <= HSL.S_MAX) {
				if (hsl.L - TweakStep * 2 >= 0) {
					B1.Fill = new SolidColorBrush(hsl.addL(-TweakStep * 2).addS(TweakStep).toRGB());
				}
				else {
					B1.Fill = new SolidColorBrush(Colors.Transparent);
				}
				if (hsl.L - TweakStep >= 0) {
					B2.Fill = new SolidColorBrush(hsl.addL(-TweakStep).addS(TweakStep).toRGB());
				}
				else {
					B2.Fill = new SolidColorBrush(Colors.Transparent);
				}

				B3.Fill = new SolidColorBrush(hsl.addL(0).addS(TweakStep).toRGB());

				if (hsl.L + TweakStep <= HSL.L_MAX) {
					B4.Fill = new SolidColorBrush(hsl.addL(TweakStep).addS(TweakStep).toRGB());
				}
				else {
					B4.Fill = new SolidColorBrush(Colors.Transparent);
				}
				if (hsl.L + TweakStep * 2 <= HSL.L_MAX) {
					B5.Fill = new SolidColorBrush(hsl.addL(TweakStep * 2).addS(TweakStep).toRGB());
				}
				else {
					B5.Fill = new SolidColorBrush(Colors.Transparent);
				}

				p_top_L2.Opacity = 0.0;
			}
			else {
				B1.Fill = new SolidColorBrush(Colors.Transparent);
				B2.Fill = new SolidColorBrush(Colors.Transparent);
				B3.Fill = new SolidColorBrush(Colors.Transparent);
				B4.Fill = new SolidColorBrush(Colors.Transparent);
				B5.Fill = new SolidColorBrush(Colors.Transparent);

				p_top_L2.Opacity = 1.0;
			}

			if (hsl.L - TweakStep * 2 >= 0) {
				C1.Fill = new SolidColorBrush(hsl.addL(-TweakStep * 2).addS(0).toRGB());
				p_left_L1.Opacity = 0.0;
			}
			else {
				C1.Fill = new SolidColorBrush(Colors.Transparent);
				p_left_L1.Opacity = 1.0;
			}
			if (hsl.L - TweakStep >= 0) {
				C2.Fill = new SolidColorBrush(hsl.addL(-TweakStep).addS(0).toRGB());
				p_left_L2.Opacity = 0.0;
			}
			else {
				C2.Fill = new SolidColorBrush(Colors.Transparent);
				p_left_L2.Opacity = 1.0;
			}

			C3.Fill = new SolidColorBrush(c);

			if (hsl.L + TweakStep <= HSL.L_MAX) {
				C4.Fill = new SolidColorBrush(hsl.addL(TweakStep).addS(0).toRGB());
				p_right_L2.Opacity = 0.0;
			}
			else {
				C4.Fill = new SolidColorBrush(Colors.Transparent);
				p_right_L2.Opacity = 1.0;
			}
			if (hsl.L + TweakStep * 2 <= HSL.L_MAX) {
				C5.Fill = new SolidColorBrush(hsl.addL(TweakStep * 2).addS(0).toRGB());
				p_right_L1.Opacity = 0.0;
			}
			else {
				C5.Fill = new SolidColorBrush(Colors.Transparent);
				p_right_L1.Opacity = 1.0;
			}


			if (hsl.S - TweakStep >= 0) {
				if (hsl.L - TweakStep * 2 >= 0) {
					D1.Fill = new SolidColorBrush(hsl.addL(-TweakStep * 2).addS(-TweakStep).toRGB());
				}
				else {
					D1.Fill = new SolidColorBrush(Colors.Transparent);
				}
				if (hsl.L - TweakStep >= 0) {
					D2.Fill = new SolidColorBrush(hsl.addL(-TweakStep).addS(-TweakStep).toRGB());
				}
				else {
					D2.Fill = new SolidColorBrush(Colors.Transparent);
				}

				D3.Fill = new SolidColorBrush(hsl.addL(0).addS(-TweakStep).toRGB());

				if (hsl.L + TweakStep <= HSL.L_MAX) {
					D4.Fill = new SolidColorBrush(hsl.addL(TweakStep).addS(-TweakStep).toRGB());
				}
				else {
					D4.Fill = new SolidColorBrush(Colors.Transparent);
				}
				if (hsl.L + TweakStep * 2 <= HSL.L_MAX) {
					D5.Fill = new SolidColorBrush(hsl.addL(TweakStep * 2).addS(-TweakStep).toRGB());
				}
				else {
					D5.Fill = new SolidColorBrush(Colors.Transparent);
				}

				p_bottom_L2.Opacity = 0.0;
			}
			else {
				D1.Fill = new SolidColorBrush(Colors.Transparent);
				D2.Fill = new SolidColorBrush(Colors.Transparent);
				D3.Fill = new SolidColorBrush(Colors.Transparent);
				D4.Fill = new SolidColorBrush(Colors.Transparent);
				D5.Fill = new SolidColorBrush(Colors.Transparent);

				p_bottom_L2.Opacity = 1.0;
			}

			if (hsl.S - TweakStep * 2 >= 0) {
				if (hsl.L - TweakStep * 2 >= 0) {
					E1.Fill = new SolidColorBrush(hsl.addL(-TweakStep * 2).addS(-TweakStep * 2).toRGB());
				}
				else {
					E1.Fill = new SolidColorBrush(Colors.Transparent);
				}
				if (hsl.L - TweakStep >= 0) {
					E2.Fill = new SolidColorBrush(hsl.addL(-TweakStep).addS(-TweakStep * 2).toRGB());
				}
				else {
					E2.Fill = new SolidColorBrush(Colors.Transparent);
				}

				E3.Fill = new SolidColorBrush(hsl.addL(0).addS(-TweakStep * 2).toRGB());

				if (hsl.L + TweakStep <= HSL.L_MAX) {
					E4.Fill = new SolidColorBrush(hsl.addL(TweakStep).addS(-TweakStep * 2).toRGB());
				}
				else {
					E4.Fill = new SolidColorBrush(Colors.Transparent);
				}
				if (hsl.L + TweakStep * 2 <= HSL.L_MAX) {
					E5.Fill = new SolidColorBrush(hsl.addL(TweakStep * 2).addS(-TweakStep * 2).toRGB());
				}
				else {
					E5.Fill = new SolidColorBrush(Colors.Transparent);
				}

				p_bottom_L1.Opacity = 0.0;
			}
			else {
				E1.Fill = new SolidColorBrush(Colors.Transparent);
				E2.Fill = new SolidColorBrush(Colors.Transparent);
				E3.Fill = new SolidColorBrush(Colors.Transparent);
				E4.Fill = new SolidColorBrush(Colors.Transparent);
				E5.Fill = new SolidColorBrush(Colors.Transparent);

				p_bottom_L1.Opacity = 1.0;
			}

		}

		private void g_namedColor_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			//updating tweakboard
			currentColor = (Color)g_namedColor.Background.GetValue(SolidColorBrush.ColorProperty);
			updateTweakBoard();
		}

		private void r_tweakOriginal_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			//updating tweakboard
			currentColor = (Color)r_tweakOriginal.Fill.GetValue(SolidColorBrush.ColorProperty);
			updateTweakBoard();
		}

		//update tweak board using currentColor
		private void updateTweakBoard() {
			//clearing select rect
			foreach (System.Windows.Shapes.Path p in g_tweakColors.Children.OfType<System.Windows.Shapes.Path>()) {
				p.Stroke = null;
				p.StrokeThickness = 0;
			}

			//moving select rect to center
			C3.Stroke = new SolidColorBrush(Colors.White);
			C3.StrokeThickness = 2;
			TweakSelectionHighlight.Stop();
			Storyboard.SetTarget(TweakSelectionHighlight, C3);
			TweakSelectionHighlight.Begin();

			changeTweakBoard(currentColor);
		}

		private void ib_add_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			r_addColor.Fill = new SolidColorBrush(currentColor);
			namedColor nc = namedColor.findNamedColors(currentColor, namedColor.NAMEDCOLOR_DEVIATION);

			string _title = "";
			string _desc = "";
			if (nc != null) {
				//if the deviation is basically greater than 0 then add * to the name
				if (nc.deviation > 0.1)
					setNameAndDescription(out _title, out _desc, nc.colorName.Trim() + "*", string.Format("Almost {0}. This color deviates {1} points from the actual named color whose values are: R: {2} G: {3} B: {4}", nc.colorName.Trim(), Math.Round(nc.deviation, 2), nc.color.R, nc.color.G, nc.color.B));
				else
					setNameAndDescription(out _title, out _desc, nc.colorName.Trim(), "");
			}
			else
				setNameAndDescription(out _title, out _desc, RGB.ColorToHex(currentColor), "");


			tb_addColor_name.Text = _title;
			tb_addColor_description.Text = _desc;

			setAddColorAppBarIcons();
			VisualStateManager.GoToState(this, "AddColorMode", false);
			ib_overlay.Visibility = System.Windows.Visibility.Collapsed;

		}

		private void ib_addSet_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			setAddColorSetAppBarIcons();

			gotoAddSetMode();
		}

		#endregion

		#region add Set Mode

		private void gotoAddSetMode() {
			ib_overlay.Visibility = System.Windows.Visibility.Collapsed;
			fillSetRect();

			if (isManual) {
				cb_keepImage.IsChecked = false;
				cb_keepImage.Visibility = System.Windows.Visibility.Collapsed;
			}
			else {
				cb_keepImage.IsChecked = true;
				cb_keepImage.Visibility = System.Windows.Visibility.Visible;
			}
			VisualStateManager.GoToState(this, "AddSetMode", false);
		}

		private void fillSetRect() {

			ObservableCollection<setColorModel> palette = new ObservableCollection<setColorModel>();

			if (isManual) {

				Guid setid = Guid.NewGuid();

				namedColor nC = namedColor.findNamedColors(currentColor, namedColor.NAMEDCOLOR_DEVIATION);

				string _title = "";
				string _desc = "";
				if (nC != null) {
					if (nC.deviation > 0.1)
						setNameAndDescription(out _title, out  _desc, nC.colorName + "*", string.Format("Almost {0}. This color deviates {1} points from the actual named color whose values are: R: {2} G: {3} B: {4}", nC.colorName, Math.Round(nC.deviation, 2), nC.color.R, nC.color.G, nC.color.B));
					else
						setNameAndDescription(out _title, out  _desc, nC.colorName, "");
				}
				else
					setNameAndDescription(out _title, out  _desc, RGB.ColorToHex(currentColor), "");

				palette.Add(setColorModel.createNewSetColorModel(0, _title, _desc, currentColor, setid));

			}
			else {

				int i = 0;
				Guid setid = Guid.NewGuid();
				foreach (colorHolder ch in g_colors.Children.OfType<colorHolder>()) {

					Color c = (Color)ch.Background.GetValue(SolidColorBrush.ColorProperty);
					namedColor nC = namedColor.findNamedColors(c, namedColor.NAMEDCOLOR_DEVIATION);

					string _title = "";
					string _desc = "";
					if (nC != null) {
						if (nC.deviation > 0.1)
							setNameAndDescription(out _title, out  _desc, nC.colorName + "*", string.Format("Almost {0}. This color deviates {1} points from the actual named color whose values are: R: {2} G: {3} B: {4}", nC.colorName, Math.Round(nC.deviation, 2), nC.color.R, nC.color.G, nC.color.B));
						else
							setNameAndDescription(out _title, out  _desc, nC.colorName, "");
					}
					else
						setNameAndDescription(out _title, out  _desc, RGB.ColorToHex(c), "");

					palette.Add(setColorModel.createNewSetColorModel(i, _title, _desc, c, setid));

					i++;
				}

			}

			sr_setColors.setColorPalette = palette;
			sr_setColors.load();
		}

		#endregion

		#region Storyboards

		//reference http://www.codeproject.com/Articles/153455/Windows-Phone-7-Animations-Alternatives-Performanc
		private Storyboard MakeRemoveModeStoryboard() {
			Storyboard sb = new Storyboard();
			sb.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 300));
			int cnt = g_colors.Children.Count;
			double width = g_colors.ActualWidth;
			double itemWidth = width / cnt;
			if (itemWidth < 100) {
				itemWidth = 100;
				sv_colors.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
			}
			foreach (colorHolder ch in g_colors.Children) {
				sb.Children.Add(CreateWidthAnimation(ch, itemWidth));
			}
			return sb;
		}

		private Storyboard MakeUndoRemoveModeStoryboard() {
			Storyboard sb = new Storyboard();
			sb.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 300));
			int cnt = g_colors.Children.Count;
			double width = g_colors.ActualWidth;
			double itemWidth = g_colors.ActualWidth / (2 * g_colors.Children.Count());
			sv_colors.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
			foreach (colorHolder ch in g_colors.Children) {
				sb.Children.Add(CreateWidthAnimation(ch, itemWidth));
			}
			return sb;
		}

		private Storyboard MakeDeleteStoryboard(colorHolder ch) {
			Storyboard sb = new Storyboard();
			sb.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 300));
			int cnt = g_colors.Children.Count;
			double width = g_colors.ActualWidth;
			double itemWidth = 0;
			sb.Children.Add(CreateWidthAnimation(ch, itemWidth));
			return sb;
		}

		private Timeline CreateWidthAnimation(UIElement target, double w) {
			DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
			Storyboard.SetTargetProperty(animation, new PropertyPath("FrameworkElement.Width"));
			Storyboard.SetTarget(animation, target);

			KeyTime kt1 = KeyTime.FromTimeSpan(TimeSpan.Zero);
			KeyTime kt2 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 300));

			animation.KeyFrames.Add(new EasingDoubleKeyFrame() {
				KeyTime = kt2,
				Value = w
			});

			return animation;
		}

		#endregion

		#region Name Colors

		private void turnOnNamedColor(Color c) {
			namedColor closestNamedColor = namedColor.findNamedColors(c, namedColor.NAMEDCOLOR_DEVIATION);

			if (closestNamedColor != null) {
				tb_namedColor.Text = closestNamedColor.colorName;
				g_namedColor.Background = closestNamedColor.colorBrush;
				VisualStateManager.GoToState(this, "tweakNamedColor", true);
			}
		}

		private void turnOffNamedColor(Color c) {
			namedColor closestNamedColor = namedColor.findNamedColors(c, namedColor.NAMEDCOLOR_DEVIATION);

			if (closestNamedColor != null) {
				if ((closestNamedColor.colorName) != (tb_namedColor.Text))
					VisualStateManager.GoToState(this, "tweakNormal", true);
			}
			else
				VisualStateManager.GoToState(this, "tweakNormal", true);
		}

		#endregion

		#region AppBar

		private void setAddColorAppBarIcons() {

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBarIconButton abib_addColor = new ApplicationBarIconButton(new Uri("/Images/light/add.png", UriKind.Relative));
			abib_addColor.Text = "add Color";
			abib_addColor.Click += new EventHandler(abib_addColor_Click);

			ApplicationBar.Buttons.Add(abib_addColor);

		}

		private void setAddColorSetAppBarIcons() {

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBarIconButton abib_addColorSet = new ApplicationBarIconButton(new Uri("/Images/light/addSet.png", UriKind.Relative));
			abib_addColorSet.Text = "add Set";
			abib_addColorSet.Click += new EventHandler(abib_addColorSet_Click);

			ApplicationBar.Buttons.Add(abib_addColorSet);

		}

		private void clearAppBar() {
			ApplicationBar.IsVisible = false;
			ApplicationBar.Buttons.Clear();
		}

		void abib_addColorSet_Click(object sender, EventArgs e) {

			setModel sm = new setModel() {
				Id = sr_setColors.setColorPalette[0].Set_Id,
				Title = tb_addSet_name.Text.Trim(),
				Description = tb_addSet_description.Text.Trim(),
				setColorPalette = sr_setColors.setColorPalette,
				DateCreated = DateTime.Now,
				LastUpdated = DateTime.Now,
				Snap = false,
				IsUploaded = false,
			};

			if ((bool)cb_keepImage.IsChecked && !isManual) {
				MemoryStream ms = new MemoryStream();
				WriteableBitmap wb = new WriteableBitmap((BitmapSource)i_picture.Source);
				wb.SaveJpeg(ms, wb.PixelWidth, wb.PixelHeight, 0, 100);
				sm.Picture = new pictureModel() {
					Img = ms.ToArray(),
					SetId = sm.Id
				};
			}

			sm.saveSet();

			App.returnToMain = true;
			NavigationService.GoBack();
		}

		void abib_addColor_Click(object sender, EventArgs e) {
			colorModel cm = new colorModel() {
				Id = Guid.NewGuid(),
				Title = tb_addColor_name.Text.Trim(),
				Description = tb_addColor_description.Text.Trim(),
				ColorBrush = new SolidColorBrush(currentColor),
				IsUploaded = false
			};

			cm.saveColor(true);

			App.returnToMain = true;
			NavigationService.GoBack();
		}

		#endregion

		private void setNameAndDescription(out string _title, out string _desc, string title, string desc) {

			App.autoNameOptionType autoNameOption = StorageHelper.loadSetting<App.autoNameOptionType>("AutoNameOptions");
			if (autoNameOption == App.autoNameOptionType.all) {
				_title = title;
				_desc = desc;
			}
			else if (autoNameOption == App.autoNameOptionType.name) {
				_title = title;
				_desc = "";
			}
			else if (autoNameOption == App.autoNameOptionType.desc) {
				_title = "";
				_desc = desc;
			}
			else if (autoNameOption == App.autoNameOptionType.none) {
				_title = "";
				_desc = "";
			}
			else {
				_title = title;
				_desc = desc;
			}

		}

		private void setOverlayImageButtonVisibility() {
			switch (StorageHelper.loadSetting<App.helpVisibilityType>("helpVisibilityOptions")) {
				case App.helpVisibilityType.hide:
					ib_overlay.Visibility = System.Windows.Visibility.Collapsed;
					r_overlay.Visibility = System.Windows.Visibility.Collapsed;
					break;
				case App.helpVisibilityType.show:
					ib_overlay.Visibility = System.Windows.Visibility.Visible;
					r_overlay.Visibility = System.Windows.Visibility.Visible;
					break;
			}
		}

		private void ib_overlay_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			FrameworkElement oth = g_grids;

			if (g_Tweak.Visibility == System.Windows.Visibility.Visible) {
				o_tweak.objectToHide = oth;
				o_tweak.addOverlay(new tweakOverlay());
				p_tweak_overlay.IsOpen = true;
			}
			else {
				o_image.objectToHide = oth;
				o_image.addOverlay(new selectorOverlay());
				p_image_overlay.IsOpen = true;
			}

		}

		void p_image_overlay_Closed(object sender, EventArgs e) {
			//nothing to do, appbar should already be hidden	
		}

		void p_tweak_overlay_Closed(object sender, EventArgs e) {
			//nothing to do, appbar should already be hidden	
		}

		void p_manual_overlay_Closed(object sender, EventArgs e) {
			//TODO:
		}

		protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e) {

			e.Cancel = true;

			if (p_image_overlay.IsOpen)
				p_image_overlay.IsOpen = false;
			else if (p_tweak_overlay.IsOpen)
				p_tweak_overlay.IsOpen = false;
			else if (g_addColor.Visibility == System.Windows.Visibility.Visible) {
				VisualStateManager.GoToState(this, "TweakMode", false);
				ib_overlay.Visibility = System.Windows.Visibility.Visible;
				clearAppBar();

			}
			else if (g_addColorSet.Visibility == System.Windows.Visibility.Visible) {
				if (colorCollection.Count == 1 || isManual)
					VisualStateManager.GoToState(this, "TweakMode", false);
				else
					VisualStateManager.GoToState(this, "SelectionMode", false);
				ib_overlay.Visibility = System.Windows.Visibility.Visible;
				clearAppBar();

			}
			else if (g_Tweak.Visibility == System.Windows.Visibility.Visible) {
				if (isManual)
					e.Cancel = false;
				else
					VisualStateManager.GoToState(this, "SelectionMode", false);
			}
			else if (g_Selector.Visibility == System.Windows.Visibility.Visible)
				e.Cancel = false;
			else {
				if (chooser != null && chooser.GetType() == typeof(colorChooserTask)) {
					if (((colorChooserTask)chooser)._Popup.IsOpen) {
						((colorChooserTask)chooser)._Popup.IsOpen = false;
						e.Cancel = false;
					}
				}
			}

		}




	}
}