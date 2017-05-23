/*
 * To add Offline Sync Support:
 *  1) Add the NuGet package Microsoft.Azure.Mobile.Client.SQLiteStore (and dependencies) to all client projects
 *  2) Uncomment the #define OFFLINE_SYNC_ENABLED
 *
 * For more information, see: http://go.microsoft.com/fwlink/?LinkId=620342
 */
#define OFFLINE_SYNC_ENABLED

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace JotDown
{
    public partial class TodoItemManager
    {
        static TodoItemManager defaultInstance = new TodoItemManager();
        MobileServiceClient client;
        
        IMobileServiceSyncTable<TodoItem> todoTable;

        private TodoItemManager()
        {
            this.client = new MobileServiceClient( Constants.ApplicationURL );
            
            var store = new MobileServiceSQLiteStore( Constants.OfflineDbPath );
            store.DefineTable<TodoItem>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync( store );

            this.todoTable = client.GetSyncTable<TodoItem>();
        }

        public static TodoItemManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return todoTable is IMobileServiceSyncTable<TodoItem>; }
        }

        public async Task<ObservableCollection<TodoItem>> GetTodoItemsAsync( bool syncItems = false )
        {
            var list = await GetAllTodoItemsAsync();
            return new ObservableCollection<TodoItem>(list.Where(i => !i.Done && i.Account.Equals(Constants.GetProperty<string>("UserId"))));
        }

        public async Task<ObservableCollection<TodoItem>> GetAllTodoItemsAsync( bool syncItems = false )
        {
            try
            {
                if (syncItems)
                {
                    await this.SyncAsync();
                }
                IEnumerable<TodoItem> items = await todoTable
                    .Where( todoItem => !todoItem.Done )
                    .ToEnumerableAsync();

                return new ObservableCollection<TodoItem>( items.Reverse() );
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine( @"Invalid sync operation: {0}", msioe.Message );
            }
            catch (Exception e)
            {
                Debug.WriteLine( @"Sync error: {0}", e.Message );
            }
            return null;
        }

        public async Task SaveTaskAsync( TodoItem item )
        {
            if (item.Id == null)
            {
                await todoTable.InsertAsync( item );
            }
            else
            {
                await todoTable.UpdateAsync( item );
            }
        }
        
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.todoTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allTodoItems",
                    this.todoTable.CreateQuery() );
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync( error.Result );
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine( @"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"] );
                }
            }
        }
    }
}
