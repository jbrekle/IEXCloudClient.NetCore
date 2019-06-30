using System.Collections.Generic;
using IEXCloudClient.NetCore.Helper;

namespace IEXCloudClient.NetCore
{
	public interface ICompany {
		string Symbol { get; set; }
        string CompanyName { get; set; }
        string Exchange { get; set; }
        string Industry { get; set; }
        string Website { get; set; }
        string Description { get; set; }
        string CEO { get; set; }
        string IssueType { get; set; }
        string Sector { get; set; }
        string SecurityName { get; set; }
        List<string> Tags { get; set; }
	}

	public class Company : ICompany {
		public Company()
		{
			this.Tags = new List<string>();
		}

        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public string Exchange { get; set; }
        public string Industry { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string CEO { get; set; }
        public string IssueType { get; set; }
        public string Sector { get; set; }
        public string SecurityName { get; set; }
        public List<string> Tags { get; set; }
		
		public override string ToString(){
			return this.ToTableString();
		}
    }

	public interface ICompanyFull : ICompany {
        List<IBalanceSheet> BalanceSheets { get; set; }
        List<IDividend> Dividends { get; set; }
        IQuote Quote { get; set; }
        List<IEarnings> Earnings { get; set; }
	}

    public class CompanyFull : Company, ICompanyFull
    {
        public List<IBalanceSheet> BalanceSheets { get; set; }
        public List<IDividend> Dividends { get; set; }
        public IQuote Quote { get; set; }
        public List<IEarnings> Earnings { get; set; }
		
		public override string ToString(){
			return this.ToTableString();
		}
    }
}
