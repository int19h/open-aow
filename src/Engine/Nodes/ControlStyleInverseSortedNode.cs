// Translated from Engine.pas.

namespace Aow.Engine.Nodes;

// _controlStyle
public class ControlStyleInverseSortedNode : InverseSortedNode
{
    // Engine.pas:1695
    public new const int ClassId = 263;

    public ControlStyleInverseSortedNode(CustomNode owner) : base(owner) => _controlStyle = base.GetControlStyle();

    private ControlStyles _controlStyle;

    // Engine.pas:1700
    public override ControlStyles GetControlStyle() => _controlStyle;

    // Engine.pas:1705
    public void SetControlStyle(ControlStyles value) => _controlStyle = value;
}
