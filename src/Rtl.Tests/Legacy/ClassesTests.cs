
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class ClassesTests
{
    private sealed class Identical
    {
        public override bool Equals(object? other) => other is Identical;
        public override int GetHashCode() => 0;
    }

    [Fact]
    public void ListPropertiesAndErrorsFollowSource()
    {
        // CLASSES.PAS:1435..1619, including pointer identity and Count's zero fill.
        Identical first = new(); Identical equal = new();
        DelphiList<Identical> l = new();
        Assert.Throws<ArgumentOutOfRangeException>(() => l.First()); Assert.Throws<ArgumentOutOfRangeException>(() => l.Last());
        Assert.Throws<ArgumentOutOfRangeException>(() => l[-1]); Assert.Throws<ArgumentOutOfRangeException>(() => l[0] = first);
        Assert.Throws<ArgumentOutOfRangeException>(() => l.Count = -1); Assert.Throws<ArgumentOutOfRangeException>(() => l.Capacity = -1);
        Assert.Throws<ArgumentOutOfRangeException>(() => l.Count = 134217728); Assert.Throws<ArgumentOutOfRangeException>(() => l.Capacity = 134217728);
        l.Add(first); Assert.Equal(-1, l.IndexOf(equal)); Assert.Same(first, l.List[0]);
        Assert.Throws<ArgumentOutOfRangeException>(() => l.Capacity = 0);
        l[0] = equal; Assert.Same(equal, l[0]);
        l.Count = 3; Assert.Equal(4, l.Capacity); Assert.Null(l[1]); Assert.Null(l[2]);
        l.Count = 1; l.Count = 5; Assert.Equal(5, l.Capacity); Assert.All(l.List.Skip(1), Assert.Null);
        Assert.Throws<ArgumentOutOfRangeException>(() => l.Exchange(0, 5)); Assert.Throws<ArgumentOutOfRangeException>(() => l.Exchange(-1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => l.Move(0, 5)); Assert.Throws<ArgumentOutOfRangeException>(() => l.Move(-1, 0));
        l.Move(-1, -1);
        Assert.Same(l, l.Expand()); Assert.Equal(9, l.Capacity);
        Assert.Equal("problem 7", Assert.Throws<ArgumentOutOfRangeException>(() => DelphiListCore.Error("problem %d", 7)).Message);
        l.Dispose(); Assert.Equal(0, l.Count); Assert.Empty(l.List);
    }

    private sealed class Assigned : Persistent
    {
        internal Persistent? Target { get; private set; }

        protected internal override void AssignTo(Persistent target) => Target = target;
    }

    private sealed class Owned(Persistent owner) : Persistent
    {
        public override Persistent GetOwner() => owner;
    }

    [Fact]
    public void PersistentAssignDispatchAndNamePaths()
    {
        using         // CLASSES.PAS:1817..1857.
                Persistent target = new(); Assigned source = new();
        target.Assign(source); Assert.Same(target, source.Target);
        Assert.Equal("Cannot assign a nil to a Persistent", Assert.Throws<FormatException>(() => target.Assign(null)).Message);
        Assert.Equal("Cannot assign a Persistent to a Persistent", Assert.Throws<FormatException>(() => target.Assign(new Persistent())).Message);
        Assert.Null(target.GetOwner()); Assert.Equal("Persistent", target.GetNamePath());
        Assert.Equal("Persistent.Owned", new Owned(target).GetNamePath());
        Assert.Equal("Owned", new Owned(new Component(null)).GetNamePath());
        target.DefineProperties(null);
    }

    private sealed class Watched(List<string> log) : Component
    {
        public override void Notification(Component component, Operation operation)
        {
            log.Add($"notify {Name} {component.Name} {operation} destroying={component.ComponentState.Contains(Rtl.ComponentState.Destroying)}");
            base.Notification(component, operation);
        }

        public override void Dispose()
        {
            log.Add($"destroy {Name} owner={Owner?.Name} state={ComponentState.Contains(Rtl.ComponentState.Destroying)}");
            base.Dispose();
        }

        internal void Loading() => ComponentState = ComponentState.With(Rtl.ComponentState.Loading);
    }

    [Fact]
    public void ComponentsValidateNamesAndExposeSourceParentHooks()
    {
        // CLASSES.PAS:6170..6329. Parent and child hooks are empty in TComponent itself.
        Component owner = new(null) { Name = "Root", Tag = 42 };
        Component first = new(owner) { Name = "A_1" };
        Component second = new(owner) { Name = "B" };
        Assert.Same(owner, first.Owner); Assert.Same(owner, first.GetOwner());
        Assert.Equal(2, owner.ComponentCount); Assert.Same(first, owner.Components[0]);
        Assert.Equal(42, owner.Tag); Assert.Equal("A_1", first.GetNamePath());
        Assert.Same(first, owner.FindComponent("a_1")); Assert.Null(owner.FindComponent("")); Assert.Null(owner.FindComponent("missing"));
        Assert.Throws<ArgumentException>(() => second.Name = "a_1");
        foreach (string name in new string[] { "1abc", "bad-name", "a b", "\xe0", "x\0y" })
        {
            Assert.Throws<ArgumentException>(() => second.Name = name);
        }

        first.Name = "a_1"; first.Name = "a_1";
        first.ComponentIndex = 99; Assert.Same(first, owner.Components[1]);
        first.ComponentIndex = -1; Assert.Same(first, owner.Components[0]); first.ComponentIndex = 0;
        Assert.Throws<ArgumentOutOfRangeException>(() => owner.Components[2]); Assert.Throws<ArgumentOutOfRangeException>(() => new Component(null).Components[0]);
        Assert.False(first.HasParent()); Assert.Null(first.GetParentComponent());
        int calls = 0; owner.GetChildren(_ => calls++, owner); Assert.Equal(0, calls);
        owner.SetChildOrder(first, 1); Assert.Equal(0, first.ComponentIndex);
        owner.DefineProperties(null); owner.Loaded();
        owner.RemoveComponent(first); Assert.Null(first.Owner); Assert.Equal(-1, first.ComponentIndex); first.ComponentIndex = 0;
        owner.InsertComponent(first); Assert.Equal(1, first.ComponentIndex);
        using Component duplicate = new(null) { Name = "B" }; Assert.Throws<ArgumentException>(() => owner.InsertComponent(duplicate)); Assert.Null(duplicate.Owner);
        owner.DestroyComponents(); Assert.Equal(0, owner.ComponentCount); Assert.Null(first.Owner); Assert.Null(second.Owner);
        owner.Dispose();
    }

    [Fact]
    public void ComponentDestructionNotifiesThenMarksThenDestroysLastChildFirst()
    {
        // CLASSES.PAS:6024..6052 and :6112..6154.
        List<string> log = [];
        Watched root = new(log) { Name = "root" }; Watched a = new(log) { Name = "a" }; Watched b = new(log) { Name = "b" }; Watched observer = new(log) { Name = "observer" };
        root.InsertComponent(a); root.InsertComponent(b); root.FreeNotification(observer); root.FreeNotification(observer); log.Clear();
        root.Dispose();
        Assert.Equal(new[] { "destroy root owner= state=False", "notify observer root Remove destroying=False", "destroy b owner= state=True", "destroy a owner= state=True" }, log);
        Assert.True(root.ComponentState.Contains(ComponentState.Destroying)); Assert.True(a.ComponentState.Contains(ComponentState.Destroying));
        Assert.False(root.ComponentState.Contains(ComponentState.Designing));
        log.Clear(); observer.Dispose(); Assert.Single(log);
        Watched loading = new(log); loading.Loading(); Assert.True(loading.ComponentState.Contains(ComponentState.Loading)); loading.Loaded(); Assert.False(loading.ComponentState.Contains(ComponentState.Loading));
        Component owner = new(null); a = new Watched(log) { Name = "a" }; b = new Watched(log) { Name = "b" }; owner.InsertComponent(a); owner.InsertComponent(b); a.FreeNotification(b); log.Clear();
        a.Dispose(); Assert.Equal(1, owner.ComponentCount); Assert.Contains(log, line => line.StartsWith("notify b a Remove", StringComparison.Ordinal)); owner.Dispose();
        using Component tree = new(null); Component leaf = new(new Component(tree)); tree.Destroying(); tree.Destroying();
        Assert.True(leaf.ComponentState.Contains(ComponentState.Destroying));
    }

    [Fact]
    public void ComponentConstructorInsertsAndDestroyRemovesOwnerMembership()
    {
        using Component owner = new(null);
        Component instance = new(owner);
        Assert.Same(owner, instance.Owner);
        Assert.Equal(1, owner.ComponentCount);
        Assert.Same(instance, owner.Components[0]);
        instance.Dispose();
        Assert.Null(instance.Owner);
        Assert.Equal(0, owner.ComponentCount);
    }

    [Fact]
    public void PublishedAttributeStoresItsDefault()
    {
        Assert.Null(new PublishedAttribute().DefaultValue);
        Assert.Equal("base", new PublishedAttribute("base").DefaultValue);
    }
}
