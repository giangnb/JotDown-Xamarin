using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace JotDown
{
    public partial class NoteList : ContentPage
    {
        //TodoItemManager manager;

        public NoteList()
        {
            InitializeComponent();

            //manager = TodoItemManager.DefaultManager;

            // OnPlatform<T> doesn't currently support the "Windows" target platform, so we have this check here.
            if (Constants.TodoManager.IsOfflineEnabled &&
                (Device.RuntimePlatform == Device.Windows || Device.RuntimePlatform == Device.WinPhone))
            {
                syncButton.IsVisible = true;
            }

            InitData();
        }

        private async void InitData()
        {
            todoList.ItemsSource = await Constants.TodoManager.GetTodoItemsAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Set syncItems to true in order to synchronize the data on startup when running in offline mode
            await RefreshItems( true, syncItems: true );

            // Refresh items only when authenticated.
            if (Constants.GetProperty<bool>("LoggedIn").Equals(true))
            {
                // Set syncItems to true in order to synchronize the data
                // on startup when running in offline mode.
                await RefreshItems(true, syncItems: false);

                // Hide the Sign-in button.
                btnAccount.Text = "Account";
            }
            else
            {
                btnAccount.Text = "Sign In";
                syncButton.IsVisible = false;
            }
        }

        // Data methods
        async Task AddItem( TodoItem item )
        {
            await Constants.TodoManager.SaveTaskAsync( item );
            todoList.ItemsSource = await Constants.TodoManager.GetTodoItemsAsync();
        }

        async Task CompleteItem( TodoItem item )
        {
            item.Done = true;
            await Constants.TodoManager.SaveTaskAsync( item );
            todoList.ItemsSource = await Constants.TodoManager.GetTodoItemsAsync();
        }

        public async void OnAdd( object sender, EventArgs e )
        {
            await Navigation.PushAsync( new EditItem() );
            //var todo = new TodoItem { Name = newItemName.Text };
            //await AddItem( todo );

            //newItemName.Text = string.Empty;
            //newItemName.Unfocus();
        }

        // Event handlers
        public async void OnSelected( object sender, SelectedItemChangedEventArgs e )
        {
            var todo = e.SelectedItem as TodoItem;
            await Navigation.PushAsync(new NoteDetail(todo), true);
        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#context
        public async void OnComplete( object sender, EventArgs e )
        {
            var mi = ((MenuItem) sender);
            var todo = mi.CommandParameter as TodoItem;
            await CompleteItem( todo );
        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#pulltorefresh
        public async void OnRefresh( object sender, EventArgs e )
        {
            var list = (ListView) sender;
            Exception error = null;
            try
            {
                await RefreshItems( false, true );
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
                await DisplayAlert( "Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK" );
            }
        }

        public async void OnSyncItems( object sender, EventArgs e )
        {
            await RefreshItems( true, true );
        }

        private async Task RefreshItems( bool showActivityIndicator, bool syncItems )
        {
            using (var scope = new ActivityIndicatorScope( syncIndicator, showActivityIndicator ))
            {
                todoList.ItemsSource = await Constants.TodoManager.GetTodoItemsAsync( syncItems );
            }
        }

        private class ActivityIndicatorScope : IDisposable
        {
            private bool showIndicator;
            private ActivityIndicator indicator;
            private Task indicatorDelay;

            public ActivityIndicatorScope( ActivityIndicator indicator, bool showIndicator )
            {
                this.indicator = indicator;
                this.showIndicator = showIndicator;

                if (showIndicator)
                {
                    indicatorDelay = Task.Delay( 2000 );
                    SetIndicatorActivity( true );
                }
                else
                {
                    indicatorDelay = Task.FromResult( 0 );
                }
            }

            private void SetIndicatorActivity( bool isActive )
            {
                this.indicator.IsVisible = isActive;
                this.indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (showIndicator)
                {
                    indicatorDelay.ContinueWith( t => SetIndicatorActivity( false ), TaskScheduler.FromCurrentSynchronizationContext() );
                }
            }
        }

        private async void BtnAccount_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Account());
        }

        private async void BtnEdit_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditItem( ((MenuItem) sender).CommandParameter as TodoItem ) );
        }

        private async void BtnAbout_OnClicked(object sender, EventArgs e)
        {
            await DisplayAlert("JotDown.",
                "Made by Giang Nguyen\nwww.giangnb.com\n\nPowered by Xamarin.Forms + Azure Mobile Service", 
                "Close");
        }

        private void BtnCancelSearch_OnClicked(object sender, EventArgs e)
        {
            InitData();
            TxtSearch.Text = "";
            BtnCancelSearch.IsVisible = false;
        }

        private async void BtnSearch_OnClicked(object sender, EventArgs e)
        {
            BtnCancelSearch.IsVisible = true;
            var list = await Constants.TodoManager.GetTodoItemsAsync();
            var s = TxtSearch.Text.ToLower();
            todoList.ItemsSource = list.Where(
                i => i.Name.ToLower().Contains(s)
                  && i.Note.ToLower().Contains( s ) );
        }

        private void TxtSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtSearch.Text.Length <= 0)
            {
                BtnSearch.IsVisible = false;
            }
            else
            {
                BtnSearch.IsVisible = true;
            }
        }
    }
}



