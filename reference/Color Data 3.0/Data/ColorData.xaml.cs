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
using Color_Data_3._0.Classes.ColorClasses;
using Microsoft.Phone.Shell;
using Color_Data_3._0.Selectors;
using Color_Data_3._0.Support;

namespace Color_Data_3._0.Data {
	public partial class ColorData : PhoneApplicationPage {

		private colorModel cm;

		private enum AppBarState {
			None,
			Hide,
			Standard,
			Edit,
			Add,
			AddToSet
		}

		ApplicationBarIconButton abib_edit;
		ApplicationBarIconButton abib_editAccept;
		ApplicationBarIconButton abib_add;
		ApplicationBarIconButton abib_addToSet;

		private AppBarState AppBarIcons = AppBarState.None;

		public ColorData() {
			InitializeComponent();

			abib_edit = new ApplicationBarIconButton(new Uri("/Images/light/edit.png", UriKind.Relative));
			abib_edit.Text = "edit";
			abib_edit.Click += new EventHandler(abib_edit_Click);

			abib_editAccept = new ApplicationBarIconButton(new Uri("/Images/light/check.png", UriKind.Relative));
			abib_editAccept.Text = "accept";
			abib_editAccept.Click += new EventHandler(abib_editAccept_Click);

			abib_add = new ApplicationBarIconButton(new Uri("/Images/light/add.png", UriKind.Relative));
			abib_add.Text = "add";
			abib_add.Click += new EventHandler(abib_add_Click);

			abib_addToSet = new ApplicationBarIconButton(new Uri("/Images/light/addSet.png", UriKind.Relative));
			abib_addToSet.Text = "add set";
			abib_addToSet.Click += new EventHandler(abib_addSet_Click);

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

			if (App.currentColor.GetType() == typeof(colorModel)) {
				cm = (colorModel)App.currentColor;
				setContentMenus_Color();
			}
			else if (App.currentColor.GetType() == typeof(setColorModel)) {
				cm = (colorModel)App.currentColor;
				setContentMenus_SetColor();
			}
			else
				throw new InvalidCastException("App.currentColor was not a colorModel object or a setColorModel object when trying to navigate to the colorData page");

			Color currentColor = (Color)cm.ColorBrush.GetValue(SolidColorBrush.ColorProperty);
			SetCurrentColorValues(currentColor);

			SetComplementValues(currentColor);
			SetSplitComplementValues(currentColor);
			SetTriadValues(currentColor);
			SetAnalogousValues(currentColor);
			SetMonochromeValues(currentColor);

			PanoramaItem pi = pano.SelectedItem as PanoramaItem;
			if (pi == null || pi.Equals(pi_color))
				setStandardAppBarIcons();
			else
				clearAppBarIcons();

			setOverlayImageButtonVisibility();
		}

		protected void setContentMenus_Color() {
			ContextMenu cm_c = ContextMenuService.GetContextMenu(scb_complement);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Collapsed;
			ContextMenu cm_sc1 = ContextMenuService.GetContextMenu(scb_splitComplement1);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Collapsed;
			ContextMenu cm_sc2 = ContextMenuService.GetContextMenu(scb_splitComplement2);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Collapsed;
			ContextMenu cm_t1 = ContextMenuService.GetContextMenu(scb_triad1);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Collapsed;
			ContextMenu cm_t2 = ContextMenuService.GetContextMenu(scb_triad2);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Collapsed;
			ContextMenu cm_a1 = ContextMenuService.GetContextMenu(scb_analogous1);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Collapsed;
			ContextMenu cm_a2 = ContextMenuService.GetContextMenu(scb_analogous2);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Collapsed;
			ContextMenu cm_m1 = ContextMenuService.GetContextMenu(scb_monochromatic1);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Collapsed;
			ContextMenu cm_m2 = ContextMenuService.GetContextMenu(scb_monochromatic2);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Collapsed;
			ContextMenu cm_m3 = ContextMenuService.GetContextMenu(scb_monochromatic3);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Collapsed;
			ContextMenu cm_m4 = ContextMenuService.GetContextMenu(scb_monochromatic4);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Collapsed;
			ContextMenu cm_m5 = ContextMenuService.GetContextMenu(scb_monochromatic5);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Collapsed;

		}

