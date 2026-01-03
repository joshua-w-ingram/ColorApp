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
using System.Linq;
using Color_Data_3._0.db;
using System.Collections.Generic;

namespace Color_Data_3._0.Classes {

	public static class communicationHelper {

		public delegate void userIdRequestHandler(bool success);
		public static event userIdRequestHandler userIdRetrieved;

		public delegate void userDownloadHandler(bool success, cd3.ResultType result);
		public static event userDownloadHandler userDownloaded;

		public static void RequestUserId() {
			if (ApplicationLicense.IsTrial)
				return;

			cd3.colordata3SoapClient soap = new cd3.colordata3SoapClient();
			soap.requestUserIdCompleted += new EventHandler<cd3.requestUserIdCompletedEventArgs>(soap_requestUserIdCompleted);
			soap.requestUserIdAsync();

		}

		static void soap_requestUserIdCompleted(object sender, cd3.requestUserIdCompletedEventArgs e) {
			try {
				if (e.Error != null)
					userIdRetrieved(false);

				App.uid = e.Result;
				userIdRetrieved(true);
			}
			catch {
				userIdRetrieved(false);
			}
		}

		public static void DownloadUser(string uid) {
			if (ApplicationLicense.IsTrial)
				return;

			cd3.colordata3SoapClient soap = new cd3.colordata3SoapClient();
			soap.DownloadUserCompleted += new EventHandler<cd3.DownloadUserCompletedEventArgs>(soap_DownloadUserCompleted);
			soap.DownloadUserAsync(uid);

		}

		public static void soap_DownloadUserCompleted(object sender, cd3.DownloadUserCompletedEventArgs e) {
			try {
				if (e.Error != null)
					userDownloaded(false, cd3.ResultType.failure_unknown);

				if (!e.Result.RequestResult.success)
					userDownloaded(false, e.Result.RequestResult.result);

				cd3.user u = e.Result.User;

				foreach (cd3.set s in u.sets) {

					setModel sm = new setModel() {
						Id = s.id,
						Title = s.title,
						Description = s.description,
						DateCreated = s.dateCreated,
						LastUpdated = s.lastUpdated,
						IsUploaded = true,
						Snap = false
					};

					foreach (cd3.setColor sc in s.colors) {
						cd3.color c = sc.Color;
						sm.setColorPalette.Add(new setColorModel() {
							Id = c.id,
							Set_Id = s.id,
							Title = c.title,
							Description = c.description,
							ColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)c.r, (byte)c.g, (byte)c.b)),
							X = sc.x,
							Y = sc.y,
							Position = sc.position,
							Z = sc.z,
							IconSize = sc.IconSize,
							IsUploaded = true
						});
					}

					sm.saveSet();

				}

