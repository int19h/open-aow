// Translated from Engine.pas.
using Aow.Formats.Tree;
using Aow.Rtl;

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
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _controlStyle = persister.Set("ControlStyle", value: _controlStyle);
    }

}
