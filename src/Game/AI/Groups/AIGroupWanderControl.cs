#nullable disable warnings
// Translated from AGCPlugins.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.AI.Groups;

// mode, move limit, move settings, control
public class AIGroupWanderControl : AIGroupTask
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
    public AIGroupWanderControl(CustomNode? owner) : base(owner) => _moveSettings = new MoveControlSettings(null) { Holder = CreationHolder() };

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
        switch (State)
        {
            case 0:
                AIGroupControl control = _control;
                _moveSettings.Reset();
                {
                    _moveSettings.Player = control.Group.Player;
                    _moveSettings.Mode |= MoveModes.AI;
                    _moveSettings.SetSourcePosition(control.Location);
                }

                {
                    MoveControlSettings moveControlSettings = _moveSettings;
                    moveControlSettings.MovePointsMax = MovePointsMax;
                    moveControlSettings.ArmyHexSprite = control.ArmyHexSprite;
                    moveControlSettings.Selection = control.Selection;
                    moveControlSettings.MovePointTable = control.MovePointTable;
                    moveControlSettings.Mode |= MoveModes.LogProcessedMapFields;
                    StrategicMap.Current.MoveControl.ProcessMove(_moveSettings);
                }

                {
                    AIGroup group = control.Group;
                    // { Original reloads the current control. }
                    MapCoordinate aimed = _control.Group.TargetLocation ?? MapCoordinate.None;
                    State = group.TargetMode == 1 && aimed.X > 0 && _moveSettings.GetMovePoints(aimed.X, aimed.Y, aimed.MapLevel) != 16777215 && _control.Group.TargetLocation != control.Location
                        ? 2
                        : 1;
                }

                return false;
            case 1:
                MoveControlSettings settings = _moveSettings;
                DoubleItemList<MapField> fields = [];
                MapField? field;
                int i;
                if (settings.ProcessedMapFields.Count > 40)
                {
                    i = settings.ProcessedMapFields.Count - 1;
                    for (; i >= 0; i -= 1)
                    {
                        if ((i & 0b0011) == 0b0011)
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
                    while (fields.Count != 0 && field is null)
                    {
                        i = StrategicMap.Current.Random(Round(fields.Count * 0.1) + 1);
                        field = fields[i].First;
                        if (field.GetPlayer() != -1)
                        {
                            fields.Delete(i);
                            field = null;
                        }
                    }

                    if (field is not null)
                    {
                        // { Original reads the list item before GetXYL. }
                        int targetWeight = fields[i].Second;
                        MapCoordinate location = field.MapCoordinate;
                        _ = _control.Group.SetupTarget(location, movePoints: targetWeight, targetType: AIGroupTargetStatus.Execute, mode: 1, transporterId: -1, gold: 0, mana: 0);
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

                return false;
            case 2:
                {
                    AIGroupControl aiGroupControl = _control;
                    aiGroupControl.SetupME();
                    MapCoordinate aimed = aiGroupControl.Group.TargetLocation ?? MapCoordinate.None;
                    aiGroupControl.ExecuteME(aimed.X, aimed.Y, L: aimed.MapLevel);
                }

                State += 1;
                return false;
            case 3:
                Done();
                return false;
            default:
                return false;
        }
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
