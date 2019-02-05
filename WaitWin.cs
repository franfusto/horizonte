/// <summary>
/// File: WaitWin.cs
/// Author: Francisco Fuster e-mail: franfusto@gmail.com
/// Copyright 2016, Francisco Fuster, All rights reserved.
/// </summary>
using System;
using Gtk;
using System.Threading;

namespace horizonte
{
	public static class WaitWin
	{
		public static SplashWindows win; 
		public static WaitWinController controller;
		public static void Init()
		{
			if (controller == null)
				controller = new WaitWinController ();
		}
		public  static void Hide()
		{
			Init ();
			controller.Hide ();

		}
		public  static void SetText(string Text,int mode)
		{
			Init ();
			//Gtk.Application.Invoke (delegate {
				controller.SetText (Text, mode);
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration ();
			//});

		}

	}
	public class WaitWinController
	{
		public virtual void Hide()
		{
			WaitWin.win.Hide ();
		}
		public virtual void SetText(string Text, int mode)
		{
			if (!horizonte.hGestorModulo.GtkEnabled) {
				Console.WriteLine (Text);
			} else {
				if (WaitWin.win == null)
					WaitWin.win =new SplashWindows(); 
				WaitWin.win.ShowAll ();
				WaitWin.win.SetText(Text,mode);
			}
		}
	}
	public class SplashWindows: Gtk.Window
	{


		public Label TxtLabel = new Label();

        public Gdk.PixbufAnimation WaitImg = Gdk.PixbufAnimation.LoadFromResource("horizonte.espere.gif");
        public Gdk.Pixbuf ErrorImg = Gdk.Pixbuf.LoadFromResource("horizonte.error.png");
        //public Gdk.Pixbuf ErrorImg = new Gdk.Pixbuf(null, "error.png");
        //public Gdk.PixbufAnimation ErrorImg = new Gdk.PixbufAnimation("/home/programacion3/Desktop/error.gif");
		HBox hbox1 = new HBox ();
		Image img1 = new Image ();

		public SplashWindows () : base (WindowType.Popup)
		//public SplashWindows () : base (WindowType.Toplevel)
		{
			try {
				this.SkipTaskbarHint = true;
				this.CanFocus = false;
				this.KeepAbove = true;
				this.Decorated = false;
				this.SetDefaultSize (100, 50);
				this.Resizable = false;
				this.Move( 50  , 50); 


				this.Add (hbox1);
                img1.PixbufAnimation = WaitImg;
               // img1.Pixbuf = WaitImg;
				hbox1.PackStart (img1,false,false,3);
				hbox1.PackStart (TxtLabel,false,true,3);
				//hbox1.Add (TxtLabel);
				SetText ("Procesando.....",0);
				//this.ShowAll ();

				
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}


		}
		public void SetImg (int mode)
		{
			try {
				switch (mode) {
				case 0: // info
                        this.img1.PixbufAnimation = WaitImg;
                        //this.img1.Pixbuf = WaitImg;
					this.ModifyBg (StateType.Normal, new Gdk.Color (211, 171, 255));
					break;
				case 1: // error
					this.img1.Pixbuf = ErrorImg;
					//this.img1.PixbufAnimation = ErrorImg;
					this.ModifyBg (StateType.Normal, new Gdk.Color (255, 200, 200));
					new Thread (delegate() {
						Thread.Sleep (2000);
						Gtk.Application.Invoke (delegate {
							WaitWin.Hide ();
						});
						
					}).Start ();
					break;
				default:
					break;
				}
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
		}
		public void SetText (string Text)
		{
			SetText (Text,0);
		}

		public void SetText (string Text, int mode)
		{
			try {
				SetImg (mode);
				img1.Show ();
				this.TxtLabel.Text = Text;
			} catch (Exception ex) {
				Console.WriteLine (ex);				
			}
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			a.RetVal = true;
		}

	}

}

