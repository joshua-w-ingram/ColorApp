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
using System.ComponentModel;
using System.Linq;
using Color_Data_3._0.db;
using System.Windows.Media.Imaging;
using System.IO;

namespace Color_Data_3._0.Classes {
	public class pictureModel : INotifyPropertyChanged {

		private int _Id;
		public int Id {
			get {
				return _Id;
			}
			set {
				if (value != _Id) {
					_Id = value;
					NotifyPropertyChanged("Id");
				}
			}
		}

		private byte[] _Img;
		public byte[] Img {
			get {
				return _Img;
			}
			set {
				if (value != _Img) {
					_Img = value;
					NotifyPropertyChanged("Img");
				}
			}
		}

		private bool _IsUploaded;
		public bool IsUploaded {
			get {
				return _IsUploaded;
			}
			set {
				if (value != _IsUploaded) {
					_IsUploaded = value;
					NotifyPropertyChanged("IsUploaded");
				}
			}
		}

		public BitmapImage picture {
			get {
				MemoryStream ms = new MemoryStream(Img);
				BitmapImage bmp = new BitmapImage();
				bmp.SetSource(ms);
				return bmp;
			}
		}

		private Guid _SetId;
		public Guid SetId {
			get {
				return _SetId;
			}
			set {
				if (value != _SetId) {
					_SetId = value;
					NotifyPropertyChanged("SetId");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (null != handler) {
				handler(this, new PropertyChangedEventArgs(propertyName));
			}

		}

		public static pictureModel getPicture(Guid setid) {

			db.Picture currentPicture = null;
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {
				IQueryable<db.Picture> query = from p in context.Pictures
									 where p.Set_id == setid
									 select p;

				currentPicture = query.FirstOrDefault<db.Picture>();

			}

			if (currentPicture == null)
				return null;

			pictureModel pm = new pictureModel() {
				Id = currentPicture.Id,
				SetId = currentPicture.Set_id,
				Img = currentPicture.Img.ToArray()
			};

			return pm;
		}

		public void savePicture() {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.Picture> query = from p in context.Pictures
									 where p.Id == Id
									 select p;

				if (query.Count() == 0) {
					db.Picture p = new db.Picture {
						Set_id = this.SetId,
						IsUploaded = this.IsUploaded,

						Img = this.Img
					};
					context.Pictures.InsertOnSubmit(p);
					context.SubmitChanges();
				}
			}

		}

		public static void pictureUploaded(Guid setId) {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.Picture> query = from p in context.Pictures
									 where p.Set_id == setId
									 select p;

				if (query.Count() == 0) {
					//don't throw an exception. just ignore 
					//throw new ArgumentException("The set id is not valid when trying to update IsUploaded.");
				}
				else {
					db.Picture p = query.First();
					p.IsUploaded = true;
				}
				context.SubmitChanges();
			}

		}

		public static void deletePicture(pictureModel pm) {
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.Picture> query = from p in context.Pictures
									 where p.Id == pm.Id
									 select p;

				if (query.Count() == 0) {
					return;
				}
				else {
					db.Picture s = query.FirstOrDefault<db.Picture>();
					context.Pictures.DeleteOnSubmit(s);
				}

				context.SubmitChanges();

			}
		}

	}
}
