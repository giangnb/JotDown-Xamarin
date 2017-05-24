using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Microsoft.WindowsAzure.MobileServices;
using UIKit;

namespace JotDown.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IAuthenticate
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

	    // Define a authenticated user.
	    private MobileServiceUser user;

	    public async Task<MobileServiceUser> Authenticate( MobileServiceAuthenticationProvider provider)
	    {
	        var success = false;
	        var message = string.Empty;
	        try
	        {
	            // Sign in with Facebook login using a server-managed flow.
	            if (user == null)
	            {
	                user = await TodoItemManager.DefaultManager.CurrentClient
	                    .LoginAsync( UIApplication.SharedApplication.KeyWindow.RootViewController,
	                        provider );
	                if (user != null)
	                {
	                    message = "You are now signed-in.";
	                    success = true;
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

	        return success?user:null;
	    }

	    public async void Logout()
	    {
	        foreach (var cookie in NSHttpCookieStorage.SharedStorage.Cookies)
	        {
	            NSHttpCookieStorage.SharedStorage.DeleteCookie( cookie );
	        }
	        await TodoItemManager.DefaultManager.CurrentClient.LogoutAsync();
        }
	}
}

