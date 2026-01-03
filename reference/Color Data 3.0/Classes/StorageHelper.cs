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
using System.IO.IsolatedStorage;
using System.IO;

namespace Color_Data_3._0.Classes {

	public class StorageHelper {

		public static bool containsSetting(string settingName) {
			IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
			return appSettings.Contains(settingName);
		}

		public static void saveSetting<T>(string settingName, T objectToSave) {
			IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
			if (appSettings.Contains(settingName)) {
				appSettings[settingName] = objectToSave;
			}
			else {
				appSettings.Add(settingName, objectToSave);
			}
			appSettings.Save();
		}

		public static T loadSetting<T>(string settingName) {
			IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
			if (appSettings.Contains(settingName))
				return (T)appSettings[settingName];
			else
				return default(T);
		}

	}


}
