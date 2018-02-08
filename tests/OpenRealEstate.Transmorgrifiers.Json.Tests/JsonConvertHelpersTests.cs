using OpenRealEstate.Core;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.FakeData;
using Shouldly;
using System.Linq;
using Xunit;

namespace OpenRealEstate.Transmorgrifiers.Json.Tests
{
    public class JsonConvertHelpersTests
    {
        public class SerializeObjectTests
        {
            [Fact]
            public void GivenAListing_SerializeObject_ReturnsSomeJson()
            {
                // Arrange.
                var listings = FakeListings.CreateAFakeListing<ResidentialListing>();

                // Act.
                var json = listings.SerializeObject();
                
                // Assert.
                json.ShouldStartWith("{");
                json.ShouldEndWith("}");
                json.Length.ShouldBeGreaterThan(2);
            }

            [Fact]
            public void GivenSomeListings_SerializeObject_ReturnsSomeJson()
            {
                // Arrange.
                var listings = FakeListings.CreateFakeListings<ResidentialListing>();

                // Act.
                var json = listings.SerializeObject();

                // Assert.
                json.ShouldStartWith("[");
                json.ShouldEndWith("]");
                json.Length.ShouldBeGreaterThan(2);
            }

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
        }
    }
}