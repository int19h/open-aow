using Aow.Units.Animation;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWE;
using Aow.Units.AoWImage;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWRelations;
using Aow.Units.AoWWindow;
using Aow.Units.AoWWinMan;
using Aow.Units.AOWWinManager;
using Aow.Units.Bridge;
using Aow.Units.Engine;
using Aow.Units.HolyG;
using Aow.Units.HSEngine;
using Aow.Units.HxBridge;
using Aow.Units.Mountain;
using Aow.Units.PoisonP;
using Aow.Units.Sound;
using Aow.Units.Terrain;
using Aow.Units.UnHolyG;
using Aow.Units.UnitSpells;
using Game = Aow.Units.AoWE.AoWEUnit;
using HolySummoning = Aow.Units.HolyG.AnonymousEnum_hgSummoning;
using UnholySummoning = Aow.Units.UnHolyG.AnonymousEnum_hgSummoning;

namespace Aow.Units.Tests;

// Object casts that throw where the original unchecked cast kept running.
[Collection("Engine")]
[EngineState]
public sealed class ObjectCastCrashTests
{
    private sealed class MapScope : IDisposable
    {
        private readonly TAoWHSMap previous = Game.AoWHSMap;

        public MapScope()
        {
            Map = new TAoWHSMap(null!);
            var container = new TMapContainer(null!);
            var level = new TMapLevel(container);
            level.CreateMapFieldList(1, 1);
            container.FMap = Map;
            Map.FMapContainer = container;
            Field = container.GetField(0, 0, 0);
            Game.AoWHSMap = Map;
        }

        public TAoWHSMap Map { get; }

        public TMapField Field { get; }

        public void Dispose() => Game.AoWHSMap = previous;
    }

    private sealed class MasteryUnit : TAbstractUnit
    {
        public MasteryUnit() : base(null!)
        {
        }

        public override bool ExpandAbility(int AbilityID)
        {
            var data = new TUnitEnchantmentAbilityData(null!);
            data.FEnchantmentAbilityID = (ushort)AbilityID;
            data.FEnchantment = new TUnitEnchantment(null!);
            data.FEnchantment.FUpkeep = 5;
            data.FEnchantment.FSource = -3;
            AddAbilityData(data);
            return true;
        }
    }

    private sealed class RecordingRelWindow : TRelWindow
    {
        public RecordingRelWindow() : base(null!)
        {
        }

        public int ShownPlayer { get; private set; } = -2;

        public int ShowCount { get; private set; }

        public override void ShowPlayerActions(sbyte player)
        {
            ShowCount += 1;
            ShownPlayer = player;
        }
    }

    private sealed class ListedTerrain : TTerrainMO
    {
        public ListedTerrain(TMapObjectResourceList list) : base(null!)
        {
            Resources = list;
        }

        public TMapObjectResourceList Resources { get; }

        public bool Destroyed { get; private set; }

        public override TMapObjectResourceList GetResourceList(TEngine engine) => Resources;

        public override void Destroy() => Destroyed = true;
    }

    private static void Own(TMapObject sprite, TMapField field)
    {
        sprite.FResource = new TMapObjectResource(null!);
        sprite.Owner = field;
    }

    private static TAnimation AnimationAtOrigin()
    {
        var animation = new TAnimation(null!);
        animation.FXhx = 0;
        animation.FYhx = 0;
        animation.FLhx = 0;
        return animation;
    }

