// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;
using Aow.Rtl;

namespace Aow.Game.Combat;

// PathInvalid. TargetImages. RangedPathHexSprite. TargetX. TargetY. CursorImage. CursorFrame.
// RangedPath. Obstacles
public partial class CombatUnitSelectionControl : MoveSelector
{
    // AoWTC.pas:2723
    public CombatUnitSelectionControl(EngineObject creator) : base(creator)
    {
        RangedPath = new Point3DList();
        Obstacles = new DelphiList<RangedObstacle>();
        RangedPathHexSprite = new RangedPathHexSprite(null) { Holder = Creator.CreationHolder() };
        RangedPathHexSprite.Activate();
        TargetImages = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TargHex.ILB");
    }

    private Set<PathKind> _pathInvalid;

    public CustomImageLibrary TargetImages { get; }
    public RangedPathHexSprite RangedPathHexSprite { get; }

    private int _targetX;
    private int _targetY;
    private ushort _cursorImage;
    private ushort _cursorFrame;

    public HexSprite MouseOverTarget { get; private set; }

    public Point3DList RangedPath;
    public DelphiList<RangedObstacle> Obstacles;

    // AoWTC.pas:2733
    public override void Dispose()
    {
        for (int I = 0, last = Obstacles.Count - 1; I <= last; I++)
        {
            (Obstacles[I] as IDisposable)?.Dispose();
        }

        Obstacles?.Dispose();
        RangedPath?.Dispose();
        RangedPathHexSprite?.Dispose();
        GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TargHex.ILB");
        base.Dispose();
    }

