using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Color_Data_3._0.db;
using System.Linq;

namespace Color_Data_3._0.Classes {

	public class setModel : INotifyPropertyChanged {

		public const int MAX_COLORS = 20;

		#region "properties"

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

		private string _Description;
		public string Description {
			get {
				return _Description;
			}
			set {
				if (value != _Description) {
					_Description = value;
					NotifyPropertyChanged("Description");
				}
			}
		}

		private DateTime _DateCreated;
		public DateTime DateCreated {
			get {
				return _DateCreated;
			}
			set {
				if (value != _DateCreated) {
					_DateCreated = value;
					NotifyPropertyChanged("DateCreated");
				}
			}
		}

		private DateTime _LastUpdated;
		public DateTime LastUpdated {
			get {
				return _LastUpdated;
			}
			set {
				if (value != _LastUpdated) {
					_LastUpdated = value;
					NotifyPropertyChanged("LastUpdated");
				}
			}
		}

		private bool _Snap;
		public bool Snap {
			get {
				return _Snap;
			}
			set {
				if (value != _Snap) {
					_Snap = value;
					NotifyPropertyChanged("Snap");
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

		private ObservableCollection<setColorModel> _setColors;
		public ObservableCollection<setColorModel> setColorPalette {
			get {
				return _setColors;
			}
			set {
				if (value != _setColors) {
					_setColors = value;
					NotifyPropertyChanged("setColorPalette");
				}
			}
		}

		private pictureModel _Picture;
		public pictureModel Picture {
			get {
				return _Picture;
			}
			set {
				if (value != _Picture) {
					_Picture = value;
					NotifyPropertyChanged("Picture");
				}
			}
		}

		#endregion

		public setModel() {
			setColorPalette = new ObservableCollection<setColorModel>();
		}

		public void LoadSetData(Guid id) {

			setModel s = setModel.getSet(id);

			Id = s.Id;
			Title = s.Title;
			Description = s.Description;
			setColorPalette = setColorModel.getSetColors(id);
			DateCreated = s.DateCreated;
			LastUpdated = s.LastUpdated;
			Snap = s.Snap;
			IsUploaded = s.IsUploaded;
			Picture = pictureModel.getPicture(id);

		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (null != handler) {
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public static setModel createEmptySet() {
			return createEmptySet("", "");
		}

		public static setModel createEmptySet(string title, string desc) {
			return new setModel() {
				Id = Guid.NewGuid(),
				Title = title,
				Description = desc,
				DateCreated = DateTime.Now,
				LastUpdated = DateTime.Now,
				Snap = false,
				IsUploaded = false
			};
		}

		public static setModel getSet(Guid id) {

			db.Set currentSet = null;
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {
				IQueryable<db.Set> query = from s in context.Sets
								   where s.Id == id
								   select s;

				currentSet = query.FirstOrDefault<db.Set>();

			}

			if (currentSet == null)
				return null;

			setModel sm = new setModel() {
				Id = currentSet.Id,
				Title = currentSet.Title,
				Description = currentSet.Description,
				setColorPalette = setColorModel.getSetColors(id),
				DateCreated = currentSet.DateCreated,
				LastUpdated = currentSet.LastUpdated,
				Snap = currentSet.Snap,
				IsUploaded = currentSet.IsUploaded,
				Picture = pictureModel.getPicture(id)
			};

			return sm;
		}

		public static ObservableCollection<setModel> getSets() {
			return getSets(string.Empty);
		}

		public static ObservableCollection<setModel> getSets(string searchString) {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {
				IQueryable<setModel> query = from s in context.Sets
								     where s.Title.Contains(searchString)
								     select new setModel() {
									     Id = s.Id,
									     Title = s.Title,
									     Description = s.Description,
									     setColorPalette = setColorModel.getSetColors(s.Id),
									     DateCreated = s.DateCreated,
									     LastUpdated = s.LastUpdated,
									     Snap = s.Snap,
									     IsUploaded = s.IsUploaded,
									     Picture = pictureModel.getPicture(s.Id)
								     };

				ObservableCollection<setModel> setModelList = new ObservableCollection<setModel>(query);
				return setModelList;
			}

		}

		public static ObservableCollection<setModel> getSetsNotUploaded() {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {
				IQueryable<setModel> query = from s in context.Sets
								     where s.IsUploaded == false
								     select new setModel() {
									     Id = s.Id,
									     Title = s.Title,
									     Description = s.Description,
									     setColorPalette = setColorModel.getSetColors(s.Id),
									     DateCreated = s.DateCreated,
									     LastUpdated = s.LastUpdated,
									     Snap = s.Snap,
									     IsUploaded = s.IsUploaded,
									     Picture = pictureModel.getPicture(s.Id)
								     };

				ObservableCollection<setModel> setModelList = new ObservableCollection<setModel>(query);
				return setModelList;
			}

		}

		public void saveSet() {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.Set> query = from s in context.Sets
								   where s.Id == Id
								   select s;

				if (query.Count() == 0) {
					db.Set s = new db.Set {
						Id = this.Id,
						Title = this.Title,
						Description = this.Description,
						IsUploaded = this.IsUploaded,
						DateCreated = DateTime.Now,
						LastUpdated = DateTime.Now,
						Snap = this.Snap
					};
					context.Sets.InsertOnSubmit(s);
				}
				else {
					db.Set s = query.First();
					s.Title = this.Title;
					s.Description = this.Description;
					s.IsUploaded = false;
					s.LastUpdated = DateTime.Now;
					s.Snap = this.Snap;
				}
				context.SubmitChanges();
			}

			foreach (setColorModel scm in setColorPalette) {
				scm.saveSetColor(false);
			}

			if (Picture != null)
				Picture.savePicture();

			if (StorageHelper.loadSetting<App.webCommType>("webCommOptions") == App.webCommType.auto) {
				communicationHelper.UploadSet(this);
			}

		}

		public static void setUploaded(Guid id) {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.Set> query = from s in context.Sets
								   where s.Id == id
								   select s;

				if (query.Count() == 0) {
					//don't throw an exception. just ignore 
					//throw new ArgumentException("The set id is not valid when trying to update IsUploaded.");
				}
				else {
					db.Set s = query.First();
					s.IsUploaded = true;
				}
				context.SubmitChanges();
			}

		}

		public static void deleteSet(setModel sm) {
			if (sm == null)
				return;

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				if (sm.Picture != null) {
					IQueryable<db.Picture> query_p = from p in context.Pictures
										   where p.Id == sm.Picture.Id
										   select p;

					if (query_p.Count() == 0) {
						// do nothing
					}
					else {
						db.Picture p = query_p.FirstOrDefault<db.Picture>();
						context.Pictures.DeleteOnSubmit(p);
					}

				}

				IQueryable<db.SetColor> query_sc = from sc in context.SetColors
									     where sc.Set_id == sm.Id
									     select sc;

				foreach (db.SetColor sc in query_sc) {
					db.DeletedSetColor dsc = new DeletedSetColor { Set_id = sc.Set_id, Color_id = sc.Color_id };
					context.SetColors.DeleteOnSubmit(sc);
				}

				IQueryable<db.Set> query = from s in context.Sets
								   where s.Id == sm.Id
								   select s;

				if (query.Count() == 0) {
					// do nothing
				}
				else {
					db.Set s = query.FirstOrDefault<db.Set>();
					context.Sets.DeleteOnSubmit(s);

					db.DeletedSet ds = new DeletedSet {
						Set_id = sm.Id
					};
				}
				context.SubmitChanges();
			}

			if (StorageHelper.loadSetting<App.webCommType>("webCommOptions") == App.webCommType.auto) {
				communicationHelper.DeleteSet(sm.Id);
			}

		}

		public static List<DeletedSet> deletedSets() {
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<DeletedSet> query = from ds in context.DeletedSets
										 select ds;

				return query.ToList<DeletedSet>();
			}
		}

		public static void clearDeletedSet(Guid id) {
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.DeletedSet> query_ds = from ds in context.DeletedSets
										 where ds.Set_id == id
										 select ds;


				if (query_ds.Count() == 0) {
					// do nothing
				}
				else {

					db.DeletedSet ds = query_ds.FirstOrDefault<db.DeletedSet>();
					context.DeletedSets.DeleteOnSubmit(ds);
					context.SubmitChanges();

				}


			}
		}

