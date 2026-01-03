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
using Color_Data_3._0.Support;

namespace Color_Data_3._0.Data {
	public partial class NamedColorData : PhoneApplicationPage {

		public static string namedColorName;
		public static Color currentNamedColor;

		private enum AppBarState {
			None,
			Hide,
			Standard,
			Add
		}

		ApplicationBarIconButton abib_addNamedColor;
		ApplicationBarIconButton abib_add;

		private AppBarState AppBarIcons = AppBarState.None;

		public NamedColorData() {
			InitializeComponent();

			abib_add = new ApplicationBarIconButton(new Uri("/Images/light/add.png", UriKind.Relative));
			abib_add.Text = "add color";
			abib_add.Click += new EventHandler(abib_add_Click);


			abib_addNamedColor = new ApplicationBarIconButton(new Uri("/Images/light/add.png", UriKind.Relative));
			abib_addNamedColor.Text = "add color";
			abib_addNamedColor.Click += new EventHandler(abib_addNamedColor_Click);
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
			base.OnNavigatedTo(e);

			SetCurrentColorValues();

			SetComplementValues();
			SetSplitComplementValues();
			SetTriadValues();
			SetAnalogousValues();
			SetMonochromeValues();

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

		protected void SetCurrentColorValues() {

			pi_color.Header = namedColorName;
			r_color.Fill = new SolidColorBrush(currentNamedColor);
			r_original_value.Fill = new SolidColorBrush(Colors.Transparent);
			//r_original_value.Fill = new SolidColorBrush(currentColor);
			r_currentColor.Fill = new SolidColorBrush(currentNamedColor);


			tb_value_R.Text = currentNamedColor.R.ToString();
			tb_value_G.Text = currentNamedColor.G.ToString();
			tb_value_B.Text = currentNamedColor.B.ToString();

			HSL hsl = HSL.convertRGB(currentNamedColor);

			tb_value_H.Text = hsl.H.ToString("0.00");
			tb_value_S.Text = (hsl.S * 100).ToString("0.00");
			tb_value_L.Text = (hsl.L * 100).ToString("0.00");

			CMYK cmyk = CMYK.convertRGB(currentNamedColor);

			tb_value_C.Text = (cmyk.C * 100).ToString("0.00");
			tb_value_M.Text = (cmyk.M * 100).ToString("0.00");
			tb_value_Y.Text = (cmyk.Y * 100).ToString("0.00");
			tb_value_K.Text = (cmyk.K * 100).ToString("0.00");

			tb_value_hex.Text = RGB.ColorToHex(currentNamedColor);
			tb_value_websafe.Text = RGB.ColorToWebsafeHex(currentNamedColor);

		}

		protected void SetComplementValues() {

			r_original_complement.Fill = new SolidColorBrush(Colors.Transparent);
			//r_original_complement.Fill = new SolidColorBrush(currentColor);

			HSL hsl = HSL.convertRGB(currentNamedColor).addH(180);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_complement.ColorBrush = new SolidColorBrush(c);
			scb_complement.Title = namedColorName + " Complement";
			scb_complement.Desc = "complementary color (180° Hue) to " + namedColorName;

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

		protected void SetSplitComplementValues() {

			r_original_splitComplement.Fill = new SolidColorBrush(Colors.Transparent);
			//r_original_splitComplement.Fill = new SolidColorBrush(currentColor);

			SetSplitComplement1Values(currentNamedColor);
			SetSplitComplement2Values(currentNamedColor);

		}

		#region "split complements"

		protected void SetSplitComplement1Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).addH(150);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_splitComplement1.ColorBrush = new SolidColorBrush(c);
			scb_splitComplement1.Title = namedColorName + " Split Complement";
			scb_splitComplement1.Desc = "split complementary color (150° Hue) to " + namedColorName;

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
			scb_splitComplement2.Title = namedColorName + " Split Complement";
			scb_splitComplement2.Desc = "split complementary color (210° Hue) to " + namedColorName;

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

		protected void SetTriadValues() {


			r_original_triad.Fill = new SolidColorBrush(Colors.Transparent);
			//r_original_triad.Fill = new SolidColorBrush(currentColor);

			SetTriad1Values(currentNamedColor);
			SetTriad2Values(currentNamedColor);

		}

		#region "Triads"

		protected void SetTriad1Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).addH(120);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_triad1.ColorBrush = new SolidColorBrush(c);
			scb_triad1.Title = namedColorName + " Triad";
			scb_triad1.Desc = "triad color (120° Hue) to " + namedColorName;

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
			scb_triad2.Title = namedColorName + " Triad";
			scb_triad2.Desc = "triad color (240° Hue) to " + namedColorName;

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

		protected void SetAnalogousValues() {

			r_original_analogous.Fill = new SolidColorBrush(Colors.Transparent);
			//r_original_analogous.Fill = new SolidColorBrush(currentColor);

			SetAnalogous1Values(currentNamedColor);
			SetAnalogous2Values(currentNamedColor);

		}

