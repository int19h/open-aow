// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.Combat;

public partial class CombatPlayer : EngineObject
{
    // Combat2.pas:115
    public new const int ClassId = 131744;

    public CombatPlayer(ECustomNode owner) : base(owner)
    { }

    public sbyte Player;
    public byte IndependentRelation;
    public byte SideIndex;

    // Combat2.pas:118
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Player), 20);
        stream.rwByte(ref IndependentRelation, 21);
        stream.rwData(Bytes.Of(ref SideIndex, 1), 1, 22);
    }

    // Engine.pas:524

}
