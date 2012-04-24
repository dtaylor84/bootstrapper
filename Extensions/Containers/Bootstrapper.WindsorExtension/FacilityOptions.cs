using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel;

namespace Bootstrap.Windsor
{
    public class FacilityOptions : IFacilityOptions
    {
        public ICollection<IFacility> Facilities { get; private set; }

        public FacilityOptions()
        {
            Facilities = new List<IFacility>();
        }

        public IFacilityOptions And<TFacility>() where TFacility : IFacility, new()
        {
            var facility = Activator.CreateInstance<TFacility>();
            Facilities.Add(facility);
            return this;
        }

        public IFacilityOptions And<TFacility>(Action<TFacility> onCreate) where TFacility : IFacility, new()
        {
            var facility = Activator.CreateInstance<TFacility>();
            onCreate(facility);
            Facilities.Add(facility);
            return this;
        }

        public IFacility[] Select()
        {
            return Facilities.ToArray();
        }
    }
}