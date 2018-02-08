using OpenRealEstate.Core;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Transmorgrifiers.Json.Tests
{
    public class StatusTypeConvertTests
    {
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
