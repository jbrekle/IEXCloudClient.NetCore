using System;
using IEXCloudClient.NetCore.Helper;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore
{
    public interface IMarketVolumeUS
    {
        string MIC { get; set; }
        string TapeID { get; set; }
        string VenueName { get; set; }
        decimal Volume { get; set; }
        decimal TapeA { get; set; }
        decimal TapeB { get; set; }
        decimal TapeC { get; set; }
        decimal MarketPercent { get; set; }
        DateTime LastUpdated { get; set; }
    }

    public class MarketVolumeUS : IMarketVolumeUS
    {
        public string MIC { get; set; }
        public string TapeID { get; set; }
        public string VenueName { get; set; }
        public decimal Volume { get; set; }
        public decimal TapeA { get; set; }
        public decimal TapeB { get; set; }
        public decimal TapeC { get; set; }
        public decimal MarketPercent { get; set; }

        [JsonConverter(typeof (UnixDateTimeConverter))]
        public DateTime LastUpdated { get; set; }
        
		public override string ToString(){
			return this.ToTableString();
		}
    }
}
