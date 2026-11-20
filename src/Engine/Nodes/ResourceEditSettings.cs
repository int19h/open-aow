// Translated from Engine.pas.
using Aow.Formats.Tree;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Engine.Nodes;

// _editors, editor names with ids
public class ResourceEditSettings(CustomNode owner) : ControlStyleObject(owner)
{

    // Engine.pas:8662
    public new const int ClassId = 1_026;

    // Engine.pas:8677
    internal int GetUniqueEditId()
    {
        int result = 1;
        int i = 0;
        bool found = Editors.Count == 0;
        while (!found)
        {
            if (result != ObjectToInt(Editors.Objects[i]))
            {
                i += 1;
                found = i == Editors.Count;
            }
            else
            {
                i = 0;
                result += 1;
            }
        }

        return result;
    }

    // Engine.pas:8656
    public override void Dispose()
    {
        Editors?.Dispose();
        base.Dispose();
    }

    // Engine.pas:8667
    public string GetUniqueEditorName(string name)
    {
        string s = name;
        while (Editors.IndexOf(s) != -1)
        {
            s = name + IntToStr(Random(268435455));
        }

        return s;
    }

    // Engine.pas:8700
    public int GetEditId(string name)
    {
        int i = Editors.IndexOf(name);
        int result;
        if (i != -1)
        {
            return ObjectToInt(Editors.Objects[i]);
        }
        else
        {
            result = GetUniqueEditId();
            _ = Editors.AddObject(name, item: IntToObject(result));
        }

        return result;
    }

    // Engine.pas:8714
    public int RemoveEditor(string name)
    {
        int result = Editors.IndexOf(name);
        if (result != -1)
        {
            Editors.Delete(result);
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
            PersistSupport.LinesWithValues(persister, name: "Editors", lines: Editors);
        }
    }

    public DelphiStringList Editors { get; } = new DelphiStringList();

}
