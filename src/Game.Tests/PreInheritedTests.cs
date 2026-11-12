using Aow.Engine.Nodes;

namespace Aow.Game.Tests;

public sealed class PreInheritedTests
{
    private static readonly List<string> s_events = [];
    private static bool s_fail;
    private static int s_destroyed;

    private sealed class HookOwner : EngineFixture.Owner
    {

        public override int AddChild(EngineObject child)
        {
            Assert.Equal(17, ((Leaf)child).State);
            s_events.Add("owner");
            return 0;
        }
    }

    private class First : EngineObject
    {

        private protected First(ConstructionContext? context, ECustomNode? owner) : base(context!, owner!) => s_events.Add("first body");
    }

    private class Middle : First
    {

        private protected Middle(ConstructionContext? context, ECustomNode? owner) : base(context, owner) => s_events.Add("middle body");
    }

    private sealed class Leaf : Middle
    {
        private readonly int _initialized = InitializeField();

        internal int State { get; private set; }

        internal Leaf(ConstructionContext? context, ECustomNode? owner) : base(context, owner) => s_events.Add("leaf body");

        private static int InitializeField() { s_events.Add("leaf field"); return 1; }

        private protected sealed override void InitializeBeforeInherited()
        {
            Assert.Equal(1, _initialized);
            s_events.Add("hook");
            State = 17;
            if (s_fail) throw new InvalidDataException("hook failure");
        }

        public override void Dispose()
        {
            Assert.Equal(17, State);
            s_destroyed++;
            s_events.Add("destroy");
            base.Dispose();
        }
    }

    [Fact]
    public void FieldHookInsertionAndThreeBodiesRunInDelphiOrder()
    {
        s_events.Clear();
        s_fail = false;
        var owner = new HookOwner();
        _ = new Leaf(null, owner);
        Assert.Equal(["leaf field", "hook", "owner", "first body", "middle body", "leaf body"], s_events);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CapturePrecedesHookAndOnlyContextDestroysFailure(bool guarded)
    {
        s_events.Clear();
        s_fail = true;
        s_destroyed = 0;
        try
        {
            var error = Assert.Throws<InvalidDataException>(() => guarded
                ? ConstructionContext.Create(static context => new Leaf(context, null))
                : new Leaf(null, null));
            Assert.Equal("hook failure", error.Message);
            Assert.Equal(guarded ? 1 : 0, s_destroyed);
            Assert.Equal(guarded ? ["leaf field", "hook", "destroy"] : ["leaf field", "hook"], s_events);
        }
        finally { s_fail = false; }
    }
}
