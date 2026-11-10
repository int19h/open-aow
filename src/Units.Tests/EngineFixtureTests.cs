
namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed class EngineFixtureTests
{
    [Fact]
    public void StartupAndTestServicesUseTheSameIsolatedRootAndGameSet()
    {
        Assert.Equal(EngineFixture.Root, Runtime.Files.CurrentDirectory);
        Assert.Equal(Path.Combine(EngineFixture.Root, "AoW.exe"), Runtime.Application.ExeName);
        Assert.Equal(EngineFixture.Root, EngineFixture.Engine.StartupDirectory.TrimEnd('\\', '/'));
        Assert.Same(EngineFixture.GameSet, Aow.Game.Engine.AoWHSSet.Current);
        Assert.Throws<Xunit.Sdk.TrueException>(() => Runtime.Files.FileExists(Path.Combine(Path.GetTempPath(), "outside-fixture")));
    }
}
