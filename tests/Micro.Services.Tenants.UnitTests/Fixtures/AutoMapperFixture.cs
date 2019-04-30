using System;
using AutoMapper;

namespace Micro.Services.Tenants.UnitTests.Fixtures
{
    public class AutoMapperFixture : IDisposable
    {
        public AutoMapperFixture()
        {
            Mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Startup).Assembly);
            }).CreateMapper();
        }

        public IMapper Mapper { get; }

        public void Dispose()
        {
            
        }
    }
}
