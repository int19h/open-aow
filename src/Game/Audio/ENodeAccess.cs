// Translated from Sound.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Audio;

// { TENode keeps its child list private in Engine.pas: the sequences and the library reach it
//     through this copy of TENode's layout (+8 FChildren). The code is the same as with a
//     protected FChildren, which is probably what the original Engine unit had. }
public partial class ENodeAccess : ECustomNode
{
    public ENodeAccess(ECustomNode owner) : base(owner)
    { }

    internal EObjectList FChildren;
}
