// Translated from IndependentAG.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class PatrolAIGroupControl : AIGroupControl
{
    // Heading 0 is the centre, 1 to 6 the hex sides (fixed-arrays.md, named constant).
    public const int HexDirectionSlotCount = 7;

    public PatrolAIGroupControl(ECustomNode owner) : base(owner)
    { }

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
                    ((PatrolAIGroup)Group).PatrolTo.Packed = 16777215;
                }
                else
                {
                    Done();
                }

                break;
        }
    }

    // IndependentAG.pas:426
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        MapField mapField;
        mapField = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(target.Location.Packed);
        if (mapField.GetPlayer() != Group.Player)
        {
            SetupCP(target);
            var execute = CombatPredictor.Execute();
            if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
            {
                if (CombatPredictor.GetSuperiority() > 80)
                {
                    if (CombatPredictor.GetEfficiency() > 50)
                    {
                        var efficiency = CombatPredictor.GetEfficiency();
                        target.Data.Priority = efficiency + target.Data.Priority * 100;
                        base.TargetFoundEvent(sender, target, ref searchDone);
                    }
                }
            }
        }
    }

    // IndependentAG.pas:444
    public override void ProcessStates()
    {
        AIGroupDefenseControl defense;
        AIRecruitmentGroupFinder recruitment;
        AIGroupRecruitControl recruit;
        AIGroupTargetFinder finder;
        MoveControlSettings settings;
        switch (State)
        {
            case 0:
                if (((PatrolAIGroup)Group).PatrolStrength == 0)
                {
                    ((PatrolAIGroup)Group).PatrolStrength = StrengthMax;
                }

                defense = new AIGroupDefenseControl(null);
                defense.DefendMode = AIGroupDefenseControl.GroupDefendMode.Wait;
                ActivatePlugin(defense);
                defense.Release();
                State = 1;
                break;
            case 1:
                if (((PatrolAIGroup)Group).PatrolTo.X == -1 || ((PatrolAIGroup)Group).PatrolTo.MapLevel == Location.MapLevel && HexGeometry.DeltaHexToRadius(((PatrolAIGroup)Group).PatrolTo.X, ((PatrolAIGroup)Group).PatrolTo.Y, Location.X, Location.Y) <= 2)
                {
                    ResetPatrolToLocation();
                }

                State += 1;
                break;
            case 2:
                if (StrengthMax < ((PatrolAIGroup)Group).PatrolStrength)
                {
                    State += 1;
                    recruitment = new AIRecruitmentGroupFinder(null);
                    recruitment.Setup(this);
                    ActivatePlugin(recruitment);
                    recruitment.Release();
                }
                else
                {
                    State = 4;
                }

                break;
            case 3:
                if (RecruitmentGroups.Count != 0 && StrengthMax < ((PatrolAIGroup)Group).PatrolStrength)
                {
                    recruit = new AIGroupRecruitControl(null);
                    recruit.Reset();
                    recruit.RecruitStrength = ((PatrolAIGroup)Group).PatrolStrength - StrengthMax;
                    recruit.Mode = AIGroupRecruitControlMode.AutoRecruit;
                    ActivatePlugin(recruit);
                    recruit.Release();
                }

                State += 1;
                break;
            case 4:
                finder = new AIGroupTargetFinder(null);
                finder.Setup(this);
                var moves = Army.Moves(Selection);
                finder.Selector.MovePointsMax = moves;
                ActivatePlugin(finder);
                finder.Release();
                State += 1;
                break;
            case 5:
                Targets.Validate();
                if (Targets.Count != 0)
                {
                    SetupME();
                    var targetLocation = Targets.GetTargets(0);
                    var target = Targets.GetTargets(0);
                    ExecuteME(Targets.GetTargets(0).Location.X, target.Location.Y, targetLocation.Location.MapLevel);
                    Targets.Delete(0);
                }
                else
                {
                    State += 1;
                }

                break;
            case 6:
                if (StrategicMap.Current.Random(3) != 0 && ((PatrolAIGroup)Group).PatrolTo.X != -1)
                {
                    State += 1;
                    SetupME();
                    ExecuteME(((PatrolAIGroup)Group).PatrolTo.X, ((PatrolAIGroup)Group).PatrolTo.Y, ((PatrolAIGroup)Group).PatrolTo.MapLevel);
                }
                else
                {
                    Done();
                }

                break;
            case 7:
                settings = new MoveControlSettings(null);
                settings.Player = Group.Player;
                settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.AI);
                settings.SetSourcePosition(Location.X, Location.Y, Location.MapLevel);
                settings.SetDestinationPosition(((PatrolAIGroup)Group).PatrolFrom.X, ((PatrolAIGroup)Group).PatrolFrom.Y, ((PatrolAIGroup)Group).PatrolFrom.MapLevel);
                settings.MovePointsMax = 48;
                settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.IgnoreArmies);
                settings.ArmyHexSprite = ArmyHexSprite;
                settings.Selection = Selection;
                settings.MovePointTable = MovePointTable;
                StrategicMap.Current.MoveControl.ProcessMove(settings);
                if (settings.Path.Count == 0)
                {
                    ((PatrolAIGroup)Group).PatrolFrom.Packed = 16777215;
                }

                settings.Release();
                Done();
                break;
        }
    }

    // IndependentAG.pas:324
    public bool ResetPatrolToLocation()
    {
        bool result = default;
        int previousHeading = default;
        int X = default;
        int Y = default;
        DoubleItemList<MapField> newList = default;
        PatrolAIGroup group = default;
        MoveControlSettings settings = default;
        DelphiList<MapField> fields = default;
        var lists = new DoubleItemList<MapField>[HexDirectionSlotCount];
        MapFieldMessage message = default;
        int J = default;
        int heading = default;
        int lastHeading = default;
        MapField mapField = default;
        ByteList choices = default;
        DoubleItemList<MapField> selectedList = default;
        group = (PatrolAIGroup)Group;
        if (group.PatrolFrom.X == -1)
        {
            group.PatrolFrom = Location;
        }

        settings = new MoveControlSettings(null);
        settings.Player = group.Player;
        settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.AI, MoveModeElement.IgnoreArmies);
        settings.SetSourcePosition(group.PatrolFrom.X, group.PatrolFrom.Y, group.PatrolFrom.MapLevel);
        settings.MovePointsMax = 48;
        settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
        settings.ArmyHexSprite = ArmyHexSprite;
        settings.Selection = Selection;
        settings.MovePointTable = MovePointTable;
        StrategicMap.Current.MoveControl.ProcessMove(settings);
        fields = settings.ProcessedMapFields;
        for (int I = 0; I < HexDirectionSlotCount; I++)
        {
            var doubleItemList = new DoubleItemList<MapField>();
            lists[I] = doubleItemList;
        }

        X = group.PatrolFrom.X;
        Y = group.PatrolFrom.Y;
        for (int I = fields.Count - 1; I >= 0; I--)
        {
            // { Original samples every fourth processed field. }
            if ((I & 3) == 3)
            {
                mapField = fields.List[I];
                heading = HexGeometry.DeltaHexToHexDirection(X, Y, mapField.HexX, mapField.HexY);
                var movePoints = settings.GetMovePoints(mapField.HexX, mapField.HexY, mapField.HexLevel);
                lists[heading].AddAbility(mapField, movePoints);
            }
        }

        for (int I = 0; I < HexDirectionSlotCount; I++)
        {
            lists[I].Sort(ComparePatrolFields);
        }

        message.MessageId = 4384;
        message.Param[0] = ArmyHexSprite;
        for (int I = 1; I < HexDirectionSlotCount; I++)
        {
            newList = new DoubleItemList<MapField>();
            J = 0;
            while (newList.Count != 30 && J != lists[I].Count)
            {
                mapField = lists[I].List[J].First;
                if (mapField.GetPlayer() == -1)
                {
                    message.Result = 0;
                    mapField.SendMapFieldMessage(ref message);
                    if (message.Result.Int == 0)
                    {
                        var item = lists[I][J];
                        newList.Add(item);
                    }
                }

                J += 1;
            }

            lists[I]?.Dispose();
            lists[I] = newList;
        }

        mapField = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(group.PatrolFrom.Packed);
        if (mapField.GetPlayer() == -1)
        {
            lists[0].AddAbility(mapField, 0);
        }

        if (group.PatrolTo.X != -1)
        {
            lastHeading = HexGeometry.DeltaHexToHexDirection(group.PatrolFrom.X, group.PatrolFrom.Y, group.PatrolTo.X, group.PatrolTo.Y);
        }
        else
        {
            lastHeading = 0;
        }

        previousHeading = group.PreviousHeading;
        choices = new ByteList();
        for (int I = 0; I < HexDirectionSlotCount; I++)
        {
            if (lists[I].Count != 0 && lastHeading != I && I != previousHeading)
            {
                if (previousHeading == 0 || PatrolHeadingDistance(I, lastHeading) > 1 || PatrolHeadingDistance(I, previousHeading) > 1)
                {
                    choices.Add((byte)I);
                }
            }
        }

        if (choices.Count == 0)
        {
            for (int I = 0; I < HexDirectionSlotCount; I++)
            {
                if (lists[I].Count != 0 && lastHeading != I)
                {
                    choices.Add((byte)I);
                }
            }
        }

        if (choices.Count != 0)
        {
            group.PreviousHeading = (sbyte)lastHeading;
            heading = choices[StrategicMap.Current.Random(choices.Count)];
            selectedList = lists[heading];
            var random = StrategicMap.Current.Random(selectedList.Count);
            mapField = selectedList.List[random].First;
            var mapCoordinate = mapField.GetMapCoordinate();
            group.PatrolTo.Packed = mapCoordinate;
            result = true;
        }
        else
        {
            result = false;
        }

        for (int I = 0; I < HexDirectionSlotCount; I++)
        {
            lists[I]?.Dispose();
        }

        choices?.Dispose();
        settings.Release();
        return result;
    }

    private static int ComparePatrolFields(in DoubleItem<MapField> A, in DoubleItem<MapField> B)
    {
        return B.Second - A.Second;
    }

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