		protected void setContentMenus_SetColor() {
			ContextMenu cm_c = ContextMenuService.GetContextMenu(scb_complement);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Visible;
			ContextMenu cm_sc1 = ContextMenuService.GetContextMenu(scb_splitComplement1);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Visible;
			ContextMenu cm_sc2 = ContextMenuService.GetContextMenu(scb_splitComplement2);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Visible;
			ContextMenu cm_t1 = ContextMenuService.GetContextMenu(scb_triad1);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Visible;
			ContextMenu cm_t2 = ContextMenuService.GetContextMenu(scb_triad2);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Visible;
			ContextMenu cm_a1 = ContextMenuService.GetContextMenu(scb_analogous1);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Visible;
			ContextMenu cm_a2 = ContextMenuService.GetContextMenu(scb_analogous2);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Visible;
			ContextMenu cm_m1 = ContextMenuService.GetContextMenu(scb_monochromatic1);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Visible;
			ContextMenu cm_m2 = ContextMenuService.GetContextMenu(scb_monochromatic2);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Visible;
			ContextMenu cm_m3 = ContextMenuService.GetContextMenu(scb_monochromatic3);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Visible;
			ContextMenu cm_m4 = ContextMenuService.GetContextMenu(scb_monochromatic4);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Visible;
			ContextMenu cm_m5 = ContextMenuService.GetContextMenu(scb_monochromatic5);
			((MenuItem)cm_c.Items[1]).Visibility = System.Windows.Visibility.Visible;
		}

		protected void SetCurrentColorValues(Color currentColor) {

			if (string.IsNullOrEmpty(cm.Title))
				pi_color.Header = "color";
			else
				pi_color.Header = cm.Title;
			r_color.Fill = new SolidColorBrush(currentColor);
			r_original_value.Fill = new SolidColorBrush(Colors.Transparent);
			//r_original_value.Fill = new SolidColorBrush(currentColor);
			r_currentColor.Fill = new SolidColorBrush(currentColor);
			if (!string.IsNullOrEmpty(cm.Description)) {
				tb_desc.Visibility = System.Windows.Visibility.Visible;
				tb_desc.Text = cm.Description;
			}
			else
				tb_desc.Visibility = System.Windows.Visibility.Collapsed;

			tb_value_R.Text = currentColor.R.ToString();
			tb_value_G.Text = currentColor.G.ToString();
			tb_value_B.Text = currentColor.B.ToString();

			HSL hsl = HSL.convertRGB(currentColor);

			tb_value_H.Text = hsl.H.ToString("0.00");
			tb_value_S.Text = (hsl.S * 100).ToString("0.00");
			tb_value_L.Text = (hsl.L * 100).ToString("0.00");

			CMYK cmyk = CMYK.convertRGB(currentColor);

			tb_value_C.Text = (cmyk.C * 100).ToString("0.00");
			tb_value_M.Text = (cmyk.M * 100).ToString("0.00");
			tb_value_Y.Text = (cmyk.Y * 100).ToString("0.00");
			tb_value_K.Text = (cmyk.K * 100).ToString("0.00");

			tb_value_hex.Text = RGB.ColorToHex(currentColor);
			tb_value_websafe.Text = RGB.ColorToWebsafeHex(currentColor);

		}

