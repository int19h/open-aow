// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI;
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
        _victoryLocations = new BitList();
        UnfogRange = 0;
    }

    public new const int ClassId = 131916;

    public sbyte Player;
    public byte ProductionPriority;
    private byte _flagId;
    private readonly BitList _victoryLocations;

    public byte UnfogRange { get; protected set; }

    // Structur.PAS:1977
    public override void Dispose()
    {
        base.Dispose();
        _victoryLocations?.Dispose();
    }

    // Structur.PAS:2180
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Player), 5);
        stream.Payload(6, _victoryLocations.LoadFromStream, _victoryLocations.SaveToStream);
        stream.Byte(ref ProductionPriority, 7);
    }

    // Structur.PAS:1970

    // Structur.PAS:2328
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001989)
        {
            if (StrategicMap.Current.Players.GetNumberOfPlayers() < Player)
            {
                SetPlayer(0);
            }

            UpdateFlagId();
        }
        else
        {
            if (message.MessageId == 537001992)
            {
                UpdateExploration();
                UpdateUnfog();
            }
        }
    }

    // Structur.PAS:2188
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out pixelX, out pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        if (_flagId != 63 && (!StrategicMap.Current.FogEnabled || !Fogged))
        {
            var playerStructureResource = (PlayerStructureResource)_resource;
            if (playerStructureResource.FlagType <= 1)
            {
                GamePack.Current.FlagControl.ShowFlag(playerStructureResource.FlagX + pixelXSurface, playerStructureResource.FlagY + pixelYSurface, playerStructureResource.Engine.HexSpriteMap.Frame + pixelX, _flagId, lockedSurface);
            }
            else
            {
                playerStructureResource.FlagControl.ShowStructureFlag(playerStructureResource.FlagX + pixelXSurface, playerStructureResource.FlagY + pixelYSurface, playerStructureResource.Engine.HexSpriteMap.Frame + pixelX, _flagId, lockedSurface);
            }
        }

        ((StructureResource)_resource).Show(pixelXSurface, pixelYSurface, lockedSurface, this);
    }

    // Structur.PAS:2370
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (message.MessageId == 4364 && message.HexNumber == 0 && _resource.TerrainList.Terrain[message.HexNumber] >= 0)
        {
            if (!StrategicMap.Current.FogEnabled || !Fogged)
            {
                if (Player >= 0)
                {
                    if (Player > 0)
                    {
                        var players = StrategicMap.Current.Players.GetPlayers(Player);
                        message.Param[0] = message.Param[0].Int + (players.PlayerRace + 1) * 256;
                    }
                    else
                    {
                        message.Param[0] = message.Param[0].Int + 3328;
                    }
                }
                else
                {
                    message.Param[0] = message.Param[0].Int + 3584;
                }

                if (StrategicMap.Current.VictoryCondition == StrategicMapVictoryCondition.CaptureLocations && _victoryLocations[StrategicMap.Current.SeatedPlayer])
                {
                    message.Param[0] = message.Param[0].Int | 16777216;
                }
            }
            else
            {
                message.Param[0] = message.Param[0].Int + 3584;
            }
        }
        else
        {
            base.MapFieldMessageProc(ref message);
        }

        if (message.MessageId == 4352)
        {
            if (_resource.TerrainList.Terrain[message.HexNumber] >= 0 && !GetRazed())
            {
                if (Player == 0)
                {
                    if (((PlayerStructureResource)_resource).Claimed)
                    {
                        message.Result = 0;
                    }
                    else
                    {
                        message.Result = -1;
                    }
                }
                else
                {
                    message.Result = Player;
                }
            }
        }
        else
        {
            if (message.MessageId == 4365 && _resource.TerrainList.Terrain[message.HexNumber] >= 0)
            {
                bool condition = Player > 0;
                if (condition)
                {
                    var visibilityRange = VisibilityRange();
                    condition = message.Param[0].Int <= visibilityRange;
                }

                if (condition)
                {
                    var trueVisionRange = TrueVisionRange();
                    StrategicMap.Current.UpdatePlayerSeen(Player, message.MapField.HexX, message.MapField.HexY, message.MapField.HexLevel, message.Param[0].Int, trueVisionRange);
                }
            }
            else
            {
                if (message.MessageId == 4400 && _resource.TerrainList.Terrain[message.HexNumber] >= 0)
                {
                    message.Result = Player;
                }
                else
                {
                    if (message.MessageId == 4353 && _resource.TerrainList.Terrain[message.HexNumber] >= 0)
                    {
                        var moveAction = GetMoveAction((sbyte)message.Param[0].Int, (MoveAction)message.Param[1].Int);
                        message.Param[1] = (int)moveAction;
                    }
                    else
                    {
                        if (message.MessageId == 4362 && Player > 0)
                        {
                            if (Player == message.Param[1].Int || StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation((sbyte)message.Param[1].Int) == DiplomaticRelation.Alliance)
                            {
                                var visibilityRange = VisibilityRange();
                                if (message.Param[0].Int <= visibilityRange)
                                {
                                    message.Result = 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (message.MessageId == 4363 && Player > 0)
        {
            if (Player == message.Param[1].Int || StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation((sbyte)message.Param[1].Int) == DiplomaticRelation.Alliance)
            {
                var trueVisionRange = TrueVisionRange();
                if (message.Param[0].Int <= trueVisionRange)
                {
                    message.Result = 1;
                }
            }
        }
        else
        {
            if (message.MessageId == 4368 && _resource.TerrainList.Terrain[message.HexNumber] >= 0)
            {
                FinalizeCombat(message.Param[0].As<Battle>());
            }
            else
            {
                if (message.MessageId == 4361 && _resource.TerrainList.Terrain[message.HexNumber] >= 0)
                {
                    var combatEventText = GetCombatEventText(message.Param[0].As<Battle>());
                    message.Result.As<CombatResultQuery>().Text = combatEventText;
                }
            }
        }
    }

    // Structur.PAS:2302
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (_resource.TerrainList.Terrain[info.HexNumber] >= 0)
        {
            if (!GetRazed() && Player != -1 && ((MoveControl)info.Sender).Player != Player && info.MoveOverCode != 1)
            {
                moveResult.Code = -1;
                moveResult.Done = true;
            }
        }
    }

    // Structur.PAS:2313
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        int result;
        if (_resource.TerrainList.Terrain[info.HexNumber] >= 0 && (!GetRazed() || GetBuilding()))
        {
            if (Player == 0)
            {
                if (((MoveControl)info.Sender).Player == Player || !((PlayerStructureResource)_resource).Claimed)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            else
            {
                if (((MoveControl)info.Sender).Player == Player || Player == -1)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
        }
        else
        {
            result = 1;
        }

        return result;
    }

    // Structur.PAS:2285
    public override void Activate()
    {
        base.Activate();
        UpdateFlagId();
        UpdateUnfog();
        RegisterVictoryLocations();
        if (Player != -1)
        {
            StrategicMap.Current.Players.GetPlayers(Player).Structures.AddChild(this);
        }
    }

    // Structur.PAS:2294
    public override void Deactivate()
    {
        if (Player != -1)
        {
            StrategicMap.Current.Players.GetPlayers(Player).Structures.RemoveChild(this);
        }

        UnregisterVictoryLocations();
        base.Deactivate();
        UpdateUnfog();
    }

    // Structur.PAS:2343
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (_resource.TerrainList.Terrain[info.HexNumber] >= 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | 4096 | 8192 | 16384 | 33554432 | 16777216 | 134217728;
        }
    }

    // Structur.PAS:2084
    public override void NewDay()
    {
        base.NewDay();
        if (StrategicMap.Current.Day == 1 && Player > 0)
        {
            UpdateExploration();
            UpdatePlayerSeen();
        }
    }

    // Structur.PAS:2078
    public override void NewTurn(sbyte player)
    {
        base.NewTurn(player);
        if (player == Player && ProductionPriority != 255)
        {
            ProductionPriority = (byte)(ProductionPriority + 1);
        }
    }

    // Structur.PAS:2279
    public override void SeatedPlayerChanged()
    {
        base.SeatedPlayerChanged();
        UpdateUnfog();
    }

    // Structur.PAS:2168
    public override void ArmyPlaced(ArmyHexSprite placedArmy)
    {
        UpdatePlayer();
    }

    // Structur.PAS:2171
    public override void ArmyChanged(ArmyHexSprite army)
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
    public override bool ExecuteRebuild(Troop troop)
    {
        bool result = base.ExecuteRebuild(troop);
        if (result)
        {
            SetPlayer(troop.Player);
        }

        return result;
    }

    // Structur.PAS:2041
    public override bool ExecuteBuild(Troop troop, int days)
    {
        bool result = base.ExecuteBuild(troop, days);
        if (result)
        {
            SetPlayer(troop.Player);
        }

        return result;
    }

    // Structur.PAS:2352
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        if (GetRazed() && !GetBuilding())
        {
            return;
        }

        switch (info.Mode)
        {
            case AITargetMode.Attack:
                if (info.Player != Player && !GetRazed())
                {
                    if (Player == 0 || AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, Player, 0))
                    {
                        var defaultAITargetPriority = GetDefaultAITargetPriority();
                        data.Priority = defaultAITargetPriority;
                    }
                }

                break;
        }
    }

    // Structur.PAS:2030
    public override bool CanRebuild(Troop troop, ref string reason) => base.CanRebuild(troop, ref reason);

    // Structur.PAS:2130
    public virtual void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            if (GetRazed() && !GetBuilding())
            {
                return;
            }

            ProductionPriority = 0;
            if (Player != -1)
            {
                StrategicMap.Current.Players.GetPlayers(Player).Structures.RemoveChild(this);
            }

            if (StrategicMap.Current.Day > 0)
            {
                StructureCaptured(Player, player);
            }

            Player = player;
            UpdateUnfog();
            UpdatePlayerSeen();
            UpdateExploration();
            var hexNumberToRadius = HexGeometry.HexNumberToRadius(_resource.TerrainList.Count);
            var hexLevel = GetHexLevel();
            var hexY = GetHexY();
            var hexX = GetHexX();
            StrategicMap.Current.ScannerIconsChangedRadius(hexX, hexY, hexLevel, hexNumberToRadius);
            UpdateFlagId();
            FogChanged();
            var hexLevel2 = GetHexLevel();
            var hexY2 = GetHexY();
            var hexX2 = GetHexX();
            StrategicMap.Current.NotifyPlayerChanged(hexX2, hexY2, hexLevel2);
            if (Player != -1)
            {
                StrategicMap.Current.Players.GetPlayers(Player).Structures.AddChild(this);
            }

            if (Player > 0 && StrategicMap.Current.VictoryCondition == StrategicMapVictoryCondition.CaptureLocations)
            {
                for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
                {
                    if (_victoryLocations[I])
                    {
                        if (I == Player || StrategicMap.Current.AlliedVictory && StrategicMap.Current.Players.GetDiplomaticRelation(Player, (sbyte)I) == DiplomaticRelation.Alliance)
                        {
                            StrategicMap.Current.Players.GetPlayers(I).CheckCaptureLocations();
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
    public virtual void UpdateFlagId()
    {
        if (!Status.Contains(MapObjectStatusElement.EditMode))
        {
            if (Player > 0)
            {
                _flagId = (byte)StrategicMap.Current.Players.GetPlayers(Player).PlayerRace;
            }
            else
            {
                if (((PlayerStructureResource)_resource).Claimed)
                {
                    _flagId = 32;
                }
                else
                {
                    _flagId = 63;
                }
            }
        }
        else
        {
            _flagId = 63;
        }
    }

    // Structur.PAS:2208
    public virtual int VisibilityRange()
    {
        int result;
        if (!GetBuilding())
        {
            result = 6;
            if (GetHexLevel() != 0 || Player > 0 && (((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments.Image) & 8) != 0)
            {
                result = (int)(((uint)(result + 1)) >> 1);
            }
        }
        else
        {
            result = 4;
        }

        return result;
    }

    // Structur.PAS:2249
    public virtual void UpdateUnfog()
    {
        int range;
        int X = default;
        int Y = default;
        if (!Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            return;
        }

        if (Status.Contains(MapObjectStatusElement.Activated) && Player != -1 && StrategicMap.Current.SeatedPlayer != -1 && (StrategicMap.Current.SeatedPlayer == Player || StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation(StrategicMap.Current.SeatedPlayer) == DiplomaticRelation.Alliance))
        {
            range = VisibilityRange();
        }
        else
        {
            range = 0;
        }

        if (range != UnfogRange)
        {
            if (UnfogRange != 0)
            {
                for (int I = 0, last = _resource.TerrainList.Count - 1; I <= last; I++)
                {
                    if (_resource.TerrainList.Terrain[I] >= 0)
                    {
                        HexGeometry.CenterHexNumberToHex(_hexX, _hexY, I, ref X, ref Y);
                        StrategicMap.Current.FogArea(X, Y, Owner.HexLevel, UnfogRange);
                    }
                }
            }

            UnfogRange = (byte)range;
            if (UnfogRange != 0)
            {
                for (int I = 0, last = _resource.TerrainList.Count - 1; I <= last; I++)
                {
                    if (_resource.TerrainList.Terrain[I] >= 0)
                    {
                        HexGeometry.CenterHexNumberToHex(_hexX, _hexY, I, ref X, ref Y);
                        StrategicMap.Current.UnFogArea(X, Y, Owner.HexLevel, UnfogRange);
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
        if (Status.Contains(MapObjectStatusElement.PlacedOnMap) && Player > 0)
        {
            for (int player = 1, last = StrategicMap.Current.Players.Count - 1; player <= last; player++)
            {
                if (player == Player || StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation((sbyte)player) == DiplomaticRelation.Alliance)
                {
                    for (int I = 0, lastI = _resource.TerrainList.Count - 1; I <= lastI; I++)
                    {
                        if (_resource.TerrainList.Terrain[I] >= 0)
                        {
                            HexGeometry.CenterHexNumberToHex(_hexX, _hexY, I, ref X, ref Y);
                            var visibilityRange = VisibilityRange();
                            StrategicMap.Current.ExploreArea(X, Y, Owner.HexLevel, (sbyte)player, visibilityRange);
                        }
                    }
                }
            }
        }
    }

    // Structur.PAS:2124
    // { Original leaves the result unchanged for a razed structure. }
    public virtual void FinalizeCombat(Battle combat)
    {
        if (combat.CombatResult == CombatResult.Victory || combat.CombatResult == CombatResult.NoDefenders)
        {
            SetPlayer(combat.CombatData.Players.GetPlayers(combat.CombatData.AttackerPlayer).Player);
        }
    }

    // Structur.PAS:2115
    public virtual string GetCombatEventText(Battle combat)
    {
        string result = "";
        if (!GetRazed())
        {
            if (combat.CombatResult == CombatResult.Victory || combat.CombatResult == CombatResult.NoDefenders)
            {
                var editName = EditName();
                result = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.StructureCapturedFormat), [editName]);
            }
            else
            {
                result = "";
            }
        }

        return result;
    }

    // Structur.PAS:2094
    public virtual void StructureCaptured(sbyte oldPlayer, sbyte newPlayer)
    {
        LocationTextEventLog log;
        if (!GetRazed() && !GetBuilding() && oldPlayer == 0 && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && Status.Contains(MapObjectStatusElement.Activated) && !((PlayerStructureResource)_resource).Claimed)
        {
            log = new LocationTextEventLog(null) { Holder = CreationHolder() };
            log._player = newPlayer;
            var editName = EditName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.StructureCapturedFormat), [editName]);
            log.HexX = GetHexX();
            log.HexY = GetHexY();
            log.HexLevel = GetHexLevel();
            var hexY = GetHexY();
            var hexLevel = GetHexLevel();
            var hexX = GetHexX();
            EventLog.DistributeLocationEventLog(log, Player, hexX, hexY, hexLevel, false);
            log.Release();
        }
    }

    // Structur.PAS:2363
    public virtual MoveAction GetMoveAction(sbyte player, MoveAction action)
    {
        MoveAction result;
        if (player != Player && !GetRazed() && (Player != 0 || ((PlayerStructureResource)_resource).Claimed))
        {
            result = MoveAction.Attack;
        }
        else
        {
            result = action;
        }

        return result;
    }

    // Structur.PAS:2349
    public virtual int GetDefaultAITargetPriority() => 10;

    // Structur.PAS:2047
    public virtual bool UpdatePlayer()
    {
        sbyte player;
        bool different;
        int count;
        MapLevel level;
        HexNumberWalkData walk = default;
        ArmyHexSprite army;
        player = -1;
        different = false;
        count = GetTerrainCount();
        level = Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, _hexX, _hexY, 0);
        while (walk.HexNumber < count && !different)
        {
            if (_resource.TerrainList.Terrain[walk.HexNumber] >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                army = (ArmyHexSprite)level.GetField(walk.X, walk.Y).FindOwnedChild(131607);
                if (army != null)
                {
                    if (player != -1)
                    {
                        different = army.Army.Player != player;
                    }
                    else
                    {
                        player = army.Army.Player;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        if (player != -1)
        {
            if (different)
            {
                SetPlayer(-1);
            }
            else
            {
                SetPlayer(player);
            }
        }

        return !different;
    }

    // Structur.PAS:1986
    public bool GetVictoryLocation(sbyte player) => _victoryLocations[player];

    // Structur.PAS:1991
    public void SetVictoryLocation(sbyte player, bool value)
    {
        if (_victoryLocations[player] != value)
        {
            _victoryLocations[player] = value;
            if (value)
            {
                StrategicMap.Current.Players.GetPlayers(player).RegisterVictoryLocation(this);
            }
            else
            {
                StrategicMap.Current.Players.GetPlayers(player).UnregisterVictoryLocation(this);
            }

            Update();
        }
    }

    // Structur.PAS:2002
    public void RegisterVictoryLocations()
    {
        for (int I = 1, last = _victoryLocations.Count - 1; I <= last; I++)
        {
            if (_victoryLocations[I])
            {
                if (StrategicMap.Current.Players.GetPlayers(I) != null)
                {
                    StrategicMap.Current.Players.GetPlayers(I).RegisterVictoryLocation(this);
                }
                else
                {
                    _victoryLocations[I] = false;
                }
            }
        }
    }

    // Structur.PAS:2013
    public void UnregisterVictoryLocations()
    {
        for (int I = 1, last = _victoryLocations.Count - 1; I <= last; I++)
        {
            if (_victoryLocations[I] && StrategicMap.Current.Players.GetPlayers(I) != null)
            {
                StrategicMap.Current.Players.GetPlayers(I).UnregisterVictoryLocation(this);
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
        if (Status.Contains(MapObjectStatusElement.PlacedOnMap) && Player > 0)
        {
            for (int I = 0, last = _resource.TerrainList.Count - 1; I <= last; I++)
            {
                if (_resource.TerrainList.Terrain[I] >= 0)
                {
                    HexGeometry.CenterHexNumberToHex(_hexX, _hexY, I, ref X, ref Y);
                    var hexLevel = GetHexLevel();
                    var visibilityRange = VisibilityRange();
                    var trueVisionRange = TrueVisionRange();
                    StrategicMap.Current.UpdatePlayerSeen(Player, X, Y, hexLevel, visibilityRange, trueVisionRange);
                }
            }
        }
    }

    // intf_P1_09.inc:249

}
