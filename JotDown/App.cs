using System;
using System.IO;
using System.Reflection;
using JotDown.Services;
using Xamarin.Forms;

namespace JotDown
{
	public class App : Application
	{
		public App ()
		{
		    Constants.InitialiseProperties();
			// The root page of your application
			MainPage = new NavigationPage( new NoteList() );
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


        //Authenticate
	    public static IAuthenticate Authenticator { get; private set; }

	    public static void Init( IAuthenticate authenticator )
	    {
	        Authenticator = authenticator;
	    }
    }
}

