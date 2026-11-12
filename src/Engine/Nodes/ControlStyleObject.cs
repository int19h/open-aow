// Translated from Engine.pas.
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// _controlStyle
public partial class ControlStyleObject : EngineObject
{
    // Engine.pas:8228
    public new const int ClassId = 81;

    internal Set<ControlStyleElement> _controlStyle;

    // Engine.pas:8222
    public ControlStyleObject(ECustomNode owner) : this(null, owner) { }

    // Engine.pas:8222
    private protected ControlStyleObject(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        _controlStyle = base.GetControlStyle();
    }

    // Engine.pas:8233
    public override Set<ControlStyleElement> GetControlStyle() => _controlStyle;

    // Engine.pas:8238
    public void SetControlStyle(Set<ControlStyleElement> value)
    {
        _controlStyle = value;
    }

    // Engine.pas:8243
    public override void ReadWrite(EStorageStream stream)
    {
        RwSet(ref _controlStyle, 1, 1, stream.rwData, 0);
    }

    public override void PersistFields(IPersister persister)
    {
        persister.Set("ControlStyle", ref _controlStyle);
    }

}
