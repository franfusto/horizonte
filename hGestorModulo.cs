/// <summary>
/// File: hGestorModulo.cs
/// Author: Francisco Fuster e-mail: franfusto@gmail.com
/// Copyright 2016, Francisco Fuster, All rights reserved.
/// </summary>
using System;
using Gtk;
//using Gdk;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
//using DevOne.Security.Cryptography.BCrypt;
//using System.Net;
//using System.Text;
//using System.IO;
//using System.Threading.Tasks;

namespace horizonte
{
	public enum CoreModes
	{
		Gtk,
		Xwt,
		Console,
		Web
	}
	public static class hGestorModulo
	{
		public static event EventHandler Disposing;
		public static event EventHandler ModulesLoaded;
		/// creamos el ensamblado para el módulo
		public static Assembly currAssembly;
		public static string ConfigFile;
		private static string ModKey=string.Empty;
		public static int ExecLeft = -1 ;
		public static void Dispose ()
		{
			Disposing (null, EventArgs.Empty);
			//Util.AlertMessage ("Dispose hgestormodulo");
		}


		// creamos el diccionario donde guardaremos la lista de commandos
		public static  Dictionary<string,object> CommandList = new Dictionary<string, object>();
		// creamos el auxiliar de configuración
		public static  hConfigTool ConfigTool;
		// creamos la tabla para guarar la configuración
		public static  DataTable TablaModulos ;

		public static  String ArchivoXmlConfig;

		//public DataTable _ModuloConfigDataTable;

		public static CoreModes CoreMode {

			get	{
				return CurMode;
			}
			set	{
				try {
					CurMode = value;
					switch (value) {
					case CoreModes.Console:
						GtkEnabled = false;
						XwtEnabled = false;
						WebEnabled = false;
						break;
					case CoreModes.Web:
						GtkEnabled = false;
						XwtEnabled = false;
						WebEnabled = true;
						break;
					case CoreModes.Xwt:
						GtkEnabled = false;
						XwtEnabled = true;
						WebEnabled = false;
						break;
					default: // Gtk
						GtkEnabled = true;
						XwtEnabled = false;
						WebEnabled = false;
						break;
					}
				} catch (Exception ex) {
					Console.WriteLine (ex);
				}

			}
		}
		private static CoreModes CurMode = CoreModes.Gtk;
		public static  bool GtkEnabled = true;
		public static  bool XwtEnabled = false;
		public static  bool WebEnabled = false;



		#region Registro

		private static string registgername;
		public static string RegisterName {
			get
			{return registgername; }
		}
		private static string registerkey;
		public static string RegisterKey {
			get
			{return registerkey; }
		}
		private static bool coreactive = false;
		public static bool CoreActive {
			get
			{return coreactive; }
		}

		//public static void AskRegister()
		//{
		//	try {
		//		string configroup;
		//		string configkey;
		//		ConfigTool.GetValor ("HKEY", out configkey, out configroup);
		//		string configcert;
		//		ConfigTool.GetValor ("HCERT", out configcert, out configroup);
		//		string configname;
		//		ConfigTool.GetValor ("HNAME", out configname, out configroup);
		//		string hregsrv;
		//		ConfigTool.GetValor ("HREGSRV", out hregsrv, out configroup);
		//		string val1 = System.Environment.UserName ;
		//		string val2 = System.Environment.MachineName;
		//		string val3 = System.Environment.OSVersion.ToString();
		//		string val4 = ModKey;
		//		//HREGSRV
		//		var request = (HttpWebRequest)WebRequest.Create(hregsrv);
		//		var postData = "a1=" + configkey;
		//		postData += "&a2=" + configname;
		//		postData += "&a3=" + val1;
		//		postData += "&a4=" + val2;
		//		postData += "&a5=" + val3;
		//		postData += "&a6=" + val4;

