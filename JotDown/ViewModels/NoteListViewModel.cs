using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JotDown.Annotations;

namespace JotDown.ViewModels
{
    class NoteListViewModel : INotifyPropertyChanged
    {
        TodoItemManager manager;
        public ObservableCollection<TodoItem> Items { get; set; }

        public NoteListViewModel()
        {
            manager = TodoItemManager.DefaultManager;
            InitData();
        }

        private async void InitData()
        {
            Items = await manager.GetTodoItemsAsync();
        }

        // Data methods
        public async Task AddItem( TodoItem item )
        {
            await manager.SaveTaskAsync( item );
            Items = await manager.GetTodoItemsAsync();
        }

        public async Task CompleteItem( TodoItem item )
        {
            item.Done = true;
            await manager.SaveTaskAsync( item );
            Items = await manager.GetTodoItemsAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
