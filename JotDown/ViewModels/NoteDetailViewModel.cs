using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JotDown.ViewModels
{
    public class NoteDetailViewModel : INotifyPropertyChanged
    {
        TodoItemManager manager;

        public NoteDetailViewModel( TodoItem todo )
        {
            manager = TodoItemManager.DefaultManager;
            Item = todo;
            SyncItem();
        }

        private async void SyncItem()
        {
            var list = await manager.GetTodoItemsAsync(true);
            var selectedItem = list.FirstOrDefault(item => item.Id.Equals(Item.Id));
            if (selectedItem != null)
            {
                Item = selectedItem;
            }
        }

        public TodoItem Item { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged( [CallerMemberName]string propertyName = "" ) =>
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );

        public async Task CompleteItem(Item checkItem)
        {
            var todos = Item.Todo;
            foreach (Item item in todos)
            {
                if (item.Name.Equals(checkItem.Name) && item.Complete == checkItem.Complete)
                {
                    item.Complete = !checkItem.Complete;
                    return;
                }
            }
            Item.Todo = todos;
            await manager.SaveTaskAsync( Item );
            SyncItem();
        }
    }
}
