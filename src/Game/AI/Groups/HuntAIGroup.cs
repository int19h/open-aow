// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// hunted unit IDs
public partial class HuntAIGroup : AIGroup, IAIGroupClass
{
    // AIGroup.pas:1442
    public new const int ClassId = 136288;

    // AIGroup.pas:1430
    public HuntAIGroup(ECustomNode owner) : base(owner)
    {
        TargetUnitIds = new Int32List();
    }

    public Int32List TargetUnitIds;

    // AIGroup.pas:1436
    public override void Dispose()
    {
        TargetUnitIds?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:1473
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStreamProc(30, TargetUnitIds.LoadFromStream, TargetUnitIds.SaveToStream);
    }

    // AIGroup.pas:1457
    public bool HuntingPlayer(sbyte player)
    {
        int I;
        Troop troop;
        bool result = false;
        I = TargetUnitIds.Count;
        while (I != 0)
        {
            I -= 1;
            troop = AoWHSMap.Current.UnitControl.FindUnit(TargetUnitIds.List[I]);
            // { Original bug: each entry overwrites the result, including earlier matches. }
            result = troop != null && player == troop.Player;
        }

        return result;
    }

    // intf_P5_07.inc:660

    // AIGroup.pas:1452
    public override EngineClass GroupControlClass() => EngineClass.Of<HuntAIGroupControl>();

    // AIGroup.pas:1447
    public static new GroupBehavior Behavior => GroupBehavior.Hunt;

}
