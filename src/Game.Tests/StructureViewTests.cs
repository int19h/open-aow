#nullable disable
using System.Reflection;
using System.Text;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Magic;
using Aow.Game.UI.Dialogs;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Xunit.Abstractions;

namespace Aow.Game.Tests;

// Brief 9.6-2: the structure view virtuals against the cascades and the class id switch they
// replaced. The Old* methods copy those cascades and that switch from main 9ee2ef8, with each screen
// call replaced by the name of its view key and each kind action by "Action(<kind>)".
[Collection("Engine")]
[EngineState]
public sealed class StructureViewTests(ITestOutputHelper output)
{
    private const sbyte Viewer = 1;
    private const sbyte OtherPlayer = 2;

    private static Type[] StructureTypes() => [.. EngineClass.All
        .Where(c => typeof(Structure).IsAssignableFrom(c.Type))
        .Select(c => c.Type).OrderBy(t => t.Name, StringComparer.Ordinal)];

    // The original's army query on the structure's field tells that a kind action ran. No army
    // stands on the field, so every action stops at that query. A structure's owner is a field.
    private sealed class ArmyOwner() : EngineFixture.RecordingField
    {
        internal int ArmyQueries { get; private set; }

        public override EngineObject FindOwnedChild(int classId)
        {
            if (classId == ArmyHexSprite.ClassId)
            {
                ArmyQueries++;
            }

            return null;
        }
    }

    private sealed record State(bool Owned, bool Debug, bool Building, bool Razed, bool War)
    {
        public override string ToString()
            => $"{(Owned ? "owned" : "other")}{(Debug ? ",debug" : "")}{(Building ? ",building" : "")}{(Razed ? ",razed" : "")}{(War ? ",war" : "")}";
    }

    private static bool Razeable(Type type) => type.GetMethod(nameof(Structure.GetRazed)).DeclaringType != typeof(Structure);

    private static IEnumerable<State> States(Type type)
    {
        foreach (bool owned in new[] { true, false })
        {
            foreach (bool debug in new[] { false, true })
            {
                foreach (bool building in new[] { false, true })
                {
                    foreach (bool razed in Razeable(type) ? new[] { false, true } : [false])
                    {
                        foreach (bool war in type == typeof(City) ? new[] { false, true } : [false])
                        {
                            yield return new State(owned, debug, building, razed, war);
                        }
                    }
                }
            }
        }
    }

    private static void Apply(Structure structure, State state)
    {
        if (structure is PlayerStructure player)
        {
            player.Player = state.Owned ? Viewer : OtherPlayer;
        }

        EngineFixture.Engine.Debug = state.Debug;
        structure.BuildingPlayer = state.Building ? (sbyte)3 : (sbyte)-1;
        if (state.Razed)
        {
            Raze(structure);
        }

        if (structure is City city)
        {
            Set<City.ModeElement> war = Set<City.ModeElement>.Of(City.ModeElement.WarMode);
            city.Mode = state.War ? city.Mode + war : city.Mode - war;
        }
        Assert.Equal(state.Razed, structure.GetRazed());
        Assert.Equal(state.Building, structure.GetBuilding());
    }

    // Each GetRazed override reads a field of its own.
    private static void Raze(Structure structure)
    {
        switch (structure)
        {
            case City city: city.Mode += Set<City.ModeElement>.Of(City.ModeElement.Razed); break;
            case Farm farm: farm.Mode += Set<Farm.ModeElement>.Of(Farm.ModeElement.Razed); break;
            case Mine mine: mine.State |= 1; break;
            case PowerNode node: node.State += Set<PowerNode.StateElement>.Of(PowerNode.StateElement.Razed); break;
            case ProductionPlace place: place.RazedState = 1; break;
            case Teleport teleport: teleport.State += Set<Teleport.StateElement>.Of(Teleport.StateElement.Razed); break;
            case Tower tower: tower.State += Set<Tower.StateElement>.Of(Tower.StateElement.Razed); break;
            case Altar altar: altar.Mode = AltarMode.Razed; break;
            default: throw new InvalidOperationException($"no razed state for {structure.GetType().Name}");
        }
    }

