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
using media = System.Windows.Media;
using Color_Data_3._0.db;

namespace Color_Data_3._0.Classes {
	public class setColorModel : colorModel {

		public const int INITIAL_RECT_SIZE = 100;
		public const int MAX_ROWS = 5;

		private Guid _setId;
		public Guid Set_Id {
			get {
				return _setId;
			}
			set {
				if (value != _setId) {
					_setId = value;
					base.NotifyPropertyChanged("Set_Id");
				}
			}
		}

		private double _X;
		public double X {
			get {
				return _X;
			}
			set {
				if (value != _X) {
					_X = value;
					base.NotifyPropertyChanged("X");
				}
			}
		}

		private double _Y;
		public double Y {
			get {
				return _Y;
			}
			set {
				if (value != _Y) {
					_Y = value;
					base.NotifyPropertyChanged("Y");
				}
			}
		}

		private int _Z;
		public int Z {
			get {
				return _Z;
			}
			set {
				if (value != _Z) {
					_Z = value;
					base.NotifyPropertyChanged("Z");
				}
			}
		}

		private short _Position;
		public short Position {
			get {
				return _Position;
			}
			set {
				if (value != _Position) {
					_Position = value;
					base.NotifyPropertyChanged("Position");
				}
			}
		}

		private int _iconSize;
		public int IconSize {
			get {
				return _iconSize;
			}
			set {
				if (value != _iconSize) {
					_iconSize = value;
					base.NotifyPropertyChanged("IconSize");
				}
			}
		}

		private bool _IsSetColorUploaded;
		public bool IsSetColorUploaded {
			get {
				return _IsSetColorUploaded;
			}
			set {
				if (value != _IsSetColorUploaded) {
					_IsSetColorUploaded = value;
					NotifyPropertyChanged("IsSetColorUploaded");
				}
			}
		}

		public static setColorModel getSetColor(Guid set_id, Guid color_id) {

			db.SetColor setColor = null;
			db.Color color = null;
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.SetColor> query = from sc in context.SetColors
									  where sc.Set_id == set_id && sc.Color_id == color_id
									  orderby sc.Position ascending
									  select sc;

				IQueryable<db.Color> query_color = from _c in context.Colors
									     where _c.Id == color_id
									     select _c;

				setColor = query.FirstOrDefault<db.SetColor>();
				color = query_color.FirstOrDefault<db.Color>();
			}

			setColorModel scm = new setColorModel() {
				Id = color.Id,
				Title = color.Title,
				Description = color.Description,
				ColorBrush = new SolidColorBrush(media.Color.FromArgb(255, (byte)color.R, (byte)color.G, (byte)color.B)),
				IsUploaded = color.IsUploaded,
				Set_Id = setColor.Set_id,
				X = setColor.X,
				Y = setColor.Y,
				Z = setColor.Z,
				Position = setColor.Position,
				IconSize = setColor.IconSize,
				IsSetColorUploaded = setColor.IsUploaded
			};

			return scm;

		}

