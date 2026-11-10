using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game;
using Aow.Game.Audio;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Net;
using Aow.Game.World;
using Aow.Game.World.Map;
using MovePath = Aow.Game.World.Movement.Movepath;
using UnitSfxManager = Aow.Game.Audio.SFXManager;

namespace Aow.Units.Tests;

public sealed class MapNetSoundConstructionTests
{
    private sealed class MapOwner : EngineFixture.Owner
    {

        public override int AddChild(EObject child)
        {
            Assert.Same(this, child.Owner);
            base.AddChild(child);
            return Members.Count - 1;
        }
    }

    public static IEnumerable<object[]> EngineCases()
    {
        Type[] types = [typeof(MapField), typeof(MapContainer), typeof(VisibilityList), typeof(HSMap),
            typeof(HexagonSprite), typeof(MapObject), typeof(AbstractHexagon), typeof(AbstractBorderHexagon), typeof(AbstractRoad),
            typeof(SFX), typeof(SFXSequence), typeof(MapObjectResource), typeof(MapObjectResourceList), typeof(MapLevel),
            typeof(HexagonSpriteSelection), typeof(MORTerrainControl), typeof(HexagonControl),
            typeof(RoadControl), typeof(HSSet), typeof(CustomFixedVisibleHS), typeof(LibraryImageHS),
            typeof(HPBasedHS), typeof(MultiHexMOR), typeof(AbstractHexagonResource),
            typeof(PlayerCommunicator), typeof(TokenEvent), typeof(TokenEventList), typeof(DataPackage),
            typeof(PlayerCommunicationSettings), typeof(PlayerCommunicationSettingsList),
            typeof(TokenPackage), typeof(PassTokenTE), typeof(SyncTE), typeof(Songlist), typeof(SFXLibrary)];
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
        var meta = EClass.Of(type);
        var previous = (EClass?)engine.FClassTable.Find(meta.ClassID);
        if (previous is not null) engine.UnregisterEClasses([previous]);
        engine.RegisterEClasses([EClass.Of(type)]);
        var direct = type == typeof(MapLevel) ? new MapLevel(argument)
            : (EObject)Activator.CreateInstance(type, [argument])!;
        EObject[] values = [direct, meta.Create(argument), engine.CreateEObject(meta.ClassID, argument)];
        try
        {
            foreach (var value in values)
            {
                Assert.IsType(type, value);
                Assert.Same(argument, value.Owner);
                Assert.Same(meta, value.EClass);
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

    private static void AssertState(EObject value)
    {
        switch (value)
        {
            case MapField field:
                Assert.Equal(-1, field.TerrainType);
                Assert.Equal(-1, field.OverlayType);
                Assert.Equal(3, field.FCapacity);
                Assert.Equal(0, field.Count);
                Assert.Equal(24, field.EClass.InstanceSize);
                break;
            case MapContainer container:
                Assert.Equal(0, container.Count);
                break;
            case HSMap map:
                Assert.IsType<HSMapMouse>(map.Mouse);
                Assert.Same(map, map.Mouse.FHSMap);
                Assert.Same(map, map.Selection.Owner);
                Assert.Same(map, map.Timer.Owner);
                Assert.Equal(0, map.FHSChangedEventsStore.Count);
                Assert.Equal(0, map.FTerrainChangedEventsStore.Count);
                Assert.Equal(0, map.FViewPortChangedEventsStore.Count);
                Assert.Equal(0, map.FNewFrameEventsStore.Count);
                Assert.Equal(0, map.FDestroyEventsStore.Count);
                Assert.Equal(0, map.StaticVisibleHS.Count);
                Assert.Equal(0, map.DynamicVisibleHS.Count);
                Assert.Equal(0, map.StaticFixedVisibleHS.Count);
                Assert.Equal(0, map.DynamicFixedVisibleHS.Count);
                break;
            case MapObjectResource resource:
                Assert.NotNull(resource.FTerrainList);
                Assert.NotNull(resource.FImageLibrary);
                Assert.Equal(value is AbstractHexagonResource ? 0 : 26, resource.Level);
                if (value is MultiHexMOR multi)
                {
                    Assert.Equal(16, multi.dXmainP);
                    Assert.Equal(32, multi.dYmainP);
                }
                break;
            case MapLevel level:
                Assert.Equal(65793, level.MapFieldCID);
                Assert.Equal(0, level.Fields.Length);
                Assert.Equal(124, level.EClass.InstanceSize);
                break;
            case HexagonSpriteSelection selection:
                Assert.Equal(0, selection.LocationHS.Count);
                Assert.Equal(0, selection.FOnSelectStore.Count);
                Assert.Equal(0, selection.FOnDeselectStore.Count);
                break;
            case MORTerrainControl control:
                Assert.Equal(typeof(MapObject), control.MOClass);
                Assert.Equal(5, control.Tries);
                Assert.Equal(0, control.FResources.Count);
                break;
            case HSSet set:
                Assert.NotNull(set.FPlugInControl);
                Assert.Equal(0, set.FTerrainTypes.Count);
                Assert.Equal(0, set.FOverlayTypes.Count);
                Assert.Equal(0, set.FDecorationTypes.Count);
                Assert.Same(set, set.FHexagonControl.Owner);
                Assert.Same(set, set.FRoadControl.Owner);
                Assert.NotNull(set.FMapObjectResourceList);
                Assert.NotNull(set.FResourceEditSettings);
                Assert.NotNull(set.FImageLibraryManager);
                Assert.Equal(0, set.FSFXManager.SFX.Count);
                break;
            case LibraryImageHS image:
                Assert.Equal(26, image.Level);
                Assert.Equal(Set<LibraryImageHS.StatusElement>.Of(LibraryImageHS.StatusElement.MapIsOwner), image.FStatus);
                break;
            case HPBasedHS hp:
                Assert.Equal(int.MinValue, hp.FXhp);
                Assert.Equal(int.MinValue, hp.FYhp);
                Assert.Equal(-1, hp.FLhx);
                break;
            case MapObject sprite:
                Assert.Null(sprite.FResource);
                Assert.Equal(default, sprite.FStatus);
                break;
            case SFX effect:
                Assert.Equal("", effect.FileName);
                break;
            case PlayerCommunicator player:
                Assert.Null(player.FReceivedPackage);
                Assert.Equal(0L, player.FSendStream.Length);
                Assert.Equal(0L, player.FReceiveStream.Length);
                Assert.Equal(0, player.FOnReceiveStore.Count);
                Assert.Equal(0, player.FOnProcessStore.Count);
                break;
            case TokenEvent token:
                Assert.Equal(1, token.FRefCount);
                Assert.Equal(-1, token.TokenCounter);
                break;
            case TokenEventList events:
                Assert.Equal(0, events.Count);
                Assert.NotNull(events.FList);
                break;
            case DataPackage package:
                Assert.Equal(1, package.FRefCount);
                if (package is TokenPackage tokens) Assert.Equal(0, tokens.TokenEvents.Count);
                break;
            case PlayerCommunicationSettings settings:
                Assert.Equal(ConnectionMode.Local, settings.FConnectionMode);
                break;
            case Songlist songs:
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
            0 => new NewHSMapSettings(),
            1 => new TerrainList(),
            2 => new HSMapMouse(),
            3 => new XYLList(),
            4 => new TokenControl(null!, 7),
            5 => new TokenExecuter(null!),
            6 => new TokenManager(null!),
            7 => new PlayerCommunicationControl(),
            8 => new PackageDataList(),
            9 => new RemotePCC(),
            10 => new LocalPCC(),
            11 => new TokenPackageDistributer(null!),
            12 => new SoundSystem(),
            13 => new SFXList(),
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
                case NewHSMapSettings map:
                    Assert.Equal((64, 64, 1, 66048, 65795, 65794),
                        (map.Width, map.Height, map.Levels, map.HSMapClassID, map.MapContainerClassID, map.MapLevelClassID));
                    Assert.Equal("", map.HSSFileName);
                    break;
                case TerrainList terrain: Assert.Equal(0, terrain.Count); break;
                case HSMapMouse mouse:
                    Assert.Equal(int.MinValue, mouse.FX);
                    Assert.Equal(int.MinValue, mouse.FY);
                    Assert.Equal(0, mouse.FOnUpStore.Count);
                    Assert.Equal(0, mouse.FOnDownStore.Count);
                    Assert.Equal(0, mouse.FOnMoveStore.Count);
                    Assert.Equal(0, mouse.FOnDblClickStore.Count);
                    Assert.Equal(0, mouse.FOnCaptureChangedStore.Count);
                    break;
                case XYLList list:
                    Assert.Equal(-1, list.FDefault.X);
                    Assert.Equal(-1, list.FDefault.Y);
                    Assert.Equal(-1, list.FDefault.L);
                    Assert.Equal(0, list.FDefault.Flag);
                    Assert.Equal(0, list.Count);
                    break;
                case TokenControl control:
                    Assert.Equal(7, control.FPlayer);
                    Assert.True(control.FActive);
                    Assert.Equal(0, control.Count);
                    break;
                case TokenExecuter executer:
                    Assert.Equal(0, executer.FEvents.Count);
                    Assert.Equal(0, executer.FSystemEvents.Count);
                    Assert.Equal(0, executer.FTCList.Count);
                    Assert.Equal(0, executer.FOnEventsExecutedStore.Count);
                    Assert.Equal(0, executer.FOnActivateTCStore.Count);
                    break;
                case TokenManager manager:
                    Assert.Null(manager.Owner);
                    Assert.Equal(0, manager.Count);
                    Assert.Same(manager, manager.FTokenExecuter.FTokenManager);
                    Assert.Same(manager, manager.FTokenPackageDistributer.FTokenManager);
                    break;
                case PlayerCommunicationControl communication:
                    Assert.Equal(1, communication.FRefCount);
                    Assert.Equal(0, communication.Count);
                    if (value is RemotePCC remote) Assert.Equal(0, remote.FPackageDataList.Count);
                    if (value is LocalPCC local) Assert.Equal(0, local.FPackageDataList.Count);
                    break;
                case PackageDataList packages: Assert.Equal(0, packages.Count); break;
                case TokenPackageDistributer distributer: Assert.Null(distributer.FTokenManager); break;
                case SoundSystem sound: Assert.Equal(1, sound.FRefCount); break;
                case UnitSfxManager sfx: Assert.Equal(0, sfx.SFX.Count); break;
                case SongPlayer song:
                    Assert.Equal(-1, song.CurrentIndex);
                    Assert.Equal(0, song.Songs.Count);
                    Assert.NotNull(song.Songs.OnChange);
                    Assert.Equal(0, song.SongNames.Count);
                    break;
                case StringList strings: Assert.Equal(0, strings.Count); break;
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
        Assert.Same(control, manager.FTokenExecuter.FTCList[0]);
        Assert.Equal(1, manager.FTokenExecuter.FTCList.Count);
        control.Dispose();
        Assert.Equal(0, manager.FTokenExecuter.FTCList.Count);
    }

    [Fact]
    public void SfxComponentInsertionPrecedesPrivateListAllocation()
    {
        var owner = new ObservingComponent();
        var value = new UnitSfxManager(owner);
        Assert.True(owner.Observed);
        Assert.Same(owner, value.Owner);
        Assert.Equal(0, value.SFX.Count);
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
                Assert.Null(manager.FSFX);
                Observed = true;
            }
            base.Notification(child, operation);
        }
    }
}
