using Aow.Game.Audio;
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Abilities.Passive;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Summon;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Game.World.Terrain;
using HolySummoning = Aow.Game.Magic.Spells.HolySummoning;
using UnholySummoning = Aow.Game.Magic.Spells.UnholySummoning;

namespace Aow.Units.Tests;

// Object casts that throw where the original unchecked cast kept running.
[Collection("Engine")]
[EngineState]
public sealed class ObjectCastCrashTests
{
    private sealed class MapScope : IDisposable
    {
        private readonly AoWHSMap previous = Aow.Game.World.Map.AoWHSMap.Current;

        public MapScope()
        {
            Map = new AoWHSMap(null!);
            var container = new MapContainer(null!);
            var level = new MapLevel(container);
            level.CreateMapFieldList(1, 1);
            container.FMap = Map;
            Map.FMapContainer = container;
            Field = container.GetField(0, 0, 0);
            Aow.Game.World.Map.AoWHSMap.Current = Map;
        }

        public AoWHSMap Map { get; }

        public MapField Field { get; }

        public void Dispose() => Aow.Game.World.Map.AoWHSMap.Current = previous;
    }

    private sealed class MasteryUnit : AbstractUnit
    {
        public MasteryUnit() : base(null!)
        {
        }

        public override bool ExpandAbility(int AbilityID)
        {
            var data = new UnitEnchantmentAbilityData(null!);
            data.FEnchantmentAbilityID = (ushort)AbilityID;
            data.FEnchantment = new UnitEnchantment(null!);
            data.FEnchantment.FUpkeep = 5;
            data.FEnchantment.FSource = -3;
            AddAbilityData(data);
            return true;
        }
    }

    private sealed class RecordingRelWindow : RelWindow
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

    private sealed class ListedTerrain : TerrainMO
    {
        public ListedTerrain(MapObjectResourceList list) : base(null!)
        {
            Resources = list;
        }

        public MapObjectResourceList Resources { get; }

        public bool Destroyed { get; private set; }

        public override MapObjectResourceList GetResourceList(Engine engine) => Resources;

        public override void Dispose() => Destroyed = true;
    }

    private static void Own(MapObject sprite, MapField field)
    {
        sprite.FResource = new MapObjectResource(null!);
        sprite.Owner = field;
    }

    private static Animation AnimationAtOrigin()
    {
        var animation = new Animation(null!);
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

        var holy = new HolyGround(null!);
        Own(holy, scope.Field);
        holy.FMode = Set<HolySummoning>.Of(HolySummoning.Summoning);
        holy.NewFrameSummonAnimation(animation, 8);
        Assert.True(holy.FMode.Contains(HolySummoning.Summoning));
        holy.NewFrameSummonAnimation(animation, 9);
        Assert.False(holy.FMode.Contains(HolySummoning.Summoning));

        var unholy = new UnHolyGround(null!);
        Own(unholy, scope.Field);
        unholy.FMode = Set<UnholySummoning>.Of(UnholySummoning.Summoning);
        unholy.NewFrameSummonAnimation(animation, 8);
        Assert.True(unholy.FMode.Contains(UnholySummoning.Summoning));
        unholy.NewFrameSummonAnimation(animation, 9);
        Assert.False(unholy.FMode.Contains(UnholySummoning.Summoning));

        var plant = new PoisonPlant(null!);
        Own(plant, scope.Field);
        plant.FPlantStatus = Set<PoisonPlantStatusElement>.Of(PoisonPlantStatusElement.Summoning);
        plant.NewFrameSummonAnimation(animation, 8);
        Assert.True(plant.FPlantStatus.Contains(PoisonPlantStatusElement.Summoning));
        plant.NewFrameSummonAnimation(animation, 9);
        Assert.False(plant.FPlantStatus.Contains(PoisonPlantStatusElement.Summoning));

        var mountain = new RaisedMountain(null!);
        Own(mountain, scope.Field);
        mountain.FMode = Set<AnonymousEnum_rmRaising>.Of(AnonymousEnum_rmRaising.Raising);
        mountain.NewFrameRaiseTerrainAnimation(animation, 8);
        Assert.True(mountain.FMode.Contains(AnonymousEnum_rmRaising.Raising));
        mountain.NewFrameRaiseTerrainAnimation(animation, 9);
        Assert.False(mountain.FMode.Contains(AnonymousEnum_rmRaising.Raising));
    }

