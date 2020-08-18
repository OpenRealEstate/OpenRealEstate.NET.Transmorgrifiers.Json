using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenRealEstate.Core;

namespace OpenRealEstate.Transmorgrifiers.Json.CustomConverters
{
    /*
     *
       Old format:

       "landDetails": {
           "area": {
               "type": "Square Meter",
               "value": 400
           },
           "frontage": {
               "type": "Meter",
               "value": 0
           },
           "depths": [],
           "crossOver": null
       },

       
       Current format:

        "landDetails": {
            "area": {
              "type": "square",
              "value": 80.0
            },
            "sides": [
              {
                "name": "frontage",
                "length": {
                  "type": "meter",
                  "value": 20.0
                }
              },
              {
                "name": "rear",
                "length": {
                 "type": "meter",
                 "value": 40.0
               }
             },
             {
               "name": "left",
               "length": {
                 "type": "meter",
                 "value": 60.0
               }
             },
             {
               "name": "right",
               "length": {
                 "type": "meter",
                 "value": 20.0
               }
             }
           ],
           "crossOver": "left"
        },
     */

    /// <summary>
    /// This parses the old and current serialized formats.
    /// </code>
    /// </summary>
    public class LandDetailsConverter : JsonConverter<LandDetails>
    {

        private static readonly string[] OldPropertyNames = new[]
        {
            "frontage",
            "rear",
            "left",
            "right"
        };

        public override LandDetails ReadJson(JsonReader reader, 
                                             Type objectType, 
                                             LandDetails existingValue, 
                                             bool hasExistingValue, 
                                             JsonSerializer serializer)
        {
            // Unchanged: "area" and "crossOver".
            // Removed: "frontage", "rear", "left" and "right"
            // Added: "sides[]"

            var items = JToken.ReadFrom(reader);
            if (!items.HasValues)
            {
                return null;
            }

            var keyValues = items.Children<JProperty>();

            var isOldFormat = keyValues.Select(keyValue => keyValue.Name)
                .Intersect(OldPropertyNames, StringComparer.OrdinalIgnoreCase)
                .Any();

            LandDetails landDetails = null;

            if (isOldFormat)
            {
                var landDetailsV1 = items.ToObject<LandDetailsV1>();
                if (landDetailsV1 != null)
                {
                    landDetails = ToLandDetails(landDetailsV1);
                }
            }
            else
            {
                landDetails = new LandDetails();
                serializer.Populate(items.CreateReader(), landDetails);
            }

            return landDetails;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, 
                                       LandDetails value, 
                                       JsonSerializer serializer)
        {
            var landDeails = JObject.FromObject(value);
            landDeails.WriteTo(writer);
        }

        private LandDetails ToLandDetails(LandDetailsV1 landDetailsV1)
        {
            if (landDetailsV1 is null)
            {
                throw new ArgumentNullException(nameof(landDetailsV1));
            }

            var landDetails = new LandDetails();

            if (landDetailsV1.Area != null)
            {
                landDetails.Area = new UnitOfMeasure
                {
                    Type = landDetailsV1.Area.Type,
                    Value = landDetailsV1.Area.Value
                };
            }

            if (landDetailsV1.Frontage?.Value > 0)
            {
                var frontage = new Side
                {
                    Name = "frontage",
                    Type = "meters",
                    Value = landDetailsV1.Frontage.Value
                };

                landDetails.Sides.Add(frontage);
            }

            if (landDetailsV1.Depths?.Any() == true)
            {
                var depths = from depth in landDetailsV1.Depths
                             where depth.Value > 0
                             select new Side
                             {
                                 Name = depth.Side,
                                 Type = depth.Type,
                                 Value = depth.Value
                             };

                foreach (var depth in depths)
                {
                    landDetails.Sides.Add(depth);
                }
            }

            landDetails.CrossOver = landDetailsV1.CrossOver;

            return landDetails;
        }
    }
}
