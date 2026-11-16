#nullable disable
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

    public PatrolAIGroupControl(CustomNode owner) : base(owner)
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
                    ((PatrolAIGroup)Group).PatrolTo = MapCoordinate.None;
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
        mapField = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(target.Location);
        if (mapField.GetPlayer() != Group.Player)
        {
            SetupCP(target);
            CombatResult execute = CombatPredictor.Execute();
            if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
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

                defense = new AIGroupDefenseControl(null) { Holder = CreationHolder() };
                defense.DefendMode = AIGroupDefenseControl.GroupDefendMode.Wait;
                ActivatePlugin(defense);
                defense.Release();
                State = 1;
                break;
            case 1:
                if (((PatrolAIGroup)Group).PatrolTo.X == -1 || (((PatrolAIGroup)Group).PatrolTo.MapLevel == Location.MapLevel && HexGeometry.DeltaHexToRadius(((PatrolAIGroup)Group).PatrolTo.X, ((PatrolAIGroup)Group).PatrolTo.Y, Location.X, Location.Y) <= 2))
                {
                    ResetPatrolToLocation();
                }

                State += 1;
                break;
            case 2:
                if (StrengthMax < ((PatrolAIGroup)Group).PatrolStrength)
                {
                    State += 1;
                    recruitment = new AIRecruitmentGroupFinder(null) { Holder = CreationHolder() };
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
                    recruit = new AIGroupRecruitControl(null) { Holder = CreationHolder() };
                    recruit.Reset();
                    recruit.RecruitStrength = ((PatrolAIGroup)Group).PatrolStrength - StrengthMax;
                    recruit.Mode = AIGroupRecruitControlMode.AutoRecruit;
                    ActivatePlugin(recruit);
                    recruit.Release();
                }

                State += 1;
                break;
            case 4:
                finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                finder.Setup(this);
                sbyte moves = Army.Moves(Selection);
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
                    AIGroupControlTarget targetLocation = Targets.GetTargets(0);
                    AIGroupControlTarget target = Targets.GetTargets(0);
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
                settings = new MoveControlSettings(null) { Holder = CreationHolder() };
                settings.Player = Group.Player;
                settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.AI);
                settings.SetSourcePosition(Location);
                settings.SetDestinationPosition(((PatrolAIGroup)Group).PatrolFrom);
                settings.MovePointsMax = 48;
                settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.IgnoreArmies);
                settings.ArmyHexSprite = ArmyHexSprite;
                settings.Selection = Selection;
                settings.MovePointTable = MovePointTable;
                StrategicMap.Current.MoveControl.ProcessMove(settings);
                if (settings.Path.Count == 0)
                {
                    ((PatrolAIGroup)Group).PatrolFrom = MapCoordinate.None;
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
        int x = default;
        int y = default;
        DoubleItemList<MapField> newList = default;
        PatrolAIGroup group = default;
        MoveControlSettings settings = default;
        DelphiList<MapField> fields = default;
        DoubleItemList<MapField>[] lists = new DoubleItemList<MapField>[HexDirectionSlotCount];
        MapFieldMessage message = default;
        int j = default;
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

        settings = new MoveControlSettings(null) { Holder = CreationHolder() };
        settings.Player = group.Player;
        settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.AI, MoveModeElement.IgnoreArmies);
        settings.SetSourcePosition(group.PatrolFrom);
        settings.MovePointsMax = 48;
        settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
        settings.ArmyHexSprite = ArmyHexSprite;
        settings.Selection = Selection;
        settings.MovePointTable = MovePointTable;
        StrategicMap.Current.MoveControl.ProcessMove(settings);
        fields = settings.ProcessedMapFields;
        for (int i = 0; i < HexDirectionSlotCount; i++)
        {
            DoubleItemList<MapField> doubleItemList = new();
            lists[i] = doubleItemList;
        }

        x = group.PatrolFrom.X;
        y = group.PatrolFrom.Y;
        for (int i = fields.Count - 1; i >= 0; i--)
        {
            // { Original samples every fourth processed field. }
            if ((i & 3) == 3)
            {
                mapField = fields.List[i];
                heading = HexGeometry.DeltaHexToHexDirection(x, y, mapField.HexX, mapField.HexY);
                int movePoints = settings.GetMovePoints(mapField.HexX, mapField.HexY, mapField.HexLevel);
                lists[heading].AddAbility(mapField, movePoints);
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
            newList = new DoubleItemList<MapField>();
            j = 0;
            while (newList.Count != 30 && j != lists[i].Count)
            {
                mapField = lists[i].List[j].First;
                if (mapField.GetPlayer() == -1)
                {
                    message.Result = 0;
                    mapField.SendMapFieldMessage(ref message);
                    if (message.Result.Int == 0)
                    {
                        DoubleItem<MapField> item = lists[i][j];
                        newList.Add(item);
                    }
                }

                j += 1;
            }

            lists[i]?.Dispose();
            lists[i] = newList;
        }

        mapField = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(group.PatrolFrom);
        if (mapField.GetPlayer() == -1)
        {
            lists[0].AddAbility(mapField, 0);
        }

        lastHeading = group.PatrolTo.X != -1
            ? HexGeometry.DeltaHexToHexDirection(group.PatrolFrom.X, group.PatrolFrom.Y, group.PatrolTo.X, group.PatrolTo.Y)
            : 0;

        previousHeading = group.PreviousHeading;
        choices = new ByteList();
        for (int i = 0; i < HexDirectionSlotCount; i++)
        {
            if (lists[i].Count != 0 && lastHeading != i && i != previousHeading)
            {
                if (previousHeading == 0 || PatrolHeadingDistance(i, lastHeading) > 1 || PatrolHeadingDistance(i, previousHeading) > 1)
                {
                    choices.Add((byte)i);
                }
            }
        }

        if (choices.Count == 0)
        {
            for (int i = 0; i < HexDirectionSlotCount; i++)
            {
                if (lists[i].Count != 0 && lastHeading != i)
                {
                    choices.Add((byte)i);
                }
            }
        }

        if (choices.Count != 0)
        {
            group.PreviousHeading = (sbyte)lastHeading;
            heading = choices[StrategicMap.Current.Random(choices.Count)];
            selectedList = lists[heading];
            int random = StrategicMap.Current.Random(selectedList.Count);
            mapField = selectedList.List[random].First;
            MapCoordinate mapCoordinate = mapField.GetMapCoordinate();
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

        choices?.Dispose();
        settings.Release();
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
