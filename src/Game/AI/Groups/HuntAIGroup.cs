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
    public new const int ClassID = 136288;

    // AIGroup.pas:1430
    public HuntAIGroup(ECustomNode owner) : base(owner)
    {
        FTargetUnitIDs = new Int32List();
    }

    public Int32List FTargetUnitIDs;

    // AIGroup.pas:1436
    public override void Dispose()
    {
        FTargetUnitIDs?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:1473
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(30, FTargetUnitIDs.LoadFromStream, FTargetUnitIDs.SaveToStream);
    }

    // AIGroup.pas:1457
    public bool HuntingPlayer(sbyte Player)
    {
        int I;
        Troop AUnit;
        bool Result = false;
        I = FTargetUnitIDs.Count;
        while (I != 0)
        {
            I -= 1;
            AUnit = AoWHSMap.Current.FUnitControl.FindUnit(FTargetUnitIDs.List[I]);
            // { Original bug: each entry overwrites the result, including earlier matches. }
            Result = AUnit != null && Player == AUnit.FPlayer;
        }

        return Result;
    }

    // intf_P5_07.inc:660

    // AIGroup.pas:1452
    public override EngineClass GroupControlClass() => EngineClass.Of<HuntAIGroupControl>();

    // AIGroup.pas:1447
    public static new GroupBehavior Behavior => GroupBehavior.Hunt;

}