		protected void SetComplementValues(Color currentColor) {

			r_original_complement.Fill = new SolidColorBrush(Colors.Transparent);
			//r_original_complement.Fill = new SolidColorBrush(currentColor);

			HSL hsl = HSL.convertRGB(currentColor).addH(180);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_complement.ColorBrush = new SolidColorBrush(c);
			scb_complement.Title = cm.Title+ " Complement";
			scb_complement.Desc = "complementary color (180° Hue) to " + cm.Title;

			tb_complement_R.Text = RGB.R_text(c.R);
			tb_complement_G.Text = RGB.G_text(c.G);
			tb_complement_B.Text = RGB.B_text(c.B);

			tb_complement_H.Text = HSL.H_text(hsl.H);
			tb_complement_S.Text = HSL.S_text(hsl.S);
			tb_complement_L.Text = HSL.L_text(hsl.L);

			tb_complement_C.Text = CMYK.C_text(cmyk.C);
			tb_complement_M.Text = CMYK.M_text(cmyk.M);
			tb_complement_Y.Text = CMYK.Y_text(cmyk.Y);
			tb_complement_K.Text = CMYK.K_text(cmyk.K);

			tb_complement_actual.Text = RGB.ColorToHex(c);
			tb_complement_websafe.Text = RGB.ColorToWebsafeHex(c);

		}

		protected void SetSplitComplementValues(Color currentColor) {

			r_original_splitComplement.Fill = new SolidColorBrush(Colors.Transparent);
			//r_original_splitComplement.Fill = new SolidColorBrush(currentColor);

			SetSplitComplement1Values(currentColor);
			SetSplitComplement2Values(currentColor);

		}

		#region "split complements"

		protected void SetSplitComplement1Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).addH(150);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_splitComplement1.ColorBrush = new SolidColorBrush(c);
			scb_splitComplement1.Title = cm.Title+ " Split Complement";
			scb_splitComplement1.Desc = "split complementary color (150° Hue) to " + cm.Title;

			tb_splitComplement1_R.Text = RGB.R_textShort(c.R);
			tb_splitComplement1_G.Text = RGB.G_textShort(c.G);
			tb_splitComplement1_B.Text = RGB.B_textShort(c.B);

			tb_splitComplement1_H.Text = HSL.H_textShort(hsl.H);
			tb_splitComplement1_S.Text = HSL.S_textShort(hsl.S);
			tb_splitComplement1_L.Text = HSL.L_textShort(hsl.L);

			tb_splitComplement1_C.Text = CMYK.C_textShort(cmyk.C);
			tb_splitComplement1_M.Text = CMYK.M_textShort(cmyk.M);
			tb_splitComplement1_Y.Text = CMYK.Y_textShort(cmyk.Y);
			tb_splitComplement1_K.Text = CMYK.K_textShort(cmyk.K);

