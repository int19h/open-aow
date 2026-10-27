// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWSendEmailGameDlg
{
    public partial class TSendEmailGameDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "SendEmailGameDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 300,
                WinLeft = 130,
                WinTop = 81,
                WinWidth = 380,
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
                FreeSurfaceOnHide = false,
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
                WinHeight = 270,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            TitleLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TitleLbl",
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
                Text = "Send Email Game",
                TextColor = 0
            };
            LayoutSession.EndRead(TitleLbl);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 233,
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
                WinWidth = 338,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(BCeltL);
            TCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "TCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 22,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 21,
                    RightOffset = 21,
                    TopOffset = 22,
                    BottomOffset = 10
                },
                Masked = false,
                WinWidth = 338,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(TCeltL);
            TCeltLineL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltLineL",
                Priority = 100,
                WinLeft = 28,
                WinTop = 22,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 28,
                    RightOffset = 28,
                    TopOffset = 22,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 324,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCeltLineL);
            AOWImage1 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AOWImage1",
                Priority = 100,
                WinLeft = 28,
                WinTop = 233,
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
                    LeftOffset = 28,
                    RightOffset = 28,
                    TopOffset = 22,
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
            LayoutSession.EndRead(AOWImage1);
            AOWImage2 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AOWImage2",
                Priority = 100,
                WinLeft = 325,
                WinTop = 233,
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
                    LeftOffset = 28,
                    RightOffset = 28,
                    TopOffset = 22,
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
            LayoutSession.EndRead(AOWImage2);
            OKCancelPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "OKCancelPnl",
                Priority = 99999,
                WinLeft = 0,
                WinTop = 230,
                WinWidth = 380,
                Masked = true,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                WinHeight = 40,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(OKCancelPnl);
            OKBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "OKBtn",
                Priority = 99999,
                WinLeft = 57,
                WinTop = 3,
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
                    LeftOffset = 57,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "OK",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 120,
                OnClick = Layout_OKBtnClick_1
            };
            LayoutSession.EndRead(OKBtn);
            ExitBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ExitBtn",
                Priority = 99999,
                WinLeft = 203,
                WinTop = 3,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Exit email game without sending."
                },
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
                    RightOffset = 57,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Exit",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 120,
                OnClick = Layout_ExitBtnClick_2
            };
            LayoutSession.EndRead(ExitBtn);
            EmailAddressEdit = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "EmailAddressEdit",
                Tag = 1,
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Next players' Email address"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 279,
                BorderWidth = 8,
                BorderHeight = 5,
                OnChange = Layout_EmailAddressEditChange_3,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 1,
                TextColor = 11002593,
                Editable = true,
                ILIndex = 15,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(EmailAddressEdit);
            SeletModeInfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SeletModeInfoLbl",
                Priority = 0,
                WinLeft = 50,
                WinTop = 120,
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
                    LeftOffset = 50,
                    RightOffset = 0,
                    TopOffset = 120,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Send mode:",
                TextColor = 16777215
            };
            LayoutSession.EndRead(SeletModeInfoLbl);
            ManualChk = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "ManualChk",
                Priority = 99999,
                WinLeft = 20,
                WinTop = 15,
                LayoutToolTipText = new LayoutStrings()
                {
                    "An Email program will need to be used"
                },
                CaptureInput = true,
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
                    TopOffset = 15,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 46,
                ILIndexLit = 45,
                ILIndexUp = 45,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                RadioIndex = 1
            };
            LayoutSession.EndRead(ManualChk);
            AutomatedChk = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "AutomatedChk",
                Priority = 99999,
                WinLeft = 20,
                WinTop = 41,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Age of Wonders sends the game"
                },
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
                    LeftOffset = 20,
                    RightOffset = 0,
                    TopOffset = 45,
                    BottomOffset = 15
                },
                Masked = false,
                BorderHeight = 20,
                BorderHeightSelected = 0,
                ILIndexDown = 46,
                ILIndexLit = 45,
                ILIndexUp = 45,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                RadioIndex = 1
            };
            LayoutSession.EndRead(AutomatedChk);
            ManualLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ManualLbl",
                Priority = 0,
                WinLeft = 45,
                WinTop = 16,
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
                    LeftOffset = 45,
                    RightOffset = 0,
                    TopOffset = 16,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Manual (use Email program)",
                TextColor = 9415072
            };
            LayoutSession.EndRead(ManualLbl);
            AutomatedLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AutomatedLbl",
                Priority = 0,
                WinLeft = 45,
                WinTop = 42,
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
                    LeftOffset = 45,
                    RightOffset = 0,
                    TopOffset = 42,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Automated",
                TextColor = 9415072
            };
            LayoutSession.EndRead(AutomatedLbl);
            RecipientNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "RecipientNameLbl",
                Priority = 0,
                WinLeft = 50,
                WinTop = 60,
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
                    LeftOffset = 50,
                    RightOffset = 0,
                    TopOffset = 60,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##########",
                TextColor = 16777215
            };
            LayoutSession.EndRead(RecipientNameLbl);
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
                CurrentImage = 17,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TitleIcon);
            ModePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ModePnl",
                Priority = 3000,
                WinLeft = 50,
                WinTop = 140,
                WinWidth = 280,
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
                    LeftOffset = 50,
                    RightOffset = 50,
                    TopOffset = 200,
                    BottomOffset = 60
                },
                WinHeight = 70,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ModePnl);
            ModePnlin = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ModePnlin",
                Priority = 3000,
                WinLeft = 1,
                WinTop = 1,
                WinWidth = 279,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                WinHeight = 68,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ModePnlin);
            EditPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "EditPnl",
                Priority = 100,
                WinLeft = 50,
                WinTop = 80,
                WinWidth = 280,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 50,
                    RightOffset = 50,
                    TopOffset = 80,
                    BottomOffset = 1
                },
                WinHeight = 22,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(EditPnl);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 345,
                WinTop = 4,
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
                    RightOffset = 20,
                    TopOffset = 4,
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
                OnClick = Layout_ExitBtnClick_2
            };
            LayoutSession.EndRead(Close);
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
            TitleLbl.AOWWindow = Dlg;
            BCeltL.AOWWindow = Dlg;
            TCeltL.AOWWindow = Dlg;
            TCeltLineL.AOWWindow = Dlg;
            AOWImage1.AOWWindow = Dlg;
            AOWImage2.AOWWindow = Dlg;
            OKCancelPnl.AOWWindow = Dlg;
            OKBtn.AOWWindow = OKCancelPnl;
            ExitBtn.AOWWindow = OKCancelPnl;
            EmailAddressEdit.AOWWindow = EditPnl;
            SeletModeInfoLbl.AOWWindow = Dlg;
            ManualChk.AOWWindow = ModePnlin;
            AutomatedChk.AOWWindow = ModePnlin;
            ManualLbl.AOWWindow = ModePnlin;
            AutomatedLbl.AOWWindow = ModePnlin;
            RecipientNameLbl.AOWWindow = Dlg;
            TitleIcon.AOWWindow = Dlg;
            ModePnl.AOWWindow = Dlg;
            ModePnlin.AOWWindow = ModePnl;
            EditPnl.AOWWindow = Dlg;
            Close.AOWWindow = Dlg;
            Loaded();
            Dlg.Loaded();
            TitleLbl.Loaded();
            BCeltL.Loaded();
            TCeltL.Loaded();
            TCeltLineL.Loaded();
            AOWImage1.Loaded();
            AOWImage2.Loaded();
            OKCancelPnl.Loaded();
            OKBtn.Loaded();
            ExitBtn.Loaded();
            EmailAddressEdit.Loaded();
            SeletModeInfoLbl.Loaded();
            ManualChk.Loaded();
            AutomatedChk.Loaded();
            ManualLbl.Loaded();
            AutomatedLbl.Loaded();
            RecipientNameLbl.Loaded();
            TitleIcon.Loaded();
            ModePnl.Loaded();
            ModePnlin.Loaded();
            EditPnl.Loaded();
            Close.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TitleLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCeltLineL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AOWImage1.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AOWImage2.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            OKBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            OKBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            ExitBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ExitBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            EmailAddressEdit.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            EmailAddressEdit.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            EmailAddressEdit.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            SeletModeInfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10bs;
            ManualChk.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ManualChk.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            AutomatedChk.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AutomatedChk.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            ManualLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            AutomatedLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            RecipientNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10bs;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            ModePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ModePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ModePnlin.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ModePnlin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            EditPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            EditPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
        }

        private void Layout_DlgHide_0(TObject p0)
        {
            DlgHide();
        }

        private void Layout_OKBtnClick_1(TObject p0)
        {
            OKBtnClick();
        }

        private void Layout_ExitBtnClick_2(TObject p0)
        {
            ExitBtnClick();
        }

        private void Layout_EmailAddressEditChange_3(TObject p0)
        {
            EmailAddressEditChange();
        }
    }
}
