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
    public Set<ModeElement> FMode;
    public int FMovePointsMax;
    public MoveControlSettings FMoveSettings;
    public AIGroupControl FControl;

    // AGCPlugins.pas:303
    public override void Dispose()
    {
        FMoveSettings.Release();
        base.Dispose();
    }

    // AGCPlugins.pas:297
    public AIGroupWanderControl(ECustomNode owner) : base(owner)
    {
        FMoveSettings = new MoveControlSettings(null);
    }

    // AGCPlugins.pas:319
    public override bool Activate(AIGroupControl Control)
    {
        FControl = Control;
        FState = 0;
        return true;
    }

    // AGCPlugins.pas:329
    public override bool Process()
    {
        bool Result = default;
        AIGroupControl Control = default;
        MoveControlSettings Settings = default;
        DoubleItemList<AoWMapField> Fields = default;
        AoWMapField Field = default;
        int I = default;
        int TargetWeight = default;
        MapCoordinate Location = default;
        switch (FState)
        {
            case 0:
                Control = FControl;
                FMoveSettings.Reset();
                {
                    this.FMoveSettings.FPlayer = Control.FGroup.FPlayer;
                    this.FMoveSettings.FMode = this.FMoveSettings.FMode + Set<MoveModeElement>.Of(MoveModeElement.AI);
                    this.FMoveSettings.SetSourcePos(Control.FLocation.X, Control.FLocation.Y, Control.FLocation.L);
                }

                {
                    var moveControlSettings = FMoveSettings;
                    moveControlSettings.FMovePointsMax = this.FMovePointsMax;
                    moveControlSettings.FArmyHS = Control.FArmyHS;
                    moveControlSettings.FSelection = Control.FSelection;
                    moveControlSettings.FMovePointTable = Control.FMovePointTable;
                    moveControlSettings.FMode = moveControlSettings.FMode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
                    AoWHSMap.Current.FMoveControl.ProcessMove(FMoveSettings);
                }

                {
                    var group = Control.FGroup;
                    // { Original reloads the current control. }
                    if (group.FTargetMode == 1 && this.FControl.FGroup.FTargetXYL.X > 0 && FMoveSettings.GetMovePoints(group.FTargetXYL.X, group.FTargetXYL.Y, group.FTargetXYL.L) != 16777215 && Unsafe.As<MapCoordinate, int>(ref this.FControl.FGroup.FTargetXYL) != Unsafe.As<MapCoordinate, int>(ref Control.FLocation))
                    {
                        FState = 2;
                    }
                    else
                    {
                        FState = 1;
                    }
                }

                Result = false;
                break;
            case 1:
                Settings = FMoveSettings;
                Fields = new DoubleItemList<AoWMapField>();
                if (Settings.FProcessedMapFields.Count > 40)
                {
                    I = Settings.FProcessedMapFields.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        if ((I & 3) == 3)
                        {
                            Field = (AoWMapField)Settings.FProcessedMapFields[I];
                            if (FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx) != 16777215)
                            {
                                var movePoints = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                                Fields.AddAB(Field, movePoints);
                            }
                        }
                    }
                }
                else
                {
                    I = Settings.FProcessedMapFields.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        Field = (AoWMapField)Settings.FProcessedMapFields[I];
                        if (FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx) != 16777215)
                        {
                            var movePoints = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                            Fields.AddAB(Field, movePoints);
                        }
                    }
                }

                if (Fields.Count > 0)
                {
                    Fields.Sort(CompareWanderFields);
                    Field = null;
                    I = 0;
                    while (Fields.Count != 0 && Field == null)
                    {
                        I = AoWHSMap.Current.Random(Round((double)Fields.Count * 0.1) + 1);
                        Field = Fields[I].A;
                        if (Field.GetPlayer() != -1)
                        {
                            Fields.Delete(I);
                            Field = null;
                        }
                    }

                    if (Field != null)
                    {
                        // { Original reads the list item before GetXYL. }
                        TargetWeight = Fields[I].B;
                        Location.Packed = Field.GetXYL();
                        FControl.FGroup.SetupTargetEx(Location, TargetWeight, AIGroupTargetStatus.Execute, 1, -1, 0, 0);
                        FState += 1;
                    }
                    else
                    {
                        Done();
                    }

                    Fields?.Dispose();
                }
                else
                {
                    // { Original leaks Fields when no reachable field was collected. }
                    Done();
                }

                Result = false;
                break;
            case 2:
                {
                    var aiGroupControl = FControl;
                    aiGroupControl.SetupME();
                    aiGroupControl.ExecuteME(aiGroupControl.FGroup.FTargetXYL.X, aiGroupControl.FGroup.FTargetXYL.Y, aiGroupControl.FGroup.FTargetXYL.L);
                }

                FState += 1;
                Result = false;
                break;
            case 3:
                Done();
                Result = false;
                break;
            default:
                Result = false;
                break;
        }

        return Result;
    }

    // AGCPlugins.pas:309
    public void Reset()
    {
        FMode = Set<ModeElement>.Of();
        FMovePointsMax = 75;
        FMoveSettings.Reset();
    }

    // AGCPlugins.pas:316
    public bool Setup(AIGroupControl Control)
    {
        Reset();
        return true;
    }

    private static int CompareWanderFields(in DoubleItem<AoWMapField> Item1, in DoubleItem<AoWMapField> Item2)
    {
        return Item2.B - Item1.B;
    }

    public enum ModeElement : byte
    {
        TransportersAllowed = 0
    }

}
