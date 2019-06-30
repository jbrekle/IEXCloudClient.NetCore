using System;
using IEXCloudClient.NetCore.Helper;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore
{
    public interface IBalanceSheet
    {
        DateTime ReportDate { get; set; }
        decimal CurrentCash { get; set; }
        decimal ShortTermInvestments { get; set; }
        decimal Receivables { get; set; }
        decimal Inventory { get; set; }
        decimal OtherCurrentAssets { get; set; }
        decimal CurrentAssets { get; set; }
        decimal LongTermInvestments { get; set; }
        decimal PropertyPlantEquipment { get; set; }
        decimal Goodwill { get; set; }
        decimal IntangibleAssets { get; set; }
        decimal OtherAssets { get; set; }
        decimal AccountsPayable { get; set; }
        decimal CurrentLongTermDebt { get; set; }
        decimal OtherCurrentLiabilities { get; set; }
        decimal TotalCurrentLiabilities { get; set; }
        decimal LongTermDebt { get; set; }
        decimal OtherLiabilities { get; set; }
        decimal MinorityInterest { get; set; }
        decimal TotalLiabilities { get; set; }
        decimal Commonstock { get; set; }
        decimal RetainedEarnings { get; set; }
        decimal TreasuryStock { get; set; }
        decimal CapitalSurplus { get; set; }
        decimal ShareholderEquity { get; set; }
        decimal NetTangibleAssets { get; set; }
        decimal TotalAssets { get; set; }
    }

	internal class BalanceSheetResponse {
		public string Symbol { get; set; }

		[JsonProperty("balancesheet")]
		public BalanceSheet[] BalanceSheets { get; set; }
	}

    public class BalanceSheet : IBalanceSheet
    {		
		[JsonProperty("reportDate")]
		public DateTime ReportDate { get; set; }
        public decimal CurrentCash { get; set; }
        public decimal ShortTermInvestments { get; set; }
        public decimal Receivables { get; set; }
        public decimal Inventory { get; set; }
        public decimal OtherCurrentAssets { get; set; }
        public decimal CurrentAssets { get; set; }
        public decimal LongTermInvestments { get; set; }
        public decimal PropertyPlantEquipment { get; set; }
        public decimal Goodwill { get; set; }
        public decimal IntangibleAssets { get; set; }
        public decimal OtherAssets { get; set; }
        public decimal AccountsPayable { get; set; }
        public decimal CurrentLongTermDebt { get; set; }
        public decimal OtherCurrentLiabilities { get; set; }
        public decimal TotalCurrentLiabilities { get; set; }
        public decimal LongTermDebt { get; set; }
        public decimal OtherLiabilities { get; set; }
        public decimal MinorityInterest { get; set; }
        public decimal TotalLiabilities { get; set; }
        public decimal Commonstock { get; set; }
        public decimal RetainedEarnings { get; set; }
        public decimal TreasuryStock { get; set; }
        public decimal CapitalSurplus { get; set; }
        public decimal ShareholderEquity { get; set; }
        public decimal NetTangibleAssets { get; set; }
        public decimal TotalAssets { get; set; }
	
		public override string ToString(){
			return this.ToTableString();
		}
	}
}
