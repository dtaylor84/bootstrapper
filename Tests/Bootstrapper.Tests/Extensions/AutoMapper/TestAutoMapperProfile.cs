﻿using AutoMapper;
using Bootstrap.Tests.Extensions.TestImplementations;

namespace Bootstrap.Tests.Extensions.AutoMapper
{
    public class TestAutoMapperProfile: Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<ITestInterface, TestImplementation>();
        }
    }
}
