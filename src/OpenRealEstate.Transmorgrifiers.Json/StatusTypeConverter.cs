using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using OpenRealEstate.Core;
using System;

namespace OpenRealEstate.Transmorgrifiers.Json
{
    public class StatusTypeConverter : StringEnumConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(StatusType);
        }

        public override object ReadJson(JsonReader reader, 
                                        Type objectType, 
                                        object existingValue, 
                                        JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                var isNullable = (Nullable.GetUnderlyingType(objectType) != null);

                if (!isNullable)
                {
                    throw new JsonSerializationException();
                }

                return null;
            }

            var token = JToken.Load(reader);
            var value = token.ToString();
            if (string.Equals(value, "current", StringComparison.OrdinalIgnoreCase))
            {
                return StatusType.Available;
            }
            else if (string.Equals(value, "offmarket", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(value, "withdrawn", StringComparison.OrdinalIgnoreCase))
            {
                return StatusType.Removed;
            }
            else
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }   
        }
    }
}