		//		var data = Encoding.ASCII.GetBytes(postData);
		//		request.Method = "POST";
		//		request.ContentType = "application/x-www-form-urlencoded";
		//		request.ContentLength = data.Length;
		//		using (var stream = request.GetRequestStream())
		//		{
		//			stream.Write(data, 0, data.Length);
		//		}
		//		var response = (HttpWebResponse)request.GetResponse();
		//		var responseString = new StreamReader(response.GetResponseStream())
		//			.ReadToEnd().Trim();
		//	//TODO: Validar el texto devuelto para ver si encaja con el formato del hash.......

		//		Console.WriteLine(Tr.Catalog.GetString ("RECIBIMOS DEL REGISTRO WEB:##")  +
		//			Environment.NewLine +  responseString + Environment.NewLine +"##");
		//		hGestorModulo.ConfigTool.SetValue("HCERT",responseString,"");
		//		hGestorModulo.ConfigTool.SaveConfig();
		//		Console.WriteLine(responseString);
		//		Console.WriteLine(Tr.Catalog.GetString("Se ha registrado el programa, reinicie la aplicación"));
		//		Quit();

		//	} catch (Exception ex) {
		//		horizonte.LogEx.CatchEx (ex,"AskRegister","");
		//		Console.WriteLine(Tr.Catalog.GetString("No se ha podido registrar el programa"));
		//		Quit();
		//	}

		//}
		public static void Quit()
		{
			Disposing(null, EventArgs.Empty);
			Environment.Exit(1);
		}
		//private static void validate()
		//{
		//	try {

		//		coreactive = false;
		//		string configkey;
		//		string configroup;
		//		if (!ConfigTool.GetValor("HKEY",out configkey,out configroup)){
		//			Console.WriteLine(Tr.Catalog.GetString ("NO SE ENCUENTRA LA CLAVE DE RIEGISTRO. SALIENDO...."));
		//			coreactive = false;
		//		}
		//		string configcert;
		//		if (!ConfigTool.GetValor("HCERT",out configcert,out configroup)){
		//			Console.WriteLine(Tr.Catalog.GetString ("NO SE ENCUENTRA EL CERTIFICADO DE LA APLICACIÓN. SALIENDO...."));
		//			coreactive = false;
		//		}
		//		if (string.IsNullOrEmpty(configcert))
		//		{
		//			Console.WriteLine(Tr.Catalog.GetString ("NO SE ENCUENTRA EL CERTIFICADO DE LA APLICACIÓN. SALIENDO...."));
		//			coreactive = false;					
		//		}
		//		string configname;
		//		if (!ConfigTool.GetValor("HNAME",out configname,out configroup)){
		//			Console.WriteLine(Tr.Catalog.GetString ("NO SE ENCUENTRA EL NOMBRE DE REGISTRO. SALIENDO...."));
		//			coreactive = false;
		//		}
		//		string modkey;
		//		if (!ConfigTool.GetValor("MODKEY",out modkey,out modkey)){
		//			Console.WriteLine(Tr.Catalog.GetString ("NO SE ENCUENTRA LA CLAVE DE MÓDULO SALIENDO...."));
		//			coreactive = false;
		//		}


		//		////
		//		string val1 = System.Environment.UserName ;
		//		string val2 = System.Environment.MachineName;
		//		string val3 = System.Environment.OSVersion.ToString();
		//		ModKey = OrderModKey (modkey); //comprobamos la lista de modulos ordenamos y establedemos ModKeyList
		//		string val4 = ModKey ;
		//		string step0 = configkey+val1+val2+val3+configname + val4;
		//		string step1 = step0.Trim ();
		//		string hpass = step1.ToUpper ();



		//		if (BCryptHelper.CheckPassword (hpass, configcert)) {
		//			Console.WriteLine (Tr.Catalog.GetString ("### NUCLEO REGISTRADO ####"));
		//			coreactive = true;
		//		} else {
		//			Console.WriteLine (Tr.Catalog.GetString ("*******PROGRAMA NO REGISTRADO*********"));
		//			int lombriz = CheckLombriz ();
		//			if (lombriz == 0)
		//			{
		//				AskRegister ();
		//				coreactive = false;
		//			}else
		//			{
		//				ExecLeft = lombriz;
		//				Console.WriteLine ("### DEMO: " + lombriz.ToString ()  + Tr.Catalog.GetString (" EJECUCIONES DISPONIBLES ####"));
		//				coreactive = true;
		//			}
		//			//throw new Exception("PROGRAMA NO REGISTRADO");

