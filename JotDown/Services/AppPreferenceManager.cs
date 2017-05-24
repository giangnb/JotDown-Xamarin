using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JotDown.Models;
using Microsoft.WindowsAzure.MobileServices;
using SQLite;

namespace JotDown.Services
{
    public class AppPreferenceManager
    {
        private static AppPreferenceManager instance = new AppPreferenceManager();
        private SQLiteConnection conn;

        public AppPreferenceManager()
        {
            conn = new SQLiteConnection( Constants.OfflineDbPath );
            conn.CreateTable<AppPreference>();
        }

        public static AppPreferenceManager DefaultManager
        {
            get
            {
                return instance;
            }
            private set { instance = value; }
        }

        public void Add(string key, object value)
        {
            conn.InsertOrReplace(new AppPreference() {Key = key, Value = value});
        }

        public List<AppPreference> Fetch()
        {
            return conn.Table<AppPreference>().ToList();
        }

        public object Fetch(string key)
        {
            var items = Fetch();
            var item = items.FirstOrDefault(p => p.Key.ToLower().Equals(key.ToLower()));
            return item?.Value;
        }

        public void Remove(string key)
        {
            conn.Delete( new AppPreference() { Key = key } );
        }

    }
}