//using System;
//using System.Threading.Tasks;
//using JotDown.ViewModels;
//using Xamarin.Forms;

//namespace JotDown
//{
//    public partial class NoteList : ContentPage
//    {
//        TodoItemManager manager;
//        private NoteListViewModel viewmodel = new NoteListViewModel();

//        public NoteList()
//        {
//            InitializeComponent();

//            manager = TodoItemManager.DefaultManager;

//            BindingContext = viewmodel;

//            // OnPlatform<T> doesn't currently support the "Windows" target platform, so we have this check here.
//            if (manager.IsOfflineEnabled &&
//                (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone))
//            {
//                var syncButton = new Button
//                {
//                    Text = "Sync",
//                    HeightRequest = 30
//                };
//                syncButton.Clicked += OnSyncItems;

//                buttonsPanel.Children.Add( syncButton );
//            }
//        }

//        protected override async void OnAppearing()
//        {
//            base.OnAppearing();

//            // Set syncItems to true in order to synchronize the data on startup when running in offline mode
//            await RefreshItems( true, syncItems: true );
//        }

//        public async void OnAdd( object sender, EventArgs e )
//        {
//            var todo = new TodoItem { Name = newItemName.Text };
//            await viewmodel.AddItem( todo );

//            newItemName.Text = string.Empty;
//            newItemName.Unfocus();
//        }

//        // Event handlers
//        public async void OnSelected( object sender, SelectedItemChangedEventArgs e )
//        {
//            var todo = e.SelectedItem as TodoItem;
//            if (Device.OS != TargetPlatform.iOS && todo != null)
//            {
//                // Not iOS - the swipe-to-delete is discoverable there
//                if (Device.OS == TargetPlatform.Android)
//                {
//                    await DisplayAlert( todo.Name, "Press-and-hold to complete task " + todo.Name, "Got it!" );
//                }
//                else
//                {
//                    // Windows, not all platforms support the Context Actions yet
//                    if (await DisplayAlert( "Mark completed?", "Do you wish to complete " + todo.Name + "?", "Complete", "Cancel" ))
//                    {
//                        await viewmodel.CompleteItem( todo );
//                    }
//                }
//            }

//            // prevents background getting highlighted
//            todoList.SelectedItem = null;
//        }

//        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#context
//        public async void OnComplete( object sender, EventArgs e )
//        {
//            var mi = ((MenuItem) sender);
//            var todo = mi.CommandParameter as TodoItem;
//            await viewmodel.CompleteItem( todo );
//        }

//        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#pulltorefresh
//        public async void OnRefresh( object sender, EventArgs e )
//        {
//            var list = (ListView) sender;
//            Exception error = null;
//            try
//            {
//                await RefreshItems( false, true );
//            }
//            catch (Exception ex)
//            {
//                error = ex;
//            }
//            finally
//            {
//                list.EndRefresh();
//            }

//            if (error != null)
//            {
//                await DisplayAlert( "Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK" );
//            }
//        }

//        public async void OnSyncItems( object sender, EventArgs e )
//        {
//            await RefreshItems( true, true );
//        }

//        private async Task RefreshItems( bool showActivityIndicator, bool syncItems )
//        {
//            using (var scope = new ActivityIndicatorScope( syncIndicator, showActivityIndicator ))
//            {
//                viewmodel.Items = await manager.GetTodoItemsAsync( syncItems );
//            }
//        }

//        private class ActivityIndicatorScope : IDisposable
//        {
//            private bool showIndicator;
//            private ActivityIndicator indicator;
//            private Task indicatorDelay;

//            public ActivityIndicatorScope( ActivityIndicator indicator, bool showIndicator )
//            {
//                this.indicator = indicator;
//                this.showIndicator = showIndicator;

//                if (showIndicator)
//                {
//                    indicatorDelay = Task.Delay( 2000 );
//                    SetIndicatorActivity( true );
//                }
//                else
//                {
//                    indicatorDelay = Task.FromResult( 0 );
//                }
//            }

//            private void SetIndicatorActivity( bool isActive )
//            {
//                this.indicator.IsVisible = isActive;
//                this.indicator.IsRunning = isActive;
//            }

//            public void Dispose()
//            {
//                if (showIndicator)
//                {
//                    indicatorDelay.ContinueWith( t => SetIndicatorActivity( false ), TaskScheduler.FromCurrentSynchronizationContext() );
//                }
//            }
//        }
//    }
//}

