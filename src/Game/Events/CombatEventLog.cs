// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// { }
// FLogbook
public partial class CombatEventLog : EventLog
{
    public CombatEventLog(ECustomNode owner) : base(owner)
    { }

    // Combat2.pas:413
    public new const int ClassID = 131810;

    public CombatLogbook FLogbook;

    // Combat2.pas:405
    public override void Dispose()
    {
        CombatLogbook L;
        L = FLogbook;
        if (L != null)
        {
            L.Release();
            FLogbook = null;
        }

        base.Dispose();
    }

    // Combat2.pas:428
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwCreateEObject(ref FLogbook, null, 20);
    }

    // Combat2.pas:461
    public override string GetText() => FLogbook.FFinalData.FHeader;

    // Combat2.pas:431
    public override int GetTextColor()
    {
        int Result;
        CombatData Data;
        Data = FLogbook.FFinalData;
        if (Data.FPlayers.GetPlayers(Data.FAttackerPlayer).FPlayer != 0)
        {
            var players = Data.FPlayers.GetPlayers(Data.FAttackerPlayer);
            var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(players.FPlayer);
            Result = AbilityRules.RaceColors[fPlayersPlayers.FPlayerRace];
        }
        else
        {
            Result = AbilityRules.RaceColors[12];
        }

        return Result;
    }

    // Combat2.pas:440
    public override int GetLeftIcon()
    {
        int Result;
        CombatData Data;
        Data = FLogbook.FFinalData;
        if (Data.FPlayers.GetPlayers(Data.FAttackerPlayer).FPlayer != 0)
        {
            var players = Data.FPlayers.GetPlayers(Data.FAttackerPlayer);
            Result = AoWHSMap.Current.FPlayers.GetPlayers(players.FPlayer).FPlayerRace;
        }
        else
        {
            Result = 12;
        }

        return Result;
    }

    // Combat2.pas:449
    public override int GetMiddleIcon() => 13;

    // Combat2.pas:452
    public override int GetRightIcon()
    {
        int Result;
        CombatData Data;
        Data = FLogbook.FFinalData;
        if (Data.FPlayers.GetPlayers(Data.FDefenderPlayer).FPlayer != 0)
        {
            var players = Data.FPlayers.GetPlayers(Data.FDefenderPlayer);
            Result = AoWHSMap.Current.FPlayers.GetPlayers(players.FPlayer).FPlayerRace;
        }
        else
        {
            Result = 12;
        }

        return Result;
    }

    // Combat2.pas:467
    public override void PlayLoggedSFX(EventLogbook Logbook)
    {
        if (FLogbook.FInitialData.FPlayers.IndexOfPlayer((byte)AoWHSMap.Current.FSeatedPlayer) != -1)
        {
            AudioPlayback.PlaySample("SFX\\EVENTS\\ROLL.WAV");
        }
        else
        {
            AudioPlayback.PlaySample("SFX\\EVENTS\\NEWEVENT.WAV");
        }
    }

    // Combat2.pas:474
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool ShowCombat = default;
        CombatData Data = default;
        bool Result = false;
        switch (Trigger)
        {
            case ExecuteTrigger.User:
                AoWHSMap.Current.TriggerExecuteEventLog(this, false, Done);
                Result = true;
                break;
            case ExecuteTrigger.Event:
            case ExecuteTrigger.NewTurn:
                if (FLogbook.FInitialData.FPlayers.IndexOfPlayer((byte)AoWHSMap.Current.FSeatedPlayer) != -1)
                {
                    Data = FLogbook.FInitialData;
                    ShowCombat = Data.FPlayers.GetPlayers(Data.FAttackerPlayer).FPlayer == AoWHSMap.Current.FSeatedPlayer;
                    ShowCombat = ShowCombat || AoWHSMap.Current.FRequestCombatType && AoWHSMap.Current.FConnectionType == PlayerConnectionType.Email && FLogbook.FInitialData.FPlayers.GetPlayers(FLogbook.FInitialData.FDefenderPlayer).FPlayer == AoWHSMap.Current.FSeatedPlayer;
                    ShowCombat = ShowCombat || Trigger == ExecuteTrigger.Event && AoWHSMap.Current.FPlayMode == PlayMode.Turn && GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Combat);
                    ShowCombat = ShowCombat || Trigger == ExecuteTrigger.Event && AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous && GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Combat);
                    ShowCombat = ShowCombat && FLogbook.FCombatResult != CombatResult.NoDefenders;
                }
                else
                {
                    ShowCombat = false;
                }

                ShowCombat = ShowCombat || FLogbook.F_CombatLogbook_19;
                if (ShowCombat)
                {
                    AoWHSMap.Current.TriggerExecuteEventLog(this, false, Done);
                    Result = true;
                }
                else
                {
                    Result = false;
                }

                break;
        }

        return Result;
    }

    // Combat2.pas:416
    public void SetCombatLogbook(CombatLogbook Logbook)
    {
        CombatLogbook Old;
        Old = FLogbook;
        if (Old != Logbook)
        {
            if (Old != null)
            {
                Old.Release();
            }

            FLogbook = Logbook;
            if (FLogbook != null)
            {
                FLogbook.AddRef();
            }
        }
    }

    public override void ViewLocation()
    {
        var data = FLogbook.FFinalData;
        CenterViewTo((sbyte)data.FX, (sbyte)data.FY, (sbyte)data.FL);
    }
}
