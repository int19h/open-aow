#nullable disable
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
public partial class TacticalBattle(CustomNode owner) : Battle(owner)
{
    // AoWTC.pas:5081
    public new const int ClassId = 2228496;

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
        _ = CombatData.TurnOrder.Add((byte)CombatData.AttackerPlayer);
        HostPlayer = -1;
        for (int i = 0, last = StrategicMap.Current.Players.GetCount() - 1; i <= last; i++)
        {
            if (HostPlayer == -1)
            {
                if (StrategicMap.Current.Players.GetPlayers(i).PlayerType == PlayerType.Human)
                {
                    if (StrategicMap.Current.Players.GetPlayers(i).Status != PlayerStatus.Terminated)
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
                    _ = CombatData.TurnOrder.Add((byte)i);
                }
            }

            CombatPlayer playerPlayer = CombatData.Players.GetPlayers((sbyte)i);
            if (StrategicMap.Current.Players.GetPlayers(playerPlayer.Player).PlayerType != PlayerType.Human)
            {
                if (StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl is RemotePlayerCommunicationControl remote)
                {
                    PlayerCommunicationSettings playerConnectionMode = StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(HostPlayer);
                    CombatPlayer player = CombatData.Players.GetPlayers((sbyte)i);
                    remote.SetConnectionMode(player.Player, playerConnectionMode.ConnectionMode);
                    PlayerCommunicationSettings playerSystemId = StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(HostPlayer);
                    CombatPlayer player2 = CombatData.Players.GetPlayers((sbyte)i);
                    remote.SetSystemId(player2.Player, playerSystemId.SystemId);
                }
            }
        }

        for (int i = 0, last = StrategicMap.Current.TokenManager.GetCount() - 1; i <= last; i++)
        {
            if (StrategicMap.Current.TokenManager.GetTokenControl(i).Local() || StrategicMap.Current.TokenManager.GetTokenControl(i).Global())
            {
                StrategicMap.Current.TokenManager.GetTokenControl(i).ClearEvents();
            }
        }

        _ = CombatData.TurnOrder.Add((byte)CombatData.DefenderPlayer);
        for (int i = 0, last = CombatData.Players.GetCount() - 1; i <= last; i++)
        {
            if (CombatData.TurnOrder.IndexOf((byte)i) == -1)
            {
                if (CombatData.Players.GetPlayers((sbyte)i).SideIndex == 1)
                {
                    _ = CombatData.TurnOrder.Add((byte)i);
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
        int i = side.GetCount();
        while (!result && i > 0)
        {
            i -= 1;
            result = (side.GetObjects(i).WallCombatFeatures() & Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.WallPassing)) != Set<WallCombatFeaturesElement>.Of();
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
            SetLogbookHeader(AbilityRules.TranslateResourceString(GetEngine(), GameText.CannotBreachWallText));
            CombatResult = CombatResult.CannotBreachWall;
            result = CombatResult;
        }
        else
        {
            if (Sides[(int)CombatSideIndex.Defender].GetCount() == Sides[(int)CombatSideIndex.Defender].ConquerCount)
            {
                SetLogCombatActions(1);
                SetLogbookHeader(AbilityRules.TranslateResourceString(GetEngine(), GameText.CannotInflictDamageText));
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
                    StrategicMap.Current.TokenManager.TokenExecutor.HoldTacticalBattle = true;
                    RandomBattleMapGenerator generator = new(this);
                    _ = generator.RandomMap(this, CombatData.Location.X, CombatData.Location.Y, CombatData.Location.MapLevel);
                    generator?.Dispose();
                    bool condition = StrategicMap.Current.StartingGame || StrategicMap.Current.VisibleForPlayer(CombatData.Location.X, CombatData.Location.Y, CombatData.Location.MapLevel, StrategicMap.Current.SeatedPlayer);
                    CombatMap.Current.PlaySounds = condition;
                    if (!CombatMap.Current.PlaySounds)
                    {
                        for (int i = 0, last = CombatData.Players.GetCount() - 1; i <= last; i++)
                        {
                            CombatPlayer playerPlayer = CombatData.Players.GetPlayers((sbyte)i);
                            if (StrategicMap.Current.Players.GetPlayers(playerPlayer.Player).PlayerType == PlayerType.Human)
                            {
                                CombatPlayer player = CombatData.Players.GetPlayers((sbyte)i);
                                if (StrategicMap.Current.Players.GetPlayers(player.Player).GetLocal())
                                {
                                    CombatMap.Current.PlaySounds = true;
                                }
                            }
                        }
                    }

                    CombatMap.Current.DeselectEventsStore.Raise(this);
                    int seated = -1;
                    for (int i = 0, last = CombatData.Players.GetCount() - 1; i <= last; i++)
                    {
                        if (seated == -1)
                        {
                            CombatPlayer playerPlayer = CombatData.Players.GetPlayers((sbyte)i);
                            if (StrategicMap.Current.Players.GetPlayers(playerPlayer.Player).PlayerType == PlayerType.Human)
                            {
                                CombatPlayer player = CombatData.Players.GetPlayers((sbyte)i);
                                if (StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(player.Player).ConnectionMode != ConnectionMode.Remote)
                                {
                                    CombatPlayer player2 = CombatData.Players.GetPlayers((sbyte)i);
                                    if (StrategicMap.Current.Players.GetPlayers(player2.Player).Status != PlayerStatus.Terminated)
                                    {
                                        seated = i;
                                    }
                                }
                            }
                        }
                    }

                    CombatMap.Current.SetSeatedCombatPlayer((sbyte)seated);
                    CombatMap.Current.CenterViewTo(32, 29, 0);
                    CombatMap.Current.Combat.NewRound();
                    byte turnOrder = CombatData.TurnOrder[0];
                    CombatMap.Current.NewTurn((sbyte)turnOrder);
                    result = CombatResult.Busy;
                    SetTerminatedHandler(_ => CombatMap.Current.EndCombat());
                    SetLogCombatActions(1);
                }
            }
        }

        FrameFixtureInputs.TacticalInitialized?.Invoke();
        return result;
    }
}
