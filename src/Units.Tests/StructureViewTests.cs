using System.Text;
using Xunit.Abstractions;
using Aow.Units.AoWE;
using Aow.Units.AoWInfoWin;
using Aow.Units.aowMapEvents;
using Aow.Units.Altar;
using Aow.Units.Arena;
using Aow.Units.Cave;
using Aow.Units.City;
using Aow.Units.Engine;
using Aow.Units.ExploreS;
using Aow.Units.Farm;
using Aow.Units.Mine;
using Aow.Units.PowerNode;
using Aow.Units.PrdPlace;
using Aow.Units.Sign;
using Aow.Units.Teleport;
using Aow.Units.Tower;
using Aow.Units.WizardsTower;
using Game = Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Tests;

// Brief 9.6-2: the structure view virtuals against the cascades and the class id switch they
// replaced. The Old* methods copy those cascades and that switch from main 9ee2ef8, with each screen
// call replaced by the name of its view key and each kind action by "Action(<kind>)".
[Collection("Engine")]
[EngineState]
public sealed class StructureViewTests(ITestOutputHelper output)
{
    private const sbyte Viewer = 1;
    private const sbyte OtherPlayer = 2;

    private static Type[] StructureTypes() => EClass.All
        .Where(c => typeof(TStructure).IsAssignableFrom(c.Type))
        .Select(c => c.Type).OrderBy(t => t.Name, StringComparer.Ordinal).ToArray();

    // The original's army query on the structure's field tells that a kind action ran. No army
    // stands on the field, so every action stops at that query.
    private sealed class ArmyOwner : EngineFixture.Owner
    {
        internal int ArmyQueries;
        public override TEObject FindOwnedChild(int AClassID)
        {
            if (AClassID == TArmyHS.ClassID) ArmyQueries++;
            return null!;
        }
    }

    private sealed record State(bool Owned, bool Debug, bool Building, bool Razed, bool War)
    {
        public override string ToString() =>
            $"{(Owned ? "owned" : "other")}{(Debug ? ",debug" : "")}{(Building ? ",building" : "")}{(Razed ? ",razed" : "")}{(War ? ",war" : "")}";
    }

    private static bool Razeable(Type type) => type.GetMethod(nameof(TStructure.GetRazed))!.DeclaringType != typeof(TStructure);

    private static IEnumerable<State> States(Type type)
    {
        foreach (bool owned in new[] { true, false })
            foreach (bool debug in new[] { false, true })
                foreach (bool building in new[] { false, true })
                    foreach (bool razed in Razeable(type) ? new[] { false, true } : [false])
                        foreach (bool war in type == typeof(TCity) ? new[] { false, true } : [false])
                            yield return new State(owned, debug, building, razed, war);
    }

    private static void Apply(TStructure structure, State state)
    {
        if (structure is TPlayerStructure player) player.FPlayer = state.Owned ? Viewer : OtherPlayer;
        Game.AoWEngine.FDebug = state.Debug;
        structure.FBuildingPlayer = state.Building ? (sbyte)3 : (sbyte)-1;
        if (state.Razed) Raze(structure);
        if (structure is TCity city)
        {
            var war = TSet<TCityModeElement>.Of(TCityModeElement.cmWarmode);
            city.FMode = state.War ? city.FMode + war : city.FMode - war;
        }
        Assert.Equal(state.Razed, structure.GetRazed());
        Assert.Equal(state.Building, structure.GetBuilding());
    }

    // Each GetRazed override reads a field of its own.
    private static void Raze(TStructure structure)
    {
        switch (structure)
        {
            case TCity city: city.FMode = city.FMode + TSet<TCityModeElement>.Of(TCityModeElement.cmRazed); break;
            case TFarm farm: farm.FMode = farm.FMode + TSet<TFarmModeElement>.Of(TFarmModeElement.fmRazed); break;
            case TMine mine: mine.FState |= 1; break;
            case TPowerNode node: node.FState = node.FState + TSet<TPowerNodeStateElement>.Of(TPowerNodeStateElement.pnsRazed); break;
            case TProductionPlace place: place.FRazedState = 1; break;
            case TTeleport teleport: teleport.FState = teleport.FState + TSet<TTeleportStateElement>.Of(TTeleportStateElement.tsRazed); break;
            case TTower tower: tower.FState = tower.FState + TSet<TTowerStateElement>.Of(TTowerStateElement.tsRazed); break;
            case TAltar altar: altar.FMode = TAltarMode.amRazed; break;
            default: throw new InvalidOperationException("no razed state for " + structure.GetType().Name);
        }
    }