    // AoWInfoWin.pas:616 ShowStructureInfo.
    private static string OldInfoPanel(Structure structure, sbyte seated)
    {
        return structure.GetBuilding()
            ? "Building"
            : structure.GetRazed()
            ? "Razed"
            : structure is PlayerStructure playerStructure
            ? playerStructure.Player == seated || EngineFixture.Engine.Debug
                ? structure is City city
                    ? city.Mode.Contains(City.ModeElement.WarMode) ? "WarCity" : "City"
                    : structure is ProductionPlace productionPlace
                    ? productionPlace.GetMagicSphere() == MagicSphere.None ? "Production" : "ProductionIncome"
                    : structure is Mine
                    ? "Mine"
                    : structure is PowerNode
                    ? "PowerNode"
                    : structure is Farm ? "Farm" : structure is Tower ? "WatchTower" : structure is Altar ? "Altar" : "Generic"
                : structure is City otherCity && otherCity.Mode.Contains(City.ModeElement.WarMode) ? "WarCity" : "OtherPlayer"
            : structure is Sign
            ? "Sign"
            : structure is Cave
            ? "Cave"
            : structure is Teleport
            ? "Teleport"
            : structure is ExplorationSite
            ? "ExplorationSite"
            : structure is WizardsTower ? "WizardsTower" : structure is Arena ? "Arena" : "Generic";
    }

    // AoWInfoWin.pas:190 MainBtnClick, the structure branch. The altar arm runs with targeting
    // under way, so it cancels the targeting.
    private static string OldInfoButton(Structure structure)
    {
        return structure is City
            ? "CityScreen"
            : structure is ProductionPlace
            ? "ProductionScreen"
            : structure is Cave
            ? "Action(Cave)"
            : structure is Teleport
            ? "Action(Teleport)"
            : structure is ExplorationSite
            ? "Action(ExplorationSite)"
            : structure is WizardsTower
            ? "Action(WizardsTower)"
            : structure is Arena ? "Action(Arena)" : structure is Altar ? "Action(Altar)" : "None";
    }

    // aowMapEvents.pas:518 TheMapStructureDblClick, past the selected structure test.
    private static string OldDoubleClick(Structure structure, sbyte seated)
    {
        return structure is City city && city.Player == seated
            ? "CityScreen"
            : structure is ProductionPlace productionPlace && productionPlace.CanDoubleClick() && ((PlayerStructure)structure).Player == seated
            ? "ProductionScreen"
            : structure is WizardsTower ? "Action(WizardsTower)" : structure is Arena ? "Action(Arena)" : "None";
    }

