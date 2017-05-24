using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using Microsoft.WindowsAzure.MobileServices;

namespace JotDown.Droid
{
	[Activity (Label = "JotDown.Droid",
		Icon = "@drawable/icon",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		Theme = "@android:style/Theme.Holo.Light")]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, IAuthenticate
	{
	    private MobileServiceUser user;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Initialize Azure Mobile Apps
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			// Initialize Xamarin Forms
			global::Xamarin.Forms.Forms.Init (this, bundle);

		    // Initialize the authenticator before loading the app.
		    App.Init( (IAuthenticate) this );

            // Load the main application
            LoadApplication (new App ());
		}

	    public async Task<MobileServiceUser> Authenticate( MobileServiceAuthenticationProvider provider)
	    {
	        var success = false;
	        var message = string.Empty;
	        try
	        {
	            // Sign in with Facebook login using a server-managed flow.
	            user = await TodoItemManager.DefaultManager.CurrentClient.LoginAsync( this,
	                provider );
	            if (user != null)
	            {
	                message = "You are now signed-in.";
	                success = true;
	            }
	        }
	        catch (Exception ex)
	        {
	            message = ex.Message;
	        }

	        // Display the success or failure message.
	        AlertDialog.Builder builder = new AlertDialog.Builder( this );
	        builder.SetMessage( message );
	        builder.SetTitle( "Sign-in result" );
	        builder.Create().Show();

	        return success?user:null;
	    }

	    public async void Logout()
	    {
	        CookieManager.Instance.RemoveAllCookie();
	        await TodoItemManager.DefaultManager.CurrentClient.LogoutAsync();
        }
	}
}

