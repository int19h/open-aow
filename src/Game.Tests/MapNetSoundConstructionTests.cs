using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Game.Audio;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Net;
using Aow.Game.World;
using Aow.Game.World.Map;
using MovePath = Aow.Game.World.Movement.MovePath;
using UnitSfxManager = Aow.Game.Audio.SfxManager;

namespace Aow.Game.Tests;

public sealed class MapNetSoundConstructionTests
{
    private sealed class MapOwner : EngineFixture.Owner
    {

        public override int AddChild(EngineObject child)
        {
            Assert.Same(this, child.Owner);
            base.AddChild(child);
            return Members.Count - 1;
        }
    }

    public static IEnumerable<object[]> EngineCases()
    {
        Type[] types = [typeof(MapField), typeof(MapContainer), typeof(VisibilityList), typeof(HSMap),
            typeof(HexSprite), typeof(MapObject), typeof(AbstractHexagon), typeof(AbstractBorderHexagon), typeof(AbstractRoad),
            typeof(Sfx), typeof(SfxSequence), typeof(MapObjectResource), typeof(MapObjectResourceList), typeof(MapLevel),
            typeof(HexSpriteSelection), typeof(MapObjectResourceTerrainControl), typeof(HexControl),
            typeof(RoadControl), typeof(HSSet), typeof(CustomFixedVisibleHexSprite), typeof(MultiHexMapObjectResource), typeof(AbstractHexagonResource),
            typeof(PlayerCommunicator), typeof(TokenEvent), typeof(TokenEventList), typeof(DataPackage),
            typeof(PlayerCommunicationSettings), typeof(PlayerCommunicationSettingsList),
            typeof(TokenPackage), typeof(PassTokenTokenEvent), typeof(SyncTokenEvent), typeof(SongList), typeof(SfxLibrary)];
        foreach (Type type in types)
            foreach (bool owned in new[] { false, true }) yield return [type, owned];
    }

    [Theory]
    [MemberData(nameof(EngineCases))]
    public void DirectMetaAndSavedIdKeepExactTypeOwnerAndState(Type type, bool owned)
    {
        if (EngineHost.Engines is null) EngineHost.Initialize();
        GameEngine engine = new GameEngine(null!);
        var owner = new MapOwner();
        ECustomNode argument = owned ? type == typeof(MapContainer) ? new HSMap(null!) : owner : null!;
        var meta = EngineClass.Of(type);
        if (engine._classTable.TryGetValue(meta.ClassId, out EngineClass? previous) && previous is not null)
            engine.UnregisterEngineClasses([previous]);
        engine.RegisterEngineClasses([EngineClass.Of(type)]);
        var direct = type == typeof(MapLevel) ? new MapLevel(argument)
            : (EngineObject)Activator.CreateInstance(type, [argument])!;
        EngineObject[] values = [direct, meta.CreateInstance(argument), engine.CreateEngineObject(meta.ClassId, argument)];
        try
        {
            foreach (var value in values)
            {
                Assert.IsType(type, value);
                Assert.Same(argument, value.Owner);
                Assert.Same(meta, value.EngineClass);
                AssertState(value);
            }
            Assert.Equal(owned && type != typeof(MapField) && type != typeof(MapContainer) ? 3 : 0, owner.Members.Count);
        }
        finally
        {
            foreach (var value in values) value.Dispose();
            if (argument is HSMap mapOwner) mapOwner.Dispose();
            engine.Dispose();
        }
        Assert.Empty(owner.Members);
    }

