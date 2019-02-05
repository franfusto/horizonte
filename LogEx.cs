using System;

namespace horizonte
{
	public static class LogEx
	{
		public static void CatchEx(Exception Ex)
		{
			CatchEx (Ex,"");
		}

		public static void CatchEx(Exception Ex,string Ref)
		{
			CatchEx (Ex,Ref,null);
		}

		public static void CatchEx(Exception Ex,string Refer,object Sender)
		{
			try {
				if (horizonte.hGestorModulo.GtkEnabled)
					WaitWin.SetText ("Se ha producido un errro. Revise los datos y el registro",1);
				Console.WriteLine ("** " + Refer + " **"  );
				Console.WriteLine (Ex);
			} catch (Exception ex) {
				Console.WriteLine (ex);				
			}

		}
	}
}

