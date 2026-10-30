using System;
using System.Collections.Concurrent;
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

// One descriptor per action class. Holder<T> is the only constructor, so a type has one descriptor
// and the registered list compares by reference.
public sealed class AIPlayerActionClass
{
    private static readonly ConcurrentDictionary<Type, AIPlayerActionClass> byType = new();

    private AIPlayerActionClass(EClass eclass, int priority, TSet<TAIPlayerProcessPhase> supportedPhases)
    {
        Class = eclass;
        Priority = priority;
        SupportedPhases = supportedPhases;
    }

    public EClass Class { get; }
    public int Priority { get; }
    public TSet<TAIPlayerProcessPhase> SupportedPhases { get; }
    public TAIPlayerAction Create(TECustomNode AOwner) => (TAIPlayerAction)Class.Create(AOwner);

    public static AIPlayerActionClass Of<T>() where T : TAIPlayerAction, IAIPlayerActionClass => Holder<T>.Value;

    // The descriptor of a type known only at run time, read from the same Holder<T> as Of<T>().
    public static AIPlayerActionClass Of(Type type) => byType.GetOrAdd(type, static t =>
        (AIPlayerActionClass)typeof(Holder<>).MakeGenericType(t).GetField(nameof(Holder<TAIPlayerAction>.Value))!.GetValue(null)!);

    private static class Holder<T> where T : TAIPlayerAction, IAIPlayerActionClass
    {
        public static readonly AIPlayerActionClass Value = new(EClass.Of<T>(), T.Priority, T.SupportedPhases);
    }
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

// One descriptor per group class. Holder<T> is the only constructor, so a type has one descriptor
// and the registered list compares by reference.
public sealed class AIGroupClass
{
    private static readonly ConcurrentDictionary<Type, AIGroupClass> byType = new();

    private AIGroupClass(EClass eclass, TGroupBehavior behavior)
    {
        Class = eclass;
        Behavior = behavior;
    }

    public EClass Class { get; }
    public TGroupBehavior Behavior { get; }
    public TAIGroup Create(TECustomNode AOwner) => (TAIGroup)Class.Create(AOwner);

    public static AIGroupClass Of<T>() where T : TAIGroup, IAIGroupClass => Holder<T>.Value;

    // The descriptor of a type known only at run time, read from the same Holder<T> as Of<T>().
    public static AIGroupClass Of(Type type) => byType.GetOrAdd(type, static t =>
        (AIGroupClass)typeof(Holder<>).MakeGenericType(t).GetField(nameof(Holder<TAIGroup>.Value))!.GetValue(null)!);

    private static class Holder<T> where T : TAIGroup, IAIGroupClass
    {
        public static readonly AIGroupClass Value = new(EClass.Of<T>(), T.Behavior);
    }
}

public partial class TAIGroup
{
    private AIGroupClass groupClass;
    // The group descriptor of the instance's exact type, read once and cached like EClass.
    public AIGroupClass GroupClass => groupClass ??= AIGroupClass.Of(GetType());
}
