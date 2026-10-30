// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWWinMan
{
    public partial class TGeneral : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "General";
            LayoutOnCreate = Layout_GeneralCreate_0;
            LayoutOnDestroy = Layout_GeneralDestroy_1;
            Manager = new Aow.Units.AOWWinManager.TAOWWinManager(this);
            session.BeginRead(Manager);
            Manager.Name = "Manager";
            Manager.OnKeyDown = ManagerKeyDown;
            Manager.OnUpdate = Layout_ManagerUpdate_2;
            Manager.OnDebugMessage = ManagerDebugMessage;
            Manager.Cursor = 0;
            Manager.OnChangeCursor = Layout_ManagerChangeCursor_3;
            Manager.WinLeft = 0;
            Manager.WinTop = 0;
            Manager.WinWidth = 640;
            Manager.WinHeight = 480;
            Manager.ILIndexTBack = 0;
            Manager.ILIndexTFore = 1;
            Manager.OverlayPriority = 0;
            Manager.BorderLeft = 0;
            Manager.BorderRight = 0;
            Manager.BorderTop = 0;
            Manager.BorderBottom = 48;
            Manager.DrawOverlay = true;
            session.EndRead(Manager);
            ToolTip = new Aow.Units.AOWToolTip.TAOWToolTip(this);
            session.BeginRead(ToolTip);
            ToolTip.Name = "ToolTip";
            ToolTip.Visible = false;
            ToolTip.Priority = 25000;
            ToolTip.WinLeft = 0;
            ToolTip.WinTop = 0;
            ToolTip.WinWidth = 200;
            ToolTip.Masked = false;
            ToolTip.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone;
            ToolTip.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone;
            ToolTip.Alignment.HeightPercent = (byte)(0);
            ToolTip.Alignment.WidthPercent = (byte)(0);
            ToolTip.Alignment.LeftOffPercent = 0;
            ToolTip.Alignment.RightOffPercent = 0;
            ToolTip.Alignment.TopOffPercent = 0;
            ToolTip.Alignment.BottomOffPercent = 0;
            ToolTip.Alignment.LeftOffset = 0;
            ToolTip.Alignment.RightOffset = 0;
            ToolTip.Alignment.TopOffset = 0;
            ToolTip.Alignment.BottomOffset = 0;
            ToolTip.UseVMem = false;
            ToolTip.BorderWidth = 10;
            ToolTip.BorderHeight = 3;
            ToolTip.OverlapTopCorners = false;
            ToolTip.Cursor = 0;
            ToolTip.DoubleBuffered = false;
            ToolTip.MaskColor = 0;
            ToolTip.FreeSurfaceOnHide = false;
            ToolTip.ILIndexFrame = 24;
            ToolTip.ILIndexTexture = 3;
            ToolTip.TextColor = 0;
            ToolTip.ToolTipStatic = false;
            ToolTip.ToolTipDelay = 25;
            ToolTip.ShowDuration = 250;
            ToolTip.ItemHeight = 12;
            session.EndRead(ToolTip);
            session.EndRead(this);
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
