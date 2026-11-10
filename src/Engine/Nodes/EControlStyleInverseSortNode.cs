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

    internal Set<EControlStyleElement> FControlStyle;

    // Engine.pas:1700
    public override Set<EControlStyleElement> GetControlStyle() => FControlStyle;

    // Engine.pas:1705
    public void SetControlStyle(Set<EControlStyleElement> Value)
    {
        FControlStyle = Value;
    }

    // Engine.pas:963

}
