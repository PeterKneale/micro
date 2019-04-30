using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Handlers;
using Micro.Services.Tenants.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Micro.Services.Tenants.UnitTests.Handlers
{
    public class CreateTenantHandlerTests : IClassFixture<DatabaseContextFixture>, IClassFixture<AutoMapperFixture>
    {
        private readonly DatabaseContextFixture _db;
        private readonly AutoMapperFixture _map;
        private readonly CreateTenantCommand _cmd;
        private readonly CancellationToken _cancel;

        public CreateTenantHandlerTests(DatabaseContextFixture db, AutoMapperFixture map)
        {
            _db = db;
            _map = map;
            _cmd = new Fixture().Create<CreateTenantCommand>();
            _cancel = new CancellationTokenSource().Token;
        }

        [Fact]
        public async Task Verify_handler_returns_model()
        {
            // Arrange
            var handler = new CreateTenantHandler(_db.DatabaseContext, _map.Mapper);

            // act
            var result = await handler.Handle(_cmd, _cancel);

            // assert
            result.Tenant.Should().NotBeNull("tenant should be returned");
            result.Tenant.Name.Should().Be(_cmd.Name, "tenant name should match that on command");
            result.Tenant.Id.Should().BeGreaterThan(0, "tenant id should be created");
        }

        [Fact]
        public async Task Verify_handler_writes_to_db()
        {
            // Arrange
            var handler = new CreateTenantHandler(_db.DatabaseContext, _map.Mapper);

            // act
            var result = await handler.Handle(_cmd, _cancel);

            // assert
            var tenant = await _db.VerificationDatabaseContext.Tenants.SingleOrDefaultAsync(x => x.Id == result.Tenant.Id);
            tenant.Should().NotBeNull("tenant should be in database");
            tenant.Id.Should().BeGreaterThan(0, "primary key be generated");
            tenant.Name.Should().Be(result.Tenant.Name, "tenant name should match");
        }

        [Fact]
        public async Task Verify_tenant_names_must_be_unique()
        {
            // Arrange
            var handler = new CreateTenantHandler(_db.DatabaseContext, _map.Mapper);

            // act
            await handler.Handle(_cmd, _cancel);
            Func<Task> task = async () => { await handler.Handle(_cmd, _cancel); };

            // assert
            task.Should().Throw<NotUniqueException>("tenant name is already in use");
        }
    }
}
