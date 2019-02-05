/// <summary>
/// File: hNotify.cs
/// Author: Francisco Fuster e-mail: franfusto@gmail.com
/// Copyright 2016, Francisco Fuster, All rights reserved.
/// </summary>
using System;
using System.Reflection;
using System.Collections.Generic;
namespace horizonte
{
	public static class hNotify
	{
		public static Int32 LastDelegateId = 0;

		public static List<string> ChannelList = new List<string>();
		public static  Dictionary<int,hDelegateInfo> DelegatePool = new Dictionary<int, hDelegateInfo>();


		public static void RegisterDelegate (string RegisterChannel,object Sender, String DelegateMethod)
		{
			try {
		
				int Id = LastDelegateId + 1;
				LastDelegateId ++;
				Type Panel =  Sender.GetType();
				MethodInfo Delegate = Panel.GetMethod(DelegateMethod); 
				
				hDelegateInfo _dinf = new hDelegateInfo();
				_dinf.Channel = RegisterChannel;
				_dinf.Delegate = Delegate;
				_dinf.Sender = Sender;
				
				DelegatePool.Add(Id,_dinf);		
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"RegisterDelegate","");
			}
		

		}
		public static bool SendNotify (hNotifyElement NotifyElement)
		{
			try {
				string SendTo = NotifyElement.hNotifyChannel;
				
				foreach (hDelegateInfo DelegateInfo in DelegatePool.Values) {
					if (DelegateInfo.Channel == SendTo)
					{
						DelegateInfo.Delegate.Invoke(DelegateInfo.Sender,new object[]{NotifyElement});
					}
				}
				return true;
			} catch (Exception ex) {
				horizonte.LogEx.CatchEx (ex,"SendNotify","");
				return false;
			}

		}

	}

	public class hDelegateInfo
	{
		private string _Channel;
		private MethodInfo _Delegate;
		private object _Sender;
		public hDelegateInfo ()
		{
		}
		public string Channel {
			get{return _Channel;}
			set{_Channel =value;}
		}
		public MethodInfo Delegate {
			get{return _Delegate;}
			set{_Delegate =value;}
		}
		public Object Sender {
			get{return _Sender;}
			set{_Sender =value;}
		}

	}

	public class hNotifyElement
	{
		private string Channel;
		private string Message;
		private string Value;


		public hNotifyElement()
		{

		}

		public string hNotifyChannel {
			get{return Channel;}
			set{Channel = value;}
		}
		public string hNotifyMessage {
			get{return Message;}
			set{Message = value;}
		}
		public string hNotifyValue {
			get{return Value;}
			set{Value = value;}
		}

	
	}
}

