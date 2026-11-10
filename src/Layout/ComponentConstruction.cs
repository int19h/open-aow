// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Controls
{
    public partial class TAoWGauge
    {
        public LayoutStrings LayoutToolTipText
        {
            set
            {
                value.CopyTo(ToolTipText);
            }
        }
    }
}

namespace Aow.Game.Draw.Images
{
    public partial class TInterfaceIL
    {
        public byte[] LayoutData
        {
            set
            {
                LayoutSession.ReadImageData(this, value);
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TMWindow
    {
        public TNotifyEvent<TMWindow> LayoutOnCreate { get; set; }
    }
}

namespace Aow.Game.Ui.Windows
{
    public partial class TAOWMenu
    {
        public LayoutStrings LayoutItems
        {
            set
            {
                value.CopyTo(Items);
            }
        }
    }
}

namespace Aow.Game.Ui.Controls
{
    public partial class TAOWMenuBar
    {
        public LayoutStrings LayoutItems
        {
            set
            {
                value.CopyTo(Items);
            }
        }

        public LayoutStrings LayoutToolTipText
        {
            set
            {
                value.CopyTo(ToolTipText);
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TArmyEventDlg
    {
        public TNotifyEvent<TArmyEventDlg> LayoutOnCreate { get; set; }
        public TNotifyEvent<TArmyEventDlg> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Controls
{
    public partial class TAOWButton
    {
        public LayoutStrings LayoutToolTipText
        {
            set
            {
                value.CopyTo(ToolTipText);
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TCampaignJoinDlg
    {
        public TNotifyEvent<TCampaignJoinDlg> LayoutOnCreate { get; set; }
        public TNotifyEvent<TCampaignJoinDlg> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Controls
{
    public partial class TAOWCheckBox
    {
        public LayoutStrings LayoutToolTipText
        {
            set
            {
                value.CopyTo(ToolTipText);
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TCityScreen
    {
        public TNotifyEvent<TCityScreen> LayoutOnCreate { get; set; }
        public TNotifyEvent<TCityScreen> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Controls
{
    public partial class TAOWComboBox
    {
        public LayoutStrings LayoutItems
        {
            set
            {
                value.CopyTo(Items);
            }
        }
    }
}

namespace Aow.Game.Ui.Controls
{
    public partial class TAOWEdit
    {
        public LayoutStrings LayoutToolTipText
        {
            set
            {
                value.CopyTo(ToolTipText);
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TEmailGameSettingsDlg
    {
        public TNotifyEvent<TEmailGameSettingsDlg> LayoutOnCreate { get; set; }
        public TNotifyEvent<TEmailGameSettingsDlg> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TFileDlgWindow
    {
        public TNotifyEvent<TFileDlgWindow> LayoutOnCreate { get; set; }
        public TNotifyEvent<TFileDlgWindow> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Controls
{
    public partial class TAOWHScrollBar
    {
        public LayoutStrings LayoutToolTipText
        {
            set
            {
                value.CopyTo(ToolTipText);
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class THeroJoinDlg
    {
        public TNotifyEvent<THeroJoinDlg> LayoutOnCreate { get; set; }
        public TNotifyEvent<THeroJoinDlg> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Controls
{
    public partial class TAOWImage
    {
        public LayoutStrings LayoutToolTipText
        {
            set
            {
                value.CopyTo(ToolTipText);
            }
        }
    }
}

namespace Aow.Game.Ui.Controls
{
    public partial class TAOWLabel
    {
        public LayoutStrings LayoutToolTipText
        {
            set
            {
                value.CopyTo(ToolTipText);
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TLeaderSetupWin
    {
        public TNotifyEvent<TLeaderSetupWin> LayoutOnCreate { get; set; }
        public TNotifyEvent<TLeaderSetupWin> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Controls
{
    public partial class TAOWListBox
    {
        public LayoutStrings LayoutItems
        {
            set
            {
                value.CopyTo(Items);
            }
        }

        public LayoutStrings LayoutToolTipText
        {
            set
            {
                value.CopyTo(ToolTipText);
            }
        }
    }
}

namespace Aow.Game.Ui.Controls
{
    public partial class TAOWMemo
    {
        public LayoutStrings LayoutItems
        {
            set
            {
                value.CopyTo(Items);
            }
        }

        public LayoutStrings LayoutToolTipText
        {
            set
            {
                value.CopyTo(ToolTipText);
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TPartyJoinDlg
    {
        public TNotifyEvent<TPartyJoinDlg> LayoutOnCreate { get; set; }
        public TNotifyEvent<TPartyJoinDlg> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TProductionScreen
    {
        public TNotifyEvent<TProductionScreen> LayoutOnCreate { get; set; }
        public TNotifyEvent<TProductionScreen> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TRelWindow
    {
        public TNotifyEvent<TRelWindow> LayoutOnCreate { get; set; }
        public TNotifyEvent<TRelWindow> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TResultsWin
    {
        public TNotifyEvent<TResultsWin> LayoutOnCreate { get; set; }
        public TNotifyEvent<TResultsWin> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Net
{
    public partial class TmsMessage
    {
        public string ContentType { get; set; } = "";
        public bool ReturnReceipt { get; set; }
    }
}

namespace Aow.Game.Net
{
    public partial class TmsSMTP
    {
        public int DefaultPort { get; set; }
        public SmtpError<TmsSMTP> OnError { get; set; }
        public SmtpProgress<TmsSMTP> OnProgress { get; set; }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TSoundDlg
    {
        public TNotifyEvent<TSoundDlg> LayoutOnCreate { get; set; }
        public TNotifyEvent<TSoundDlg> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TSpellBook
    {
        public TNotifyEvent<TSpellBook> LayoutOnCreate { get; set; }
        public TNotifyEvent<TSpellBook> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Controls
{
    public partial class TAOWTabPanel
    {
        public LayoutStrings LayoutItems
        {
            set
            {
                value.CopyTo(Items);
            }
        }

        public LayoutStrings LayoutToolTipText
        {
            set
            {
                value.CopyTo(ToolTipText);
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TTitleScene
    {
        public TNotifyEvent<TTitleScene> LayoutOnCreate { get; set; }
        public TNotifyEvent<TTitleScene> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TUnitWindow
    {
        public TNotifyEvent<TUnitWindow> LayoutOnCreate { get; set; }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TVideoDialog
    {
        public TNotifyEvent<TVideoDialog> LayoutOnCreate { get; set; }
        public TNotifyEvent<TVideoDialog> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TGeneral
    {
        public TNotifyEvent<TGeneral> LayoutOnCreate { get; set; }
        public TNotifyEvent<TGeneral> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TWorldMap
    {
        public TNotifyEvent<TWorldMap> LayoutOnCreate { get; set; }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TFastCombatWindow
    {
        public TNotifyEvent<TFastCombatWindow> LayoutOnCreate { get; set; }
        public TNotifyEvent<TFastCombatWindow> LayoutOnDestroy { get; set; }

        public override void Dispose()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}

namespace Aow.Game.Layout
{
    public class LayoutTIvTranslator : TIvTranslator
    {
        public LayoutTIvTranslator(TComponent owner) : base(owner)
        {
        }
    }
}

namespace Aow.Game.Ui.Grids
{
    public partial class TAOWTurnButton
    {
        public LayoutStrings LayoutToolTipText
        {
            set
            {
                value.CopyTo(ToolTipText);
            }
        }
    }
}

namespace Aow.Game.Layout
{
    public class LayoutTVideoPlayer : TVideoPlayer
    {
        public LayoutTVideoPlayer(TComponent owner) : base(owner)
        {
        }

        public TVideoPlayerLoopMode Loopmode { get; set; }
        public TStreamSoundEvent OnStreamSound { get; set; }
        public TSet<TVideoPlayerOptions_Element> Options { get; set; }
    }
}

namespace Aow.Game.Layout
{
    public class LayoutTGDPlay : TGDPlay
    {
        public LayoutTGDPlay(TComponent owner) : base(owner)
        {
        }

        public int FilePacketSize { get; set; }
        public bool GauranteeMsgs { get; set; }
        public int MaxPlayers { get; set; }
        public TGDChatMessageEvent OnChatMessage { get; set; }
        public TGDDestroyPlayerEvent OnDestroyPlayer { get; set; }
        public TNotifyEvent<LayoutTGDPlay> OnEjected { get; set; }
        public TGDFileDoneMessageEvent OnFileDoneMessage { get; set; }
        public TGDFileProgressMessageEvent OnFileProgressMessage { get; set; }
        public TGDFileStartMessageEvent OnFileStartMessage { get; set; }
        public TGDMessageEvent OnMessage { get; set; }
        public TGDMessageErrorEvent OnMessageError { get; set; }
        public TGDPingMessageEvent OnPingMessage { get; set; }
        public TGDSessionLostEvent OnSessionLost { get; set; }
        public bool UseCodes { get; set; }
        public bool UseSetupDialogs { get; set; }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class TSetupEvents
    {
        public TNotifyEvent<TSetupEvents> LayoutOnCreate { get; set; }
    }
}

namespace Aow.Game.Layout
{
    public enum TVideoPlayerLoopMode
    {
        vplmNone,
        vplmLoopLongest,
        vplmLoopVideo,
        vplmLoopSound,
        vplmLoopIndependent
    }
}

namespace Aow.Game.Layout
{
    public enum TVideoPlayerOptions_Element
    {
        vpoUseDirectSound,
        vpoSoundEnabled,
        vpoVideoEnabled
    }
}

namespace Aow.Game.Layout
{
    public delegate void SmtpError<in TSender>(TSender sender, ref string error);
}

namespace Aow.Game.Layout
{
    public delegate void SmtpProgress<in TSender>(TSender sender, int progress, int total);
}

namespace Aow.Game.Layout
{
    public delegate void TGDChatMessageEvent(ref string ChatMsg, ref int ToID, ref int FromID);
}

namespace Aow.Game.Layout
{
    public delegate void TGDDestroyPlayerEvent(int ID, int NumPlayers);
}

namespace Aow.Game.Layout
{
    public delegate void TGDFileDoneMessageEvent(int ID, TGDFile gdFile);
}

namespace Aow.Game.Layout
{
    public delegate void TGDFileProgressMessageEvent(int ID, int Position, int Size);
}

namespace Aow.Game.Layout
{
    public delegate void TGDFileStartMessageEvent(int ID, ref string Name, int Size, ref byte Response);
}

namespace Aow.Game.Layout
{
    public delegate void TGDMessageErrorEvent(int ID, int Result);
}

namespace Aow.Game.Layout
{
    public delegate void TGDMessageEvent(ref object Data, ref int Size, ref int ToID, ref int FromID);
}

namespace Aow.Game.Layout
{
    public delegate void TGDPingMessageEvent(int PingMS, int PingPlayer);
}

namespace Aow.Game.Layout
{
    public delegate void TGDSessionLostEvent();
}

namespace Aow.Game.Layout
{
    public delegate void TStreamSoundEvent(TStreamSoundMode Mode, object Data, int DataSize);
}
