
using Aow.Game.Layout;
using Aow.Game.UI.Controls;

namespace Aow.Game.Tests;

// Brief 9.3b-12: the layout session owns the loading flags and the module failure boundary.
public sealed class LayoutSessionTests
{
    private sealed class Tracked(Component owner) : Component(owner)
    {
        public bool Destroyed { get; private set; }

        public override void Dispose()
        {
            Destroyed = true;
            base.Dispose();
        }
    }

    // The second label is complete and owned when its Name setter rejects the duplicate.
    private class FailingModule : DataModule, ILayoutModule
    {
        public FailingModule() : base(null) { }

        public Tracked? First { get; private set; }
        public Label? Second { get; private set; }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "Failing";
            First = new Tracked(this);
            session.BeginRead(First);
            First.Name = "Child";
            session.EndRead(First);
            Second = new Label(this);
            session.BeginRead(Second);
            Second.Name = "Child";
            session.EndRead(Second);
            session.Resolve(this);
        }
    }

    private sealed class ThrowingCleanupModule : FailingModule
    {
        public override void Dispose()
        {
            base.Dispose();
            throw new InvalidOperationException("cleanup");
        }
    }

    private sealed class GoodModule : DataModule, ILayoutModule
    {
        public GoodModule() : base(null) { }

        public Tracked? Child { get; private set; }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "Good";
            Child = new Tracked(this);
            session.BeginRead(Child);
            Child.Name = "Child";
            session.EndRead(Child);
            session.EndRead(this);
            Child.Loaded();
            Loaded();
            session.Resolve(this);
        }
    }

    private static FailingModule? s_failingGlobal;
    private static GoodModule? s_goodGlobal;

    [Fact]
    public void FailingModuleLeavesItsGlobalNullAndItsTreeDestroyed()
    {
        LayoutSession session = new();
        FailingModule module = new();
        s_failingGlobal = null;
        ArgumentException error = Assert.Throws<ArgumentException>(() => session.Load(module, value => s_failingGlobal = value));
        Assert.Contains("Child", error.Message);
        Assert.Null(s_failingGlobal);
        Assert.Equal(0, module.ComponentCount);
        Assert.True(module.ComponentState.Contains(ComponentStates.Destroying));
        Assert.True(module.First!.Destroyed);
        Assert.Null(module.First.Owner);
        Assert.NotNull(module.Second);
        Assert.Null(module.Second!.Owner);
        Assert.True(module.Second.ComponentState.Contains(ComponentStates.Destroying));
        Assert.Empty(session.Modules);
    }

    [Fact]
    public void CleanupFailureIsReportedWithTheOriginal()
    {
        LayoutSession session = new();
        ThrowingCleanupModule module = new();
        AggregateException error = Assert.Throws<AggregateException>(() => session.Load(module, value => s_failingGlobal = value));
        Assert.Null(s_failingGlobal);
        Assert.Collection(error.InnerExceptions,
            original => Assert.IsType<ArgumentException>(original),
            cleanup => Assert.Equal("cleanup", cleanup.Message));
        Assert.Equal(0, module.ComponentCount);
    }

    [Fact]
    public void SuccessfulLoadPublishesBeforeInitializationAndKeepsTheGlobal()
    {
        LayoutSession session = new();
        using GoodModule module = new();
        GoodModule? seen = null;
        session.Load(module, value =>
        {
            s_goodGlobal = value;
            seen ??= value;
        });
        Assert.Same(module, s_goodGlobal);
        Assert.Same(module, seen);
        Assert.Same(module, session.Modules["Good"]);
        Assert.Equal(1, module.ComponentCount);
        Assert.False(module.Child!.Destroyed);
        Assert.True(module.ComponentState == ComponentStates.None);
    }

    [Fact]
    public void LoadingFlagsAreSetByTheSessionAndClearedInTwoSteps()
    {
        LayoutSession session = new();
        using DataModule root = new(null);
        Label child = new(root);
        Assert.True(child.ComponentState == ComponentStates.None);
        session.Begin(root);
        Assert.True(root.ComponentState.Contains(ComponentStates.Loading));
        Assert.True(root.ComponentState.Contains(ComponentStates.Reading));
        session.BeginRead(child);
        Assert.True(child.ComponentState.Contains(ComponentStates.Loading));
        Assert.True(child.ComponentState.Contains(ComponentStates.Reading));
        session.EndRead(child);
        Assert.False(child.ComponentState.Contains(ComponentStates.Reading));
        Assert.True(child.ComponentState.Contains(ComponentStates.Loading));
        child.Loaded();
        Assert.True(child.ComponentState == ComponentStates.None);
        session.EndRead(root);
        root.Loaded();
        Assert.True(root.ComponentState == ComponentStates.None);
    }
}
