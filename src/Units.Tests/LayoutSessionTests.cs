using System.Reflection;
using Aow.Layout;
using Aow.Rtl;
using Aow.Units.AoWLabel;

namespace Aow.Units.Tests;

// Brief 9.3b-12: the layout session owns the loading flags and the module failure boundary.
public sealed class LayoutSessionTests
{
    private sealed class Tracked : TComponent
    {
        public Tracked(TComponent owner) : base(owner) { }

        public bool Destroyed { get; private set; }

        public override void Dispose() { Destroyed = true; base.Dispose(); }
    }

    // The second label is complete and owned when its Name setter rejects the duplicate.
    private class FailingModule : TDataModule, ILayoutModule
    {
        public FailingModule() : base(null!) { }

        public Tracked? First;
        public TAOWLabel? Second;

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "Failing";
            First = new Tracked(this);
            session.BeginRead(First);
            First.Name = "Child";
            session.EndRead(First);
            Second = new TAOWLabel(this);
            session.BeginRead(Second);
            Second.Name = "Child";
            session.EndRead(Second);
            session.Resolve(this);
        }
    }

    private sealed class ThrowingCleanupModule : FailingModule
    {
        public override void Dispose() { base.Dispose(); throw new InvalidOperationException("cleanup"); }
    }

    private sealed class GoodModule : TDataModule, ILayoutModule
    {
        public GoodModule() : base(null!) { }

        public Tracked? Child;

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

    private static FailingModule? FailingGlobal;
    private static GoodModule? GoodGlobal;

    [Fact]
    public void FailingModuleLeavesItsGlobalNullAndItsTreeDestroyed()
    {
        var session = new LayoutSession();
        var module = new FailingModule();
        FailingGlobal = null;
        var error = Assert.Throws<ArgumentException>(() => session.Load(module, value => FailingGlobal = value));
        Assert.Contains("Child", error.Message);
        Assert.Null(FailingGlobal);
        Assert.Equal(0, module.ComponentCount);
        Assert.True(module.ComponentState.Contains(TComponentState.csDestroying));
        Assert.True(module.First!.Destroyed);
        Assert.Null(module.First.Owner);
        Assert.NotNull(module.Second);
        Assert.Null(module.Second!.Owner);
        Assert.True(module.Second.ComponentState.Contains(TComponentState.csDestroying));
        Assert.Empty(session.Modules);
    }

    [Fact]
    public void CleanupFailureIsReportedWithTheOriginal()
    {
        var session = new LayoutSession();
        var module = new ThrowingCleanupModule();
        var error = Assert.Throws<AggregateException>(() => session.Load(module, value => FailingGlobal = value));
        Assert.Null(FailingGlobal);
        Assert.Collection(error.InnerExceptions,
            original => Assert.IsType<ArgumentException>(original),
            cleanup => Assert.Equal("cleanup", cleanup.Message));
        Assert.Equal(0, module.ComponentCount);
    }

    [Fact]
    public void SuccessfulLoadPublishesBeforeInitializationAndKeepsTheGlobal()
    {
        var session = new LayoutSession();
        using var module = new GoodModule();
        GoodModule? seen = null;
        session.Load(module, value => { GoodGlobal = value; seen ??= value; });
        Assert.Same(module, GoodGlobal);
        Assert.Same(module, seen);
        Assert.Same(module, session.Modules["Good"]);
        Assert.Equal(1, module.ComponentCount);
        Assert.False(module.Child!.Destroyed);
        Assert.True(module.ComponentState.IsEmpty);
    }

    [Fact]
    public void LoadingFlagsAreSetByTheSessionAndClearedInTwoSteps()
    {
        var session = new LayoutSession();
        using var root = new TDataModule(null!);
        var child = new TAOWLabel(root);
        Assert.True(child.ComponentState.IsEmpty);
        session.Begin(root);
        Assert.True(root.ComponentState.Contains(TComponentState.csLoading));
        Assert.True(root.ComponentState.Contains(TComponentState.csReading));
        session.BeginRead(child);
        Assert.True(child.ComponentState.Contains(TComponentState.csLoading));
        Assert.True(child.ComponentState.Contains(TComponentState.csReading));
        session.EndRead(child);
        Assert.False(child.ComponentState.Contains(TComponentState.csReading));
        Assert.True(child.ComponentState.Contains(TComponentState.csLoading));
        child.Loaded();
        Assert.True(child.ComponentState.IsEmpty);
        session.EndRead(root);
        root.Loaded();
        Assert.True(root.ComponentState.IsEmpty);
    }

}
