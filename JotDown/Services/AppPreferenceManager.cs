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
        private SQLiteAsyncConnection conn;

        private AppPreferenceManager()
        {
            conn = new SQLiteAsyncConnection( Constants.PreferenceDbPath );
            conn.CreateTableAsync<AppPreference>();
        }

        public static AppPreferenceManager DefaultManager
        {
            get { return instance; }
            private set { instance = value; }
        }

        public async void Add(string key, object value)
        {
            await conn.InsertOrReplaceAsync(new AppPreference() {Key = key, Value = value});
        }

        public Task<List<AppPreference>> Fetch()
        {
            return conn.Table<AppPreference>().ToListAsync();
        }

        public async Task<object> Fetch(string key)
        {
            var items = await Fetch();
            var item = items.FirstOrDefault(p => p.Key.ToLower().Equals(key.ToLower()));
            return item?.Value;
        }

        public async void Remove(string key)
        {
            await conn.DeleteAsync( new AppPreference() { Key = key } );
        }

    }
}
