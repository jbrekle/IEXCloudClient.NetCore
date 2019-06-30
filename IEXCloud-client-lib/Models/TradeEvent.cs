using System;
using IEXCloudClient.NetCore.Helper;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore
{    public interface ITradeEvent
    {
        decimal Price {get; set;}
        int Size {get; set;}
        long TradeId {get; set;}
        bool IsISO {get; set;}
        bool IsOddLot {get; set;}
        bool IsOutsideRegularHours {get; set;}
        bool IsSinglePriceCross {get; set;}
        bool IsTradeThroughExempt {get; set;}

        DateTime TradeTime {get; set;}

        String Symbol {get; set;}
    }
    
    public class TradeEvent : ITradeEvent
    {
        public decimal Price {get; set;}
        public int Size {get; set;}
        public long TradeId {get; set;}
        public bool IsISO {get; set;}
        public bool IsOddLot {get; set;}
        public bool IsOutsideRegularHours {get; set;}
        public bool IsSinglePriceCross {get; set;}
        public bool IsTradeThroughExempt {get; set;}

        [JsonConverter(typeof (UnixDateTimeConverter))]
        [JsonProperty("timestamp")]
        public DateTime TradeTime {get; set;}

        [JsonIgnore]
        public String Symbol {get; set;}

		public override string ToString(){
			return this.ToTableString();
		}
    }

    public class TradeMessage {
        public String Symbol {get; set;}

        [JsonProperty("data")]
        public TradeEvent Trade {get; set;}
    }
}
