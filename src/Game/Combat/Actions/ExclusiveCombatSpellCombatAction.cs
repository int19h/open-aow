// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// source ID, target ID, casting mana, success, spell ID
public partial class ExclusiveCombatSpellCombatAction : CombatAction
{
    public ExclusiveCombatSpellCombatAction(ECustomNode owner) : base(owner)
    { }

    public byte SourceId;
    public byte TargetId;
    public int ManaCost;
    public bool Succeeded;
    public int SpellId;

    // CombatSpells.pas:502
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref SourceId, 20);
        stream.Byte(ref TargetId, 21);
        stream.Int32(ref ManaCost, 22);
        stream.Boolean(ref Succeeded, 23);
        stream.Int32(ref SpellId, 24);
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
