using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using Effort.Provider;

namespace Sentry.EntityFramework.Tests;

public class SentryCommandInterceptorTests
{
    private class Fixture
    {
        public DbConnection DbConnection { get; }
        public TestDbContext DbContext { get; }
        public IQueryLogger QueryLogger { get; } = Substitute.For<IQueryLogger>();
        public Fixture()
        {
            DbConnection = Effort.DbConnectionFactory.CreateTransient();
            DbContext = new TestDbContext(DbConnection, true);
        }
    }

    private readonly Fixture _fixture = new();

    [Fact]
    public void UseBreadCrumbs_SentryDatabaseLogging_AddsInterceptor()
    {
        var interceptor = SentryDatabaseLogging.UseBreadcrumbs(_fixture.QueryLogger, initOnce: false);
        Assert.NotNull(interceptor);
    }

    [Fact]
    public void NonQueryExecuting_SentryCommandInterceptor_CapturesQuery()
    {
        var expected = new
        {
            Query = "Expected query string"
        };

        var interceptor = SentryDatabaseLogging.UseBreadcrumbs(_fixture.QueryLogger, initOnce: false);

        var command = new EffortCommand
        {
            CommandText = expected.Query
        };

        interceptor.NonQueryExecuting(command, new DbCommandInterceptionContext<int>());
        _fixture.QueryLogger.Received(1).Log(expected.Query, BreadcrumbLevel.Debug);
    }

    [Fact]
    public void NonQueryExecuting_WithException_CapturesQuery()
    {
        var expected = new
        {
            Query = "Expected query string"
        };

        var interceptor = SentryDatabaseLogging.UseBreadcrumbs(_fixture.QueryLogger, initOnce: false);

        var command = new EffortCommand
        {
            CommandText = expected.Query
        };

        interceptor.NonQueryExecuting(command, new DbCommandInterceptionContext<int> { Exception = new Exception() });
        _fixture.QueryLogger.Received(1).Log(expected.Query, BreadcrumbLevel.Error);
    }

    [Fact]
    public void ReaderExecuting_SentryCommandInterceptor_CapturesQuery()
    {
        var expected = new
        {
            Query = "Expected query string"
        };

        var interceptor = SentryDatabaseLogging.UseBreadcrumbs(_fixture.QueryLogger, initOnce: false);

        var command = new EffortCommand
        {
            CommandText = expected.Query
        };

        interceptor.ReaderExecuting(command, new DbCommandInterceptionContext<DbDataReader>());
        _fixture.QueryLogger.Received(1).Log(expected.Query, BreadcrumbLevel.Debug);
    }

    [Fact]
    public void ReaderExecuting_WithException_CapturesQuery()
    {
        var expected = new
        {
            Query = "Expected query string"
        };

        var interceptor = SentryDatabaseLogging.UseBreadcrumbs(_fixture.QueryLogger, initOnce: false);

        var command = new EffortCommand
        {
            CommandText = expected.Query
        };

        interceptor.ReaderExecuting(command, new DbCommandInterceptionContext<DbDataReader> { Exception = new Exception() });
        _fixture.QueryLogger.Received(1).Log(expected.Query, BreadcrumbLevel.Error);
    }

    [Fact]
    public void ScalarExecuting_SentryCommandInterceptor_CapturesQuery()
    {
        var expected = new
        {
            Query = "Expected query string"
        };

        var interceptor = SentryDatabaseLogging.UseBreadcrumbs(_fixture.QueryLogger, initOnce: false);

        var command = new EffortCommand
        {
            CommandText = expected.Query
        };

        interceptor.ScalarExecuting(command, new DbCommandInterceptionContext<object>());
        _fixture.QueryLogger.Received(1).Log(expected.Query, BreadcrumbLevel.Debug);
    }

    [Fact]
    public void ScalarExecuting_WithException_CapturesQuery()
    {
        var expected = new
        {
            Query = "Expected query string"
        };

        var interceptor = SentryDatabaseLogging.UseBreadcrumbs(_fixture.QueryLogger, initOnce: false);

        var command = new EffortCommand
        {
            CommandText = expected.Query
        };

        interceptor.ScalarExecuting(command, new DbCommandInterceptionContext<object> { Exception = new Exception() });
        _fixture.QueryLogger.Received(1).Log(expected.Query, BreadcrumbLevel.Error);
    }

    [Fact]
    public void FirstOrDefault_FromDatabase_CapturesQuery()
    {
        _ = SentryDatabaseLogging.UseBreadcrumbs(_fixture.QueryLogger, initOnce: false);

        _ = _fixture.DbContext.TestTable.FirstOrDefault();

        _fixture.QueryLogger.Received().Log(Arg.Any<string>(), BreadcrumbLevel.Debug);
    }
}
