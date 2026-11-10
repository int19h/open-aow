// Translated from NetworkE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Net;

// { The original reaches TENode's child list (Engine.TENode.FChildren, +8) from this unit; the
//     recovered Engine.pas keeps the field private, so this layout view of TENode reads it (a
//     typecast of a field or parameter compiles to no code) }
public partial class ENodeView : ECustomNode
{
    public ENodeView(ECustomNode owner) : base(owner)
    { }

    public EObjectList FChildren;
}
