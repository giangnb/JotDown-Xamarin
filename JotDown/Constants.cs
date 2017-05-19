using System;
using System.Linq;
using Xamarin.Forms;

namespace JotDown
{
	public static class Constants
	{
        // Replace strings with your Azure Mobile App endpoint.  
        public static string ApplicationURL = @"https://jotdown.azurewebsites.net";

	    public static TodoItem CurrentTodo;

	    public static readonly TodoItemManager TodoManager = TodoItemManager.DefaultManager;

	    public static void InitialiseProperties()
	    {
	        if (!Application.Current.Properties.ContainsKey("LoggedIn"))
	        {
	            Application.Current.Properties["LoggedIn"] = false;
	            Application.Current.Properties["UserId"] = "";
	            Application.Current.Properties["UserName"] = "";
            }

	        if (!Application.Current.Properties.ContainsKey( "FirstStart" ))
	        {
	            Application.Current.Properties["FirstStart"] = true;
	        }

	        if (!Application.Current.Properties.ContainsKey( "Experimental" ))
	        {
	            Application.Current.Properties["Experimental"] = false;
	        }
        }
	}
}

