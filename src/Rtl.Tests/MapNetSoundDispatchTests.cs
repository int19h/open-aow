using Aow.Platform;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.Sound;
using SoundApi = Aow.Units.Sound.Sound;
using Sample = Aow.Units.Sound.TSample;

namespace Aow.Rtl.Tests;

public sealed class MapNetSoundDispatchTests
{
    private sealed class ConvertedField(TECustomNode owner) : TMapField(owner)
    {
        internal int OwnerCalls;
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class LegacyField : TMapField
    {
        internal int OwnerCalls;
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ConvertedMap(TECustomNode owner) : THSMap(owner)
    {
        internal int MouseCalls;
        public override THSMapMouse CreateMouse()
        {
            Assert.Null(FSelection);
            Assert.Null(FTimer);
            MouseCalls++;
            return new THSMapMouse();
        }
    }

    private sealed class LegacyMap : THSMap
    {
        internal int MouseCalls;
        public override THSMapMouse CreateMouse()
        {
            Assert.Null(FSelection);
            Assert.Null(FTimer);
            MouseCalls++;
            return new THSMapMouse();
        }
    }

    private sealed class SpecializedField(TECustomNode owner) : TMapField(owner);
    private sealed class SpecializedLevel : TMapLevel
    {
        private SpecializedLevel(ConstructionContext context, TECustomNode owner) : base(context, owner) { }
        internal static new SpecializedLevel Create(TECustomNode owner) =>
            ConstructionContext.Create(owner, static (context, value) => new SpecializedLevel(context, value));
        public override TEObject.Meta MapFieldClass() => TEObject.Meta.Of<SpecializedField>();
    }

    static MapNetSoundDispatchTests()
    {
        EObjectCreation.Register(typeof(SpecializedField), new TMapField.Meta(), static owner => new SpecializedField(owner), 40);
        EObjectCreation.Register(typeof(SpecializedLevel), new TMapLevel.Meta(), static owner => SpecializedLevel.Create(owner), 124);
    }

    [Fact]
    public void SemanticAndLegacyFieldBodiesRunOnce()
    {
        var converted = new ConvertedField(null!);
        var legacy = new LegacyField();
        legacy.Create__init(null!);
        Assert.Equal(1, converted.OwnerCalls);
        Assert.Equal(1, legacy.OwnerCalls);
        Assert.Equal(3, converted.FCapacity);
        Assert.Equal(3, legacy.FCapacity);
        Assert.Equal(-1, converted.TerrainType);
        Assert.Equal(-1, legacy.TerrainType);
        converted.Destroy();
        legacy.Destroy();
    }

    [Fact]
    public void BothMapRoutesKeepVirtualMouseCreationAtTheSamePosition()
    {
        var converted = new ConvertedMap(null!);
        var legacy = new LegacyMap();
        legacy.Create__init(null!);
        Assert.Equal(1, converted.MouseCalls);
        Assert.Equal(1, legacy.MouseCalls);
        Assert.Same(converted, converted.Mouse.FHSMap);
        Assert.Same(legacy, legacy.Mouse.FHSMap);
        Assert.Equal(converted.Count, legacy.Count);
        converted.Destroy();
        legacy.Destroy();
    }

    [Fact]
    public void DynamicFieldsKeepTheirExactTypeSizeAndCoordinates()
    {
        var level = SpecializedLevel.Create(null!);
        level.Lhx = 2;
        level.CreateMapFieldList(3, 2);
        Assert.Equal(40, level.FieldSize);
        Assert.Equal(6, level.Fields.Length);
        for (int y = 0; y < 2; y++)
            for (int x = 0; x < 3; x++)
            {
                var field = Assert.IsType<SpecializedField>(level.GetField(x, y));
                Assert.Same(level, field.Owner);
                Assert.Equal(x, field.Xhx);
                Assert.Equal(y, field.Yhx);
                Assert.Equal(2, field.Lhx);
            }
        level.Destroy();
    }

    [Fact]
    public void LegacyMouseAllocatesEachBaseEventListOnce()
    {
        var mouse = Aow.Units.AoWE.TAbstractAoWHSMapMouse.Create();
        Assert.Equal(mouse.InstanceId + 1, mouse.FOnUp.InstanceId);
        Assert.Equal(mouse.InstanceId + 5, mouse.FOnCaptureChanged.InstanceId);
        Assert.Equal(int.MinValue, mouse.FX);
        Assert.Equal(int.MinValue, mouse.FY);
        mouse.Destroy();
    }

    private sealed class SoundState : IDisposable
    {
        private readonly TSampleList samples = SoundApi.SampleList;
        private readonly TSampleDataList data = SoundApi.SampleDataList;
        private readonly TSoundSystemList systems = SoundApi.SoundSystems;
        private readonly TSoundSystem system = SoundApi.SoundSystem;
        private readonly TSongPlayer songs = SoundApi.SongPlayer;
        internal SoundState()
        {
            SoundApi.SampleList = new TSampleList();
            SoundApi.SampleDataList = new TSampleDataList();
            SoundApi.SoundSystems = new TSoundSystemList();
            SoundApi.SoundSystem = null!;
            SoundApi.SongPlayer = null!;
            TSdlSoundSystem.Install(mute: true, audioOutput: false);
        }
        public void Dispose()
        {
            SoundApi.ReleaseSoundSystem();
            SoundApi.SampleList = samples;
            SoundApi.SampleDataList = data;
            SoundApi.SoundSystems = systems;
            SoundApi.SoundSystem = system;
            SoundApi.SongPlayer = songs;
        }
    }

    private static TWaveFormatEx Format => new()
    {
        wFormatTag = 1, nChannels = 1, nSamplesPerSec = 8000, nBlockAlign = 1, wBitsPerSample = 8
    };

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SoundDataRoutesKeepTypeCacheAndReferenceCounts(bool memory)
    {
        using var state = new SoundState();
        var meta = TSampleData.Meta.Of<TSampleData>();
        var direct = memory ? TSampleData.CreateEx(Format, new byte[] { 128 }, 1) : TSampleData.Create("direct");
        var dynamic = memory ? meta.CreateEx(Format, new byte[] { 128 }, 1) : meta.Create("dynamic");
        foreach (var value in new[] { direct, dynamic })
        {
            Assert.IsType<TSampleData>(value);
            Assert.Equal(1, value.RefCount);
            Assert.Equal(memory ? "" : value == direct ? "direct" : "dynamic", value.Name.ToString());
            if (!memory) Assert.Same(value, SoundApi.SampleDataList.Find(value.Name));
        }
        Assert.Equal(memory ? 0 : 2, SoundApi.SampleDataList.Count);
        var sample = Sample.Create(direct);
        var dynamicSample = Sample.Meta.Of<Sample>().Create(dynamic);
        Assert.IsType<Sample>(sample);
        Assert.IsType<Sample>(dynamicSample);
        Assert.Equal(2, direct.RefCount);
        Assert.Equal(2, dynamic.RefCount);
        Assert.Equal(2, SoundApi.SampleList.Count);
        Assert.Equal(0, sample.Delay);
        Assert.Equal(default, sample.Status);
        sample.Destroy();
        dynamicSample.Destroy();
        Assert.Equal(1, direct.RefCount);
        Assert.Equal(1, dynamic.RefCount);
        direct.Release();
        dynamic.Release();
        Assert.Equal(0, SoundApi.SampleList.Count);
        Assert.Equal(0, SoundApi.SampleDataList.Count);
    }

    [Theory]
    [InlineData("SDL")]
    [InlineData("Galaxy")]
    public void SdlFacadesCreateExactSilentTypes(string name)
    {
        using var state = new SoundState();
        var meta = SoundApi.SoundSystems.Find(name);
        var system = meta.Create();
        Assert.Equal(name == "SDL" ? "TSdlSoundSystem" : "GalaxyAlias", system.GetType().Name);
        Assert.Equal(1, system.FRefCount);
        system.Destroy();
        Assert.True(SoundApi.InitializeSoundSystem(name,
            new TSoundSettings { SampleRate = 8000, SampleVoices = 1, SFXEnabled = true }));
        var sampleDataMeta = (TSampleData.Meta)MetaRegistry.Of(typeof(TSdlSoundSystem).Assembly.GetType("Aow.Platform.SdlSampleData")!);
        var data = sampleDataMeta.CreateEx(Format, new byte[] { 128 }, 1);
        Assert.Equal("SdlSampleData", data.GetType().Name);
        Assert.Equal(1, data.RefCount);
        Assert.Equal(0, SoundApi.SampleDataList.Count);
        var sampleMeta = (Sample.Meta)MetaRegistry.Of(typeof(TSdlSoundSystem).Assembly.GetType("Aow.Platform.SdlSample")!);
        var sample = sampleMeta.Create(data);
        Assert.Equal("SdlSample", sample.GetType().Name);
        Assert.Equal(-1, sample.Volume);
        Assert.False(sample.Playing);
        Assert.False(sample.Looped);
        Assert.Equal(TSamplePanning.spCenter, sample.Panning);
        Assert.Equal(2, data.RefCount);
        Assert.Equal(1, SoundApi.SampleList.Count);
        sample.Destroy();
        Assert.Equal(1, data.RefCount);
        data.Release();
        Assert.Equal(0, SoundApi.SampleList.Count);
    }

    [Fact]
    public void InvalidMemoryWaveLeavesBothListsUnchanged()
    {
        using var state = new SoundState();
        var meta = (TSampleData.Meta)MetaRegistry.Of(typeof(TSdlSoundSystem).Assembly.GetType("Aow.Platform.SdlSampleData")!);
        Assert.Throws<InvalidDataException>(() => meta.CreateEx(default, new byte[1], 1));
        Assert.Equal(0, SoundApi.SampleList.Count);
        Assert.Equal(0, SoundApi.SampleDataList.Count);
    }

    [Fact]
    public void NullBackendKeepsCompletedSampleCreation()
    {
        using var state = new SoundState();
        SoundApi.SoundSystems.Clear();
        NullSoundSystem.Install();
        var system = TSoundSystem.Meta.Of<NullSoundSystem>().Create();
        Assert.IsType<NullSoundSystem>(system);
        Assert.Equal(1, system.FRefCount);
        var data = TSampleData.Create("silent");
        var sample = ((Sample.Meta)MetaRegistry.Of(typeof(NullSoundSystem).Assembly.GetType("Aow.Units.Sound.NullSample")!)).Create(data);
        Assert.Equal("NullSample", sample.GetType().Name);
        Assert.Equal(2, data.RefCount);
        Assert.False(sample.Playing);
        sample.Destroy();
        Assert.Equal(1, data.RefCount);
        data.Release();
        system.Destroy();
    }
}
