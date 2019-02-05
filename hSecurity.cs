/// <summary>
/// File: hSecurity.cs
/// Author: Francisco Fuster e-mail: franfusto@gmail.com
/// Copyright 2016, Francisco Fuster, All rights reserved.
/// </summary>
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;



namespace horizonte
{



	public static class hSecurity
	{
		private static string _InstanceKey = string.Empty;
		private static string _AuthUser = string.Empty;
		private static string _AuthGroups = string.Empty;
		private static bool _Logged = false;
		private static List<string> _IPlist = new List<string>();
		private static List<string> _GroupLIst = new List<string>();
		private static bool _Online = false;

		public static void Login (string User, string Groups)
		{
			try {
				if (!_Logged) {
					_AuthUser = User;
					_AuthGroups = Groups;
					string[] Glist;
					Glist = Groups.Split(new char[] {' ','\t'}, StringSplitOptions.RemoveEmptyEntries);
					_GroupLIst.AddRange	(Glist);
					_Logged = true;
				}
			} catch (Exception ex) {
				_AuthGroups = string.Empty;
				_AuthUser = string.Empty;
				_Logged =false;
				horizonte.LogEx.CatchEx (ex,"Login","");
			}


		}
		public static string AuthUser 
		{
			get { return _AuthUser;}

		}
		public static string AutGroup 
		{
			get { return _AuthGroups;}
		}
		public static string InstanceKey {
			get {
				if (_InstanceKey == string.Empty) {
					_InstanceKey = System.Guid.NewGuid ().ToString("B");
				}
				return _InstanceKey;
			}
		}

		public static bool UserInGroup (string Grupo)
		{
			for (int i = 0; i < _GroupLIst.Count; i++) {
				if (_GroupLIst[i].ToString() == Grupo){
					return true;	
				}
			}
			return false;
		}





	}
}