    // AoWInfoWin.pas:616 ShowStructureInfo.
    private static string OldInfoPanel(TStructure Structure, sbyte Seated)
    {
        if (Structure.GetBuilding()) return "Building";
        if (Structure.GetRazed()) return "Razed";
        if (Structure is TPlayerStructure)
        {
            if (((TPlayerStructure)Structure).FPlayer == Seated || Game.AoWEngine.FDebug)
            {
                if (Structure is TCity) return ((TCity)Structure).FMode.Contains(TCityModeElement.cmWarmode) ? "WarCity" : "City";
                if (Structure is TProductionPlace) return ((TProductionPlace)Structure).GetMagicSphere() == TMagicSphere.msNone ? "Production" : "ProductionIncome";
                if (Structure is TMine) return "Mine";
                if (Structure is TPowerNode) return "PowerNode";
                if (Structure is TFarm) return "Farm";
                if (Structure is TTower) return "WatchTower";
                if (Structure is TAltar) return "Altar";
                return "Generic";
            }
            if (Structure is TCity && ((TCity)Structure).FMode.Contains(TCityModeElement.cmWarmode)) return "WarCity";
            return "OtherPlayer";
        }
        if (Structure is TSign) return "Sign";
        if (Structure is TCave) return "Cave";
        if (Structure is TTeleport) return "Teleport";
        if (Structure is TExplorationSite) return "ExplorationSite";
        if (Structure is TWizardsTower) return "WizardsTower";
        if (Structure is TArena) return "Arena";
        return "Generic";
    }

    // AoWInfoWin.pas:190 MainBtnClick, the structure branch. The altar arm runs with targeting
    // under way, so it cancels the targeting.
    private static string OldInfoButton(TStructure Structure)
    {
        if (Structure is TCity) return "CityScreen";
        if (Structure is TProductionPlace) return "ProductionScreen";
        if (Structure is TCave) return "Action(TCave)";
        if (Structure is TTeleport) return "Action(TTeleport)";
        if (Structure is TExplorationSite) return "Action(TExplorationSite)";
        if (Structure is TWizardsTower) return "Action(TWizardsTower)";
        if (Structure is TArena) return "Action(TArena)";
        if (Structure is TAltar) return "Action(TAltar)";
        return "None";
    }

    // aowMapEvents.pas:518 TheMapStructureDblClick, past the selected structure test.
    private static string OldDoubleClick(TStructure Structure, sbyte Seated)
    {
        if (Structure is TCity && ((TCity)Structure).FPlayer == Seated) return "CityScreen";
        if (Structure is TProductionPlace && ((TProductionPlace)Structure).CanDblClick() && ((TPlayerStructure)Structure).FPlayer == Seated) return "ProductionScreen";
        if (Structure is TWizardsTower) return "Action(TWizardsTower)";
        if (Structure is TArena) return "Action(TArena)";
        return "None";
    }

