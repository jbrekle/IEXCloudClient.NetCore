using System;
using IEXCloudClient.NetCore.Helper;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore
{
    public interface ISectorPerformance
    {
        string Type { get; set; }
        string Name { get; set; }
        decimal Performance { get; set; }
        DateTime LastUpdated { get; set; }
    }

    public class SectorPerformance : ISectorPerformance
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public decimal Performance { get; set; }
		
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime LastUpdated { get; set; }

		public override string ToString(){
			return this.ToTableString();
		}
    }
}
