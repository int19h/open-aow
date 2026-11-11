// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// FControlStyle
public partial class EControlStyleInverseSortNode : EInverseSortNode
{
    // Engine.pas:1695
    public new const int ClassID = 263;

    public EControlStyleInverseSortNode(ECustomNode owner) : base(owner)
    {
        FControlStyle = base.GetControlStyle();
    }

    internal Set<ControlStyleElement> FControlStyle;

    // Engine.pas:1700
    public override Set<ControlStyleElement> GetControlStyle() => FControlStyle;

    // Engine.pas:1705
    public void SetControlStyle(Set<ControlStyleElement> Value)
    {
        FControlStyle = Value;
    }

    // Engine.pas:963

}