    // AoWTC.pas:4156 TRndTacHsm.RandomMap, the class id switch for a structure. The bridge arm
    // stays in the generator, a bridge is no structure.
    private static string OldCombatMapName(int ClassID, bool SiteCombat, ref bool RandomTerrain, bool FRandomTerrain,
        Func<int, bool> NeighborHoldsCity, out bool TakesItems, out int FixedTags)
    {
        string FileName = "TCMaps\\";
        TakesItems = false;
        FixedTags = 0;
        string Site(string Prefix, int Variants, bool Items, out bool TakesItems, ref int FixedTags)
        {
            TakesItems = false;
            if (!SiteCombat) return "Blank000";
            TakesItems = Items;
            string Name = Prefix + Runtime.IntToStr(Runtime.Random(Variants));
            for (int I = 1; I <= 6; I++) FixedTags |= 1 << I;
            return Name;
        }
        switch (ClassID)
        {
            case 131336:
                FileName = FileName + "0";
                for (int I = 1; I <= 6; I++)
                {
                    if (NeighborHoldsCity(I))
                    {
                        FileName = FileName + Runtime.IntToStr(I);
                        FixedTags |= 1 << I;
                    }
                }
                while (FileName.Length < 11) FileName = FileName + "x";
                FileName = FileName + "C000";
                break;
            case 131950: FileName = FileName + "Farmx000"; break;
            case 131962: FileName = FileName + "Minex000"; break;
            case 131328: FileName = FileName + "SLife000"; break;
            case 133454: FileName = FileName + "NodeA000"; break;
            case 132270: FileName = FileName + "NodeD000"; break;
            case 131482: FileName = FileName + "NodeC000"; break;
            case 132302: FileName = FileName + "NodeE000"; break;
            case 132062: FileName = FileName + "NodeF000"; break;
            case 131953: FileName = FileName + "NodeL000"; break;
            case 132275: FileName = FileName + "NodeW000"; break;
            case 132670: FileName = FileName + "RPool000"; break;
            case 133422: FileName = FileName + "BGuil000"; break;
            case 132286: FileName = FileName + "SYard000"; break;
            case 132158: FileName = FileName + "TFort000"; break;
            case 131966: FileName = FileName + (RandomTerrain ? "CaveN000" : "CaveE000"); break;
            case 131332:
                FileName = FileName + (FRandomTerrain ? "TeleN000" : "Telep000");
                RandomTerrain = true;
                break;
            case 131886: FileName = FileName + Site("eMrLr00", 4, true, out TakesItems, ref FixedTags); break;
            case 131982: FileName = FileName + Site("eZigg00", 2, true, out TakesItems, ref FixedTags); break;
            case 131850: FileName = FileName + Site("eCrpt00", 2, true, out TakesItems, ref FixedTags); break;
            case 131994: FileName = FileName + Site("eRuin00", 2, true, out TakesItems, ref FixedTags); break;
            case 131882: FileName = FileName + Site("eDung00", 2, false, out TakesItems, ref FixedTags); break;
            default: FileName = FileName + "Blank000"; break;
        }
        return FileName["TCMaps\\".Length..];
    }

    private sealed class Scene : IDisposable
    {
        private readonly TAoWHSMap previousMap = Game.AoWHSMap;
        private readonly bool previousDebug = Game.AoWEngine.FDebug;
        private readonly List<TStructure> created = [];
        internal readonly TAoWHSMap Map;

        internal Scene()
        {
            Map = new TAoWHSMap(null!);
            Map.FSeatedPlayer = Viewer;
            Game.AoWHSMap = Map;
        }

        internal (TStructure Structure, ArmyOwner Owner) Create(Type type)
        {
            var owner = new ArmyOwner();
            var structure = (TStructure)EClass.Of(type).Create(owner);
            Assert.IsType(type, structure);
            created.Add(structure);
            return (structure, owner);
        }

