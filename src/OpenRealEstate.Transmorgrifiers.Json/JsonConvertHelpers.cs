using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OpenRealEstate.Core;
using OpenRealEstate.Transmorgrifiers.Json.CustomConverters;

namespace OpenRealEstate.Transmorgrifiers.Json
{
    public static class JsonConvertHelpers
    {
        /// <summary>
        /// Some common Json settings for serialization.
        /// The emphasis here is on 'READABILITY' -> making the json look pretty (instead of a minimal file size).
        /// </summary>
        /// <remarks>- Camel-cased property names. i.e. user or firstName or pewPew.<br/>
        ///          - Formatting is indented.<br/>
        ///          - Requires a property "listingType" : Residential | Rental | Land | Rural
        ///          - Can auto parse/convert the REA 'StatusType' to the ORE StatusType.</remarks>
        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(), // Lowercase, first character.
            Converters = new JsonConverter[]
            {
                new ListingConverter(),
                new StatusTypeConverter(),
                new StringEnumConverter(),
                new LandDetailsConverter()
            },
            Formatting = Formatting.Indented
        };

        public static string SerializeObject(this Listing listing)
        {
            return JsonConvert.SerializeObject(listing, JsonSerializerSettings);
        }

        public static string SerializeObject(this IEnumerable<Listing> listings)
        {
            return JsonConvert.SerializeObject(listings, JsonSerializerSettings);
        }

        public static void SerializeObject(this IEnumerable<Listing> listings, TextWriter textWriter)
        {
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                var serializer = JsonSerializer.Create(JsonSerializerSettings);

                serializer.Serialize(jsonWriter, listings);
            }
        }

        public static Listing DeserializeObject(string json)
        {
            return DeserializeObject<Listing>(json);
        }

        public static T DeserializeObject<T>(string json) where T : Listing
        {
            return JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings);
        }

        public static IEnumerable<Listing> DeserializeObjects(string json)
        {
            return DeserializeObjects<Listing>(json);
        }

        public static IEnumerable<T> DeserializeObjects<T>(string json) where T : Listing
        {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(json, JsonSerializerSettings);
        }
    }
}
