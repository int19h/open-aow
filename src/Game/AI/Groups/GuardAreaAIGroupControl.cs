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
    public override void MoveExecutorDone(object sender)
    {
        switch (State)
        {
            case 1:
                if (MoveExecutor.Status == AIMoveExecutorStatus.OutOfMovePoints)
                {
                    Done();
                }
                else
                {
                    if (MoveExecutor.Status == AIMoveExecutorStatus.Invalidated)
                    {
                        Done();
                    }
                }

                break;
            case 2:
                if (MoveExecutor.Status == AIMoveExecutorStatus.Failed)
                {
                    ((GuardAreaAIGroup)Group).GuardLocation.Packed = 16777215;
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
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        SetupCP(target);
        // { Original also accepts crDefeat when the subsequent efficiency tests pass. }
        var execute = CombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
        {
            if (CombatPredictor.GetEfficiency() > 0)
            {
                var efficiency = CombatPredictor.GetEfficiency();
                target.Data.Priority = efficiency + (target.Data.Priority * 100 - target.MovePoints * 5);
                base.TargetFoundEvent(sender, target, ref searchDone);
            }
        }
    }

    // IndependentAG.pas:205
    public override void ProcessStates()
    {
        MapField mapField;
        AIGroupTargetFinder finder;
        MoveControlSettings settings;
        switch (State)
        {
            case 0:
                if (((GuardAreaAIGroup)Group).GuardLocation.X == -1)
                {
                    ((GuardAreaAIGroup)Group).GuardLocation = Location;
                }

                finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                finder.Setup(this);
                {
                    finder.Selector.SourceLocation = ((GuardAreaAIGroup)Group).GuardLocation;
                    finder.Selector.TargetInfo.Mode = AITargetMode.GuardArea;
                }

                var moves = Army.Moves(Selection);
                finder.Selector.MovePointsMax = moves;
                ActivatePlugin(finder);
                finder.Release();
                State += 1;
                break;
            case 1:
                Targets.Validate();
                if (Targets.Count != 0)
                {
                    if (Army.Moves(Selection) != Army.MovePoints(Selection) || ((GuardAreaAIGroup)Group).GuardLocation.Packed != Location.Packed)
                    {
                        settings = new MoveControlSettings(null) { Holder = CreationHolder() };
                        settings.Player = Army.Player;
                        settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.AI);
                        settings.SetSourcePosition(Army.X, Army.Y, Army.MapLevel);
                        settings.MovePointsMax = Army.MovePoints(Selection);
                        settings.ArmyHexSprite = ArmyHexSprite;
                        settings.Selection = Selection;
                        settings.MovePointTable = MovePointTable;
                        StrategicMap.Current.MoveControl.ProcessMove(settings);
                        for (int I = Targets.Count - 1; I >= 0; I--)
                        {
                            if (settings.GetMovePointsMapCoordinate(Targets.GetTargets(I).Location.Packed) == 16777215)
                            {
                                Targets.Delete(I);
                            }
                        }

                        settings.Release();
                    }

                    if (Targets.Count != 0)
                    {
                        Targets.SortOnPriority();
                        SetupME();
                        var targetLocation = Targets.GetTargets(0);
                        var target = Targets.GetTargets(0);
                        ExecuteME(Targets.GetTargets(0).Location.X, target.Location.Y, targetLocation.Location.MapLevel);
                        Targets.Delete(0);
                    }
                    else
                    {
                        State = 2;
                    }
                }
                else
                {
                    State = 2;
                }

                break;
            case 2:
                if (((GuardAreaAIGroup)Group).GuardLocation.Packed != Location.Packed)
                {
                    mapField = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(((GuardAreaAIGroup)Group).GuardLocation.Packed);
                    if (mapField.GetPlayer() == -1 || mapField.GetPlayer() == Group.Player)
                    {
                        SetupME();
                        var guardAreaAIGroup = (GuardAreaAIGroup)Group;
                        ExecuteME(guardAreaAIGroup.GuardLocation.X, guardAreaAIGroup.GuardLocation.Y, guardAreaAIGroup.GuardLocation.MapLevel);
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
