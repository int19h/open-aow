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
    public MessageEventLog(ECustomNode owner) : base(owner)
    {
        FMessage = new StringList();
        FTitle = AbilityRules.TranslateRStr(GameText.MessageRStr);
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
