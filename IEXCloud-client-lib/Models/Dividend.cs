using System;
using System.ComponentModel;
using System.Diagnostics;
using IEXCloudClient.NetCore.Helper;

namespace IEXCloudClient.NetCore
{
    public interface IDividend
    {
        DateTime ExDate { get; set; }
        DateTime PaymentDate { get; set; }
        DateTime RecordDate { get; set; }
        DateTime DeclaredDate { get; set; }
        decimal Amount { get; set; }
        string Flag { get; set; }
		string Currency { get; set; }
		string Description { get; set; }
		string Frequency{ get; set; }
    }

	public class Dividend : IDividend
    {
        public DateTime ExDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime DeclaredDate { get; set; }
        public decimal Amount { get; set; }
        public string Flag { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string Frequency { get; set; }

		public override string ToString(){
			return this.ToTableString();
		}
	}
}
