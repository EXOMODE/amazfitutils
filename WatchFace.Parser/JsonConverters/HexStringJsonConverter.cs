using System;
using Newtonsoft.Json;

namespace WatchFace.Parser.JsonConverters
{
    public class HexStringJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue($"0x{value:X6}");
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(uint) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var str = (string)reader.Value;
            if (str == null || !str.StartsWith("0x"))
                throw new JsonSerializationException();
            return Convert.ToInt64(str.Substring(2), 16);
        }
    }
}