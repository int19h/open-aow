// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.Combat;

public partial class CombatPlayer : EngineObject
{
    // Combat2.pas:115
    public new const int ClassID = 131744;

    public CombatPlayer(ECustomNode owner) : base(owner)
    { }

    public sbyte FPlayer;
    public byte FIndependentRelation;
    public byte FSideIndex;

    // Combat2.pas:118
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 20);
        Stream.rwByte(ref FIndependentRelation, 21);
        Stream.rwData(Bytes.Of(ref FSideIndex, 1), 1, 22);
    }

    // Engine.pas:524

}
