using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using JotDown.Services;
using Microsoft.WindowsAzure.MobileServices;
using UIKit;

namespace JotDown.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, 
        IAuthenticate
    {
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// Initialize Azure Mobile Apps
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			// Initialize Xamarin Forms
			global::Xamarin.Forms.Forms.Init ();

		    App.Init( this );

            LoadApplication (new App ());

            return base.FinishedLaunching (app, options);
		}

        public async Task<bool> Authenticate(MobileServiceAuthenticationProvider provider)
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                if (Constants.User == null)
                {
                    Constants.User = await TodoItemManager.DefaultManager.CurrentClient
                        .LoginAsync( UIApplication.SharedApplication.KeyWindow.RootViewController,
                            provider );
                    if (Constants.User != null)
                    {
                        message = "You've signed in successfully.";
                        success = true;
                        Constants.SetProperty( "LoggedIn", true );
                        Constants.SetProperty( "UserId", Constants.User.UserId );
                        Constants.SetProperty( "UserAccount", Constants.User );
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            UIAlertView avAlert = new UIAlertView( "Sign-in result", message, null, "OK", null );
            avAlert.Show();

            return success;
        }
    }
}

