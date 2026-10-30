using Game = Aow.Units.AoWE.AoWE;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed class EngineFixtureTests
{
    [Fact]
    public void StartupAndTestServicesUseTheSameIsolatedRootAndGameSet()
    {
        Assert.Equal(EngineFixture.Root, Runtime.Files.CurrentDirectory);
        Assert.Equal(Path.Combine(EngineFixture.Root, "AoW.exe"), Runtime.Application.ExeName.ToString());
        Assert.Equal(EngineFixture.Root, EngineFixture.Engine.StartupDirectory.ToString().TrimEnd('\\', '/'));
        Assert.Same(EngineFixture.GameSet, Game.AoWHSSet);
        Assert.Throws<Xunit.Sdk.TrueException>(() => Runtime.Files.FileExists(Path.Combine(Path.GetTempPath(), "outside-fixture")));
    }
}
