using System.Linq;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.Testing;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Transmorgrifiers.Json.Tests
{
    public class ListingConverterTests : TestHelperUtilities
    {
        [Fact]
        public void GivenSomeBadCasingJson_Parse_ReturnsAListing()
        {
            // Arrange.
            var listing = CreateListings(typeof(ResidentialListing), 1);
            var json = JsonConvertHelpers.SerializeObject(listing);
            json = json.Replace("listingType", "ListingType");
            var transmorgrifier = new JsonTransmorgrifier();

            // Act.
            var result = transmorgrifier.Parse(json);

            // Assert.
            result.Errors.Count.ShouldBe(0);
            result.UnhandledData.Count.ShouldBe(0);
            result.Listings.Count.ShouldBe(1);
            result.TransmorgrifierName.ShouldBe("Json");
        }

        [Fact]
        public void GivenSomeJsonMissingAListingTypeKey_Parse_ReturnsAnError()
        {
            // Arrange.
            var listing = CreateListings(typeof(ResidentialListing), 1);
            var json = JsonConvertHelpers.SerializeObject(listing);
            json = json.Replace("listingType", "ListingTypeXXXX");
            var transmorgrifier = new JsonTransmorgrifier();

            // Act.
            var result = transmorgrifier.Parse(json);

            // Assert.
            result.Errors.Count.ShouldBe(1);

            var error = result.Errors.First();
            error.ExceptionMessage.ShouldStartWith("Failed to find the json-property 'listingType' which is expected");
            error.InvalidData.ShouldNotBeNullOrWhiteSpace();
            error.ListingId.ShouldBeNullOrWhiteSpace();
            error.AgencyId.ShouldBeNullOrWhiteSpace();

            result.UnhandledData.Count.ShouldBe(0);
            result.Listings.Count.ShouldBe(0);
            result.TransmorgrifierName.ShouldBe("Json");
        }
    }
}
