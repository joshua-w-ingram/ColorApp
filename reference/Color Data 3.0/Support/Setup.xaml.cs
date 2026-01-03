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
using Microsoft.Phone.Tasks;
using System.Text;

namespace Color_Data_3._0.Support {
	public partial class Setup : PhoneApplicationPage {

		//Test Code:DCAM6G

		public Setup() {
			InitializeComponent();
		}

		protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e) {

			MessageBoxResult result = MessageBox.Show("Are you sure you want to quit Setup?", "Back Pressed", MessageBoxButton.OKCancel);

			if (result == MessageBoxResult.Cancel)
				e.Cancel = true;

			App.disableSetup = true;
		}

		private void b_firstTime_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			w_waiting.Show();

			communicationHelper.userIdRetrieved += ((success) => {
				w_waiting.Hide();
				if (success) {
					VisualStateManager.GoToState(this, "FirstTimer", false);
					tb_code.Text = App.uid;
				}
				else
					VisualStateManager.GoToState(this, "Failed", false);

			});

			communicationHelper.RequestUserId();

		}

		private void b_Veteran_Tap(object sender, System.Windows.Input.GestureEventArgs e) {

			VisualStateManager.GoToState(this, "Veteran", false);

		}

		private void b_emailCode_Tap(object sender, System.Windows.Input.GestureEventArgs e) {

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("This is my Color Data Account Code:");

			sb.AppendLine();
			sb.AppendLine("code: " + App.uid);
			sb.AppendLine();
			sb.AppendLine("I can use this code for accessing my Color Data information online at:");
			sb.AppendLine(@"http://colordata.info");

			EmailComposeTask emailComposeTask = new EmailComposeTask();
			emailComposeTask.Subject = "Color Data Account Code";
			emailComposeTask.Body = sb.ToString();
			emailComposeTask.Show();
		}

		private void b_exit_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			if (((Button)(sender)).Name == "b_exit3")
				App.disableSetup = true;
			NavigationService.GoBack();
		}

		private void b_downloadData_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
			tb_invalidCode.Visibility = System.Windows.Visibility.Collapsed;

			if (tbox_code.Text.Trim().Length != 6) {
				tb_invalidCode.Visibility = System.Windows.Visibility.Visible;
				return;
			}

			w_waiting.Show();

			communicationHelper.userDownloaded += ((success, result) => {
				w_waiting.Hide();
				if (success) {
					App.uid = tbox_code.Text.Trim();
					VisualStateManager.GoToState(this, "VeteranDone", false);
				}
				else {
					tb_invalidCode.Visibility = System.Windows.Visibility.Visible;
					VisualStateManager.GoToState(this, "Veteran", false);
				}

			});

			communicationHelper.DownloadUser(tbox_code.Text.Trim());
		}

		private void tbox_code_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
			tbox_code.Text = tbox_code.Text.ToUpper();
			tbox_code.SelectionStart = tbox_code.Text.Length;
			tbox_code.SelectionLength = 0;

		}

	}
}