		//		}
		//	} catch (Exception ex) {
		//		//Util.AlertMessage("ERROR DE REGISTRO");
		//		Console.WriteLine (Tr.Catalog.GetString ("*******ERROR DE REGISTRO*********"));
		//		horizonte.LogEx.CatchEx (ex,"Validate","");
		//		throw new Exception(Tr.Catalog.GetString ("PROGRAMA NO REGISTRADO"));
		//		coreactive = false;
		//		//Application.Quit ();
		//	}



		//}

		#endregion
		//private static List<string> ModKeyList = new List<string>();
		//private static string OrderModKey (string ModKey)
		//{
		//	try {
		//		ModKeyList.Clear ();
		//		if(ModKey==string.Empty)
		//			return ModKey;
		//		if(ModKey.Length % 3 != 0)
		//			return string.Empty;
		//		int nmod = ModKey.Length / 3;
		//		int start =0;
		//		string key="";
		//		for (int modnumb = 0; modnumb < nmod; modnumb++) {
		//			start = modnumb * 3;
		//			key = ModKey.Substring (start,3) ;
		//			ModKeyList.Add (key);
		//		}
		//		ModKeyList.Sort ();
		//		string orderkeylist = string.Empty;
		//		foreach (string ordkey in ModKeyList) {
		//			orderkeylist += ordkey;
		//		}
		//		return orderkeylist;
		//	} catch (Exception ex) {
		//		LogEx.CatchEx (ex, "OrderModKey");
		//		return string.Empty;
		//	}
		//}
		//private static int CheckLombriz()
		//{
		//	try {

		//		string filelock = "horizonte.bmp";

		//		if (!File.Exists(filelock))
		//			return 0;
		//		DateTime lastwrite = File.GetLastWriteTime(filelock);
		//		DateTime creationtime = File.GetCreationTime(filelock);
		//		DateTime lastaccesstime =  File.GetLastAccessTime(filelock);
		//		StreamReader sreader = new StreamReader (filelock);
		//		BinaryReader b = new BinaryReader (sreader.BaseStream);
		//		byte[] bit = b.ReadBytes ((int)sreader.BaseStream.Length);


		//		byte dig1 = bit [300];
		//		byte dig2 = bit [301];

		//		int curintcount = 0;
		//		string curstrcount = (char)dig1 +"" + (char)dig2;
		//		int.TryParse (curstrcount,out curintcount);
		//		//
		//		//curintcount = 101;
		//		//
		//		int newintcount = curintcount - 1;
		//		if (newintcount <=0)
		//			return 0;
		//		string newstrcount = newintcount.ToString ();
		//		char[] newchar = newstrcount.ToCharArray ();
		//		if (newchar.Length == 1)
		//		{
		//			bit[300] = (byte)'0';
		//			bit[301] = (byte)newchar[0];
		//		}else{
		//			bit [300] = (byte)newchar[0]; 
		//			bit [301] = (byte)newchar[1];
		//		}
		//		sreader.Close ();
		//		File.WriteAllBytes (filelock, bit);

		//		File.SetLastWriteTime(filelock,lastwrite);
		//		File.SetCreationTime(filelock,creationtime);
		//		File.SetLastAccessTime(filelock,lastaccesstime);
		//		return newintcount;    


		//	} catch (Exception ex) {
		//		horizonte.LogEx.CatchEx (ex,"Lombriz","");
		//		return 0;
		//	}

		//}


		public static  void LoadConfig ()
		{
			LoadConfig("horizonte");
		}

