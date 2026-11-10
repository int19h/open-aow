using Aow.Game;
using Aow.Game.Engine;
namespace Aow.Units.Tests;

/// <summary>Pins the unit table of <see cref="global::Aow.Game.Startup"/>: the order of the initialization and
/// finalization sections, for the full set and for a selection, as the nested chain before the
/// table ran them.</summary>
public sealed class StartupOrderTests
{
    // The live steps, in initialization order. Finalization runs the steps that have a finalize, reversed.
    private const string Names = "GFXE ImageLib Engine Sound AoWE City BehaviorAG MainAG IndependentAG AoWEReg AoWTitleScene";

    private const string Initializations = Names;

    private const string Finalizations = "IndependentAG MainAG BehaviorAG City AoWE Sound Engine ImageLib GFXE";

    private static readonly (string Name, string Type, string Init, string Fin)[] Delegates =
    [
        ("GFXE", "Aow.Game.Draw.GfxState", "Initialize", "Finalize"),
        ("ImageLib", "Aow.Game.Draw.Images.ImageLibraries", "Initialize", "Finalize"),
        ("Engine", "Aow.Engine.Nodes.EngineHost", "Initialize", "Finalize"),
        ("Sound", "Aow.Game.Audio.AudioHost", "Initialize", "Finalize"),
        ("AoWE", "Aow.Game.Magic.Abilities.GameTables", "Initialize", "Finalize"),
        ("City", "Aow.Game.World.Structures.CityRegistrator", "SubscribeMapLoaded", "UnsubscribeMapLoaded"),
        ("BehaviorAG", "Aow.Game.Ai.Groups.BehaviorRegistrator", "SubscribeMapLoaded", "UnsubscribeMapLoaded"),
        ("MainAG", "Aow.Game.Ai.Groups.MainRegistrator", "SubscribeMapLoaded", "UnsubscribeMapLoaded"),
        ("IndependentAG", "Aow.Game.Ai.Groups.IndependentRegistrator", "SubscribeMapLoaded", "UnsubscribeMapLoaded"),
        ("AoWEReg", "Aow.Game.GameClasses", "Register", ""),
        ("AoWTitleScene", "Aow.Game.Ui.Dialogs.TitleCaptions", "Initialize", ""),
    ];

    private static string[] Split(string text) => text.Split((char[])[' ', '\n'], StringSplitOptions.RemoveEmptyEntries);

    private static void AssertDelegate(Action? section, string type, string name)
    {
        if (name.Length == 0)
        {
            Assert.Null(section);
            return;
        }
        Assert.NotNull(section);
        Assert.Equal(name, section.Method.Name);
        Assert.Equal(type, section.Method.DeclaringType!.FullName);
    }

    private static (StartupUnit[] Units, List<string> Calls) Recording()
    {
        var calls = new List<string>();
        var units = Startup.Units.Select(u => new StartupUnit(u.Name,
            u.Initialize is null ? null! : () => calls.Add("I " + u.Name),
            u.Finalize is null ? null! : () => calls.Add("F " + u.Name))).ToArray();
        return (units, calls);
    }

    [Fact]
    public void TableKeepsTheUnitOrder()
    {
        Assert.Equal(Split(Names), Startup.Units.Select(u => u.Name));
        Assert.Equal(Delegates.Length, Startup.Units.Length);
        for (int i = 0; i < Delegates.Length; i++)
        {
            Assert.Equal(Delegates[i].Name, Startup.Units[i].Name);
            AssertDelegate(Startup.Units[i].Initialize, Delegates[i].Type, Delegates[i].Init);
            AssertDelegate(Startup.Units[i].Finalize, Delegates[i].Type, Delegates[i].Fin);
        }
    }

    [Fact]
    public void FullSetInitializesInOrderAndFinalizesInReverse()
    {
        var (units, calls) = Recording();
        var state = new bool[units.Length];
        Startup.InitializeUnits(units, state, null!);
        Assert.Equal(Split(Initializations).Select(n => "I " + n), calls);
        Assert.All(state, Assert.True);
        calls.Clear();
        Startup.InitializeUnits(units, state, null!);
        Assert.Empty(calls);
        Startup.FinalizeUnits(units, state, null!);
        Assert.Equal(Split(Finalizations).Select(n => "F " + n), calls);
        Assert.All(state, Assert.False);
    }

    [Fact]
    public void SelectionLimitsInitializationAndFinalization()
    {
        var (units, calls) = Recording();
        var state = new bool[units.Length];
        Startup.InitializeUnits(units, state, ["AoWTitleScene", "Engine", "ImageLib", "Sound"]);
        Assert.Equal(["I ImageLib", "I Engine", "I Sound", "I AoWTitleScene"], calls);
        calls.Clear();
        // Units outside the finalization selection stay initialized. The title step has no finalize,
        // and GFXE was not initialized.
        Startup.FinalizeUnits(units, state, ["Engine", "AoWTitleScene", "GFXE"]);
        Assert.Equal(["F Engine"], calls);
        calls.Clear();
        // Only initialized units are finalized: GFXE was never initialized.
        Startup.FinalizeUnits(units, state, null!);
        Assert.Equal(["F Sound", "F ImageLib"], calls);
        Assert.All(state, Assert.False);
    }

    [Fact]
    public void FailingInitializationStopsTheRun()
    {
        var calls = new List<string>();
        StartupUnit[] units = [new("A", () => calls.Add("A"), null!), new("B", () => throw new InvalidOperationException("B"), null!),
            new("C", () => calls.Add("C"), null!)];
        var state = new bool[units.Length];
        Assert.Throws<InvalidOperationException>(() => Startup.InitializeUnits(units, state, null!));
        Assert.Equal(["A"], calls);
        Assert.Equal([true, false, false], state);
    }

    [Fact]
    public void FailingFinalizationFinalizesTheRestAndRethrowsTheLastFailure()
    {
        var calls = new List<string>();
        StartupUnit[] units = [new("A", null!, () => throw new InvalidOperationException("A")), new("B", null!, () => calls.Add("B")),
            new("C", null!, () => throw new InvalidOperationException("C")), new("D", null!, () => calls.Add("D"))];
        bool[] state = [true, true, true, true];
        var failure = Assert.Throws<InvalidOperationException>(() => Startup.FinalizeUnits(units, state, null!));
        Assert.Equal("A", failure.Message);
        Assert.Equal(["D", "B"], calls);
        Assert.All(state, Assert.False);
    }
}