				foreach (cd3.color c in u.colors) {
					colorModel cm = new colorModel() {
						Id = c.id,
						Title = c.title,
						Description = c.description,
						ColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)c.r, (byte)c.g, (byte)c.b)),
						IsUploaded = true
					};

					cm.saveColor(true);
				}


				userDownloaded(true, cd3.ResultType.retrieved);
			}
			catch {
				userDownloaded(false, cd3.ResultType.failure_unknown);
			}
		}

		public static void UploadSet(setModel sm) {
			if (ApplicationLicense.IsTrial)
				return;

			cd3.colordata3SoapClient soap = new cd3.colordata3SoapClient();
			soap.UploadSetCompleted += new EventHandler<cd3.UploadSetCompletedEventArgs>(soap_UploadSetCompleted);
			soap.UploadSetAsync(App.uid, new cd3.set() {
				id = sm.Id,
				title = sm.Title,
				description = sm.Description,
				dateCreated = sm.DateCreated,
				lastUpdated = sm.LastUpdated
			});
		}

		public static void soap_UploadSetCompleted(object sender, cd3.UploadSetCompletedEventArgs e) {
			try {
				if (e.Error != null)
					return;

				//check to make sure the call was successful. if not, act like nothing happened
				if (e.Result.success != true)
					return;

				//TODO: comment back in after debugging
				//setModel.setUploaded(e.Result.setId);

				setModel sm = setModel.getSet(e.Result.setId);
				foreach (setColorModel scm in sm.setColorPalette) {
					if (!scm.IsUploaded || !scm.IsSetColorUploaded)
						UploadSetColor(scm);
				}
				if (sm.Picture != null)
					uploadPicture(sm.Picture);

			}
			catch {
				//nothing to do... 
				//we don't want to update local database because our data has not made it to the server
			}
		}

		public static void UploadSetColor(setColorModel scm) {
			if (ApplicationLicense.IsTrial)
				return;

			cd3.colordata3SoapClient soap = new cd3.colordata3SoapClient();
			soap.UploadSetColorCompleted += new EventHandler<cd3.UploadSetColorCompletedEventArgs>(soap_UploadSetColorCompleted);
			soap.UploadSetColorAsync(App.uid, scm.Set_Id,
				new cd3.setColor() {
					Color = new cd3.color() {
						id = scm.Id,
						title = scm.Title,
						description = scm.Description,
						r = scm.ColorValues.R,
						g = scm.ColorValues.G,
						b = scm.ColorValues.B
					},
					x = scm.X,
					y = scm.Y,
					position = scm.Position,
					z = scm.Z,
					IconSize = scm.IconSize
				});
		}

		public static void soap_UploadSetColorCompleted(object sender, cd3.UploadSetColorCompletedEventArgs e) {
			try {
				if (e.Error != null)
					return;

				//check to make sure the call was successful. if not, act like nothing happened
				if (e.Result.success != true)
					return;

				//TODO: comment back in after debugging
				//colorModel.colorUploaded(e.Result.colorId);
				//TODO: comment back in after debugging
				//setColorModel.setColorUploaded(e.Result.colorId, e.Result.setId);

			}
			catch {
				//nothing to do... 
				//we don't want to update local database because our data has not made it to the server
			}

		}

		public static void UploadColor(colorModel cm) {
			if (ApplicationLicense.IsTrial)
				return;

			cd3.colordata3SoapClient soap = new cd3.colordata3SoapClient();
			soap.UploadColorCompleted += new EventHandler<cd3.UploadColorCompletedEventArgs>(soap_UploadColorCompleted);
			soap.UploadColorAsync(App.uid, new cd3.color() {
				id = cm.Id,
				title = cm.Title,
				description = cm.Description,
				r = cm.ColorValues.R,
				g = cm.ColorValues.G,
				b = cm.ColorValues.B
			});
		}

		public static void soap_UploadColorCompleted(object sender, cd3.UploadColorCompletedEventArgs e) {
			try {
				if (e.Error != null)
					return;

				//check to make sure the call was successful. if not, act like nothing happened
				if (e.Result.success != true)
					return;

				//TODO: comment back in after debugging
				//colorModel.colorUploaded(e.Result.colorId);

			}
			catch {
				//nothing to do... 
				//we don't want to update local database because our data has not made it to the server
			}
		}

		public static void uploadPicture(pictureModel pm) {
			if (ApplicationLicense.IsTrial)
				return;

			cd3.colordata3SoapClient cd3_sc = new cd3.colordata3SoapClient();
			cd3_sc.UploadPictureCompleted += new EventHandler<cd3.UploadPictureCompletedEventArgs>(cd3_sc_UploadPictureCompleted);
			cd3_sc.UploadPictureAsync(App.uid, pm.SetId, pm.Img);
		}

		static void cd3_sc_UploadPictureCompleted(object sender, cd3.UploadPictureCompletedEventArgs e) {
			try {
				if (e.Error != null)
					return;

				//check to make sure the call was successful. if not, act like nothing happened
				if (e.Result.success != true)
					return;

				pictureModel.pictureUploaded(e.Result.setId);

			}
			catch {
				//nothing to do... 
				//we don't want to update local database because our data has not made it to the server
			}
		}

		public static void DeleteSet(Guid setId) {
			if (ApplicationLicense.IsTrial)
				return;

			cd3.colordata3SoapClient cd3_sc = new cd3.colordata3SoapClient();
			cd3_sc.DeleteSetCompleted += new EventHandler<cd3.DeleteSetCompletedEventArgs>(cd3_sc_DeleteSetCompleted);
			cd3_sc.DeleteSetAsync(App.uid, setId);

		}

		public static void cd3_sc_DeleteSetCompleted(object sender, cd3.DeleteSetCompletedEventArgs e) {
			try {
				if (e.Error != null)
					return;

				//check to make sure the call was successful. if not, act like nothing happened
				if (e.Result.success != true)
					return;

				setModel.clearDeletedSet(e.Result.setId);

			}
			catch {
				//nothing to do... 
				//we don't want to update local database because our data has not made it to the server
			}
		}

		public static void DeleteSetColor(Guid setId, Guid colorId) {
			if (ApplicationLicense.IsTrial)
				return;

			cd3.colordata3SoapClient cd3_sc = new cd3.colordata3SoapClient();
			cd3_sc.DeleteSetColorCompleted += new EventHandler<cd3.DeleteSetColorCompletedEventArgs>(cd3_sc_DeleteSetColorCompleted);
			cd3_sc.DeleteSetAsync(App.uid, setId);

		}

		static void cd3_sc_DeleteSetColorCompleted(object sender, cd3.DeleteSetColorCompletedEventArgs e) {
			try {
				if (e.Error != null)
					return;

				//check to make sure the call was successful. if not, act like nothing happened
				if (e.Result.success != true)
					return;

				setColorModel.clearDeletedSetColor(e.Result.setId, e.Result.colorId);

			}
			catch {
				//nothing to do... 
				//we don't want to update local database because our data has not made it to the server
			}
		}

		public static void DeleteColor(Guid colorId) {
			if (ApplicationLicense.IsTrial)
				return;

			cd3.colordata3SoapClient cd3_sc = new cd3.colordata3SoapClient();
			cd3_sc.DeleteColorCompleted += new EventHandler<cd3.DeleteColorCompletedEventArgs>(cd3_sc_DeleteColorCompleted);
			cd3_sc.DeleteColorAsync(App.uid, colorId);

		}

		public static void cd3_sc_DeleteColorCompleted(object sender, cd3.DeleteColorCompletedEventArgs e) {
			try {
				if (e.Error != null)
					return;

				//check to make sure the call was successful. if not, act like nothing happened
				if (e.Result.success != true)
					return;

				colorModel.clearDeletedColor(e.Result.colorId);

		
			}
			catch {
				//nothing to do... 
				//we don't want to update local database because our data has not made it to the server
			}
		}

		public static void Sync() {
			if (ApplicationLicense.IsTrial)
				return;

			foreach (colorModel cm in colorModel.getColorsNotUploaded()) {
				communicationHelper.UploadColor(cm);
			}

			foreach (setModel sm in setModel.getSetsNotUploaded()) {
				communicationHelper.UploadSet(sm);
			}

			List<DeletedSet> dsl = setModel.deletedSets();
			foreach (DeletedSet ds in dsl) {
				communicationHelper.DeleteSet(ds.Set_id);
			}

			List<DeletedSetColor> dscl = setColorModel.deletedSetColors();
			foreach (DeletedSetColor dsc in dscl) {
				communicationHelper.DeleteSetColor(dsc.Set_id, dsc.Color_id);
			}

			List<DeletedColor> dcl = colorModel.deletedColors();
			foreach (DeletedColor dc in dcl) {
				communicationHelper.DeleteColor(dc.Color_id);
			}

		}


	}
}
