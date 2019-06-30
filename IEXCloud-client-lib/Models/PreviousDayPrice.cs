
using IEXCloudClient.NetCore.Helper;

namespace IEXCloudClient.NetCore
{
    public interface IPreviousDayPrice
    {
        string Symbol { get; set; }
        string Date { get; set; }
        decimal Open { get; set; }
        decimal Close { get; set; }
        decimal High { get; set; }
        decimal Low { get; set; }
        decimal Volume { get; set; }
        decimal UnadjustedVolume { get; set; }
        decimal Change { get; set; }
        decimal ChangePercent { get; set; }
    }

    public class PreviousDayPrice : IPreviousDayPrice
    {
        public string Symbol { get; set; }
        public string Date { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }
        public decimal UnadjustedVolume { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }

		public override string ToString(){
			return this.ToTableString();
		}
    }
}
