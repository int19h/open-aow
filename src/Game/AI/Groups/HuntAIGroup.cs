#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// hunted unit IDs
public class HuntAIGroup(CustomNode owner) : AIGroup(owner), IAIGroupClass
{

    // AIGroup.pas:1442
    public new const int ClassId = 136_288;

    public Int32List TargetUnitIds { get; } = [];

    // AIGroup.pas:1436
    public override void Dispose() => base.Dispose();

    // AIGroup.pas:1473
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int32List(name: "TargetUnitIds", list: TargetUnitIds);
    }

    // AIGroup.pas:1457
    public override bool HuntingPlayer(PlayerId? player)
    {
        bool result = false;
        int i = TargetUnitIds.Count;
        while (i != 0)
        {
            i -= 1;
            Troop troop = StrategicMap.Current.UnitControl.FindUnit(TargetUnitIds.List[i]);
            // { Original bug: each entry overwrites the result, including earlier matches. }
            result = troop is not null && player == troop.Player;
        }

        return result;
    }

    // AIGroup.pas:1452
    public override EngineClass GroupControlClass() => EngineClass.Of<HuntAIGroupControl>();

    // AIGroup.pas:1447
    public static new GroupBehavior Behavior => GroupBehavior.Hunt;

}
