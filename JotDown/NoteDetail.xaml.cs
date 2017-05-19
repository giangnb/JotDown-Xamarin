using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JotDown.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JotDown
{
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class NoteDetail : ContentPage
    {
        private NoteDetailViewModel viewmodel;

        public NoteDetail()
        {
            InitializeComponent();
            var todo = new TodoItem() { Name = "Test", Note = "This is text content" };
            BindingContext = viewmodel = new NoteDetailViewModel( todo );
        }

        public NoteDetail(TodoItem todo)
        {
            InitializeComponent();
            BindingContext = viewmodel =  new NoteDetailViewModel(todo);
        }

        private async void OnComplete(object sender, EventArgs e)
        {
            var mi = ((MenuItem) sender);
            var item = mi.CommandParameter as Item;
            await viewmodel.CompleteItem( item );
        }
    }
}