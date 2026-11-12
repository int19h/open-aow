using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// One explicit registration per group class keeps descriptor identity in the game lists.
public sealed class AIGroupClass
{
    private static readonly Catalog<Type, AIGroupClass> s_catalog = new("AI group class");

    private AIGroupClass(EngineClass engineClass, GroupBehavior behavior)
    {
        Class = engineClass;
        Behavior = behavior;
    }

    public EngineClass Class { get; }
    public string StorageKey => Class.StorageKey;
    public GroupBehavior Behavior { get; }

    public AIGroup CreateInstance(ECustomNode owner, EngineObject holder) => (AIGroup)Class.CreateInstance(owner, holder);

    public static AIGroupClass Register<T>() where T : AIGroup, IAIGroupClass =>
        s_catalog.Add(typeof(T), new AIGroupClass(EngineClass.Of<T>(), T.Behavior));

    public static AIGroupClass Of<T>() where T : AIGroup, IAIGroupClass => s_catalog[typeof(T)];
    public static AIGroupClass Of(Type type) => s_catalog[type];

    public static IReadOnlyList<AIGroupClass> All => s_catalog.All;
}
