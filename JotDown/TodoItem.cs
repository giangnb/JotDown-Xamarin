using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace JotDown
{
	public class TodoItem
	{
	    [JsonProperty(PropertyName = "account")]
	    public string Account { get; set; }

        string id;
		string name = "";
	    string content = "";
		bool note = true;

		[JsonProperty(PropertyName = "id")]
		public string Id
		{
			get { return id; }
			set { id = value;}
		}

		[JsonProperty(PropertyName = "text")]
		public string Name
		{
			get { return name; }
			set { name = value;}
		}

	    [JsonProperty( PropertyName = "content" )]
	    public string Content
	    {
	        get { return content; }
	        set { content = value; }
	    }

        [JsonProperty(PropertyName = "note")]
		public bool Note
		{
			get { return note; }
			set { note = value;}
		}

        [JsonIgnore]
	    public string TruncateContent
	    {
	        get
	        {
	            return Content.Replace( "\r", ", " );
            }
            private set { }
	    }

        [Version]
        public string Version { get; set; }
	}
}

