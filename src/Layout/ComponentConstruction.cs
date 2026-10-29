// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AOWChatWin
{
    public partial class TChatWindow
    {
        public TChatWindow()
        {
        }

        public TChatWindow(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AOWControlWin
{
    public partial class TControlWin
    {
        public TControlWin()
        {
        }

        public TControlWin(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AOWDialogWin
{
    public partial class TDWin
    {
        public TDWin()
        {
        }

        public TDWin(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

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
        public TMWindow()
        {
        }

        public TMWindow(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.AOWMultiWin
{
    public partial class TMultiWindow
    {
        public TMultiWindow()
        {
        }

        public TMultiWindow(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AOWPyreScrn
{
    public partial class TPyreScreen
    {
        public TPyreScreen()
        {
        }

        public TPyreScreen(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AOWTitleWindow
{
    public partial class TTitleWindow
    {
        public TTitleWindow()
        {
        }

        public TTitleWindow(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWArenaDlg
{
    public partial class TArenaDlg
    {
        public TArenaDlg()
        {
        }

        public TArenaDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWArmyEventDlg
{
    public partial class TArmyEventDlg
    {
        public TArmyEventDlg()
        {
        }

        public TArmyEventDlg(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.AoWArmyInfoDlg
{
    public partial class TArmyInfoDlg
    {
        public TArmyInfoDlg()
        {
        }

        public TArmyInfoDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWButton
{
    public partial class TAOWButton
    {
        public TAOWButton()
        {
        }

        public TAOWButton(TComponent owner)
        {
            Create__init(owner);
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

namespace Aow.Units.AoWCampaignJoin
{
    public partial class TCampaignJoinDlg
    {
        public TCampaignJoinDlg()
        {
        }

        public TCampaignJoinDlg(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.AoWCampaignSettings
{
    public partial class TCampaignSettingsDlg
    {
        public TCampaignSettingsDlg()
        {
        }

        public TCampaignSettingsDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWCampaignTransferDlg
{
    public partial class TCampaignTransferDlg
    {
        public TCampaignTransferDlg()
        {
        }

        public TCampaignTransferDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWCastUnitSpellDlg
{
    public partial class TCastUnitSpellDlg
    {
        public TCastUnitSpellDlg()
        {
        }

        public TCastUnitSpellDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWCheckBox
{
    public partial class TAOWCheckBox
    {
        public TAOWCheckBox()
        {
        }

        public TAOWCheckBox(TComponent owner)
        {
            Create__init(owner);
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

namespace Aow.Units.AoWCityJoinDlg
{
    public partial class TCityJoinDlg
    {
        public TCityJoinDlg()
        {
        }

        public TCityJoinDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWCityRebelDlg
{
    public partial class TCityRebelDlg
    {
        public TCityRebelDlg()
        {
        }

        public TCityRebelDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWCityScreen
{
    public partial class TCityScreen
    {
        public TCityScreen()
        {
        }

        public TCityScreen(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.AoWConstructDlg
{
    public partial class TConstructDlg
    {
        public TConstructDlg()
        {
        }

        public TConstructDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWDeformFaceDlg
{
    public partial class TDeformFaceDlg
    {
        public TDeformFaceDlg()
        {
        }

        public TDeformFaceDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWDisjunctionDlg
{
    public partial class TDisjunctionDlg
    {
        public TDisjunctionDlg()
        {
        }

        public TDisjunctionDlg(TComponent owner)
        {
            Create__init(owner);
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
        public TEmailGameSettingsDlg()
        {
        }

        public TEmailGameSettingsDlg(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.AoWEmailSettingsDlg
{
    public partial class TEmailSettingsDlg
    {
        public TEmailSettingsDlg()
        {
        }

        public TEmailSettingsDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWFileDialog
{
    public partial class TFileDlgWindow
    {
        public TFileDlgWindow()
        {
        }

        public TFileDlgWindow(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.AoWFrame
{
    public partial class TAoWFrame
    {
        public TAoWFrame()
        {
        }

        public TAoWFrame(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWGamePlayDlg
{
    public partial class TGamePlayDlg
    {
        public TGamePlayDlg()
        {
        }

        public TGamePlayDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWHScroll
{
    public partial class TAOWHScrollBar
    {
        public TAOWHScrollBar()
        {
        }

        public TAOWHScrollBar(TComponent owner)
        {
            Create__init(owner);
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

namespace Aow.Units.AoWHeroInfoDlg
{
    public partial class THeroInfoDlg
    {
        public THeroInfoDlg()
        {
        }

        public THeroInfoDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWHeroJoinDlg
{
    public partial class THeroJoinDlg
    {
        public THeroJoinDlg()
        {
        }

        public THeroJoinDlg(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.AoWHeroUpgradeDlg
{
    public partial class THeroUpgradeDlg
    {
        public THeroUpgradeDlg()
        {
        }

        public THeroUpgradeDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWImage
{
    public partial class TAOWImage
    {
        public TAOWImage()
        {
        }

        public TAOWImage(TComponent owner)
        {
            Create__init(owner);
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

namespace Aow.Units.AoWInfoBanner
{
    public partial class TInfoBanner
    {
        public TInfoBanner()
        {
        }

        public TInfoBanner(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWInfoWin
{
    public partial class TIWindow
    {
        public TIWindow()
        {
        }

        public TIWindow(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWIntroScene
{
    public partial class TIntroScene
    {
        public TIntroScene()
        {
        }

        public TIntroScene(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWItemBanner
{
    public partial class TItemBanner
    {
        public TItemBanner()
        {
        }

        public TItemBanner(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWItemsFoundDlg
{
    public partial class TItemsFoundDlg
    {
        public TItemsFoundDlg()
        {
        }

        public TItemsFoundDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWLabel
{
    public partial class TAOWLabel
    {
        public TAOWLabel()
        {
        }

        public TAOWLabel(TComponent owner)
        {
            Create__init(owner);
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

namespace Aow.Units.AoWLeaderSetup
{
    public partial class TLeaderSetupWin
    {
        public TLeaderSetupWin()
        {
        }

        public TLeaderSetupWin(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.AoWMagic
{
    public partial class TMagicWin
    {
        public TMagicWin()
        {
        }

        public TMagicWin(TComponent owner)
        {
            Create__init(owner);
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

namespace Aow.Units.AoWMessageDlg
{
    public partial class TMessageDlg
    {
        public TMessageDlg()
        {
        }

        public TMessageDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWMsgReceiveDlg
{
    public partial class TMsgReceiveDlg
    {
        public TMsgReceiveDlg()
        {
        }

        public TMsgReceiveDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWNewTurnDialog
{
    public partial class TNewTurnDlg
    {
        public TNewTurnDlg()
        {
        }

        public TNewTurnDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWPInfoWin
{
    public partial class TPInfoW
    {
        public TPInfoW()
        {
        }

        public TPInfoW(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWPOP3Dlg
{
    public partial class TPOP3Dlg
    {
        public TPOP3Dlg()
        {
        }

        public TPOP3Dlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWPWindows
{
    public partial class TPopupWindows
    {
        public TPopupWindows()
        {
        }

        public TPopupWindows(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWPartyJoin
{
    public partial class TPartyJoinDlg
    {
        public TPartyJoinDlg()
        {
        }

        public TPartyJoinDlg(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.AoWPasswordDlg
{
    public partial class TPasswordDlg
    {
        public TPasswordDlg()
        {
        }

        public TPasswordDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWPowerDlg
{
    public partial class TPowerDlg
    {
        public TPowerDlg()
        {
        }

        public TPowerDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWProdScreen
{
    public partial class TProductionScreen
    {
        public TProductionScreen()
        {
        }

        public TProductionScreen(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.AoWProgressDialog
{
    public partial class TProgressDialog
    {
        public TProgressDialog()
        {
        }

        public TProgressDialog(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWRealm
{
    public partial class TRealmWin
    {
        public TRealmWin()
        {
        }

        public TRealmWin(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWRelations
{
    public partial class TRelWindow
    {
        public TRelWindow()
        {
        }

        public TRelWindow(TComponent owner)
        {
            Create__init(owner);
        }

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
        public TResultsWin()
        {
        }

        public TResultsWin(TComponent owner)
        {
            Create__init(owner);
        }

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
    public partial class TSMTPDlg
    {
        public TSMTPDlg()
        {
        }

        public TSMTPDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWSMTPDlg
{
    public partial class TmsMessage
    {
        public TmsMessage()
        {
        }

        public TmsMessage(TComponent owner)
        {
            Create__init(owner);
        }

        public Aow.Rtl.AnsiString ContentType { get; set; }
        public bool ReturnReceipt { get; set; }
    }
}

namespace Aow.Units.AoWSMTPDlg
{
    public partial class TmsSMTP
    {
        public TmsSMTP()
        {
        }

        public TmsSMTP(TComponent owner)
        {
            Create__init(owner);
        }

        public int DefaultPort { get; set; }
        public Aow.Layout.SmtpError OnError { get; set; }
        public Aow.Layout.SmtpProgress OnProgress { get; set; }
    }
}

namespace Aow.Units.AoWSWindow
{
    public partial class TSWindow
    {
        public TSWindow()
        {
        }

        public TSWindow(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWScenSetup
{
    public partial class TScenSetup
    {
        public TScenSetup()
        {
        }

        public TScenSetup(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWSelFactionWin
{
    public partial class TSelFactionWin
    {
        public TSelFactionWin()
        {
        }

        public TSelFactionWin(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWSendEmailGameDlg
{
    public partial class TSendEmailGameDlg
    {
        public TSendEmailGameDlg()
        {
        }

        public TSendEmailGameDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWSoundDlg
{
    public partial class TSoundDlg
    {
        public TSoundDlg()
        {
        }

        public TSoundDlg(TComponent owner)
        {
            Create__init(owner);
        }

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
        public TSpellBook()
        {
        }

        public TSpellBook(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.AoWSyncDlg
{
    public partial class TSynchronizeDlg
    {
        public TSynchronizeDlg()
        {
        }

        public TSynchronizeDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWTBWindow
{
    public partial class TTBWindow
    {
        public TTBWindow()
        {
        }

        public TTBWindow(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWTCScanner
{
    public partial class TTCScanner
    {
        public TTCScanner()
        {
        }

        public TTCScanner(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWTCUnit
{
    public partial class TTCUnit
    {
        public TTCUnit()
        {
        }

        public TTCUnit(TComponent owner)
        {
            Create__init(owner);
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

namespace Aow.Units.AoWTargetSelection
{
    public partial class TTargetSelectionDlg
    {
        public TTargetSelectionDlg()
        {
        }

        public TTargetSelectionDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWTitleScene
{
    public partial class TTitleScene
    {
        public TTitleScene()
        {
        }

        public TTitleScene(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.AoWTutorialWin
{
    public partial class TTutorialWindow
    {
        public TTutorialWindow()
        {
        }

        public TTutorialWindow(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWUnitBanner
{
    public partial class TUnitBanner
    {
        public TUnitBanner()
        {
        }

        public TUnitBanner(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWUnitSelDlg
{
    public partial class TUnitSelectionDlg
    {
        public TUnitSelectionDlg()
        {
        }

        public TUnitSelectionDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWUnitWindow
{
    public partial class TUnitWindow
    {
        public TUnitWindow()
        {
        }

        public TUnitWindow(TComponent owner)
        {
            Create__init(owner);
        }

        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
    }
}

namespace Aow.Units.AoWVScroll
{
    public partial class TAOWVScrollBar
    {
        public TAOWVScrollBar()
        {
        }

        public TAOWVScrollBar(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWVideoDlg
{
    public partial class TVideoDialog
    {
        public TVideoDialog()
        {
        }

        public TVideoDialog(TComponent owner)
        {
            Create__init(owner);
        }

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
        public TGeneral()
        {
        }

        public TGeneral(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.AoWWizardsTowerDlg
{
    public partial class TWizardsTowerDlg
    {
        public TWizardsTowerDlg()
        {
        }

        public TWizardsTowerDlg(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.AoWWorldMap
{
    public partial class TWorldMap
    {
        public TWorldMap()
        {
        }

        public TWorldMap(TComponent owner)
        {
            Create__init(owner);
        }

        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
    }
}

namespace Aow.Units.AowUserWindow
{
    public partial class TAoWUserWindow
    {
        public TAoWUserWindow()
        {
        }

        public TAoWUserWindow(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.FastCombatWin
{
    public partial class TFastCombatWindow
    {
        public TFastCombatWindow()
        {
        }

        public TFastCombatWindow(TComponent owner)
        {
            Create__init(owner);
        }

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

namespace Aow.Units.FontMod
{
    public partial class TFontModule
    {
        public TFontModule()
        {
        }

        public TFontModule(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Layout
{
    public class LayoutTIvTranslator : Aow.Rtl.TIvTranslator
    {
        public LayoutTIvTranslator(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.StartMenu
{
    public partial class TStartMenu
    {
        public TStartMenu()
        {
        }

        public TStartMenu(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.TCFCDlg
{
    public partial class TTCFCDialog
    {
        public TTCFCDialog()
        {
        }

        public TTCFCDialog(TComponent owner)
        {
            Create__init(owner);
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
        public LayoutTVideoPlayer(TComponent owner)
        {
            Create__init(owner);
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
        public LayoutTGDPlay(TComponent owner)
        {
            Create__init(owner);
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
        public Aow.Rtl.AnsiString PlayerName { get; set; }
        public Aow.Rtl.AnsiString SessionName { get; set; }
        public bool UseCodes { get; set; }
        public bool UseSetupDialogs { get; set; }
    }
}

namespace Aow.Units.aowHelpWin
{
    public partial class THelpWindow
    {
        public THelpWindow()
        {
        }

        public THelpWindow(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.aowIntGfxMod
{
    public partial class TIntGfxMod
    {
        public TIntGfxMod()
        {
        }

        public TIntGfxMod(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.aowMapEvents
{
    public partial class TMapEvents
    {
        public TMapEvents()
        {
        }

        public TMapEvents(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.aowSetupEvents
{
    public partial class TSetupEvents
    {
        public TSetupEvents()
        {
        }

        public TSetupEvents(TComponent owner)
        {
            Create__init(owner);
        }

        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
    }
}

namespace Aow.Units.aowTCMapEvents
{
    public partial class TTCMapEvents
    {
        public TTCMapEvents()
        {
        }

        public TTCMapEvents(TComponent owner)
        {
            Create__init(owner);
        }
    }
}

namespace Aow.Units.aowUserDrawBox
{
    public partial class TaowUserDrawBox
    {
        public TaowUserDrawBox()
        {
        }

        public TaowUserDrawBox(TComponent owner)
        {
            Create__init(owner);
        }
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
    public delegate void SmtpError(Aow.Rtl.TObject sender, ref Aow.Rtl.AnsiString error);
}

namespace Aow.Layout
{
    public delegate void SmtpProgress(Aow.Rtl.TObject sender, int progress, int total);
}

namespace Aow.Layout
{
    public delegate void TGDChatMessageEvent(ref Aow.Rtl.AnsiString ChatMsg, ref int ToID, ref int FromID);
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
    public delegate void TGDFileStartMessageEvent(int ID, ref Aow.Rtl.AnsiString Name, int Size, ref byte Response);
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
