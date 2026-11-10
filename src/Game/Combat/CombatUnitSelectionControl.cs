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

// FPathInvalid. FTargetImages. FRangedPathHS. FTargetX. FTargetY. FCursorImage. FCursorFrame.
// FRangedPath. FObstacles
public partial class CombatUnitSelectionControl : MoveSelector
{
    // AoWTC.pas:2723
    public CombatUnitSelectionControl() : base()
    {
        FRangedPath = new XYZList();
        FObstacles = new DelphiList();
        FRangedPathHS = new RangedPathHS(null);
        FRangedPathHS.Activate();
        FTargetImages = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TargHex.ILB");
    }

    public Set<AnonymousEnum_piMove> FPathInvalid;
    public CustomImageLibrary FTargetImages;
    public RangedPathHS FRangedPathHS;
    public int FTargetX;
    public int FTargetY;
    public ushort FCursorImage;
    public ushort FCursorFrame;
    public HexagonSprite FMouseOverTarget;
    public XYZList FRangedPath;
    public DelphiList FObstacles;

    // AoWTC.pas:2733
    public override void Dispose()
    {
        for (int I = 0, last = FObstacles.Count - 1; I <= last; I++)
        {
            (FObstacles[I] as IDisposable)?.Dispose();
        }

        FObstacles?.Dispose();
        FRangedPath?.Dispose();
        FRangedPathHS?.Dispose();
        AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TargHex.ILB");
        base.Dispose();
    }

