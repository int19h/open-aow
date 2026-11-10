// Translated from EventLog.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Session;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Events;

public enum SaveGameEventLogModeElement : byte
{
    DisableCancel = 0
}

// { ot=1 }
// day, unique ID, player, reference count
public partial class EventLog : EObject
{

    public EventLog(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
    }

    public int FDay = World.Map.AoWHSMap.Current is { } map ? map.FDay : 0;
    public int FID;
    public sbyte FPlayer = World.Map.AoWHSMap.Current is { } map ? map.FCurrentPlayer : (sbyte)0;
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
            var count = World.Map.AoWHSMap.Current.FPlayers.GetCount();
            condition = FPlayer < count;
        }

        if (condition)
        {
            Index = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerRace;
        }
        else
        {
            Index = 12;
        }

        return Magic.Abilities.AbilityRules.RaceColors[Index];
    }

    // EventLog.pas:386
    public virtual int GetLeftIcon()
    {
        int Result;
        bool condition = FPlayer > 0;
        if (condition)
        {
            var count = World.Map.AoWHSMap.Current.FPlayers.GetCount();
            condition = FPlayer < count;
        }

        if (condition)
        {
            Result = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerRace;
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
        Audio.AudioPlayback.PlaySample("SFX\\EVENTS\\NEWEVENT.WAV");
    }

    // EventLog.pas:449
    public virtual void NotifyLogged(EventLogbook Logbook)
    {
        if (World.Map.AoWHSMap.Current.FSeatedPlayer == Logbook.FPlayer && World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FGameOverStatus == GameOverStatus.None)
        {
            PlayLoggedSFX(Logbook);
        }
    }

    // EventLog.pas:403
    public virtual bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool Result = default;
        if (Trigger == ExecuteTrigger.User || ((Trigger == ExecuteTrigger.Event || (Trigger == ExecuteTrigger.NewTurn && World.Map.AoWHSMap.Current.FRequestCombatType)) && World.Map.AoWHSMap.Current.FSeatedPlayer == FPlayer))
        {
            World.Map.AoWHSMap.Current.TriggerExecuteEventLog(this, Mode, Done);
            Result = true;
        }
        else
        {
            if (Trigger == ExecuteTrigger.Event)
            {
                Audio.AudioPlayback.PlaySample("SFX\\EVENTS\\NEWEVENT.WAV");
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
            World.Map.AoWHSMap.Current.CenterViewTo(X, Y, L);
            World.Map.AoWHSMap.Current.PlayCenterAnimation(X, Y, L);
        }
    }

    // EventLog.pas:419
    public void CenterViewTo(sbyte X, sbyte Y, sbyte L)
    {
        World.Map.AoWHSMap.Current.CenterViewTo(X, Y, L);
        World.Map.AoWHSMap.Current.PlayCenterAnimation(X, Y, L);
    }

    internal static void DistributeEventLog(EventLog Log, bool Execute)
    {
        Player Player;
        EventLog CopyLog;
        for (int I = 0, last = World.Map.AoWHSMap.Current.FPlayers.GetCount() - 1; I <= last; I++)
        {
            Player = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(I);
            if (Player.FStatus == Session.Player.Status.Playing)
            {
                CopyLog = (EventLog)Log.Copy(Magic.Abilities.GameTables.AoWEngine);
                Player.FEventLogbook.AddEvent(CopyLog);
                if (I == World.Map.AoWHSMap.Current.FSeatedPlayer && Execute)
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
        for (int I = 0, last = World.Map.AoWHSMap.Current.FPlayers.GetCount() - 1; I <= last; I++)
        {
            TargetPlayer = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(I);
            if (TargetPlayer.FStatus == Session.Player.Status.Playing && (I == Player || !World.Map.AoWHSMap.Current.FoggedForPlayer(X, Y, L, (sbyte)I)))
            {
                CopyLog = (EventLog)Log.Copy(Magic.Abilities.GameTables.AoWEngine);
                TargetPlayer.FEventLogbook.AddEvent(CopyLog);
                if (I == World.Map.AoWHSMap.Current.FSeatedPlayer && World.Map.AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType == PlayerType.Human && Execute)
                {
                    CopyLog.Execute(false, ExecuteTrigger.Event, null);
                }

                CopyLog.Release();
            }
        }
    }

}

// text
public partial class TextEventLog : EventLog
{
    public TextEventLog(ECustomNode owner) : base(owner)
    { }

    // EventLog.pas:713
    public new const int ClassID = 131809;

    public string FText = "";

    // EventLog.pas:716
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwString(ref FText, 15);
    }

    // EventLog.pas:719
    public override string GetText() => FText;

    // EventLog.pas:722
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done) => false;

}

