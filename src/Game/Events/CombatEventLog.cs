// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Audio;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// { }
// Logbook
public partial class CombatEventLog : EventLog
{
    public CombatEventLog(ECustomNode owner) : base(owner)
    { }

    // Combat2.pas:413
    public new const int ClassId = 131810;

    public CombatLogbook Logbook { get; private set; }

    // Combat2.pas:405
    public override void Dispose()
    {
        CombatLogbook L;
        L = Logbook;
        if (L != null)
        {
            L.Release();
            Logbook = null;
        }

        base.Dispose();
    }

    // Combat2.pas:428
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Logbook = persister.Created("Logbook", objectValue: Logbook);
    }

    // Combat2.pas:461
    public override string GetText() => Logbook.FinalData.Header;

    // Combat2.pas:431
    public override int GetTextColor()
    {
        int result;
        CombatData data;
        data = Logbook.FinalData;
        if (data.Players.GetPlayers(data.AttackerPlayer).Player != 0)
        {
            var players = data.Players.GetPlayers(data.AttackerPlayer);
            var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(players.Player);
            result = AbilityRules.RaceColors[fPlayersPlayers.PlayerRace];
        }
        else
        {
            result = AbilityRules.RaceColors[12];
        }

        return result;
    }

    // Combat2.pas:440
    public override int GetLeftIcon()
    {
        int result;
        CombatData data;
        data = Logbook.FinalData;
        if (data.Players.GetPlayers(data.AttackerPlayer).Player != 0)
        {
            var players = data.Players.GetPlayers(data.AttackerPlayer);
            result = StrategicMap.Current.Players.GetPlayers(players.Player).PlayerRace;
        }
        else
        {
            result = 12;
        }

        return result;
    }

    // Combat2.pas:449
    public override int GetMiddleIcon() => 13;

    // Combat2.pas:452
    public override int GetRightIcon()
    {
        int result;
        CombatData data;
        data = Logbook.FinalData;
        if (data.Players.GetPlayers(data.DefenderPlayer).Player != 0)
        {
            var players = data.Players.GetPlayers(data.DefenderPlayer);
            result = StrategicMap.Current.Players.GetPlayers(players.Player).PlayerRace;
        }
        else
        {
            result = 12;
        }

        return result;
    }

    // Combat2.pas:467
    public override void PlayLoggedSfx(EventLogbook logbook)
    {
        if (Logbook.InitialData.Players.IndexOfPlayer((byte)StrategicMap.Current.SeatedPlayer) != -1)
        {
            AudioPlayback.PlaySample("SFX\\EVENTS\\ROLL.WAV");
        }
        else
        {
            AudioPlayback.PlaySample("SFX\\EVENTS\\NEWEVENT.WAV");
        }
    }

    // Combat2.pas:474
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool showCombat = default;
        CombatData data = default;
        bool result = false;
        switch (trigger)
        {
            case ExecuteTrigger.User:
                StrategicMap.Current.TriggerExecuteEventLog(this, false, done);
                result = true;
                break;
            case ExecuteTrigger.Event:
            case ExecuteTrigger.NewTurn:
                if (Logbook.InitialData.Players.IndexOfPlayer((byte)StrategicMap.Current.SeatedPlayer) != -1)
                {
                    data = Logbook.InitialData;
                    showCombat = data.Players.GetPlayers(data.AttackerPlayer).Player == StrategicMap.Current.SeatedPlayer;
                    showCombat = showCombat || StrategicMap.Current.RequestCombatType && StrategicMap.Current.ConnectionType == PlayerConnectionType.Email && Logbook.InitialData.Players.GetPlayers(Logbook.InitialData.DefenderPlayer).Player == StrategicMap.Current.SeatedPlayer;
                    showCombat = showCombat || trigger == ExecuteTrigger.Event && StrategicMap.Current.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Combat);
                    showCombat = showCombat || trigger == ExecuteTrigger.Event && StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Combat);
                    showCombat = showCombat && Logbook.CombatResult != CombatResult.NoDefenders;
                }
                else
                {
                    showCombat = false;
                }

                showCombat = showCombat || Logbook.PlayerEliminated;
                if (showCombat)
                {
                    StrategicMap.Current.TriggerExecuteEventLog(this, false, done);
                    result = true;
                }
                else
                {
                    result = false;
                }

                break;
        }

        return result;
    }

    // Combat2.pas:416
    public void SetCombatLogbook(CombatLogbook logbook)
    {
        CombatLogbook old;
        old = Logbook;
        if (old != logbook)
        {
            if (old != null)
            {
                old.Release();
            }

            Logbook = logbook;
            if (Logbook != null)
            {
                Logbook.AddRef();
            }
        }
    }

    public override void ViewLocation()
    {
        var data = Logbook.FinalData;
        CenterViewTo((sbyte)data.X, (sbyte)data.Y, (sbyte)data.MapLevel);
    }
}
