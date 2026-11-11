// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Host;
using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat;

// Inherited combat layout. FHostPlayer
public partial class TacticalBattle : Battle
{
    // AoWTC.pas:5081
    public new const int ClassID = 2228496;

    public TacticalBattle(ECustomNode owner) : base(owner)
    { }

    public sbyte FHostPlayer;

    // AoWTC.pas:5086
    public override CombatUnit CreateCombatUnit() => new TacticalBattleUnit(null);

    // AoWTC.pas:5157
    public override CombatPlayer CreateCombatPlayer() => new TacticalBattlePlayer(null);

    // AoWTC.pas:5097
    public override void Initialize()
    {
        base.Initialize();
        FCombatData.FTurnOrder.Clear();
        FCombatData.FTurnOrder.Add((byte)FCombatData.FAttackerPlayer);
        FHostPlayer = -1;
        for (int I = 0, last = AoWHSMap.Current.FPlayers.GetCount() - 1; I <= last; I++)
        {
            if (FHostPlayer == -1)
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType == PlayerType.Human)
                {
                    if (AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus != Player.Status.Terminated)
                    {
                        FHostPlayer = (sbyte)I;
                    }
                }
            }
        }

        for (int I = 0, last = FCombatData.FPlayers.GetCount() - 1; I <= last; I++)
        {
            if (FCombatData.FTurnOrder.IndexOf((byte)I) == -1)
            {
                if (FCombatData.FPlayers.GetPlayers((sbyte)I).FSideIndex == 0)
                {
                    FCombatData.FTurnOrder.Add((byte)I);
                }
            }

            var playerPlayer = FCombatData.FPlayers.GetPlayers((sbyte)I);
            if (AoWHSMap.Current.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType != PlayerType.Human)
            {
                if (AoWHSMap.Current.FPlayerCommunicator.PCC is RemotePlayerCommunicationControl)
                {
                    var playerConnectionMode = AoWHSMap.Current.FPlayerCommunicator.PCC.GetPlayers(FHostPlayer);
                    var player = FCombatData.FPlayers.GetPlayers((sbyte)I);
                    ((RemotePlayerCommunicationControl)AoWHSMap.Current.FPlayerCommunicator.PCC).SetConnectionMode(player.FPlayer, playerConnectionMode.ConnectionMode);
                    var playerSystemID = AoWHSMap.Current.FPlayerCommunicator.PCC.GetPlayers(FHostPlayer);
                    var player2 = FCombatData.FPlayers.GetPlayers((sbyte)I);
                    ((RemotePlayerCommunicationControl)AoWHSMap.Current.FPlayerCommunicator.PCC).SetSystemID(player2.FPlayer, playerSystemID.SystemID);
                }
            }
        }

        for (int I = 0, last = AoWHSMap.Current.FTokenManager.GetCount() - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.FTokenManager.GetTokenControl(I).Local() || AoWHSMap.Current.FTokenManager.GetTokenControl(I).Global())
            {
                AoWHSMap.Current.FTokenManager.GetTokenControl(I).ClearEvents();
            }
        }

        FCombatData.FTurnOrder.Add((byte)FCombatData.FDefenderPlayer);
        for (int I = 0, last = FCombatData.FPlayers.GetCount() - 1; I <= last; I++)
        {
            if (FCombatData.FTurnOrder.IndexOf((byte)I) == -1)
            {
                if (FCombatData.FPlayers.GetPlayers((sbyte)I).FSideIndex == 1)
                {
                    FCombatData.FTurnOrder.Add((byte)I);
                }
            }
        }

        SetLogCombatActions(1);
    }

    // AoWTC.pas:5091
    public override void Deactivate()
    {
        if (FCombatResult != CombatResult.CannotBreachWall)
        {
            UpdateStatus();
        }

        base.Deactivate();
    }

    // AoWTC.pas:5145
    public bool ValidateWallCombat()
    {
        int I;
        CombatSide Side;
        Side = FSides[(int)CombatSideIndex.Attacker];
        bool Result = FCombatData.FParties.GetParty(0).FWall == 0;
        I = Side.GetCount();
        while (!Result && I > 0)
        {
            I -= 1;
            Result = (Side.GetObjects(I).WallCombatFeatures() & Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.WallPassing)) != Set<WallCombatFeaturesElement>.Of();
        }

        return Result;
    }

    public override CombatResult Execute()
    {
        if (HeadlessHooks.Tactical || FrameFixtureInputs.Tactical)
        {
            FHostPlayer = 0;
            HeadlessHooks.TacticalExecutions++;
        }
        CombatResult Result = default;
        RandomBattleMapGenerator Generator = default;
        int Seated = default;
        if (!ValidateWallCombat())
        {
            SetLogCombatActions(1);
            SetLogbookHeader(AbilityRules.TranslateRStr(GameText.CannotBreachWallRStr));
            FCombatResult = CombatResult.CannotBreachWall;
            Result = FCombatResult;
        }
        else
        {
            if (FSides[(int)CombatSideIndex.Defender].GetCount() == FSides[(int)CombatSideIndex.Defender].FConquerCount)
            {
                SetLogCombatActions(1);
                SetLogbookHeader(AbilityRules.TranslateRStr(GameText.CannotInflictDamageRStr));
                FCombatResult = CombatResult.CannotInflictDamage;
                Result = FCombatResult;
            }
            else
            {
                if (FSides[(int)CombatSideIndex.Defender].GetCount() == 0)
                {
                    Result = CombatResult.NoDefenders;
                }
                else
                {
                    AoWHSMap.Current.FTokenManager.TokenExecuter.HoldTC = true;
                    Generator = new RandomBattleMapGenerator();
                    Generator.RandomMap(this, FCombatData.FX, FCombatData.FY, FCombatData.FL);
                    Generator?.Dispose();
                    bool condition = AoWHSMap.Current.FStartingGame || AoWHSMap.Current.VisibleForPlayer(FCombatData.FX, FCombatData.FY, FCombatData.FL, AoWHSMap.Current.FSeatedPlayer);
                    CombatMap.Current.FPlaySounds = condition;
                    if (!CombatMap.Current.FPlaySounds)
                    {
                        for (int I = 0, last = FCombatData.FPlayers.GetCount() - 1; I <= last; I++)
                        {
                            var playerPlayer = FCombatData.FPlayers.GetPlayers((sbyte)I);
                            if (AoWHSMap.Current.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType == PlayerType.Human)
                            {
                                var player = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                if (AoWHSMap.Current.FPlayers.GetPlayers(player.FPlayer).GetLocal())
                                {
                                    CombatMap.Current.FPlaySounds = true;
                                }
                            }
                        }
                    }

                    CombatMap.Current.FDeselectEventsStore.Raise(this);
                    Seated = -1;
                    for (int I = 0, last = FCombatData.FPlayers.GetCount() - 1; I <= last; I++)
                    {
                        if (Seated == -1)
                        {
                            var playerPlayer = FCombatData.FPlayers.GetPlayers((sbyte)I);
                            if (AoWHSMap.Current.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType == PlayerType.Human)
                            {
                                var player = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                if (AoWHSMap.Current.FPlayerCommunicator.PCC.GetPlayers(player.FPlayer).ConnectionMode != ConnectionMode.Remote)
                                {
                                    var player2 = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                    if (AoWHSMap.Current.FPlayers.GetPlayers(player2.FPlayer).FStatus != Player.Status.Terminated)
                                    {
                                        Seated = I;
                                    }
                                }
                            }
                        }
                    }

                    CombatMap.Current.SetSeatedCombatPlayer((sbyte)Seated);
                    CombatMap.Current.CenterViewTo(32, 29, 0);
                    CombatMap.Current.FCombat.NewRound();
                    var turnOrder = FCombatData.FTurnOrder[0];
                    CombatMap.Current.NewTurn((sbyte)turnOrder);
                    Result = CombatResult.Busy;
                    FOnTerminated = _ => CombatMap.Current.EndCombat();
                    SetLogCombatActions(1);
                }
            }
        }

        FrameFixtureInputs.TacticalInitialized?.Invoke();
        return Result;
    }
}
