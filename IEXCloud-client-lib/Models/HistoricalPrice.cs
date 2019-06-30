using System;
using IEXCloudClient.NetCore.Helper;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore
{
    public interface IHistoricalPrice
    {
        DateTime Date { get; set; }
        decimal Open { get; set; }
        decimal High { get; set; }
        decimal Low { get; set; }
        decimal Close { get; set; }
        decimal Volume { get; set; }
        decimal UOpen { get; set; }
        decimal UHigh { get; set; }
        decimal ULow { get; set; }
        decimal UClose { get; set; }
        decimal UVolume { get; set; }
        decimal Change { get; set; }
        decimal ChangePercent { get; set; }
        string Label { get; set; }
        decimal ChangeOverTime { get; set; }
    }

    public class HistoricalPrice : IHistoricalPrice
    {
        [JsonConverter(typeof (YearMonthDayConverter))]
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public decimal UOpen { get; set; }
        public decimal UHigh { get; set; }
        public decimal ULow { get; set; }
        public decimal UClose { get; set; }
        public decimal UVolume { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }
        public string Label { get; set; }
        public decimal ChangeOverTime { get; set; }
		
		public override string ToString(){
			return this.ToTableString();
		}
    }
}
