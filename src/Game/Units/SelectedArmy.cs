// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// { Interface declaration run owned by P2. }
// mode, road builder ID, selected graphics references, ignore next mouse-down
public partial class SelectedArmy : MoveSelector
{
    // Army.PAS:3567
    public SelectedArmy() : base()
    {
        FSelectedUnitGFX = new Int32List();
    }

    public Mode FMode;
    public int FRoadBuilderID;
    public Int32List FSelectedUnitGFX;
    public bool FIgnoreMouseDown;

    // Army.PAS:3573
    public override void Dispose()
    {
        ResetSelectedUnitGFX();
        FSelectedUnitGFX?.Dispose();
        base.Dispose();
    }

    // Army.PAS:3837
    public override void MouseDownEvent(object Sender)
    {
        ArmyHexSprite ArmyHS;
        int Index;
        if (!FIgnoreMouseDown)
        {
            if (AoWHSMap.Current.Mouse.Shift.Contains(ShiftState.Shift))
            {
                if (FMoveSettingsList.Count == 1 && FMoveSettingsList.GetMoveSettings(0).FPath.Count != 0)
                {
                    var item = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                    var fPathItem = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                    var item2 = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                    if (!ValidMultiMoveSelection((AoWMapField)FMap.MapContainer.GetField(item2.X, fPathItem.Y, item.L)))
                    {
                        return;
                    }
                }

                if (AoWHSMap.Current.Mouse.Xhx != -1)
                {
                    var yhx = AoWHSMap.Current.Mouse.Yhx;
                    var xhx = AoWHSMap.Current.Mouse.Xhx;
                    ArmyHS = (ArmyHexSprite)AoWHSMap.Current.MapContainer.GetField(xhx, yhx, AoWHSMap.Current.ViewL).FindOwnedChild(131607);
                    if (ArmyHS != null && ArmyHS.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer)
                    {
                        Index = FMoveSettingsList.IndexOfMoveObject(ArmyHS);
                        if (Index == -1)
                        {
                            AddMoveArmy(ArmyHS, 255);
                            Update();
                            AoWHSMap.Current.Selection.SelectLocationHS(ArmyHS);
                            SetCurrentMove(-1);
                        }
                        else
                        {
                            ArmyHS.Unselect();
                            DeleteMoveObject(Index);
                            if (FMoveSettingsList.Count == 1)
                            {
                                SetCurrentMove(0);
                            }
                        }
                    }
                }
            }
            else
            {
                if (AoWHSMap.Current.Mouse.Shift.Contains(ShiftState.Ctrl))
                {
                    if (FMoveSettingsList.Count > 1 && AoWHSMap.Current.Mouse.Xhx != -1)
                    {
                        var yhx = AoWHSMap.Current.Mouse.Yhx;
                        var xhx = AoWHSMap.Current.Mouse.Xhx;
                        ArmyHS = (ArmyHexSprite)AoWHSMap.Current.MapContainer.GetField(xhx, yhx, AoWHSMap.Current.ViewL).FindOwnedChild(131607);
                        if (ArmyHS != null && ArmyHS.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer)
                        {
                            Index = FMoveSettingsList.IndexOfMoveObject(ArmyHS);
                            if (Index != -1)
                            {
                                if (Index != FCurrentMove)
                                {
                                    SetCurrentMove(Index);
                                    Update();
                                }
                                else
                                {
                                    SetCurrentMove(-1);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (AoWHSMap.Current.Mouse.Capture != AoWHSMap.Current.FTargetSelection && (AoWHSMap.Current.Mouse.Capture != this || FMap.Mouse.Button == MouseButton.Right))
                    {
                        Unselect();
                    }
                    else
                    {
                        if (AoWHSMap.Current.Mouse.Capture == this && AoWHSMap.Current.Mouse.Button == MouseButton.Left)
                        {
                            var yhx = FMap.Mouse.Yhx;
                            SelectPath((sbyte)FMap.Mouse.Xhx, (sbyte)yhx, (sbyte)FMap.ViewL);
                        }
                    }
                }
            }
        }
        else
        {
            FIgnoreMouseDown = false;
        }
    }

    // Army.PAS:3751
    public override void CalculateMovePath(int Index, sbyte X, sbyte Y, sbyte L)
    {
        MoveSettings Settings;
        ArmyHexSprite ArmyHS;
        if (X != -1)
        {
            Settings = FMoveSettingsList.GetMoveSettings(Index);
            ArmyHS = (ArmyHexSprite)Settings.FMoveObject;
            Settings.FPathMovePoints = 0;
            FMoveControlSettings.Reset();
            var lhx = ArmyHS.GetLhx();
            var yhx = ArmyHS.GetYhx();
            var xhx = ArmyHS.GetXhx();
            FMoveControlSettings.SetSourcePos(xhx, yhx, lhx);
            FMoveControlSettings.FPlayer = ArmyHS.FArmy.FPlayer;
            FMoveControlSettings.FArmyHS = ArmyHS;
            FMoveControlSettings.SetDestinationPos(X, Y, L);
            FMoveControlSettings.FUnits.AddUnitsEx(ArmyHS.FArmy, (byte)Settings.FSelection);
            if (AoWHSMap.Current.FExplorationEnabled)
            {
                FMoveControlSettings.FMode = FMoveControlSettings.FMode + Set<MoveModeElement>.Of(MoveModeElement.ExploredTerrainOnly);
            }

            ArmyHS.FArmy.CreateMovePointTable((byte)Settings.FSelection, Bytes.Of(ref FMoveControlSettings.FMovePointTable));
            AoWHSMap.Current.FMoveControl.ProcessMove(FMoveControlSettings);
            if (FMoveControlSettings.FPath.Count != 0)
            {
                Settings.FPath.Assign(FMoveControlSettings.FPath);
                Settings.FPathMovePoints = Settings.FPath.PosToMovePoints(0);
                var item = Settings.FPath[0];
                var fPathItem = Settings.FPath[0];
                var item2 = Settings.FPath[0];
                ArmyHS.FArmy.SetDestinationPos(item2.X, fPathItem.Y, item.L);
                FMoveControlSettings.FPath.Clear();
                if (FMode == Mode.BuildRoads)
                {
                    if (Settings.FMovePoints < Settings.FPathMovePoints)
                    {
                        Settings.FPath.Clear();
                        var armyNotEnoughMovePoints = AbilityRules.TranslateRStr(ArmyNotEnoughMovePointsRStr);
                        AoWHSMap.Current.ShowMessage(armyNotEnoughMovePoints);
                    }
                    else
                    {
                        if (Settings.FPath.Count != 0)
                        {
                            var roadConstructionCost = AbilityRules.RoadConstructionCost(GetArmyHS().FArmy.FPlayer, Settings.FPath, Settings.FMovePoints);
                            var format = AbilityRules.Format(AbilityRules.TranslateRStr(ArmyRoadConstructionCostRStr), [IntToStr(roadConstructionCost)]);
                            AoWHSMap.Current.ShowMessage(format);
                        }
                    }
                }
            }
            else
            {
                FMoveSettingsList.GetMoveSettings(Index).FPath.Clear();
                ArmyHS.FArmy.ResetDestinationPos();
            }
        }
    }

    // Army.PAS:3580
    public override void SetCurrentMove(int Value)
    {
        if (FCurrentMove != -1 && GetArmyHS() != null)
        {
            var armyHS = GetArmyHS();
            AoWHSMap.Current.Selection.TriggerDeselectEvent(armyHS);
        }

        FCurrentMove = Value;
        if (FCurrentMove != -1 && GetArmyHS() != null)
        {
            var armyHS = GetArmyHS();
            AoWHSMap.Current.Selection.TriggerSelectEvent(armyHS);
        }

        UpdateSelectedUnitGFX();
    }

    // Army.PAS:3795
    public override void SelectPath(sbyte X, sbyte Y, sbyte L)
    {
        bool Valid;
        int I;
        MoveSettings Settings;
        if (GetArmyHS() != null && GetArmyHS().GetXhx() == AoWHSMap.Current.Mouse.Xhx && GetArmyHS().GetYhx() == AoWHSMap.Current.Mouse.Yhx && GetArmyHS().GetLhx() == AoWHSMap.Current.ViewL)
        {
            var lhx = GetArmyHS().GetLhx();
            var yhx = GetArmyHS().GetYhx();
            var xhx = GetArmyHS().GetXhx();
            AoWHSMap.Current.Selection.SelectLocationHX(xhx, yhx, lhx);
        }
        else
        {
            if (FMoveSettingsList.Count > 1 && FCurrentMove == -1)
            {
                Valid = true;
                I = FMoveSettingsList.Count;
                while (Valid && I != 0)
                {
                    I -= 1;
                    Settings = FMoveSettingsList.GetMoveSettings(I);
                    Valid = Settings.FPath.Count != 0 && Settings.FPath[0].X == X && Settings.FPath[0].Y == Y && Settings.FPath[0].L == L;
                }

                if (!Valid)
                {
                    if (ValidateSelection((AoWMapField)FMap.MapContainer.GetField(X, Y, L)))
                    {
                        I = 0;
                        int last = FMoveSettingsList.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            CalculateMovePath(I, X, Y, L);
                        }
                    }
                    else
                    {
                        I = 0;
                        int last = FMoveSettingsList.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            FMoveSettingsList.GetMoveSettings(I).FPath.Clear();
                        }
                    }
                }
                else
                {
                    ExecuteMove();
                }
            }
            else
            {
                base.SelectPath(X, Y, L);
            }
        }
    }

    // Army.PAS:3831
    public override void Unselect()
    {
        base.Unselect();
        Select(null);
    }

    // Army.PAS:3742
    public override bool ValidateSelection(AoWMapField Field)
    {
        bool Result;
        if (FMoveSettingsList.Count > 1)
        {
            Result = ValidMultiMoveSelection(Field);
        }
        else
        {
            Result = base.ValidateSelection(Field);
        }

        if (FMode == Mode.BuildRoads)
        {
            if (Field.GetPlayer() != -1 && Field.GetPlayer() != GetArmyHS().FArmy.FPlayer)
            {
                Result = false;
            }
        }

        return Result;
    }

    // Army.PAS:3904
    public override void ExecuteMove()
    {
        ArmyHexSprite ArmyHS;
        AoWMapField Field;
        int Cost;
        if (FMoveSettingsList.Count == 1)
        {
            var moveSettings = FMoveSettingsList.GetMoveSettings(FCurrentMove);
            if (moveSettings.FPath.Count > 1 && GetArmyHS() != null)
            {
                ArmyHS = GetArmyHS();
                var item = moveSettings.FPath[0];
                var fPathItem = moveSettings.FPath[0];
                var item2 = moveSettings.FPath[0];
                Field = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(item2.X, fPathItem.Y, item.L);
                bool condition = Field.GetMoveAction(ArmyHS.FArmy.FPlayer) != MoveAction.Attack || Field.GetPlayer() <= 0;
                if (!condition)
                {
                    var player = Field.GetPlayer();
                    var diplomaticRelation = AoWHSMap.Current.FPlayers.GetDiplomaticRelation(ArmyHS.FArmy.FPlayer, player);
                    condition = Set<DiplomaticRelation>.Of(DiplomaticRelation.War, DiplomaticRelation.Unknown).Contains(diplomaticRelation);
                }

                if (condition)
                {
                    if (FMode == Mode.BuildRoads)
                    {
                        Cost = AbilityRules.RoadConstructionCost(ArmyHS.FArmy.FPlayer, moveSettings.FPath, moveSettings.FMovePoints);
                        if (Cost <= AoWHSMap.Current.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer).FGems)
                        {
                            if (ArmyHexSprite.MoveArmyEx(ArmyHS, GetSelection(), moveSettings.FPath, Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation, ArmyMoveModeElement.BuildRoads), false))
                            {
                                moveSettings.FPath.Clear();
                                if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                                {
                                    AoWHSMap.Current.Selection.UnselectLocation();
                                }

                                DeactivateMoveSelection();
                            }
                        }
                        else
                        {
                            var notEnoughGold = AbilityRules.TranslateRStr(GameText.NotEnoughGoldRStr);
                            AoWHSMap.Current.ShowWarning(notEnoughGold);
                        }
                    }
                    else
                    {
                        var item3 = moveSettings.FPath[0];
                        var item4 = moveSettings.FPath[0];
                        var item5 = moveSettings.FPath[0];
                        ArmyHS.FArmy.SetDestinationPos(item5.X, item4.Y, item3.L);
                        if (ArmyHexSprite.MoveArmy(ArmyHS, GetSelection(), moveSettings.FPath))
                        {
                            var movePoints = ArmyHS.FArmy.MovePoints(255);
                            if (moveSettings.FPath.MovePointsToPos(movePoints) > 0)
                            {
                                var army = ArmyHS.FArmy;
                                army.FUserMode = army.FUserMode + Set<ArmyUserModeElement>.Of(ArmyUserModeElement.TurnDone);
                            }

                            moveSettings.FPath.Clear();
                            if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                            {
                                AoWHSMap.Current.Selection.UnselectLocation();
                            }

                            DeactivateMoveSelection();
                        }
                    }
                }
                else
                {
                    if (((Field.FFieldFlags >> 16) & 255) != 0 || !AoWHSMap.Current.FFogEnabled)
                    {
                        var player = Field.GetPlayer();
                        var players = AoWHSMap.Current.FPlayers.GetPlayers(player);
                        var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[players.FPlayerRace]);
                        var format = AbilityRules.Format(AbilityRules.TranslateRStr(ArmyNotAtWarRStr), [raceNamesPlural]);
                        AoWHSMap.Current.ShowMessage(format);
                    }
                }
            }
        }
        else
        {
            if (FMoveSettingsList.Count > 1 && ExecuteMultiArmyMove(FMoveSettingsList))
            {
                for (int I = 0, last = FMoveSettingsList.Count - 1; I <= last; I++)
                {
                    var moveSettings = FMoveSettingsList.GetMoveSettings(I);
                    var item = moveSettings.FPath[0];
                    var fPathItem = moveSettings.FPath[0];
                    var item2 = moveSettings.FPath[0];
                    ((ArmyHexSprite)moveSettings.FMoveObject).FArmy.SetDestinationPos(item2.X, fPathItem.Y, item.L);
                    moveSettings.FPath.Clear();
                }

                Deactivate();
                AoWHSMap.Current.Selection.UnselectLocation();
                Clear();
            }
        }
    }

    // Army.PAS:4007
    public override void Deactivate()
    {
        base.Deactivate();
        FMode = Mode.Default;
    }

    // Army.PAS:3642
    public override void ShowMovePaths(in LockedSurface Surface)
    {
        if (this == AoWHSMap.Current.Mouse.Capture)
        {
            base.ShowMovePaths(Surface);
        }
    }

    // Army.PAS:3590
    public int AddMoveArmy(ArmyHexSprite ArmyHS, byte Selection)
    {
        int Index;
        Index = AddMoveObject(ArmyHS);
        FMoveSettingsList.GetMoveSettings(Index).FSelection = Selection;
        var movePoints = ArmyHS.FArmy.MovePoints(Selection);
        FMoveSettingsList.GetMoveSettings(Index).FMovePoints = movePoints;
        var moves = ArmyHS.FArmy.Moves(Selection);
        FMoveSettingsList.GetMoveSettings(Index).FMoves = moves;
        return Index;
    }

    // Army.PAS:3600
    public ArmyHexSprite GetArmyHS()
    {
        ArmyHexSprite Result;
        if (FCurrentMove != -1)
        {
            Result = (ArmyHexSprite)FMoveSettingsList.GetMoveSettings(FCurrentMove).FMoveObject;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Army.PAS:3607
    public byte GetSelection()
    {
        byte Result;
        if (FCurrentMove != -1)
        {
            Result = (byte)FMoveSettingsList.GetMoveSettings(FCurrentMove).FSelection;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Army.PAS:3614
    public void SetSelection(byte Selection)
    {
        int Points;
        if (FCurrentMove != -1)
        {
            Selection = GetArmyHS().FArmy.ValidateMoveSelection(Selection);
            if (FMode == Mode.BuildRoads)
            {
                Selection = (byte)(Selection | (1 << GetArmyHS().FArmy.IndexOfID(FRoadBuilderID)));
            }

            if (Selection != 0)
            {
                FMoveSettingsList.GetMoveSettings(FCurrentMove).FSelection = Selection;
                Points = GetArmyHS().FArmy.MovePoints(Selection);
                FMoveSettingsList.GetMoveSettings(FCurrentMove).FMovePoints = Points;
                var moves = GetArmyHS().FArmy.Moves(Selection);
                FMoveSettingsList.GetMoveSettings(FCurrentMove).FMoves = moves;
            }

            // { Store the signed move count directly. }
            Update();
        }
    }

    // Army.PAS:3634
    public Set<MoveType> GetMoveTypes()
    {
        Set<MoveType> Result;
        if (FCurrentMove != -1)
        {
            var moveSettings = FMoveSettingsList.GetMoveSettings(FCurrentMove);
            Result = ((ArmyHexSprite)FMoveSettingsList.GetMoveSettings(FCurrentMove).FMoveObject).FArmy.MoveTypes((byte)moveSettings.FSelection);
        }
        else
        {
            Result = Set<MoveType>.Of();
        }

        return Result;
    }

    // Army.PAS:3647
    public void ResetSelectedUnitGFX()
    {
        for (int I = FSelectedUnitGFX.Count - 1; I >= 0; I--)
        {
            var item = FSelectedUnitGFX[I];
            AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(item).Release();
            FSelectedUnitGFX.Delete(I);
        }
    }

    // Army.PAS:3657
    public void UpdateSelectedUnitGFX()
    {
        int Index;
        Int32List List;
        List = new Int32List();
        if (GetArmyHS() != null)
        {
            for (int I = 0, last = GetArmyHS().FArmy.Count - 1; I <= last; I++)
            {
                Index = GetArmyHS().FArmy.GetUnit(I).GetUnitGFXResourceIndex();
                List.Add(Index);
                AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(Index).AddRef();
            }
        }

        ResetSelectedUnitGFX();
        FSelectedUnitGFX.Assign(List);
        List?.Dispose();
    }

    // Army.PAS:3673
    public void OnDestroyArmy(Army Sender)
    {
        int Index;
        if (FStatus.Contains(MoveSelectorStatusElement.Activated))
        {
            if (Sender != null)
            {
                var armyHS = Sender.GetArmyHS();
                Index = FMoveSettingsList.IndexOfMoveObject(armyHS);
                if (Index != -1)
                {
                    DeleteMoveObject(Index);
                }
            }
            else
            {
                Deactivate();
            }
        }
    }

    // Army.PAS:3685
    public void RectangleSelectEvent(GameMapMouse Sender)
    {
        int X1 = default;
        int Y1 = default;
        int X2 = default;
        int Y2 = default;
        int Xhp = default;
        int Yhp = default;
        ArmyHSList Armies;
        Rect R;
        MapLevel Level;
        ArmyHexSprite ArmyHS;
        Armies = new ArmyHSList(null);
        R = Sender.FSelectionRectangle;
        Level = AoWHSMap.Current.MapContainer.GetMapLevel(AoWHSMap.Current.ViewL);
        HexGeometry.HPtoHX(R.Left, R.Top, ref X1, ref Y1);
        HexGeometry.HPtoHX(R.Right, R.Bottom, ref X2, ref Y2);
        for (int X = X1 - 1, last = X2 + 1; X <= last; X++)
        {
            for (int Y = Y1 - 1, lastY = Y2 + 1; Y <= lastY; Y++)
            {
                HexGeometry.HXtoHP(X, Y, ref Xhp, ref Yhp);
                Xhp += 16;
                Yhp += 16;
                if (X >= 0 && X < Level.Width && Y > 0 && Y < Level.Height && Xhp >= R.Left && Xhp < R.Right && Yhp >= R.Top && Yhp < R.Bottom)
                {
                    ArmyHS = ((AoWMapField)Level.GetField(X, Y)).GetArmyHS();
                    if (ArmyHS != null && ArmyHS.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer)
                    {
                        Armies.AddChild(ArmyHS);
                    }
                }
            }
        }

        if (Armies.Count != 0)
        {
            Clear();
            AoWHSMap.Current.Selection.UnselectLocation();
            for (int I = 0, last = Armies.Count - 1; I <= last; I++)
            {
                AddMoveArmy(Armies.GetArmyHS(I), 255);
                var armyHS = Armies.GetArmyHS(I);
                AoWHSMap.Current.Selection.SelectLocationHS(armyHS);
            }

            if (Armies.Count == 1)
            {
                SetCurrentMove(0);
            }
            else
            {
                SetCurrentMove(-1);
            }

            Update();
        }

        Armies?.Dispose();
    }

    // Army.PAS:3731
    public bool ValidMultiMoveSelection(AoWMapField Field)
    {
        sbyte Player;
        bool Valid;
        ArmyHexSprite ArmyHS;
        Player = Field.GetPlayer();
        Valid = Player == -1 || Player == AoWHSMap.Current.FSeatedPlayer;
        ArmyHS = Field.GetArmyHS();
        if (ArmyHS != null)
        {
            Valid = Valid && ArmyHS.FArmy.Transporter(255) == null;
        }

        return Valid && FMode != Mode.BuildRoads;
    }

    // Army.PAS:4002
    public void ActivateMoveSelection()
    {
        Activate(AoWHSMap.Current);
    }

    // Army.PAS:4013
    public void DeactivateMoveSelection()
    {
        Deactivate();
    }

    // Army.PAS:4018
    public void Update()
    {
        byte Selection;
        MapField Field;
        ArmyHexSprite ArmyHS;
        UpdateSelectedUnitGFX();
        if (GetArmyHS() != null)
        {
            var selection = GetSelection();
            GetArmyHS().FArmy.CreateMovePointTable(selection, Bytes.Of(ref FMoveControlSettings.FMovePointTable));
        }

        for (int I = 0, last = FMoveSettingsList.Count - 1; I <= last; I++)
        {
            Selection = (byte)FMoveSettingsList.GetMoveSettings(I).FSelection;
            ArmyHS = (ArmyHexSprite)FMoveSettingsList.GetMoveSettings(I).FMoveObject;
            var movePoints = ArmyHS.FArmy.MovePoints(Selection);
            FMoveSettingsList.GetMoveSettings(I).FMovePoints = movePoints;
            var moves = ArmyHS.FArmy.Moves(Selection);
            FMoveSettingsList.GetMoveSettings(I).FMoves = moves;
        }

        if (FStatus.Contains(MoveSelectorStatusElement.Activated))
        {
            for (int I = 0, last = FMoveSettingsList.Count - 1; I <= last; I++)
            {
                ArmyHS = (ArmyHexSprite)FMoveSettingsList.GetMoveSettings(I).FMoveObject;
                if (ArmyHS.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer)
                {
                    if (ArmyHS.FArmy.FDestinationX != -1 && ArmyHS.FArmy.FDestinationX != 0)
                    {
                        var army = ArmyHS.FArmy;
                        Field = AoWHSMap.Current.MapContainer.GetField(army.FDestinationX, army.FDestinationY, army.FDestinationL);
                        if (this.ValidateSelection((AoWMapField)Field))
                        {
                            CalculateMovePath(I, army.FDestinationX, army.FDestinationY, army.FDestinationL);
                        }
                        else
                        {
                            ArmyHS.FArmy.ResetDestinationPos();
                        }
                    }
                    else
                    {
                        if (FMoveSettingsList.GetMoveSettings(I).FPath.Count != 0)
                        {
                            var location = FMoveSettingsList.GetMoveSettings(I).FPath[0];
                            CalculateMovePath(I, location.X, location.Y, location.L);
                        }
                    }
                }
            }
        }

        UpdateMoveCursor();
    }

    // Army.PAS:4063
    public void OutputGroupInfo()
    {
        string Text;
        AIGroup Group;
        for (int I = 0, last = GetArmyHS().FArmy.Count - 1; I <= last; I++)
        {
            Text = GetArmyHS().FArmy.GetUnit(I).GetName() + "(ID:" + IntToStr(GetArmyHS().FArmy.GetUnit(I).FUnitID) + ")";
            Group = GetArmyHS().FArmy.GetUnit(I).FGroup;
            if (Group != null)
            {
                var behavior = Group.GroupClass.Behavior;
                Text = Text + TGroupBehaviorStr[(int)behavior] + "(GID:" + IntToStr(Group.FGroupID) + "/Target:" + IntToStr(Group.FTargetXYL.X) + "," + IntToStr(Group.FTargetXYL.Y) + "," + IntToStr(Group.FTargetXYL.L) + "/Budget:" + IntToStr(Group.FGoldBudget) + "/MP:" + IntToStr(Group.FTargetMovePoints) + ")";
                if (Group.FTargetType == AIGroupTargetStatus.Setup)
                {
                    Text = Text + "-setup";
                }
                else
                {
                    if (Group.FTargetType == AIGroupTargetStatus.Execute)
                    {
                        Text = Text + "-execute";
                    }
                }

                var unit = GetArmyHS().FArmy.GetUnit(I);
                if (Group.FLeader == unit)
                {
                    Text = Text + "/Leader";
                }
            }

            GameTables.AoWEngine.OutputDebugStr(Text);
        }

        var strengthMax = GetArmyHS().FArmy.GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(DamageType.None));
        GameTables.AoWEngine.OutputDebugStr("Army strength: " + IntToStr(strengthMax));
    }

    // Army.PAS:4090
    public void SetArmyHS(ArmyHexSprite ArmyHS)
    {
        if (ArmyHS != GetArmyHS() && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && FMoveSettingsList.Count <= 1)
        {
            Clear();
            FIgnoreMouseDown = false;
            FMode = Mode.Default;
            if (ArmyHS != null)
            {
                if (ArmyHS.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer)
                {
                    ActivateMoveSelection();
                }

                AddMoveArmy(ArmyHS, 255);
                FCurrentMove = 0;
                Update();
            }
            else
            {
                DeactivateMoveSelection();
                ResetSelectedUnitGFX();
            }

            if (GetArmyHS() != null && GameTables.AoWEngine.FRegistry.GetDebugMode())
            {
                OutputGroupInfo();
            }
        }
    }

    // Army.PAS:4114
    public void Select(ArmyHexSprite ArmyHS)
    {
        AoWHSMap.Current.Selection.UnselectLocation();
        if (ArmyHS != null)
        {
            AoWHSMap.Current.Selection.SelectLocationHS(ArmyHS);
        }
    }

    // Army.PAS:4120
    public void SelectBuildRoad(Troop AUnit)
    {
        ECustomNode Army;
        ArmyHexSprite ArmyHS;
        Army = AUnit.Owner;
        if (Army is Army)
        {
            ArmyHS = AUnit.ArmyOwner.GetArmyHS();
            ArmyHS.FArmy.ResetDestinationPos();
            if (ArmyHS != GetArmyHS())
            {
                AoWHSMap.Current.Selection.UnselectLocation();
                AoWHSMap.Current.Selection.SelectLocationHS(ArmyHS);
                SetSelection(255);
            }
            else
            {
                DeleteMoves();
                AddMoveArmy(ArmyHS, 255);
                FCurrentMove = 0;
            }

            var selectRoadConstructionPath = AbilityRules.TranslateRStr(SelectRoadConstructionPathRStr);
            AoWHSMap.Current.ShowMessage(selectRoadConstructionPath);
            FRoadBuilderID = AUnit.FUnitID;
            FMode = Mode.BuildRoads;
        }
    }

    // Army.PAS:4146
    public void Center()
    {
        if (GetArmyHS() != null)
        {
            var aoWHSMap = AoWHSMap.Current;
            var lhx = GetArmyHS().GetLhx();
            var yhx = GetArmyHS().GetYhx();
            aoWHSMap.CenterViewTo(GetArmyHS().GetXhx(), yhx, lhx);
        }
    }

    // Army.PAS:4153
    public void TurnDone()
    {
        if (GetArmyHS() != null && GetArmyHS().FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer)
        {
            var armyHS = GetArmyHS();
            GetArmyHS().FArmy.FUserMode = armyHS.FArmy.FUserMode + Set<ArmyUserModeElement>.Of(ArmyUserModeElement.TurnDone);
            AoWHSMap.Current.SelectNextArmy();
        }
    }

    // Army.PAS:4162
    public void ToggleGuard()
    {
        ArmyHexSprite ArmyHS;
        Army Army;
        ArmyHS = GetArmyHS();
        if (ArmyHS != null && ArmyHS.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer)
        {
            Army = ArmyHS.FArmy;
            if (ArmyHS.FArmy.FUserMode.Contains(ArmyUserModeElement.Guard))
            {
                Army.FUserMode = ArmyHS.FArmy.FUserMode - Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
                Army.Update();
            }
            else
            {
                Army.FUserMode = ArmyHS.FArmy.FUserMode + Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
                AoWHSMap.Current.SelectNextArmy();
            }
        }
    }

    // intf_P2_02.inc:23
    public override void UpdateMoveCursor()
    {
        var map = AoWHSMap.Current;
        var mouse = map.Mouse;
        if (mouse.Xhx == -1 || mouse.Yhx == -1 || mouse.Capture != this) return;
        var field = (AoWMapField)map.MapContainer.GetField(mouse.Xhx, mouse.Yhx, map.ViewL);
        bool visible = !map.FFogEnabled || ((field.FFieldFlags >> 16) & 255) != 0;
        if (FCurrentMove != -1 && (!map.FExplorationEnabled || field.GetExplored(map.FSeatedPlayer)) && visible)
            mouse.CursorMode = field.GetMoveAction(GetArmyHS().FArmy.FPlayer) switch
            {
                MoveAction.Attack => 258,
                MoveAction.Negotiate => 259,
                _ => 256
            };
        else mouse.CursorMode = FMoveSettingsList.Count <= 1 ? 256 : 0;
        if (mouse.CursorMode != 256) return;
        var types = GetMoveTypes();
        if (types.Contains(MoveType.Walking)) mouse.Cursor = 64;
        else if (types.Contains(MoveType.Swimming)) mouse.Cursor = 66;
        else if (types.Contains(MoveType.Flying)) mouse.Cursor = 68;
        int cost = FMoveControlSettings.FMovePointTable[field.TerrainType][field.OverlayType + 1];
        if (cost == 0 || map.FExplorationEnabled && !field.GetExplored(map.FSeatedPlayer)) mouse.Cursor |= 1;
    }

    private static StringArray19 TGroupBehaviorStr = StringArray19.From(default(ReadOnlySpan<string>));
    private static readonly string SelectRoadConstructionPathRStr = "Select road construction path";
    private static readonly string ArmyNotEnoughMovePointsRStr = "Not enough move points";
    private static readonly string ArmyRoadConstructionCostRStr = "Constructing road will cost %s gold";
    private static readonly string ArmyNotAtWarRStr = "Not at war with the %s";

    private static bool ExecuteMultiArmyMove(MoveSettingsList Moves)
    {
        bool flag = false;
        MultiMoveTokenEvent tMultiMoveTE = null;
        MovePath tMovepath = null;
        MoveSettings tMoveSettings = null;
        ArmyDefaultMoveTokenEvent tArmyDefaultMoveTE = null;
        bool flag2 = Moves.Count != 0;
        if (flag2)
        {
            MoveSettings moveSettings = Moves.GetMoveSettings(0);
            flag2 = AoWHSMap.Current.FTokenManager.PlayerReady(((ArmyHexSprite)moveSettings.FMoveObject).FArmy.FPlayer);
        }
        if (flag2)
        {
            tMovepath = new MovePath();
            tMultiMoveTE = new MultiMoveTokenEvent(null);
            tMultiMoveTE.FSimultaneous = false;
            Moves.CalculateMovePositions(ValidateMultiArmyMove);
            int i = 0;
            for (int num = Moves.Count - 1; i <= num; i++)
            {
                tMoveSettings = Moves.GetMoveSettings(i);
                if (tMoveSettings.FMovePos < tMoveSettings.FPath.Count - 1)
                {
                    tMovepath.Clear();
                    int j = tMoveSettings.FMovePos;
                    for (int num2 = tMoveSettings.FPath.Count - 1; j <= num2; j++)
                    {
                        tMovepath.Add(tMoveSettings.FPath[j]);
                    }
                    tArmyDefaultMoveTE = new ArmyDefaultMoveTokenEvent(null);
                    MapCoordinate tXYL = tMovepath[0];
                    MapCoordinate tXYL2 = tMovepath[0];
                    MapCoordinate tXYL3 = tMovepath[0];
                    GameTables.AoWEngine.OutputDebugStr(string.Concat(new string[6]
                    {
                        "Moving to: ",
                        RtlUnit.IntToStr((int)tXYL.X),
                        ",",
                        RtlUnit.IntToStr((int)tXYL2.Y),
                        ",",
                        RtlUnit.IntToStr((int)tXYL3.L)
                    }));
                    if (tArmyDefaultMoveTE.Setup((ArmyHexSprite)tMoveSettings.FMoveObject, (byte)tMoveSettings.FSelection, tMovepath, Set<ArmyMoveModeElement>.Of(Array.Empty<ArmyMoveModeElement>())))
                    {
                        tMultiMoveTE.FEvents.Add(tArmyDefaultMoveTE);
                    }
                    tArmyDefaultMoveTE.Release();
                }
            }
            if (tMultiMoveTE.FEvents.Count != 0)
            {
                MoveSettings moveSettings2 = Moves.GetMoveSettings(0);
                AoWHSMap.Current.FTokenManager.GetTokenControl(((ArmyHexSprite)moveSettings2.FMoveObject).FArmy.FPlayer).AddEvent(tMultiMoveTE);
                flag = true;
            }
            else
            {
                flag = false;
            }
            tMultiMoveTE.Release();
            if (tMovepath != null)
            {
                ((Persistent)tMovepath).Dispose();
            }
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    private static bool ValidateMultiArmyMove(MoveSettingsList Moves, int Index)
    {
        bool flag = true;
        sbyte fPlayer = ((ArmyHexSprite)Moves.GetMoveSettings(Index).FMoveObject).FArmy.FPlayer;
        MoveSettings moveSettings = Moves.GetMoveSettings(Index);
        int num = moveSettings.FMovePos;
        while (flag && num != moveSettings.FPath.Count)
        {
            MapCoordinate tXYL = moveSettings.FPath[num];
            MapCoordinate tXYL2 = moveSettings.FPath[num];
            MapCoordinate tXYL3 = moveSettings.FPath[num];
            AoWMapField tAoWMapField = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(tXYL3.X, tXYL2.Y, tXYL.L);
            sbyte player = tAoWMapField.GetPlayer();
            flag = player == -1 || player == fPlayer;
            num++;
        }
        int UnitCount;
        int Transporters;
        int Capacity;
        if (flag)
        {
            UnitCount = 0;
            Transporters = 0;
            Capacity = 8;
            MoveSettings moveSettings2 = Moves.GetMoveSettings(Index);
            AddArmy((ArmyHexSprite)Moves.GetMoveSettings(Index).FMoveObject, (byte)moveSettings2.FSelection);
            MapCoordinate moveLoc = Moves.GetMoveSettings(Index).GetMoveLoc();
            AoWMapField tAoWMapField = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(moveLoc.X, moveLoc.Y, moveLoc.L);
            ArmyHexSprite tArmyHS = (ArmyHexSprite)tAoWMapField.FindOwnedChild(131607);
            if (tArmyHS != null)
            {
                AddArmy(tArmyHS, 255);
            }
            for (num = Moves.Count - 1; num >= 0; num--)
            {
                if (num != Index)
                {
                    moveSettings = Moves.GetMoveSettings(num);
                    bool flag2 = moveSettings.FValid && Moves.GetMoveSettings(Index).GetMovePrio() >= moveSettings.GetMovePrio();
                    if (flag2)
                    {
                        MapCoordinate moveLoc2 = Moves.GetMoveSettings(Index).GetMoveLoc();
                        flag2 = Unsafe.BitCast<MapCoordinate, int>(moveSettings.GetMoveLoc()) == Unsafe.BitCast<MapCoordinate, int>(moveLoc2);
                    }
                    if (flag2)
                    {
                        AddArmy((ArmyHexSprite)moveSettings.FMoveObject, (byte)moveSettings.FSelection);
                    }
                }
            }
            flag = UnitCount <= Capacity && Transporters <= 1;
        }
        return flag;
        [CompilerGenerated]
        void AddArmy(ArmyHexSprite ArmyHS, byte Selection)
        {
            int num2 = ArmyHS.FArmy.SelectionCount(Selection);
            UnitCount += num2;
            Troop tAbstractUnit = ArmyHS.FArmy.Transporter(Selection);
            if (tAbstractUnit != null)
            {
                Transporters++;
                if (tAbstractUnit.GetTransportCapacity() + 1 < Capacity)
                {
                    Capacity = tAbstractUnit.GetTransportCapacity() + 1;
                }
            }
        }
    }

    public enum Mode : byte
    {
        Default = 0, BuildRoads = 1
    }
}
