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
using System.Collections.Generic;
using System.Xml.Linq;

namespace Color_Data_3._0.Classes {
	public class xmlHelper {

		public static XElement MergeElements(XElement e1, XElement e2) {
			var attrComparer = new XAttributeEqualityComparer();
			var nameComparer = new XNameComparer();

			var attributes = e2.Attributes().Union(e1.Attributes(), attrComparer).Cast<XNode>();

			var elements1 = e1.Elements().OrderBy(e => e.Name, nameComparer).ToArray();
			var elements2 = e2.Elements().OrderBy(e => e.Name, nameComparer).ToArray();
			var elements = new List<XNode>();
			int i1 = 0, i2 = 0;
			while (i1 < elements1.Length && i2 < elements2.Length) {
				XElement e = null;
				int compResult = nameComparer.Compare(elements1[i1].Name, elements2[i2].Name);
				if (compResult < 0) {
					e = elements1[i1];
					i1++;
				}
				else if (compResult > 0) {
					e = elements2[i2];
					i2++;
				}
				else {
					e = MergeElements(elements1[i1], elements2[i2]);
					i1++;
					i2++;
				}
				elements.Add(e);
			}
			while (i1 < elements1.Length) {
				elements.Add(elements1[i1]);
				i1++;
			}
			while (i2 < elements2.Length) {
				elements.Add(elements2[i2]);
				i2++;
			}

			var nodes = attributes.Concat(elements).ToArray();
			string value = null;
			if (elements.Count == 0) {
				if (!string.IsNullOrEmpty(e1.Value))
					value = e1.Value;
				if (!string.IsNullOrEmpty(e2.Value))
					value = e2.Value;
			}
			if (value != null)
				return new XElement(e1.Name, nodes, value);
			else
				return new XElement(e1.Name, nodes);
		}

		class XNameComparer : IComparer<XName> {
			public int Compare(XName x, XName y) {
				int result = string.Compare(x.Namespace.NamespaceName, y.Namespace.NamespaceName);
				if (result == 0)
					result = string.Compare(x.LocalName, y.LocalName);
				return result;
			}
		}

		class XAttributeEqualityComparer : IEqualityComparer<XAttribute> {
			public bool Equals(XAttribute x, XAttribute y) {
				return x.Name == y.Name;
			}

			public int GetHashCode(XAttribute x) {
				return x.Name.GetHashCode();
			}
		}


	}
}
