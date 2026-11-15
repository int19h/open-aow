// Translated from Engine.pas.
using Aow.Formats.Tree;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// _editors, editor names with ids
public partial class ResourceEditSettings : ControlStyleObject
{
    // Engine.pas:8662
    public new const int ClassId = 1026;

    private readonly StringList _editors;

    // Engine.pas:8677
    internal int GetUniqueEditId()
    {
        int i;
        bool found;
        int result = 1;
        i = 0;
        found = _editors.Count == 0;
        while (!found)
        {
            if (result != ObjectToInt(_editors.Objects[i]))
            {
                i += 1;
                found = i == _editors.Count;
            }
            else
            {
                i = 0;
                result += 1;
            }
        }

        return result;
    }

    // Engine.pas:8650
    public ResourceEditSettings(CustomNode owner) : base(owner) => _editors = new StringList();

    // Engine.pas:8656
    public override void Dispose()
    {
        _editors?.Dispose();
        base.Dispose();
    }

    // Engine.pas:8667
    public string GetUniqueEditorName(string name)
    {
        string s;
        s = name;
        while (_editors.IndexOf(s) != -1)
        {
            s = name + IntToStr(Random(268435455));
        }

        return s;
    }

    // Engine.pas:8700
    public int GetEditId(string name)
    {
        int result;
        int i;
        i = _editors.IndexOf(name);
        if (i != -1)
        {
            result = ObjectToInt(_editors.Objects[i]);
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
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        // The original ReadWrite leaves out the editors under text only (the translate stream).
        if (!PersistSupport.Translating(persister))
        {
            PersistSupport.LinesWithValues(persister, "Editors", _editors);
        }
    }

    public StringList Editors => _editors;

}
