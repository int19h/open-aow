// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// _controlStyle
public partial class EControlStyleInverseSortNode : EInverseSortNode
{
    // Engine.pas:1695
    public new const int ClassId = 263;

    public EControlStyleInverseSortNode(ECustomNode owner) : base(owner)
    {
        _controlStyle = base.GetControlStyle();
    }

    private Set<ControlStyleElement> _controlStyle;

    // Engine.pas:1700
    public override Set<ControlStyleElement> GetControlStyle() => _controlStyle;

    // Engine.pas:1705
    public void SetControlStyle(Set<ControlStyleElement> value)
    {
        _controlStyle = value;
    }

    // Engine.pas:963

}
