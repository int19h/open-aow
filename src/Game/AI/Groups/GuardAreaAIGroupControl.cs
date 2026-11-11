// Translated from IndependentAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class GuardAreaAIGroupControl : AIGroupControl
{
    public GuardAreaAIGroupControl(ECustomNode owner) : base(owner)
    { }

    // IndependentAG.pas:189
    public override void MoveExecuterDone(object Sender)
    {
        switch (FState)
        {
            case 1:
                if (FMoveExecuter.FStatus == AIMoveExecutor.Status.OutOfMovePoints)
                {
                    Done();
                }
                else
                {
                    if (FMoveExecuter.FStatus == AIMoveExecutor.Status.Invalidated)
                    {
                        Done();
                    }
                }

                break;
            case 2:
                if (FMoveExecuter.FStatus == AIMoveExecutor.Status.Failed)
                {
                    ((GuardAreaAIGroup)FGroup).FGuardLocation.Packed = 16777215;
                    Done();
                }
                else
                {
                    Done();
                }

                break;
        }
    }

    // IndependentAG.pas:176
    public override void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
    {
        SetupCP(Target);
        // { Original also accepts crDefeat when the subsequent efficiency tests pass. }
        var execute = FCombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
        {
            if (FCombatPredictor.GetEfficiency() > 0)
            {
                var efficiency = FCombatPredictor.GetEfficiency();
                Target.FData.Priority = efficiency + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                base.TargetFoundEvent(Sender, Target, ref SearchDone);
            }
        }
    }

    // IndependentAG.pas:205
    public override void ProcessStates()
    {
        AoWMapField MapF;
        AIGroupTargetFinder Finder;
        MoveControlSettings Settings;
        switch (FState)
        {
            case 0:
                if (((GuardAreaAIGroup)FGroup).FGuardLocation.X == -1)
                {
                    ((GuardAreaAIGroup)FGroup).FGuardLocation = FLocation;
                }

                Finder = new AIGroupTargetFinder(null);
                Finder.Setup(this);
                {
                    Finder.FSelector.FSourceLocation = ((GuardAreaAIGroup)FGroup).FGuardLocation;
                    Finder.FSelector.FTargetInfo.Mode = AITargetMode.GuardArea;
                }

                var moves = FArmy.Moves(FSelection);
                Finder.FSelector.FMovePointsMax = moves;
                ActivatePlugin(Finder);
                Finder.Release();
                FState += 1;
                break;
            case 1:
                FTargets.Validate();
                if (FTargets.Count != 0)
                {
                    if (FArmy.Moves(FSelection) != FArmy.MovePoints(FSelection) || ((GuardAreaAIGroup)FGroup).FGuardLocation.Packed != FLocation.Packed)
                    {
                        Settings = new MoveControlSettings(null);
                        Settings.FPlayer = FArmy.FPlayer;
                        Settings.FMode = Settings.FMode + Set<MoveModeElement>.Of(MoveModeElement.AI);
                        Settings.SetSourcePos(FArmy.FX, FArmy.FY, FArmy.FL);
                        Settings.FMovePointsMax = FArmy.MovePoints(FSelection);
                        Settings.FArmyHS = FArmyHS;
                        Settings.FSelection = FSelection;
                        Settings.FMovePointTable = FMovePointTable;
                        AoWHSMap.Current.FMoveControl.ProcessMove(Settings);
                        for (int I = FTargets.Count - 1; I >= 0; I--)
                        {
                            if (Settings.GetMovePointsXYL(FTargets.GetTargets(I).FLocation.Packed) == 16777215)
                            {
                                FTargets.Delete(I);
                            }
                        }

                        Settings.Release();
                    }

                    if (FTargets.Count != 0)
                    {
                        FTargets.SortOnPriority();
                        SetupME();
                        var targetLocation = FTargets.GetTargets(0);
                        var target = FTargets.GetTargets(0);
                        ExecuteME(FTargets.GetTargets(0).FLocation.X, target.FLocation.Y, targetLocation.FLocation.L);
                        FTargets.Delete(0);
                    }
                    else
                    {
                        FState = 2;
                    }
                }
                else
                {
                    FState = 2;
                }

                break;
            case 2:
                if (((GuardAreaAIGroup)FGroup).FGuardLocation.Packed != FLocation.Packed)
                {
                    MapF = (AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(((GuardAreaAIGroup)FGroup).FGuardLocation.Packed);
                    if (MapF.GetPlayer() == -1 || MapF.GetPlayer() == FGroup.FPlayer)
                    {
                        SetupME();
                        var guardAreaAG = (GuardAreaAIGroup)FGroup;
                        ExecuteME(guardAreaAG.FGuardLocation.X, guardAreaAG.FGuardLocation.Y, guardAreaAG.FGuardLocation.L);
                    }
                    else
                    {
                        Done();
                    }
                }
                else
                {
                    Done();
                }

                break;
        }
    }

    // IndependentAG.pas:280
    public override void ProcessStatesLooped()
    {
        ProcessStates();
    }
}
