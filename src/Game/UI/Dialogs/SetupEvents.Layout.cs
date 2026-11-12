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
        LayoutDirectPlay createdDPlay = new LayoutDirectPlay(this);
        DPlay = createdDPlay;
        session.BeginRead(createdDPlay);
        createdDPlay.Name = "DPlay";
        createdDPlay.SessionName = "No Name's Game";
        createdDPlay.MaxPlayers = 12;
        createdDPlay.PlayerName = "No Name";
        createdDPlay.OnMessage = DPlayMessage;
        createdDPlay.OnChatMessage = DPlayChatMessage;
        createdDPlay.OnPingMessage = DPlayPingMessage;
        createdDPlay.OnFileStartMessage = DPlayFileStartMessage;
        createdDPlay.OnFileProgressMessage = DPlayFileProgressMessage;
        createdDPlay.OnFileDoneMessage = DPlayFileDoneMessage;
        createdDPlay.OnDestroyPlayer = DPlayDestroyPlayer;
        createdDPlay.OnSessionLost = DPlaySessionLost;
        createdDPlay.OnMessageError = Layout_DPlayMessageError_1;
        createdDPlay.GuaranteeMessages = true;
        createdDPlay.UseCodes = true;
        createdDPlay.OnEjected = _ => Layout_DPlayEjected_2();
        createdDPlay.UseSetupDialogs = false;
        createdDPlay.FilePacketSize = 1000;
        session.EndRead(createdDPlay);
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
        DPlay.Loaded();
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
