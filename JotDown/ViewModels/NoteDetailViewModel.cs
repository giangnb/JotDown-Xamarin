using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JotDown.Annotations;

namespace JotDown.ViewModels
{
    public class NoteDetailViewModel : INotifyPropertyChanged
    {
        public NoteDetailViewModel( TodoItem todo )
        {
            Item = todo;

            if (Item.IsTodo)
            {
                InitLists();
            }
        }

        private void InitLists()
        {
            Pending = Item.Todo.Where( i => !i.Complete ).ToList();
            Completed = Item.Todo.Where( i => i.Complete ).ToList();
        }

        public TodoItem Item { get; set; }
        public List<Item> Pending = new List<Item>();
        public List<Item> Completed = new List<Item>();

        public string Note
        {
            get
            {
                if (Item.IsNote)
                {
                    return Item.Note;
                }
                else
                {
                    string ctx = "";
                    foreach (Item i in Item.Todo)
                    {
                        ctx += " - " + i.Name + "\n";
                    }
                    return ctx;
                }
            }
            set { }
        }

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
            await TodoItemManager.DefaultManager.SaveTaskAsync( Item );
            InitLists();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
