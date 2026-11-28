using Aow.Game.Draw.Images;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Second-engine pack ownership (issue 335, decision of 2026-11-24). Disposing a world object
// releases the image libraries of the pack its own engine owns, while the legacy global holds
// another pack. The global pack's libraries keep their reference counts.
[Collection("Engine")]
[EngineState]
public sealed class WorldPackOwnershipTests
{
    private const int ReferenceCount = 2;

    [Fact]
    public void FlagControlReleasesTheFlagsOfItsOwnPack()
    {
        (GamePack globalPack, ImageLibraryManagerObject globalFlagsLibrary) = CreateGlobalPack(fileName: "FLAGS.ILB");
        GameEngine otherEngine = CreateOtherEngine();
        GamePack otherPack = GamePack.Create(null, holder: otherEngine);
        Assert.NotNull(otherPack.ImageLibraryManager);
        ImageLibraryManagerObject otherFlagsLibrary = otherPack.ImageLibraryManager.DiskList.AddImageLibraryFile(fileName: "FLAGS.ILB");
        otherFlagsLibrary.RefCount = ReferenceCount;
        GamePack? previousGamePack = GamePack.Current;
        GamePack.Current = globalPack;
        try
        {
            otherPack.FlagControl.Dispose();
        }
        finally
        {
            GamePack.Current = previousGamePack;
        }

        Assert.Equal(ReferenceCount, globalFlagsLibrary.RefCount);
        Assert.Equal(ReferenceCount - 1, otherFlagsLibrary.RefCount);
    }

    [Fact]
    public void DisposingAPackReleasesItsOwnFlagControlFlags()
    {
        (GamePack globalPack, ImageLibraryManagerObject globalFlagsLibrary) = CreateGlobalPack(fileName: "FLAGS.ILB");
        GameEngine otherEngine = CreateOtherEngine();
        GamePack otherPack = GamePack.Create(null, holder: otherEngine);
        Assert.NotNull(otherPack.ImageLibraryManager);
        ImageLibraryManagerObject otherFlagsLibrary = otherPack.ImageLibraryManager.DiskList.AddImageLibraryFile(fileName: "FLAGS.ILB");
        otherFlagsLibrary.RefCount = ReferenceCount;
        GamePack? previousGamePack = GamePack.Current;
        GamePack.Current = globalPack;
        try
        {
            otherPack.Dispose();
        }
        finally
        {
            GamePack.Current = previousGamePack;
        }

        Assert.Equal(ReferenceCount, globalFlagsLibrary.RefCount);
        Assert.Equal(ReferenceCount - 1, otherFlagsLibrary.RefCount);
    }

    [Fact]
    public void FlagControlDoubleDisposeReleasesItsOwnFlagsOnce()
    {
        (GamePack globalPack, ImageLibraryManagerObject globalFlagsLibrary) = CreateGlobalPack(fileName: "FLAGS.ILB");
        GameEngine otherEngine = CreateOtherEngine();
        GamePack otherPack = GamePack.Create(null, holder: otherEngine);
        Assert.NotNull(otherPack.ImageLibraryManager);
        ImageLibraryManagerObject otherFlagsLibrary = otherPack.ImageLibraryManager.DiskList.AddImageLibraryFile(fileName: "FLAGS.ILB");
        otherFlagsLibrary.RefCount = ReferenceCount;
        GamePack? previousGamePack = GamePack.Current;
        GamePack.Current = globalPack;
        try
        {
            otherPack.FlagControl.Dispose();
            otherPack.FlagControl.Dispose();
        }
        finally
        {
            GamePack.Current = previousGamePack;
        }

        Assert.Equal(ReferenceCount, globalFlagsLibrary.RefCount);
        Assert.Equal(ReferenceCount - 1, otherFlagsLibrary.RefCount);
    }

    [Fact]
    public void DetachedFlagControlDisposeReleasesNothing()
    {
        (GamePack globalPack, ImageLibraryManagerObject globalFlagsLibrary) = CreateGlobalPack(fileName: "FLAGS.ILB");
        GameEngine otherEngine = CreateOtherEngine();
        GamePack otherPack = GamePack.Create(null, holder: otherEngine);
        Assert.NotNull(otherPack.ImageLibraryManager);
        ImageLibraryManagerObject otherFlagsLibrary = otherPack.ImageLibraryManager.DiskList.AddImageLibraryFile(fileName: "FLAGS.ILB");
        otherFlagsLibrary.RefCount = ReferenceCount;
        otherPack.FlagControl.SetOwner(null);
        GamePack? previousGamePack = GamePack.Current;
        GamePack.Current = globalPack;
        try
        {
            otherPack.FlagControl.Dispose();
        }
        finally
        {
            GamePack.Current = previousGamePack;
        }

        Assert.Equal(ReferenceCount, globalFlagsLibrary.RefCount);
        Assert.Equal(ReferenceCount, otherFlagsLibrary.RefCount);
    }

    [Theory]
    [InlineData("GENERAL.ILB")]
    [InlineData("CURSORS.ILB")]
    [InlineData("COMBAT.ILB")]
    public void StrategicMapReleasesTheLibrariesOfItsEnginesPack(string fileName)
    {
        (GamePack globalPack, ImageLibraryManagerObject globalLibrary) = CreateGlobalPack(fileName);
        GameEngine otherEngine = CreateOtherEngine();
        GamePack otherPack = GamePack.Create(null, holder: otherEngine);
        otherEngine.GamePack = otherPack;
        Assert.NotNull(otherPack.ImageLibraryManager);
        ImageLibraryManagerObject otherLibrary = otherPack.ImageLibraryManager.DiskList.AddImageLibraryFile(fileName);
        otherLibrary.RefCount = ReferenceCount;
        DisposeMapWhileGlobal(new StrategicMap(null, holder: otherEngine), globalPack: globalPack);

        Assert.Equal(ReferenceCount, globalLibrary.RefCount);
        Assert.Equal(ReferenceCount - 1, otherLibrary.RefCount);
    }

    [Fact]
    public void StrategicMapOnAnEngineWithoutAPackReleasesNothing()
    {
        (GamePack globalPack, ImageLibraryManagerObject globalLibrary) = CreateGlobalPack(fileName: "GENERAL.ILB");
        GameEngine otherEngine = CreateOtherEngine();
        DisposeMapWhileGlobal(new StrategicMap(null, holder: otherEngine), globalPack: globalPack);

        Assert.Equal(ReferenceCount, globalLibrary.RefCount);
    }

    private static (GamePack GamePack, ImageLibraryManagerObject Library) CreateGlobalPack(string fileName)
    {
        ImageLibraryManager manager = new(null);
        ImageLibraryManagerObject library = manager.DiskList.AddImageLibraryFile(fileName);
        library.RefCount = ReferenceCount;
        return (EngineFixture.CreateGamePack(manager), library);
    }

    private static GameEngine CreateOtherEngine()
        => new(null) { Registry = EngineFixture.Engine.Registry, Dictionary = EngineFixture.Engine.Dictionary };

    private static void DisposeMapWhileGlobal(StrategicMap map, GamePack globalPack)
    {
        GamePack? previousGamePack = GamePack.Current;
        GamePack.Current = globalPack;
        try
        {
            map.Dispose();
        }
        finally
        {
            GamePack.Current = previousGamePack;
        }
    }
}