			tb_splitComplement1_actual.Text = RGB.ColorToHex(c);
			tb_splitComplement1_websafe.Text = RGB.ColorToWebsafeHex(c);

		}

		protected void SetSplitComplement2Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).addH(210);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_splitComplement2.ColorBrush = new SolidColorBrush(c);
			scb_splitComplement2.Title = cm.Title+ " Split Complement";
			scb_splitComplement2.Desc = "split complementary color (210° Hue) to " + cm.Title;

			tb_splitComplement2_R.Text = RGB.R_textShort(c.R);
			tb_splitComplement2_G.Text = RGB.G_textShort(c.G);
			tb_splitComplement2_B.Text = RGB.B_textShort(c.B);

			tb_splitComplement2_H.Text = HSL.H_textShort(hsl.H);
			tb_splitComplement2_S.Text = HSL.S_textShort(hsl.S);
			tb_splitComplement2_L.Text = HSL.L_textShort(hsl.L);

			tb_splitComplement2_C.Text = CMYK.C_textShort(cmyk.C);
			tb_splitComplement2_M.Text = CMYK.M_textShort(cmyk.M);
			tb_splitComplement2_Y.Text = CMYK.Y_textShort(cmyk.Y);
			tb_splitComplement2_K.Text = CMYK.K_textShort(cmyk.K);

			tb_splitComplement2_actual.Text = RGB.ColorToHex(c);
			tb_splitComplement2_websafe.Text = RGB.ColorToWebsafeHex(c);


		}

		#endregion

		protected void SetTriadValues(Color currentColor) {


			r_original_triad.Fill = new SolidColorBrush(Colors.Transparent);
			//r_original_triad.Fill = new SolidColorBrush(currentColor);

			SetTriad1Values(currentColor);
			SetTriad2Values(currentColor);

		}

		#region "Triads"

		protected void SetTriad1Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).addH(120);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_triad1.ColorBrush = new SolidColorBrush(c);
			scb_triad1.Title = cm.Title+ " Triad";
			scb_triad1.Desc = "triad color (120° Hue) to " + cm.Title;

			tb_triad1_R.Text = RGB.R_textShort(c.R);
			tb_triad1_G.Text = RGB.G_textShort(c.G);
			tb_triad1_B.Text = RGB.B_textShort(c.B);

			tb_triad1_H.Text = HSL.H_textShort(hsl.H);
			tb_triad1_S.Text = HSL.S_textShort(hsl.S);
			tb_triad1_L.Text = HSL.L_textShort(hsl.L);

			tb_triad1_C.Text = CMYK.C_textShort(cmyk.C);
			tb_triad1_M.Text = CMYK.M_textShort(cmyk.M);
			tb_triad1_Y.Text = CMYK.Y_textShort(cmyk.Y);
			tb_triad1_K.Text = CMYK.K_textShort(cmyk.K);

			tb_triad1_actual.Text = RGB.ColorToHex(c);
			tb_triad1_websafe.Text = RGB.ColorToWebsafeHex(c);

		}

		protected void SetTriad2Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).addH(240);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_triad2.ColorBrush = new SolidColorBrush(c);
			scb_triad2.Title = cm.Title+ " Triad";
			scb_triad2.Desc = "triad color (240° Hue) to " + cm.Title;

			tb_triad2_R.Text = RGB.R_textShort(c.R);
			tb_triad2_G.Text = RGB.G_textShort(c.G);
			tb_triad2_B.Text = RGB.B_textShort(c.B);

			tb_triad2_H.Text = HSL.H_textShort(hsl.H);
			tb_triad2_S.Text = HSL.S_textShort(hsl.S);
			tb_triad2_L.Text = HSL.L_textShort(hsl.L);

			tb_triad2_C.Text = CMYK.C_textShort(cmyk.C);
			tb_triad2_M.Text = CMYK.M_textShort(cmyk.M);
			tb_triad2_Y.Text = CMYK.Y_textShort(cmyk.Y);
			tb_triad2_K.Text = CMYK.K_textShort(cmyk.K);

			tb_triad2_actual.Text = RGB.ColorToHex(c);
			tb_triad2_websafe.Text = RGB.ColorToWebsafeHex(c);


		}

		#endregion

		protected void SetAnalogousValues(Color currentColor) {

			r_original_analogous.Fill = new SolidColorBrush(Colors.Transparent);
			//r_original_analogous.Fill = new SolidColorBrush(currentColor);

			SetAnalogous1Values(currentColor);
			SetAnalogous2Values(currentColor);

		}

		#region "Analogous"

		protected void SetAnalogous1Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).addH(30);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_analogous1.ColorBrush = new SolidColorBrush(c);
			scb_analogous1.Title = cm.Title+ " Analogous";
			scb_analogous1.Desc = "analogous color (30° Hue) to " + cm.Title;

			tb_analogous1_R.Text = RGB.R_textShort(c.R);
			tb_analogous1_G.Text = RGB.G_textShort(c.G);
			tb_analogous1_B.Text = RGB.B_textShort(c.B);

			tb_analogous1_H.Text = HSL.H_textShort(hsl.H);
			tb_analogous1_S.Text = HSL.S_textShort(hsl.S);
			tb_analogous1_L.Text = HSL.L_textShort(hsl.L);

			tb_analogous1_C.Text = CMYK.C_textShort(cmyk.C);
			tb_analogous1_M.Text = CMYK.M_textShort(cmyk.M);
			tb_analogous1_Y.Text = CMYK.Y_textShort(cmyk.Y);
			tb_analogous1_K.Text = CMYK.K_textShort(cmyk.K);

			tb_analogous1_actual.Text = RGB.ColorToHex(c);
			tb_analogous1_websafe.Text = RGB.ColorToWebsafeHex(c);

		}

		protected void SetAnalogous2Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).addH(330);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_analogous2.ColorBrush = new SolidColorBrush(c);
			scb_analogous2.Title = cm.Title+ " Analogous";
			scb_analogous2.Desc = "analogous color (330° Hue) to " + cm.Title;

			tb_analogous2_R.Text = RGB.R_textShort(c.R);
			tb_analogous2_G.Text = RGB.G_textShort(c.G);
			tb_analogous2_B.Text = RGB.B_textShort(c.B);

			tb_analogous2_H.Text = HSL.H_textShort(hsl.H);
			tb_analogous2_S.Text = HSL.S_textShort(hsl.S);
			tb_analogous2_L.Text = HSL.L_textShort(hsl.L);

			tb_analogous2_C.Text = CMYK.C_textShort(cmyk.C);
			tb_analogous2_M.Text = CMYK.M_textShort(cmyk.M);
			tb_analogous2_Y.Text = CMYK.Y_textShort(cmyk.Y);
			tb_analogous2_K.Text = CMYK.K_textShort(cmyk.K);

			tb_analogous2_actual.Text = RGB.ColorToHex(c);
			tb_analogous2_websafe.Text = RGB.ColorToWebsafeHex(c);


		}

		#endregion

		protected void SetMonochromeValues(Color currentColor) {

			r_original_monochromatic.Fill = new SolidColorBrush(Colors.Transparent);
			//r_original_monochromatic.Fill = new SolidColorBrush(currentColor);

			SetMonochrome1Values(currentColor);
			SetMonochrome2Values(currentColor);
			SetMonochrome3Values(currentColor);
			SetMonochrome4Values(currentColor);
			SetMonochrome5Values(currentColor);

		}

		#region "Monochromes"

		protected void SetMonochrome1Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).withL(0.1);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_monochromatic1.ColorBrush = new SolidColorBrush(c);
			scb_monochromatic1.Title = cm.Title+ " Monochrome";
			scb_monochromatic1.Desc = "monochrome color (0.1 Lightness) to " + cm.Title;

			tb_monochromatic1_R.Text = RGB.R_textShort(c.R);
			tb_monochromatic1_G.Text = RGB.G_textShort(c.G);
			tb_monochromatic1_B.Text = RGB.B_textShort(c.B);

			tb_monochromatic1_H.Text = HSL.H_textShort(hsl.H);
			tb_monochromatic1_S.Text = HSL.S_textShort(hsl.S);
			tb_monochromatic1_L.Text = HSL.L_textShort(hsl.L);

			tb_monochromatic1_C.Text = CMYK.C_textShort(cmyk.C);
			tb_monochromatic1_M.Text = CMYK.M_textShort(cmyk.M);
			tb_monochromatic1_Y.Text = CMYK.Y_textShort(cmyk.Y);
			tb_monochromatic1_K.Text = CMYK.K_textShort(cmyk.K);

			tb_monochromatic1_actual.Text = RGB.ColorToHex(c);
			tb_monochromatic1_websafe.Text = RGB.ColorToWebsafeHex(c);

		}

		protected void SetMonochrome2Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).withL(0.3);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_monochromatic2.ColorBrush = new SolidColorBrush(c);
			scb_monochromatic2.Title = cm.Title+ " Monochrome";
			scb_monochromatic2.Desc = "monochrome color (0.3 Lightness) to " + cm.Title;

			tb_monochromatic2_R.Text = RGB.R_textShort(c.R);
			tb_monochromatic2_G.Text = RGB.G_textShort(c.G);
			tb_monochromatic2_B.Text = RGB.B_textShort(c.B);

			tb_monochromatic2_H.Text = HSL.H_textShort(hsl.H);
			tb_monochromatic2_S.Text = HSL.S_textShort(hsl.S);
			tb_monochromatic2_L.Text = HSL.L_textShort(hsl.L);

			tb_monochromatic2_C.Text = CMYK.C_textShort(cmyk.C);
			tb_monochromatic2_M.Text = CMYK.M_textShort(cmyk.M);
			tb_monochromatic2_Y.Text = CMYK.Y_textShort(cmyk.Y);
			tb_monochromatic2_K.Text = CMYK.K_textShort(cmyk.K);

			tb_monochromatic2_actual.Text = RGB.ColorToHex(c);
			tb_monochromatic2_websafe.Text = RGB.ColorToWebsafeHex(c);

		}

		protected void SetMonochrome3Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).withL(0.5);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_monochromatic3.ColorBrush = new SolidColorBrush(c);
			scb_monochromatic3.Title = cm.Title+ " Monochrome";
			scb_monochromatic3.Desc = "monochrome color (0.5 Lightness) to " + cm.Title;

			tb_monochromatic3_R.Text = RGB.R_textShort(c.R);
			tb_monochromatic3_G.Text = RGB.G_textShort(c.G);
			tb_monochromatic3_B.Text = RGB.B_textShort(c.B);

			tb_monochromatic3_H.Text = HSL.H_textShort(hsl.H);
			tb_monochromatic3_S.Text = HSL.S_textShort(hsl.S);
			tb_monochromatic3_L.Text = HSL.L_textShort(hsl.L);

			tb_monochromatic3_C.Text = CMYK.C_textShort(cmyk.C);
			tb_monochromatic3_M.Text = CMYK.M_textShort(cmyk.M);
			tb_monochromatic3_Y.Text = CMYK.Y_textShort(cmyk.Y);
			tb_monochromatic3_K.Text = CMYK.K_textShort(cmyk.K);

			tb_monochromatic3_actual.Text = RGB.ColorToHex(c);
			tb_monochromatic3_websafe.Text = RGB.ColorToWebsafeHex(c);

		}

		protected void SetMonochrome4Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).withL(0.7);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_monochromatic4.ColorBrush = new SolidColorBrush(c);
			scb_monochromatic4.Title = cm.Title+ " Monochrome";
			scb_monochromatic4.Desc = "monochrome color (0.7 Lightness) to " + cm.Title;

			tb_monochromatic4_R.Text = RGB.R_textShort(c.R);
			tb_monochromatic4_G.Text = RGB.G_textShort(c.G);
			tb_monochromatic4_B.Text = RGB.B_textShort(c.B);

			tb_monochromatic4_H.Text = HSL.H_textShort(hsl.H);
			tb_monochromatic4_S.Text = HSL.S_textShort(hsl.S);
			tb_monochromatic4_L.Text = HSL.L_textShort(hsl.L);

			tb_monochromatic4_C.Text = CMYK.C_textShort(cmyk.C);
			tb_monochromatic4_M.Text = CMYK.M_textShort(cmyk.M);
			tb_monochromatic4_Y.Text = CMYK.Y_textShort(cmyk.Y);
			tb_monochromatic4_K.Text = CMYK.K_textShort(cmyk.K);

			tb_monochromatic4_actual.Text = RGB.ColorToHex(c);
			tb_monochromatic4_websafe.Text = RGB.ColorToWebsafeHex(c);

		}

		protected void SetMonochrome5Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).withL(0.9);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_monochromatic5.ColorBrush = new SolidColorBrush(c);
			scb_monochromatic5.Title = cm.Title+ " Monochrome";
			scb_monochromatic5.Desc = "monochrome color (0.9 Lightness) to " + cm.Title;

			tb_monochromatic5_R.Text = RGB.R_textShort(c.R);
			tb_monochromatic5_G.Text = RGB.G_textShort(c.G);
			tb_monochromatic5_B.Text = RGB.B_textShort(c.B);

			tb_monochromatic5_H.Text = HSL.H_textShort(hsl.H);
			tb_monochromatic5_S.Text = HSL.S_textShort(hsl.S);
			tb_monochromatic5_L.Text = HSL.L_textShort(hsl.L);

			tb_monochromatic5_C.Text = CMYK.C_textShort(cmyk.C);
			tb_monochromatic5_M.Text = CMYK.M_textShort(cmyk.M);
			tb_monochromatic5_Y.Text = CMYK.Y_textShort(cmyk.Y);
			tb_monochromatic5_K.Text = CMYK.K_textShort(cmyk.K);

			tb_monochromatic5_actual.Text = RGB.ColorToHex(c);
			tb_monochromatic5_websafe.Text = RGB.ColorToWebsafeHex(c);

		}

		#endregion

		private void GestureListener_Tap(object sender, Microsoft.Phone.Controls.GestureEventArgs e) {
			simpleColorButton scb = sender as simpleColorButton;
			ContextMenu contextMenu = ContextMenuService.GetContextMenu(scb);
			if (contextMenu.Parent == null) {
				contextMenu.Tag = scb;
				contextMenu.IsOpen = true;
			}
		}

		private void mi_addColorList_Click(object sender, RoutedEventArgs e) {
			simpleColorButton scb = (simpleColorButton)((ContextMenu)((MenuItem)sender).Parent).Tag;
			ec_addColor.Background = scb.ColorBrush;
			ec_addColor.Title = scb.Title;
			ec_addColor.Description = scb.Desc;

			setAddAppBarIcons();
			pano.Visibility = System.Windows.Visibility.Collapsed;

			p_addColor.IsOpen = true;
		}

		private void mi_addCurrentSet_Click(object sender, RoutedEventArgs e) {
			simpleColorButton scb = (simpleColorButton)((ContextMenu)((MenuItem)sender).Parent).Tag;
			ec_addToSetColor.Background = scb.ColorBrush;
			ec_addToSetColor.Title = scb.Title;
			ec_addToSetColor.Description = scb.Desc;
			

			setAddToSetAppBarIcons();
			pano.Visibility = System.Windows.Visibility.Collapsed;

			p_addColorToSet.IsOpen = true;
		}

		private void mi_ManualSelector_Click(object sender, RoutedEventArgs e) {
			simpleColorButton scb = (simpleColorButton)((ContextMenu)((MenuItem)sender).Parent).Tag;
			ColorSelector.StartColor = (Color)scb.ColorBrush.GetValue(SolidColorBrush.ColorProperty);

			NavigationService.Navigate(new Uri("/CameraLibrary/" + CameraLibrarySelector.selectorType.manual, UriKind.Relative));
		}

		private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e) {

			PanoramaItem pi = e.AddedItems[0] as PanoramaItem;
			if (pi.Equals(pi_color))
				setStandardAppBarIcons();
			else
				clearAppBarIcons();

		}

		#region "appBars"

		private void setStandardAppBarIcons() {
			if (AppBarIcons == AppBarState.Standard)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_edit);

			AppBarIcons = AppBarState.Standard;
		}

		private void setEditAppBarIcons() {
			if (AppBarIcons == AppBarState.Edit)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_editAccept);

			AppBarIcons = AppBarState.Edit;
		}

		private void setAddAppBarIcons() {
			if (AppBarIcons == AppBarState.Add)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_add);

			AppBarIcons = AppBarState.Add;
		}

		private void setAddToSetAppBarIcons() {
			if (AppBarIcons == AppBarState.AddToSet)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_addToSet);

			AppBarIcons = AppBarState.AddToSet;
		}

		private void clearAppBarIcons() {
			if (AppBarIcons == AppBarState.None)
				return;

			ApplicationBar.IsVisible = true;
			ApplicationBar.Buttons.Clear();

			AppBarIcons = AppBarState.None;
		}

		private void HideAppBar() {
			if (AppBarIcons == AppBarState.Hide)
				return;

			ApplicationBar.IsVisible = false;

			AppBarIcons = AppBarState.Hide;
		}

		#endregion

		private void abib_edit_Click(object sender, EventArgs e) {

			if (cm == null)
				throw new NullReferenceException("cm did not return a colorModel object when appbar edit icon was pushed");

			ec_editColor.Background = cm.ColorBrush;
			ec_editColor.Title = cm.Title;
			ec_editColor.Description = cm.Description;

			setEditAppBarIcons();
			pano.Visibility = System.Windows.Visibility.Collapsed;

			p_editColor.IsOpen = true;
		}

		private void abib_editAccept_Click(object sender, EventArgs e) {

			if (cm == null)
				throw new NullReferenceException("cm was not a colorModel object when appbar editAccept icon was pushed");

			cm.Title = ec_editColor.Title;
			cm.Description = ec_editColor.Description;

			cm.saveColor(true);

			p_editColor.IsOpen = false;
		}

		private void abib_add_Click(object sender, EventArgs e) {
			colorModel _cm = new colorModel() {
				Id = Guid.NewGuid(),
				Title = ec_addColor.Title,
				Description = ec_addColor.Description,
				ColorBrush = ec_addColor.Background,
				IsUploaded = false
			};

			_cm.saveColor(true);

			App.returnToMain = true;
			NavigationService.GoBack();
		}

		private void abib_addSet_Click(object sender, EventArgs e) {
			if (App.currentColor.GetType() != typeof(setColorModel))
				throw new InvalidCastException("App.currentColor was not a setColorModel object when trying to add the new color to a set");
			setColorModel scm = (setColorModel)App.currentColor;
			setModel sm = setModel.getSet(scm.Set_Id);
			setColorModel scm_new = setColorModel.createNewSetColorModel(sm.setColorPalette.Count, ec_addToSetColor.Title, ec_addToSetColor.Description, (Color)ec_addToSetColor.Background.GetValue(SolidColorBrush.ColorProperty), scm.Set_Id);
			scm_new.saveSetColor(true);
			App.currentSet.LoadSetData(scm.Set_Id);
			NavigationService.GoBack();
		}

		void edit_Closed(object sender, EventArgs e) {
			pi_color.Header = cm.Title;
			tb_desc.Text = cm.Description;

			pano.Visibility = System.Windows.Visibility.Visible;
			setStandardAppBarIcons();
		}

		void add_Closed(object sender, EventArgs e) {
			pano.Visibility = System.Windows.Visibility.Visible;
			setStandardAppBarIcons();
		}

		void addToSet_Closed(object sender, EventArgs e) {
			pano.Visibility = System.Windows.Visibility.Visible;
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

			FrameworkElement oth = pano;
			o_mainPage.objectToHide = oth;
			o_mainPage.addOverlay(new colorOverlay());

			HideAppBar();

			p_overlay.IsOpen = true;
		}

		void p_overlay_Closed(object sender, EventArgs e) {
			PanoramaItem pi = pano.SelectedItem as PanoramaItem;
			if (pi.Equals(pi_color))
				setStandardAppBarIcons();
			else
				clearAppBarIcons();
		}

		protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e) {
			if (p_editColor.IsOpen) {
				e.Cancel = true;
				p_editColor.IsOpen = false;
			}
			if (p_addColor.IsOpen) {
				e.Cancel = true;
				p_addColor.IsOpen = false;
			}
			if (p_addColorToSet.IsOpen) {
				e.Cancel = true;
				p_addColorToSet.IsOpen = false;
			}
			if (p_overlay.IsOpen) {
				e.Cancel = true;
				p_overlay.IsOpen = false;
			}
		}


	}
}