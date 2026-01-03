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
using Microsoft.Phone.Tasks;
using System.Windows.Controls.Primitives;

namespace Color_Data_3._0.Selectors {


	public class imageResult : TaskEventArgs {

		public override TaskResult TaskResult {
			get {
				return base.TaskResult;
			}
		}

		public System.IO.Stream image { get; set; }
	}

	public class colorResult : TaskEventArgs {

		public override TaskResult TaskResult {
			get {
				return base.TaskResult;
			}
		}
	}

	public class colorChooserTask : ChooserBase<PhotoResult> {

		private Popup p;
		public Popup _Popup { 
			get {
				return p;
			} 
		}

		public override void Show() {
			p = new Popup();
			p.IsOpen = true;
			ColorSelector cs = new ColorSelector();
			p.Child = cs;
			p.Closed += new EventHandler(p_Closed);
		}

		void p_Closed(object sender, EventArgs e) {
			Popup p = sender as Popup;
			if (p.Tag != null) {
				TaskResult tr = (TaskResult)p.Tag;
				var r = new PhotoResult();
				FireCompleted(sender, new PhotoResult(tr), null);
			}
			else
				FireCompleted(sender, new PhotoResult(TaskResult.None), null);
		}


	}
}
