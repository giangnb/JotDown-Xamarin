using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace JotDown
{
	public class TodoItem
	{
	    public TodoItem()
	    {
	        Account = Constants.GetProperty<string>("UserId").Result;
	    }

		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

	    [JsonProperty(PropertyName = "account")]
	    public string Account { get; set; }

	    [JsonProperty( PropertyName = "name" )]
	    public string Name { get; set; }

	    [JsonProperty( PropertyName = "note" )]
	    public string Note { get; set; } = "";

	    [JsonProperty( PropertyName = "tag" )]
	    public string Tag { get; set; }

	    [JsonProperty(PropertyName = "isnote")]
	    public bool IsNote { get; set; } = true;

        [JsonProperty(PropertyName = "complete")]
        public bool Done { get; set; }

	    [JsonIgnore]
	    public List<Item> Todo
	    {
	        get { return !IsNote ? JsonConvert.DeserializeObject<List<Item>>( Note ) : null; }
	        set
	        {
	            Note = JsonConvert.SerializeObject( value );
	            IsNote = false;
	        }
	    }

	    [JsonIgnore]
	    public bool IsTodo
	    {
	        get
	        {
	            return !IsNote;
	        }
	        set
	        {
	            if (value)
	            {
	                ConvertToTodo();
	                IsNote = false;
	            }
	            else
	            {
	                ConvertToNote();
	                IsNote = true;
	            }
	        }
	    }

	    public bool ConvertToNote()
	    {
	        if (IsNote)
	        {
	            return false;
	        }

	        try
	        {
	            foreach (var item in Todo)
	            {
	                Note += $"{item.Name}\n";
	            }
	            IsNote = true;
	            return true;
	        }
	        catch (Exception)
	        {
	            return false;
	        }
	    }

	    public bool ConvertToTodo()
	    {
	        if (IsTodo)
	        {
	            return false;
	        }

	        try
	        {
	            var temp = new List<Item>();
	            foreach (var s in Note.Replace( "\r", "" ).Split( '\n' ))
	            {
	                temp.Add( new Item() { Name = s } );
	            }
	            Todo = temp;
	            IsNote = false;
	            return true;
	        }
	        catch (Exception)
	        {
	            return false;
	        }
	    }

        [Version]
        public string Version { get; set; }
	}

    public class Item
    {
        [JsonProperty( PropertyName = "name" )]
        public string Name { get; set; }

        [JsonProperty( PropertyName = "complete" )]
        public bool Complete { get; set; } = false;
    }
}

