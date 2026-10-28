// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWSelFactionWin
{
    public partial class TSelFactionWin
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "SelFactionWin";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            FactionPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "FactionPnl",
                OnMouseMove = FactionPnlMouseMove,
                OnKeyDown = FactionPnlKeyDown,
                OnUpdate = Layout_FactionPnlUpdate_0,
                Visible = false,
                Priority = 5,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
                Masked = false,
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
                WinHeight = 480,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 640,
                MinHeight = 480
            };
            LayoutSession.EndRead(FactionPnl);
            TopLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TopLbl",
                Priority = 1000,
                WinLeft = 256,
                WinTop = 12,
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
                    TopOffset = 12,
                    BottomOffset = 0
                },
                Masked = false,
                Text = "Choose a Faction",
                TextColor = 0
            };
            LayoutSession.EndRead(TopLbl);
            LightPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LightPnl",
                OnMouseDown = LightPnlMouseDown,
                OnMouseMove = LightPnlMouseMove,
                Priority = 5,
                WinLeft = 352,
                WinTop = 0,
                WinWidth = 288,
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
                WinHeight = 480,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LightPnl);
            LText = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "LText",
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 140,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 140,
                    BottomOffset = 80
                },
                Masked = false,
                WinWidth = 288,
                WinHeight = 260,
                LayoutItems = new LayoutStrings()
                {
                    "· Preserve harmony over the earth and its inhabitants.",
                    "",
                    "· Accept the Humans and their presence.",
                    "",
                    "· Actively oppose the Cult of Storms.",
                    "",
                    "· Avoid bloodshed and conflict when possible.",
                    ""
                },
                BorderWidth = 20,
                BorderHeight = 20,
                TextColor = 16777215,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 20,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(LText);
            LStart = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LStart",
                Visible = false,
                Priority = 0,
                WinLeft = 149,
                WinTop = 450,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 30,
                    TopOffset = 0,
                    BottomOffset = 30
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 3,
                ILIndexLit = 0,
                ILIndexUp = 0,
                Text = "Join!",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 109,
                OnClick = LStartClick
            };
            LayoutSession.EndRead(LStart);
            LBack = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LBack",
                Visible = false,
                Priority = 0,
                WinLeft = 30,
                WinTop = 450,
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 30
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 3,
                ILIndexLit = 0,
                ILIndexUp = 0,
                Text = "Back",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 109,
                OnClick = Layout_LBackClick_1
            };
            LayoutSession.EndRead(LBack);
            LSphere = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LSphere",
                Visible = false,
                Priority = 10,
                WinLeft = 271,
                WinTop = 32,
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
                    LeftOffset = 271,
                    RightOffset = 0,
                    TopOffset = 32,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 4,
                WinWidth = 100,
                WinHeight = 100,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LSphere);
            DarkPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "DarkPnl",
                OnMouseDown = DarkPnlMouseDown,
                OnMouseMove = DarkPnlMouseMove,
                Priority = 5,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 288,
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
                WinHeight = 480,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(DarkPnl);
            DStart = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DStart",
                Visible = false,
                Priority = 0,
                WinLeft = 149,
                WinTop = 450,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 30,
                    TopOffset = 0,
                    BottomOffset = 30
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 3,
                ILIndexLit = 0,
                ILIndexUp = 0,
                Text = "Join!",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 109,
                OnClick = LStartClick
            };
            LayoutSession.EndRead(DStart);
            DBack = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DBack",
                Visible = false,
                Priority = 0,
                WinLeft = 30,
                WinTop = 450,
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 30
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 3,
                ILIndexLit = 0,
                ILIndexUp = 0,
                Text = "Back",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 109,
                OnClick = Layout_DBackClick_2
            };
            LayoutSession.EndRead(DBack);
            LName = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LName",
                Visible = false,
                Priority = 0,
                WinLeft = 97,
                WinTop = 110,
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
                    TopOffset = 110,
                    BottomOffset = 0
                },
                Masked = false,
                Text = "The Keepers",
                TextColor = 0
            };
            LayoutSession.EndRead(LName);
            DText = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "DText",
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 140,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 140,
                    BottomOffset = 80
                },
                Masked = false,
                WinWidth = 288,
                WinHeight = 260,
                LayoutItems = new LayoutStrings()
                {
                    "· Rid the world of both the Humans and Keepers.",
                    "",
                    "· Rebuild the Elven Court in the Valley of Storms.",
                    "",
                    "· Resurrect the greatest Elven Lord, Inioch.",
                    "",
                    "· Restore order under their own law and power.",
                    "",
                    ""
                },
                BorderWidth = 20,
                BorderHeight = 20,
                TextColor = 16777215,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 20,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(DText);
            DName = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DName",
                Visible = false,
                Priority = 0,
                WinLeft = 97,
                WinTop = 110,
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
                    TopOffset = 110,
                    BottomOffset = 0
                },
                Masked = false,
                Text = "The Cult of Storms",
                TextColor = 0
            };
            LayoutSession.EndRead(DName);
            DSphere = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "DSphere",
                Visible = false,
                Priority = 10,
                WinLeft = 271,
                WinTop = 32,
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
                    LeftOffset = 271,
                    RightOffset = 0,
                    TopOffset = 32,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 4,
                AnimEnd = 4,
                WinWidth = 100,
                WinHeight = 100,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(DSphere);
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
            TopLbl.AOWWindow = FactionPnl;
            LightPnl.AOWWindow = FactionPnl;
            LText.AOWWindow = LightPnl;
            LStart.AOWWindow = LightPnl;
            LBack.AOWWindow = LightPnl;
            LSphere.AOWWindow = FactionPnl;
            DarkPnl.AOWWindow = FactionPnl;
            DStart.AOWWindow = DarkPnl;
            DBack.AOWWindow = DarkPnl;
            LName.AOWWindow = LightPnl;
            DText.AOWWindow = DarkPnl;
            DName.AOWWindow = DarkPnl;
            DSphere.AOWWindow = FactionPnl;
            Loaded();
            FactionPnl.Loaded();
            TopLbl.Loaded();
            LightPnl.Loaded();
            LText.Loaded();
            LStart.Loaded();
            LBack.Loaded();
            LSphere.Loaded();
            DarkPnl.Loaded();
            DStart.Loaded();
            DBack.Loaded();
            LName.Loaded();
            DText.Loaded();
            DName.Loaded();
            DSphere.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            FactionPnl.AOWWindow = Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin;
            FactionPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin;
            TopLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            LText.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            LStart.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            LStart.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            LBack.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            LBack.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            LSphere.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin;
            DStart.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            DStart.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            DBack.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            DBack.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            LName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            DText.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            DName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            DSphere.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin;
        }

        private void Layout_FactionPnlUpdate_0(TObject p0)
        {
            FactionPnlUpdate();
        }

        private void Layout_LBackClick_1(TObject p0)
        {
            LBackClick();
        }

        private void Layout_DBackClick_2(TObject p0)
        {
            DBackClick();
        }
    }
}
