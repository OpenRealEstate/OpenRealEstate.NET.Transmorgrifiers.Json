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

        /// <summary>
        /// Length, in meters.
        /// </summary>
        public UnitOfMeasure Frontage { get; set; }

        public IList<DepthV1> Depths { get; set; }

        public string CrossOver { get; set; }
    }

    internal class DepthV1 : UnitOfMeasure
    {
        /// <summary>
        /// Which side is this? Left, Right, Front, etc?
        /// </summary>
        public string Side { get; set; }
    }
}
