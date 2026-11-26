// Layout translated from recovered Delphi forms.
#nullable disable warnings
using Aow.Game.Layout;
using Aow.Game.Net;
using Aow.Game.Session;

namespace Aow.Game.UI.Dialogs;

public sealed partial class SetupEvents : ILayoutModule
{
    public void InitializeComponent() => InitializeComponent(LayoutSession.Default);

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "SetupEvents";
        LayoutDirectPlay createdDirectPlay = new(this);
        DirectPlay = createdDirectPlay;
        session.BeginRead(createdDirectPlay);
        createdDirectPlay.Name = "DPlay";
        createdDirectPlay.SessionName = "No Name's Game";
        createdDirectPlay.MaxPlayers = 12;
        createdDirectPlay.PlayerName = "No Name";
        createdDirectPlay.MessageReceived += (_, e) => DPlayMessage(e.Data, e.Size, e.ToId, e.FromId);
        createdDirectPlay.ChatMessageReceived += (_, e) => DPlayChatMessage(e.Message, e.ToId, e.FromId);
        createdDirectPlay.PingReceived += (_, e) => DPlayPingMessage(e.Milliseconds, e.Player);
        createdDirectPlay.FileTransferStarting += (_, e) => DPlayFileStartMessage(e.Id, e.Name, e.Size, e.Response);
        createdDirectPlay.FileTransferProgressed += (_, e) => DPlayFileProgressMessage(e.Id, e.Position, e.Size);
        createdDirectPlay.FileTransferCompleted += (_, e) => DPlayFileDoneMessage(e.Id, fileData: e.File);
        createdDirectPlay.PlayerDestroyed += (_, e) => DPlayDestroyPlayer(e.Id, numberPlayers: e.PlayerCount);
        createdDirectPlay.SessionLost += (_, _) => DPlaySessionLost();
        createdDirectPlay.MessageFailed += (_, _) => DPlayMessageError();
        createdDirectPlay.GuaranteeMessages = true;
        createdDirectPlay.UseCodes = true;
        createdDirectPlay.Ejected += (_, _) => DPlayEjected();
        createdDirectPlay.UseSetupDialogs = false;
        createdDirectPlay.FilePacketSize = 1000;
        session.EndRead(createdDirectPlay);
        SetupControl = new SetupControl(this, host: this, transport: SetupTransport.Unported);
        session.BeginRead(SetupControl);
        SetupControl.Name = "SetupControl";
        SetupControl.PlayerSetupCompleted += _ => SetupControlPlayerSetupDone();
        SetupControl.SettingChanged += _ => SetupControlSettingChanged();
        SetupControl.SendingMaps += _ => SetupControlSendingMaps();
        session.EndRead(SetupControl);
        session.EndRead(this);
        Loaded();
        DirectPlay.Loaded();
        SetupControl.Loaded();
        session.Resolve(this);
        if (session.FireCreateEvents)
        {
            SetupEventsCreate();
        }
    }
}
