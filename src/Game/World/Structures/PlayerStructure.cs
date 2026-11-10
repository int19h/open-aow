// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Ai;

using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World.Structures;

// player, turns owned (FProductionPriority), flag ID, victory-location bits, unfog range
public partial class PlayerStructure : Structure
{

    public PlayerStructure(ECustomNode owner) : base(owner)
    {
        FVictoryLocations = new BitList();
        FUnfogRange = 0;
    }

    public new const int ClassID = 131916;

    public sbyte FPlayer;
    public byte FProductionPriority;
    public byte FFlagID;
    public BitList FVictoryLocations;
    public byte FUnfogRange;

    // Structur.PAS:1977
    public override void Dispose()
    {
        base.Dispose();
        FVictoryLocations?.Dispose();
    }

    // Structur.PAS:2180
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 5);
        Stream.rwStreamProc(6, FVictoryLocations.LoadFromStream, FVictoryLocations.SaveToStream);
        Stream.rwByte(ref FProductionPriority, 7);
    }

    // Structur.PAS:1970

    // Structur.PAS:2328
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001989)
        {
            if (AoWHSMap.Current.FPlayers.GetNumberOfPlayers() < FPlayer)
            {
                SetPlayer(0);
            }

            UpdateFlagID();
        }
        else
        {
            if (Msg.MsgID == 537001992)
            {
                UpdateExploration();
                UpdateUnfog();
            }
        }
    }

    // Structur.PAS:2188
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        if (FFlagID != 63 && (!AoWHSMap.Current.FFogEnabled || !FFogged))
        {
            var playerStructureResource = (PlayerStructureResource)FResource;
            if (playerStructureResource.FFlagType <= 1)
            {
                AoWHSSet.Current.FFlagControl.ShowFlag(playerStructureResource.FFlagX + XhpSurf, playerStructureResource.FFlagY + YhpSurf, playerStructureResource.Engine.HSMap.Frame + Xhp, FFlagID, SurfaceDesc);
            }
            else
            {
                playerStructureResource.FFlagControl.ShowStructureFlag(playerStructureResource.FFlagX + XhpSurf, playerStructureResource.FFlagY + YhpSurf, playerStructureResource.Engine.HSMap.Frame + Xhp, FFlagID, SurfaceDesc);
            }
        }

        ((StructureResource)FResource).Show(XhpSurf, YhpSurf, SurfaceDesc, this);
    }

    // Structur.PAS:2370
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        if (Msg.MsgID == 4364 && Msg.HN == 0 && FResource.TerrainList.Terrain[Msg.HN] >= 0)
        {
            if (!AoWHSMap.Current.FFogEnabled || !FFogged)
            {
                if (FPlayer >= 0)
                {
                    if (FPlayer > 0)
                    {
                        var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
                        Msg.Param[0] = Msg.Param[0].Int + (players.FPlayerRace + 1) * 256;
                    }
                    else
                    {
                        Msg.Param[0] = Msg.Param[0].Int + 3328;
                    }
                }
                else
                {
                    Msg.Param[0] = Msg.Param[0].Int + 3584;
                }

                if (AoWHSMap.Current.FVictoryCondition == AoWHSMap.VictoryCondition.CaptureLocations && FVictoryLocations[AoWHSMap.Current.FSeatedPlayer])
                {
                    Msg.Param[0] = Msg.Param[0].Int | 16777216;
                }
            }
            else
            {
                Msg.Param[0] = Msg.Param[0].Int + 3584;
            }
        }
        else
        {
            base.MapFieldMsgProc(ref Msg);
        }

        if (Msg.MsgID == 4352)
        {
            if (FResource.TerrainList.Terrain[Msg.HN] >= 0 && !GetRazed())
            {
                if (FPlayer == 0)
                {
                    if (((PlayerStructureResource)FResource).FClaimed)
                    {
                        Msg.Result = 0;
                    }
                    else
                    {
                        Msg.Result = -1;
                    }
                }
                else
                {
                    Msg.Result = FPlayer;
                }
            }
        }
        else
        {
            if (Msg.MsgID == 4365 && FResource.TerrainList.Terrain[Msg.HN] >= 0)
            {
                bool condition = FPlayer > 0;
                if (condition)
                {
                    var visibilityRange = VisibilityRange();
                    condition = Msg.Param[0].Int <= visibilityRange;
                }

                if (condition)
                {
                    var trueVisionRange = TrueVisionRange();
                    AoWHSMap.Current.UpdatePlayerSeen(FPlayer, Msg.MapField.Xhx, Msg.MapField.Yhx, Msg.MapField.Lhx, Msg.Param[0].Int, trueVisionRange);
                }
            }
            else
            {
                if (Msg.MsgID == 4400 && FResource.TerrainList.Terrain[Msg.HN] >= 0)
                {
                    Msg.Result = FPlayer;
                }
                else
                {
                    if (Msg.MsgID == 4353 && FResource.TerrainList.Terrain[Msg.HN] >= 0)
                    {
                        var moveAction = GetMoveAction((sbyte)Msg.Param[0].Int, (MoveAction)Msg.Param[1].Int);
                        Msg.Param[1] = (int)moveAction;
                    }
                    else
                    {
                        if (Msg.MsgID == 4362 && FPlayer > 0)
                        {
                            if (FPlayer == Msg.Param[1].Int || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FDiplomaticRelations.GetDiplomaticRelation((sbyte)Msg.Param[1].Int) == DiplomaticRelation.Alliance)
                            {
                                var visibilityRange = VisibilityRange();
                                if (Msg.Param[0].Int <= visibilityRange)
                                {
                                    Msg.Result = 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (Msg.MsgID == 4363 && FPlayer > 0)
        {
            if (FPlayer == Msg.Param[1].Int || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FDiplomaticRelations.GetDiplomaticRelation((sbyte)Msg.Param[1].Int) == DiplomaticRelation.Alliance)
            {
                var trueVisionRange = TrueVisionRange();
                if (Msg.Param[0].Int <= trueVisionRange)
                {
                    Msg.Result = 1;
                }
            }
        }
        else
        {
            if (Msg.MsgID == 4368 && FResource.TerrainList.Terrain[Msg.HN] >= 0)
            {
                FinalizeCombat(Msg.Param[0].As<Combat.Battle>());
            }
            else
            {
                if (Msg.MsgID == 4361 && FResource.TerrainList.Terrain[Msg.HN] >= 0)
                {
                    var combatEventText = GetCombatEventText(Msg.Param[0].As<Combat.Battle>());
                    Msg.Result.As<CombatResultQuery>().Text = combatEventText;
                }
            }
        }
    }

    // Structur.PAS:2302
    public override void CanMoveOn(in HSMoveOnInfo Info, ref HSMoveOnResult MoveResult)
    {
        if (FResource.TerrainList.Terrain[Info.HN] >= 0)
        {
            if (!GetRazed() && FPlayer != -1 && ((MoveControl)Info.Sender).FPlayer != FPlayer && Info.MoveOverCode != 1)
            {
                MoveResult.Code = -1;
                MoveResult.Done = true;
            }
        }
    }

    // Structur.PAS:2313
    public override int CanMoveOver(in HSSenderInfo Info)
    {
        int Result;
        if (FResource.TerrainList.Terrain[Info.HN] >= 0 && (!GetRazed() || GetBuilding()))
        {
            if (FPlayer == 0)
            {
                if (((MoveControl)Info.Sender).FPlayer == FPlayer || !((PlayerStructureResource)FResource).FClaimed)
                {
                    Result = 1;
                }
                else
                {
                    Result = 0;
                }
            }
            else
            {
                if (((MoveControl)Info.Sender).FPlayer == FPlayer || FPlayer == -1)
                {
                    Result = 1;
                }
                else
                {
                    Result = 0;
                }
            }
        }
        else
        {
            Result = 1;
        }

        return Result;
    }

    // Structur.PAS:2285
    public override void Activate()
    {
        base.Activate();
        UpdateFlagID();
        UpdateUnfog();
        RegisterVictoryLocations();
        if (FPlayer != -1)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FStructures.AddChild(this);
        }
    }

    // Structur.PAS:2294
    public override void Deactivate()
    {
        if (FPlayer != -1)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FStructures.RemoveChild(this);
        }

        UnregisterVictoryLocations();
        base.Deactivate();
        UpdateUnfog();
    }

    // Structur.PAS:2343
    public override void UpdateMapField(in HSMapFieldInfo Info)
    {
        if (FResource.TerrainList.Terrain[Info.HN] >= 0)
        {
            ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 4096 | 8192 | 16384 | 33554432 | 16777216 | 134217728;
        }
    }

    // Structur.PAS:2084
    public override void NewDay()
    {
        base.NewDay();
        if (AoWHSMap.Current.FDay == 1 && FPlayer > 0)
        {
            UpdateExploration();
            UpdatePlayerSeen();
        }
    }

    // Structur.PAS:2078
    public override void NewTurn(sbyte Player)
    {
        base.NewTurn(Player);
        if (Player == FPlayer && FProductionPriority != 255)
        {
            FProductionPriority = (byte)(FProductionPriority + 1);
        }
    }

    // Structur.PAS:2279
    public override void SeatedPlayerChanged()
    {
        base.SeatedPlayerChanged();
        UpdateUnfog();
    }

    // Structur.PAS:2168
    public override void ArmyPlaced(ArmyHS PlacedArmy)
    {
        UpdatePlayer();
    }

    // Structur.PAS:2171
    public override void ArmyChanged(ArmyHS Army)
    {
        UpdatePlayer();
    }

    // Structur.PAS:2022
    public override void BuildingDone()
    {
        base.BuildingDone();
        UpdatePlayer();
        UpdateExploration();
        UpdateUnfog();
    }

    // Structur.PAS:2035
    public override bool ExecuteRebuild(AbstractUnit AUnit)
    {
        bool Result = base.ExecuteRebuild(AUnit);
        if (Result)
        {
            SetPlayer(AUnit.FPlayer);
        }

        return Result;
    }

    // Structur.PAS:2041
    public override bool ExecuteBuild(AbstractUnit AUnit, int Days)
    {
        bool Result = base.ExecuteBuild(AUnit, Days);
        if (Result)
        {
            SetPlayer(AUnit.FPlayer);
        }

        return Result;
    }

    // Structur.PAS:2352
    public override void UpdateAITarget(in AITargetInfo Info, ref AITargetData Data)
    {
        if (GetRazed() && !GetBuilding())
        {
            return;
        }

        switch (Info.Mode)
        {
            case AITargetMode.Attack:
                if (Info.Player != FPlayer && !GetRazed())
                {
                    if (FPlayer == 0 || AbilityRules.ValidAttackRelation(Info.Player, Info.IndependentRelation, FPlayer, 0))
                    {
                        var defaultAITargetPriority = GetDefaultAITargetPriority();
                        Data.Priority = defaultAITargetPriority;
                    }
                }

                break;
        }
    }

    // Structur.PAS:2030
    public override bool CanRebuild(AbstractUnit AUnit, ref string Reason) => base.CanRebuild(AUnit, ref Reason);

    // Structur.PAS:2130
    public virtual void SetPlayer(sbyte Player)
    {
        if (Player != FPlayer)
        {
            if (GetRazed() && !GetBuilding())
            {
                return;
            }

            FProductionPriority = 0;
            if (FPlayer != -1)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FStructures.RemoveChild(this);
            }

            if (AoWHSMap.Current.FDay > 0)
            {
                StructureCaptured(FPlayer, Player);
            }

            FPlayer = Player;
            UpdateUnfog();
            UpdatePlayerSeen();
            UpdateExploration();
            var hNtoRad = HexGeometry.HNtoRad(FResource.TerrainList.Count);
            var lhx = GetLhx();
            var yhx = GetYhx();
            var xhx = GetXhx();
            AoWHSMap.Current.ScannerIconsChangedRad(xhx, yhx, lhx, hNtoRad);
            UpdateFlagID();
            FogChanged();
            var lhx2 = GetLhx();
            var yhx2 = GetYhx();
            var xhx2 = GetXhx();
            AoWHSMap.Current.NotifyPlayerChanged(xhx2, yhx2, lhx2);
            if (FPlayer != -1)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FStructures.AddChild(this);
            }

            if (FPlayer > 0 && AoWHSMap.Current.FVictoryCondition == AoWHSMap.VictoryCondition.CaptureLocations)
            {
                for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
                {
                    if (FVictoryLocations[I])
                    {
                        if (I == FPlayer || AoWHSMap.Current.FAlliedVictory && AoWHSMap.Current.FPlayers.GetDiplomaticRelation(FPlayer, (sbyte)I) == DiplomaticRelation.Alliance)
                        {
                            AoWHSMap.Current.FPlayers.GetPlayers(I).CheckCaptureLocations();
                        }
                    }
                }
            }

            Update();
        }
    }

    // Structur.PAS:2174
    public virtual int GetIncome() => 0;

    // Structur.PAS:2177
    public virtual int GetPower() => 0;

    // Structur.PAS:2159
    public virtual void UpdateFlagID()
    {
        if (!FStatus.Contains(MapObjectStatusElement.EditMode))
        {
            if (FPlayer > 0)
            {
                FFlagID = (byte)AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerRace;
            }
            else
            {
                if (((PlayerStructureResource)FResource).FClaimed)
                {
                    FFlagID = 32;
                }
                else
                {
                    FFlagID = 63;
                }
            }
        }
        else
        {
            FFlagID = 63;
        }
    }

    // Structur.PAS:2208
    public virtual int VisibilityRange()
    {
        int Result;
        if (!GetBuilding())
        {
            Result = 6;
            if (GetLhx() != 0 || FPlayer > 0 && (((byte)AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FEnemyEnchantments.Image) & 8) != 0)
            {
                Result = (int)(((uint)(Result + 1)) >> 1);
            }
        }
        else
        {
            Result = 4;
        }

        return Result;
    }

    // Structur.PAS:2249
    public virtual void UpdateUnfog()
    {
        int Range;
        int X = default;
        int Y = default;
        if (!FStatus.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            return;
        }

        if (FStatus.Contains(MapObjectStatusElement.Activated) && FPlayer != -1 && AoWHSMap.Current.FSeatedPlayer != -1 && (AoWHSMap.Current.FSeatedPlayer == FPlayer || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FDiplomaticRelations.GetDiplomaticRelation(AoWHSMap.Current.FSeatedPlayer) == DiplomaticRelation.Alliance))
        {
            Range = VisibilityRange();
        }
        else
        {
            Range = 0;
        }

        if (Range != FUnfogRange)
        {
            if (FUnfogRange != 0)
            {
                for (int I = 0, last = FResource.TerrainList.Count - 1; I <= last; I++)
                {
                    if (FResource.TerrainList.Terrain[I] >= 0)
                    {
                        HexGeometry.CenterHNtoHX(FXhx, FYhx, I, ref X, ref Y);
                        AoWHSMap.Current.FogArea(X, Y, ((MapField)Owner).Lhx, FUnfogRange);
                    }
                }
            }

            FUnfogRange = (byte)Range;
            if (FUnfogRange != 0)
            {
                for (int I = 0, last = FResource.TerrainList.Count - 1; I <= last; I++)
                {
                    if (FResource.TerrainList.Terrain[I] >= 0)
                    {
                        HexGeometry.CenterHNtoHX(FXhx, FYhx, I, ref X, ref Y);
                        AoWHSMap.Current.UnFogArea(X, Y, ((MapField)Owner).Lhx, FUnfogRange);
                    }
                }
            }
        }
    }

    // Structur.PAS:2233
    public virtual void UpdateExploration()
    {
        int X = default;
        int Y = default;
        if (FStatus.Contains(MapObjectStatusElement.PlacedOnMap) && FPlayer > 0)
        {
            for (int Player = 1, last = AoWHSMap.Current.FPlayers.Count - 1; Player <= last; Player++)
            {
                if (Player == FPlayer || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FDiplomaticRelations.GetDiplomaticRelation((sbyte)Player) == DiplomaticRelation.Alliance)
                {
                    for (int I = 0, lastI = FResource.TerrainList.Count - 1; I <= lastI; I++)
                    {
                        if (FResource.TerrainList.Terrain[I] >= 0)
                        {
                            HexGeometry.CenterHNtoHX(FXhx, FYhx, I, ref X, ref Y);
                            var visibilityRange = VisibilityRange();
                            AoWHSMap.Current.ExploreArea(X, Y, ((MapField)Owner).Lhx, (sbyte)Player, visibilityRange);
                        }
                    }
                }
            }
        }
    }

    // Structur.PAS:2124
    // { Original leaves the result unchanged for a razed structure. }
    public virtual void FinalizeCombat(Combat.Battle Combat)
    {
        if (Combat.FCombatResult == CombatResult.Victory || Combat.FCombatResult == CombatResult.NoDefenders)
        {
            SetPlayer(Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FAttackerPlayer).FPlayer);
        }
    }

    // Structur.PAS:2115
    public virtual string GetCombatEventText(Combat.Battle Combat)
    {
        string Result = "";
        if (!GetRazed())
        {
            if (Combat.FCombatResult == CombatResult.Victory || Combat.FCombatResult == CombatResult.NoDefenders)
            {
                var editName = EditName();
                Result = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.StructureCapturedRStr), [editName]);
            }
            else
            {
                Result = "";
            }
        }

        return Result;
    }

    // Structur.PAS:2094
    public virtual void StructureCaptured(sbyte OldPlayer, sbyte NewPlayer)
    {
        LocationTextEventLog Log;
        if (!GetRazed() && !GetBuilding() && OldPlayer == 0 && !AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting) && FStatus.Contains(MapObjectStatusElement.Activated) && !((PlayerStructureResource)FResource).FClaimed)
        {
            Log = new LocationTextEventLog(null);
            Log.FPlayer = NewPlayer;
            var editName = EditName();
            Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.StructureCapturedRStr), [editName]);
            Log.FXhx = GetXhx();
            Log.FYhx = GetYhx();
            Log.FLhx = GetLhx();
            var yhx = GetYhx();
            var lhx = GetLhx();
            var xhx = GetXhx();
            EventLog.DistributeLocationEventLog(Log, FPlayer, xhx, yhx, lhx, false);
            Log.Release();
        }
    }

    // Structur.PAS:2363
    public virtual MoveAction GetMoveAction(sbyte Player, MoveAction Action)
    {
        MoveAction Result;
        if (Player != FPlayer && !GetRazed() && (FPlayer != 0 || ((PlayerStructureResource)FResource).FClaimed))
        {
            Result = MoveAction.Attack;
        }
        else
        {
            Result = Action;
        }

        return Result;
    }

    // Structur.PAS:2349
    public virtual int GetDefaultAITargetPriority() => 10;

    // Structur.PAS:2047
    public virtual bool UpdatePlayer()
    {
        sbyte Player;
        bool Different;
        int Count;
        MapLevel Level;
        HNWalkData Walk = default;
        ArmyHS Army;
        Player = -1;
        Different = false;
        Count = GetTerrainCount();
        Level = (MapLevel)Owner.Owner;
        HNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
        while (Walk.HN < Count && !Different)
        {
            if (FResource.TerrainList.Terrain[Walk.HN] >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Army = (ArmyHS)Level.GetField(Walk.X, Walk.Y).FindOwnedChild(131607);
                if (Army != null)
                {
                    if (Player != -1)
                    {
                        Different = Army.FArmy.FPlayer != Player;
                    }
                    else
                    {
                        Player = Army.FArmy.FPlayer;
                    }
                }
            }

            HNWalkData.WalkToNextHN(ref Walk);
        }

        if (Player != -1)
        {
            if (Different)
            {
                SetPlayer(-1);
            }
            else
            {
                SetPlayer(Player);
            }
        }

        return !Different;
    }

    // Structur.PAS:1986
    public bool GetVictoryLocation(sbyte Player) => FVictoryLocations[Player];

    // Structur.PAS:1991
    public void SetVictoryLocation(sbyte Player, bool Value)
    {
        if (FVictoryLocations[Player] != Value)
        {
            FVictoryLocations[Player] = Value;
            if (Value)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(Player).RegisterVictoryLocation(this);
            }
            else
            {
                AoWHSMap.Current.FPlayers.GetPlayers(Player).UnregisterVictoryLocation(this);
            }

            Update();
        }
    }

    // Structur.PAS:2002
    public void RegisterVictoryLocations()
    {
        for (int I = 1, last = FVictoryLocations.Count - 1; I <= last; I++)
        {
            if (FVictoryLocations[I])
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(I) != null)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(I).RegisterVictoryLocation(this);
                }
                else
                {
                    FVictoryLocations[I] = false;
                }
            }
        }
    }

    // Structur.PAS:2013
    public void UnregisterVictoryLocations()
    {
        for (int I = 1, last = FVictoryLocations.Count - 1; I <= last; I++)
        {
            if (FVictoryLocations[I] && AoWHSMap.Current.FPlayers.GetPlayers(I) != null)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(I).UnregisterVictoryLocation(this);
            }
        }
    }

    // Structur.PAS:2205
    public int TrueVisionRange() => 1;

    // Structur.PAS:2220
    public void UpdatePlayerSeen()
    {
        int X = default;
        int Y = default;
        if (FStatus.Contains(MapObjectStatusElement.PlacedOnMap) && FPlayer > 0)
        {
            for (int I = 0, last = FResource.TerrainList.Count - 1; I <= last; I++)
            {
                if (FResource.TerrainList.Terrain[I] >= 0)
                {
                    HexGeometry.CenterHNtoHX(FXhx, FYhx, I, ref X, ref Y);
                    var lhx = GetLhx();
                    var visibilityRange = VisibilityRange();
                    var trueVisionRange = TrueVisionRange();
                    AoWHSMap.Current.UpdatePlayerSeen(FPlayer, X, Y, lhx, visibilityRange, trueVisionRange);
                }
            }
        }
    }

    // intf_P1_09.inc:249

}
