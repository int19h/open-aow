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
public partial class EventLog : EngineObject
{

    public EventLog(ECustomNode owner) : base(owner)
    {
        RefCount = 1;
    }

    public int _day = AoWHSMap.Current is { } map ? map.Day : 0;
    public int Id;
    public sbyte _player = AoWHSMap.Current is { } map ? map.CurrentPlayer : (sbyte)0;
    public int RefCount;

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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref _day, 10);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _player), 11);
        stream.rwInteger(ref Id, 12);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int32("TEventLog.Day", ref _day);
        PersistSupport.UnsignedByte(persister, "Player", ref _player);
        persister.Int32("ID", ref Id);
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
            var count = AoWHSMap.Current.Players.GetCount();
            condition = _player < count;
        }

        if (condition)
        {
            index = AoWHSMap.Current.Players.GetPlayers(_player).PlayerRace;
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
            var count = AoWHSMap.Current.Players.GetCount();
            condition = _player < count;
        }

        if (condition)
        {
            result = AoWHSMap.Current.Players.GetPlayers(_player).PlayerRace;
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
        if (AoWHSMap.Current.SeatedPlayer == logbook.Player && AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GameOverStatus == GameOverStatus.None)
        {
            PlayLoggedSfx(logbook);
        }
    }

    // EventLog.pas:403
    public virtual bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result = default;
        if (trigger == ExecuteTrigger.User || ((trigger == ExecuteTrigger.Event || (trigger == ExecuteTrigger.NewTurn && AoWHSMap.Current.RequestCombatType)) && AoWHSMap.Current.SeatedPlayer == _player))
        {
            AoWHSMap.Current.TriggerExecuteEventLog(this, mode, done);
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
            AoWHSMap.Current.CenterViewTo(X, Y, L);
            AoWHSMap.Current.PlayCenterAnimation(X, Y, L);
        }
    }

    // EventLog.pas:419
    public void CenterViewTo(sbyte X, sbyte Y, sbyte L)
    {
        AoWHSMap.Current.CenterViewTo(X, Y, L);
        AoWHSMap.Current.PlayCenterAnimation(X, Y, L);
    }

    internal static void DistributeEventLog(EventLog log, bool execute)
    {
        Player player;
        EventLog copyLog;
        for (int I = 0, last = AoWHSMap.Current.Players.GetCount() - 1; I <= last; I++)
        {
            player = AoWHSMap.Current.Players.GetPlayers(I);
            if (player.Status == PlayerStatus.Playing)
            {
                copyLog = (EventLog)log.Copy(GameTables.GameEngine);
                player.EventLogbook.AddEvent(copyLog);
                if (I == AoWHSMap.Current.SeatedPlayer && execute)
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
        for (int I = 0, last = AoWHSMap.Current.Players.GetCount() - 1; I <= last; I++)
        {
            targetPlayer = AoWHSMap.Current.Players.GetPlayers(I);
            if (targetPlayer.Status == PlayerStatus.Playing && (I == player || !AoWHSMap.Current.FoggedForPlayer(X, Y, L, (sbyte)I)))
            {
                copyLog = (EventLog)log.Copy(GameTables.GameEngine);
                targetPlayer.EventLogbook.AddEvent(copyLog);
                if (I == AoWHSMap.Current.SeatedPlayer && AoWHSMap.Current.Players.GetPlayers(I).PlayerType == PlayerType.Human && execute)
                {
                    copyLog.Execute(false, ExecuteTrigger.Event, null);
                }

                copyLog.Release();
            }
        }
    }

}
