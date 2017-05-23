using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using JotDown.Services;
using UWP;
using Xamarin.Forms;

[assembly: Dependency( typeof( LocalFile ) )]
namespace UWP
{
    public class LocalFile : ILocalFile
    {
        public async void SaveText( string filename, string text )
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync( filename, CreationCollisionOption.ReplaceExisting );
            await FileIO.WriteTextAsync( sampleFile, text );
        }

        public async Task<string> LoadTextAsync( string filename )
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync( filename );
            string text = await Windows.Storage.FileIO.ReadTextAsync( sampleFile );
            return text;
        }

        public string LoadText( string filename )
        {
            return LoadTextAsync(filename).Result;
        }
    }
}
