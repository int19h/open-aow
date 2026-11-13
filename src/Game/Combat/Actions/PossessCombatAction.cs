// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// (possession succeeded)
public partial class PossessCombatAction : SingleTargetCombatAction
{
    public new const int ClassId = 131765;

    public PossessCombatAction(ECustomNode owner) : base(owner)
    { }

    public bool Successful;

    // AoWAb.PAS:2471
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Boolean(ref Successful, 20);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Successful = persister.Boolean("Successful", value: Successful);
    }

    // AoWAb.PAS:2484
    public override void Execute(Battle combat)
    {
        CombatUnit source;
        CombatUnit target;
        Troop newUnit;
        Troop oldUnit;
        PossessedAbilityData data;
        int experience;
        if (Successful)
        {
            source = (CombatUnit)combat.CombatData.FindId(SourceId);
            target = (CombatUnit)combat.CombatData.FindId(TargetId);
            for (int I = 0; I <= 2; I++)
            {
                ((CommandAbility)GamePack.Current.AbilityControl.GetAbility(AbilityRules.CommandAbilityIds[I])).ResetCommandedUnits(target);
            }

            for (int I = 0; I <= 2; I++)
            {
                ((CommandAbility)GamePack.Current.AbilityControl.GetAbility(AbilityRules.CommandAbilityIds[I])).Uncommand(target);
            }

            newUnit = target.Troop;
            newUnit.AddRef();
            target.SetUnit(null);
            target.DestroyObject();
            oldUnit = source.Troop;
            oldUnit.AddRef();
            oldUnit.Killed();
            oldUnit.SetOwner(null);
            source.SetUnit(newUnit);
            if (oldUnit.UnitStatus.Contains(UnitStatusElement.Moving))
            {
                source.Troop.UnitStatus = source.Troop.UnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
            }

            GamePack.Current.AbilityControl.GetAbility(107).Expand(source.Troop);
            if (oldUnit.GetAbilityEnabled(107))
            {
                data = (PossessedAbilityData)oldUnit.GetAbilityData(107);
                if (data != null)
                {
                    experience = data.SourceExperience;
                }
                else
                {
                    experience = 0;
                }
            }
            else
            {
                experience = oldUnit.GetExperience();
            }

            data = (PossessedAbilityData)source.Troop.GetAbilityData(107);
            if (data != null)
            {
                data.SourceExperience = source.Troop.GetUnitLevel() + experience;
            }

            newUnit.Release();
            oldUnit.Release();
        }
    }

    // AoWAb.PAS:2537
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            GamePack.Current.AbilityControl.GetAbility(106).SfxLibrary.Play(0);
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
            GamePack.Current.AbilityControl.GetAbility(106).ImageSequences.Show(0, X + 32, Y + 32, viewer.Time, surface);
        }
    }

    // AoWAb.PAS:2477
    public void Initialize(byte sourceId, byte targetId, bool successful)
    {
        SourceId = sourceId;
        TargetId = targetId;
        Successful = successful;
    }

    // intf_P3_02.inc:38

}
