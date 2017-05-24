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
    public partial class TodoDetail : ContentPage
    {
        private TodoItem todo;

        public TodoDetail(TodoItem todo)
        {
            this.todo = todo;
            InitializeComponent();

            if (todo != null)
            {
                UpdateData();
            }
        }

        private void UpdateData()
        {
            Title = todo.Name.Length>0?todo.Name:"Untitled Note";
            if (todo.Note)
            {
                TxtContent.Text = todo.Content.Length > 0 ? todo.Content : "";
            }
            else
            {
                TxtContent.Text = "";
                foreach (string s in todo.Content.Split('\n', '\r'))
                {
                    TxtContent.Text += " - " + s + "\n";
                }
            }
        }

        private async void BtnEdit_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodoEdit(todo), true);
        }

        private async void BtnDelete_OnClicked(object sender, EventArgs e)
        {
            var ch = await DisplayAlert("Delete note", "You are going to delete this note permanently.\nAre you sure?",
                "Yes, delete it", "No");
            if (ch)
            {
                TodoItemManager.DefaultManager.DeleteAsync( todo );
            }
        }

        protected override void OnAppearing()
        {
            RefreshData();
        }

        private async void RefreshData()
        {
            var list = await TodoItemManager.DefaultManager.GetTodoItemsAsync(App.authenticated!=null);
            var t = list.FirstOrDefault(i => i.Id.Equals(todo.Id));
            if (t != null)
            {
                todo = t;
                UpdateData();
            }
        }
    }
}