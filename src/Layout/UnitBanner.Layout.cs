// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWUnitBanner
{
    public partial class TUnitBanner
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "UnitBanner";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            UnitNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UnitNameLbl",
                Priority = 0,
                WinLeft = 42,
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
                TextColor = 1321830
            };
            LayoutSession.EndRead(UnitNameLbl);
            AttackIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AttackIcon",
                Priority = 0,
                WinLeft = 30,
                WinTop = 48,
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
                    TopOffset = 0,
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
                WinTop = 65,
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
                    TopOffset = 0,
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
            MovesIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "MovesIcon",
                Priority = 0,
                WinLeft = 23,
                WinTop = 82,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 16,
                AnimEnd = 19,
                WinWidth = 32,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(MovesIcon);
            DamageIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "DamageIcon",
                Priority = 0,
                WinLeft = 83,
                WinTop = 48,
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
                    TopOffset = 0,
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
                WinTop = 65,
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
                    TopOffset = 0,
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
            HitsIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "HitsIcon",
                Priority = 0,
                WinLeft = 83,
                WinTop = 82,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 4,
                AnimEnd = 6,
                WinWidth = 15,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(HitsIcon);
            Attack = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Attack",
                Priority = 0,
                WinLeft = 50,
                WinTop = 53,
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
                TextColor = 1321830
            };
            LayoutSession.EndRead(Attack);
            Defense = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Defense",
                Priority = 0,
                WinLeft = 50,
                WinTop = 70,
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
                TextColor = 1321830
            };
            LayoutSession.EndRead(Defense);
            Moves = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Moves",
                Priority = 0,
                WinLeft = 50,
                WinTop = 87,
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
                TextColor = 1321830
            };
            LayoutSession.EndRead(Moves);
            Damage = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Damage",
                Priority = 0,
                WinLeft = 103,
                WinTop = 53,
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
                TextColor = 1321830
            };
            LayoutSession.EndRead(Damage);
            Resistance = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Resistance",
                Priority = 0,
                WinLeft = 103,
                WinTop = 70,
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
                TextColor = 1321830
            };
            LayoutSession.EndRead(Resistance);
            Hits = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Hits",
                Priority = 0,
                WinLeft = 103,
                WinTop = 87,
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
                TextColor = 1321830
            };
            LayoutSession.EndRead(Hits);
            BannerPopup = new Aow.Units.AOWMenu.TAOWMenu(this)
            {
                Name = "BannerPopup",
                OnMouseDown = Layout_BannerPopupMouseDown_0,
                Visible = false,
                Priority = 350,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 160,
                Masked = true,
                OnShow = BannerPopupShow,
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
                TextColor = 8734252,
                TextSelColor = 16777215,
                ItemHeight = 13,
                OwnerItem = 0,
                SubMenuShowDelay = 0,
                WinHeight = 184
            };
            LayoutSession.EndRead(BannerPopup);
            AbLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "AbLB",
                OnMouseDown = AbLBMouseDown,
                OnMouseMove = AbLBMouseMove,
                OnUpdate = Layout_AbLBUpdate_1,
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
                ItemHeight = 10,
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
            UnitNameLbl.AOWWindow = BannerPopup;
            AttackIcon.AOWWindow = BannerPopup;
            DefenseIcon.AOWWindow = BannerPopup;
            MovesIcon.AOWWindow = BannerPopup;
            DamageIcon.AOWWindow = BannerPopup;
            ResistanceIcon.AOWWindow = BannerPopup;
            HitsIcon.AOWWindow = BannerPopup;
            Attack.AOWWindow = BannerPopup;
            Defense.AOWWindow = BannerPopup;
            Moves.AOWWindow = BannerPopup;
            Damage.AOWWindow = BannerPopup;
            Resistance.AOWWindow = BannerPopup;
            Hits.AOWWindow = BannerPopup;
            AbLB.AOWWindow = BannerPopup;
            AbLB.VScrollBar = AbVSB;
            AbVSB.AOWWindow = BannerPopup;
            Loaded();
            UnitNameLbl.Loaded();
            AttackIcon.Loaded();
            DefenseIcon.Loaded();
            MovesIcon.Loaded();
            DamageIcon.Loaded();
            ResistanceIcon.Loaded();
            HitsIcon.Loaded();
            Attack.Loaded();
            Defense.Loaded();
            Moves.Loaded();
            Damage.Loaded();
            Resistance.Loaded();
            Hits.Loaded();
            BannerPopup.Loaded();
            AbLB.Loaded();
            AbVSB.Loaded();
            session.Resolve(this);
            UnitNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AttackIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AttackIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            DefenseIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            DefenseIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            MovesIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MovesIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            DamageIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            DamageIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            ResistanceIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ResistanceIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            HitsIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            HitsIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            Attack.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Defense.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Moves.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Damage.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Resistance.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Hits.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            BannerPopup.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            BannerPopup.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            BannerPopup.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            BannerPopup.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            BannerPopup.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AbLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AbVSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbVSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbVSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbVSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
        }

        private void Layout_BannerPopupMouseDown_0(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
        {
            BannerPopupMouseDown(default(int), (byte)p1, default(int), default(int), default(int));
        }

        private void Layout_AbLBUpdate_1(TObject p0)
        {
            AbLBUpdate();
        }
    }
}
