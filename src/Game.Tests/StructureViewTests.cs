using System.Reflection;
using System.Text;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
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

    private static Type[] StructureTypes() => EngineClass.All
        .Where(c => typeof(Structure).IsAssignableFrom(c.Type))
        .Select(c => c.Type).OrderBy(t => t.Name, StringComparer.Ordinal).ToArray();

    // The original's army query on the structure's field tells that a kind action ran. No army
    // stands on the field, so every action stops at that query.
    private sealed class ArmyOwner : EngineFixture.Owner
    {
        internal int ArmyQueries { get; private set; }

        public override EngineObject FindOwnedChild(int classId)
        {
            if (classId == ArmyHexSprite.ClassId) ArmyQueries++;
            return null!;
        }
    }

    private sealed record State(bool Owned, bool Debug, bool Building, bool Razed, bool War)
    {
        public override string ToString() =>
            $"{(Owned ? "owned" : "other")}{(Debug ? ",debug" : "")}{(Building ? ",building" : "")}{(Razed ? ",razed" : "")}{(War ? ",war" : "")}";
    }

    private static bool Razeable(Type type) => type.GetMethod(nameof(Structure.GetRazed))!.DeclaringType != typeof(Structure);

    private static IEnumerable<State> States(Type type)
    {
        foreach (bool owned in new[] { true, false })
            foreach (bool debug in new[] { false, true })
                foreach (bool building in new[] { false, true })
                    foreach (bool razed in Razeable(type) ? new[] { false, true } : [false])
                        foreach (bool war in type == typeof(City) ? new[] { false, true } : [false])
                            yield return new State(owned, debug, building, razed, war);
    }

    private static void Apply(Structure structure, State state)
    {
        if (structure is PlayerStructure player) player.Player = state.Owned ? Viewer : OtherPlayer;
        GameTables.GameEngine.Debug = state.Debug;
        structure.BuildingPlayer = state.Building ? (sbyte)3 : (sbyte)-1;
        if (state.Razed) Raze(structure);
        if (structure is City city)
        {
            var war = Set<City.ModeElement>.Of(City.ModeElement.WarMode);
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
            case City city: city.Mode = city.Mode + Set<City.ModeElement>.Of(City.ModeElement.Razed); break;
            case Farm farm: farm.Mode = farm.Mode + Set<Farm.ModeElement>.Of(Farm.ModeElement.Razed); break;
            case Mine mine: mine.State |= 1; break;
            case PowerNode node: node.State = node.State + Set<PowerNode.StateElement>.Of(PowerNode.StateElement.Razed); break;
            case ProductionPlace place: place.RazedState = 1; break;
            case Teleport teleport: teleport.State = teleport.State + Set<Teleport.StateElement>.Of(Teleport.StateElement.Razed); break;
            case Tower tower: tower.State = tower.State + Set<Tower.StateElement>.Of(Tower.StateElement.Razed); break;
            case Altar altar: altar.Mode = AltarMode.Razed; break;
            default: throw new InvalidOperationException("no razed state for " + structure.GetType().Name);
        }
    }

    // AoWInfoWin.pas:616 ShowStructureInfo.
    private static string OldInfoPanel(Structure structure, sbyte seated)
    {
        if (structure.GetBuilding()) return "Building";
        if (structure.GetRazed()) return "Razed";
        if (structure is PlayerStructure)
        {
            if (((PlayerStructure)structure).Player == seated || GameTables.GameEngine.Debug)
            {
                if (structure is City) return ((City)structure).Mode.Contains(City.ModeElement.WarMode) ? "WarCity" : "City";
                if (structure is ProductionPlace) return ((ProductionPlace)structure).GetMagicSphere() == MagicSphere.None ? "Production" : "ProductionIncome";
                if (structure is Mine) return "Mine";
                if (structure is PowerNode) return "PowerNode";
                if (structure is Farm) return "Farm";
                if (structure is Tower) return "WatchTower";
                if (structure is Altar) return "Altar";
                return "Generic";
            }
            if (structure is City && ((City)structure).Mode.Contains(City.ModeElement.WarMode)) return "WarCity";
            return "OtherPlayer";
        }
        if (structure is Sign) return "Sign";
        if (structure is Cave) return "Cave";
        if (structure is Teleport) return "Teleport";
        if (structure is ExplorationSite) return "ExplorationSite";
        if (structure is WizardsTower) return "WizardsTower";
        if (structure is Arena) return "Arena";
        return "Generic";
    }

    // AoWInfoWin.pas:190 MainBtnClick, the structure branch. The altar arm runs with targeting
    // under way, so it cancels the targeting.
    private static string OldInfoButton(Structure structure)
    {
        if (structure is City) return "CityScreen";
        if (structure is ProductionPlace) return "ProductionScreen";
        if (structure is Cave) return "Action(Cave)";
        if (structure is Teleport) return "Action(Teleport)";
        if (structure is ExplorationSite) return "Action(ExplorationSite)";
        if (structure is WizardsTower) return "Action(WizardsTower)";
        if (structure is Arena) return "Action(Arena)";
        if (structure is Altar) return "Action(Altar)";
        return "None";
    }

    // aowMapEvents.pas:518 TheMapStructureDblClick, past the selected structure test.
    private static string OldDoubleClick(Structure structure, sbyte seated)
    {
        if (structure is City && ((City)structure).Player == seated) return "CityScreen";
        if (structure is ProductionPlace && ((ProductionPlace)structure).CanDoubleClick() && ((PlayerStructure)structure).Player == seated) return "ProductionScreen";
        if (structure is WizardsTower) return "Action(WizardsTower)";
        if (structure is Arena) return "Action(Arena)";
        return "None";
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
            if (!siteCombat) return "Blank000";
            takesItems = items;
            string name = prefix + Runtime.IntToStr(Runtime.Random(variants));
            for (int I = 1; I <= 6; I++) fixedTags |= 1 << I;
            return name;
        }
        switch (classId)
        {
            case 131336:
                fileName = fileName + "0";
                for (int I = 1; I <= 6; I++)
                {
                    if (neighborHoldsCity(I))
                    {
                        fileName = fileName + Runtime.IntToStr(I);
                        fixedTags |= 1 << I;
                    }
                }
                while (fileName.Length < 11) fileName = fileName + "x";
                fileName = fileName + "C000";
                break;
            case 131950: fileName = fileName + "Farmx000"; break;
            case 131962: fileName = fileName + "Minex000"; break;
            case 131328: fileName = fileName + "SLife000"; break;
            case 133454: fileName = fileName + "NodeA000"; break;
            case 132270: fileName = fileName + "NodeD000"; break;
            case 131482: fileName = fileName + "NodeC000"; break;
            case 132302: fileName = fileName + "NodeE000"; break;
            case 132062: fileName = fileName + "NodeF000"; break;
            case 131953: fileName = fileName + "NodeL000"; break;
            case 132275: fileName = fileName + "NodeW000"; break;
            case 132670: fileName = fileName + "RPool000"; break;
            case 133422: fileName = fileName + "BGuil000"; break;
            case 132286: fileName = fileName + "SYard000"; break;
            case 132158: fileName = fileName + "TFort000"; break;
            case 131966: fileName = fileName + (randomTerrain ? "CaveN000" : "CaveE000"); break;
            case 131332:
                fileName = fileName + (fRandomTerrain ? "TeleN000" : "Telep000");
                randomTerrain = true;
                break;
            case 131886: fileName = fileName + Site("eMrLr00", 4, true, out takesItems, ref fixedTags); break;
            case 131982: fileName = fileName + Site("eZigg00", 2, true, out takesItems, ref fixedTags); break;
            case 131850: fileName = fileName + Site("eCrpt00", 2, true, out takesItems, ref fixedTags); break;
            case 131994: fileName = fileName + Site("eRuin00", 2, true, out takesItems, ref fixedTags); break;
            case 131882: fileName = fileName + Site("eDung00", 2, false, out takesItems, ref fixedTags); break;
            default: fileName = fileName + "Blank000"; break;
        }
        return fileName["TCMaps\\".Length..];
    }

    private sealed class Scene : IDisposable
    {
        private readonly AoWHSMap _previousMap = AoWHSMap.Current;
        private readonly bool _previousDebug = GameTables.GameEngine.Debug;
        private readonly List<Structure> _created = [];

        internal AoWHSMap Map { get; }

        internal Scene()
        {
            Map = new AoWHSMap(null!);
            Map.SeatedPlayer = Viewer;
            AoWHSMap.Current = Map;
        }

        internal (Structure Structure, ArmyOwner Owner) Create(Type type)
        {
            var owner = new ArmyOwner();
            var structure = (Structure)EngineClass.Of(type).CreateInstance(owner);
            Assert.IsType(type, structure);
            _created.Add(structure);
            return (structure, owner);
        }

        public void Dispose()
        {
            AoWHSMap.Current = _previousMap;
            GameTables.GameEngine.Debug = _previousDebug;
            // Some kinds need a world to tear down (ClassRegistryTests lists them).
            foreach (var structure in _created) _ = Record.Exception(structure.Dispose);
        }
    }

    private static string Declaring(Type type, string method) => type.GetMethod(method)!.DeclaringType!.Name;

    private static string Observed(StructureView view, Type type, string method, ArmyOwner owner, int queries, bool cancelled) =>
        view != StructureView.None ? view.Name
        : owner.ArmyQueries > queries || cancelled ? $"Action({Declaring(type, method)})"
        : "None";

    [Fact]
    public void EveryStructureClassIsCovered()
    {
        var types = StructureTypes();
        Assert.Equal(40, types.Length);
        Assert.Contains(typeof(Structure), types);
        Assert.DoesNotContain(types, t => t.Assembly != typeof(Structure).Assembly);
    }

    [Fact]
    public void InfoPanelMatchesTheOldCascade()
    {
        var mismatches = new List<string>();
        var table = new StringBuilder();
        using var scene = new Scene();
        foreach (var type in StructureTypes())
        {
            var (structure, _) = scene.Create(type);
            var row = new List<string>();
            foreach (var state in States(type))
            {
                Apply(structure, state);
                string expected = OldInfoPanel(structure, Viewer);
                var view = structure.InfoPanel(Viewer);
                Assert.True(InfoWindow.StructurePanels.TryGet(view, out _), $"{type.Name} {state}: no panel renderer for {view}");
                if (view.Name != expected) mismatches.Add($"{type.Name} {state}: {view.Name}, old cascade {expected}");
                row.Add($"{state}={view.Name}");
                // A razed state has no plain reset in every kind, so the next state starts from a new instance.
                if (state.Razed) structure = scene.Create(type).Structure;
            }
            table.AppendLine($"{type.Name}: {string.Join(" ", row.Distinct())}");
        }
        output.WriteLine(table.ToString());
        Assert.Empty(mismatches);
    }

    [Fact]
    public void InfoButtonMatchesTheOldCascade()
    {
        var mismatches = new List<string>();
        using var scene = new Scene();
        foreach (var type in StructureTypes())
        {
            foreach (bool owned in new[] { true, false })
            {
                var (structure, owner) = scene.Create(type);
                if (structure is PlayerStructure player) player.Player = owned ? Viewer : OtherPlayer;
                // Targeting under way from this structure, the state the altar arm tests first.
                var selection = scene.Map.TargetSelection;
                selection.Status = Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Activated);
                selection.Source = structure;
                int queries = owner.ArmyQueries;
                string expected = OldInfoButton(structure);
                var view = structure.InfoButton(Viewer);
                bool cancelled = !selection.Status.Contains(TargetSelectionStatusElement.Activated);
                selection.Status = Set<TargetSelectionStatusElement>.Of();
                selection.Source = null;
                if (view != StructureView.None)
                    Assert.True(InfoWindow.StructureButtons.TryGet(view, out _), $"{type.Name}: no button renderer for {view}");
                string actual = Observed(view, type, nameof(Structure.InfoButton), owner, queries, cancelled);
                if (actual != expected) mismatches.Add($"{type.Name} {(owned ? "owned" : "other")}: {actual}, old cascade {expected}");
                output.WriteLine($"{type.Name} {(owned ? "owned" : "other")}: {actual}");
            }
        }
        Assert.Empty(mismatches);
    }

    [Fact]
    public void DoubleClickMatchesTheOldCascade()
    {
        var mismatches = new List<string>();
        using var scene = new Scene();
        foreach (var type in StructureTypes())
        {
            foreach (bool owned in new[] { true, false })
            {
                var (structure, owner) = scene.Create(type);
                if (structure is PlayerStructure player) player.Player = owned ? Viewer : OtherPlayer;
                int queries = owner.ArmyQueries;
                string expected = OldDoubleClick(structure, Viewer);
                // The old cascade only names the arm, so the army count is unchanged here.
                Assert.Equal(queries, owner.ArmyQueries);
                var view = structure.DoubleClick(Viewer);
                if (view != StructureView.None)
                    Assert.True(MapEvents.StructureScreens.TryGet(view, out _), $"{type.Name}: no screen renderer for {view}");
                string actual = Observed(view, type, nameof(Structure.DoubleClick), owner, queries, false);
                if (actual != expected) mismatches.Add($"{type.Name} {(owned ? "owned" : "other")}: {actual}, old cascade {expected}");
                output.WriteLine($"{type.Name} {(owned ? "owned" : "other")}: {actual}");
            }
        }
        Assert.Empty(mismatches);
    }

    [Fact]
    public void CombatMapNameMatchesTheOldSwitch()
    {
        int[][] neighbors = [[], [2, 5], [1, 2, 3, 4, 5, 6], [6]];
        var mismatches = new List<string>();
        int previousSeed = Runtime.RandSeed;
        using var scene = new Scene();
        try
        {
            foreach (var type in StructureTypes())
            {
                var (structure, _) = scene.Create(type);
                var names = new SortedSet<string>(StringComparer.Ordinal);
                foreach (bool siteCombat in new[] { false, true })
                    foreach (bool randomTerrain in new[] { false, true })
                        foreach (var pattern in neighbors)
                        {
                            Runtime.RandSeed = 0x5EED + pattern.Length;
                            var site = new CombatMapSite(siteCombat, randomTerrain, (direction, kind) =>
                            {
                                Assert.Equal(typeof(City), kind);
                                return pattern.Contains(direction);
                            });
                            string actual = structure.CombatMapName(site);
                            int actualSeed = Runtime.RandSeed;
                            Runtime.RandSeed = 0x5EED + pattern.Length;
                            bool expectedTerrain = randomTerrain;
                            string expected = OldCombatMapName(EngineClass.Of(type).ClassId, siteCombat, ref expectedTerrain, randomTerrain,
                                pattern.Contains, out bool expectedItems, out int expectedTags);
                            string state = $"{type.Name} site={siteCombat} random={randomTerrain} neighbors=[{string.Join(",", pattern)}]";
                            if (actual != expected) mismatches.Add($"{state}: {actual}, old switch {expected}");
                            if (site.RandomTerrain != expectedTerrain) mismatches.Add($"{state}: random terrain {site.RandomTerrain}, old switch {expectedTerrain}");
                            if (site.TakesCombatItems != expectedItems) mismatches.Add($"{state}: items {site.TakesCombatItems}, old switch {expectedItems}");
                            if (site.FixedTags != expectedTags) mismatches.Add($"{state}: tags {site.FixedTags}, old switch {expectedTags}");
                            if (actualSeed != Runtime.RandSeed) mismatches.Add($"{state}: random draws differ");
                            names.Add(actual.Length == 8 && actual[0] == 'e' ? actual[..7] + "?" : actual);
                        }
                output.WriteLine($"{type.Name} ({(EngineClass.Of(type).ClassId)}): {string.Join(" ", names)}");
            }
        }
        finally { Runtime.RandSeed = previousSeed; }
        Assert.Empty(mismatches);
    }

    // The switch matched exact class ids, so no structure class inherits a map name from a class
    // above it. Each override sits on the class whose id the switch named.
    [Fact]
    public void NoStructureInheritsAnotherClassMapName()
    {
        var resolved = new StringBuilder();
        foreach (var type in StructureTypes())
        {
            var declaring = type.GetMethod(nameof(Structure.CombatMapName))!.DeclaringType!;
            Assert.True(declaring == type || declaring == typeof(Structure), $"{type.Name} inherits CombatMapName from {declaring.Name}");
            var owner = type.GetMethod("OwnerPanel", BindingFlags.Instance | BindingFlags.NonPublic);
            resolved.AppendLine($"{type.Name}: InfoPanel {Declaring(type, nameof(Structure.InfoPanel))}" +
                (owner is null ? "" : $" (OwnerPanel {owner.DeclaringType!.Name})") + ", " +
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
        var duplicate = Assert.Throws<InvalidOperationException>(() =>
            InfoWindow.StructurePanels.Add(StructureView.Generic, static (_, _) => { }));
        Assert.Contains("structure info panel", duplicate.Message);
    }
}
