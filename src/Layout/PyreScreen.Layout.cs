// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;
using Aow.Layout;
using Aow.Units.AoWPanel;
using Aow.Units.AoWComp;
using Aow.Units.AoWImage;
using Aow.Units.aowUserDrawBox;

namespace Aow.Units.AOWPyreScrn
{
    public partial class TPyreScreen : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "PyreScreen";
            DeadPnl = new TAOWPanel(this);
            session.BeginRead(DeadPnl);
            DeadPnl.Name = "DeadPnl";
            DeadPnl.OnMouseDown = DeadPnlMouseDown;
            DeadPnl.OnKeyDown = DeadPnlKeyDown;
            DeadPnl.OnUpdate = Layout_DeadPnlUpdate_0;
            DeadPnl.Visible = false;
            DeadPnl.Priority = 5;
            DeadPnl.WinLeft = 0;
            DeadPnl.WinTop = 0;
            DeadPnl.WinWidth = 640;
            DeadPnl.Masked = true;
            DeadPnl.Alignment.AlignWidth = TAOWAlignWidth.awCenter;
            DeadPnl.Alignment.AlignHeight = TAOWAlignHeight.ahCenter;
            DeadPnl.Alignment.HeightPercent = 0;
            DeadPnl.Alignment.WidthPercent = 0;
            DeadPnl.Alignment.LeftOffPercent = 0;
            DeadPnl.Alignment.RightOffPercent = 0;
            DeadPnl.Alignment.TopOffPercent = 43;
            DeadPnl.Alignment.BottomOffPercent = 0;
            DeadPnl.Alignment.LeftOffset = 0;
            DeadPnl.Alignment.RightOffset = 0;
            DeadPnl.Alignment.TopOffset = 0;
            DeadPnl.Alignment.BottomOffset = 0;
            DeadPnl.WinHeight = 480;
            DeadPnl.ILIndexFrame = 2;
            DeadPnl.ILIndexTexture = 0;
            DeadPnl.OwnerItem = 0;
            DeadPnl.MinWidth = 640;
            DeadPnl.MinHeight = 480;
            session.EndRead(DeadPnl);
            PyreTop = new TAOWImage(this);
            session.BeginRead(PyreTop);
            PyreTop.Name = "PyreTop";
            PyreTop.Priority = 0;
            PyreTop.WinLeft = 0;
            PyreTop.WinTop = 0;
            PyreTop.CaptureInput = false;
            PyreTop.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
            PyreTop.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            PyreTop.Alignment.HeightPercent = 0;
            PyreTop.Alignment.WidthPercent = 0;
            PyreTop.Alignment.LeftOffPercent = 0;
            PyreTop.Alignment.RightOffPercent = 0;
            PyreTop.Alignment.TopOffPercent = 0;
            PyreTop.Alignment.BottomOffPercent = 0;
            PyreTop.Alignment.LeftOffset = 0;
            PyreTop.Alignment.RightOffset = 0;
            PyreTop.Alignment.TopOffset = 0;
            PyreTop.Alignment.BottomOffset = 0;
            PyreTop.Masked = false;
            PyreTop.AnimStart = 0;
            PyreTop.CurrentImage = 9;
            PyreTop.AnimEnd = 0;
            PyreTop.WinWidth = 640;
            PyreTop.WinHeight = 43;
            PyreTop.Animate = false;
            PyreTop.AnimFrameDelay = 0;
            session.EndRead(PyreTop);
            LeaderFaceFr = new TAOWImage(this);
            session.BeginRead(LeaderFaceFr);
            LeaderFaceFr.Name = "LeaderFaceFr";
            LeaderFaceFr.OnPreDraw = LeaderFaceFrPreDraw;
            LeaderFaceFr.Priority = 1000;
            LeaderFaceFr.WinLeft = 225;
            LeaderFaceFr.WinTop = 5;
            LeaderFaceFr.CaptureInput = true;
            LeaderFaceFr.Alignment.AlignWidth = TAOWAlignWidth.awCenter;
            LeaderFaceFr.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            LeaderFaceFr.Alignment.HeightPercent = 0;
            LeaderFaceFr.Alignment.WidthPercent = 0;
            LeaderFaceFr.Alignment.LeftOffPercent = 0;
            LeaderFaceFr.Alignment.RightOffPercent = 0;
            LeaderFaceFr.Alignment.TopOffPercent = 0;
            LeaderFaceFr.Alignment.BottomOffPercent = 0;
            LeaderFaceFr.Alignment.LeftOffset = 0;
            LeaderFaceFr.Alignment.RightOffset = 0;
            LeaderFaceFr.Alignment.TopOffset = 5;
            LeaderFaceFr.Alignment.BottomOffset = 0;
            LeaderFaceFr.Masked = true;
            LeaderFaceFr.AnimStart = 0;
            LeaderFaceFr.CurrentImage = 10;
            LeaderFaceFr.AnimEnd = 2;
            LeaderFaceFr.WinWidth = 191;
            LeaderFaceFr.WinHeight = 198;
            LeaderFaceFr.Animate = false;
            LeaderFaceFr.AnimFrameDelay = 0;
            session.EndRead(LeaderFaceFr);
            PyreAnimation = new TaowUserDrawBox(this);
            session.BeginRead(PyreAnimation);
            PyreAnimation.Name = "PyreAnimation";
            PyreAnimation.OnDraw = PyreAnimationDraw;
            PyreAnimation.OnUpdate = Layout_PyreAnimationUpdate_1;
            PyreAnimation.Priority = 10;
            PyreAnimation.WinLeft = 0;
            PyreAnimation.WinTop = 0;
            PyreAnimation.CaptureInput = true;
            PyreAnimation.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
            PyreAnimation.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            PyreAnimation.Alignment.HeightPercent = 0;
            PyreAnimation.Alignment.WidthPercent = 0;
            PyreAnimation.Alignment.LeftOffPercent = 0;
            PyreAnimation.Alignment.RightOffPercent = 0;
            PyreAnimation.Alignment.TopOffPercent = 0;
            PyreAnimation.Alignment.BottomOffPercent = 0;
            PyreAnimation.Alignment.LeftOffset = 0;
            PyreAnimation.Alignment.RightOffset = 0;
            PyreAnimation.Alignment.TopOffset = 43;
            PyreAnimation.Alignment.BottomOffset = 0;
            PyreAnimation.Masked = false;
            PyreAnimation.WinWidth = 640;
            PyreAnimation.WinHeight = 437;
            session.EndRead(PyreAnimation);
            session.EndRead(this);
            PyreTop.AOWWindow = DeadPnl;
            LeaderFaceFr.AOWWindow = DeadPnl;
            PyreAnimation.AOWWindow = DeadPnl;
            Loaded();
            DeadPnl.Loaded();
            PyreTop.Loaded();
            LeaderFaceFr.Loaded();
            PyreAnimation.Loaded();
            session.Resolve(this);
            DeadPnl.AOWWindow = E2Globals.E2Globals.TitleWindow.TitleWin;
            PyreTop.ImageLib = aowIntGfxMod.aowIntGfxMod.IntGfxMod.PyreFrames;
            LeaderFaceFr.ImageLib = aowIntGfxMod.aowIntGfxMod.IntGfxMod.PyreFrames;
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
