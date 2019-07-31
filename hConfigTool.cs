/// <summary>
/// File: hConfigTool.cs
/// Author: Francisco Fuster e-mail: franfusto@gmail.com
/// Copyright 2016, Francisco Fuster, All rights reserved.
/// </summary>
using System;
using System.Data;
using Gtk;
using System.Text.RegularExpressions;
using System.IO;
using NGettext;

namespace horizonte
{
	public static class Tr
	{
		public static  ICatalog Catalog = new Catalog("horizonte", "./locale");
	}
	public class hConfigTool
	{
		public DataSet GeneralDataSet;//= new DataSet();
		public DataTable GeneralConfigTable ;//= new DataTable();
		public DataSet MachineDataSet;//= new DataSet();
		public DataTable MachineConfigTable ;//= new DataTable();
		public DataSet UserDataSet;//= new DataSet();
		public DataTable UserConfigTable ;//= new DataTable();


		public bool OnlyGeneral = false;
		public string GeneralConfigFile;
		public string MachineConfigFile;
		public string UserConfigFile;

		public bool IsUnixSystem;


		public hConfigTool (string ConfigName, bool onlygeneral)
		{
			init (ConfigName,null,onlygeneral);
		}

		public hConfigTool(FileInfo File)
		{
			init (null,File,true);
		}

		public hConfigTool (string ConfigName)
		{
			init (ConfigName,null,false);
		}
		private void GetOsVersion()
		{
			try {
				switch (System.Environment.OSVersion.Platform) {
				case PlatformID.MacOSX:
				case PlatformID.Unix:
					IsUnixSystem = true;
					break;
				default:
					IsUnixSystem = false;
				break;
				}

			} catch (Exception ex) {
				LogEx.CatchEx (ex, "GetOsVersion");
			}
		}

