#nullable disable warnings
// Translated from Engine.pas.
using System.Diagnostics;
using System.Globalization;
using Aow.Formats.Tree;
using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// _owner
[DebuggerDisplay("{ToString(),nq}")]
public class EngineObject : IDisposable, IPersistable, IOwned
{
    // Engine.pas:2461
    public const int ClassId = 80;

    // The engine class descriptor of the instance's exact type, read once and cached.
    public EngineClass EngineClass => field ??= EngineClass.Of(GetType());

    public override string ToString() => Describe(() => string.Create(CultureInfo.InvariantCulture, handler: $"{GetType().Name} class={EngineClass.Of(GetType()).ClassId}"));

    protected string Describe(Func<string> describe)
    {
        try
        {
            return describe();
        }
        catch
        {
            return $"{GetType().Name} class=?";
        }
    }

    private protected EngineObject? ConstructionHolder { get; private set; }

    // The Delphi pre-inherited statements run after capture and before any inherited body. The
    // owner reaches the hook before the base body assigns it to this instance.
    private protected virtual void InitializeBeforeInherited(CustomNode? owner) { }

    public EngineObject(CustomNode? owner) : this(null, owner) { }

    private protected EngineObject(ConstructionContext? context, CustomNode? owner)
    {
        ConstructionHolder = context?.State as EngineObject;
        context?.Capture(this);
        InitializeBeforeInherited(owner);
        SetOwner(owner);
    }

    private CustomNode? _owner;

    // Engine.pas:2487. The walk reads each owner through IOwned.
    internal string GetEngineObjectPath()
    {
        string path = GetType().Name;
        for (IOwner? owner = ((IOwned)this).Owner; owner is not null; owner = (owner as IOwned)?.Owner)
        {
            path = owner.GetType().Name + path;
        }

        return path;
    }

    // Engine.pas:2442
    public virtual int ClassVersion() => 0;

    // Engine.pas:2563
    public virtual ControlStyles GetControlStyle() => EngineServices.Stored;

    // Engine.pas:2568
    public virtual void SetOwner(CustomNode? owner)
    {
        if (owner != _owner)
        {
            CheckOwner(owner);
            _ = (_owner?.RemoveChild(this));

            _owner = owner;
            _ = (_owner?.AddChild(this));
        }
    }

    // The node through which a node without an owner reaches its engine: the engine for what it
    // registers, creates, loads or copies, a node for the node its constructor keeps in a field.
    // A node reaches its engine through its owner, and without an owner through its holder.
    public EngineObject? Holder { get; set; }

    // The holder for a node this node creates without an owner: the engine once this node reaches
    // it, else this node, through which the created node reaches the engine later. The engine
    // itself keeps a holder walk short and stops it from looping through a node that comes to own
    // its creator (a troop creates the army sprite that then holds it).
    public EngineObject CreationHolder() => (EngineObject?)GetEngine() ?? this;

    // Engine.pas:2558. The original walks the owners only and reads the engine global where a node
    // has no owner.
    public virtual GameEngine? GetEngine() => _owner is not null ? _owner.GetEngine() : Holder?.GetEngine();

    // Engine.pas:2502
    public virtual int LastMessage
    {
        get => 2_147_483_647;
        // Engine.pas:2507
        set
        {
        }
    }

    // Engine.pas:2590
    // The stored fields. MainPersist is the stored image.
    public virtual void PersistFields(IPersister persister)
    {
    }

    void IPersistable.Persist(IPersister persister)
    {
        if (TreePersistenceRefusal(GetType()) is string reason)
        {
            throw new InvalidOperationException(reason);
        }

        MainPersist(persister);
    }

    /// <summary>
    /// Null when the class persists through the tree. Every engine class does (issue 185 PE-4-a).
    /// </summary>
    public static string? TreePersistenceRefusal(Type? type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return null;
    }

    // Engine.pas:2594
    public virtual void MainPersist(IPersister persister)
    {
        if (persister.Loading)
        {
            Clear();
        }

        PersistFields(persister);
    }

    // Engine.pas:2447
    public virtual int AddRef() => throw new NotSupportedException("Reference counting not supported");

