// Translated from Engine.pas.
using System.Reflection;

using Aow.Engine.Storage;

using Aow.Formats.Tree;
using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// FOwner
public partial class EObject : IDisposable, IPersistable
{
    // Engine.pas:2461
    public const int ClassID = 80;

    private EClass eclass;

    // The engine class descriptor of the instance's exact type, read once and cached.
    public EClass EClass => eclass ??= EClass.Of(GetType());

    // The Delphi pre-inherited statements run after capture and before any inherited body.
    private protected virtual void InitializeBeforeInherited() { }

    public EObject(ECustomNode owner) : this(null, owner) { }

    private protected EObject(ConstructionContext context, ECustomNode owner)
    {
        context?.Capture(this);
        InitializeBeforeInherited();
        SetOwner(owner);
    }

    protected internal ECustomNode FOwner;

    // Engine.pas:2487
    internal string GetEObjectPath()
    {
        EObject P;
        string S;
        S = this.GetType().Name;
        P = FOwner;
        while (P != null)
        {
            S = P.GetType().Name + "" + S;
            P = P.FOwner;
        }

        return S;
    }

    // Engine.pas:2442
    public virtual int ClassVersion() => 0;

    // Engine.pas:2563
    public virtual Set<EControlStyleElement> GetControlStyle() => Set<EControlStyleElement>.Of(EngineServices.csStored);

    // Engine.pas:2568
    public virtual void SetOwner(ECustomNode AOwner)
    {
        if (AOwner != FOwner)
        {
            if (FOwner != null)
            {
                FOwner.RemoveChild(this);
            }

            FOwner = AOwner;
            if (FOwner != null)
            {
                FOwner.AddChild(this);
            }
        }
    }

    // Engine.pas:2558
    public virtual GameEngine GetEngine() => FOwner.GetEngine();

    // Engine.pas:2502
    public virtual int GetLastMsg() => 2147483647;

    // Engine.pas:2507
    public virtual void SetLastMsg(int Value)
    {
    }

    // Engine.pas:2590
    public virtual void ReadWrite(EStorageStream Stream)
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
        const BindingFlags declared = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        for (var current = type; current is not null && current != typeof(object); current = current.BaseType)
        {
            bool image = current.GetMethod(nameof(MainReadWrite), declared) is not null
                || current.GetMethod("ReadWriteChildren", declared) is not null;
            bool tree = current.GetMethod(nameof(MainPersist), declared) is not null;
            if (image && !tree)
                return "Class " + current.Name + " overrides MainReadWrite or ReadWriteChildren and has no MainPersist.";
            bool fields = current.GetMethod(nameof(ReadWrite), declared) is not null;
            bool persisted = current.GetMethod(nameof(PersistFields), declared) is not null;
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
    public virtual void MainReadWrite(EStorageStream Stream)
    {
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            Clear();
        }

        ReadWrite(Stream);
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
    public virtual bool Compare(EObject Obj, GameEngine AEngine)
    {
        MemoryStream S1;
        MemoryStream S2;
        bool Result = false;
        if (EClass.ClassID != Obj.EClass.ClassID)
        {
            return Result;
        }

        S1 = new MemoryStream();
        S2 = new MemoryStream();
        try
        {
            AEngine.WriteEObject(S1, this, null);
            AEngine.WriteEObject(S2, Obj, null);
            bool condition = S1.Length == S2.Length;
            if (condition)
            {
                var size = (int)S1.Length;
                condition = CompareMem(S1.GetBuffer().AsSpan(0, size), S2.GetBuffer().AsSpan(0, size), size);
            }

            Result = condition;
        }
        finally
        {
            S1?.Dispose();
            S2?.Dispose();
        }

        return Result;
    }

    // Engine.pas:2466
    public virtual EditorClass Editor()
    {
        GameEngine E;
        EditorClass Result = null;
        E = GetEngine();
        if (E != null)
        {
            for (var C = EClass; C.Type != typeof(EObject) && Result == null; C = C.Parent)
            {
                Result = E.FEditors.FindEditor(C.ClassID);
            }
        }

        if (Result == null)
        {
            Result = EditorClass.Of<EObjectEditor>();
        }

        return Result;
    }

    // Engine.pas:2529
    public virtual void Assign(EObject Source, GameEngine AEngine)
    {
        MemoryStream S;
        S = new MemoryStream();
        try
        {
            AEngine.WriteEObject(S, Source, null);
            S.Position = 0;
            AEngine.ReadEObject(S, this, null);
        }
        finally
        {
            S.Dispose();
        }
    }

    // Engine.pas:2483
    public virtual void TimerEvent(int Tick, EObject Sender)
    {
    }

    // Engine.pas:2585
    public virtual void MsgProc(ref EMsg Msg, EObject Sender)
    {
        if (Msg.MsgID == 1)
        {
            SetLastMsg(2147483647);
        }
    }

    // Engine.pas:2578
    public virtual void SetArrayOwner(ECustomNode AOwner, int Index)
    {
        if (FOwner != null)
        {
            FOwner.RemoveChild(this);
        }

        FOwner = AOwner;
        if (FOwner != null)
        {
            FOwner.SetChild(Index, this);
        }
    }

    // Engine.pas:2436
    public virtual void Dispose()
    {
        SetOwner(null);
    }

    // Engine.pas:2543
    public EObject Copy(GameEngine AEngine)
    {
        EObject Result = default;
        MemoryStream S;
        S = new MemoryStream();
        try
        {
            AEngine.WriteEObject(S, this, null);
            S.Position = 0;
            Result = EClass.Create(null);
            AEngine.ReadEObject(S, Result, null);
        }
        finally
        {
            S.Dispose();
        }

        return Result;
    }

    public ECustomNode Owner { get => FOwner; set => SetOwner(value); }
    public string EObjectPath { get => GetEObjectPath(); }
    public int LastMsg { get => GetLastMsg(); set => SetLastMsg(value); }

}
