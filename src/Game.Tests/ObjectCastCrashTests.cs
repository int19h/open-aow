
using Aow.Game.Audio;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Abilities.Passive;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Dialogs;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Game.World.Terrain;
using HolySummoning = Aow.Game.Magic.Spells.HolySummoning;
using UnholySummoning = Aow.Game.Magic.Spells.UnholySummoning;

namespace Aow.Game.Tests;

// Object casts that throw where the original unchecked cast kept running.
[Collection("Engine")]
[EngineState]
public sealed class ObjectCastCrashTests
{
    private sealed class MapScope : IDisposable
    {
        private readonly AoWHSMap _previous = AoWHSMap.Current;

        public MapScope()
        {
            Map = new AoWHSMap(null!);
            var container = new MapContainer(null!);
            var level = new MapLevel(container);
            level.CreateMapFieldList(1, 1);
            container.Map = Map;
            Map.MapContainer = container;
            Field = container.GetField(0, 0, 0);
            AoWHSMap.Current = Map;
        }

        public AoWHSMap Map { get; }

        public MapField Field { get; }

        public void Dispose() => AoWHSMap.Current = _previous;
    }

    private sealed class MasteryUnit : Troop
    {
        public MasteryUnit() : base(null!)
        {
        }

        public override bool ExpandAbility(int abilityId)
        {
            var data = new UnitEnchantmentAbilityData(null!);
            data.EnchantmentAbilityId = (ushort)abilityId;
            data.Enchantment = new UnitEnchantment(null!);
            data.Enchantment.Upkeep = 5;
            data.Enchantment.Source = -3;
            AddAbilityData(data);
            return true;
        }
    }

    private sealed class RecordingRelationWindow : RelationWindow
    {
        public RecordingRelationWindow() : base(null!)
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

    private sealed class ListedTerrain : TerrainMapObject
    {
        public ListedTerrain(MapObjectResourceList list) : base(null!)
        {
            Resources = list;
        }

        public MapObjectResourceList Resources { get; }

        public bool Destroyed { get; private set; }

        public override MapObjectResourceList GetResourceList(GameEngine engine) => Resources;

        public override void Dispose() => Destroyed = true;
    }

    private static void Own(MapObject sprite, MapField field)
    {
        sprite._resource = new MapObjectResource(null!);
        sprite.Owner = field;
    }

    private static Animation AnimationAtOrigin()
    {
        var animation = new Animation(null!);
        animation._hexX = 0;
        animation._hexY = 0;
        animation._hexLevel = 0;
        return animation;
    }

    [Fact]
    public void SummonFrameClearsTheModeFromTheField()
    {
        using var scope = new MapScope();
        var animation = AnimationAtOrigin();

        var holy = new HolyGround(null!);
        Own(holy, scope.Field);
        holy.Mode = Set<HolySummoning>.Of(HolySummoning.Summoning);
        holy.NewFrameSummonAnimation(animation, 8);
        Assert.True(holy.Mode.Contains(HolySummoning.Summoning));
        holy.NewFrameSummonAnimation(animation, 9);
        Assert.False(holy.Mode.Contains(HolySummoning.Summoning));

        var unholy = new UnholyGround(null!);
        Own(unholy, scope.Field);
        unholy.Mode = Set<UnholySummoning>.Of(UnholySummoning.Summoning);
        unholy.NewFrameSummonAnimation(animation, 8);
        Assert.True(unholy.Mode.Contains(UnholySummoning.Summoning));
        unholy.NewFrameSummonAnimation(animation, 9);
        Assert.False(unholy.Mode.Contains(UnholySummoning.Summoning));

        var plant = new PoisonPlant(null!);
        Own(plant, scope.Field);
        plant.PlantStatus = Set<PoisonPlantStatusElement>.Of(PoisonPlantStatusElement.Summoning);
        plant.NewFrameSummonAnimation(animation, 8);
        Assert.True(plant.PlantStatus.Contains(PoisonPlantStatusElement.Summoning));
        plant.NewFrameSummonAnimation(animation, 9);
        Assert.False(plant.PlantStatus.Contains(PoisonPlantStatusElement.Summoning));

        var mountain = new RaisedMountain(null!);
        Own(mountain, scope.Field);
        mountain.Mode = Set<RaisedMountainMode>.Of(RaisedMountainMode.Raising);
        mountain.NewFrameRaiseTerrainAnimation(animation, 8);
        Assert.True(mountain.Mode.Contains(RaisedMountainMode.Raising));
        mountain.NewFrameRaiseTerrainAnimation(animation, 9);
        Assert.False(mountain.Mode.Contains(RaisedMountainMode.Raising));
    }

