using System;
using IEXCloudClient.NetCore.Helper;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore
{
    public interface IApiStatus
    {
        string Status { get; set; }
        bool StatusUp { get; }
        string Version { get; set; }
        DateTime Time { get; set; }
    }
    
    public class ApiStatus : IApiStatus
    {
        public string Status { get; set; }

        [JsonIgnore]
        public bool StatusUp {
            get { return Status == "up"; } 
        }

        public string Version { get; set; }

        [JsonConverter(typeof (UnixDateTimeConverter))]
        public DateTime Time { get; set; }

		public override string ToString(){
			return this.ToTableString();
		}
    }
}