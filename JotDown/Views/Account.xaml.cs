using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

            if ((bool) Application.Current.Properties["LoggedIn"])
            {
                FrameAccount.IsVisible = true;
            }
            else
            {
                FrameLogIn.IsVisible = true;
            }
        }

        private void BtnLoginMicrosoft_OnClicked(object sender, EventArgs e)
        {
        }

        private void BtnLoginFacebook_OnClicked(object sender, EventArgs e)
        {
        }

        private void BtnLoginTwitter_OnClicked(object sender, EventArgs e)
        {
        }

        private void BtnLoginGoogle_OnClicked(object sender, EventArgs e)
        {
        }

        private void BtnLogout_OnClicked(object sender, EventArgs e)
        {
        }
    }
}