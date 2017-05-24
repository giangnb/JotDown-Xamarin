using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JotDown.Services;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace JotDown
{
	public static class Constants
	{
        // Replace strings with your Azure Mobile App endpoint.  
        public static string ApplicationURL = @"https://jotdown.azurewebsites.net";

	    //public static TodoItemManager TodoManager = new TodoItemManager();

        public static AppPreferenceManager AppPreference = new AppPreferenceManager();

	    public static readonly string OfflineDbPath = @"localstore.db";

        // Define a authenticated user.
        public static MobileServiceUser User = null;

        // Properties & Shared Prreferences
        public static void InitialiseProperties()
        {
            //AppPreference = new AppPreferenceManager();
            AppPreference.Add( "LoggedIn", false );
            AppPreference.Add( "UserId", "" );
            AppPreference.Add( "UserName", "" );
        }

        public static void SetProperty(string name, object value)
	    {
            AppPreference.Add( name, value );
        }

	    public static T GetProperty<T>( string name )
	    {
            var o = AppPreference.Fetch( name );
            if (o != null)
            {
                return (T) o;
            }
            return default(T);
	    }
    }
}