		public static void InitEntorno()
		{
			try {
				//validate();
				coreactive = true;
				/// cargamos el ensamblado en la instancia actual
				currAssembly = Assembly.GetExecutingAssembly();
				if (coreactive)
				{	
					//cargamos la informacion de configuración de los módulos en la tabla de configuraciones
					TablaModulos = ConfigTool.GeneralDataSet.Tables["Modulos"];
					//cargamos los comandos de horizonte
					PanelModulo panelmudulo = new PanelModulo();
					AddCommandToList("ListCommands","horizonte_ListCommands",Tr.Catalog.GetString("Listar módulos en consola")
						,"horizonte_ListCommands",panelmudulo);
					AddCommandToList("ListModules","horizonte_ListModules",Tr.Catalog.GetString("Listar módulos en consola")
						,"horizonte_ListModules",panelmudulo);
					AddCommandToList("Quit","horizonte_Quit",Tr.Catalog.GetString("Cerrar aplicación")
						,"horizonte_Quit",panelmudulo);
					AddCommandToList("BackupEnviroment","horizonte_BackupEnviroment",Tr.Catalog.GetString("Copia de seguridad de la configuración y los módulos")
						,"horizonte_BackupEnviroment",panelmudulo);
					AddCommandToList("RestoreEnviroment","horizonte_RestoreEnviroment",Tr.Catalog.GetString("Cerrar aplicación")
						,"horizonte_RestoreEnviroment",panelmudulo);
					AddCommandToList("AskRegister","horizonte_Register",Tr.Catalog.GetString("Solicitar registro")
						,"horizonte_Register",panelmudulo);
					AddCommandToList("ShowWidget","horizonte_ShowWidget",Tr.Catalog.GetString("Mostra widget")
						,"horizonte_ShowWidget",panelmudulo);
					AddCommandToList("ShowWidgetArgs","horizonte_ShowWidgetArgs",Tr.Catalog.GetString("Mostra widget")
						,"horizonte_ShowWidgetArgs",panelmudulo);
					//Cargamos los módulos
					CargarModulos();
					DoInit();
					
				}
				
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"InitEntorno","");
			}
		}

		private static void SetEntorno()
		{
			try {
				DataTable Sets = ConfigTool.GeneralDataSet.Tables ["Enviroment"];
				foreach (DataRow set in Sets.Rows) {
					System.Environment.SetEnvironmentVariable (set [0].ToString(),
						ConfigTool.ParseConfigValue ( set [1].ToString()));
				}
				
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"SetEntorno","");
			}
			
