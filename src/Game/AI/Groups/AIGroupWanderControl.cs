// Translated from AGCPlugins.pas.
using System.Runtime.CompilerServices;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// mode, move limit, move settings, control
public partial class AIGroupWanderControl : AIGroupTask
{
    public int MovePointsMax { get; set; }

    private MoveControlSettings _moveSettings;
    private AIGroupControl _control;

    // AGCPlugins.pas:303
    public override void Dispose()
    {
        _moveSettings.Release();
        base.Dispose();
    }

    // AGCPlugins.pas:297
    public AIGroupWanderControl(ECustomNode owner) : base(owner)
    {
        _moveSettings = new MoveControlSettings(null);
    }

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
        DoubleItemList<AoWMapField> fields = default;
        AoWMapField field = default;
        int I = default;
        int targetWeight = default;
        MapCoordinate location = default;
        switch (State)
        {
            case 0:
                control = _control;
                _moveSettings.Reset();
                {
                    this._moveSettings.Player = control.Group.Player;
                    this._moveSettings.Mode = this._moveSettings.Mode + Set<MoveModeElement>.Of(MoveModeElement.AI);
                    this._moveSettings.SetSourcePosition(control.Location.X, control.Location.Y, control.Location.MapLevel);
                }

                {
                    var moveControlSettings = _moveSettings;
                    moveControlSettings.MovePointsMax = this.MovePointsMax;
                    moveControlSettings.ArmyHexSprite = control.ArmyHexSprite;
                    moveControlSettings.Selection = control.Selection;
                    moveControlSettings.MovePointTable = control.MovePointTable;
                    moveControlSettings.Mode = moveControlSettings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
                    AoWHSMap.Current.MoveControl.ProcessMove(_moveSettings);
                }

                {
                    var group = control.Group;
                    // { Original reloads the current control. }
                    if (group.TargetMode == 1 && this._control.Group.TargetMapCoordinate.X > 0 && _moveSettings.GetMovePoints(group.TargetMapCoordinate.X, group.TargetMapCoordinate.Y, group.TargetMapCoordinate.MapLevel) != 16777215 && Unsafe.As<MapCoordinate, int>(ref this._control.Group.TargetMapCoordinate) != Unsafe.As<MapCoordinate, int>(ref control.Location))
                    {
                        State = 2;
                    }
                    else
                    {
                        State = 1;
                    }
                }

                result = false;
                break;
            case 1:
                settings = _moveSettings;
                fields = new DoubleItemList<AoWMapField>();
                if (settings.ProcessedMapFields.Count > 40)
                {
                    I = settings.ProcessedMapFields.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        if ((I & 3) == 3)
                        {
                            field = (AoWMapField)settings.ProcessedMapFields[I];
                            if (_moveSettings.GetMovePoints(field.HexX, field.HexY, field.HexLevel) != 16777215)
                            {
                                var movePoints = _moveSettings.GetMovePoints(field.HexX, field.HexY, field.HexLevel);
                                fields.AddAbility(field, movePoints);
                            }
                        }
                    }
                }
                else
                {
                    I = settings.ProcessedMapFields.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        field = (AoWMapField)settings.ProcessedMapFields[I];
                        if (_moveSettings.GetMovePoints(field.HexX, field.HexY, field.HexLevel) != 16777215)
                        {
                            var movePoints = _moveSettings.GetMovePoints(field.HexX, field.HexY, field.HexLevel);
                            fields.AddAbility(field, movePoints);
                        }
                    }
                }

                if (fields.Count > 0)
                {
                    fields.Sort(CompareWanderFields);
                    field = null;
                    I = 0;
                    while (fields.Count != 0 && field == null)
                    {
                        I = AoWHSMap.Current.Random(Round((double)fields.Count * 0.1) + 1);
                        field = fields[I].First;
                        if (field.GetPlayer() != -1)
                        {
                            fields.Delete(I);
                            field = null;
                        }
                    }

                    if (field != null)
                    {
                        // { Original reads the list item before GetXYL. }
                        targetWeight = fields[I].Second;
                        location.Packed = field.GetMapCoordinate();
                        _control.Group.SetupTarget(location, targetWeight, AIGroupTargetStatus.Execute, 1, -1, 0, 0);
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
                    var aiGroupControl = _control;
                    aiGroupControl.SetupME();
                    aiGroupControl.ExecuteME(aiGroupControl.Group.TargetMapCoordinate.X, aiGroupControl.Group.TargetMapCoordinate.Y, aiGroupControl.Group.TargetMapCoordinate.MapLevel);
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

    private static int CompareWanderFields(in DoubleItem<AoWMapField> item1, in DoubleItem<AoWMapField> item2)
    {
        return item2.Second - item1.Second;
    }

    public enum ModeElement : byte
    {
        TransportersAllowed = 0
    }

}
