using System;
using IEXCloudClient.NetCore.Helper;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore
{
    public interface IDelayedQuote
    {
        string Symbol { get; set; }
        decimal DelayedPrice { get; set; }
        decimal DelayedSize { get; set; }
        DateTime DelayedPriceTime { get; set; }
        DateTime ProcessedTime { get; set; }
    }

    public class DelayedQuote : IDelayedQuote
    {
        public string Symbol { get; set; }
        public decimal DelayedPrice { get; set; }
        public decimal DelayedSize { get; set; }
        [JsonConverter(typeof (UnixDateTimeConverter))]
        public DateTime DelayedPriceTime { get; set; }
		[JsonConverter(typeof (UnixDateTimeConverter))]
        public DateTime ProcessedTime { get; set; }

		public override string ToString(){
			return this.ToTableString();
		}
    }
}