		#region "Analogous"

		protected void SetAnalogous1Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).addH(30);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_analogous1.ColorBrush = new SolidColorBrush(c);
			scb_analogous1.Title = namedColorName + " Analogous";
			scb_analogous1.Desc = "analogous color (30° Hue) to " + namedColorName;

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
			scb_analogous2.Title = namedColorName + " Analogous";
			scb_analogous2.Desc = "analogous color (330° Hue) to " + namedColorName;

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

		protected void SetMonochromeValues() {

			r_original_monochromatic.Fill = new SolidColorBrush(Colors.Transparent);
			//r_original_monochromatic.Fill = new SolidColorBrush(currentColor);

			SetMonochrome1Values(currentNamedColor);
			SetMonochrome2Values(currentNamedColor);
			SetMonochrome3Values(currentNamedColor);
			SetMonochrome4Values(currentNamedColor);
			SetMonochrome5Values(currentNamedColor);

		}

		#region "Monochromes"

		protected void SetMonochrome1Values(Color currentColor) {

			HSL hsl = HSL.convertRGB(currentColor).withL(0.1);
			Color c = hsl.toRGB();
			CMYK cmyk = CMYK.convertRGB(c);

			scb_monochromatic1.ColorBrush = new SolidColorBrush(c);
			scb_monochromatic1.Title = namedColorName + " Monochrome";
			scb_monochromatic1.Desc = "monochrome color (0.1 Lightness) to " + namedColorName;

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
			scb_monochromatic2.Title = namedColorName + " Monochrome";
			scb_monochromatic2.Desc = "monochrome color (0.3 Lightness) to " + namedColorName;

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
			scb_monochromatic3.Title = namedColorName + " Monochrome";
			scb_monochromatic3.Desc = "monochrome color (0.5 Lightness) to " + namedColorName;

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
			scb_monochromatic4.Title = namedColorName + " Monochrome";
			scb_monochromatic4.Desc = "monochrome color (0.7 Lightness) to " + namedColorName;

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
			scb_monochromatic5.Title = namedColorName + " Monochrome";
			scb_monochromatic5.Desc = "monochrome color (0.9 Lightness) to " + namedColorName;

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

		void abib_addNamedColor_Click(object sender, EventArgs e) {

			ec_addColor.Background = new SolidColorBrush(currentNamedColor);
			ec_addColor.Title = namedColorName;
			ec_addColor.Description = "";

			setAddAppBarIcons();
			pano.Visibility = System.Windows.Visibility.Collapsed;

			p_addColor.IsOpen = true;
		}

		private void mi_addColorList_Click(object sender, RoutedEventArgs e) {
			simpleColorButton scb = (simpleColorButton)((ContextMenu)((MenuItem)sender).Parent).Tag;

			Color c = (Color)scb.ColorBrush.GetValue(SolidColorBrush.ColorProperty);
			namedColor nc = namedColor.findNamedColors(c, namedColor.NAMEDCOLOR_DEVIATION);

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
				setNameAndDescription(out _title, out _desc, scb.Title, scb.Desc);


			ec_addColor.Background = scb.ColorBrush;
			ec_addColor.Title = _title;
			ec_addColor.Description = _desc;

			setAddAppBarIcons();
			pano.Visibility = System.Windows.Visibility.Collapsed;

			p_addColor.IsOpen = true;
		}

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

		private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e) {

			PanoramaItem pi = e.AddedItems[0] as PanoramaItem;
			if (pi.Equals(pi_color))
				setStandardAppBarIcons();
			else
				clearAppBarIcons();

		}

		private void setStandardAppBarIcons() {
			if (AppBarIcons == AppBarState.Standard)
				return;

			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_addNamedColor);
			ApplicationBar.IsVisible = true;

			AppBarIcons = AppBarState.Standard;
		}

		private void setAddAppBarIcons() {
			if (AppBarIcons == AppBarState.Add)
				return;

			ApplicationBar.Buttons.Clear();

			ApplicationBar.Buttons.Add(abib_add);
			ApplicationBar.IsVisible = true;

			AppBarIcons = AppBarState.Add;
		}

		private void clearAppBarIcons() {
			if (AppBarIcons == AppBarState.None)
				return;

			ApplicationBar.Buttons.Clear();

			AppBarIcons = AppBarState.None;
		}

		private void HideAppBar() {
			if (AppBarIcons == AppBarState.Hide)
				return;

			ApplicationBar.IsVisible = false;

			AppBarIcons = AppBarState.Hide;
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

		void add_Closed(object sender, EventArgs e) {
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
			o_mainPage.addOverlay(new namedColorOverlay());

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
			if (p_addColor.IsOpen) {
				e.Cancel = true;
				p_addColor.IsOpen = false;
			}
			if (p_overlay.IsOpen) {
				e.Cancel = true;
				p_overlay.IsOpen = false;
			}
		}


	}
}