using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenRealEstate.Transmorgrifiers.Json
{
    // *** This code was taken from an SO answer about Json Converting + multiple concrete types.
    //     It's worth noting that this is not the ACCEPTED answer, but another answer in the same question/thread.
    //     The accepted answer is ok for most of the time. But the answer I used has some added benefit fixes.
    // REFERENCE: http://stackoverflow.com/a/21632292/30674

    /// <inheritdoc />
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>Create an instance of objectType, based properties in the JSON object</summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        protected abstract T Create(Type objectType, JObject jObject);

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return typeof (T).IsAssignableFrom(objectType);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                        object existingValue,
                                        JsonSerializer serializer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            // Load JObject from stream.
            var jObject = JObject.Load(reader);

            // Create target object based on JObject.
            var target = Create(objectType, jObject);

            // Create a new reader for this jObject, and set all properties to match the original reader.
            var jObjectReader = jObject.CreateReader();
            jObjectReader.Culture = reader.Culture;
            jObjectReader.DateParseHandling = reader.DateParseHandling;
            jObjectReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            jObjectReader.FloatParseHandling = reader.FloatParseHandling;

            // Populate the object properties.
            serializer.Populate(jObjectReader, target);

            return target;
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
