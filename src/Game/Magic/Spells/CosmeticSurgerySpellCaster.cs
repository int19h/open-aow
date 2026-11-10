// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// replacement face index, target unit ID
public partial class CosmeticSurgerySpellCaster : SpellCaster
{
    public CosmeticSurgerySpellCaster(ECustomNode owner) : base(owner)
    { }

    public int FFaceIndex;
    public int FTargetUnitID;

    // UnitSpells.pas:870
    public override bool CanCast(ref string Reason) => base.CanCast(ref Reason);

    // UnitSpells.pas:875
    public override bool Cast()
    {
        bool Result;
        AbstractUnit Source;
        AbstractUnit Target;
        UnitSpellTE Event;
        string Reason = "";
        ((UnitSpell)AoWHSSet.Current.FSpellControl.GetSpell(59)).ResetTargetSelection();
        Source = AoWHSMap.Current.FUnitControl.FindUnit(FCasterSourceID);
        Target = AoWHSMap.Current.FUnitControl.FindUnit(FTargetUnitID);
        if (Source != null && Target != null && CanCast(ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new UnitSpellTE(null);
            var spell = AoWHSSet.Current.FSpellControl.GetSpell(59);
            Event.Initialize(FCasterSpellID, Source, null, spell.FBaseManaCost);
            Event.FTargetUnitIDs.Add(FTargetUnitID);
            Event.FSpellData[0] = FFaceIndex;
            Event.FTargetPlayer = (byte)Target.FPlayer;
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

}
