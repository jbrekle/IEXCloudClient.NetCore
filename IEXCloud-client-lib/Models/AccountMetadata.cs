using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public interface IAccountMetadata
{
    bool PayAsYouGoEnabled {get; set;}

    DateTime EffectiveDate {get; set;}

    DateTime EndDateEffective {get; set;}

    SubscriptionTermType SubscriptionTermType {get; set;}

    Tier Tier {get; set;}
    long MessageLimit {get; set;}
    long MessagesUsed {get; set;}
    long CircuitBreaker {get; set;}
}

public class AccountMetadata : IAccountMetadata
{
    public bool PayAsYouGoEnabled {get; set;}

    [JsonConverter(typeof (UnixDateTimeConverter))]
    public DateTime EffectiveDate {get; set;}

    [JsonConverter(typeof (UnixDateTimeConverter))]
    public DateTime EndDateEffective {get; set;}
    
    [JsonConverter(typeof(StringEnumConverter))]
    public SubscriptionTermType SubscriptionTermType {get; set;}

    [JsonProperty("tierName")]
    [JsonConverter(typeof(StringEnumConverter))]
    public Tier Tier {get; set;}
    public long MessageLimit {get; set;}
    public long MessagesUsed {get; set;}
    public long CircuitBreaker {get; set;}
}