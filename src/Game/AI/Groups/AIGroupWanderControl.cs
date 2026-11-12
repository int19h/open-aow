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
    public Set<ModeElement> Mode;
    public int MovePointsMax;
    public MoveControlSettings MoveSettings;
    public AIGroupControl Control;

    // AGCPlugins.pas:303
    public override void Dispose()
    {
        MoveSettings.Release();
        base.Dispose();
    }

    // AGCPlugins.pas:297
    public AIGroupWanderControl(ECustomNode owner) : base(owner)
    {
        MoveSettings = new MoveControlSettings(null);
    }

    // AGCPlugins.pas:319
    public override bool Activate(AIGroupControl control)
    {
        Control = control;
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
                control = Control;
                MoveSettings.Reset();
                {
                    this.MoveSettings.Player = control.Group.Player;
                    this.MoveSettings.Mode = this.MoveSettings.Mode + Set<MoveModeElement>.Of(MoveModeElement.AI);
                    this.MoveSettings.SetSourcePosition(control.Location.X, control.Location.Y, control.Location.MapLevel);
                }

                {
                    var moveControlSettings = MoveSettings;
                    moveControlSettings.MovePointsMax = this.MovePointsMax;
                    moveControlSettings.ArmyHexSprite = control.ArmyHexSprite;
                    moveControlSettings.Selection = control.Selection;
                    moveControlSettings.MovePointTable = control.MovePointTable;
                    moveControlSettings.Mode = moveControlSettings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
                    AoWHSMap.Current.MoveControl.ProcessMove(MoveSettings);
                }

                {
                    var group = control.Group;
                    // { Original reloads the current control. }
                    if (group.TargetMode == 1 && this.Control.Group.TargetMapCoordinate.X > 0 && MoveSettings.GetMovePoints(group.TargetMapCoordinate.X, group.TargetMapCoordinate.Y, group.TargetMapCoordinate.MapLevel) != 16777215 && Unsafe.As<MapCoordinate, int>(ref this.Control.Group.TargetMapCoordinate) != Unsafe.As<MapCoordinate, int>(ref control.Location))
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
                settings = MoveSettings;
                fields = new DoubleItemList<AoWMapField>();
                if (settings.ProcessedMapFields.Count > 40)
                {
                    I = settings.ProcessedMapFields.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        if ((I & 3) == 3)
                        {
                            field = (AoWMapField)settings.ProcessedMapFields[I];
                            if (MoveSettings.GetMovePoints(field.HexX, field.HexY, field.HexLevel) != 16777215)
                            {
                                var movePoints = MoveSettings.GetMovePoints(field.HexX, field.HexY, field.HexLevel);
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
                        if (MoveSettings.GetMovePoints(field.HexX, field.HexY, field.HexLevel) != 16777215)
                        {
                            var movePoints = MoveSettings.GetMovePoints(field.HexX, field.HexY, field.HexLevel);
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
                        Control.Group.SetupTarget(location, targetWeight, AIGroupTargetStatus.Execute, 1, -1, 0, 0);
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
                    var aiGroupControl = Control;
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
        Mode = Set<ModeElement>.Of();
        MovePointsMax = 75;
        MoveSettings.Reset();
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
