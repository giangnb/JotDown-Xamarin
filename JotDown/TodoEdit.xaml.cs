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
    //[XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class TodoEdit : ContentPage
    {
        private TodoItem todo;
        private TodoItemManager manager;
        public TodoEdit()
        {
            manager = TodoItemManager.DefaultManager;

            todo = new TodoItem();
            InitializeComponent();
            Title = "New note";
        }

        public TodoEdit(TodoItem todo)
        {
            manager = TodoItemManager.DefaultManager;

            this.todo = todo;
            InitializeComponent();
            Title = "Edit note";

            LoadData();
        }

        private void LoadData()
        {
            TxtName.Text = todo.Name;
            TxtContent.Text = todo.Content;
            SwList.IsToggled = !todo.Note;
        }

        private void SwList_OnToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                LblContent.Text = "Todo list (Separate each item by a line break)";
            }
            else
            {
                LblContent.Text = "Note content";
            }
        }

        private async void BtnSave_OnClicked(object sender, EventArgs e)
        {
            if (DataValite())
            {
                todo.Name = TxtName.Text.Trim();
                todo.Content = TxtContent.Text.Trim();
                todo.Note = !SwList.IsToggled;
                await manager.SaveTaskAsync(todo);
                await Navigation.PopAsync(true);
            }
            else
            {
                await DisplayAlert("Can't save note", "Please provide note's name and content!", "Ok");
            }
        }

        private bool DataValite()
        {
            return TxtName.Text.Trim().Length > 0 &&
                   TxtContent.Text.Trim().Length > 0;
        }
    }
}