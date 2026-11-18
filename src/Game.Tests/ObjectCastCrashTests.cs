#nullable disable
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
        private readonly StrategicMap _previous = StrategicMap.Current;

        public MapScope()
        {
            Map = new StrategicMap(null, holder: EngineFixture.Engine);
            MapContainer container = new(null) { Holder = EngineFixture.Engine };
            MapLevel level = new(container);
            level.CreateMapFieldList(1, 1);
            container.Map = Map;
            Map.MapContainer = container;
            Field = container.GetField(0, 0, 0);
            StrategicMap.Current = Map;
        }

        public StrategicMap Map { get; }

        public MapField Field { get; }

        public void Dispose() => StrategicMap.Current = _previous;
    }

    private sealed class MasteryUnit : Troop
    {
        public MasteryUnit() : base(null)
        {
        }

        public override bool ExpandAbility(int abilityId)
        {
            UnitEnchantmentAbilityData data = new(null)
            {
                Holder = EngineFixture.Engine,
                EnchantmentAbilityId = (ushort)abilityId,
                Enchantment = new UnitEnchantment(null)
                {
                    Holder = EngineFixture.Engine,
                    Upkeep = 5,
                    Source = -3
                }
            };
            AddAbilityData(data);
            return true;
        }
    }

    private sealed class RecordingRelationWindow(UIContext ui) : RelationWindow(ui)
    {
        public int ShownPlayer { get; private set; } = -2;

        public int ShowCount { get; private set; }

        public override void ShowPlayerActions(sbyte player)
        {
            ShowCount += 1;
            ShownPlayer = player;
        }
    }

    private sealed class ListedTerrain(MapObjectResourceList list) : TerrainMapObject(null)
    {
        public MapObjectResourceList Resources { get; } = list;

        public bool Destroyed { get; private set; }

        public override MapObjectResourceList GetResourceList(GameEngine engine) => Resources;

        public override void Dispose() => Destroyed = true;
    }

    private static void Own(MapObject sprite, MapField field)
    {
        sprite.SetResourceWithoutOverrides(new MapObjectResource(null) { Holder = EngineFixture.Engine });
        sprite.Owner = field;
    }

    private static Animation AnimationAtOrigin()
    {
        Animation animation = new(null)
        {
            Holder = EngineFixture.Engine,
            Location = new MapCoordinate(0, 0, 0)
        };
        return animation;
    }

    [Fact]
    public void SummonFrameClearsTheModeFromTheField()
    {
        using MapScope scope = new();
        Animation animation = AnimationAtOrigin();

        HolyGround holy = new(null) { Holder = EngineFixture.Engine };
        Own(holy, scope.Field);
        holy.Mode = Set<HolySummoning>.Of(HolySummoning.Summoning);
        holy.NewFrameSummonAnimation(animation, 8);
        Assert.True(holy.Mode.Contains(HolySummoning.Summoning));
        holy.NewFrameSummonAnimation(animation, 9);
        Assert.False(holy.Mode.Contains(HolySummoning.Summoning));

        UnholyGround unholy = new(null) { Holder = EngineFixture.Engine };
        Own(unholy, scope.Field);
        unholy.Mode = Set<UnholySummoning>.Of(UnholySummoning.Summoning);
        unholy.NewFrameSummonAnimation(animation, 8);
        Assert.True(unholy.Mode.Contains(UnholySummoning.Summoning));
        unholy.NewFrameSummonAnimation(animation, 9);
        Assert.False(unholy.Mode.Contains(UnholySummoning.Summoning));

        PoisonPlant plant = new(null) { Holder = EngineFixture.Engine };
        Own(plant, scope.Field);
        plant.PlantStatus = Set<PoisonPlantStatusElement>.Of(PoisonPlantStatusElement.Summoning);
        plant.NewFrameSummonAnimation(animation, 8);
        Assert.True(plant.PlantStatus.Contains(PoisonPlantStatusElement.Summoning));
        plant.NewFrameSummonAnimation(animation, 9);
        Assert.False(plant.PlantStatus.Contains(PoisonPlantStatusElement.Summoning));

        RaisedMountain mountain = new(null) { Holder = EngineFixture.Engine };
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
        SpellControl control = GamePack.Current.SpellControl;
        control.RegisterSpell(new StoneSkin(null, holder: EngineFixture.Engine));
        control.RegisterSpell(new Bless(null, holder: EngineFixture.Engine));
        try
        {
            MasteryUnit unit = new();
            unit.SetEarthMasteryStoneSkin();
            unit.SetFireMasteryProtection();
            unit.SetLifeMasteryBlessing();

            UnitEnchantment earth = ((UnitEnchantmentAbilityData)unit.GetAbilityData(153)).Enchantment;
            UnitEnchantment fire = ((UnitEnchantmentAbilityData)unit.GetAbilityData(167)).Enchantment;
            UnitEnchantment life = ((UnitEnchantmentAbilityData)unit.GetAbilityData(158)).Enchantment;
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
        int seed = DelphiRandom.RandSeed;
        try
        {
            MapObjectResourceList list = new(null) { Holder = EngineFixture.Engine };
            _ = list.AddChild(new MapObjectResource(null) { Holder = EngineFixture.Engine });
            TerrainMapObjectResource match = new(null)
            {
                Holder = EngineFixture.Engine,
                TerrainType = 4,
                DecorationType = 2
            };
            _ = list.AddChild(match);

            TerrainMapObjectResource resource = new(null)
            {
                Holder = EngineFixture.Engine,
                TerrainType = 4,
                DecorationType = 9
            };
            ListedTerrain finder = new(list);
            finder.SetResourceWithoutOverrides(resource);
            Assert.Same(match, finder.FindDecorationResource(2));

            MapObjectResourceList clearing = new(null) { Holder = EngineFixture.Engine };
            _ = clearing.AddChild(new MapObjectResource(null) { Holder = EngineFixture.Engine });
            TerrainMapObjectResource wide = new(null) { Holder = EngineFixture.Engine };
            wide.TerrainList.SetTerrain(0, 1);
            wide.TerrainList.SetTerrain(1, 1);
            ListedTerrain terrain = new(clearing);
            terrain.SetResourceWithoutOverrides(wide);
            terrain.ClearTerrain(null, 0);
            Assert.True(terrain.Destroyed);
        }
        finally
        {
            DelphiRandom.RandSeed = seed;
        }
    }

    [Fact]
    public void ShowRelationsSkipsAnImageAndChecksABox()
    {
        using MapScope scope = new();
        SoundSystem previousSound = AudioHost.SoundSystem;
        AudioHost.SoundSystem = null;
        UIContext ui = new(EngineFixture.Engine);
        ui.Add(new General(ui)).Manager = new WindowManager(null);
        try
        {
            RecordingRelationWindow window = new(ui)
            {
                Player1Image = new Image(null),
                Player2Image = new Image(null),
                Player3Image = new Image(null),
                Player4Image = new Image(null),
                Player5Image = new Image(null),
                Player6Image = new Image(null),
                Player7Image = new Image(null),
                Player8Image = new Image(null),
                Player9Image = new Image(null),
                Player10Image = new Image(null),
                Player11Image = new Image(null),
                Player12Image = new Image(null),
                Race1Image = new Image(null),
                Race2Image = new Image(null),
                Race3Image = new Image(null),
                Race4Image = new Image(null),
                Race5Image = new Image(null),
                Race6Image = new Image(null),
                Race7Image = new Image(null),
                Race8Image = new Image(null),
                Race9Image = new Image(null),
                Race10Image = new Image(null),
                Race11Image = new Image(null),
                Race12Image = new Image(null),
                Player1CheckBox = new CheckBox(null),
                Player2CheckBox = new CheckBox(null),
                Player3CheckBox = new CheckBox(null),
                Player4CheckBox = new CheckBox(null),
                Player5CheckBox = new CheckBox(null),
                Player6CheckBox = new CheckBox(null),
                Player7CheckBox = new CheckBox(null),
                Player8CheckBox = new CheckBox(null),
                Player9CheckBox = new CheckBox(null),
                Player10CheckBox = new CheckBox(null),
                Player11CheckBox = new CheckBox(null),
                Player12CheckBox = new CheckBox(null),
                Race1CheckBox = new CheckBox(null),
                Race2CheckBox = new CheckBox(null),
                Race3CheckBox = new CheckBox(null),
                Race4CheckBox = new CheckBox(null),
                Race5CheckBox = new CheckBox(null),
                Race6CheckBox = new CheckBox(null),
                Race7CheckBox = new CheckBox(null),
                Race8CheckBox = new CheckBox(null),
                Race9CheckBox = new CheckBox(null),
                Race10CheckBox = new CheckBox(null),
                Race11CheckBox = new CheckBox(null),
                Race12CheckBox = new CheckBox(null),
                MainWindow = new Window(null),
                RelationPanel = new Panel(null),
                PlayerGlow = new Image(null),
                NewActionPanel = new Panel(null),
                IncomingPanel = new Panel(null),
                MessageEditPanel = new Panel(null),
                SendBribePanel = new Panel(null),
                ReceivedMessagePanel = new Panel(null),
                IncomingInfoPanel = new Panel(null),
                ActionsList = new ListBox<DiplomaticAction>(null),
                SendMessageMemo = new Memo(null),
                IncomingMemo = new Memo(null),
                SelectActionButton = new Button(null)
            };
            window.RelationWindowCreate();

            window.ShowRelations(0);
            Assert.False(window.Race12Image.Checked);
            Assert.False(window.Player1CheckBox.Checked);
            Assert.Equal(0, window.ShowCount);

            window.ShowRelations(1);
            Assert.False(window.Race12Image.Checked);
            Assert.True(window.Player1CheckBox.Checked);
            Assert.False(window.RelationPanel.Aligned);
            Assert.False(window.MainWindow.Drawn);
            Assert.Equal(0, window.ShowCount);

            Player player = scope.Map.Players.GetPlayers(1);
            player.PlayerIndex = 1;
            player.Status = PlayerStatus.Playing;
            scope.Map.SeatedPlayer = 1;
            window.ShowRelations(1);
            Assert.Equal(1, window.ShowCount);
            Assert.Equal(1, window.ShownPlayer);
            Assert.True(window.Player1CheckBox.Checked);
            Assert.False(window.Race12Image.Checked);
        }
        finally
        {
            AudioHost.SoundSystem = previousSound;
        }
    }

    // The original refreshes the overlay of the previous field's owner read as a field. A sprite's
    // owner is always a field and a field's owner a level, so nothing is refreshed (issue 158).
    [Fact]
    public void PlaceHexRefreshesNoFieldThroughThePreviousFieldsLevel()
    {
        using MapScope scope = new();
        MapLevel level = scope.Field.Owner;
        MapField marker = new(level)
        {
            HexX = 20,
            HexY = 20,
            OverlayType = 4
        };
        HexBridge bridge = new(marker);
        bridge.SetResourceWithoutOverrides(new MapObjectResource(null) { Holder = EngineFixture.Engine });
        Assert.False(bridge.PlaceHex(scope.Map.MapContainer, 0, 0, 0));
        Assert.Same(scope.Field, bridge.Owner);
        Assert.Equal(4, marker.OverlayType);
    }

    [Fact]
    public void MasteryGuardianDefaultsNeedNoSummonedSpell()
    {
        // MasterySpellCast and TSummonSpell read these through GetSpell(155), which has no spell.
        Assert.Null(GamePack.Current.SpellControl.GetSpell(155));
        Unit unit = new(null)
        {
            Holder = EngineFixture.Engine,
            UnitResource = new UnitResource(null) { Holder = EngineFixture.Engine, Level = 2 }
        };
        Assert.Equal(6, SummonedAbility.GetDefaultManaUpkeep(unit));
        Assert.Equal(5, SummonedAbility.GetDefaultDispelResistance(unit));
        Assert.Equal(5, new SummonSyron(null, holder: EngineFixture.Engine).GetDispelResistance(unit));
        Assert.Equal(0, SummonedAbility.GetDefaultManaUpkeep(new MasteryUnit()));
    }

    [Fact]
    public void MovingAPlacedBridgeSkipsTheLevelOverlay()
    {
        StrategicMap previous = StrategicMap.Current;
        try
        {
            StrategicMap map = new(null, holder: EngineFixture.Engine);
            MapContainer container = new(null) { Holder = EngineFixture.Engine };
            MapLevel level = new(container);
            level.CreateMapFieldList(2, 1);
            container.Map = map;
            map.MapContainer = container;
            StrategicMap.Current = map;
            MapField first = container.GetField(0, 0, 0);
            MapField second = container.GetField(1, 0, 0);
            Bridge bridge = new(null) { Holder = EngineFixture.Engine };
            bridge.SetResourceWithoutOverrides(new Bridge.Resource(null) { Holder = EngineFixture.Engine });
            Assert.True(bridge.PlaceHex(container, 0, 0, 0));
            Assert.Same(first, bridge.Owner);
            Assert.True(bridge.PlaceHex(container, 1, 0, 0));
            Assert.Same(second, bridge.Owner);
            Assert.Same(level, first.Owner);
        }
        finally
        {
            StrategicMap.Current = previous;
        }
    }
}
