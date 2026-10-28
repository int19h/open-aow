// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWSpellBook
{
    public partial class TSpellBook
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "SpellBook";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_SpellBookCreate_0;
            LayoutOnDestroy = Layout_SpellBookDestroy_1;
            LeftHeader = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LeftHeader",
                Priority = 500,
                WinLeft = 87,
                WinTop = 9,
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
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Global Spells",
                TextColor = 5908276
            };
            LayoutSession.EndRead(LeftHeader);
            LeftSpellPage = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LeftSpellPage",
                Visible = false,
                Priority = 0,
                WinLeft = 29,
                WinTop = 11,
                WinWidth = 272,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 372,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LeftSpellPage);
            S1Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "S1Pnl",
                Priority = 1000,
                WinLeft = 8,
                WinTop = 30,
                WinWidth = 262,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
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
                WinHeight = 80,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(S1Pnl);
            S1Spell = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S1Spell",
                Priority = 0,
                WinLeft = 57,
                WinTop = 3,
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
                Text = "####",
                TextColor = 1254460
            };
            LayoutSession.EndRead(S1Spell);
            S1Class = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S1Class",
                Priority = 0,
                WinLeft = 57,
                WinTop = 16,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S1Class);
            S1Memo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "S1Memo",
                Priority = 0,
                WinLeft = 2,
                WinTop = 41,
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
                WinWidth = 260,
                WinHeight = 35,
                BorderWidth = 10,
                BorderHeight = 0,
                TextColor = 1254460,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(S1Memo);
            S1Cost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S1Cost",
                Priority = 0,
                WinLeft = 57,
                WinTop = 28,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S1Cost);
            S1Icn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "S1Icn",
                OnDraw = SpellIconDraw,
                Priority = 0,
                WinLeft = 10,
                WinTop = 2,
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
                AnimStart = 0,
                CurrentImage = 38,
                AnimEnd = 0,
                WinWidth = 40,
                WinHeight = 35,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(S1Icn);
            S2Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "S2Pnl",
                Tag = 1,
                Priority = 1000,
                WinLeft = 8,
                WinTop = 110,
                WinWidth = 262,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 110,
                    BottomOffset = 0
                },
                WinHeight = 80,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(S2Pnl);
            S2Spell = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S2Spell",
                Tag = 1,
                Priority = 0,
                WinLeft = 57,
                WinTop = 3,
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
                Text = "####",
                TextColor = 1254460
            };
            LayoutSession.EndRead(S2Spell);
            S2Class = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S2Class",
                Tag = 1,
                Priority = 0,
                WinLeft = 57,
                WinTop = 16,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S2Class);
            S2Memo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "S2Memo",
                Tag = 1,
                Priority = 0,
                WinLeft = 2,
                WinTop = 41,
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
                WinWidth = 260,
                WinHeight = 35,
                BorderWidth = 10,
                BorderHeight = 0,
                TextColor = 1254460,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(S2Memo);
            S2Cost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S2Cost",
                Tag = 1,
                Priority = 0,
                WinLeft = 57,
                WinTop = 28,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S2Cost);
            S2Icn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "S2Icn",
                Tag = 1,
                OnDraw = SpellIconDraw,
                Priority = 0,
                WinLeft = 10,
                WinTop = 2,
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
                AnimStart = 0,
                CurrentImage = 38,
                AnimEnd = 0,
                WinWidth = 40,
                WinHeight = 35,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(S2Icn);
            S3Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "S3Pnl",
                Tag = 2,
                Priority = 1000,
                WinLeft = 8,
                WinTop = 190,
                WinWidth = 262,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 190,
                    BottomOffset = 0
                },
                WinHeight = 80,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(S3Pnl);
            S3Spell = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S3Spell",
                Tag = 2,
                Priority = 0,
                WinLeft = 57,
                WinTop = 3,
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
                Text = "####",
                TextColor = 1254460
            };
            LayoutSession.EndRead(S3Spell);
            S3Class = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S3Class",
                Tag = 2,
                Priority = 0,
                WinLeft = 57,
                WinTop = 16,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S3Class);
            S3Memo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "S3Memo",
                Tag = 2,
                Priority = 0,
                WinLeft = 2,
                WinTop = 41,
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
                WinWidth = 260,
                WinHeight = 35,
                BorderWidth = 10,
                BorderHeight = 0,
                TextColor = 1254460,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(S3Memo);
            S3Cost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S3Cost",
                Tag = 2,
                Priority = 0,
                WinLeft = 57,
                WinTop = 28,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S3Cost);
            S3Icn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "S3Icn",
                Tag = 2,
                OnDraw = SpellIconDraw,
                Priority = 0,
                WinLeft = 10,
                WinTop = 2,
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
                AnimStart = 0,
                CurrentImage = 38,
                AnimEnd = 0,
                WinWidth = 40,
                WinHeight = 35,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(S3Icn);
            S4Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "S4Pnl",
                Tag = 3,
                Priority = 1000,
                WinLeft = 8,
                WinTop = 270,
                WinWidth = 262,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 270,
                    BottomOffset = 0
                },
                WinHeight = 80,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(S4Pnl);
            S4Spell = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S4Spell",
                Tag = 3,
                Priority = 0,
                WinLeft = 57,
                WinTop = 3,
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
                Text = "####",
                TextColor = 1254460
            };
            LayoutSession.EndRead(S4Spell);
            S4Class = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S4Class",
                Tag = 3,
                Priority = 0,
                WinLeft = 57,
                WinTop = 16,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S4Class);
            S4Memo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "S4Memo",
                Tag = 3,
                Priority = 0,
                WinLeft = 2,
                WinTop = 41,
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
                WinWidth = 260,
                WinHeight = 35,
                BorderWidth = 10,
                BorderHeight = 0,
                TextColor = 1254460,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(S4Memo);
            S4Cost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S4Cost",
                Tag = 3,
                Priority = 0,
                WinLeft = 57,
                WinTop = 28,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S4Cost);
            S4Icn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "S4Icn",
                Tag = 3,
                OnDraw = SpellIconDraw,
                Priority = 0,
                WinLeft = 10,
                WinTop = 2,
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
                AnimStart = 0,
                CurrentImage = 38,
                AnimEnd = 0,
                WinWidth = 40,
                WinHeight = 35,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(S4Icn);
            RightHeader = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "RightHeader",
                Priority = 2000,
                WinLeft = 85,
                WinTop = 9,
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
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Global Spells",
                TextColor = 1254460
            };
            LayoutSession.EndRead(RightHeader);
            RightSpellPage = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RightSpellPage",
                Visible = false,
                Priority = 1000,
                WinLeft = 304,
                WinTop = 10,
                WinWidth = 268,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 372,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RightSpellPage);
            S5Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "S5Pnl",
                Tag = 4,
                Priority = 1000,
                WinLeft = 6,
                WinTop = 30,
                WinWidth = 262,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
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
                WinHeight = 80,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(S5Pnl);
            S5Spell = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S5Spell",
                Tag = 4,
                Priority = 0,
                WinLeft = 57,
                WinTop = 3,
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
                Text = "####",
                TextColor = 1254460
            };
            LayoutSession.EndRead(S5Spell);
            S5Class = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S5Class",
                Tag = 4,
                Priority = 0,
                WinLeft = 57,
                WinTop = 16,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S5Class);
            S5Memo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "S5Memo",
                Tag = 4,
                Priority = 0,
                WinLeft = 2,
                WinTop = 41,
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
                WinWidth = 260,
                WinHeight = 35,
                BorderWidth = 10,
                BorderHeight = 0,
                TextColor = 1254460,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(S5Memo);
            S5Cost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S5Cost",
                Tag = 4,
                Priority = 0,
                WinLeft = 57,
                WinTop = 28,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S5Cost);
            S5Icn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "S5Icn",
                Tag = 4,
                OnDraw = SpellIconDraw,
                Priority = 0,
                WinLeft = 10,
                WinTop = 2,
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
                AnimStart = 0,
                CurrentImage = 38,
                AnimEnd = 0,
                WinWidth = 40,
                WinHeight = 35,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(S5Icn);
            S6Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "S6Pnl",
                Tag = 5,
                Priority = 1000,
                WinLeft = 6,
                WinTop = 110,
                WinWidth = 262,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 110,
                    BottomOffset = 0
                },
                WinHeight = 80,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(S6Pnl);
            S6Spell = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S6Spell",
                Tag = 5,
                Priority = 0,
                WinLeft = 57,
                WinTop = 3,
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
                Text = "####",
                TextColor = 1254460
            };
            LayoutSession.EndRead(S6Spell);
            S6Class = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S6Class",
                Tag = 5,
                Priority = 0,
                WinLeft = 57,
                WinTop = 16,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S6Class);
            S6Memo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "S6Memo",
                Tag = 5,
                Priority = 0,
                WinLeft = 2,
                WinTop = 41,
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
                WinWidth = 260,
                WinHeight = 35,
                BorderWidth = 10,
                BorderHeight = 0,
                TextColor = 1254460,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(S6Memo);
            S6Cost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S6Cost",
                Tag = 5,
                Priority = 0,
                WinLeft = 57,
                WinTop = 28,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S6Cost);
            S6Icn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "S6Icn",
                Tag = 5,
                OnDraw = SpellIconDraw,
                Priority = 0,
                WinLeft = 10,
                WinTop = 2,
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
                AnimStart = 0,
                CurrentImage = 38,
                AnimEnd = 0,
                WinWidth = 40,
                WinHeight = 35,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(S6Icn);
            S7Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "S7Pnl",
                Tag = 6,
                Priority = 1000,
                WinLeft = 6,
                WinTop = 190,
                WinWidth = 262,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 190,
                    BottomOffset = 0
                },
                WinHeight = 80,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(S7Pnl);
            S7Spell = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S7Spell",
                Tag = 6,
                Priority = 0,
                WinLeft = 57,
                WinTop = 3,
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
                Text = "####",
                TextColor = 1254460
            };
            LayoutSession.EndRead(S7Spell);
            S7Class = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S7Class",
                Tag = 6,
                Priority = 0,
                WinLeft = 57,
                WinTop = 16,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S7Class);
            S7Memo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "S7Memo",
                Tag = 6,
                Priority = 0,
                WinLeft = 2,
                WinTop = 41,
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
                WinWidth = 260,
                WinHeight = 35,
                BorderWidth = 10,
                BorderHeight = 0,
                TextColor = 1254460,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(S7Memo);
            S7Cost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S7Cost",
                Tag = 6,
                Priority = 0,
                WinLeft = 57,
                WinTop = 28,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S7Cost);
            S7Icn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "S7Icn",
                Tag = 6,
                OnDraw = SpellIconDraw,
                Priority = 0,
                WinLeft = 10,
                WinTop = 2,
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
                AnimStart = 0,
                CurrentImage = 38,
                AnimEnd = 0,
                WinWidth = 40,
                WinHeight = 35,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(S7Icn);
            S8Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "S8Pnl",
                Tag = 7,
                Priority = 1000,
                WinLeft = 6,
                WinTop = 270,
                WinWidth = 262,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 270,
                    BottomOffset = 0
                },
                WinHeight = 80,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(S8Pnl);
            S8Spell = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S8Spell",
                Tag = 7,
                Priority = 0,
                WinLeft = 57,
                WinTop = 3,
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
                Text = "####",
                TextColor = 1254460
            };
            LayoutSession.EndRead(S8Spell);
            S8Class = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S8Class",
                Tag = 7,
                Priority = 0,
                WinLeft = 57,
                WinTop = 16,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S8Class);
            S8Memo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "S8Memo",
                Tag = 7,
                Priority = 0,
                WinLeft = 2,
                WinTop = 41,
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
                WinWidth = 260,
                WinHeight = 35,
                BorderWidth = 10,
                BorderHeight = 0,
                TextColor = 1254460,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(S8Memo);
            S8Cost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "S8Cost",
                Tag = 7,
                Priority = 0,
                WinLeft = 57,
                WinTop = 28,
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
                Text = "####",
                TextColor = 5908276
            };
            LayoutSession.EndRead(S8Cost);
            S8Icn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "S8Icn",
                Tag = 7,
                OnDraw = SpellIconDraw,
                Priority = 0,
                WinLeft = 10,
                WinTop = 2,
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
                AnimStart = 0,
                CurrentImage = 38,
                AnimEnd = 0,
                WinWidth = 40,
                WinHeight = 35,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(S8Icn);
            S1Btn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "S1Btn",
                Priority = 2000,
                WinLeft = 0,
                WinTop = 0,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 26,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 254,
                OnClick = SelectSpellBtnClick
            };
            LayoutSession.EndRead(S1Btn);
            CurrentPointsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CurrentPointsLbl",
                Priority = 9999,
                WinLeft = 67,
                WinTop = 427,
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
                Text = "20",
                TextColor = 13948116
            };
            LayoutSession.EndRead(CurrentPointsLbl);
            Line = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Line",
                Priority = 9999,
                WinLeft = 68,
                WinTop = 418,
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
                Text = "-",
                TextColor = 12632256
            };
            LayoutSession.EndRead(Line);
            CastingPointsMaxLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CastingPointsMaxLbl",
                Priority = 9999,
                WinLeft = 64,
                WinTop = 411,
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
                Text = "20",
                TextColor = 11447982
            };
            LayoutSession.EndRead(CastingPointsMaxLbl);
            GlobalBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "GlobalBtn",
                Priority = 9999,
                WinLeft = 149,
                WinTop = 385,
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
                BorderHeight = 27,
                BorderHeightSelected = 0,
                ILIndexDown = 17,
                ILIndexLit = 18,
                ILIndexUp = 16,
                TextColor = 16777215,
                TextSelColor = 0,
                WinWidth = 39,
                OnClick = GlobalBtnClick
            };
            LayoutSession.EndRead(GlobalBtn);
            UnitBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "UnitBtn",
                Priority = 9999,
                WinLeft = 195,
                WinTop = 386,
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
                BorderHeight = 27,
                BorderHeightSelected = 0,
                ILIndexDown = 23,
                ILIndexLit = 24,
                ILIndexUp = 22,
                TextColor = 16777215,
                TextSelColor = 0,
                WinWidth = 39,
                OnClick = Layout_UnitBtnClick_2
            };
            LayoutSession.EndRead(UnitBtn);
            CombatBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CombatBtn",
                Priority = 9999,
                WinLeft = 241,
                WinTop = 386,
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
                BorderHeight = 27,
                BorderHeightSelected = 0,
                ILIndexDown = 20,
                ILIndexLit = 21,
                ILIndexUp = 19,
                TextColor = 16777215,
                TextSelColor = 0,
                WinWidth = 39,
                OnClick = Layout_CombatBtnClick_3
            };
            LayoutSession.EndRead(CombatBtn);
            InfoBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "InfoBtn",
                Priority = 9998,
                WinLeft = 51,
                WinTop = 384,
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
                BorderHeight = 27,
                BorderHeightSelected = 0,
                ILIndexDown = 14,
                ILIndexLit = 15,
                ILIndexUp = 13,
                TextColor = 16777215,
                TextSelColor = 0,
                WinWidth = 39,
                OnClick = InfoBtnClick
            };
            LayoutSession.EndRead(InfoBtn);
            PrevBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PrevBtn",
                Priority = 20000,
                WinLeft = 28,
                WinTop = 343,
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
                    BottomOffset = 82
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 2,
                ILIndexLit = 3,
                ILIndexUp = 1,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 55,
                OnClick = Layout_PrevBtnClick_4
            };
            LayoutSession.EndRead(PrevBtn);
            NextBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "NextBtn",
                Priority = 20000,
                WinLeft = 518,
                WinTop = 343,
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
                    RightOffset = 26,
                    TopOffset = 0,
                    BottomOffset = 82
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 5,
                ILIndexLit = 6,
                ILIndexUp = 4,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 55,
                OnClick = Layout_NextBtnClick_5
            };
            LayoutSession.EndRead(NextBtn);
            Cancel = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Cancel",
                Priority = 0,
                WinLeft = 520,
                WinTop = 386,
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
                    RightOffset = 40,
                    TopOffset = 0,
                    BottomOffset = 18
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 8,
                ILIndexLit = 9,
                ILIndexUp = 7,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 39,
                OnClick = Layout_CancelClick_6
            };
            LayoutSession.EndRead(Cancel);
            S2Btn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "S2Btn",
                Tag = 1,
                Priority = 2000,
                WinLeft = 0,
                WinTop = 0,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 26,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 254,
                OnClick = SelectSpellBtnClick
            };
            LayoutSession.EndRead(S2Btn);
            S3Btn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "S3Btn",
                Tag = 2,
                Priority = 2000,
                WinLeft = 0,
                WinTop = 0,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 26,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 254,
                OnClick = SelectSpellBtnClick
            };
            LayoutSession.EndRead(S3Btn);
            S4Btn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "S4Btn",
                Tag = 3,
                Priority = 2000,
                WinLeft = 0,
                WinTop = 0,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 26,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 254,
                OnClick = SelectSpellBtnClick
            };
            LayoutSession.EndRead(S4Btn);
            S5btn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "S5btn",
                Tag = 4,
                Priority = 2000,
                WinLeft = 0,
                WinTop = 0,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 26,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 254,
                OnClick = SelectSpellBtnClick
            };
            LayoutSession.EndRead(S5btn);
            S6Btn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "S6Btn",
                Tag = 5,
                Priority = 2000,
                WinLeft = 0,
                WinTop = 0,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 26,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 254,
                OnClick = SelectSpellBtnClick
            };
            LayoutSession.EndRead(S6Btn);
            S7Btn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "S7Btn",
                Tag = 6,
                Priority = 2000,
                WinLeft = 0,
                WinTop = 0,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 26,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 254,
                OnClick = SelectSpellBtnClick
            };
            LayoutSession.EndRead(S7Btn);
            S8Btn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "S8Btn",
                Tag = 7,
                Priority = 2000,
                WinLeft = 0,
                WinTop = 0,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 26,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 254,
                OnClick = SelectSpellBtnClick
            };
            LayoutSession.EndRead(S8Btn);
            CastingPage = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "CastingPage",
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 268,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 372,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(CastingPage);
            CastingPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "CastingPnl",
                Visible = false,
                Priority = 0,
                WinLeft = 3,
                WinTop = 182,
                WinWidth = 262,
                Masked = true,
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
                    BottomOffset = 30
                },
                WinHeight = 160,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(CastingPnl);
            CastingSpell = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CastingSpell",
                Priority = 0,
                WinLeft = 57,
                WinTop = 3,
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
                    LeftOffset = 57,
                    RightOffset = 0,
                    TopOffset = 3,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#########",
                TextColor = 1254460
            };
            LayoutSession.EndRead(CastingSpell);
            CastingClass = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CastingClass",
                Priority = 0,
                WinLeft = 57,
                WinTop = 16,
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
                Text = "###########",
                TextColor = 5908276
            };
            LayoutSession.EndRead(CastingClass);
            CastingMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "CastingMemo",
                Priority = 0,
                WinLeft = 2,
                WinTop = 41,
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
                WinWidth = 260,
                WinHeight = 35,
                BorderWidth = 10,
                BorderHeight = 0,
                TextColor = 1254460,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(CastingMemo);
            CastingCost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CastingCost",
                Priority = 0,
                WinLeft = 57,
                WinTop = 28,
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
                Text = "###########",
                TextColor = 5908276
            };
            LayoutSession.EndRead(CastingCost);
            CastingIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CastingIcn",
                OnDraw = Layout_CastingIcnDraw_7,
                Priority = 0,
                WinLeft = 10,
                WinTop = 2,
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
                AnimStart = 0,
                CurrentImage = 38,
                AnimEnd = 0,
                WinWidth = 40,
                WinHeight = 35,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CastingIcn);
            LeftInfoPage = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LeftInfoPage",
                Priority = 0,
                WinLeft = 29,
                WinTop = 11,
                WinWidth = 272,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 372,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LeftInfoPage);
            CastingStatus = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CastingStatus",
                Priority = 0,
                WinLeft = 86,
                WinTop = 85,
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
                    TopOffset = 85,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##########",
                TextColor = 5908276
            };
            LayoutSession.EndRead(CastingStatus);
            CastingPointsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CastingPointsLbl",
                Priority = 500,
                WinLeft = 76,
                WinTop = 55,
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
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#############",
                TextColor = 5908276
            };
            LayoutSession.EndRead(CastingPointsLbl);
            CastingBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CastingBtn",
                Priority = 10,
                WinLeft = 0,
                WinTop = 0,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 26,
                ILIndexUp = 25,
                TextColor = 1254460,
                TextSelColor = 0,
                WinWidth = 254,
                OnClick = Layout_CastingBtnClick_8
            };
            LayoutSession.EndRead(CastingBtn);
            Sel4 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel4",
                Priority = 10,
                WinLeft = 109,
                WinTop = 20,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 109,
                    RightOffset = 0,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                OnClick = Layout_Sel1Click_9
            };
            LayoutSession.EndRead(Sel4);
            SelPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SelPnl",
                Priority = 10,
                WinLeft = 0,
                WinTop = 262,
                WinWidth = 272,
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
                    TopOffset = 0,
                    BottomOffset = 40
                },
                WinHeight = 70,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SelPnl);
            Sel3 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel3",
                Priority = 10,
                WinLeft = 74,
                WinTop = 20,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 74,
                    RightOffset = 77,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                OnClick = Layout_Sel1Click_9
            };
            LayoutSession.EndRead(Sel3);
            Sel2 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel2",
                Priority = 10,
                WinLeft = 39,
                WinTop = 20,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 39,
                    RightOffset = 43,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                OnClick = Layout_Sel1Click_9
            };
            LayoutSession.EndRead(Sel2);
            Sel1 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel1",
                Priority = 10,
                WinLeft = 4,
                WinTop = 20,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
                    RightOffset = 10,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                OnClick = Layout_Sel1Click_9
            };
            LayoutSession.EndRead(Sel1);
            Sel5 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel5",
                Priority = 10,
                WinLeft = 144,
                WinTop = 20,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 75,
                    RightOffset = 76,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                OnClick = Layout_Sel1Click_9
            };
            LayoutSession.EndRead(Sel5);
            Sel6 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel6",
                Priority = 10,
                WinLeft = 179,
                WinTop = 20,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 39,
                    RightOffset = 41,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                OnClick = Layout_Sel1Click_9
            };
            LayoutSession.EndRead(Sel6);
            Sel7 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel7",
                Priority = 10,
                WinLeft = 214,
                WinTop = 20,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 9,
                    RightOffset = 6,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                OnClick = Layout_Sel1Click_9
            };
            LayoutSession.EndRead(Sel7);
            SelLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SelLbl",
                Priority = 10,
                WinLeft = 67,
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
                Text = "Spheres of Magic",
                TextColor = 1254460
            };
            LayoutSession.EndRead(SelLbl);
            SpellBookLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SpellBookLbl",
                Priority = 10,
                WinLeft = 95,
                WinTop = 25,
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
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Spell Book",
                TextColor = 1254460
            };
            LayoutSession.EndRead(SpellBookLbl);
            NameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "NameLbl",
                Priority = 10,
                WinLeft = 96,
                WinTop = 210,
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
                    TopOffset = 210,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "########",
                TextColor = 1254460
            };
            LayoutSession.EndRead(NameLbl);
            ManaPoolLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ManaPoolLbl",
                Priority = 0,
                WinLeft = 98,
                WinTop = 80,
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
                    TopOffset = 80,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "########",
                TextColor = 5908276
            };
            LayoutSession.EndRead(ManaPoolLbl);
            CastingLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CastingLbl",
                Priority = 0,
                WinLeft = 64,
                WinTop = 155,
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
                    TopOffset = 155,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Currently Casting",
                TextColor = 1254460
            };
            LayoutSession.EndRead(CastingLbl);
            FaceFrame = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "FaceFrame",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 27,
                AnimEnd = 0,
                WinWidth = 272,
                WinHeight = 372,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(FaceFrame);
            Face = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Face",
                OnDraw = FaceDraw,
                Priority = 10,
                WinLeft = 85,
                WinTop = 61,
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
                    TopOffset = 61,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 0,
                WinWidth = 103,
                WinHeight = 128,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Face);
            Dlg = new Aow.Units.AoWBookWindow.TAoWBookWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                OnUpdate = Layout_DlgUpdate_10,
                Visible = false,
                Priority = 2000,
                WinLeft = 21,
                WinTop = 8,
                WinWidth = 599,
                Masked = true,
                OnHide = Layout_DlgHide_11,
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
                BorderWidth = 30,
                BorderHeight = 14,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 5808200,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 28,
                ILIndexTexture = 0,
                DragHeight = 0,
                Draggable = false,
                Resizable = false,
                MinWidth = 599,
                MinHeight = 465,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = false,
                Modal = true,
                WinHeight = 465,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            NoneCastingLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "NoneCastingLbl",
                Priority = 0,
                WinLeft = 118,
                WinTop = 185,
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
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "None",
                TextColor = 5908276
            };
            LayoutSession.EndRead(NoneCastingLbl);
            ResearchPage = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ResearchPage",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 268,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 372,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ResearchPage);
            ResearchName = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResearchName",
                Priority = 0,
                WinLeft = 57,
                WinTop = 3,
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
                    LeftOffset = 57,
                    RightOffset = 0,
                    TopOffset = 3,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##########",
                TextColor = 1254460
            };
            LayoutSession.EndRead(ResearchName);
            ResearchClass = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResearchClass",
                Priority = 0,
                WinLeft = 57,
                WinTop = 16,
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
                Text = "#########",
                TextColor = 5908276
            };
            LayoutSession.EndRead(ResearchClass);
            ResearchMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "ResearchMemo",
                Priority = 0,
                WinLeft = 2,
                WinTop = 41,
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
                WinWidth = 260,
                WinHeight = 35,
                BorderWidth = 10,
                BorderHeight = 0,
                TextColor = 1254460,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(ResearchMemo);
            ResearchCost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResearchCost",
                Priority = 0,
                WinLeft = 57,
                WinTop = 28,
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
                Text = "##########",
                TextColor = 5908276
            };
            LayoutSession.EndRead(ResearchCost);
            ResearchIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ResearchIcn",
                OnDraw = Layout_ResearchIcnDraw_12,
                Priority = 0,
                WinLeft = 10,
                WinTop = 2,
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
                AnimStart = 0,
                CurrentImage = 38,
                AnimEnd = 0,
                WinWidth = 40,
                WinHeight = 35,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(ResearchIcn);
            ResearchStatus = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResearchStatus",
                Priority = 0,
                WinLeft = 91,
                WinTop = 85,
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
                    TopOffset = 85,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#########",
                TextColor = 5908276
            };
            LayoutSession.EndRead(ResearchStatus);
            ResearchNone = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResearchNone",
                Priority = 0,
                WinLeft = 118,
                WinTop = 185,
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
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "None",
                TextColor = 5908276
            };
            LayoutSession.EndRead(ResearchNone);
            ResearchPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ResearchPnl",
                Priority = 0,
                WinLeft = 3,
                WinTop = 182,
                WinWidth = 262,
                Masked = true,
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
                    TopOffset = 150,
                    BottomOffset = 30
                },
                WinHeight = 160,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ResearchPnl);
            ResearchLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResearchLbl",
                Priority = 0,
                WinLeft = 43,
                WinTop = 155,
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
                    TopOffset = 155,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Currently Researching:",
                TextColor = 1254460
            };
            LayoutSession.EndRead(ResearchLbl);
            RightInfoPage = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RightInfoPage",
                Priority = 0,
                WinLeft = 313,
                WinTop = 11,
                WinWidth = 268,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 372,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RightInfoPage);
            ManaInfo = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ManaInfo",
                Priority = 10,
                WinLeft = 113,
                WinTop = 25,
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
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Mana",
                TextColor = 1254460
            };
            LayoutSession.EndRead(ManaInfo);
            Sep = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Sep",
                Priority = 0,
                WinLeft = 18,
                WinTop = 100,
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
                    TopOffset = 100,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 36,
                AnimEnd = 0,
                WinWidth = 233,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Sep);
            ResearchPointsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResearchPointsLbl",
                Priority = 500,
                WinLeft = 80,
                WinTop = 55,
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
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "############",
                TextColor = 5908276
            };
            LayoutSession.EndRead(ResearchPointsLbl);
            ResearchManaPoolLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResearchManaPoolLbl",
                Priority = 0,
                WinLeft = 85,
                WinTop = 80,
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
                    TopOffset = 80,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "###########",
                TextColor = 5908276
            };
            LayoutSession.EndRead(ResearchManaPoolLbl);
            RManaInfo = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "RManaInfo",
                Priority = 10,
                WinLeft = 99,
                WinTop = 25,
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
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Research",
                TextColor = 1254460
            };
            LayoutSession.EndRead(RManaInfo);
            AOWImage1 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AOWImage1",
                Priority = 0,
                WinLeft = 18,
                WinTop = 100,
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
                    TopOffset = 100,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 36,
                AnimEnd = 0,
                WinWidth = 233,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(AOWImage1);
            ResearchBookLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResearchBookLbl",
                Visible = false,
                Priority = 10,
                WinLeft = 80,
                WinTop = 25,
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
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Research Book",
                TextColor = 1254460
            };
            LayoutSession.EndRead(ResearchBookLbl);
            CancelResearchBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelResearchBtn",
                Priority = 100,
                WinLeft = 64,
                WinTop = 296,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Removes selected resource"
                },
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
                    BottomOffset = 50
                },
                Masked = true,
                BorderHeight = 8,
                BorderHeightSelected = 8,
                ILIndexDown = 6,
                ILIndexLit = 3,
                ILIndexUp = 0,
                Text = "Cancel",
                TextColor = 1254460,
                TextSelColor = 16777215,
                WinWidth = 140,
                OnClick = Layout_CancelResearchBtnClick_13
            };
            LayoutSession.EndRead(CancelResearchBtn);
            CancelCastingBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelCastingBtn",
                Priority = 100,
                WinLeft = 61,
                WinTop = 114,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Cancels spell casting"
                },
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
                    BottomOffset = 20
                },
                Masked = true,
                BorderHeight = 8,
                BorderHeightSelected = 8,
                ILIndexDown = 6,
                ILIndexLit = 3,
                ILIndexUp = 0,
                Text = "Cancel",
                TextColor = 1254460,
                TextSelColor = 16777215,
                WinWidth = 140,
                OnClick = Layout_CancelCastingBtnClick_14
            };
            LayoutSession.EndRead(CancelCastingBtn);
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
            LeftHeader.AOWWindow = LeftSpellPage;
            LeftSpellPage.AOWWindow = Dlg;
            S1Pnl.AOWWindow = LeftSpellPage;
            S1Spell.AOWWindow = S1Pnl;
            S1Class.AOWWindow = S1Pnl;
            S1Memo.AOWWindow = S1Pnl;
            S1Cost.AOWWindow = S1Pnl;
            S1Icn.AOWWindow = S1Pnl;
            S2Pnl.AOWWindow = LeftSpellPage;
            S2Spell.AOWWindow = S2Pnl;
            S2Class.AOWWindow = S2Pnl;
            S2Memo.AOWWindow = S2Pnl;
            S2Cost.AOWWindow = S2Pnl;
            S2Icn.AOWWindow = S2Pnl;
            S3Pnl.AOWWindow = LeftSpellPage;
            S3Spell.AOWWindow = S3Pnl;
            S3Class.AOWWindow = S3Pnl;
            S3Memo.AOWWindow = S3Pnl;
            S3Cost.AOWWindow = S3Pnl;
            S3Icn.AOWWindow = S3Pnl;
            S4Pnl.AOWWindow = LeftSpellPage;
            S4Spell.AOWWindow = S4Pnl;
            S4Class.AOWWindow = S4Pnl;
            S4Memo.AOWWindow = S4Pnl;
            S4Cost.AOWWindow = S4Pnl;
            S4Icn.AOWWindow = S4Pnl;
            RightHeader.AOWWindow = RightSpellPage;
            RightSpellPage.AOWWindow = Dlg;
            S5Pnl.AOWWindow = RightSpellPage;
            S5Spell.AOWWindow = S5Pnl;
            S5Class.AOWWindow = S5Pnl;
            S5Memo.AOWWindow = S5Pnl;
            S5Cost.AOWWindow = S5Pnl;
            S5Icn.AOWWindow = S5Pnl;
            S6Pnl.AOWWindow = RightSpellPage;
            S6Spell.AOWWindow = S6Pnl;
            S6Class.AOWWindow = S6Pnl;
            S6Memo.AOWWindow = S6Pnl;
            S6Cost.AOWWindow = S6Pnl;
            S6Icn.AOWWindow = S6Pnl;
            S7Pnl.AOWWindow = RightSpellPage;
            S7Spell.AOWWindow = S7Pnl;
            S7Class.AOWWindow = S7Pnl;
            S7Memo.AOWWindow = S7Pnl;
            S7Cost.AOWWindow = S7Pnl;
            S7Icn.AOWWindow = S7Pnl;
            S8Pnl.AOWWindow = RightSpellPage;
            S8Spell.AOWWindow = S8Pnl;
            S8Class.AOWWindow = S8Pnl;
            S8Memo.AOWWindow = S8Pnl;
            S8Cost.AOWWindow = S8Pnl;
            S8Icn.AOWWindow = S8Pnl;
            S1Btn.AOWWindow = S1Pnl;
            CurrentPointsLbl.AOWWindow = Dlg;
            Line.AOWWindow = Dlg;
            CastingPointsMaxLbl.AOWWindow = Dlg;
            GlobalBtn.AOWWindow = Dlg;
            UnitBtn.AOWWindow = Dlg;
            CombatBtn.AOWWindow = Dlg;
            InfoBtn.AOWWindow = Dlg;
            PrevBtn.AOWWindow = Dlg;
            NextBtn.AOWWindow = Dlg;
            Cancel.AOWWindow = Dlg;
            S2Btn.AOWWindow = S2Pnl;
            S3Btn.AOWWindow = S3Pnl;
            S4Btn.AOWWindow = S4Pnl;
            S5btn.AOWWindow = S5Pnl;
            S6Btn.AOWWindow = S6Pnl;
            S7Btn.AOWWindow = S7Pnl;
            S8Btn.AOWWindow = S8Pnl;
            CastingPage.AOWWindow = RightInfoPage;
            CastingPnl.AOWWindow = CastingPage;
            CastingSpell.AOWWindow = CastingPnl;
            CastingClass.AOWWindow = CastingPnl;
            CastingMemo.AOWWindow = CastingPnl;
            CastingCost.AOWWindow = CastingPnl;
            CastingIcn.AOWWindow = CastingPnl;
            LeftInfoPage.AOWWindow = Dlg;
            CastingStatus.AOWWindow = CastingPnl;
            CastingPointsLbl.AOWWindow = CastingPage;
            CastingBtn.AOWWindow = CastingPnl;
            Sel4.AOWWindow = SelPnl;
            SelPnl.AOWWindow = LeftInfoPage;
            Sel3.AOWWindow = SelPnl;
            Sel2.AOWWindow = SelPnl;
            Sel1.AOWWindow = SelPnl;
            Sel5.AOWWindow = SelPnl;
            Sel6.AOWWindow = SelPnl;
            Sel7.AOWWindow = SelPnl;
            SelLbl.AOWWindow = SelPnl;
            SpellBookLbl.AOWWindow = LeftInfoPage;
            NameLbl.AOWWindow = LeftInfoPage;
            ManaPoolLbl.AOWWindow = CastingPage;
            CastingLbl.AOWWindow = CastingPage;
            FaceFrame.AOWWindow = LeftInfoPage;
            Face.AOWWindow = LeftInfoPage;
            NoneCastingLbl.AOWWindow = CastingPage;
            ResearchPage.AOWWindow = RightInfoPage;
            ResearchName.AOWWindow = ResearchPnl;
            ResearchClass.AOWWindow = ResearchPnl;
            ResearchMemo.AOWWindow = ResearchPnl;
            ResearchCost.AOWWindow = ResearchPnl;
            ResearchIcn.AOWWindow = ResearchPnl;
            ResearchStatus.AOWWindow = ResearchPnl;
            ResearchNone.AOWWindow = ResearchPage;
            ResearchPnl.AOWWindow = ResearchPage;
            ResearchLbl.AOWWindow = ResearchPage;
            RightInfoPage.AOWWindow = Dlg;
            ManaInfo.AOWWindow = CastingPage;
            Sep.AOWWindow = CastingPage;
            ResearchPointsLbl.AOWWindow = ResearchPage;
            ResearchManaPoolLbl.AOWWindow = ResearchPage;
            RManaInfo.AOWWindow = ResearchPage;
            AOWImage1.AOWWindow = ResearchPage;
            ResearchBookLbl.AOWWindow = LeftInfoPage;
            CancelResearchBtn.AOWWindow = ResearchPage;
            CancelCastingBtn.AOWWindow = CastingPnl;
            Loaded();
            LeftHeader.Loaded();
            LeftSpellPage.Loaded();
            S1Pnl.Loaded();
            S1Spell.Loaded();
            S1Class.Loaded();
            S1Memo.Loaded();
            S1Cost.Loaded();
            S1Icn.Loaded();
            S2Pnl.Loaded();
            S2Spell.Loaded();
            S2Class.Loaded();
            S2Memo.Loaded();
            S2Cost.Loaded();
            S2Icn.Loaded();
            S3Pnl.Loaded();
            S3Spell.Loaded();
            S3Class.Loaded();
            S3Memo.Loaded();
            S3Cost.Loaded();
            S3Icn.Loaded();
            S4Pnl.Loaded();
            S4Spell.Loaded();
            S4Class.Loaded();
            S4Memo.Loaded();
            S4Cost.Loaded();
            S4Icn.Loaded();
            RightHeader.Loaded();
            RightSpellPage.Loaded();
            S5Pnl.Loaded();
            S5Spell.Loaded();
            S5Class.Loaded();
            S5Memo.Loaded();
            S5Cost.Loaded();
            S5Icn.Loaded();
            S6Pnl.Loaded();
            S6Spell.Loaded();
            S6Class.Loaded();
            S6Memo.Loaded();
            S6Cost.Loaded();
            S6Icn.Loaded();
            S7Pnl.Loaded();
            S7Spell.Loaded();
            S7Class.Loaded();
            S7Memo.Loaded();
            S7Cost.Loaded();
            S7Icn.Loaded();
            S8Pnl.Loaded();
            S8Spell.Loaded();
            S8Class.Loaded();
            S8Memo.Loaded();
            S8Cost.Loaded();
            S8Icn.Loaded();
            S1Btn.Loaded();
            CurrentPointsLbl.Loaded();
            Line.Loaded();
            CastingPointsMaxLbl.Loaded();
            GlobalBtn.Loaded();
            UnitBtn.Loaded();
            CombatBtn.Loaded();
            InfoBtn.Loaded();
            PrevBtn.Loaded();
            NextBtn.Loaded();
            Cancel.Loaded();
            S2Btn.Loaded();
            S3Btn.Loaded();
            S4Btn.Loaded();
            S5btn.Loaded();
            S6Btn.Loaded();
            S7Btn.Loaded();
            S8Btn.Loaded();
            CastingPage.Loaded();
            CastingPnl.Loaded();
            CastingSpell.Loaded();
            CastingClass.Loaded();
            CastingMemo.Loaded();
            CastingCost.Loaded();
            CastingIcn.Loaded();
            LeftInfoPage.Loaded();
            CastingStatus.Loaded();
            CastingPointsLbl.Loaded();
            CastingBtn.Loaded();
            Sel4.Loaded();
            SelPnl.Loaded();
            Sel3.Loaded();
            Sel2.Loaded();
            Sel1.Loaded();
            Sel5.Loaded();
            Sel6.Loaded();
            Sel7.Loaded();
            SelLbl.Loaded();
            SpellBookLbl.Loaded();
            NameLbl.Loaded();
            ManaPoolLbl.Loaded();
            CastingLbl.Loaded();
            FaceFrame.Loaded();
            Face.Loaded();
            Dlg.Loaded();
            NoneCastingLbl.Loaded();
            ResearchPage.Loaded();
            ResearchName.Loaded();
            ResearchClass.Loaded();
            ResearchMemo.Loaded();
            ResearchCost.Loaded();
            ResearchIcn.Loaded();
            ResearchStatus.Loaded();
            ResearchNone.Loaded();
            ResearchPnl.Loaded();
            ResearchLbl.Loaded();
            RightInfoPage.Loaded();
            ManaInfo.Loaded();
            Sep.Loaded();
            ResearchPointsLbl.Loaded();
            ResearchManaPoolLbl.Loaded();
            RManaInfo.Loaded();
            AOWImage1.Loaded();
            ResearchBookLbl.Loaded();
            CancelResearchBtn.Loaded();
            CancelCastingBtn.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            LeftHeader.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            S1Spell.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            S1Class.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S1Memo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S1Cost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S1Icn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            S2Spell.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            S2Class.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S2Memo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S2Cost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S2Icn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            S3Spell.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            S3Class.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S3Memo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S3Cost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S3Icn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            S4Spell.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            S4Class.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S4Memo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S4Cost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S4Icn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            RightHeader.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            S5Spell.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            S5Class.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S5Memo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S5Cost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S5Icn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            S6Spell.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            S6Class.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S6Memo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S6Cost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S6Icn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            S7Spell.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            S7Class.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S7Memo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S7Cost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S7Icn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            S8Spell.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            S8Class.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S8Memo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S8Cost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            S8Icn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            S1Btn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            S1Btn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            CurrentPointsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.TBarNumber;
            Line.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.TBarNumber;
            CastingPointsMaxLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.TBarNumber;
            GlobalBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            GlobalBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            UnitBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            UnitBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            CombatBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            CombatBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            InfoBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            InfoBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            PrevBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            NextBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            Cancel.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            S2Btn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            S2Btn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            S3Btn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            S3Btn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            S4Btn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            S4Btn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            S5btn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            S5btn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            S6Btn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            S6Btn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            S7Btn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            S7Btn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            S8Btn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            S8Btn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            CastingSpell.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            CastingClass.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            CastingMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            CastingCost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            CastingIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            CastingStatus.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            CastingPointsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            CastingBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CastingBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            Sel4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel5.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel6.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel7.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            SelLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SelLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            SpellBookLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SpellBookLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            NameLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            NameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            ManaPoolLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ManaPoolLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            CastingLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CastingLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            FaceFrame.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            Face.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            NoneCastingLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            ResearchName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            ResearchClass.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ResearchMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ResearchCost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ResearchIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            ResearchStatus.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            ResearchNone.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            ResearchLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ResearchLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            ManaInfo.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ManaInfo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            Sep.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            ResearchPointsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            ResearchManaPoolLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ResearchManaPoolLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            RManaInfo.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RManaInfo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            AOWImage1.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            ResearchBookLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ResearchBookLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            CancelResearchBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CancelResearchBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            CancelResearchBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            CancelCastingBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CancelCastingBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            CancelCastingBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_SpellBookCreate_0(TObject p0)
        {
            SpellBookCreate();
        }

        private void Layout_SpellBookDestroy_1(TObject p0)
        {
            SpellBookDestroy();
        }

        private void Layout_UnitBtnClick_2(TObject p0)
        {
            UnitBtnClick();
        }

        private void Layout_CombatBtnClick_3(TObject p0)
        {
            CombatBtnClick();
        }

        private void Layout_PrevBtnClick_4(TObject p0)
        {
            PrevBtnClick();
        }

        private void Layout_NextBtnClick_5(TObject p0)
        {
            NextBtnClick();
        }

        private void Layout_CancelClick_6(TObject p0)
        {
            CancelClick();
        }

        private void Layout_CastingIcnDraw_7(Aow.Units.AoWComp.TAoWComponent p0, Aow.Units.GFXE.TSurfaceDesc p1)
        {
            CastingIcnDraw((Aow.Units.AoWImage.TAOWImage)p0, p1);
        }

        private void Layout_CastingBtnClick_8(TObject p0)
        {
            CastingBtnClick();
        }

        private void Layout_Sel1Click_9(TObject p0)
        {
            Sel1Click();
        }

        private void Layout_DlgUpdate_10(TObject p0)
        {
            DlgUpdate();
        }

        private void Layout_DlgHide_11(TObject p0)
        {
            DlgHide();
        }

        private void Layout_ResearchIcnDraw_12(Aow.Units.AoWComp.TAoWComponent p0, Aow.Units.GFXE.TSurfaceDesc p1)
        {
            ResearchIcnDraw((Aow.Units.AoWImage.TAOWImage)p0, p1);
        }

        private void Layout_CancelResearchBtnClick_13(TObject p0)
        {
            CancelResearchBtnClick();
        }

        private void Layout_CancelCastingBtnClick_14(TObject p0)
        {
            CancelCastingBtnClick();
        }
    }
}
