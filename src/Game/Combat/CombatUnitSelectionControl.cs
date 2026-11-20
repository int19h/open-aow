#nullable disable
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

    // CreateMoveControlSettings builds these settings, and the path writer reads them from here.
    private CombatMoveControlSettings _combatMoveSettings;

    public CustomImageLibrary TargetImages { get; }
    public RangedPathHexSprite RangedPathHexSprite { get; }

    private int _targetX;
    private int _targetY;
    private ushort _cursorImage;
    private ushort _cursorFrame;

    public HexSprite MouseOverTarget { get; private set; }

    public Point3DList RangedPath { get; private set; }
    public DelphiList<RangedObstacle> Obstacles { get; private set; }

    // AoWTC.pas:2733
    public override void Dispose()
    {
        for (int i = 0, last = Obstacles.Count - 1; i <= last; i++)
        {
            (Obstacles[i] as IDisposable)?.Dispose();
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
                    if (GetTacticalBattleUnitHexSprite() is not null && GetTacticalBattleUnitHexSprite().SelectedAbility > -1)
                    {
                        _ = GetTacticalBattleUnitHexSprite().SelectAbility(-1);
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
                            PathStep pathL = MoveSettingsList.GetMoveSettings(0).Path[0];
                            PathStep pathY = MoveSettingsList.GetMoveSettings(0).Path[0];
                            PathStep pathX = MoveSettingsList.GetMoveSettings(0).Path[0];
                            if (!ValidMultiMoveSelection(CombatMap.Current.MapContainer.GetField(pathX.Location.X, pathY.Location.Y, pathL.Location.MapLevel)))
                            {
                                return;
                            }
                        }

                        if (CombatMap.Current.Mouse.HexX != -1 && CombatMap.Current.Mouse.HexY != -1)
                        {
                            int hexY = CombatMap.Current.Mouse.HexY;
                            int hexX = CombatMap.Current.Mouse.HexX;
                            unitHexSprite = CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).FindOwnedChild<TacticalBattleUnitHexSprite>(2228484);
                            if (unitHexSprite is not null && unitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedCombatPlayer)
                            {
                                index = MoveSettingsList.IndexOfMoveObject(unitHexSprite);
                                if (index == -1)
                                {
                                    _ = AddUnit(unitHexSprite);
                                    Update();
                                    CombatMap.Current.Selection.SelectLocationHexSprite(unitHexSprite);
                                    if (unitHexSprite.Unit.GetLocked())
                                    {
                                        int indexOfMoveObject = MoveSettingsList.IndexOfMoveObject(unitHexSprite);
                                        MoveSettingsList.GetMoveSettings(indexOfMoveObject).Path.Clear();
                                    }
                                    else
                                    {
                                        if (MoveSettingsList.GetMoveSettings(0).Path.Count > 0)
                                        {
                                            MoveSettings moveSettingPath = MoveSettingsList.GetMoveSettings(0);
                                            PathStep path = MoveSettingsList.GetMoveSettings(0).Path[moveSettingPath.Path.Count - 1];
                                            deltaX = unitHexSprite.GetHexX() - path.Location.X;
                                            MoveSettings moveSetting = MoveSettingsList.GetMoveSettings(0);
                                            PathStep pathY = MoveSettingsList.GetMoveSettings(0).Path[moveSetting.Path.Count - 1];
                                            deltaY = unitHexSprite.GetHexY() - pathY.Location.Y;
                                            PathStep path2 = MoveSettingsList.GetMoveSettings(0).Path[0];
                                            PathStep pathX = MoveSettingsList.GetMoveSettings(0).Path[0];
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
                                int hexY = CombatMap.Current.Mouse.HexY;
                                int hexX = CombatMap.Current.Mouse.HexX;
                                unitHexSprite = CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).FindOwnedChild<TacticalBattleUnitHexSprite>(2228484);
                                if (unitHexSprite is not null && unitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedCombatPlayer)
                                {
                                    index = MoveSettingsList.IndexOfMoveObject(unitHexSprite);
                                    if (index == -1)
                                    {
                                        _ = AddUnit(unitHexSprite);
                                        Update();
                                        CombatMap.Current.Selection.SelectLocationHexSprite(unitHexSprite);
                                        if (unitHexSprite.Unit.GetLocked())
                                        {
                                            int indexOfMoveObject = MoveSettingsList.IndexOfMoveObject(unitHexSprite);
                                            MoveSettingsList.GetMoveSettings(indexOfMoveObject).Path.Clear();
                                        }
                                        else
                                        {
                                            if (MoveSettingsList.GetMoveSettings(0).Path.Count > 0)
                                            {
                                                MoveSettings moveSettingPath = MoveSettingsList.GetMoveSettings(0);
                                                PathStep path = MoveSettingsList.GetMoveSettings(0).Path[moveSettingPath.Path.Count - 1];
                                                deltaX = unitHexSprite.GetHexX() - path.Location.X;
                                                MoveSettings moveSetting = MoveSettingsList.GetMoveSettings(0);
                                                PathStep pathY = MoveSettingsList.GetMoveSettings(0).Path[moveSetting.Path.Count - 1];
                                                deltaY = unitHexSprite.GetHexY() - pathY.Location.Y;
                                                PathStep path2 = MoveSettingsList.GetMoveSettings(0).Path[0];
                                                PathStep pathX = MoveSettingsList.GetMoveSettings(0).Path[0];
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
                                if (GetTacticalBattleUnitHexSprite() is not null)
                                {
                                    int hexY = CombatMap.Current.Mouse.HexY;
                                    int hexX = CombatMap.Current.Mouse.HexX;
                                    unitHexSprite = CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).FindOwnedChild<TacticalBattleUnitHexSprite>(2228484);
                                    if (GetTacticalBattleUnitHexSprite().SelectedAbility == -1)
                                    {
                                        targets = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
                                    }
                                    else
                                    {
                                        if (GetTacticalBattleUnitHexSprite().SelectedAbility == 52)
                                        {
                                            TacticalBattleUnitHexSprite tacticalBattleUnitHexSprite = GetTacticalBattleUnitHexSprite();
                                            TacticalBattleUnitHexSprite tacticalBattleUnitHexSprite2 = GetTacticalBattleUnitHexSprite();
                                            GamePack.Current.SpellControl.GetSpell(tacticalBattleUnitHexSprite2.SelectedSpell).GetCombatInfo(tacticalBattleUnitHexSprite.Unit.Troop, ref spellInfo);
                                            targets = spellInfo.TargetTypes;
                                        }
                                        else
                                        {
                                            TacticalBattleUnitHexSprite tacticalBattleUnitHexSprite = GetTacticalBattleUnitHexSprite();
                                            TacticalBattleUnitHexSprite tacticalBattleUnitHexSprite2 = GetTacticalBattleUnitHexSprite();
                                            GamePack.Current.AbilityControl.GetAbility(tacticalBattleUnitHexSprite2.SelectedAbility).GetCombatInfo(tacticalBattleUnitHexSprite.Unit.Troop, ref abilityInfo);
                                            targets = abilityInfo.TargetTypes;
                                        }
                                    }

                                    if (unitHexSprite is null)
                                    {
                                        ExecuteMoveSelection();
                                    }
                                    else
                                    {
                                        sbyte player = GetTacticalBattleUnitHexSprite().Unit.GetPlayer();
                                        if (unitHexSprite.Unit.GetPlayer() == player && !targets.Contains(CombatTargetTypeElement.Friend))
                                        {
                                            CombatMap.Current.Selection.UnselectLocation();
                                            CombatMap.Current.Selection.SelectLocationHexSprite(unitHexSprite);
                                        }
                                        else
                                        {
                                            sbyte fUnitPlayer = GetTacticalBattleUnitHexSprite().Unit.GetPlayer();
                                            bool condition = unitHexSprite.Unit.GetPlayer() == fUnitPlayer && targets.Contains(CombatTargetTypeElement.Friend);
                                            if (!condition)
                                            {
                                                sbyte player2 = GetTacticalBattleUnitHexSprite().Unit.GetPlayer();
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
            int hexY = CombatMap.Current.Mouse.HexY;
            int hexX = CombatMap.Current.Mouse.HexX;
            unitHexSprite = CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).FindOwnedChild<TacticalBattleUnitHexSprite>(2228484);
            CombatMap.Current.Selection.UnselectLocation();
            if (unitHexSprite is not null && CombatMap.Current.Mouse.Button == MouseButton.Left)
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
        MoveSettings moveSettings = MoveSettingsList.GetMoveSettings(index);
        temporary = new DelphiList<MovementHexSprite>();
        if (MoveSettingsList.GetCount() > 0)
        {
            for (int i = 0, last = MoveSettingsList.GetCount() - 1; i <= last; i++)
            {
                if (i != index)
                {
                    if (MoveSettingsList.GetMoveSettings(i).Path.Count > 1)
                    {
                        movement = new MovementHexSprite(null) { Holder = Creator.CreationHolder() };
                        MoveSettings moveSettingPath = MoveSettingsList.GetMoveSettings(i);
                        PathStep path = MoveSettingsList.GetMoveSettings(i).Path[moveSettingPath.Path.Count - 1];
                        MoveSettings moveSetting = MoveSettingsList.GetMoveSettings(i);
                        PathStep pathX = MoveSettingsList.GetMoveSettings(i).Path[moveSetting.Path.Count - 1];
                        _ = CombatMap.Current.PlaceHex(movement, pathX.Location.X, path.Location.Y, 0);
                        movement.Enabled = true;
                        _ = temporary.Add(movement);
                        movement = new MovementHexSprite(null) { Holder = Creator.CreationHolder() };
                        PathStep pathY = MoveSettingsList.GetMoveSettings(i).Path[0];
                        PathStep path2 = MoveSettingsList.GetMoveSettings(i).Path[0];
                        _ = CombatMap.Current.PlaceHex(movement, path2.Location.X, pathY.Location.Y, 0);
                        movement.Enabled = false;
                        _ = temporary.Add(movement);
                    }
                }
            }
        }

        moveSettings.PathMovePoints = 0;
        unitHexSprite = (TacticalBattleUnitHexSprite)moveSettings.MoveObject;
        _combatMoveSettings.MoveFlags = GetTacticalBattleUnitHexSprite() is not null && (GetTacticalBattleUnitHexSprite().SelectedAbility == 117 || GetTacticalBattleUnitHexSprite().SelectedAbility == 65)
            ? Set<CombatMoveModeElement>.FromMask((byte)_pathInvalid.Image) + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Climbing)
            : Set<CombatMoveModeElement>.FromMask((byte)_pathInvalid.Image);

        MoveControlSettings.Reset();
        sbyte hexLevel = unitHexSprite.GetHexLevel();
        sbyte hexY = unitHexSprite.GetHexY();
        sbyte hexX = unitHexSprite.GetHexX();
        MoveControlSettings.SetSourcePosition(hexX, hexY, hexLevel);
        sbyte player = unitHexSprite.Unit.GetPlayer();
        MoveControlSettings.Player = player;
        MoveControlSettings.ArmyHexSprite = unitHexSprite;
        MoveControlSettings.Selection = 0;
        MoveControlSettings.SetDestinationPosition(X, Y, L);
        _ = MoveControlSettings.MovePointTable;
        unitHexSprite.Unit.Troop.CreateMovePointTable(out MovePointTable movePointTable);
        MoveControlSettings.MovePointTable = movePointTable;
        CombatMap.Current.MoveControl.ProcessMove(MoveControlSettings);
        MoveSettingsList.GetMoveSettings(index).Path.Assign(MoveControlSettings.Path);
        MoveSettingsList.GetMoveSettings(index).MovePoints = unitHexSprite.Unit.MovePoints;
        int moves = unitHexSprite.Unit.GetMoves();
        MoveSettingsList.GetMoveSettings(index).Moves = moves;
        if (MoveControlSettings.Path.Count != 0)
        {
            for (int i = 0, last = MoveControlSettings.Path.Count - 2; i <= last; i++)
            {
                PathStep pathY = MoveControlSettings.Path[i];
                PathStep pathX = MoveControlSettings.Path[i];
                bool condition = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y].FindOwnedChild(2229508) is not null;
                if (!condition)
                {
                    PathStep path = MoveControlSettings.Path[i];
                    PathStep path2 = MoveControlSettings.Path[i];
                    condition = CombatMap.Current.MapContainer.MapLevels[0].Field[path2.Location.X, path.Location.Y].FindOwnedChild(2229510) is not null;
                }

                if (condition)
                {
                    PathStep path = MoveControlSettings.Path[i];
                    PathStep path2 = MoveControlSettings.Path[i];
                    MapField mapField = CombatMap.Current.MapContainer.MapLevels[0].Field[path2.Location.X, path.Location.Y];
                    moveSettings.Path.List[i] = moveSettings.Path.List[i] with { MoveCost = MoveControlSettings.MovePointTable[mapField.TerrainType][-1 - MovementTables.FirstOverlay] };
                }
                else
                {
                    PathStep path = MoveControlSettings.Path[i];
                    PathStep path2 = MoveControlSettings.Path[i];
                    MapField item = CombatMap.Current.MapContainer.MapLevels[0].Field[path2.Location.X, path.Location.Y];
                    PathStep path3 = MoveControlSettings.Path[i];
                    PathStep path4 = MoveControlSettings.Path[i];
                    MapField fieldItem = CombatMap.Current.MapContainer.MapLevels[0].Field[path4.Location.X, path3.Location.Y];
                    moveSettings.Path.List[i] = moveSettings.Path.List[i] with { MoveCost = MoveControlSettings.MovePointTable[fieldItem.TerrainType][item.OverlayType - MovementTables.FirstOverlay] };
                }
            }

            moveSettings.PathMovePoints = moveSettings.Path.PositionToMovePoints(0);
            MoveControlSettings.Path.Clear();
        }

        for (int i = 0, last = temporary.Count - 1; i <= last; i++)
        {
            (temporary[i] as IDisposable)?.Dispose();
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
        if (CombatMap.Current is null)
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

        if (unitHexSprite is null)
        {
            return;
        }

        int hexY = CombatMap.Current.Mouse.HexY;
        int hexX = CombatMap.Current.Mouse.HexX;
        field = CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel);
        if (CombatMap.Current.ExplorationEnabled && !field.GetExplored(CombatMap.Current.SeatedPlayer))
        {
            goto FinishCursor;
        }

        targetHexSprite = field.FindOwnedChild<TacticalBattleUnitHexSprite>(2228484);
        if (unitHexSprite.Unit.CombatPlayer != CombatMap.Current.SeatedCombatPlayer && targetHexSprite is not null)
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

            if (targetHexSprite is null || (targetHexSprite == unitHexSprite && !targets.Contains(CombatTargetTypeElement.Friend)))
            {
                if (_cursorImage is < 85 or > 88)
                {
                    _cursorImage = 85;
                }
            }
            else
            {
                if (GamePack.Current.AbilityControl.GetAbility(unitHexSprite.SelectedAbility).GetCombatMode(unitHexSprite.Unit.Troop) == CombatMode.Touch)
                {
                    if (((targets.Contains(CombatTargetTypeElement.Friend) && unitHexSprite.Unit.GetSide() == targetHexSprite.Unit.GetSide()) || (targets.Contains(CombatTargetTypeElement.Enemy) && unitHexSprite.Unit.GetSide() != targetHexSprite.Unit.GetSide())) && GamePack.Current.AbilityControl.GetAbility<TouchAbility>(unitHexSprite.SelectedAbility).CanTouch(unitHexSprite.Unit, targetHexSprite.Unit))
                    {
                        if (_cursorImage is < 105 or > 107)
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
                    if ((targets.Contains(CombatTargetTypeElement.Friend) && unitHexSprite.Unit.GetSide() == targetHexSprite.Unit.GetSide()) || (targets.Contains(CombatTargetTypeElement.Enemy) && unitHexSprite.Unit.GetSide() != targetHexSprite.Unit.GetSide()))
                    {
                        if (_cursorImage is < 80 or > 83)
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
            if (targetHexSprite is not null && targetHexSprite != unitHexSprite)
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
                wall = field.FindOwnedChild<CityWall>(2229508) ?? field.FindOwnedChild(2229510) as CityWall;
                if (wall is not null)
                {
                    if (wall is not CityWallDoor || unitHexSprite.Unit.GetSide() == CombatSideIndex.Attacker)
                    {
                        cost = -1;
                    }

                    if (wall is not CityWallDoor && unitHexSprite.Unit.Troop.GetAbilityEnabled(36))
                    {
                        cost = 1;
                    }

                    if (unitHexSprite.Unit.Troop.GetAbilityEnabled(66) || unitHexSprite.Unit.Troop.GetAbilityEnabled(1))
                    {
                        cost = 1;
                    }
                }
            }

            if (cost <= 0 || (CombatMap.Current.ExplorationEnabled && !field.GetExplored(CombatMap.Current.SeatedPlayer)))
            {
                CombatMap.Current.Mouse.Cursor = CombatMap.Current.Mouse.Cursor + 1;
            }

            CombatMap.Current.Mouse.CursorMode = 256;
        }
    }

    // AoWTC.pas:2744
    public override MoveControlSettings CreateMoveControlSettings()
    {
        _combatMoveSettings = new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };
        return _combatMoveSettings;
    }

    // AoWTC.pas:2754
    public override void SetCurrentMove(int value)
    {
        if (CurrentMove != -1 && GetTacticalBattleUnitHexSprite() is not null)
        {
            TacticalBattleUnitHexSprite tacticalBattleUnitHexSprite = GetTacticalBattleUnitHexSprite();
            CombatMap.Current.Selection.TriggerDeselectEvent(tacticalBattleUnitHexSprite);
        }

        base.SetCurrentMove(value);
        if (CurrentMove != -1 && GetTacticalBattleUnitHexSprite() is not null)
        {
            TacticalBattleUnitHexSprite tacticalBattleUnitHexSprite = GetTacticalBattleUnitHexSprite();
            CombatMap.Current.Selection.TriggerSelectEvent(tacticalBattleUnitHexSprite);
        }
    }

    // AoWTC.pas:3316
    public override void Unselect()
    {
        DeactivateRangedPath();
        GetTacticalBattleUnitHexSprite()?.SelectedAbility = -1;

        CombatMap.Current.Selection.UnselectLocation();
        DeactivateMoveSelection();
        base.Unselect();
    }

    // AoWTC.pas:3274
    public override void ShowMovePaths(in LockedSurface surface)
    {
        int x = default;
        int y = default;
        base.ShowMovePaths(surface);
        if (GetTacticalBattleUnitHexSprite() is not null)
        {
            if (GetTacticalBattleUnitHexSprite().SelectedAbility is not 65 and not 114)
            {
                return;
            }

            if (MoveSettingsList.GetCount() == 1 && MoveSettingsList.GetMoveSettings(0).Path.Count > 1)
            {
                for (int i = 1; i <= 6; i++)
                {
                    PathStep path = MoveSettingsList.GetMoveSettings(0).Path[1];
                    HexGeometry.CenterHexNumberToHex(MoveSettingsList.GetMoveSettings(0).Path[1].Location.X, path.Location.Y, i, ref x, ref y);
                    HexGeometry.HexToPixel(x, y, out int pixelX, out int pixelY);
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
        int moves = U.Unit.GetMoves();
        MoveSettingsList.GetMoveSettings(result).Moves = moves;
        return result;
    }

    // AoWTC.pas:2763
    public TacticalBattleUnitHexSprite GetTacticalBattleUnitHexSprite()
    {
        TacticalBattleUnitHexSprite result = CurrentMove != -1
            ? (TacticalBattleUnitHexSprite)MoveSettingsList.GetMoveSettings(CurrentMove).MoveObject
            : null;
        return result;
    }

    // AoWTC.pas:2770
    public Set<MoveType> GetMoveTypes()
    {
        Set<MoveType> result = CurrentMove != -1
            ? ((TacticalBattleUnitHexSprite)MoveSettingsList.GetMoveSettings(CurrentMove).MoveObject).Unit.Troop.GetMoveTypes()
            : Set<MoveType>.Of();
        return result;
    }

    // AoWTC.pas:2777
    public void RectangleSelectEvent(GameMapMouse sender)
    {
        TacticalBattleUnitHexSprite unitHexSprite;
        MapField field;
        MapLevel level;
        DelphiList<TacticalBattleUnitHexSprite> units;
        Rect r;
        units = new DelphiList<TacticalBattleUnitHexSprite>();
        r = sender.SelectionRectangle;
        level = CombatMap.Current.MapContainer.MapLevels[CombatMap.Current.ViewMapLevel];
        HexGeometry.PixelToHex(r.Left, r.Top, out int x1, out int y1);
        HexGeometry.PixelToHex(r.Right, r.Bottom, out int x2, out int y2);
        for (int x = x1 - 1, last = x2 + 1; x <= last; x++)
        {
            for (int y = y1 - 1, lastY = y2 + 1; y <= lastY; y++)
            {
                HexGeometry.HexToPixel(x, y, out int pixelX, out int pixelY);
                pixelX += 16;
                pixelY += 8;
                if (x >= 0 && x < level.Width && y > 0 && y < level.Height && pixelX >= r.Left && pixelX < r.Right && pixelY >= r.Top && pixelY < r.Bottom)
                {
                    field = level.Field[x, y];
                    unitHexSprite = field.FindOwnedChild<TacticalBattleUnitHexSprite>(2228484);
                    if (unitHexSprite is not null && unitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedCombatPlayer)
                    {
                        _ = units.Add(unitHexSprite);
                    }
                }
            }
        }

        if (units.Count != 0)
        {
            Clear();
            CombatMap.Current.Selection.UnselectLocation();
            for (int i = 0, last = units.Count - 1; i <= last; i++)
            {
                _ = AddUnit(units[i]);
                TacticalBattleUnitHexSprite unit = units[i];
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
                range = info.Range > 0 ? (ushort)info.Range : (ushort)100;
            }
            else
            {
                TacticalBattleUnitHexSprite tacticalBattleUnitHexSprite = GetTacticalBattleUnitHexSprite();
                if (GamePack.Current.AbilityControl.GetAbility(unitHexSprite.SelectedAbility).GetCombatMode(tacticalBattleUnitHexSprite.Unit.Troop) == CombatMode.LongRange)
                {
                    range = 12;
                }
                else
                {
                    TacticalBattleUnitHexSprite tacticalBattleUnitHexSprite2 = GetTacticalBattleUnitHexSprite();
                    range = GamePack.Current.AbilityControl.GetAbility(unitHexSprite.SelectedAbility).GetCombatMode(tacticalBattleUnitHexSprite2.Unit.Troop) == CombatMode.MediumRange
                        ? (ushort)8
                        : (ushort)4;
                }
            }

            bool condition = unitHexSprite.Unit.GetMoves() == unitHexSprite.Unit.MovePoints && X > 0;
            if (condition)
            {
                sbyte yhx = unitHexSprite.GetHexY();
                condition = HexGeometry.DeltaHexToRadius(X, Y, unitHexSprite.GetHexX(), yhx) <= range;
            }

            if (condition)
            {
                // { Original retains this unused with temporary. }
                _ = GetTacticalBattleUnitHexSprite();
                _targetX = X;
                _targetY = Y;
                sbyte yhx = unitHexSprite.GetHexY();
                direction = HexGeometry.DeltaHexToHexNumber(unitHexSprite.GetHexX(), yhx, X, Y);
                hexX = X;
                hexY = Y;
                switch (GetTacticalBattleUnitHexSprite().SelectedAbility)
                {
                    case 30:
                    case >= 86 and <= 90:
                        sbyte unitHexSpriteHexY = unitHexSprite.GetHexY();
                        HexGeometry.CenterHexNumberToHex(unitHexSprite.GetHexX(), unitHexSpriteHexY, TacticalTables.BreathHexNumber[(TacticalTables.BreathDirection[direction - 1] * 12) - 12], ref hexX, ref hexY);
                        break;
                    case 52:
                        if (TacticalTables.SpellTypes[unitHexSprite.SelectedSpell] == BattleEffectType.Breath)
                        {
                            sbyte hexY2 = unitHexSprite.GetHexY();
                            HexGeometry.CenterHexNumberToHex(unitHexSprite.GetHexX(), hexY2, TacticalTables.BreathHexNumber[(TacticalTables.BreathDirection[direction - 1] * 12) - 12], ref hexX, ref hexY);
                        }

                        break;
                    default:
                        break;
                }

                sbyte xhx = GetTacticalBattleUnitHexSprite().GetHexX();
                sbyte hexY3 = GetTacticalBattleUnitHexSprite().GetHexY();
                CombatSideIndex side = GetTacticalBattleUnitHexSprite().Unit.GetSide();
                TacticalBattleUnitHexSprite tacticalBattleUnitHexSprite = GetTacticalBattleUnitHexSprite();
                Point3DList rangedPath = RangedPath;
                DelphiList<RangedObstacle> obstacles = Obstacles;
                TacticalTables.MakeAbilityPath(side, (short)GetTacticalBattleUnitHexSprite().SelectedAbility, (short)tacticalBattleUnitHexSprite.SelectedSpell, xhx, hexY3, (short)hexX, (short)hexY, rangedPath, obstacles);
                RangedPath = rangedPath;
                Obstacles = obstacles;
            }
        }

        RangedPathHexSprite.Selection = this;
        _ = CombatMap.Current.DynamicFixedVisibleHexSprite.AddChild(RangedPathHexSprite);
    }

    // AoWTC.pas:2918
    public void MoveSelectedRoute()
    {
        TacticalBattleUnitHexSprite unitHexSprite;
        EngineObject target;
        int movePoints;
        if (MoveSettingsList.GetCount() > 0)
        {
            if (GetTacticalBattleUnitHexSprite() is not null)
            {
                if (MoveSettingsList.GetMoveSettings(0).Path.Count > 0)
                {
                    MoveSettings moveSettings = MoveSettingsList.GetMoveSettings(0);
                    unitHexSprite = GetTacticalBattleUnitHexSprite();
                    if (unitHexSprite.SelectedAbility != 117)
                    {
                        PathStep pathY = moveSettings.Path[0];
                        PathStep pathX = moveSettings.Path[0];
                        target = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y].FindOwnedChild(2228484);
                    }
                    else
                    {
                        PathStep pathY = moveSettings.Path[0];
                        PathStep pathX = moveSettings.Path[0];
                        target = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y].FindOwnedChild(2229508);
                        if (target is null)
                        {
                            PathStep path = moveSettings.Path[0];
                            PathStep path2 = moveSettings.Path[0];
                            target = CombatMap.Current.MapContainer.MapLevels[0].Field[path2.Location.X, path.Location.Y].FindOwnedChild(2229510);
                        }
                    }

                    if (MoveSettingsList.GetMoveSettings(0).Path.MovePointsToPosition(unitHexSprite.Unit.MovePoints) == 0 && (target is not null || unitHexSprite.SelectedAbility is 65 or 114))
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
                    for (int i = 0, last = MoveSettingsList.GetCount() - 1; i <= last; i++)
                    {
                        MoveSettings moveSettings = MoveSettingsList.GetMoveSettings(i);
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
        int x = default;
        int y = default;
        int direction;
        bool AllPathsValid()
        {
            int i;
            bool condition = MoveSettingsList.GetMoveSettings(0).Path.Count > 0;
            if (condition)
            {
                PathStep path = MoveSettingsList.GetMoveSettings(0).Path[0];
                int hexX = CombatMap.Current.Mouse.HexX;
                condition = path.Location.X == hexX;
            }

            bool test = condition;
            if (test)
            {
                PathStep path = MoveSettingsList.GetMoveSettings(0).Path[0];
                int hexY = CombatMap.Current.Mouse.HexY;
                test = path.Location.Y == hexY;
            }

            bool result = test;
            i = MoveSettingsList.GetCount();
            while (result && i != 0)
            {
                i -= 1;
                MoveSettings moveSettings = MoveSettingsList.GetMoveSettings(i);
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
                        int hexY = CombatMap.Current.Mouse.HexY;
                        int hexX = CombatMap.Current.Mouse.HexX;
                        notExplorationEnabled = CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).GetExplored(CombatMap.Current.SeatedPlayer);
                    }

                    if (notExplorationEnabled)
                    {
                        for (int i = 0, last = MoveSettingsList.GetCount() - 1; i <= last; i++)
                        {
                            MoveSettingsList.GetMoveSettings(i).Path.Clear();
                        }

                        for (int a = 0, last = MoveSettingsList.GetCount() - 1; a <= last; a++)
                        {
                            for (int b = a + 1, lastB = MoveSettingsList.GetCount() - 1; b <= lastB; b++)
                            {
                                sbyte hexY = ((HexSprite)MoveSettingsList.GetMoveSettings(b).MoveObject).GetHexY();
                                sbyte hexX = ((HexSprite)MoveSettingsList.GetMoveSettings(b).MoveObject).GetHexX();
                                int mouseHexY = CombatMap.Current.Mouse.HexY;
                                int deltaHexToRadius2 = HexGeometry.DeltaHexToRadius(CombatMap.Current.Mouse.HexX, mouseHexY, hexX, hexY);
                                sbyte hexY2 = ((HexSprite)MoveSettingsList.GetMoveSettings(a).MoveObject).GetHexY();
                                sbyte hexX2 = ((HexSprite)MoveSettingsList.GetMoveSettings(a).MoveObject).GetHexX();
                                int hexY3 = CombatMap.Current.Mouse.HexY;
                                if (deltaHexToRadius2 < HexGeometry.DeltaHexToRadius(CombatMap.Current.Mouse.HexX, hexY3, hexX2, hexY2))
                                {
                                    MoveSettingsList.ExchangeChildren(a, b);
                                }
                            }
                        }

                        if (((TacticalBattleUnitHexSprite)MoveSettingsList.GetMoveSettings(0).MoveObject).Unit.GetLocked())
                        {
                            MoveSettingsList.GetMoveSettings(0).Path.Clear();
                        }
                        else
                        {
                            int hexY = CombatMap.Current.Mouse.HexY;
                            CalculateMovePath(0, (sbyte)CombatMap.Current.Mouse.HexX, (sbyte)hexY, (sbyte)CombatMap.Current.ViewMapLevel);
                        }

                        if (MoveSettingsList.GetMoveSettings(0).Path.Count > 1)
                        {
                            for (int i = 1, last = MoveSettingsList.GetCount() - 1; i <= last; i++)
                            {
                                if (((TacticalBattleUnitHexSprite)MoveSettingsList.GetMoveSettings(i).MoveObject).Unit.GetLocked())
                                {
                                    MoveSettingsList.GetMoveSettings(i).Path.Clear();
                                }
                                else
                                {
                                    sbyte hexY = ((HexSprite)MoveSettingsList.GetMoveSettings(i).MoveObject).GetHexY();
                                    sbyte hexX = ((HexSprite)MoveSettingsList.GetMoveSettings(i).MoveObject).GetHexX();
                                    sbyte hexY2 = ((HexSprite)MoveSettingsList.GetMoveSettings(0).MoveObject).GetHexY();
                                    direction = HexGeometry.DeltaHexToHexNumber(((HexSprite)MoveSettingsList.GetMoveSettings(0).MoveObject).GetHexX(), hexY2, hexX, hexY);
                                    PathStep path = MoveSettingsList.GetMoveSettings(0).Path[0];
                                    HexGeometry.CenterHexNumberToHex(MoveSettingsList.GetMoveSettings(0).Path[0].Location.X, path.Location.Y, direction, ref x, ref y);
                                    CalculateMovePath(i, (sbyte)x, (sbyte)y, 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0, last = MoveSettingsList.GetCount() - 1; i <= last; i++)
                        {
                            MoveSettingsList.GetMoveSettings(i).Path.Clear();
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
                MoveSettings moveSettings = MoveSettingsList.GetMoveSettings(CurrentMove);
                bool condition = moveSettings.Path.Count != 0;
                if (condition)
                {
                    PathStep path = moveSettings.Path[0];
                    int hexX = CombatMap.Current.Mouse.HexX;
                    condition = path.Location.X == hexX;
                }

                bool test = condition;
                if (test)
                {
                    PathStep path = moveSettings.Path[0];
                    int hexY = CombatMap.Current.Mouse.HexY;
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
                    int hexY = CombatMap.Current.Mouse.HexY;
                    int hexX = CombatMap.Current.Mouse.HexX;
                    CalculateMovePath(CurrentMove, (sbyte)hexX, (sbyte)hexY, (sbyte)CombatMap.Current.ViewMapLevel);
                }
            }
        }
        else
        {
            if (GetTacticalBattleUnitHexSprite().SelectedAbility == -1 && GetTacticalBattleUnitHexSprite().GetHexX() == CombatMap.Current.Mouse.HexX && GetTacticalBattleUnitHexSprite().GetHexY() == CombatMap.Current.Mouse.HexY)
            {
                sbyte hexLevel = GetTacticalBattleUnitHexSprite().GetHexLevel();
                sbyte hexY = GetTacticalBattleUnitHexSprite().GetHexY();
                sbyte hexX = GetTacticalBattleUnitHexSprite().GetHexX();
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
                        int hexX = CombatMap.Current.Mouse.HexX;
                        condition = _targetX == hexX;
                    }

                    bool test = condition;
                    if (test)
                    {
                        int hexY = CombatMap.Current.Mouse.HexY;
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
                            sbyte hexLevel = unitHexSprite.GetHexLevel();
                            sbyte hexY = unitHexSprite.GetHexY();
                            sbyte hexX = unitHexSprite.GetHexX();
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
                                        int hexY = CombatMap.Current.Mouse.HexY;
                                        int hexX = CombatMap.Current.Mouse.HexX;
                                        if (!CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).GetExplored(CombatMap.Current.SeatedPlayer))
                                        {
                                            return;
                                        }
                                    }

                                    int mouseHexY = CombatMap.Current.Mouse.HexY;
                                    int mouseHexX = CombatMap.Current.Mouse.HexX;
                                    CalculateMovePath(CurrentMove, (sbyte)mouseHexX, (sbyte)mouseHexY, 0);
                                }
                            }
                            else
                            {
                                effectType = unitHexSprite.SelectedAbility == 52
                                    ? TacticalTables.SpellTypes[unitHexSprite.SelectedSpell]
                                    : TacticalTables.AbilityTypes[unitHexSprite.SelectedAbility];

                                switch (effectType)
                                {
                                    case BattleEffectType.None:
                                        _ = unitHexSprite.SelectAbility(-1);
                                        break;
                                    case BattleEffectType.Touch:
                                        if (CombatMap.Current.ExplorationEnabled)
                                        {
                                            int hexY = CombatMap.Current.Mouse.HexY;
                                            int hexX = CombatMap.Current.Mouse.HexX;
                                            if (!CombatMap.Current.MapContainer.GetField(hexX, hexY, CombatMap.Current.ViewMapLevel).GetExplored(CombatMap.Current.SeatedPlayer))
                                            {
                                                return;
                                            }
                                        }

                                        if (!unitHexSprite.Unit.GetLocked())
                                        {
                                            int hexY = CombatMap.Current.Mouse.HexY;
                                            CalculateMovePath(0, (sbyte)CombatMap.Current.Mouse.HexX, (sbyte)hexY, 0);
                                        }

                                        break;
                                    case BattleEffectType.Arrow:
                                    case BattleEffectType.Rock:
                                    case BattleEffectType.Comet:
                                    case BattleEffectType.Breath:
                                    case BattleEffectType.CometBall:
                                    case BattleEffectType.Effect:
                                    case BattleEffectType.FBExplosion:
                                    case BattleEffectType.ScreenNESW:
                                    case BattleEffectType.ScreenNS:
                                    case BattleEffectType.Ray:
                                    case BattleEffectType.Chain:
                                    case BattleEffectType.Swarm:
                                    case BattleEffectType.ScreenSwarm:
                                    case BattleEffectType.SFOverlay:
                                    case BattleEffectType.DamLightning:
                                    case BattleEffectType.DamFire:
                                    case BattleEffectType.DamPoison:
                                    case BattleEffectType.DamMagic:
                                    case BattleEffectType.DamHoly:
                                    case BattleEffectType.DamDeath:
                                    case BattleEffectType.DamCold:
                                    case BattleEffectType.SacredWrath:
                                    case BattleEffectType.SWComet:
                                    case BattleEffectType.Dispel:
                                    case BattleEffectType.WallSmoke:
                                    case BattleEffectType.Blood:
                                    case BattleEffectType.AltArrow:
                                    case BattleEffectType.Tremors:
                                    case BattleEffectType.Geyser:
                                    case BattleEffectType.GeyserWater:
                                    case BattleEffectType.HailImpact:
                                    case BattleEffectType.Cloud:
                                    case BattleEffectType.CloudPuff:
                                    case BattleEffectType.HitGround:
                                    case BattleEffectType.HitWater:
                                    case BattleEffectType.HighPrayer:
                                    case BattleEffectType.PrayerGlow:
                                    case BattleEffectType.Resurrect:
                                    case BattleEffectType.FrostBeam:
                                    case BattleEffectType.Frost:
                                    case BattleEffectType.MindDecay:
                                    case BattleEffectType.DecayLeg:
                                    case BattleEffectType.ShockWave:
                                    case BattleEffectType.DecayComet:
                                    case BattleEffectType.SacFlame:
                                    case BattleEffectType.Winds:
                                    case BattleEffectType.SacFire:
                                    case BattleEffectType.BreathDam:
                                    default:
                                        {
                                            int hexY = CombatMap.Current.Mouse.HexY;
                                            CalculateRangedPath((sbyte)CombatMap.Current.Mouse.HexX, (sbyte)hexY);
                                            if (RangedPath.Count > 0)
                                            {
                                                int mouseHexY = CombatMap.Current.Mouse.HexY;
                                                int hexX = CombatMap.Current.Mouse.HexX;
                                                MouseOverTarget = CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, mouseHexY].FindOwnedChild<HexSprite>(2228484);
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
        if (GetTacticalBattleUnitHexSprite() is not null)
        {
            _ = MoveControlSettings.MovePointTable;
            GetTacticalBattleUnitHexSprite().Unit.Troop.CreateMovePointTable(out MovePointTable movePointTable);
            MoveControlSettings.MovePointTable = movePointTable;
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
                if (U is not null && U.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer)
                {
                    ActivateMoveSelection();
                    _ = AddUnit(U);
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
        if (U is not null)
        {
            CombatMap.Current.Selection.SelectLocationHexSprite(U);
        }
    }

    // AoWTC.pas:3331
    public void DeactivateRangedPath()
    {
        if (RangedPathHexSprite.Selection is not null)
        {
            _ = CombatMap.Current.DynamicFixedVisibleHexSprite.RemoveChild(RangedPathHexSprite);
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
            multiMoveTokenEvent = new MultiMoveTokenEvent(null)
            {
                Holder = moves,
                Simultaneous = false
            };
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
                        _ = movePath.Add(moveSettings3.Path[l]);
                    }
                    combatMoveTokenEvent = new CombatMoveTokenEvent(null) { Holder = moves };
                    if (combatMoveTokenEvent.Setup(((TacticalBattleUnitHexSprite)moveSettings3.MoveObject).Unit, movePath))
                    {
                        multiMoveTokenEvent.Events.Add(combatMoveTokenEvent);
                    }
                    _ = combatMoveTokenEvent.Release();
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
            _ = multiMoveTokenEvent.Release();
            movePath?.Dispose();
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
