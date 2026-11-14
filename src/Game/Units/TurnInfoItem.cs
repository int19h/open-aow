// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Units;

// gold, mana, structures, army strength, heroes
public partial class TurnInfoItem : EngineObject
{
    public new const int ClassId = 131739;

    public TurnInfoItem(ECustomNode owner) : base(owner)
    { }

    public int Gold { get; set; }
    public int Mana { get; set; }
    public int Structures { get; set; }
    public int ArmyStrength { get; set; }
    public byte Heroes { get; set; }

    // Player.PAS:161
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Gold = persister.Int32("Gold", value: Gold);
        Mana = persister.Int32("Mana", value: Mana);
        Structures = persister.Int32("Structures", value: Structures);
        ArmyStrength = persister.Int32("ArmyStrength", value: ArmyStrength);
        Heroes = persister.Byte("Heroes", value: Heroes);
    }

    // Player.PAS:171
    public int GetSuperiority() => ArmyStrength / 100 + Structures * 10 + (int)(((uint)Gold) >> 3) + (int)(((uint)Mana) >> 3) + Heroes * 100;

    // Engine.pas:524

}