    // AoWTC.pas:3080
    public override void MouseDownEvent(object sender)
    {
        TacticalBattleUnitHexSprite unitHexSprite;
        int index;
        int deltaX;
        int deltaY;
        Set<CombatTargetTypeElement> targets;
        SpellCombatInfo spellInfo = default;
        AbilityCombatInfo abilityInfo = default;
        if (CombatMap.Current.SeatedCombatPlayer == CombatMap.Current.CurrentCombatPlayer)
        {
            if (!CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).Auto)
            {
                if (CombatMap.Current.Mouse.Button == MouseButton.Right)
                {
                    if (GetTacticalBattleUnitHexSprite() != null && GetTacticalBattleUnitHexSprite().SelectedAbility > -1)
                    {
                        GetTacticalBattleUnitHexSprite().SelectAbility(-1);
                        return;
                    }

                    Unselect();
                }
                else
                {
                    if (CombatMap.Current.Mouse.Shift.Contains(ShiftState.Shift))
                    {
                        if (MoveSettingsList.GetCount() == 1 && MoveSettingsList.GetMoveSettings(0).Path.Count != 0)
                        {
                            var pathL = MoveSettingsList.GetMoveSettings(0).Path[0];
                            var pathY = MoveSettingsList.GetMoveSettings(0).Path[0];
                            var pathX = MoveSettingsList.GetMoveSettings(0).Path[0];
                            if (!ValidMultiMoveSelection(CombatMap.Current.MapContainer.GetField(pathX.Location.X, pathY.Location.Y, pathL.Location.MapLevel)))
                            {
                                return;
                            }
                        }

                        if (CombatMap.Current.Mouse.HexX != -1 && CombatMap.Current.Mouse.HexY != -1)
                        {
                            var hexY = CombatMap.Current.Mouse.HexY;
                            var hexX = CombatMap.Current.Mouse.HexX;
                            unitHexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).FindOwnedChild(2228484);
                            if (unitHexSprite != null && unitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedCombatPlayer)
                            {
                                index = MoveSettingsList.IndexOfMoveObject(unitHexSprite);
                                if (index == -1)
                                {
                                    AddUnit(unitHexSprite);
                                    Update();
                                    CombatMap.Current.Selection.SelectLocationHexSprite(unitHexSprite);
                                    if (unitHexSprite.Unit.GetLocked())
                                    {
                                        var indexOfMoveObject = MoveSettingsList.IndexOfMoveObject(unitHexSprite);
                                        MoveSettingsList.GetMoveSettings(indexOfMoveObject).Path.Clear();
                                    }
                                    else
                                    {
                                        if (MoveSettingsList.GetMoveSettings(0).Path.Count > 0)
                                        {
                                            var moveSettingPath = MoveSettingsList.GetMoveSettings(0);
                                            var path = MoveSettingsList.GetMoveSettings(0).Path[moveSettingPath.Path.Count - 1];
                                            deltaX = unitHexSprite.GetHexX() - path.Location.X;
                                            var moveSetting = MoveSettingsList.GetMoveSettings(0);
                                            var pathY = MoveSettingsList.GetMoveSettings(0).Path[moveSetting.Path.Count - 1];
                                            deltaY = unitHexSprite.GetHexY() - pathY.Location.Y;
                                            var path2 = MoveSettingsList.GetMoveSettings(0).Path[0];
                                            var pathX = MoveSettingsList.GetMoveSettings(0).Path[0];
                                            CalculateMovePath(MoveSettingsList.IndexOfMoveObject(unitHexSprite), (sbyte)(pathX.Location.X + deltaX), (sbyte)(path2.Location.Y + deltaY), 0);
                                        }
                                    }

                                    SetCurrentMove(-1);
                                }
                                else
                                {
                                    unitHexSprite.Unselect();
                                    DeleteMoveObject(index);
                                    SetCurrentMove(MoveSettingsList.GetCount() - 1);
                                    Update();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (CombatMap.Current.Mouse.Shift.Contains(ShiftState.Ctrl))
                        {
                            if (MoveSettingsList.GetCount() > 0 && CombatMap.Current.Mouse.HexX != -1 && CombatMap.Current.Mouse.HexY != -1)
                            {
                                var hexY = CombatMap.Current.Mouse.HexY;
                                var hexX = CombatMap.Current.Mouse.HexX;
                                unitHexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).FindOwnedChild(2228484);
                                if (unitHexSprite != null && unitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedCombatPlayer)
                                {
                                    index = MoveSettingsList.IndexOfMoveObject(unitHexSprite);
                                    if (index == -1)
                                    {
                                        AddUnit(unitHexSprite);
                                        Update();
                                        CombatMap.Current.Selection.SelectLocationHexSprite(unitHexSprite);
                                        if (unitHexSprite.Unit.GetLocked())
                                        {
                                            var indexOfMoveObject = MoveSettingsList.IndexOfMoveObject(unitHexSprite);
                                            MoveSettingsList.GetMoveSettings(indexOfMoveObject).Path.Clear();
                                        }
                                        else
                                        {
                                            if (MoveSettingsList.GetMoveSettings(0).Path.Count > 0)
                                            {
                                                var moveSettingPath = MoveSettingsList.GetMoveSettings(0);
                                                var path = MoveSettingsList.GetMoveSettings(0).Path[moveSettingPath.Path.Count - 1];
                                                deltaX = unitHexSprite.GetHexX() - path.Location.X;
                                                var moveSetting = MoveSettingsList.GetMoveSettings(0);
                                                var pathY = MoveSettingsList.GetMoveSettings(0).Path[moveSetting.Path.Count - 1];
                                                deltaY = unitHexSprite.GetHexY() - pathY.Location.Y;
                                                var path2 = MoveSettingsList.GetMoveSettings(0).Path[0];
                                                var pathX = MoveSettingsList.GetMoveSettings(0).Path[0];
                                                CalculateMovePath(MoveSettingsList.IndexOfMoveObject(unitHexSprite), (sbyte)(pathX.Location.X + deltaX), (sbyte)(path2.Location.Y + deltaY), 0);
                                            }
                                        }

                                        SetCurrentMove(MoveSettingsList.IndexOfMoveObject(unitHexSprite));
                                    }
                                    else
                                    {
                                        if (CurrentMove != index)
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
                            if (CombatMap.Current.Mouse.HexX != -1 && CombatMap.Current.Mouse.HexY != -1)
                            {
                                if (GetTacticalBattleUnitHexSprite() != null)
                                {
                                    var hexY = CombatMap.Current.Mouse.HexY;
                                    var hexX = CombatMap.Current.Mouse.HexX;
                                    unitHexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).FindOwnedChild(2228484);
                                    if (GetTacticalBattleUnitHexSprite().SelectedAbility == -1)
                                    {
                                        targets = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
                                    }
                                    else
                                    {
                                        if (GetTacticalBattleUnitHexSprite().SelectedAbility == 52)
                                        {
                                            var tacticalBattleUnitHexSprite = GetTacticalBattleUnitHexSprite();
                                            var tacticalBattleUnitHexSprite2 = GetTacticalBattleUnitHexSprite();
                                            GamePack.Current.SpellControl.GetSpell(tacticalBattleUnitHexSprite2.SelectedSpell).GetCombatInfo(tacticalBattleUnitHexSprite.Unit.Troop, ref spellInfo);
                                            targets = spellInfo.TargetTypes;
                                        }
                                        else
                                        {
                                            var tacticalBattleUnitHexSprite = GetTacticalBattleUnitHexSprite();
                                            var tacticalBattleUnitHexSprite2 = GetTacticalBattleUnitHexSprite();
                                            GamePack.Current.AbilityControl.GetAbility(tacticalBattleUnitHexSprite2.SelectedAbility).GetCombatInfo(tacticalBattleUnitHexSprite.Unit.Troop, ref abilityInfo);
                                            targets = abilityInfo.TargetTypes;
                                        }
                                    }

                                    if (unitHexSprite == null)
                                    {
                                        ExecuteMoveSelection();
                                    }
                                    else
                                    {
                                        var player = GetTacticalBattleUnitHexSprite().Unit.GetPlayer();
                                        if (unitHexSprite.Unit.GetPlayer() == player && !targets.Contains(CombatTargetTypeElement.Friend))
                                        {
                                            CombatMap.Current.Selection.UnselectLocation();
                                            CombatMap.Current.Selection.SelectLocationHexSprite(unitHexSprite);
                                        }
                                        else
                                        {
                                            var fUnitPlayer = GetTacticalBattleUnitHexSprite().Unit.GetPlayer();
                                            bool condition = unitHexSprite.Unit.GetPlayer() == fUnitPlayer && targets.Contains(CombatTargetTypeElement.Friend);
                                            if (!condition)
                                            {
                                                var player2 = GetTacticalBattleUnitHexSprite().Unit.GetPlayer();
                                                condition = unitHexSprite.Unit.GetPlayer() != player2 && targets.Contains(CombatTargetTypeElement.Enemy);
                                            }

                                            if (condition)
                                            {
                                                ExecuteMoveSelection();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ExecuteMoveSelection();
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            var hexY = CombatMap.Current.Mouse.HexY;
            var hexX = CombatMap.Current.Mouse.HexX;
            unitHexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).FindOwnedChild(2228484);
            CombatMap.Current.Selection.UnselectLocation();
            if (unitHexSprite != null && CombatMap.Current.Mouse.Button == MouseButton.Left)
            {
                CombatMap.Current.Selection.SelectLocationHexSprite(unitHexSprite);
            }
        }
    }

    // AoWTC.pas:2823
    public override void CalculateMovePath(int index, sbyte X, sbyte Y, sbyte L)
    {
        TacticalBattleUnitHexSprite unitHexSprite;
        DelphiList<MovementHexSprite> temporary;
        MovementHexSprite movement;
        var moveSettings = MoveSettingsList.GetMoveSettings(index);
        temporary = new DelphiList<MovementHexSprite>();
        if (MoveSettingsList.GetCount() > 0)
        {
            for (int I = 0, last = MoveSettingsList.GetCount() - 1; I <= last; I++)
            {
                if (I != index)
                {
                    if (MoveSettingsList.GetMoveSettings(I).Path.Count > 1)
                    {
                        movement = new MovementHexSprite(null) { Holder = Creator.CreationHolder() };
                        var moveSettingPath = MoveSettingsList.GetMoveSettings(I);
                        var path = MoveSettingsList.GetMoveSettings(I).Path[moveSettingPath.Path.Count - 1];
                        var moveSetting = MoveSettingsList.GetMoveSettings(I);
                        var pathX = MoveSettingsList.GetMoveSettings(I).Path[moveSetting.Path.Count - 1];
                        CombatMap.Current.PlaceHex(movement, pathX.Location.X, path.Location.Y, 0);
                        movement.Enabled = true;
                        temporary.Add(movement);
                        movement = new MovementHexSprite(null) { Holder = Creator.CreationHolder() };
                        var pathY = MoveSettingsList.GetMoveSettings(I).Path[0];
                        var path2 = MoveSettingsList.GetMoveSettings(I).Path[0];
                        CombatMap.Current.PlaceHex(movement, path2.Location.X, pathY.Location.Y, 0);
                        movement.Enabled = false;
                        temporary.Add(movement);
                    }
                }
            }
        }

        moveSettings.PathMovePoints = 0;
        unitHexSprite = (TacticalBattleUnitHexSprite)moveSettings.MoveObject;
        if (GetTacticalBattleUnitHexSprite() != null && (GetTacticalBattleUnitHexSprite().SelectedAbility == 117 || GetTacticalBattleUnitHexSprite().SelectedAbility == 65))
        {
            ((CombatMoveControlSettings)MoveControlSettings).MoveFlags = Set<CombatMoveModeElement>.FromMask((byte)_pathInvalid.Image) + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Climbing);
        }
        else
        {
            ((CombatMoveControlSettings)MoveControlSettings).MoveFlags = Set<CombatMoveModeElement>.FromMask((byte)_pathInvalid.Image);
        }

        MoveControlSettings.Reset();
        var hexLevel = unitHexSprite.GetHexLevel();
        var hexY = unitHexSprite.GetHexY();
        var hexX = unitHexSprite.GetHexX();
        MoveControlSettings.SetSourcePosition(hexX, hexY, hexLevel);
        var player = unitHexSprite.Unit.GetPlayer();
        MoveControlSettings.Player = player;
        MoveControlSettings.ArmyHexSprite = unitHexSprite;
        MoveControlSettings.Selection = 0;
        MoveControlSettings.SetDestinationPosition(X, Y, L);
        unitHexSprite.Unit.Troop.CreateMovePointTable(out MoveControlSettings.MovePointTable);
        CombatMap.Current.MoveControl.ProcessMove(MoveControlSettings);
        MoveSettingsList.GetMoveSettings(index).Path.Assign(MoveControlSettings.Path);
        MoveSettingsList.GetMoveSettings(index).MovePoints = unitHexSprite.Unit.MovePoints;
        var moves = unitHexSprite.Unit.GetMoves();
        MoveSettingsList.GetMoveSettings(index).Moves = moves;
        if (MoveControlSettings.Path.Count != 0)
        {
            for (int I = 0, last = MoveControlSettings.Path.Count - 2; I <= last; I++)
            {
                var pathY = MoveControlSettings.Path[I];
                var pathX = MoveControlSettings.Path[I];
                bool condition = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y].FindOwnedChild(2229508) != null;
                if (!condition)
                {
                    var path = MoveControlSettings.Path[I];
                    var path2 = MoveControlSettings.Path[I];
                    condition = CombatMap.Current.MapContainer.MapLevels[0].Field[path2.Location.X, path.Location.Y].FindOwnedChild(2229510) != null;
                }

                if (condition)
                {
                    var path = MoveControlSettings.Path[I];
                    var path2 = MoveControlSettings.Path[I];
                    var mapField = CombatMap.Current.MapContainer.MapLevels[0].Field[path2.Location.X, path.Location.Y];
                    moveSettings.Path.List[I] = moveSettings.Path.List[I] with { MoveCost = MoveControlSettings.MovePointTable[mapField.TerrainType][-1 - MovementTables.FirstOverlay] };
                }
                else
                {
                    var path = MoveControlSettings.Path[I];
                    var path2 = MoveControlSettings.Path[I];
                    var item = CombatMap.Current.MapContainer.MapLevels[0].Field[path2.Location.X, path.Location.Y];
                    var path3 = MoveControlSettings.Path[I];
                    var path4 = MoveControlSettings.Path[I];
                    var fieldItem = CombatMap.Current.MapContainer.MapLevels[0].Field[path4.Location.X, path3.Location.Y];
                    moveSettings.Path.List[I] = moveSettings.Path.List[I] with { MoveCost = MoveControlSettings.MovePointTable[fieldItem.TerrainType][item.OverlayType - MovementTables.FirstOverlay] };
                }
            }

            moveSettings.PathMovePoints = moveSettings.Path.PositionToMovePoints(0);
            MoveControlSettings.Path.Clear();
        }

        for (int I = 0, last = temporary.Count - 1; I <= last; I++)
        {
            (temporary[I] as IDisposable)?.Dispose();
        }

        temporary?.Dispose();
    }

    // AoWTC.pas:3178
    public override void UpdateMoveCursor()
    {
        MapField field;
        int cost;
        TacticalBattleUnitHexSprite unitHexSprite;
        TacticalBattleUnitHexSprite targetHexSprite;
        Set<CombatTargetTypeElement> targets;
        CityWall wall;
        SpellCombatInfo spellInfo = default;
        AbilityCombatInfo abilityInfo = default;
        if (CombatMap.Current == null)
        {
            return;
        }

        unitHexSprite = GetTacticalBattleUnitHexSprite();
        CombatMap.Current.Mouse.CursorMode = 0;
        if (CombatMap.Current.Mouse.HexX == -1)
        {
            return;
        }

        if (CombatMap.Current.Mouse.HexY == -1)
        {
            return;
        }

        if (unitHexSprite == null)
        {
            return;
        }

        var hexY = CombatMap.Current.Mouse.HexY;
        var hexX = CombatMap.Current.Mouse.HexX;
        field = CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel);
        if (CombatMap.Current.ExplorationEnabled && !field.GetExplored(CombatMap.Current.SeatedPlayer))
        {
            goto FinishCursor;
        }

        targetHexSprite = (TacticalBattleUnitHexSprite)field.FindOwnedChild(2228484);
        if (unitHexSprite.Unit.CombatPlayer != CombatMap.Current.SeatedCombatPlayer && targetHexSprite != null)
        {
            CombatMap.Current.Mouse.CursorMode = 257;
            goto FinishCursor;
        }

        if (unitHexSprite.SelectedAbility > -1)
        {
            _cursorFrame = (ushort)(_cursorFrame + 1);
            if (_cursorFrame >= 5)
            {
                _cursorFrame = 0;
                _cursorImage = (ushort)(_cursorImage + 1);
            }

            if (unitHexSprite.SelectedAbility == 52)
            {
                GamePack.Current.SpellControl.GetSpell(unitHexSprite.SelectedSpell).GetCombatInfo(unitHexSprite.Unit.Troop, ref spellInfo);
                targets = spellInfo.TargetTypes;
            }
            else
            {
                GamePack.Current.AbilityControl.GetAbility(unitHexSprite.SelectedAbility).GetCombatInfo(unitHexSprite.Unit.Troop, ref abilityInfo);
                targets = abilityInfo.TargetTypes;
            }

            if (targetHexSprite == null || (targetHexSprite == unitHexSprite && !targets.Contains(CombatTargetTypeElement.Friend)))
            {
                if (_cursorImage < 85 || _cursorImage > 88)
                {
                    _cursorImage = 85;
                }
            }
            else
            {
                if (GamePack.Current.AbilityControl.GetAbility(unitHexSprite.SelectedAbility).GetCombatMode(unitHexSprite.Unit.Troop) == CombatMode.Touch)
                {
                    if ((targets.Contains(CombatTargetTypeElement.Friend) && unitHexSprite.Unit.GetSide() == targetHexSprite.Unit.GetSide() || targets.Contains(CombatTargetTypeElement.Enemy) && unitHexSprite.Unit.GetSide() != targetHexSprite.Unit.GetSide()) && ((TouchAbility)GamePack.Current.AbilityControl.GetAbility(unitHexSprite.SelectedAbility)).CanTouch(unitHexSprite.Unit, targetHexSprite.Unit))
                    {
                        if (_cursorImage < 105 || _cursorImage > 107)
                        {
                            _cursorImage = 105;
                        }
                    }
                    else
                    {
                        _cursorImage = 89;
                    }
                }
                else
                {
                    if (targets.Contains(CombatTargetTypeElement.Friend) && unitHexSprite.Unit.GetSide() == targetHexSprite.Unit.GetSide() || targets.Contains(CombatTargetTypeElement.Enemy) && unitHexSprite.Unit.GetSide() != targetHexSprite.Unit.GetSide())
                    {
                        if (_cursorImage < 80 || _cursorImage > 83)
                        {
                            _cursorImage = 80;
                        }
                    }
                    else
                    {
                        _cursorImage = 89;
                    }
                }
            }

            CombatMap.Current.Mouse.CursorMode = 256;
            CombatMap.Current.Mouse.Cursor = _cursorImage;
        }
        else
        {
            if (targetHexSprite != null && targetHexSprite != unitHexSprite)
            {
                if (targetHexSprite.Unit.GetPlayer() == unitHexSprite.Unit.GetPlayer())
                {
                    CombatMap.Current.Mouse.CursorMode = 257;
                }
                else
                {
                    if (unitHexSprite.Unit.GetAbilityEnabled(1) || !targetHexSprite.Unit.GetAbilityEnabled(1))
                    {
                        CombatMap.Current.Mouse.CursorMode = 258;
                    }
                    else
                    {
                        _cursorImage = 89;
                        CombatMap.Current.Mouse.CursorMode = 256;
                        CombatMap.Current.Mouse.Cursor = _cursorImage;
                    }
                }
            }
        }

    FinishCursor:
        if (CombatMap.Current.Mouse.CursorMode != 0)
        {
            return;
        }

        if (unitHexSprite.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer)
        {
            if (GetMoveTypes().Contains(MoveType.Walking))
            {
                CombatMap.Current.Mouse.Cursor = 64;
            }
            else
            {
                if (GetMoveTypes().Contains(MoveType.Swimming))
                {
                    CombatMap.Current.Mouse.Cursor = 66;
                }
                else
                {
                    if (GetMoveTypes().Contains(MoveType.Flying))
                    {
                        CombatMap.Current.Mouse.Cursor = 68;
                    }
                }
            }

            cost = MoveControlSettings.MovePointTable[field.TerrainType][field.OverlayType - MovementTables.FirstOverlay];
            if (unitHexSprite.Unit.MovePoints == 0)
            {
                cost = 0;
            }
            else
            {
                wall = (CityWall)field.FindOwnedChild(2229508);
                if (wall == null)
                {
                    wall = field.FindOwnedChild(2229510) as CityWall;
                }

                if (wall != null)
                {
                    if (!(wall is CityWallDoor) || unitHexSprite.Unit.GetSide() == CombatSideIndex.Attacker)
                    {
                        cost = -1;
                    }

                    if (!(wall is CityWallDoor) && unitHexSprite.Unit.Troop.GetAbilityEnabled(36))
                    {
                        cost = 1;
                    }

                    if (unitHexSprite.Unit.Troop.GetAbilityEnabled(66) || unitHexSprite.Unit.Troop.GetAbilityEnabled(1))
                    {
                        cost = 1;
                    }
                }
            }

            if (cost <= 0 || CombatMap.Current.ExplorationEnabled && !field.GetExplored(CombatMap.Current.SeatedPlayer))
            {
                CombatMap.Current.Mouse.Cursor = CombatMap.Current.Mouse.Cursor + 1;
            }

            CombatMap.Current.Mouse.CursorMode = 256;
        }
    }

    // AoWTC.pas:2744
    public override MoveControlSettings CreateMoveControlSettings() => new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };

    // AoWTC.pas:2754
    public override void SetCurrentMove(int value)
    {
        if (CurrentMove != -1 && GetTacticalBattleUnitHexSprite() != null)
        {
            var tacticalBattleUnitHexSprite = GetTacticalBattleUnitHexSprite();
            CombatMap.Current.Selection.TriggerDeselectEvent(tacticalBattleUnitHexSprite);
        }

        base.SetCurrentMove(value);
        if (CurrentMove != -1 && GetTacticalBattleUnitHexSprite() != null)
        {
            var tacticalBattleUnitHexSprite = GetTacticalBattleUnitHexSprite();
            CombatMap.Current.Selection.TriggerSelectEvent(tacticalBattleUnitHexSprite);
        }
    }

    // AoWTC.pas:3316
    public override void Unselect()
    {
        DeactivateRangedPath();
        if (GetTacticalBattleUnitHexSprite() != null)
        {
            GetTacticalBattleUnitHexSprite().SelectedAbility = -1;
        }

        CombatMap.Current.Selection.UnselectLocation();
        DeactivateMoveSelection();
        base.Unselect();
    }

    // AoWTC.pas:3274
    public override void ShowMovePaths(in LockedSurface surface)
    {
        int X = default;
        int Y = default;
        int pixelX = default;
        int pixelY = default;
        base.ShowMovePaths(surface);
        if (GetTacticalBattleUnitHexSprite() != null)
        {
            if (GetTacticalBattleUnitHexSprite().SelectedAbility != 65 && GetTacticalBattleUnitHexSprite().SelectedAbility != 114)
            {
                return;
            }

            if (MoveSettingsList.GetCount() == 1 && MoveSettingsList.GetMoveSettings(0).Path.Count > 1)
            {
                for (int I = 1; I <= 6; I++)
                {
                    var path = MoveSettingsList.GetMoveSettings(0).Path[1];
                    HexGeometry.CenterHexNumberToHex(MoveSettingsList.GetMoveSettings(0).Path[1].Location.X, path.Location.Y, I, ref X, ref Y);
                    HexGeometry.HexToPixel(X, Y, out pixelX, out pixelY);
                    TargetImages[0].Show(pixelX - CombatMap.Current.ViewPixelX - 8, pixelY - CombatMap.Current.ViewPixelY + 2, surface);
                }
            }
        }
    }

    // AoWTC.pas:2747
    public int AddUnit(TacticalBattleUnitHexSprite U)
    {
        int result = AddMoveObject(U);
        MoveSettingsList.GetMoveSettings(result).MovePoints = U.Unit.MovePoints;
        var moves = U.Unit.GetMoves();
        MoveSettingsList.GetMoveSettings(result).Moves = moves;
        return result;
    }

    // AoWTC.pas:2763
    public TacticalBattleUnitHexSprite GetTacticalBattleUnitHexSprite()
    {
        TacticalBattleUnitHexSprite result;
        if (CurrentMove != -1)
        {
            result = (TacticalBattleUnitHexSprite)MoveSettingsList.GetMoveSettings(CurrentMove).MoveObject;
        }
        else
        {
            result = null;
        }

        return result;
    }

    // AoWTC.pas:2770
    public Set<MoveType> GetMoveTypes()
    {
        Set<MoveType> result;
        if (CurrentMove != -1)
        {
            result = ((TacticalBattleUnitHexSprite)MoveSettingsList.GetMoveSettings(CurrentMove).MoveObject).Unit.Troop.GetMoveTypes();
        }
        else
        {
            result = Set<MoveType>.Of();
        }

        return result;
    }

    // AoWTC.pas:2777
    public void RectangleSelectEvent(GameMapMouse sender)
    {
        int x1 = default;
        int y1 = default;
        int x2 = default;
        int y2 = default;
        int pixelX = default;
        int pixelY = default;
        TacticalBattleUnitHexSprite unitHexSprite;
        MapField field;
        MapLevel level;
        DelphiList<TacticalBattleUnitHexSprite> units;
        Rect R;
        units = new DelphiList<TacticalBattleUnitHexSprite>();
        R = sender.SelectionRectangle;
        level = CombatMap.Current.MapContainer.MapLevels[CombatMap.Current.ViewMapLevel];
        HexGeometry.PixelToHex(R.Left, R.Top, out x1, out y1);
        HexGeometry.PixelToHex(R.Right, R.Bottom, out x2, out y2);
        for (int X = x1 - 1, last = x2 + 1; X <= last; X++)
        {
            for (int Y = y1 - 1, lastY = y2 + 1; Y <= lastY; Y++)
            {
                HexGeometry.HexToPixel(X, Y, out pixelX, out pixelY);
                pixelX += 16;
                pixelY += 8;
                if (X >= 0 && X < level.Width && Y > 0 && Y < level.Height && pixelX >= R.Left && pixelX < R.Right && pixelY >= R.Top && pixelY < R.Bottom)
                {
                    field = level.Field[X, Y];
                    unitHexSprite = (TacticalBattleUnitHexSprite)field.FindOwnedChild(2228484);
                    if (unitHexSprite != null && unitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedCombatPlayer)
                    {
                        units.Add(unitHexSprite);
                    }
                }
            }
        }

        if (units.Count != 0)
        {
            Clear();
            CombatMap.Current.Selection.UnselectLocation();
            for (int I = 0, last = units.Count - 1; I <= last; I++)
            {
                AddUnit(units[I]);
                var unit = units[I];
                CombatMap.Current.Selection.SelectLocationHexSprite(unit);
            }

            if (units.Count == 1)
            {
                SetCurrentMove(0);
            }
            else
            {
                SetCurrentMove(-1);
            }

            Update();
        }

        units?.Dispose();
    }

    // AoWTC.pas:2807
    public void MoveSelectionEvent()
    {
        if (CombatMap.Current.SeatedCombatPlayer == CombatMap.Current.CurrentCombatPlayer)
        {
            if (CombatMap.Current.Mouse.Capture != this)
            {
                CombatMap.Current.Selection.UnselectLocation();
            }
            else
            {
                if (CombatMap.Current.Mouse.Button == MouseButton.Right)
                {
                    if (RangedPath.Count == 0)
                    {
                        CombatMap.Current.Selection.UnselectLocation();
                    }
                    else
                    {
                        RangedPath.Clear();
                        MouseOverTarget = null;
                        CombatMap.Current.MouseOverTargetEventsStore.Raise(MouseOverTarget);
                    }
                }
                else
                {
                    if (CombatMap.Current.Mouse.Capture == this && CombatMap.Current.Mouse.Button == MouseButton.Left)
                    {
                        ExecuteMoveSelection();
                    }
                }
            }
        }
    }

    // AoWTC.pas:2887
    public void CalculateRangedPath(sbyte X, sbyte Y)
    {
        TacticalBattleUnitHexSprite unitHexSprite;
        ushort range;
        SpellCombatInfo info = default;
        int direction;
        int hexX;
        int hexY;
        RangedPath.Clear();
        direction = 0;
        int last = Obstacles.Count - 1;
        for (; direction <= last; direction += 1)
        {
            (Obstacles[direction] as IDisposable)?.Dispose();
        }

        Obstacles.Clear();
        unitHexSprite = GetTacticalBattleUnitHexSprite();
        if (CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].TerrainType != 15)
        {
            if (unitHexSprite.SelectedAbility == 52)
            {
                GamePack.Current.SpellControl.GetSpell(unitHexSprite.SelectedSpell).GetCombatInfo(unitHexSprite.Unit.Troop, ref info);
                if (info.Range > 0)
                {
                    range = (ushort)info.Range;
                }
                else
                {
                    range = 100;
                }
            }
            else
            {
                var tacticalBattleUnitHexSprite = GetTacticalBattleUnitHexSprite();
                if (GamePack.Current.AbilityControl.GetAbility(unitHexSprite.SelectedAbility).GetCombatMode(tacticalBattleUnitHexSprite.Unit.Troop) == CombatMode.LongRange)
                {
                    range = 12;
                }
                else
                {
                    var tacticalBattleUnitHexSprite2 = GetTacticalBattleUnitHexSprite();
                    if (GamePack.Current.AbilityControl.GetAbility(unitHexSprite.SelectedAbility).GetCombatMode(tacticalBattleUnitHexSprite2.Unit.Troop) == CombatMode.MediumRange)
                    {
                        range = 8;
                    }
                    else
                    {
                        range = 4;
                    }
                }
            }

            bool condition = unitHexSprite.Unit.GetMoves() == unitHexSprite.Unit.MovePoints && X > 0;
            if (condition)
            {
                var yhx = unitHexSprite.GetHexY();
                condition = HexGeometry.DeltaHexToRadius(X, Y, unitHexSprite.GetHexX(), yhx) <= range;
            }

            if (condition)
            {
                // { Original retains this unused with temporary. }
                GetTacticalBattleUnitHexSprite();
                _targetX = X;
                _targetY = Y;
                var yhx = unitHexSprite.GetHexY();
                direction = HexGeometry.DeltaHexToHexNumber(unitHexSprite.GetHexX(), yhx, X, Y);
                hexX = X;
                hexY = Y;
                switch (GetTacticalBattleUnitHexSprite().SelectedAbility)
                {
                    case 30:
                    case >= 86 and <= 90:
                        var unitHexSpriteHexY = unitHexSprite.GetHexY();
                        HexGeometry.CenterHexNumberToHex(unitHexSprite.GetHexX(), unitHexSpriteHexY, TacticalTables.BreathHexNumber[TacticalTables.BreathDirection[direction - 1] * 12 - 12], ref hexX, ref hexY);
                        break;
                    case 52:
                        if (TacticalTables.SpellTypes[unitHexSprite.SelectedSpell] == BattleEffectType.Breath)
                        {
                            var hexY2 = unitHexSprite.GetHexY();
                            HexGeometry.CenterHexNumberToHex(unitHexSprite.GetHexX(), hexY2, TacticalTables.BreathHexNumber[TacticalTables.BreathDirection[direction - 1] * 12 - 12], ref hexX, ref hexY);
                        }

                        break;
                }

                var xhx = GetTacticalBattleUnitHexSprite().GetHexX();
                var hexY3 = GetTacticalBattleUnitHexSprite().GetHexY();
                var side = GetTacticalBattleUnitHexSprite().Unit.GetSide();
                var tacticalBattleUnitHexSprite = GetTacticalBattleUnitHexSprite();
                TacticalTables.MakeAbilityPath(side, (short)GetTacticalBattleUnitHexSprite().SelectedAbility, (short)tacticalBattleUnitHexSprite.SelectedSpell, xhx, hexY3, (short)hexX, (short)hexY, ref RangedPath, ref Obstacles);
            }
        }

        RangedPathHexSprite.Selection = this;
        CombatMap.Current.DynamicFixedVisibleHexSprite.AddChild(RangedPathHexSprite);
    }

    // AoWTC.pas:2918
    public void MoveSelectedRoute()
    {
        TacticalBattleUnitHexSprite unitHexSprite;
        EngineObject target;
        int movePoints;
        if (MoveSettingsList.GetCount() > 0)
        {
            if (GetTacticalBattleUnitHexSprite() != null)
            {
                if (MoveSettingsList.GetMoveSettings(0).Path.Count > 0)
                {
                    var moveSettings = MoveSettingsList.GetMoveSettings(0);
                    unitHexSprite = GetTacticalBattleUnitHexSprite();
                    if (unitHexSprite.SelectedAbility != 117)
                    {
                        var pathY = moveSettings.Path[0];
                        var pathX = moveSettings.Path[0];
                        target = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y].FindOwnedChild(2228484);
                    }
                    else
                    {
                        var pathY = moveSettings.Path[0];
                        var pathX = moveSettings.Path[0];
                        target = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y].FindOwnedChild(2229508);
                        if (target == null)
                        {
                            var path = moveSettings.Path[0];
                            var path2 = moveSettings.Path[0];
                            target = CombatMap.Current.MapContainer.MapLevels[0].Field[path2.Location.X, path.Location.Y].FindOwnedChild(2229510);
                        }
                    }

                    if (MoveSettingsList.GetMoveSettings(0).Path.MovePointsToPosition(unitHexSprite.Unit.MovePoints) == 0 && (target != null || unitHexSprite.SelectedAbility == 65 || unitHexSprite.SelectedAbility == 114))
                    {
                        movePoints = GetTacticalBattleUnitHexSprite().Unit.MovePoints;
                        if (movePoints > 0)
                        {
                            if (unitHexSprite.SelectedAbility == -1)
                            {
                                if (GetTacticalBattleUnitHexSprite().MeleeMoveTacticalBattle(moveSettings.Path))
                                {
                                    moveSettings.Path.Clear();
                                }
                            }
                            else
                            {
                                if (GamePack.Current.AbilityControl.GetAbility(unitHexSprite.SelectedAbility).GetCombatMode(unitHexSprite.Unit.Troop) == CombatMode.Touch)
                                {
                                    if (GetTacticalBattleUnitHexSprite().AbilityTouchMoveTacticalBattle(moveSettings.Path))
                                    {
                                        moveSettings.Path.Clear();
                                        UpdateMoveCursor();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (TacticalBattleUnit.MoveCombatUnit(unitHexSprite.Unit, moveSettings.Path))
                        {
                            moveSettings.Path.Clear();
                        }
                    }
                }
            }
        }
    }

    // AoWTC.pas:2943
    public void MoveUnits()
    {
        if (MoveSettingsList.GetCount() > 0)
        {
            if (MoveSettingsList.GetCount() > 1)
            {
                if (SetupMultiTacticalBattleUnitMove(MoveSettingsList))
                {
                    for (int I = 0, last = MoveSettingsList.GetCount() - 1; I <= last; I++)
                    {
                        var moveSettings = MoveSettingsList.GetMoveSettings(I);
                        moveSettings.Path.Clear();
                    }
                }

                CombatMap.Current.Selection.UnselectLocation();
                DeactivateMoveSelection();
                Clear();
            }
            else
            {
                MoveSelectedRoute();
            }
        }
    }

    // AoWTC.pas:2956
    public void ExecuteMoveSelection()
    {
        TacticalBattleUnitHexSprite unitHexSprite;
        BattleEffectType effectType;
        int X = default;
        int Y = default;
        int direction;
        bool AllPathsValid()
        {
            int I;
            bool condition = MoveSettingsList.GetMoveSettings(0).Path.Count > 0;
            if (condition)
            {
                var path = MoveSettingsList.GetMoveSettings(0).Path[0];
                var hexX = CombatMap.Current.Mouse.HexX;
                condition = path.Location.X == hexX;
            }

            bool test = condition;
            if (test)
            {
                var path = MoveSettingsList.GetMoveSettings(0).Path[0];
                var hexY = CombatMap.Current.Mouse.HexY;
                test = path.Location.Y == hexY;
            }

            bool result = test;
            I = MoveSettingsList.GetCount();
            while (result && I != 0)
            {
                I -= 1;
                var moveSettings = MoveSettingsList.GetMoveSettings(I);
                result = moveSettings.Path.Count != 0;
            }

            return result;
        }

        DeactivateRangedPath();
        if (MoveSettingsList.GetCount() > 1)
        {
            if (CurrentMove == -1)
            {
                if (!AllPathsValid())
                {
                    bool notExplorationEnabled = !CombatMap.Current.ExplorationEnabled;
                    if (!notExplorationEnabled)
                    {
                        var hexY = CombatMap.Current.Mouse.HexY;
                        var hexX = CombatMap.Current.Mouse.HexX;
                        notExplorationEnabled = CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).GetExplored(CombatMap.Current.SeatedPlayer);
                    }

                    if (notExplorationEnabled)
                    {
                        for (int I = 0, last = MoveSettingsList.GetCount() - 1; I <= last; I++)
                        {
                            MoveSettingsList.GetMoveSettings(I).Path.Clear();
                        }

                        for (int A = 0, last = MoveSettingsList.GetCount() - 1; A <= last; A++)
                        {
                            for (int B = A + 1, lastB = MoveSettingsList.GetCount() - 1; B <= lastB; B++)
                            {
                                var hexY = ((HexSprite)MoveSettingsList.GetMoveSettings(B).MoveObject).GetHexY();
                                var hexX = ((HexSprite)MoveSettingsList.GetMoveSettings(B).MoveObject).GetHexX();
                                var mouseHexY = CombatMap.Current.Mouse.HexY;
                                var deltaHexToRadius2 = HexGeometry.DeltaHexToRadius(CombatMap.Current.Mouse.HexX, mouseHexY, hexX, hexY);
                                var hexY2 = ((HexSprite)MoveSettingsList.GetMoveSettings(A).MoveObject).GetHexY();
                                var hexX2 = ((HexSprite)MoveSettingsList.GetMoveSettings(A).MoveObject).GetHexX();
                                var hexY3 = CombatMap.Current.Mouse.HexY;
                                if (deltaHexToRadius2 < HexGeometry.DeltaHexToRadius(CombatMap.Current.Mouse.HexX, hexY3, hexX2, hexY2))
                                {
                                    MoveSettingsList.ExchangeChildren(A, B);
                                }
                            }
                        }

                        if (((TacticalBattleUnitHexSprite)MoveSettingsList.GetMoveSettings(0).MoveObject).Unit.GetLocked())
                        {
                            MoveSettingsList.GetMoveSettings(0).Path.Clear();
                        }
                        else
                        {
                            var hexY = CombatMap.Current.Mouse.HexY;
                            CalculateMovePath(0, (sbyte)CombatMap.Current.Mouse.HexX, (sbyte)hexY, (sbyte)CombatMap.Current.ViewMapLevel);
                        }

                        if (MoveSettingsList.GetMoveSettings(0).Path.Count > 1)
                        {
                            for (int I = 1, last = MoveSettingsList.GetCount() - 1; I <= last; I++)
                            {
                                if (((TacticalBattleUnitHexSprite)MoveSettingsList.GetMoveSettings(I).MoveObject).Unit.GetLocked())
                                {
                                    MoveSettingsList.GetMoveSettings(I).Path.Clear();
                                }
                                else
                                {
                                    var hexY = ((HexSprite)MoveSettingsList.GetMoveSettings(I).MoveObject).GetHexY();
                                    var hexX = ((HexSprite)MoveSettingsList.GetMoveSettings(I).MoveObject).GetHexX();
                                    var hexY2 = ((HexSprite)MoveSettingsList.GetMoveSettings(0).MoveObject).GetHexY();
                                    direction = HexGeometry.DeltaHexToHexNumber(((HexSprite)MoveSettingsList.GetMoveSettings(0).MoveObject).GetHexX(), hexY2, hexX, hexY);
                                    var path = MoveSettingsList.GetMoveSettings(0).Path[0];
                                    HexGeometry.CenterHexNumberToHex(MoveSettingsList.GetMoveSettings(0).Path[0].Location.X, path.Location.Y, direction, ref X, ref Y);
                                    CalculateMovePath(I, (sbyte)X, (sbyte)Y, 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int I = 0, last = MoveSettingsList.GetCount() - 1; I <= last; I++)
                        {
                            MoveSettingsList.GetMoveSettings(I).Path.Clear();
                        }

                        return;
                    }
                }
                else
                {
                    MoveUnits();
                }
            }
            else
            {
                var moveSettings = MoveSettingsList.GetMoveSettings(CurrentMove);
                bool condition = moveSettings.Path.Count != 0;
                if (condition)
                {
                    var path = moveSettings.Path[0];
                    var hexX = CombatMap.Current.Mouse.HexX;
                    condition = path.Location.X == hexX;
                }

                bool test = condition;
                if (test)
                {
                    var path = moveSettings.Path[0];
                    var hexY = CombatMap.Current.Mouse.HexY;
                    test = path.Location.Y == hexY;
                }

                if (test && moveSettings.Path[0].Location.MapLevel == CombatMap.Current.ViewMapLevel)
                {
                    MoveUnits();
                    return;
                }

                if (((TacticalBattleUnitHexSprite)MoveSettingsList.GetMoveSettings(CurrentMove).MoveObject).Unit.GetLocked())
                {
                    MoveSettingsList.GetMoveSettings(CurrentMove).Path.Clear();
                }
                else
                {
                    var hexY = CombatMap.Current.Mouse.HexY;
                    var hexX = CombatMap.Current.Mouse.HexX;
                    CalculateMovePath(CurrentMove, (sbyte)hexX, (sbyte)hexY, (sbyte)CombatMap.Current.ViewMapLevel);
                }
            }
        }
        else
        {
            if (GetTacticalBattleUnitHexSprite().SelectedAbility == -1 && GetTacticalBattleUnitHexSprite().GetHexX() == CombatMap.Current.Mouse.HexX && GetTacticalBattleUnitHexSprite().GetHexY() == CombatMap.Current.Mouse.HexY)
            {
                var hexLevel = GetTacticalBattleUnitHexSprite().GetHexLevel();
                var hexY = GetTacticalBattleUnitHexSprite().GetHexY();
                var hexX = GetTacticalBattleUnitHexSprite().GetHexX();
                CombatMap.Current.Selection.SelectLocationHex(hexX, hexY, hexLevel);
                return;
            }

            if (!GetTacticalBattleUnitHexSprite().Unit.GetLocked() && StrategicMap.Current.TokenManager.PlayerReady(CombatMap.Current.CurrentPlayer))
            {
                unitHexSprite = GetTacticalBattleUnitHexSprite();
                if (AllPathsValid())
                {
                    MoveUnits();
                }
                else
                {
                    bool condition = RangedPath.Count > 0;
                    if (condition)
                    {
                        var hexX = CombatMap.Current.Mouse.HexX;
                        condition = _targetX == hexX;
                    }

                    bool test = condition;
                    if (test)
                    {
                        var hexY = CombatMap.Current.Mouse.HexY;
                        test = _targetY == hexY;
                    }

                    if (test)
                    {
                        if (unitHexSprite.AbilityRangedTacticalBattle((ushort)_targetX, (ushort)_targetY))
                        {
                            RangedPath.Clear();
                            UpdateMoveCursor();
                        }
                    }
                    else
                    {
                        if (unitHexSprite.SelectedAbility == -1 && unitHexSprite.GetHexX() == CombatMap.Current.Mouse.HexX && unitHexSprite.GetHexY() == CombatMap.Current.Mouse.HexY)
                        {
                            var hexLevel = unitHexSprite.GetHexLevel();
                            var hexY = unitHexSprite.GetHexY();
                            var hexX = unitHexSprite.GetHexX();
                            CombatMap.Current.Selection.SelectLocationHex(hexX, hexY, hexLevel);
                            return;
                        }

                        if (!unitHexSprite.Moving())
                        {
                            if (unitHexSprite.SelectedAbility == -1)
                            {
                                if (unitHexSprite.Unit.MovePoints > 0 && !unitHexSprite.Unit.GetLocked())
                                {
                                    if (CombatMap.Current.ExplorationEnabled)
                                    {
                                        var hexY = CombatMap.Current.Mouse.HexY;
                                        var hexX = CombatMap.Current.Mouse.HexX;
                                        if (!CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).GetExplored(CombatMap.Current.SeatedPlayer))
                                        {
                                            return;
                                        }
                                    }

                                    var mouseHexY = CombatMap.Current.Mouse.HexY;
                                    var mouseHexX = CombatMap.Current.Mouse.HexX;
                                    CalculateMovePath(CurrentMove, (sbyte)mouseHexX, (sbyte)mouseHexY, 0);
                                }
                            }
                            else
                            {
                                if (unitHexSprite.SelectedAbility == 52)
                                {
                                    effectType = TacticalTables.SpellTypes[unitHexSprite.SelectedSpell];
                                }
                                else
                                {
                                    effectType = TacticalTables.AbilityTypes[unitHexSprite.SelectedAbility];
                                }

                                switch (effectType)
                                {
                                    case BattleEffectType.None:
                                        unitHexSprite.SelectAbility(-1);
                                        break;
                                    case BattleEffectType.Touch:
                                        if (CombatMap.Current.ExplorationEnabled)
                                        {
                                            var hexY = CombatMap.Current.Mouse.HexY;
                                            var hexX = CombatMap.Current.Mouse.HexX;
                                            if (!CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).GetExplored(CombatMap.Current.SeatedPlayer))
                                            {
                                                return;
                                            }
                                        }

                                        if (!unitHexSprite.Unit.GetLocked())
                                        {
                                            var hexY = CombatMap.Current.Mouse.HexY;
                                            CalculateMovePath(0, (sbyte)CombatMap.Current.Mouse.HexX, (sbyte)hexY, 0);
                                        }

                                        break;
                                    default:
                                        {
                                            var hexY = CombatMap.Current.Mouse.HexY;
                                            CalculateRangedPath((sbyte)CombatMap.Current.Mouse.HexX, (sbyte)hexY);
                                            if (RangedPath.Count > 0)
                                            {
                                                var mouseHexY = CombatMap.Current.Mouse.HexY;
                                                var hexX = CombatMap.Current.Mouse.HexX;
                                                MouseOverTarget = (HexSprite)CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, mouseHexY].FindOwnedChild(2228484);
                                                CombatMap.Current.MouseOverTargetEventsStore.Raise(MouseOverTarget);
                                            }
                                        }

                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // AoWTC.pas:3073
    public bool ValidMultiMoveSelection(MapField field)
    {
        sbyte player;
        player = field.GetPlayer();
        return player == -1;
    }

    // AoWTC.pas:3259
    public void ActivateMoveSelection()
    {
        if (!Status.Contains(MoveSelectorStatusElement.Activated))
        {
            Activate(CombatMap.Current);
        }
    }

    // AoWTC.pas:3264
    public void DeactivateMoveSelection()
    {
        if (Status.Contains(MoveSelectorStatusElement.Activated))
        {
            Deactivate();
            RangedPath.Clear();
            MoveSettingsList.Clear();
            CombatMap.Current.MouseMoveEvent(this);
        }
    }

    // AoWTC.pas:3293
    public void Update()
    {
        if (GetTacticalBattleUnitHexSprite() != null)
        {
            GetTacticalBattleUnitHexSprite().Unit.Troop.CreateMovePointTable(out MoveControlSettings.MovePointTable);
        }

        _pathInvalid = Set<PathKind>.Of(PathKind.Ranged);
    }

    // AoWTC.pas:3300
    public void SetTacticalBattleUnitHexSprite(TacticalBattleUnitHexSprite U)
    {
        if (GetTacticalBattleUnitHexSprite() != U)
        {
            if (MoveSettingsList.GetCount() <= 1)
            {
                Clear();
                if (U != null && U.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer)
                {
                    ActivateMoveSelection();
                    AddUnit(U);
                    SetCurrentMove(0);
                    Update();
                }
                else
                {
                    DeactivateMoveSelection();
                }

                RangedPath.Clear();
                UpdateMoveCursor();
            }
        }
    }

    // AoWTC.pas:3325
    public void Select(TacticalBattleUnitHexSprite U)
    {
        CombatMap.Current.Selection.UnselectLocation();
        if (U != null)
        {
            CombatMap.Current.Selection.SelectLocationHexSprite(U);
        }
    }

    // AoWTC.pas:3331
    public void DeactivateRangedPath()
    {
        if (RangedPathHexSprite.Selection != null)
        {
            CombatMap.Current.DynamicFixedVisibleHexSprite.RemoveChild(RangedPathHexSprite);
            RangedPathHexSprite.Selection = null;
        }
    }

    private static bool SetupMultiTacticalBattleUnitMove(MoveSettingsList moves)
    {
        bool flag = false;
        MultiMoveTokenEvent multiMoveTokenEvent = null;
        CombatMoveTokenEvent combatMoveTokenEvent = null;
        MovePath movePath = null;
        bool flag2 = moves.Count != 0;
        if (flag2)
        {
            sbyte player = ((TacticalBattleUnitHexSprite)moves.GetMoveSettings(0).MoveObject).Unit.GetPlayer();
            flag2 = StrategicMap.Current.TokenManager.PlayerReady(player);
        }
        if (flag2)
        {
            movePath = new MovePath();
            multiMoveTokenEvent = new MultiMoveTokenEvent(null) { Holder = moves };
            multiMoveTokenEvent.Simultaneous = false;
            moves.CalculateMovePositions(ValidateTacticalBattleMovePosition);
            int i = 1;
            for (int number = moves.Count - 1; i <= number; i++)
            {
                for (int j = 0; j <= i - 1; j++)
                {
                    MoveSettings moveSettings = moves.GetMoveSettings(j);
                    MoveSettings moveSettings2 = moves.GetMoveSettings(i);
                    if (moveSettings.Path.Count > moveSettings2.Path.Count)
                    {
                        moves.ExchangeChildren(i, j);
                    }
                }
            }
            int k = 0;
            for (int number2 = moves.Count - 1; k <= number2; k++)
            {
                MoveSettings moveSettings3 = moves.GetMoveSettings(k);
                if (moveSettings3.MovePosition < moveSettings3.Path.Count - 1)
                {
                    movePath.Clear();
                    int l = moveSettings3.MovePosition;
                    for (int number3 = moveSettings3.Path.Count - 1; l <= number3; l++)
                    {
                        movePath.Add(moveSettings3.Path[l]);
                    }
                    combatMoveTokenEvent = new CombatMoveTokenEvent(null) { Holder = moves };
                    if (combatMoveTokenEvent.Setup(((TacticalBattleUnitHexSprite)moveSettings3.MoveObject).Unit, movePath))
                    {
                        multiMoveTokenEvent.Events.Add(combatMoveTokenEvent);
                    }
                    combatMoveTokenEvent.Release();
                }
            }
            if (multiMoveTokenEvent.Events.Count != 0)
            {
                sbyte player2 = ((TacticalBattleUnitHexSprite)moves.GetMoveSettings(0).MoveObject).Unit.GetPlayer();
                StrategicMap.Current.TokenManager.GetTokenControl(player2).AddEvent(multiMoveTokenEvent);
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
            int m = 0;
            for (int number4 = CombatMap.Current.UnitSelection.MoveSettingsList.Count - 1; m <= number4; m++)
            {
                MoveSettings moveSettings4 = CombatMap.Current.UnitSelection.MoveSettingsList.GetMoveSettings(m);
                moveSettings4.Path.Clear();
            }
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    private static bool ValidateTacticalBattleMovePosition(MoveSettingsList moves, int index)
    {
        bool flag = true;
        if (flag)
        {
            for (int number = moves.Count - 1; number >= 0; number--)
            {
                if (number != index)
                {
                    MoveSettings moveSettings = moves.GetMoveSettings(number);
                    bool flag2 = moveSettings.Valid && moves.GetMoveSettings(index).GetMovePrio() >= moveSettings.GetMovePrio();
                    if (flag2)
                    {
                        MapCoordinate moveLocation = moves.GetMoveSettings(index).GetMoveLocation();
                        flag2 = moveSettings.GetMoveLocation() == moveLocation;
                    }
                    if (flag2)
                    {
                        flag = false;
                    }
                }
            }
        }
        return flag;
    }

    public enum PathKind : byte
    {
        Move = 0, Ranged = 1
    }

}
