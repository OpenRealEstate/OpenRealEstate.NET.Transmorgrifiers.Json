using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Homely.Testing;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Residential;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Transmorgrifiers.Json.Tests.CustomConvertersTests
{
    public class LandDetailsConverterTests
    {
        [Theory]
        [InlineData("old-frontage-sample.json")]
        [InlineData("old-frontage-different-casing-sample.json")]
        public async Task GivenAValidOldFrontageJson_Deserialize_PopulatesANewListing(string jsonFile)
        {
            // Arrange.
            var json = await File.ReadAllTextAsync($"Sample Data/{jsonFile}");

            var expectedLandDetails = new LandDetails
            {
                Sides = new List<Side>
                {
                    new Side
                    {
                        Name = "Frontage",
                        Type = "Meter",
                        Value = 2m
                    }
                },
            };

            // Act.
            var listing = JsonConvertHelpers.DeserializeObject<ResidentialListing>(json);

            // Assert.
            listing.ShouldNotBeNull();
            listing.LandDetails.ShouldLookLike(expectedLandDetails);
        }

        [Theory]
        //[InlineData("old-land-details-sample.json")]
        [InlineData("old-land-details-different-casing-sample.json")]
        public async Task GivenAValidOldLandDetailsJson_Deserialize_PopulatesANewListing(string jsonFile)
        {
            // Arrange.
            var json = await File.ReadAllTextAsync($"Sample Data/{jsonFile}");

            var expectedLandDetails = new LandDetails
            {
                Area = new UnitOfMeasure
                {
                    Type = "Square Meter",
                    Value = 400
                },
                Sides = new List<Side>
                {
                    new Side
                    {
                        Name = "Frontage",
                        Type = "Meter",
                        Value = 123m
                    },
                    new Side
                    {
                        Name = "left",
                        Type = "m2",
                        Value = 10.1m
                    },
                    new Side
                    {
                        Name = "right",
                        Type = "m2",
                        Value = 20.2m
                    }
                },
                CrossOver = "RightThenLeft"
            };

            // Act.
            var listing = JsonConvertHelpers.DeserializeObject<ResidentialListing>(json);

            // Assert.
            listing.ShouldNotBeNull();
            listing.LandDetails.ShouldLookLike(expectedLandDetails);
        }
    }
}
