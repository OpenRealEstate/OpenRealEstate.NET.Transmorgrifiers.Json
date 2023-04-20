using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OpenRealEstate.Core.Land;
using OpenRealEstate.Core.Rental;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.Core.Rural;
using OpenRealEstate.Testing;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Transmorgrifiers.Json.Tests
{
    public class ParseTests : TestHelperUtilities
    {
        [Theory]
        [InlineData(typeof(ResidentialListing), 1)]
        [InlineData(typeof(ResidentialListing), 3)]
        [InlineData(typeof(RentalListing), 1)]
        [InlineData(typeof(LandListing), 1)]
        [InlineData(typeof(RuralListing), 1)]
        public void GivenSomeValidJson_Parse_ReturnsAListing(Type listingType,
                                                             int listingCount)
        {
            // Arrange.
            var existingListing = CreateListings(listingType, listingCount);
            var json = JsonConvertHelpers.SerializeObject(existingListing);
            var transmorgrifier = new JsonTransmorgrifier();

            // Act.
            var result = transmorgrifier.Parse(json);

            // Assert.
            result.Listings.Count.ShouldBe(listingCount);
            result.UnhandledData.Count.ShouldBe(0);
            result.Errors.Count.ShouldBe(0);

            for (var i = 0; i < result.Listings.Count; i++)
            {
                if (listingType == typeof(ResidentialListing))
                {
                    ResidentialListingAssertHelpers.AssertResidentialListing(
                        (ResidentialListing)result.Listings[i].Listing,
                        (ResidentialListing)existingListing[i]);
                }
                else if (listingType == typeof(RentalListing))
                {
                    RentalListingAssertHelpers.AssertRentalListing(
                        (RentalListing)result.Listings[i].Listing,
                        (RentalListing)existingListing[i]);
                }
                else if (listingType == typeof(LandListing))
                {
                    LandListingAssertHelpers.AssertLandListing(
                        (LandListing)result.Listings[i].Listing,
                        (LandListing)existingListing[i]);
                }
                else if (listingType == typeof(RuralListing))
                {
                    RuralListingAssertHelpers.AssertRuralListing(
                        (RuralListing)result.Listings[i].Listing,
                        (RuralListing)existingListing[i]);
                }
                else
                {
                    throw new Exception($"Failed to assert the suggested type: '{listingType}'.");
                }
            }
        }

        [Fact]
        public async Task GivenSomeValidJsonViaAFile_Parse_ReturnsAListing()
        {
            // Arrange.
            const string file = "Sample Data/rental.json";
            var json = await File.ReadAllTextAsync(file);
            var transmorgrifier = new JsonTransmorgrifier();

            // Act.
            var listingResult = transmorgrifier.Parse(json);

            // Assert.
            listingResult.Errors.ShouldBeEmpty();
            listingResult.Listings.Count.ShouldBe(1);

            var listing = listingResult.Listings.First();
            listing.Warnings.ShouldBeEmpty();  
        }

        [Fact]
        public void GivenSomeIllegalJson_Parse_ReturnsAndError()
        {
            // Arrange.
            const string json = "sadsdf";
            var transmorgrifier = new JsonTransmorgrifier();

            // Act.
            var result = transmorgrifier.Parse(json);

            // Assert.
            result.Listings.Count.ShouldBe(0);
            result.UnhandledData.Count.ShouldBe(0);
            result.Errors.Count.ShouldBe(1);
            result.Errors.First()
                  .ExceptionMessage.ShouldBe(
                      "Unexpected character encountered while parsing value: s. Path '', line 0, position 0.");
            result.Errors.First().InvalidData.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GivenSomeJsonWithAnMissingListingType_Parse_ReturnsAnError()
        {
            // Arrange.
            var existingListing = CreateListings(typeof(ResidentialListing), 1);
            var json = JsonConvertHelpers.SerializeObject(existingListing).Replace("\"Residential\",", "\"blah\",");

            var transmorgrifier = new JsonTransmorgrifier();

            // Act.
            var result = transmorgrifier.Parse(json);

            // Assert.
            result.Listings.Count.ShouldBe(0);
            result.UnhandledData.Count.ShouldBe(0);
            result.Errors.Count.ShouldBe(1);
            result.Errors.First()
                  .ExceptionMessage.ShouldBe(
                      "Invalid value found in the expected json-property 'listingType'. Only the following values (ie. listing types) as supported: residential, rental, land or rural.");
            result.Errors.First().InvalidData.ShouldNotBeNullOrWhiteSpace();
        }

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
