// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

public partial class SimultaniousPlayerControl : PlayerControl
{
    public new const int ClassID = 131665;

    public SimultaniousPlayerControl(ECustomNode owner) : base(owner)
    { }

    // PlayerC.PAS:1343
    public override void NewDay()
    {
        AoWHSMap.Current.Selection.UnselectLocation();
        base.NewDay();
        for (int I = 0, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            NewTurn((sbyte)I);
        }

        AudioPlayback.PlaySample("SFX\\EVENTS\\NEWTURN.WAV");
        if (GameTables.AoWEngine.FRegistry.GetAutoSave())
        {
            AutoSave();
        }
    }

    // PlayerC.PAS:1258
    public override void NewTurn(sbyte Player)
    {
        EventLog Log;
        SpellList Spells = default;
        if (Player == AoWHSMap.Current.FSeatedPlayer)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType == PlayerType.Human)
            {
                if (AoWHSMap.Current.FDay == 1)
                {
                    if (AoWHSMap.Current.FGameType == GameType.Custom)
                    {
                        Log = new ShowBookEventLog(null);
                        AoWHSMap.Current.TriggerExecuteEventLog(Log, false, null);
                        Log.Release();
                    }

                    if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FResearchSpellID == 0)
                    {
                        Spells = new SpellList();
                        AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.ListResearchSpells(ref Spells, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
                        if (Spells.GetCount() > 0)
                        {
                            Log = new PlayerMagicEventLog(null);
                            ((PlayerMagicEventLog)Log).FMode = PlayerMagicEventLog.PlayerMagicEventMode.StartResearch;
                            AoWHSMap.Current.TriggerExecuteEventLog(Log, false, null);
                            Log.Release();
                        }

                        Spells?.Dispose();
                    }
                }
                else
                {
                    AoWHSMap.Current.ResetTurnTimer();
                    AoWHSMap.Current.EnableTurnTimer();
                }
            }
        }

        base.NewTurn(Player);
        if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatus == Session.Player.Status.Playing)
            {
                AoWHSMap.Current.FAIExecuter.ActivatePlayer(Player);
            }
        }
    }

    // PlayerC.PAS:1663
    public override void ValidateTurnTiming()
    {
        if (AoWHSMap.Current.GetTurnTimerEnabled() && AoWHSMap.Current.GetTurnTimeLeft() <= 0)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FGameOverStatus == GameOverStatus.None && !AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FSingleProcessCycle)
            {
                if (AoWHSMap.Current.FTokenManager.PlayerReady(AoWHSMap.Current.FSeatedPlayer))
                {
                    AbilityRules.EndPlayerTurn(AoWHSMap.Current.FSeatedPlayer);
                }
            }
        }
    }

    // PlayerC.PAS:1299
    public override void Setup()
    {
        base.Setup();
        AoWHSMap.Current.FTokenManager.TokenExecuter.HoldTC = false;
    }

    // PlayerC.PAS:1364
    public override void StartNewMap()
    {
        NewTurnEventLog Log = default;
        base.StartNewMap();
        AoWHSMap.Current.FShowIndependentMoves = false;
        for (int I = 0, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            if (I == 0 || AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType != PlayerType.Independent)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus = Player.Status.Playing;
            }
        }

        AoWHSMap.Current.FPlayers.Update();
        AoWHSMap.Current.FCurrentPlayer = 0;
        var item = AoWHSMap.Current.FPlayers.FLocalHumanPlayers[0];
        AoWHSMap.Current.SetSeatedPlayer((sbyte)item);
        AoWHSMap.Current.SeatedPlayerChanged();
        Assert(AoWHSMap.Current.FSeatedPlayer != 0, "", "PlayerC.PAS", 1584);
        AoWHSMap.Current.ResetSeatedPlayerView();
        AoWHSMap.Current.FTokenManager.TokenExecuter.ActivateTC(AoWHSMap.Current.FCurrentPlayer);
        Log = new NewTurnEventLog(null);
        Log.FTurnPlayer = AoWHSMap.Current.FSeatedPlayer;
        Log.FMode = NewTurnEventLog.Mode.Dialog;
        Log.FDialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
        Log.FText = AbilityRules.TranslateRStr(LetTheGameBeginRStr);
        AoWHSMap.Current.TriggerExecuteEventLog(Log, false, StartGameCallBack);
        Log.Release();
        if (AoWHSMap.Current.FConnectionType == PlayerConnectionType.Network)
        {
            AoWHSMap.Current.ResetTurnTimer();
            AoWHSMap.Current.EnableTurnTimer();
        }
        else
        {
            AoWHSMap.Current.FAIExecuter.Lock();
        }

        NewDay();
    }

    // PlayerC.PAS:1639
    public override void StartLoadedMap()
    {
        var item = AoWHSMap.Current.FPlayers.FLocalHumanPlayers[0];
        if (!IsEmpty(AoWHSMap.Current.FPlayers.GetPlayers(item).FSettings))
        {
            var fLocalHumanPlayersItem = AoWHSMap.Current.FPlayers.FLocalHumanPlayers[0];
            ValidatePassword(AoWHSMap.Current.FPlayers.GetPlayers(fLocalHumanPlayersItem).FSettings);
        }
        else
        {
            StartLoadedMapEx();
        }
    }

    // PlayerC.PAS:1636
    public override void PasswordValidated()
    {
        StartLoadedMapEx();
    }

    // PlayerC.PAS:1652
    public override void SetPlayerGameOverStatus(sbyte Player, GameOverStatus Status)
    {
        base.SetPlayerGameOverStatus(Player, Status);
        AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatus = Session.Player.Status.Terminated;
        AoWHSMap.Current.FTokenManager.TokenExecuter.GetTC(Player).Active = false;
        AoWHSMap.Current.FPlayers.Update();
        AoWHSMap.Current.TriggerPlayerTerminatedEvent(Player);
        if (AoWHSMap.Current.FPlayers.FLocalHumanPlayers.Count == 0)
        {
            AoWHSMap.Current.TriggerGameTerminatedEvent();
        }
        else
        {
            EndTurn(Player);
        }
    }

    // PlayerC.PAS:1646
    public override void GameOverEventHandled(sbyte Player)
    {
        AoWHSMap.Current.SetSeatedPlayer(-1);
        AoWHSMap.Current.TriggerGameTerminatedEvent();
    }

    // PlayerC.PAS:1305
    public override void EndTurn(sbyte Player)
    {
        int I;
        bool AllDone;
        EndTurnEventLog Log;
        EMsg Msg = default;
        AoWHSMap.Current.FPlayers.GetPlayers(Player).SetEndTurn(true);
        AllDone = true;
        I = 1;
        while (AllDone && I != AoWHSMap.Current.FPlayers.Count)
        {
            AllDone = AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus != Session.Player.Status.Playing || AoWHSMap.Current.FPlayers.GetPlayers(I).FSingleProcessCycle;
            I += 1;
        }

        if (AllDone)
        {
            if (AoWHSMap.Current.FSeatedPlayer == Player)
            {
                AoWHSMap.Current.ClearExecuteEventLogBuffer();
            }

            AoWHSMap.Current.LockRelationsChanged();
            I = 0;
            int last = AoWHSMap.Current.FPlayers.Count - 1;
            for (; I <= last; I += 1)
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus == Session.Player.Status.Playing)
                {
                    if (AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType != PlayerType.Human)
                    {
                        AoWHSMap.Current.FAIExecuter.DeactivatePlayer((sbyte)I);
                    }

                    Log = new EndTurnEventLog(null);
                    AoWHSMap.Current.FPlayers.GetPlayers(I).FEventLogbook.AddEvent(Log);
                    Log.Release();
                }

                EMsg.InitMsg(ref Msg, 537001991, this);
                Msg.Payload = I;
                AoWHSMap.Current.SendMsg(ref Msg, this);
            }

            AoWHSMap.Current.UnlockRelationsChanged();
            NewDay();
        }
    }

    // PlayerC.PAS:1354
    public void StartGameCallBack(EventLog Log)
    {
        if (AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network)
        {
            AoWHSMap.Current.FAIExecuter.Unlock();
            AoWHSMap.Current.ResetTurnTimer();
            AoWHSMap.Current.EnableTurnTimer();
        }
    }

    // PlayerC.PAS:1603
    public void StartLoadedMapEx()
    {
        NewTurnEventLog Log = default;
        Player Player = default;
        AoWHSMap.Current.FShowIndependentMoves = false;
        var item = AoWHSMap.Current.FPlayers.FLocalHumanPlayers[0];
        AoWHSMap.Current.SetSeatedPlayer((sbyte)item);
        AoWHSMap.Current.SeatedPlayerChanged();
        AoWHSMap.Current.ResetSeatedPlayerView();
        for (int I = 0, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            Player = AoWHSMap.Current.FPlayers.GetPlayers(I);
            if (Player.FStatus == Session.Player.Status.Playing && Player.FPlayerType != PlayerType.Human)
            {
                AoWHSMap.Current.FAIExecuter.ActivatePlayer((sbyte)I);
            }
        }

        for (int I = 0, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus != Session.Player.Status.Playing)
            {
                AoWHSMap.Current.FTokenManager.TokenControls[I].Active = false;
            }
        }

        AoWHSMap.Current.FTokenManager.TokenExecuter.ActivateTC(AoWHSMap.Current.FCurrentPlayer);
        Log = new NewTurnEventLog(null);
        Log.FTurnPlayer = AoWHSMap.Current.FSeatedPlayer;
        Log.FMode = NewTurnEventLog.Mode.Dialog;
        Log.FDialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
        Log.FText = AbilityRules.TranslateRStr(TheGameContinuesRStr);
        AoWHSMap.Current.TriggerExecuteEventLog(Log, false, StartGameCallBack);
        Log.Release();
        if (AoWHSMap.Current.FConnectionType == PlayerConnectionType.Network)
        {
            AoWHSMap.Current.ResetTurnTimer();
            AoWHSMap.Current.EnableTurnTimer();
        }
        else
        {
            AoWHSMap.Current.FAIExecuter.Lock();
        }
    }

    private static readonly string LetTheGameBeginRStr = "Let the game begin";
    private static readonly string TheGameContinuesRStr = "The game continues";

    // intf_P1_04.inc:26

    // intf_P1_04.inc:26

}
