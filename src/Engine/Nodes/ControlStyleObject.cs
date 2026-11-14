// Translated from Engine.pas.
using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// ControlStyle
public partial class ControlStyleObject : EngineObject
{
    // Engine.pas:8228
    public new const int ClassId = 81;

    internal Set<ControlStyleElement> ControlStyle { get; set; }

    // Engine.pas:8222
    public ControlStyleObject(ECustomNode owner) : this(null, owner) { }

    // Engine.pas:8222
    private protected ControlStyleObject(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        ControlStyle = base.GetControlStyle();
    }

    // Engine.pas:8233
    public override Set<ControlStyleElement> GetControlStyle() => ControlStyle;

    // Engine.pas:8238
    public void SetControlStyle(Set<ControlStyleElement> value)
    {
        ControlStyle = value;
    }

    // Engine.pas:8243
    public override void PersistFields(IPersister persister)
    {
        ControlStyle = persister.Set("ControlStyle", ControlStyle);
    }

}
