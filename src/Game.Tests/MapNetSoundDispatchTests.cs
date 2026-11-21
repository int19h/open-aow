#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Audio;

using Aow.Game.World.Map;
using Aow.Platform;
using Sample = Aow.Game.Audio.Sample;

namespace Aow.Game.Tests;

public sealed class MapNetSoundDispatchTests
{
    private sealed class ConvertedField(CustomNode owner) : MapField(owner)
    {
        public override void SetOwner(CustomNode owner) => base.SetOwner(owner);
    }

    private sealed class ConvertedMap(CustomNode owner) : GameMap(owner)
    {
        internal int MouseCalls { get; private set; }

        public override GameMapMouse CreateMouse()
        {
            Assert.Null(Selection);
            MouseCalls++;
            return new GameMapMouse();
        }
    }

    private sealed class SpecializedField(CustomNode owner) : MapField(owner);

    private sealed class SpecializedLevel(CustomNode owner) : MapLevel(owner)
    {
        public override EngineClass MapFieldClass() => EngineClass.Of<SpecializedField>();
    }

    static MapNetSoundDispatchTests()
    {
        _ = EngineClass.Register(MapField.ClassId, static owner => new SpecializedField(owner));
        _ = EngineClass.Register(MapLevel.ClassId, static owner => new SpecializedLevel(owner));
    }

    [Fact]
    public void MapConstructorKeepsVirtualMouseCreationBeforeChildren()
    {
        using ConvertedMap converted = new(null!) { Holder = EngineFixture.Engine };
        Assert.Equal(1, converted.MouseCalls);
        Assert.Same(converted, converted.Mouse.HexSpriteMap);
        Assert.NotNull(converted.Selection);
    }

    [Fact]
    public void DynamicFieldsKeepTheirExactTypeAndCoordinates()
    {
        SpecializedLevel level = new(null!)
        {
            Holder = EngineFixture.Engine,
            HexLevel = 2
        };
        level.CreateMapFieldList(3, 2);
        Assert.Equal(6, level.Fields.Length);
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                SpecializedField field = Assert.IsType<SpecializedField>(level.GetField(x, y));
                Assert.Same(level, field.Owner);
                Assert.Equal(x, field.HexX);
                Assert.Equal(y, field.HexY);
                Assert.Equal(2, field.HexLevel);
            }
        }

        level.Dispose();
    }

    [Fact]
    public void LegacyMouseStartsWithEmptyEvents()
    {
        GameMapMouse mouse = new();
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
            AudioHost.SampleList = [];
            AudioHost.SampleDataList = [];
            AudioHost.SoundSystems = [];
            AudioHost.SoundSystem = null;
            AudioHost.SongPlayer = null;
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

    private static WaveFormat Format => new(FormatTag: 1, Channels: 1, SamplesPerSecond: 8000, BlockAlign: 1, BitsPerSample: 8);

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SoundDataRoutesKeepTypeCacheAndReferenceCounts(bool memory)
    {
        using SoundState state = new();
        SampleDataClass meta = SampleData.Descriptor;
        SampleData direct = memory ? SampleData.Create(Format, data: new byte[] { 128 }, size: 1) : SampleData.Create("direct");
        SampleData dynamic = memory ? meta.CreateInstance(Format, data: new byte[] { 128 }, size: 1) : meta.CreateInstance("dynamic");
        foreach (SampleData? value in new[] { direct, dynamic })
        {
            _ = Assert.IsType<SampleData>(value);
            Assert.Equal(1, value.RefCount);
            Assert.Equal(memory ? "" : value == direct ? "direct" : "dynamic", value.Name);
            if (!memory)
            {
                Assert.Same(value, AudioHost.SampleDataList.Find(value.Name));
            }
        }
        Assert.Equal(memory ? 0 : 2, AudioHost.SampleDataList.Count);
        Sample sample = Sample.Create(direct);
        Sample dynamicSample = Sample.Descriptor.CreateInstance(dynamic);
        _ = Assert.IsType<Sample>(sample);
        _ = Assert.IsType<Sample>(dynamicSample);
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
        Assert.Empty(AudioHost.SampleList);
        Assert.Empty(AudioHost.SampleDataList);
    }

    [Fact]
    public void SoundAliasesKeepDistinctDescriptorsOverOneRuntimeType()
    {
        using SoundState state = new();
        SoundSystemClass sdl = AudioHost.SoundSystems.Find("SDL");
        SoundSystemClass galaxy = AudioHost.SoundSystems.Find("Galaxy");
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
        InvalidOperationException error = Assert.Throws<InvalidOperationException>(() => NullSoundSystem.RegisterSoundSystem(sdl));
        Assert.Equal("Sound System with name: SDL already registered", error.Message);
    }

    [Theory]
    [InlineData("SDL")]
    [InlineData("Galaxy")]
    public void SdlFacadesCreateExactSilentTypes(string name)
    {
        using SoundState state = new();
        SoundSystemClass meta = AudioHost.SoundSystems.Find(name);
        SoundSystem system = meta.CreateInstance();
        _ = Assert.IsType<SdlSoundSystem>(system);
        Assert.Equal(1, system.RefCount);
        system.Dispose();
        Assert.True(AudioPlayback.InitializeSoundSystem(name,
            settings: new SoundSettings { SampleRate = 8000, SampleVoices = 1, SfxEnabled = true }));
        SampleDataClass sampleDataMeta = SdlSoundSystem.SampleData;
        SampleData data = sampleDataMeta.CreateInstance(Format, data: new byte[] { 128 }, size: 1);
        Assert.Equal("SdlSampleData", data.GetType().Name);
        Assert.Equal(1, data.RefCount);
        Assert.Empty(AudioHost.SampleDataList);
        SampleClass sampleMeta = SdlSoundSystem.Samples;
        Sample sample = sampleMeta.CreateInstance(data);
        Assert.Equal("SdlSample", sample.GetType().Name);
        Assert.Equal(-1, sample.Volume);
        Assert.False(sample.Playing);
        Assert.False(sample.Looped);
        Assert.Equal(SamplePanning.Center, sample.Panning);
        Assert.Equal(2, data.RefCount);
        _ = Assert.Single(AudioHost.SampleList);
        sample.Dispose();
        Assert.Equal(1, data.RefCount);
        data.Release();
        Assert.Empty(AudioHost.SampleList);
    }

    [Fact]
    public void InvalidMemoryWaveLeavesBothListsUnchanged()
    {
        using SoundState state = new();
        SampleDataClass meta = SdlSoundSystem.SampleData;
        _ = Assert.Throws<InvalidDataException>(() => meta.CreateInstance(default, data: new byte[1], size: 1));
        Assert.Empty(AudioHost.SampleList);
        Assert.Empty(AudioHost.SampleDataList);
    }

    [Fact]
    public void NullBackendKeepsCompletedSampleCreation()
    {
        using SoundState state = new();
        AudioHost.SoundSystems.Clear();
        NullSoundSystem.Install();
        SoundSystem system = NullSoundSystem.Descriptor.CreateInstance();
        _ = Assert.IsType<NullSoundSystem>(system);
        Assert.Equal(1, system.RefCount);
        SampleData data = SampleData.Create("silent");
        Sample sample = NullSoundSystem.Samples.CreateInstance(data);
        Assert.Equal("NullSample", sample.GetType().Name);
        Assert.Equal(2, data.RefCount);
        Assert.False(sample.Playing);
        sample.Dispose();
        Assert.Equal(1, data.RefCount);
        data.Release();
        system.Dispose();
    }
}
