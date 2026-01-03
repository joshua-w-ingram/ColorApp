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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Color_Data_3._0.Controls;
using System.Windows.Resources;
using System.Xml.Linq;
using Color_Data_3._0.Classes;
using Color_Data_3._0.db;

namespace Color_Data_3._0 {
	public partial class App : Application {

		public static string uid {
			get {
				return StorageHelper.loadSetting<string>("UserId");
			}
			set {
				StorageHelper.saveSetting<string>("UserId", value);
			}
		}

		public static setModel currentSet { get; set; }
		public static colorModel currentColor { get; set; }

		/// <summary>
		/// used for navigating all the way back to main page when nested too deep
		/// </summary>
		public static bool returnToMain { get; set; }

		/// <summary>
		/// if this is used to tell the splash screen to play on first load
		/// </summary>
		public static bool isFirstLoad { get; set; }
		/// <summary>
		/// if user exits setup, this is used keep them from being prompted again until next launch
		/// </summary>
		public static bool disableSetup = false;

		private static XElement _colorNameXml;
		public static XElement colorNameXml {
			get {
				//Get a stream to the XML file which contains the data and load it into the XElement. 
				StreamResourceInfo xml;
				switch (StorageHelper.loadSetting<namedColorDatabaseType>("NamedColorDatabase")) {
					case namedColorDatabaseType.microsoft:
						xml = Application.GetResourceStream(new Uri("/Color_Data_3;component/Classes/ColorLists/microsoftColors.xml", UriKind.Relative));
						break;
					case namedColorDatabaseType.crayon:
						xml = Application.GetResourceStream(new Uri("/Color_Data_3;component/Classes/ColorLists/crayonColors.xml", UriKind.Relative));
						break;
					case namedColorDatabaseType.w3c:
						xml = Application.GetResourceStream(new Uri("/Color_Data_3;component/Classes/ColorLists/w3cColors.xml", UriKind.Relative));
						break;
					case namedColorDatabaseType.wiki:
						xml = Application.GetResourceStream(new Uri("/Color_Data_3;component/Classes/ColorLists/wikiColors.xml", UriKind.Relative));
						break;
					default:
						xml = Application.GetResourceStream(new Uri("/Color_Data_3;component/Classes/ColorLists/microsoftColors.xml", UriKind.Relative));
						break;
				}
				_colorNameXml = XElement.Load(xml.Stream);

				return _colorNameXml;
			}
		}

		public enum namedColorDatabaseType {
			microsoft,
			crayon,
			w3c,
			wiki
		}

		public enum autoNameOptionType {
			all,
			name,
			desc,
			none
		}

		public enum namedColorDistanceType {
			p30,
			p5,
			p10,
			p50,
			all
		}

		public enum searchVisibilityType {
			auto,
			hide,
			show
		}

		public enum helpVisibilityType {
			show,
			hide
		}

		public enum splashVisibilityType {
			show,
			hide
		}

		public enum webCommType {
			auto,
			manual,
			disabled
		}

		/// <summary>
		/// Provides easy access to the root frame of the Phone Application.
		/// </summary>
		/// <returns>The root frame of the Phone Application.</returns>
		public PhoneApplicationFrame RootFrame { get; private set; }

		/// <summary>
		/// Constructor for the Application object.
		/// </summary>
		public App() {
			// Global handler for uncaught exceptions. 
			UnhandledException += Application_UnhandledException;

			// Standard Silverlight initialization
			InitializeComponent();

			// Phone-specific initialization
			InitializePhoneApplication();

			RootFrame.UriMapper = Resources["UriMapper"] as UriMapper;

			// Show graphics profiling information while debugging.
			if (System.Diagnostics.Debugger.IsAttached) {
				// Display the current frame rate counters.
				Application.Current.Host.Settings.EnableFrameRateCounter = true;

				// Show the areas of the app that are being redrawn in each frame.
				//Application.Current.Host.Settings.EnableRedrawRegions = true;

				// Enable non-production analysis visualization mode, 
				// which shows areas of a page that are handed off to GPU with a colored overlay.
				//Application.Current.Host.Settings.EnableCacheVisualization = true;

				// Disable the application idle detection by setting the UserIdleDetectionMode property of the
				// application's PhoneApplicationService object to Disabled.
				// Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
				// and consume battery power when the user is not using the phone.
				PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
			}

		}

		// Code to execute when the application is launching (eg, from Start)
		// This code will not execute when the application is reactivated
		private void Application_Launching(object sender, LaunchingEventArgs e) {
			Color_Data_3._0.Classes.dbConnection.createDatabase();
			isFirstLoad = true;

			if (StorageHelper.loadSetting<App.webCommType>("webCommOptions") == App.webCommType.auto)
				communicationHelper.Sync();
		}

		// Code to execute when the application is activated (brought to foreground)
		// This code will not execute when the application is first launched
		private void Application_Activated(object sender, ActivatedEventArgs e) {
			// Ensure that application state is restored appropriately
		}

		// Code to execute when the application is deactivated (sent to background)
		// This code will not execute when the application is closing
		private void Application_Deactivated(object sender, DeactivatedEventArgs e) {
			// Ensure that required application state is persisted here.
		}

		// Code to execute when the application is closing (eg, user hit Back)
		// This code will not execute when the application is deactivated
		private void Application_Closing(object sender, ClosingEventArgs e) {

		}

		// Code to execute if a navigation fails
		private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e) {
			if (System.Diagnostics.Debugger.IsAttached) {
				// A navigation has failed; break into the debugger
				System.Diagnostics.Debugger.Break();
			}
		}

		// Code to execute on Unhandled Exceptions
		private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e) {
			if (System.Diagnostics.Debugger.IsAttached) {
				// An unhandled exception has occurred; break into the debugger
				System.Diagnostics.Debugger.Break();
			}
		}

		#region Phone application initialization

		// Avoid double-initialization
		private bool phoneApplicationInitialized = false;

		// Do not add any additional code to this method
		private void InitializePhoneApplication() {
			if (phoneApplicationInitialized)
				return;

			// Create the frame but don't set it as RootVisual yet; this allows the splash
			// screen to remain active until the application is ready to render.
			RootFrame = new PhoneApplicationFrame();
			RootFrame.Navigated += CompleteInitializePhoneApplication;

			// Handle navigation failures
			RootFrame.NavigationFailed += RootFrame_NavigationFailed;

			// Ensure we don't initialize again
			phoneApplicationInitialized = true;
		}

		// Do not add any additional code to this method
		private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e) {
			// Set the root visual to allow the application to render
			if (RootVisual != RootFrame)
				RootVisual = RootFrame;

			// Remove this handler since it is no longer needed
			RootFrame.Navigated -= CompleteInitializePhoneApplication;
		}

		#endregion
	}
}