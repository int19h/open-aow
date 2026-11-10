using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

// One explicit registration per group class keeps descriptor identity in the game lists.
public sealed class AIGroupClass
{
    private static readonly Catalog<Type, AIGroupClass> catalog = new("AI group class");

    private AIGroupClass(EClass eclass, GroupBehavior behavior)
    {
        Class = eclass;
        Behavior = behavior;
    }

    public EClass Class { get; }
    public string ClassName => Class.ClassName;
    public GroupBehavior Behavior { get; }

    public AIGroup Create(ECustomNode AOwner) => (AIGroup)Class.Create(AOwner);

    public static AIGroupClass Register<T>() where T : AIGroup, IAIGroupClass =>
        catalog.Add(typeof(T), new AIGroupClass(EClass.Of<T>(), T.Behavior));

    public static AIGroupClass Of<T>() where T : AIGroup, IAIGroupClass => catalog[typeof(T)];
    public static AIGroupClass Of(Type type) => catalog[type];

    public static IReadOnlyList<AIGroupClass> All => catalog.All;
}
