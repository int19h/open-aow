// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Units;

// gold, mana, structures, army strength, heroes
public partial class TurnInfoItem : EngineObject
{
    public new const int ClassID = 131739;

    public TurnInfoItem(ECustomNode owner) : base(owner)
    { }

    public int FGold;
    public int FMana;
    public int FStructures;
    public int FArmyStrength;
    public byte FHeroes;

    // Player.PAS:161
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FGold, 25);
        Stream.rwInteger(ref FMana, 26);
        Stream.rwInteger(ref FStructures, 27);
        Stream.rwInteger(ref FArmyStrength, 28);
        Stream.rwByte(ref FHeroes, 29);
    }

    // Player.PAS:171
    public int GetSuperiority() => FArmyStrength / 100 + FStructures * 10 + (int)(((uint)FGold) >> 3) + (int)(((uint)FMana) >> 3) + FHeroes * 100;

    // Engine.pas:524

}
