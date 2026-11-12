// Translated from Engine.pas.
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// _controlStyle
public partial class EControlStyleNode : ENode
{
    // Engine.pas:8199
    public new const int ClassId = 257;

    public EControlStyleNode(ECustomNode owner) : base(owner)
    {
        _controlStyle = base.GetControlStyle();
    }

    private Set<ControlStyleElement> _controlStyle;

    // Engine.pas:8204
    public override Set<ControlStyleElement> GetControlStyle() => _controlStyle;

    // Engine.pas:8209
    public void SetControlStyle(Set<ControlStyleElement> value)
    {
        _controlStyle = value;
    }

    // Engine.pas:8214
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        TransferSet(ref _controlStyle, 1, 2, stream.Bytes, 0);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _controlStyle = persister.Set("ControlStyle", value: _controlStyle);
    }

}
