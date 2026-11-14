// Translated from Engine.pas.
using System.Collections.Concurrent;
using System.Reflection;

using Aow.Engine.Storage;

using Aow.Formats.Tree;
using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// _owner
public partial class EngineObject : IDisposable, IPersistable, IOwned
{
    // Engine.pas:2461
    public const int ClassId = 80;

    private EngineClass _engineClass;

    // The engine class descriptor of the instance's exact type, read once and cached.
    public EngineClass EngineClass => _engineClass ??= EngineClass.Of(GetType());

    // The Delphi pre-inherited statements run after capture and before any inherited body.
    private protected virtual void InitializeBeforeInherited() { }

    public EngineObject(ECustomNode owner) : this(null, owner) { }

    private protected EngineObject(ConstructionContext context, ECustomNode owner)
    {
        context?.Capture(this);
        InitializeBeforeInherited();
        SetOwner(owner);
    }

    private ECustomNode _owner;

    // Engine.pas:2487. The walk reads each owner through IOwned.
    internal string GetEngineObjectPath()
    {
        string path = GetType().Name;
        for (var owner = ((IOwned)this).Owner; owner != null; owner = (owner as IOwned)?.Owner)
        {
            path = owner.GetType().Name + path;
        }

        return path;
    }

    // Engine.pas:2442
    public virtual int ClassVersion() => 0;

    // Engine.pas:2563
    public virtual Set<ControlStyleElement> GetControlStyle() => Set<ControlStyleElement>.Of(EngineServices.Stored);

    // Engine.pas:2568
    public virtual void SetOwner(ECustomNode owner)
    {
        if (owner != _owner)
        {
            CheckOwner(owner);
            if (_owner != null)
            {
                _owner.RemoveChild(this);
            }

            _owner = owner;
            if (_owner != null)
            {
                _owner.AddChild(this);
            }
        }
    }

    // The node through which a node without an owner reaches its engine: the engine for what it
    // registers, creates, loads or copies, a node for the node its constructor keeps in a field.
    // A node reaches its engine through its owner, and without an owner through its holder.
    public EngineObject Holder { get; set; }

    // The holder for a node this node creates without an owner: the engine once this node reaches
    // it, else this node, through which the created node reaches the engine later. The engine
    // itself keeps a holder walk short and stops it from looping through a node that comes to own
    // its creator (a troop creates the army sprite that then holds it).
    public EngineObject CreationHolder() => (EngineObject)GetEngine() ?? this;

    // Engine.pas:2558. The original walks the owners only and reads the engine global where a node
    // has no owner.
    public virtual GameEngine GetEngine() => _owner is not null ? _owner.GetEngine() : Holder?.GetEngine();

    // Engine.pas:2502
    public virtual int GetLastMessage() => 2147483647;

    // Engine.pas:2507
    public virtual void SetLastMessage(int value)
    {
    }

    // Engine.pas:2590
    public virtual void ReadWrite(StorageStream stream)
    {
    }

    // Tree form of ReadWrite. MainPersist is the stored image.
    public virtual void PersistFields(IPersister persister)
    {
    }

    void IPersistable.Persist(IPersister persister)
    {
        if (TreePersistenceRefusal(GetType()) is string reason)
            throw new InvalidOperationException(reason);
        MainPersist(persister);
    }

    /// <summary>
    /// A class that stores through MainReadWrite needs the same MainPersist, through
    /// ReadWriteChildren the same PersistChildren, and through ReadWrite its own PersistFields.
    /// The name is the class that is missing it. Null means the closure can use the tree.
    /// </summary>
    public static string TreePersistenceRefusal(Type type)
    {
        const BindingFlags Declared = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        for (var current = type; current is not null && current != typeof(object); current = current.BaseType)
        {
            if (Declares(current, nameof(MainReadWrite)) && !Declares(current, nameof(MainPersist)))
                return "Class " + current.Name + " overrides MainReadWrite and has no MainPersist.";
            if (Declares(current, nameof(ECustomNode.ReadWriteChildren)) && !Declares(current, nameof(ECustomNode.PersistChildren)))
                return "Class " + current.Name + " overrides ReadWriteChildren and has no PersistChildren.";
            if (Declares(current, nameof(ReadWrite)) && !Declares(current, nameof(PersistFields)))
                return "Class " + current.Name + " overrides ReadWrite and has no PersistFields.";
        }
        return null;

        static bool Declares(Type declaring, string name) => declaring.GetMethod(name, Declared) is not null;
    }

    private static readonly ConcurrentDictionary<Type, bool> s_streamRetired = new();