// location
public partial class LocationTextEventLog : TextEventLog
{
    // EventLog.pas:725
    public LocationTextEventLog(ECustomNode owner) : base(owner)
    {
        FXhx = -1;
        FYhx = -1;
        FLhx = -1;
    }

    // EventLog.pas:728
    public new const int ClassID = 131816;

    public sbyte FXhx;
    public sbyte FYhx;
    public sbyte FLhx;

    // EventLog.pas:731
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FXhx), 20);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FYhx), 21);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLhx), 22);
    }

    // EventLog.pas:740
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = FXhx;
        Y = FYhx;
        L = FLhx;
    }

    // EventLog.pas:743
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        base.Execute(Mode, Trigger, Done);
        if (Trigger == ExecuteTrigger.User)
        {
            ViewLocation();
        }

        return false;
    }

    // EventLog.pas:737
    public void SetLocation(sbyte X, sbyte Y, sbyte L)
    {
        FXhx = X;
        FYhx = Y;
        FLhx = L;
    }

}

// title, message, layout, buttons, pressed button
public partial class MessageEventLog : EventLog
{
    // EventLog.pas:824
    public MessageEventLog(ECustomNode owner) : base(owner)
    {
        FMessage = new StringList();
        FTitle = Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameText.MessageRStr);
        FButtons = Buttons.Ok;
        FPressedButton = PressedEventLogButton.None;
        FLayout = Layout.Normal;
    }

    public string FTitle = "";
    public string FHeader = "";
    public StringCollection FMessage;
    public Layout FLayout;
    public Buttons FButtons;
    public PressedEventLogButton FPressedButton;

    // EventLog.pas:832
    public override void Dispose()
    {
        FMessage?.Dispose();
        base.Dispose();
    }

    // EventLog.pas:835
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FMessage, 30);
        Stream.rwData(Bytes.Of(ref FLayout, 1), 1, 31);
        Stream.rwData(Bytes.Of(ref FButtons, 1), 1, 32);
        Stream.rwString(ref FTitle, 33);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Lines(persister, "Message", FMessage);
        persister.Enum("Layout", ref FLayout);
        persister.Enum("Buttons", ref FButtons);
        persister.String("Title", ref FTitle);
    }

    // EventLog.pas:842
    public virtual bool OK()
    {
        FPressedButton = PressedEventLogButton.Ok;
        return true;
    }

    // EventLog.pas:845
    public virtual bool Cancel()
    {
        FPressedButton = PressedEventLogButton.Cancel;
        return true;
    }

    // EventLog.pas:848
    public virtual bool Yes()
    {
        FPressedButton = PressedEventLogButton.Yes;
        return true;
    }

    // EventLog.pas:854
    public virtual bool No()
    {
        FPressedButton = PressedEventLogButton.No;
        return true;
    }

    public enum Buttons : byte
    {
        Ok = 0, OkCancel = 1, YesNo = 2, YesNoCancel = 3
    }

    public enum Layout : byte
    {
        Normal = 0, Header = 1
    }

}

// message, help title, mode
public partial class HelpMessageEventLog : EventLog
{
    // EventLog.pas:860
    public HelpMessageEventLog(ECustomNode owner) : base(owner)
    {
        FMessage = new StringList();
        FHelpFileName = Magic.Abilities.AbilityRules.TranslateRStr(HelpRStr);
        FMode = 0;
    }

    public StringList FMessage;
    public string FHelpFileName = "";
    public byte FMode;

    // EventLog.pas:868
    public override void Dispose()
    {
        FMessage?.Dispose();
        base.Dispose();
    }

    private static readonly string HelpRStr = "Help";

}

[LegacyObject]
// list of -byte callback records
public partial class ExecuteEventLogCallBackList : IDisposable
{
    public List<ExecuteEventLogInfo> FList = [];

    // EventLog.pas:283
    public virtual void Dispose()
    {
        Clear();
    }

    // EventLog.pas:277
    public ExecuteEventLogCallBackList()
    {
        CaptureAddress.Assign(this);
        FList = new List<ExecuteEventLogInfo>();
    }