    [Fact]
    public void SummonFrameClearsTheModeFromTheField()
    {
        using var scope = new MapScope();
        var animation = AnimationAtOrigin();

        var holy = new THolyGround(null!);
        Own(holy, scope.Field);
        holy.FMode = TSet<HolySummoning>.Of(HolySummoning.hgSummoning);
        holy.NewFrameSummonAnimation(animation, 8);
        Assert.True(holy.FMode.Contains(HolySummoning.hgSummoning));
        holy.NewFrameSummonAnimation(animation, 9);
        Assert.False(holy.FMode.Contains(HolySummoning.hgSummoning));

        var unholy = new TUnHolyGround(null!);
        Own(unholy, scope.Field);
        unholy.FMode = TSet<UnholySummoning>.Of(UnholySummoning.hgSummoning);
        unholy.NewFrameSummonAnimation(animation, 8);
        Assert.True(unholy.FMode.Contains(UnholySummoning.hgSummoning));
        unholy.NewFrameSummonAnimation(animation, 9);
        Assert.False(unholy.FMode.Contains(UnholySummoning.hgSummoning));

        var plant = new TPoisonPlant(null!);
        Own(plant, scope.Field);
        plant.FPlantStatus = TSet<TPoisonPlantStatusElement>.Of(TPoisonPlantStatusElement.ppsSummoning);
        plant.NewFrameSummonAnimation(animation, 8);
        Assert.True(plant.FPlantStatus.Contains(TPoisonPlantStatusElement.ppsSummoning));
        plant.NewFrameSummonAnimation(animation, 9);
        Assert.False(plant.FPlantStatus.Contains(TPoisonPlantStatusElement.ppsSummoning));

        var mountain = new TRaisedMountain(null!);
        Own(mountain, scope.Field);
        mountain.FMode = TSet<AnonymousEnum_rmRaising>.Of(AnonymousEnum_rmRaising.rmRaising);
        mountain.NewFrameRaiseTerrainAnimation(animation, 8);
        Assert.True(mountain.FMode.Contains(AnonymousEnum_rmRaising.rmRaising));
        mountain.NewFrameRaiseTerrainAnimation(animation, 9);
        Assert.False(mountain.FMode.Contains(AnonymousEnum_rmRaising.rmRaising));
    }

    [Fact]
    public void MasterySettersWriteUpkeepSourceAndResistance()
    {
        var control = Game.AoWHSSet.FSpellControl;
        control.RegisterSpell(new TStoneSkin(null!));
        control.RegisterSpell(new TBless(null!));
        try
        {
            var unit = new MasteryUnit();
            unit.SetEarthMasteryStoneSkin();
            unit.SetFireMasteryProtection();
            unit.SetLifeMasteryBlessing();

            var earth = ((TUnitEnchantmentAbilityData)unit.GetAbilityData(153)).FEnchantment;
            var fire = ((TUnitEnchantmentAbilityData)unit.GetAbilityData(167)).FEnchantment;
            var life = ((TUnitEnchantmentAbilityData)unit.GetAbilityData(158)).FEnchantment;
            Assert.Equal(0, earth.FUpkeep);
            Assert.Equal(-2, earth.FSource);
            Assert.Equal(3, earth.FDispelResistance);
            Assert.Equal(0, fire.FUpkeep);
            Assert.Equal(-2, fire.FSource);
            Assert.Equal(7, fire.FDispelResistance);
            Assert.Equal(0, life.FUpkeep);
            Assert.Equal(-2, life.FSource);
            Assert.Equal(3, life.FDispelResistance);
        }
        finally
        {
            if (control.GetSpell(17) != null)
            {
                control.UnregisterSpellID(17);
            }

            if (control.GetSpell(56) != null)
            {
                control.UnregisterSpellID(56);
            }
        }
    }

    [Fact]
    public void TerrainScanSkipsANonTerrainResource()
    {
        int seed = Runtime.RandSeed;
        try
        {
            var list = new TMapObjectResourceList(null!);
            list.FChildren.Add(new TMapObjectResource(null!));
            var match = new TTerrainMOR(null!);
            match.FTerrainType = 4;
            match.FDecorationType = 2;
            list.FChildren.Add(match);

            var resource = new TTerrainMOR(null!);
            resource.FTerrainType = 4;
            resource.FDecorationType = 9;
            var finder = new ListedTerrain(list);
            finder.FResource = resource;
            Assert.Same(match, finder.FindDecorationResource(2));

            var clearing = new TMapObjectResourceList(null!);
            clearing.FChildren.Add(new TMapObjectResource(null!));
            var wide = new TTerrainMOR(null!);
            wide.TerrainList.SetTerrain(0, 1);
            wide.TerrainList.SetTerrain(1, 1);
            var terrain = new ListedTerrain(clearing);
            terrain.FResource = wide;
            terrain.ClearTerrain(null!, 0);
            Assert.True(terrain.Destroyed);
        }
        finally
        {
            Runtime.RandSeed = seed;
        }
    }