		public static ObservableCollection<setColorModel> getSetColors(Guid set_id) {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {
				IQueryable<setColorModel> query = from sc in context.SetColors
									    join c in context.Colors on sc.Color_id equals c.Id
									    where sc.Set_id == set_id
									    orderby sc.Position ascending
									    select new setColorModel() {
										    Id = c.Id,
										    Title = c.Title,
										    Description = c.Description,
										    ColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)c.R, (byte)c.G, (byte)c.B)),
										    IsUploaded = c.IsUploaded,
										    Set_Id = sc.Set_id,
										    X = sc.X,
										    Y = sc.Y,
										    Z = sc.Z,
										    Position = sc.Position,
										    IconSize = sc.IconSize,
										    IsSetColorUploaded = sc.IsUploaded
									    };

				ObservableCollection<setColorModel> cm = new ObservableCollection<setColorModel>(query);

				return cm;
			}


		}

		public static setColorModel createNewSetColorModel(int pos, string title, string desc, System.Windows.Media.Color c, Guid setId) {
			return new setColorModel() {
				Id = Guid.NewGuid(),
				Title = title,
				Description = desc,
				ColorBrush = new SolidColorBrush(c),
				IconSize = setColorModel.INITIAL_RECT_SIZE,
				IsUploaded = false,
				Set_Id = setId,
				Position = (short)pos,
				X = Math.Floor(pos / MAX_ROWS) * (setColorModel.INITIAL_RECT_SIZE + 10),
				Y = (pos % MAX_ROWS) * (setColorModel.INITIAL_RECT_SIZE + 10),
				Z = pos,
				IsSetColorUploaded = false
			};
		}

		public static setColorModel ConvertColorModel(colorModel cm, setModel sm, int pos) {
			return new setColorModel() {
				Id = cm.Id,
				Title = cm.Title,
				Description = cm.Description,
				ColorBrush = cm.ColorBrush,
				IconSize = setColorModel.INITIAL_RECT_SIZE,
				IsUploaded = false,
				Set_Id = sm.Id,
				Position = (short)pos,
				X = Math.Floor(pos / MAX_ROWS) * (setColorModel.INITIAL_RECT_SIZE + 10),
				Y = (pos % MAX_ROWS) * (setColorModel.INITIAL_RECT_SIZE + 10),
				Z = pos,
				IsSetColorUploaded = false
			};
		}

		public void saveSetColor(bool upload) {
			saveColor(false);

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.SetColor> query = from sc in context.SetColors
									  where sc.Color_id == Id && sc.Set_id == Set_Id
									  select sc;

				if (query.Count() == 0) {
					db.SetColor sc = new db.SetColor {
						Color_id = this.Id,
						Set_id = this.Set_Id,
						X = this.X,
						Y = this.Y,
						Position = this.Position,
						Z = this.Z,
						IconSize = this.IconSize,
						IsUploaded = this.IsSetColorUploaded
					};
					context.SetColors.InsertOnSubmit(sc);
				}
				else {
					db.SetColor sc = query.First();
					sc.X = this.X;
					sc.Y = this.Y;
					sc.Position = this.Position;
					sc.Z = this.Z;
					sc.IconSize = this.IconSize;
					sc.IsUploaded = this.IsSetColorUploaded;
				}
				context.SubmitChanges();
			}


			if (upload && StorageHelper.loadSetting<App.webCommType>("webCommOptions") == App.webCommType.auto) {
				communicationHelper.UploadSetColor(this);
			}

		}

		public static void setColorUploaded(Guid colorId, Guid setId) {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.SetColor> query = from sc in context.SetColors
									  where sc.Color_id == colorId && sc.Set_id == setId
									  select sc;

				if (query.Count() == 0) {
					//don't throw an exception. just ignore 
					//throw new ArgumentException("The set id is not valid when trying to update IsUploaded.");
				}
				else {
					db.SetColor sc = query.First();
					sc.IsUploaded = true;
				}
				context.SubmitChanges();
			}

		}

		public static void deleteSetColor(setColorModel scm) {
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.SetColor> query = from sc in context.SetColors
									  where sc.Color_id == scm.Id && sc.Set_id == scm.Set_Id
									  select sc;

				if (query.Count() == 0) {
					return;
				}
				else {
					db.SetColor sc = query.FirstOrDefault<db.SetColor>();
					context.SetColors.DeleteOnSubmit(sc);

					db.DeletedSetColor dsc = new DeletedSetColor {
						Color_id = scm.Id,
						Set_id = scm.Set_Id
					};
				}
				context.SubmitChanges();
			}

			if (StorageHelper.loadSetting<App.webCommType>("webCommOptions") == App.webCommType.auto) {
				communicationHelper.DeleteSetColor(scm.Set_Id, scm.Id);
			}
		}

		public static List<DeletedSetColor> deletedSetColors() {
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<DeletedSetColor> query = from dsc in context.DeletedSetColors
										select dsc;

				return query.ToList<DeletedSetColor>();
			}
		}

		public static void clearDeletedSetColor(Guid setId, Guid colorId) {
			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				IQueryable<db.DeletedSetColor> query_dsc = from dsc in context.DeletedSetColors
											 where dsc.Set_id == setId && dsc.Color_id == colorId
											 select dsc;


				if (query_dsc.Count() == 0) {
					// do nothing
				}
				else {

					db.DeletedSetColor dsc = query_dsc.FirstOrDefault<db.DeletedSetColor>();
					context.DeletedSetColors.DeleteOnSubmit(dsc);
					context.SubmitChanges();

				}


			}
		}

		//WARNING: fixed values 
		public int IncreaseIconSize(int inc, int maxWidth, int maxHeight) {
			if (IconSize < 200) {

				if (X + IconSize + inc > maxWidth)
					X -= X + IconSize + inc - maxWidth;
				if (Y + IconSize + inc > maxHeight)
					Y -= Y + IconSize + inc - maxHeight;

				IconSize += inc;
			}

			return IconSize;
		}

		//WARNING: fixed values 
		public int decreaseIconSize(int inc) {
			if (IconSize > 90) {
				IconSize -= inc;
			}
			return IconSize;
		}

	}
}