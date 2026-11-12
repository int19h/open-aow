using System.Runtime.CompilerServices;

namespace Aow.Rtl.Tests;

public sealed class ConstructionContextTests
{
    private sealed class ConstructionTrace
    {
        internal List<string> Facts { get; } = [];
        internal List<ConstructionContext> Contexts { get; } = [];

        internal void Fact(string label, string value) => Facts.Add($"{label} {value}");
        internal void Number(string label, int value) => Fact(label, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }

    private sealed class Construct(string message) : System.Exception(message);

    private sealed class Cleanup(string message) : System.Exception(message);

    private class Captured
    {
        protected ConstructionTrace ConstructionTrace { get; }

        protected Captured(ConstructionContext context, ConstructionTrace trace)
        {
            context.Capture(this);
            ConstructionTrace = trace;
            trace.Contexts.Add(context);
        }
    }

    private class Base : Captured
, IDisposable
    {
        private readonly int _baseValue;

        protected Base(ConstructionContext context, ConstructionTrace trace, int failAt) : base(context, trace)
        {
            _baseValue = 10;
            trace.Number("base_body", _baseValue);
            try { if (failAt == 1) throw new Construct("base"); }
            finally { trace.Fact("base_unwind", "done"); }
        }

        public virtual void Dispose()
        {
            ConstructionTrace.Number("base_destroy", _baseValue);
        }
    }

    private class Middle : Base
    {
        private readonly int _middleValue;

        protected Middle(ConstructionContext context, ConstructionTrace trace, int failAt) : base(context, trace, failAt)
        {
            _middleValue = 20;
            trace.Number("middle_body", _middleValue);
            try { if (failAt == 2) throw new Construct("middle"); }
            finally { trace.Fact("middle_unwind", "done"); }
        }

        public override void Dispose()
        {
            ConstructionTrace.Number("middle_destroy", _middleValue);
            base.Dispose();
        }
    }

    private sealed class Leaf : Middle
    {
        private readonly int _leafValue;

        private Leaf(ConstructionContext context, ConstructionTrace trace, int failAt) : base(context, trace, failAt)
        {
            _leafValue = 30;
            trace.Number("leaf_body", _leafValue);
            try { if (failAt == 3) throw new Construct("leaf"); }
            finally { trace.Fact("leaf_unwind", "done"); }
        }

        internal static Leaf New(ConstructionTrace trace, int failAt) => ConstructionContext.Create(context => new Leaf(context, trace, failAt));

        public override void Dispose()
        {
            ConstructionTrace.Number("leaf_destroy", _leafValue);
            base.Dispose();
        }
    }

    private sealed class Owned : Captured
, IDisposable
    {
        private readonly List<object> _owner;

        private Owned(ConstructionContext context, ConstructionTrace trace, List<object> owner) : base(context, trace)
        {
            this._owner = owner;
            owner.Add(this);
            trace.Number("owner_inserted", owner.Count);
            throw new Construct("owner");
        }

        internal static Owned New(ConstructionTrace trace, List<object> owner) => ConstructionContext.Create(context => new Owned(context, trace, owner));

        public void Dispose()
        {
            ConstructionTrace.Number("owner_at_destroy", _owner.Count);
            _owner.Remove(this);
            ConstructionTrace.Number("owner_removed", _owner.Count);
        }
    }

    private sealed class Parent : Captured
, IDisposable
    {
        private readonly Leaf? _child;

        private Parent(ConstructionContext context, ConstructionTrace trace) : base(context, trace)
        {
            trace.Fact("parent_body", "enter");
            try { _child = Leaf.New(trace, 2); }
            finally { trace.Number("parent_unwind_child_nil", _child is null ? 1 : 0); }
        }

        internal static Parent New(ConstructionTrace trace) => ConstructionContext.Create(context => new Parent(context, trace));

        public void Dispose()
        {
            ConstructionTrace.Number("parent_destroy_child_nil", _child is null ? 1 : 0);
            _child?.Dispose();
        }
    }