    // AoWTC.pas:3080
    public override void MouseDownEvent(object Sender)
    {
        TacticalCombatUnitHS UnitHS;
        int Index;
        int DX;
        int DY;
        Set<CombatTargetTypeElement> Targets;
        SpellCombatInfo SpellInfo = default;
        AbilityCombatInfo AbilityInfo = default;
        if (AoWCombatMap.Current.FSeatedCombatPlayer == AoWCombatMap.Current.FCurrentCombatPlayer)
        {
            if (!AoWCombatMap.Current.FPlayers.GetPlayers(AoWCombatMap.Current.FCurrentCombatPlayer).FAuto)
            {
                if (AoWCombatMap.Current.Mouse.Button == MouseButton.Right)
                {
                    if (GetTCUnitHS() != null && GetTCUnitHS().FSelectedAbility > -1)
                    {
                        GetTCUnitHS().SelectAbility(-1);
                        return;
                    }

                    Unselect();
                }
                else
                {
                    if (AoWCombatMap.Current.Mouse.Shift.Contains(ShiftState.Shift))
                    {
                        if (FMoveSettingsList.GetCount() == 1 && FMoveSettingsList.GetMoveSettings(0).FPath.Count != 0)
                        {
                            var pathL = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                            var pathY = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                            var pathX = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                            if (!ValidMultiMoveSelection((AoWMapField)AoWCombatMap.Current.MapContainer.GetField(pathX.X, pathY.Y, pathL.L)))
                            {
                                return;
                            }
                        }

                        if (AoWCombatMap.Current.Mouse.Xhx != -1 && AoWCombatMap.Current.Mouse.Yhx != -1)
                        {
                            var yhx = AoWCombatMap.Current.Mouse.Yhx;
                            var xhx = AoWCombatMap.Current.Mouse.Xhx;
                            UnitHS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL).FindOwnedChild(2228484);
                            if (UnitHS != null && UnitHS.FUnit.FCombatPlayer == AoWCombatMap.Current.FSeatedCombatPlayer)
                            {
                                Index = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                if (Index == -1)
                                {
                                    AddUnit(UnitHS);
                                    Update();
                                    AoWCombatMap.Current.Selection.SelectLocationHS(UnitHS);
                                    if (UnitHS.FUnit.GetLocked())
                                    {
                                        var indexOfMoveObject = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                        FMoveSettingsList.GetMoveSettings(indexOfMoveObject).FPath.Clear();
                                    }
                                    else
                                    {
                                        if (FMoveSettingsList.GetMoveSettings(0).FPath.Count > 0)
                                        {
                                            var moveSettingPath = FMoveSettingsList.GetMoveSettings(0);
                                            var path = FMoveSettingsList.GetMoveSettings(0).FPath[moveSettingPath.FPath.Count - 1];
                                            DX = UnitHS.GetXhx() - path.X;
                                            var moveSetting = FMoveSettingsList.GetMoveSettings(0);
                                            var pathY = FMoveSettingsList.GetMoveSettings(0).FPath[moveSetting.FPath.Count - 1];
                                            DY = UnitHS.GetYhx() - pathY.Y;
                                            var path2 = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                            var pathX = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                            CalculateMovePath(FMoveSettingsList.IndexOfMoveObject(UnitHS), (sbyte)(pathX.X + DX), (sbyte)(path2.Y + DY), 0);
                                        }
                                    }

                                    SetCurrentMove(-1);
                                }
                                else
                                {
                                    UnitHS.Unselect();
                                    DeleteMoveObject(Index);
                                    SetCurrentMove(FMoveSettingsList.GetCount() - 1);
                                    Update();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (AoWCombatMap.Current.Mouse.Shift.Contains(ShiftState.Ctrl))
                        {
                            if (FMoveSettingsList.GetCount() > 0 && AoWCombatMap.Current.Mouse.Xhx != -1 && AoWCombatMap.Current.Mouse.Yhx != -1)
                            {
                                var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                var xhx = AoWCombatMap.Current.Mouse.Xhx;
                                UnitHS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL).FindOwnedChild(2228484);
                                if (UnitHS != null && UnitHS.FUnit.FCombatPlayer == AoWCombatMap.Current.FSeatedCombatPlayer)
                                {
                                    Index = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                    if (Index == -1)
                                    {
                                        AddUnit(UnitHS);
                                        Update();
                                        AoWCombatMap.Current.Selection.SelectLocationHS(UnitHS);
                                        if (UnitHS.FUnit.GetLocked())
                                        {
                                            var indexOfMoveObject = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                            FMoveSettingsList.GetMoveSettings(indexOfMoveObject).FPath.Clear();
                                        }
                                        else
                                        {
                                            if (FMoveSettingsList.GetMoveSettings(0).FPath.Count > 0)
                                            {
                                                var moveSettingPath = FMoveSettingsList.GetMoveSettings(0);
                                                var path = FMoveSettingsList.GetMoveSettings(0).FPath[moveSettingPath.FPath.Count - 1];
                                                DX = UnitHS.GetXhx() - path.X;
                                                var moveSetting = FMoveSettingsList.GetMoveSettings(0);
                                                var pathY = FMoveSettingsList.GetMoveSettings(0).FPath[moveSetting.FPath.Count - 1];
                                                DY = UnitHS.GetYhx() - pathY.Y;
                                                var path2 = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                                var pathX = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                                CalculateMovePath(FMoveSettingsList.IndexOfMoveObject(UnitHS), (sbyte)(pathX.X + DX), (sbyte)(path2.Y + DY), 0);
                                            }
                                        }

                                        SetCurrentMove(FMoveSettingsList.IndexOfMoveObject(UnitHS));
                                    }
                                    else
                                    {
                                        if (FCurrentMove != Index)
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
                            if (AoWCombatMap.Current.Mouse.Xhx != -1 && AoWCombatMap.Current.Mouse.Yhx != -1)
                            {
                                if (GetTCUnitHS() != null)
                                {
                                    var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                    var xhx = AoWCombatMap.Current.Mouse.Xhx;
                                    UnitHS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL).FindOwnedChild(2228484);
                                    if (GetTCUnitHS().FSelectedAbility == -1)
                                    {
                                        Targets = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
                                    }
                                    else
                                    {
                                        if (GetTCUnitHS().FSelectedAbility == 52)
                                        {
                                            var tcUnitHS = GetTCUnitHS();
                                            var tcUnitHS2 = GetTCUnitHS();
                                            AoWHSSet.Current.FSpellControl.GetSpell(tcUnitHS2.FSelectedSpell).GetCombatInfo(tcUnitHS.FUnit.FUnit, ref SpellInfo);
                                            Targets = SpellInfo.TargetTypes;
                                        }
                                        else
                                        {
                                            var tcUnitHS = GetTCUnitHS();
                                            var tcUnitHS2 = GetTCUnitHS();
                                            AoWHSSet.Current.FAbilityControl.GetAbility(tcUnitHS2.FSelectedAbility).GetCombatInfo(tcUnitHS.FUnit.FUnit, ref AbilityInfo);
                                            Targets = AbilityInfo.TargetTypes;
                                        }
                                    }

                                    if (UnitHS == null)
                                    {
                                        ExecuteMoveSelection();
                                    }
                                    else
                                    {
                                        var player = GetTCUnitHS().FUnit.GetPlayer();
                                        if (UnitHS.FUnit.GetPlayer() == player && !Targets.Contains(CombatTargetTypeElement.Friend))
                                        {
                                            AoWCombatMap.Current.Selection.UnselectLocation();
                                            AoWCombatMap.Current.Selection.SelectLocationHS(UnitHS);
                                        }
                                        else
                                        {
                                            var fUnitPlayer = GetTCUnitHS().FUnit.GetPlayer();
                                            bool condition = UnitHS.FUnit.GetPlayer() == fUnitPlayer && Targets.Contains(CombatTargetTypeElement.Friend);
                                            if (!condition)
                                            {
                                                var player2 = GetTCUnitHS().FUnit.GetPlayer();
                                                condition = UnitHS.FUnit.GetPlayer() != player2 && Targets.Contains(CombatTargetTypeElement.Enemy);
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
            var yhx = AoWCombatMap.Current.Mouse.Yhx;
            var xhx = AoWCombatMap.Current.Mouse.Xhx;
            UnitHS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL).FindOwnedChild(2228484);
            AoWCombatMap.Current.Selection.UnselectLocation();
            if (UnitHS != null && AoWCombatMap.Current.Mouse.Button == MouseButton.Left)
            {
                AoWCombatMap.Current.Selection.SelectLocationHS(UnitHS);
            }
        }
    }

    // AoWTC.pas:2823
    public override void CalculateMovePath(int Index, sbyte X, sbyte Y, sbyte L)
    {
        TacticalCombatUnitHS UnitHS;
        DelphiList Temporary;
        MovementHS Movement;
        var moveSettings = FMoveSettingsList.GetMoveSettings(Index);
        Temporary = new DelphiList();
        if (FMoveSettingsList.GetCount() > 0)
        {
            for (int I = 0, last = FMoveSettingsList.GetCount() - 1; I <= last; I++)
            {
                if (I != Index)
                {
                    if (FMoveSettingsList.GetMoveSettings(I).FPath.Count > 1)
                    {
                        Movement = new MovementHS(null);
                        var moveSettingPath = FMoveSettingsList.GetMoveSettings(I);
                        var path = FMoveSettingsList.GetMoveSettings(I).FPath[moveSettingPath.FPath.Count - 1];
                        var moveSetting = FMoveSettingsList.GetMoveSettings(I);
                        var pathX = FMoveSettingsList.GetMoveSettings(I).FPath[moveSetting.FPath.Count - 1];
                        AoWCombatMap.Current.PlaceHX(Movement, pathX.X, path.Y, 0);
                        Movement.FEnabled = true;
                        Temporary.Add(Movement);
                        Movement = new MovementHS(null);
                        var pathY = FMoveSettingsList.GetMoveSettings(I).FPath[0];
                        var path2 = FMoveSettingsList.GetMoveSettings(I).FPath[0];
                        AoWCombatMap.Current.PlaceHX(Movement, path2.X, pathY.Y, 0);
                        Movement.FEnabled = false;
                        Temporary.Add(Movement);
                    }
                }
            }
        }

        moveSettings.FPathMovePoints = 0;
        UnitHS = (TacticalCombatUnitHS)moveSettings.FMoveObject;
        if (GetTCUnitHS() != null && (GetTCUnitHS().FSelectedAbility == 117 || GetTCUnitHS().FSelectedAbility == 65))
        {
            ((CombatMoveControlSettings)FMoveControlSettings).FMoveFlags = Set<CombatMoveModeElement>.FromMask((byte)FPathInvalid.Image) + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Climbing);
        }
        else
        {
            ((CombatMoveControlSettings)FMoveControlSettings).FMoveFlags = Set<CombatMoveModeElement>.FromMask((byte)FPathInvalid.Image);
        }

        FMoveControlSettings.Reset();
        var lhx = UnitHS.GetLhx();
        var yhx = UnitHS.GetYhx();
        var xhx = UnitHS.GetXhx();
        FMoveControlSettings.SetSourcePos(xhx, yhx, lhx);
        var player = UnitHS.FUnit.GetPlayer();
        FMoveControlSettings.FPlayer = player;
        FMoveControlSettings.FArmyHS = UnitHS;
        FMoveControlSettings.FSelection = 0;
        FMoveControlSettings.SetDestinationPos(X, Y, L);
        UnitHS.FUnit.FUnit.CreateMovePointTable(ref FMoveControlSettings.FMovePointTable);
        AoWCombatMap.Current.FMoveControl.ProcessMove(FMoveControlSettings);
        FMoveSettingsList.GetMoveSettings(Index).FPath.Assign(FMoveControlSettings.FPath);
        FMoveSettingsList.GetMoveSettings(Index).FMovePoints = UnitHS.FUnit.FMovePoints;
        var moves = UnitHS.FUnit.GetMoves();
        FMoveSettingsList.GetMoveSettings(Index).FMoves = moves;
        if (FMoveControlSettings.FPath.Count != 0)
        {
            for (int I = 0, last = FMoveControlSettings.FPath.Count - 2; I <= last; I++)
            {
                var pathY = FMoveControlSettings.FPath[I];
                var pathX = FMoveControlSettings.FPath[I];
                bool condition = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y].FindOwnedChild(2229508) != null;
                if (!condition)
                {
                    var path = FMoveControlSettings.FPath[I];
                    var path2 = FMoveControlSettings.FPath[I];
                    condition = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path2.X, path.Y].FindOwnedChild(2229510) != null;
                }

                if (condition)
                {
                    var path = FMoveControlSettings.FPath[I];
                    var path2 = FMoveControlSettings.FPath[I];
                    var mapField = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path2.X, path.Y];
                    moveSettings.FPath.List[I].Flag = FMoveControlSettings.FMovePointTable[mapField.TerrainType][-1 - -1];
                }
                else
                {
                    var path = FMoveControlSettings.FPath[I];
                    var path2 = FMoveControlSettings.FPath[I];
                    var item = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path2.X, path.Y];
                    var path3 = FMoveControlSettings.FPath[I];
                    var path4 = FMoveControlSettings.FPath[I];
                    var fieldItem = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path4.X, path3.Y];
                    moveSettings.FPath.List[I].Flag = FMoveControlSettings.FMovePointTable[fieldItem.TerrainType][item.OverlayType - -1];
                }
            }

            moveSettings.FPathMovePoints = moveSettings.FPath.PosToMovePoints(0);
            FMoveControlSettings.FPath.Clear();
        }

        for (int I = 0, last = Temporary.Count - 1; I <= last; I++)
        {
            (Temporary[I] as IDisposable)?.Dispose();
        }

        Temporary?.Dispose();
    }

    // AoWTC.pas:3178
    public override void UpdateMoveCursor()
    {
        AoWMapField Field;
        int Cost;
        TacticalCombatUnitHS UnitHS;
        TacticalCombatUnitHS TargetHS;
        Set<CombatTargetTypeElement> Targets;
        CityWall Wall;
        SpellCombatInfo SpellInfo = default;
        AbilityCombatInfo AbilityInfo = default;
        if (AoWCombatMap.Current == null)
        {
            return;
        }

        UnitHS = GetTCUnitHS();
        AoWCombatMap.Current.Mouse.CursorMode = 0;
        if (AoWCombatMap.Current.Mouse.Xhx == -1)
        {
            return;
        }

        if (AoWCombatMap.Current.Mouse.Yhx == -1)
        {
            return;
        }

        if (UnitHS == null)
        {
            return;
        }

        var yhx = AoWCombatMap.Current.Mouse.Yhx;
        var xhx = AoWCombatMap.Current.Mouse.Xhx;
        Field = (AoWMapField)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL);
        if (AoWCombatMap.Current.FExplorationEnabled && !Field.GetExplored(AoWCombatMap.Current.FSeatedPlayer))
        {
            goto FinishCursor;
        }

        TargetHS = (TacticalCombatUnitHS)Field.FindOwnedChild(2228484);
        if (UnitHS.FUnit.FCombatPlayer != AoWCombatMap.Current.FSeatedCombatPlayer && TargetHS != null)
        {
            AoWCombatMap.Current.Mouse.CursorMode = 257;
            goto FinishCursor;
        }

        if (UnitHS.FSelectedAbility > -1)
        {
            FCursorFrame = (ushort)(FCursorFrame + 1);
            if (FCursorFrame >= 5)
            {
                FCursorFrame = 0;
                FCursorImage = (ushort)(FCursorImage + 1);
            }

            if (UnitHS.FSelectedAbility == 52)
            {
                AoWHSSet.Current.FSpellControl.GetSpell(UnitHS.FSelectedSpell).GetCombatInfo(UnitHS.FUnit.FUnit, ref SpellInfo);
                Targets = SpellInfo.TargetTypes;
            }
            else
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatInfo(UnitHS.FUnit.FUnit, ref AbilityInfo);
                Targets = AbilityInfo.TargetTypes;
            }

            if (TargetHS == null || (TargetHS == UnitHS && !Targets.Contains(CombatTargetTypeElement.Friend)))
            {
                if (FCursorImage < 85 || FCursorImage > 88)
                {
                    FCursorImage = 85;
                }
            }
            else
            {
                if (AoWHSSet.Current.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatMode(UnitHS.FUnit.FUnit) == CombatMode.Touch)
                {
                    if ((Targets.Contains(CombatTargetTypeElement.Friend) && UnitHS.FUnit.GetSide() == TargetHS.FUnit.GetSide() || Targets.Contains(CombatTargetTypeElement.Enemy) && UnitHS.FUnit.GetSide() != TargetHS.FUnit.GetSide()) && ((TouchAbility)AoWHSSet.Current.FAbilityControl.GetAbility(UnitHS.FSelectedAbility)).CanTouch(UnitHS.FUnit, TargetHS.FUnit))
                    {
                        if (FCursorImage < 105 || FCursorImage > 107)
                        {
                            FCursorImage = 105;
                        }
                    }
                    else
                    {
                        FCursorImage = 89;
                    }
                }
                else
                {
                    if (Targets.Contains(CombatTargetTypeElement.Friend) && UnitHS.FUnit.GetSide() == TargetHS.FUnit.GetSide() || Targets.Contains(CombatTargetTypeElement.Enemy) && UnitHS.FUnit.GetSide() != TargetHS.FUnit.GetSide())
                    {
                        if (FCursorImage < 80 || FCursorImage > 83)
                        {
                            FCursorImage = 80;
                        }
                    }
                    else
                    {
                        FCursorImage = 89;
                    }
                }
            }

            AoWCombatMap.Current.Mouse.CursorMode = 256;
            AoWCombatMap.Current.Mouse.Cursor = FCursorImage;
        }
        else
        {
            if (TargetHS != null && TargetHS != UnitHS)
            {
                if (TargetHS.FUnit.GetPlayer() == UnitHS.FUnit.GetPlayer())
                {
                    AoWCombatMap.Current.Mouse.CursorMode = 257;
                }
                else
                {
                    if (UnitHS.FUnit.GetAbilityEnabled(1) || !TargetHS.FUnit.GetAbilityEnabled(1))
                    {
                        AoWCombatMap.Current.Mouse.CursorMode = 258;
                    }
                    else
                    {
                        FCursorImage = 89;
                        AoWCombatMap.Current.Mouse.CursorMode = 256;
                        AoWCombatMap.Current.Mouse.Cursor = FCursorImage;
                    }
                }
            }
        }

    FinishCursor:
        if (AoWCombatMap.Current.Mouse.CursorMode != 0)
        {
            return;
        }

        if (UnitHS.FUnit.GetPlayer() == AoWCombatMap.Current.FSeatedPlayer)
        {
            if (GetMoveTypes().Contains(MoveType.Walking))
            {
                AoWCombatMap.Current.Mouse.Cursor = 64;
            }
            else
            {
                if (GetMoveTypes().Contains(MoveType.Swimming))
                {
                    AoWCombatMap.Current.Mouse.Cursor = 66;
                }
                else
                {
                    if (GetMoveTypes().Contains(MoveType.Flying))
                    {
                        AoWCombatMap.Current.Mouse.Cursor = 68;
                    }
                }
            }

            Cost = FMoveControlSettings.FMovePointTable[Field.TerrainType][Field.OverlayType - -1];
            if (UnitHS.FUnit.FMovePoints == 0)
            {
                Cost = 0;
            }
            else
            {
                Wall = (CityWall)Field.FindOwnedChild(2229508);
                if (Wall == null)
                {
                    Wall = Field.FindOwnedChild(2229510) as CityWall;
                }

                if (Wall != null)
                {
                    if (!(Wall is CityWallDoor) || UnitHS.FUnit.GetSide() == CombatSideIndex.Attacker)
                    {
                        Cost = -1;
                    }

                    if (!(Wall is CityWallDoor) && UnitHS.FUnit.FUnit.GetAbilityEnabled(36))
                    {
                        Cost = 1;
                    }

                    if (UnitHS.FUnit.FUnit.GetAbilityEnabled(66) || UnitHS.FUnit.FUnit.GetAbilityEnabled(1))
                    {
                        Cost = 1;
                    }
                }
            }

            if (Cost <= 0 || AoWCombatMap.Current.FExplorationEnabled && !Field.GetExplored(AoWCombatMap.Current.FSeatedPlayer))
            {
                AoWCombatMap.Current.Mouse.Cursor = AoWCombatMap.Current.Mouse.Cursor + 1;
            }

            AoWCombatMap.Current.Mouse.CursorMode = 256;
        }
    }

    // AoWTC.pas:2744
    public override MoveControlSettings CreateMCS() => new CombatMoveControlSettings(null);

    // AoWTC.pas:2754
    public override void SetCurrentMove(int Value)
    {
        if (FCurrentMove != -1 && GetTCUnitHS() != null)
        {
            var tcUnitHS = GetTCUnitHS();
            AoWCombatMap.Current.Selection.TriggerDeselectEvent(tcUnitHS);
        }

        base.SetCurrentMove(Value);
        if (FCurrentMove != -1 && GetTCUnitHS() != null)
        {
            var tcUnitHS = GetTCUnitHS();
            AoWCombatMap.Current.Selection.TriggerSelectEvent(tcUnitHS);
        }
    }

    // AoWTC.pas:3316
    public override void Unselect()
    {
        DeactivateRangedPath();
        if (GetTCUnitHS() != null)
        {
            GetTCUnitHS().FSelectedAbility = -1;
        }

        AoWCombatMap.Current.Selection.UnselectLocation();
        DeactivateMoveSelection();
        base.Unselect();
    }

    // AoWTC.pas:3274
    public override void ShowMovePaths(in SurfaceDesc Surface)
    {
        int X = default;
        int Y = default;
        int Xhp = default;
        int Yhp = default;
        base.ShowMovePaths(Surface);
        if (GetTCUnitHS() != null)
        {
            if (GetTCUnitHS().FSelectedAbility != 65 && GetTCUnitHS().FSelectedAbility != 114)
            {
                return;
            }

            if (FMoveSettingsList.GetCount() == 1 && FMoveSettingsList.GetMoveSettings(0).FPath.Count > 1)
            {
                for (int I = 1; I <= 6; I++)
                {
                    var path = FMoveSettingsList.GetMoveSettings(0).FPath[1];
                    HexGeometry.CenterHNtoHX(FMoveSettingsList.GetMoveSettings(0).FPath[1].X, path.Y, I, ref X, ref Y);
                    HexGeometry.HXtoHP(X, Y, ref Xhp, ref Yhp);
                    FTargetImages[0].Show(Xhp - AoWCombatMap.Current.ViewXhp - 8, Yhp - AoWCombatMap.Current.ViewYhp + 2, Surface);
                }
            }
        }
    }

    // AoWTC.pas:2747
    public int AddUnit(TacticalCombatUnitHS U)
    {
        int Result = AddMoveObject(U);
        FMoveSettingsList.GetMoveSettings(Result).FMovePoints = U.FUnit.FMovePoints;
        var moves = U.FUnit.GetMoves();
        FMoveSettingsList.GetMoveSettings(Result).FMoves = moves;
        return Result;
    }

    // AoWTC.pas:2763
    public TacticalCombatUnitHS GetTCUnitHS()
    {
        TacticalCombatUnitHS Result;
        if (FCurrentMove != -1)
        {
            Result = (TacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(FCurrentMove).FMoveObject;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // AoWTC.pas:2770
    public Set<MoveType> GetMoveTypes()
    {
        Set<MoveType> Result;
        if (FCurrentMove != -1)
        {
            Result = ((TacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(FCurrentMove).FMoveObject).FUnit.FUnit.GetMoveTypes();
        }
        else
        {
            Result = Set<MoveType>.Of();
        }

        return Result;
    }

    // AoWTC.pas:2777
    public void RectangleSelectEvent(AbstractAoWHSMapMouse Sender)
    {
        int X1 = default;
        int Y1 = default;
        int X2 = default;
        int Y2 = default;
        int PX = default;
        int PY = default;
        TacticalCombatUnitHS UnitHS;
        MapField Field;
        MapLevel Level;
        DelphiList Units;
        Rect R;
        Units = new DelphiList();
        R = Sender.FSelectionRectangle;
        Level = AoWCombatMap.Current.MapContainer.MapLevels[AoWCombatMap.Current.ViewL];
        HexGeometry.HPtoHX(R.Left, R.Top, ref X1, ref Y1);
        HexGeometry.HPtoHX(R.Right, R.Bottom, ref X2, ref Y2);
        for (int X = X1 - 1, last = X2 + 1; X <= last; X++)
        {
            for (int Y = Y1 - 1, lastY = Y2 + 1; Y <= lastY; Y++)
            {
                HexGeometry.HXtoHP(X, Y, ref PX, ref PY);
                PX += 16;
                PY += 8;
                if (X >= 0 && X < Level.Width && Y > 0 && Y < Level.Height && PX >= R.Left && PX < R.Right && PY >= R.Top && PY < R.Bottom)
                {
                    Field = Level.Field[X, Y];
                    UnitHS = (TacticalCombatUnitHS)Field.FindOwnedChild(2228484);
                    if (UnitHS != null && UnitHS.FUnit.FCombatPlayer == AoWCombatMap.Current.FSeatedCombatPlayer)
                    {
                        Units.Add(UnitHS);
                    }
                }
            }
        }

        if (Units.Count != 0)
        {
            Clear();
            AoWCombatMap.Current.Selection.UnselectLocation();
            for (int I = 0, last = Units.Count - 1; I <= last; I++)
            {
                AddUnit((TacticalCombatUnitHS)Units[I]);
                var unit = Units[I];
                AoWCombatMap.Current.Selection.SelectLocationHS((HexagonSprite)unit);
            }

            if (Units.Count == 1)
            {
                SetCurrentMove(0);
            }
            else
            {
                SetCurrentMove(-1);
            }

            Update();
        }

        Units?.Dispose();
    }

    // AoWTC.pas:2807
    public void MoveSelectionEvent()
    {
        if (AoWCombatMap.Current.FSeatedCombatPlayer == AoWCombatMap.Current.FCurrentCombatPlayer)
        {
            if (AoWCombatMap.Current.Mouse.Capture != this)
            {
                AoWCombatMap.Current.Selection.UnselectLocation();
            }
            else
            {
                if (AoWCombatMap.Current.Mouse.Button == MouseButton.Right)
                {
                    if (FRangedPath.Count == 0)
                    {
                        AoWCombatMap.Current.Selection.UnselectLocation();
                    }
                    else
                    {
                        FRangedPath.Clear();
                        FMouseOverTarget = null;
                        AoWCombatMap.Current.FMouseOverTargetEventsStore.Raise(FMouseOverTarget);
                    }
                }
                else
                {
                    if (AoWCombatMap.Current.Mouse.Capture == this && AoWCombatMap.Current.Mouse.Button == MouseButton.Left)
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
        TacticalCombatUnitHS UnitHS;
        ushort Range;
        SpellCombatInfo Info = default;
        int Direction;
        int NX;
        int NY;
        FRangedPath.Clear();
        Direction = 0;
        int last = FObstacles.Count - 1;
        for (; Direction <= last; Direction += 1)
        {
            (FObstacles[Direction] as IDisposable)?.Dispose();
        }

        FObstacles.Clear();
        UnitHS = GetTCUnitHS();
        if (AoWCombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].TerrainType != 15)
        {
            if (UnitHS.FSelectedAbility == 52)
            {
                AoWHSSet.Current.FSpellControl.GetSpell(UnitHS.FSelectedSpell).GetCombatInfo(UnitHS.FUnit.FUnit, ref Info);
                if (Info.Range > 0)
                {
                    Range = (ushort)Info.Range;
                }
                else
                {
                    Range = 100;
                }
            }
            else
            {
                var tcUnitHS = GetTCUnitHS();
                if (AoWHSSet.Current.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatMode(tcUnitHS.FUnit.FUnit) == CombatMode.LongRange)
                {
                    Range = 12;
                }
                else
                {
                    var tcUnitHS2 = GetTCUnitHS();
                    if (AoWHSSet.Current.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatMode(tcUnitHS2.FUnit.FUnit) == CombatMode.MediumRange)
                    {
                        Range = 8;
                    }
                    else
                    {
                        Range = 4;
                    }
                }
            }

            bool condition = UnitHS.FUnit.GetMoves() == UnitHS.FUnit.FMovePoints && X > 0;
            if (condition)
            {
                var yhx = UnitHS.GetYhx();
                condition = HexGeometry.dHXtoRad(X, Y, UnitHS.GetXhx(), yhx) <= Range;
            }

            if (condition)
            {
                // { Original retains this unused with temporary. }
                GetTCUnitHS();
                FTargetX = X;
                FTargetY = Y;
                var yhx = UnitHS.GetYhx();
                Direction = HexGeometry.dHXtoHN(UnitHS.GetXhx(), yhx, X, Y);
                NX = X;
                NY = Y;
                switch (GetTCUnitHS().FSelectedAbility)
                {
                    case 30:
                    case >= 86 and <= 90:
                        var unitHSYhx = UnitHS.GetYhx();
                        HexGeometry.CenterHNtoHX(UnitHS.GetXhx(), unitHSYhx, TacticalTables.BreathHN[TacticalTables.BreathDirection[Direction - 1] * 12 - 12], ref NX, ref NY);
                        break;
                    case 52:
                        if (TacticalTables.SpellTypes[UnitHS.FSelectedSpell] == TfxType.Breath)
                        {
                            var yhx2 = UnitHS.GetYhx();
                            HexGeometry.CenterHNtoHX(UnitHS.GetXhx(), yhx2, TacticalTables.BreathHN[TacticalTables.BreathDirection[Direction - 1] * 12 - 12], ref NX, ref NY);
                        }

                        break;
                }

                var xhx = GetTCUnitHS().GetXhx();
                var yhx3 = GetTCUnitHS().GetYhx();
                var side = GetTCUnitHS().FUnit.GetSide();
                var tcUnitHS = GetTCUnitHS();
                TacticalTables.MakeAbilPath(side, (short)GetTCUnitHS().FSelectedAbility, (short)tcUnitHS.FSelectedSpell, xhx, yhx3, (short)NX, (short)NY, ref FRangedPath, ref FObstacles);
            }
        }

        FRangedPathHS.FSelection = this;
        AoWCombatMap.Current.DynamicFixedVisibleHS.AddChild(FRangedPathHS);
    }

    // AoWTC.pas:2918
    public void MoveSelectedRoute()
    {
        TacticalCombatUnitHS UnitHS;
        EObject Target;
        int MovePoints;
        if (FMoveSettingsList.GetCount() > 0)
        {
            if (GetTCUnitHS() != null)
            {
                if (FMoveSettingsList.GetMoveSettings(0).FPath.Count > 0)
                {
                    var moveSettings = FMoveSettingsList.GetMoveSettings(0);
                    UnitHS = GetTCUnitHS();
                    if (UnitHS.FSelectedAbility != 117)
                    {
                        var pathY = moveSettings.FPath[0];
                        var pathX = moveSettings.FPath[0];
                        Target = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y].FindOwnedChild(2228484);
                    }
                    else
                    {
                        var pathY = moveSettings.FPath[0];
                        var pathX = moveSettings.FPath[0];
                        Target = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y].FindOwnedChild(2229508);
                        if (Target == null)
                        {
                            var path = moveSettings.FPath[0];
                            var path2 = moveSettings.FPath[0];
                            Target = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path2.X, path.Y].FindOwnedChild(2229510);
                        }
                    }

                    if (FMoveSettingsList.GetMoveSettings(0).FPath.MovePointsToPos(UnitHS.FUnit.FMovePoints) == 0 && (Target != null || UnitHS.FSelectedAbility == 65 || UnitHS.FSelectedAbility == 114))
                    {
                        MovePoints = GetTCUnitHS().FUnit.FMovePoints;
                        if (MovePoints > 0)
                        {
                            if (UnitHS.FSelectedAbility == -1)
                            {
                                if (GetTCUnitHS().MeleeMoveTC(moveSettings.FPath))
                                {
                                    moveSettings.FPath.Clear();
                                }
                            }
                            else
                            {
                                if (AoWHSSet.Current.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatMode(UnitHS.FUnit.FUnit) == CombatMode.Touch)
                                {
                                    if (GetTCUnitHS().AbTouchMoveTC(moveSettings.FPath))
                                    {
                                        moveSettings.FPath.Clear();
                                        UpdateMoveCursor();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (TacticalCombatUnit.MoveCombatUnit(UnitHS.FUnit, moveSettings.FPath))
                        {
                            moveSettings.FPath.Clear();
                        }
                    }
                }
            }
        }
    }

    // AoWTC.pas:2943
    public void MoveUnits()
    {
        if (FMoveSettingsList.GetCount() > 0)
        {
            if (FMoveSettingsList.GetCount() > 1)
            {
                if (SetupMultiTCUnitMove(FMoveSettingsList))
                {
                    for (int I = 0, last = FMoveSettingsList.GetCount() - 1; I <= last; I++)
                    {
                        var moveSettings = FMoveSettingsList.GetMoveSettings(I);
                        moveSettings.FPath.Clear();
                    }
                }

                AoWCombatMap.Current.Selection.UnselectLocation();
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
        TacticalCombatUnitHS UnitHS;
        TfxType EffectType;
        int X = default;
        int Y = default;
        int Direction;
        bool AllPathsValid()
        {
            int I;
            bool condition = FMoveSettingsList.GetMoveSettings(0).FPath.Count > 0;
            if (condition)
            {
                var path = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                var xhx = AoWCombatMap.Current.Mouse.Xhx;
                condition = path.X == xhx;
            }

            bool test = condition;
            if (test)
            {
                var path = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                var yhx = AoWCombatMap.Current.Mouse.Yhx;
                test = path.Y == yhx;
            }

            bool Result = test;
            I = FMoveSettingsList.GetCount();
            while (Result && I != 0)
            {
                I -= 1;
                var moveSettings = FMoveSettingsList.GetMoveSettings(I);
                Result = moveSettings.FPath.Count != 0;
            }

            return Result;
        }

        DeactivateRangedPath();
        if (FMoveSettingsList.GetCount() > 1)
        {
            if (FCurrentMove == -1)
            {
                if (!AllPathsValid())
                {
                    bool notExplorationEnabled = !AoWCombatMap.Current.FExplorationEnabled;
                    if (!notExplorationEnabled)
                    {
                        var yhx = AoWCombatMap.Current.Mouse.Yhx;
                        var xhx = AoWCombatMap.Current.Mouse.Xhx;
                        notExplorationEnabled = ((AoWMapField)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL)).GetExplored(AoWCombatMap.Current.FSeatedPlayer);
                    }

                    if (notExplorationEnabled)
                    {
                        for (int I = 0, last = FMoveSettingsList.GetCount() - 1; I <= last; I++)
                        {
                            FMoveSettingsList.GetMoveSettings(I).FPath.Clear();
                        }

                        for (int A = 0, last = FMoveSettingsList.GetCount() - 1; A <= last; A++)
                        {
                            for (int B = A + 1, lastB = FMoveSettingsList.GetCount() - 1; B <= lastB; B++)
                            {
                                var yhx = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(B).FMoveObject).GetYhx();
                                var xhx = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(B).FMoveObject).GetXhx();
                                var mouseYhx = AoWCombatMap.Current.Mouse.Yhx;
                                var dHXtoRad2 = HexGeometry.dHXtoRad(AoWCombatMap.Current.Mouse.Xhx, mouseYhx, xhx, yhx);
                                var yhx2 = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(A).FMoveObject).GetYhx();
                                var xhx2 = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(A).FMoveObject).GetXhx();
                                var yhx3 = AoWCombatMap.Current.Mouse.Yhx;
                                if (dHXtoRad2 < HexGeometry.dHXtoRad(AoWCombatMap.Current.Mouse.Xhx, yhx3, xhx2, yhx2))
                                {
                                    FMoveSettingsList.FChildren.Exchange(A, B);
                                }
                            }
                        }

                        if (((TacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(0).FMoveObject).FUnit.GetLocked())
                        {
                            FMoveSettingsList.GetMoveSettings(0).FPath.Clear();
                        }
                        else
                        {
                            var yhx = AoWCombatMap.Current.Mouse.Yhx;
                            CalculateMovePath(0, (sbyte)AoWCombatMap.Current.Mouse.Xhx, (sbyte)yhx, (sbyte)AoWCombatMap.Current.ViewL);
                        }

                        if (FMoveSettingsList.GetMoveSettings(0).FPath.Count > 1)
                        {
                            for (int I = 1, last = FMoveSettingsList.GetCount() - 1; I <= last; I++)
                            {
                                if (((TacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(I).FMoveObject).FUnit.GetLocked())
                                {
                                    FMoveSettingsList.GetMoveSettings(I).FPath.Clear();
                                }
                                else
                                {
                                    var yhx = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(I).FMoveObject).GetYhx();
                                    var xhx = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(I).FMoveObject).GetXhx();
                                    var yhx2 = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(0).FMoveObject).GetYhx();
                                    Direction = HexGeometry.dHXtoHN(((HexagonSprite)FMoveSettingsList.GetMoveSettings(0).FMoveObject).GetXhx(), yhx2, xhx, yhx);
                                    var path = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                    HexGeometry.CenterHNtoHX(FMoveSettingsList.GetMoveSettings(0).FPath[0].X, path.Y, Direction, ref X, ref Y);
                                    CalculateMovePath(I, (sbyte)X, (sbyte)Y, 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int I = 0, last = FMoveSettingsList.GetCount() - 1; I <= last; I++)
                        {
                            FMoveSettingsList.GetMoveSettings(I).FPath.Clear();
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
                var moveSettings = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                bool condition = moveSettings.FPath.Count != 0;
                if (condition)
                {
                    var path = moveSettings.FPath[0];
                    var xhx = AoWCombatMap.Current.Mouse.Xhx;
                    condition = path.X == xhx;
                }

                bool test = condition;
                if (test)
                {
                    var path = moveSettings.FPath[0];
                    var yhx = AoWCombatMap.Current.Mouse.Yhx;
                    test = path.Y == yhx;
                }

                if (test && moveSettings.FPath[0].L == AoWCombatMap.Current.ViewL)
                {
                    MoveUnits();
                    return;
                }

                if (((TacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(FCurrentMove).FMoveObject).FUnit.GetLocked())
                {
                    FMoveSettingsList.GetMoveSettings(FCurrentMove).FPath.Clear();
                }
                else
                {
                    var yhx = AoWCombatMap.Current.Mouse.Yhx;
                    var xhx = AoWCombatMap.Current.Mouse.Xhx;
                    CalculateMovePath(FCurrentMove, (sbyte)xhx, (sbyte)yhx, (sbyte)AoWCombatMap.Current.ViewL);
                }
            }
        }
        else
        {
            if (GetTCUnitHS().FSelectedAbility == -1 && GetTCUnitHS().GetXhx() == AoWCombatMap.Current.Mouse.Xhx && GetTCUnitHS().GetYhx() == AoWCombatMap.Current.Mouse.Yhx)
            {
                var lhx = GetTCUnitHS().GetLhx();
                var yhx = GetTCUnitHS().GetYhx();
                var xhx = GetTCUnitHS().GetXhx();
                AoWCombatMap.Current.Selection.SelectLocationHX(xhx, yhx, lhx);
                return;
            }

            if (!GetTCUnitHS().FUnit.GetLocked() && AoWHSMap.Current.FTokenManager.PlayerReady(AoWCombatMap.Current.FCurrentPlayer))
            {
                UnitHS = GetTCUnitHS();
                if (AllPathsValid())
                {
                    MoveUnits();
                }
                else
                {
                    bool condition = FRangedPath.Count > 0;
                    if (condition)
                    {
                        var xhx = AoWCombatMap.Current.Mouse.Xhx;
                        condition = FTargetX == xhx;
                    }

                    bool test = condition;
                    if (test)
                    {
                        var yhx = AoWCombatMap.Current.Mouse.Yhx;
                        test = FTargetY == yhx;
                    }

                    if (test)
                    {
                        if (UnitHS.AbRangedTC((ushort)FTargetX, (ushort)FTargetY))
                        {
                            FRangedPath.Clear();
                            UpdateMoveCursor();
                        }
                    }
                    else
                    {
                        if (UnitHS.FSelectedAbility == -1 && UnitHS.GetXhx() == AoWCombatMap.Current.Mouse.Xhx && UnitHS.GetYhx() == AoWCombatMap.Current.Mouse.Yhx)
                        {
                            var lhx = UnitHS.GetLhx();
                            var yhx = UnitHS.GetYhx();
                            var xhx = UnitHS.GetXhx();
                            AoWCombatMap.Current.Selection.SelectLocationHX(xhx, yhx, lhx);
                            return;
                        }

                        if (!UnitHS.Moving())
                        {
                            if (UnitHS.FSelectedAbility == -1)
                            {
                                if (UnitHS.FUnit.FMovePoints > 0 && !UnitHS.FUnit.GetLocked())
                                {
                                    if (AoWCombatMap.Current.FExplorationEnabled)
                                    {
                                        var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                        var xhx = AoWCombatMap.Current.Mouse.Xhx;
                                        if (!((AoWMapField)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL)).GetExplored(AoWCombatMap.Current.FSeatedPlayer))
                                        {
                                            return;
                                        }
                                    }

                                    var mouseYhx = AoWCombatMap.Current.Mouse.Yhx;
                                    var mouseXhx = AoWCombatMap.Current.Mouse.Xhx;
                                    CalculateMovePath(FCurrentMove, (sbyte)mouseXhx, (sbyte)mouseYhx, 0);
                                }
                            }
                            else
                            {
                                if (UnitHS.FSelectedAbility == 52)
                                {
                                    EffectType = TacticalTables.SpellTypes[UnitHS.FSelectedSpell];
                                }
                                else
                                {
                                    EffectType = TacticalTables.AbilTypes[UnitHS.FSelectedAbility];
                                }

                                switch (EffectType)
                                {
                                    case TfxType.None:
                                        UnitHS.SelectAbility(-1);
                                        break;
                                    case TfxType.Touch:
                                        if (AoWCombatMap.Current.FExplorationEnabled)
                                        {
                                            var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                            var xhx = AoWCombatMap.Current.Mouse.Xhx;
                                            if (!((AoWMapField)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL)).GetExplored(AoWCombatMap.Current.FSeatedPlayer))
                                            {
                                                return;
                                            }
                                        }

                                        if (!UnitHS.FUnit.GetLocked())
                                        {
                                            var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                            CalculateMovePath(0, (sbyte)AoWCombatMap.Current.Mouse.Xhx, (sbyte)yhx, 0);
                                        }

                                        break;
                                    default:
                                        {
                                            var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                            CalculateRangedPath((sbyte)AoWCombatMap.Current.Mouse.Xhx, (sbyte)yhx);
                                            if (FRangedPath.Count > 0)
                                            {
                                                var mouseYhx = AoWCombatMap.Current.Mouse.Yhx;
                                                var xhx = AoWCombatMap.Current.Mouse.Xhx;
                                                FMouseOverTarget = (HexagonSprite)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[xhx, mouseYhx].FindOwnedChild(2228484);
                                                AoWCombatMap.Current.FMouseOverTargetEventsStore.Raise(FMouseOverTarget);
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
    public bool ValidMultiMoveSelection(AoWMapField Field)
    {
        sbyte Player;
        Player = Field.GetPlayer();
        return Player == -1;
    }

    // AoWTC.pas:3259
    public void ActivateMoveSelection()
    {
        if (!FStatus.Contains(MoveSelectorStatusElement.Activated))
        {
            Activate(AoWCombatMap.Current);
        }
    }

    // AoWTC.pas:3264
    public void DeactivateMoveSelection()
    {
        if (FStatus.Contains(MoveSelectorStatusElement.Activated))
        {
            Deactivate();
            FRangedPath.Clear();
            FMoveSettingsList.Clear();
            AoWCombatMap.Current.MouseMoveEvent(this);
        }
    }

    // AoWTC.pas:3293
    public void Update()
    {
        if (GetTCUnitHS() != null)
        {
            GetTCUnitHS().FUnit.FUnit.CreateMovePointTable(ref FMoveControlSettings.FMovePointTable);
        }

        FPathInvalid = Set<AnonymousEnum_piMove>.Of(AnonymousEnum_piMove.Ranged);
    }

    // AoWTC.pas:3300
    public void SetTCUnitHS(TacticalCombatUnitHS U)
    {
        if (GetTCUnitHS() != U)
        {
            if (FMoveSettingsList.GetCount() <= 1)
            {
                Clear();
                if (U != null && U.FUnit.GetPlayer() == AoWCombatMap.Current.FSeatedPlayer)
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

                FRangedPath.Clear();
                UpdateMoveCursor();
            }
        }
    }

    // AoWTC.pas:3325
    public void Select(TacticalCombatUnitHS U)
    {
        AoWCombatMap.Current.Selection.UnselectLocation();
        if (U != null)
        {
            AoWCombatMap.Current.Selection.SelectLocationHS(U);
        }
    }

    // AoWTC.pas:3331
    public void DeactivateRangedPath()
    {
        if (FRangedPathHS.FSelection != null)
        {
            AoWCombatMap.Current.DynamicFixedVisibleHS.RemoveChild(FRangedPathHS);
            FRangedPathHS.FSelection = null;
        }
    }

    private static bool SetupMultiTCUnitMove(MoveSettingsList Moves)
    {
        bool flag = false;
        MultiMoveTE tMultiMoveTE = null;
        CombatMoveTE tCombatMoveTE = null;
        Movepath tMovepath = null;
        bool flag2 = Moves.Count != 0;
        if (flag2)
        {
            sbyte player = ((TacticalCombatUnitHS)Moves.GetMoveSettings(0).FMoveObject).FUnit.GetPlayer();
            flag2 = AoWHSMap.Current.FTokenManager.PlayerReady(player);
        }
        if (flag2)
        {
            tMovepath = new Movepath();
            tMultiMoveTE = new MultiMoveTE(null);
            tMultiMoveTE.FSimultaneous = false;
            Moves.CalculateMovePositions(ValidateTCMovePosition);
            int i = 1;
            for (int num = Moves.Count - 1; i <= num; i++)
            {
                for (int j = 0; j <= i - 1; j++)
                {
                    MoveSettings moveSettings = Moves.GetMoveSettings(j);
                    MoveSettings moveSettings2 = Moves.GetMoveSettings(i);
                    if (moveSettings.FPath.Count > moveSettings2.FPath.Count)
                    {
                        ((DelphiList)Moves.FChildren).Exchange(i, j);
                    }
                }
            }
            int k = 0;
            for (int num2 = Moves.Count - 1; k <= num2; k++)
            {
                MoveSettings moveSettings3 = Moves.GetMoveSettings(k);
                if (moveSettings3.FMovePos < moveSettings3.FPath.Count - 1)
                {
                    tMovepath.Clear();
                    int l = moveSettings3.FMovePos;
                    for (int num3 = moveSettings3.FPath.Count - 1; l <= num3; l++)
                    {
                        tMovepath.Add(moveSettings3.FPath[l]);
                    }
                    tCombatMoveTE = new CombatMoveTE(null);
                    if (tCombatMoveTE.Setup(((TacticalCombatUnitHS)moveSettings3.FMoveObject).FUnit, tMovepath))
                    {
                        tMultiMoveTE.FEvents.Add(tCombatMoveTE);
                    }
                    tCombatMoveTE.Release();
                }
            }
            if (tMultiMoveTE.FEvents.Count != 0)
            {
                sbyte player2 = ((TacticalCombatUnitHS)Moves.GetMoveSettings(0).FMoveObject).FUnit.GetPlayer();
                AoWHSMap.Current.FTokenManager.GetTokenControl(player2).AddEvent(tMultiMoveTE);
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
            int m = 0;
            for (int num4 = AoWCombatMap.Current.FUnitSelection.FMoveSettingsList.Count - 1; m <= num4; m++)
            {
                MoveSettings moveSettings4 = AoWCombatMap.Current.FUnitSelection.FMoveSettingsList.GetMoveSettings(m);
                moveSettings4.FPath.Clear();
            }
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    private static bool ValidateTCMovePosition(MoveSettingsList Moves, int Index)
    {
        bool flag = true;
        if (flag)
        {
            for (int num = Moves.Count - 1; num >= 0; num--)
            {
                if (num != Index)
                {
                    MoveSettings moveSettings = Moves.GetMoveSettings(num);
                    bool flag2 = moveSettings.FValid && Moves.GetMoveSettings(Index).GetMovePrio() >= moveSettings.GetMovePrio();
                    if (flag2)
                    {
                        XYL moveLoc = Moves.GetMoveSettings(Index).GetMoveLoc();
                        flag2 = moveSettings.GetMoveLoc().Packed == moveLoc.Packed;
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

    public enum AnonymousEnum_piMove : byte
    {
        Move = 0, Ranged = 1
    }

}
