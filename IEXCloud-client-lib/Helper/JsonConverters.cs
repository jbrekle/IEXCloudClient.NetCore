using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IEXCloudClient.NetCore.Helper
{
    internal class UnixDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
                throw new Exception("Wrong Token Type");

            long ticks = (long) reader.Value;
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(ticks);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    internal class YearMonthDayConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                throw new Exception("Wrong Token Type");

            var date = (string) reader.Value;
            return DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    //https://stackoverflow.com/a/12202914
    public class ConcreteTypeConverter<TConcrete> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            //assume we can convert to anything for now
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //explicitly specify the concrete type we want to create
            return serializer.Deserialize<TConcrete>(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //use the default serialization - it works fine
            serializer.Serialize(writer, value);
        }
    }

    internal class SystemEventTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(String);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                throw new Exception("Wrong Token Type");
            var value = (string)reader.Value;
            switch (value)
            {
                case "O":
                    return SystemEventType.StartOfMessages;
                case "S":
                    return SystemEventType.StartOfSystemHours;
                case "R":
                    return SystemEventType.StartOfRegularMarketHours;
                case "M":
                    return SystemEventType.EndOfRegularMarketHours;
                case "E":
                    return SystemEventType.EndOfSystemHours;
                case "C":
                    return SystemEventType.EndOfMessages;
                default: 
                    throw new Exception($"SystemEventType {value} unknown");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }    
    
    internal class FinancialQuarterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(String);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                throw new Exception("Wrong Token Type");
            var value = (string)reader.Value;
            var splitted = value.Split(' ');
            var year = int.Parse(splitted[1]);
            var quarter = int.Parse(splitted[0][1].ToString());
            return new FinancialQuarter(){Quarter = quarter, Year = year};
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}