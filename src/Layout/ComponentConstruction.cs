// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AOWGauge
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

namespace Aow.Units.AOWInterfaceImgLib
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

namespace Aow.Units.AOWMWindow
{
    public partial class TMWindow
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
    }
}

namespace Aow.Units.AOWMenu
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

namespace Aow.Units.AOWMenuBar
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

namespace Aow.Units.AoWArmyEventDlg
{
    public partial class TArmyEventDlg
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWButton
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

namespace Aow.Units.AoWCampaignJoin
{
    public partial class TCampaignJoinDlg
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWCheckBox
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

namespace Aow.Units.AoWCityScreen
{
    public partial class TCityScreen
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWComboBox
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

namespace Aow.Units.AoWEdit
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

namespace Aow.Units.AoWEmailGameSettingsDlg
{
    public partial class TEmailGameSettingsDlg
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWFileDialog
{
    public partial class TFileDlgWindow
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWHScroll
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

namespace Aow.Units.AoWHeroJoinDlg
{
    public partial class THeroJoinDlg
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWImage
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

namespace Aow.Units.AoWLabel
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

namespace Aow.Units.AoWLeaderSetup
{
    public partial class TLeaderSetupWin
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWListBox
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

namespace Aow.Units.AoWMemo
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

namespace Aow.Units.AoWPartyJoin
{
    public partial class TPartyJoinDlg
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWProdScreen
{
    public partial class TProductionScreen
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWRelations
{
    public partial class TRelWindow
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWResultsWin
{
    public partial class TResultsWin
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWSMTPDlg
{
    public partial class TmsMessage
    {
        public string ContentType { get; set; } = "";
        public bool ReturnReceipt { get; set; }
    }
}

namespace Aow.Units.AoWSMTPDlg
{
    public partial class TmsSMTP
    {
        public int DefaultPort { get; set; }
        public Aow.Layout.SmtpError OnError { get; set; }
        public Aow.Layout.SmtpProgress OnProgress { get; set; }
    }
}

namespace Aow.Units.AoWSoundDlg
{
    public partial class TSoundDlg
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWSpellBook
{
    public partial class TSpellBook
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWTabPanel
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

namespace Aow.Units.AoWTitleScene
{
    public partial class TTitleScene
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWUnitWindow
{
    public partial class TUnitWindow
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
    }
}

namespace Aow.Units.AoWVideoDlg
{
    public partial class TVideoDialog
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWWinMan
{
    public partial class TGeneral
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Units.AoWWorldMap
{
    public partial class TWorldMap
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
    }
}

namespace Aow.Units.FastCombatWin
{
    public partial class TFastCombatWindow
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
        public Aow.Rtl.TNotifyEvent LayoutOnDestroy { get; set; }

        public override void Destroy()
        {
            Destroying();
            try
            {
                LayoutOnDestroy?.Invoke(this);
            }
            finally
            {
                base.Destroy();
            }
        }
    }
}

namespace Aow.Layout
{
    public class LayoutTIvTranslator : Aow.Rtl.TIvTranslator
    {
        public LayoutTIvTranslator(TComponent owner) : base(owner)
        {
        }
    }
}

namespace Aow.Units.TurnButton
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

namespace Aow.Layout
{
    public class LayoutTVideoPlayer : Aow.Rtl.TVideoPlayer
    {
        public LayoutTVideoPlayer(TComponent owner) : base(owner)
        {
        }

        public Aow.Layout.TVideoPlayerLoopMode Loopmode { get; set; }
        public Aow.Layout.TStreamSoundEvent OnStreamSound { get; set; }
        public Aow.Rtl.TSet<Aow.Layout.TVideoPlayerOptions_Element> Options { get; set; }
    }
}

namespace Aow.Layout
{
    public class LayoutTGDPlay : Aow.Rtl.TGDPlay
    {
        public LayoutTGDPlay(TComponent owner) : base(owner)
        {
        }

        public int FilePacketSize { get; set; }
        public bool GauranteeMsgs { get; set; }
        public int MaxPlayers { get; set; }
        public Aow.Layout.TGDChatMessageEvent OnChatMessage { get; set; }
        public Aow.Layout.TGDDestroyPlayerEvent OnDestroyPlayer { get; set; }
        public Aow.Rtl.TNotifyEvent OnEjected { get; set; }
        public Aow.Layout.TGDFileDoneMessageEvent OnFileDoneMessage { get; set; }
        public Aow.Layout.TGDFileProgressMessageEvent OnFileProgressMessage { get; set; }
        public Aow.Layout.TGDFileStartMessageEvent OnFileStartMessage { get; set; }
        public Aow.Layout.TGDMessageEvent OnMessage { get; set; }
        public Aow.Layout.TGDMessageErrorEvent OnMessageError { get; set; }
        public Aow.Layout.TGDPingMessageEvent OnPingMessage { get; set; }
        public Aow.Layout.TGDSessionLostEvent OnSessionLost { get; set; }
        public string PlayerName { get; set; } = "";
        public string SessionName { get; set; } = "";
        public bool UseCodes { get; set; }
        public bool UseSetupDialogs { get; set; }
    }
}

namespace Aow.Units.aowSetupEvents
{
    public partial class TSetupEvents
    {
        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
    }
}

namespace Aow.Layout
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

namespace Aow.Layout
{
    public enum TVideoPlayerOptions_Element
    {
        vpoUseDirectSound,
        vpoSoundEnabled,
        vpoVideoEnabled
    }
}

namespace Aow.Layout
{
    public delegate void SmtpError(Aow.Rtl.TObject sender, ref string error);
}

namespace Aow.Layout
{
    public delegate void SmtpProgress(Aow.Rtl.TObject sender, int progress, int total);
}

namespace Aow.Layout
{
    public delegate void TGDChatMessageEvent(ref string ChatMsg, ref int ToID, ref int FromID);
}

namespace Aow.Layout
{
    public delegate void TGDDestroyPlayerEvent(int ID, int NumPlayers);
}

namespace Aow.Layout
{
    public delegate void TGDFileDoneMessageEvent(int ID, Aow.Rtl.TGDFile gdFile);
}

namespace Aow.Layout
{
    public delegate void TGDFileProgressMessageEvent(int ID, int Position, int Size);
}

namespace Aow.Layout
{
    public delegate void TGDFileStartMessageEvent(int ID, ref string Name, int Size, ref byte Response);
}

namespace Aow.Layout
{
    public delegate void TGDMessageErrorEvent(int ID, int Result);
}

namespace Aow.Layout
{
    public delegate void TGDMessageEvent(ref object Data, ref int Size, ref int ToID, ref int FromID);
}

namespace Aow.Layout
{
    public delegate void TGDPingMessageEvent(int PingMS, int PingPlayer);
}

namespace Aow.Layout
{
    public delegate void TGDSessionLostEvent();
}

namespace Aow.Layout
{
    public delegate void TStreamSoundEvent(Aow.Rtl.TStreamSoundMode Mode, object Data, int DataSize);
}
