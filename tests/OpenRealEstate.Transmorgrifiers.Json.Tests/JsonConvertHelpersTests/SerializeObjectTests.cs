using OpenRealEstate.Core.Residential;
using OpenRealEstate.FakeData;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Transmorgrifiers.Json.Tests
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
    }
}
