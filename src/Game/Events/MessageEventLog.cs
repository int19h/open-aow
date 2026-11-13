// Translated from EventLog.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Events;

// title, message, layout, buttons, pressed button
public partial class MessageEventLog : EventLog
{
    // EventLog.pas:824
    public MessageEventLog(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Message = new StringList();
        Title = AbilityRules.TranslateResourceString(GetEngine(), GameText.MessageText);
        Buttons = MessageEventLogButtons.Ok;
        PressedButton = PressedEventLogButton.None;
        Layout = MessageEventLogLayout.Normal;
    }

    public string Title = "";

    public string Header { get; } = "";
    public StringCollection Message { get; }

    public MessageEventLogLayout Layout;
    public MessageEventLogButtons Buttons;

    public PressedEventLogButton PressedButton { get; private set; }

    // EventLog.pas:832
    public override void Dispose()
    {
        Message?.Dispose();
        base.Dispose();
    }

    // EventLog.pas:835
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Lines(Message, 30);
        stream.Bytes(Bytes.Of(ref Layout, 1), 1, 31);
        stream.Bytes(Bytes.Of(ref Buttons, 1), 1, 32);
        stream.String(ref Title, 33);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Lines(persister, name: "Message", lines: Message);
        Layout = persister.Enum("Layout", value: Layout);
        Buttons = persister.Enum("Buttons", value: Buttons);
        Title = persister.String("Title", value: Title);
    }

    // EventLog.pas:842
    public virtual bool Ok()
    {
        PressedButton = PressedEventLogButton.Ok;
        return true;
    }

    // EventLog.pas:845
    public virtual bool Cancel()
    {
        PressedButton = PressedEventLogButton.Cancel;
        return true;
    }

    // EventLog.pas:848
    public virtual bool Yes()
    {
        PressedButton = PressedEventLogButton.Yes;
        return true;
    }

    // EventLog.pas:854
    public virtual bool No()
    {
        PressedButton = PressedEventLogButton.No;
        return true;
    }

}

public enum MessageEventLogButtons : byte
{
    Ok = 0, OkCancel = 1, YesNo = 2, YesNoCancel = 3
}

public enum MessageEventLogLayout : byte
{
    Normal = 0, Header = 1
}