    // EventLog.pas:290
    public void Clear()
    {
        while (FList.Count != 0)
        {
            Delete(FList.Count - 1);
        }
    }

    // EventLog.pas:295
    public int GetCount() => FList.Count;

    // EventLog.pas:300
    public ref ExecuteEventLogInfo GetItem(int Index) => ref CollectionsMarshal.AsSpan(FList)[Index];

    // EventLog.pas:305
    public void Add(in ExecuteEventLogInfo Info)
    {
        FList.Add(Info);
    }

    // EventLog.pas:314
    public void Delete(int Index)
    {
        if (Index < FList.Count)
        {
            FList.RemoveAt(Index);
        }
    }

    // EventLog.pas:133
}

// update count, changed callback
public partial class EventLogList : ENode
{
    public EventLogList(ECustomNode owner) : base(owner)
    { }

    public int FUpdateCount;
    public NotifyEvent<object> FOnChanged;

    // EventLog.pas:481
    public override void MainReadWrite(EStorageStream Stream)
    {
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            BeginUpdate();
        }

        base.MainReadWrite(Stream);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.Compare, StorageModeElement.Translate)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
            {
                for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
                {
                    ((EObject)FChildren.List[I]).Release();
                }
            }

            EndUpdate();
        }
    }

    // EventLog.pas:471
    public override void Clear()
    {
        for (int I = GetCount() - 1; I >= 0; I--)
        {
            ((EObject)FChildren.List[I]).Release();
        }

        FChildren.Clear();
    }

    // EventLog.pas:478
    public override void DestroyAllChildren()
    {
        Clear();
    }

    // EventLog.pas:464
    public override int RemoveChild(EObject Child)
    {
        int Result = FChildren.Remove(Child);
        if (Result != -1)
        {
            Child.SetOwner(null);
            Child.Release();
            Changed();
        }

        return Result;
    }

    // EventLog.pas:456
    public override int AddChild(EObject Child)
    {
        FChildren.Add(Child);
        if (Child.AddRef() == -1)
        {
            throw new NotSupportedException("EObject doesn't support ReferenceCounting");
        }

        int Result = FChildren.Count - 1;
        Changed();
        return Result;
    }

    // EventLog.pas:494
    public void Changed()
    {
        if (FUpdateCount == 0 && FOnChanged != null)
        {
            FOnChanged(this);
        }
    }

    // EventLog.pas:499
    public void BeginUpdate()
    {
        FUpdateCount += 1;
    }

    // EventLog.pas:502
    public void EndUpdate()
    {
        FUpdateCount -= 1;
        if (FUpdateCount == 0)
        {
            Changed();
        }
    }

    // EventLog.pas:505
    public EventLog GetEvents(int Index) => (EventLog)FChildren.List[Index];

    // EventLog.pas:508
    public void AddEvent(EventLog Event)
    {
        AddChild(Event);
    }

    // EventLog.pas:511
    public void RemoveEvent(EventLog Event)
    {
        RemoveChild(Event);
    }

    // EventLog.pas:514
    public EventLog FindID(int ID)
    {
        int I;
        I = GetCount();
        EventLog Result = null;
        while (I != 0 && Result == null)
        {
            I -= 1;
            if (((EventLog)FChildren.List[I]).FID == ID)
            {
                Result = (EventLog)FChildren.List[I];
                break;
            }
        }

        return Result;
    }

    // Engine.pas:631
}

// events, player, next ID
public partial class EventLogbook : EObject
{
    // EventLog.pas:532
    public new const int ClassID = 131808;

    public EventLogList FEvents;
    public sbyte FPlayer;
    public int FNextID;

    // EventLog.pas:529
    public override void Dispose()
    {
        FEvents?.Dispose();
        base.Dispose();
    }

