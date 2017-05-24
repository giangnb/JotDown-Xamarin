using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JotDown
{
    //[XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class Account : ContentPage
    {
        public Account()
        {
            InitializeComponent();
            
                FrameAccount.IsVisible = App.authenticated!=null;
                FrameLogin.IsVisible = App.authenticated==null;
        }

        private async void BtnLogin_OnClicked(object sender, EventArgs e)
        {
            MobileServiceAuthenticationProvider provider;

            var choose = await DisplayActionSheet( "Choose a service", "Cancel", null, "Microsoft Account", "Facebook",
                "Google+", "Twitter" );
            switch (choose)
            {
                case "Microsoft Account":
                    provider = MobileServiceAuthenticationProvider.MicrosoftAccount;
                    break;
                case "Facebook":
                    provider = MobileServiceAuthenticationProvider.Facebook;
                    break;
                case "Google+":
                    provider = MobileServiceAuthenticationProvider.Google;
                    break;
                case "Twitter":
                    provider = MobileServiceAuthenticationProvider.Twitter;
                    break;
                default:
                    return;
            }

            if (App.Authenticator != null)
                App.authenticated = await App.Authenticator.Authenticate( provider );

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            if (App.authenticated != null)
            {
                FrameAccount.IsVisible = true;
                FrameLogin.IsVisible = false;
                await TodoItemManager.DefaultManager.GetTodoItemsAsync(true);
            }
        }

        private void BtnLogout_OnClicked(object sender, EventArgs e)
        {
            App.Authenticator.Logout();
            FrameAccount.IsVisible = false;
            FrameLogin.IsVisible = true;
        }

        protected override void OnAppearing()
        {
            if (App.authenticated != null)
            {
                FrameAccount.IsVisible = true;
                FrameLogin.IsVisible = false;
            }
        }
    }
}