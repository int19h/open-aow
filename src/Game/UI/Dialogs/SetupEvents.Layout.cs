// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.Session;

namespace Aow.Game.UI.Dialogs;

public partial class SetupEvents : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "SetupEvents";
        LayoutOnCreate += _ => SetupEventsCreate();
        LayoutDirectPlay createdDirectPlay = new LayoutDirectPlay(this);
        DirectPlay = createdDirectPlay;
        session.BeginRead(createdDirectPlay);
        createdDirectPlay.Name = "DPlay";
        createdDirectPlay.SessionName = "No Name's Game";
        createdDirectPlay.MaxPlayers = 12;
        createdDirectPlay.PlayerName = "No Name";
        createdDirectPlay.OnMessage += DPlayMessage;
        createdDirectPlay.OnChatMessage += DPlayChatMessage;
        createdDirectPlay.OnPingMessage += DPlayPingMessage;
        createdDirectPlay.OnFileStartMessage += DPlayFileStartMessage;
        createdDirectPlay.OnFileProgressMessage += DPlayFileProgressMessage;
        createdDirectPlay.OnFileDoneMessage += DPlayFileDoneMessage;
        createdDirectPlay.OnDestroyPlayer += DPlayDestroyPlayer;
        createdDirectPlay.OnSessionLost += DPlaySessionLost;
        createdDirectPlay.OnMessageError += (_, _) => DPlayMessageError();
        createdDirectPlay.GuaranteeMessages = true;
        createdDirectPlay.UseCodes = true;
        createdDirectPlay.OnEjected += _ => DPlayEjected();
        createdDirectPlay.UseSetupDialogs = false;
        createdDirectPlay.FilePacketSize = 1000;
        session.EndRead(createdDirectPlay);
        SetupControl = new SetupControl(this);
        session.BeginRead(SetupControl);
        SetupControl.Name = "SetupControl";
        SetupControl.OnSendMap += SetupControlSendMap;
        SetupControl.OnSendData += SetupControlSendData;
        SetupControl.OnPlayerSetupDone += _ => SetupControlPlayerSetupDone();
        SetupControl.OnSettingChanged += _ => SetupControlSettingChanged();
        SetupControl.OnCustomizeLeader += SetupControlCustomizeLeader;
        SetupControl.OnLoadMap += SetupControlLoadMap;
        SetupControl.OnStart += _ => SetupControlStart();
        SetupControl.OnSynchronize += SetupControlSynchronize;
        SetupControl.OnEjectPlayer += SetupControlEjectPlayer;
        SetupControl.OnIncompatibleVersion += SetupControlIncompatibleVersion;
        SetupControl.OnSendingMaps += _ => SetupControlSendingMaps();
        session.EndRead(SetupControl);
        session.EndRead(this);
        Loaded();
        DirectPlay.Loaded();
        SetupControl.Loaded();
        session.Resolve(this);
        if (session.FireCreateEvents)
            LayoutOnCreate?.Invoke(this);
    }
}
