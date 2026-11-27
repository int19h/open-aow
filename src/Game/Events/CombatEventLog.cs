#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Audio;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Graphics;

namespace Aow.Game.Events;

// Logbook
public partial class CombatEventLog(CustomNode? owner) : EventLog(owner)
{

    // Combat2.pas:413
    public new const int ClassId = 131_810;

    public CombatLogbook? Logbook { get; private set; }

    // Combat2.pas:405
    public override void Dispose()
    {
        CombatLogbook? l = Logbook;
        if (l is not null)
        {
            _ = l.Release();
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
    public override Color GetTextColor()
    {
        CombatData data = Logbook.FinalData;
        if (data.Players.GetPlayers(data.AttackerPlayer).Player != 0)
        {
            CombatPlayer players = data.Players.GetPlayers(data.AttackerPlayer);
            Player? fPlayersPlayers = GetEngine().StrategicMap.Players.GetPlayers(players.Player);
            return AbilityRules.RaceColors[fPlayersPlayers.PlayerRace];
        }
        else
        {
            return AbilityRules.RaceColors[12];
        }
    }

    // Combat2.pas:440
    public override int LeftIcon
    {
        get
        {
            CombatData data = Logbook.FinalData;
            if (data.Players.GetPlayers(data.AttackerPlayer).Player != 0)
            {
                CombatPlayer players = data.Players.GetPlayers(data.AttackerPlayer);
                return GetEngine().StrategicMap.Players.GetPlayers(players.Player).PlayerRace;
            }
            else
            {
                return 12;
            }
        }
    }

    // Combat2.pas:449
    public override int MiddleIcon => 13;

    // Combat2.pas:452
    public override int RightIcon
    {
        get
        {
            CombatData data = Logbook.FinalData;
            if (data.Players.GetPlayers(data.DefenderPlayer).Player != 0)
            {
                CombatPlayer players = data.Players.GetPlayers(data.DefenderPlayer);
                return GetEngine().StrategicMap.Players.GetPlayers(players.Player).PlayerRace;
            }
            else
            {
                return 12;
            }
        }
    }

    // Combat2.pas:467
    public override void PlayLoggedSfx(EventLogbook logbook)
    {
        _ = Logbook.InitialData.Players.IndexOfPlayer((byte)PlayerIdExtensions.ToOriginal(GetEngine().StrategicMap.SeatedPlayer)) != -1
            ? AudioPlayback.PlaySample("SFX\\EVENTS\\ROLL.WAV")
            : AudioPlayback.PlaySample("SFX\\EVENTS\\NEWEVENT.WAV");
    }

    // Combat2.pas:474
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent? done)
    {
        switch (trigger)
        {
            case ExecuteTrigger.User:
                GetEngine().StrategicMap.TriggerExecuteEventLog(this, immediate: false, done);
                return true;
            case ExecuteTrigger.Event:
            case ExecuteTrigger.NewTurn:
                bool showCombat;
                if (Logbook.InitialData.Players.IndexOfPlayer((byte)PlayerIdExtensions.ToOriginal(GetEngine().StrategicMap.SeatedPlayer)) != -1)
                {
                    CombatData data = Logbook.InitialData;
                    StrategicMap strategicMap = GetEngine().StrategicMap;
                    showCombat = data.Players.GetPlayers(data.AttackerPlayer).Player == PlayerIdExtensions.ToOriginal(strategicMap.SeatedPlayer);
                    showCombat = showCombat || (strategicMap.RequestCombatType && strategicMap.ConnectionType == PlayerConnectionType.Email && Logbook.InitialData.Players.GetPlayers(Logbook.InitialData.DefenderPlayer).Player == PlayerIdExtensions.ToOriginal(strategicMap.SeatedPlayer));
                    showCombat = showCombat || (trigger == ExecuteTrigger.Event && strategicMap.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvents.Combat));
                    showCombat = showCombat || (trigger == ExecuteTrigger.Event && strategicMap.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvents.Combat));
                    showCombat = showCombat && Logbook.CombatResult != CombatResult.NoDefenders;
                }
                else
                {
                    showCombat = false;
                }

                showCombat = showCombat || Logbook.PlayerEliminated;
                if (showCombat)
                {
                    GetEngine().StrategicMap.TriggerExecuteEventLog(this, immediate: false, done);
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                break;
        }

        return false;
    }

    // Combat2.pas:416
    public void SetCombatLogbook(CombatLogbook logbook)
    {
        CombatLogbook? old = Logbook;
        if (old != logbook)
        {
            _ = (old?.Release());

            Logbook = logbook;
            _ = (Logbook?.AddRef());
        }
    }

    public override void ViewLocation()
    {
        CombatData data = Logbook.FinalData;
        CenterViewTo(data.Location.X, data.Location.Y, L: data.Location.MapLevel);
    }
}
