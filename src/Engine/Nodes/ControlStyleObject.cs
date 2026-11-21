#nullable disable
// Translated from Engine.pas.
using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// ControlStyle
public class ControlStyleObject : EngineObject
{
    // Engine.pas:8228
    public new const int ClassId = 81;

    internal ControlStyles ControlStyle { get; set; }

    // Engine.pas:8222
    public ControlStyleObject(CustomNode owner) : this(null, owner) { }

    // Engine.pas:8222
    private protected ControlStyleObject(ConstructionContext context, CustomNode owner) : base(context, owner) => ControlStyle = base.GetControlStyle();

    // Engine.pas:8233
    public override ControlStyles GetControlStyle() => ControlStyle;

    // Engine.pas:8238
    public void SetControlStyle(ControlStyles value) => ControlStyle = value;

    // Engine.pas:8243
    public override void PersistFields(IPersister persister) => ControlStyle = persister.Set("ControlStyle", ControlStyle);

}
