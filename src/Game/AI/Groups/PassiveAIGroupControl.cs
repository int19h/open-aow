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
        MapCoordinate Location = default;
        GroupDefenseSelector Selector;
        AIGroup Group;
        Troop AUnit;
        switch (FState)
        {
            case 0:
                CombinePassiveGroups();
                Location.Packed = FGroup.GetLoc();
                Selector = new GroupDefenseSelector();
                Selector.Execute(FGroup, Location.X, Location.Y, Location.L, null);
                if (Selector.FDefenders.Count != FGroup.Count && FGroup.FPlayer != 0)
                {
                    var randomGroupBehavior = AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer).FAIControl.GetRandomGroupBehavior();
                    Group = AoWHSMap.Current.FAIGroupManager.CreateGroup(FGroup.FPlayer, FGroup.FIndependentRelation, randomGroupBehavior);
                    for (int I = FGroup.Count - 1; I >= 0; I--)
                    {
                        AUnit = FGroup.GetUnit(I);
                        if (Selector.FDefenders.IndexOf(AUnit) == -1)
                        {
                            Group.AddUnit(AUnit);
                        }
                    }

                    Group.Release();
                }

                if (Selector.FDefenseLocation.Packed != 16777215)
                {
                    FGroup.FTargetXYL = Selector.FDefenseLocation;
                    FGroup.FTargetMode = (byte)AIGroupDefenseControl.GroupDefendMode.Custom;
                    SetupME();
                    FMoveExecuter.FMode = FMoveExecuter.FMode - Set<AIMoveExecutorModeElement>.Of(AIMoveExecutorModeElement.Adjacent);
                    ExecuteME(Selector.FDefenseLocation.X, Selector.FDefenseLocation.Y, Selector.FDefenseLocation.L);
                    Done();
                }
                else
                {
                    Done();
                }

                Selector?.Dispose();
                break;
        }
    }

    // MainAG.pas:176
    public void CombinePassiveGroups()
    {
        Player Player;
        MapCoordinate Location = default;
        MapCoordinate OtherLocation = default;
        AIGroup Group;
        Player = AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer);
        Location.Packed = FGroup.GetLoc();
        for (int I = Player.FAIGroups.Count - 1; I >= 0; I--)
        {
            Group = Player.FAIGroups.GetGroups(I);
            if (FGroup.FGroupID != Group.FGroupID && Group.EClass.ClassID == 136280 && FGroup.FIndependentRelation == Group.FIndependentRelation)
            {
                OtherLocation.Packed = Group.GetLoc();
                if (Location.L == OtherLocation.L && HexGeometry.dHXtoRad(Location.X, Location.Y, OtherLocation.X, OtherLocation.Y) <= 1)
                {
                    FGroup.AddUnits(Group);
                }
            }
        }
    }
}
