using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace JotDown
{
	public class App : Application
	{
	    public static MobileServiceUser authenticated = null;

        public App ()
		{
            // The root page of your application
			MainPage = new NavigationPage( new Main() );
		}

		protected override void OnStart ()
		{
            // Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

	    public static IAuthenticate Authenticator { get; private set; }

	    public static void Init( IAuthenticate authenticator )
	    {
	        Authenticator = authenticator;
	    }
    }
}

