using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JotDown.Annotations;
using Xamarin.Forms;

namespace JotDown.ViewModels
{
    public class EditItemViewModel : INotifyPropertyChanged
    {
        public TodoItem Item { get; set; }
        public string Title { get; set; }
        public string SaveButton { get; set; }
        public bool Validate { get; set; }

        public EditItemViewModel()
        {
            Item = new TodoItem();
            Title = "New Note";
            SaveButton = "Create note";
        }

        public EditItemViewModel(TodoItem item)
        {
            Item = item ?? new TodoItem();
            Title = "Note Editor";
            SaveButton = "Save changes";
            DoValidate();
        }

        public string ParsedNote
        {
            get
            {
                if (Item.IsNote)
                {
                    return Item.Note;
                }
                else
                {
                    StringBuilder sb = new StringBuilder("");
                    foreach (Item i in Item.Todo)
                    {
                        sb.Append(i.Name).Append("\n");
                    }
                    return sb.ToString();
                }
            }
            set
            {
                if (Item.IsNote)
                {
                    Item.Note = value.Trim();
                }
                else
                {
                    var list = new List<Item>();
                    foreach (string s in value.Trim().Split('\n'))
                    {
                        list.Add(new Item(){Complete = false, Name = s});
                    }
                    Item.Todo = list;
                }
            }
        }

        public bool DoValidate()
        {
            if (Item.Note == null || Item.Name == null)
            {
                Validate = false;
                return false;
            }
            Validate = Item.Name.Length > 0 &&
                   ParsedNote.Trim().Length > 0;
            return Validate;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            DoValidate();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task SaveItem()
        {
            await Constants.TodoManager.SaveTaskAsync( Item );
        }
    }
}