			//System.Environment.SetEnvironmentVariable ("GTK2_RC_FILES", "/home/paco/Desktop/Proyecto_HORIZONT-E/KREEMECO/KREEMECO-LINUX/temas/Orange/gtkrc");
		}


		public static  void LoadConfig (string ConfigName)
		{
			try {
				ConfigFile = ConfigName;
				Disposing =  new EventHandler (delegate(object sender, EventArgs e)
					{});
				ModulesLoaded = new EventHandler(delegate(object sender, EventArgs e) 
					{
						//Console.WriteLine ("moduload");
					});
				//Establecemos el ConfigTool que vamos a usar.
				ConfigTool = new hConfigTool(ConfigName);
				ConfigTool.ReadConfig();
				SetEntorno();
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"LoadConfig","");
			}
		}



		public static void DoInit()
		{
			try {
				foreach (DataRow R in TablaModulos.Rows)
				{
					try {
						//comprobamos sí esta activo
						if (bool.Parse((string)R["Activado"])) 
						{
							RunCommand(R[0]+"_Init");
						}

					} catch (Exception ex) {
						horizonte.LogEx.CatchEx (ex,"DoInit","");
					}
				}
				ModulesLoaded(null,EventArgs.Empty);

			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"DoInit","");
			}

		}



		public static void AddCommandToList (string Metodo,string CommandKey,
	                           string CommandDescrip, string CommandName,object Modulo)
		{
			try {
				Type Panel =  Modulo.GetType();
				MethodInfo mi_Metodo = Panel.GetMethod(Metodo); 
				hCommand mi_cmd = new horizonte.hCommand();
				mi_cmd.CommandKey = CommandKey;
				/////// modo pruebas, siempre true
				mi_cmd.Activado = true; //////
				mi_cmd.CommandDescrip =CommandDescrip;
				mi_cmd.Metodo = Metodo;
				mi_cmd.CommandName = CommandName;
				mi_cmd.CommandAction = mi_Metodo;
				mi_cmd.Panel = Modulo;
				CommandList.Add(mi_cmd.CommandKey,mi_cmd);	
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"AddCommandToList: " + CommandKey,Modulo);

			}


		}

		public static void DescargarComandos (string ModName)
		{
			try {
				DataTable ModuloCommandTable;
				ModuloCommandTable = ConfigTool.GeneralDataSet.Tables[ModName];
				foreach (DataRow R in ModuloCommandTable.Rows) 
				{	
					try{
					CommandList.Remove((string)R["CommandKey"]);
					}
					catch (Exception ex) {
						Console.WriteLine(ex);
					}
				}
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"DescargarComandos","");
			}
		}

		public static void CargarModulos ()
		{
			CargarModulos("");
		}

		public static  void CargarModulos(string CargarModName)
		{
			try {
				
			
				//recorremos la tabla de modulos y
				bool Cargar ;
				
				foreach (DataRow _Mod in TablaModulos.Rows) {
					Cargar = true;
					//recuperamos los datos del módulo
					string RutaDll = ConfigTool.ParseConfigValue ((string) _Mod["RutaDLL"]);
					string ModuloDLL = (string)_Mod["ModuloDLL"];
					string ModuloPanel = (string)_Mod["ModuloPanel"];
					string ModuloNombre = (string)_Mod["Nombre"];
					bool Activado = bool.Parse((string)_Mod["Activado"]);
					// Comprobamos la key del módulo

					// si esta activo cargamos el ensamblado
					// y creamos los comandos
					if (Activado)
					{
						//comprobamos sí se ha pasado el nombre del módulo
						if (CargarModName != "") //queremos cargar solo un módulo
						{
							if (CargarModName != ModuloNombre)
							{
								Cargar =false;
							}
							
						}
						if (Cargar)
						{
							try {
								string _r0 = System.IO.Path.Combine(ConfigTool.BasePath().ToString(),RutaDll,ModuloDLL);
								//								/// creamos el ensamblado para el módulo
								//								Assembly miEnsamblado;
								//								/// cargamos el ensamblado en la instancia actual
								//								miEnsamblado = Assembly.GetExecutingAssembly();
								/// cargamos el módulo en el ensanblado
								//currAssembly = Assembly.LoadFile(_r0);
								currAssembly = Assembly.LoadFrom(_r0);
								//currAssembly = Assembly.Load(_r0);

								///creamos un Tipo para la clase que actua como panel de control
								Type Panel = currAssembly.GetType(ModuloPanel);
								///creamos un objeto basado en el tipo recuperado
								iModuloHorizonte _ModInstance =(iModuloHorizonte)Activator.CreateInstance(Panel);
								// Comprobamos la key del módulo
//								if (_ModInstance.ModId() != string.Empty)
//								{
//									if (!ModKeyList.Contains ( _ModInstance.ModId() ))
//									{
//										Console.WriteLine 
//										("** KEY " + _ModInstance.ModId () +
//										" no registrda *******");
//										continue;
//									}
//								}
								//Añadimos a la lista de comandos los comandos  del
								//módulo que están en la configuración.
								//cargamos la sección de configuración del módulo en una tabla
								DataTable ModuloCommandTable;
								ModuloCommandTable = ConfigTool.GeneralDataSet.Tables[ModuloNombre];
								foreach (DataRow R in ModuloCommandTable.Rows) 
								{
									AddCommandToList((string)R["Metodo"],(string)R["CommandKey"],(string)R["CommandDescrip"],(string)R["CommandName"],_ModInstance);
								}
							} catch (Exception ex) {
								horizonte.LogEx.CatchEx (ex,"CargarComandos","");
							}
						}
					}
				}
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"CargarComandos","");
			}
		}
		public static object RunCommand (string CommandKey)
		{
		
			return (object)RunCommand(CommandKey,null);
		}

		public static object RunCommand (string CommandKeyor, object[] Arg)
		{
			string CommandKey = string.Empty;
			try {

				// codificación rutas url.... hay que cambiar la codificación.
				//CommandKey = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(CommandKeyor.Trim()));
				//Console.WriteLine(CommandKeyor);
				//Console.WriteLine(CommandKey);
				//Console.WriteLine(CommandKeyor == CommandKey);
				CommandKey = CommandKeyor;
//				foreach (char c in CommandKey )
//				{
//					//if (c > 127)
//						//Console.WriteLine("Caracter no imprimible " + CommandKey);
//					
//				}

				if (!CommandList.ContainsKey(CommandKey)){
					Console.WriteLine("no existe el comando: " + CommandKey);
				}

				// recuperamos del diccionario el comando correspondiente al CommandKey
				hCommand RCommand = (hCommand)CommandList[CommandKey];
				
				// creamos el objeto de respuesta
				object ResObject = new object();
				
				// invocamos el método de la clase Panel con los argumentos
				ResObject = (object)RCommand.CommandAction.Invoke(RCommand.Panel,Arg);
				
				//devolvemos el objeto
				return ResObject;
				
			} catch (Exception ex) {
				//LOG
				//object[]  _SetUpParam = new object[]{this,Catalog.GetString(string.Format("Error al procesar RunComman {0} ",CommandKey)),ex.ToString(),1};
				//RunCommand("Log_Trace",_SetUpParam);
				Console.WriteLine ("*RunCommand* "+CommandKey+" ***");
				LogEx.CatchEx (ex,"RunCommand",null);
				return new object();

			}



		}
		public static string TestCommand (string Command, string Arg)
		{
			return TestCommandWn ("",0,0, Command, Arg);
		}

		public static string TestCommandWn (string winname,int Width,int Heigth, string Command, string Arg)
		{
			object ResCommand;
			string txtRes = "";
			// ejecutamos el comando con o sín parámetros
			if( Arg == "")
			{
				try {
					ResCommand = RunCommand(Command);
				} catch (Exception ex) {
					Console.WriteLine(ex);
					return "";
				}

			}
			else
			{

				string[] _args = new string[Arg.Split (';').Length];
				_args = Arg.Split (';');
				//this.textview1.Buffer.Text =(string) hGestorModulo.RunCommand (_Comm, args);	
				//object[] Args = new string[1];
				//Args[0] = Arg;
				try {
					ResCommand = RunCommand(Command,_args);
				} catch (Exception ex) {
					//horizonte.LogEx.CatchEx (ex,"TestCommand","");
					return "";
				}

			}
			// intentamos mostrar el resultado
			// puede ser que el método sea void
			String _resType;
			try {
				
				_resType = ResCommand.GetType().ToString();	
			} catch (Exception ex) {
				//horizonte.LogEx.CatchEx (ex,"TestCommand","");
				_resType = "";

			}

			switch (_resType) 
			{
				case "": 
				case "System.void":
				txtRes =Tr.Catalog.GetString ("Cadena vacia");
				break;
				case "System.String":
				case "System.Boolean": 
				case "System.Int16":
				case "System.Int32": 
				case "System.Int64":
					txtRes = ResCommand.ToString();
					break;
				default: // probamos mostrarlo en una ventana
					try {
					//comprobamos el tamaño de la venta
					if (Width == 0 || Heigth == 0)
					{
						Width = (ResCommand as Widget).SizeRequest ().Width;
						Heigth = (ResCommand as Widget).SizeRequest ().Height;
					}
					Gtk.Window _Wres = new horizonte.WidgetWindow((Widget)ResCommand,winname);
					_Wres.SetSizeRequest (Width,Heigth);
					_Wres.SetPosition (WindowPosition.Center);

					(ResCommand as Widget).Show();
					_Wres.ShowAll();
					txtRes = "Gtk.Widget";
								
					} catch (Exception ex) {
						//horizonte.LogEx.CatchEx (ex,"TestCommand","");
					}
				break;
			}


			return txtRes;

		}



	}
}