    /// <summary>
    /// True when a class of the type's chain persists only through the tree: it declares
    /// PersistFields without ReadWrite, MainPersist without MainReadWrite or PersistChildren
    /// without ReadWriteChildren (issue 185 PE-3-i). The storage streams store such an object
    /// through the tree and the original backend.
    /// </summary>
    public static bool StreamRetired(Type type) => s_streamRetired.GetOrAdd(type, static type =>
    {
        const BindingFlags Declared = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        for (var current = type; current is not null && current != typeof(object); current = current.BaseType)
        {
            if (Declares(current, nameof(PersistFields)) && !Declares(current, nameof(ReadWrite))
                || Declares(current, nameof(MainPersist)) && !Declares(current, nameof(MainReadWrite))
                || Declares(current, nameof(ECustomNode.PersistChildren)) && !Declares(current, nameof(ECustomNode.ReadWriteChildren)))
                return true;
        }
        return false;

        static bool Declares(Type declaring, string name) => declaring.GetMethod(name, Declared) is not null;
    });

    public virtual void MainPersist(IPersister persister)
    {
        if (persister.Loading)
        {
            Clear();
        }

        PersistFields(persister);
    }

    // Engine.pas:2594
    public virtual void MainReadWrite(StorageStream stream)
    {
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Text, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            Clear();
        }

        ReadWrite(stream);
    }

    // Engine.pas:2447
    public virtual int AddRef()
    {
        throw new NotSupportedException("Reference counting not supported");
    }

    // Engine.pas:2452
    public virtual int Release()
    {
        throw new NotSupportedException("Reference counting not supported");
    }

    // Engine.pas:2457
    public virtual void Clear()
    {
    }

    // Engine.pas:2511
    // The original compares the images WriteEObject writes. Two objects of one class whose trees
    // are equal write equal images (persistence-tree.md 13).
    public virtual bool Compare(EngineObject objectValue, GameEngine engine)
    {
        if (EngineClass.ClassId != objectValue.EngineClass.ClassId)
        {
            return false;
        }

        var factory = new EnginePersistableFactory(engine);
        return TreeWriter.WriteObject(this, factory: factory).ValueEquals(TreeWriter.WriteObject(objectValue, factory: factory));
    }

    // Engine.pas:2466
    public virtual EditorClass Editor()
    {
        GameEngine E;
        EditorClass result = null;
        E = GetEngine();
        if (E != null)
        {
            for (var type = GetType(); type != typeof(EngineObject) && result == null; type = type.BaseType)
            {
                result = E.Editors.FindEditor(EngineClass.Of(type).ClassId);
            }
        }

        if (result == null)
        {
            result = EditorClass.Of<EngineObjectEditor>();
        }

        return result;
    }

    // Engine.pas:2529
    // The original writes the source with WriteEObject and reads the image into this object with
    // ReadEObject. The source's tree loads into this object the same way: the load clears it
    // first, a member the source leaves out takes its fallback, and a failure goes to the load's
    // handler, of which a copy has none (persistence-tree.md 13).
    public virtual void Assign(EngineObject source, GameEngine engine)
    {
        var factory = new EnginePersistableFactory(engine);
        TreeReader.ReadObject(TreeWriter.WriteObject(source, factory: factory), objectValue: this, factory: factory);
    }

    // Engine.pas:2483
    public virtual void TimerEvent(int tick, EngineObject sender)
    {
    }

    // Engine.pas:2585
    public virtual void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        if (message.MessageId == 1)
        {
            SetLastMessage(2147483647);
        }
    }

    // Engine.pas:2578
    public virtual void SetArrayOwner(ECustomNode owner, int index)
    {
        CheckOwner(owner);
        if (_owner != null)
        {
            _owner.RemoveChild(this);
        }

        _owner = owner;
        if (_owner != null)
        {
            _owner.SetChild(index, this);
        }
    }

    // Engine.pas:2436
    public virtual void Dispose()
    {
        SetOwner(null);
    }

    // Engine.pas:2543
    // A new object of this class loads the tree of this one, as Assign loads it.
    public EngineObject Copy(GameEngine engine)
    {
        var factory = new EnginePersistableFactory(engine);
        var tree = TreeWriter.WriteObject(this, factory: factory);
        var result = EngineClass.CreateInstance(null, holder: engine);
        TreeReader.ReadObject(tree, objectValue: result, factory: factory);
        return result;
    }

    public ECustomNode Owner { get => _owner; set => SetOwner(value); }

    IOwner IOwned.Owner => _owner;

    protected internal void SetOwnerWithoutNotify(ECustomNode value)
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
    protected virtual bool AcceptsOwner(ECustomNode owner) => true;

    private void CheckOwner(ECustomNode owner)
    {
        if (owner != null && !AcceptsOwner(owner))
        {
            throw new ArgumentException(GetType().Name + " does not take an owner of type " + owner.GetType().Name + ".", nameof(owner));
        }
    }

    public string EngineObjectPath { get => GetEngineObjectPath(); }
    public int LastMessage { get => GetLastMessage(); set => SetLastMessage(value); }

}
