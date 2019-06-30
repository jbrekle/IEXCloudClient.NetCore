using System;
using IEXCloudClient.NetCore.Helper;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore
{
    public interface INewsArticle
    {
        DateTime PublicationTime { get; set; }
        string Source { get; set; }
        string Url { get; set; }
        string Summary { get; set; }
        string Related { get; set; }
        string Image { get; set; }
        string Language { get; set; }
        bool HasPaywall { get; set; }
    }


	public class NewsArticle : INewsArticle
	{
		[JsonProperty("datetime")]
        [JsonConverter(typeof (UnixDateTimeConverter))]
		public DateTime PublicationTime {get; set;}

		public string Source {get; set;}

		public string Url {get; set;}

		public string Summary {get; set;}

		public string Related {get; set;}

		public string Image {get; set;}

		[JsonProperty("lang")]
		public string Language {get; set;}

		public bool HasPaywall {get; set;}
        
		public override string ToString(){
			return this.ToTableString();
		}
	}

}
