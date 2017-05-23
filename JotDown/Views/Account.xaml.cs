using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JotDown.Services;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JotDown
{
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class Account : ContentPage
    {
        public Account()
        {
            InitializeComponent();

            ShowFrame();
        }

        private async void ShowFrame()
        {
            var service = await Constants.GetProperty<string>( "AuthService" );
            TxtService.Text = $"Logged in using {service??""} account";

            if ((bool) await Constants.GetProperty<bool>( "LoogedIn" ))
            {
                FrameAccount.IsVisible = true;
            }
            else
            {
                FrameLogIn.IsVisible = true;
            }
        }

        private async void BtnLoginMicrosoft_OnClicked(object sender, EventArgs e)
        {
            Constants.SetProperty("AuthService", "Microsoft Account");
            await LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
        }

        private async void BtnLoginFacebook_OnClicked( object sender, EventArgs e )
        {
            Constants.SetProperty( "AuthService", "Facebook" );
            await LoginAsync( MobileServiceAuthenticationProvider.Facebook );
        }

        private async void BtnLoginTwitter_OnClicked( object sender, EventArgs e )
        {
            Constants.SetProperty( "AuthService", "Twitter" );
            await LoginAsync( MobileServiceAuthenticationProvider.Twitter );
        }

        private async void BtnLoginGoogle_OnClicked( object sender, EventArgs e )
        {
            Constants.SetProperty( "AuthService", "Google+" );
            await LoginAsync( MobileServiceAuthenticationProvider.Google );
        }

        private async void BtnLogout_OnClicked(object sender, EventArgs e)
        {
            Constants.SetProperty( "LoggedIn", false );
            Constants.SetProperty( "UserId", "" );
            var list = await Constants.TodoManager.GetTodoItemsAsync(false);
            foreach (TodoItem item in list)
            {
                item.Account = "";
            }
        }

        private async Task LoginAsync(MobileServiceAuthenticationProvider provider)
        {
            bool auth = false;
            if (App.Authenticator != null)
            {
                auth = await App.Authenticator.Authenticate(provider);
            }

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            if (auth)
            {
                // Assign all current items to current account
                var list = await Constants.TodoManager.GetAllTodoItemsAsync();
                foreach (TodoItem item in list.ToList())
                {
                    item.Account = await Constants.GetProperty<string>("UserId");
                    await Constants.TodoManager.SaveTaskAsync(item);
                }
                await Constants.TodoManager.SyncAsync();

                //Change UI
                Constants.SetProperty( "LoggedIn", true );
                FrameAccount.IsVisible = true;
                FrameLogIn.IsVisible = false;
                var service = await Constants.GetProperty<string>( "AuthService" );
                TxtService.Text = $"Logged in using {service} account";
            }
        }
    }
}