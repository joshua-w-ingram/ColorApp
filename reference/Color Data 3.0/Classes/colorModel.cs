using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

using System.Windows.Media;
using Color_Data_3._0.db;
using Color_Data_3._0.Classes.ColorLists;
using System.Text.RegularExpressions;
namespace Color_Data_3._0.Classes {
	public class colorModel : INotifyPropertyChanged {

		private Guid _Id;
		public Guid Id {
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

		private string _Title;
		public string Title {
			get {
				return _Title;
			}
			set {
				if (value != _Title) {
					_Title = value;
					NotifyPropertyChanged("Title");
				}
			}
		}

		private string _Desc;
		public string Description {
			get {
				return _Desc;
			}
			set {
				if (value != _Desc) {
					_Desc = value;
					NotifyPropertyChanged("Desc");
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

		private Brush _Color;
		public Brush ColorBrush {
			get {
				return _Color;
			}
			set {
				if (value != _Color) {
					_Color = value;
					NotifyPropertyChanged("Color");
				}
			}
		}

		public System.Windows.Media.Color ColorValues {
			get {
				System.Windows.Media.Color c = (System.Windows.Media.Color)ColorBrush.GetValue(SolidColorBrush.ColorProperty);
				return c;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(String propertyName) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (null != handler) {
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public static colorModel getColor(Guid id) {

			db.Color c;
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {
				IQueryable<db.Color> query = from _c in context.Colors
								     where _c.Id == id
								     select _c;

				c = query.FirstOrDefault<db.Color>();
			}

			if (c == null)
				return null;

			colorModel cm = new colorModel() {
				Id = c.Id,
				Title = c.Title,
				Description = c.Description,
				ColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)c.R, (byte)c.G, (byte)c.B)),
				IsUploaded = c.IsUploaded
			};

			return cm;
		}

		public static ObservableCollection<colorModel> getColors() {
			return getColors(string.Empty);
		}

		public static ObservableCollection<colorModel> getColors(string searchString) {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {
				IQueryable<colorModel> query = from c in context.Colors
									 join sc in context.SetColors
									 on c.Id equals sc.Color_id into tempColors
									 from _c in tempColors.DefaultIfEmpty()
									 where _c.Set_id == null && c.Title.Contains(searchString)
									 select new colorModel() {
										 Id = c.Id,
										 Title = c.Title,
										 Description = c.Description,
										 ColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)c.R, (byte)c.G, (byte)c.B)),
										 IsUploaded = c.IsUploaded
									 };

				ObservableCollection<colorModel> colorModelList = new ObservableCollection<colorModel>(query);
				return colorModelList;

			}
		}

		public static ObservableCollection<colorModel> getColorsNotUploaded() {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {
				IQueryable<colorModel> query = from c in context.Colors
									 join sc in context.SetColors
									 on c.Id equals sc.Color_id into tempColors
									 from _c in tempColors.DefaultIfEmpty()
									 where _c.Set_id == null && c.IsUploaded == false
									 select new colorModel() {
										 Id = c.Id,
										 Title = c.Title,
										 Description = c.Description,
										 ColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)c.R, (byte)c.G, (byte)c.B)),
										 IsUploaded = c.IsUploaded
									 };

				ObservableCollection<colorModel> colorModelList = new ObservableCollection<colorModel>(query);
				return colorModelList;

			}
		}

		public static ObservableCollection<colorModel> getAllColors() {
			IList<db.Color> colorList = null;
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {
				IQueryable<db.Color> query = from c in context.Colors select c;
				colorList = query.ToList();
			}

			ObservableCollection<colorModel> colorModelList = new ObservableCollection<colorModel>();
			foreach (db.Color c in colorList) {
				colorModelList.Add(new colorModel() {
					Id = c.Id,
					Title = c.Title,
					Description = c.Description,
					ColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)c.R, (byte)c.G, (byte)c.B)),
					IsUploaded = c.IsUploaded
				});
			}

			return colorModelList;

		}

		public void saveColor(bool upload) {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.Color> query = from c in context.Colors
								     where c.Id == Id
								     select c;

				if (query.Count() == 0) {
					db.Color c = new db.Color {
						Id = this.Id,
						Title = this.Title,
						Description = this.Description,
						R = this.ColorValues.R,
						G = this.ColorValues.G,
						B = this.ColorValues.B,
						IsUploaded = this.IsUploaded
					};
					context.Colors.InsertOnSubmit(c);
				}
				else {
					db.Color c = query.First();
					c.Title = this.Title;
					c.Description = this.Description;
					c.R = this.ColorValues.R;
					c.G = this.ColorValues.G;
					c.B = this.ColorValues.B;
					c.IsUploaded = false;
				}
				context.SubmitChanges();
			}

			if (upload && StorageHelper.loadSetting<App.webCommType>("webCommOptions") == App.webCommType.auto) {
				communicationHelper.UploadColor(this);
			}

		}

		public static void colorUploaded(Guid id) {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.Color> query = from c in context.Colors
								     where c.Id == id
								     select c;

				if (query.Count() == 0) {
					//don't throw an exception. just ignore 
					//throw new ArgumentException("The set id is not valid when trying to update IsUploaded.");
				}
				else {
					db.Color c = query.First();
					c.IsUploaded = true;
				}
				context.SubmitChanges();
			}

		}

		public static void deleteColor(colorModel cm) {
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.Color> query = from c in context.Colors
								     where c.Id == cm.Id
								     select c;

				if (query.Count() == 0) {
					return;
				}
				else {
					db.Color c = query.FirstOrDefault<db.Color>();
					context.Colors.DeleteOnSubmit(c);

					db.DeletedColor dc = new DeletedColor {
						Color_id = c.Id
					};
				}
				context.SubmitChanges();
			}

			if (StorageHelper.loadSetting<App.webCommType>("webCommOptions") == App.webCommType.auto) {
				communicationHelper.DeleteColor(cm.Id);
			}

		}

		public static List<DeletedColor> deletedColors() {
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<DeletedColor> query = from dc in context.DeletedColors
								 select dc;

				return query.ToList<DeletedColor>();
			}
		}

		public static void clearDeletedColor(Guid id) {
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.DeletedColor> query_ds = from ds in context.DeletedColors
										   where ds.Color_id == id
										   select ds;


				if (query_ds.Count() == 0) {
					// do nothing
				}
				else {

					db.DeletedColor ds = query_ds.FirstOrDefault<db.DeletedColor>();
					context.DeletedColors.DeleteOnSubmit(ds);
					context.SubmitChanges();

				}


			}
		}
	}
}