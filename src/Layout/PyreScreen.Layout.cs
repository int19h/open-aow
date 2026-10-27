// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AOWPyreScrn
{
    public partial class TPyreScreen
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "PyreScreen";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            DeadPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "DeadPnl",
                OnMouseDown = DeadPnlMouseDown,
                OnKeyDown = DeadPnlKeyDown,
                OnUpdate = Layout_DeadPnlUpdate_0,
                Visible = false,
                Priority = 5,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 43,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 480,
                ILIndexFrame = 2,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 640,
                MinHeight = 480
            };
            LayoutSession.EndRead(DeadPnl);
            PyreTop = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "PyreTop",
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
                CurrentImage = 9,
                AnimEnd = 0,
                WinWidth = 640,
                WinHeight = 43,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(PyreTop);
            LeaderFaceFr = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LeaderFaceFr",
                OnPreDraw = LeaderFaceFrPreDraw,
                Priority = 1000,
                WinLeft = 225,
                WinTop = 5,
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
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 10,
                AnimEnd = 2,
                WinWidth = 191,
                WinHeight = 198,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LeaderFaceFr);
            PyreAnimation = new Aow.Units.aowUserDrawBox.TaowUserDrawBox(this)
            {
                Name = "PyreAnimation",
                OnDraw = PyreAnimationDraw,
                OnUpdate = Layout_PyreAnimationUpdate_1,
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
                    TopOffset = 43,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 640,
                WinHeight = 437
            };
            LayoutSession.EndRead(PyreAnimation);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            PyreTop.AOWWindow = DeadPnl;
            LeaderFaceFr.AOWWindow = DeadPnl;
            PyreAnimation.AOWWindow = DeadPnl;
            Loaded();
            DeadPnl.Loaded();
            PyreTop.Loaded();
            LeaderFaceFr.Loaded();
            PyreAnimation.Loaded();
            session.Resolve(this);
            DeadPnl.AOWWindow = Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin;
            PyreTop.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PyreFrames;
            LeaderFaceFr.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PyreFrames;
        }

        private void Layout_DeadPnlUpdate_0(TObject p0)
        {
            DeadPnlUpdate();
        }

        private void Layout_PyreAnimationUpdate_1(TObject p0)
        {
            PyreAnimationUpdate();
        }
    }
}
