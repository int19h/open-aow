// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWFileDialog
{
    public partial class TFileDlgWindow
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "FileDlgWindow";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_FileDlgWindowCreate_0;
            LayoutOnDestroy = Layout_FileDlgWindowDestroy_1;
            FileDialog = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "FileDialog",
                OnKeyDown = FileDialogKeyDown,
                Visible = false,
                Priority = 300,
                WinLeft = 130,
                WinTop = 35,
                WinWidth = 380,
                Masked = true,
                OnHide = Layout_FileDialogHide_2,
                OnShow = Layout_FileDialogShow_3,
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
                DragHeight = 20,
                Draggable = true,
                Resizable = false,
                MinWidth = 0,
                MinHeight = 0,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = true,
                WinHeight = 363,
                Dockable = false
            };
            LayoutSession.EndRead(FileDialog);
            CancelBut = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelBut",
                Priority = 250,
                WinLeft = 205,
                WinTop = 326,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 54,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 53,
                    TopOffset = 0,
                    BottomOffset = 10
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
                WinWidth = 122,
                OnClick = Layout_CloseClick_4
            };
            LayoutSession.EndRead(CancelBut);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 326,
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
            Openbut = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Openbut",
                Priority = 250,
                WinLeft = 53,
                WinTop = 326,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 54,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 53,
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
                Text = "Open",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 122,
                OnClick = OpenbutClick
            };
            LayoutSession.EndRead(Openbut);
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
                Text = "Open",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            FileLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "FileLB",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 309,
                WinHeight = 140,
                BorderWidth = 21,
                BorderHeight = 3,
                OnChange = Layout_FileLBChange_5,
                OnDoubleClick = OpenbutClick,
                Selected = -1,
                OnDrawItem = FileLBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(FileLB);
            FileName = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "FileName",
                Priority = 1000,
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
                    BottomOffset = 46
                },
                Masked = false,
                WinWidth = 309,
                BorderWidth = 7,
                BorderHeight = 5,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 2,
                TextColor = 11002593,
                Editable = false,
                ILIndex = 15,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(FileName);
            ScenLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ScenLbl",
                Visible = false,
                Priority = 0,
                WinLeft = 134,
                WinTop = 142,
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
                    LeftOffset = 36,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 62
                },
                Masked = true,
                Text = "Scenario Info",
                TextColor = 0
            };
            LayoutSession.EndRead(ScenLbl);
            DriveCmb = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "DriveCmb",
                Priority = 2000,
                WinLeft = 135,
                WinTop = 58,
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
                    RightOffset = 35,
                    TopOffset = 58,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 210,
                BorderWidth = 7,
                BorderHeight = 5,
                ListBorderWidth = 7,
                ListBorderHeight = 5,
                NumberVisible = 4,
                OnChange = Layout_DriveCmbChange_6,
                Selected = -1,
                PriorityOpened = 2000,
                TextColor = 11002593,
                TextSelColor = 0,
                ILEIndex = 27,
                ILFIndex = 113,
                ILTnIndex = 21,
                ILTsIndex = 8,
                ItemHeight = 17
            };
            LayoutSession.EndRead(DriveCmb);
            DriveL = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DriveL",
                Priority = 0,
                WinLeft = 7,
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
                    LeftOffset = 7,
                    RightOffset = 0,
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Drive",
                TextColor = 9415072
            };
            LayoutSession.EndRead(DriveL);
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
                OnClick = Layout_CloseClick_4
            };
            LayoutSession.EndRead(Close);
            UpDirBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "UpDirBtn",
                Priority = 900,
                WinLeft = 278,
                WinTop = 20,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Backs up one directory level."
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 4,
                ILIndexDown = 57,
                ILIndexLit = 58,
                ILIndexUp = 56,
                TextColor = 9402970,
                TextSelColor = 12632256,
                WinWidth = 32,
                OnClick = Layout_UpDirBtnClick_7
            };
            LayoutSession.EndRead(UpDirBtn);
            PathL = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PathL",
                Priority = 0,
                WinLeft = 7,
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
                    LeftOffset = 7,
                    RightOffset = 0,
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Path",
                TextColor = 9415072
            };
            LayoutSession.EndRead(PathL);
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
                CurrentImage = 8,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TitleIcon);
            DriveSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "DriveSB",
                Visible = false,
                Priority = 2000,
                WinLeft = 328,
                WinTop = 77,
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
                Masked = false,
                Min = 0,
                Max = 2,
                SmallChange = 1,
                LargeChange = 3,
                Position = 0,
                WinHeight = 76,
                WinWidth = 17,
                NumberVisible = 2,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(DriveSB);
            CeltM = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltM",
                Priority = 100,
                WinLeft = 177,
                WinTop = 326,
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
            LPlayersI = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LPlayersI",
                Priority = 0,
                WinLeft = -5,
                WinTop = -12,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Number of players"
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
                    LeftOffset = -5,
                    RightOffset = 0,
                    TopOffset = -12,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 10,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 45,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LPlayersI);
            LSavedInfo = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LSavedInfo",
                Priority = 0,
                WinLeft = 14,
                WinTop = 156,
                WinWidth = 310,
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
                    LeftOffset = 14,
                    RightOffset = 14,
                    TopOffset = 60,
                    BottomOffset = 0
                },
                WinHeight = 60,
                ILIndexFrame = 105,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LSavedInfo);
            LSizeI = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LSizeI",
                Priority = 0,
                WinLeft = -5,
                WinTop = -12,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Map Size"
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
                    LeftOffset = -5,
                    RightOffset = 0,
                    TopOffset = -12,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 7,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 45,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LSizeI);
            LTownsI = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LTownsI",
                Priority = 0,
                WinLeft = -5,
                WinTop = -12,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Number of Towns"
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
                    LeftOffset = -5,
                    RightOffset = 0,
                    TopOffset = -12,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 45,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LTownsI);
            LTurnsI = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LTurnsI",
                Priority = 0,
                WinLeft = -8,
                WinTop = -12,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Current Turn"
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
                    LeftOffset = -8,
                    RightOffset = 0,
                    TopOffset = -12,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 6,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 45,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LTurnsI);
            LPlayers = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LPlayers",
                Priority = 10,
                WinLeft = 46,
                WinTop = 13,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Number of players"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 60,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##",
                TextColor = 9415072
            };
            LayoutSession.EndRead(LPlayers);
            LSize = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LSize",
                Priority = 10,
                WinLeft = 46,
                WinTop = 13,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Map Size"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 60,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##",
                TextColor = 9415072
            };
            LayoutSession.EndRead(LSize);
            LTowns = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LTowns",
                Priority = 10,
                WinLeft = 49,
                WinTop = 13,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Number of Towns"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 60,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##",
                TextColor = 9415072
            };
            LayoutSession.EndRead(LTowns);
            LTurns = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LTurns",
                Priority = 10,
                WinLeft = 45,
                WinTop = 13,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Current Turn"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 60,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##",
                TextColor = 9415072
            };
            LayoutSession.EndRead(LTurns);
            CeltL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltL",
                Priority = 100,
                WinLeft = 27,
                WinTop = 326,
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
                WinLeft = 326,
                WinTop = 326,
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
            TCelt = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCelt",
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
                    RightOffset = 30,
                    TopOffset = 22,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 322,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCelt);
            SPlayersI = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SPlayersI",
                Priority = 0,
                WinLeft = 0,
                WinTop = -10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Number of players"
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = -10,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 10,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 45,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SPlayersI);
            NewInfo = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "NewInfo",
                Priority = 10,
                WinLeft = 14,
                WinTop = 156,
                WinWidth = 310,
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
                    LeftOffset = 14,
                    RightOffset = 14,
                    TopOffset = 60,
                    BottomOffset = 0
                },
                WinHeight = 60,
                ILIndexFrame = 105,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(NewInfo);
            SSizeI = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SSizeI",
                Priority = 0,
                WinLeft = 0,
                WinTop = -11,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Map Size"
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = -11,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 7,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 45,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SSizeI);
            STownsI = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "STownsI",
                Priority = 0,
                WinLeft = 0,
                WinTop = -9,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Number of Towns"
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = -9,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 45,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(STownsI);
            SPlayers = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SPlayers",
                Priority = 0,
                WinLeft = 61,
                WinTop = 13,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Number of players"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 60,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##",
                TextColor = 9415072
            };
            LayoutSession.EndRead(SPlayers);
            SSize = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SSize",
                Priority = 0,
                WinLeft = 63,
                WinTop = 13,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Map Size"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 60,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##",
                TextColor = 9415072
            };
            LayoutSession.EndRead(SSize);
            STowns = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "STowns",
                Priority = 0,
                WinLeft = 61,
                WinTop = 13,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Number of Towns"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 60,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##",
                TextColor = 9415072
            };
            LayoutSession.EndRead(STowns);
            NewPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "NewPnl",
                Priority = 0,
                WinLeft = 21,
                WinTop = 103,
                WinWidth = 338,
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
                    LeftOffset = 21,
                    RightOffset = 21,
                    TopOffset = 103,
                    BottomOffset = 44
                },
                WinHeight = 216,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(NewPnl);
            SaveLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "SaveLB",
                Priority = 30,
                WinLeft = 1,
                WinTop = 14,
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
                    LeftOffset = 1,
                    RightOffset = 129,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 180,
                WinHeight = 125,
                BorderWidth = 21,
                BorderHeight = 4,
                OnChange = Layout_FileLBChange_5,
                OnDoubleClick = OpenbutClick,
                Selected = -1,
                OnDrawItem = FileLBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(SaveLB);
            SFileName = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "SFileName",
                Priority = 1000,
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
                    BottomOffset = 46
                },
                Masked = false,
                WinWidth = 309,
                BorderWidth = 7,
                BorderHeight = 5,
                OnChange = Layout_SFileNameChange_8,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 2,
                TextColor = 11002593,
                Editable = true,
                ILIndex = 15,
                OnReturn = Layout_SFileNameReturn_9,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(SFileName);
            SavedInfo = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SavedInfo",
                Visible = false,
                Priority = 5,
                WinLeft = 122,
                WinTop = 142,
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
                    LeftOffset = 36,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 62
                },
                Masked = true,
                Text = "Saved Game Info",
                TextColor = 0
            };
            LayoutSession.EndRead(SavedInfo);
            SavePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SavePnl",
                Priority = 0,
                WinLeft = 21,
                WinTop = 103,
                WinWidth = 338,
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
                    LeftOffset = 21,
                    RightOffset = 21,
                    TopOffset = 103,
                    BottomOffset = 44
                },
                WinHeight = 216,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SavePnl);
            DateLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "DateLB",
                Priority = 20,
                WinLeft = 180,
                WinTop = 14,
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
                    LeftOffset = 180,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 130,
                WinHeight = 125,
                BorderWidth = 8,
                BorderHeight = 4,
                OnChange = Layout_FileLBChange_5,
                OnDoubleClick = OpenbutClick,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(DateLB);
            SaveBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SaveBtn",
                Priority = 100,
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
                    RightOffset = 129,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Saved Games",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 180,
                OnClick = SaveBtnClick
            };
            LayoutSession.EndRead(SaveBtn);
            DateBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DateBtn",
                Priority = 100,
                WinLeft = 180,
                WinTop = 0,
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
                    LeftOffset = 180,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Date",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 130,
                OnClick = DateBtnClick
            };
            LayoutSession.EndRead(DateBtn);
            NSBar = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "NSBar",
                Priority = 0,
                WinLeft = 293,
                WinTop = 0,
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
                Masked = false,
                Min = 0,
                Max = 12,
                SmallChange = 1,
                LargeChange = 6,
                Position = 0,
                WinHeight = 140,
                WinWidth = 17,
                NumberVisible = 7,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(NSBar);
            DriveFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "DriveFr",
                Priority = 10,
                WinLeft = 1,
                WinTop = 0,
                WinWidth = 100,
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
                    BottomOffset = 0
                },
                WinHeight = 21,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(DriveFr);
            TopPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TopPnl",
                Priority = 0,
                WinLeft = 35,
                WinTop = 58,
                WinWidth = 310,
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
                    LeftOffset = 35,
                    RightOffset = 35,
                    TopOffset = 58,
                    BottomOffset = 0
                },
                WinHeight = 42,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TopPnl);
            PathFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PathFr",
                Priority = 0,
                WinLeft = 1,
                WinTop = 20,
                WinWidth = 70,
                Masked = true,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 19,
                    BottomOffset = 1
                },
                WinHeight = 21,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PathFr);
            PathEdit = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "PathEdit",
                Priority = 1000,
                WinLeft = 70,
                WinTop = 20,
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
                    LeftOffset = 70,
                    RightOffset = 32,
                    TopOffset = 19,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 208,
                BorderWidth = 7,
                BorderHeight = 5,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 2,
                TextColor = 11002593,
                Editable = false,
                ILIndex = 15,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(PathEdit);
            NewLBFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "NewLBFr",
                Priority = 10,
                WinLeft = 14,
                WinTop = 6,
                WinWidth = 310,
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
                    LeftOffset = 14,
                    RightOffset = 14,
                    TopOffset = 6,
                    BottomOffset = 76
                },
                WinHeight = 141,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(NewLBFr);
            NewInfo1 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "NewInfo1",
                Priority = 0,
                WinLeft = 1,
                WinTop = 20,
                WinWidth = 105,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 66,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 1
                },
                WinHeight = 39,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(NewInfo1);
            NewInfo2 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "NewInfo2",
                Priority = 0,
                WinLeft = 105,
                WinTop = 20,
                WinWidth = 103,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 33,
                    RightOffPercent = 33,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 1
                },
                WinHeight = 39,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(NewInfo2);
            NewInfo3 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "NewInfo3",
                Priority = 0,
                WinLeft = 207,
                WinTop = 20,
                WinWidth = 103,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 66,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 1
                },
                WinHeight = 39,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(NewInfo3);
            SaveLBFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SaveLBFr",
                Priority = 10,
                WinLeft = 14,
                WinTop = 6,
                WinWidth = 310,
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
                    LeftOffset = 14,
                    RightOffset = 14,
                    TopOffset = 6,
                    BottomOffset = 89
                },
                WinHeight = 140,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SaveLBFr);
            SSBar = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "SSBar",
                Priority = 20,
                WinLeft = 293,
                WinTop = 14,
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
                Masked = false,
                Min = 0,
                Max = 13,
                SmallChange = 1,
                LargeChange = 6,
                Position = 0,
                WinHeight = 125,
                WinWidth = 17,
                NumberVisible = 7,
                OnChange = Layout_SSBarChange_10,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(SSBar);
            SavedInfo1 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SavedInfo1",
                Priority = 0,
                WinLeft = 1,
                WinTop = 20,
                WinWidth = 77,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 1
                },
                WinHeight = 39,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SavedInfo1);
            SavedInfo2 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SavedInfo2",
                Priority = 0,
                WinLeft = 77,
                WinTop = 20,
                WinWidth = 78,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 24,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 1
                },
                WinHeight = 39,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SavedInfo2);
            SavedInfo3 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SavedInfo3",
                Priority = 0,
                WinLeft = 154,
                WinTop = 20,
                WinWidth = 82,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 49,
                    RightOffPercent = 24,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 1
                },
                WinHeight = 39,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SavedInfo3);
            SavedInfo4 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SavedInfo4",
                Priority = 0,
                WinLeft = 235,
                WinTop = 20,
                WinWidth = 75,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 1
                },
                WinHeight = 39,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SavedInfo4);
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
            CancelBut.AOWWindow = FileDialog;
            BCeltL.AOWWindow = FileDialog;
            Openbut.AOWWindow = FileDialog;
            Title.AOWWindow = FileDialog;
            FileLB.AOWWindow = NewLBFr;
            FileLB.VScrollBar = NSBar;
            FileName.AOWWindow = NewInfo;
            ScenLbl.AOWWindow = NewPnl;
            DriveCmb.AOWWindow = FileDialog;
            DriveCmb.VScrollBar = DriveSB;
            DriveL.AOWWindow = DriveFr;
            Close.AOWWindow = FileDialog;
            UpDirBtn.AOWWindow = TopPnl;
            PathL.AOWWindow = PathFr;
            TitleIcon.AOWWindow = FileDialog;
            DriveSB.AOWWindow = FileDialog;
            CeltM.AOWWindow = FileDialog;
            LPlayersI.AOWWindow = SavedInfo1;
            LSavedInfo.AOWWindow = SavePnl;
            LSizeI.AOWWindow = SavedInfo2;
            LTownsI.AOWWindow = SavedInfo3;
            LTurnsI.AOWWindow = SavedInfo4;
            LPlayers.AOWWindow = SavedInfo1;
            LSize.AOWWindow = SavedInfo2;
            LTowns.AOWWindow = SavedInfo3;
            LTurns.AOWWindow = SavedInfo4;
            CeltL.AOWWindow = FileDialog;
            CeltR.AOWWindow = FileDialog;
            TCeltL.AOWWindow = FileDialog;
            TCelt.AOWWindow = FileDialog;
            SPlayersI.AOWWindow = NewInfo3;
            NewInfo.AOWWindow = NewPnl;
            SSizeI.AOWWindow = NewInfo1;
            STownsI.AOWWindow = NewInfo2;
            SPlayers.AOWWindow = NewInfo3;
            SSize.AOWWindow = NewInfo1;
            STowns.AOWWindow = NewInfo2;
            NewPnl.AOWWindow = FileDialog;
            SaveLB.AOWWindow = SaveLBFr;
            SFileName.AOWWindow = LSavedInfo;
            SavedInfo.AOWWindow = SavePnl;
            SavePnl.AOWWindow = FileDialog;
            DateLB.AOWWindow = SaveLBFr;
            DateLB.VScrollBar = SSBar;
            SaveBtn.AOWWindow = SaveLBFr;
            DateBtn.AOWWindow = SaveLBFr;
            NSBar.AOWWindow = NewLBFr;
            DriveFr.AOWWindow = TopPnl;
            TopPnl.AOWWindow = FileDialog;
            PathFr.AOWWindow = TopPnl;
            PathEdit.AOWWindow = TopPnl;
            NewLBFr.AOWWindow = NewPnl;
            NewInfo1.AOWWindow = NewInfo;
            NewInfo2.AOWWindow = NewInfo;
            NewInfo3.AOWWindow = NewInfo;
            SaveLBFr.AOWWindow = SavePnl;
            SSBar.AOWWindow = SaveLBFr;
            SavedInfo1.AOWWindow = LSavedInfo;
            SavedInfo2.AOWWindow = LSavedInfo;
            SavedInfo3.AOWWindow = LSavedInfo;
            SavedInfo4.AOWWindow = LSavedInfo;
            Loaded();
            FileDialog.Loaded();
            CancelBut.Loaded();
            BCeltL.Loaded();
            Openbut.Loaded();
            Title.Loaded();
            FileLB.Loaded();
            FileName.Loaded();
            ScenLbl.Loaded();
            DriveCmb.Loaded();
            DriveL.Loaded();
            Close.Loaded();
            UpDirBtn.Loaded();
            PathL.Loaded();
            TitleIcon.Loaded();
            DriveSB.Loaded();
            CeltM.Loaded();
            LPlayersI.Loaded();
            LSavedInfo.Loaded();
            LSizeI.Loaded();
            LTownsI.Loaded();
            LTurnsI.Loaded();
            LPlayers.Loaded();
            LSize.Loaded();
            LTowns.Loaded();
            LTurns.Loaded();
            CeltL.Loaded();
            CeltR.Loaded();
            TCeltL.Loaded();
            TCelt.Loaded();
            SPlayersI.Loaded();
            NewInfo.Loaded();
            SSizeI.Loaded();
            STownsI.Loaded();
            SPlayers.Loaded();
            SSize.Loaded();
            STowns.Loaded();
            NewPnl.Loaded();
            SaveLB.Loaded();
            SFileName.Loaded();
            SavedInfo.Loaded();
            SavePnl.Loaded();
            DateLB.Loaded();
            SaveBtn.Loaded();
            DateBtn.Loaded();
            NSBar.Loaded();
            DriveFr.Loaded();
            TopPnl.Loaded();
            PathFr.Loaded();
            PathEdit.Loaded();
            NewLBFr.Loaded();
            NewInfo1.Loaded();
            NewInfo2.Loaded();
            NewInfo3.Loaded();
            SaveLBFr.Loaded();
            SSBar.Loaded();
            SavedInfo1.Loaded();
            SavedInfo2.Loaded();
            SavedInfo3.Loaded();
            SavedInfo4.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            FileDialog.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            FileDialog.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            FileDialog.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CancelBut.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBut.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            Openbut.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Openbut.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            FileLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            FileLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            FileLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            FileLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            FileName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            FileName.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            ScenLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ScenLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DriveCmb.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DriveCmb.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            DriveCmb.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            DriveCmb.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            DriveCmb.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            DriveL.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            DriveL.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            UpDirBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UpDirBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            UpDirBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            PathL.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PathL.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            DriveSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            DriveSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            DriveSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            DriveSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            CeltM.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            LPlayersI.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LPlayersI.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            LSavedInfo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            LSavedInfo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            LSizeI.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LSizeI.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            LTownsI.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LTownsI.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            LTurnsI.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LTurnsI.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            LPlayers.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LPlayers.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            LSize.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LSize.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            LTowns.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LTowns.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            LTurns.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LTurns.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CeltL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CeltR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCelt.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            SPlayersI.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SPlayersI.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            NewInfo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            NewInfo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SSizeI.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SSizeI.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            STownsI.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            STownsI.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            SPlayers.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SPlayers.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SSize.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SSize.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            STowns.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            STowns.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            NewPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SaveLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SaveLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SaveLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SaveLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            SFileName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SFileName.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            SavedInfo.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SavedInfo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SavePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            DateLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DateLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            DateLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            DateLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            SaveBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SaveBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            DateBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DateBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            NSBar.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            NSBar.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            NSBar.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            NSBar.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            DriveFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            DriveFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TopPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TopPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PathFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PathFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PathEdit.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PathEdit.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            NewLBFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            NewLBFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            NewInfo1.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            NewInfo1.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            NewInfo2.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            NewInfo2.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            NewInfo3.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            NewInfo3.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SaveLBFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SaveLBFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SSBar.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SSBar.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SSBar.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SSBar.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SavedInfo1.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SavedInfo1.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SavedInfo2.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SavedInfo2.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SavedInfo3.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SavedInfo3.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SavedInfo4.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SavedInfo4.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_FileDlgWindowCreate_0(TObject p0)
        {
            FileDlgWindowCreate();
        }

        private void Layout_FileDlgWindowDestroy_1(TObject p0)
        {
            FileDlgWindowDestroy();
        }

        private void Layout_FileDialogHide_2(TObject p0)
        {
            FileDialogHide();
        }

        private void Layout_FileDialogShow_3(TObject p0)
        {
            FileDialogShow();
        }

        private void Layout_CloseClick_4(TObject p0)
        {
            CloseClick();
        }

        private void Layout_FileLBChange_5(TObject p0)
        {
            FileLBChange((Aow.Units.AoWListBox.TAOWListBox)p0);
        }

        private void Layout_DriveCmbChange_6(TObject p0)
        {
            DriveCmbChange();
        }

        private void Layout_UpDirBtnClick_7(TObject p0)
        {
            UpDirBtnClick();
        }

        private void Layout_SFileNameChange_8(TObject p0)
        {
            SFileNameChange((int)Aow.Rtl.Rtl.ObjectToInt(p0));
        }

        private void Layout_SFileNameReturn_9(TObject p0)
        {
            SFileNameReturn();
        }

        private void Layout_SSBarChange_10(TObject p0)
        {
            SSBarChange();
        }
    }
}
