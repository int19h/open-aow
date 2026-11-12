using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Audio;

using Aow.Game.World.Map;
using Aow.Platform;
using Sample = Aow.Game.Audio.Sample;

namespace Aow.Game.Tests;

public sealed class MapNetSoundDispatchTests
{
    private sealed class ConvertedField(ECustomNode owner) : MapField(owner)
    {
        private int _ownerCalls;

        public override void SetOwner(ECustomNode owner) { _ownerCalls++; base.SetOwner(owner); }
    }

    private sealed class ConvertedMap(ECustomNode owner) : HSMap(owner)
    {
        internal int MouseCalls { get; private set; }

        public override HexSpriteMapMouse CreateMouse()
        {
            Assert.Null(Selection);
            Assert.Null(Timer);
            MouseCalls++;
            return new HexSpriteMapMouse();
        }
    }

    private sealed class SpecializedField(ECustomNode owner) : MapField(owner);

    private sealed class SpecializedLevel(ECustomNode owner) : MapLevel(owner)
    {
        public override EngineClass MapFieldClass() => EngineClass.Of<SpecializedField>();
    }

    static MapNetSoundDispatchTests()
    {
        EngineClass.Register(MapField.ClassId, static owner => new SpecializedField(owner), 40);
        EngineClass.Register(MapLevel.ClassId, static owner => new SpecializedLevel(owner), 124);
    }

    [Fact]
    public void MapConstructorKeepsVirtualMouseCreationBeforeChildren()
    {
        using var converted = new ConvertedMap(null!);
        Assert.Equal(1, converted.MouseCalls);
        Assert.Same(converted, converted.Mouse.HexSpriteMap);
        Assert.NotNull(converted.Selection);
        Assert.NotNull(converted.Timer);
    }

    [Fact]
    public void DynamicFieldsKeepTheirExactTypeSizeAndCoordinates()
    {
        var level = new SpecializedLevel(null!);
        level.HexLevel = 2;
        level.CreateMapFieldList(3, 2);
        Assert.Equal(40, level.FieldSize);
        Assert.Equal(6, level.Fields.Length);
        for (int y = 0; y < 2; y++)
            for (int x = 0; x < 3; x++)
            {
                var field = Assert.IsType<SpecializedField>(level.GetField(x, y));
                Assert.Same(level, field.Owner);
                Assert.Equal(x, field.HexX);
                Assert.Equal(y, field.HexY);
                Assert.Equal(2, field.HexLevel);
            }
        level.Dispose();
    }

    [Fact]
    public void LegacyMouseStartsWithEmptyEvents()
    {
        var mouse = new GameMapMouse();
        Assert.Equal(0, mouse.OnUpStore.Count);
        Assert.Equal(0, mouse.OnDownStore.Count);
        Assert.Equal(0, mouse.OnMoveStore.Count);
        Assert.Equal(0, mouse.OnDoubleClickStore.Count);
        Assert.Equal(0, mouse.OnCaptureChangedStore.Count);
        Assert.Equal(int.MinValue, mouse.X);
        Assert.Equal(int.MinValue, mouse.Y);
        mouse.Dispose();
    }

    private sealed class SoundState : IDisposable
    {
        private readonly SampleList _samples = AudioHost.SampleList;
        private readonly SampleDataList _data = AudioHost.SampleDataList;
        private readonly SoundSystemList _systems = AudioHost.SoundSystems;
        private readonly SoundSystem _system = AudioHost.SoundSystem;
        private readonly SongPlayer _songs = AudioHost.SongPlayer;

        internal SoundState()
        {
            AudioHost.SampleList = new SampleList();
            AudioHost.SampleDataList = new SampleDataList();
            AudioHost.SoundSystems = new SoundSystemList();
            AudioHost.SoundSystem = null!;
            AudioHost.SongPlayer = null!;
            SdlSoundSystem.Install(mute: true, audioOutput: false);
        }

        public void Dispose()
        {
            AudioPlayback.ReleaseSoundSystem();
            AudioHost.SampleList = _samples;
            AudioHost.SampleDataList = _data;
            AudioHost.SoundSystems = _systems;
            AudioHost.SoundSystem = _system;
            AudioHost.SongPlayer = _songs;
        }
    }

