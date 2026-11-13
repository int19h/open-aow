// Translated from AoWE.pas.
using System.Collections.Immutable;
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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// { Interface declaration run owned by P2. }
// mode, road builder ID, selected graphics references, ignore next mouse-down
public partial class SelectedArmy : MoveSelector
{
    // Army.PAS:3567
    public SelectedArmy(EngineObject creator) : base(creator)
    {
        _selectedUnitGraphics = new Int32List();
    }

    private SelectedArmyMode _mode;
    private int _roadBuilderId;
    private readonly Int32List _selectedUnitGraphics;

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
        ArmyHexSprite armyHexSprite;
        int index;
        if (!IgnoreMouseDown)
        {
            if (StrategicMap.Current.Mouse.Shift.Contains(ShiftState.Shift))
            {
                if (MoveSettingsList.Count == 1 && MoveSettingsList.GetMoveSettings(0).Path.Count != 0)
                {
                    var item = MoveSettingsList.GetMoveSettings(0).Path[0];
                    var fPathItem = MoveSettingsList.GetMoveSettings(0).Path[0];
                    var item2 = MoveSettingsList.GetMoveSettings(0).Path[0];
                    if (!ValidMultiMoveSelection(Map.MapContainer.GetField(item2.X, fPathItem.Y, item.MapLevel)))
                    {
                        return;
                    }
                }

                if (StrategicMap.Current.Mouse.HexX != -1)
                {
                    var hexY = StrategicMap.Current.Mouse.HexY;
                    var hexX = StrategicMap.Current.Mouse.HexX;
                    armyHexSprite = (ArmyHexSprite)StrategicMap.Current.MapContainer.GetField(hexX, hexY, StrategicMap.Current.ViewMapLevel).FindOwnedChild(131607);
                    if (armyHexSprite != null && armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
                    {
                        index = MoveSettingsList.IndexOfMoveObject(armyHexSprite);
                        if (index == -1)
                        {
                            AddMoveArmy(armyHexSprite, 255);
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
                if (StrategicMap.Current.Mouse.Shift.Contains(ShiftState.Ctrl))
                {
                    if (MoveSettingsList.Count > 1 && StrategicMap.Current.Mouse.HexX != -1)
                    {
                        var hexY = StrategicMap.Current.Mouse.HexY;
                        var hexX = StrategicMap.Current.Mouse.HexX;
                        armyHexSprite = (ArmyHexSprite)StrategicMap.Current.MapContainer.GetField(hexX, hexY, StrategicMap.Current.ViewMapLevel).FindOwnedChild(131607);
                        if (armyHexSprite != null && armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
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
                            var hexY = Map.Mouse.HexY;
                            SelectPath((sbyte)Map.Mouse.HexX, (sbyte)hexY, (sbyte)Map.ViewMapLevel);
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
        MoveSettings settings;
        ArmyHexSprite armyHexSprite;
        if (X != -1)
        {
            settings = MoveSettingsList.GetMoveSettings(index);
            armyHexSprite = (ArmyHexSprite)settings.MoveObject;
            settings.PathMovePoints = 0;
            MoveControlSettings.Reset();
            var hexLevel = armyHexSprite.GetHexLevel();
            var hexY = armyHexSprite.GetHexY();
            var hexX = armyHexSprite.GetHexX();
            MoveControlSettings.SetSourcePosition(hexX, hexY, hexLevel);
            MoveControlSettings.Player = armyHexSprite.Army.Player;
            MoveControlSettings.ArmyHexSprite = armyHexSprite;
            MoveControlSettings.SetDestinationPosition(X, Y, L);
            MoveControlSettings.Units.AddUnits(armyHexSprite.Army, (byte)settings.Selection);
            if (StrategicMap.Current.ExplorationEnabled)
            {
                MoveControlSettings.Mode = MoveControlSettings.Mode + Set<MoveModeElement>.Of(MoveModeElement.ExploredTerrainOnly);
            }

            armyHexSprite.Army.CreateMovePointTable((byte)settings.Selection, out MoveControlSettings.MovePointTable);
            StrategicMap.Current.MoveControl.ProcessMove(MoveControlSettings);
            if (MoveControlSettings.Path.Count != 0)
            {
                settings.Path.Assign(MoveControlSettings.Path);
                settings.PathMovePoints = settings.Path.PositionToMovePoints(0);
                var item = settings.Path[0];
                var fPathItem = settings.Path[0];
                var item2 = settings.Path[0];
                armyHexSprite.Army.SetDestinationPosition(item2.X, fPathItem.Y, item.MapLevel);
                MoveControlSettings.Path.Clear();
                if (_mode == SelectedArmyMode.BuildRoads)
                {
                    if (settings.MovePoints < settings.PathMovePoints)
                    {
                        settings.Path.Clear();
                        var armyNotEnoughMovePoints = AbilityRules.TranslateResourceString(Creator.GetEngine(), s_armyNotEnoughMovePointsText);
                        StrategicMap.Current.ShowMessage(armyNotEnoughMovePoints);
                    }
                    else
                    {
                        if (settings.Path.Count != 0)
                        {
                            var roadConstructionCost = AbilityRules.RoadConstructionCost(GetArmyHexSprite().Army.Player, settings.Path, settings.MovePoints);
                            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(Creator.GetEngine(), s_armyRoadConstructionCostFormat), [IntToStr(roadConstructionCost)]);
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
        if (CurrentMove != -1 && GetArmyHexSprite() != null)
        {
            var armyHexSprite = GetArmyHexSprite();
            StrategicMap.Current.Selection.TriggerDeselectEvent(armyHexSprite);
        }

        CurrentMove = value;
        if (CurrentMove != -1 && GetArmyHexSprite() != null)
        {
            var armyHexSprite = GetArmyHexSprite();
            StrategicMap.Current.Selection.TriggerSelectEvent(armyHexSprite);
        }

        UpdateSelectedUnitGraphics();
    }

    // Army.PAS:3795
    public override void SelectPath(sbyte X, sbyte Y, sbyte L)
    {
        bool valid;
        int I;
        MoveSettings settings;
        if (GetArmyHexSprite() != null && GetArmyHexSprite().GetHexX() == StrategicMap.Current.Mouse.HexX && GetArmyHexSprite().GetHexY() == StrategicMap.Current.Mouse.HexY && GetArmyHexSprite().GetHexLevel() == StrategicMap.Current.ViewMapLevel)
        {
            var hexLevel = GetArmyHexSprite().GetHexLevel();
            var hexY = GetArmyHexSprite().GetHexY();
            var hexX = GetArmyHexSprite().GetHexX();
            StrategicMap.Current.Selection.SelectLocationHex(hexX, hexY, hexLevel);
        }
        else
        {
            if (MoveSettingsList.Count > 1 && CurrentMove == -1)
            {
                valid = true;
                I = MoveSettingsList.Count;
                while (valid && I != 0)
                {
                    I -= 1;
                    settings = MoveSettingsList.GetMoveSettings(I);
                    valid = settings.Path.Count != 0 && settings.Path[0].X == X && settings.Path[0].Y == Y && settings.Path[0].MapLevel == L;
                }

                if (!valid)
                {
                    if (ValidateSelection(Map.MapContainer.GetField(X, Y, L)))
                    {
                        I = 0;
                        int last = MoveSettingsList.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            CalculateMovePath(I, X, Y, L);
                        }
                    }
                    else
                    {
                        I = 0;
                        int last = MoveSettingsList.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            MoveSettingsList.GetMoveSettings(I).Path.Clear();
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
        bool result;
        if (MoveSettingsList.Count > 1)
        {
            result = ValidMultiMoveSelection(field);
        }
        else
        {
            result = base.ValidateSelection(field);
        }

        if (_mode == SelectedArmyMode.BuildRoads)
        {
            if (field.GetPlayer() != -1 && field.GetPlayer() != GetArmyHexSprite().Army.Player)
            {
                result = false;
            }
        }

        return result;
    }

    // Army.PAS:3904
    public override void ExecuteMove()
    {
        ArmyHexSprite armyHexSprite;
        MapField field;
        int cost;
        if (MoveSettingsList.Count == 1)
        {
            var moveSettings = MoveSettingsList.GetMoveSettings(CurrentMove);
            if (moveSettings.Path.Count > 1 && GetArmyHexSprite() != null)
            {
                armyHexSprite = GetArmyHexSprite();
                var item = moveSettings.Path[0];
                var fPathItem = moveSettings.Path[0];
                var item2 = moveSettings.Path[0];
                field = StrategicMap.Current.MapContainer.GetField(item2.X, fPathItem.Y, item.MapLevel);
                bool condition = field.GetMoveAction(armyHexSprite.Army.Player) != MoveAction.Attack || field.GetPlayer() <= 0;
                if (!condition)
                {
                    var player = field.GetPlayer();
                    var diplomaticRelation = StrategicMap.Current.Players.GetDiplomaticRelation(armyHexSprite.Army.Player, player);
                    condition = Set<DiplomaticRelation>.Of(DiplomaticRelation.War, DiplomaticRelation.Unknown).Contains(diplomaticRelation);
                }

                if (condition)
                {
                    if (_mode == SelectedArmyMode.BuildRoads)
                    {
                        cost = AbilityRules.RoadConstructionCost(armyHexSprite.Army.Player, moveSettings.Path, moveSettings.MovePoints);
                        if (cost <= StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).Gems)
                        {
                            if (ArmyHexSprite.MoveArmy(armyHexSprite, GetSelection(), moveSettings.Path, Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation, ArmyMoveModeElement.BuildRoads), false))
                            {
                                moveSettings.Path.Clear();
                                if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                                {
                                    StrategicMap.Current.Selection.UnselectLocation();
                                }

                                DeactivateMoveSelection();
                            }
                        }
                        else
                        {
                            var notEnoughGold = AbilityRules.TranslateResourceString(Creator.GetEngine(), GameText.NotEnoughGoldText);
                            StrategicMap.Current.ShowWarning(notEnoughGold);
                        }
                    }
                    else
                    {
                        var item3 = moveSettings.Path[0];
                        var item4 = moveSettings.Path[0];
                        var item5 = moveSettings.Path[0];
                        armyHexSprite.Army.SetDestinationPosition(item5.X, item4.Y, item3.MapLevel);
                        if (ArmyHexSprite.MoveArmy(armyHexSprite, GetSelection(), moveSettings.Path))
                        {
                            var movePoints = armyHexSprite.Army.MovePoints(255);
                            if (moveSettings.Path.MovePointsToPosition(movePoints) > 0)
                            {
                                var army = armyHexSprite.Army;
                                army.UserMode = army.UserMode + Set<ArmyUserModeElement>.Of(ArmyUserModeElement.TurnDone);
                            }

                            moveSettings.Path.Clear();
                            if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                            {
                                StrategicMap.Current.Selection.UnselectLocation();
                            }

                            DeactivateMoveSelection();
                        }
                    }
                }
                else
                {
                    if (((field.FieldFlags >> 16) & 255) != 0 || !StrategicMap.Current.FogEnabled)
                    {
                        var player = field.GetPlayer();
                        var players = StrategicMap.Current.Players.GetPlayers(player);
                        var raceNamesPlural = AbilityRules.TranslateResourceString(Creator.GetEngine(), GameTables.RaceNamesPluralTexts[players.PlayerRace]);
                        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(Creator.GetEngine(), s_armyNotAtWarFormat), [raceNamesPlural]);
                        StrategicMap.Current.ShowMessage(format);
                    }
                }
            }
        }
        else
        {
            if (MoveSettingsList.Count > 1 && ExecuteMultiArmyMove(MoveSettingsList))
            {
                for (int I = 0, last = MoveSettingsList.Count - 1; I <= last; I++)
                {
                    var moveSettings = MoveSettingsList.GetMoveSettings(I);
                    var item = moveSettings.Path[0];
                    var fPathItem = moveSettings.Path[0];
                    var item2 = moveSettings.Path[0];
                    ((ArmyHexSprite)moveSettings.MoveObject).Army.SetDestinationPosition(item2.X, fPathItem.Y, item.MapLevel);
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
        int index;
        index = AddMoveObject(armyHexSprite);
        MoveSettingsList.GetMoveSettings(index).Selection = selection;
        var movePoints = armyHexSprite.Army.MovePoints(selection);
        MoveSettingsList.GetMoveSettings(index).MovePoints = movePoints;
        var moves = armyHexSprite.Army.Moves(selection);
        MoveSettingsList.GetMoveSettings(index).Moves = moves;
        return index;
    }

    // Army.PAS:3600
    public ArmyHexSprite GetArmyHexSprite()
    {
        ArmyHexSprite result;
        if (CurrentMove != -1)
        {
            result = (ArmyHexSprite)MoveSettingsList.GetMoveSettings(CurrentMove).MoveObject;
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Army.PAS:3607
    public byte GetSelection()
    {
        byte result;
        if (CurrentMove != -1)
        {
            result = (byte)MoveSettingsList.GetMoveSettings(CurrentMove).Selection;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Army.PAS:3614
    public void SetSelection(byte selection)
    {
        int points;
        if (CurrentMove != -1)
        {
            selection = GetArmyHexSprite().Army.ValidateMoveSelection(selection);
            if (_mode == SelectedArmyMode.BuildRoads)
            {
                selection = (byte)(selection | (1 << GetArmyHexSprite().Army.IndexOfId(_roadBuilderId)));
            }

            if (selection != 0)
            {
                MoveSettingsList.GetMoveSettings(CurrentMove).Selection = selection;
                points = GetArmyHexSprite().Army.MovePoints(selection);
                MoveSettingsList.GetMoveSettings(CurrentMove).MovePoints = points;
                var moves = GetArmyHexSprite().Army.Moves(selection);
                MoveSettingsList.GetMoveSettings(CurrentMove).Moves = moves;
            }

            // { Store the signed move count directly. }
            Update();
        }
    }

    // Army.PAS:3634
    public Set<MoveType> GetMoveTypes()
    {
        Set<MoveType> result;
        if (CurrentMove != -1)
        {
            var moveSettings = MoveSettingsList.GetMoveSettings(CurrentMove);
            result = ((ArmyHexSprite)MoveSettingsList.GetMoveSettings(CurrentMove).MoveObject).Army.MoveTypes((byte)moveSettings.Selection);
        }
        else
        {
            result = Set<MoveType>.Of();
        }

        return result;
    }

    // Army.PAS:3647
    public void ResetSelectedUnitGraphics()
    {
        for (int I = _selectedUnitGraphics.Count - 1; I >= 0; I--)
        {
            var item = _selectedUnitGraphics[I];
            GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(item).Release();
            _selectedUnitGraphics.Delete(I);
        }
    }

    // Army.PAS:3657
    public void UpdateSelectedUnitGraphics()
    {
        int index;
        Int32List list;
        list = new Int32List();
        if (GetArmyHexSprite() != null)
        {
            for (int I = 0, last = GetArmyHexSprite().Army.Count - 1; I <= last; I++)
            {
                index = GetArmyHexSprite().Army.GetUnit(I).GetUnitGraphicsResourceIndex();
                list.Add(index);
                GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(index).AddRef();
            }
        }

        ResetSelectedUnitGraphics();
        _selectedUnitGraphics.Assign(list);
        list?.Dispose();
    }

    // Army.PAS:3673
    public void OnDestroyArmy(Army sender)
    {
        int index;
        if (Status.Contains(MoveSelectorStatusElement.Activated))
        {
            if (sender != null)
            {
                var armyHexSprite = sender.GetArmyHexSprite();
                index = MoveSettingsList.IndexOfMoveObject(armyHexSprite);
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
        int x1 = default;
        int y1 = default;
        int x2 = default;
        int y2 = default;
        int pixelX = default;
        int pixelY = default;
        ArmyHSList armies;
        Rect R;
        MapLevel level;
        ArmyHexSprite armyHexSprite;
        armies = new ArmyHSList(null) { Holder = Creator.CreationHolder() };
        R = sender.SelectionRectangle;
        level = StrategicMap.Current.MapContainer.GetMapLevel(StrategicMap.Current.ViewMapLevel);
        HexGeometry.PixelToHex(R.Left, R.Top, out x1, out y1);
        HexGeometry.PixelToHex(R.Right, R.Bottom, out x2, out y2);
        for (int X = x1 - 1, last = x2 + 1; X <= last; X++)
        {
            for (int Y = y1 - 1, lastY = y2 + 1; Y <= lastY; Y++)
            {
                HexGeometry.HexToPixel(X, Y, out pixelX, out pixelY);
                pixelX += 16;
                pixelY += 16;
                if (X >= 0 && X < level.Width && Y > 0 && Y < level.Height && pixelX >= R.Left && pixelX < R.Right && pixelY >= R.Top && pixelY < R.Bottom)
                {
                    armyHexSprite = level.GetField(X, Y).GetArmyHexSprite();
                    if (armyHexSprite != null && armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
                    {
                        armies.AddChild(armyHexSprite);
                    }
                }
            }
        }

        if (armies.Count != 0)
        {
            Clear();
            StrategicMap.Current.Selection.UnselectLocation();
            for (int I = 0, last = armies.Count - 1; I <= last; I++)
            {
                AddMoveArmy(armies.GetArmyHexSprite(I), 255);
                var armyHS = armies.GetArmyHexSprite(I);
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
        sbyte player;
        bool valid;
        ArmyHexSprite armyHexSprite;
        player = field.GetPlayer();
        valid = player == -1 || player == StrategicMap.Current.SeatedPlayer;
        armyHexSprite = field.GetArmyHexSprite();
        if (armyHexSprite != null)
        {
            valid = valid && armyHexSprite.Army.Transporter(255) == null;
        }

        return valid && _mode != SelectedArmyMode.BuildRoads;
    }

    // Army.PAS:4002
    public void ActivateMoveSelection()
    {
        Activate(StrategicMap.Current);
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
        MapField field;
        ArmyHexSprite armyHexSprite;
        UpdateSelectedUnitGraphics();
        if (GetArmyHexSprite() != null)
        {
            var selection = GetSelection();
            GetArmyHexSprite().Army.CreateMovePointTable(selection, out MoveControlSettings.MovePointTable);
        }

        for (int I = 0, last = MoveSettingsList.Count - 1; I <= last; I++)
        {
            Selection = (byte)MoveSettingsList.GetMoveSettings(I).Selection;
            armyHexSprite = (ArmyHexSprite)MoveSettingsList.GetMoveSettings(I).MoveObject;
            var movePoints = armyHexSprite.Army.MovePoints(Selection);
            MoveSettingsList.GetMoveSettings(I).MovePoints = movePoints;
            var moves = armyHexSprite.Army.Moves(Selection);
            MoveSettingsList.GetMoveSettings(I).Moves = moves;
        }

        if (Status.Contains(MoveSelectorStatusElement.Activated))
        {
            for (int I = 0, last = MoveSettingsList.Count - 1; I <= last; I++)
            {
                armyHexSprite = (ArmyHexSprite)MoveSettingsList.GetMoveSettings(I).MoveObject;
                if (armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
                {
                    if (armyHexSprite.Army.DestinationX != -1 && armyHexSprite.Army.DestinationX != 0)
                    {
                        var army = armyHexSprite.Army;
                        field = StrategicMap.Current.MapContainer.GetField(army.DestinationX, army.DestinationY, army.DestinationMapLevel);
                        if (this.ValidateSelection(field))
                        {
                            CalculateMovePath(I, army.DestinationX, army.DestinationY, army.DestinationMapLevel);
                        }
                        else
                        {
                            armyHexSprite.Army.ResetDestinationPosition();
                        }
                    }
                    else
                    {
                        if (MoveSettingsList.GetMoveSettings(I).Path.Count != 0)
                        {
                            var location = MoveSettingsList.GetMoveSettings(I).Path[0];
                            CalculateMovePath(I, location.X, location.Y, location.MapLevel);
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
        string text;
        AIGroup group;
        for (int I = 0, last = GetArmyHexSprite().Army.Count - 1; I <= last; I++)
        {
            text = GetArmyHexSprite().Army.GetUnit(I).GetName() + "(ID:" + IntToStr(GetArmyHexSprite().Army.GetUnit(I).UnitId) + ")";
            group = GetArmyHexSprite().Army.GetUnit(I).Group;
            if (group != null)
            {
                var behavior = group.GroupClass.Behavior;
                text = text + s_groupBehaviorTexts[(int)behavior] + "(GID:" + IntToStr(group.GroupId) + "/Target:" + IntToStr(group.TargetMapCoordinate.X) + "," + IntToStr(group.TargetMapCoordinate.Y) + "," + IntToStr(group.TargetMapCoordinate.MapLevel) + "/Budget:" + IntToStr(group.GoldBudget) + "/MP:" + IntToStr(group.TargetMovePoints) + ")";
                if (group.TargetType == AIGroupTargetStatus.Setup)
                {
                    text = text + "-setup";
                }
                else
                {
                    if (group.TargetType == AIGroupTargetStatus.Execute)
                    {
                        text = text + "-execute";
                    }
                }

                var troop = GetArmyHexSprite().Army.GetUnit(I);
                if (group.Leader == troop)
                {
                    text = text + "/Leader";
                }
            }

            Creator.GetEngine().OutputDebugString(text);
        }

        var strengthMax = GetArmyHexSprite().Army.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.None));
        Creator.GetEngine().OutputDebugString("Army strength: " + IntToStr(strengthMax));
    }

    // Army.PAS:4090
    public void SetArmyHexSprite(ArmyHexSprite armyHexSprite)
    {
        if (armyHexSprite != GetArmyHexSprite() && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && MoveSettingsList.Count <= 1)
        {
            Clear();
            IgnoreMouseDown = false;
            _mode = SelectedArmyMode.Default;
            if (armyHexSprite != null)
            {
                if (armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
                {
                    ActivateMoveSelection();
                }

                AddMoveArmy(armyHexSprite, 255);
                CurrentMove = 0;
                Update();
            }
            else
            {
                DeactivateMoveSelection();
                ResetSelectedUnitGraphics();
            }

            if (GetArmyHexSprite() != null && Creator.GetEngine().Registry.GetDebugMode())
            {
                OutputGroupInfo();
            }
        }
    }

    // Army.PAS:4114
    public void Select(ArmyHexSprite armyHexSprite)
    {
        StrategicMap.Current.Selection.UnselectLocation();
        if (armyHexSprite != null)
        {
            StrategicMap.Current.Selection.SelectLocationHexSprite(armyHexSprite);
        }
    }

    // Army.PAS:4120
    public void SelectBuildRoad(Troop troop)
    {
        ECustomNode army;
        ArmyHexSprite armyHexSprite;
        army = troop.Owner;
        if (army is Army)
        {
            armyHexSprite = troop.ArmyOwner.GetArmyHexSprite();
            armyHexSprite.Army.ResetDestinationPosition();
            if (armyHexSprite != GetArmyHexSprite())
            {
                StrategicMap.Current.Selection.UnselectLocation();
                StrategicMap.Current.Selection.SelectLocationHexSprite(armyHexSprite);
                SetSelection(255);
            }
            else
            {
                DeleteMoves();
                AddMoveArmy(armyHexSprite, 255);
                CurrentMove = 0;
            }

            var selectRoadConstructionPath = AbilityRules.TranslateResourceString(Creator.GetEngine(), s_selectRoadConstructionPathText);
            StrategicMap.Current.ShowMessage(selectRoadConstructionPath);
            _roadBuilderId = troop.UnitId;
            _mode = SelectedArmyMode.BuildRoads;
        }
    }

    // Army.PAS:4146
    public void Center()
    {
        if (GetArmyHexSprite() != null)
        {
            var hexSpriteMap = StrategicMap.Current;
            var hexLevel = GetArmyHexSprite().GetHexLevel();
            var hexY = GetArmyHexSprite().GetHexY();
            hexSpriteMap.CenterViewTo(GetArmyHexSprite().GetHexX(), hexY, hexLevel);
        }
    }

    // Army.PAS:4153
    public void TurnDone()
    {
        if (GetArmyHexSprite() != null && GetArmyHexSprite().Army.Player == StrategicMap.Current.SeatedPlayer)
        {
            var armyHexSprite = GetArmyHexSprite();
            GetArmyHexSprite().Army.UserMode = armyHexSprite.Army.UserMode + Set<ArmyUserModeElement>.Of(ArmyUserModeElement.TurnDone);
            StrategicMap.Current.SelectNextArmy();
        }
    }

    // Army.PAS:4162
    public void ToggleGuard()
    {
        ArmyHexSprite armyHexSprite;
        Army army;
        armyHexSprite = GetArmyHexSprite();
        if (armyHexSprite != null && armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
        {
            army = armyHexSprite.Army;
            if (armyHexSprite.Army.UserMode.Contains(ArmyUserModeElement.Guard))
            {
                army.UserMode = armyHexSprite.Army.UserMode - Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
                army.Update();
            }
            else
            {
                army.UserMode = armyHexSprite.Army.UserMode + Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
                StrategicMap.Current.SelectNextArmy();
            }
        }
    }

    // intf_P2_02.inc:23
    public override void UpdateMoveCursor()
    {
        var map = StrategicMap.Current;
        var mouse = map.Mouse;
        if (mouse.HexX == -1 || mouse.HexY == -1 || mouse.Capture != this) return;
        var field = map.MapContainer.GetField(mouse.HexX, mouse.HexY, map.ViewMapLevel);
        bool visible = !map.FogEnabled || ((field.FieldFlags >> 16) & 255) != 0;
        if (CurrentMove != -1 && (!map.ExplorationEnabled || field.GetExplored(map.SeatedPlayer)) && visible)
            mouse.CursorMode = field.GetMoveAction(GetArmyHexSprite().Army.Player) switch
            {
                MoveAction.Attack => 258,
                MoveAction.Negotiate => 259,
                _ => 256
            };
        else mouse.CursorMode = MoveSettingsList.Count <= 1 ? 256 : 0;
        if (mouse.CursorMode != 256) return;
        var types = GetMoveTypes();
        if (types.Contains(MoveType.Walking)) mouse.Cursor = 64;
        else if (types.Contains(MoveType.Swimming)) mouse.Cursor = 66;
        else if (types.Contains(MoveType.Flying)) mouse.Cursor = 68;
        int cost = MoveControlSettings.MovePointTable[field.TerrainType][field.OverlayType - MovementTables.FirstOverlay];
        if (cost == 0 || map.ExplorationEnabled && !field.GetExplored(map.SeatedPlayer)) mouse.Cursor |= 1;
    }

    private static readonly ImmutableArray<string> s_groupBehaviorTexts = ["", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""];
    private static readonly string s_selectRoadConstructionPathText = "Select road construction path";
    private static readonly string s_armyNotEnoughMovePointsText = "Not enough move points";
    private static readonly string s_armyRoadConstructionCostFormat = "Constructing road will cost %s gold";
    private static readonly string s_armyNotAtWarFormat = "Not at war with the %s";

    private static bool ExecuteMultiArmyMove(MoveSettingsList moves)
    {
        bool flag = false;
        MultiMoveTokenEvent multiMoveTokenEvent = null;
        MovePath movePath = null;
        MoveSettings moveSettingsLocal = null;
        ArmyDefaultMoveTokenEvent armyDefaultMoveTokenEvent = null;
        bool flag2 = moves.Count != 0;
        if (flag2)
        {
            MoveSettings moveSettings = moves.GetMoveSettings(0);
            flag2 = StrategicMap.Current.TokenManager.PlayerReady(((ArmyHexSprite)moveSettings.MoveObject).Army.Player);
        }
        if (flag2)
        {
            movePath = new MovePath();
            multiMoveTokenEvent = new MultiMoveTokenEvent(null) { Holder = moves };
            multiMoveTokenEvent.Simultaneous = false;
            moves.CalculateMovePositions(ValidateMultiArmyMove);
            int i = 0;
            for (int number = moves.Count - 1; i <= number; i++)
            {
                moveSettingsLocal = moves.GetMoveSettings(i);
                if (moveSettingsLocal.MovePosition < moveSettingsLocal.Path.Count - 1)
                {
                    movePath.Clear();
                    int j = moveSettingsLocal.MovePosition;
                    for (int number2 = moveSettingsLocal.Path.Count - 1; j <= number2; j++)
                    {
                        movePath.Add(moveSettingsLocal.Path[j]);
                    }
                    armyDefaultMoveTokenEvent = new ArmyDefaultMoveTokenEvent(null) { Holder = moves };
                    MapCoordinate mapCoordinate = movePath[0];
                    MapCoordinate mapCoordinate2 = movePath[0];
                    MapCoordinate mapCoordinate3 = movePath[0];
                    moves.GetEngine().OutputDebugString(string.Concat(new string[6]
                    {
                        "Moving to: ",
                        RtlUnit.IntToStr((int)mapCoordinate.X),
                        ",",
                        RtlUnit.IntToStr((int)mapCoordinate2.Y),
                        ",",
                        RtlUnit.IntToStr((int)mapCoordinate3.MapLevel)
                    }));
                    if (armyDefaultMoveTokenEvent.Setup((ArmyHexSprite)moveSettingsLocal.MoveObject, (byte)moveSettingsLocal.Selection, movePath, Set<ArmyMoveModeElement>.Of(Array.Empty<ArmyMoveModeElement>())))
                    {
                        multiMoveTokenEvent.Events.Add(armyDefaultMoveTokenEvent);
                    }
                    armyDefaultMoveTokenEvent.Release();
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
            multiMoveTokenEvent.Release();
            if (movePath != null)
            {
                ((Persistent)movePath).Dispose();
            }
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
            MapCoordinate mapCoordinate = moveSettings.Path[number];
            MapCoordinate mapCoordinate2 = moveSettings.Path[number];
            MapCoordinate mapCoordinate3 = moveSettings.Path[number];
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
            AddArmy((ArmyHexSprite)moves.GetMoveSettings(index).MoveObject, (byte)moveSettings2.Selection);
            MapCoordinate moveLocation = moves.GetMoveSettings(index).GetMoveLocation();
            MapField mapField = StrategicMap.Current.MapContainer.GetField(moveLocation.X, moveLocation.Y, moveLocation.MapLevel);
            ArmyHexSprite armyHexSprite = (ArmyHexSprite)mapField.FindOwnedChild(131607);
            if (armyHexSprite != null)
            {
                AddArmy(armyHexSprite, 255);
            }
            for (number = moves.Count - 1; number >= 0; number--)
            {
                if (number != index)
                {
                    moveSettings = moves.GetMoveSettings(number);
                    bool flag2 = moveSettings.Valid && moves.GetMoveSettings(index).GetMovePrio() >= moveSettings.GetMovePrio();
                    if (flag2)
                    {
                        MapCoordinate moveLocation2 = moves.GetMoveSettings(index).GetMoveLocation();
                        flag2 = Unsafe.BitCast<MapCoordinate, int>(moveSettings.GetMoveLocation()) == Unsafe.BitCast<MapCoordinate, int>(moveLocation2);
                    }
                    if (flag2)
                    {
                        AddArmy((ArmyHexSprite)moveSettings.MoveObject, (byte)moveSettings.Selection);
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
            if (troop != null)
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