    [Fact]
    public void MasterySettersWriteUpkeepSourceAndResistance()
    {
        var control = Aow.Game.Engine.AoWHSSet.Current.FSpellControl;
        control.RegisterSpell(new StoneSkin(null!));
        control.RegisterSpell(new Bless(null!));
        try
        {
            var unit = new MasteryUnit();
            unit.SetEarthMasteryStoneSkin();
            unit.SetFireMasteryProtection();
            unit.SetLifeMasteryBlessing();

            var earth = ((UnitEnchantmentAbilityData)unit.GetAbilityData(153)).FEnchantment;
            var fire = ((UnitEnchantmentAbilityData)unit.GetAbilityData(167)).FEnchantment;
            var life = ((UnitEnchantmentAbilityData)unit.GetAbilityData(158)).FEnchantment;
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
            var list = new MapObjectResourceList(null!);
            list.FChildren.Add(new MapObjectResource(null!));
            var match = new TerrainMOR(null!);
            match.FTerrainType = 4;
            match.FDecorationType = 2;
            list.FChildren.Add(match);

            var resource = new TerrainMOR(null!);
            resource.FTerrainType = 4;
            resource.FDecorationType = 9;
            var finder = new ListedTerrain(list);
            finder.FResource = resource;
            Assert.Same(match, finder.FindDecorationResource(2));

            var clearing = new MapObjectResourceList(null!);
            clearing.FChildren.Add(new MapObjectResource(null!));
            var wide = new TerrainMOR(null!);
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
        var previousGeneral = Aow.Game.Ui.Dialogs.DialogHosts.General;
        var previousSound = Aow.Game.Audio.AudioHost.SoundSystem;
        Aow.Game.Audio.AudioHost.SoundSystem = null;
        Aow.Game.Ui.Dialogs.DialogHosts.General = new General(null!);
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager = new AOWWinManager(null!);
        try
        {
            var window = new RecordingRelWindow();
            window.P1I = new AOWImage(null!);
            window.P2I = new AOWImage(null!);
            window.P3I = new AOWImage(null!);
            window.P4I = new AOWImage(null!);
            window.P5I = new AOWImage(null!);
            window.P6I = new AOWImage(null!);
            window.P7I = new AOWImage(null!);
            window.P8I = new AOWImage(null!);
            window.P9I = new AOWImage(null!);
            window.P10I = new AOWImage(null!);
            window.P11I = new AOWImage(null!);
            window.P12I = new AOWImage(null!);
            window.R1I = new AOWImage(null!);
            window.R2I = new AOWImage(null!);
            window.R3I = new AOWImage(null!);
            window.R4I = new AOWImage(null!);
            window.R5I = new AOWImage(null!);
            window.R6I = new AOWImage(null!);
            window.R7I = new AOWImage(null!);
            window.R8I = new AOWImage(null!);
            window.R9I = new AOWImage(null!);
            window.R10I = new AOWImage(null!);
            window.R11I = new AOWImage(null!);
            window.R12I = new AOWImage(null!);
            window.P1CB = new AOWCheckBox(null!);
            window.P2CB = new AOWCheckBox(null!);
            window.P3CB = new AOWCheckBox(null!);
            window.P4CB = new AOWCheckBox(null!);
            window.P5CB = new AOWCheckBox(null!);
            window.P6CB = new AOWCheckBox(null!);
            window.P7CB = new AOWCheckBox(null!);
            window.P8CB = new AOWCheckBox(null!);
            window.P9CB = new AOWCheckBox(null!);
            window.P10CB = new AOWCheckBox(null!);
            window.P11CB = new AOWCheckBox(null!);
            window.P12CB = new AOWCheckBox(null!);
            window.R1CB = new AOWCheckBox(null!);
            window.R2CB = new AOWCheckBox(null!);
            window.R3CB = new AOWCheckBox(null!);
            window.R4CB = new AOWCheckBox(null!);
            window.R5CB = new AOWCheckBox(null!);
            window.R6CB = new AOWCheckBox(null!);
            window.R7CB = new AOWCheckBox(null!);
            window.R8CB = new AOWCheckBox(null!);
            window.R9CB = new AOWCheckBox(null!);
            window.R10CB = new AOWCheckBox(null!);
            window.R11CB = new AOWCheckBox(null!);
            window.R12CB = new AOWCheckBox(null!);
            window.RelWin = new AOWWindow(null!);
            window.RelPnl = new AOWPanel(null!);
            window.PlyrGlow = new AOWImage(null!);
            window.RNewActionPnl = new AOWPanel(null!);
            window.RIncomingPnl = new AOWPanel(null!);
            window.RMsgEditPnl = new AOWPanel(null!);
            window.RSBribePnl = new AOWPanel(null!);
            window.RMsgRecPnl = new AOWPanel(null!);
            window.IncommingInfoPnl = new AOWPanel(null!);
            window.ActionsLB = new AOWListBox(null!);
            window.SMemo = new AOWMemo(null!);
            window.MMemo = new AOWMemo(null!);
            window.ASelect = new AOWButton(null!);
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
            player.FStatus = Aow.Game.Session.Player.Status.Playing;
            scope.Map.FSeatedPlayer = 1;
            window.ShowRelations(1);
            Assert.Equal(1, window.ShowCount);
            Assert.Equal(1, window.ShownPlayer);
            Assert.True(window.P1CB.Checked);
            Assert.False(window.R12I.FChecked);
        }
        finally
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General = previousGeneral;
            Aow.Game.Audio.AudioHost.SoundSystem = previousSound;
        }
    }

