using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using JotDown;
using Microsoft.WindowsAzure.MobileServices;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : IAuthenticate
    {
        public MainPage()
        {
            this.InitializeComponent();

            JotDown.App.Init( this );

            LoadApplication(new JotDown.App());
        }

        // Define a authenticated user.
        private MobileServiceUser user;

        public async Task<MobileServiceUser> Authenticate( MobileServiceAuthenticationProvider provider)
        {
            string message = string.Empty;
            var success = false;

            try
            {
                // Sign in with Facebook login using a server-managed flow.
                if (user == null)
                {
                    user = await TodoItemManager.DefaultManager.CurrentClient
                        .LoginAsync( provider );
                    if (user != null)
                    {
                        success = true;
                        message = "You are now signed-in.";
                    }
                }

            }
            catch (Exception ex)
            {
                message = string.Format( "Authentication Failed: {0}", ex.Message );
            }

            // Display the success or failure message.
            await new MessageDialog( message, "Sign-in result" ).ShowAsync();

            return success?user:null;
        }

        public async void Logout()
        {
            await TodoItemManager.DefaultManager.CurrentClient.LogoutAsync();
        }
    }
}