    // AoWTC.pas:4156 TRndTacHsm.RandomMap, the class id switch for a structure. The bridge arm
    // stays in the generator, a bridge is no structure.
    private static string OldCombatMapName(int classId, bool siteCombat, ref bool randomTerrain, bool fRandomTerrain,
        Func<int, bool> neighborHoldsCity, out bool takesItems, out int fixedTags)
    {
        string fileName = "TCMaps\\";
        takesItems = false;
        fixedTags = 0;
        string Site(string prefix, int variants, bool items, out bool takesItems, ref int fixedTags)
        {
            takesItems = false;
            if (!siteCombat)
            {
                return "Blank000";
            }

            takesItems = items;
            string name = prefix + DelphiConversions.IntToStr(DelphiRandom.Random(variants));
            for (int i = 1; i <= 6; i++)
            {
                fixedTags |= 1 << i;
            }

            return name;
        }
        switch (classId)
        {
            case 131336:
                fileName += "0";
                for (int i = 1; i <= 6; i++)
                {
                    if (neighborHoldsCity(i))
                    {
                        fileName += DelphiConversions.IntToStr(i);
                        fixedTags |= 1 << i;
                    }
                }
                while (fileName.Length < 11)
                {
                    fileName += "x";
                }

                fileName += "C000";
                break;
            case 131950: fileName += "Farmx000"; break;
            case 131962: fileName += "Minex000"; break;
            case 131328: fileName += "SLife000"; break;
            case 133454: fileName += "NodeA000"; break;
            case 132270: fileName += "NodeD000"; break;
            case 131482: fileName += "NodeC000"; break;
            case 132302: fileName += "NodeE000"; break;
            case 132062: fileName += "NodeF000"; break;
            case 131953: fileName += "NodeL000"; break;
            case 132275: fileName += "NodeW000"; break;
            case 132670: fileName += "RPool000"; break;
            case 133422: fileName += "BGuil000"; break;
            case 132286: fileName += "SYard000"; break;
            case 132158: fileName += "TFort000"; break;
            case 131966: fileName += randomTerrain ? "CaveN000" : "CaveE000"; break;
            case 131332:
                fileName += fRandomTerrain ? "TeleN000" : "Telep000";
                randomTerrain = true;
                break;
            case 131886: fileName += Site("eMrLr00", 4, true, out takesItems, ref fixedTags); break;
            case 131982: fileName += Site("eZigg00", 2, true, out takesItems, ref fixedTags); break;
            case 131850: fileName += Site("eCrpt00", 2, true, out takesItems, ref fixedTags); break;
            case 131994: fileName += Site("eRuin00", 2, true, out takesItems, ref fixedTags); break;
            case 131882: fileName += Site("eDung00", 2, false, out takesItems, ref fixedTags); break;
            default: fileName += "Blank000"; break;
        }
        return fileName["TCMaps\\".Length..];
    }

    private sealed class Scene : IDisposable
    {
        private readonly StrategicMap _previousMap = StrategicMap.Current;
        private readonly bool _previousDebug = EngineFixture.Engine.Debug;
        private readonly List<Structure> _created = [];

        internal StrategicMap Map { get; }

        internal Scene()
        {
            Map = new StrategicMap(null, holder: EngineFixture.Engine)
            {
                SeatedPlayer = Viewer
            };
            StrategicMap.Current = Map;
        }

        internal (Structure Structure, ArmyOwner Owner) Create(Type type)
        {
            ArmyOwner owner = new() { Holder = EngineFixture.Engine };
            Structure structure = (Structure)EngineClass.Of(type).CreateInstance(owner, holder: EngineFixture.Engine);
            Assert.IsType(type, structure);
            _created.Add(structure);
            return (structure, owner);
        }

        public void Dispose()
        {
            StrategicMap.Current = _previousMap;
            EngineFixture.Engine.Debug = _previousDebug;
            // Some kinds need a world to tear down (ClassRegistryTests lists them).
            foreach (Structure structure in _created)
            {
                _ = Record.Exception(structure.Dispose);
            }
        }
    }

    private static string Declaring(Type type, string method) => type.GetMethod(method).DeclaringType.Name;

    private static string Observed(StructureView view, Type type, string method, ArmyOwner owner, int queries, bool cancelled)
        => view != StructureView.None ? view.Name
        : owner.ArmyQueries > queries || cancelled ? $"Action({Declaring(type, method)})"
        : "None";

    [Fact]
    public void EveryStructureClassIsCovered()
    {
        Type[] types = StructureTypes();
        Assert.Equal(40, types.Length);
        Assert.Contains(typeof(Structure), types);
        Assert.DoesNotContain(types, t => t.Assembly != typeof(Structure).Assembly);
    }

    [Fact]
    public void InfoPanelMatchesTheOldCascade()
    {
        List<string> mismatches = [];
        StringBuilder table = new();
        using Scene scene = new();
        foreach (Type type in StructureTypes())
        {
            (Structure structure, _) = scene.Create(type);
            List<string> row = [];
            foreach (State state in States(type))
            {
                Apply(structure, state);
                string expected = OldInfoPanel(structure, Viewer);
                StructureView view = structure.InfoPanel(Viewer);
                Assert.True(InfoWindow.StructurePanels.TryGet(view, out _), $"{type.Name} {state}: no panel renderer for {view}");
                if (view.Name != expected)
                {
                    mismatches.Add($"{type.Name} {state}: {view.Name}, old cascade {expected}");
                }

                row.Add($"{state}={view.Name}");
                // A razed state has no plain reset in every kind, so the next state starts from a new instance.
                if (state.Razed)
                {
                    structure = scene.Create(type).Structure;
                }
            }
            _ = table.AppendLine($"{type.Name}: {string.Join(" ", row.Distinct())}");
        }
        output.WriteLine(table.ToString());
        Assert.Empty(mismatches);
    }

