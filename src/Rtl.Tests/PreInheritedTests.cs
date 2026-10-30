using Aow.Units.Engine;

namespace Aow.Rtl.Tests;

public sealed class PreInheritedTests
{
    private static readonly List<string> Events = [];
    private static bool fail;
    private static int destroyed;
    private sealed class Owner : TECustomNode
    {
        public override int AddChild(TEObject child)
        {
            Assert.Equal(17, ((Leaf)child).State);
            Events.Add("owner");
            return 0;
        }
    }
    private class First : TEObject
    {
        protected First() { }
        private protected First(ConstructionContext? context, TECustomNode? owner) : base(context!, owner!) => Events.Add("first body");
    }
    private class Middle : First
    {
        protected Middle() { }
        private protected Middle(ConstructionContext? context, TECustomNode? owner) : base(context, owner) => Events.Add("middle body");
    }
    private sealed class Leaf : Middle
    {
        private readonly int initialized = InitializeField();
        internal int State;
        internal Leaf() { }
        internal Leaf(ConstructionContext? context, TECustomNode? owner) : base(context, owner) => Events.Add("leaf body");
        private static int InitializeField() { Events.Add("leaf field"); return 1; }
        private protected sealed override void InitializeBeforeInherited()
        {
            Assert.Equal(1, initialized);
            Events.Add("hook");
            State = 17;
            if (fail) throw new InvalidDataException("hook failure");
        }
        public override void Destroy()
        {
            Assert.Equal(17, State);
            destroyed++;
            Events.Add("destroy");
            base.Destroy();
        }
    }
    [Fact]
    public void FieldHookInsertionAndThreeBodiesRunInDelphiOrder()
    {
        Events.Clear();
        fail = false;
        var owner = new Owner();
        _ = new Leaf(null, owner);
        Assert.Equal(["leaf field", "hook", "owner", "first body", "middle body", "leaf body"], Events);
    }
    [Fact]
    public void RawAndLegacyRoutesSkipHook()
    {
        Events.Clear();
        fail = false;
        var value = new Leaf();
        Assert.Equal(["leaf field"], Events);
        value.Create__init(null!);
        Assert.Equal(["leaf field"], Events);
        Assert.Equal(0, value.State);
    }
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CapturePrecedesHookAndOnlyContextDestroysFailure(bool guarded)
    {
        Events.Clear();
        fail = true;
        destroyed = 0;
        try
        {
            var error = Assert.Throws<InvalidDataException>(() => guarded
                ? ConstructionContext.Create(static context => new Leaf(context, null))
                : new Leaf(null, null));
            Assert.Equal("hook failure", error.Message);
            Assert.Equal(guarded ? 1 : 0, destroyed);
            Assert.Equal(guarded ? ["leaf field", "hook", "destroy"] : ["leaf field", "hook"], Events);
        }
        finally { fail = false; }
    }
}
