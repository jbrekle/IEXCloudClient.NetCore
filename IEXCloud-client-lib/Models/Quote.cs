
using IEXCloudClient.NetCore.Helper;

namespace IEXCloudClient.NetCore
{
	public interface IQuote
	{
		string Symbol { get; set; }
        string CompanyName { get; set; }
        string CalculationPrice { get; set; }
        decimal Open { get; set; }
        long OpenTime { get; set; }
        decimal Close { get; set; }
        long CloseTime { get; set; }
        decimal High { get; set; }
        decimal Low { get; set; }
        decimal LatestPrice { get; set; }
        string LatestSource { get; set; }
        string LatestTime { get; set; }
        long LatestUpdate { get; set; }
        decimal IexRealtimePrice { get; set; }
        long LatestVolume { get; set; }
        long IexRealtimeSize { get; set; }
        long IexLastUpdated { get; set; }
        decimal DelayedPrice { get; set; }
        long DelayedPriceTime { get; set; }
        decimal ExtendedPrice { get; set; }
        decimal ExtendedChange { get; set; }
        decimal ExtendedChangePercent { get; set; }
        long ExtendedPriceTime { get; set; }
        decimal Change { get; set; }
        decimal ChangePercent { get; set; }
        decimal IexMarketPercent { get; set; }
        decimal IexVolume { get; set; }
        decimal AvgTotalVolume { get; set; }
        decimal IexBidPrice { get; set; }
        decimal IexBidSize { get; set; }
        decimal IexAskPrice { get; set; }
        decimal IexAskSize { get; set; }
        decimal MarketCap { get; set; }
        decimal Week52High { get; set; }
        decimal Week52Low { get; set; }
        decimal YtdChange { get; set; }
	}
    
    public class Quote : IQuote
    {
		public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public string CalculationPrice { get; set; }
        public decimal Open { get; set; }
        public long OpenTime { get; set; }
        public decimal Close { get; set; }
        public long CloseTime { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal LatestPrice { get; set; }
        public string LatestSource { get; set; }
        public string LatestTime { get; set; }
        public long LatestUpdate { get; set; }
        public decimal IexRealtimePrice { get; set; }
        public long LatestVolume { get; set; }
        public long IexRealtimeSize { get; set; }
        public long IexLastUpdated { get; set; }
        public decimal DelayedPrice { get; set; }
        public long DelayedPriceTime { get; set; }
        public decimal ExtendedPrice { get; set; }
        public decimal ExtendedChange { get; set; }
        public decimal ExtendedChangePercent { get; set; }
        public long ExtendedPriceTime { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }
        public decimal IexMarketPercent { get; set; }
        public decimal IexVolume { get; set; }
        public decimal AvgTotalVolume { get; set; }
        public decimal IexBidPrice { get; set; }
        public decimal IexBidSize { get; set; }
        public decimal IexAskPrice { get; set; }
        public decimal IexAskSize { get; set; }
        public decimal MarketCap { get; set; }
        public decimal Week52High { get; set; }
        public decimal Week52Low { get; set; }
        public decimal YtdChange { get; set; }

		public override string ToString(){
			return this.ToTableString();
		}
    }
}
