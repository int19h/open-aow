#nullable disable
// Translated from EventLog.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Audio;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Events;

// { ot=1 }
// day, unique ID, player, reference count
public partial class EventLog : EngineObject, IOwned<EventLogList>, IOwnedSink<EventLogList>
{
    // The list that holds the log. AcceptsOwner admits only that list, so the cast never fails.
    public new EventLogList Owner { get => (EventLogList)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is EventLogList;

    void IOwnedSink<EventLogList>.SetOwner(EventLogList owner) => SetOwner(owner);

    public EventLog(CustomNode owner) : base(owner) => RefCount = 1;

    public int Day { get; set; } = StrategicMap.Current is { } map ? map.Day : 0;
    public int Id { get; set; }

    public sbyte Player { get; set; } = StrategicMap.Current is { } map ? map.CurrentPlayer : (sbyte)0;

    public int RefCount { get; private set; }

    // EventLog.pas:334
    public override void SetOwner(CustomNode owner)
    {
        if (RefCount != 0)
        {
            AddRef();
            base.SetOwner(owner);
            Release();
        }
        else
        {
            base.SetOwner(owner);
        }
    }

    // EventLog.pas:358
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Day = persister.Int32("Day", value: Day);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        Id = persister.Int32("Id", value: Id);
    }

    // EventLog.pas:345
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // EventLog.pas:351
    public override int Release()
    {
        RefCount -= 1;
        int result = RefCount;
        if (RefCount == 0)
        {
            Dispose();
        }

        return result;
    }

    // EventLog.pas:371
    public virtual string GetText() => "Default TEventLog";

    // EventLog.pas:376
    public virtual int GetTextColor()
    {
        int index;
        bool condition = Player > 0;
        if (condition)
        {
            int count = StrategicMap.Current.Players.GetCount();
            condition = Player < count;
        }

        index = condition ? StrategicMap.Current.Players.GetPlayers(Player).PlayerRace : 12;

        return AbilityRules.RaceColors[index];
    }

    // EventLog.pas:386
    public virtual int GetLeftIcon()
    {
        int result;
        bool condition = Player > 0;
        if (condition)
        {
            int count = StrategicMap.Current.Players.GetCount();
            condition = Player < count;
        }

        result = condition ? StrategicMap.Current.Players.GetPlayers(Player).PlayerRace : 12;

        return result;
    }

    // EventLog.pas:393
    public virtual int GetMiddleIcon() => -1;

    // EventLog.pas:398
    public virtual int GetRightIcon() => -1;

    // EventLog.pas:366
    public virtual bool GetVisible() => true;

    // EventLog.pas:425
    public virtual MapCoordinate GetLocation() => MapCoordinate.None;

    // EventLog.pas:444
    public virtual void PlayLoggedSfx(EventLogbook logbook) => AudioPlayback.PlaySample("SFX\\EVENTS\\NEWEVENT.WAV");

    // EventLog.pas:449
    public virtual void NotifyLogged(EventLogbook logbook)
    {
        if (StrategicMap.Current.SeatedPlayer == logbook.Player && StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GameOverStatus == GameOverStatus.None)
        {
            PlayLoggedSfx(logbook);
        }
    }

    // EventLog.pas:403
    public virtual bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result = default;
        if (trigger == ExecuteTrigger.User || ((trigger == ExecuteTrigger.Event || (trigger == ExecuteTrigger.NewTurn && StrategicMap.Current.RequestCombatType)) && StrategicMap.Current.SeatedPlayer == Player))
        {
            StrategicMap.Current.TriggerExecuteEventLog(this, mode, done);
            result = true;
        }
        else
        {
            if (trigger == ExecuteTrigger.Event)
            {
                AudioPlayback.PlaySample("SFX\\EVENTS\\NEWEVENT.WAV");
            }

            result = false;
        }

        return result;
    }

    // EventLog.pas:432
    public virtual void ViewLocation()
    {
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        MapCoordinate coordinate16 = GetLocation();
        x = coordinate16.X;
        y = coordinate16.Y;
        l = coordinate16.MapLevel;
        if (x != -1)
        {
            StrategicMap.Current.CenterViewTo(x, y, l);
            StrategicMap.Current.PlayCenterAnimation(x, y, l);
        }
    }

    // EventLog.pas:419
    public void CenterViewTo(sbyte X, sbyte Y, sbyte L)
    {
        StrategicMap.Current.CenterViewTo(X, Y, L);
        StrategicMap.Current.PlayCenterAnimation(X, Y, L);
    }

    internal static void DistributeEventLog(EventLog log, bool execute)
    {
        Player player;
        EventLog copyLog;
        for (int i = 0, last = StrategicMap.Current.Players.GetCount() - 1; i <= last; i++)
        {
            player = StrategicMap.Current.Players.GetPlayers(i);
            if (player.Status == PlayerStatus.Playing)
            {
                copyLog = (EventLog)log.Copy(log.GetEngine());
                player.EventLogbook.AddEvent(copyLog);
                if (i == StrategicMap.Current.SeatedPlayer && execute)
                {
                    copyLog.Execute(false, ExecuteTrigger.Event, null);
                }

                copyLog.Release();
            }
        }
    }

    internal static void DistributeLocationEventLog(EventLog log, sbyte player, sbyte x, sbyte y, sbyte level, bool execute)
    {
        Player targetPlayer;
        EventLog copyLog;
        for (int i = 0, last = StrategicMap.Current.Players.GetCount() - 1; i <= last; i++)
        {
            targetPlayer = StrategicMap.Current.Players.GetPlayers(i);
            if (targetPlayer.Status == PlayerStatus.Playing && (i == player || !StrategicMap.Current.FoggedForPlayer(x, y, level, (sbyte)i)))
            {
                copyLog = (EventLog)log.Copy(log.GetEngine());
                targetPlayer.EventLogbook.AddEvent(copyLog);
                if (i == StrategicMap.Current.SeatedPlayer && StrategicMap.Current.Players.GetPlayers(i).PlayerType == PlayerType.Human && execute)
                {
                    copyLog.Execute(false, ExecuteTrigger.Event, null);
                }

                copyLog.Release();
            }
        }
    }

}
