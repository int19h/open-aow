#nullable disable
// Translated from AoWE.pas.
using System.Collections.Immutable;
using System.Globalization;
using System.Runtime.CompilerServices;
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

namespace Aow.Game.Units;

// { Interface declaration run owned by P2. }
// mode, road builder ID, selected graphics references, ignore next mouse-down
public sealed class SelectedArmy(EngineObject creator) : MoveSelector(creator)
{

    private SelectedArmyMode _mode;
    private int _roadBuilderId;
    private readonly Int32List _selectedUnitGraphics = new();

    public bool IgnoreMouseDown { get; set; }

    // Army.PAS:3573
    public override void Dispose()
    {
        ResetSelectedUnitGraphics();
        _selectedUnitGraphics?.Dispose();
        base.Dispose();
    }

    // Army.PAS:3837
    public override void MouseDownEvent(object sender)
    {
        if (!IgnoreMouseDown)
        {
            ArmyHexSprite armyHexSprite;
            int index;
            if (StrategicMap.Current.Mouse.Shift.Contains(ShiftStates.Shift))
            {
                if (MoveSettingsList.Count == 1 && MoveSettingsList.GetMoveSettings(0).Path.Count != 0)
                {
                    PathStep item = MoveSettingsList.GetMoveSettings(0).Path[0];
                    PathStep fPathItem = MoveSettingsList.GetMoveSettings(0).Path[0];
                    PathStep item2 = MoveSettingsList.GetMoveSettings(0).Path[0];
                    if (!ValidMultiMoveSelection(Map.MapContainer.GetField(item2.Location.X, fPathItem.Location.Y, item.Location.MapLevel)))
                    {
                        return;
                    }
                }

                if (StrategicMap.Current.Mouse.HexX != -1)
                {
                    int hexY = StrategicMap.Current.Mouse.HexY;
                    int hexX = StrategicMap.Current.Mouse.HexX;
                    armyHexSprite = StrategicMap.Current.MapContainer.GetField(hexX, hexY, StrategicMap.Current.ViewMapLevel).GetArmyHexSprite();
                    if (armyHexSprite is not null && armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
                    {
                        index = MoveSettingsList.IndexOfMoveObject(armyHexSprite);
                        if (index == -1)
                        {
                            _ = AddMoveArmy(armyHexSprite, selection: 255);
                            Update();
                            StrategicMap.Current.Selection.SelectLocationHexSprite(armyHexSprite);
                            SetCurrentMove(-1);
                        }
                        else
                        {
                            armyHexSprite.Unselect();
                            DeleteMoveObject(index);
                            if (MoveSettingsList.Count == 1)
                            {
                                SetCurrentMove(0);
                            }
                        }
                    }
                }
            }
            else
            {
                if (StrategicMap.Current.Mouse.Shift.Contains(ShiftStates.Ctrl))
                {
                    if (MoveSettingsList.Count > 1 && StrategicMap.Current.Mouse.HexX != -1)
                    {
                        int hexY = StrategicMap.Current.Mouse.HexY;
                        int hexX = StrategicMap.Current.Mouse.HexX;
                        armyHexSprite = StrategicMap.Current.MapContainer.GetField(hexX, hexY, StrategicMap.Current.ViewMapLevel).GetArmyHexSprite();
                        if (armyHexSprite is not null && armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
                        {
                            index = MoveSettingsList.IndexOfMoveObject(armyHexSprite);
                            if (index != -1)
                            {
                                if (index != CurrentMove)
                                {
                                    SetCurrentMove(index);
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
                    if (StrategicMap.Current.Mouse.Capture != StrategicMap.Current.TargetSelection && (StrategicMap.Current.Mouse.Capture != this || Map.Mouse.Button == MouseButton.Right))
                    {
                        Unselect();
                    }
                    else
                    {
                        if (StrategicMap.Current.Mouse.Capture == this && StrategicMap.Current.Mouse.Button == MouseButton.Left)
                        {
                            int hexY = Map.Mouse.HexY;
                            SelectPath((sbyte)Map.Mouse.HexX, (sbyte)hexY, L: (sbyte)Map.ViewMapLevel);
                        }
                    }
                }
            }
        }
        else
        {
            IgnoreMouseDown = false;
        }
    }

    // Army.PAS:3751
    public override void CalculateMovePath(int index, sbyte X, sbyte Y, sbyte L)
    {
        if (X != -1)
        {
            MoveSettings settings = MoveSettingsList.GetMoveSettings(index);
            ArmyHexSprite armyHexSprite = (ArmyHexSprite)settings.MoveObject;
            settings.PathMovePoints = 0;
            MoveControlSettings.Reset();
            sbyte hexLevel = armyHexSprite.HexLevel;
            sbyte hexY = armyHexSprite.HexY;
            sbyte hexX = armyHexSprite.HexX;
            MoveControlSettings.SetSourcePosition(hexX, hexY, hexLevel);
            MoveControlSettings.Player = armyHexSprite.Army.Player;
            MoveControlSettings.ArmyHexSprite = armyHexSprite;
            MoveControlSettings.SetDestinationPosition(X, Y, L);
            MoveControlSettings.Units.AddUnits(armyHexSprite.Army.Units, selection: (byte)settings.Selection);
            if (StrategicMap.Current.ExplorationEnabled)
            {
                MoveControlSettings.Mode |= MoveModes.ExploredTerrainOnly;
            }

            _ = MoveControlSettings.MovePointTable;
            MovePointTable movePointTable = armyHexSprite.Army.CreateMovePointTable((byte)settings.Selection);
            MoveControlSettings.MovePointTable = movePointTable;
            StrategicMap.Current.MoveControl.ProcessMove(MoveControlSettings);
            if (MoveControlSettings.Path.Count != 0)
            {
                settings.Path.Assign(MoveControlSettings.Path);
                settings.PathMovePoints = settings.Path.PositionToMovePoints(0);
                PathStep item = settings.Path[0];
                PathStep fPathItem = settings.Path[0];
                PathStep item2 = settings.Path[0];
                armyHexSprite.Army.SetDestinationPosition(item2.Location.X, fPathItem.Location.Y, L: item.Location.MapLevel);
                MoveControlSettings.Path.Clear();
                if (_mode == SelectedArmyMode.BuildRoads)
                {
                    if (settings.MovePoints < settings.PathMovePoints)
                    {
                        settings.Path.Clear();
                        string armyNotEnoughMovePoints = AbilityRules.TranslateResourceString(Creator.GetEngine(), S: s_armyNotEnoughMovePointsText);
                        StrategicMap.Current.ShowMessage(armyNotEnoughMovePoints);
                    }
                    else
                    {
                        if (settings.Path.Count != 0)
                        {
                            int roadConstructionCost = AbilityRules.RoadConstructionCost(ArmyHexSprite.Army.Player, settings.Path, settings.MovePoints);
                            string format = AbilityRules.Format(AbilityRules.TranslateResourceString(Creator.GetEngine(), S: s_armyRoadConstructionCostFormat), arguments: [roadConstructionCost.ToString(CultureInfo.InvariantCulture)]);
                            StrategicMap.Current.ShowMessage(format);
                        }
                    }
                }
            }
            else
            {
                MoveSettingsList.GetMoveSettings(index).Path.Clear();
                armyHexSprite.Army.ResetDestinationPosition();
            }
        }
    }

    // Army.PAS:3580
    public override void SetCurrentMove(int value)
    {
        if (CurrentMove != -1 && ArmyHexSprite is not null)
        {
            ArmyHexSprite armyHexSprite = ArmyHexSprite;
            StrategicMap.Current.Selection.TriggerDeselectEvent(armyHexSprite);
        }

        CurrentMove = value;
        if (CurrentMove != -1 && ArmyHexSprite is not null)
        {
            ArmyHexSprite armyHexSprite = ArmyHexSprite;
            StrategicMap.Current.Selection.TriggerSelectEvent(armyHexSprite);
        }

        UpdateSelectedUnitGraphics();
    }

    // Army.PAS:3795
    public override void SelectPath(sbyte X, sbyte Y, sbyte L)
    {
        if (ArmyHexSprite is not null && ArmyHexSprite.HexX == StrategicMap.Current.Mouse.HexX && ArmyHexSprite.HexY == StrategicMap.Current.Mouse.HexY && ArmyHexSprite.HexLevel == StrategicMap.Current.ViewMapLevel)
        {
            sbyte hexLevel = ArmyHexSprite.HexLevel;
            sbyte hexY = ArmyHexSprite.HexY;
            sbyte hexX = ArmyHexSprite.HexX;
            StrategicMap.Current.Selection.SelectLocationHex(hexX, hexY, hexLevel);
        }
        else
        {
            if (MoveSettingsList.Count > 1 && CurrentMove == -1)
            {
                bool valid = true;
                int i = MoveSettingsList.Count;
                while (valid && i != 0)
                {
                    i -= 1;
                    MoveSettings settings = MoveSettingsList.GetMoveSettings(i);
                    valid = settings.Path.Count != 0 && settings.Path[0].Location.X == X && settings.Path[0].Location.Y == Y && settings.Path[0].Location.MapLevel == L;
                }

                if (!valid)
                {
                    if (ValidateSelection(Map.MapContainer.GetField(X, Y, L)))
                    {
                        i = 0;
                        int last = MoveSettingsList.Count - 1;
                        for (; i <= last; i += 1)
                        {
                            CalculateMovePath(i, X, Y, L);
                        }
                    }
                    else
                    {
                        i = 0;
                        int last = MoveSettingsList.Count - 1;
                        for (; i <= last; i += 1)
                        {
                            MoveSettingsList.GetMoveSettings(i).Path.Clear();
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
    public override bool ValidateSelection(MapField field)
    {
        bool result = MoveSettingsList.Count > 1 ? ValidMultiMoveSelection(field) : base.ValidateSelection(field);
        if (_mode == SelectedArmyMode.BuildRoads)
        {
            if (field.GetPlayer() != -1 && field.GetPlayer() != ArmyHexSprite.Army.Player)
            {
                return false;
            }
        }

        return result;
    }

    // Army.PAS:3904
    public override void ExecuteMove()
    {
        if (MoveSettingsList.Count == 1)
        {
            MoveSettings moveSettings = MoveSettingsList.GetMoveSettings(CurrentMove);
            if (moveSettings.Path.Count > 1 && ArmyHexSprite is not null)
            {
                ArmyHexSprite armyHexSprite = ArmyHexSprite;
                PathStep item = moveSettings.Path[0];
                PathStep fPathItem = moveSettings.Path[0];
                PathStep item2 = moveSettings.Path[0];
                MapField field = StrategicMap.Current.MapContainer.GetField(item2.Location.X, fPathItem.Location.Y, item.Location.MapLevel);
                bool condition = field.GetMoveAction(armyHexSprite.Army.Player) != MoveAction.Attack || field.GetPlayer() <= 0;
                if (!condition)
                {
                    sbyte player = field.GetPlayer();
                    DiplomaticRelation diplomaticRelation = StrategicMap.Current.Players.GetDiplomaticRelation(armyHexSprite.Army.Player, otherPlayer: player);
                    condition = diplomaticRelation is DiplomaticRelation.War or DiplomaticRelation.Unknown;
                }

                if (condition)
                {
                    if (_mode == SelectedArmyMode.BuildRoads)
                    {
                        int cost = AbilityRules.RoadConstructionCost(armyHexSprite.Army.Player, moveSettings.Path, moveSettings.MovePoints);
                        if (cost <= StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).Gems)
                        {
                            if (ArmyHexSprite.MoveArmy(armyHexSprite, Selection, moveSettings.Path, mode: ArmyMoveModes.SelectEndLocation | ArmyMoveModes.BuildRoads, force: false))
                            {
                                moveSettings.Path.Clear();
                                if (StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
                                {
                                    StrategicMap.Current.Selection.UnselectLocation();
                                }

                                DeactivateMoveSelection();
                            }
                        }
                        else
                        {
                            string notEnoughGold = AbilityRules.TranslateResourceString(Creator.GetEngine(), S: GameText.NotEnoughGoldText);
                            StrategicMap.Current.ShowWarning(notEnoughGold);
                        }
                    }
                    else
                    {
                        PathStep item3 = moveSettings.Path[0];
                        PathStep item4 = moveSettings.Path[0];
                        PathStep item5 = moveSettings.Path[0];
                        armyHexSprite.Army.SetDestinationPosition(item5.Location.X, item4.Location.Y, L: item3.Location.MapLevel);
                        if (ArmyHexSprite.MoveArmy(armyHexSprite, Selection, moveSettings.Path))
                        {
                            sbyte movePoints = armyHexSprite.Army.MovePoints(255);
                            if (moveSettings.Path.MovePointsToPosition(movePoints) > 0)
                            {
                                Army army = armyHexSprite.Army;
                                army.UserMode |= ArmyUserModes.TurnDone;
                            }

                            moveSettings.Path.Clear();
                            if (StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
                            {
                                StrategicMap.Current.Selection.UnselectLocation();
                            }

                            DeactivateMoveSelection();
                        }
                    }
                }
                else
                {
                    if (field.FogCount != 0 || !StrategicMap.Current.FogEnabled)
                    {
                        sbyte player = field.GetPlayer();
                        Player players = StrategicMap.Current.Players.GetPlayers(player);
                        string raceNamesPlural = AbilityRules.TranslateResourceString(Creator.GetEngine(), S: GameTables.RaceNamesPluralTexts[players.PlayerRace]);
                        string format = AbilityRules.Format(AbilityRules.TranslateResourceString(Creator.GetEngine(), S: s_armyNotAtWarFormat), arguments: [raceNamesPlural]);
                        StrategicMap.Current.ShowMessage(format);
                    }
                }
            }
        }
        else
        {
            if (MoveSettingsList.Count > 1 && ExecuteMultiArmyMove(MoveSettingsList))
            {
                for (int i = 0, last = MoveSettingsList.Count - 1; i <= last; i++)
                {
                    MoveSettings moveSettings = MoveSettingsList.GetMoveSettings(i);
                    PathStep item = moveSettings.Path[0];
                    PathStep fPathItem = moveSettings.Path[0];
                    PathStep item2 = moveSettings.Path[0];
                    ((ArmyHexSprite)moveSettings.MoveObject).Army.SetDestinationPosition(item2.Location.X, fPathItem.Location.Y, L: item.Location.MapLevel);
                    moveSettings.Path.Clear();
                }

                Deactivate();
                StrategicMap.Current.Selection.UnselectLocation();
                Clear();
            }
        }
    }

    // Army.PAS:4007
    public override void Deactivate()
    {
        base.Deactivate();
        _mode = SelectedArmyMode.Default;
    }

    // Army.PAS:3642
    public override void ShowMovePaths(in LockedSurface surface)
    {
        if (this == StrategicMap.Current.Mouse.Capture)
        {
            base.ShowMovePaths(surface);
        }
    }

    // Army.PAS:3590
    public int AddMoveArmy(ArmyHexSprite armyHexSprite, byte selection)
    {
        int index = AddMoveObject(armyHexSprite);
        MoveSettingsList.GetMoveSettings(index).Selection = selection;
        sbyte movePoints = armyHexSprite.Army.MovePoints(selection);
        MoveSettingsList.GetMoveSettings(index).MovePoints = movePoints;
        sbyte moves = armyHexSprite.Army.Moves(selection);
        MoveSettingsList.GetMoveSettings(index).Moves = moves;
        return index;
    }

    // Army.PAS:3600
    public ArmyHexSprite ArmyHexSprite => CurrentMove != -1 ? (ArmyHexSprite)MoveSettingsList.GetMoveSettings(CurrentMove).MoveObject : null;

    // Army.PAS:3607
    public byte Selection => CurrentMove != -1 ? (byte)MoveSettingsList.GetMoveSettings(CurrentMove).Selection : (byte)0;

    // Army.PAS:3614
    public void SetSelection(byte selection)
    {
        if (CurrentMove != -1)
        {
            selection = ArmyHexSprite.Army.ValidateMoveSelection(selection);
            if (_mode == SelectedArmyMode.BuildRoads)
            {
                selection = (byte)(selection | (1 << ArmyHexSprite.Army.Units.IndexOfId(_roadBuilderId)));
            }

            if (selection != 0)
            {
                MoveSettingsList.GetMoveSettings(CurrentMove).Selection = selection;
                int points = ArmyHexSprite.Army.MovePoints(selection);
                MoveSettingsList.GetMoveSettings(CurrentMove).MovePoints = points;
                sbyte moves = ArmyHexSprite.Army.Moves(selection);
                MoveSettingsList.GetMoveSettings(CurrentMove).Moves = moves;
            }

            // { Store the signed move count directly. }
            Update();
        }
    }

    // Army.PAS:3634
    public MoveTypes GetMoveTypes()
    {
        if (CurrentMove != -1)
        {
            MoveSettings moveSettings = MoveSettingsList.GetMoveSettings(CurrentMove);
            return ((ArmyHexSprite)MoveSettingsList.GetMoveSettings(CurrentMove).MoveObject).Army.MoveTypes((byte)moveSettings.Selection);
        }
        else
        {
            return MoveTypes.None;
        }
    }

    // Army.PAS:3647
    public void ResetSelectedUnitGraphics()
    {
        for (int i = _selectedUnitGraphics.Count - 1; i >= 0; i--)
        {
            int item = _selectedUnitGraphics[i];
            _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(item).Release();
            _selectedUnitGraphics.Delete(i);
        }
    }

    // Army.PAS:3657
    public void UpdateSelectedUnitGraphics()
    {
        Int32List list = new();
        if (ArmyHexSprite is not null)
        {
            for (int i = 0, last = ArmyHexSprite.Army.Units.Count - 1; i <= last; i++)
            {
                int index = ArmyHexSprite.Army.Units.GetUnit(i).UnitGraphicsResourceIndex;
                _ = list.Add(index);
                _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(index).AddRef();
            }
        }

        ResetSelectedUnitGraphics();
        _selectedUnitGraphics.Assign(list);
        list?.Dispose();
    }

    // Army.PAS:3673
    public void OnDestroyArmy(Army sender)
    {
        if (Status.Contains(MoveSelectorStatuses.Activated))
        {
            if (sender is not null)
            {
                ArmyHexSprite armyHexSprite = sender.GetArmyHexSprite();
                int index = MoveSettingsList.IndexOfMoveObject(armyHexSprite);
                if (index != -1)
                {
                    DeleteMoveObject(index);
                }
            }
            else
            {
                Deactivate();
            }
        }
    }

    // Army.PAS:3685
    public void RectangleSelectEvent(GameMapMouse sender)
    {
        ArmyHSList armies = new(null) { Holder = Creator.CreationHolder() };
        Rect r = sender.SelectionRectangle;
        MapLevel level = StrategicMap.Current.MapContainer.GetMapLevel(StrategicMap.Current.ViewMapLevel);
        HexGeometry.PixelToHex(r.Left, vertical: r.Top, out int x1, out int y1);
        HexGeometry.PixelToHex(r.Right, vertical: r.Bottom, out int x2, out int y2);
        for (int x = x1 - 1, last = x2 + 1; x <= last; x++)
        {
            for (int y = y1 - 1, lastY = y2 + 1; y <= lastY; y++)
            {
                HexGeometry.HexToPixel(x, y, out int pixelX, out int pixelY);
                pixelX += 16;
                pixelY += 16;
                if (x >= 0 && x < level.Width && y > 0 && y < level.Height && pixelX >= r.Left && pixelX < r.Right && pixelY >= r.Top && pixelY < r.Bottom)
                {
                    ArmyHexSprite armyHexSprite = level.GetField(x, y).GetArmyHexSprite();
                    if (armyHexSprite is not null && armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
                    {
                        _ = armies.AddChild(armyHexSprite);
                    }
                }
            }
        }

        if (armies.Count != 0)
        {
            Clear();
            StrategicMap.Current.Selection.UnselectLocation();
            for (int i = 0, last = armies.Count - 1; i <= last; i++)
            {
                _ = AddMoveArmy(armies[i], selection: 255);
                ArmyHexSprite armyHS = armies[i];
                StrategicMap.Current.Selection.SelectLocationHexSprite(armyHS);
            }

            if (armies.Count == 1)
            {
                SetCurrentMove(0);
            }
            else
            {
                SetCurrentMove(-1);
            }

            Update();
        }

        armies?.Dispose();
    }

    // Army.PAS:3731
    public bool ValidMultiMoveSelection(MapField field)
    {
        sbyte player = field.GetPlayer();
        bool valid = player == -1 || player == StrategicMap.Current.SeatedPlayer;
        ArmyHexSprite armyHexSprite = field.GetArmyHexSprite();
        if (armyHexSprite is not null)
        {
            valid = valid && armyHexSprite.Army.Transporter(255) is null;
        }

        return valid && _mode != SelectedArmyMode.BuildRoads;
    }

    // Army.PAS:4002
    public void ActivateMoveSelection() => Activate(StrategicMap.Current);

    // Army.PAS:4013
    public void DeactivateMoveSelection() => Deactivate();

    // Army.PAS:4018
    public void Update()
    {
        UpdateSelectedUnitGraphics();
        if (ArmyHexSprite is not null)
        {
            byte selection = Selection;
            _ = MoveControlSettings.MovePointTable;
            MovePointTable movePointTable = ArmyHexSprite.Army.CreateMovePointTable(selection);
            MoveControlSettings.MovePointTable = movePointTable;
        }

        ArmyHexSprite armyHexSprite;
        for (int i = 0, last = MoveSettingsList.Count - 1; i <= last; i++)
        {
            byte selection2 = (byte)MoveSettingsList.GetMoveSettings(i).Selection;
            armyHexSprite = (ArmyHexSprite)MoveSettingsList.GetMoveSettings(i).MoveObject;
            sbyte movePoints = armyHexSprite.Army.MovePoints(selection2);
            MoveSettingsList.GetMoveSettings(i).MovePoints = movePoints;
            sbyte moves = armyHexSprite.Army.Moves(selection2);
            MoveSettingsList.GetMoveSettings(i).Moves = moves;
        }

        if (Status.Contains(MoveSelectorStatuses.Activated))
        {
            for (int i = 0, last = MoveSettingsList.Count - 1; i <= last; i++)
            {
                armyHexSprite = (ArmyHexSprite)MoveSettingsList.GetMoveSettings(i).MoveObject;
                if (armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
                {
                    if (armyHexSprite.Army.Destination.X is not -1 and not 0)
                    {
                        Army army = armyHexSprite.Army;
                        MapField field = StrategicMap.Current.MapContainer.GetField(army.Destination.X, army.Destination.Y, army.Destination.MapLevel);
                        if (ValidateSelection(field))
                        {
                            CalculateMovePath(i, army.Destination.X, army.Destination.Y, L: army.Destination.MapLevel);
                        }
                        else
                        {
                            armyHexSprite.Army.ResetDestinationPosition();
                        }
                    }
                    else
                    {
                        if (MoveSettingsList.GetMoveSettings(i).Path.Count != 0)
                        {
                            PathStep location = MoveSettingsList.GetMoveSettings(i).Path[0];
                            CalculateMovePath(i, location.Location.X, location.Location.Y, L: location.Location.MapLevel);
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
        for (int i = 0, last = ArmyHexSprite.Army.Units.Count - 1; i <= last; i++)
        {
            string text = $"{ArmyHexSprite.Army.Units.GetUnit(i).GetName()}(ID:{ArmyHexSprite.Army.Units.GetUnit(i).UnitId.ToString(CultureInfo.InvariantCulture)})";
            AIGroup group = ArmyHexSprite.Army.Units.GetUnit(i).Group;
            if (group is not null)
            {
                GroupBehavior behavior = group.GroupClass.Behavior;
                MapCoordinate target = group.TargetLocation ?? MapCoordinate.None;
                text = $"{text}{s_groupBehaviorTexts[(int)behavior]}(GID:{group.GroupId.ToString(CultureInfo.InvariantCulture)}/Target:{((int)target.X).ToString(CultureInfo.InvariantCulture)},{((int)target.Y).ToString(CultureInfo.InvariantCulture)},{((int)target.MapLevel).ToString(CultureInfo.InvariantCulture)}/Budget:{group.GoldBudget.ToString(CultureInfo.InvariantCulture)}/MP:{group.TargetMovePoints.ToString(CultureInfo.InvariantCulture)})";
                if (group.TargetType == AIGroupTargetStatus.Setup)
                {
                    text += "-setup";
                }
                else
                {
                    if (group.TargetType == AIGroupTargetStatus.Execute)
                    {
                        text += "-execute";
                    }
                }

                Troop troop = ArmyHexSprite.Army.Units.GetUnit(i);
                if (group.Leader == troop)
                {
                    text += "/Leader";
                }
            }

            Creator.GetEngine().OutputDebugString(text);
        }

        int strengthMax = ArmyHexSprite.Army.Units.GetStrengthMax(AbilityRules.AllDamageTypes & ~DamageTypes.NonSpecific);
        Creator.GetEngine().OutputDebugString($"Army strength: {strengthMax.ToString(CultureInfo.InvariantCulture)}");
    }

    // Army.PAS:4090
    public void SetArmyHexSprite(ArmyHexSprite armyHexSprite)
    {
        if (armyHexSprite != ArmyHexSprite && !StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) && MoveSettingsList.Count <= 1)
        {
            Clear();
            IgnoreMouseDown = false;
            _mode = SelectedArmyMode.Default;
            if (armyHexSprite is not null)
            {
                if (armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
                {
                    ActivateMoveSelection();
                }

                _ = AddMoveArmy(armyHexSprite, selection: 255);
                CurrentMove = 0;
                Update();
            }
            else
            {
                DeactivateMoveSelection();
                ResetSelectedUnitGraphics();
            }

            if (ArmyHexSprite is not null && Creator.GetEngine().Registry.GetDebugMode())
            {
                OutputGroupInfo();
            }
        }
    }

    // Army.PAS:4114
    public void Select(ArmyHexSprite armyHexSprite)
    {
        StrategicMap.Current.Selection.UnselectLocation();
        if (armyHexSprite is not null)
        {
            StrategicMap.Current.Selection.SelectLocationHexSprite(armyHexSprite);
        }
    }

    // Army.PAS:4120
    public void SelectBuildRoad(Troop troop)
    {
        CustomNode army = troop.Owner;
        if (army is Army)
        {
            ArmyHexSprite armyHexSprite = troop.ArmyOwner.GetArmyHexSprite();
            armyHexSprite.Army.ResetDestinationPosition();
            if (armyHexSprite != ArmyHexSprite)
            {
                StrategicMap.Current.Selection.UnselectLocation();
                StrategicMap.Current.Selection.SelectLocationHexSprite(armyHexSprite);
                SetSelection(255);
            }
            else
            {
                DeleteMoves();
                _ = AddMoveArmy(armyHexSprite, selection: 255);
                CurrentMove = 0;
            }

            string selectRoadConstructionPath = AbilityRules.TranslateResourceString(Creator.GetEngine(), S: s_selectRoadConstructionPathText);
            StrategicMap.Current.ShowMessage(selectRoadConstructionPath);
            _roadBuilderId = troop.UnitId;
            _mode = SelectedArmyMode.BuildRoads;
        }
    }

    // Army.PAS:4146
    public void Center()
    {
        if (ArmyHexSprite is not null)
        {
            StrategicMap hexSpriteMap = StrategicMap.Current;
            sbyte hexLevel = ArmyHexSprite.HexLevel;
            sbyte hexY = ArmyHexSprite.HexY;
            hexSpriteMap.CenterViewTo(ArmyHexSprite.HexX, hexY, hexLevel);
        }
    }

    // Army.PAS:4153
    public void TurnDone()
    {
        if (ArmyHexSprite is not null && ArmyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
        {
            ArmyHexSprite armyHexSprite = ArmyHexSprite;
            ArmyHexSprite.Army.UserMode = armyHexSprite.Army.UserMode | ArmyUserModes.TurnDone;
            _ = StrategicMap.Current.SelectNextArmy();
        }
    }

    // Army.PAS:4162
    public void ToggleGuard()
    {
        ArmyHexSprite armyHexSprite = ArmyHexSprite;
        if (armyHexSprite is not null && armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
        {
            Army army = armyHexSprite.Army;
            if (armyHexSprite.Army.UserMode.Contains(ArmyUserModes.Guard))
            {
                army.UserMode = armyHexSprite.Army.UserMode & ~ArmyUserModes.Guard;
                army.Update();
            }
            else
            {
                army.UserMode = armyHexSprite.Army.UserMode | ArmyUserModes.Guard;
                _ = StrategicMap.Current.SelectNextArmy();
            }
        }
    }

    // intf_P2_02.inc:23
    public override void UpdateMoveCursor()
    {
        StrategicMap map = StrategicMap.Current;
        HexSpriteMapMouse mouse = map.Mouse;
        if (mouse.HexX == -1 || mouse.HexY == -1 || mouse.Capture != this)
        {
            return;
        }

        MapField field = map.MapContainer.GetField(mouse.HexX, mouse.HexY, map.ViewMapLevel);
        bool visible = !map.FogEnabled || field.FogCount != 0;
        mouse.CursorMode = CurrentMove != -1 && (!map.ExplorationEnabled || field.GetExplored(map.SeatedPlayer)) && visible
            ? (field.GetMoveAction(ArmyHexSprite.Army.Player) switch
            {
                MoveAction.Attack => 258,
                MoveAction.Negotiate => 259,
                MoveAction.Normal or MoveAction.Combine or _ => 256
            })
            : MoveSettingsList.Count <= 1 ? 256 : 0;

        if (mouse.CursorMode != 256)
        {
            return;
        }

        MoveTypes types = GetMoveTypes();
        if (types.Contains(MoveTypes.Walking))
        {
            mouse.Cursor = 64;
        }
        else if (types.Contains(MoveTypes.Swimming))
        {
            mouse.Cursor = 66;
        }
        else if (types.Contains(MoveTypes.Flying))
        {
            mouse.Cursor = 68;
        }

        int cost = MoveControlSettings.MovePointTable[field.TerrainType][field.OverlayType - MovementTables.FirstOverlay];
        if (cost == 0 || (map.ExplorationEnabled && !field.GetExplored(map.SeatedPlayer)))
        {
            mouse.Cursor |= 1;
        }
    }

    private static readonly ImmutableArray<string> s_groupBehaviorTexts = ["", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""];
    private static readonly string s_selectRoadConstructionPathText = "Select road construction path";
    private static readonly string s_armyNotEnoughMovePointsText = "Not enough move points";
    private static readonly string s_armyRoadConstructionCostFormat = "Constructing road will cost %s gold";
    private static readonly string s_armyNotAtWarFormat = "Not at war with the %s";

    private static bool ExecuteMultiArmyMove(MoveSettingsList moves)
    {
        bool flag2 = moves.Count != 0;
        if (flag2)
        {
            MoveSettings moveSettings = moves.GetMoveSettings(0);
            flag2 = StrategicMap.Current.TokenManager.PlayerReady(((ArmyHexSprite)moveSettings.MoveObject).Army.Player);
        }
        bool flag;
        if (flag2)
        {
            MovePath movePath = new();
            MultiMoveTokenEvent multiMoveTokenEvent = new(null)
            {
                Holder = moves,
                Simultaneous = false
            };
            moves.CalculateMovePositions(ValidateMultiArmyMove);
            int i = 0;
            for (int number = moves.Count - 1; i <= number; i++)
            {
                MoveSettings moveSettingsLocal = moves.GetMoveSettings(i);
                if (moveSettingsLocal.MovePosition < moveSettingsLocal.Path.Count - 1)
                {
                    movePath.Clear();
                    int j = moveSettingsLocal.MovePosition;
                    for (int number2 = moveSettingsLocal.Path.Count - 1; j <= number2; j++)
                    {
                        _ = movePath.Add(moveSettingsLocal.Path[j]);
                    }
                    ArmyDefaultMoveTokenEvent armyDefaultMoveTokenEvent = new(null) { Holder = moves };
                    MapCoordinate mapCoordinate = movePath[0].Location;
                    MapCoordinate mapCoordinate2 = movePath[0].Location;
                    MapCoordinate mapCoordinate3 = movePath[0].Location;
                    moves.GetEngine().OutputDebugString(string.Concat(new string[6]
                    {
                        "Moving to: ",
                        ((int)mapCoordinate.X).ToString(CultureInfo.InvariantCulture),
                        ",",
                        ((int)mapCoordinate2.Y).ToString(CultureInfo.InvariantCulture),
                        ",",
                        ((int)mapCoordinate3.MapLevel).ToString(CultureInfo.InvariantCulture)
                    }));
                    if (armyDefaultMoveTokenEvent.Setup((ArmyHexSprite)moveSettingsLocal.MoveObject, selection: (byte)moveSettingsLocal.Selection, path: movePath, mode: ArmyMoveModes.None))
                    {
                        multiMoveTokenEvent.Events.Add(armyDefaultMoveTokenEvent);
                    }
                    _ = armyDefaultMoveTokenEvent.Release();
                }
            }
            if (multiMoveTokenEvent.Events.Count != 0)
            {
                MoveSettings moveSettings2 = moves.GetMoveSettings(0);
                StrategicMap.Current.TokenManager.GetTokenControl(((ArmyHexSprite)moveSettings2.MoveObject).Army.Player).AddEvent(multiMoveTokenEvent);
                flag = true;
            }
            else
            {
                flag = false;
            }
            _ = multiMoveTokenEvent.Release();
            movePath?.Dispose();
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    private static bool ValidateMultiArmyMove(MoveSettingsList moves, int index)
    {
        bool flag = true;
        sbyte fPlayer = ((ArmyHexSprite)moves.GetMoveSettings(index).MoveObject).Army.Player;
        MoveSettings moveSettings = moves.GetMoveSettings(index);
        int number = moveSettings.MovePosition;
        while (flag && number != moveSettings.Path.Count)
        {
            MapCoordinate mapCoordinate = moveSettings.Path[number].Location;
            MapCoordinate mapCoordinate2 = moveSettings.Path[number].Location;
            MapCoordinate mapCoordinate3 = moveSettings.Path[number].Location;
            MapField mapField = StrategicMap.Current.MapContainer.GetField(mapCoordinate3.X, mapCoordinate2.Y, mapCoordinate.MapLevel);
            sbyte player = mapField.GetPlayer();
            flag = player == -1 || player == fPlayer;
            number++;
        }
        int unitCount;
        int transporters;
        int capacity;
        if (flag)
        {
            unitCount = 0;
            transporters = 0;
            capacity = 8;
            MoveSettings moveSettings2 = moves.GetMoveSettings(index);
            AddArmy((ArmyHexSprite)moves.GetMoveSettings(index).MoveObject, selection: (byte)moveSettings2.Selection);
            MapCoordinate moveLocation = moves.GetMoveSettings(index).MoveLocation;
            MapField mapField = StrategicMap.Current.MapContainer.GetField(moveLocation.X, moveLocation.Y, moveLocation.MapLevel);
            ArmyHexSprite armyHexSprite = mapField.GetArmyHexSprite();
            if (armyHexSprite is not null)
            {
                AddArmy(armyHexSprite, selection: 255);
            }
            for (number = moves.Count - 1; number >= 0; number--)
            {
                if (number != index)
                {
                    moveSettings = moves.GetMoveSettings(number);
                    bool flag2 = moveSettings.Valid && moves.GetMoveSettings(index).GetMovePrio() >= moveSettings.GetMovePrio();
                    if (flag2)
                    {
                        MapCoordinate moveLocation2 = moves.GetMoveSettings(index).MoveLocation;
                        flag2 = moveSettings.MoveLocation == moveLocation2;
                    }
                    if (flag2)
                    {
                        AddArmy((ArmyHexSprite)moveSettings.MoveObject, selection: (byte)moveSettings.Selection);
                    }
                }
            }
            flag = unitCount <= capacity && transporters <= 1;
        }
        return flag;
        [CompilerGenerated]
        void AddArmy(ArmyHexSprite armyHexSprite, byte selection)
        {
            int number2 = armyHexSprite.Army.SelectionCount(selection);
            unitCount += number2;
            Troop troop = armyHexSprite.Army.Transporter(selection);
            if (troop is not null)
            {
                transporters++;
                if (troop.GetTransportCapacity() + 1 < capacity)
                {
                    capacity = troop.GetTransportCapacity() + 1;
                }
            }
        }
    }
}

public enum SelectedArmyMode : byte
{
    Default = 0, BuildRoads = 1
}
