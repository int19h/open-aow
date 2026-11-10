// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

public partial class HuntAGC : AIGroupControl
{
    public HuntAGC(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:1479
    public override void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
    {
        AoWMapField Field;
        ArmyHS Army;
        int I;
        HuntAG Group;
        bool Found;
        Field = (AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(Unsafe.As<XYL, int>(ref Target.FLocation));
        if (Field.GetPlayer() != FGroup.FPlayer)
        {
            Army = Field.GetArmyHS();
            if (Army != null)
            {
                Group = (HuntAG)FGroup;
                Found = false;
                I = Group.FTargetUnitIDs.Count;
                while (!Found && I != 0)
                {
                    I -= 1;
                    var item = Group.FTargetUnitIDs[I];
                    Found = Army.FArmy.IndexOfID(item) != -1;
                }

                if (Found)
                {
                    SetupCP(Target);
                    var execute = FCombatPredictor.Execute();
                    if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
                    {
                        if (FCombatPredictor.GetEfficiency() != 0)
                        {
                            Target.FData.Priority = 1000 - Target.FMovePoints;
                            base.TargetFoundEvent(Sender, Target, ref SearchDone);
                        }
                    }
                }
            }
        }
    }

    // AIGroup.pas:1515
    public override void ProcessStates()
    {
        AIGroupTargetFinder Finder;
        HuntAG Group;
        AbstractUnit AUnit;
        AIGroup NewGroup;
        switch (FState)
        {
            case 0:
                Group = (HuntAG)FGroup;
                {
                    for (int I = Group.FTargetUnitIDs.Count - 1; I >= 0; I--)
                    {
                        var item = Group.FTargetUnitIDs[I];
                        AUnit = AoWHSMap.Current.FUnitControl.FindUnit(item);
                        if (AUnit == null || AUnit.FPlayer == 0)
                        {
                            Group.FTargetUnitIDs.Delete(I);
                        }
                    }
                }

                if (Group.FTargetUnitIDs.Count != 0)
                {
                    Finder = new AIGroupTargetFinder(null);
                    Finder.Setup(this);
                    Finder.FSelector.FTargetInfo.Mode = AITargetMode.Hunt;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                }
                else
                {
                    NewGroup = AoWHSMap.Current.FAIGroupManager.CreateGroup(FGroup.FPlayer, FGroup.FIndependentRelation, GroupBehavior.Patrol);
                    NewGroup.AddUnits(FGroup);
                    NewGroup.Release();
                    Done();
                }

                break;
            case 1:
                if (FTargets.Count != 0)
                {
                    SetupME();
                    var targets = FTargets.GetTargets(0);
                    var fTargetsTargets = FTargets.GetTargets(0);
                    ExecuteME(FTargets.GetTargets(0).FLocation.X, fTargetsTargets.FLocation.Y, targets.FLocation.L);
                    FTargets.Delete(0);
                }
                else
                {
                    Done();
                }

                break;
        }
    }
}
