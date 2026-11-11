// Translated from Engine.pas.
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// FControlStyle
public partial class ControlStyleObject : EngineObject
{
    // Engine.pas:8228
    public new const int ClassID = 81;

    internal Set<ControlStyleElement> FControlStyle;

    // Engine.pas:8222
    public ControlStyleObject(ECustomNode owner) : this(null, owner) { }

    // Engine.pas:8222
    private protected ControlStyleObject(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        FControlStyle = base.GetControlStyle();
    }

    // Engine.pas:8233
    public override Set<ControlStyleElement> GetControlStyle() => FControlStyle;

    // Engine.pas:8238
    public void SetControlStyle(Set<ControlStyleElement> Value)
    {
        FControlStyle = Value;
    }

    // Engine.pas:8243
    public override void ReadWrite(EStorageStream Stream)
    {
        RwSet(ref FControlStyle, 1, 1, Stream.rwData, 0);
    }

    public override void PersistFields(IPersister persister)
    {
        persister.Set("ControlStyle", ref FControlStyle);
    }

}