    // EventLog.pas:547
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FEvents, 10);
        Stream.rwInteger(ref FNextID, 11);
    }

    // EventLog.pas:526
    public EventLogbook(ECustomNode owner) : base(owner)
    {
        FEvents = new EventLogList(null);
    }

    // EventLog.pas:535
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        FEvents.MsgProc(ref Msg, Sender);
    }

    // EventLog.pas:538
    public int GetCount() => FEvents.GetCount();

    // EventLog.pas:541
    public NotifyEvent<object> GetOnChanged() => FEvents.FOnChanged;

    // EventLog.pas:544
    public void SetOnChanged(NotifyEvent<object> Value)
    {
        FEvents.FOnChanged = Value;
    }

    // EventLog.pas:550
    public void ListEvents(StringCollection Strings)
    {
        for (int I = 0, last = FEvents.FChildren.Count - 1; I <= last; I++)
        {
            if (((EventLog)FEvents.FChildren.List[I]).GetVisible())
            {
                Strings.AddObject(((EventLog)FEvents.FChildren.List[I]).GetText(), IntToObject(I));
            }
        }
    }

    // EventLog.pas:558
    public EventLog GetLogs(int Index) => (EventLog)FEvents.FChildren.List[Index];

    // EventLog.pas:561
    public int CreateUniqueID()
    {
        FNextID += 1;
        while (FindID(FNextID) != null)
        {
            FNextID += 1;
            if (FNextID > 16777215)
            {
                FNextID = 1;
            }
        }

        return FNextID;
    }

    // EventLog.pas:572
    public EventLog FindID(int ID) => FEvents.FindID(ID);

    // EventLog.pas:575
    public int FindFirstLog(int Player, int Day)
    {
        int Result = 0;
        while (Result != this.FEvents.FChildren.Count && (((EventLog)this.FEvents.FChildren.List[Result]).FPlayer != Player || ((EventLog)this.FEvents.FChildren.List[Result]).FDay != Day))
        {
            Result += 1;
        }

        if (Result == this.FEvents.FChildren.Count)
        {
            Result = -1;
        }

        return Result;
    }

    // EventLog.pas:587
    public int FindFirstLogEx(int AClassID, int Player, int Day)
    {
        int Result = 0;
        while (Result != this.FEvents.FChildren.Count && (((EventLog)this.FEvents.FChildren.List[Result]).FPlayer != Player || ((EventLog)this.FEvents.FChildren.List[Result]).FDay != Day || ((EObject)this.FEvents.FChildren.List[Result]).EClass.ClassID != AClassID))
        {
            Result += 1;
        }

        if (Result == this.FEvents.FChildren.Count)
        {
            Result = -1;
        }

        return Result;
    }

    // EventLog.pas:600
    public void AddEvent(EventLog Event)
    {
        Assert(World.Map.AoWHSMap.Current.GetSynchronised(), "", "EventLog.pas", 704);
        Event.SetOwner(FEvents);
        Event.FID = CreateUniqueID();
        Event.NotifyLogged(this);
    }

    // EventLog.pas:710
    public void RemoveEvent(EventLog Event)
    {
        FEvents.RemoveChild(Event);
    }

}

public partial class UserEventLog : EventLog
{
    public UserEventLog(ECustomNode owner) : base(owner)
    { }

    public object FData;
}

public partial class EndTurnEventLog : EventLog
{
    public EndTurnEventLog(ECustomNode owner) : base(owner)
    { }

    // EventLog.pas:753
    public new const int ClassID = 131811;

    // EventLog.pas:750
    public override bool GetVisible() => false;

    // EventLog.pas:756
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done) => false;

}

// message
public partial class ShowMessageEventLog : EventLog
{
    // EventLog.pas:759
    public ShowMessageEventLog(ECustomNode owner) : base(owner)
    {
        FMessage = new StringList();
    }

    // EventLog.pas:765
    public new const int ClassID = 131830;

    public StringList FMessage;

    // EventLog.pas:762
    public override void Dispose()
    {
        FMessage?.Dispose();
        base.Dispose();
    }

    // EventLog.pas:768
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FMessage, 20);
    }

}

// message, inherited player
public partial class GameOverEventLog : EventLog
{
    // EventLog.pas:771
    public GameOverEventLog(ECustomNode owner) : base(owner)
    {
        FMessage = new StringList();
    }

    // EventLog.pas:777
    public new const int ClassID = 131812;

    public StringList FMessage;
    public sbyte FTargetPlayer;

    // EventLog.pas:774
    public override void Dispose()
    {
        FMessage?.Dispose();
        base.Dispose();
    }

