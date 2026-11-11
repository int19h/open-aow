using System.Collections.Concurrent;
using System.Reflection;

namespace Aow.Rtl;

public class Persistent : IDisposable
{
    // CLASSES.PAS TPersistent.Assign, AssignTo and AssignError.
    public virtual void Assign(Persistent? source) { if (source is null) AssignError(null); else source.AssignTo(this); }

    protected internal virtual void AssignTo(Persistent target) => target.AssignError(this);
    protected void AssignError(Persistent? source) => throw new FormatException($"Cannot assign a {(source is null ? "nil" : source.GetType().Name)} to a {GetType().Name}");
    // CLASSES.PAS TPersistent.GetOwner and GetNamePath.
    public virtual Persistent? GetOwner() => null;

    public virtual string GetNamePath()
    {
        string name = GetType().Name;
        string owner = GetOwner()?.GetNamePath() ?? "";
        return owner.Length == 0 ? name : owner + "." + name;
    }

    // CLASSES.PAS TPersistent.DefineProperties.
    public virtual void DefineProperties(Filer? filer) { }

    public virtual void Dispose()
    {
    }
}

public enum ComponentState : byte { Loading, Reading, Writing, Destroying, Designing, Ancestor, Updating, Fixups }

public delegate void NotifyEvent<in TSender>(TSender sender);

public class Component : Persistent
{
    private readonly List<Component> children = [];
    private List<Component>? freeNotifies;
    private string name = "";

    public Component(Component? owner = null) => owner?.InsertComponent(this);

    public Component? Owner { get; private set; }
    public int Tag { get; set; }
    // The layout session sets the loading flags from outside the component.
    public Set<ComponentState> ComponentState { get; protected internal set; }
    public int ComponentCount => children.Count;

    // CLASSES.PAS TComponent.GetComponent uses TList's range error.
    public IndexedProperty<int, Component> Components => new(i =>
    {
        if (i < 0 || i >= children.Count) DelphiList.Error("List index out of bounds (%d)", i);
        return children[i];
    });

    // CLASSES.PAS TComponent.SetName and ValidateRename.
    public string Name
    {
        get => name;
        set
        {
            if (name == value) return;
            static bool Letter(byte c) => c is >= 65 and <= 90 or >= 97 and <= 122 or 95;
            if (value.Length > 0 && (!Letter(Cp1252.Byte(value[0])) || Cp1252.Encode(value).Skip(1).Any(c => !Letter(c) && c is not (>= 48 and <= 57))))
                throw new ArgumentException($"''{value}'' is not a valid component name");
            Owner?.ValidateRename(this, name, value);
            name = value;
        }
    }

    protected virtual void ValidateRename(Component component, string oldName, string newName)
    {
        if (RtlUnit.CompareText(oldName, newName) != 0 && FindComponent(newName) is not null) throw new ArgumentException($"A component named {newName} already exists");
    }

    // CLASSES.PAS TComponent.GetComponentIndex and SetComponentIndex.
    public int ComponentIndex
    {
        get => Owner?.children.IndexOf(this) ?? -1;
        set
        {
            if (Owner is null) return;
            int i = ComponentIndex, target = Math.Clamp(value, 0, Owner.children.Count - 1);
            if (i >= 0 && i != target) { Owner.children.RemoveAt(i); Owner.children.Insert(target, this); }
        }
    }

    // CLASSES.PAS TComponent.InsertComponent.
    public void InsertComponent(Component component)
    {
        component.ValidateContainer(this); ValidateRename(component, "", component.name);
        children.Add(component); component.Owner = this; Notification(component, Operation.Insert);
    }

    protected virtual void ValidateContainer(Component container) => container.ValidateInsert(this);

    protected virtual void ValidateInsert(Component component) { }

    // CLASSES.PAS TComponent.RemoveComponent and Remove.
    public void RemoveComponent(Component component) { Notification(component, Operation.Remove); component.Owner = null; children.Remove(component); ValidateRename(component, component.name, ""); }

    // CLASSES.PAS TComponent.FindComponent.
    public Component? FindComponent(string value) => value.Length == 0 ? null : children.FirstOrDefault(c => RtlUnit.CompareText(c.name, value) == 0);

    // CLASSES.PAS TComponent.FreeNotification establishes reciprocal notifications.
    public void FreeNotification(Component component)
    {
        if (Owner is not null && component.Owner == Owner) return;
        freeNotifies ??= [];
        if (!freeNotifies.Contains(component)) { freeNotifies.Add(component); component.FreeNotification(this); }
    }

    // CLASSES.PAS TComponent.Notification.
    public virtual void Notification(Component component, Operation operation)
    {
        if (operation == Operation.Remove && freeNotifies is not null) { freeNotifies.Remove(component); if (freeNotifies.Count == 0) freeNotifies = null; }
        for (int i = 0; i < children.Count; i++) children[i].Notification(component, operation);
    }

    // CLASSES.PAS TComponent.Destroying marks the complete ownership tree first.
    public void Destroying()
    {
        if (ComponentState.Contains(Rtl.ComponentState.Destroying)) return;
        ComponentState = ComponentState.With(Rtl.ComponentState.Destroying);
        foreach (var child in children) child.Destroying();
    }

    // CLASSES.PAS TComponent.DestroyComponents detaches before calling the destructor.
    public void DestroyComponents()
    {
        while (children.Count != 0)
        {
            var child = children[^1]; child.Owner = null; children.RemoveAt(children.Count - 1); child.Dispose();
        }
    }

    // CLASSES.PAS TComponent.Destroy delivers free notifications before Destroying.
    public override void Dispose()
    {
        if (freeNotifies is not null)
        {
            for (int i = 0; freeNotifies is not null && i < freeNotifies.Count; i++) freeNotifies[i].Notification(this, Operation.Remove);
            freeNotifies = null;
        }
        Destroying(); DestroyComponents(); Owner?.RemoveComponent(this); base.Dispose();
    }

    // CLASSES.PAS TComponent.Loaded, GetNamePath, GetOwner and parent hooks.
    public virtual void Loaded() => ComponentState = ComponentState.Without(Rtl.ComponentState.Loading);
    public override string GetNamePath() => Name;
    public override Persistent? GetOwner() => Owner;

    public virtual void GetChildren(GetChildProc action, Component root) { }

    public virtual Component? GetParentComponent() => null;
    public virtual bool HasParent() => false;

    public virtual void SetChildOrder(Component child, int order) { }

    public enum Operation : byte { Insert, Remove }

    public delegate void GetChildProc(Component child);
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class PublishedAttribute : Attribute
{
    public PublishedAttribute() : base() { }

    public PublishedAttribute(object? defaultValue) => DefaultValue = defaultValue;

    public object? DefaultValue { get; }
}
