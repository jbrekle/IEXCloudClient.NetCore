using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using IEXCloudClient.NetCore.Helper;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore
{
    public interface IEarnings
    {
        decimal ActualEPS { get; set; }
        decimal ConsensusEPS { get; set; }
        string AnnounceTime { get; set; }
        decimal NumberOfEstimates { get; set; }
        decimal EPSSurpriseDollar { get; set; }
        DateTime EPSReportDate { get; set; }
        FinancialQuarter FiscalPeriod { get; set; }
        DateTime FiscalEndDate { get; set; }
        decimal YearAgo { get; set; }
        decimal YearAgoChangePercent { get; set; }
    }

    public class EarningResponse {
        public string Symbol { get; set; }
        public List<Earnings> Earnings { get; set; }
    }

    public struct FinancialQuarter {
        public int Year { get; set; }
        public int Quarter { get; set; }
    }

    public class Earnings : IEarnings
    {
        public decimal ActualEPS { get; set; }
        public decimal ConsensusEPS { get; set; }
        public string AnnounceTime { get; set; }
        public decimal NumberOfEstimates { get; set; }
        public decimal EPSSurpriseDollar { get; set; }
        public DateTime EPSReportDate { get; set; }
        [JsonConverter(typeof(FinancialQuarterConverter))]
        public FinancialQuarter FiscalPeriod { get; set; }
        public DateTime FiscalEndDate { get; set; }
        public decimal YearAgo { get; set; }
        public decimal YearAgoChangePercent { get; set; }

		public override string ToString(){
			return this.ToTableString();
		}
    }
}