    // EventLog.pas:780
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FMessage, 20);
    }

    // EventLog.pas:783
    public override string GetText()
    {
        string Result = "";
        switch (World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FGameOverStatus)
        {
            case GameOverStatus.Lost:
                var name = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetName();
                Result = RtlUnit.Format(Magic.Abilities.AbilityRules.TranslateRStr(GameOverDefeatEventLogRStr), [name]);
                break;
            case GameOverStatus.Won:
                var name2 = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetName();
                Result = RtlUnit.Format(Magic.Abilities.AbilityRules.TranslateRStr(GameOverVictoryEventLogRStr), [name2]);
                break;
        }

        return Result;
    }

    // EventLog.pas:794
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool Result = default;
        switch (Trigger)
        {
            case ExecuteTrigger.User:
                if (World.Map.AoWHSMap.Current.FSeatedPlayer != FPlayer)
                {
                    World.Map.AoWHSMap.Current.TriggerExecuteEventLog(this, Mode, Done);
                    Result = true;
                }
                else
                {
                    Result = false;
                }

                break;
            case ExecuteTrigger.Event:
                World.Map.AoWHSMap.Current.TriggerExecuteEventLog(this, Mode, Done);
                Result = true;
                break;
            case ExecuteTrigger.NewTurn:
                World.Map.AoWHSMap.Current.TriggerExecuteEventLog(this, Mode, Done);
                Result = true;
                break;
            default:
                Result = false;
                break;
        }

        return Result;
    }

    // EventLog.pas:791
    public void Handled()
    {
        World.Map.AoWHSMap.Current.GameOverEventHandled(FPlayer);
    }

    private static readonly string GameOverDefeatEventLogRStr = "%s is defeated!";
    private static readonly string GameOverVictoryEventLogRStr = "%s is victorious!";

}

// inherited text
public partial class NewDayEventLog : TextEventLog
{
    public NewDayEventLog(ECustomNode owner) : base(owner)
    { }

    // EventLog.pas:809
    public new const int ClassID = 131813;

    // EventLog.pas:812
    public override int GetLeftIcon() => 18;

    // EventLog.pas:815
    public override void PlayLoggedSFX(EventLogbook Logbook)
    {
        if (World.Map.AoWHSMap.Current.FDay > 1)
        {
            base.PlayLoggedSFX(Logbook);
        }
    }

}

public partial class ShowBookEventLog : EventLog
{
    public ShowBookEventLog(ECustomNode owner) : base(owner)
    { }

    // EventLog.pas:818
    public new const int ClassID = 131818;

    // EventLog.pas:821
    public override bool GetVisible() => false;

}

// inherited message and title
public partial class TutorialMessageEventLog : HelpMessageEventLog
{
    // EventLog.pas:871
    public TutorialMessageEventLog(ECustomNode owner) : base(owner)
    {
        FHelpFileName = Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameText.TutorialRStr);
    }

}

// directory, default name, file name, title
public partial class SaveGameEventLog : EventLog
{
    // EventLog.pas:874
    public SaveGameEventLog(ECustomNode owner) : base(owner)
    {
        FTitle = Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameText.SaveRStr);
        FMode = Set<SaveGameEventLogModeElement>.Of();
    }

    public Set<SaveGameEventLogModeElement> FMode;
    public string FDirectory = "";
    public string FDefaultName = "";
    public string FFileName = "";
    public string FTitle = "";

}

// password, title, prompt, button
public partial class PasswordEventLog : EventLog
{
    // EventLog.pas:877
    public PasswordEventLog(ECustomNode owner) : base(owner)
    {
        FPassword = "";
        FPrompt = Magic.Abilities.AbilityRules.TranslateRStr(EnterPasswordRStr);
        FTitle = Magic.Abilities.AbilityRules.TranslateRStr(PasswordVerificationRStr);
        FPressedButton = PressedEventLogButton.None;
    }

    public string FPassword = "";
    public string FTitle = "";
    public string FPrompt = "";
    public PressedEventLogButton FPressedButton;

    private static readonly string EnterPasswordRStr = "Enter password:";
    private static readonly string PasswordVerificationRStr = "Password Verification";

}

public delegate void ExecuteDoneEvent(EventLog Log);

public struct ExecuteEventLogInfo
{
    public EventLog Log;
    public ExecuteDoneEvent Done;
    public bool Immediate;
}

public enum ExecuteTrigger : byte
{
    Event = 0,
    User = 1,
    NewTurn = 2
}

public enum PressedEventLogButton : byte
{
    None = 0,
    Ok = 1,
    Cancel = 2,
    Yes = 3,
    No = 4
}
