// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World;

public partial class P3NodeAccess : ENode
{
    public P3NodeAccess(ECustomNode owner) : base(owner)
    { }

    public EObjectList ChildrenList { get => FChildren; }
}
