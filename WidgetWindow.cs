using System;
using Gtk;

namespace horizonte
{
	public class WidgetWindow : Gtk.Window
	{
		private Widget appwidget;

		public WidgetWindow (Widget AppWidget,string WinName): base(Gtk.WindowType.Toplevel)
		{
			this.Title = WinName;
			DeleteEvent += OnDeleteEvent;
			appwidget = AppWidget;
			this.Add (appwidget);
			appwidget.Destroyed += (object sender, EventArgs e) => this.Destroy();
			appwidget.Show ();
			this.Show ();
		}


		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			appwidget.Destroy ();
		}


	}
}

