using System;
using IEXCloudClient.NetCore.Helper;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore
{
	public interface IPriceTime
	{
	     decimal Price {get; set;}
	     DateTime Time {get; set;}
	}

    public interface IOHLC
    {
        IPriceTime Open { get; set; }
        IPriceTime Close { get; set; }
        decimal High { get; set; }
        decimal Low { get; set; }
        string Symbol { get; set; }
    }

	public class PriceTime : IPriceTime
	{
	    public decimal Price {get; set;}
		
        [JsonConverter(typeof (UnixDateTimeConverter))]
	    public DateTime Time {get; set;}

		public override string ToString(){
			return this.ToTableString();
		}
	}

    public class OHLC : IOHLC
    {
		[JsonConverter(typeof(ConcreteTypeConverter<PriceTime>))]
        public IPriceTime Open { get; set; }
		[JsonConverter(typeof(ConcreteTypeConverter<PriceTime>))]
        public IPriceTime Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public string Symbol { get; set; }

		public override string ToString(){
			return this.ToTableString();
		}
    }
}
