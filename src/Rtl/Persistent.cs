using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace Aow.Rtl;

public class TPersistent : TObject
{
    // CLASSES.PAS TPersistent.Assign, AssignTo and AssignError.
    public virtual void Assign(TPersistent? source) { if (source is null) AssignError(null); else source.AssignTo(this); }
    protected internal virtual void AssignTo(TPersistent target) => target.AssignError(this);
    protected void AssignError(TPersistent? source) => throw new EConvertError($"Cannot assign a {(source is null ? "nil" : ClassNameOf(source.GetType()))} to a {ClassNameOf(GetType())}");
    // CLASSES.PAS TPersistent.GetOwner and GetNamePath.
    public virtual TPersistent? GetOwner() => null;
    public virtual string GetNamePath()
    {
        string name = ClassNameOf(GetType());
        string owner = GetOwner()?.GetNamePath() ?? "";
        return owner.Length == 0 ? name : owner + "." + name;
    }
    // CLASSES.PAS TPersistent.DefineProperties.
    public virtual void DefineProperties(TFiler? filer) { }
}

public enum TOperation : byte { opInsert, opRemove }
public enum TComponentState : byte { csLoading, csReading, csWriting, csDestroying, csDesigning, csAncestor, csUpdating, csFixups }
public delegate void TNotifyEvent(TObject sender);
public delegate void TGetChildProc(TComponent child);

public class TComponent : TPersistent
{
    private readonly List<TComponent> children = [];
    private List<TComponent>? freeNotifies;
    private string name = "";
    public TComponent(TComponent? owner = null) => owner?.InsertComponent(this);

    public TComponent? Owner { get; private set; }
    public int Tag { get; set; }
    // The layout session sets the loading flags from outside the component.
    public TSet<TComponentState> ComponentState { get; protected internal set; }
    public int ComponentCount => children.Count;
    // CLASSES.PAS TComponent.GetComponent uses TList's range error.
    public IndexedProperty<int, TComponent> Components => new(i =>
    {
        if (i < 0 || i >= children.Count) TList.Error("List index out of bounds (%d)", i);
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
                throw new EComponentError($"''{value}'' is not a valid component name");
            Owner?.ValidateRename(this, name, value);
            name = value;
        }
    }
    protected virtual void ValidateRename(TComponent component, string oldName, string newName)
    {
        if (Rtl.CompareText(oldName, newName) != 0 && FindComponent(newName) is not null) throw new EComponentError($"A component named {newName} already exists");
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
    public void InsertComponent(TComponent component)
    {
        component.ValidateContainer(this); ValidateRename(component, "", component.name);
        children.Add(component); component.Owner = this; Notification(component, TOperation.opInsert);
    }
    protected virtual void ValidateContainer(TComponent container) => container.ValidateInsert(this);
    protected virtual void ValidateInsert(TComponent component) { }
    // CLASSES.PAS TComponent.RemoveComponent and Remove.
    public void RemoveComponent(TComponent component) { Notification(component, TOperation.opRemove); component.Owner = null; children.Remove(component); ValidateRename(component, component.name, ""); }
    // CLASSES.PAS TComponent.FindComponent.
    public TComponent? FindComponent(string value) => value.Length == 0 ? null : children.FirstOrDefault(c => Rtl.CompareText(c.name, value) == 0);
    // CLASSES.PAS TComponent.FreeNotification establishes reciprocal notifications.
    public void FreeNotification(TComponent component)
    {
        if (Owner is not null && component.Owner == Owner) return;
        freeNotifies ??= [];
        if (!freeNotifies.Contains(component)) { freeNotifies.Add(component); component.FreeNotification(this); }
    }
    // CLASSES.PAS TComponent.Notification.
    public virtual void Notification(TComponent component, TOperation operation)
    {
        if (operation == TOperation.opRemove && freeNotifies is not null) { freeNotifies.Remove(component); if (freeNotifies.Count == 0) freeNotifies = null; }
        for (int i = 0; i < children.Count; i++) children[i].Notification(component, operation);
    }
    // CLASSES.PAS TComponent.Destroying marks the complete ownership tree first.
    public void Destroying()
    {
        if (ComponentState.Contains(TComponentState.csDestroying)) return;
        ComponentState = ComponentState.With(TComponentState.csDestroying);
        foreach (var child in children) child.Destroying();
    }
    // CLASSES.PAS TComponent.DestroyComponents detaches before calling the destructor.
    public void DestroyComponents()
    {
        while (children.Count != 0)
        {
            var child = children[^1]; child.Owner = null; children.RemoveAt(children.Count - 1); child.Destroy();
        }
    }
    // CLASSES.PAS TComponent.Destroy delivers free notifications before Destroying.
    public override void Destroy()
    {
        if (freeNotifies is not null)
        {
            for (int i = 0; freeNotifies is not null && i < freeNotifies.Count; i++) freeNotifies[i].Notification(this, TOperation.opRemove);
            freeNotifies = null;
        }
        Destroying(); DestroyComponents(); Owner?.RemoveComponent(this); base.Destroy();
    }
    // CLASSES.PAS TComponent.Loaded, GetNamePath, GetOwner and parent hooks.
    public virtual void Loaded() => ComponentState = ComponentState.Without(TComponentState.csLoading);
    public override string GetNamePath() => Name;
    public override TPersistent? GetOwner() => Owner;
    public virtual void GetChildren(TGetChildProc action, TComponent root) { }
    public virtual TComponent? GetParentComponent() => null;
    public virtual bool HasParent() => false;
    public virtual void SetChildOrder(TComponent child, int order) { }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class PublishedAttribute : Attribute
{
    public PublishedAttribute() : base() { }
    public PublishedAttribute(object? defaultValue) => DefaultValue = defaultValue;
    public object? DefaultValue { get; }
}

public sealed record PublishedProperty(string Name, Type Type, object? DefaultValue, Action<object, object?> Setter);

public static class Rtti
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<PublishedProperty>> cache = new();
    public static IReadOnlyList<PublishedProperty> PublishedProperties(Type type) => cache.GetOrAdd(type, t => Array.AsReadOnly(t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(p => p.GetCustomAttribute<PublishedAttribute>() is not null && p.SetMethod is not null && p.GetIndexParameters().Length == 0)
        .OrderBy(p => p.Name, StringComparer.Ordinal)
        .Select(p => new PublishedProperty(p.Name, p.PropertyType, p.GetCustomAttribute<DefaultValueAttribute>()?.Value ?? p.GetCustomAttribute<PublishedAttribute>()!.DefaultValue, (o, v) => p.SetValue(o, v))).ToArray()));
}
