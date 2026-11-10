// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

// visits every string of an object tree through TranslateString and TranslateStrings. nothing
// is read or written
public partial class TranslateStorageStream : EStorageStream
{
    // Engine.pas:10825
    public TranslateStorageStream(GameEngine AEngine) : base(null, AEngine)
    {
        FPropertyTable = new PropertyTable();
    }

    // Engine.pas:10831
    public override void Dispose()
    {
        FPropertyTable?.Dispose();
        base.Dispose();
    }

    // Engine.pas:10837
    public override Set<StorageModeElement> StorageMode() => Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText, StorageModeElement.Translate);

    // Engine.pas:10842
    public override void rwString(ref string S, int ID)
    {
        TranslateString(ref S);
    }

    // Engine.pas:10847
    public override void rwStrings(StringCollection Strings, int ID)
    {
        TranslateStrings(ref Strings);
    }

    // Engine.pas:10852
    public override void rwStringsEx(StringCollection Strings, int ID)
    {
        TranslateStrings(ref Strings);
    }

    // Engine.pas:10857
    public override void rwCreateEObject(ref EObject Obj, ECustomNode AOwner, int ID)
    {
        EObject O;
        O = Obj;
        if (O != null)
        {
            rwEObject(O, ID);
        }
    }

    // Engine.pas:10865
    public override void rwEObject(EObject Obj, int ID)
    {
        bool Continue = default;
        try
        {
            FPropertyTable.BackupSettings();
            Obj.MainReadWrite(this);
            FPropertyTable.RestoreSettings();
        }
        catch
        {
            FPropertyTable.RestoreSettings();
            // { the original leaves Continue uninitialized here }
            if (FOnError != null)
            {
                var classID = Obj.EClass.ClassID;
                FOnError(this, "Error reading object (cid:" + IntToStr(classID) + ")", ref Continue);
            }

            if (!Continue)
            {
                throw;
            }
        }
    }

    // Engine.pas:10882
    public override void rwEObjectStream(EObject Obj, int ID)
    {
        rwEObject(Obj, ID);
    }

    // Engine.pas:10887
    public override void rwChildren(ECustomNode Node, int ID)
    {
        EObject C;
        for (int I = 0, last = Node.Count - 1; I <= last; I++)
        {
            C = Node.Children[I];
            if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineServices.csShared) && C.GetControlStyle().Contains(EngineServices.csStored))
            {
                rwEObject(C, I);
            }
        }
    }

    // Engine.pas:10901
    public virtual void TranslateStrings(ref StringCollection Strings)
    {
    }

    // Engine.pas:10905
    public virtual void TranslateString(ref string S)
    {
    }

}