    private static void AssertState(EngineObject value)
    {
        switch (value)
        {
            case MapField field:
                Assert.Equal(-1, field.TerrainType);
                Assert.Equal(-1, field.OverlayType);
                Assert.Equal(3, field._capacity);
                Assert.Equal(0, field.Count);
                Assert.Equal(24, field.EngineClass.InstanceSize);
                break;
            case MapContainer container:
                Assert.Equal(0, container.Count);
                break;
            case HSMap map:
                Assert.IsType<HexSpriteMapMouse>(map.Mouse);
                Assert.Same(map, map.Mouse._hexSpriteMap);
                Assert.Same(map, map.Selection.Owner);
                Assert.Same(map, map.Timer.Owner);
                Assert.Equal(0, map._hexSpriteChangedEventsStore.Count);
                Assert.Equal(0, map._terrainChangedEventsStore.Count);
                Assert.Equal(0, map._viewPortChangedEventsStore.Count);
                Assert.Equal(0, map._newFrameEventsStore.Count);
                Assert.Equal(0, map._destroyEventsStore.Count);
                Assert.Equal(0, map.StaticVisibleHexSprite.Count);
                Assert.Equal(0, map.DynamicVisibleHexSprite.Count);
                Assert.Equal(0, map.StaticFixedVisibleHexSprite.Count);
                Assert.Equal(0, map.DynamicFixedVisibleHexSprite.Count);
                break;
            case MapObjectResource resource:
                Assert.NotNull(resource._terrainList);
                Assert.NotNull(resource._imageLibrary);
                Assert.Equal(value is AbstractHexagonResource ? 0 : 26, resource.Level);
                if (value is MultiHexMapObjectResource multi)
                {
                    Assert.Equal(16, multi.MainPointOffsetX);
                    Assert.Equal(32, multi.MainPointOffsetY);
                }
                break;
            case MapLevel level:
                Assert.Equal(65793, level.MapFieldClassId);
                Assert.Equal(0, level.Fields.Length);
                Assert.Equal(124, level.EngineClass.InstanceSize);
                break;
            case HexSpriteSelection selection:
                Assert.Empty(selection.LocationHexSprite);
                Assert.Equal(0, selection._onSelectStore.Count);
                Assert.Equal(0, selection._onDeselectStore.Count);
                break;
            case MapObjectResourceTerrainControl control:
                Assert.Equal(typeof(MapObject), control.MapObjectClass);
                Assert.Equal(5, control.Tries);
                Assert.Empty(control._resources);
                break;
            case HSSet set:
                Assert.NotNull(set._pluginControl);
                Assert.Equal(0, set._terrainTypes.Count);
                Assert.Equal(0, set._overlayTypes.Count);
                Assert.Equal(0, set._decorationTypes.Count);
                Assert.Same(set, set._hexControl.Owner);
                Assert.Same(set, set._roadControl.Owner);
                Assert.NotNull(set._mapObjectResourceList);
                Assert.NotNull(set._resourceEditSettings);
                Assert.NotNull(set._imageLibraryManager);
                Assert.Equal(0, set._sfxManager.Sfx.Count);
                break;
            case MapObject sprite:
                Assert.Null(sprite._resource);
                Assert.Equal(default, sprite._status);
                break;
            case Sfx effect:
                Assert.Equal("", effect.FileName);
                break;
            case PlayerCommunicator player:
                Assert.Null(player._receivedPackage);
                Assert.Equal(0L, player._sendStream.Length);
                Assert.Equal(0L, player._receiveStream.Length);
                Assert.Equal(0, player._onReceiveStore.Count);
                Assert.Equal(0, player._onProcessStore.Count);
                break;
            case TokenEvent token:
                Assert.Equal(1, token._refCount);
                Assert.Equal(-1, token.TokenCounter);
                break;
            case TokenEventList events:
                Assert.Equal(0, events.Count);
                Assert.NotNull(events._list);
                break;
            case DataPackage package:
                Assert.Equal(1, package._refCount);
                if (package is TokenPackage tokens) Assert.Equal(0, tokens.TokenEvents.Count);
                break;
            case PlayerCommunicationSettings settings:
                Assert.Equal(ConnectionMode.Local, settings._connectionMode);
                break;
            case SongList songs:
                Assert.Equal(0, songs.Songs.Count);
                Assert.NotNull(songs.Songs.OnChange);
                Assert.Equal(0, songs.SongNames.Count);
                break;
            case ECustomNode node:
                Assert.Equal(0, node.Count);
                break;
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    [InlineData(13)]
    [InlineData(14)]
    [InlineData(15)]
    [InlineData(16)]
    [InlineData(17)]
    [InlineData(18)]
    [InlineData(19)]
    public void HelpersKeepTheirFactoryState(int kind)
    {
        object value = kind switch
        {
            0 => new NewHexSpriteMapSettings(),
            1 => new TerrainList(),
            2 => new HexSpriteMapMouse(),
            3 => new MapCoordinateList(),
            4 => new TokenControl(null!, 7),
            5 => new TokenExecutor(null!),
            6 => new TokenManager(null!),
            7 => new PlayerCommunicationControl(),
            8 => new PackageDataList(),
            9 => new RemotePlayerCommunicationControl(),
            10 => new LocalPlayerCommunicationControl(),
            11 => new TokenPackageDistributor(null!),
            12 => new SoundSystem(),
            13 => new SfxList(),
            14 => new UnitSfxManager(null!),
            15 => new SongPlayer(),
            16 => new SampleList(),
            17 => new SampleDataList(),
            18 => new SoundSystemList(),
            _ => new MovePath()
        };
        try
        {
            switch (value)
            {
                case NewHexSpriteMapSettings map:
                    Assert.Equal((64, 64, 1, 66048, 65795, 65794),
                        (map.Width, map.Height, map.Levels, map.HexSpriteMapClassId, map.MapContainerClassId, map.MapLevelClassId));
                    Assert.Equal("", map.HssFileName);
                    break;
                case TerrainList terrain: Assert.Equal(0, terrain.Count); break;
                case HexSpriteMapMouse mouse:
                    Assert.Equal(int.MinValue, mouse._x);
                    Assert.Equal(int.MinValue, mouse._y);
                    Assert.Equal(0, mouse._onUpStore.Count);
                    Assert.Equal(0, mouse._onDownStore.Count);
                    Assert.Equal(0, mouse._onMoveStore.Count);
                    Assert.Equal(0, mouse._onDoubleClickStore.Count);
                    Assert.Equal(0, mouse._onCaptureChangedStore.Count);
                    break;
                case MapCoordinateList list:
                    Assert.Equal(-1, list._default.X);
                    Assert.Equal(-1, list._default.Y);
                    Assert.Equal(-1, list._default.MapLevel);
                    Assert.Equal(0, list._default.Tag);
                    Assert.Equal(0, list.Count);
                    break;
                case TokenControl control:
                    Assert.Equal(7, control._player);
                    Assert.True(control._active);
                    Assert.Equal(0, control.Count);
                    break;
                case TokenExecutor executor:
                    Assert.Equal(0, executor._events.Count);
                    Assert.Equal(0, executor._systemEvents.Count);
                    Assert.Empty(executor._tacticalBattleList);
                    Assert.Equal(0, executor._onEventsExecutedStore.Count);
                    Assert.Equal(0, executor._onActivateTacticalBattleStore.Count);
                    break;
                case TokenManager manager:
                    Assert.Null(manager.Owner);
                    Assert.Equal(0, manager.Count);
                    Assert.Same(manager, manager._tokenExecutor._tokenManager);
                    Assert.Same(manager, manager._tokenPackageDistributor._tokenManager);
                    break;
                case PlayerCommunicationControl communication:
                    Assert.Equal(1, communication._refCount);
                    Assert.Equal(0, communication.Count);
                    if (value is RemotePlayerCommunicationControl remote) Assert.Equal(0, remote._packageDataList.Count);
                    if (value is LocalPlayerCommunicationControl local) Assert.Equal(0, local._packageDataList.Count);
                    break;
                case PackageDataList packages: Assert.Equal(0, packages.Count); break;
                case TokenPackageDistributor distributor: Assert.Null(distributor._tokenManager); break;
                case SoundSystem sound: Assert.Equal(1, sound.RefCount); break;
                case UnitSfxManager sfx: Assert.Equal(0, sfx.Sfx.Count); break;
                case SongPlayer song:
                    Assert.Equal(-1, song.CurrentIndex);
                    Assert.Equal(0, song.Songs.Count);
                    Assert.NotNull(song.Songs.OnChange);
                    Assert.Equal(0, song.SongNames.Count);
                    break;
                case StringList strings: Assert.Equal(0, strings.Count); break;
                case SampleList samples: Assert.Equal(0, samples.Count); break;
                case SampleDataList data: Assert.Equal(0, data.Count); break;
                case DelphiList list: Assert.Equal(0, list.Count); break;
            }
        }
        finally { ((value ?? throw new NullReferenceException()) as IDisposable)?.Dispose(); }
    }

    [Fact]
    public void TokenControlPublicationHasNoLaterConstructorWork()
    {
        using var manager = new TokenManager(null!);
        var control = new TokenControl(manager, 7);
        Assert.Same(control, manager._tokenExecutor._tacticalBattleList[0]);
        Assert.Single(manager._tokenExecutor._tacticalBattleList);
        control.Dispose();
        Assert.Empty(manager._tokenExecutor._tacticalBattleList);
    }

    [Fact]
    public void SfxComponentInsertionPrecedesPrivateListAllocation()
    {
        var owner = new ObservingComponent();
        var value = new UnitSfxManager(owner);
        Assert.True(owner.Observed);
        Assert.Same(owner, value.Owner);
        Assert.Equal(0, value.Sfx.Count);
        value.Dispose();
        Assert.Equal(0, owner.ComponentCount);
    }

    private sealed class ObservingComponent : Component
    {
        internal bool Observed;

        public override void Notification(Component child, Component.Operation operation)
        {
            if (operation == Component.Operation.Insert && child is UnitSfxManager manager)
            {
                Assert.Null(manager._sfx);
                Observed = true;
            }
            base.Notification(child, operation);
        }
    }
}
