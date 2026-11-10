// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// spell ID, casting mana. source and target IDs inherited
public partial class CombatSpellCA : DamageCA
{
    // CombatSpells.pas:447
    public new const int ClassID = 131778;

    public CombatSpellCA(ECustomNode owner) : base(owner)
    { }

    public int FSpellID;
    public int FManaCost;

    // CombatSpells.pas:452
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FSpellID, 30);
        Stream.rwInteger(ref FManaCost, 31);
    }

    // CombatSpells.pas:459
    public override void Execute(Battle Combat)
    {
        CombatObject Source;
        base.Execute(Combat);
        if (FManaCost != 0 && Combat.FStatus.Contains(CombatStatusElement.Executing))
        {
            Source = Combat.FCombatData.FindID(FSourceID);
            AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CombatCastingDone(Source, FManaCost);
        }
    }

    // CombatSpells.pas:471
    public override bool Play(CombatViewer Viewer)
    {
        if (Viewer.FTime == 0)
        {
            AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
        }

        if (Viewer.FDelay == Viewer.FTime)
        {
            Execute(Viewer.FCombat);
        }

        return Viewer.FDelay * 2 > Viewer.FTime;
    }

    // CombatSpells.pas:479
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        if (Obj.FID == FSourceID)
        {
            var spell = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
            GameTables.AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
        }
        else
        {
            if (Obj.FID == FTargetID)
            {
                AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
            }
        }
    }

    // CombatSpells.pas:490
    public override void ShowFieldObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        // { Original calls ShowObject here, bypassing inherited field markers. }
        base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        if (Obj.FID == FSourceID)
        {
            var spell = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
            GameTables.AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
        }
        else
        {
            if (Obj.FID == FTargetID)
            {
                AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
            }
        }
    }

}
