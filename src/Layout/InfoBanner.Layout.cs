// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWInfoBanner
{
    public partial class TInfoBanner : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "InfoBanner";
            HeaderLbl = new Aow.Units.AoWLabel.TAOWLabel(this);
            session.BeginRead(HeaderLbl);
            HeaderLbl.Name = "HeaderLbl";
            HeaderLbl.Priority = 0;
            HeaderLbl.WinLeft = 71;
            HeaderLbl.WinTop = 30;
            HeaderLbl.CaptureInput = true;
            HeaderLbl.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter;
            HeaderLbl.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
            HeaderLbl.Alignment.HeightPercent = (byte)(0);
            HeaderLbl.Alignment.WidthPercent = (byte)(0);
            HeaderLbl.Alignment.LeftOffPercent = 0;
            HeaderLbl.Alignment.RightOffPercent = 0;
            HeaderLbl.Alignment.TopOffPercent = 0;
            HeaderLbl.Alignment.BottomOffPercent = 0;
            HeaderLbl.Alignment.LeftOffset = 0;
            HeaderLbl.Alignment.RightOffset = 0;
            HeaderLbl.Alignment.TopOffset = 30;
            HeaderLbl.Alignment.BottomOffset = 0;
            HeaderLbl.Masked = true;
            HeaderLbl.Text = "######";
            HeaderLbl.TextColor = 1321830;
            session.EndRead(HeaderLbl);
            InfoMemo = Aow.Units.AoWMemo.TAOWMemo.Create(this);
            session.BeginRead(InfoMemo);
            InfoMemo.Name = "InfoMemo";
            InfoMemo.Priority = 0;
            InfoMemo.WinLeft = 30;
            InfoMemo.WinTop = 45;
            InfoMemo.CaptureInput = true;
            InfoMemo.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            InfoMemo.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
            InfoMemo.Alignment.HeightPercent = (byte)(0);
            InfoMemo.Alignment.WidthPercent = (byte)(0);
            InfoMemo.Alignment.LeftOffPercent = 0;
            InfoMemo.Alignment.RightOffPercent = 0;
            InfoMemo.Alignment.TopOffPercent = 0;
            InfoMemo.Alignment.BottomOffPercent = 0;
            InfoMemo.Alignment.LeftOffset = 30;
            InfoMemo.Alignment.RightOffset = 30;
            InfoMemo.Alignment.TopOffset = 45;
            InfoMemo.Alignment.BottomOffset = 20;
            InfoMemo.Masked = true;
            InfoMemo.WinWidth = 100;
            InfoMemo.WinHeight = 119;
            InfoMemo.LayoutItems = new LayoutStrings()
            {
                "######"
            };
            InfoMemo.BorderWidth = 0;
            InfoMemo.BorderHeight = 0;
            InfoMemo.TextColor = 0;
            InfoMemo.CursorChar = (byte)124;
            InfoMemo.BlinkDelay = (byte)(0);
            InfoMemo.Editable = false;
            InfoMemo.ILIndexFrame = 0;
            InfoMemo.ILIndexTexture = 0;
            InfoMemo.ItemHeight = 10;
            InfoMemo.ColorChangeChar = (byte)0;
            InfoMemo.TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter;
            session.EndRead(InfoMemo);
            BannerPopup = Aow.Units.AOWMenu.TAOWMenu.Create(this);
            session.BeginRead(BannerPopup);
            BannerPopup.Name = "BannerPopup";
            BannerPopup.OnMouseDown = BannerPopupMouseDown;
            BannerPopup.Visible = false;
            BannerPopup.Priority = 350;
            BannerPopup.WinLeft = 0;
            BannerPopup.WinTop = 0;
            BannerPopup.WinWidth = 238;
            BannerPopup.Masked = true;
            BannerPopup.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone;
            BannerPopup.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone;
            BannerPopup.Alignment.HeightPercent = (byte)(0);
            BannerPopup.Alignment.WidthPercent = (byte)(0);
            BannerPopup.Alignment.LeftOffPercent = 0;
            BannerPopup.Alignment.RightOffPercent = 0;
            BannerPopup.Alignment.TopOffPercent = 0;
            BannerPopup.Alignment.BottomOffPercent = 0;
            BannerPopup.Alignment.LeftOffset = 0;
            BannerPopup.Alignment.RightOffset = 0;
            BannerPopup.Alignment.TopOffset = 0;
            BannerPopup.Alignment.BottomOffset = 0;
            BannerPopup.UseVMem = false;
            BannerPopup.BorderWidth = 0;
            BannerPopup.BorderHeight = 0;
            BannerPopup.OverlapTopCorners = false;
            BannerPopup.Cursor = 0;
            BannerPopup.DoubleBuffered = true;
            BannerPopup.MaskColor = 8388736;
            BannerPopup.FreeSurfaceOnHide = true;
            BannerPopup.ILIndexFrame = 19;
            BannerPopup.ILIndexTNormal = 27;
            BannerPopup.ILIndexTSelected = 27;
            BannerPopup.TextColor = 8734252;
            BannerPopup.TextSelColor = 16777215;
            BannerPopup.ItemHeight = 13;
            BannerPopup.OwnerItem = 0;
            BannerPopup.SubMenuShowDelay = 0;
            BannerPopup.WinHeight = 155;
            session.EndRead(BannerPopup);
            InfoMemoSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this);
            session.BeginRead(InfoMemoSB);
            InfoMemoSB.Name = "InfoMemoSB";
            InfoMemoSB.Priority = 0;
            InfoMemoSB.WinLeft = 117;
            InfoMemoSB.WinTop = 45;
            InfoMemoSB.CaptureInput = true;
            InfoMemoSB.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone;
            InfoMemoSB.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone;
            InfoMemoSB.Alignment.HeightPercent = (byte)(0);
            InfoMemoSB.Alignment.WidthPercent = (byte)(0);
            InfoMemoSB.Alignment.LeftOffPercent = 0;
            InfoMemoSB.Alignment.RightOffPercent = 0;
            InfoMemoSB.Alignment.TopOffPercent = 0;
            InfoMemoSB.Alignment.BottomOffPercent = 0;
            InfoMemoSB.Alignment.LeftOffset = 0;
            InfoMemoSB.Alignment.RightOffset = 0;
            InfoMemoSB.Alignment.TopOffset = 0;
            InfoMemoSB.Alignment.BottomOffset = 0;
            InfoMemoSB.Masked = true;
            InfoMemoSB.Min = 0;
            InfoMemoSB.Max = 14;
            InfoMemoSB.SmallChange = 1;
            InfoMemoSB.LargeChange = 10;
            InfoMemoSB.Position = 0;
            InfoMemoSB.WinHeight = 119;
            InfoMemoSB.WinWidth = 13;
            InfoMemoSB.NumberVisible = 10;
            InfoMemoSB.VanishWhenFull = true;
            InfoMemoSB.ILIndexIncUp = 42;
            InfoMemoSB.ILIndexIncDown = 43;
            InfoMemoSB.ILIndexDecUp = 40;
            InfoMemoSB.ILIndexDecDown = 41;
            InfoMemoSB.ILIndexBack = 47;
            InfoMemoSB.ILIndexTab = 44;
            session.EndRead(InfoMemoSB);
            session.EndRead(this);
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
