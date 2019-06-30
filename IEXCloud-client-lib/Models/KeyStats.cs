using IEXCloudClient.NetCore.Helper;

namespace IEXCloudClient.NetCore
{
    public interface IKeyStats
    {
        string CompanyName { get; set; }
        decimal MarketCap { get; set; }
        decimal Week52High { get; set; }
        decimal Week52Low { get; set; }
        decimal Week52Change { get; set; }
        decimal SharesOutstanding { get; set; }
        decimal Employees { get; set; }
        decimal Avg30Volume { get; set; }
        decimal Avg10Volume { get; set; }
        decimal Float { get; set; }
        decimal TtmEPS { get; set; }
        decimal TtmDividendRate { get; set; }
        decimal DividendYield { get; set; }
        string NextDividendDate { get; set; }
        string NextEarningsDate { get; set; }
        decimal PeRatio { get; set; }
        decimal Day200MovingAvg { get; set; }
        decimal Day50MovingAvg { get; set; }
        decimal MaxChangePercent { get; set; }
        decimal Year5ChangePercent { get; set; }
        decimal Year2ChangePercent { get; set; }
        decimal Year1ChangePercent { get; set; }
        decimal YtdChangePercent { get; set; }
        decimal Month6ChangePercent { get; set; }
        decimal Month3ChangePercent { get; set; }
        decimal Month1ChangePercent { get; set; }
        decimal Day5ChangePercent { get; set; }
        decimal Day30ChangePercent { get; set; }
    }

    public class KeyStats : IKeyStats
    {
        public string CompanyName { get; set; }
        public decimal MarketCap { get; set; }
        public decimal Week52High { get; set; }
        public decimal Week52Low { get; set; }
        public decimal Week52Change { get; set; }
        public decimal SharesOutstanding { get; set; }
        public decimal Employees { get; set; }
        public decimal Avg30Volume { get; set; }
        public decimal Avg10Volume { get; set; }
        public decimal Float { get; set; }
        public decimal TtmEPS { get; set; }
        public decimal TtmDividendRate { get; set; }
        public decimal DividendYield { get; set; }
        public string NextDividendDate { get; set; }
        public string NextEarningsDate { get; set; }
        public decimal PeRatio { get; set; }
        public decimal Day200MovingAvg { get; set; }
        public decimal Day50MovingAvg { get; set; }
        public decimal MaxChangePercent { get; set; }
        public decimal Year5ChangePercent { get; set; }
        public decimal Year2ChangePercent { get; set; }
        public decimal Year1ChangePercent { get; set; }
        public decimal YtdChangePercent { get; set; }
        public decimal Month6ChangePercent { get; set; }
        public decimal Month3ChangePercent { get; set; }
        public decimal Month1ChangePercent { get; set; }
        public decimal Day5ChangePercent { get; set; }
        public decimal Day30ChangePercent { get; set; }
        
		public override string ToString(){
			return this.ToTableString();
		}
    }
}