    private sealed class Throwing : Base
    {
        private Throwing(ConstructionContext context, ConstructionTrace trace) : base(context, trace, 0) => throw new Construct("original");
        internal static Throwing New(ConstructionTrace trace) => ConstructionContext.Create(context => new Throwing(context, trace));

        public override void Dispose()
        {
            ConstructionTrace.Fact("throwing_destroy", "enter");
            base.Dispose();
            throw new Cleanup("replacement");
        }
    }

    private class VirtualBase : Captured
    {
        protected VirtualBase(ConstructionContext context, ConstructionTrace trace) : base(context, trace) => Observe();
        protected virtual void Observe() => ConstructionTrace.Fact("virtual_base", "unexpected");
    }

    private sealed class VirtualLeaf : VirtualBase
    {
        private readonly int _value;

        private VirtualLeaf(ConstructionContext context, ConstructionTrace trace) : base(context, trace)
        {
            _value = 77;
            trace.Number("virtual_after", _value);
        }

        internal static VirtualLeaf New(ConstructionTrace trace) => ConstructionContext.Create(context => new VirtualLeaf(context, trace));
        protected override void Observe() => ConstructionTrace.Number("virtual_derived_value", _value);
    }

    private class PlainBase : Captured
, IDisposable
    {
        internal int Value { get; private set; }

        protected PlainBase(ConstructionContext context, ConstructionTrace trace, bool fail) : base(context, trace) => Initialize(fail);

        // A Delphi constructor invoked on an existing object remains an ordinary method.
        protected void Initialize(bool fail)
        {
            Value++;
            ConstructionTrace.Number("plain_body", Value);
            if (fail) throw new Construct("plain");
        }

        public virtual void Dispose()
        {
            ConstructionTrace.Number("plain_destroy", Value);
        }
    }

    private sealed class PlainLeaf : PlainBase
    {
        private PlainLeaf(ConstructionContext context, ConstructionTrace trace) : base(context, trace, false) { }

        internal static PlainLeaf New(ConstructionTrace trace) => ConstructionContext.Create(context => new PlainLeaf(context, trace));
        internal void Reenter() => Initialize(true);
    }

    [Fact]
    public void RealConstructorsMatchEveryRecordedDelphiFact()
    {
        var trace = new ConstructionTrace();
        for (int failAt = 1; failAt <= 3; failAt++)
        {
            trace.Number("chain_case", failAt);
            Leaf? objectValue = null;
            try { objectValue = Leaf.New(trace, failAt); }
            catch (Construct e) { trace.Fact("chain_exception", e.Message); }
            trace.Number("chain_result_nil", objectValue is null ? 1 : 0);
        }
        var owner = new List<object>();
        Owned? owned = null;
        try { owned = Owned.New(trace, owner); }
        catch (Construct e) { trace.Fact("owner_exception", e.Message); }
        trace.Number("owner_after", owner.Count);
        trace.Number("owner_result_nil", owned is null ? 1 : 0);
        Parent? parent = null;
        try { parent = Parent.New(trace); }
        catch (Construct e) { trace.Fact("parent_exception", e.Message); }
        trace.Number("parent_result_nil", parent is null ? 1 : 0);
        Throwing? throwing = null;
        try { throwing = Throwing.New(trace); }
        catch (Cleanup e)
        {
            trace.Fact("throwing_exception_class", e.GetType().Name);
            trace.Fact("throwing_exception", e.Message);
        }
        trace.Number("throwing_result_nil", throwing is null ? 1 : 0);
        _ = VirtualLeaf.New(trace);
        var plain = PlainLeaf.New(trace);
        try
        {
            try { plain.Reenter(); }
            catch (Construct e) { trace.Fact("plain_exception", e.Message); }
            trace.Number("plain_after", plain.Value);
        }
        finally { plain?.Dispose(); }
        Assert.Equal(62, trace.Facts.Count);
        trace.Facts.Add($"end {trace.Facts.Count}");
        Assert.Equal(Oracle.Split('\n'), trace.Facts);
        Assert.Equal(9, trace.Contexts.Count);
        Assert.Equal(trace.Contexts.Count, trace.Contexts.Distinct().Count());
    }

