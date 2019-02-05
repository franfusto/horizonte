/// <summary>
/// File: Util.cs
/// Author: Francisco Fuster e-mail: franfusto@gmail.com
/// Copyright 2016, Francisco Fuster, All rights reserved.
/// </summary>
using System;
using System.ComponentModel;
using System.Drawing;
using Gtk;
using Gdk;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace horizonte
{

	public static class Util
	{
		public static string GetRandomString(int length)
		{
			Random obj = new Random();
			string posibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			int longitud = posibles.Length;
			char letra;
			string nuevacadena = "";
			for (int i = 0; i < length; i++)
			{
				letra = posibles[obj.Next(longitud)];
				nuevacadena += letra.ToString();
			}
			return nuevacadena;

		}

		public static System.Drawing.Bitmap ToBitmap(this Pixbuf pix)
		{
			try {
				TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
				//// Possible file formats are: "jpeg", "png", "ico" and "bmp"
				return (Bitmap)tc.ConvertFrom(pix.SaveToBuffer("jpeg")); 
				
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex, "ToBitmap","");
				return null;
			}
		}

		public static Gdk.Pixbuf ToPixbuf (this Bitmap bmp)
		{
			try {
				
			using (MemoryStream stream = new MemoryStream()) {
				bmp.Save(stream,System.Drawing.Imaging.ImageFormat.Bmp);
				stream.Position = 0;
				Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(stream);
				return pixbuf;
			}
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex, "ToPixbuf", "");	
				return null;
			}


		}
		public static byte[] GetBytes(string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}
		
		public static string GetString(byte[] bytes)
		{
			char[] chars = new char[bytes.Length / sizeof(char)];
			System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
			return new string(chars);
		}

		public static void GtkDoPending()
		{
			if ( hGestorModulo.CoreMode == CoreModes.Gtk) {
				while (Gtk.Application.EventsPending ())
					Gtk.Application.RunIteration ();
			}
		}

		public static void Zip(string RutaArchivos, string ZipDestino)
		{
			Zip(RutaArchivos, ZipDestino, string.Empty);
		}
		public static void Zip(string RutaArchivos, string ZipDestino, string password)
		{
			try {
				using (ZipFile zip = new ZipFile())
				{
					GtkDoPending();
					if (password != string.Empty)
						zip.Password = password;				
					zip.AddDirectory(RutaArchivos);
					GtkDoPending();
					zip.Save(ZipDestino);
					GtkDoPending();
				}

				
			} catch (System.Exception ex) {
				horizonte.LogEx.CatchEx (ex, "Zip", "");
			}

		}

		public static void UnZip(string ArchivoZip, string RutaDestino)
		{
			UnZip(ArchivoZip, RutaDestino, string.Empty);
		}
		public static void UnZip(string ArchivoZip, string RutaDestino,string Password)
		{
			try {
				using (ZipFile zip = ZipFile.Read(ArchivoZip))
				{
					
					if (Password != string.Empty)
						zip.Password = Password;
					foreach (ZipEntry e in zip)
					{
						GtkDoPending();
						e.Extract(RutaDestino);
					}
				}

				
			} 
			catch (Ionic.Zip.BadPasswordException)
			{
				string pass = string.Empty;
				Util.InputBox(out pass, "Introduzca la clave del archivo", "Password", "");
				UnZip(ArchivoZip, RutaDestino, pass);
			}
			catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"Unzip","");
			}
		}