    [Fact]
    public void ShowRelationsSkipsAnImageAndChecksABox()
    {
        using var scope = new MapScope();
        var previousGeneral = AoWWinManUnit.General;
        var previousSound = SoundUnit.SoundSystem;
        SoundUnit.SoundSystem = null;
        AoWWinManUnit.General = new TGeneral(null!);
        AoWWinManUnit.General.Manager = new TAOWWinManager(null!);
        try
        {
            var window = new RecordingRelWindow();
            window.P1I = new TAOWImage(null!);
            window.P2I = new TAOWImage(null!);
            window.P3I = new TAOWImage(null!);
            window.P4I = new TAOWImage(null!);
            window.P5I = new TAOWImage(null!);
            window.P6I = new TAOWImage(null!);
            window.P7I = new TAOWImage(null!);
            window.P8I = new TAOWImage(null!);
            window.P9I = new TAOWImage(null!);
            window.P10I = new TAOWImage(null!);
            window.P11I = new TAOWImage(null!);
            window.P12I = new TAOWImage(null!);
            window.R1I = new TAOWImage(null!);
            window.R2I = new TAOWImage(null!);
            window.R3I = new TAOWImage(null!);
            window.R4I = new TAOWImage(null!);
            window.R5I = new TAOWImage(null!);
            window.R6I = new TAOWImage(null!);
            window.R7I = new TAOWImage(null!);
            window.R8I = new TAOWImage(null!);
            window.R9I = new TAOWImage(null!);
            window.R10I = new TAOWImage(null!);
            window.R11I = new TAOWImage(null!);
            window.R12I = new TAOWImage(null!);
            window.P1CB = new TAOWCheckBox(null!);
            window.P2CB = new TAOWCheckBox(null!);
            window.P3CB = new TAOWCheckBox(null!);
            window.P4CB = new TAOWCheckBox(null!);
            window.P5CB = new TAOWCheckBox(null!);
            window.P6CB = new TAOWCheckBox(null!);
            window.P7CB = new TAOWCheckBox(null!);
            window.P8CB = new TAOWCheckBox(null!);
            window.P9CB = new TAOWCheckBox(null!);
            window.P10CB = new TAOWCheckBox(null!);
            window.P11CB = new TAOWCheckBox(null!);
            window.P12CB = new TAOWCheckBox(null!);
            window.R1CB = new TAOWCheckBox(null!);
            window.R2CB = new TAOWCheckBox(null!);
            window.R3CB = new TAOWCheckBox(null!);
            window.R4CB = new TAOWCheckBox(null!);
            window.R5CB = new TAOWCheckBox(null!);
            window.R6CB = new TAOWCheckBox(null!);
            window.R7CB = new TAOWCheckBox(null!);
            window.R8CB = new TAOWCheckBox(null!);
            window.R9CB = new TAOWCheckBox(null!);
            window.R10CB = new TAOWCheckBox(null!);
            window.R11CB = new TAOWCheckBox(null!);
            window.R12CB = new TAOWCheckBox(null!);
            window.RelWin = new TAOWWindow(null!);
            window.RelPnl = new TAOWPanel(null!);
            window.PlyrGlow = new TAOWImage(null!);
            window.RNewActionPnl = new TAOWPanel(null!);
            window.RIncomingPnl = new TAOWPanel(null!);
            window.RMsgEditPnl = new TAOWPanel(null!);
            window.RSBribePnl = new TAOWPanel(null!);
            window.RMsgRecPnl = new TAOWPanel(null!);
            window.IncommingInfoPnl = new TAOWPanel(null!);
            window.ActionsLB = new TAOWListBox(null!);
            window.SMemo = new TAOWMemo(null!);
            window.MMemo = new TAOWMemo(null!);
            window.ASelect = new TAOWButton(null!);
            window.RelWindowCreate();

            window.ShowRelations(0);
            Assert.False(window.R12I.FChecked);
            Assert.False(window.P1CB.Checked);
            Assert.Equal(0, window.ShowCount);

            window.ShowRelations(1);
            Assert.False(window.R12I.FChecked);
            Assert.True(window.P1CB.Checked);
            Assert.False(window.RelPnl.FAligned);
            Assert.False(window.RelWin.FDrawn);
            Assert.Equal(0, window.ShowCount);

            var player = scope.Map.FPlayers.GetPlayers(1);
            player.FPlayerIndex = 1;
            player.FStatus = TPlayerStatus.psPlaying;
            scope.Map.FSeatedPlayer = 1;
            window.ShowRelations(1);
            Assert.Equal(1, window.ShowCount);
            Assert.Equal(1, window.ShownPlayer);
            Assert.True(window.P1CB.Checked);
            Assert.False(window.R12I.FChecked);
        }
        finally
        {
            AoWWinManUnit.General = previousGeneral;
            SoundUnit.SoundSystem = previousSound;
        }
    }

