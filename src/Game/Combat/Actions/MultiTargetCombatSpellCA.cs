// Translated from CombatSpells.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// source ID, target byte list, casting mana, success, spell ID
public partial class MultiTargetCombatSpellCA : CombatAction
{
    public byte FSourceID;
    public ByteList FTargetIDs;
    public int FManaCost;
    public bool FSucceeded;
    public int FSpellID;

    // CombatSpells.pas:563
    public override void Dispose()
    {
        FTargetIDs?.Dispose();
        base.Dispose();
    }

    // CombatSpells.pas:569
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FSourceID, 20);
        Stream.rwStreamProc(21, FTargetIDs.LoadFromStream, FTargetIDs.SaveToStream);
        Stream.rwInteger(ref FManaCost, 22);
        Stream.rwBool(ref FSucceeded, 23);
        Stream.rwInteger(ref FSpellID, 24);
    }

    // CombatSpells.pas:557
    public MultiTargetCombatSpellCA(ECustomNode owner) : base(owner)
    {
        FTargetIDs = new ByteList();
    }

    // CombatSpells.pas:579
    public override void Execute(Battle Combat)
    {
        CombatObject Source;
        if (FManaCost != 0 && Combat.FStatus.Contains(CombatStatusElement.Executing))
        {
            Source = Combat.FCombatData.FindID(FSourceID);
            AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CombatCastingDone(Source, FManaCost);
        }
    }

    // CombatSpells.pas:591
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

        return Viewer.FDelay * 5 > Viewer.FTime;
    }

    // CombatSpells.pas:599
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        if (Obj.FID == FSourceID)
        {
            Obj.ShowAttackAnimation(Viewer.FTime, Direction, X, Y, Surface);
        }
        else
        {
            base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        }

        if (Obj.FID == FSourceID)
        {
            var spell = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
            GameTables.AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
        }
        else
        {
            if (FTargetIDs.IndexOf(Obj.FID) != -1)
            {
                AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
            }
        }
    }

    // CombatSpells.pas:612
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
            if (FTargetIDs.IndexOf(Obj.FID) != -1)
            {
                AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
            }
        }
    }

    // CombatSpells.pas:624
    public override void ShowFightingObjects(CombatViewer Viewer, int Frame, int X, int Y, in SurfaceDesc Surface)
    {
        CombatObject Source;
        Source = Viewer.FCombat.FCombatData.FindID(FSourceID);
        Viewer.DefaultShowFightingObjects(null, Source, Frame, X, Y, Surface);
    }

}
