#nullable disable warnings
// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Host;
using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Combat;

// Inherited combat layout. HostPlayer
public sealed partial class TacticalBattle(CustomNode? owner) : Battle(owner)
{
    // AoWTC.pas:5081
    public new const int ClassId = 2_228_496;

    public sbyte HostPlayer { get; private set; }

    // AoWTC.pas:5086
    public override CombatUnit CreateCombatUnit() => new TacticalBattleUnit(null) { Holder = CreationHolder() };

    // AoWTC.pas:5157
    public override CombatPlayer CreateCombatPlayer() => new TacticalBattlePlayer(null) { Holder = CreationHolder() };

    // AoWTC.pas:5097
    public override void Initialize()
    {
        base.Initialize();
        CombatData.TurnOrder.Clear();
        _ = CombatData.TurnOrder.Add(CombatData.AttackerPlayer.TruncateToByte());
        HostPlayer = -1;
        StrategicMap strategicMap = GetEngine().StrategicMap;
        for (int i = 0, last = strategicMap.Players.GetCount() - 1; i <= last; i++)
        {
            if (HostPlayer == -1)
            {
                if (strategicMap.Players.GetPlayers(i).PlayerType == PlayerType.Human)
                {
                    if (strategicMap.Players.GetPlayers(i).Status != PlayerStatus.Terminated)
                    {
                        HostPlayer = (sbyte)i;
                    }
                }
            }
        }

        for (int i = 0, last = CombatData.Players.GetCount() - 1; i <= last; i++)
        {
            if (CombatData.TurnOrder.IndexOf((byte)i) == -1)
            {
                if (CombatData.Players.GetPlayers((sbyte)i).SideIndex == 0)
                {
                    _ = CombatData.TurnOrder.Add(i.TruncateToByte());
                }
            }

            CombatPlayer playerPlayer = CombatData.Players.GetPlayers((sbyte)i);
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            if (engineStrategicMap.Players.GetPlayers(playerPlayer.Player).PlayerType != PlayerType.Human)
            {
                if (engineStrategicMap.PlayerCommunicator.PlayerCommunicationControl is RemotePlayerCommunicationControl remote)
                {
                    PlayerCommunicationSettings? playerConnectionMode = engineStrategicMap.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(HostPlayer);
                    CombatPlayer player = CombatData.Players.GetPlayers((sbyte)i);
                    remote.SetConnectionMode(player.Player, mode: playerConnectionMode.ConnectionMode);
                    PlayerCommunicationSettings? playerSystemId = engineStrategicMap.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(HostPlayer);
                    CombatPlayer player2 = CombatData.Players.GetPlayers((sbyte)i);
                    remote.SetSystemId(player2.Player, playerSystemId.SystemId);
                }
            }
        }

        for (int i = 0, last = GetEngine().StrategicMap.TokenManager.GetCount() - 1; i <= last; i++)
        {
            StrategicMap localStrategicMap = GetEngine().StrategicMap;
            if (localStrategicMap.TokenManager.GetTokenControl(PlayerId.FromOriginal(i)).Local() || localStrategicMap.TokenManager.GetTokenControl(PlayerId.FromOriginal(i)).Global())
            {
                localStrategicMap.TokenManager.GetTokenControl(PlayerId.FromOriginal(i)).ClearEvents();
            }
        }

        _ = CombatData.TurnOrder.Add(CombatData.DefenderPlayer.TruncateToByte());
        for (int i = 0, last = CombatData.Players.GetCount() - 1; i <= last; i++)
        {
            if (CombatData.TurnOrder.IndexOf((byte)i) == -1)
            {
                if (CombatData.Players.GetPlayers((sbyte)i).SideIndex == 1)
                {
                    _ = CombatData.TurnOrder.Add(i.TruncateToByte());
                }
            }
        }

        SetLogCombatActions(1);
    }

    // AoWTC.pas:5091
    public override void Deactivate()
    {
        if (CombatResult != CombatResult.CannotBreachWall)
        {
            UpdateStatus();
        }

        base.Deactivate();
    }

    // AoWTC.pas:5145
    public bool ValidateWallCombat()
    {
        CombatSide side = Sides[(int)CombatSideIndex.Attacker];
        bool result = CombatData.Parties.GetParty(0).Wall == 0;
        if (!result)
        {
            result = side.FindLastIndex(static combatObject => (combatObject.WallCombatFeatures() & (WallCombatFeatures.WallBreaching | WallCombatFeatures.WallPassing)) != WallCombatFeatures.None) != -1;
        }

        return result;
    }

