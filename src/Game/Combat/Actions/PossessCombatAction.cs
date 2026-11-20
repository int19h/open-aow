#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// (possession succeeded)
public class PossessCombatAction(CustomNode owner) : SingleTargetCombatAction(owner)
{
    public new const int ClassId = 131765;

    public bool Successful { get; private set; }

    public override bool TouchSucceeded => Successful;

    // AoWAb.PAS:2471
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Successful = persister.Boolean("Successful", value: Successful);
    }

    // AoWAb.PAS:2484
    public override void Execute(Battle combat)
    {
        if (Successful)
        {
            CombatUnit source = combat.CombatData.FindId<CombatUnit>(SourceId);
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            for (int i = 0; i <= 2; i++)
            {
                GamePack.Current.AbilityControl.GetAbility<CommandAbility>(AbilityRules.CommandAbilityIds[i]).ResetCommandedUnits(target);
            }

            for (int i = 0; i <= 2; i++)
            {
                GamePack.Current.AbilityControl.GetAbility<CommandAbility>(AbilityRules.CommandAbilityIds[i]).Uncommand(target);
            }

            Troop newUnit = target.Troop;
            _ = newUnit.AddRef();
            target.SetUnit(null);
            target.DestroyObject();
            Troop oldUnit = source.Troop;
            _ = oldUnit.AddRef();
            oldUnit.Killed();
            oldUnit.SetOwner(null);
            source.SetUnit(newUnit);
            if (oldUnit.UnitStatus.Contains(UnitStatusElement.Moving))
            {
                source.Troop.UnitStatus = source.Troop.UnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
            }

            _ = GamePack.Current.AbilityControl[107].Expand(source.Troop);
            PossessedAbilityData data;
            int experience;
            if (oldUnit.GetAbilityEnabled(107))
            {
                data = GamePack.Current.AbilityControl.GetAbility<PossessedAbility>(107).GetData(oldUnit);
                experience = data is not null ? data.SourceExperience : 0;
            }
            else
            {
                experience = oldUnit.GetExperience();
            }

            data = GamePack.Current.AbilityControl.GetAbility<PossessedAbility>(107).GetData(source.Troop);
            data?.SourceExperience = source.Troop.UnitLevel + experience;

            _ = newUnit.Release();
            _ = oldUnit.Release();
        }
    }

    // AoWAb.PAS:2537
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            _ = GamePack.Current.AbilityControl[106].SfxLibrary.Play(0);
        }

        if (viewer.Delay == viewer.Time)
        {
            Execute(viewer.Combat);
        }

        return viewer.Delay * 2 > viewer.Time;
    }

    // AoWAb.PAS:2529
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (objectValue.Id == SourceId || objectValue.Id == TargetId)
        {
            _ = GamePack.Current.AbilityControl[106].ImageSequences.Show(0, X + 32, Y + 32, frame: viewer.Time, lockedSurface: surface);
        }
    }

    // AoWAb.PAS:2477
    public void Initialize(byte sourceId, byte targetId, bool successful)
    {
        SourceId = sourceId;
        TargetId = targetId;
        Successful = successful;
    }
}
