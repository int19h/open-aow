// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.aowSetupEvents
{
    public partial class TSetupEvents
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "SetupEvents";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_SetupEventsCreate_0;
            DPlay = new Aow.Layout.LayoutTGDPlay(this)
            {
                Name = "DPlay",
                SessionName = "No Name's Game",
                MaxPlayers = 12,
                PlayerName = "No Name",
                OnMessage = DPlayMessage,
                OnChatMessage = DPlayChatMessage,
                OnPingMessage = DPlayPingMessage,
                OnFileStartMessage = DPlayFileStartMessage,
                OnFileProgressMessage = DPlayFileProgressMessage,
                OnFileDoneMessage = DPlayFileDoneMessage,
                OnDestroyPlayer = DPlayDestroyPlayer,
                OnSessionLost = DPlaySessionLost,
                OnMessageError = Layout_DPlayMessageError_1,
                GauranteeMsgs = true,
                UseCodes = true,
                OnEjected = Layout_DPlayEjected_2,
                UseSetupDialogs = false,
                FilePacketSize = 1000
            };
            LayoutSession.EndRead(DPlay);
            SetupControl = new Aow.Units.AoWSetup.TSetupControl(this)
            {
                Name = "SetupControl",
                OnSendMap = SetupControlSendMap,
                OnSendData = SetupControlSendData,
                OnPlayerSetupDone = Layout_SetupControlPlayerSetupDone_3,
                OnSettingChanged = Layout_SetupControlSettingChanged_4,
                OnCustomizeLeader = SetupControlCustomizeLeader,
                OnLoadMap = SetupControlLoadMap,
                OnStart = Layout_SetupControlStart_5,
                OnSynchronize = SetupControlSynchronize,
                OnEjectPlayer = SetupControlEjectPlayer,
                OnIncompatibleVersion = SetupControlIncompatibleVersion,
                OnSendingMaps = Layout_SetupControlSendingMaps_6
            };
            LayoutSession.EndRead(SetupControl);
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