        public void Dispose()
        {
            Game.AoWHSMap = previousMap;
            Game.AoWEngine.FDebug = previousDebug;
            // Some kinds need a world to tear down (ClassRegistryTests lists them).
            foreach (var structure in created) _ = Record.Exception(structure.Destroy);
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
        Assert.Equal(42, types.Length);
        Assert.Contains(typeof(TStructure), types);
        Assert.DoesNotContain(types, t => t.Assembly != typeof(TStructure).Assembly);
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
                Assert.True(TIWindow.StructurePanels.TryGet(view, out _), $"{type.Name} {state}: no panel renderer for {view}");
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
                if (structure is TPlayerStructure player) player.FPlayer = owned ? Viewer : OtherPlayer;
                // Targeting under way from this structure, the state the altar arm tests first.
                var selection = scene.Map.FTargetSelection;
                selection.FStatus = TSet<TTargetSelectionStatusElement>.Of(TTargetSelectionStatusElement.tsActivated);
                selection.FSource = structure;
                int queries = owner.ArmyQueries;
                string expected = OldInfoButton(structure);
                var view = structure.InfoButton(Viewer);
                bool cancelled = !selection.FStatus.Contains(TTargetSelectionStatusElement.tsActivated);
                selection.FStatus = TSet<TTargetSelectionStatusElement>.Of();
                selection.FSource = null;
                if (view != StructureView.None)
                    Assert.True(TIWindow.StructureButtons.TryGet(view, out _), $"{type.Name}: no button renderer for {view}");
                string actual = Observed(view, type, nameof(TStructure.InfoButton), owner, queries, cancelled);
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
                if (structure is TPlayerStructure player) player.FPlayer = owned ? Viewer : OtherPlayer;
                int queries = owner.ArmyQueries;
                string expected = OldDoubleClick(structure, Viewer);
                // The old cascade only names the arm, so the army count is unchanged here.
                Assert.Equal(queries, owner.ArmyQueries);
                var view = structure.DoubleClick(Viewer);
                if (view != StructureView.None)
                    Assert.True(TMapEvents.StructureScreens.TryGet(view, out _), $"{type.Name}: no screen renderer for {view}");
                string actual = Observed(view, type, nameof(TStructure.DoubleClick), owner, queries, false);
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
                                Assert.Equal(typeof(TCity), kind);
                                return pattern.Contains(direction);
                            });
                            string actual = structure.CombatMapName(site);
                            int actualSeed = Runtime.RandSeed;
                            Runtime.RandSeed = 0x5EED + pattern.Length;
                            bool expectedTerrain = randomTerrain;
                            string expected = OldCombatMapName(EClass.Of(type).ClassID, siteCombat, ref expectedTerrain, randomTerrain,
                                pattern.Contains, out bool expectedItems, out int expectedTags);
                            string state = $"{type.Name} site={siteCombat} random={randomTerrain} neighbors=[{string.Join(",", pattern)}]";
                            if (actual != expected) mismatches.Add($"{state}: {actual}, old switch {expected}");
                            if (site.RandomTerrain != expectedTerrain) mismatches.Add($"{state}: random terrain {site.RandomTerrain}, old switch {expectedTerrain}");
                            if (site.TakesCombatItems != expectedItems) mismatches.Add($"{state}: items {site.TakesCombatItems}, old switch {expectedItems}");
                            if (site.FixedTags != expectedTags) mismatches.Add($"{state}: tags {site.FixedTags}, old switch {expectedTags}");
                            if (actualSeed != Runtime.RandSeed) mismatches.Add($"{state}: random draws differ");
                            names.Add(actual.Length == 8 && actual[0] == 'e' ? actual[..7] + "?" : actual);
                        }
                output.WriteLine($"{type.Name} ({EClass.Of(type).ClassID}): {string.Join(" ", names)}");
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
            var declaring = type.GetMethod(nameof(TStructure.CombatMapName))!.DeclaringType!;
            Assert.True(declaring == type || declaring == typeof(TStructure), $"{type.Name} inherits CombatMapName from {declaring.Name}");
            var owner = type.GetMethod("OwnerPanel", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            resolved.AppendLine($"{type.Name}: InfoPanel {Declaring(type, nameof(TStructure.InfoPanel))}" +
                (owner is null ? "" : $" (OwnerPanel {owner.DeclaringType!.Name})") + ", " +
                $"InfoButton {Declaring(type, nameof(TStructure.InfoButton))}, DoubleClick {Declaring(type, nameof(TStructure.DoubleClick))}, " +
                $"CombatMapName {declaring.Name}");
        }
        output.WriteLine(resolved.ToString());
    }

    [Fact]
    public void TheScreensRegisterEveryOriginalView()
    {
        Assert.Equal(19, TIWindow.StructurePanels.All.Count);
        Assert.Equal(2, TIWindow.StructureButtons.All.Count);
        Assert.Equal(2, TMapEvents.StructureScreens.All.Count);
        Assert.False(TIWindow.StructureButtons.TryGet(StructureView.None, out _));
        Assert.False(TMapEvents.StructureScreens.TryGet(StructureView.None, out _));
        var duplicate = Assert.Throws<InvalidOperationException>(() =>
            TIWindow.StructurePanels.Add(StructureView.Generic, static (_, _) => { }));
        Assert.Contains("structure info panel", duplicate.Message);
    }
}
