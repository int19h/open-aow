// Layout translated from recovered Delphi forms.
#nullable disable warnings
using Aow.Game.Layout;
using Aow.Game.Session;

namespace Aow.Game.UI.Dialogs;

public sealed partial class SetupEvents : ILayoutModule
{
    public void InitializeComponent() => InitializeComponent(LayoutSession.Default);

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "SetupEvents";
        LayoutOnCreate += _ => SetupEventsCreate();
        LayoutDirectPlay createdDirectPlay = new(this);
        DirectPlay = createdDirectPlay;
        session.BeginRead(createdDirectPlay);
        createdDirectPlay.Name = "DPlay";
        createdDirectPlay.SessionName = "No Name's Game";
        createdDirectPlay.MaxPlayers = 12;
        createdDirectPlay.PlayerName = "No Name";
        createdDirectPlay.Message += DPlayMessage;
        createdDirectPlay.ChatMessage += DPlayChatMessage;
        createdDirectPlay.PingMessage += DPlayPingMessage;
        createdDirectPlay.FileStartMessage += DPlayFileStartMessage;
        createdDirectPlay.FileProgressMessage += DPlayFileProgressMessage;
        createdDirectPlay.FileDoneMessage += DPlayFileDoneMessage;
        createdDirectPlay.DestroyPlayer += DPlayDestroyPlayer;
        createdDirectPlay.SessionLost += DPlaySessionLost;
        createdDirectPlay.MessageError += (_, _) => DPlayMessageError();
        createdDirectPlay.GuaranteeMessages = true;
        createdDirectPlay.UseCodes = true;
        createdDirectPlay.Ejected += _ => DPlayEjected();
        createdDirectPlay.UseSetupDialogs = false;
        createdDirectPlay.FilePacketSize = 1000;
        session.EndRead(createdDirectPlay);
        SetupControl = new SetupControl(this);
        session.BeginRead(SetupControl);
        SetupControl.Name = "SetupControl";
        SetupControl.MapSent += SetupControlSendMap;
        SetupControl.SendData += SetupControlSendData;
        SetupControl.PlayerSetupCompleted += _ => SetupControlPlayerSetupDone();
        SetupControl.SettingChanged += _ => SetupControlSettingChanged();
        SetupControl.CustomizeLeader += SetupControlCustomizeLeader;
        SetupControl.OnLoadMap += SetupControlLoadMap;
        SetupControl.Start += _ => SetupControlStart();
        SetupControl.Synchronize += SetupControlSynchronize;
        SetupControl.PlayerEjected += SetupControlEjectPlayer;
        SetupControl.IncompatibleVersion += SetupControlIncompatibleVersion;
        SetupControl.SendingMaps += _ => SetupControlSendingMaps();
        session.EndRead(SetupControl);
        session.EndRead(this);
        Loaded();
        DirectPlay.Loaded();
        SetupControl.Loaded();
        session.Resolve(this);
        if (session.FireCreateEvents)
        {
            LayoutOnCreate?.Invoke(this);
        }
    }
}