    // Engine.pas:2452
    public virtual int Release() => throw new NotSupportedException("Reference counting not supported");

    // Engine.pas:2457
    public virtual void Clear()
    {
    }

    // Engine.pas:2511
    // The original compares the images WriteEObject writes. Two objects of one class whose trees
    // are equal write equal images (persistence-tree.md 13).
    public virtual bool Compare(EngineObject? objectValue, GameEngine? engine)
    {
        if (EngineClass.ClassId != objectValue.EngineClass.ClassId)
        {
            return false;
        }

        EnginePersistableFactory factory = new(engine);
        return TreeWriter.WriteObject(this, factory: factory).ValueEquals(TreeWriter.WriteObject(objectValue, factory: factory));
    }

    // Engine.pas:2466
    public virtual EditorClass Editor()
    {
        EditorClass? result = null;
        GameEngine? e = GetEngine();
        if (e is not null)
        {
            for (Type? type = GetType(); type != typeof(EngineObject) && result is null; type = type.BaseType)
            {
                result = e.Editors.FindEditor(EngineClass.Of(type).ClassId);
            }
        }

        result ??= EditorClass.Of<EngineObjectEditor>();

        return result;
    }

    // Engine.pas:2529
    // The original writes the source with WriteEObject and reads the image into this object with
    // ReadEObject. The source's tree loads into this object the same way: the load clears it
    // first, a member the source leaves out takes its fallback, and a failure goes to the load's
    // handler, of which a copy has none (persistence-tree.md 13).
    public virtual void Assign(EngineObject? source, GameEngine? engine)
    {
        EnginePersistableFactory factory = new(engine);
        TreeReader.ReadObject(TreeWriter.WriteObject(source, factory: factory), objectValue: this, factory: factory);
    }

    // Engine.pas:2585
    public virtual void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        if (message.MessageId == 1)
        {
            LastMessage = 2_147_483_647;
        }
    }

    // Engine.pas:2578
    public virtual void SetArrayOwner(CustomNode owner, int index)
    {
        CheckOwner(owner);
        _ = (_owner?.RemoveChild(this));

        _owner = owner;
        _owner?.SetChild(index, child: this);
    }

    // Engine.pas:2436
    public virtual void Dispose() => SetOwner(null);

    // Engine.pas:2543
    // A new object of this class loads the tree of this one, as Assign loads it.
    public EngineObject Copy(GameEngine? engine)
    {
        EnginePersistableFactory factory = new(engine);
        ObjectNode tree = TreeWriter.WriteObject(this, factory: factory);
        EngineObject result = EngineClass.CreateInstance(null, holder: engine);
        TreeReader.ReadObject(tree, objectValue: result, factory: factory);
        return result;
    }

    // A copy of this object, typed by the caller. Copy preserves the runtime
    // class, so the typed copy keeps the single cast.
    public T Copy<T>(GameEngine? engine) where T : EngineObject => (T)Copy(engine);

    public CustomNode? Owner { get => _owner; set => SetOwner(value); }

    // AcceptsOwner admits only T, so the cast never fails.
    protected T GetOwner<T>() where T : CustomNode => (T)_owner;

    IOwner? IOwned.Owner => _owner;

    protected internal void SetOwnerWithoutNotify(CustomNode? value)
    {
        CheckOwner(value);
        _owner = value;
    }

    /// <summary>
    /// Whether this object takes the owner. A class with a typed owner (IOwned of its owner type)
    /// overrides it. SetOwner, SetArrayOwner and SetOwnerWithoutNotify are the only writes of the
    /// owner, and each checks it here, so the typed owner read needs no check of its own. The
    /// loader passes every owner through one of them.
    /// </summary>
    protected virtual bool AcceptsOwner(CustomNode owner) => true;

    private void CheckOwner(CustomNode? owner)
    {
        if (owner is not null && !AcceptsOwner(owner))
        {
            throw new ArgumentException($"{GetType().Name} does not take an owner of type {owner.GetType().Name}.", paramName: nameof(owner));
        }
    }

    public string EngineObjectPath => GetEngineObjectPath();
}
