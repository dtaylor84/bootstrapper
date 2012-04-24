using System;
using Castle.MicroKernel;

namespace Bootstrap.Windsor
{
    public static class Facilities
    {
        public static IFacilityOptions Include<TFacility>() where TFacility : IFacility, new()
        {
            return new FacilityOptions().And<TFacility>();
        }

        public static IFacilityOptions Include<TFacility>(Action<TFacility> onCreate)
            where TFacility : IFacility, new()
        {
            return new FacilityOptions().And(onCreate);
        }
    }
}