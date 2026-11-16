// Translated from IndependentAG.pas.
using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// from, to, previous heading, strength
public partial class PatrolAIGroup : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:295
    public new const int ClassId = 136274;

    public MapCoordinate PatrolFrom { get; internal set; }
    public MapCoordinate PatrolTo { get; internal set; }

    // The fourth bytes of the packed locations stored at ids 40 (low byte) and 41 (high byte), 0 from
    // every writer and in every corpus file, kept so that a loaded value saves back unchanged.
    public ushort UnknownBytes { get; set; }

    public sbyte PreviousHeading { get; set; }
    public int PatrolStrength { get; set; }

    // IndependentAG.pas:304
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int from = StoredCoordinateImage.From(PatrolFrom, unknownBytes: (byte)UnknownBytes).ToInt32();
        from = persister.Int32("PatrolFrom", value: from);
        (PatrolFrom, byte fromUnknown) = StoredCoordinateImage.FromInt32(from);
        int to = StoredCoordinateImage.From(PatrolTo, unknownBytes: (byte)(UnknownBytes >> 8)).ToInt32();
        to = persister.Int32("PatrolTo", value: to);
        (PatrolTo, byte toUnknown) = StoredCoordinateImage.FromInt32(to);
        UnknownBytes = (ushort)(fromUnknown | (toUnknown << 8));
        PreviousHeading = PersistSupport.UnsignedByte(persister, name: "PreviousHeading", value: PreviousHeading);
        PatrolStrength = persister.Int32("PatrolStrength", value: PatrolStrength);
    }

    // IndependentAG.pas:285
    public PatrolAIGroup(CustomNode owner) : base(owner)
    {
        ProcessPriority = 1;
        RecruitmentPriority = 2;
        PatrolFrom = MapCoordinate.None;
        PatrolTo = MapCoordinate.None;
        PreviousHeading = 0;
    }

    // IndependentAG.pas:301
    public override EngineClass GroupControlClass() => EngineClass.Of<PatrolAIGroupControl>();

    // IndependentAG.pas:298
    public static new GroupBehavior Behavior => GroupBehavior.Patrol;

    public override void OnControlMoveFailed() => PatrolTo = MapCoordinate.None;

    public override void ProcessControlStates(AIGroupControl control)
    {
        AIGroupDefenseControl defense;
        AIRecruitmentGroupFinder recruitment;
        AIGroupRecruitControl recruit;
        AIGroupTargetFinder finder;
        MoveControlSettings settings;
        switch (control.State)
        {
            case 0:
                if (PatrolStrength == 0)
                {
                    PatrolStrength = control.StrengthMax;
                }

                defense = new AIGroupDefenseControl(null) { Holder = control.CreationHolder() };
                defense.DefendMode = AIGroupDefenseControl.GroupDefendMode.Wait;
                control.ActivatePlugin(defense);
                defense.Release();
                control.State = 1;
                break;
            case 1:
                if (PatrolTo.X == -1 || (PatrolTo.MapLevel == control.Location.MapLevel && HexGeometry.DeltaHexToRadius(PatrolTo.X, PatrolTo.Y, control.Location.X, control.Location.Y) <= 2))
                {
                    ResetPatrolToLocation(control);
                }

                control.State += 1;
                break;
            case 2:
                if (control.StrengthMax < PatrolStrength)
                {
                    control.State += 1;
                    recruitment = new AIRecruitmentGroupFinder(null) { Holder = control.CreationHolder() };
                    recruitment.Setup(control);
                    control.ActivatePlugin(recruitment);
                    recruitment.Release();
                }
                else
                {
                    control.State = 4;
                }

                break;
            case 3:
                if (control.RecruitmentGroups.Count != 0 && control.StrengthMax < PatrolStrength)
                {
                    recruit = new AIGroupRecruitControl(null) { Holder = control.CreationHolder() };
                    recruit.Reset();
                    recruit.RecruitStrength = PatrolStrength - control.StrengthMax;
                    recruit.Mode = AIGroupRecruitControlMode.AutoRecruit;
                    control.ActivatePlugin(recruit);
                    recruit.Release();
                }

                control.State += 1;
                break;
            case 4:
                finder = new AIGroupTargetFinder(null) { Holder = control.CreationHolder() };
                finder.Setup(control);
                sbyte moves = control.Army.Moves(control.Selection);
                finder.Selector.MovePointsMax = moves;
                control.ActivatePlugin(finder);
                finder.Release();
                control.State += 1;
                break;
            case 5:
                control.Targets.Validate();
                if (control.Targets.Count != 0)
                {
                    control.SetupME();
                    AIGroupControlTarget targetLocation = control.Targets.GetTargets(0);
                    AIGroupControlTarget target = control.Targets.GetTargets(0);
                    control.ExecuteME(control.Targets.GetTargets(0).Location.X, target.Location.Y, targetLocation.Location.MapLevel);
                    control.Targets.Delete(0);
                }
                else
                {
                    control.State += 1;
                }

                break;
            case 6:
                if (StrategicMap.Current.Random(3) != 0 && PatrolTo.X != -1)
                {
                    control.State += 1;
                    control.SetupME();
                    control.ExecuteME(PatrolTo.X, PatrolTo.Y, PatrolTo.MapLevel);
                }
                else
                {
                    control.Done();
                }

                break;
            case 7:
                settings = new MoveControlSettings(null) { Holder = control.CreationHolder() };
                settings.Player = Player;
                settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.AI);
                settings.SetSourcePosition(control.Location);
                settings.SetDestinationPosition(PatrolFrom);
                settings.MovePointsMax = 48;
                settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.IgnoreArmies);
                settings.ArmyHexSprite = control.ArmyHexSprite;
                settings.Selection = control.Selection;
                settings.MovePointTable = control.MovePointTable;
                StrategicMap.Current.MoveControl.ProcessMove(settings);
                if (settings.Path.Count == 0)
                {
                    PatrolFrom = MapCoordinate.None;
                }

                settings.Release();
                control.Done();
                break;
        }
    }

    // IndependentAG.pas:324
    public bool ResetPatrolToLocation(AIGroupControl control)
    {
        bool result;
        int previousHeading;
        int x;
        int y;
        DoubleItemList<MapField> newList;
        MoveControlSettings settings;
        DelphiList<MapField> fields;
        DoubleItemList<MapField>[] lists = new DoubleItemList<MapField>[PatrolAIGroupControl.HexDirectionSlotCount];
        MapFieldMessage message = default;
        int j;
        int heading;
        int lastHeading;
        MapField mapField;
        ByteList choices;
        DoubleItemList<MapField> selectedList;
        if (PatrolFrom.X == -1)
        {
            PatrolFrom = control.Location;
        }

        settings = new MoveControlSettings(null) { Holder = control.CreationHolder() };
        settings.Player = Player;
        settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.AI, MoveModeElement.IgnoreArmies);
        settings.SetSourcePosition(PatrolFrom);
        settings.MovePointsMax = 48;
        settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
        settings.ArmyHexSprite = control.ArmyHexSprite;
        settings.Selection = control.Selection;
        settings.MovePointTable = control.MovePointTable;
        StrategicMap.Current.MoveControl.ProcessMove(settings);
        fields = settings.ProcessedMapFields;
        for (int i = 0; i < PatrolAIGroupControl.HexDirectionSlotCount; i++)
        {
            DoubleItemList<MapField> doubleItemList = new();
            lists[i] = doubleItemList;
        }

        x = PatrolFrom.X;
        y = PatrolFrom.Y;
        for (int i = fields.Count - 1; i >= 0; i--)
        {
            // { Original samples every fourth processed field. }
            if ((i & 3) == 3)
            {
                mapField = fields.List[i]!;
                heading = HexGeometry.DeltaHexToHexDirection(x, y, mapField.HexX, mapField.HexY);
                int movePoints = settings.GetMovePoints(mapField.HexX, mapField.HexY, mapField.HexLevel);
                lists[heading].AddAbility(mapField, movePoints);
            }
        }

        for (int i = 0; i < PatrolAIGroupControl.HexDirectionSlotCount; i++)
        {
            lists[i].Sort(ComparePatrolFields);
        }

        message.MessageId = 4384;
        MapFieldMessageParameters param = message.Param;
        param[0] = control.ArmyHexSprite;
        message.Param = param;
        for (int i = 1; i < PatrolAIGroupControl.HexDirectionSlotCount; i++)
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

        mapField = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(PatrolFrom);
        if (mapField.GetPlayer() == -1)
        {
            lists[0].AddAbility(mapField, 0);
        }

        lastHeading = PatrolTo.X != -1
            ? HexGeometry.DeltaHexToHexDirection(PatrolFrom.X, PatrolFrom.Y, PatrolTo.X, PatrolTo.Y)
            : 0;

        previousHeading = PreviousHeading;
        choices = new ByteList();
        for (int i = 0; i < PatrolAIGroupControl.HexDirectionSlotCount; i++)
        {
            if (lists[i].Count != 0 && lastHeading != i && i != previousHeading)
            {
                if (previousHeading == 0 || PatrolHeadingDistance(i, lastHeading: lastHeading) > 1 || PatrolHeadingDistance(i, lastHeading: previousHeading) > 1)
                {
                    choices.Add((byte)i);
                }
            }
        }

        if (choices.Count == 0)
        {
            for (int i = 0; i < PatrolAIGroupControl.HexDirectionSlotCount; i++)
            {
                if (lists[i].Count != 0 && lastHeading != i)
                {
                    choices.Add((byte)i);
                }
            }
        }

        if (choices.Count != 0)
        {
            PreviousHeading = (sbyte)lastHeading;
            heading = choices[StrategicMap.Current.Random(choices.Count)];
            selectedList = lists[heading];
            int random = StrategicMap.Current.Random(selectedList.Count);
            mapField = selectedList.List[random].First;
            MapCoordinate mapCoordinate = mapField.GetMapCoordinate();
            PatrolTo = mapCoordinate;
            result = true;
        }
        else
        {
            result = false;
        }

        for (int i = 0; i < PatrolAIGroupControl.HexDirectionSlotCount; i++)
        {
            lists[i]?.Dispose();
        }

        choices?.Dispose();
        settings.Release();
        return result;
    }

    private static int ComparePatrolFields(in DoubleItem<MapField> first, in DoubleItem<MapField> second) => second.Second - first.Second;

    private static int PatrolHeadingDistance(int heading, int lastHeading)
    {
        int number = Math.Abs(lastHeading - heading);
        if (number > 3)
        {
            number = 6 - number;
        }

        return number;
    }

}
