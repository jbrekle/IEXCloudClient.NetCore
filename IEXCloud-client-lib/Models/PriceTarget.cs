using System;
using IEXCloudClient.NetCore.Helper;

namespace IEXCloudClient.NetCore
{
    public interface IPriceTarget
    {
        string Symbol { get; set; }
        DateTime UpdatedDate { get; set; }
        decimal PriceTargetAverage { get; set; }
        decimal PriceTargetHigh { get; set; }
        decimal PriceTargetLow { get; set; }
        decimal NumberOfAnalysts { get; set; }
    }

    public class PriceTarget : IPriceTarget
    {
        public string Symbol { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal PriceTargetAverage { get; set; }
        public decimal PriceTargetHigh { get; set; }
        public decimal PriceTargetLow { get; set; }
        public decimal NumberOfAnalysts { get; set; }

		public override string ToString(){
			return this.ToTableString();
		}
    }

}
