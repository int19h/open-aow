// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWItemBanner
{
    public partial class TItemBanner
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "ItemBanner";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            ItemNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ItemNameLbl",
                Priority = 0,
                WinLeft = 65,
                WinTop = 80,
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
                    TopOffset = 80,
                    BottomOffset = 0
                },
                Masked = false,
                TextColor = 1321830
            };
            LayoutSession.EndRead(ItemNameLbl);
            IBannerPopup = new Aow.Units.AOWMenu.TAOWMenu(this)
            {
                Name = "IBannerPopup",
                OnMouseDown = Layout_IBannerPopupMouseDown_0,
                Visible = false,
                Priority = 350,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 160,
                Masked = true,
                OnShow = IBannerPopupShow,
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
                UseVMem = true,
                BorderWidth = 0,
                BorderHeight = 0,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 16711935,
                FreeSurfaceOnHide = false,
                ILIndexFrame = 10,
                ILIndexTNormal = 18,
                ILIndexTSelected = 18,
                TextColor = 1321830,
                TextSelColor = 1321830,
                ItemHeight = 13,
                OwnerItem = 0,
                SubMenuShowDelay = 0,
                WinHeight = 184
            };
            LayoutSession.EndRead(IBannerPopup);
            AttackIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AttackIcon",
                Priority = 0,
                WinLeft = 30,
                WinTop = 59,
                CaptureInput = false,
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
                    TopOffset = 59,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 6,
                WinWidth = 15,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(AttackIcon);
            DefenseIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "DefenseIcon",
                Priority = 0,
                WinLeft = 30,
                WinTop = 77,
                CaptureInput = false,
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
                    TopOffset = 77,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 6,
                WinWidth = 15,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(DefenseIcon);
            DamageIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "DamageIcon",
                Priority = 0,
                WinLeft = 83,
                WinTop = 59,
                CaptureInput = false,
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
                    TopOffset = 59,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 2,
                AnimEnd = 6,
                WinWidth = 15,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(DamageIcon);
            ResistanceIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ResistanceIcon",
                Priority = 0,
                WinLeft = 83,
                WinTop = 77,
                CaptureInput = false,
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
                    TopOffset = 77,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 6,
                WinWidth = 15,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(ResistanceIcon);
            Attack = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Attack",
                Priority = 0,
                WinLeft = 50,
                WinTop = 64,
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
                    TopOffset = 64,
                    BottomOffset = 0
                },
                Masked = false,
                TextColor = 1321830
            };
            LayoutSession.EndRead(Attack);
            Defense = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Defense",
                Priority = 0,
                WinLeft = 50,
                WinTop = 82,
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
                    TopOffset = 82,
                    BottomOffset = 0
                },
                Masked = false,
                TextColor = 1321830
            };
            LayoutSession.EndRead(Defense);
            Damage = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Damage",
                Priority = 0,
                WinLeft = 103,
                WinTop = 64,
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
                    TopOffset = 64,
                    BottomOffset = 0
                },
                Masked = false,
                TextColor = 1321830
            };
            LayoutSession.EndRead(Damage);
            Resistance = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Resistance",
                Priority = 0,
                WinLeft = 103,
                WinTop = 82,
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
                    TopOffset = 82,
                    BottomOffset = 0
                },
                Masked = false,
                TextColor = 1321830
            };
            LayoutSession.EndRead(Resistance);
            DrawBox = new Aow.Units.aowUserDrawBox.TaowUserDrawBox(this)
            {
                Name = "DrawBox",
                OnPreDraw = DrawBoxPreDraw,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                WinWidth = 41,
                WinHeight = 42
            };
            LayoutSession.EndRead(DrawBox);
            AbLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "AbLB",
                Priority = 0,
                WinLeft = 25,
                WinTop = 110,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 110,
                    BottomOffset = 24
                },
                Masked = true,
                WinWidth = 110,
                WinHeight = 49,
                BorderWidth = 0,
                BorderHeight = 0,
                Selected = -1,
                TextColor = 1321830,
                TextSelColor = 1530774,
                ILIndexFrame = 0,
                ILIndexTNormal = 0,
                ILIndexTSelected = 0,
                ItemHeight = 12,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(AbLB);
            AbVSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "AbVSB",
                Priority = 0,
                WinLeft = 122,
                WinTop = 110,
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
                Max = 13,
                SmallChange = 1,
                LargeChange = 3,
                Position = 0,
                WinHeight = 49,
                WinWidth = 13,
                NumberVisible = 3,
                VanishWhenFull = true,
                ILIndexIncUp = 42,
                ILIndexIncDown = 43,
                ILIndexDecUp = 40,
                ILIndexDecDown = 41,
                ILIndexBack = 47,
                ILIndexTab = 44
            };
            LayoutSession.EndRead(AbVSB);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            ItemNameLbl.AOWWindow = IBannerPopup;
            AttackIcon.AOWWindow = IBannerPopup;
            DefenseIcon.AOWWindow = IBannerPopup;
            DamageIcon.AOWWindow = IBannerPopup;
            ResistanceIcon.AOWWindow = IBannerPopup;
            Attack.AOWWindow = IBannerPopup;
            Defense.AOWWindow = IBannerPopup;
            Damage.AOWWindow = IBannerPopup;
            Resistance.AOWWindow = IBannerPopup;
            DrawBox.AOWWindow = IBannerPopup;
            AbLB.AOWWindow = IBannerPopup;
            AbLB.VScrollBar = AbVSB;
            AbVSB.AOWWindow = IBannerPopup;
            Loaded();
            ItemNameLbl.Loaded();
            IBannerPopup.Loaded();
            AttackIcon.Loaded();
            DefenseIcon.Loaded();
            DamageIcon.Loaded();
            ResistanceIcon.Loaded();
            Attack.Loaded();
            Defense.Loaded();
            Damage.Loaded();
            Resistance.Loaded();
            DrawBox.Loaded();
            AbLB.Loaded();
            AbVSB.Loaded();
            session.Resolve(this);
            ItemNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            IBannerPopup.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            IBannerPopup.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            IBannerPopup.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            IBannerPopup.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            IBannerPopup.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AttackIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AttackIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            DefenseIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            DefenseIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            DamageIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            DamageIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            ResistanceIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ResistanceIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            Attack.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Defense.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Damage.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Resistance.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AbLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AbVSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbVSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbVSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbVSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
        }

        private void Layout_IBannerPopupMouseDown_0(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
        {
            IBannerPopupMouseDown(default(int), (byte)p1, default(int), default(int), default(int));
        }
    }
}
