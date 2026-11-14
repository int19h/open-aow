// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// source ID, target ID, casting mana, success, spell ID
public partial class ExclusiveCombatSpellCombatAction : CombatAction
{
    public ExclusiveCombatSpellCombatAction(ECustomNode owner) : base(owner)
    { }

    public byte SourceId { get; set; }
    public byte TargetId { get; set; }
    public int ManaCost { get; set; }
    public bool Succeeded { get; set; }
    public int SpellId { get; set; }

    // CombatSpells.pas:502
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SourceId = persister.Byte("SourceId", value: SourceId);
        TargetId = persister.Byte("TargetId", value: TargetId);
        ManaCost = persister.Int32("ManaCost", value: ManaCost);
        Succeeded = persister.Boolean("Succeeded", value: Succeeded);
        SpellId = persister.Int32("SpellId", value: SpellId);
    }

    // CombatSpells.pas:512
    public override void Execute(Battle combat)
    {
        CombatObject source;
        if (ManaCost != 0 && combat.Status.Contains(CombatStatusElement.Executing))
        {
            source = combat.CombatData.FindId(SourceId);
            GamePack.Current.SpellControl.GetSpell(SpellId).CombatCastingDone(source, ManaCost);
        }
    }

    // CombatSpells.pas:524
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

        return viewer.Delay * 3 > viewer.Time;
    }

    // CombatSpells.pas:532
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        if (objectValue.Id == SourceId)
        {
            objectValue.ShowAttackAnimation(viewer.Time, direction, X, Y, surface);
        }
        else
        {
            base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        }

        if (objectValue.Id == SourceId)
        {
            var spell = GamePack.Current.SpellControl.GetSpell(SpellId);
            GetEngine().CombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.SpellName, surface);
        }
        else
        {
            if (objectValue.Id == TargetId)
            {
                GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.Show(11, X + 32, Y + 32, viewer.Time, surface);
            }
        }
    }

    // CombatSpells.pas:545
    public override void ShowFieldObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        // { Original calls ShowObject here, bypassing inherited field markers. }
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (objectValue.Id == SourceId)
        {
            var spell = GamePack.Current.SpellControl.GetSpell(SpellId);
            GetEngine().CombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.SpellName, surface);
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
