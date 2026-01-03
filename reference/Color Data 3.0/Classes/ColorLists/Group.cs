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
using System.Collections.Generic;

namespace Color_Data_3._0.Classes.ColorLists {

	public class Group<T> : IEnumerable<T> {

		public string Title { get; set; }

		public IList<T> Items { get; set; }

		public bool isNotEmpty {
			get {
				return !(Items == null || Items.Count == 0);
			}
		}

		public Group(string name, IEnumerable<T> items) {
			this.Title = name;
			this.Items = new List<T>(items);
		}

		public override bool Equals(object obj) {
			Group<T> that = obj as Group<T>;

			return (that != null) && (this.Title.Equals(that.Title));
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}



		public IEnumerator<T> GetEnumerator() {
			return this.Items.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.Items.GetEnumerator();
		}
	}

}
