using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JotDown
{
    //[XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class TodoList : ContentPage
    {
        TodoItemManager manager;

        public TodoList()
        {
            InitializeComponent();

            manager = TodoItemManager.DefaultManager;

            // OnPlatform<T> doesn't currently support the "Windows" target platform, so we have this check here.
            if (manager.IsOfflineEnabled &&
                (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone))
            {
                var syncButton = new Button
                {
                    Text = "Sync items",
                    HeightRequest = 30
                };
                syncButton.Clicked += OnSyncItems;

                buttonsPanel.Children.Add(syncButton);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Set syncItems to true in order to synchronize the data on startup when running in offline mode
            await RefreshItems(true, syncItems: true);

            if (App.authenticated!=null)
            {
                // Set syncItems to true in order to synchronize the data
                // on startup when running in offline mode.
                await RefreshItems( true, syncItems: false );
            }
        }

        public async void OnAdd(object sender, EventArgs e)
        {
            if (App.authenticated == null)
            {
                bool ch = await DisplayAlert( "Login before adding note", "Please sign-in to save and sync your notes", "Ok", "Later" );
                if (ch)
                {
                    await Navigation.PushAsync(new Account());
                    return;
                }
            }
            
            await Navigation.PushAsync( new TodoEdit(), true );
        }

        // Event handlers
        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var todo = e.SelectedItem as TodoItem;
            if (Device.OS != TargetPlatform.iOS && todo != null)
            {
                await Navigation.PushAsync( new TodoDetail( todo ), true );
            }
        }
        
        public async void OnEdit(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var todo = mi.CommandParameter as TodoItem;
            await Navigation.PushAsync(new TodoEdit(todo), true);
        }

        public async void OnDelete( object sender, EventArgs e )
        {
            if (await DisplayAlert("Delete note", "Are you sure?", "Yes, delete", "No"))
            {
                var mi = ((MenuItem) sender);
                var todo = mi.CommandParameter as TodoItem;
                TodoItemManager.DefaultManager.DeleteAsync(todo);
                await RefreshItems(true, true);
            }
        }
        
        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(false, true);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        public async void OnSyncItems(object sender, EventArgs e)
        {
            await RefreshItems(true, true);
        }

        private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                todoList.ItemsSource = await manager.GetTodoItemsAsync(syncItems && App.authenticated != null );
            }
        }

        private class ActivityIndicatorScope : IDisposable
        {
            private bool showIndicator;
            private ActivityIndicator indicator;
            private Task indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                this.indicator = indicator;
                this.showIndicator = showIndicator;

                if (showIndicator)
                {
                    indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                this.indicator.IsVisible = isActive;
                this.indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (showIndicator)
                {
                    indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        private async void NewItemName_OnCompleted(object sender, EventArgs e)
        {
            var list = await manager.GetTodoItemsAsync(false);
            if (TxtSearch.Text.Length > 0)
            {
                todoList.ItemsSource =
                    list.Where(i => i.Name.Contains(TxtSearch.Text) || i.Content.Contains(TxtSearch.Text));
            }
            else
            {
                todoList.ItemsSource = list;
            }
        }
    }
}