//		public static void CopyAll(string sourceDir, string targetDir)
//		{
//			try {
//				DirectoryInfo source = new  System.IO.DirectoryInfo(sourceDir);
//				DirectoryInfo target = new System.IO.DirectoryInfo(targetDir);
//
//
//				if (!System.IO.File.Exists (target.FullName)) {
//					System.IO.Directory.CreateDirectory (target.FullName);
//				}
//
//				// Copy each file into the new directory.
//				foreach (FileInfo fi in source.GetFiles())
//				{
//					Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
//					fi.CopyTo(System.IO.Path.Combine(target.FullName, fi.Name), true);
//				}
//
//				// Copy each subdirectory using recursion.
//				foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
//				{
//					DirectoryInfo nextTargetSubDir =
//						target.CreateSubdirectory(diSourceSubDir.Name);
//					CopyAll(diSourceSubDir.FullName, nextTargetSubDir.FullName);
//				}	
//			} catch (Exception ex) {
//				Util.AlertMessage (Catalog.GetString("Error al copiar archivos"));
//				Console.WriteLine (ex);
//			}
//
//		}

		public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
		{
			try {
				Directory.CreateDirectory(target.FullName);

				// Copy each file into the new directory.
				foreach (FileInfo fi in source.GetFiles())
				{
			
					fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
				}

				// Copy each subdirectory using recursion.
				foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
				{
					GtkDoPending();
					DirectoryInfo nextTargetSubDir =
						target.CreateSubdirectory(diSourceSubDir.Name);
					CopyAll(diSourceSubDir, nextTargetSubDir);
				}	
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"CopyAll","");
			}

		}
		public static void Copy (string Source, string Dest, bool IsDir)
		{	
			try {
				
				if (IsDir) {
					CopyAll (new System.IO.DirectoryInfo(Source), new System.IO.DirectoryInfo(Dest));
				} else {
					System.IO.File.Copy (Source, Dest);
				}
			} catch (Exception ex) {
				LogEx.CatchEx (ex,"Copy");
			}	
//
//			string Recursive = "";
//			if (IsDir) {
//				Recursive = "-r ";
//			}
//			string param = Recursive + SourceDir +" -d " + DestDir;
//			Process P;
//			P = Process.Start("cp",param);
//			P.WaitForExit(2000);
		}
		public static void Delete (string FileDir, bool IsDir)
		{
			if (IsDir && (System.IO.Directory.Exists (FileDir))) {
				System.IO.Directory.Delete (FileDir, true);
			} else {
				System.IO.File.Delete(FileDir);
			}
		}

		public static void AlertMessage( string Mensaje)
		{
			try {
				Gtk.Application.Invoke (delegate {
					MessageDialog md = new MessageDialog(null, 
						DialogFlags.Modal, MessageType.Warning, 
						ButtonsType.Ok, Mensaje);
					md.Run();
					md.Destroy();
				});
				
			} catch (Exception ex) {
				LogEx.CatchEx (ex, "AlertMessage");
			}
		}

		public static void ReemplazarTexto (string FileName, string InText, string OutTest)
		{
			try {
				using (StreamWriter fileWrite = new StreamWriter("tmp.txt"))
				{
					using (StreamReader fielRead = new StreamReader(FileName))
					{
						string line;

						while ((line = fielRead.ReadLine()) != null)
						{
							GtkDoPending();
							if(line.Contains(InText)){
								//Gtk.Application.Invoke (delegate {
									fileWrite.WriteLine(line.Replace(InText,OutTest));

								//});
							}
							else
							{
								//Gtk.Application.Invoke (delegate {
									fileWrite.WriteLine(line);

								//});
							}
						}
					}
				}
				File.Delete(FileName);
				File.Move("tmp.txt", FileName);	
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}

		}
		public static  Dictionary<string,string> SplitDocData (string DocData)
		{

			try {
				Dictionary<string,string> DataKeyList = new Dictionary<string,string> ();
				string[] lines = DocData.Split(new [] { '\r', '\n',';' });
				foreach (string item in lines) {
					string[] parts = item.Split (new [] { '=',':'});
					if (parts.Length == 0)
					{
						//nada
					}else{
						if (parts.Length==1){
							//
							DataKeyList.Add(parts[0],"");
						}else{
							DataKeyList.Add(parts[0],parts[1]);	
						}
					}

				}


				return DataKeyList;



			} catch (Exception ex) {

				horizonte.LogEx.CatchEx (ex,"SplitDocData","");
				return new Dictionary<string,string>();
			}
		}
		public static bool InputBox (out string IntroText, string Mensaje,string Titulo,string DefaultText)
		{
			bool ret;
			Dialog dialog = null;
			ResponseType response = ResponseType.None;
			try {
				if (!horizonte.hGestorModulo.GtkEnabled) {
					Gtk.Application.Invoke (delegate {
						Console.WriteLine ("** " + Titulo + " **");
						Console.WriteLine (Mensaje);
					});
					IntroText = Console.ReadLine ();
					return true;
				}else			{
					dialog = new Dialog (
						Titulo,
						null,
						DialogFlags.DestroyWithParent | DialogFlags.Modal,
						Tr.Catalog.GetString (Tr.Catalog.GetString ("Aceptar")), ResponseType.Yes,
						Tr.Catalog.GetString (Tr.Catalog.GetString("Cancelar")), ResponseType.No
					);
					Label _l = new Label (Mensaje);
					Entry _e = new Entry (DefaultText);
					dialog.VBox.Add (_l);
					dialog.VBox.Add (_e);
					dialog.BorderWidth = 20;
					dialog.ShowAll ();
					response = (ResponseType) dialog.Run ();
					if (response == ResponseType.Yes)
					{
						IntroText = _e.Text;
						ret = true;
					}
					else
					{
						IntroText = "";
						ret = false;
					}
					if (dialog != null)	dialog.Destroy ();
					return ret;
				}
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"InputBox","");
				if (dialog != null)	dialog.Destroy ();
				IntroText = "";
				return false;
			}
		}
	}
}

//
