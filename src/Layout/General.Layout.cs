// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWWinMan
{
    public partial class TGeneral
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "General";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_GeneralCreate_0;
            LayoutOnDestroy = Layout_GeneralDestroy_1;
            Manager = new Aow.Units.AOWWinManager.TAOWWinManager(this)
            {
                Name = "Manager",
                OnKeyDown = ManagerKeyDown,
                OnUpdate = Layout_ManagerUpdate_2,
                OnDebugMessage = ManagerDebugMessage,
                Cursor = 0,
                OnChangeCursor = Layout_ManagerChangeCursor_3,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
                WinHeight = 480,
                ILIndexTBack = 0,
                ILIndexTFore = 1,
                OverlayPriority = 0,
                BorderLeft = 0,
                BorderRight = 0,
                BorderTop = 0,
                BorderBottom = 48,
                DrawOverlay = true
            };
            LayoutSession.EndRead(Manager);
            ToolTip = new Aow.Units.AOWToolTip.TAOWToolTip(this)
            {
                Name = "ToolTip",
                Visible = false,
                Priority = 25000,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 200,
                Masked = false,
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
                UseVMem = false,
                BorderWidth = 10,
                BorderHeight = 3,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = false,
                MaskColor = 0,
                FreeSurfaceOnHide = false,
                ILIndexFrame = 24,
                ILIndexTexture = 3,
                TextColor = 0,
                ToolTipStatic = false,
                ToolTipDelay = 25,
                ShowDuration = 250,
                ItemHeight = 12
            };
            LayoutSession.EndRead(ToolTip);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            ToolTip.WinManager = Manager;
            Loaded();
            Manager.Loaded();
            ToolTip.Loaded();
            session.Resolve(this);
            Manager.BackgroundTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Manager.ForegroundTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ToolTip.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ToolTip.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ToolTip.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_GeneralCreate_0(TObject p0)
        {
            GeneralCreate();
        }

        private void Layout_GeneralDestroy_1(TObject p0)
        {
            GeneralDestroy();
        }

        private void Layout_ManagerUpdate_2(TObject p0)
        {
            ManagerUpdate();
        }

        private void Layout_ManagerChangeCursor_3(TObject p0)
        {
            ManagerChangeCursor();
        }
    }
}
