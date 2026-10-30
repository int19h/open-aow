// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWCampaignJoin
{
    public partial class TCampaignJoinDlg : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "CampaignJoinDlg";
            LayoutOnCreate = Layout_CampaignJoinDlgCreate_0;
            LayoutOnDestroy = Layout_CampaignJoinDlgDestroy_1;
            CharGenPnl = Aow.Units.AoWPanel.TAOWPanel.Create(this);
            session.BeginRead(CharGenPnl);
            CharGenPnl.Name = "CharGenPnl";
            CharGenPnl.OnMouseDown = CharGenPnlMouseDown;
            CharGenPnl.Visible = false;
            CharGenPnl.Priority = 5;
            CharGenPnl.WinLeft = 0;
            CharGenPnl.WinTop = 0;
            CharGenPnl.WinWidth = 640;
            CharGenPnl.Masked = false;
            CharGenPnl.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter;
            CharGenPnl.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter;
            CharGenPnl.Alignment.HeightPercent = (byte)(0);
            CharGenPnl.Alignment.WidthPercent = (byte)(0);
            CharGenPnl.Alignment.LeftOffPercent = 0;
            CharGenPnl.Alignment.RightOffPercent = 0;
            CharGenPnl.Alignment.TopOffPercent = 0;
            CharGenPnl.Alignment.BottomOffPercent = 0;
            CharGenPnl.Alignment.LeftOffset = 0;
            CharGenPnl.Alignment.RightOffset = 0;
            CharGenPnl.Alignment.TopOffset = 0;
            CharGenPnl.Alignment.BottomOffset = 0;
            CharGenPnl.WinHeight = 480;
            CharGenPnl.ILIndexFrame = 0;
            CharGenPnl.ILIndexTexture = 11;
            CharGenPnl.OwnerItem = 0;
            CharGenPnl.MinWidth = 640;
            CharGenPnl.MinHeight = 480;
            session.EndRead(CharGenPnl);
            LeaderTypePnl = Aow.Units.AoWPanel.TAOWPanel.Create(this);
            session.BeginRead(LeaderTypePnl);
            LeaderTypePnl.Name = "LeaderTypePnl";
            LeaderTypePnl.Visible = false;
            LeaderTypePnl.Priority = 5;
            LeaderTypePnl.WinLeft = 120;
            LeaderTypePnl.WinTop = 90;
            LeaderTypePnl.WinWidth = 400;
            LeaderTypePnl.Masked = false;
            LeaderTypePnl.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter;
            LeaderTypePnl.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter;
            LeaderTypePnl.Alignment.HeightPercent = (byte)(0);
            LeaderTypePnl.Alignment.WidthPercent = (byte)(0);
            LeaderTypePnl.Alignment.LeftOffPercent = 0;
            LeaderTypePnl.Alignment.RightOffPercent = 0;
            LeaderTypePnl.Alignment.TopOffPercent = 0;
            LeaderTypePnl.Alignment.BottomOffPercent = 0;
            LeaderTypePnl.Alignment.LeftOffset = 0;
            LeaderTypePnl.Alignment.RightOffset = 0;
            LeaderTypePnl.Alignment.TopOffset = 0;
            LeaderTypePnl.Alignment.BottomOffset = 0;
            LeaderTypePnl.WinHeight = 300;
            LeaderTypePnl.ILIndexFrame = 32;
            LeaderTypePnl.ILIndexTexture = 2;
            LeaderTypePnl.OwnerItem = 0;
            LeaderTypePnl.MinWidth = 0;
            LeaderTypePnl.MinHeight = 0;
            session.EndRead(LeaderTypePnl);
            CustomBtn = new Aow.Units.AoWButton.TAOWButton(this);
            session.BeginRead(CustomBtn);
            CustomBtn.Name = "CustomBtn";
            CustomBtn.Priority = 250;
            CustomBtn.WinLeft = 210;
            CustomBtn.WinTop = 258;
            CustomBtn.CaptureInput = true;
            CustomBtn.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
            CustomBtn.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;
            CustomBtn.Alignment.HeightPercent = (byte)(0);
            CustomBtn.Alignment.WidthPercent = (byte)(0);
            CustomBtn.Alignment.LeftOffPercent = 50;
            CustomBtn.Alignment.RightOffPercent = 32;
            CustomBtn.Alignment.TopOffPercent = 0;
            CustomBtn.Alignment.BottomOffPercent = 0;
            CustomBtn.Alignment.LeftOffset = 10;
            CustomBtn.Alignment.RightOffset = 0;
            CustomBtn.Alignment.TopOffset = 0;
            CustomBtn.Alignment.BottomOffset = 20;
            CustomBtn.Masked = true;
            CustomBtn.BorderHeight = 6;
            CustomBtn.BorderHeightSelected = 7;
            CustomBtn.ILIndexDown = 46;
            CustomBtn.ILIndexLit = 49;
            CustomBtn.ILIndexUp = 43;
            CustomBtn.Text = "Custom";
            CustomBtn.TextColor = 0;
            CustomBtn.TextSelColor = 16777215;
            CustomBtn.WinWidth = 150;
            CustomBtn.OnClick = Layout_CustomBtnClick_2;
            session.EndRead(CustomBtn);
            DefaultBtn = new Aow.Units.AoWButton.TAOWButton(this);
            session.BeginRead(DefaultBtn);
            DefaultBtn.Name = "DefaultBtn";
            DefaultBtn.Priority = 250;
            DefaultBtn.WinLeft = 40;
            DefaultBtn.WinTop = 258;
            DefaultBtn.CaptureInput = true;
            DefaultBtn.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight;
            DefaultBtn.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;
            DefaultBtn.Alignment.HeightPercent = (byte)(0);
            DefaultBtn.Alignment.WidthPercent = (byte)(0);
            DefaultBtn.Alignment.LeftOffPercent = 10;
            DefaultBtn.Alignment.RightOffPercent = 50;
            DefaultBtn.Alignment.TopOffPercent = 0;
            DefaultBtn.Alignment.BottomOffPercent = 0;
            DefaultBtn.Alignment.LeftOffset = 0;
            DefaultBtn.Alignment.RightOffset = 10;
            DefaultBtn.Alignment.TopOffset = 0;
            DefaultBtn.Alignment.BottomOffset = 20;
            DefaultBtn.Masked = true;
            DefaultBtn.BorderHeight = 6;
            DefaultBtn.BorderHeightSelected = 7;
            DefaultBtn.ILIndexDown = 46;
            DefaultBtn.ILIndexLit = 49;
            DefaultBtn.ILIndexUp = 43;
            DefaultBtn.Text = "Default";
            DefaultBtn.TextColor = 0;
            DefaultBtn.TextSelColor = 16777215;
            DefaultBtn.WinWidth = 150;
            DefaultBtn.OnClick = Layout_DefaultBtnClick_3;
            session.EndRead(DefaultBtn);
            LeaderTypeMsg = Aow.Units.AoWMemo.TAOWMemo.Create(this);
            session.BeginRead(LeaderTypeMsg);
            LeaderTypeMsg.Name = "LeaderTypeMsg";
            LeaderTypeMsg.Priority = 0;
            LeaderTypeMsg.WinLeft = 10;
            LeaderTypeMsg.WinTop = 38;
            LeaderTypeMsg.CaptureInput = false;
            LeaderTypeMsg.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            LeaderTypeMsg.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
            LeaderTypeMsg.Alignment.HeightPercent = (byte)(0);
            LeaderTypeMsg.Alignment.WidthPercent = (byte)(0);
            LeaderTypeMsg.Alignment.LeftOffPercent = 0;
            LeaderTypeMsg.Alignment.RightOffPercent = 0;
            LeaderTypeMsg.Alignment.TopOffPercent = 0;
            LeaderTypeMsg.Alignment.BottomOffPercent = 0;
            LeaderTypeMsg.Alignment.LeftOffset = 10;
            LeaderTypeMsg.Alignment.RightOffset = 10;
            LeaderTypeMsg.Alignment.TopOffset = 38;
            LeaderTypeMsg.Alignment.BottomOffset = 0;
            LeaderTypeMsg.Masked = true;
            LeaderTypeMsg.WinWidth = 380;
            LeaderTypeMsg.WinHeight = 65;
            LeaderTypeMsg.LayoutItems = new LayoutStrings()
            {
                "Do you wish to start with a default leader or custom made character?"
            };
            LeaderTypeMsg.BorderWidth = 20;
            LeaderTypeMsg.BorderHeight = 0;
            LeaderTypeMsg.TextColor = 16777215;
            LeaderTypeMsg.CursorChar = (byte)124;
            LeaderTypeMsg.BlinkDelay = (byte)(0);
            LeaderTypeMsg.Editable = false;
            LeaderTypeMsg.ILIndexFrame = 0;
            LeaderTypeMsg.ILIndexTexture = 0;
            LeaderTypeMsg.ItemHeight = 20;
            LeaderTypeMsg.ColorChangeChar = (byte)0;
            LeaderTypeMsg.TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter;
            session.EndRead(LeaderTypeMsg);
            DefaultFacePnl = Aow.Units.AoWPanel.TAOWPanel.Create(this);
            session.BeginRead(DefaultFacePnl);
            DefaultFacePnl.Name = "DefaultFacePnl";
            DefaultFacePnl.OnMouseDown = Layout_DefaultFacePnlMouseDown_4;
            DefaultFacePnl.OnDraw = DefaultFacePnlDraw;
            DefaultFacePnl.Priority = 0;
            DefaultFacePnl.WinLeft = 62;
            DefaultFacePnl.WinTop = 103;
            DefaultFacePnl.WinWidth = 103;
            DefaultFacePnl.Masked = true;
            DefaultFacePnl.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
            DefaultFacePnl.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
            DefaultFacePnl.Alignment.HeightPercent = (byte)(0);
            DefaultFacePnl.Alignment.WidthPercent = (byte)(0);
            DefaultFacePnl.Alignment.LeftOffPercent = 0;
            DefaultFacePnl.Alignment.RightOffPercent = 0;
            DefaultFacePnl.Alignment.TopOffPercent = 0;
            DefaultFacePnl.Alignment.BottomOffPercent = 0;
            DefaultFacePnl.Alignment.LeftOffset = 62;
            DefaultFacePnl.Alignment.RightOffset = 0;
            DefaultFacePnl.Alignment.TopOffset = 103;
            DefaultFacePnl.Alignment.BottomOffset = 0;
            DefaultFacePnl.WinHeight = 128;
            DefaultFacePnl.ILIndexFrame = 40;
            DefaultFacePnl.ILIndexTexture = 0;
            DefaultFacePnl.OwnerItem = 0;
            DefaultFacePnl.MinWidth = 0;
            DefaultFacePnl.MinHeight = 0;
            session.EndRead(DefaultFacePnl);
            CustomFacePnl = Aow.Units.AoWPanel.TAOWPanel.Create(this);
            session.BeginRead(CustomFacePnl);
            CustomFacePnl.Name = "CustomFacePnl";
            CustomFacePnl.Priority = 0;
            CustomFacePnl.WinLeft = 235;
            CustomFacePnl.WinTop = 103;
            CustomFacePnl.WinWidth = 103;
            CustomFacePnl.Masked = true;
            CustomFacePnl.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight;
            CustomFacePnl.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
            CustomFacePnl.Alignment.HeightPercent = (byte)(0);
            CustomFacePnl.Alignment.WidthPercent = (byte)(0);
            CustomFacePnl.Alignment.LeftOffPercent = 0;
            CustomFacePnl.Alignment.RightOffPercent = 0;
            CustomFacePnl.Alignment.TopOffPercent = 0;
            CustomFacePnl.Alignment.BottomOffPercent = 0;
            CustomFacePnl.Alignment.LeftOffset = 62;
            CustomFacePnl.Alignment.RightOffset = 62;
            CustomFacePnl.Alignment.TopOffset = 103;
            CustomFacePnl.Alignment.BottomOffset = 0;
            CustomFacePnl.WinHeight = 128;
            CustomFacePnl.ILIndexFrame = 40;
            CustomFacePnl.ILIndexTexture = 12;
            CustomFacePnl.OwnerItem = 0;
            CustomFacePnl.MinWidth = 0;
            CustomFacePnl.MinHeight = 0;
            session.EndRead(CustomFacePnl);
            ScrollTextDB = new Aow.Units.aowUserDrawBox.TaowUserDrawBox(this);
            session.BeginRead(ScrollTextDB);
            ScrollTextDB.Name = "ScrollTextDB";
            ScrollTextDB.OnDraw = ScrollTextDBDraw;
            ScrollTextDB.OnUpdate = Layout_ScrollTextDBUpdate_5;
            ScrollTextDB.Priority = 0;
            ScrollTextDB.WinLeft = 150;
            ScrollTextDB.WinTop = 380;
            ScrollTextDB.CaptureInput = true;
            ScrollTextDB.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            ScrollTextDB.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;
            ScrollTextDB.Alignment.HeightPercent = (byte)(0);
            ScrollTextDB.Alignment.WidthPercent = (byte)(0);
            ScrollTextDB.Alignment.LeftOffPercent = 0;
            ScrollTextDB.Alignment.RightOffPercent = 0;
            ScrollTextDB.Alignment.TopOffPercent = 0;
            ScrollTextDB.Alignment.BottomOffPercent = 0;
            ScrollTextDB.Alignment.LeftOffset = 150;
            ScrollTextDB.Alignment.RightOffset = 150;
            ScrollTextDB.Alignment.TopOffset = 0;
            ScrollTextDB.Alignment.BottomOffset = 0;
            ScrollTextDB.Masked = true;
            ScrollTextDB.WinWidth = 340;
            ScrollTextDB.WinHeight = 100;
            session.EndRead(ScrollTextDB);
            var created__Translator = new Aow.Layout.LayoutTIvTranslator(this);
            Translator = created__Translator;
            session.BeginRead(created__Translator);
            created__Translator.Name = "Translator";
            created__Translator.DictionaryName = "AoW";
            created__Translator.TargetsData.Add(new TranslationTarget("", "Items", 0));
            created__Translator.TargetsData.Add(new TranslationTarget("", "Text", 0));
            created__Translator.TargetsData.Add(new TranslationTarget("", "ToolTipText", 0));
            session.EndRead(created__Translator);
            session.EndRead(this);
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
