using System.Collections.Generic;
using OpenRealEstate.Core;

namespace OpenRealEstate.Transmorgrifiers.Json.CustomConverters
{
    internal class LandDetailsV1
    {
        public LandDetailsV1()
        {
            Depths = new List<DepthV1>();
        }

        public UnitOfMeasure Area { get; set; }

        public UnitOfMeasure Frontage { get; set; }

        public IList<DepthV1> Depths { get; set; }

        public string CrossOver { get; set; }
    }

    internal class DepthV1 : UnitOfMeasure
    {
        public string Side { get; set; }
    }
}
