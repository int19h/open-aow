// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (successful)
public partial class WebCombatAction : SingleTargetCombatAction
{
    public new const int ClassId = 131768;

    public WebCombatAction(ECustomNode owner) : base(owner)
    { }

    public bool Successful;

    // AoWAb.PAS:2765
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Boolean(ref Successful, 20);
    }

    // AoWAb.PAS:2778
    public override void Execute(Battle combat)
    {
        CombatUnit target;
        if (Successful)
        {
            target = (CombatUnit)combat.CombatData.FindId(TargetId);
            GamePack.Current.AbilityControl.GetAbility(97).Expand(target.Troop);
        }
    }

    // AoWAb.PAS:2797
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            GamePack.Current.AbilityControl.GetAbility(27).SfxLibrary.Play(0);
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
            GamePack.Current.AbilityControl.GetAbility(27).ImageSequences.Show(0, X + 32, Y + 32, viewer.Time, surface);
        }
    }

    // AoWAb.PAS:2771
    public void Initialize(byte sourceId, byte targetId, bool successful)
    {
        SourceId = sourceId;
        TargetId = targetId;
        Successful = successful;
    }

    // intf_P3_02.inc:38

}
