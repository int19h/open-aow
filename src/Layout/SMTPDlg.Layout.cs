// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWSMTPDlg
{
    public partial class TSMTPDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "SMTPDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnUpdate = Layout_DlgUpdate_0,
                Visible = false,
                Priority = 300,
                WinLeft = 127,
                WinTop = 111,
                WinWidth = 386,
                Masked = true,
                OnHide = Layout_DlgHide_1,
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
                WinHeight = 210,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 173,
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
                WinWidth = 344,
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
                WinWidth = 344,
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
                WinWidth = 330,
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
                WinTop = 173,
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
                WinLeft = 331,
                WinTop = 173,
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
            ExitBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ExitBtn",
                Visible = false,
                Priority = 1000,
                WinLeft = 128,
                WinTop = 173,
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
                    LeftOffset = 0,
                    RightOffset = 57,
                    TopOffset = 0,
                    BottomOffset = 0
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
                WinWidth = 130,
                OnClick = Layout_ExitBtnClick_2
            };
            LayoutSession.EndRead(ExitBtn);
            SendingGameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SendingGameLbl",
                Priority = 0,
                WinLeft = 71,
                WinTop = 0,
                CaptureInput = false,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Sending email game...",
                TextColor = 0
            };
            LayoutSession.EndRead(SendingGameLbl);
            ProgressBar = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "ProgressBar",
                OnUpdate = Layout_ProgressBarUpdate_3,
                Priority = 0,
                WinLeft = 0,
                WinTop = 65,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 65,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 306,
                WinHeight = 30,
                Min = 0,
                Max = 100,
                Position = 50,
                ILIndexFrame = 30,
                ILIndexTNormal = 14,
                ILIndexTSelected = 6
            };
            LayoutSession.EndRead(ProgressBar);
            RecipientLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "RecipientLbl",
                Priority = 0,
                WinLeft = 122,
                WinTop = 35,
                CaptureInput = false,
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
                    TopOffset = 35,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Recipient:",
                TextColor = 0
            };
            LayoutSession.EndRead(RecipientLbl);
            SendPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SendPnl",
                Visible = false,
                Priority = 100,
                WinLeft = 40,
                WinTop = 55,
                WinWidth = 306,
                Masked = true,
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
                    LeftOffset = 40,
                    RightOffset = 40,
                    TopOffset = 55,
                    BottomOffset = 10
                },
                WinHeight = 145,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SendPnl);
            FailedPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "FailedPnl",
                Priority = 100,
                WinLeft = 30,
                WinTop = 60,
                WinWidth = 326,
                Masked = true,
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
                    TopOffset = 60,
                    BottomOffset = 10
                },
                WinHeight = 140,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(FailedPnl);
            FailedMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "FailedMemo",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                CaptureInput = false,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 326,
                WinHeight = 80,
                LayoutItems = new LayoutStrings()
                {
                    "Warning!",
                    "Cannot send email game, please check your email settings and make sure you are online."
                },
                BorderWidth = 0,
                BorderHeight = 0,
                TextColor = 0,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 18,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(FailedMemo);
            SettingsBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SettingsBtn",
                Priority = 1000,
                WinLeft = 108,
                WinTop = 83,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 35
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Settings",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 110,
                OnClick = Layout_SettingsBtnClick_4
            };
            LayoutSession.EndRead(SettingsBtn);
            RetryBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RetryBtn",
                Priority = 1000,
                WinLeft = 30,
                WinTop = 113,
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Retry",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 110,
                OnClick = Layout_RetryBtnClick_5
            };
            LayoutSession.EndRead(RetryBtn);
            ExitBtn2 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ExitBtn2",
                Priority = 1000,
                WinLeft = 186,
                WinTop = 113,
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
                    RightOffset = 30,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Abort",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 110,
                OnClick = Layout_ExitBtnClick_2
            };
            LayoutSession.EndRead(ExitBtn2);
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
                Text = "Sending Email",
                TextColor = 0
            };
            LayoutSession.EndRead(TitleLbl);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 351,
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
                WinWidth = 15
            };
            LayoutSession.EndRead(Close);
            TitleIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TitleIcon",
                Priority = 0,
                WinLeft = 20,
                WinTop = 5,
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
                CurrentImage = 18,
                AnimEnd = 0,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TitleIcon);
            IMSMessage = new Aow.Units.AoWSMTPDlg.TmsMessage(this)
            {
                Name = "IMSMessage",
                ContentType = "text/plain",
                ReturnReceipt = false
            };
            LayoutSession.EndRead(IMSMessage);
            IMSSMTP = new Aow.Units.AoWSMTPDlg.TmsSMTP(this)
            {
                Name = "IMSSMTP",
                DefaultPort = 25,
                OnError = IMSSMTPError,
                OnProgress = IMSSMTPProgress
            };
            LayoutSession.EndRead(IMSSMTP);
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
            BCeltL.AOWWindow = Dlg;
            TCeltL.AOWWindow = Dlg;
            TCeltLineL.AOWWindow = Dlg;
            AOWImage1.AOWWindow = Dlg;
            AOWImage2.AOWWindow = Dlg;
            ExitBtn.AOWWindow = SendPnl;
            SendingGameLbl.AOWWindow = SendPnl;
            ProgressBar.AOWWindow = SendPnl;
            RecipientLbl.AOWWindow = SendPnl;
            SendPnl.AOWWindow = Dlg;
            FailedPnl.AOWWindow = Dlg;
            FailedMemo.AOWWindow = FailedPnl;
            SettingsBtn.AOWWindow = FailedPnl;
            RetryBtn.AOWWindow = FailedPnl;
            ExitBtn2.AOWWindow = FailedPnl;
            TitleLbl.AOWWindow = Dlg;
            Close.AOWWindow = Dlg;
            TitleIcon.AOWWindow = Dlg;
            Loaded();
            Dlg.Loaded();
            BCeltL.Loaded();
            TCeltL.Loaded();
            TCeltLineL.Loaded();
            AOWImage1.Loaded();
            AOWImage2.Loaded();
            ExitBtn.Loaded();
            SendingGameLbl.Loaded();
            ProgressBar.Loaded();
            RecipientLbl.Loaded();
            SendPnl.Loaded();
            FailedPnl.Loaded();
            FailedMemo.Loaded();
            SettingsBtn.Loaded();
            RetryBtn.Loaded();
            ExitBtn2.Loaded();
            TitleLbl.Loaded();
            Close.Loaded();
            TitleIcon.Loaded();
            IMSMessage.Loaded();
            IMSSMTP.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCeltLineL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AOWImage1.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AOWImage2.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            ExitBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ExitBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            SendingGameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            ProgressBar.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            ProgressBar.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ProgressBar.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            RecipientLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            FailedMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            SettingsBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SettingsBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            RetryBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RetryBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            ExitBtn2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ExitBtn2.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            TitleLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
        }

        private void Layout_DlgUpdate_0(TObject p0)
        {
            DlgUpdate();
        }

        private void Layout_DlgHide_1(TObject p0)
        {
            DlgHide();
        }

        private void Layout_ExitBtnClick_2(TObject p0)
        {
            ExitBtnClick();
        }

        private void Layout_ProgressBarUpdate_3(TObject p0)
        {
            ProgressBarUpdate();
        }

        private void Layout_SettingsBtnClick_4(TObject p0)
        {
            SettingsBtnClick();
        }

        private void Layout_RetryBtnClick_5(TObject p0)
        {
            RetryBtnClick();
        }
    }
}
