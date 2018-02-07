using System;
using Newtonsoft.Json.Linq;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Land;
using OpenRealEstate.Core.Rental;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.Core.Rural;

namespace OpenRealEstate.Transmorgrifiers.Json
{
    // NOTE: This converter is used mainly for _DESERIALIZATION_ of some json.
    //       So .. given some json .. how do we know which type of concrete
    //       'listing' class should be create?
    //        That is what this class (and specificially, the overrided method) does.

    public class ListingConverter : JsonCreationConverter<Listing>
    {
        
        /// <summary>
        /// This JsonConverter isn't used to serialize a Listing, 
        /// otherwise we get some weird circular reference loop, at the root level of the object model!
        /// </summary>
        public override bool CanWrite => false;

        /// <inheritdoc />
        protected override Listing Create(Type objectType, JObject jObject)
        {
            const string fieldName = "ListingType";

            var value = jObject[fieldName]?.ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception(
                    $"Failed to find the field '{fieldName}' which is expected, so we know which type of Listing instance to deserialize the data into.");
            }

            if (value.Equals("Residential", StringComparison.OrdinalIgnoreCase))
            {
                return new ResidentialListing();
            }

            if (value.Equals("Rental", StringComparison.OrdinalIgnoreCase))
            {
                return new RentalListing();
            }

            if (value.Equals("Land", StringComparison.OrdinalIgnoreCase))
            {
                return new LandListing();
            }

            if (value.Equals("Rural", StringComparison.OrdinalIgnoreCase))
            {
                return new RuralListing();
            }

            throw new Exception(
                $"Invalid value found in the expected field '{fieldName}'. Only the following values (ie. listing types) as supported: residential, rental, land or rural.");
        }
    }
}