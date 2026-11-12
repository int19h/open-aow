// Translated from EventLog.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is EventLogList;

    void IOwnedSink<EventLogList>.SetOwner(EventLogList owner) => SetOwner(owner);

    public EventLog(ECustomNode owner) : base(owner)
    {
        RefCount = 1;
    }

    public int _day = StrategicMap.Current is { } map ? map.Day : 0;
    public int Id;
    public sbyte _player = StrategicMap.Current is { } map ? map.CurrentPlayer : (sbyte)0;

    public int RefCount { get; private set; }

    // EventLog.pas:334
    public override void SetOwner(ECustomNode owner)
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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref _day, 10);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref _player), 11);
        stream.Int32(ref Id, 12);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _day = persister.Int32("TEventLog.Day", _day);
        _player = PersistSupport.UnsignedByte(persister, "Player", _player);
        Id = persister.Int32("ID", Id);
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
        bool condition = _player > 0;
        if (condition)
        {
            var count = StrategicMap.Current.Players.GetCount();
            condition = _player < count;
        }

        if (condition)
        {
            index = StrategicMap.Current.Players.GetPlayers(_player).PlayerRace;
        }
        else
        {
            index = 12;
        }

        return AbilityRules.RaceColors[index];
    }

    // EventLog.pas:386
    public virtual int GetLeftIcon()
    {
        int result;
        bool condition = _player > 0;
        if (condition)
        {
            var count = StrategicMap.Current.Players.GetCount();
            condition = _player < count;
        }

        if (condition)
        {
            result = StrategicMap.Current.Players.GetPlayers(_player).PlayerRace;
        }
        else
        {
            result = 12;
        }

        return result;
    }

    // EventLog.pas:393
    public virtual int GetMiddleIcon() => -1;

    // EventLog.pas:398
    public virtual int GetRightIcon() => -1;

    // EventLog.pas:366
    public virtual bool GetVisible() => true;

    // EventLog.pas:425
    public virtual void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = -1;
        Y = -1;
        L = -1;
    }

    // EventLog.pas:444
    public virtual void PlayLoggedSfx(EventLogbook logbook)
    {
        AudioPlayback.PlaySample("SFX\\EVENTS\\NEWEVENT.WAV");
    }

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
        if (trigger == ExecuteTrigger.User || ((trigger == ExecuteTrigger.Event || (trigger == ExecuteTrigger.NewTurn && StrategicMap.Current.RequestCombatType)) && StrategicMap.Current.SeatedPlayer == _player))
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
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        GetLocation(ref X, ref Y, ref L);
        if (X != -1)
        {
            StrategicMap.Current.CenterViewTo(X, Y, L);
            StrategicMap.Current.PlayCenterAnimation(X, Y, L);
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
        for (int I = 0, last = StrategicMap.Current.Players.GetCount() - 1; I <= last; I++)
        {
            player = StrategicMap.Current.Players.GetPlayers(I);
            if (player.Status == PlayerStatus.Playing)
            {
                copyLog = (EventLog)log.Copy(GameTables.GameEngine);
                player.EventLogbook.AddEvent(copyLog);
                if (I == StrategicMap.Current.SeatedPlayer && execute)
                {
                    copyLog.Execute(false, ExecuteTrigger.Event, null);
                }

                copyLog.Release();
            }
        }
    }

    internal static void DistributeLocationEventLog(EventLog log, sbyte player, sbyte X, sbyte Y, sbyte L, bool execute)
    {
        Player targetPlayer;
        EventLog copyLog;
        for (int I = 0, last = StrategicMap.Current.Players.GetCount() - 1; I <= last; I++)
        {
            targetPlayer = StrategicMap.Current.Players.GetPlayers(I);
            if (targetPlayer.Status == PlayerStatus.Playing && (I == player || !StrategicMap.Current.FoggedForPlayer(X, Y, L, (sbyte)I)))
            {
                copyLog = (EventLog)log.Copy(GameTables.GameEngine);
                targetPlayer.EventLogbook.AddEvent(copyLog);
                if (I == StrategicMap.Current.SeatedPlayer && StrategicMap.Current.Players.GetPlayers(I).PlayerType == PlayerType.Human && execute)
                {
                    copyLog.Execute(false, ExecuteTrigger.Event, null);
                }

                copyLog.Release();
            }
        }
    }

}
