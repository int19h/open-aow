// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.aowSetupEvents
{
    public partial class TSetupEvents : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "SetupEvents";
            LayoutOnCreate = Layout_SetupEventsCreate_0;
            var created__DPlay = new Aow.Layout.LayoutTGDPlay(this);
            DPlay = created__DPlay;
            session.BeginRead(created__DPlay);
            created__DPlay.Name = "DPlay";
            created__DPlay.SessionName = "No Name's Game";
            created__DPlay.MaxPlayers = 12;
            created__DPlay.PlayerName = "No Name";
            created__DPlay.OnMessage = DPlayMessage;
            created__DPlay.OnChatMessage = DPlayChatMessage;
            created__DPlay.OnPingMessage = DPlayPingMessage;
            created__DPlay.OnFileStartMessage = DPlayFileStartMessage;
            created__DPlay.OnFileProgressMessage = DPlayFileProgressMessage;
            created__DPlay.OnFileDoneMessage = DPlayFileDoneMessage;
            created__DPlay.OnDestroyPlayer = DPlayDestroyPlayer;
            created__DPlay.OnSessionLost = DPlaySessionLost;
            created__DPlay.OnMessageError = Layout_DPlayMessageError_1;
            created__DPlay.GauranteeMsgs = true;
            created__DPlay.UseCodes = true;
            created__DPlay.OnEjected = Layout_DPlayEjected_2;
            created__DPlay.UseSetupDialogs = false;
            created__DPlay.FilePacketSize = 1000;
            session.EndRead(created__DPlay);
            SetupControl = new Aow.Units.AoWSetup.TSetupControl(this);
            session.BeginRead(SetupControl);
            SetupControl.Name = "SetupControl";
            SetupControl.OnSendMap = SetupControlSendMap;
            SetupControl.OnSendData = SetupControlSendData;
            SetupControl.OnPlayerSetupDone = Layout_SetupControlPlayerSetupDone_3;
            SetupControl.OnSettingChanged = Layout_SetupControlSettingChanged_4;
            SetupControl.OnCustomizeLeader = SetupControlCustomizeLeader;
            SetupControl.OnLoadMap = SetupControlLoadMap;
            SetupControl.OnStart = Layout_SetupControlStart_5;
            SetupControl.OnSynchronize = SetupControlSynchronize;
            SetupControl.OnEjectPlayer = SetupControlEjectPlayer;
            SetupControl.OnIncompatibleVersion = SetupControlIncompatibleVersion;
            SetupControl.OnSendingMaps = Layout_SetupControlSendingMaps_6;
            session.EndRead(SetupControl);
            session.EndRead(this);
            Loaded();
            DPlay.Loaded();
            SetupControl.Loaded();
            session.Resolve(this);
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_SetupEventsCreate_0(TObject p0)
        {
            SetupEventsCreate();
        }

        private void Layout_DPlayMessageError_1(int p0, int p1)
        {
            DPlayMessageError();
        }

        private void Layout_DPlayEjected_2(TObject p0)
        {
            DPlayEjected();
        }

        private void Layout_SetupControlPlayerSetupDone_3(Aow.Units.AoWSetup.TSetupControl p0)
        {
            SetupControlPlayerSetupDone();
        }

        private void Layout_SetupControlSettingChanged_4(Aow.Units.AoWSetup.TSetupControl p0)
        {
            SetupControlSettingChanged();
        }

        private void Layout_SetupControlStart_5(Aow.Units.AoWSetup.TSetupControl p0)
        {
            SetupControlStart();
        }

        private void Layout_SetupControlSendingMaps_6(Aow.Units.AoWSetup.TSetupControl p0)
        {
            SetupControlSendingMaps();
        }
    }
}
