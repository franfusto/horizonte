using System;
using System.IO;
using System.Data;
using Gtk;

namespace horizonte
{
	public class PanelModulo
	{
		public PanelModulo ()
		{
		}
		/// <summary>
		/// Listar los módulos en consola.
		/// </summary>
		public static void ListModules()
		{

			foreach (DataRow R in hGestorModulo.TablaModulos.Rows)
			{
				Console.WriteLine((string)R[0] + ";" + (string)R[1]);
			}
		}
		/// <summary>
		/// Lista todos los comandos disponibles en consola
		/// </summary>
		public static void ListCommands()
		{
			foreach (string _cmd in hGestorModulo.CommandList.Keys) {
				Console.WriteLine(_cmd);
			}
		}
		public static void Quit()
		{
			hGestorModulo.Dispose ();
			if (hGestorModulo.GtkEnabled)
				Gtk.Application.Quit();
		}

		public static void BackupEnviroment()
		{
			string signature = DateTime.Now.Ticks.ToString();
			string tmpfolder = signature;
			string filename = signature+".back";
			string modfolder = hGestorModulo.ConfigTool.GetValor ("RutaModulos");

			try {
				// creamos carpeta temporal
				WaitWin.SetText("Creando backup.....",0);
				if (Directory.Exists(tmpfolder))
					Directory.Delete(tmpfolder,true);
				Directory.CreateDirectory(tmpfolder);
				//copiamos contenido de la carpeta mudulos
				Util.CopyAll(new DirectoryInfo(modfolder),
					new DirectoryInfo (tmpfolder));
				//copiamos el archivo de configuración
				Util.Copy(hGestorModulo.ConfigFile,Path.Combine(tmpfolder,hGestorModulo.ConfigFile),false);
				//comprimimos la carpeta temporal
				Util.AlertMessage(Tr.Catalog.GetString("Creado backup en ") + filename);

			} catch (Exception ex) {
				WaitWin.Hide ();
				LogEx.CatchEx (ex);
			}
		}
		public static void RestoreEnviroment(string BackupFile)
		{
			try {
				// hor://comando/horizonte_ShowWidget/hbuild_ModConsole
			} catch (Exception ex) {
				LogEx.CatchEx (ex);
			}
		}
		public static void AskRegister()
		{
			horizonte.hGestorModulo.AskRegister ();
		}
		public static void ShowWidget(string winname, string widget)
		{
			horizonte.hGestorModulo.TestCommandWn (winname,0,0,widget,"");
		}
		public static void ShowWidgetArgs(string winname, string widget,string param)
		{
			horizonte.hGestorModulo.TestCommandWn (winname,0,0,widget,param);
		}
	}
}

