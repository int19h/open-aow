// Translated from Engine.pas.
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

    // Engine.pas:2558
    public virtual GameEngine GetEngine() => _owner.GetEngine();

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
    /// A class that stores through MainReadWrite or ReadWriteChildren needs the same MainPersist.
    /// A class that stores through ReadWrite needs its own PersistFields.
    /// The name is the class that is missing it. Null means the closure can use the tree.
    /// </summary>
    public static string TreePersistenceRefusal(Type type)
    {
        const BindingFlags Declared = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        for (var current = type; current is not null && current != typeof(object); current = current.BaseType)
        {
            bool image = current.GetMethod(nameof(MainReadWrite), Declared) is not null
                || current.GetMethod("ReadWriteChildren", Declared) is not null;
            bool tree = current.GetMethod(nameof(MainPersist), Declared) is not null;
            if (image && !tree)
                return "Class " + current.Name + " overrides MainReadWrite or ReadWriteChildren and has no MainPersist.";
            bool fields = current.GetMethod(nameof(ReadWrite), Declared) is not null;
            bool persisted = current.GetMethod(nameof(PersistFields), Declared) is not null;
            if (fields && !persisted)
                return "Class " + current.Name + " overrides ReadWrite and has no PersistFields.";
        }
        return null;
    }

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
    public virtual bool Compare(EngineObject objectValue, GameEngine engine)
    {
        MemoryStream s1;
        MemoryStream s2;
        bool result = false;
        if (EngineClass.ClassId != objectValue.EngineClass.ClassId)
        {
            return result;
        }

        s1 = new MemoryStream();
        s2 = new MemoryStream();
        try
        {
            engine.WriteEngineObject(s1, this, null);
            engine.WriteEngineObject(s2, objectValue, null);
            bool condition = s1.Length == s2.Length;
            if (condition)
            {
                var size = (int)s1.Length;
                condition = CompareMem(s1.GetBuffer().AsSpan(0, size), s2.GetBuffer().AsSpan(0, size), size);
            }

            result = condition;
        }
        finally
        {
            s1?.Dispose();
            s2?.Dispose();
        }

        return result;
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
    public virtual void Assign(EngineObject source, GameEngine engine)
    {
        MemoryStream S;
        S = new MemoryStream();
        try
        {
            engine.WriteEngineObject(S, source, null);
            S.Position = 0;
            engine.ReadEngineObject(S, this, null);
        }
        finally
        {
            S.Dispose();
        }
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
    public EngineObject Copy(GameEngine engine)
    {
        EngineObject result = default;
        MemoryStream S;
        S = new MemoryStream();
        try
        {
            engine.WriteEngineObject(S, this, null);
            S.Position = 0;
            result = EngineClass.CreateInstance(null);
            engine.ReadEngineObject(S, result, null);
        }
        finally
        {
            S.Dispose();
        }

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
