using Microsoft.Extensions.Logging;

namespace Aow.Game.Tests;

public sealed class LoggerIsolationTests
{
    [Fact]
    public void RecordingHostRestoresTheGlobalFactoryInsideAnIsolationScope()
    {
        ILoggerFactory previous = RtlHost.GlobalLoggerFactory;
        using ILoggerFactory isolated = LoggerFactory.Create(builder => { });
        using (RtlHost.IsolateLoggerFactory(isolated))
        {
            using EngineFixture.RecordingHost host = new("fixture");
            RtlHost.InstallHost(host);
        }

        Assert.Same(previous, RtlHost.GlobalLoggerFactory);
        Assert.Same(previous, RtlHost.LoggerFactory);
    }
}
