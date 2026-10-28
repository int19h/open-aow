// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWSoundDlg
{
    public partial class TSoundDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "SoundDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_SoundDlgWindowCreate_0;
            LayoutOnDestroy = Layout_SoundDlgWindowDestroy_1;
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 300,
                WinLeft = 110,
                WinTop = 16,
                WinWidth = 420,
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
                WinHeight = 400,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            CloseBut = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CloseBut",
                Priority = 250,
                WinLeft = 142,
                WinTop = 363,
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
                    LeftOffset = 266,
                    RightOffset = 28,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Close",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 136,
                OnClick = Layout_CloseClick_2
            };
            LayoutSession.EndRead(CloseBut);
            TCeltLL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltLL",
                Priority = 100,
                WinLeft = 28,
                WinTop = 363,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 81,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCeltLL);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 363,
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
                WinWidth = 378,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(BCeltL);
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
                Text = "Audio Settings",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            SongLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "SongLB",
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
                WinWidth = 172,
                WinHeight = 114,
                BorderWidth = 6,
                BorderHeight = 3,
                OnChange = Layout_SongLBChange_3,
                OnDoubleClick = AddBtnClick,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(SongLB);
            SongLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SongLbl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 10,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Songs Folder:",
                TextColor = 0
            };
            LayoutSession.EndRead(SongLbl);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 385,
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
                OnClick = Layout_CloseClick_2
            };
            LayoutSession.EndRead(Close);
            PlayListPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PlayListPnl",
                Priority = 0,
                WinLeft = 30,
                WinTop = 170,
                WinWidth = 360,
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
                    LeftOffset = 30,
                    RightOffset = 30,
                    TopOffset = 20,
                    BottomOffset = 50
                },
                WinHeight = 180,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PlayListPnl);
            SoundTab = new Aow.Units.AoWTabPanel.TAOWTabPanel(this)
            {
                Name = "SoundTab",
                Priority = 1000,
                WinLeft = 20,
                WinTop = 150,
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
                    LeftOffset = 20,
                    RightOffset = 20,
                    TopOffset = 150,
                    BottomOffset = 0
                },
                Masked = true,
                LayoutItems = new LayoutStrings()
                {
                    "PLAY LIST",
                    "SOUND OPTIONS"
                },
                BorderHeight = 6,
                OnChange = Layout_SoundTabChange_4,
                Selected = 0,
                WinWidth = 380,
                TextColor = 0,
                TextSelColor = 9415072,
                ILIndexDown = 67,
                ILIndexUp = 70,
                BorderHeightSelected = 7
            };
            LayoutSession.EndRead(SoundTab);
            PlayLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "PlayLB",
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
                    LeftOffPercent = 1,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 172,
                WinHeight = 114,
                BorderWidth = 20,
                BorderHeight = 3,
                OnChange = Layout_PlayLBChange_5,
                OnDoubleClick = Layout_PlayBtnClick_6,
                Selected = -1,
                OnDrawItem = PlayLBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(PlayLB);
            AddBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AddBtn",
                Priority = 250,
                WinLeft = 0,
                WinTop = 148,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Add the selected song to the Jukebox Playlist."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Add to List",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 173,
                OnClick = AddBtnClick
            };
            LayoutSession.EndRead(AddBtn);
            RemoveBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RemoveBtn",
                Priority = 250,
                WinLeft = 187,
                WinTop = 148,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Remove the selected song from the Jukebox Playlist."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 26,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Remove",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 80,
                OnClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RemoveBtn);
            PlayBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PlayBtn",
                Priority = 250,
                WinLeft = 273,
                WinTop = 148,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Play the selected song."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 76,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Play",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 87,
                OnClick = Layout_PlayBtnClick_6
            };
            LayoutSession.EndRead(PlayBtn);
            PlayLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PlayLbl",
                Priority = 0,
                WinLeft = 187,
                WinTop = 10,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Play List:",
                TextColor = 0
            };
            LayoutSession.EndRead(PlayLbl);
            CurrentSongLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CurrentSongLbl",
                Priority = 0,
                WinLeft = 75,
                WinTop = 8,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 51,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 62
                },
                Masked = true,
                Text = "#########",
                TextColor = 0
            };
            LayoutSession.EndRead(CurrentSongLbl);
            TCeltLR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltLR",
                Priority = 100,
                WinLeft = 311,
                WinTop = 363,
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
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 81,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCeltLR);
            MusicVolLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MusicVolLbl",
                Priority = 0,
                WinLeft = 55,
                WinTop = 38,
                LayoutToolTipText = new LayoutStrings()
                {
                    "The volume to play music at."
                },
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 44,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 38
                },
                Masked = true,
                Text = "Music Volume:",
                TextColor = 0
            };
            LayoutSession.EndRead(MusicVolLbl);
            SFXVolLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SFXVolLbl",
                Priority = 0,
                WinLeft = 61,
                WinTop = 69,
                LayoutToolTipText = new LayoutStrings()
                {
                    "The volume to play sfx at."
                },
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 44,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 7
                },
                Masked = true,
                Text = "SFX Volume:",
                TextColor = 0
            };
            LayoutSession.EndRead(SFXVolLbl);
            MainPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "MainPnl",
                Priority = 0,
                WinLeft = 90,
                WinTop = 50,
                WinWidth = 240,
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
                    LeftOffset = 90,
                    RightOffset = 90,
                    TopOffset = 50,
                    BottomOffset = 50
                },
                WinHeight = 88,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(MainPnl);
            SoundLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SoundLbl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 10,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Sound Quality:",
                TextColor = 0
            };
            LayoutSession.EndRead(SoundLbl);
            SettingsPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SettingsPnl",
                Visible = false,
                Priority = 0,
                WinLeft = 30,
                WinTop = 180,
                WinWidth = 360,
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
                    LeftOffset = 30,
                    RightOffset = 30,
                    TopOffset = 50,
                    BottomOffset = 50
                },
                WinHeight = 170,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SettingsPnl);
            SFXLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SFXLbl",
                Priority = 0,
                WinLeft = 187,
                WinTop = 10,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Active Sounds:",
                TextColor = 0
            };
            LayoutSession.EndRead(SFXLbl);
            SQPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SQPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 30,
                WinWidth = 173,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 30,
                    BottomOffset = 40
                },
                WinHeight = 100,
                ILIndexFrame = 121,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SQPnl);
            SQCB1 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "SQCB1",
                Priority = 0,
                WinLeft = 15,
                WinTop = 14,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Do not play music or sfx."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 51,
                ILIndexLit = 52,
                ILIndexUp = 50,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = SQCB1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(SQCB1);
            SQLBL1 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SQLBL1",
                Priority = 0,
                WinLeft = 35,
                WinTop = 15,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Do not play music or sfx."
                },
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
                    LeftOffset = 35,
                    RightOffset = 0,
                    TopOffset = 15,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "No Sound",
                TextColor = 9415072
            };
            LayoutSession.EndRead(SQLBL1);
            SQCB2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "SQCB2",
                Priority = 0,
                WinLeft = 15,
                WinTop = 34,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Play music and sfx in low quality (recommended for slower computers)."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 34,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 51,
                ILIndexLit = 52,
                ILIndexUp = 50,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = SQCB2Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(SQCB2);
            SQLBL2 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SQLBL2",
                Priority = 0,
                WinLeft = 35,
                WinTop = 35,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Play music and sfx in low quality (recommended for slower computers)."
                },
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
                    LeftOffset = 35,
                    RightOffset = 0,
                    TopOffset = 35,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Low",
                TextColor = 9415072
            };
            LayoutSession.EndRead(SQLBL2);
            SQCB3 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "SQCB3",
                Priority = 0,
                WinLeft = 15,
                WinTop = 54,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Play music and sfx in medium quality (recommended for most computers)."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 54,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 51,
                ILIndexLit = 52,
                ILIndexUp = 50,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = SQCB3Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(SQCB3);
            SQLBL3 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SQLBL3",
                Priority = 0,
                WinLeft = 35,
                WinTop = 55,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Play music and sfx in medium quality (recommended for most computers)."
                },
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
                    LeftOffset = 35,
                    RightOffset = 0,
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Normal",
                TextColor = 9415072
            };
            LayoutSession.EndRead(SQLBL3);
            SQCB4 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "SQCB4",
                Priority = 0,
                WinLeft = 15,
                WinTop = 74,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Play music and sfx in high quality (recommended for fast computers)."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 51,
                ILIndexLit = 52,
                ILIndexUp = 50,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = SQCB4Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(SQCB4);
            SQLBL4 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SQLBL4",
                Priority = 0,
                WinLeft = 35,
                WinTop = 75,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Play music and sfx in high quality (recommended for fast computers)."
                },
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
                    LeftOffset = 35,
                    RightOffset = 0,
                    TopOffset = 75,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "High",
                TextColor = 9415072
            };
            LayoutSession.EndRead(SQLBL4);
            SFXPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SFXPnl",
                Priority = 0,
                WinLeft = 187,
                WinTop = 30,
                WinWidth = 173,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 30,
                    BottomOffset = 40
                },
                WinHeight = 100,
                ILIndexFrame = 121,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SFXPnl);
            MusicEnabledChk = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "MusicEnabledChk",
                Priority = 0,
                WinLeft = 15,
                WinTop = 20,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether or not to play music."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 48,
                ILIndexLit = 49,
                ILIndexUp = 47,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = MusicEnabledChkChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(MusicEnabledChk);
            SFXLBL2 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SFXLBL2",
                Priority = 0,
                WinLeft = 33,
                WinTop = 22,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether or not to play music."
                },
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
                    LeftOffset = 33,
                    RightOffset = 0,
                    TopOffset = 22,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Music",
                TextColor = 9415072
            };
            LayoutSession.EndRead(SFXLBL2);
            SFXEnabledChk = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "SFXEnabledChk",
                Priority = 0,
                WinLeft = 15,
                WinTop = 45,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether or not to play sfx."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 45,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 48,
                ILIndexLit = 49,
                ILIndexUp = 47,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = SFXEnabledChkChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(SFXEnabledChk);
            SFXLBL3 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SFXLBL3",
                Priority = 0,
                WinLeft = 33,
                WinTop = 46,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether or not to play sfx."
                },
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
                    LeftOffset = 33,
                    RightOffset = 0,
                    TopOffset = 46,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Sound Effects",
                TextColor = 9415072
            };
            LayoutSession.EndRead(SFXLBL3);
            TopLine = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "TopLine",
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
                WinWidth = 378,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(TopLine);
            AOWImage4 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AOWImage4",
                Priority = 1000,
                WinLeft = 28,
                WinTop = 22,
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
                    LeftOffset = 28,
                    RightOffset = 0,
                    TopOffset = 22,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 351,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(AOWImage4);
            HarpL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "HarpL",
                Priority = 0,
                WinLeft = 30,
                WinTop = 55,
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 2,
                WinWidth = 51,
                WinHeight = 76,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(HarpL);
            HarpR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "HarpR",
                Priority = 0,
                WinLeft = 339,
                WinTop = 55,
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
                    LeftOffset = 35,
                    RightOffset = 30,
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 2,
                WinWidth = 51,
                WinHeight = 76,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(HarpR);
            MusicVolume = new Aow.Units.AoWHScroll.TAOWHScrollBar(this)
            {
                Name = "MusicVolume",
                Priority = 0,
                WinLeft = 140,
                WinTop = 35,
                LayoutToolTipText = new LayoutStrings()
                {
                    "The volume to play music at."
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
                    LeftOffset = 140,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 35
                },
                Masked = true,
                Min = 0,
                Max = 100,
                SmallChange = 1,
                LargeChange = 10,
                Position = 0,
                WinHeight = 18,
                WinWidth = 92,
                NumberVisible = 10,
                OnChange = Layout_MusicVolumeChange_7,
                VanishWhenFull = false,
                ILIndexIncUp = 0,
                ILIndexIncDown = 0,
                ILIndexDecUp = 0,
                ILIndexDecDown = 0,
                ILIndexBack = 80,
                ILIndexTab = 83
            };
            LayoutSession.EndRead(MusicVolume);
            SFXVolume = new Aow.Units.AoWHScroll.TAOWHScrollBar(this)
            {
                Name = "SFXVolume",
                Priority = 0,
                WinLeft = 140,
                WinTop = 66,
                LayoutToolTipText = new LayoutStrings()
                {
                    "The volume to play sfx at."
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
                    LeftOffset = 140,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                Masked = true,
                Min = 0,
                Max = 100,
                SmallChange = 1,
                LargeChange = 10,
                Position = 0,
                WinHeight = 18,
                WinWidth = 92,
                NumberVisible = 10,
                OnChange = Layout_SFXVolumeChange_8,
                VanishWhenFull = false,
                ILIndexIncUp = 0,
                ILIndexIncDown = 0,
                ILIndexDecUp = 0,
                ILIndexDecDown = 0,
                ILIndexBack = 80,
                ILIndexTab = 83
            };
            LayoutSession.EndRead(SFXVolume);
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
                CurrentImage = 10,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TitleIcon);
            SongSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "SongSB",
                Visible = false,
                Priority = 1,
                WinLeft = 156,
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
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 114,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(SongSB);
            PlaySB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "PlaySB",
                Visible = false,
                Priority = 1,
                WinLeft = 156,
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
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 114,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(PlaySB);
            DirectSoundEnabledChk = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "DirectSoundEnabledChk",
                Priority = 0,
                WinLeft = 15,
                WinTop = 70,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether or not to use DirectSound (using DirectSound is generally faster, but may not work on older hardware)."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 70,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 48,
                ILIndexLit = 49,
                ILIndexUp = 47,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = DirectSoundEnabledChkChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(DirectSoundEnabledChk);
            UseDirectSoundLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UseDirectSoundLbl",
                Priority = 0,
                WinLeft = 33,
                WinTop = 72,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether or not to use DirectSound (using DirectSound is generally faster, but may not work on older hardware)."
                },
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
                    LeftOffset = 33,
                    RightOffset = 0,
                    TopOffset = 72,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Use Direct Sound",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UseDirectSoundLbl);
            FolderPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "FolderPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 25,
                WinWidth = 173,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 25,
                    BottomOffset = 40
                },
                WinHeight = 115,
                ILIndexFrame = 105,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(FolderPnl);
            PlayPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PlayPnl",
                Priority = 0,
                WinLeft = 187,
                WinTop = 25,
                WinWidth = 173,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 25,
                    BottomOffset = 40
                },
                WinHeight = 115,
                ILIndexFrame = 105,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PlayPnl);
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
            CloseBut.AOWWindow = Dlg;
            TCeltLL.AOWWindow = Dlg;
            BCeltL.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            SongLB.AOWWindow = FolderPnl;
            SongLB.VScrollBar = SongSB;
            SongLbl.AOWWindow = PlayListPnl;
            Close.AOWWindow = Dlg;
            PlayListPnl.AOWWindow = Dlg;
            PlayListPnl.TabPanel = SoundTab;
            SoundTab.AOWWindow = Dlg;
            PlayLB.AOWWindow = PlayPnl;
            PlayLB.VScrollBar = PlaySB;
            AddBtn.AOWWindow = PlayListPnl;
            RemoveBtn.AOWWindow = PlayListPnl;
            PlayBtn.AOWWindow = PlayListPnl;
            PlayLbl.AOWWindow = PlayListPnl;
            CurrentSongLbl.AOWWindow = MainPnl;
            TCeltLR.AOWWindow = Dlg;
            MusicVolLbl.AOWWindow = MainPnl;
            SFXVolLbl.AOWWindow = MainPnl;
            MainPnl.AOWWindow = Dlg;
            SoundLbl.AOWWindow = SettingsPnl;
            SettingsPnl.AOWWindow = Dlg;
            SettingsPnl.TabPanel = SoundTab;
            SFXLbl.AOWWindow = SettingsPnl;
            SQPnl.AOWWindow = SettingsPnl;
            SQCB1.AOWWindow = SQPnl;
            SQLBL1.AOWWindow = SQPnl;
            SQCB2.AOWWindow = SQPnl;
            SQLBL2.AOWWindow = SQPnl;
            SQCB3.AOWWindow = SQPnl;
            SQLBL3.AOWWindow = SQPnl;
            SQCB4.AOWWindow = SQPnl;
            SQLBL4.AOWWindow = SQPnl;
            SFXPnl.AOWWindow = SettingsPnl;
            MusicEnabledChk.AOWWindow = SFXPnl;
            SFXLBL2.AOWWindow = SFXPnl;
            SFXEnabledChk.AOWWindow = SFXPnl;
            SFXLBL3.AOWWindow = SFXPnl;
            TopLine.AOWWindow = Dlg;
            AOWImage4.AOWWindow = Dlg;
            HarpL.AOWWindow = Dlg;
            HarpR.AOWWindow = Dlg;
            MusicVolume.AOWWindow = MainPnl;
            SFXVolume.AOWWindow = MainPnl;
            TitleIcon.AOWWindow = Dlg;
            SongSB.AOWWindow = FolderPnl;
            PlaySB.AOWWindow = PlayPnl;
            DirectSoundEnabledChk.AOWWindow = SFXPnl;
            UseDirectSoundLbl.AOWWindow = SFXPnl;
            FolderPnl.AOWWindow = PlayListPnl;
            FolderPnl.TabPanel = SoundTab;
            PlayPnl.AOWWindow = PlayListPnl;
            PlayPnl.TabPanel = SoundTab;
            Loaded();
            Dlg.Loaded();
            CloseBut.Loaded();
            TCeltLL.Loaded();
            BCeltL.Loaded();
            Title.Loaded();
            SongLB.Loaded();
            SongLbl.Loaded();
            Close.Loaded();
            PlayListPnl.Loaded();
            SoundTab.Loaded();
            PlayLB.Loaded();
            AddBtn.Loaded();
            RemoveBtn.Loaded();
            PlayBtn.Loaded();
            PlayLbl.Loaded();
            CurrentSongLbl.Loaded();
            TCeltLR.Loaded();
            MusicVolLbl.Loaded();
            SFXVolLbl.Loaded();
            MainPnl.Loaded();
            SoundLbl.Loaded();
            SettingsPnl.Loaded();
            SFXLbl.Loaded();
            SQPnl.Loaded();
            SQCB1.Loaded();
            SQLBL1.Loaded();
            SQCB2.Loaded();
            SQLBL2.Loaded();
            SQCB3.Loaded();
            SQLBL3.Loaded();
            SQCB4.Loaded();
            SQLBL4.Loaded();
            SFXPnl.Loaded();
            MusicEnabledChk.Loaded();
            SFXLBL2.Loaded();
            SFXEnabledChk.Loaded();
            SFXLBL3.Loaded();
            TopLine.Loaded();
            AOWImage4.Loaded();
            HarpL.Loaded();
            HarpR.Loaded();
            MusicVolume.Loaded();
            SFXVolume.Loaded();
            TitleIcon.Loaded();
            SongSB.Loaded();
            PlaySB.Loaded();
            DirectSoundEnabledChk.Loaded();
            UseDirectSoundLbl.Loaded();
            FolderPnl.Loaded();
            PlayPnl.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CloseBut.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CloseBut.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            TCeltLL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SongLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SongLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SongLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SongLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            SongLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            PlayListPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SoundTab.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SoundTab.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            PlayLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PlayLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PlayLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PlayLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            AddBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AddBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            AddBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            RemoveBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RemoveBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RemoveBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            PlayBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PlayBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PlayBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            PlayLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            CurrentSongLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            TCeltLR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            MusicVolLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MusicVolLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SFXVolLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SFXVolLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MainPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SoundLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SettingsPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SFXLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SQPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SQPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SQCB1.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SQCB1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            SQLBL1.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SQLBL1.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            SQCB2.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SQCB2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            SQLBL2.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SQLBL2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            SQCB3.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SQCB3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            SQLBL3.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SQLBL3.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            SQCB4.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SQCB4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            SQLBL4.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SQLBL4.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            SFXPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SFXPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            MusicEnabledChk.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MusicEnabledChk.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            SFXLBL2.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SFXLBL2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            SFXEnabledChk.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SFXEnabledChk.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            SFXLBL3.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SFXLBL3.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            TopLine.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AOWImage4.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            HarpL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.SoundWin;
            HarpR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.SoundWin;
            MusicVolume.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MusicVolume.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MusicVolume.ILLBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MusicVolume.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SFXVolume.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SFXVolume.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SFXVolume.ILLBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SFXVolume.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            SongSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SongSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SongSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SongSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlaySB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlaySB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlaySB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlaySB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            DirectSoundEnabledChk.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            DirectSoundEnabledChk.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            UseDirectSoundLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UseDirectSoundLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            FolderPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PlayPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_SoundDlgWindowCreate_0(TObject p0)
        {
            SoundDlgWindowCreate();
        }

        private void Layout_SoundDlgWindowDestroy_1(TObject p0)
        {
            SoundDlgWindowDestroy();
        }

        private void Layout_CloseClick_2(TObject p0)
        {
            CloseClick();
        }

        private void Layout_SongLBChange_3(TObject p0)
        {
            SongLBChange();
        }

        private void Layout_SoundTabChange_4(TObject p0)
        {
            SoundTabChange();
        }

        private void Layout_PlayLBChange_5(TObject p0)
        {
            PlayLBChange();
        }

        private void Layout_PlayBtnClick_6(TObject p0)
        {
            PlayBtnClick();
        }

        private void Layout_MusicVolumeChange_7(TObject p0)
        {
            MusicVolumeChange();
        }

        private void Layout_SFXVolumeChange_8(TObject p0)
        {
            SFXVolumeChange();
        }
    }
}
