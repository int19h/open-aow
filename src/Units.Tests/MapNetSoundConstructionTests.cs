using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.NetworkE;
using Aow.Units.Sound;
using EngineApi = Aow.Units.Engine.EngineUnit;
using MovePath = Aow.Units.AoWE.TMovepath;
using UnitSfxManager = Aow.Units.Sound.TSFXManager;

namespace Aow.Units.Tests;

public sealed class MapNetSoundConstructionTests
{
    private sealed class MapOwner : EngineFixture.Owner
    {

        public override int AddChild(TEObject child)
        {
            Assert.Same(this, child.Owner);
            base.AddChild(child);
            return Members.Count - 1;
        }
    }

    public static IEnumerable<object[]> EngineCases()
    {
        Type[] types = [typeof(TMapField), typeof(TMapContainer), typeof(TVisibilityList), typeof(THSMap),
            typeof(THexagonSprite), typeof(TMapObject), typeof(TAbstractHexagon), typeof(TAbstractBorderHexagon), typeof(TAbstractRoad),
            typeof(TSFX), typeof(TSFXSequence), typeof(TMapObjectResource), typeof(TMapObjectResourceList), typeof(TMapLevel),
            typeof(THexagonSpriteSelection), typeof(TMORTerrainControl), typeof(THexagonControl),
            typeof(TRoadControl), typeof(THSSet), typeof(TCustomFixedVisibleHS), typeof(TLibraryImageHS),
            typeof(THPBasedHS), typeof(TMultiHexMOR), typeof(TAbstractHexagonResource),
            typeof(TPlayerCommunicator), typeof(TTokenEvent), typeof(TTokenEventList), typeof(TDataPackage),
            typeof(TPlayerCommunicationSettings), typeof(TPlayerCommunicationSettingsList),
            typeof(TTokenPackage), typeof(TPassTokenTE), typeof(TSyncTE), typeof(TSonglist), typeof(TSFXLibrary)];
        foreach (Type type in types)
            foreach (bool owned in new[] { false, true }) yield return [type, owned];
    }

    [Theory]
    [MemberData(nameof(EngineCases))]
    public void DirectMetaAndSavedIdKeepExactTypeOwnerAndState(Type type, bool owned)
    {
        if (EngineApi.Engines is null) EngineApi.Initialize();
        var engine = new TEngine(null!);
        var owner = new MapOwner();
        TECustomNode argument = owned ? type == typeof(TMapContainer) ? new THSMap(null!) : owner : null!;
        var meta = EClass.Of(type);
        var previous = (EClass?)engine.FClassTable.Find(meta.ClassID);
        if (previous is not null) engine.UnregisterEClasses([previous]);
        engine.RegisterEClasses([EClass.Of(type)]);
        var direct = type == typeof(TMapLevel) ? new TMapLevel(argument)
            : (TEObject)Activator.CreateInstance(type, [argument])!;
        TEObject[] values = [direct, meta.Create(argument), engine.CreateEObject(meta.ClassID, argument)];
        try
        {
            foreach (var value in values)
            {
                Assert.IsType(type, value);
                Assert.Same(argument, value.Owner);
                Assert.Same(meta, value.EClass);
                AssertState(value);
            }
            Assert.Equal(owned && type != typeof(TMapField) && type != typeof(TMapContainer) ? 3 : 0, owner.Members.Count);
        }
        finally
        {
            foreach (var value in values) value.Destroy();
            if (argument is THSMap mapOwner) mapOwner.Destroy();
            engine.Destroy();
        }
        Assert.Empty(owner.Members);
    }