    [Fact]
    public void InfoButtonMatchesTheOldCascade()
    {
        List<string> mismatches = [];
        using Scene scene = new();
        foreach (Type type in StructureTypes())
        {
            foreach (bool owned in new[] { true, false })
            {
                (Structure structure, ArmyOwner owner) = scene.Create(type);
                if (structure is PlayerStructure player)
                {
                    player.Player = owned ? Viewer : OtherPlayer;
                }
                // Targeting under way from this structure, the state the altar arm tests first.
                TargetSelectionControl selection = scene.Map.TargetSelection;
                selection.Status = Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Activated);
                selection.Source = structure;
                int queries = owner.ArmyQueries;
                string expected = OldInfoButton(structure);
                StructureView view = structure.InfoButton(Viewer);
                bool cancelled = !selection.Status.Contains(TargetSelectionStatusElement.Activated);
                selection.Status = Set<TargetSelectionStatusElement>.Of();
                selection.Source = null;
                if (view != StructureView.None)
                {
                    Assert.True(InfoWindow.StructureButtons.TryGet(view, out _), $"{type.Name}: no button renderer for {view}");
                }

                string actual = Observed(view, type, nameof(Structure.InfoButton), owner, queries, cancelled);
                if (actual != expected)
                {
                    mismatches.Add($"{type.Name} {(owned ? "owned" : "other")}: {actual}, old cascade {expected}");
                }

                output.WriteLine($"{type.Name} {(owned ? "owned" : "other")}: {actual}");
            }
        }
        Assert.Empty(mismatches);
    }

    [Fact]
    public void DoubleClickMatchesTheOldCascade()
    {
        List<string> mismatches = [];
        using Scene scene = new();
        foreach (Type type in StructureTypes())
        {
            foreach (bool owned in new[] { true, false })
            {
                (Structure structure, ArmyOwner owner) = scene.Create(type);
                if (structure is PlayerStructure player)
                {
                    player.Player = owned ? Viewer : OtherPlayer;
                }

                int queries = owner.ArmyQueries;
                string expected = OldDoubleClick(structure, Viewer);
                // The old cascade only names the arm, so the army count is unchanged here.
                Assert.Equal(queries, owner.ArmyQueries);
                StructureView view = structure.DoubleClick(Viewer);
                if (view != StructureView.None)
                {
                    Assert.True(MapEvents.StructureScreens.TryGet(view, out _), $"{type.Name}: no screen renderer for {view}");
                }

                string actual = Observed(view, type, nameof(Structure.DoubleClick), owner, queries, false);
                if (actual != expected)
                {
                    mismatches.Add($"{type.Name} {(owned ? "owned" : "other")}: {actual}, old cascade {expected}");
                }

                output.WriteLine($"{type.Name} {(owned ? "owned" : "other")}: {actual}");
            }
        }
        Assert.Empty(mismatches);
    }

    [Fact]
    public void CombatMapNameMatchesTheOldSwitch()
    {
        int[][] neighbors = [[], [2, 5], [1, 2, 3, 4, 5, 6], [6]];
        List<string> mismatches = [];
        int previousSeed = DelphiRandom.RandSeed;
        using Scene scene = new();
        try
        {
            foreach (Type type in StructureTypes())
            {
                (Structure structure, _) = scene.Create(type);
                SortedSet<string> names = [with(StringComparer.Ordinal)];
                foreach (bool siteCombat in new[] { false, true })
                {
                    foreach (bool randomTerrain in new[] { false, true })
                    {
                        foreach (int[] pattern in neighbors)
                        {
                            DelphiRandom.RandSeed = 0x5EED + pattern.Length;
                            CombatMapSite site = new(siteCombat, randomTerrain, (direction, kind) =>
                            {
                                Assert.Equal(typeof(City), kind);
                                return pattern.Contains(direction);
                            });
                            string actual = structure.CombatMapName(site);
                            int actualSeed = DelphiRandom.RandSeed;
                            DelphiRandom.RandSeed = 0x5EED + pattern.Length;
                            bool expectedTerrain = randomTerrain;
                            string expected = OldCombatMapName(EngineClass.Of(type).ClassId, siteCombat, ref expectedTerrain, randomTerrain,
                                pattern.Contains, out bool expectedItems, out int expectedTags);
                            string state = $"{type.Name} site={siteCombat} random={randomTerrain} neighbors=[{string.Join(",", pattern)}]";
                            if (actual != expected)
                            {
                                mismatches.Add($"{state}: {actual}, old switch {expected}");
                            }

                            if (site.RandomTerrain != expectedTerrain)
                            {
                                mismatches.Add($"{state}: random terrain {site.RandomTerrain}, old switch {expectedTerrain}");
                            }

                            if (site.TakesCombatItems != expectedItems)
                            {
                                mismatches.Add($"{state}: items {site.TakesCombatItems}, old switch {expectedItems}");
                            }

                            if (site.FixedTags != expectedTags)
                            {
                                mismatches.Add($"{state}: tags {site.FixedTags}, old switch {expectedTags}");
                            }

                            if (actualSeed != DelphiRandom.RandSeed)
                            {
                                mismatches.Add($"{state}: random draws differ");
                            }

                            _ = names.Add(actual.Length == 8 && actual[0] == 'e' ? $"{actual[..7]}?" : actual);
                        }
                    }
                }

                output.WriteLine($"{type.Name} ({EngineClass.Of(type).ClassId}): {string.Join(" ", names)}");
            }
        }
        finally
        {
            DelphiRandom.RandSeed = previousSeed;
        }
        Assert.Empty(mismatches);
    }

    // The switch matched exact class ids, so no structure class inherits a map name from a class
    // above it. Each override sits on the class whose id the switch named.
    [Fact]
    public void NoStructureInheritsAnotherClassMapName()
    {
        StringBuilder resolved = new();
        foreach (Type type in StructureTypes())
        {
            Type declaring = type.GetMethod(nameof(Structure.CombatMapName)).DeclaringType;
            Assert.True(declaring == type || declaring == typeof(Structure), $"{type.Name} inherits CombatMapName from {declaring.Name}");
            MethodInfo owner = type.GetMethod("OwnerPanel", BindingFlags.Instance | BindingFlags.NonPublic);
            _ = resolved.AppendLine($"{type.Name}: InfoPanel {Declaring(type, nameof(Structure.InfoPanel))}" +
                (owner is null ? "" : $" (OwnerPanel {owner.DeclaringType.Name})") + ", " +
                $"InfoButton {Declaring(type, nameof(Structure.InfoButton))}, DoubleClick {Declaring(type, nameof(Structure.DoubleClick))}, " +
                $"CombatMapName {declaring.Name}");
        }
        output.WriteLine(resolved.ToString());
    }

    [Fact]
    public void TheScreensRegisterEveryOriginalView()
    {
        Assert.Equal(19, InfoWindow.StructurePanels.All.Count);
        Assert.Equal(2, InfoWindow.StructureButtons.All.Count);
        Assert.Equal(2, MapEvents.StructureScreens.All.Count);
        Assert.False(InfoWindow.StructureButtons.TryGet(StructureView.None, out _));
        Assert.False(MapEvents.StructureScreens.TryGet(StructureView.None, out _));
        InvalidOperationException duplicate = Assert.Throws<InvalidOperationException>(() =>
            InfoWindow.StructurePanels.Add(StructureView.Generic, static (_, _) => { }));
        Assert.Contains("structure info panel", duplicate.Message);
    }
}
