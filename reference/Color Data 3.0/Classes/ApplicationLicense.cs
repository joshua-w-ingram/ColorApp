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
using Microsoft.Phone.Marketplace;

namespace Color_Data_3._0.Classes {
	public class ApplicationLicense {

		public static bool IsTrial {
			get {
#if TRIAL
				return true;
#else
				LicenseInformation license = new LicenseInformation();
				return license.IsTrial();
#endif
			}

		}

	}

}

