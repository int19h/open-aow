using System.ComponentModel;
using static Aow.Rtl.Rtl;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class ClassesTests
{
    private sealed class Identical : TObject
    {
        public override bool Equals(object? other) => other is Identical;
        public override int GetHashCode() => 0;
    }

    [Fact]
    public void ListPropertiesAndErrorsFollowSource()
    {
        // CLASSES.PAS:1435..1619, including pointer identity and Count's zero fill.
        var first = new Identical(); var equal = new Identical();
        var l = new TList();
        Assert.Throws<EListError>(() => l.First()); Assert.Throws<EListError>(() => l.Last());
        Assert.Throws<EListError>(() => l[-1]); Assert.Throws<EListError>(() => l[0] = first);
        Assert.Throws<EListError>(() => l.Count = -1); Assert.Throws<EListError>(() => l.Capacity = -1);
        Assert.Throws<EListError>(() => l.Count = 134217728); Assert.Throws<EListError>(() => l.Capacity = 134217728);
        l.Add(first); Assert.Equal(-1, l.IndexOf(equal)); Assert.Same(first, l.List[0]);
        Assert.Throws<EListError>(() => l.Capacity = 0);
        l[0] = equal; Assert.Same(equal, l[0]);
        l.Count = 3; Assert.Equal(4, l.Capacity); Assert.Null(l[1]); Assert.Null(l[2]);
        l.Count = 1; l.Count = 5; Assert.Equal(5, l.Capacity); Assert.All(l.List.Skip(1), Assert.Null);
        Assert.Throws<EListError>(() => l.Exchange(0, 5)); Assert.Throws<EListError>(() => l.Exchange(-1, 0));
        Assert.Throws<EListError>(() => l.Move(0, 5)); Assert.Throws<EListError>(() => l.Move(-1, 0));
        l.Move(-1, -1);
        Assert.Same(l, l.Expand()); Assert.Equal(9, l.Capacity);
        Assert.Equal((AnsiString)"problem 7", Assert.Throws<EListError>(() => TList.Error("problem %d", 7)).Message);
        l.Destroy(); Assert.Equal(0, l.Count); Assert.Empty(l.List);
    }

    private sealed class Assigned : TPersistent
    {
        internal TPersistent? Target;
        protected internal override void AssignTo(TPersistent target) => Target = target;
    }
    private sealed class Owned(TPersistent owner) : TPersistent
    {
        public override TPersistent GetOwner() => owner;
    }

    [Fact]
    public void PersistentAssignDispatchAndNamePaths()
    {
        // CLASSES.PAS:1817..1857.
        var target = new TPersistent(); var source = new Assigned();
        target.Assign(source); Assert.Same(target, source.Target);
        Assert.Equal((AnsiString)"Cannot assign a nil to a TPersistent", Assert.Throws<EConvertError>(() => target.Assign(null)).Message);
        Assert.Equal((AnsiString)"Cannot assign a TPersistent to a TPersistent", Assert.Throws<EConvertError>(() => target.Assign(new TPersistent())).Message);
        Assert.Null(target.GetOwner()); Assert.Equal((AnsiString)"TPersistent", target.GetNamePath());
        Assert.Equal((AnsiString)"TPersistent.Owned", new Owned(target).GetNamePath());
        Assert.Equal((AnsiString)"Owned", new Owned(new TComponent()).GetNamePath());
        target.DefineProperties(null); target.Destroy();
    }

    private sealed class Watched(List<string> log) : TComponent
    {
        public override void Notification(TComponent component, TOperation operation)
        {
            log.Add($"notify {Name} {component.Name} {operation} destroying={component.ComponentState.Contains(TComponentState.csDestroying)}");
            base.Notification(component, operation);
        }
        public override void Destroy()
        {
            log.Add($"destroy {Name} owner={Owner?.Name} state={ComponentState.Contains(TComponentState.csDestroying)}");
            base.Destroy();
        }
        internal void Loading() => ComponentState = ComponentState.With(TComponentState.csLoading);
    }

    [Fact]
    public void ComponentsValidateNamesAndExposeSourceParentHooks()
    {
        // CLASSES.PAS:6170..6329. Parent and child hooks are empty in TComponent itself.
        var owner = new TComponent { Name = "Root", Tag = 42 };
        var first = new TComponent(owner) { Name = "A_1" };
        var second = new TComponent(owner) { Name = "B" };
        Assert.Same(owner, first.Owner); Assert.Same(owner, first.GetOwner());
        Assert.Equal(2, owner.ComponentCount); Assert.Same(first, owner.Components[0]);
        Assert.Equal(42, owner.Tag); Assert.Equal((AnsiString)"A_1", first.GetNamePath());
        Assert.Same(first, owner.FindComponent("a_1")); Assert.Null(owner.FindComponent(default)); Assert.Null(owner.FindComponent("missing"));
        Assert.Throws<EComponentError>(() => second.Name = "a_1");
        foreach (AnsiString name in new AnsiString[] { "1abc", "bad-name", "a b", "\xe0", "x\0y" }) Assert.Throws<EComponentError>(() => second.Name = name);
        first.Name = "a_1"; first.Name = "a_1";
        first.ComponentIndex = 99; Assert.Same(first, owner.Components[1]);
        first.ComponentIndex = -1; Assert.Same(first, owner.Components[0]); first.ComponentIndex = 0;
        Assert.Throws<EListError>(() => owner.Components[2]); Assert.Throws<EListError>(() => new TComponent().Components[0]);
        Assert.False(first.HasParent()); Assert.Null(first.GetParentComponent());
        int calls = 0; owner.GetChildren(_ => calls++, owner); Assert.Equal(0, calls);
        owner.SetChildOrder(first, 1); Assert.Equal(0, first.ComponentIndex);
        owner.DefineProperties(null); owner.Loaded();
        owner.RemoveComponent(first); Assert.Null(first.Owner); Assert.Equal(-1, first.ComponentIndex); first.ComponentIndex = 0;
        owner.InsertComponent(first); Assert.Equal(1, first.ComponentIndex);
        var duplicate = new TComponent { Name = "B" }; Assert.Throws<EComponentError>(() => owner.InsertComponent(duplicate)); Assert.Null(duplicate.Owner);
        owner.DestroyComponents(); Assert.Equal(0, owner.ComponentCount); Assert.Null(first.Owner); Assert.Null(second.Owner);
        owner.Destroy(); duplicate.Destroy();
    }

    [Fact]
    public void ComponentDestructionNotifiesThenMarksThenDestroysLastChildFirst()
    {
        // CLASSES.PAS:6024..6052 and :6112..6154.
        var log = new List<string>();
        var root = new Watched(log) { Name = "root" }; var a = new Watched(log) { Name = "a" }; var b = new Watched(log) { Name = "b" }; var observer = new Watched(log) { Name = "observer" };
        root.InsertComponent(a); root.InsertComponent(b); root.FreeNotification(observer); root.FreeNotification(observer); log.Clear();
        root.Destroy();
        Assert.Equal(new[] { "destroy root owner= state=False", "notify observer root opRemove destroying=False", "destroy b owner= state=True", "destroy a owner= state=True" }, log);
        Assert.True(root.ComponentState.Contains(TComponentState.csDestroying)); Assert.True(a.ComponentState.Contains(TComponentState.csDestroying));
        Assert.False(root.ComponentState.Contains(TComponentState.csDesigning));
        log.Clear(); observer.Destroy(); Assert.Single(log);
        var loading = new Watched(log); loading.Loading(); Assert.True(loading.ComponentState.Contains(TComponentState.csLoading)); loading.Loaded(); Assert.False(loading.ComponentState.Contains(TComponentState.csLoading));
        var owner = new TComponent(); a = new Watched(log) { Name = "a" }; b = new Watched(log) { Name = "b" }; owner.InsertComponent(a); owner.InsertComponent(b); a.FreeNotification(b); log.Clear();
        a.Destroy(); Assert.Equal(1, owner.ComponentCount); Assert.Contains(log, line => line.StartsWith("notify b a opRemove", StringComparison.Ordinal)); owner.Destroy();
        var tree = new TComponent(); var sub = new TComponent(tree); var leaf = new TComponent(sub); tree.Destroying(); tree.Destroying();
        Assert.True(leaf.ComponentState.Contains(TComponentState.csDestroying)); tree.Destroy();
    }

    private sealed class CreatedComponent : TComponent
    {
        internal static CreatedComponent? Last;
        internal bool Destroyed;
        internal bool Fail;
        public override void Create__init(TComponent? owner)
        {
            Last = this; base.Create__init(owner); if (Fail) throw new EInvalidOperation("constructor failed");
        }
        public override void Destroy() { Destroyed = true; base.Destroy(); }
    }

    [Fact]
    public void ComponentMetaclassRunsVirtualInitializerAndCleansUpFailures()
    {
        MetaRegistry.Register(typeof(CreatedComponent), new TComponent.Meta(), () => new CreatedComponent(), 0);
        var meta = TComponent.Meta.Of<CreatedComponent>(); var owner = new TComponent();
        var instance = (CreatedComponent)meta.Create(owner); Assert.Same(owner, instance.Owner); Assert.Same(meta, instance.ClassType); instance.Destroy(); Assert.True(instance.Destroyed);
        var failing = new FailureMeta();
        Assert.Throws<EInvalidOperation>(() => failing.Create(owner)); Assert.True(CreatedComponent.Last!.Destroyed); Assert.Equal(0, owner.ComponentCount); owner.Destroy();
    }
    private sealed class FailureMeta : TComponent.Meta
    {
        public override TObject NewInstance() => new CreatedComponent { Fail = true };
    }

    private class PublishedBase : TComponent
    {
        [Published, DefaultValue(7)] public int Amount { get; set; }
        [Published] public int ReadOnly => 1;
        public int Hidden { get; set; }
    }
    private sealed class PublishedChild : PublishedBase
    {
        [Published("base")] public string Caption { get; set; } = "";
        [Published] public int this[int index] { get => index; set { } }
    }

    [Fact]
    public void PublishedPropertiesAreCachedInheritedAndWritable()
    {
        var properties = Rtti.PublishedProperties(typeof(PublishedChild));
        Assert.Same(properties, Rtti.PublishedProperties(typeof(PublishedChild)));
        Assert.Equal(new[] { "Amount", "Caption" }, properties.Select(p => p.Name));
        var obj = new PublishedChild(); properties[0].Setter(obj, 19); properties[1].Setter(obj, "hello");
        Assert.Equal(19, obj.Amount); Assert.Equal("hello", obj.Caption);
        Assert.Equal(typeof(int), properties[0].Type); Assert.Equal(7, properties[0].DefaultValue); Assert.Equal("base", properties[1].DefaultValue);
        Assert.Null(new PublishedAttribute().DefaultValue);
        Assert.Empty(Rtti.PublishedProperties(typeof(TObject)));
    }
}
