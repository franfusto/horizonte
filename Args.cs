/// <summary>
/// File: Args.cs
/// Author: Francisco Fuster e-mail: franfusto@gmail.com
/// Copyright 2016, Francisco Fuster, All rights reserved.
/// </summary>
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace horizonte
{
	public static class Args
	{
		public static bool Cont = true;
		public static bool ProcArgs(string[] args)
		{

			try {
				
			// Pasamos los argumentos a una diccionario
			Regex Spliter = new Regex (@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			//Regex Remover = new Regex (@"^['""]?(.*?)['""]?$",RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Dictionary<string,string> ArgsList = new Dictionary<string,string> ();
			string[] Partes;
			foreach (string arg in args) {
				if (!SpecialArg(arg))
				{
					Partes = Spliter.Split (arg, 2);
					if (Partes.Length == 1) {
						ArgsList.Add (Partes [0], "");
					} else {
						ArgsList.Add (Partes [0], Partes [1]);
					}
				}

			}
			// recorremos el diccionario de Argumentos
			string ArgKey;
			string ArgValue;
			foreach (string _key in ArgsList.Keys) {
				ArgValue = ArgsList [_key];

				//Comprobamos sí el Key se encuentra también en la lista de comandos
				//y ejecutamos esl método asociado

				if (hGestorModulo.CommandList.ContainsKey (_key)) {
						if (ArgValue == "") {
							hGestorModulo.TestCommand (_key,"");
						} else {
							hGestorModulo.TestCommand (_key, ArgValue);
						}
				}
				// procesamos otros argumentos

			}
			return Cont;
			} catch (Exception ex) {
				LogEx.CatchEx (ex,"ProcArgs");				
				return Cont;
			}
		}
		private static bool SpecialArg(string arg)
		{
			try {
				if (arg=="--help")
				{
					Help ();
					return true;
				}
				if (arg=="--OnlyArgs")
				{
					Cont = false;
					return true;
				}
				if (arg=="--noGtk")
				{
					horizonte.hGestorModulo.GtkEnabled = false;
					return true;
				}

				return false;
			} catch (Exception ex) {
				return false;
				LogEx.CatchEx (ex,"SpecialArg");
			}
		}

		private static void Help()
		{
			Console.WriteLine (Tr.Catalog.GetString (@"
Horizonte, Tool for modular development with C#
(C) Francisco Fuster - franfusto@gmail.com
All rights reserved.

Usage:
	application.exe [options] [Comand]=[arg1;arg2;..argN] ... [Comand]=[arg1;arg2;..argN]

Options:
	--help		Show this help info.
	--noGtk		Disable GTK use in horizonte core.
	--OnlyArgs	Execute only Commands passed in arguments command line.

Useful Commands:
	hgesmod_MainForm				: Allow load modules
	hconfig_MainForm				: Mangage configuration entries.
	hconfig_ComDisp					: List of available commands
	hconfig_ModLoad					: List of loaded modules
	hconfig_RepoStore				: List of modules in repository
	hconfig_ConfigGeneralEditor		: General configuration editor
	hconfig_ConfigMachineEditor		: General configuration editor
	hconfig_ConfigUserEditor		: General configuration editor
	hconfig_DataMod					: Xml configuration editor

			"));
		}

	}
}

