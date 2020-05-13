using System.Linq;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.FakeData;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Transmorgrifiers.Json.Tests.JsonConvertHelpersTests
{
    public class DeserializeObjectTests
    {
        [Fact]
        public void GivenSomeJsonOfASingleListing_DeserializeObject_ReturnsAListing()
        {
            // Arrange.
            var listing = FakeListings.CreateAFakeListing<ResidentialListing>();
            var json = listing.SerializeObject();

            // Act.
            var convertedListing = JsonConvertHelpers.DeserializeObject(json);

            // Assert.
            convertedListing.Id.ShouldBe(listing.Id);
        }

        [Fact]
        public void GivenSomeJsonOfASingleListing_DeserializeObjectToResidentiaListing_ReturnsAListing()
        {
            // Arrange.
            var listing = FakeListings.CreateAFakeListing<ResidentialListing>();
            var json = listing.SerializeObject();

            // Act.
            var convertedListing = JsonConvertHelpers.DeserializeObject<ResidentialListing>(json);

            // Assert.
            convertedListing.Id.ShouldBe(listing.Id);
        }

        [Fact]
        public void GivenSomeJsonOfASingleListing_DeserializeObjectToAbstractListing_ReturnsAListing()
        {
            // Arrange.
            var listing = FakeListings.CreateAFakeListing<ResidentialListing>();
            var json = listing.SerializeObject();

            // Act.
            var convertedListing = JsonConvertHelpers.DeserializeObject<Listing>(json);

            // Assert.
            convertedListing.Id.ShouldBe(listing.Id);
        }

        [Fact]
        public void GivenSomeJsonOfAnArrayOfListings_DeserializeObject_ReturnsACollectionOfListings()
        {
            // Arrange.
            var listings = FakeListings.CreateFakeListings<ResidentialListing>();
            var json = listings.SerializeObject();

            // Act.
            var convertedListings = JsonConvertHelpers.DeserializeObjects(json);

            // Assert.
            convertedListings.Count().ShouldBe(listings.Count());
        }

        [Fact]
        public void GivenSomeJsonOfAnArrayOfListings_DeserializeObjectsToResidentialListing_ReturnsACollectionOfListings()
        {
            // Arrange.
            var listings = FakeListings.CreateFakeListings<ResidentialListing>();
            var json = listings.SerializeObject();

            // Act.
            var convertedListings = JsonConvertHelpers.DeserializeObjects<ResidentialListing>(json);

            // Assert.
            convertedListings.Count().ShouldBe(listings.Count());
        }

        [Fact]
        public void GivenSomeJsonOfAnArrayOfListings_DeserializeObjectsToAbstractlListing_ReturnsACollectionOfListings()
        {
            // Arrange.
            var listings = FakeListings.CreateFakeListings<ResidentialListing>();
            var json = listings.SerializeObject();

            // Act.
            var convertedListings = JsonConvertHelpers.DeserializeObjects<Listing>(json);

            // Assert.
            convertedListings.Count().ShouldBe(listings.Count());
        }

        [Theory]
        [InlineData("current", StatusType.Available)]
        [InlineData("withdrawn", StatusType.Removed)]
        [InlineData("offmarket", StatusType.Removed)]
        public void GivenAnOlderStatusType_DeserializeObject_ReturnsAListingWithTheCorrectlyConvertedStatusType(string oldStatusType,
                                                                                                                StatusType expectedStatusType)
        {
            // Arrange.
            var listing = FakeData.FakeListings.CreateAFakeResidentialListing();
            var originalJson = JsonConvertHelpers.SerializeObject(listing);
            var json = originalJson.Replace("Available", oldStatusType);
            json.ShouldContain($"\"StatusType\": \"{oldStatusType}\"");

            // Act.
            var newListing = JsonConvertHelpers.DeserializeObject(json);

            // Assert.
            newListing.StatusType.ShouldBe(expectedStatusType);
        }
    }
}
