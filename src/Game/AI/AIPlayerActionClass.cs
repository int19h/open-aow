using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Game.Session;
using Aow.Rtl;

namespace Aow.Game.AI;

// One explicit registration per action class keeps descriptor identity in the game lists.
public sealed class AIPlayerActionClass
{
    private static readonly Catalog<Type, AIPlayerActionClass> s_catalog = new("AI player action class");
    private readonly Func<int> _priority;

    private AIPlayerActionClass(EngineClass engineClass, Func<int> priority, Set<AIPlayerProcessPhase> supportedPhases)
    {
        Class = engineClass;
        this._priority = priority;
        SupportedPhases = supportedPhases;
    }

    public EngineClass Class { get; }
    public string StorageKey => Class.StorageKey;
    // Abstract base actions register without invoking their abstract priority getter.
    public int Priority => _priority();
    public Set<AIPlayerProcessPhase> SupportedPhases { get; }

    public AIPlayerAction CreateInstance(ECustomNode owner, EngineObject holder) => (AIPlayerAction)Class.CreateInstance(owner, holder);

    public static AIPlayerActionClass Register<T>() where T : AIPlayerAction, IAIPlayerActionClass =>
        s_catalog.Add(typeof(T), new AIPlayerActionClass(EngineClass.Of<T>(), static () => T.Priority, T.SupportedPhases));

    public static AIPlayerActionClass Of<T>() where T : AIPlayerAction, IAIPlayerActionClass => s_catalog[typeof(T)];
    public static AIPlayerActionClass Of(Type type) => s_catalog[type];

    public static IReadOnlyList<AIPlayerActionClass> All => s_catalog.All;
}
