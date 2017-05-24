using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JotDown.ViewModels;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JotDown
{
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class NoteDetail : ContentPage
    {
        private NoteDetailViewModel viewmodel;
        private TodoItem todo;

        public NoteDetail(TodoItem todo)
        {
            this.todo = todo;

            InitializeComponent();
            
            if (todo != null)
            {
                UpdateData();
            }
            BindingContext = viewmodel = new NoteDetailViewModel( todo );

            this.Appearing += OnAppearing;
        }

        private async void OnAppearing(object sender, EventArgs eventArgs)
        {
            var list = await TodoItemManager.DefaultManager.GetTodoItemsAsync(true);
            var selected = list.FirstOrDefault(p => p.Id.Equals(todo.Id));
            if (selected != null)
            {
                todo = selected;
                viewmodel.Item = selected;
                UpdateData();
            }
        }

        private void UpdateData()
        {
            this.Title = todo.Name.Length>0?todo.Name:"Note detail";
            if (todo.IsNote)
            {
                LblNote.Text = todo.Note.Length > 0?todo.Note:"This is an empty note.";
            }
        }

        private async void OnComplete(object sender, EventArgs e)
        {
            var mi = ((MenuItem) sender);
            var item = mi.CommandParameter as Item;
            var todos = todo.Todo;
            foreach (Item t in todos)
            {
                if (item.Name.Equals( t.Name ) && item.Complete == t.Complete)
                {
                    t.Complete = !item.Complete;
                    return;
                }
            }
            todo.Todo = todos;
            await TodoItemManager.DefaultManager.SaveTaskAsync(todo);
            await TodoItemManager.DefaultManager.SyncAsync();
            UpdateData();
        }

        private async void Lists_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = e.SelectedItem as Item;
            var context = selected.Complete ? "Pending" : "Completed";
            bool accept = await DisplayAlert($"Mark item as {context}?", "", "Yes", "No");
            if (accept)
            {
                await viewmodel.CompleteItem(selected);
            }
        }

        private async void BtnEdit_Clicked( object sender, EventArgs e )
        {
            var choose = await this.DisplayActionSheet( "", "Cancel", "Delete", "Edit", "Convert" );
            switch (choose)
            {
                case "Edit":
                    await Navigation.PushAsync( new EditItem( viewmodel.Item ), true );
                    break;
                case "Delete":
                    break;
                case "Convert":
                    var context = "Dou you want to convert this note to ";
                    context += viewmodel.Item.IsNote ? "Todo List" : "Plain text Note";
                    if (await DisplayAlert( "Convert note", context + "?", "Yes", "No"))
                    {
                        
                    }
                    break;
            }
        }
    }
}