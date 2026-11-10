// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// spell ID, mana cost charged after execution
public partial class DispelMagicSpellCA : DispelMagicCA
{
    // UnitSpells.pas:975
    public new const int ClassID = 131793;

    public DispelMagicSpellCA(ECustomNode owner) : base(owner)
    { }

    public int FSpellID;
    public int FManaCost;

    // UnitSpells.pas:980
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FSpellID, 60);
        Stream.rwInteger(ref FManaCost, 61);
    }

    // UnitSpells.pas:987
    public override void Execute(Battle Combat)
    {
        CombatUnit Source;
        base.Execute(Combat);
        if (FManaCost != 0 && Combat.FStatus.Contains(CombatStatusElement.Executing))
        {
            if (AoWHSSet.Current.FSpellControl.GetSpell(FSpellID) != null)
            {
                Source = (CombatUnit)Combat.FCombatData.FindID(FSourceID);
                AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CastingDone(Source.FUnit, FManaCost);
            }
        }
    }

}
