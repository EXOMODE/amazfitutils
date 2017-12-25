using System;
using Newtonsoft.Json;

namespace WatchFace.Parser.JsonConverters
{
    public class DrawingOrderJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value:X}");
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(uint) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var str = (string) reader.Value;
            if (str == null)
                throw new JsonSerializationException();
            return Convert.ToInt64(str, 16);
        }
    }
}