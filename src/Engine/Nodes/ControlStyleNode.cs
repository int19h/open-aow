// Translated from Engine.pas.
using Aow.Formats.Tree;

namespace Aow.Engine.Nodes;

// _controlStyle
public class ControlStyleNode : OwningNode
{
    // Engine.pas:8199
    public new const int ClassId = 257;

    public ControlStyleNode(CustomNode owner) : base(owner) => _controlStyle = base.GetControlStyle();

    private ControlStyles _controlStyle;

    // Engine.pas:8204
    public override ControlStyles GetControlStyle() => _controlStyle;

    // Engine.pas:8209
    public void SetControlStyle(ControlStyles value) => _controlStyle = value;

    // Engine.pas:8214
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _controlStyle = persister.Set("ControlStyle", value: _controlStyle);
    }
}
