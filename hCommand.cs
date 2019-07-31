/// <summary>
/// File: hCommand.cs
/// Author: Francisco Fuster e-mail: franfusto@gmail.com
/// Copyright 2016, Francisco Fuster, All rights reserved.
/// </summary>
using System;
using Gdk;

using System.Reflection;

namespace horizonte
{

	public class hCommand 
	{

		public hCommand()
		{


		}
		private CommandTypeList _CommandType;
		/// <summary>
		/// Tipo de dato devuelto por el método
		/// </summary>
		public CommandTypeList CommandType
		{
			get{ return _CommandType;}
			set{ _CommandType = value;}
		}
		private string _CommandKey;
		/// <summary>
		/// Clave del comando, por la cual podremos localizar el comando en el diccionario.
		/// Tambíen podemos pasar al ejecutable principal argumentos a este comando y ejecutarlo
		/// desde la línea de commandos usando el CommandKey como argumento.
		/// </summary>
		/// <value>
		/// The command key.
		/// </value>
		public string CommandKey
		{
			get{ return _CommandKey;}
			set{ _CommandKey = value;}
		}

		private string _CommandName;
		/// <summary>
		/// Nombre del comando que se mostrará al usuario
		/// </summary>
		/// <value>
		/// The name of the command.
		/// </value>
		public string CommandName {
			get{ return _CommandName;}
			set{ _CommandName = value;}
		}
		private string _CommandDescrip;
		/// <summary>
		/// Descripción del comando para el usuario
		/// </summary>
		/// <value>
		/// The command descrip.
		/// </value>
		public string CommandDescrip {
			get{ return _CommandDescrip;}
			set{ _CommandDescrip = value;}
		}
		private string _Metodo;
		/// <summary>
		/// Nombre de método contenido en la clase Panel
		/// </summary>
		/// <value>
		/// The metodo.
		/// </value>
		public string Metodo {
			get{ return _Metodo;}
			set{ _Metodo = value;}
		}
	
		private MethodInfo _CommandAction;
		/// <summary>
		/// Información del métdo correspondiente al Método
		/// </summary>
		/// <value>
		/// The command action.
		/// </value>
		public MethodInfo CommandAction {
			get{ return _CommandAction;}
			set{ _CommandAction = value;}
		}
		/// <summary>
		/// Representación de la Clase principal del módulo, 
		/// que implementea iModuloHorizonte
		/// </summary>
		private object _Panel;
		public object Panel{
			get{ return _Panel;}
			set{ _Panel = value;
			
			}
		}
	
		private bool _Activado;
	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="horizonte.hCommand"/> is activado.
	/// </summary>
	/// <value>
	/// <c>true</c> if activado; otherwise, <c>false</c>.
	/// </value>
		public bool Activado {
			get{ return _Activado;}
			set{ _Activado = value;}
		}
	}

	public enum CommandTypeList
	{

		noreturn,
		System_Int64,
		System_String,
		Gtk_Widget,

	}


}