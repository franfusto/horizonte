/// <summary>
/// File: IModuloHorizonte.cs
/// Author: Francisco Fuster e-mail: franfusto@gmail.com
/// Copyright 2016, Francisco Fuster, All rights reserved.
/// </summary>
using System;
using Gtk;
using Gdk;


namespace horizonte
{
	/// <summary>
	/// Interface that all the moudules must implement.
	/// </summary>
	public interface iModuloHorizonte
	{
		string ModId();
		bool Init ();
		bool SetUp (string SetUpParam);
		bool Delete ( string DeleteParam);
		Widget ConfigForm ();
		Widget MainForm ();
	}
}

