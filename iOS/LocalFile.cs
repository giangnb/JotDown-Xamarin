using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using JotDown.iOS;
using JotDown.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency( typeof( LocalFile ) )]
namespace JotDown.iOS
{
    public class LocalFile : ILocalFile
    {
        public void SaveText( string filename, string text )
        {
            var documentsPath = System.Environment.GetFolderPath( System.Environment.SpecialFolder.Personal );
            var filePath = Path.Combine( documentsPath, filename );
            System.IO.File.WriteAllText( filePath, text );
        }
        public string LoadText( string filename )
        {
            var documentsPath = System.Environment.GetFolderPath( System.Environment.SpecialFolder.Personal );
            var filePath = Path.Combine( documentsPath, filename );
            return System.IO.File.ReadAllText( filePath );
        }
    }
}