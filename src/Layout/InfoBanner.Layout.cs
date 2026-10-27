// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWInfoBanner
{
    public partial class TInfoBanner
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "InfoBanner";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            HeaderLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HeaderLbl",
                Priority = 0,
                WinLeft = 71,
                WinTop = 30,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 30,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "######",
                TextColor = 1321830
            };
            LayoutSession.EndRead(HeaderLbl);
            InfoMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "InfoMemo",
                Priority = 0,
                WinLeft = 30,
                WinTop = 45,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 30,
                    RightOffset = 30,
                    TopOffset = 45,
                    BottomOffset = 20
                },
                Masked = true,
                WinWidth = 100,
                WinHeight = 119,
                LayoutItems = new LayoutStrings()
                {
                    "######"
                },
                BorderWidth = 0,
                BorderHeight = 0,
                TextColor = 0,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 10,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(InfoMemo);
            BannerPopup = new Aow.Units.AOWMenu.TAOWMenu(this)
            {
                Name = "BannerPopup",
                OnMouseDown = BannerPopupMouseDown,
                Visible = false,
                Priority = 350,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 238,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 0,
                BorderHeight = 0,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 8388736,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 19,
                ILIndexTNormal = 27,
                ILIndexTSelected = 27,
                TextColor = 8734252,
                TextSelColor = 16777215,
                ItemHeight = 13,
                OwnerItem = 0,
                SubMenuShowDelay = 0,
                WinHeight = 155
            };
            LayoutSession.EndRead(BannerPopup);
            InfoMemoSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "InfoMemoSB",
                Priority = 0,
                WinLeft = 117,
                WinTop = 45,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Min = 0,
                Max = 14,
                SmallChange = 1,
                LargeChange = 10,
                Position = 0,
                WinHeight = 119,
                WinWidth = 13,
                NumberVisible = 10,
                VanishWhenFull = true,
                ILIndexIncUp = 42,
                ILIndexIncDown = 43,
                ILIndexDecUp = 40,
                ILIndexDecDown = 41,
                ILIndexBack = 47,
                ILIndexTab = 44
            };
            LayoutSession.EndRead(InfoMemoSB);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            HeaderLbl.AOWWindow = BannerPopup;
            InfoMemo.AOWWindow = BannerPopup;
            InfoMemo.VScrollBar = InfoMemoSB;
            InfoMemoSB.AOWWindow = BannerPopup;
            Loaded();
            HeaderLbl.Loaded();
            InfoMemo.Loaded();
            BannerPopup.Loaded();
            InfoMemoSB.Loaded();
            session.Resolve(this);
            HeaderLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            InfoMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            BannerPopup.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            BannerPopup.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            BannerPopup.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            BannerPopup.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            BannerPopup.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            InfoMemoSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            InfoMemoSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            InfoMemoSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            InfoMemoSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
        }
    }
}
