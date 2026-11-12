// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// spell ID, casting mana. source and target IDs inherited
public partial class CombatSpellCombatAction : DamageCombatAction
{
    // CombatSpells.pas:447
    public new const int ClassId = 131778;

    public CombatSpellCombatAction(ECustomNode owner) : base(owner)
    { }

    public int SpellId;
    public int ManaCost;

    // CombatSpells.pas:452
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref SpellId, 30);
        stream.Int32(ref ManaCost, 31);
    }

    // CombatSpells.pas:459
    public override void Execute(Battle combat)
    {
        CombatObject source;
        base.Execute(combat);
        if (ManaCost != 0 && combat.Status.Contains(CombatStatusElement.Executing))
        {
            source = combat.CombatData.FindId(SourceId);
            GamePack.Current.SpellControl.GetSpell(SpellId).CombatCastingDone(source, ManaCost);
        }
    }

    // CombatSpells.pas:471
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            GamePack.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(0);
        }

        if (viewer.Delay == viewer.Time)
        {
            Execute(viewer.Combat);
        }

        return viewer.Delay * 2 > viewer.Time;
    }

    // CombatSpells.pas:479
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (objectValue.Id == SourceId)
        {
            var spell = GamePack.Current.SpellControl.GetSpell(SpellId);
            GameTables.GameEngine.CombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.SpellName, surface);
        }
        else
        {
            if (objectValue.Id == TargetId)
            {
                GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.Show(11, X + 32, Y + 32, viewer.Time, surface);
            }
        }
    }

    // CombatSpells.pas:490
    public override void ShowFieldObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        // { Original calls ShowObject here, bypassing inherited field markers. }
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (objectValue.Id == SourceId)
        {
            var spell = GamePack.Current.SpellControl.GetSpell(SpellId);
            GameTables.GameEngine.CombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.SpellName, surface);
        }
        else
        {
            if (objectValue.Id == TargetId)
            {
                GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.Show(11, X + 32, Y + 32, viewer.Time, surface);
            }
        }
    }

}
