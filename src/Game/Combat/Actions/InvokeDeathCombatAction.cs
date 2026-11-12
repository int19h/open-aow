// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (successful)
public partial class InvokeDeathCombatAction : SingleTargetCombatAction
{
    public new const int ClassId = 131780;

    public InvokeDeathCombatAction(ECustomNode owner) : base(owner)
    { }

    public bool Successful;

    // AoWAb.PAS:3400
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Boolean(ref Successful, 20);
    }

    // AoWAb.PAS:3420
    public override void Execute(Battle combat)
    {
        CombatObject source;
        CombatObject target;
        if (Successful)
        {
            source = combat.CombatData.FindId(SourceId);
            target = combat.CombatData.FindId(TargetId);
            source.DoDamage(target, target.GetHitPoints());
        }
    }

    // AoWAb.PAS:3412
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            GamePack.Current.AbilityControl.GetAbility(35).SfxLibrary.Play(0);
        }

        if (viewer.Delay == viewer.Time)
        {
            Execute(viewer.Combat);
        }

        return viewer.Delay * 2 > viewer.Time;
    }

    // AoWAb.PAS:3405
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (objectValue.Id == TargetId)
        {
            GamePack.Current.AbilityControl.GetAbility(35).ImageSequences.Show(0, X + 32, Y + 32, viewer.Time, surface);
        }
    }

    // AoWAb.PAS:3431
    public void Initialize(byte sourceId, byte targetId, bool successful)
    {
        SourceId = sourceId;
        TargetId = targetId;
        Successful = successful;
    }

    // intf_P3_02.inc:38

}
