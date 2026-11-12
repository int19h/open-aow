// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (successful)
public partial class EntangleCombatAction : SingleTargetCombatAction
{
    public new const int ClassId = 131774;

    public EntangleCombatAction(ECustomNode owner) : base(owner)
    { }

    public bool Successful;

    // AoWAb.PAS:2927
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwBool(ref Successful, 20);
    }

    // AoWAb.PAS:2938
    public override void Execute(Battle combat)
    {
        CombatUnit source;
        CombatUnit target;
        UnitEnchantmentAbilityData data;
        if (Successful)
        {
            source = (CombatUnit)combat.CombatData.FindId(SourceId);
            target = (CombatUnit)combat.CombatData.FindId(TargetId);
            AoWHSSet.Current.AbilityControl.GetAbility(94).Expand(target.Troop);
            data = (UnitEnchantmentAbilityData)target.Troop.GetAbilityData(94);
            if (data != null)
            {
                data.EnchantmentPlayerIndex = source.CombatPlayer;
            }
        }
    }

    // AoWAb.PAS:2960
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            AoWHSSet.Current.AbilityControl.GetAbility(40).SfxLibrary.Play(0);
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
            AoWHSSet.Current.AbilityControl.GetAbility(40).ImageSequences.Show(0, X + 32, Y + 32, viewer.Time, surface);
        }
    }

    // AoWAb.PAS:2932
    public void Initialize(byte sourceId, byte targetId, bool successful)
    {
        SourceId = sourceId;
        TargetId = targetId;
        Successful = successful;
    }

    // intf_P3_02.inc:38

}
