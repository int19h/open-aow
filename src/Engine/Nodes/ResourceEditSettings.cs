// Translated from Engine.pas.
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// FEditors, editor names with ids
public partial class ResourceEditSettings : ControlStyleObject
{
    // Engine.pas:8662
    public new const int ClassID = 1026;

    internal StringList FEditors;

    // Engine.pas:8677
    internal int GetUniqueEditID()
    {
        int I;
        bool Found;
        int Result = 1;
        I = 0;
        Found = FEditors.Count == 0;
        while (!Found)
        {
            if (Result != ObjectToInt(FEditors.Objects[I]))
            {
                I += 1;
                Found = I == FEditors.Count;
            }
            else
            {
                I = 0;
                Result += 1;
            }
        }

        return Result;
    }

    // Engine.pas:8650
    public ResourceEditSettings(ECustomNode owner) : base(owner)
    {
        FEditors = new StringList();
    }

    // Engine.pas:8656
    public override void Dispose()
    {
        FEditors?.Dispose();
        base.Dispose();
    }

    // Engine.pas:8667
    public string GetUniqueEditorName(string Name)
    {
        string S;
        S = Name;
        while (FEditors.IndexOf(S) != -1)
        {
            S = Name + IntToStr(Random(268435455));
        }

        return S;
    }

    // Engine.pas:8700
    public int GetEditID(string Name)
    {
        int Result;
        int I;
        I = FEditors.IndexOf(Name);
        if (I != -1)
        {
            Result = ObjectToInt(FEditors.Objects[I]);
        }
        else
        {
            Result = GetUniqueEditID();
            FEditors.AddObject(Name, IntToObject(Result));
        }

        return Result;
    }

    // Engine.pas:8714
    public int RemoveEditor(string Name)
    {
        int Result = FEditors.IndexOf(Name);
        if (Result != -1)
        {
            FEditors.Delete(Result);
        }

        return Result;
    }

    // Engine.pas:8720
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(EngineServices.smText))
        {
            Stream.rwStringsEx(FEditors, 3);
        }
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.LinesEx(persister, "Editors", FEditors);
    }

    public StringList Editors { get => FEditors; }

}
