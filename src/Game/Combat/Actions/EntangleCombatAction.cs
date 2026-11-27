#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

namespace Aow.Game.Combat.Actions;

// (successful)
public class EntangleCombatAction(CustomNode owner) : SingleTargetCombatAction(owner)
{
    public new const int ClassId = 131_774;

    public bool Successful { get; private set; }

    public override bool TouchSucceeded => Successful;

    // AoWAb.PAS:2927
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Successful = persister.Boolean("Successful", value: Successful);
    }

    // AoWAb.PAS:2938
    public override void Execute(Battle combat)
    {
        if (Successful)
        {
            CombatUnit source = combat.CombatData.FindId<CombatUnit>(SourceId);
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            _ = GetEngine().GamePack.AbilityControl[94].Expand(target.Troop);
            UnitEnchantmentAbilityData data = target.Troop.GetAbilityData<UnitEnchantmentAbilityData>(94);
            data?.EnchantmentPlayerIndex = PlayerId.FromOriginal(source.CombatPlayer);
        }
    }

    // AoWAb.PAS:2960
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            _ = GetEngine().GamePack.AbilityControl[40].SfxLibrary.Play(0);
        }

        if (viewer.Delay == viewer.Time)
        {
            Execute(viewer.Combat);
        }

        return viewer.Delay * 2 > viewer.Time;
    }

    // AoWAb.PAS:2953
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (objectValue.Id == TargetId)
        {
            _ = GetEngine().GamePack.AbilityControl[40].ImageSequences.Show(0, X + 32, Y + 32, frame: viewer.Time, lockedSurface: surface);
        }
    }

    // AoWAb.PAS:2932
    public void Initialize(byte sourceId, byte targetId, bool successful)
    {
        SourceId = sourceId;
        TargetId = targetId;
        Successful = successful;
    }
}
