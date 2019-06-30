
using IEXCloudClient.NetCore.Helper;

namespace IEXCloudClient.NetCore
{
    public interface IVenueVolume
    {
        decimal Volume { get; set; }
        string Venue { get; set; }
        string VenueName { get; set; }
        string Date { get; set; }
        decimal MarketPercent { get; set; }
        decimal AvgMarketPercent { get; set; }
    }

    public class VenueVolume : IVenueVolume
    {
        public decimal Volume { get; set; }
        public string Venue { get; set; }
        public string VenueName { get; set; }
        public string Date { get; set; }
        public decimal MarketPercent { get; set; }
        public decimal AvgMarketPercent { get; set; }

		public override string ToString(){
			return this.ToTableString();
		}
    }
}
