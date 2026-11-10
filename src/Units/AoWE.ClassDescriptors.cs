using Aow.Game.Ai;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Session;
using Aow.Rtl;

namespace Aow.Game.Session
{

    // The class-side members of an AI player action class that the action manager reads without an
    // instance (AI.pas TAIPlayerAction class methods). A class with its own values re-lists the
    // interface and hides the inherited statics with `new`, a class without one inherits them.
    public interface IAIPlayerActionClass
    {
        static abstract int Priority { get; }
        static abstract Set<AIPlayerProcessPhase> SupportedPhases { get; }
    }
}

namespace Aow.Game.Ai
{

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

    public partial class AIPlayerAction
    {
        private AIPlayerActionClass actionClass;

        // The action descriptor of the instance's exact type, read once and cached like EClass.
        public AIPlayerActionClass ActionClass => actionClass ??= AIPlayerActionClass.Of(GetType());
    }
}

namespace Aow.Game.Ai.Groups
{

    // The class-side member of an AI group class that the group manager reads before it creates a
    // group (AIGroup.pas TAIGroup.Behavior).
    public interface IAIGroupClass
    {
        static abstract GroupBehavior Behavior { get; }
    }

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

    public partial class AIGroup
    {
        private AIGroupClass groupClass;

        // The group descriptor of the instance's exact type, read once and cached like EClass.
        public AIGroupClass GroupClass => groupClass ??= AIGroupClass.Of(GetType());
    }
}
