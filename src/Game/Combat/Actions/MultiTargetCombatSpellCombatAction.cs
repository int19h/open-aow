// Translated from CombatSpells.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// source ID, target byte list, casting mana, success, spell ID
public partial class MultiTargetCombatSpellCombatAction : CombatAction
{
    public byte SourceId { get; protected set; }

    public ByteList TargetIds { get; }

    public int ManaCost { get; set; }

    private bool _succeeded;

    public int SpellId { get; protected set; }

    // CombatSpells.pas:563
    public override void Dispose()
    {
        TargetIds?.Dispose();
        base.Dispose();
    }

    // CombatSpells.pas:569
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SourceId = persister.Byte("SourceId", value: SourceId);
        PersistSupport.ByteList(persister, name: "TargetIds", list: TargetIds);
        ManaCost = persister.Int32("ManaCost", value: ManaCost);
        _succeeded = persister.Boolean("Succeeded", value: _succeeded);
        SpellId = persister.Int32("SpellId", value: SpellId);
    }

    // CombatSpells.pas:557
    public MultiTargetCombatSpellCombatAction(ECustomNode owner) : base(owner)
    {
        TargetIds = new ByteList();
    }

    // CombatSpells.pas:579
    public override void Execute(Battle combat)
    {
        CombatObject source;
        if (ManaCost != 0 && combat.Status.Contains(CombatStatusElement.Executing))
        {
            source = combat.CombatData.FindId(SourceId);
            GamePack.Current.SpellControl.GetSpell(SpellId).CombatCastingDone(source, ManaCost);
        }
    }

    // CombatSpells.pas:591
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

        return viewer.Delay * 5 > viewer.Time;
    }

    // CombatSpells.pas:599
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
            if (TargetIds.IndexOf(objectValue.Id) != -1)
            {
                GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.Show(11, X + 32, Y + 32, viewer.Time, surface);
            }
        }
    }

    // CombatSpells.pas:612
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
            if (TargetIds.IndexOf(objectValue.Id) != -1)
            {
                GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.Show(11, X + 32, Y + 32, viewer.Time, surface);
            }
        }
    }

    // CombatSpells.pas:624
    public override void ShowFightingObjects(CombatViewer viewer, int frame, int X, int Y, in LockedSurface surface)
    {
        CombatObject source;
        source = viewer.Combat.CombatData.FindId(SourceId);
        viewer.DefaultShowFightingObjects(null, source, frame, X, Y, surface);
    }

}
