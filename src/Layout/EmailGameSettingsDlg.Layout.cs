// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWEmailGameSettingsDlg
{
    public partial class TEmailGameSettingsDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "EmailGameSettingsDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_EmailGameSettingsDlgCreate_0;
            LayoutOnDestroy = Layout_EmailGameSettingsDlgDestroy_1;
            OKBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "OKBtn",
                Priority = 9999,
                WinLeft = 90,
                WinTop = 297,
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
                    LeftOffset = 90,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 11
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
                WinWidth = 110,
                OnClick = OKBtnClick
            };
            LayoutSession.EndRead(OKBtn);
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
                CurrentImage = 17,
                AnimEnd = 0,
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
                Text = "Email Game Settings",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            CancelBut = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelBut",
                Priority = 9999,
                WinLeft = 256,
                WinTop = 297,
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
                    RightOffset = 84,
                    TopOffset = 0,
                    BottomOffset = 11
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Cancel",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 110,
                OnClick = Layout_CancelButClick_2
            };
            LayoutSession.EndRead(CancelBut);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 520,
                WinLeft = 95,
                WinTop = 49,
                WinWidth = 450,
                Masked = true,
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
                MaskColor = 6268490,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 32,
                ILIndexTexture = 2,
                DragHeight = 20,
                Draggable = true,
                Resizable = false,
                MinWidth = 0,
                MinHeight = 0,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = true,
                WinHeight = 335,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            LocalEmailAdressLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LocalEmailAdressLbl",
                Priority = 100,
                WinLeft = 50,
                WinTop = 30,
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
                    LeftOffset = 50,
                    RightOffset = 0,
                    TopOffset = 30,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Local Email Address",
                TextColor = 16777215
            };
            LayoutSession.EndRead(LocalEmailAdressLbl);
            DecoL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "DecoL",
                Priority = 1000,
                WinLeft = 35,
                WinTop = 297,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
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
                    BottomOffset = 11
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 0,
                WinWidth = 81,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(DecoL);
            DecoR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "DecoR",
                Priority = 1000,
                WinLeft = 341,
                WinTop = 297,
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
                    RightOffset = 28,
                    TopOffset = 0,
                    BottomOffset = 11
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 0,
                WinWidth = 81,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(DecoR);
            AoWFrame1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "AoWFrame1",
                Priority = 0,
                WinLeft = 21,
                WinTop = 297,
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
                    BottomOffset = 11
                },
                Masked = false,
                WinWidth = 408,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(AoWFrame1);
            LocalEmailAddressEdit = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "LocalEmailAddressEdit",
                OnUpdate = Layout_LocalEmailAddressEditUpdate_3,
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
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
                WinWidth = 199,
                BorderWidth = 8,
                BorderHeight = 6,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 1,
                TextColor = 11002593,
                Editable = false,
                ILIndex = 15,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(LocalEmailAddressEdit);
            PlayerEMailAddressesLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PlayerEMailAddressesLbl",
                Priority = 100,
                WinLeft = 50,
                WinTop = 132,
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
                    LeftOffset = 50,
                    RightOffset = 0,
                    TopOffset = 70,
                    BottomOffset = 185
                },
                Masked = true,
                Text = "Player Email Addresses",
                TextColor = 16777215
            };
            LayoutSession.EndRead(PlayerEMailAddressesLbl);
            EmailAddressEdit1 = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "EmailAddressEdit1",
                Priority = 0,
                WinLeft = 120,
                WinTop = 0,
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
                    LeftOffset = 120,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 230,
                BorderWidth = 8,
                BorderHeight = 5,
                OnChange = EmailAddressEdit1Change,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 1,
                TextColor = 11002593,
                Editable = true,
                ILIndex = 15,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(EmailAddressEdit1);
            PlayerPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PlayerPnl",
                Priority = 1,
                WinLeft = 50,
                WinTop = 153,
                WinWidth = 350,
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
                    LeftOffset = 50,
                    RightOffset = 50,
                    TopOffset = 70,
                    BottomOffset = 60
                },
                WinHeight = 122,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PlayerPnl);
            PlayerLbl1 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PlayerLbl1",
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
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(PlayerLbl1);
            PlayerSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "PlayerSB",
                Priority = 200,
                WinLeft = 333,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                Min = 0,
                Max = 4,
                SmallChange = 1,
                LargeChange = 5,
                Position = 0,
                WinHeight = 121,
                WinWidth = 17,
                NumberVisible = 4,
                OnChange = Layout_PlayerSBChange_4,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(PlayerSB);
            PlayerLbl2 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PlayerLbl2",
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
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(PlayerLbl2);
            PlayerLbl3 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PlayerLbl3",
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
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(PlayerLbl3);
            EmailAddressEdit2 = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "EmailAddressEdit2",
                Tag = 1,
                Priority = 0,
                WinLeft = 120,
                WinTop = 20,
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
                    LeftOffset = 120,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 230,
                BorderWidth = 8,
                BorderHeight = 6,
                OnChange = EmailAddressEdit1Change,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 1,
                TextColor = 11002593,
                Editable = true,
                ILIndex = 15,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(EmailAddressEdit2);
            EmailAddressEdit3 = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "EmailAddressEdit3",
                Tag = 2,
                Priority = 0,
                WinLeft = 120,
                WinTop = 40,
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
                    LeftOffset = 120,
                    RightOffset = 0,
                    TopOffset = 40,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 230,
                BorderWidth = 8,
                BorderHeight = 6,
                OnChange = EmailAddressEdit1Change,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 1,
                TextColor = 11002593,
                Editable = true,
                ILIndex = 15,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(EmailAddressEdit3);
            PlayerLbl4 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PlayerLbl4",
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
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(PlayerLbl4);
            EmailAddressEdit4 = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "EmailAddressEdit4",
                Tag = 3,
                Priority = 0,
                WinLeft = 120,
                WinTop = 60,
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
                    LeftOffset = 120,
                    RightOffset = 0,
                    TopOffset = 60,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 230,
                BorderWidth = 8,
                BorderHeight = 6,
                OnChange = EmailAddressEdit1Change,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 1,
                TextColor = 11002593,
                Editable = true,
                ILIndex = 15,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(EmailAddressEdit4);
            EmailGameNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "EmailGameNameLbl",
                Priority = 100,
                WinLeft = 50,
                WinTop = 80,
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
                    LeftOffset = 50,
                    RightOffset = 0,
                    TopOffset = 80,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Email Game Name",
                TextColor = 16777215
            };
            LayoutSession.EndRead(EmailGameNameLbl);
            EmailGameNameEdit = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "EmailGameNameEdit",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
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
                WinWidth = 349,
                BorderWidth = 8,
                BorderHeight = 6,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 1,
                TextColor = 11002593,
                Editable = true,
                ILIndex = 15,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(EmailGameNameEdit);
            SettingsBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SettingsBtn",
                Priority = 9999,
                WinLeft = 265,
                WinTop = 50,
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
                    LeftOffset = 265,
                    RightOffset = 50,
                    TopOffset = 50,
                    BottomOffset = 11
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Settings",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 135,
                OnClick = Layout_SettingsBtnClick_5
            };
            LayoutSession.EndRead(SettingsBtn);
            Player1Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Player1Pnl",
                Priority = 100,
                WinLeft = 1,
                WinTop = 0,
                WinWidth = 120,
                OnDrawBack = Player6PnlDrawBack,
                Masked = true,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                WinHeight = 21,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Player1Pnl);
            PowePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PowePnl",
                Priority = 1,
                WinLeft = 1,
                WinTop = 0,
                WinWidth = 202,
                Masked = false,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 78
                },
                WinHeight = 22,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PowePnl);
            Player2Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Player2Pnl",
                Tag = 1,
                Priority = 100,
                WinLeft = 1,
                WinTop = 20,
                WinWidth = 120,
                OnDrawBack = Player6PnlDrawBack,
                Masked = true,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 1
                },
                WinHeight = 21,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Player2Pnl);
            Player3Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Player3Pnl",
                Tag = 2,
                Priority = 100,
                WinLeft = 1,
                WinTop = 40,
                WinWidth = 120,
                OnDrawBack = Player6PnlDrawBack,
                Masked = true,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 40,
                    BottomOffset = 1
                },
                WinHeight = 21,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Player3Pnl);
            Player4Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Player4Pnl",
                Tag = 3,
                Priority = 100,
                WinLeft = 1,
                WinTop = 60,
                WinWidth = 120,
                OnDrawBack = Player6PnlDrawBack,
                Masked = true,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 60,
                    BottomOffset = 1
                },
                WinHeight = 21,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Player4Pnl);
            PlayerLbl5 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PlayerLbl5",
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
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(PlayerLbl5);
            EmailAddressEdit5 = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "EmailAddressEdit5",
                Tag = 4,
                Priority = 0,
                WinLeft = 120,
                WinTop = 80,
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
                    LeftOffset = 120,
                    RightOffset = 0,
                    TopOffset = 80,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 230,
                BorderWidth = 8,
                BorderHeight = 6,
                OnChange = EmailAddressEdit1Change,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 1,
                TextColor = 11002593,
                Editable = true,
                ILIndex = 15,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(EmailAddressEdit5);
            PlayerLbl6 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PlayerLbl6",
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
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(PlayerLbl6);
            EmailAddressEdit6 = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "EmailAddressEdit6",
                Tag = 5,
                Priority = 0,
                WinLeft = 120,
                WinTop = 100,
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
                    LeftOffset = 120,
                    RightOffset = 0,
                    TopOffset = 100,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 230,
                BorderWidth = 8,
                BorderHeight = 6,
                OnChange = EmailAddressEdit1Change,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 1,
                TextColor = 11002593,
                Editable = true,
                ILIndex = 15,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(EmailAddressEdit6);
            Player5Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Player5Pnl",
                Tag = 4,
                Priority = 100,
                WinLeft = 1,
                WinTop = 80,
                WinWidth = 120,
                OnDrawBack = Player6PnlDrawBack,
                Masked = true,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 80,
                    BottomOffset = 1
                },
                WinHeight = 21,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Player5Pnl);
            Player6Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Player6Pnl",
                Tag = 5,
                Priority = 100,
                WinLeft = 1,
                WinTop = 100,
                WinWidth = 120,
                OnDrawBack = Player6PnlDrawBack,
                Masked = true,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 100,
                    BottomOffset = 1
                },
                WinHeight = 21,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Player6Pnl);
            NamePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "NamePnl",
                Priority = 1,
                WinLeft = 50,
                WinTop = 100,
                WinWidth = 350,
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
                    TopOffset = 100,
                    BottomOffset = 60
                },
                WinHeight = 22,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(NamePnl);
            LocalPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LocalPnl",
                Priority = 1,
                WinLeft = 50,
                WinTop = 50,
                WinWidth = 200,
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
                    RightOffset = 200,
                    TopOffset = 50,
                    BottomOffset = 60
                },
                WinHeight = 22,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LocalPnl);
            CloseBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CloseBtn",
                Priority = 0,
                WinLeft = 415,
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
                OnClick = Layout_CancelButClick_2
            };
            LayoutSession.EndRead(CloseBtn);
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
            OKBtn.AOWWindow = Dlg;
            TitleIcon.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            CancelBut.AOWWindow = Dlg;
            LocalEmailAdressLbl.AOWWindow = Dlg;
            DecoL.AOWWindow = Dlg;
            DecoR.AOWWindow = Dlg;
            AoWFrame1.AOWWindow = Dlg;
            LocalEmailAddressEdit.AOWWindow = LocalPnl;
            PlayerEMailAddressesLbl.AOWWindow = Dlg;
            EmailAddressEdit1.AOWWindow = PlayerPnl;
            PlayerPnl.AOWWindow = Dlg;
            PlayerLbl1.AOWWindow = Player1Pnl;
            PlayerSB.AOWWindow = PlayerPnl;
            PlayerLbl2.AOWWindow = Player2Pnl;
            PlayerLbl3.AOWWindow = Player3Pnl;
            EmailAddressEdit2.AOWWindow = PlayerPnl;
            EmailAddressEdit3.AOWWindow = PlayerPnl;
            PlayerLbl4.AOWWindow = Player4Pnl;
            EmailAddressEdit4.AOWWindow = PlayerPnl;
            EmailGameNameLbl.AOWWindow = Dlg;
            EmailGameNameEdit.AOWWindow = NamePnl;
            SettingsBtn.AOWWindow = Dlg;
            Player1Pnl.AOWWindow = PlayerPnl;
            Player2Pnl.AOWWindow = PlayerPnl;
            Player3Pnl.AOWWindow = PlayerPnl;
            Player4Pnl.AOWWindow = PlayerPnl;
            PlayerLbl5.AOWWindow = Player5Pnl;
            EmailAddressEdit5.AOWWindow = PlayerPnl;
            PlayerLbl6.AOWWindow = Player6Pnl;
            EmailAddressEdit6.AOWWindow = PlayerPnl;
            Player5Pnl.AOWWindow = PlayerPnl;
            Player6Pnl.AOWWindow = PlayerPnl;
            NamePnl.AOWWindow = Dlg;
            LocalPnl.AOWWindow = Dlg;
            CloseBtn.AOWWindow = Dlg;
            Loaded();
            OKBtn.Loaded();
            TitleIcon.Loaded();
            Title.Loaded();
            CancelBut.Loaded();
            Dlg.Loaded();
            LocalEmailAdressLbl.Loaded();
            DecoL.Loaded();
            DecoR.Loaded();
            AoWFrame1.Loaded();
            LocalEmailAddressEdit.Loaded();
            PlayerEMailAddressesLbl.Loaded();
            EmailAddressEdit1.Loaded();
            PlayerPnl.Loaded();
            PlayerLbl1.Loaded();
            PlayerSB.Loaded();
            PlayerLbl2.Loaded();
            PlayerLbl3.Loaded();
            EmailAddressEdit2.Loaded();
            EmailAddressEdit3.Loaded();
            PlayerLbl4.Loaded();
            EmailAddressEdit4.Loaded();
            EmailGameNameLbl.Loaded();
            EmailGameNameEdit.Loaded();
            SettingsBtn.Loaded();
            Player1Pnl.Loaded();
            PowePnl.Loaded();
            Player2Pnl.Loaded();
            Player3Pnl.Loaded();
            Player4Pnl.Loaded();
            PlayerLbl5.Loaded();
            EmailAddressEdit5.Loaded();
            PlayerLbl6.Loaded();
            EmailAddressEdit6.Loaded();
            Player5Pnl.Loaded();
            Player6Pnl.Loaded();
            NamePnl.Loaded();
            LocalPnl.Loaded();
            CloseBtn.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            OKBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            OKBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBut.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CancelBut.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBut.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            LocalEmailAdressLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            DecoL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            DecoR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AoWFrame1.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            LocalEmailAddressEdit.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            LocalEmailAddressEdit.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            PlayerEMailAddressesLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            EmailAddressEdit1.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            EmailAddressEdit1.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            PlayerPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PlayerPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PlayerLbl1.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            PlayerSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlayerSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlayerSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlayerSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlayerLbl2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PlayerLbl3.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            EmailAddressEdit2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            EmailAddressEdit2.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            EmailAddressEdit3.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            EmailAddressEdit3.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            PlayerLbl4.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            EmailAddressEdit4.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            EmailAddressEdit4.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            EmailGameNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            EmailGameNameEdit.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            EmailGameNameEdit.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            SettingsBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SettingsBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Player1Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PowePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PowePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Player2Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Player3Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Player4Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PlayerLbl5.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            EmailAddressEdit5.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            EmailAddressEdit5.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            PlayerLbl6.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            EmailAddressEdit6.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            EmailAddressEdit6.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            Player5Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Player6Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            NamePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            NamePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            LocalPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            LocalPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CloseBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CloseBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_EmailGameSettingsDlgCreate_0(TObject p0)
        {
            EmailGameSettingsDlgCreate();
        }

        private void Layout_EmailGameSettingsDlgDestroy_1(TObject p0)
        {
            EmailGameSettingsDlgDestroy();
        }

        private void Layout_CancelButClick_2(TObject p0)
        {
            CancelButClick();
        }

        private void Layout_LocalEmailAddressEditUpdate_3(TObject p0)
        {
            LocalEmailAddressEditUpdate();
        }

        private void Layout_PlayerSBChange_4(TObject p0)
        {
            PlayerSBChange();
        }

        private void Layout_SettingsBtnClick_5(TObject p0)
        {
            SettingsBtnClick();
        }
    }
}
