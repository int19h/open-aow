// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (successful)
public class WebCombatAction(CustomNode owner) : SingleTargetCombatAction(owner)
{
    public new const int ClassId = 131768;

    public bool Successful { get; private set; }

    public override bool TouchSucceeded => Successful;

    // AoWAb.PAS:2765
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Successful = persister.Boolean("Successful", value: Successful);
    }

    // AoWAb.PAS:2778
    public override void Execute(Battle combat)
    {
        if (Successful)
        {
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            _ = GamePack.Current.AbilityControl[97].Expand(target.Troop);
        }
    }

    // AoWAb.PAS:2797
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            _ = GamePack.Current.AbilityControl[27].SfxLibrary.Play(0);
        }

        if (viewer.Delay == viewer.Time)
        {
            Execute(viewer.Combat);
        }

        return viewer.Delay * 2 > viewer.Time;
    }

    // AoWAb.PAS:2789
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (objectValue.Id == TargetId)
        {
            _ = GamePack.Current.AbilityControl[27].ImageSequences.Show(0, X + 32, Y + 32, viewer.Time, surface);
        }
    }

    // AoWAb.PAS:2771
    public void Initialize(byte sourceId, byte targetId, bool successful)
    {
        SourceId = sourceId;
        TargetId = targetId;
        Successful = successful;
    }
}