    [Fact]
    public void PlaceHXAcceptsANonSpriteOwner()
    {
        using (var scope = new MapScope())
        {
            var level = (MapLevel)scope.Field.Owner;
            var marker = new MapField(level);
            marker.FXhx = 20;
            marker.FYhx = 20;
            marker.OverlayType = 4;
            var holder = new EngineFixture.Owner();
            holder.FOwner = marker;
            var bridge = new HexagonBridge(holder);
            bridge.FResource = new MapObjectResource(null!);
            Assert.False(bridge.PlaceHX(scope.Map.FMapContainer, 0, 0, 0));
            Assert.Same(scope.Field, bridge.Owner);
            Assert.Equal(-1, marker.OverlayType);
        }

        using (var scope = new MapScope())
        {
            var level = (MapLevel)scope.Field.Owner;
            var marker = new MapField(level);
            marker.FXhx = 20;
            marker.FYhx = 20;
            marker.OverlayType = 4;
            var holder = new EngineFixture.Owner();
            holder.FOwner = level;
            var bridge = new HexagonBridge(holder);
            bridge.FResource = new MapObjectResource(null!);
            Assert.False(bridge.PlaceHX(scope.Map.FMapContainer, 0, 0, 0));
            Assert.Same(scope.Field, bridge.Owner);
            Assert.Equal(4, marker.OverlayType);
        }
    }

    [Fact]
    public void MasteryGuardianDefaultsNeedNoSummonedSpell()
    {
        // MasterySpellCast and TSummonSpell read these through GetSpell(155), which has no spell.
        Assert.Null(Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(155));
        var unit = new Unit(null!);
        unit.FUnitResource = new UnitResource(null!) { FLevel = 2 };
        Assert.Equal(6, SummonedAbility.GetDefaultManaUpkeep(unit));
        Assert.Equal(5, SummonedAbility.GetDefaultDispelResistance(unit));
        Assert.Equal(5, new SummonSyron(null!).GetDispelResistance(unit));
        Assert.Equal(0, SummonedAbility.GetDefaultManaUpkeep(new MasteryUnit()));
    }

    [Fact]
    public void MovingAPlacedBridgeSkipsTheLevelOverlay()
    {
        AoWHSMap previous = Aow.Game.World.Map.AoWHSMap.Current;
        try
        {
            var map = new AoWHSMap(null!);
            var container = new MapContainer(null!);
            var level = new MapLevel(container);
            level.CreateMapFieldList(2, 1);
            container.FMap = map;
            map.FMapContainer = container;
            Aow.Game.World.Map.AoWHSMap.Current = map;
            var first = container.GetField(0, 0, 0);
            var second = container.GetField(1, 0, 0);
            var bridge = new Bridge(null!);
            bridge.FResource = new Aow.Game.World.Bridge.Resource(null!);
            Assert.True(bridge.PlaceHX(container, 0, 0, 0));
            Assert.Same(first, bridge.Owner);
            Assert.True(bridge.PlaceHX(container, 1, 0, 0));
            Assert.Same(second, bridge.Owner);
            Assert.Same(level, first.Owner);
        }
        finally
        {
            Aow.Game.World.Map.AoWHSMap.Current = previous;
        }
    }
}
