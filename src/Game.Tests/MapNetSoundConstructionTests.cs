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
            Assert.True(EngineFixture.Forwards(child.Owner, this));
            base.AddChild(child);
            return Members.Count - 1;
        }
    }

    public static IEnumerable<object[]> EngineCases()
    {
        Type[] types = [typeof(MapField), typeof(MapContainer), typeof(VisibilityList), typeof(GameMap),
            typeof(HexSprite), typeof(MapObject), typeof(AbstractHexagon), typeof(AbstractBorderHexagon), typeof(AbstractRoad),
            typeof(Sfx), typeof(SfxSequence), typeof(MapObjectResource), typeof(MapObjectResourceList), typeof(MapLevel),
            typeof(HexSpriteSelection), typeof(MapObjectResourceTerrainControl), typeof(HexControl),
            typeof(RoadControl), typeof(CustomFixedVisibleHexSprite), typeof(MultiHexMapObjectResource), typeof(AbstractHexagonResource),
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
        GameEngine engine = new GameEngine(null!);
        var owner = new MapOwner();
        ECustomNode argument = owned ? type == typeof(MapContainer) ? new GameMap(null!) : EngineFixture.OwnerFor(type, owner) : null!;
        var meta = EngineClass.Of(type);
        if (engine.ClassTable.TryGetValue(meta.ClassId, out EngineClass? previous) && previous is not null)
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
            if (argument is GameMap mapOwner) mapOwner.Dispose();
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
                Assert.Equal(3, field.Capacity);
                Assert.Equal(0, field.Count);
                // Issue 200: the field is the game field (TAoWMapField).
                Assert.Same(EngineClass.Of<MapField>(), field.EngineClass);
                break;
            case MapContainer container:
                Assert.Equal(0, container.Count);
                break;
            case GameMap map:
                // Issue 200: the map is the game map, which makes the game mouse.
                Assert.IsType<GameMapMouse>(map.Mouse);
                Assert.Same(map, map.Mouse.HexSpriteMap);
                Assert.Same(map, map.Selection.Owner);
                Assert.Same(map, map.Timer.Owner);
                Assert.Equal(0, map.HexSpriteChangedEventsStore.Count);
                Assert.Equal(0, map.TerrainChangedEventsStore.Count);
                Assert.Equal(0, map.ViewPortChangedEventsStore.Count);
                Assert.Equal(0, map.NewFrameEventsStore.Count);
                Assert.Equal(0, map.DestroyEventsStore.Count);
                Assert.Equal(0, map.StaticVisibleHexSprite.Count);
                Assert.Equal(0, map.DynamicVisibleHexSprite.Count);
                Assert.Equal(0, map.StaticFixedVisibleHexSprite.Count);
                Assert.Equal(0, map.DynamicFixedVisibleHexSprite.Count);
                break;
            case MapObjectResource resource:
                Assert.NotNull(resource.TerrainList);
                Assert.NotNull(resource.ImageLibrary);
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
                // Issue 200: the level is the game level (TAoWMapLevel).
                Assert.Same(EngineClass.Of<MapLevel>(), level.EngineClass);
                break;
            case HexSpriteSelection selection:
                Assert.Empty(selection.LocationHexSprite);
                Assert.Equal(0, selection.OnSelectStore.Count);
                Assert.Equal(0, selection.OnDeselectStore.Count);
                break;
            case MapObjectResourceTerrainControl control:
                Assert.Equal(typeof(MapObject), control.MapObjectClass);
                Assert.Equal(5, control.Tries);
                Assert.Empty(control.Resources);
                break;
            case MapObject sprite:
                Assert.Null(sprite._resource);
                Assert.Equal(default, sprite.Status);
                break;
            case Sfx effect:
                Assert.Equal("", effect.FileName);
                break;
            case PlayerCommunicator player:
                Assert.Null(player.ReceivedPackage);
                Assert.Equal(0L, player.SendStream.Length);
                Assert.Equal(0L, player.ReceiveStream.Length);
                Assert.Equal(0, player.OnReceiveStore.Count);
                Assert.Equal(0, player.OnProcessStore.Count);
                break;
            case TokenEvent token:
                Assert.Equal(1, token.RefCount);
                Assert.Equal(-1, token.TokenCounter);
                break;
            case TokenEventList events:
                Assert.Equal(0, events.Count);
                Assert.NotNull(events.List);
                break;
            case DataPackage package:
                Assert.Equal(1, package.RefCount);
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
                    Assert.Equal(int.MinValue, mouse.X);
                    Assert.Equal(int.MinValue, mouse.Y);
                    Assert.Equal(0, mouse.OnUpStore.Count);
                    Assert.Equal(0, mouse.OnDownStore.Count);
                    Assert.Equal(0, mouse.OnMoveStore.Count);
                    Assert.Equal(0, mouse.OnDoubleClickStore.Count);
                    Assert.Equal(0, mouse.OnCaptureChangedStore.Count);
                    break;
                case MapCoordinateList list:
                    Assert.Equal(-1, list._default.X);
                    Assert.Equal(-1, list._default.Y);
                    Assert.Equal(-1, list._default.MapLevel);
                    Assert.Equal(0, list._default.Tag);
                    Assert.Equal(0, list.Count);
                    break;
                case TokenControl control:
                    Assert.Equal(7, control.Player);
                    Assert.True(control.Active);
                    Assert.Equal(0, control.Count);
                    break;
                case TokenExecutor executor:
                    Assert.Equal(0, executor.Events.Count);
                    Assert.Equal(0, executor.SystemEvents.Count);
                    Assert.Empty(executor.TacticalBattleList);
                    Assert.Equal(0, executor.OnEventsExecutedStore.Count);
                    Assert.Equal(0, executor.OnActivateTacticalBattleStore.Count);
                    break;
                case TokenManager manager:
                    Assert.Null(manager.Owner);
                    Assert.Equal(0, manager.Count);
                    Assert.Same(manager, manager.TokenExecutor.TokenManager);
                    Assert.Same(manager, manager.TokenPackageDistributor.TokenManager);
                    break;
                case PlayerCommunicationControl communication:
                    Assert.Equal(1, communication.RefCount);
                    Assert.Equal(0, communication.Count);
                    if (value is RemotePlayerCommunicationControl remote) Assert.Equal(0, remote.PackageDataList.Count);
                    if (value is LocalPlayerCommunicationControl local) Assert.Equal(0, local.PackageDataList.Count);
                    break;
                case PackageDataList packages: Assert.Equal(0, packages.Count); break;
                case TokenPackageDistributor distributor: Assert.Null(distributor.TokenManager); break;
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
                case SoundSystemList list: Assert.Equal(0, list.Count); break;
            }
        }
        finally { ((value ?? throw new NullReferenceException()) as IDisposable)?.Dispose(); }
    }

    [Fact]
    public void TokenControlPublicationHasNoLaterConstructorWork()
    {
        using var manager = new TokenManager(null!);
        var control = new TokenControl(manager, 7);
        Assert.Same(control, manager.TokenExecutor.TacticalBattleList[0]);
        Assert.Single(manager.TokenExecutor.TacticalBattleList);
        control.Dispose();
        Assert.Empty(manager.TokenExecutor.TacticalBattleList);
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
        internal bool Observed { get; private set; }

        public override void Notification(Component child, Component.Operation operation)
        {
            if (operation == Component.Operation.Insert && child is UnitSfxManager manager)
            {
                Assert.Null(manager.Sfx);
                Observed = true;
            }
            base.Notification(child, operation);
        }
    }
}
