using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenRealEstate.Core;

namespace OpenRealEstate.Transmorgrifiers.Json
{
    public static class JsonConvertHelpers
    {
        /// <summary>
        /// Some common Json settings for serialization.
        /// The emphasis here is on 'READABILITY' -> making the json look pretty (instead of a minimal file size).
        /// </summary>
        internal static JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings
        {
            Converters = new JsonConverter[]
            {
                
                new StringEnumConverter(),
                new ListingConverter()
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

        public static Listing DeserializeObject(string json)
        {
            return JsonConvert.DeserializeObject<Listing>(json, JsonSerializerSettings);
        }

        public static IEnumerable<Listing> DeserializeObjects(string json)
        {
            return JsonConvert.DeserializeObject<IEnumerable<Listing>>(json, JsonSerializerSettings);
        }
    }
}