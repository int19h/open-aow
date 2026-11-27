#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
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
public partial class PlayerStructure(CustomNode? owner) : Structure(owner)
{

    public new const int ClassId = 131_916;

    public override PlayerStructureResource Resource => GetResource<PlayerStructureResource>();

    public PlayerId? Player { get; set; } = PlayerId.Independent;
    public byte ProductionPriority { get; private set; }

    private byte _flagId;
    private readonly BitList _victoryLocations = [];

    public byte UnfogRange { get; protected set; }

    // Structur.PAS:1977
    public override void Dispose() => base.Dispose();

    // Structur.PAS:2180
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = persister.OptionalPlayer(name: "Player", value: Player, width: IdWidth.UnsignedByte, fallback: 0);
        _ = persister.BitList(name: "VictoryLocations", list: _victoryLocations);
        ProductionPriority = persister.Byte("ProductionPriority", value: ProductionPriority);
    }

    // Structur.PAS:2328
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001989)
        {
            if (StrategicMap.Current.Players.NumberOfPlayers < PlayerIdExtensions.ToOriginal(Player))
            {
                SetPlayer(PlayerId.Independent);
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
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        if (_flagId != 63 && (!StrategicMap.Current.FogEnabled || !Fogged))
        {
            PlayerStructureResource playerStructureResource = Resource;
            if (playerStructureResource.FlagType <= 1)
            {
                GamePack.Current.FlagControl.ShowFlag(playerStructureResource.FlagX + pixelXSurface, playerStructureResource.FlagY + pixelYSurface, frameOffset: playerStructureResource.Engine.HexSpriteMap.Frame + pixelX, _flagId, lockedSurface);
            }
            else
            {
                playerStructureResource.FlagControl.ShowStructureFlag(playerStructureResource.FlagX + pixelXSurface, playerStructureResource.FlagY + pixelYSurface, frameOffset: playerStructureResource.Engine.HexSpriteMap.Frame + pixelX, _flagId, lockedSurface);
            }
        }

        Resource.Show(pixelXSurface, pixelYSurface, lockedSurface, structure: this);
    }

    // Structur.PAS:2370
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (message.MessageId == 4364 && message.HexNumber == 0 && Resource.TerrainList.Terrain[message.HexNumber].Value is not null)
        {
            if (!StrategicMap.Current.FogEnabled || !Fogged)
            {
                if (PlayerIdExtensions.ToOriginal(Player) >= 0)
                {
                    if (PlayerIdExtensions.ToOriginal(Player) > 0)
                    {
                        Player? players = StrategicMap.Current.Players.GetPlayers(Player);
                        MapFieldMessageParameters param = message.Param;
                        param[0] = param[0].Int + ((players.PlayerRace + 1) * 256);
                        message.Param = param;
                    }
                    else
                    {
                        MapFieldMessageParameters param2 = message.Param;
                        param2[0] = param2[0].Int + 3328;
                        message.Param = param2;
                    }
                }
                else
                {
                    MapFieldMessageParameters param3 = message.Param;
                    param3[0] = param3[0].Int + 3584;
                    message.Param = param3;
                }

                if (StrategicMap.Current.VictoryCondition == StrategicMapVictoryCondition.CaptureLocations && _victoryLocations[PlayerIdExtensions.ToOriginal(StrategicMap.Current.SeatedPlayer)])
                {
                    MapFieldMessageParameters param4 = message.Param;
                    param4[0] = param4[0].Int | 0b0001_0000_0000_0000_0000_0000_0000;
                    message.Param = param4;
                }
            }
            else
            {
                MapFieldMessageParameters param5 = message.Param;
                param5[0] = param5[0].Int + 3584;
                message.Param = param5;
            }
        }
        else
        {
            base.MapFieldMessageProc(ref message);
        }

        if (message.MessageId == 4352)
        {
            if (Resource.TerrainList.Terrain[message.HexNumber].Value is not null && !GetRazed())
            {
                message.Result = Player == PlayerId.Independent ? Resource.Claimed ? (Rtl.MsgArg)0 : (Rtl.MsgArg)(-1) : Session.MsgArgIds.FromPlayer(Player);
            }
        }
        else
        {
            if (message.MessageId == 4365 && Resource.TerrainList.Terrain[message.HexNumber].Value is not null)
            {
                bool condition = PlayerIdExtensions.ToOriginal(Player) > 0;
                if (condition)
                {
                    int visibilityRange = VisibilityRange();
                    condition = message.Param[0].Int <= visibilityRange;
                }

                if (condition)
                {
                    int trueVisionRange = TrueVisionRange();
                    StrategicMap.Current.UpdatePlayerSeen(Player, message.MapField.HexX, message.MapField.HexY, L: message.MapField.HexLevel, sightRadius: message.Param[0].Int, radius: trueVisionRange);
                }
            }
            else
            {
                if (message.MessageId == 4400 && Resource.TerrainList.Terrain[message.HexNumber].Value is not null)
                {
                    message.Result = Session.MsgArgIds.FromPlayer(Player);
                }
                else
                {
                    if (message.MessageId == 4353 && Resource.TerrainList.Terrain[message.HexNumber].Value is not null)
                    {
                        MoveAction moveAction = GetMoveAction(PlayerId.FromOriginal((sbyte)message.Param[0].Int), action: (MoveAction)message.Param[1].Int);
                        MapFieldMessageParameters param6 = message.Param;
                        param6[1] = (int)moveAction;
                        message.Param = param6;
                    }
                    else
                    {
                        if (message.MessageId == 4362 && PlayerIdExtensions.ToOriginal(Player) > 0)
                        {
                            if (PlayerIdExtensions.ToOriginal(Player) == message.Param[1].Int || StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation(PlayerId.FromOriginal((sbyte)message.Param[1].Int)) == DiplomaticRelation.Alliance)
                            {
                                int visibilityRange = VisibilityRange();
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

        if (message.MessageId == 4363 && PlayerIdExtensions.ToOriginal(Player) > 0)
        {
            if (PlayerIdExtensions.ToOriginal(Player) == message.Param[1].Int || StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation(PlayerId.FromOriginal((sbyte)message.Param[1].Int)) == DiplomaticRelation.Alliance)
            {
                int trueVisionRange = TrueVisionRange();
                if (message.Param[0].Int <= trueVisionRange)
                {
                    message.Result = 1;
                }
            }
        }
        else
        {
            if (message.MessageId == 4368 && Resource.TerrainList.Terrain[message.HexNumber].Value is not null)
            {
                FinalizeCombat(message.Param[0].As<Battle>());
            }
            else
            {
                if (message.MessageId == 4361 && Resource.TerrainList.Terrain[message.HexNumber].Value is not null)
                {
                    string combatEventText = GetCombatEventText(message.Param[0].As<Battle>());
                    message.Result.As<CombatResultQuery>().Text = combatEventText;
                }
            }
        }
    }

    // Structur.PAS:2302
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (Resource.TerrainList.Terrain[info.HexNumber].Value is not null)
        {
            if (!GetRazed() && Player is not null && ((MoveControl)info.Sender).Player != Player && info.MoveOverCode != 1)
            {
                moveResult.Code = -1;
                moveResult.Done = true;
            }
        }
    }

    // Structur.PAS:2313
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        return Resource.TerrainList.Terrain[info.HexNumber].Value is not null && (!GetRazed() || Building)
            ? Player == PlayerId.Independent ? ((MoveControl)info.Sender).Player == Player || !Resource.Claimed ? 1 : 0
                : ((MoveControl)info.Sender).Player == Player || Player is null ? 1 : 0
            : 1;
    }

    // Structur.PAS:2285
    public override void Activate()
    {
        base.Activate();
        UpdateFlagId();
        UpdateUnfog();
        RegisterVictoryLocations();
        if (Player is not null)
        {
            _ = StrategicMap.Current.Players.GetPlayers(Player).Structures.AddChild(this);
        }
    }

    // Structur.PAS:2294
    public override void Deactivate()
    {
        if (Player is not null)
        {
            _ = StrategicMap.Current.Players.GetPlayers(Player).Structures.RemoveChild(this);
        }

        UnregisterVictoryLocations();
        base.Deactivate();
        UpdateUnfog();
    }

    // Structur.PAS:2343
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (Resource.TerrainList.Terrain[info.HexNumber].Value is not null)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.ViewerFlag | MapField.MoveOverFlag | MapField.MoveOnFlag | MapField.OwnerFlag | MapField.PlayerChangeListenerFlag | MapField.AITargetFlag;
        }
    }

    // Structur.PAS:2084
    public override void NewDay()
    {
        base.NewDay();
        if (StrategicMap.Current.Day == 1 && PlayerIdExtensions.ToOriginal(Player) > 0)
        {
            UpdateExploration();
            UpdatePlayerSeen();
        }
    }

    // Structur.PAS:2078
    public override void NewTurn(PlayerId player)
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
    public override void ArmyPlaced(ArmyHexSprite? placedArmy) => UpdatePlayer();

    // Structur.PAS:2171
    public override void ArmyChanged(ArmyHexSprite? army) => UpdatePlayer();

    // Structur.PAS:2022
    public override void BuildingDone()
    {
        base.BuildingDone();
        _ = UpdatePlayer();
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
        if (GetRazed() && !Building)
        {
            return;
        }

        switch (info.Mode)
        {
            case AITargetMode.Attack:
                if (info.Player != Player && !GetRazed())
                {
                    if (Player == PlayerId.Independent || AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, targetPlayer: Player, targetRace: 0, engine: GetEngine()!))
                    {
                        int defaultAITargetPriority = DefaultAITargetPriority;
                        data.Priority = defaultAITargetPriority;
                    }
                }

                break;
            case AITargetMode.Raid:
            case AITargetMode.Recruit:
            case AITargetMode.Produce:
            case AITargetMode.Hunt:
            case AITargetMode.Fortify:
            case AITargetMode.Defend:
            case AITargetMode.GuardArea:
            case AITargetMode.AttackArmies:
            default:
                break;
        }
    }

    // Structur.PAS:2030
    public override bool CanRebuild(Troop troop, ref string reason) => base.CanRebuild(troop, ref reason);

    // Structur.PAS:2130
    public virtual void SetPlayer(PlayerId? player)
    {
        if (player != Player)
        {
            if (GetRazed() && !Building)
            {
                return;
            }

            ProductionPriority = 0;
            if (Player is not null)
            {
                _ = StrategicMap.Current.Players.GetPlayers(Player).Structures.RemoveChild(this);
            }

            if (StrategicMap.Current.Day > 0)
            {
                StructureCaptured(Player, player);
            }

            Player = player;
            UpdateUnfog();
            UpdatePlayerSeen();
            UpdateExploration();
            int hexNumberToRadius = HexGeometry.HexNumberToRadius(Resource.TerrainList.Count);
            int hexLevel = HexLevel;
            int hexY = HexY;
            int hexX = HexX;
            StrategicMap.Current.ScannerIconsChangedRadius(hexX, hexY, hexLevel, radius: hexNumberToRadius);
            UpdateFlagId();
            FogChanged();
            int hexLevel2 = HexLevel;
            int hexY2 = HexY;
            int hexX2 = HexX;
            StrategicMap.Current.NotifyPlayerChanged(hexX2, hexY2, L: hexLevel2);
            if (Player is not null)
            {
                _ = StrategicMap.Current.Players.GetPlayers(Player).Structures.AddChild(this);
            }

            if (PlayerIdExtensions.ToOriginal(Player) > 0 && StrategicMap.Current.VictoryCondition == StrategicMapVictoryCondition.CaptureLocations)
            {
                for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
                {
                    if (_victoryLocations[i])
                    {
                        if (i == PlayerIdExtensions.ToOriginal(Player) || (StrategicMap.Current.AlliedVictory && StrategicMap.Current.Players.GetDiplomaticRelation(Player, otherPlayer: PlayerId.FromOriginal((sbyte)i)) == DiplomaticRelation.Alliance))
                        {
                            StrategicMap.Current.Players.GetPlayers(i).CheckCaptureLocations();
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
        _flagId = !Status.Contains(MapObjectStatuses.EditMode)
            ? PlayerIdExtensions.ToOriginal(Player) > 0
                ? (byte)StrategicMap.Current.Players.GetPlayers(Player).PlayerRace
                : Resource.Claimed ? (byte)32 : (byte)63
            : (byte)63;
    }

    // Structur.PAS:2208
    public virtual int VisibilityRange()
    {
        int result;
        if (!Building)
        {
            result = 6;
            if (HexLevel != 0 || (PlayerIdExtensions.ToOriginal(Player) > 0 && (((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments) & 8) != 0))
            {
                return (result + 1) >>> 1;
            }
        }
        else
        {
            return 4;
        }

        return result;
    }

    // Structur.PAS:2249
    public virtual void UpdateUnfog()
    {
        if (!Status.Contains(MapObjectStatuses.PlacedOnMap))
        {
            return;
        }

        int range = Status.Contains(MapObjectStatuses.Activated) && Player is not null && StrategicMap.Current.SeatedPlayer is not null && (StrategicMap.Current.SeatedPlayer == Player || StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation(StrategicMap.Current.SeatedPlayer) == DiplomaticRelation.Alliance)
            ? VisibilityRange()
            : 0;

        if (range != UnfogRange)
        {
            int x = default;
            int y = default;
            if (UnfogRange != 0)
            {
                for (int i = 0, last = Resource.TerrainList.Count - 1; i <= last; i++)
                {
                    if (Resource.TerrainList.Terrain[i].Value is not null)
                    {
                        HexGeometry.CenterHexNumberToHex(StoredHexX, StoredHexY, hexNumber: i, ref x, ref y);
                        _ = StrategicMap.Current.FogArea(x, y, Owner.HexLevel, range: UnfogRange);
                    }
                }
            }

            UnfogRange = (byte)range;
            if (UnfogRange != 0)
            {
                for (int i = 0, last = Resource.TerrainList.Count - 1; i <= last; i++)
                {
                    if (Resource.TerrainList.Terrain[i].Value is not null)
                    {
                        HexGeometry.CenterHexNumberToHex(StoredHexX, StoredHexY, hexNumber: i, ref x, ref y);
                        _ = StrategicMap.Current.UnFogArea(x, y, Owner.HexLevel, range: UnfogRange);
                    }
                }
            }
        }
    }

    // Structur.PAS:2233
    public virtual void UpdateExploration()
    {
        if (Status.Contains(MapObjectStatuses.PlacedOnMap) && PlayerIdExtensions.ToOriginal(Player) > 0)
        {
            int x = default;
            int y = default;
            for (int player = 1, last = StrategicMap.Current.Players.Count - 1; player <= last; player++)
            {
                if (player == PlayerIdExtensions.ToOriginal(Player) || StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation(PlayerId.FromOriginal((sbyte)player)) == DiplomaticRelation.Alliance)
                {
                    for (int i = 0, lastI = Resource.TerrainList.Count - 1; i <= lastI; i++)
                    {
                        if (Resource.TerrainList.Terrain[i].Value is not null)
                        {
                            HexGeometry.CenterHexNumberToHex(StoredHexX, StoredHexY, hexNumber: i, ref x, ref y);
                            int visibilityRange = VisibilityRange();
                            _ = StrategicMap.Current.ExploreArea(x, y, Owner.HexLevel, player: PlayerId.FromOriginal((sbyte)player), range: visibilityRange);
                        }
                    }
                }
            }
        }
    }

    // Structur.PAS:2124
    // { Original leaves the result unchanged for a razed structure. }
    public virtual void FinalizeCombat(Battle? combat)
    {
        if (combat.CombatResult is CombatResult.Victory or CombatResult.NoDefenders)
        {
            SetPlayer(PlayerId.FromOriginal(combat.CombatData.Players.GetPlayers(combat.CombatData.AttackerPlayer).Player));
        }
    }

    // Structur.PAS:2115
    public virtual string GetCombatEventText(Battle? combat)
    {
        if (!GetRazed())
        {
            if (combat.CombatResult is CombatResult.Victory or CombatResult.NoDefenders)
            {
                string editName = EditName();
                return AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.StructureCapturedFormat), arguments: [editName]);
            }
            else
            {
                return "";
            }
        }

        return "";
    }

    // Structur.PAS:2094
    public virtual void StructureCaptured(PlayerId? oldPlayer, PlayerId? newPlayer)
    {
        if (!GetRazed() && !Building && oldPlayer == PlayerId.Independent && !StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) && Status.Contains(MapObjectStatuses.Activated) && !Resource.Claimed)
        {
            LocationTextEventLog log = new(null, holder: CreationHolder())
            {
                Player = newPlayer
            };
            string editName = EditName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.StructureCapturedFormat), arguments: [editName]);
            log.Location = new MapCoordinate(HexX, HexY, HexLevel);
            EventLog.DistributeLocationEventLog(y: HexY, level: HexLevel, x: HexX, log: log, player: Player, execute: false);
            _ = log.Release();
        }
    }

    // Structur.PAS:2363
    public virtual MoveAction GetMoveAction(PlayerId? player, MoveAction action)
    {
        return player != Player && !GetRazed() && (Player != PlayerId.Independent || Resource.Claimed)
            ? MoveAction.Attack
            : action;
    }

    // Structur.PAS:2349
    public virtual int DefaultAITargetPriority => 10;

    // Structur.PAS:2047
    public virtual bool UpdatePlayer()
    {
        sbyte player = -1;
        bool different = false;
        int count = TerrainCount;
        MapLevel level = Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
        while (walk.HexNumber < count && !different)
        {
            if (Resource.TerrainList.Terrain[walk.HexNumber].Value is not null && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                ArmyHexSprite army = level.GetField(walk.X, walk.Y).GetArmyHexSprite();
                if (army is not null)
                {
                    if (player != -1)
                    {
                        different = PlayerIdExtensions.ToOriginal(army.Army.Player) != player;
                    }
                    else
                    {
                        player = (sbyte)PlayerIdExtensions.ToOriginal(army.Army.Player);
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        if (player != -1)
        {
            if (different)
            {
                SetPlayer(null);
            }
            else
            {
                SetPlayer(PlayerId.FromOriginal(player));
            }
        }

        return !different;
    }

    // Structur.PAS:1986
    public bool GetVictoryLocation(PlayerId player) => _victoryLocations[player.Value];

    // Structur.PAS:1991
    public void SetVictoryLocation(PlayerId player, bool value)
    {
        if (_victoryLocations[player.Value] != value)
        {
            _victoryLocations[player.Value] = value;
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
        for (int i = 1, last = _victoryLocations.Count - 1; i <= last; i++)
        {
            if (_victoryLocations[i])
            {
                if (StrategicMap.Current.Players.GetPlayers(i) is not null)
                {
                    StrategicMap.Current.Players.GetPlayers(i).RegisterVictoryLocation(this);
                }
                else
                {
                    _victoryLocations[i] = false;
                }
            }
        }
    }

    // Structur.PAS:2013
    public void UnregisterVictoryLocations()
    {
        for (int i = 1, last = _victoryLocations.Count - 1; i <= last; i++)
        {
            if (_victoryLocations[i] && StrategicMap.Current.Players.GetPlayers(i) is not null)
            {
                StrategicMap.Current.Players.GetPlayers(i).UnregisterVictoryLocation(this);
            }
        }
    }

    // Structur.PAS:2205
    public int TrueVisionRange() => 1;

    // Structur.PAS:2220
    public void UpdatePlayerSeen()
    {
        if (Status.Contains(MapObjectStatuses.PlacedOnMap) && PlayerIdExtensions.ToOriginal(Player) > 0)
        {
            int x = default;
            int y = default;
            for (int i = 0, last = Resource.TerrainList.Count - 1; i <= last; i++)
            {
                if (Resource.TerrainList.Terrain[i].Value is not null)
                {
                    HexGeometry.CenterHexNumberToHex(StoredHexX, StoredHexY, hexNumber: i, ref x, ref y);
                    int hexLevel = HexLevel;
                    int visibilityRange = VisibilityRange();
                    int trueVisionRange = TrueVisionRange();
                    StrategicMap.Current.UpdatePlayerSeen(Player, x, y, L: hexLevel, sightRadius: visibilityRange, radius: trueVisionRange);
                }
            }
        }
    }
}
