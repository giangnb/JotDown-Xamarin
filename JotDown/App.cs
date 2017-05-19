using System;

using Xamarin.Forms;

namespace JotDown
{
	public class App : Application
	{
		public App ()
		{
		    Constants.InitialiseProperties();
			// The root page of your application
			MainPage = new Page1();
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
	}
}

