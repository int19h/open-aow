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
public partial class EventLog : EObject
{

    public EventLog(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
    }

    public int FDay = AoWHSMap.Current is { } map ? map.FDay : 0;
    public int FID;
    public sbyte FPlayer = AoWHSMap.Current is { } map ? map.FCurrentPlayer : (sbyte)0;
    public int FRefCount;

    // EventLog.pas:334
    public override void SetOwner(ECustomNode AOwner)
    {
        if (FRefCount != 0)
        {
            AddRef();
            base.SetOwner(AOwner);
            Release();
        }
        else
        {
            base.SetOwner(AOwner);
        }
    }

    // EventLog.pas:358
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FDay, 10);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 11);
        Stream.rwInteger(ref FID, 12);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("TEventLog.Day", ref FDay);
        PersistSupport.UnsignedByte(persister, "Player", ref FPlayer);
        persister.Int("ID", ref FID);
    }

    // EventLog.pas:345
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // EventLog.pas:351
    public override int Release()
    {
        FRefCount -= 1;
        int Result = FRefCount;
        if (FRefCount == 0)
        {
            Dispose();
        }

        return Result;
    }

    // EventLog.pas:371
    public virtual string GetText() => "Default TEventLog";

    // EventLog.pas:376
    public virtual int GetTextColor()
    {
        int Index;
        bool condition = FPlayer > 0;
        if (condition)
        {
            var count = AoWHSMap.Current.FPlayers.GetCount();
            condition = FPlayer < count;
        }

        if (condition)
        {
            Index = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerRace;
        }
        else
        {
            Index = 12;
        }

        return AbilityRules.RaceColors[Index];
    }

    // EventLog.pas:386
    public virtual int GetLeftIcon()
    {
        int Result;
        bool condition = FPlayer > 0;
        if (condition)
        {
            var count = AoWHSMap.Current.FPlayers.GetCount();
            condition = FPlayer < count;
        }

        if (condition)
        {
            Result = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerRace;
        }
        else
        {
            Result = 12;
        }

        return Result;
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
    public virtual void PlayLoggedSFX(EventLogbook Logbook)
    {
        AudioPlayback.PlaySample("SFX\\EVENTS\\NEWEVENT.WAV");
    }

    // EventLog.pas:449
    public virtual void NotifyLogged(EventLogbook Logbook)
    {
        if (AoWHSMap.Current.FSeatedPlayer == Logbook.FPlayer && AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FGameOverStatus == GameOverStatus.None)
        {
            PlayLoggedSFX(Logbook);
        }
    }

    // EventLog.pas:403
    public virtual bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool Result = default;
        if (Trigger == ExecuteTrigger.User || ((Trigger == ExecuteTrigger.Event || (Trigger == ExecuteTrigger.NewTurn && AoWHSMap.Current.FRequestCombatType)) && AoWHSMap.Current.FSeatedPlayer == FPlayer))
        {
            AoWHSMap.Current.TriggerExecuteEventLog(this, Mode, Done);
            Result = true;
        }
        else
        {
            if (Trigger == ExecuteTrigger.Event)
            {
                AudioPlayback.PlaySample("SFX\\EVENTS\\NEWEVENT.WAV");
            }

            Result = false;
        }

        return Result;
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

    internal static void DistributeEventLog(EventLog Log, bool Execute)
    {
        Player Player;
        EventLog CopyLog;
        for (int I = 0, last = AoWHSMap.Current.FPlayers.GetCount() - 1; I <= last; I++)
        {
            Player = AoWHSMap.Current.FPlayers.GetPlayers(I);
            if (Player.FStatus == Session.Player.Status.Playing)
            {
                CopyLog = (EventLog)Log.Copy(GameTables.AoWEngine);
                Player.FEventLogbook.AddEvent(CopyLog);
                if (I == AoWHSMap.Current.FSeatedPlayer && Execute)
                {
                    CopyLog.Execute(false, ExecuteTrigger.Event, null);
                }

                CopyLog.Release();
            }
        }
    }

    internal static void DistributeLocationEventLog(EventLog Log, sbyte Player, sbyte X, sbyte Y, sbyte L, bool Execute)
    {
        Player TargetPlayer;
        EventLog CopyLog;
        for (int I = 0, last = AoWHSMap.Current.FPlayers.GetCount() - 1; I <= last; I++)
        {
            TargetPlayer = AoWHSMap.Current.FPlayers.GetPlayers(I);
            if (TargetPlayer.FStatus == Session.Player.Status.Playing && (I == Player || !AoWHSMap.Current.FoggedForPlayer(X, Y, L, (sbyte)I)))
            {
                CopyLog = (EventLog)Log.Copy(GameTables.AoWEngine);
                TargetPlayer.FEventLogbook.AddEvent(CopyLog);
                if (I == AoWHSMap.Current.FSeatedPlayer && AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType == PlayerType.Human && Execute)
                {
                    CopyLog.Execute(false, ExecuteTrigger.Event, null);
                }

                CopyLog.Release();
            }
        }
    }

}
