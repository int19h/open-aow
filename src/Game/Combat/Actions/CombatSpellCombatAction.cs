#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;
using Aow.Game.Magic.Spells;

namespace Aow.Game.Combat.Actions;

// spell ID, casting mana. source and target IDs inherited
public class CombatSpellCombatAction(CustomNode owner) : DamageCombatAction(owner)
{
    // CombatSpells.pas:447
    public new const int ClassId = 131_778;

    public int SpellId { get; set; }
    public int ManaCost { get; set; }

    // CombatSpells.pas:452
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SpellId = persister.Int32("SpellId", value: SpellId);
        ManaCost = persister.Int32("ManaCost", value: ManaCost);
    }

    // CombatSpells.pas:459
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        if (ManaCost != 0 && combat.Status.Contains(CombatStatuses.Executing))
        {
            CombatObject source = combat.CombatData.FindId(SourceId);
            GetEngine().GamePack.SpellControl[SpellId].CombatCastingDone(source, mana: ManaCost);
        }
    }

    // CombatSpells.pas:471
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            _ = GetEngine().GamePack.SpellControl[SpellId].CastSounds.Play(0);
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
            Spell spell = GetEngine().GamePack.SpellControl[SpellId];
            GetEngine().CombatFont.SetTextCentered(X + 32, Y + 32 + 8, text: spell.SpellName, lockedSurface: surface);
        }
        else
        {
            if (objectValue.Id == TargetId)
            {
                _ = GetEngine().GamePack.SpellControl[SpellId].CastImages.Show(11, X + 32, Y + 32, frame: viewer.Time, lockedSurface: surface);
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
            Spell spell = GetEngine().GamePack.SpellControl[SpellId];
            GetEngine().CombatFont.SetTextCentered(X + 32, Y + 32 + 8, text: spell.SpellName, lockedSurface: surface);
        }
        else
        {
            if (objectValue.Id == TargetId)
            {
                _ = GetEngine().GamePack.SpellControl[SpellId].CastImages.Show(11, X + 32, Y + 32, frame: viewer.Time, lockedSurface: surface);
            }
        }
    }
}
