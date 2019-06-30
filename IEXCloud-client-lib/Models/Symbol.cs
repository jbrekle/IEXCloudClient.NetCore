using System;
using IEXCloudClient.NetCore.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IEXCloudClient.NetCore
{
    public interface ISymbol
    {  
        string Symbol {get; set;}
        string Name {get; set;}
        DateTime Date {get; set;}
        CommonIssueType Type {get; set;}
        string IexId {get; set;}
        string Region {get; set;}
        string Currency {get; set;}
        bool IsEnabled {get; set;}
    }

    public class SymbolImpl : ISymbol
    {  
        public string Symbol {get; set;}
        public string Name {get; set;}
        public DateTime Date {get; set;}

        [JsonConverter(typeof(StringEnumConverter))]
        public CommonIssueType Type {get; set;}
        public string IexId {get; set;}
        public string Region {get; set;}
        public string Currency {get; set;}
        public bool IsEnabled {get; set;}

		public override string ToString(){
			return this.ToTableString();
		}
    }
}