#nullable disable
// Translated from AGCPlugins.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.AI.Groups;

// mode, move limit, move settings, control
public partial class AIGroupWanderControl : AIGroupTask
{
    public int MovePointsMax { get; set; }

    private readonly MoveControlSettings _moveSettings;
    private AIGroupControl _control;

    // AGCPlugins.pas:303
    public override void Dispose()
    {
        _ = _moveSettings.Release();
        base.Dispose();
    }

    // AGCPlugins.pas:297
    public AIGroupWanderControl(CustomNode owner) : base(owner) => _moveSettings = new MoveControlSettings(null) { Holder = CreationHolder() };

    // AGCPlugins.pas:319
    public override bool Activate(AIGroupControl control)
    {
        _control = control;
        State = 0;
        return true;
    }

    // AGCPlugins.pas:329
    public override bool Process()
    {
        bool result = default;
        AIGroupControl control = default;
        MoveControlSettings settings = default;
        DoubleItemList<MapField> fields = default;
        MapField field = default;
        int i = default;
        int targetWeight = default;
        MapCoordinate location = default;
        switch (State)
        {
            case 0:
                control = _control;
                _moveSettings.Reset();
                {
                    _moveSettings.Player = control.Group.Player;
                    _moveSettings.Mode += Set<MoveModeElement>.Of(MoveModeElement.AI);
                    _moveSettings.SetSourcePosition(control.Location);
                }

                {
                    MoveControlSettings moveControlSettings = _moveSettings;
                    moveControlSettings.MovePointsMax = MovePointsMax;
                    moveControlSettings.ArmyHexSprite = control.ArmyHexSprite;
                    moveControlSettings.Selection = control.Selection;
                    moveControlSettings.MovePointTable = control.MovePointTable;
                    moveControlSettings.Mode += Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
                    StrategicMap.Current.MoveControl.ProcessMove(_moveSettings);
                }

                {
                    AIGroup group = control.Group;
                    // { Original reloads the current control. }
                    State = group.TargetMode == 1 && _control.Group.TargetMapCoordinate.Location.X > 0 && _moveSettings.GetMovePoints(group.TargetMapCoordinate.Location.X, group.TargetMapCoordinate.Location.Y, group.TargetMapCoordinate.Location.MapLevel) != 16777215 && _control.Group.TargetMapCoordinate != AITargetLocation.At(control.Location)
                        ? 2
                        : 1;
                }

                result = false;
                break;
            case 1:
                settings = _moveSettings;
                fields = new DoubleItemList<MapField>();
                if (settings.ProcessedMapFields.Count > 40)
                {
                    i = settings.ProcessedMapFields.Count - 1;
                    for (; i >= 0; i -= 1)
                    {
                        if ((i & 3) == 3)
                        {
                            field = settings.ProcessedMapFields[i];
                            if (_moveSettings.GetMovePoints(field.HexX, field.HexY, field.HexLevel) != 16777215)
                            {
                                _ = fields.AddAbility(b: _moveSettings.GetMovePoints(field.HexX, field.HexY, field.HexLevel), a: field);
                            }
                        }
                    }
                }
                else
                {
                    i = settings.ProcessedMapFields.Count - 1;
                    for (; i >= 0; i -= 1)
                    {
                        field = settings.ProcessedMapFields[i];
                        if (_moveSettings.GetMovePoints(field.HexX, field.HexY, field.HexLevel) != 16777215)
                        {
                            _ = fields.AddAbility(b: _moveSettings.GetMovePoints(field.HexX, field.HexY, field.HexLevel), a: field);
                        }
                    }
                }

                if (fields.Count > 0)
                {
                    fields.Sort(CompareWanderFields);
                    field = null;
                    i = 0;
                    while (fields.Count != 0 && field == null)
                    {
                        i = StrategicMap.Current.Random(Round(fields.Count * 0.1) + 1);
                        field = fields[i].First;
                        if (field.GetPlayer() != -1)
                        {
                            fields.Delete(i);
                            field = null;
                        }
                    }

                    if (field != null)
                    {
                        // { Original reads the list item before GetXYL. }
                        targetWeight = fields[i].Second;
                        location = field.GetMapCoordinate();
                        _ = _control.Group.SetupTarget(location, targetWeight, AIGroupTargetStatus.Execute, 1, -1, 0, 0);
                        State += 1;
                    }
                    else
                    {
                        Done();
                    }

                    fields?.Dispose();
                }
                else
                {
                    // { Original leaks Fields when no reachable field was collected. }
                    Done();
                }

                result = false;
                break;
            case 2:
                {
                    AIGroupControl aiGroupControl = _control;
                    aiGroupControl.SetupME();
                    aiGroupControl.ExecuteME(aiGroupControl.Group.TargetMapCoordinate.Location.X, aiGroupControl.Group.TargetMapCoordinate.Location.Y, aiGroupControl.Group.TargetMapCoordinate.Location.MapLevel);
                }

                State += 1;
                result = false;
                break;
            case 3:
                Done();
                result = false;
                break;
            default:
                result = false;
                break;
        }

        return result;
    }

    // AGCPlugins.pas:309
    public void Reset()
    {
        MovePointsMax = 75;
        _moveSettings.Reset();
    }

    // AGCPlugins.pas:316
    public bool Setup(AIGroupControl control)
    {
        Reset();
        return true;
    }

    private static int CompareWanderFields(in DoubleItem<MapField> item1, in DoubleItem<MapField> item2) => item2.Second - item1.Second;

    public enum ModeElement : byte
    {
        TransportersAllowed = 0
    }

}
