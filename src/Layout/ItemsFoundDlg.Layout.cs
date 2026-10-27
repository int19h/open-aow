// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWItemsFoundDlg
{
    public partial class TItemsFoundDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "ItemsFoundDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 300,
                WinLeft = 128,
                WinTop = 109,
                WinWidth = 385,
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
                DragHeight = 20,
                Draggable = true,
                Resizable = false,
                MinWidth = 0,
                MinHeight = 0,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = true,
                WinHeight = 215,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            TitleLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TitleLbl",
                Priority = 0,
                WinLeft = 25,
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
                Text = "########",
                TextColor = 0
            };
            LayoutSession.EndRead(TitleLbl);
            PickupPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PickupPnl",
                Priority = 100,
                WinLeft = 0,
                WinTop = 175,
                WinWidth = 385,
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
            LayoutSession.EndRead(PickupPnl);
            PickupBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PickupBtn",
                Priority = 1000,
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
                Text = "Pickup",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 105,
                OnClick = Layout_PickupBtnClick_1
            };
            LayoutSession.EndRead(PickupBtn);
            CloseBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CloseBtn",
                Priority = 1000,
                WinLeft = 223,
                WinTop = 3,
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
                Text = "Close",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 105,
                OnClick = Layout_CloseBtn2Click_2
            };
            LayoutSession.EndRead(CloseBtn);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 178,
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
                WinWidth = 343,
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
                WinWidth = 343,
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
                WinWidth = 329,
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
                WinTop = 178,
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
                WinLeft = 330,
                WinTop = 178,
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
            CloseBtn2 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CloseBtn2",
                Priority = 1000,
                WinLeft = 127,
                WinTop = 3,
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
                WinWidth = 130,
                OnClick = Layout_CloseBtn2Click_2
            };
            LayoutSession.EndRead(CloseBtn2);
            OKPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "OKPnl",
                Visible = false,
                Priority = 100,
                WinLeft = 0,
                WinTop = 155,
                WinWidth = 385,
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
            LayoutSession.EndRead(OKPnl);
            MessageMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "MessageMemo",
                Priority = 0,
                WinLeft = 35,
                WinTop = 0,
                CaptureInput = false,
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
                    LeftOffset = 35,
                    RightOffset = 35,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 315,
                WinHeight = 60,
                LayoutItems = new LayoutStrings()
                {
                    "#########################",
                    "#########################",
                    "#########################"
                },
                BorderWidth = 0,
                BorderHeight = 0,
                TextColor = 0,
                CursorChar = (byte)0,
                BlinkDelay = (byte)(0),
                Editable = true,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 20,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(MessageMemo);
            MessageMemoSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "MessageMemoSB",
                Visible = false,
                Priority = 1,
                WinLeft = 333,
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
                    TopOffset = 50,
                    BottomOffset = 27
                },
                Masked = false,
                Min = 0,
                Max = 1,
                SmallChange = 1,
                LargeChange = 2,
                Position = 0,
                WinHeight = 60,
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
            LayoutSession.EndRead(MessageMemoSB);
            MemoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "MemoPnl",
                Priority = 90,
                WinLeft = 0,
                WinTop = 55,
                WinWidth = 385,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 55,
                    BottomOffset = 100
                },
                WinHeight = 60,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(MemoPnl);
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
            Gnd3 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Gnd3",
                Tag = 2,
                OnMouseDown = Layout_Gnd3MouseDown_3,
                OnDraw = Gnd3Draw,
                Priority = 1000,
                WinLeft = 172,
                WinTop = 123,
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
                    LeftOffset = 4,
                    RightOffset = 0,
                    TopOffset = 155,
                    BottomOffset = 50
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 32,
                ILIndexLit = 32,
                ILIndexUp = 32,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Gnd3);
            Gnd2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Gnd2",
                Tag = 1,
                OnMouseDown = Layout_Gnd3MouseDown_3,
                OnDraw = Gnd3Draw,
                Priority = 1000,
                WinLeft = 124,
                WinTop = 123,
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
                    LeftOffset = 124,
                    RightOffset = 0,
                    TopOffset = 155,
                    BottomOffset = 50
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 32,
                ILIndexLit = 32,
                ILIndexUp = 32,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Gnd2);
            Gnd1 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Gnd1",
                OnMouseDown = Layout_Gnd3MouseDown_3,
                OnDraw = Gnd3Draw,
                Priority = 1000,
                WinLeft = 80,
                WinTop = 123,
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
                    LeftOffset = 80,
                    RightOffset = 0,
                    TopOffset = 155,
                    BottomOffset = 50
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 32,
                ILIndexLit = 32,
                ILIndexUp = 32,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Gnd1);
            Gnd4 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Gnd4",
                Tag = 3,
                OnMouseDown = Layout_Gnd3MouseDown_3,
                OnDraw = Gnd3Draw,
                Priority = 1000,
                WinLeft = 221,
                WinTop = 123,
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
                    RightOffset = 123,
                    TopOffset = 155,
                    BottomOffset = 50
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 32,
                ILIndexLit = 32,
                ILIndexUp = 32,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Gnd4);
            Gnd5 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Gnd5",
                Tag = 4,
                OnMouseDown = Layout_Gnd3MouseDown_3,
                OnDraw = Gnd3Draw,
                Priority = 1000,
                WinLeft = 265,
                WinTop = 123,
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
                    LeftOffset = 77,
                    RightOffset = 79,
                    TopOffset = 155,
                    BottomOffset = 50
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 32,
                ILIndexLit = 32,
                ILIndexUp = 32,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Gnd5);
            LeftBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LeftBtn",
                Priority = 0,
                WinLeft = 60,
                WinTop = 138,
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
                    LeftOffset = 60,
                    RightOffset = 36,
                    TopOffset = 4,
                    BottomOffset = 63
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 33,
                ILIndexLit = 32,
                ILIndexUp = 32,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = Layout_LeftBtnClick_4
            };
            LayoutSession.EndRead(LeftBtn);
            RightBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RightBtn",
                Priority = 0,
                WinLeft = 312,
                WinTop = 138,
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
                    LeftOffset = 654,
                    RightOffset = 60,
                    TopOffset = 4,
                    BottomOffset = 63
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 35,
                ILIndexLit = 34,
                ILIndexUp = 34,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = Layout_RightBtnClick_5
            };
            LayoutSession.EndRead(RightBtn);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            TitleLbl.AOWWindow = Dlg;
            PickupPnl.AOWWindow = Dlg;
            PickupBtn.AOWWindow = PickupPnl;
            CloseBtn.AOWWindow = PickupPnl;
            BCeltL.AOWWindow = Dlg;
            TCeltL.AOWWindow = Dlg;
            TCeltLineL.AOWWindow = Dlg;
            AOWImage1.AOWWindow = Dlg;
            AOWImage2.AOWWindow = Dlg;
            CloseBtn2.AOWWindow = OKPnl;
            OKPnl.AOWWindow = Dlg;
            MessageMemo.AOWWindow = MemoPnl;
            MessageMemo.VScrollBar = MessageMemoSB;
            MessageMemoSB.AOWWindow = MemoPnl;
            MemoPnl.AOWWindow = Dlg;
            Gnd3.AOWWindow = Dlg;
            Gnd2.AOWWindow = Dlg;
            Gnd1.AOWWindow = Dlg;
            Gnd4.AOWWindow = Dlg;
            Gnd5.AOWWindow = Dlg;
            LeftBtn.AOWWindow = Dlg;
            RightBtn.AOWWindow = Dlg;
            Loaded();
            Dlg.Loaded();
            TitleLbl.Loaded();
            PickupPnl.Loaded();
            PickupBtn.Loaded();
            CloseBtn.Loaded();
            BCeltL.Loaded();
            TCeltL.Loaded();
            TCeltLineL.Loaded();
            AOWImage1.Loaded();
            AOWImage2.Loaded();
            CloseBtn2.Loaded();
            OKPnl.Loaded();
            MessageMemo.Loaded();
            MessageMemoSB.Loaded();
            MemoPnl.Loaded();
            Translator.Loaded();
            Gnd3.Loaded();
            Gnd2.Loaded();
            Gnd1.Loaded();
            Gnd4.Loaded();
            Gnd5.Loaded();
            LeftBtn.Loaded();
            RightBtn.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TitleLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PickupBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PickupBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            CloseBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CloseBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCeltLineL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AOWImage1.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AOWImage2.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CloseBtn2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CloseBtn2.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            MessageMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            MessageMemoSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MessageMemoSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MessageMemoSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MessageMemoSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            Gnd3.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Gnd3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            Gnd2.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Gnd2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            Gnd1.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Gnd1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            Gnd4.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Gnd4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            Gnd5.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Gnd5.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            LeftBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LeftBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            RightBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RightBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
        }

        private void Layout_DlgHide_0(TObject p0)
        {
            DlgHide();
        }

        private void Layout_PickupBtnClick_1(TObject p0)
        {
            PickupBtnClick();
        }

        private void Layout_CloseBtn2Click_2(TObject p0)
        {
            CloseBtn2Click();
        }

        private void Layout_Gnd3MouseDown_3(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
        {
            Gnd3MouseDown((Aow.Units.AoWCheckBox.TAOWCheckBox)p0, p1, p2, p3, p4);
        }

        private void Layout_LeftBtnClick_4(TObject p0)
        {
            LeftBtnClick();
        }

        private void Layout_RightBtnClick_5(TObject p0)
        {
            RightBtnClick();
        }
    }
}
