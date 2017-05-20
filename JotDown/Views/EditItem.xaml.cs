using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JotDown.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JotDown
{
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class EditItem : ContentPage
    {
        private EditItemViewModel viewmodel;

        public EditItem()
        {
            InitializeComponent();
            BindingContext = viewmodel = new EditItemViewModel();
            LblNotice.IsVisible = false;
        }

        public EditItem(TodoItem item)
        {
            InitializeComponent();
            BindingContext = viewmodel = new EditItemViewModel(item);
            if (item.IsTodo)
            {
                LblNotice.IsVisible = true;
            }
            else
            {
                LblNotice.IsVisible = false;
            }
        }

        private void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            try
            {
                viewmodel.Item.IsTodo = e.Value;
                if (e.Value)
                {
                    LblNotice.IsVisible = true;
                }
                else
                {
                    LblNotice.IsVisible = false;
                }
            }
            catch (Exception )
            {
                // ignore
            }
        }

        private async void SaveButton_OnClicked(object sender, EventArgs e)
        {
            if (viewmodel.DoValidate())
            {
                await viewmodel.SaveItem();
                await Navigation.PopAsync(true);
            }
            else
            {
                await DisplayAlert("Incomplete note", "Please provide the Name and Content of your note!", "Okay");
            }
        }
    }
}