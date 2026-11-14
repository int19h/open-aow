// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

// visits every string of an object tree through TranslateString and TranslateStrings. nothing
// is read or written
public partial class TranslateStorageStream : StorageStream
{
    // Engine.pas:10825
    public TranslateStorageStream(GameEngine engine) : base(null, engine)
    {
        _propertyTable = new PropertyTable();
    }

    // Engine.pas:10831
    public override void Dispose()
    {
        _propertyTable?.Dispose();
        base.Dispose();
    }

    // Engine.pas:10837
    public override Set<StorageModeElement> StorageMode() => Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Text, StorageModeElement.Translate);

    // Engine.pas:10842
    public override void String(ref string value, int id)
    {
        TranslateString(ref value);
    }

    // Engine.pas:10847
    public override void Lines(StringCollection lines, int id)
    {
        TranslateStrings(ref lines);
    }

    // Engine.pas:10852
    public override void LinesWithObjects(StringCollection lines, int id)
    {
        TranslateStrings(ref lines);
    }

    // Engine.pas:10857
    public override void Created(ref EngineObject objectValue, ECustomNode owner, int id)
    {
        EngineObject current;
        current = objectValue;
        if (current != null)
        {
            Object(current, id);
        }
    }

    // Engine.pas:10865
    public override void Object(EngineObject objectValue, int id)
    {
        bool canContinue = default;
        try
        {
            _propertyTable.BackupSettings();
            PersistObject(objectValue);
            _propertyTable.RestoreSettings();
        }
        catch
        {
            _propertyTable.RestoreSettings();
            // { the original leaves Continue uninitialized here }
            if (HasErrorHandlers)
            {
                var classId = objectValue.EngineClass.ClassId;
                RaiseError(this, "Error reading object (cid:" + IntToStr(classId) + ")", ref canContinue);
            }

            if (!canContinue)
            {
                throw;
            }
        }
    }

    // A class whose ReadWrite is retired (PE-3-i) is visited through its tree methods, with a
    // persister that translates its strings and loads and saves nothing.
    private protected override void PersistTree(EngineObject objectValue) =>
        objectValue.MainPersist(new TranslatePersister(this, node: objectValue));

    // Engine.pas:10882
    public override void SizedObject(EngineObject objectValue, int id)
    {
        Object(objectValue, id);
    }

    // Engine.pas:10887
    public override void Children(ECustomNode node, int id)
    {
        EngineObject child;
        for (int i = 0, last = node.Count - 1; i <= last; i++)
        {
            child = node.Children[i];
            if (child != null && child.Owner == node && !child.GetControlStyle().Contains(EngineServices.Shared) && child.GetControlStyle().Contains(EngineServices.Stored))
            {
                Object(child, i);
            }
        }
    }

    // Engine.pas:10901
    public virtual void TranslateStrings(ref StringCollection lines)
    {
    }

    // Engine.pas:10905
    public virtual void TranslateString(ref string value)
    {
    }

}
