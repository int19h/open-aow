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
public partial class TacticalBattle : Battle
{
    // AoWTC.pas:5081
    public new const int ClassId = 2228496;

    public TacticalBattle(ECustomNode owner) : base(owner)
    { }

    public sbyte HostPlayer { get; private set; }

    // AoWTC.pas:5086
    public override CombatUnit CreateCombatUnit() => new TacticalBattleUnit(null);

    // AoWTC.pas:5157
    public override CombatPlayer CreateCombatPlayer() => new TacticalBattlePlayer(null);

    // AoWTC.pas:5097
    public override void Initialize()
    {
        base.Initialize();
        CombatData.TurnOrder.Clear();
        CombatData.TurnOrder.Add((byte)CombatData.AttackerPlayer);
        HostPlayer = -1;
        for (int I = 0, last = AoWHSMap.Current.Players.GetCount() - 1; I <= last; I++)
        {
            if (HostPlayer == -1)
            {
                if (AoWHSMap.Current.Players.GetPlayers(I).PlayerType == PlayerType.Human)
                {
                    if (AoWHSMap.Current.Players.GetPlayers(I).Status != PlayerStatus.Terminated)
                    {
                        HostPlayer = (sbyte)I;
                    }
                }
            }
        }

        for (int I = 0, last = CombatData.Players.GetCount() - 1; I <= last; I++)
        {
            if (CombatData.TurnOrder.IndexOf((byte)I) == -1)
            {
                if (CombatData.Players.GetPlayers((sbyte)I).SideIndex == 0)
                {
                    CombatData.TurnOrder.Add((byte)I);
                }
            }

            var playerPlayer = CombatData.Players.GetPlayers((sbyte)I);
            if (AoWHSMap.Current.Players.GetPlayers(playerPlayer.Player).PlayerType != PlayerType.Human)
            {
                if (AoWHSMap.Current.PlayerCommunicator.PlayerCommunicationControl is RemotePlayerCommunicationControl)
                {
                    var playerConnectionMode = AoWHSMap.Current.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(HostPlayer);
                    var player = CombatData.Players.GetPlayers((sbyte)I);
                    ((RemotePlayerCommunicationControl)AoWHSMap.Current.PlayerCommunicator.PlayerCommunicationControl).SetConnectionMode(player.Player, playerConnectionMode.ConnectionMode);
                    var playerSystemId = AoWHSMap.Current.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(HostPlayer);
                    var player2 = CombatData.Players.GetPlayers((sbyte)I);
                    ((RemotePlayerCommunicationControl)AoWHSMap.Current.PlayerCommunicator.PlayerCommunicationControl).SetSystemId(player2.Player, playerSystemId.SystemId);
                }
            }
        }

        for (int I = 0, last = AoWHSMap.Current.TokenManager.GetCount() - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.TokenManager.GetTokenControl(I).Local() || AoWHSMap.Current.TokenManager.GetTokenControl(I).Global())
            {
                AoWHSMap.Current.TokenManager.GetTokenControl(I).ClearEvents();
            }
        }

        CombatData.TurnOrder.Add((byte)CombatData.DefenderPlayer);
        for (int I = 0, last = CombatData.Players.GetCount() - 1; I <= last; I++)
        {
            if (CombatData.TurnOrder.IndexOf((byte)I) == -1)
            {
                if (CombatData.Players.GetPlayers((sbyte)I).SideIndex == 1)
                {
                    CombatData.TurnOrder.Add((byte)I);
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
        int I;
        CombatSide side;
        side = Sides[(int)CombatSideIndex.Attacker];
        bool result = CombatData.Parties.GetParty(0).Wall == 0;
        I = side.GetCount();
        while (!result && I > 0)
        {
            I -= 1;
            result = (side.GetObjects(I).WallCombatFeatures() & Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.WallPassing)) != Set<WallCombatFeaturesElement>.Of();
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
        CombatResult result = default;
        RandomBattleMapGenerator generator = default;
        int seated = default;
        if (!ValidateWallCombat())
        {
            SetLogCombatActions(1);
            SetLogbookHeader(AbilityRules.TranslateResourceString(GameText.CannotBreachWallText));
            CombatResult = CombatResult.CannotBreachWall;
            result = CombatResult;
        }
        else
        {
            if (Sides[(int)CombatSideIndex.Defender].GetCount() == Sides[(int)CombatSideIndex.Defender].ConquerCount)
            {
                SetLogCombatActions(1);
                SetLogbookHeader(AbilityRules.TranslateResourceString(GameText.CannotInflictDamageText));
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
                    AoWHSMap.Current.TokenManager.TokenExecutor.HoldTacticalBattle = true;
                    generator = new RandomBattleMapGenerator();
                    generator.RandomMap(this, CombatData.X, CombatData.Y, CombatData.MapLevel);
                    generator?.Dispose();
                    bool condition = AoWHSMap.Current.StartingGame || AoWHSMap.Current.VisibleForPlayer(CombatData.X, CombatData.Y, CombatData.MapLevel, AoWHSMap.Current.SeatedPlayer);
                    CombatMap.Current.PlaySounds = condition;
                    if (!CombatMap.Current.PlaySounds)
                    {
                        for (int I = 0, last = CombatData.Players.GetCount() - 1; I <= last; I++)
                        {
                            var playerPlayer = CombatData.Players.GetPlayers((sbyte)I);
                            if (AoWHSMap.Current.Players.GetPlayers(playerPlayer.Player).PlayerType == PlayerType.Human)
                            {
                                var player = CombatData.Players.GetPlayers((sbyte)I);
                                if (AoWHSMap.Current.Players.GetPlayers(player.Player).GetLocal())
                                {
                                    CombatMap.Current.PlaySounds = true;
                                }
                            }
                        }
                    }

                    CombatMap.Current.DeselectEventsStore.Raise(this);
                    seated = -1;
                    for (int I = 0, last = CombatData.Players.GetCount() - 1; I <= last; I++)
                    {
                        if (seated == -1)
                        {
                            var playerPlayer = CombatData.Players.GetPlayers((sbyte)I);
                            if (AoWHSMap.Current.Players.GetPlayers(playerPlayer.Player).PlayerType == PlayerType.Human)
                            {
                                var player = CombatData.Players.GetPlayers((sbyte)I);
                                if (AoWHSMap.Current.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(player.Player).ConnectionMode != ConnectionMode.Remote)
                                {
                                    var player2 = CombatData.Players.GetPlayers((sbyte)I);
                                    if (AoWHSMap.Current.Players.GetPlayers(player2.Player).Status != PlayerStatus.Terminated)
                                    {
                                        seated = I;
                                    }
                                }
                            }
                        }
                    }

                    CombatMap.Current.SetSeatedCombatPlayer((sbyte)seated);
                    CombatMap.Current.CenterViewTo(32, 29, 0);
                    CombatMap.Current.Combat.NewRound();
                    var turnOrder = CombatData.TurnOrder[0];
                    CombatMap.Current.NewTurn((sbyte)turnOrder);
                    result = CombatResult.Busy;
                    OnTerminated = _ => CombatMap.Current.EndCombat();
                    SetLogCombatActions(1);
                }
            }
        }

        FrameFixtureInputs.TacticalInitialized?.Invoke();
        return result;
    }
}