    private static WaveFormatEx Format => new()
    {
        wFormatTag = 1,
        nChannels = 1,
        nSamplesPerSec = 8000,
        nBlockAlign = 1,
        wBitsPerSample = 8
    };

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SoundDataRoutesKeepTypeCacheAndReferenceCounts(bool memory)
    {
        using var state = new SoundState();
        var meta = SampleData.Descriptor;
        var direct = memory ? SampleData.Create(Format, new byte[] { 128 }, 1) : SampleData.Create("direct");
        var dynamic = memory ? meta.CreateInstance(Format, new byte[] { 128 }, 1) : meta.CreateInstance("dynamic");
        foreach (var value in new[] { direct, dynamic })
        {
            Assert.IsType<SampleData>(value);
            Assert.Equal(1, value.RefCount);
            Assert.Equal(memory ? "" : value == direct ? "direct" : "dynamic", value.Name);
            if (!memory) Assert.Same(value, AudioHost.SampleDataList.Find(value.Name));
        }
        Assert.Equal(memory ? 0 : 2, AudioHost.SampleDataList.Count);
        var sample = Sample.Create(direct);
        var dynamicSample = Sample.Descriptor.CreateInstance(dynamic);
        Assert.IsType<Sample>(sample);
        Assert.IsType<Sample>(dynamicSample);
        Assert.Equal(2, direct.RefCount);
        Assert.Equal(2, dynamic.RefCount);
        Assert.Equal(2, AudioHost.SampleList.Count);
        Assert.Equal(0, sample.Delay);
        Assert.Equal(default, sample.Status);
        sample.Dispose();
        dynamicSample.Dispose();
        Assert.Equal(1, direct.RefCount);
        Assert.Equal(1, dynamic.RefCount);
        direct.Release();
        dynamic.Release();
        Assert.Equal(0, AudioHost.SampleList.Count);
        Assert.Equal(0, AudioHost.SampleDataList.Count);
    }

    [Fact]
    public void SoundAliasesKeepDistinctDescriptorsOverOneRuntimeType()
    {
        using var state = new SoundState();
        var sdl = AudioHost.SoundSystems.Find("SDL");
        var galaxy = AudioHost.SoundSystems.Find("Galaxy");
        Assert.NotSame(sdl, galaxy);
        Assert.Equal("SDL", sdl.Name);
        Assert.Equal("Galaxy", galaxy.Name);
        Assert.Same(sdl, SdlSoundSystem.Descriptor);
        Assert.Same(galaxy, SdlSoundSystem.Galaxy);
        Assert.Same(sdl, AudioHost.SoundSystems.Find("sdl"));
        Assert.Same(galaxy, AudioHost.SoundSystems.Find("GALAXY"));
        SdlSoundSystem.Install(mute: true, audioOutput: false);
        Assert.Equal(2, AudioHost.SoundSystems.Count);
        Assert.Same(sdl, AudioHost.SoundSystems.Find("SDL"));
        Assert.Same(galaxy, AudioHost.SoundSystems.Find("Galaxy"));
        var error = Assert.Throws<InvalidOperationException>(() => NullSoundSystem.RegisterSoundSystem(sdl));
        Assert.Equal("Sound System with name: SDL already registered", error.Message);
    }

    [Theory]
    [InlineData("SDL")]
    [InlineData("Galaxy")]
    public void SdlFacadesCreateExactSilentTypes(string name)
    {
        using var state = new SoundState();
        var meta = AudioHost.SoundSystems.Find(name);
        var system = meta.CreateInstance();
        Assert.IsType<SdlSoundSystem>(system);
        Assert.Equal(1, system.RefCount);
        system.Dispose();
        Assert.True(AudioPlayback.InitializeSoundSystem(name,
            new SoundSettings { SampleRate = 8000, SampleVoices = 1, SfxEnabled = true }));
        var sampleDataMeta = SdlSoundSystem.SampleData;
        var data = sampleDataMeta.CreateInstance(Format, new byte[] { 128 }, 1);
        Assert.Equal("SdlSampleData", data.GetType().Name);
        Assert.Equal(1, data.RefCount);
        Assert.Equal(0, AudioHost.SampleDataList.Count);
        var sampleMeta = SdlSoundSystem.Samples;
        var sample = sampleMeta.CreateInstance(data);
        Assert.Equal("SdlSample", sample.GetType().Name);
        Assert.Equal(-1, sample.Volume);
        Assert.False(sample.Playing);
        Assert.False(sample.Looped);
        Assert.Equal(SamplePanning.Center, sample.Panning);
        Assert.Equal(2, data.RefCount);
        Assert.Equal(1, AudioHost.SampleList.Count);
        sample.Dispose();
        Assert.Equal(1, data.RefCount);
        data.Release();
        Assert.Equal(0, AudioHost.SampleList.Count);
    }

    [Fact]
    public void InvalidMemoryWaveLeavesBothListsUnchanged()
    {
        using var state = new SoundState();
        var meta = SdlSoundSystem.SampleData;
        Assert.Throws<InvalidDataException>(() => meta.CreateInstance(default, new byte[1], 1));
        Assert.Equal(0, AudioHost.SampleList.Count);
        Assert.Equal(0, AudioHost.SampleDataList.Count);
    }

    [Fact]
    public void NullBackendKeepsCompletedSampleCreation()
    {
        using var state = new SoundState();
        AudioHost.SoundSystems.Clear();
        NullSoundSystem.Install();
        var system = NullSoundSystem.Descriptor.CreateInstance();
        Assert.IsType<NullSoundSystem>(system);
        Assert.Equal(1, system.RefCount);
        var data = SampleData.Create("silent");
        var sample = NullSoundSystem.Samples.CreateInstance(data);
        Assert.Equal("NullSample", sample.GetType().Name);
        Assert.Equal(2, data.RefCount);
        Assert.False(sample.Playing);
        sample.Dispose();
        Assert.Equal(1, data.RefCount);
        data.Release();
        system.Dispose();
    }
}
