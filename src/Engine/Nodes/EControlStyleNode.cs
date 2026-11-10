// Translated from Engine.pas.
using Aow.Engine.Storage;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// FControlStyle
public partial class EControlStyleNode : ENode
{
    // Engine.pas:8199
    public new const int ClassID = 257;

    public EControlStyleNode(ECustomNode owner) : base(owner)
    {
        FControlStyle = base.GetControlStyle();
    }

    internal Set<EControlStyleElement> FControlStyle;

    // Engine.pas:8204
    public override Set<EControlStyleElement> GetControlStyle() => FControlStyle;

    // Engine.pas:8209
    public void SetControlStyle(Set<EControlStyleElement> Value)
    {
        FControlStyle = Value;
    }

    // Engine.pas:8214
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        RwSet(ref FControlStyle, 1, 2, Stream.rwData, 0);
    }

}
