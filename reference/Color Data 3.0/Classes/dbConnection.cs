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
using Color_Data_3._0.db;

namespace Color_Data_3._0.Classes {

	public static class dbConnection {

		public const string ConnectionString = @"isostore:/colordataDb.sdf";

		//Database creation should only ever happen once on first launch
		public static void createDatabase() {
			using (cdDb_DataContext context = new cdDb_DataContext(ConnectionString)) {
				if (!context.DatabaseExists()) {
					// create database if it does not exist
					context.CreateDatabase();
				}
			}

		}
	}
}