    private static void AssertState(TEObject value)
    {
        switch (value)
        {
            case TMapField field:
                Assert.Equal(-1, field.TerrainType);
                Assert.Equal(-1, field.OverlayType);
                Assert.Equal(3, field.FCapacity);
                Assert.Equal(0, field.Count);
                Assert.Equal(24, field.EClass.InstanceSize);
                break;
            case TMapContainer container:
                Assert.Equal(0, container.Count);
                break;
            case THSMap map:
                Assert.IsType<THSMapMouse>(map.Mouse);
                Assert.Same(map, map.Mouse.FHSMap);
                Assert.Same(map, map.Selection.Owner);
                Assert.Same(map, map.Timer.Owner);
                Assert.Equal(0, map.HSChangedEvents.Count);
                Assert.Equal(0, map.TerrainChangedEvents.Count);
                Assert.Equal(0, map.ViewPortChangedEvents.Count);
                Assert.Equal(0, map.NewFrameEvents.Count);
                Assert.Equal(0, map.DestroyEvents.Count);
                Assert.Equal(0, map.StaticVisibleHS.Count);
                Assert.Equal(0, map.DynamicVisibleHS.Count);
                Assert.Equal(0, map.StaticFixedVisibleHS.Count);
                Assert.Equal(0, map.DynamicFixedVisibleHS.Count);
                break;
            case TMapObjectResource resource:
                Assert.NotNull(resource.FTerrainList);
                Assert.NotNull(resource.FImageLibrary);
                Assert.Equal(value is TAbstractHexagonResource ? 0 : 26, resource.Level);
                if (value is TMultiHexMOR multi)
                {
                    Assert.Equal(16, multi.dXmainP);
                    Assert.Equal(32, multi.dYmainP);
                }
                break;
            case TMapLevel level:
                Assert.Equal(65793, level.MapFieldCID);
                Assert.Equal(0, level.Fields.Length);
                Assert.Equal(124, level.EClass.InstanceSize);
                break;
            case THexagonSpriteSelection selection:
                Assert.Equal(0, selection.LocationHS.Count);
                Assert.Equal(0, selection.OnSelect.Count);
                Assert.Equal(0, selection.OnDeselect.Count);
                break;
            case TMORTerrainControl control:
                Assert.Equal(typeof(TMapObject), control.MOClass);
                Assert.Equal(5, control.Tries);
                Assert.Equal(0, control.FResources.Count);
                break;
            case THSSet set:
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
            case TLibraryImageHS image:
                Assert.Equal(26, image.Level);
                Assert.Equal(TSet<TLibraryImageHSStatusElement>.Of(TLibraryImageHSStatusElement.lsMapIsOwner), image.FStatus);
                break;
            case THPBasedHS hp:
                Assert.Equal(int.MinValue, hp.FXhp);
                Assert.Equal(int.MinValue, hp.FYhp);
                Assert.Equal(-1, hp.FLhx);
                break;
            case TMapObject sprite:
                Assert.Null(sprite.FResource);
                Assert.Equal(default, sprite.FStatus);
                break;
            case TSFX effect:
                Assert.Equal("", effect.FileName);
                break;
            case TPlayerCommunicator player:
                Assert.Null(player.FReceivedPackage);
                Assert.Equal(0, player.FSendStream.Size);
                Assert.Equal(0, player.FReceiveStream.Size);
                Assert.Equal(0, player.OnReceive.Count);
                Assert.Equal(0, player.OnProcess.Count);
                break;
            case TTokenEvent token:
                Assert.Equal(1, token.FRefCount);
                Assert.Equal(-1, token.TokenCounter);
                break;
            case TTokenEventList events:
                Assert.Equal(0, events.Count);
                Assert.NotNull(events.FList);
                break;
            case TDataPackage package:
                Assert.Equal(1, package.FRefCount);
                if (package is TTokenPackage tokens) Assert.Equal(0, tokens.TokenEvents.Count);
                break;
            case TPlayerCommunicationSettings settings:
                Assert.Equal(TConnectionMode.cmLocal, settings.FConnectionMode);
                break;
            case TSonglist songs:
                Assert.Equal(0, songs.Songs.Count);
                Assert.NotNull(songs.Songs.OnChange);
                Assert.Equal(0, songs.SongNames.Count);
                break;
            case TECustomNode node:
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
        TObject value = kind switch
        {
            0 => new TNewHSMapSettings(),
            1 => new TTerrainList(),
            2 => new THSMapMouse(),
            3 => new TXYLList(),
            4 => new TTokenControl(null!, 7),
            5 => new TTokenExecuter(null!),
            6 => new TTokenManager(null!),
            7 => new TPlayerCommunicationControl(),
            8 => new TPackageDataList(),
            9 => new TRemotePCC(),
            10 => new TLocalPCC(),
            11 => new TTokenPackageDistributer(null!),
            12 => new TSoundSystem(),
            13 => new TSFXList(),
            14 => new UnitSfxManager(null!),
            15 => new TSongPlayer(),
            16 => new TSampleList(),
            17 => new TSampleDataList(),
            18 => new TSoundSystemList(),
            _ => new MovePath()
        };
        try
        {
            switch (value)
            {
                case TNewHSMapSettings map:
                    Assert.Equal((64, 64, 1, 66048, 65795, 65794),
                        (map.Width, map.Height, map.Levels, map.HSMapClassID, map.MapContainerClassID, map.MapLevelClassID));
                    Assert.Equal("", map.HSSFileName);
                    break;
                case TTerrainList terrain: Assert.Equal(0, terrain.Count); break;
                case THSMapMouse mouse:
                    Assert.Equal(int.MinValue, mouse.FX);
                    Assert.Equal(int.MinValue, mouse.FY);
                    foreach (var list in new[] { mouse.FOnUp, mouse.FOnDown, mouse.FOnMove, mouse.FOnDblClick, mouse.FOnCaptureChanged })
                        Assert.Equal(0, list.Count);
                    break;
                case TXYLList list:
                    Assert.Equal(-1, list.FDefault.X);
                    Assert.Equal(-1, list.FDefault.Y);
                    Assert.Equal(-1, list.FDefault.L);
                    Assert.Equal(0, list.FDefault.Flag);
                    Assert.Equal(0, list.Count);
                    break;
                case TTokenControl control:
                    Assert.Equal(7, control.FPlayer);
                    Assert.True(control.FActive);
                    Assert.Equal(0, control.Count);
                    break;
                case TTokenExecuter executer:
                    Assert.Equal(0, executer.FEvents.Count);
                    Assert.Equal(0, executer.FSystemEvents.Count);
                    Assert.Equal(0, executer.FTCList.Count);
                    Assert.Equal(0, executer.OnEventsExecuted.Count);
                    Assert.Equal(0, executer.OnActivateTC.Count);
                    break;
                case TTokenManager manager:
                    Assert.Null(manager.Owner);
                    Assert.Equal(0, manager.Count);
                    Assert.Same(manager, manager.FTokenExecuter.FTokenManager);
                    Assert.Same(manager, manager.FTokenPackageDistributer.FTokenManager);
                    break;
                case TPlayerCommunicationControl communication:
                    Assert.Equal(1, communication.FRefCount);
                    Assert.Equal(0, communication.Count);
                    if (value is TRemotePCC remote) Assert.Equal(0, remote.FPackageDataList.Count);
                    if (value is TLocalPCC local) Assert.Equal(0, local.FPackageDataList.Count);
                    break;
                case TPackageDataList packages: Assert.Equal(0, packages.Count); break;
                case TTokenPackageDistributer distributer: Assert.Null(distributer.FTokenManager); break;
                case TSoundSystem sound: Assert.Equal(1, sound.FRefCount); break;
                case UnitSfxManager sfx: Assert.Equal(0, sfx.SFX.Count); break;
                case TSongPlayer song:
                    Assert.Equal(-1, song.CurrentIndex);
                    Assert.Equal(0, song.Songs.Count);
                    Assert.NotNull(song.Songs.OnChange);
                    Assert.Equal(0, song.SongNames.Count);
                    break;
                case TStringList strings: Assert.Equal(0, strings.Count); break;
                case TList list: Assert.Equal(0, list.Count); break;
            }
        }
        finally { value.Destroy(); }
    }

    [Fact]
    public void TokenControlPublicationHasNoLaterConstructorWork()
    {
        var manager = new TTokenManager(null!);
        var control = new TTokenControl(manager, 7);
        Assert.Same(control, manager.FTokenExecuter.FTCList[0]);
        Assert.Equal(1, manager.FTokenExecuter.FTCList.Count);
        control.Destroy();
        Assert.Equal(0, manager.FTokenExecuter.FTCList.Count);
        manager.Destroy();
    }

    [Fact]
    public void SfxComponentInsertionPrecedesPrivateListAllocation()
    {
        var owner = new ObservingComponent();
        var value = new UnitSfxManager(owner);
        Assert.True(owner.Observed);
        Assert.Same(owner, value.Owner);
        Assert.Equal(0, value.SFX.Count);
        value.Destroy();
        Assert.Equal(0, owner.ComponentCount);
    }

    private sealed class ObservingComponent : TComponent
    {
        internal bool Observed;

        public override void Notification(TComponent child, TOperation operation)
        {
            if (operation == TOperation.opInsert && child is UnitSfxManager manager)
            {
                Assert.Null(manager.FSFX);
                Observed = true;
            }
            base.Notification(child, operation);
        }
    }
}