		private void init(string ConfigName,FileInfo File,  bool onlygeneral)
		{
			try {
				GetOsVersion ()	;
				OnlyGeneral = onlygeneral;

				GeneralDataSet = new System.Data.DataSet();
				GeneralConfigTable = new DataTable();
				MachineDataSet = new DataSet();
				MachineConfigTable = new DataTable();
				UserDataSet = new DataSet();
				UserConfigTable = new DataTable();
				if (ConfigName != null)
				{
					if (!ConfigName.Contains (".xml"))
					{
						ConfigName += ".xml";
					}
					File = new FileInfo(ConfigName);
				}
				if (File != null)
				{
					string cdir = File.DirectoryName;
					string cname =  File.Name.ToString ();
					//Console.WriteLine (cdir);
					//Console.WriteLine (cname);
					GeneralConfigFile =
						Path.Combine (cdir, cname);
					if (!onlygeneral)
					{
						MachineConfigFile =
							Path.Combine (cdir, System.Environment.MachineName+"."+cname);
						UserConfigFile = Path.Combine (cdir,System.Environment.UserName +"." +
							System.Environment.MachineName + "."+cname);
					}
				}


			} 
			catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"Init",this);
			}

		}

		public string BasePath ()
		{
			if (_Path == string.Empty) {
				return System.IO.Path.GetDirectoryName( Environment.GetCommandLineArgs()[0]);
			} else {
				return  _Path;
			}
			
			return _Path;
		}
		string _Path = string.Empty;
		public string InitDir {
			get
			{
				return _Path;
			}
			set
			{
				_Path = value;
			}
		}
		public bool ReadConfig()
		{
			try {
				//general 
				if (!File.Exists (GeneralConfigFile))
				{
					CreateFile (GeneralConfigFile);
				}
					GeneralDataSet.Clear();
					GeneralDataSet.ReadXml(GeneralConfigFile);
					GeneralConfigTable = GeneralDataSet.Tables["General"];
					DataColumn[] DcKey = new DataColumn[1];
					if (GeneralConfigTable != null)
					{
						DcKey[0] = GeneralConfigTable.Columns[0];
						GeneralConfigTable.PrimaryKey = DcKey;
						GeneralConfigTable.DefaultView.Sort = DcKey[0].ColumnName;
					}
				if (!OnlyGeneral)
				{
					
					// machine
					if (!File.Exists (MachineConfigFile))
					{
						CreateMachineConfigFile ();
					}
						MachineDataSet.Clear();
						MachineDataSet.ReadXml(MachineConfigFile);
						MachineConfigTable = MachineDataSet.Tables["General"];
						DataColumn[] DcKeyma = new DataColumn[1];
						DcKeyma[0] = MachineConfigTable.Columns[0];
						MachineConfigTable.PrimaryKey = DcKeyma;
						MachineConfigTable.DefaultView.Sort = DcKeyma[0].ColumnName;
					
					//user
					if (!File.Exists (UserConfigFile))
					{
						CreateUserConfigFile ();
					}
						UserDataSet.Clear();
						UserDataSet.ReadXml(UserConfigFile);
						UserConfigTable = UserDataSet.Tables["General"];
						DataColumn[] DcKeyus = new DataColumn[1];
						DcKeyus[0] =UserConfigTable.Columns[0];
						UserConfigTable.PrimaryKey = DcKeyus;
						UserConfigTable.DefaultView.Sort = DcKeyus[0].ColumnName;
				}
				return true;
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"ReadConfig",this);
				return false;
			}

		}
		public bool SaveConfig()
		{
			try {
				//general
				GeneralDataSet.WriteXml(GeneralConfigFile);
				if (!OnlyGeneral)
				{
					//machine
					MachineDataSet.WriteXml (MachineConfigFile);
					//user
					UserDataSet.WriteXml (UserConfigFile);
				}
				horizonte.hNotify.SendNotify(new hNotifyElement(){
					hNotifyChannel="horizonte",
					hNotifyMessage = "ConfigSaved",
					hNotifyValue = string.Empty
				});
				return true;	
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"SaveConfig",this);
				return false;
			}

		}
		public string Encrypt (string Valor)
		{
			try {
				if (Valor.Trim().StartsWith("==")){
					return Crypto.EncryptString (Valor.Remove(0,2));
				}else{
					return Valor;
				}
			} catch (Exception ex) {
				Console.WriteLine (ex);
				return string.Empty;
			}
		}
		public string Decrypt (string Valor)
		{
			try {
				if (Valor.Trim().EndsWith("==")){
					return Crypto.DecryptString (Valor);
				}else{
					return Valor;
				}
			} catch (Exception ex) {
				Console.WriteLine (ex);
				return string.Empty;
			}

		}
		public bool AddValue (string Key, string Valor,string Grupo)
		{
			try {

				GeneralConfigTable.Rows.Add(Key,Encrypt(Valor),Grupo);
				SaveConfig();
				return true;	
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"AddValue",this);
				return false;
			}

		}
		public bool AddMachineValue (string Key, string Valor,string Grupo)
		{
			try {
				MachineConfigTable.Rows.Add(Key,Encrypt(Valor),Grupo);
				SaveConfig();
				return true;	
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"AddMachineValue",this);
				return false;
			}

		}
		public bool AddUserValue (string Key, string Valor,string Grupo)
		{
			try {
				UserConfigTable.Rows.Add(Key,Encrypt(Valor),Grupo);
				SaveConfig();
				return true;	
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"AddUserValue",this);
				return false;
			}

		}
		public bool DeleteValue (string Key)
		{
			try {
				MessageDialog AlertMes = new MessageDialog (null, DialogFlags.Modal, MessageType.Warning, ButtonsType.YesNo,Tr.Catalog.GetString ("Va a eliminar un registro"));
				ResponseType ResAlertMes = (ResponseType)AlertMes.Run ();
				if (ResAlertMes == ResponseType.No) {
					AlertMes.Destroy ();
					return false;
				} else {
					int intRow = GeneralConfigTable.DefaultView.Find (Key);
					GeneralConfigTable.DefaultView [intRow].Delete ();
				}
				AlertMes.Destroy ();
				return true;	
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"DeleteValue",this);
				return false;
			}

		}
		public bool DeleteMachineValue (string Key)
		{
			try {
				MessageDialog AlertMes = new MessageDialog (null, DialogFlags.Modal, MessageType.Warning, ButtonsType.YesNo, Tr.Catalog.GetString ("Va a eliminar un registro"));
				ResponseType ResAlertMes = (ResponseType)AlertMes.Run ();
				if (ResAlertMes == ResponseType.No) {
					AlertMes.Destroy ();
					return false;
				} else {
					int intRow = MachineConfigTable.DefaultView.Find (Key);
					MachineConfigTable.DefaultView [intRow].Delete ();
				}
				AlertMes.Destroy ();
				return true;	
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"DeleteMachineValue",this);
				return false;
			}

		}
		public bool DeleteUserValue (string Key)
		{
			try {
				MessageDialog AlertMes = new MessageDialog (null, DialogFlags.Modal, MessageType.Warning, ButtonsType.YesNo, Tr.Catalog.GetString ("Va a eliminar un registro"));
				ResponseType ResAlertMes = (ResponseType)AlertMes.Run ();
				if (ResAlertMes == ResponseType.No) {
					AlertMes.Destroy ();
					return false;
				} else {
					int intRow = UserConfigTable.DefaultView.Find (Key);
					UserConfigTable.DefaultView [intRow].Delete ();
				}
				AlertMes.Destroy ();
				return true;	
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"DeleteUserValue",this);
				return false;
			}

		}
		public bool SetValue (string Key, string Valor,string Grupo)
		{
			try {
				int intRow = GeneralConfigTable.DefaultView.Find(Key);
				if (intRow == -1)
				{
					AddValue(Key,Valor,Grupo);
					SaveConfig();
					return true;
				}
				else
				{
					GeneralConfigTable.DefaultView[intRow][1] = Encrypt(Valor);
					GeneralConfigTable.DefaultView[intRow][2] = Grupo;
					return true;
				}
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"SetValue",this);
				return false;
			}

			
		}
		public bool SetMachineValue (string Key, string Valor,string Grupo)
		{
			try {
				int intRow = MachineConfigTable.DefaultView.Find(Key);
				if (intRow == -1)
				{
					AddMachineValue(Key,Valor,Grupo);
					SaveConfig();
					return true;
				}
				else
				{
					MachineConfigTable.DefaultView[intRow][1] = Encrypt(Valor);
					MachineConfigTable.DefaultView[intRow][2] = Grupo;
					return true;
				}
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"SetMachineValue",this);
				return false;
			}


		}
		public bool SetUserValue (string Key, string Valor,string Grupo)
		{
			try {
				int intRow = UserConfigTable.DefaultView.Find(Key);
				if (intRow == -1)
				{
					AddUserValue(Key,Valor,Grupo);
					SaveConfig();
					return true;
				}
				else
				{
					UserConfigTable.DefaultView[intRow][1] = Encrypt(Valor);
					UserConfigTable.DefaultView[intRow][2] = Grupo;
					return true;
				}
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"SetUserValue",this);
				return false;
			}


		}
		public String GetValor (string Key)
		{
			try {
				string Valor;
				string Grupo;
				GetValor(Key,out Valor, out Grupo);
				return Valor;	
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"GetValor",this);
				return "";
			}

		}
		public bool GetValor (string Key,out string Valor,out string Grupo)
		{
			try {
				// comprobamos si existe en UserConfig
				int intUserRow = UserConfigTable.DefaultView.Find(Key);
				if (intUserRow != -1)
				{
					Grupo =UserConfigTable.DefaultView[intUserRow][2].ToString();
					Valor =Decrypt(ParseConfigValue (UserConfigTable.DefaultView[intUserRow][1].ToString()));
					return true;
						
				}

				// comprobamos si existe en MachineConfig
				int intMachineRow = MachineConfigTable.DefaultView.Find(Key);
				if (intMachineRow != -1)
				{
					Grupo =MachineConfigTable.DefaultView[intMachineRow][2].ToString();
					Valor =Decrypt(ParseConfigValue (MachineConfigTable.DefaultView[intMachineRow][1].ToString()));

					return true;
				}
				// comprobamos si existe en GeneralConfig
				int intGeneralRow = GeneralConfigTable.DefaultView.Find(Key);
				if (intGeneralRow != -1)
				{
					Grupo =GeneralConfigTable.DefaultView[intGeneralRow][2].ToString();
					Valor =Decrypt(ParseConfigValue (GeneralConfigTable.DefaultView[intGeneralRow][1].ToString()));
					return true;

				// no existe, añadimos en GeneralConfig
				}
				else
				{
						AddValue(Key,"","");
						Valor = "";
						Grupo = "";
						return false;
				}	
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"GetValor",this);
				Valor = "";
				Grupo = "";
				return false;
			}


		}
		public string ParseConfigValue(string valor)
		{
			try {
				if (valor.StartsWith("'")) 
				{
					valor = valor.Substring(1, valor.Length - 1);
					return valor;
				}
				if (valor.Contains (@":\"))
				{
					return valor;
				}

				if (IsUnixSystem)
				{
					valor = valor.Replace (@"\",@"/");
				}else{
					if(!(valor.Contains("http") || valor.Contains("https") || valor.Contains("ftp:")))
					{
						valor = valor.Replace (@"/",@"\");
					}
				}

				if (valor.Contains ("$BasePath$"))
					valor = valor.Replace ("$BasePath$", horizonte.hGestorModulo.ConfigTool.BasePath ());
				if (valor.Contains ("$AuthUser$"))
					valor = valor.Replace ("$AuthUser$",horizonte.hSecurity.AuthUser);

				Match m = Regex.Match(valor, @"\(([^)]*)\)");
				while( m.Success)
				{
					valor = ParseAux (valor,m)	;
					m = m.NextMatch ();
				}
				return valor;

			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"ParseConfigValue",this);
				return valor;
			}
		}
		private string ParseAux(string text,Match m)
		{
			try {
				string key = m.Value.Substring (1,m.Value.Length -2);
				string configval = hGestorModulo.ConfigTool.GetValor (key);
				return text.Replace (m.Value,configval);
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"ParseAux",this);
				return text;
			}
		}

		private void CreateMachineConfigFile()
		{
			try {
				string Dump ;
				Dump = @"
<Configuracion>
  	<General>
		<Clave>MachineConfig</Clave>
		<Valor>TestValue</Valor>
		<Grupo></Grupo>
	</General>
 </Configuracion>
";
				System.IO.File.WriteAllText (MachineConfigFile,Dump);
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"CreateMachineConfig",this);
			}
		}
		private void CreateUserConfigFile()
		{
			try {
				string Dump ;
				Dump = @"
<Configuracion>
  	<General>
		<Clave>UserConfig</Clave>
		<Valor>TestValue</Valor>
		<Grupo></Grupo>
	</General>
 </Configuracion>
";
				System.IO.File.WriteAllText (UserConfigFile,Dump);
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"CreateUserConfigFile",this);
			}
		}

		public void CreateFile (string XMLfile)
		{
			try {
				//GeneralConfigFile = XMLfile;
				string Dump ;
				Dump = @"
<Configuracion>
  	<General>
		<Clave>RutaModulos</Clave>
		<Valor></Valor>
		<Grupo></Grupo>
	</General>
 	<Modulos>
		<Nombre></Nombre>
		<Version></Version>
		<RutaDLL></RutaDLL>
		<ModuloDLL></ModuloDLL>
		<ModuloPanel></ModuloPanel>
		<sKey></sKey>    
		<Activado></Activado>
	</Modulos>
	<NuevoModulo>
		<CommandKey></CommandKey>
		<CommandName></CommandName>
		<CommandDescrip></CommandDescrip>
		<Metodo></Metodo>
		<CommandType></CommandType>
		<Activado>true</Activado>
	</NuevoModulo>
 </Configuracion>
";
				System.IO.File.WriteAllText (XMLfile,Dump);
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"CreateFile",this);
			}



		}
	}
}

