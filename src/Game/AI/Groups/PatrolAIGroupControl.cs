#nullable disable warnings
// Translated from IndependentAG.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public class PatrolAIGroupControl(CustomNode owner) : AIGroupControl(owner)
{
    // Heading 0 is the centre, 1 to 6 the hex sides (fixed-arrays.md, named constant).
    public const int HexDirectionSlotCount = 7;

    public new PatrolAIGroup Group => GetGroup<PatrolAIGroup>();

    // IndependentAG.pas:415
    public override void MoveExecutorDone(object sender)
    {
        switch (MoveExecutor.Status)
        {
            case AIMoveExecutorStatus.Done:
                Done();
                break;
            case AIMoveExecutorStatus.Failed:
                if (State == 7)
                {
                    Group.PatrolTo = MapCoordinate.None;
                }
                else
                {
                    Done();
                }

                break;
            case AIMoveExecutorStatus.Idle:
            case AIMoveExecutorStatus.Busy:
            case AIMoveExecutorStatus.OutOfMovePoints:
            case AIMoveExecutorStatus.Invalidated:
            default:
                break;
        }
    }

    // IndependentAG.pas:426
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        MapField mapField = GetEngine().StrategicMap.MapContainer.GetFieldMapCoordinate(target.Location);
        if (mapField.GetPlayer() != Group.Player)
        {
            SetupCP(target);
            CombatResult execute = CombatPredictor.Execute();
            if (execute is CombatResult.Victory or CombatResult.Defeat or CombatResult.NoDefenders)
            {
                if (CombatPredictor.GetSuperiority() > 80)
                {
                    if (CombatPredictor.GetEfficiency() > 50)
                    {
                        int efficiency = CombatPredictor.GetEfficiency();
                        AITargetData data = target.Data;
                        data.Priority = efficiency + (data.Priority * 100);
                        target.Data = data;
                        base.TargetFoundEvent(sender, target, ref searchDone);
                    }
                }
            }
        }
    }

    // IndependentAG.pas:444
    public override void ProcessStates()
    {
        switch (State)
        {
            case 0:
                if (Group.PatrolStrength == 0)
                {
                    Group.PatrolStrength = StrengthMax;
                }

                AIGroupDefenseControl defense = new(null)
                {
                    Holder = CreationHolder(),
                    DefendMode = AIGroupDefenseControl.GroupDefendMode.Wait
                };
                ActivatePlugin(defense);
                _ = defense.Release();
                State = 1;
                break;
            case 1:
                if (Group.PatrolTo.X == -1 || (Group.PatrolTo.MapLevel == Location.MapLevel && HexGeometry.DeltaHexToRadius(Group.PatrolTo.X, Group.PatrolTo.Y, Location.X, Location.Y) <= 2))
                {
                    _ = ResetPatrolToLocation();
                }

                State += 1;
                break;
            case 2:
                if (StrengthMax < Group.PatrolStrength)
                {
                    State += 1;
                    AIRecruitmentGroupFinder recruitment = new(null) { Holder = CreationHolder() };
                    _ = recruitment.Setup(this);
                    ActivatePlugin(recruitment);
                    _ = recruitment.Release();
                }
                else
                {
                    State = 4;
                }

                break;
            case 3:
                if (RecruitmentGroups.Count != 0 && StrengthMax < Group.PatrolStrength)
                {
                    AIGroupRecruitControl recruit = new(null) { Holder = CreationHolder() };
                    recruit.Reset();
                    recruit.RecruitStrength = Group.PatrolStrength - StrengthMax;
                    recruit.Mode = AIGroupRecruitControlMode.AutoRecruit;
                    ActivatePlugin(recruit);
                    _ = recruit.Release();
                }

                State += 1;
                break;
            case 4:
                AIGroupTargetFinder finder = new(null) { Holder = CreationHolder() };
                _ = finder.Setup(this);
                int moves = Army.Moves(Selection);
                finder.Selector.MovePointsMax = moves;
                ActivatePlugin(finder);
                _ = finder.Release();
                State += 1;
                break;
            case 5:
                _ = Targets.Validate();
                if (Targets.Count != 0)
                {
                    SetupME();
                    AIGroupControlTarget targetLocation = Targets.GetTargets(0);
                    AIGroupControlTarget target = Targets.GetTargets(0);
                    ExecuteME(Targets.GetTargets(0).Location.X, target.Location.Y, L: targetLocation.Location.MapLevel);
                    Targets.Delete(0);
                }
                else
                {
                    State += 1;
                }

                break;
            case 6:
                if (GetEngine().StrategicMap.Random(3) != 0 && Group.PatrolTo.X != -1)
                {
                    State += 1;
                    SetupME();
                    ExecuteME(Group.PatrolTo.X, Group.PatrolTo.Y, L: Group.PatrolTo.MapLevel);
                }
                else
                {
                    Done();
                }

                break;
            case 7:
                MoveControlSettings settings = new(null)
                {
                    Holder = CreationHolder(),
                    Player = Group.Player
                };
                settings.Mode |= MoveModes.AI;
                settings.SetSourcePosition(Location);
                settings.SetDestinationPosition(Group.PatrolFrom);
                settings.MovePointsMax = 48;
                settings.Mode |= MoveModes.IgnoreArmies;
                settings.ArmyHexSprite = ArmyHexSprite;
                settings.Selection = Selection;
                settings.MovePointTable = MovePointTable;
                GetEngine().StrategicMap.MoveControl.ProcessMove(settings);
                if (settings.Path.Count == 0)
                {
                    Group.PatrolFrom = MapCoordinate.None;
                }

                _ = settings.Release();
                Done();
                break;
            default:
                break;
        }
    }

    // IndependentAG.pas:324
    public bool ResetPatrolToLocation()
    {
        DoubleItemList<MapField>[] lists = new DoubleItemList<MapField>[HexDirectionSlotCount];
        MapFieldMessage message = default;
        PatrolAIGroup group = Group;
        if (group.PatrolFrom.X == -1)
        {
            group.PatrolFrom = Location;
        }

        MoveControlSettings settings = new(null)
        {
            Holder = CreationHolder(),
            Player = group.Player
        };
        settings.Mode |= MoveModes.AI | MoveModes.IgnoreArmies;
        settings.SetSourcePosition(group.PatrolFrom);
        settings.MovePointsMax = 48;
        settings.Mode |= MoveModes.LogProcessedMapFields;
        settings.ArmyHexSprite = ArmyHexSprite;
        settings.Selection = Selection;
        settings.MovePointTable = MovePointTable;
        GetEngine().StrategicMap.MoveControl.ProcessMove(settings);
        DelphiList<MapField> fields = settings.ProcessedMapFields;
        for (int i = 0; i < HexDirectionSlotCount; i++)
        {
            DoubleItemList<MapField> doubleItemList = [];
            lists[i] = doubleItemList;
        }

        int x = group.PatrolFrom.X;
        int y = group.PatrolFrom.Y;
        int heading;
        MapField? mapField;
        for (int i = fields.Count - 1; i >= 0; i--)
        {
            // { Original samples every fourth processed field. }
            if ((i & 0b0011) == 0b0011)
            {
                mapField = fields.List[i];
                heading = HexGeometry.DeltaHexToHexDirection(x, y, mapField.HexX, mapField.HexY);
                int movePoints = settings.GetMovePoints(mapField.HexX, mapField.HexY, mapField.HexLevel);
                _ = lists[heading].AddAbility(mapField, movePoints);
            }
        }

        for (int i = 0; i < HexDirectionSlotCount; i++)
        {
            lists[i].Sort(ComparePatrolFields);
        }

        message.MessageId = 4384;
        MapFieldMessageParameters param = message.Param;
        param[0] = ArmyHexSprite;
        message.Param = param;
        for (int i = 1; i < HexDirectionSlotCount; i++)
        {
            DoubleItemList<MapField> newList = [];
            int j = 0;
            while (newList.Count != 30 && j != lists[i].Count)
            {
                mapField = lists[i].List[j].First;
                if (mapField.GetPlayer() is null)
                {
                    message.Result = 0;
                    mapField.SendMapFieldMessage(ref message);
                    if (message.Result.Int == 0)
                    {
                        DoubleItem<MapField> item = lists[i][j];
                        _ = newList.Add(item);
                    }
                }

                j += 1;
            }

            lists[i]?.Dispose();
            lists[i] = newList;
        }

        mapField = GetEngine().StrategicMap.MapContainer.GetFieldMapCoordinate(group.PatrolFrom);
        if (mapField.GetPlayer() is null)
        {
            _ = lists[0].AddAbility(mapField, 0);
        }

        int lastHeading = group.PatrolTo.X != -1
            ? HexGeometry.DeltaHexToHexDirection(group.PatrolFrom.X, group.PatrolFrom.Y, group.PatrolTo.X, group.PatrolTo.Y)
            : 0;

        int previousHeading = group.PreviousHeading;
        ByteList choices = [];
        for (int i = 0; i < HexDirectionSlotCount; i++)
        {
            if (lists[i].Count != 0 && lastHeading != i && i != previousHeading)
            {
                if (previousHeading == 0 || PatrolHeadingDistance(i, lastHeading) > 1 || PatrolHeadingDistance(i, previousHeading) > 1)
                {
                    _ = choices.Add(i.TruncateToByte());
                }
            }
        }

        if (choices.Count == 0)
        {
            for (int i = 0; i < HexDirectionSlotCount; i++)
            {
                if (lists[i].Count != 0 && lastHeading != i)
                {
                    _ = choices.Add(i.TruncateToByte());
                }
            }
        }

        bool result;
        if (choices.Count != 0)
        {
            group.PreviousHeading = (sbyte)lastHeading;
            heading = choices[GetEngine().StrategicMap.Random(choices.Count)];
            DoubleItemList<MapField> selectedList = lists[heading];
            int random = GetEngine().StrategicMap.Random(selectedList.Count);
            mapField = selectedList.List[random].First;
            MapCoordinate mapCoordinate = mapField.MapCoordinate;
            group.PatrolTo = mapCoordinate;
            result = true;
        }
        else
        {
            result = false;
        }

        for (int i = 0; i < HexDirectionSlotCount; i++)
        {
            lists[i]?.Dispose();
        }
        _ = settings.Release();
        return result;
    }

    private static int ComparePatrolFields(in DoubleItem<MapField> A, in DoubleItem<MapField> B) => B.Second - A.Second;

    private static int PatrolHeadingDistance(int A, int B)
    {
        int number = Math.Abs(B - A);
        if (number > 3)
        {
            number = 6 - number;
        }
        return number;
    }
}
