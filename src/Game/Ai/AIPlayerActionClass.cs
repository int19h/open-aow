using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Game.Session;
using Aow.Rtl;

namespace Aow.Game.Ai;

// One explicit registration per action class keeps descriptor identity in the game lists.
public sealed class AIPlayerActionClass
{
    private static readonly Catalog<Type, AIPlayerActionClass> catalog = new("AI player action class");
    private readonly Func<int> priority;

    private AIPlayerActionClass(EClass eclass, Func<int> priority, Set<AIPlayerProcessPhase> supportedPhases)
    {
        Class = eclass;
        this.priority = priority;
        SupportedPhases = supportedPhases;
    }

    public EClass Class { get; }
    public string ClassName => Class.ClassName;
    // Abstract base actions register without invoking their abstract priority getter.
    public int Priority => priority();
    public Set<AIPlayerProcessPhase> SupportedPhases { get; }

    public AIPlayerAction Create(ECustomNode AOwner) => (AIPlayerAction)Class.Create(AOwner);

    public static AIPlayerActionClass Register<T>() where T : AIPlayerAction, IAIPlayerActionClass =>
        catalog.Add(typeof(T), new AIPlayerActionClass(EClass.Of<T>(), static () => T.Priority, T.SupportedPhases));

    public static AIPlayerActionClass Of<T>() where T : AIPlayerAction, IAIPlayerActionClass => catalog[typeof(T)];
    public static AIPlayerActionClass Of(Type type) => catalog[type];

    public static IReadOnlyList<AIPlayerActionClass> All => catalog.All;
}