    [Fact]
    public void PlaceHXAcceptsANonSpriteOwner()
    {
        using (var scope = new MapScope())
        {
            var level = (TMapLevel)scope.Field.Owner;
            var marker = new TMapField(level);
            marker.FXhx = 20;
            marker.FYhx = 20;
            marker.OverlayType = 4;
            var holder = new EngineFixture.Owner();
            holder.FOwner = marker;
            var bridge = new THexagonBridge(holder);
            bridge.FResource = new TMapObjectResource(null!);
            Assert.False(bridge.PlaceHX(scope.Map.FMapContainer, 0, 0, 0));
            Assert.Same(scope.Field, bridge.Owner);
            Assert.Equal(-1, marker.OverlayType);
        }

        using (var scope = new MapScope())
        {
            var level = (TMapLevel)scope.Field.Owner;
            var marker = new TMapField(level);
            marker.FXhx = 20;
            marker.FYhx = 20;
            marker.OverlayType = 4;
            var holder = new EngineFixture.Owner();
            holder.FOwner = level;
            var bridge = new THexagonBridge(holder);
            bridge.FResource = new TMapObjectResource(null!);
            Assert.False(bridge.PlaceHX(scope.Map.FMapContainer, 0, 0, 0));
            Assert.Same(scope.Field, bridge.Owner);
            Assert.Equal(4, marker.OverlayType);
        }
    }

    [Fact]
    public void MovingAPlacedBridgeSkipsTheLevelOverlay()
    {
        TAoWHSMap previous = Game.AoWHSMap;
        try
        {
            var map = new TAoWHSMap(null!);
            var container = new TMapContainer(null!);
            var level = new TMapLevel(container);
            level.CreateMapFieldList(2, 1);
            container.FMap = map;
            map.FMapContainer = container;
            Game.AoWHSMap = map;
            var first = container.GetField(0, 0, 0);
            var second = container.GetField(1, 0, 0);
            var bridge = new TBridge(null!);
            bridge.FResource = new TBridgeResource(null!);
            Assert.True(bridge.PlaceHX(container, 0, 0, 0));
            Assert.Same(first, bridge.Owner);
            Assert.True(bridge.PlaceHX(container, 1, 0, 0));
            Assert.Same(second, bridge.Owner);
            Assert.Same(level, first.Owner);
        }
        finally
        {
            Game.AoWHSMap = previous;
        }
    }
}