    private sealed class Counted(Action destroy) : IDisposable
    {
        public void Dispose() => destroy();
    }

    [Fact]
    public void SuccessClosesContextWithoutDestroyingTheObject()
    {
        int destroys = 0;
        ConstructionContext? saved = null;
        var objectValue = ConstructionContext.Create(context =>
        {
            saved = context;
            var value = new Counted(() => destroys++);
            context.Capture(value);
            return value;
        });
        saved!.Fail();
        Assert.Equal(0, destroys);
        Assert.Throws<InvalidOperationException>(() => saved.Capture(objectValue));
        ((objectValue ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
        Assert.Equal(1, destroys);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CleanupClosesBeforeReentrantOrThrowingDestruction(bool throwInDestroy)
    {
        int destroys = 0;
        ConstructionContext? saved = null;
        var error = Assert.ThrowsAny<System.Exception>(() => ConstructionContext.Create<Counted>(context =>
        {
            saved = context;
            var value = new Counted(() =>
            {
                destroys++;
                context.Fail();
                if (throwInDestroy) throw new Cleanup("replacement");
            });
            context.Capture(value);
            throw new Construct("original");
        }));
        if (throwInDestroy) Assert.IsType<Cleanup>(error);
        else Assert.IsType<Construct>(error);
        saved!.Fail();
        Assert.Equal(1, destroys);
    }

    [Fact]
    public void FailureBeforeCapturePreservesTheOriginalException()
    {
        var original = new Construct("before capture");
        Assert.Same(original, Assert.Throws<Construct>(() => ConstructionContext.Create<object>(_ => throw original)));
    }

    [Fact]
    public void ContextRejectsASecondCapture()
    {
        int destroys = 0;
        var context = new ConstructionContext();
        context.Capture(new Counted(() => destroys++));
        Assert.Throws<InvalidOperationException>(() => context.Capture(new object()));
        context.Fail();
        context.Fail();
        Assert.Equal(1, destroys);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FinishedContextReleasesTheCapturedObject(bool fail)
    {
        var (context, weak) = FinishAndRelease(fail);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(weak.TryGetTarget(out _));
        GC.KeepAlive(context);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (ConstructionContext, WeakReference<object>) FinishAndRelease(bool fail)
    {
        var context = new ConstructionContext();
        var value = new object();
        var weak = new WeakReference<object>(value);
        context.Capture(value);
        if (fail) context.Fail();
        else context.Complete();
        return (context, weak);
    }

    // tests/probes/expected/ctor-failure.txt, recorded by Delphi 3 for brief 9.3b-1.
    private const string Oracle = """
chain_case 1
base_body 10
base_unwind done
leaf_destroy 0
middle_destroy 0
base_destroy 10
chain_exception base
chain_result_nil 1
chain_case 2
base_body 10
base_unwind done
middle_body 20
middle_unwind done
leaf_destroy 0
middle_destroy 20
base_destroy 10
chain_exception middle
chain_result_nil 1
chain_case 3
base_body 10
base_unwind done
middle_body 20
middle_unwind done
leaf_body 30
leaf_unwind done
leaf_destroy 30
middle_destroy 20
base_destroy 10
chain_exception leaf
chain_result_nil 1
owner_inserted 1
owner_at_destroy 1
owner_removed 0
owner_exception owner
owner_after 0
owner_result_nil 1
parent_body enter
base_body 10
base_unwind done
middle_body 20
middle_unwind done
leaf_destroy 0
middle_destroy 20
base_destroy 10
parent_unwind_child_nil 1
parent_destroy_child_nil 1
parent_exception middle
parent_result_nil 1
base_body 10
base_unwind done
throwing_destroy enter
base_destroy 10
throwing_exception_class Cleanup
throwing_exception replacement
throwing_result_nil 1
virtual_derived_value 0
virtual_after 77
plain_body 1
plain_body 2
plain_exception plain
plain_after 2
plain_destroy 2
end 62
""";
}
