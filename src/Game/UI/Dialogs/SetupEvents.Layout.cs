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
        LayoutOnCreate = _ => Layout_SetupEventsCreate_0();
        LayoutDirectPlay createdDirectPlay = new LayoutDirectPlay(this);
        DirectPlay = createdDirectPlay;
        session.BeginRead(createdDirectPlay);
        createdDirectPlay.Name = "DPlay";
        createdDirectPlay.SessionName = "No Name's Game";
        createdDirectPlay.MaxPlayers = 12;
        createdDirectPlay.PlayerName = "No Name";
        createdDirectPlay.OnMessage = DPlayMessage;
        createdDirectPlay.OnChatMessage = DPlayChatMessage;
        createdDirectPlay.OnPingMessage = DPlayPingMessage;
        createdDirectPlay.OnFileStartMessage = DPlayFileStartMessage;
        createdDirectPlay.OnFileProgressMessage = DPlayFileProgressMessage;
        createdDirectPlay.OnFileDoneMessage = DPlayFileDoneMessage;
        createdDirectPlay.OnDestroyPlayer = DPlayDestroyPlayer;
        createdDirectPlay.OnSessionLost = DPlaySessionLost;
        createdDirectPlay.OnMessageError = Layout_DPlayMessageError_1;
        createdDirectPlay.GuaranteeMessages = true;
        createdDirectPlay.UseCodes = true;
        createdDirectPlay.OnEjected = _ => Layout_DPlayEjected_2();
        createdDirectPlay.UseSetupDialogs = false;
        createdDirectPlay.FilePacketSize = 1000;
        session.EndRead(createdDirectPlay);
        SetupControl = new SetupControl(this);
        session.BeginRead(SetupControl);
        SetupControl.Name = "SetupControl";
        SetupControl.OnSendMap = SetupControlSendMap;
        SetupControl.OnSendData = SetupControlSendData;
        SetupControl.OnPlayerSetupDone = _ => Layout_SetupControlPlayerSetupDone_3();
        SetupControl.OnSettingChanged = _ => Layout_SetupControlSettingChanged_4();
        SetupControl.OnCustomizeLeader = SetupControlCustomizeLeader;
        SetupControl.OnLoadMap = SetupControlLoadMap;
        SetupControl.OnStart = _ => Layout_SetupControlStart_5();
        SetupControl.OnSynchronize = SetupControlSynchronize;
        SetupControl.OnEjectPlayer = SetupControlEjectPlayer;
        SetupControl.OnIncompatibleVersion = SetupControlIncompatibleVersion;
        SetupControl.OnSendingMaps = _ => Layout_SetupControlSendingMaps_6();
        session.EndRead(SetupControl);
        session.EndRead(this);
        Loaded();
        DirectPlay.Loaded();
        SetupControl.Loaded();
        session.Resolve(this);
        if (session.FireCreateEvents)
            LayoutOnCreate?.Invoke(this);
    }

    private void Layout_SetupEventsCreate_0()
    {
        SetupEventsCreate();
    }

    private void Layout_DPlayMessageError_1(int p0, int p1)
    {
        DPlayMessageError();
    }

    private void Layout_DPlayEjected_2()
    {
        DPlayEjected();
    }

    private void Layout_SetupControlPlayerSetupDone_3()
    {
        SetupControlPlayerSetupDone();
    }

    private void Layout_SetupControlSettingChanged_4()
    {
        SetupControlSettingChanged();
    }

    private void Layout_SetupControlStart_5()
    {
        SetupControlStart();
    }

    private void Layout_SetupControlSendingMaps_6()
    {
        SetupControlSendingMaps();
    }
}
