// Translated from MainAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class PassiveAIGroupControl : AIGroupControl
{
    public PassiveAIGroupControl(ECustomNode owner) : base(owner)
    { }

    // MainAG.pas:199
    public override void ProcessStates()
    {
        MapCoordinate location = default;
        GroupDefenseSelector selector;
        AIGroup group;
        Troop troop;
        switch (State)
        {
            case 0:
                CombinePassiveGroups();
                location.Packed = Group.GetLocation();
                selector = new GroupDefenseSelector();
                selector.Execute(Group, location.X, location.Y, location.MapLevel, null);
                if (selector.Defenders.Count != Group.Count && Group.Player != 0)
                {
                    var randomGroupBehavior = AoWHSMap.Current.Players.GetPlayers(Group.Player).AIControl.GetRandomGroupBehavior();
                    group = AoWHSMap.Current.AIGroupManager.CreateGroup(Group.Player, Group.IndependentRelation, randomGroupBehavior);
                    for (int I = Group.Count - 1; I >= 0; I--)
                    {
                        troop = Group.GetUnit(I);
                        if (selector.Defenders.IndexOf(troop) == -1)
                        {
                            group.AddUnit(troop);
                        }
                    }

                    group.Release();
                }

                if (selector.DefenseLocation.Packed != 16777215)
                {
                    Group.TargetMapCoordinate = selector.DefenseLocation;
                    Group.TargetMode = (byte)AIGroupDefenseControl.GroupDefendMode.Custom;
                    SetupME();
                    MoveExecutor.Mode = MoveExecutor.Mode - Set<AIMoveExecutorModeElement>.Of(AIMoveExecutorModeElement.Adjacent);
                    ExecuteME(selector.DefenseLocation.X, selector.DefenseLocation.Y, selector.DefenseLocation.MapLevel);
                    Done();
                }
                else
                {
                    Done();
                }

                selector?.Dispose();
                break;
        }
    }

    // MainAG.pas:176
    public void CombinePassiveGroups()
    {
        Player player;
        MapCoordinate location = default;
        MapCoordinate otherLocation = default;
        AIGroup group;
        player = AoWHSMap.Current.Players.GetPlayers(Group.Player);
        location.Packed = Group.GetLocation();
        for (int I = player.AIGroups.Count - 1; I >= 0; I--)
        {
            group = player.AIGroups.GetGroups(I);
            if (Group.GroupId != group.GroupId && group.EngineClass.ClassId == 136280 && Group.IndependentRelation == group.IndependentRelation)
            {
                otherLocation.Packed = group.GetLocation();
                if (location.MapLevel == otherLocation.MapLevel && HexGeometry.DeltaHexToRadius(location.X, location.Y, otherLocation.X, otherLocation.Y) <= 1)
                {
                    Group.AddUnits(group);
                }
            }
        }
    }
}
