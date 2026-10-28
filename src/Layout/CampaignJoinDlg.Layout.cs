// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWCampaignJoin
{
    public partial class TCampaignJoinDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "CampaignJoinDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_CampaignJoinDlgCreate_0;
            LayoutOnDestroy = Layout_CampaignJoinDlgDestroy_1;
            CharGenPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "CharGenPnl",
                OnMouseDown = CharGenPnlMouseDown,
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
                ILIndexTexture = 11,
                OwnerItem = 0,
                MinWidth = 640,
                MinHeight = 480
            };
            LayoutSession.EndRead(CharGenPnl);
            LeaderTypePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LeaderTypePnl",
                Visible = false,
                Priority = 5,
                WinLeft = 120,
                WinTop = 90,
                WinWidth = 400,
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
                WinHeight = 300,
                ILIndexFrame = 32,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LeaderTypePnl);
            CustomBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CustomBtn",
                Priority = 250,
                WinLeft = 210,
                WinTop = 258,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 32,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 20
                },
                Masked = true,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Custom",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 150,
                OnClick = Layout_CustomBtnClick_2
            };
            LayoutSession.EndRead(CustomBtn);
            DefaultBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DefaultBtn",
                Priority = 250,
                WinLeft = 40,
                WinTop = 258,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 10,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 20
                },
                Masked = true,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Default",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 150,
                OnClick = Layout_DefaultBtnClick_3
            };
            LayoutSession.EndRead(DefaultBtn);
            LeaderTypeMsg = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "LeaderTypeMsg",
                Priority = 0,
                WinLeft = 10,
                WinTop = 38,
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 38,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 380,
                WinHeight = 65,
                LayoutItems = new LayoutStrings()
                {
                    "Do you wish to start with a default leader or custom made character?"
                },
                BorderWidth = 20,
                BorderHeight = 0,
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
            LayoutSession.EndRead(LeaderTypeMsg);
            DefaultFacePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "DefaultFacePnl",
                OnMouseDown = Layout_DefaultFacePnlMouseDown_4,
                OnDraw = DefaultFacePnlDraw,
                Priority = 0,
                WinLeft = 62,
                WinTop = 103,
                WinWidth = 103,
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
                    LeftOffset = 62,
                    RightOffset = 0,
                    TopOffset = 103,
                    BottomOffset = 0
                },
                WinHeight = 128,
                ILIndexFrame = 40,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(DefaultFacePnl);
            CustomFacePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "CustomFacePnl",
                Priority = 0,
                WinLeft = 235,
                WinTop = 103,
                WinWidth = 103,
                Masked = true,
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
                    LeftOffset = 62,
                    RightOffset = 62,
                    TopOffset = 103,
                    BottomOffset = 0
                },
                WinHeight = 128,
                ILIndexFrame = 40,
                ILIndexTexture = 12,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(CustomFacePnl);
            ScrollTextDB = new Aow.Units.aowUserDrawBox.TaowUserDrawBox(this)
            {
                Name = "ScrollTextDB",
                OnDraw = ScrollTextDBDraw,
                OnUpdate = Layout_ScrollTextDBUpdate_5,
                Priority = 0,
                WinLeft = 150,
                WinTop = 380,
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
                    LeftOffset = 150,
                    RightOffset = 150,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 340,
                WinHeight = 100
            };
            LayoutSession.EndRead(ScrollTextDB);
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
            LeaderTypePnl.AOWWindow = CharGenPnl;
            CustomBtn.AOWWindow = LeaderTypePnl;
            DefaultBtn.AOWWindow = LeaderTypePnl;
            LeaderTypeMsg.AOWWindow = LeaderTypePnl;
            DefaultFacePnl.AOWWindow = LeaderTypePnl;
            CustomFacePnl.AOWWindow = LeaderTypePnl;
            ScrollTextDB.AOWWindow = CharGenPnl;
            Loaded();
            CharGenPnl.Loaded();
            LeaderTypePnl.Loaded();
            CustomBtn.Loaded();
            DefaultBtn.Loaded();
            LeaderTypeMsg.Loaded();
            DefaultFacePnl.Loaded();
            CustomFacePnl.Loaded();
            ScrollTextDB.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            CharGenPnl.AOWWindow = Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin;
            CharGenPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin;
            LeaderTypePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            LeaderTypePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CustomBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CustomBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            DefaultBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DefaultBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            LeaderTypeMsg.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            DefaultFacePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            DefaultFacePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CustomFacePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            CustomFacePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_CampaignJoinDlgCreate_0(TObject p0)
        {
            CampaignJoinDlgCreate();
        }

        private void Layout_CampaignJoinDlgDestroy_1(TObject p0)
        {
            CampaignJoinDlgDestroy();
        }

        private void Layout_CustomBtnClick_2(TObject p0)
        {
            CustomBtnClick();
        }

        private void Layout_DefaultBtnClick_3(TObject p0)
        {
            DefaultBtnClick();
        }

        private void Layout_DefaultFacePnlMouseDown_4(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
        {
            DefaultFacePnlMouseDown(default(int), default(int), default(int), default(int), default(int));
        }

        private void Layout_ScrollTextDBUpdate_5(TObject p0)
        {
            ScrollTextDBUpdate();
        }
    }
}
