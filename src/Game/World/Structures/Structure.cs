// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Draw.Images;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// ID, update count, building player, days left, shadow, fogged
public partial class Structure : FixedImageLibraryTerrainMapObject
{

    public Structure(ECustomNode owner) : base(owner)
    {
        FBuildingPlayer = -1;
        FFogged = true;
    }

    public new const int ClassID = 131914;

    public int FStructureID;
    public int FUpdateCount;
    public sbyte FBuildingPlayer;
    public byte FBuildingDays;
    public StructureShadow FShadow;
    public bool FFogged;

    // Structur.PAS:1017
    public override void Dispose()
    {
        if (FShadow != null)
        {
            DestroyShadow();
        }

        if (Owner != null && FStatus.Contains(MapObjectStatusElement.ResourceLinked) && ((StructureResource)FResource).FTerrainBound)
        {
            Owner.DestroyChildren(131334);
        }

        base.Dispose();
        if (AoWHSMap.Current != null && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            AoWHSMap.Current.StructureDestroyed(this);
        }
    }

    // Structur.PAS:1038
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FBuildingDays, 0);
        Stream.rwInteger(ref FStructureID, 98);
        Stream.rwByteEx(ref Unsafe.As<sbyte, byte>(ref FBuildingPlayer), 255, 99);
    }

    // Structur.PAS:1010

    // Structur.PAS:1629
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001985)
        {
            NewDay();
        }
        else
        {
            if (Msg.MsgID == 537001986)
            {
                NewTurn((sbyte)Msg.Payload.Int);
            }
            else
            {
                if (Msg.MsgID == 537001990)
                {
                    SeatedPlayerChanged();
                }
                else
                {
                    if (Msg.MsgID == 537002243)
                    {
                        if (!MainValidateMap(Msg.Payload.As<StringCollection>()))
                        {
                            Msg.Data[0] = 1;
                        }
                    }
                }
            }
        }
    }

    // Structur.PAS:1281
    public override sbyte GetTerrain(int Index)
    {
        sbyte Result = FResource.TerrainList.Terrain[Index];
        if (Result >= 0)
        {
            Result = FTerrainType;
        }

        return Result;
    }

    // Structur.PAS:1275
    public override sbyte GetOverlay(int Index)
    {
        sbyte Result = FResource.TerrainList.Terrain[Index];
        if (Result >= 0)
        {
            Result = 7;
        }

        return Result;
    }

    // Structur.PAS:1287
    public override bool CanChangeTerrain(in HexSpriteSenderInfo Info) => ((StructureResource)FResource).FTerrainBound || base.CanChangeTerrain(Info);

    // Structur.PAS:1297
    public override void TerrainChanged(in HexSpriteTerrainInfo Info)
    {
        if (FResource.TerrainList.Terrain[Info.HN] >= 0)
        {
            if (((StructureResource)FResource).FTerrainBound)
            {
                FTerrainType = Info.TerrainType;
                FImageIndex = GetTerrainTypeImage(FTerrainType);
            }
            else
            {
                base.TerrainChanged(Info);
            }
        }
    }

    // Structur.PAS:1647
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
        ((StructureResource)FResource).Show(Xhp - XhpSurf, Yhp - YhpSurf, SurfaceDesc, this);
    }

    // Structur.PAS:1092
    public override bool PlaceOnMap()
    {
        bool Result = base.PlaceOnMap();
        if (!GetEditMode())
        {
            UpdateRoads((MapField)Owner);
        }

        return Result;
    }

    // Structur.PAS:1098
    public override void RemoveFromMap()
    {
        MapField MapField;
        MapField = (MapField)Owner;
        base.RemoveFromMap();
        if (MapField != null && !GetEditMode())
        {
            UpdateRoads(MapField);
        }
    }

    // Structur.PAS:1599
    public override void MapFieldMsgProc(ref MapFieldMessage Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
        {
            if (Msg.MsgID == 4358)
            {
                FogChanged();
            }
            else
            {
                if (Msg.MsgID == 4608)
                {
                    UpdateAITarget(Msg.Param[0].As<AITargetQuery>().Info, ref Msg.Param[0].As<AITargetQuery>().Data);
                    if (Msg.Param[0].As<AITargetQuery>().Data.Priority < 0)
                    {
                        Msg.Result = 1;
                    }
                }
                else
                {
                    if (Msg.MsgID == 4388)
                    {
                        ArmyChanged(Msg.Param[0].As<ArmyHexSprite>());
                    }
                    else
                    {
                        if (Msg.MsgID == 4354)
                        {
                            ArmyPlaced(Msg.Param[0].As<ArmyHexSprite>());
                        }
                        else
                        {
                            if (Msg.MsgID == 4355)
                            {
                                ArmyRemoved(Msg.Param[0].As<ArmyHexSprite>());
                            }
                            else
                            {
                                if (Msg.MsgID == 4401)
                                {
                                    if (((StructureResource)FResource).FConnectRoads)
                                    {
                                        Msg.Param[1] = 1;
                                    }
                                }
                                else
                                {
                                    if (Msg.MsgID == 4386)
                                    {
                                        Msg.Result = ((StructureResource)FResource).FScannerIcon;
                                    }
                                    else
                                    {
                                        if (Msg.MsgID == 4390)
                                        {
                                            var defensePriority = GetDefensePriority();
                                            Msg.Result = (int)defensePriority;
                                        }
                                        else
                                        {
                                            if (Msg.MsgID == 4391)
                                            {
                                                if (GetDefenseRequirements((sbyte)Msg.Param[0].Int, ref Msg.Param[1].As<DefenseRequirementsQuery>().Requirements))
                                                {
                                                    Msg.Result = 1;
                                                }
                                            }
                                            else
                                            {
                                                if (Msg.MsgID == 4364 && Msg.HN == 0)
                                                {
                                                    if (FResource.TerrainList.Terrain[Msg.HN] >= 0 && (!AoWHSMap.Current.FFogEnabled || !FFogged))
                                                    {
                                                        Msg.Param[0] = Msg.Param[0].Int + 3584;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Structur.PAS:1585
    public override void Activate()
    {
        base.Activate();
        AoWHSMap.Current.FStructureControl.Register(this);
        if (AoWHSMap.Current.FShowUnitShadows && ((StructureResource)FResource).FCastsShadow && !GetEditMode())
        {
            CreateShadow();
        }
    }

    // Structur.PAS:1593
    public override void Deactivate()
    {
        AoWHSMap.Current.FStructureControl.Unregister(this);
        base.Deactivate();
    }

    // Structur.PAS:1886
    public override void UpdateMapField(in HexSpriteMapFieldInfo Info)
    {
        if (FResource.TerrainList.Terrain[Info.HN] >= 0)
        {
            ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 134217728;
        }
    }

    // Structur.PAS:1184
    public override bool CanSelect(int Xhp, int Yhp)
    {
        bool Result;
        int X = default;
        int Y = default;
        int HN;
        if (!AoWHSMap.Current.FFogEnabled || !FFogged)
        {
            HexGeometry.HPtoHX(Xhp, Yhp, ref X, ref Y);
            HN = HexGeometry.dHXtoHN(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, X, Y);
            if (HN >= FResource.TerrainList.Count || FResource.TerrainList.Terrain[HN] < 0 && FResource.TerrainList.Overlay[HN] < 0)
            {
                Result = false;
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:1206
    public override void Select()
    {
        base.Select();
        if (!AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            PlaySelectSample();
        }
    }

    // Structur.PAS:1212
    public override void Unselect()
    {
        base.Unselect();
    }

    // Structur.PAS:1308
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        bool Result;
        if (((StructureResource)FResource).FTerrainBound)
        {
            Result = AoWHSSet.Current.FPadControl.CanPlacePad(Xhx, Yhx, Lhx, ((StructureResource)FResource).FFlagType);
        }
        else
        {
            Result = base.CanPlace(MapContainer, Xhx, Yhx, Lhx);
        }

        return Result;
    }

    // Structur.PAS:1315
    public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        bool Result;
        ImageLibraryTerrainMapObject Pad;
        if (GetEditMode() || CanPlace(MapContainer, Xhx, Yhx, Lhx))
        {
            if (((StructureResource)FResource).FTerrainBound)
            {
                if (!GetEditMode())
                {
                    Pad = AoWHSSet.Current.FPadControl.PlacePad(Xhx, Yhx, Lhx, ((StructureResource)FResource).FFlagType);
                }
                else
                {
                    Pad = null;
                }

                if (Pad != null || GetEditMode())
                {
                    if (Pad != null)
                    {
                        FTerrainType = Pad.TerrainType;
                    }

                    Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
                }
                else
                {
                    Result = false;
                }
            }
            else
            {
                Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:1292
    public override bool ValidTerrainType(int TerrainType) => ((StructureResource)FResource).FTerrainBound && AoWHSSet.Current.FPadControl.FTerrainTypes[TerrainType] || base.ValidTerrainType(TerrainType);

    // Structur.PAS:1053
    public override int GetTerrainTypeImage(int TerrainType)
    {
        int Result;
        if (GetRazed())
        {
            if (TerrainType == 3)
            {
                Result = 23;
            }
            else
            {
                if (TerrainType == 5)
                {
                    Result = 24;
                }
                else
                {
                    Result = 22;
                }
            }
        }
        else
        {
            if (GetBuilding())
            {
                if (TerrainType == 3)
                {
                    Result = 26;
                }
                else
                {
                    if (TerrainType == 5)
                    {
                        Result = 27;
                    }
                    else
                    {
                        Result = 25;
                    }
                }
            }
            else
            {
                Result = base.GetTerrainTypeImage(TerrainType);
            }
        }

        return Result;
    }

    // Structur.PAS:1159
    public override sbyte ForceTerrainType(MapLevel MapLevel, sbyte Xhx, sbyte Yhx)
    {
        sbyte Result;
        if (MapLevel.Lhx != 0)
        {
            Result = 12;
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Structur.PAS:1239
    public virtual LibraryImage GetTopBorderImage()
    {
        LibraryImage Result;
        ImageSequenceList Sequences;
        Sequences = FResource.ImageSequenceList;
        if (Sequences != null)
        {
            if (Sequences.GetImageSequence(30) != null)
            {
                Result = FResource.ImageSequenceList.GetImageSequence(30).GetPreviewImage();
            }
            else
            {
                Result = null;
            }
        }
        else
        {
            Result = FResource.ImageLibrary.GetSafe(30);
        }

        return Result;
    }

    // Structur.PAS:1251
    public virtual LibraryImage GetMiddleBorderImage()
    {
        LibraryImage Result;
        ImageSequenceList Sequences;
        Sequences = FResource.ImageSequenceList;
        if (Sequences != null)
        {
            if (Sequences.GetImageSequence(31) != null)
            {
                Result = FResource.ImageSequenceList.GetImageSequence(31).GetPreviewImage();
            }
            else
            {
                Result = null;
            }
        }
        else
        {
            Result = FResource.ImageLibrary.GetSafe(31);
        }

        return Result;
    }

    // Structur.PAS:1263
    public virtual LibraryImage GetBottomBorderImage()
    {
        LibraryImage Result;
        ImageSequenceList Sequences;
        Sequences = FResource.ImageSequenceList;
        if (Sequences != null)
        {
            if (Sequences.GetImageSequence(32) != null)
            {
                Result = FResource.ImageSequenceList.GetImageSequence(32).GetPreviewImage();
            }
            else
            {
                Result = null;
            }
        }
        else
        {
            Result = FResource.ImageLibrary.GetSafe(32);
        }

        return Result;
    }

    // Structur.PAS:1174
    public virtual StringCollection GetDescription() => ((StructureResource)FResource).FDescription;

    // Structur.PAS:1164
    public virtual string GetName() => EditName();

    // Structur.PAS:1346
    public virtual void SetRazed(bool Value)
    {
    }

    // Structur.PAS:1341
    public virtual bool GetRazed() => false;

    // Structur.PAS:1336
    public virtual bool GetRazeable() => ((StructureResource)FResource).FRazeable;

    // Structur.PAS:1844
    public virtual bool RazeEx(sbyte Player)
    {
        bool Result;
        string Reason = "";
        StructureTokenEvent Event;
        if (CanRaze(Player, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Player).GetBusy())
        {
            Event = CreateTE();
            SetupTE(Event);
            Event.FAction = -1;
            Event.FParam = Player;
            AoWHSMap.Current.FTokenManager.GetTokenControl(Player).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:1755
    public virtual void SetupRazeDefenderAG(UnitList Attackers, UnitList Defenders)
    {
        HuntAIGroup Group;
        var unitsToIndependentRelation = UnitList.UnitsToIndependentRelation(Defenders, 5);
        Group = (HuntAIGroup)AoWHSMap.Current.FAIGroupManager.CreateGroup(0, unitsToIndependentRelation, GroupBehavior.Hunt);
        Group.AddUnits(Defenders);
        for (int I = Attackers.Count - 1; I >= 0; I--)
        {
            if (Attackers.GetUnit(I).FPlayer != 0)
            {
                var unit = Attackers.GetUnit(I);
                Group.FTargetUnitIDs.Add(unit.FUnitID);
            }
        }

        Group.Release();
    }

    // Structur.PAS:1130
    public virtual StructureTokenEvent CreateTE() => new StructureTokenEvent(null);

    // Structur.PAS:1135
    public virtual void SetupTE(StructureTokenEvent TE)
    {
        var lhx = GetLhx();
        var yhx = GetYhx();
        var xhx = GetXhx();
        TE.Initialize(EClass.ClassID, xhx, yhx, lhx);
    }

    // Structur.PAS:1140
    public virtual ExecuteResult ExecuteTE(StructureTokenEvent TE, TokenExecutor Executer)
    {
        ExecuteResult Result;
        Troop AUnit;
        if (TE.FAction == -1)
        {
            if (ExecuteRaze((sbyte)TE.FParam))
            {
                Result = ExecuteResult.Done;
            }
            else
            {
                Result = ExecuteResult.DoneHalt;
            }
        }
        else
        {
            if (TE.FAction == -2)
            {
                AUnit = AoWHSMap.Current.FUnitControl.FindUnit(TE.FParam);
                if (AUnit != null && ExecuteRebuild(AUnit))
                {
                    AUnit.Killed();
                    Result = ExecuteResult.Done;
                }
                else
                {
                    Result = ExecuteResult.DoneHalt;
                }
            }
            else
            {
                Result = ExecuteResult.DoneHalt;
            }
        }

        return Result;
    }

    // Structur.PAS:1505
    public virtual bool GetDefenseRequirements(sbyte Player, ref DefenseRequirements Requirements) => false;

    // Structur.PAS:1502
    public virtual DefensePriority GetDefensePriority() => DefensePriority.None;

    // Structur.PAS:1476
    public virtual void NewDay()
    {
    }

    // Structur.PAS:1479
    public virtual void NewTurn(sbyte Player)
    {
        if (Player == FBuildingPlayer)
        {
            if (FBuildingDays != 0)
            {
                FBuildingDays = (byte)(FBuildingDays - 1);
            }

            if (FBuildingDays == 0)
            {
                BuildingDone();
            }
        }
    }

    // Structur.PAS:1497
    public virtual void SeatedPlayerChanged()
    {
        FogChanged();
    }

    // Structur.PAS:1488
    public virtual void ArmyPlaced(ArmyHexSprite PlacedArmy)
    {
    }

    // Structur.PAS:1491
    public virtual void ArmyRemoved(ArmyHexSprite Army)
    {
    }

    // Structur.PAS:1494
    public virtual void ArmyChanged(ArmyHexSprite Army)
    {
    }

    // Structur.PAS:1200
    public virtual void PlaySelectSample()
    {
        if (!GetRazed() && !IsEmpty(((StructureResource)FResource).FSelectSFX))
        {
            AudioPlayback.PlaySample(((StructureResource)FResource).FSelectSFX);
        }
    }

    // Structur.PAS:1107
    public virtual void CreateShadow()
    {
        if (FShadow != null)
        {
            DestroyShadow();
        }

        FShadow = new StructureShadow(null);
        FShadow.FStructure = this;
        FShadow.Connect();
    }

    // Structur.PAS:1115
    public virtual void DestroyShadow()
    {
        if (FShadow != null)
        {
            if (FShadow.FShadowStatus.Contains(StructureShadowStatusElement.Connected))
            {
                FShadow.Disconnect();
            }

            FShadow?.Dispose();
            FShadow = null;
        }
    }

    // Structur.PAS:1639
    public virtual void ShowShadow(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
        FResource.ImageSequenceList.Show(90, Xhp - XhpSurf, Yhp - YhpSurf, 0, SurfaceDesc);
    }

    // Structur.PAS:1414
    public virtual void BuildingDone()
    {
        LocationTextEventLog Log;
        Log = new LocationTextEventLog(null);
        Log.FPlayer = FBuildingPlayer;
        if (GetRazed())
        {
            var editName = EditName();
            Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(StructureRebuiltRStr), [editName]);
        }
        else
        {
            var editName = EditName();
            Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(StructureBuiltRStr), [editName]);
        }

        var lhx = GetLhx();
        var yhx = GetYhx();
        Log.SetLocation(GetXhx(), yhx, lhx);
        var yhx2 = GetYhx();
        var lhx2 = GetLhx();
        var xhx = GetXhx();
        EventLog.DistributeLocationEventLog(Log, FBuildingPlayer, xhx, yhx2, lhx2, false);
        Log.Release();
        SetRazed(false);
        FBuildingPlayer = -1;
        FImageIndex = GetTerrainTypeImage(FTerrainType);
    }

    // Structur.PAS:1383
    public virtual bool ExecuteRebuild(Troop AUnit)
    {
        bool Result;
        int Time = default;
        int Cost = default;
        string Reason = "";
        if (CanRebuild(AUnit, ref Reason))
        {
            GetRebuildInfo(ref Cost, ref Time);
            var players = AoWHSMap.Current.FPlayers.GetPlayers(AUnit.FPlayer);
            AoWHSMap.Current.FPlayers.GetPlayers(AUnit.FPlayer).SetGems(players.FGems - Cost);
            FBuildingDays = (byte)Time;
            FBuildingPlayer = AUnit.FPlayer;
            ValidateObservation(AUnit.FPlayer);
            FImageIndex = GetTerrainTypeImage(FTerrainType);
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:1400
    public virtual bool ExecuteBuild(Troop AUnit, int Days)
    {
        FBuildingDays = (byte)Days;
        FBuildingPlayer = AUnit.FPlayer;
        FImageIndex = GetTerrainTypeImage(FTerrainType);
        ValidateObservation(AUnit.FPlayer);
        return true;
    }

    // Structur.PAS:1734
    public virtual bool GenerateRazeDefenders(sbyte Player, UnitList Units)
    {
        int Collection;
        UnitIndexList Indexes;
        var xhx = GetXhx();
        var yhx = GetYhx();
        RandSeed = xhx + AoWHSMap.Current.FInitialRandomSeed + yhx;
        Collection = ((StructureResource)FResource).FDefenders.GetRndCollection();
        if (Collection != -1)
        {
            Indexes = new UnitIndexList();
            var unitIndexListCollection = ((StructureResource)FResource).FDefenders;
            for (int I = unitIndexListCollection.GetCollectionCount(Collection) - 1; I >= 0; I--)
            {
                Indexes.Add(unitIndexListCollection.GetU(Collection, I));
            }

            Int32List.FillWithRandomUnits(Indexes, Units, 4, ((StructureResource)FResource).FRazeDefenderBudget, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature, UnitType.Machine));
            while (Units.Count > 8)
            {
                Units.DeleteUnit(Units.GetUnit(Units.Count - 1));
            }

            Indexes?.Dispose();
        }

        return true;
    }

    // Structur.PAS:1767
    public virtual bool PlaceRazeDefenders(sbyte Player)
    {
        bool Result;
        UnitList Defenders;
        UnitList Placed;
        UnitList Attackers;
        Troop AUnit;
        Defenders = new UnitList(null);
        Placed = new UnitUserList(null);
        Attackers = new UnitUserList(null);
        GenerateRazeDefenders(Player, Defenders);
        if (Defenders.Count != 0)
        {
            for (int I = Defenders.Count - 1; I >= 0; I--)
            {
                AUnit = Defenders.GetUnit(I);
                Placed.AddUnit(AUnit);
                var lhx = GetLhx();
                var yhx = GetYhx();
                AUnit.Place(GetXhx(), yhx, lhx, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
            }

            ListUnits(Attackers);
            SetupRazeDefenderAG(Attackers, Placed);
            Result = true;
        }
        else
        {
            Result = false;
        }

        Attackers?.Dispose();
        Placed?.Dispose();
        Defenders?.Dispose();
        return Result;
    }

    // Structur.PAS:1801
    public virtual bool ExecuteRaze(sbyte Player)
    {
        bool Result = default;
        string Reason = "";
        LocationTextEventLog Log = default;
        ImageSequenceAnimation Animation = default;
        if (CanRaze(Player, ref Reason))
        {
            BeginUpdate();
            Log = new LocationTextEventLog(null);
            Log.FPlayer = Player;
            var editName = EditName();
            Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(StructureRazedRStr), [editName]);
            var lhx = GetLhx();
            var yhx = GetYhx();
            Log.SetLocation(GetXhx(), yhx, lhx);
            var yhx2 = GetYhx();
            var lhx2 = GetLhx();
            EventLog.DistributeLocationEventLog(Log, Player, GetXhx(), yhx2, lhx2, false);
            Log.Release();
            if (FResource.ImageSequenceList.GetImageSequence(20) != null)
            {
                Animation = new ImageSequenceAnimation(null);
                Animation.SetImageSequence(FResource.ImageSequenceList.GetImageSequence(20));
                Animation.SetXhpOffset(Animation.FXhpOffset - 16);
                Animation.SetYhpOffset(Animation.FYhpOffset - 16);
                Animation.FOnShow = ShowRazeAnimation;
                var lhx3 = GetLhx();
                var yhx3 = GetYhx();
                var xhx = GetXhx();
                AoWHSMap.Current.PlaceHX(Animation, xhx, yhx3, lhx3);
            }

            if (FResource.ImageSequenceList.GetImageSequence(21) != null)
            {
                Animation = new ImageSequenceAnimation(null);
                Animation.SetImageSequence(FResource.ImageSequenceList.GetImageSequence(21));
                Animation.FFrameCount = 300;
                Animation.SetXhpOffset(Animation.FXhpOffset - 16);
                Animation.SetYhpOffset(Animation.FYhpOffset - 16);
                Animation.FLooped = true;
                var lhx3 = GetLhx();
                var yhx3 = GetYhx();
                var xhx = GetXhx();
                AoWHSMap.Current.PlaceHX(Animation, xhx, yhx3, lhx3);
            }

            ValidateObservation(Player);
            SetRazed(true);
            PlaceRazeDefenders(Player);
            EndUpdate();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:1169
    public virtual string GetCurrentActivityText() => "-";

    // Structur.PAS:1564
    public virtual bool ValidateMap(StringCollection Messages) => true;

    // Structur.PAS:1569
    public virtual bool MainValidateMap(StringCollection Messages)
    {
        StringList Errors;
        Errors = new StringList();
        bool Result = ValidateMap(Errors);
        if (Errors.Count != 0)
        {
            var editName = EditName();
            var xhx = GetXhx();
            var yhx = GetYhx();
            var lhx = GetLhx();
            Messages.Add(AbilityRules.Format(AbilityRules.TranslateRStr(XatLocationYRStr), [editName, "(" + IntToStr(xhx) + ", " + IntToStr(yhx) + ", " + IntToStr(lhx) + ")"]));
            for (int I = 0, last = Errors.Count - 1; I <= last; I++)
            {
                Messages.Add("  " + Errors[I]);
            }
        }

        Errors?.Dispose();
        return Result;
    }

    // Structur.PAS:1046
    public virtual bool ExecuteAI(sbyte Player, int Phase) => false;

    // Structur.PAS:1049
    public virtual void UpdateAITarget(in AITargetInfo Info, ref AITargetData Data)
    {
    }

    // Structur.PAS:1125
    public virtual void Update()
    {
        if (FUpdateCount == 0)
        {
            AoWHSMap.Current.StructureChanged(this);
        }
    }

    // Structur.PAS:1349
    public virtual bool VisibleForPlayer(sbyte Player)
    {
        HexNumberWalkData Walk = default;
        bool Result = false;
        if (Player >= 0)
        {
            HexNumberWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (true)
            {
                var terrainCount = GetTerrainCount();
                if (!(Walk.HN < terrainCount && !Result))
                    break;
                if (FResource.TerrainList.Used[Walk.HN])
                {
                    var lhx = GetLhx();
                    Result = AoWHSMap.Current.VisibleForPlayer(Walk.X, Walk.Y, lhx, Player);
                }

                HexNumberWalkData.WalkToNextHN(ref Walk);
            }
        }

        return Result;
    }

    // Structur.PAS:1179
    public virtual bool CanDblClick() => false;

    // Structur.PAS:1508
    public virtual int ListUnits(UnitList List)
    {
        HexNumberWalkData Walk = default;
        int Count;
        MapLevel Level;
        ArmyHexSprite Army;
        int Result = 0;
        Count = GetTerrainCount();
        Level = (MapLevel)Owner.Owner;
        HexNumberWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
        while (Walk.HN < Count)
        {
            if (FResource.TerrainList.Terrain[Walk.HN] >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Army = ((AoWMapField)Level.GetField(Walk.X, Walk.Y)).GetArmyHS();
                if (Army != null)
                {
                    List.AddUnits(Army.FArmy);
                    Result += Army.FArmy.Count;
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        return Result;
    }

    // Structur.PAS:1535
    public virtual int ListArmies(ArmyHSList List)
    {
        HexNumberWalkData Walk = default;
        int Count;
        MapLevel Level;
        EngineObject Army;
        List.BeginUpdate();
        int Result = 0;
        Count = GetTerrainCount();
        Level = (MapLevel)Owner.Owner;
        HexNumberWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
        while (Walk.HN < Count)
        {
            if (FResource.TerrainList.Terrain[Walk.HN] >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Army = Level.GetField(Walk.X, Walk.Y).FindOwnedChild(131607);
                if (Army != null)
                {
                    List.AddChild(Army);
                    Result += 1;
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        List.EndUpdate();
        return Result;
    }

    // Structur.PAS:1430
    public virtual void GetRebuildInfo(ref int Cost, ref int Time)
    {
        Cost = ((StructureResource)FResource).FRebuildGold;
        Time = ((StructureResource)FResource).FRebuildDays;
    }

    // Structur.PAS:1439
    public virtual bool CanRebuild(Troop AUnit, ref string Reason)
    {
        bool Result;
        int Cost = default;
        int Time = default;
        if (GetRazed())
        {
            if (!GetBuilding())
            {
                GetRebuildInfo(ref Cost, ref Time);
                if (AoWHSMap.Current.FPlayers.GetPlayers(AUnit.FPlayer).FGems >= Cost)
                {
                    Result = true;
                }
                else
                {
                    Reason = AbilityRules.TranslateRStr(GameText.NotEnoughGoldRStr);
                    Result = false;
                }
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:1458
    public virtual bool Rebuild(Troop AUnit)
    {
        bool Result;
        string Reason = "";
        StructureTokenEvent Event;
        if (CanRebuild(AUnit, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(AUnit.FPlayer).GetBusy())
        {
            Event = CreateTE();
            SetupTE(Event);
            Event.FAction = -2;
            Event.FParam = AUnit.FUnitID;
            AoWHSMap.Current.FTokenManager.GetTokenControl(AUnit.FPlayer).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:1862
    public virtual bool Raze(sbyte Player)
    {
        bool Result = default;
        string Reason = "";
        RazeStructureEventLog Log;
        if (CanRaze(Player, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Player).GetBusy())
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType == PlayerType.Human)
            {
                Log = new RazeStructureEventLog(null);
                Log.FStructure = this;
                Log.FButtons = MessageEventLog.Buttons.YesNo;
                Log.FTitle = AbilityRules.TranslateRStr(GameText.WarningRStr);
                var razeStructureWarning = AbilityRules.TranslateRStr(RazeStructureWarningRStr);
                Log.FMessage.Add(razeStructureWarning);
                var aoWETranslateRStr = AbilityRules.TranslateRStr(DoYouWishToContinueRStr);
                Log.FMessage.Add(aoWETranslateRStr);
                Log.Execute(true, ExecuteTrigger.User, null);
                Log.Release();
                Result = true;
            }
            else
            {
                Result = RazeEx(Player);
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:1708
    public virtual bool CanRaze(sbyte Player, ref string Reason)
    {
        bool Result;
        CombatPredictor Predictor;
        UnitList Defenders;
        UnitList Units;
        if (!GetRazed() && GetRazeable())
        {
            Predictor = new CombatPredictor();
            Defenders = new UnitList(null);
            Units = new UnitUserList(null);
            ListUnits(Units);
            for (int I = 0, last = Units.Count - 1; I <= last; I++)
            {
                if (Units.GetUnit(I).FPlayer == Player)
                {
                    Predictor.AddUnit(Units.GetUnit(I));
                }
            }

            Units?.Dispose();
            GenerateRazeDefenders(Player, Defenders);
            for (int I = 0, last = Defenders.Count - 1; I <= last; I++)
            {
                Predictor.AddOpponent(Defenders.GetUnit(I), 0);
            }

            Predictor.Execute();
            Result = Set<byte>.WithBounds(0, 255).With(3).With(4).With(6).Contains(Predictor.FResult) && Predictor.GetSuperiority() > 50;
            if (!Result)
            {
                Reason = AbilityRules.TranslateRStr(NotEnoughForcesRStr);
            }

            Defenders?.Dispose();
            Predictor?.Dispose();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:1027
    public void BeginUpdate()
    {
        FUpdateCount += 1;
    }

    // Structur.PAS:1032
    public void EndUpdate()
    {
        FUpdateCount -= 1;
        if (FUpdateCount == 0)
        {
            AoWHSMap.Current.StructureChanged(this);
        }
    }

    // Structur.PAS:1066
    public void UpdateRoads(MapField MapField)
    {
        MapLevel Level;
        UInt16List Table;
        int X = default;
        int Y = default;
        AbstractRoad Road;
        Level = (MapLevel)MapField.Owner;
        Table = FResource.TerrainList.CreateSurroundingHNTable();
        for (int I = 0, last = Table.Count - 1; I <= last; I++)
        {
            var item = Table[I];
            HexGeometry.CenterHNtoHX(MapField.Xhx, MapField.Yhx, item, ref X, ref Y);
            if (X >= 0 && Y >= 0 && X < Level.Width && Y < Level.Height)
            {
                Road = (AbstractRoad)Level.GetField(X, Y).FindHS(typeof(AbstractRoad));
                if (Road != null)
                {
                    Road.UpdateTransitions();
                }
            }
        }

        Table?.Dispose();
    }

    // Structur.PAS:1087
    public sbyte GetPlayer() => ((AoWMapField)Owner).GetPlayer();

    // Structur.PAS:1217
    public void FogChanged()
    {
        HexNumberWalkData Walk = default;
        int Count;
        MapLevel Level;
        FFogged = AoWHSMap.Current.FFogEnabled;
        if (FFogged)
        {
            Count = GetTerrainCount();
            Level = (MapLevel)Owner.Owner;
            HexNumberWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (Walk.HN < Count && FFogged)
            {
                if (FResource.TerrainList.Terrain[Walk.HN] >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var fieldValue = Level.GetField(Walk.X, Walk.Y);
                    FFogged = Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((AoWMapField)fieldValue).FFieldFlags).Hi).Lo == 0;
                }

                HexNumberWalkData.WalkToNextHN(ref Walk);
            }
        }
    }

    // Structur.PAS:1366
    public bool ValidateObservation(sbyte Player)
    {
        HexNumberWalkData Walk = default;
        MapLevel Level;
        bool Result = false;
        Level = (MapLevel)Owner.Owner;
        HexNumberWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
        while (true)
        {
            var terrainCount = GetTerrainCount();
            if (!(Walk.HN < terrainCount && !Result))
                break;
            if (FResource.TerrainList.Used[Walk.HN] && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                var lhx = GetLhx();
                Result = AoWHSMap.Current.ValidateObservation(Player, (sbyte)Walk.X, (sbyte)Walk.Y, lhx, 0);
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        return Result;
    }

    // Structur.PAS:1409
    public bool GetBuilding() => FBuildingPlayer != -1;

    // Structur.PAS:1655
    public bool ShowBorderImage(LockedSurface SurfaceDesc, Rect R)
    {
        LibraryImage TopImage;
        LibraryImage MiddleImage;
        LibraryImage BottomImage;
        int Height;
        int Y;
        bool Result = false;
        if (GetTopBorderImage() != null)
        {
            if (SurfaceDesc.ClipRect.Right > R.Right)
            {
                SurfaceDesc.ClipRect.Right = R.Right;
            }

            if (SurfaceDesc.ClipRect.Bottom > R.Bottom)
            {
                SurfaceDesc.ClipRect.Bottom = R.Bottom;
            }

            Height = R.Bottom - R.Top;
            TopImage = GetTopBorderImage();
            MiddleImage = GetMiddleBorderImage();
            BottomImage = GetBottomBorderImage();
            if (BottomImage != null && TopImage != null && MiddleImage != null)
            {
                // { Original uses the height, rather than R.Bottom, for the bottom image. }
                BottomImage.Show(R.Left, Height - BottomImage.Height, SurfaceDesc);
                if (Height > BottomImage.Height)
                {
                    Y = R.Bottom - BottomImage.Height - MiddleImage.Height;
                    int last = (Height - BottomImage.Height) / MiddleImage.Height;
                    for (int I = 0; I <= last; I++)
                    {
                        MiddleImage.Show(R.Left, Y, SurfaceDesc);
                        Y -= MiddleImage.Height;
                    }

                    TopImage.Show(R.Left, R.Top, SurfaceDesc);
                }

                Result = true;
            }
        }

        return Result;
    }

    // Structur.PAS:1688
    public bool ShowBorderImageDC(int DC, Rect R)
    {
        Dib16Surface Surface;
        int Width;
        int Height;
        LockedSurface SurfaceDesc = default;
        Rect BorderRect;
        bool Result = false;
        if (GetBottomBorderImage() != null)
        {
            Width = R.Right - R.Left;
            Height = R.Bottom - R.Top;
            Surface = new Dib16Surface(Width, Height, PixelFormats.pfRGB565);
            try
            {
                BorderRect = new Rect(0, 0, Width, Height);
                Surface.Lock(ref SurfaceDesc);
                Result = ShowBorderImage(SurfaceDesc, BorderRect);
                Surface.Unlock();
                /* TODO: Implement cell or array backing for this address. */
                throw new NotImplementedException();
            }
            finally
            {
                Surface?.Dispose();
            }
        }

        return Result;
    }

    // Structur.PAS:1795
    public void ShowRazeAnimation(Animation Animation, int X, int Y, in LockedSurface SurfaceDesc)
    {
        if (((ImageSequenceAnimation)Animation).FImageSequence.GetUnloopedFrameIndex(Animation.FFrameCounter) == 0 && GameTables.AoWEngine.FAnimationDetail != (byte)DetailLevel.Low)
        {
            var random = Random(2);
            AudioPlayback.PlaySample(AbilityRules.ExplosionSFX[random]);
        }
    }

    private static readonly string StructureRazedRStr = "%s razed";
    private static readonly string StructureBuiltRStr = "%s built";
    private static readonly string StructureRebuiltRStr = "%s rebuilt";
    private static readonly string XatLocationYRStr = "%0:s at location %1:s";
    private static readonly string NotEnoughForcesRStr = "Your forces aren't sufficient";
    private static readonly string RazeStructureWarningRStr = "You're about to raze this structure.";
    private static readonly string DoYouWishToContinueRStr = "Do you wish to continue?";

    // intf_P1_09.inc:78

    // intf_P1_09.inc:78

}
