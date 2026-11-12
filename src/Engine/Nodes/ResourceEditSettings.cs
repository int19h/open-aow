// Translated from Engine.pas.
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// _editors, editor names with ids
public partial class ResourceEditSettings : ControlStyleObject
{
    // Engine.pas:8662
    public new const int ClassId = 1026;

    internal StringList _editors;

    // Engine.pas:8677
    internal int GetUniqueEditId()
    {
        int I;
        bool found;
        int result = 1;
        I = 0;
        found = _editors.Count == 0;
        while (!found)
        {
            if (result != ObjectToInt(_editors.Objects[I]))
            {
                I += 1;
                found = I == _editors.Count;
            }
            else
            {
                I = 0;
                result += 1;
            }
        }

        return result;
    }

    // Engine.pas:8650
    public ResourceEditSettings(ECustomNode owner) : base(owner)
    {
        _editors = new StringList();
    }

    // Engine.pas:8656
    public override void Dispose()
    {
        _editors?.Dispose();
        base.Dispose();
    }

    // Engine.pas:8667
    public string GetUniqueEditorName(string name)
    {
        string S;
        S = name;
        while (_editors.IndexOf(S) != -1)
        {
            S = name + IntToStr(Random(268435455));
        }

        return S;
    }

    // Engine.pas:8700
    public int GetEditId(string name)
    {
        int result;
        int I;
        I = _editors.IndexOf(name);
        if (I != -1)
        {
            result = ObjectToInt(_editors.Objects[I]);
        }
        else
        {
            result = GetUniqueEditId();
            _editors.AddObject(name, IntToObject(result));
        }

        return result;
    }

    // Engine.pas:8714
    public int RemoveEditor(string name)
    {
        int result = _editors.IndexOf(name);
        if (result != -1)
        {
            _editors.Delete(result);
        }

        return result;
    }

    // Engine.pas:8720
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        if (!stream.StorageMode().Contains(EngineServices.Text))
        {
            stream.rwStringsEx(_editors, 3);
        }
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.LinesWithValues(persister, "Editors", _editors);
    }

    public StringList Editors { get => _editors; }

}
