using System;
using System.Collections.Generic;
using Castle.MicroKernel;

namespace Bootstrap.Windsor
{
    public interface IFacilityOptions
    {
        ICollection<IFacility> Facilities { get; }

        IFacilityOptions And<TFacility>() where TFacility : IFacility, new();

        IFacilityOptions And<TFacility>(Action<TFacility> onCreate) where TFacility : IFacility, new();

        IFacility[] Select();
    }
}