    [Fact]
    public void MasterySettersWriteUpkeepSourceAndResistance()
    {
        var control = AoWHSSet.Current.SpellControl;
        control.RegisterSpell(new StoneSkin(null!));
        control.RegisterSpell(new Bless(null!));
        try
        {
            var unit = new MasteryUnit();
            unit.SetEarthMasteryStoneSkin();
            unit.SetFireMasteryProtection();
            unit.SetLifeMasteryBlessing();

            var earth = ((UnitEnchantmentAbilityData)unit.GetAbilityData(153)).Enchantment;
            var fire = ((UnitEnchantmentAbilityData)unit.GetAbilityData(167)).Enchantment;
            var life = ((UnitEnchantmentAbilityData)unit.GetAbilityData(158)).Enchantment;
            Assert.Equal(0, earth.Upkeep);
            Assert.Equal(-2, earth.Source);
            Assert.Equal(3, earth.DispelResistance);
            Assert.Equal(0, fire.Upkeep);
            Assert.Equal(-2, fire.Source);
            Assert.Equal(7, fire.DispelResistance);
            Assert.Equal(0, life.Upkeep);
            Assert.Equal(-2, life.Source);
            Assert.Equal(3, life.DispelResistance);
        }
        finally
        {
            if (control.GetSpell(17) != null)
            {
                control.UnregisterSpellId(17);
            }

            if (control.GetSpell(56) != null)
            {
                control.UnregisterSpellId(56);
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
            list.AddChild(new MapObjectResource(null!));
            var match = new TerrainMapObjectResource(null!);
            match._terrainType = 4;
            match._decorationType = 2;
            list.AddChild(match);

            var resource = new TerrainMapObjectResource(null!);
            resource._terrainType = 4;
            resource._decorationType = 9;
            var finder = new ListedTerrain(list);
            finder._resource = resource;
            Assert.Same(match, finder.FindDecorationResource(2));

            var clearing = new MapObjectResourceList(null!);
            clearing.AddChild(new MapObjectResource(null!));
            var wide = new TerrainMapObjectResource(null!);
            wide.TerrainList.SetTerrain(0, 1);
            wide.TerrainList.SetTerrain(1, 1);
            var terrain = new ListedTerrain(clearing);
            terrain._resource = wide;
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
        var previousGeneral = DialogHosts.General;
        var previousSound = AudioHost.SoundSystem;
        AudioHost.SoundSystem = null;
        DialogHosts.General = new General(null!);
        DialogHosts.General.Manager = new WindowManager(null!);
        try
        {
            var window = new RecordingRelationWindow();
            window.P1I = new Image(null!);
            window.P2I = new Image(null!);
            window.P3I = new Image(null!);
            window.P4I = new Image(null!);
            window.P5I = new Image(null!);
            window.P6I = new Image(null!);
            window.P7I = new Image(null!);
            window.P8I = new Image(null!);
            window.P9I = new Image(null!);
            window.P10I = new Image(null!);
            window.P11I = new Image(null!);
            window.P12I = new Image(null!);
            window.R1I = new Image(null!);
            window.R2I = new Image(null!);
            window.R3I = new Image(null!);
            window.R4I = new Image(null!);
            window.R5I = new Image(null!);
            window.R6I = new Image(null!);
            window.R7I = new Image(null!);
            window.R8I = new Image(null!);
            window.R9I = new Image(null!);
            window.R10I = new Image(null!);
            window.R11I = new Image(null!);
            window.R12I = new Image(null!);
            window.P1CB = new CheckBox(null!);
            window.P2CB = new CheckBox(null!);
            window.P3CB = new CheckBox(null!);
            window.P4CB = new CheckBox(null!);
            window.P5CB = new CheckBox(null!);
            window.P6CB = new CheckBox(null!);
            window.P7CB = new CheckBox(null!);
            window.P8CB = new CheckBox(null!);
            window.P9CB = new CheckBox(null!);
            window.P10CB = new CheckBox(null!);
            window.P11CB = new CheckBox(null!);
            window.P12CB = new CheckBox(null!);
            window.R1CB = new CheckBox(null!);
            window.R2CB = new CheckBox(null!);
            window.R3CB = new CheckBox(null!);
            window.R4CB = new CheckBox(null!);
            window.R5CB = new CheckBox(null!);
            window.R6CB = new CheckBox(null!);
            window.R7CB = new CheckBox(null!);
            window.R8CB = new CheckBox(null!);
            window.R9CB = new CheckBox(null!);
            window.R10CB = new CheckBox(null!);
            window.R11CB = new CheckBox(null!);
            window.R12CB = new CheckBox(null!);
            window.RelWin = new Window(null!);
            window.RelPnl = new Panel(null!);
            window.PlyrGlow = new Image(null!);
            window.RNewActionPnl = new Panel(null!);
            window.RIncomingPnl = new Panel(null!);
            window.RMsgEditPnl = new Panel(null!);
            window.RSBribePnl = new Panel(null!);
            window.RMsgRecPnl = new Panel(null!);
            window.IncommingInfoPnl = new Panel(null!);
            window.ActionsLB = new ListBox(null!);
            window.SMemo = new Memo(null!);
            window.MMemo = new Memo(null!);
            window.ASelect = new Button(null!);
            window.RelWindowCreate();

            window.ShowRelations(0);
            Assert.False(window.R12I._checked);
            Assert.False(window.P1CB.Checked);
            Assert.Equal(0, window.ShowCount);

            window.ShowRelations(1);
            Assert.False(window.R12I._checked);
            Assert.True(window.P1CB.Checked);
            Assert.False(window.RelPnl.Aligned);
            Assert.False(window.RelWin.Drawn);
            Assert.Equal(0, window.ShowCount);

            var player = scope.Map.Players.GetPlayers(1);
            player.PlayerIndex = 1;
            player.Status = PlayerStatus.Playing;
            scope.Map.SeatedPlayer = 1;
            window.ShowRelations(1);
            Assert.Equal(1, window.ShowCount);
            Assert.Equal(1, window.ShownPlayer);
            Assert.True(window.P1CB.Checked);
            Assert.False(window.R12I._checked);
        }
        finally
        {
            DialogHosts.General = previousGeneral;
            AudioHost.SoundSystem = previousSound;
        }
    }

    // The original refreshes the overlay of the previous field's owner read as a field. A sprite's
    // owner is always a field and a field's owner a level, so nothing is refreshed (issue 158).
    [Fact]
    public void PlaceHexRefreshesNoFieldThroughThePreviousFieldsLevel()
    {
        using (var scope = new MapScope())
        {
            var level = scope.Field.Owner;
            var marker = new MapField(level);
            marker.HexX = 20;
            marker.HexY = 20;
            marker.OverlayType = 4;
            var bridge = new HexBridge(marker);
            bridge._resource = new MapObjectResource(null!);
            Assert.False(bridge.PlaceHex(scope.Map.MapContainer, 0, 0, 0));
            Assert.Same(scope.Field, bridge.Owner);
            Assert.Equal(4, marker.OverlayType);
        }
    }

    [Fact]
    public void MasteryGuardianDefaultsNeedNoSummonedSpell()
    {
        // MasterySpellCast and TSummonSpell read these through GetSpell(155), which has no spell.
        Assert.Null(AoWHSSet.Current.SpellControl.GetSpell(155));
        var unit = new Unit(null!);
        unit.UnitResource = new UnitResource(null!) { Level = 2 };
        Assert.Equal(6, SummonedAbility.GetDefaultManaUpkeep(unit));
        Assert.Equal(5, SummonedAbility.GetDefaultDispelResistance(unit));
        Assert.Equal(5, new SummonSyron(null!).GetDispelResistance(unit));
        Assert.Equal(0, SummonedAbility.GetDefaultManaUpkeep(new MasteryUnit()));
    }

    [Fact]
    public void MovingAPlacedBridgeSkipsTheLevelOverlay()
    {
        AoWHSMap previous = AoWHSMap.Current;
        try
        {
            var map = new AoWHSMap(null!);
            var container = new MapContainer(null!);
            var level = new MapLevel(container);
            level.CreateMapFieldList(2, 1);
            container.Map = map;
            map.MapContainer = container;
            AoWHSMap.Current = map;
            var first = container.GetField(0, 0, 0);
            var second = container.GetField(1, 0, 0);
            var bridge = new Bridge(null!);
            bridge._resource = new Bridge.Resource(null!);
            Assert.True(bridge.PlaceHex(container, 0, 0, 0));
            Assert.Same(first, bridge.Owner);
            Assert.True(bridge.PlaceHex(container, 1, 0, 0));
            Assert.Same(second, bridge.Owner);
            Assert.Same(level, first.Owner);
        }
        finally
        {
            AoWHSMap.Current = previous;
        }
    }
}
