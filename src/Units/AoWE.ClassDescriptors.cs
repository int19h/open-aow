using System;
using Aow.Rtl;
using Aow.Units.Engine;

namespace Aow.Units.AoWE;

// The class-side members of an AI player action class that the action manager reads without an
// instance (AI.pas TAIPlayerAction class methods). A class with its own values re-lists the
// interface and hides the inherited statics with `new`, a class without one inherits them.
public interface IAIPlayerActionClass
{
    static abstract int Priority { get; }
    static abstract TSet<TAIPlayerProcessPhase> SupportedPhases { get; }
}

// One explicit registration per action class keeps descriptor identity in the game lists.
public sealed class AIPlayerActionClass
{
    private static readonly Catalog<Type, AIPlayerActionClass> catalog = new("AI player action class");
    private readonly Func<int> priority;

    private AIPlayerActionClass(EClass eclass, Func<int> priority, TSet<TAIPlayerProcessPhase> supportedPhases)
    {
        Class = eclass;
        this.priority = priority;
        SupportedPhases = supportedPhases;
    }

    public EClass Class { get; }
    public string ClassName => Class.ClassName;
    // Abstract base actions register without invoking their abstract priority getter.
    public int Priority => priority();
    public TSet<TAIPlayerProcessPhase> SupportedPhases { get; }
    public TAIPlayerAction Create(TECustomNode AOwner) => (TAIPlayerAction)Class.Create(AOwner);

    public static AIPlayerActionClass Register<T>() where T : TAIPlayerAction, IAIPlayerActionClass =>
        catalog.Add(typeof(T), new AIPlayerActionClass(EClass.Of<T>(), static () => T.Priority, T.SupportedPhases));
    public static AIPlayerActionClass Of<T>() where T : TAIPlayerAction, IAIPlayerActionClass => catalog[typeof(T)];
    public static AIPlayerActionClass Of(Type type) => catalog[type];
    public static IReadOnlyList<AIPlayerActionClass> All => catalog.All;
}

public partial class TAIPlayerAction
{
    private AIPlayerActionClass actionClass;
    // The action descriptor of the instance's exact type, read once and cached like EClass.
    public AIPlayerActionClass ActionClass => actionClass ??= AIPlayerActionClass.Of(GetType());
}

// The class-side member of an AI group class that the group manager reads before it creates a
// group (AIGroup.pas TAIGroup.Behavior).
public interface IAIGroupClass
{
    static abstract TGroupBehavior Behavior { get; }
}

// One explicit registration per group class keeps descriptor identity in the game lists.
public sealed class AIGroupClass
{
    private static readonly Catalog<Type, AIGroupClass> catalog = new("AI group class");

    private AIGroupClass(EClass eclass, TGroupBehavior behavior)
    {
        Class = eclass;
        Behavior = behavior;
    }

    public EClass Class { get; }
    public string ClassName => Class.ClassName;
    public TGroupBehavior Behavior { get; }
    public TAIGroup Create(TECustomNode AOwner) => (TAIGroup)Class.Create(AOwner);

    public static AIGroupClass Register<T>() where T : TAIGroup, IAIGroupClass =>
        catalog.Add(typeof(T), new AIGroupClass(EClass.Of<T>(), T.Behavior));
    public static AIGroupClass Of<T>() where T : TAIGroup, IAIGroupClass => catalog[typeof(T)];
    public static AIGroupClass Of(Type type) => catalog[type];
    public static IReadOnlyList<AIGroupClass> All => catalog.All;
}

public partial class TAIGroup
{
    private AIGroupClass groupClass;
    // The group descriptor of the instance's exact type, read once and cached like EClass.
    public AIGroupClass GroupClass => groupClass ??= AIGroupClass.Of(GetType());
}
