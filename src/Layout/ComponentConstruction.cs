// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Controls
{
    public partial class AoWGauge
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
    public partial class InterfaceIL
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
    public partial class MWindow
    {
        public NotifyEvent<MWindow> LayoutOnCreate { get; set; }
    }
}

namespace Aow.Game.Ui.Windows
{
    public partial class AOWMenu
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
    public partial class AOWMenuBar
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
    public partial class ArmyEventDlg
    {
        public NotifyEvent<ArmyEventDlg> LayoutOnCreate { get; set; }
        public NotifyEvent<ArmyEventDlg> LayoutOnDestroy { get; set; }

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
    public partial class AOWButton
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
    public partial class CampaignJoinDlg
    {
        public NotifyEvent<CampaignJoinDlg> LayoutOnCreate { get; set; }
        public NotifyEvent<CampaignJoinDlg> LayoutOnDestroy { get; set; }

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
    public partial class AOWCheckBox
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
    public partial class CityScreen
    {
        public NotifyEvent<CityScreen> LayoutOnCreate { get; set; }
        public NotifyEvent<CityScreen> LayoutOnDestroy { get; set; }

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
    public partial class AOWComboBox
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
    public partial class AOWEdit
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
    public partial class EmailGameSettingsDlg
    {
        public NotifyEvent<EmailGameSettingsDlg> LayoutOnCreate { get; set; }
        public NotifyEvent<EmailGameSettingsDlg> LayoutOnDestroy { get; set; }

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
    public partial class FileDlgWindow
    {
        public NotifyEvent<FileDlgWindow> LayoutOnCreate { get; set; }
        public NotifyEvent<FileDlgWindow> LayoutOnDestroy { get; set; }

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
    public partial class AOWHScrollBar
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
    public partial class HeroJoinDlg
    {
        public NotifyEvent<HeroJoinDlg> LayoutOnCreate { get; set; }
        public NotifyEvent<HeroJoinDlg> LayoutOnDestroy { get; set; }

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
    public partial class AOWImage
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
    public partial class AOWLabel
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
    public partial class LeaderSetupWin
    {
        public NotifyEvent<LeaderSetupWin> LayoutOnCreate { get; set; }
        public NotifyEvent<LeaderSetupWin> LayoutOnDestroy { get; set; }

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
    public partial class AOWListBox
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
    public partial class AOWMemo
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
    public partial class PartyJoinDlg
    {
        public NotifyEvent<PartyJoinDlg> LayoutOnCreate { get; set; }
        public NotifyEvent<PartyJoinDlg> LayoutOnDestroy { get; set; }

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
    public partial class ProductionScreen
    {
        public NotifyEvent<ProductionScreen> LayoutOnCreate { get; set; }
        public NotifyEvent<ProductionScreen> LayoutOnDestroy { get; set; }

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
    public partial class RelWindow
    {
        public NotifyEvent<RelWindow> LayoutOnCreate { get; set; }
        public NotifyEvent<RelWindow> LayoutOnDestroy { get; set; }

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
    public partial class ResultsWin
    {
        public NotifyEvent<ResultsWin> LayoutOnCreate { get; set; }
        public NotifyEvent<ResultsWin> LayoutOnDestroy { get; set; }

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
    public partial class SoundDlg
    {
        public NotifyEvent<SoundDlg> LayoutOnCreate { get; set; }
        public NotifyEvent<SoundDlg> LayoutOnDestroy { get; set; }

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
    public partial class SpellBook
    {
        public NotifyEvent<SpellBook> LayoutOnCreate { get; set; }
        public NotifyEvent<SpellBook> LayoutOnDestroy { get; set; }

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
    public partial class AOWTabPanel
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
    public partial class TitleScene
    {
        public NotifyEvent<TitleScene> LayoutOnCreate { get; set; }
        public NotifyEvent<TitleScene> LayoutOnDestroy { get; set; }

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
    public partial class UnitWindow
    {
        public NotifyEvent<UnitWindow> LayoutOnCreate { get; set; }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class VideoDialog
    {
        public NotifyEvent<VideoDialog> LayoutOnCreate { get; set; }
        public NotifyEvent<VideoDialog> LayoutOnDestroy { get; set; }

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
    public partial class General
    {
        public NotifyEvent<General> LayoutOnCreate { get; set; }
        public NotifyEvent<General> LayoutOnDestroy { get; set; }

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
    public partial class WorldMap
    {
        public NotifyEvent<WorldMap> LayoutOnCreate { get; set; }
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class FastCombatWindow
    {
        public NotifyEvent<FastCombatWindow> LayoutOnCreate { get; set; }
        public NotifyEvent<FastCombatWindow> LayoutOnDestroy { get; set; }

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
    public class LayoutTIvTranslator : IvTranslator
    {
        public LayoutTIvTranslator(global::Aow.Rtl.Component owner) : base(owner)
        {
        }
    }
}

namespace Aow.Game.Ui.Grids
{
    public partial class AOWTurnButton
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
    public class LayoutTVideoPlayer : VideoPlayer
    {
        public LayoutTVideoPlayer(global::Aow.Rtl.Component owner) : base(owner)
        {
        }

        public VideoPlayerLoopMode Loopmode { get; set; }
        public StreamSoundEvent OnStreamSound { get; set; }
        public Set<VideoPlayerOptions_Element> Options { get; set; }

        public enum VideoPlayerLoopMode
        {
            None, LoopLongest, LoopVideo, LoopSound, LoopIndependent
        }

        public delegate void StreamSoundEvent(StreamSoundMode Mode, object Data, int DataSize);
    }
}

namespace Aow.Game.Layout
{
    public class LayoutTGDPlay : GDPlay
    {
        public LayoutTGDPlay(global::Aow.Rtl.Component owner) : base(owner)
        {
        }

        public int FilePacketSize { get; set; }
        public bool GauranteeMsgs { get; set; }
        public int MaxPlayers { get; set; }
        public GDChatMessageEvent OnChatMessage { get; set; }
        public GDDestroyPlayerEvent OnDestroyPlayer { get; set; }
        public NotifyEvent<LayoutTGDPlay> OnEjected { get; set; }
        public GDFileDoneMessageEvent OnFileDoneMessage { get; set; }
        public GDFileProgressMessageEvent OnFileProgressMessage { get; set; }
        public GDFileStartMessageEvent OnFileStartMessage { get; set; }
        public GDMessageEvent OnMessage { get; set; }
        public GDMessageErrorEvent OnMessageError { get; set; }
        public GDPingMessageEvent OnPingMessage { get; set; }
        public GDSessionLostEvent OnSessionLost { get; set; }
        public bool UseCodes { get; set; }
        public bool UseSetupDialogs { get; set; }

        public delegate void GDChatMessageEvent(ref string ChatMsg, ref int ToID, ref int FromID);
        public delegate void GDDestroyPlayerEvent(int ID, int NumPlayers);
        public delegate void GDFileDoneMessageEvent(int ID, GDFile gdFile);
        public delegate void GDFileProgressMessageEvent(int ID, int Position, int Size);
        public delegate void GDFileStartMessageEvent(int ID, ref string Name, int Size, ref byte Response);
        public delegate void GDMessageErrorEvent(int ID, int Result);
        public delegate void GDMessageEvent(ref object Data, ref int Size, ref int ToID, ref int FromID);
        public delegate void GDPingMessageEvent(int PingMS, int PingPlayer);
        public delegate void GDSessionLostEvent();
    }
}

namespace Aow.Game.Ui.Dialogs
{
    public partial class SetupEvents
    {
        public NotifyEvent<SetupEvents> LayoutOnCreate { get; set; }
    }
}

namespace Aow.Game.Layout
{
    public enum VideoPlayerOptions_Element
    {
        UseDirectSound,
        SoundEnabled,
        VideoEnabled
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
