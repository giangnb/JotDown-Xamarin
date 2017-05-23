using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SQLite;

namespace JotDown.Models
{
    [Table("appPref")]
    public class AppPreference
    {
        [PrimaryKey, Unique, Column("key")]
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [Column("value")]
        [JsonProperty( PropertyName = "value" )]
        public object Value { get; set; }
    }
}
