// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Units;

// gold, mana, structures, army strength, heroes
public partial class TurnInfoItem : EngineObject
{
    public new const int ClassId = 131739;

    public TurnInfoItem(ECustomNode owner) : base(owner)
    { }

    public int Gold;
    public int Mana;
    public int Structures;
    public int ArmyStrength;
    public byte Heroes;

    // Player.PAS:161
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Gold, 25);
        stream.rwInteger(ref Mana, 26);
        stream.rwInteger(ref Structures, 27);
        stream.rwInteger(ref ArmyStrength, 28);
        stream.rwByte(ref Heroes, 29);
    }

    // Player.PAS:171
    public int GetSuperiority() => ArmyStrength / 100 + Structures * 10 + (int)(((uint)Gold) >> 3) + (int)(((uint)Mana) >> 3) + Heroes * 100;

    // Engine.pas:524

}
