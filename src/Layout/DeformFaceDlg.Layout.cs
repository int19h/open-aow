// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWDeformFaceDlg
{
    public partial class TDeformFaceDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "DeformFaceDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 5000,
                WinLeft = 120,
                WinTop = 91,
                WinWidth = 400,
                Masked = true,
                OnHide = Layout_DlgHide_0,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
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
                MaskColor = 5808200,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 32,
                ILIndexTexture = 2,
                DragHeight = 0,
                Draggable = false,
                Resizable = false,
                MinWidth = 0,
                MinHeight = 0,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = true,
                WinHeight = 250,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            FaceFrame = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "FaceFrame",
                OnDraw = DrawFace,
                Priority = 500,
                WinLeft = 152,
                WinTop = 75,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 38,
                    RightOffPercent = 38,
                    TopOffPercent = 10,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 24,
                    BottomOffset = 80
                },
                Masked = false,
                WinWidth = 96,
                WinHeight = 121,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 30,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(FaceFrame);
            CancelBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelBtn",
                Priority = 500,
                WinLeft = 230,
                WinTop = 216,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 70,
                    TopOffset = 0,
                    BottomOffset = 12
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Cancel",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 100,
                OnClick = Layout_CancelBtnClick_1
            };
            LayoutSession.EndRead(CancelBtn);
            TitleIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TitleIcon",
                Priority = 0,
                WinLeft = 20,
                WinTop = 5,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 20,
                    RightOffset = 0,
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TitleIcon);
            Title = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Title",
                Priority = 0,
                WinLeft = 42,
                WinTop = 7,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 42,
                    RightOffset = 0,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Cosmagic Surgery",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            CastBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CastBtn",
                Priority = 450,
                WinLeft = 70,
                WinTop = 216,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 66,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 70,
                    RightOffset = 4,
                    TopOffset = 0,
                    BottomOffset = 12
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Cast",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 100,
                OnClick = Layout_CastBtnClick_2
            };
            LayoutSession.EndRead(CastBtn);
            SelectNewFaceLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SelectNewFaceLbl",
                Priority = 1000,
                WinLeft = 139,
                WinTop = 30,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 175,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Select New Face for:",
                TextColor = 7921903
            };
            LayoutSession.EndRead(SelectNewFaceLbl);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 363,
                WinTop = 6,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Closes the window."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 22,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 1,
                ILIndexLit = 2,
                ILIndexUp = 0,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                OnClick = Layout_CastBtnClick_2
            };
            LayoutSession.EndRead(Close);
            LeftFace = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "LeftFace",
                Tag = -1,
                OnDraw = DrawFace,
                Priority = 500,
                WinLeft = 60,
                WinTop = 75,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 15,
                    RightOffPercent = 64,
                    TopOffPercent = 10,
                    BottomOffPercent = 15,
                    LeftOffset = 0,
                    RightOffset = 22,
                    TopOffset = 24,
                    BottomOffset = 80
                },
                Masked = false,
                WinWidth = 74,
                WinHeight = 84,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 30,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(LeftFace);
            RightFace = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "RightFace",
                Tag = 1,
                OnDraw = DrawFace,
                Priority = 500,
                WinLeft = 266,
                WinTop = 75,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 64,
                    RightOffPercent = 15,
                    TopOffPercent = 10,
                    BottomOffPercent = 15,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 24,
                    BottomOffset = 80
                },
                Masked = false,
                WinWidth = 74,
                WinHeight = 84,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 30,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(RightFace);
            AoWFrame1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "AoWFrame1",
                Priority = 0,
                WinLeft = 21,
                WinTop = 213,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 21,
                    RightOffset = 21,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                WinWidth = 358,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(AoWFrame1);
            CeltM = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltM",
                Priority = 100,
                WinLeft = 187,
                WinTop = 213,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 27,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CeltM);
            CeltL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltL",
                Priority = 100,
                WinLeft = 27,
                WinTop = 213,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 27,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 27,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CeltL);
            CeltR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltR",
                Priority = 100,
                WinLeft = 346,
                WinTop = 213,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 27,
                    RightOffset = 27,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 27,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CeltR);
            PrevFaceBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PrevFaceBtn",
                Priority = 450,
                WinLeft = 86,
                WinTop = 168,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 66,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 32,
                    RightOffset = 4,
                    TopOffset = 0,
                    BottomOffset = 18
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "<<",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 50,
                OnClick = Layout_PrevFaceBtnClick_3
            };
            LayoutSession.EndRead(PrevFaceBtn);
            NextFaceBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "NextFaceBtn",
                Priority = 450,
                WinLeft = 265,
                WinTop = 168,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 66,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 32,
                    RightOffset = 4,
                    TopOffset = 0,
                    BottomOffset = 18
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = ">>",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 50,
                OnClick = Layout_NextFaceBtnClick_4
            };
            LayoutSession.EndRead(NextFaceBtn);
            UnitNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UnitNameLbl",
                Priority = 1000,
                WinLeft = 160,
                WinTop = 48,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 175,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "########",
                TextColor = 7921903
            };
            LayoutSession.EndRead(UnitNameLbl);
            Translator = new Aow.Layout.LayoutTIvTranslator(this)
            {
                Name = "Translator",
                DictionaryName = "AoW",
                TargetsData =
                {
                    new TranslationTarget("", "Items", 0),
                    new TranslationTarget("", "Text", 0),
                    new TranslationTarget("", "ToolTipText", 0)
                }
            };
            LayoutSession.EndRead(Translator);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            FaceFrame.AOWWindow = Dlg;
            CancelBtn.AOWWindow = Dlg;
            TitleIcon.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            CastBtn.AOWWindow = Dlg;
            SelectNewFaceLbl.AOWWindow = Dlg;
            Close.AOWWindow = Dlg;
            LeftFace.AOWWindow = Dlg;
            RightFace.AOWWindow = Dlg;
            AoWFrame1.AOWWindow = Dlg;
            CeltM.AOWWindow = Dlg;
            CeltL.AOWWindow = Dlg;
            CeltR.AOWWindow = Dlg;
            PrevFaceBtn.AOWWindow = Dlg;
            NextFaceBtn.AOWWindow = Dlg;
            UnitNameLbl.AOWWindow = Dlg;
            Loaded();
            Dlg.Loaded();
            FaceFrame.Loaded();
            CancelBtn.Loaded();
            TitleIcon.Loaded();
            Title.Loaded();
            CastBtn.Loaded();
            SelectNewFaceLbl.Loaded();
            Close.Loaded();
            LeftFace.Loaded();
            RightFace.Loaded();
            AoWFrame1.Loaded();
            CeltM.Loaded();
            CeltL.Loaded();
            CeltR.Loaded();
            PrevFaceBtn.Loaded();
            NextFaceBtn.Loaded();
            UnitNameLbl.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            FaceFrame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CancelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CastBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CastBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CastBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            SelectNewFaceLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            LeftFace.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            RightFace.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AoWFrame1.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CeltM.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CeltL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CeltR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            PrevFaceBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PrevFaceBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PrevFaceBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            NextFaceBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            NextFaceBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            NextFaceBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            UnitNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
        }

        private void Layout_DlgHide_0(TObject p0)
        {
            DlgHide();
        }

        private void Layout_CancelBtnClick_1(TObject p0)
        {
            CancelBtnClick();
        }

        private void Layout_CastBtnClick_2(TObject p0)
        {
            CastBtnClick();
        }

        private void Layout_PrevFaceBtnClick_3(TObject p0)
        {
            PrevFaceBtnClick();
        }

        private void Layout_NextFaceBtnClick_4(TObject p0)
        {
            NextFaceBtnClick();
        }
    }
}
