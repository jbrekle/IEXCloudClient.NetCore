using System;
using IEXCloudClient.NetCore.Helper;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore
{
    public interface ISystemEvent
    {
        SystemEventType Type {get; set;}
        DateTime Time {get; set;}
    }

    public class SystemEvent : ISystemEvent
    {
        [JsonConverter(typeof (SystemEventTypeConverter))]
        [JsonProperty("systemEvent")]
        public SystemEventType Type { get; set; }

        [JsonConverter(typeof (UnixDateTimeConverter))]
        [JsonProperty("Timestamp")]
        public DateTime Time { get; set; }
        
		public override string ToString(){
			return this.ToTableString();
		}
    }

    public enum SystemEventType {
        StartOfMessages,
        StartOfSystemHours,
        StartOfRegularMarketHours,
        EndOfRegularMarketHours,
        EndOfSystemHours,
        EndOfMessages
    }
}