    public override CombatResult Execute()
    {
        if (HeadlessHooks.Tactical || FrameFixtureInputs.Tactical)
        {
            HostPlayer = 0;
            HeadlessHooks.TacticalExecutions++;
        }
        CombatResult result;
        if (!ValidateWallCombat())
        {
            SetLogCombatActions(1);
            SetLogbookHeader(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CannotBreachWallText));
            CombatResult = CombatResult.CannotBreachWall;
            result = CombatResult;
        }
        else
        {
            if (Sides[(int)CombatSideIndex.Defender].GetCount() == Sides[(int)CombatSideIndex.Defender].ConquerCount)
            {
                SetLogCombatActions(1);
                SetLogbookHeader(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CannotInflictDamageText));
                CombatResult = CombatResult.CannotInflictDamage;
                result = CombatResult;
            }
            else
            {
                if (Sides[(int)CombatSideIndex.Defender].GetCount() == 0)
                {
                    result = CombatResult.NoDefenders;
                }
                else
                {
                    GetEngine().StrategicMap.TokenManager.TokenExecutor.HoldTacticalBattle = true;
                    RandomBattleMapGenerator generator = new(this);
                    _ = generator.RandomMap(this, CombatData.Location.X, CombatData.Location.Y, L: CombatData.Location.MapLevel);
                    StrategicMap strategicMap = GetEngine().StrategicMap;
                    bool condition = strategicMap.StartingGame || strategicMap.VisibleForPlayer(CombatData.Location.X, CombatData.Location.Y, CombatData.Location.MapLevel, player: strategicMap.SeatedPlayer);
                    CombatMap combatMap = GetEngine().CombatMap;
                    combatMap.PlaySounds = condition;
                    if (!combatMap.PlaySounds)
                    {
                        for (int i = 0, last = CombatData.Players.GetCount() - 1; i <= last; i++)
                        {
                            CombatPlayer playerPlayer = CombatData.Players.GetPlayers((sbyte)i);
                            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                            if (engineStrategicMap.Players.GetPlayers(playerPlayer.Player).PlayerType == PlayerType.Human)
                            {
                                CombatPlayer player = CombatData.Players.GetPlayers((sbyte)i);
                                if (engineStrategicMap.Players.GetPlayers(player.Player).GetLocal())
                                {
                                    combatMap.PlaySounds = true;
                                }
                            }
                        }
                    }

                    combatMap.OnBattleStarted(this);
                    int seated = -1;
                    for (int i = 0, last = CombatData.Players.GetCount() - 1; i <= last; i++)
                    {
                        if (seated == -1)
                        {
                            CombatPlayer playerPlayer = CombatData.Players.GetPlayers((sbyte)i);
                            StrategicMap getPlayersStrategicMap = GetEngine().StrategicMap;
                            if (getPlayersStrategicMap.Players.GetPlayers(playerPlayer.Player).PlayerType == PlayerType.Human)
                            {
                                CombatPlayer player = CombatData.Players.GetPlayers((sbyte)i);
                                if (getPlayersStrategicMap.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(player.Player).ConnectionMode != ConnectionMode.Remote)
                                {
                                    CombatPlayer player2 = CombatData.Players.GetPlayers((sbyte)i);
                                    if (getPlayersStrategicMap.Players.GetPlayers(player2.Player).Status != PlayerStatus.Terminated)
                                    {
                                        seated = i;
                                    }
                                }
                            }
                        }
                    }

                    GetEngine().CombatMap.SetSeatedCombatPlayer((sbyte)seated);
                    GetEngine().CombatMap.CenterViewTo(32, 29, 0);
                    GetEngine().CombatMap.Combat.NewRound();
                    int turnOrder = CombatData.TurnOrder[0];
                    GetEngine().CombatMap.NewTurn((sbyte)turnOrder);
                    result = CombatResult.Busy;
                    SetTerminatedHandler(_ => GetEngine().CombatMap.EndCombat());
                    SetLogCombatActions(1);
                }
            }
        }

        FrameFixtureInputs.TacticalInitialized?.Invoke();
        return result;
    }
}
