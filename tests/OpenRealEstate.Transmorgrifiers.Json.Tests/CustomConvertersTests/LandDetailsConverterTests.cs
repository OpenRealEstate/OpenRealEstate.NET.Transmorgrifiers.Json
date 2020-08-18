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
        [Fact]
        public async Task GivenAValidOldFrontageJson_Deserialize_PopulatesANewListing()
        {
            // Arrange.
            var json = await File.ReadAllTextAsync("Sample Data/old-frontage-sample.json");

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

        [Fact]
        public async Task GivenAValidOldLandDetailsJson_Deserialize_PopulatesANewListing()
        {
            // Arrange.
            var json = await File.ReadAllTextAsync("Sample Data/old-land-details-sample.json");

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
