using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

using Color_Data_3._0.db;
using System.Linq;
using Color_Data_3._0.Classes.ColorLists;


namespace Color_Data_3._0.Classes {

	public class MainViewModel : INotifyPropertyChanged {

		ObservableCollection<setModel> _setItems;
		public ObservableCollection<setModel> setItems {
			get {
				return _setItems;
			}
			private set {
				_setItems = value;
				NotifyPropertyChanged("setItems");
			}
		}

		public bool IsSetDataLoaded { get; private set; }

		ObservableCollection<colorModel> _colorItems;
		public ObservableCollection<colorModel> colorItems {
			get {
				return _colorItems;
			}
			private set {
				_colorItems = value;
				NotifyPropertyChanged("colorItems");
			}
		}
		public bool IsColorDataLoaded { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(String propertyName) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (null != handler) {
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public MainViewModel() {
			this.colorItems = new ObservableCollection<colorModel>();
			this.setItems = new ObservableCollection<setModel>();
		}

		public void LoadColorData() {
			colorItems = colorModel.getColors();
			this.IsColorDataLoaded = true;
		}

		public void LoadColorData(string searchString) {
			colorItems = colorModel.getColors(searchString.ToLower());
			this.IsColorDataLoaded = true;
		}

		public void LoadSetData() {

			setItems = setModel.getSets();
			this.IsSetDataLoaded = true;

		}

		public void LoadSetData(string searchString) {
			setItems = setModel.getSets(searchString.ToLower());
			this.IsColorDataLoaded = true;
		}

		//TEMP FUNCTIONS

		static Random r = new Random();

		public static string randomStringBuilder(int length) {

			StringBuilder sb = new StringBuilder();

			bool lastSpace = true;
			bool isFirst = true;

			while (sb.ToString().Length < length) {
				if (isFirst) {
					sb.Append(Convert.ToChar(r.Next(65, 90)));
					isFirst = false;
				}
				else if (r.Next(0, 7) == 0) {
					sb.Append(' ');
					lastSpace = true;
				}
				else {
					if (lastSpace && r.Next(0, 20) == 0)
						sb.Append(Convert.ToChar(r.Next(65, 90)));
					else
						sb.Append(Convert.ToChar(r.Next(97, 122)));
					lastSpace = false;
				}
			}

			return sb.ToString();
		}

		public static void addRandomSetToDB(int setCnt, int colorMinCnt, int colorMaxCnt) {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {

				for (int h = 0; h < setCnt; h++) {
					db.Set s = new db.Set();
					s.Id = Guid.NewGuid();
					s.Title = randomStringBuilder(20);
					s.Description = randomStringBuilder(150);
					s.DateCreated = DateTime.Now;
					s.LastUpdated = DateTime.Now;
					context.Sets.InsertOnSubmit(s);

					int colorCnt = r.Next(colorMinCnt, colorMaxCnt);
					for (int i = 0; i < colorCnt; i++) {
						db.Color c = new db.Color();
						c.Id = Guid.NewGuid();
						c.Title = randomStringBuilder(20);
						c.Description = randomStringBuilder(150);
						c.R = (short)r.Next(0, 255);
						c.G = (short)r.Next(0, 255);
						c.B = (short)r.Next(0, 255);

						context.Colors.InsertOnSubmit(c);

						db.SetColor sc = new SetColor();
						sc.Color_id = c.Id;
						sc.Set_id = s.Id;
						//WARNING: Hardcored values 
						sc.IconSize = 120;
						sc.X = Math.Floor(i / setColorModel.MAX_ROWS) * (sc.IconSize + 10);
						sc.Y = (i % setColorModel.MAX_ROWS) * (sc.IconSize + 10);
						sc.Position = (short)i;
						sc.Z = i;
						context.SetColors.InsertOnSubmit(sc);
					}
				}

				// save changes to the database
				context.SubmitChanges();
			}
		}

		public static void addRandomColorsToDB(int count) {

			using (cdDb_DataContext context = new cdDb_DataContext(dbConnection.ConnectionString)) {
				for (int i = 0; i < count; i++) {
					db.Color c = new db.Color();
					c.Id = Guid.NewGuid();
					c.Title = randomStringBuilder(20);
					c.Description = randomStringBuilder(150);
					c.R = (short)r.Next(0, 255);
					c.G = (short)r.Next(0, 255);
					c.B = (short)r.Next(0, 255);

					context.Colors.InsertOnSubmit(c);
				}

				// save changes to the database
				context.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
			}



		}




	}
}