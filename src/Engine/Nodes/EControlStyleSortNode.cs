// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// FControlStyle
public partial class EControlStyleSortNode : ESortNode
{
    // Engine.pas:1672
    public new const int ClassID = 261;

    internal Set<EControlStyleElement> FControlStyle;

    // Engine.pas:1666
    public EControlStyleSortNode(ECustomNode owner) : base(owner)
    {
        FControlStyle = base.GetControlStyle();
    }

    // Engine.pas:1677
    public override Set<EControlStyleElement> GetControlStyle() => FControlStyle;

    // Engine.pas:1682
    public void SetControlStyle(Set<EControlStyleElement> Value)
    {
        FControlStyle = Value;
    }

}