		public static void deleteSetAndColors(setModel sm) {
			if (sm == null)
				return;

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				if (sm.Picture != null) {
					IQueryable<db.Picture> query_p = from p in context.Pictures
										   where p.Id == sm.Picture.Id
										   select p;

					if (query_p.Count() == 0) {
						// do nothing
					}
					else {
						db.Picture p = query_p.FirstOrDefault<db.Picture>();
						context.Pictures.DeleteOnSubmit(p);
					}

				}

				IQueryable<db.SetColor> query_sc = from sc in context.SetColors
									     where sc.Set_id == sm.Id
									     select sc;

				foreach (db.SetColor sc in query_sc) {

					IQueryable<db.Color> query_c = from c in context.Colors
										 where c.Id == sc.Color_id
										 select c;

					if (query_c.Count() == 0) {
						// do nothing
					}
					else {
						db.Color c = query_c.FirstOrDefault<db.Color>();
						db.DeletedColor dc = new DeletedColor { Color_id = c.Id };
						context.Colors.DeleteOnSubmit(c);
					}

					db.DeletedSetColor dsc = new DeletedSetColor { Set_id = sc.Set_id, Color_id = sc.Color_id };
					context.SetColors.DeleteOnSubmit(sc);

					if (StorageHelper.loadSetting<App.webCommType>("webCommOptions") == App.webCommType.auto) {
						communicationHelper.DeleteColor(sc.Color_id);
					}
				}


				IQueryable<db.Set> query = from s in context.Sets
								   where s.Id == sm.Id
								   select s;

				if (query.Count() == 0) {
					// do nothing
				}
				else {
					db.Set s = query.FirstOrDefault<db.Set>();
					db.DeletedSet ds = new DeletedSet { Set_id = s.Id };
					context.Sets.DeleteOnSubmit(s);
				}
				context.SubmitChanges();
			}

			if (StorageHelper.loadSetting<App.webCommType>("webCommOptions") == App.webCommType.auto) {
				communicationHelper.DeleteSet(sm.Id);
			}

		}

		#region "setColor Manipulation"

		public void setColorZFirst(setColorModel currentScm) {
			foreach (setColorModel cscm in setColorPalette) {
				if (cscm.Z > currentScm.Z)
					cscm.Z -= 1;
			}
			currentScm.Z = setColorPalette.Count - 1;
		}

		/// <summary>
		/// update color position index based on order
		/// </summary>
		public void setColorOrder() {
			List<setColorModel> setColors = setColorPalette.ToList<setColorModel>();
			setColors.Sort(setColorComparer.setColorSorter);

			for (short i = 0; i < setColors.Count; i++) {
				setColors[i].Position = i;
			}

		}

		#endregion


	}
}