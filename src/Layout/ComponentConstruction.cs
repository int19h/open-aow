// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AOWChatWin
{
    public partial class TChatWindow : ILayoutComponent
    {
        public TChatWindow()
        {
        }

        public TChatWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AOWControlWin
{
    public partial class TControlWin : ILayoutComponent
    {
        public TControlWin()
        {
        }

        public TControlWin(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AOWDialogWin
{
    public partial class TDWin : ILayoutComponent
    {
        public TDWin()
        {
        }

        public TDWin(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AOWGauge
{
    public partial class TAoWGauge : ILayoutComponent
    {
        public TAoWGauge()
        {
        }

        public TAoWGauge(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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

namespace Aow.Units.AOWInterfaceImgLib
{
    public partial class TInterfaceIL : ILayoutComponent
    {
        public TInterfaceIL()
        {
        }

        public TInterfaceIL(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

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
    public partial class TMWindow : ILayoutComponent
    {
        public TMWindow()
        {
        }

        public TMWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
    }
}

namespace Aow.Units.AOWMenu
{
    public partial class TAOWMenu : ILayoutComponent
    {
        public TAOWMenu()
        {
        }

        public TAOWMenu(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

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
    public partial class TAOWMenuBar : ILayoutComponent
    {
        public TAOWMenuBar()
        {
        }

        public TAOWMenuBar(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

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
    public partial class TMultiWindow : ILayoutComponent
    {
        public TMultiWindow()
        {
        }

        public TMultiWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AOWPyreScrn
{
    public partial class TPyreScreen : ILayoutComponent
    {
        public TPyreScreen()
        {
        }

        public TPyreScreen(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AOWTitleWindow
{
    public partial class TTitleWindow : ILayoutComponent
    {
        public TTitleWindow()
        {
        }

        public TTitleWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AOWToolTip
{
    public partial class TAOWToolTip : ILayoutComponent
    {
        public TAOWToolTip()
        {
        }

        public TAOWToolTip(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AOWWinManager
{
    public partial class TAOWWinManager : ILayoutComponent
    {
        public TAOWWinManager()
        {
        }

        public TAOWWinManager(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWArenaDlg
{
    public partial class TArenaDlg : ILayoutComponent
    {
        public TArenaDlg()
        {
        }

        public TArenaDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWArmyEventDlg
{
    public partial class TArmyEventDlg : ILayoutComponent
    {
        public TArmyEventDlg()
        {
        }

        public TArmyEventDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TArmyInfoDlg : ILayoutComponent
    {
        public TArmyInfoDlg()
        {
        }

        public TArmyInfoDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWArnoLine
{
    public partial class TAoWArnoLine : ILayoutComponent
    {
        public TAoWArnoLine()
        {
        }

        public TAoWArnoLine(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWBookWindow
{
    public partial class TAoWBookWindow : ILayoutComponent
    {
        public TAoWBookWindow()
        {
        }

        public TAoWBookWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWButton
{
    public partial class TAOWButton : ILayoutComponent
    {
        public TAOWButton()
        {
        }

        public TAOWButton(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TCampaignJoinDlg : ILayoutComponent
    {
        public TCampaignJoinDlg()
        {
        }

        public TCampaignJoinDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TCampaignSettingsDlg : ILayoutComponent
    {
        public TCampaignSettingsDlg()
        {
        }

        public TCampaignSettingsDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWCampaignTransferDlg
{
    public partial class TCampaignTransferDlg : ILayoutComponent
    {
        public TCampaignTransferDlg()
        {
        }

        public TCampaignTransferDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWCastUnitSpellDlg
{
    public partial class TCastUnitSpellDlg : ILayoutComponent
    {
        public TCastUnitSpellDlg()
        {
        }

        public TCastUnitSpellDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWCheckBox
{
    public partial class TAOWCheckBox : ILayoutComponent
    {
        public TAOWCheckBox()
        {
        }

        public TAOWCheckBox(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TCityJoinDlg : ILayoutComponent
    {
        public TCityJoinDlg()
        {
        }

        public TCityJoinDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWCityRebelDlg
{
    public partial class TCityRebelDlg : ILayoutComponent
    {
        public TCityRebelDlg()
        {
        }

        public TCityRebelDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWCityScreen
{
    public partial class TCityScreen : ILayoutComponent
    {
        public TCityScreen()
        {
        }

        public TCityScreen(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TAOWComboBox : ILayoutComponent
    {
        public TAOWComboBox()
        {
        }

        public TAOWComboBox(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

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
    public partial class TConstructDlg : ILayoutComponent
    {
        public TConstructDlg()
        {
        }

        public TConstructDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWDeformFaceDlg
{
    public partial class TDeformFaceDlg : ILayoutComponent
    {
        public TDeformFaceDlg()
        {
        }

        public TDeformFaceDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWDisjunctionDlg
{
    public partial class TDisjunctionDlg : ILayoutComponent
    {
        public TDisjunctionDlg()
        {
        }

        public TDisjunctionDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWEdit
{
    public partial class TAOWEdit : ILayoutComponent
    {
        public TAOWEdit()
        {
        }

        public TAOWEdit(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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

namespace Aow.Units.AoWEmailGameSettingsDlg
{
    public partial class TEmailGameSettingsDlg : ILayoutComponent
    {
        public TEmailGameSettingsDlg()
        {
        }

        public TEmailGameSettingsDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TEmailSettingsDlg : ILayoutComponent
    {
        public TEmailSettingsDlg()
        {
        }

        public TEmailSettingsDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWFileDialog
{
    public partial class TFileDlgWindow : ILayoutComponent
    {
        public TFileDlgWindow()
        {
        }

        public TFileDlgWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TAoWFrame : ILayoutComponent
    {
        public TAoWFrame()
        {
        }

        public TAoWFrame(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWGamePlayDlg
{
    public partial class TGamePlayDlg : ILayoutComponent
    {
        public TGamePlayDlg()
        {
        }

        public TGamePlayDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWHScroll
{
    public partial class TAOWHScrollBar : ILayoutComponent
    {
        public TAOWHScrollBar()
        {
        }

        public TAOWHScrollBar(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class THeroInfoDlg : ILayoutComponent
    {
        public THeroInfoDlg()
        {
        }

        public THeroInfoDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWHeroJoinDlg
{
    public partial class THeroJoinDlg : ILayoutComponent
    {
        public THeroJoinDlg()
        {
        }

        public THeroJoinDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class THeroUpgradeDlg : ILayoutComponent
    {
        public THeroUpgradeDlg()
        {
        }

        public THeroUpgradeDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWImage
{
    public partial class TAOWImage : ILayoutComponent
    {
        public TAOWImage()
        {
        }

        public TAOWImage(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TInfoBanner : ILayoutComponent
    {
        public TInfoBanner()
        {
        }

        public TInfoBanner(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWInfoWin
{
    public partial class TIWindow : ILayoutComponent
    {
        public TIWindow()
        {
        }

        public TIWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWIntroScene
{
    public partial class TIntroScene : ILayoutComponent
    {
        public TIntroScene()
        {
        }

        public TIntroScene(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWItemBanner
{
    public partial class TItemBanner : ILayoutComponent
    {
        public TItemBanner()
        {
        }

        public TItemBanner(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWItemsFoundDlg
{
    public partial class TItemsFoundDlg : ILayoutComponent
    {
        public TItemsFoundDlg()
        {
        }

        public TItemsFoundDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWLabel
{
    public partial class TAOWLabel : ILayoutComponent
    {
        public TAOWLabel()
        {
        }

        public TAOWLabel(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TLeaderSetupWin : ILayoutComponent
    {
        public TLeaderSetupWin()
        {
        }

        public TLeaderSetupWin(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TAOWListBox : ILayoutComponent
    {
        public TAOWListBox()
        {
        }

        public TAOWListBox(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

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
    public partial class TMagicWin : ILayoutComponent
    {
        public TMagicWin()
        {
        }

        public TMagicWin(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWMemo
{
    public partial class TAOWMemo : ILayoutComponent
    {
        public TAOWMemo()
        {
        }

        public TAOWMemo(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

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
    public partial class TMessageDlg : ILayoutComponent
    {
        public TMessageDlg()
        {
        }

        public TMessageDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWMsgReceiveDlg
{
    public partial class TMsgReceiveDlg : ILayoutComponent
    {
        public TMsgReceiveDlg()
        {
        }

        public TMsgReceiveDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWNewTurnDialog
{
    public partial class TNewTurnDlg : ILayoutComponent
    {
        public TNewTurnDlg()
        {
        }

        public TNewTurnDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWPInfoWin
{
    public partial class TPInfoW : ILayoutComponent
    {
        public TPInfoW()
        {
        }

        public TPInfoW(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWPOP3Dlg
{
    public partial class TPOP3Dlg : ILayoutComponent
    {
        public TPOP3Dlg()
        {
        }

        public TPOP3Dlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWPWindows
{
    public partial class TPopupWindows : ILayoutComponent
    {
        public TPopupWindows()
        {
        }

        public TPopupWindows(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWPanel
{
    public partial class TAOWPanel : ILayoutComponent
    {
        public TAOWPanel()
        {
        }

        public TAOWPanel(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWPartyJoin
{
    public partial class TPartyJoinDlg : ILayoutComponent
    {
        public TPartyJoinDlg()
        {
        }

        public TPartyJoinDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TPasswordDlg : ILayoutComponent
    {
        public TPasswordDlg()
        {
        }

        public TPasswordDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWPowerDlg
{
    public partial class TPowerDlg : ILayoutComponent
    {
        public TPowerDlg()
        {
        }

        public TPowerDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWProdScreen
{
    public partial class TProductionScreen : ILayoutComponent
    {
        public TProductionScreen()
        {
        }

        public TProductionScreen(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TProgressDialog : ILayoutComponent
    {
        public TProgressDialog()
        {
        }

        public TProgressDialog(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWRealm
{
    public partial class TRealmWin : ILayoutComponent
    {
        public TRealmWin()
        {
        }

        public TRealmWin(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWRelations
{
    public partial class TRelWindow : ILayoutComponent
    {
        public TRelWindow()
        {
        }

        public TRelWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TResultsWin : ILayoutComponent
    {
        public TResultsWin()
        {
        }

        public TResultsWin(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TSMTPDlg : ILayoutComponent
    {
        public TSMTPDlg()
        {
        }

        public TSMTPDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWSMTPDlg
{
    public partial class TmsMessage : ILayoutComponent
    {
        public TmsMessage()
        {
        }

        public TmsMessage(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

        public Aow.Rtl.AnsiString ContentType { get; set; }
        public bool ReturnReceipt { get; set; }
    }
}

namespace Aow.Units.AoWSMTPDlg
{
    public partial class TmsSMTP : ILayoutComponent
    {
        public TmsSMTP()
        {
        }

        public TmsSMTP(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

        public int DefaultPort { get; set; }
        public Aow.Layout.SmtpError OnError { get; set; }
        public Aow.Layout.SmtpProgress OnProgress { get; set; }
    }
}

namespace Aow.Units.AoWSWindow
{
    public partial class TSWindow : ILayoutComponent
    {
        public TSWindow()
        {
        }

        public TSWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWScenSetup
{
    public partial class TScenSetup : ILayoutComponent
    {
        public TScenSetup()
        {
        }

        public TScenSetup(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWSelFactionWin
{
    public partial class TSelFactionWin : ILayoutComponent
    {
        public TSelFactionWin()
        {
        }

        public TSelFactionWin(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWSendEmailGameDlg
{
    public partial class TSendEmailGameDlg : ILayoutComponent
    {
        public TSendEmailGameDlg()
        {
        }

        public TSendEmailGameDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWSetup
{
    public partial class TSetupControl : ILayoutComponent
    {
        public TSetupControl()
        {
        }

        public TSetupControl(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWSoundDlg
{
    public partial class TSoundDlg : ILayoutComponent
    {
        public TSoundDlg()
        {
        }

        public TSoundDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TSpellBook : ILayoutComponent
    {
        public TSpellBook()
        {
        }

        public TSpellBook(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TSynchronizeDlg : ILayoutComponent
    {
        public TSynchronizeDlg()
        {
        }

        public TSynchronizeDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWTBWindow
{
    public partial class TTBWindow : ILayoutComponent
    {
        public TTBWindow()
        {
        }

        public TTBWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWTCScanner
{
    public partial class TTCScanner : ILayoutComponent
    {
        public TTCScanner()
        {
        }

        public TTCScanner(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWTCUnit
{
    public partial class TTCUnit : ILayoutComponent
    {
        public TTCUnit()
        {
        }

        public TTCUnit(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWTabPanel
{
    public partial class TAOWTabPanel : ILayoutComponent
    {
        public TAOWTabPanel()
        {
        }

        public TAOWTabPanel(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

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
    public partial class TTargetSelectionDlg : ILayoutComponent
    {
        public TTargetSelectionDlg()
        {
        }

        public TTargetSelectionDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWTitleScene
{
    public partial class TTitleScene : ILayoutComponent
    {
        public TTitleScene()
        {
        }

        public TTitleScene(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TTutorialWindow : ILayoutComponent
    {
        public TTutorialWindow()
        {
        }

        public TTutorialWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWUnitBanner
{
    public partial class TUnitBanner : ILayoutComponent
    {
        public TUnitBanner()
        {
        }

        public TUnitBanner(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWUnitSelDlg
{
    public partial class TUnitSelectionDlg : ILayoutComponent
    {
        public TUnitSelectionDlg()
        {
        }

        public TUnitSelectionDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWUnitWindow
{
    public partial class TUnitWindow : ILayoutComponent
    {
        public TUnitWindow()
        {
        }

        public TUnitWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
    }
}

namespace Aow.Units.AoWVScroll
{
    public partial class TAOWVScrollBar : ILayoutComponent
    {
        public TAOWVScrollBar()
        {
        }

        public TAOWVScrollBar(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWVideoDlg
{
    public partial class TVideoDialog : ILayoutComponent
    {
        public TVideoDialog()
        {
        }

        public TVideoDialog(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TGeneral : ILayoutComponent
    {
        public TGeneral()
        {
        }

        public TGeneral(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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

namespace Aow.Units.AoWWindow
{
    public partial class TAOWWindow : ILayoutComponent
    {
        public TAOWWindow()
        {
        }

        public TAOWWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWWizardsTowerDlg
{
    public partial class TWizardsTowerDlg : ILayoutComponent
    {
        public TWizardsTowerDlg()
        {
        }

        public TWizardsTowerDlg(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.AoWWorldMap
{
    public partial class TWorldMap : ILayoutComponent
    {
        public TWorldMap()
        {
        }

        public TWorldMap(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
    }
}

namespace Aow.Units.AowUserWindow
{
    public partial class TAoWUserWindow : ILayoutComponent
    {
        public TAoWUserWindow()
        {
        }

        public TAoWUserWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.CMapCtrl
{
    public partial class TAoWCombatMapControl : ILayoutComponent
    {
        public TAoWCombatMapControl()
        {
        }

        public TAoWCombatMapControl(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.FastCombatWin
{
    public partial class TFastCombatWindow : ILayoutComponent
    {
        public TFastCombatWindow()
        {
        }

        public TFastCombatWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class TFontModule : ILayoutComponent
    {
        public TFontModule()
        {
        }

        public TFontModule(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.HSEngine
{
    public partial class THSMapViewer : ILayoutComponent
    {
        public THSMapViewer()
        {
        }

        public THSMapViewer(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.ImageLib
{
    public partial class TImageLibrary : ILayoutComponent
    {
        public TImageLibrary()
        {
        }

        public TImageLibrary(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.ImageLib
{
    public partial class TImageLibraryFont : ILayoutComponent
    {
        public TImageLibraryFont()
        {
        }

        public TImageLibraryFont(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Layout
{
    public class LayoutTIvTranslator : Aow.Rtl.TIvTranslator, ILayoutComponent
    {
        public LayoutTIvTranslator(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.MapCtrl
{
    public partial class TAoWMapControl : ILayoutComponent
    {
        public TAoWMapControl()
        {
        }

        public TAoWMapControl(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.Scanner
{
    public partial class TScanner : ILayoutComponent
    {
        public TScanner()
        {
        }

        public TScanner(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.StartMenu
{
    public partial class TStartMenu : ILayoutComponent
    {
        public TStartMenu()
        {
        }

        public TStartMenu(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.TCFCDlg
{
    public partial class TTCFCDialog : ILayoutComponent
    {
        public TTCFCDialog()
        {
        }

        public TTCFCDialog(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.TurnButton
{
    public partial class TAOWTurnButton : ILayoutComponent
    {
        public TAOWTurnButton()
        {
        }

        public TAOWTurnButton(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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

namespace Aow.Layout
{
    public class LayoutTVideoPlayer : Aow.Rtl.TVideoPlayer, ILayoutComponent
    {
        public LayoutTVideoPlayer(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

        public Aow.Layout.TVideoPlayerLoopMode Loopmode { get; set; }
        public Aow.Layout.TStreamSoundEvent OnStreamSound { get; set; }
        public Aow.Rtl.TSet<Aow.Layout.TVideoPlayerOptions_Element> Options { get; set; }
    }
}

namespace Aow.Layout
{
    public class LayoutTGDPlay : Aow.Rtl.TGDPlay, ILayoutComponent
    {
        public LayoutTGDPlay(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
    public partial class THelpWindow : ILayoutComponent
    {
        public THelpWindow()
        {
        }

        public THelpWindow(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.aowIntGfxMod
{
    public partial class TIntGfxMod : ILayoutComponent
    {
        public TIntGfxMod()
        {
        }

        public TIntGfxMod(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.aowMapEvents
{
    public partial class TMapEvents : ILayoutComponent
    {
        public TMapEvents()
        {
        }

        public TMapEvents(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.aowSetupEvents
{
    public partial class TSetupEvents : ILayoutComponent
    {
        public TSetupEvents()
        {
        }

        public TSetupEvents(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }

        public Aow.Rtl.TNotifyEvent LayoutOnCreate { get; set; }
    }
}

namespace Aow.Units.aowTCMapEvents
{
    public partial class TTCMapEvents : ILayoutComponent
    {
        public TTCMapEvents()
        {
        }

        public TTCMapEvents(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
        }
    }
}

namespace Aow.Units.aowUserDrawBox
{
    public partial class TaowUserDrawBox : ILayoutComponent
    {
        public TaowUserDrawBox()
        {
        }

        public TaowUserDrawBox(TComponent owner)
        {
            Create__init(owner);
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
        }

        void ILayoutComponent.EndRead()
        {
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
