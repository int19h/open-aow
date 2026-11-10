// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.Ui.Controls;

using Aow.Game.Ui.Windows;

namespace Aow.Game.Ui.Dialogs;

public partial class General : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "General";
        LayoutOnCreate = _ => Layout_GeneralCreate_0();
        LayoutOnDestroy = _ => Layout_GeneralDestroy_1();
        Manager = new AOWWinManager(this);
        session.BeginRead(Manager);
        Manager.Name = "Manager";
        Manager.OnKeyDown = (_, ref key, shift) => ManagerKeyDown(ref key, shift);
        Manager.OnUpdate = _ => Layout_ManagerUpdate_2();
        Manager.OnDebugMessage = ManagerDebugMessage;
        Manager.Cursor = 0;
        Manager.OnChangeCursor = _ => Layout_ManagerChangeCursor_3();
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
        ToolTip = new AOWToolTip(this);
        session.BeginRead(ToolTip);
        ToolTip.Name = "ToolTip";
        ToolTip.Visible = false;
        ToolTip.Priority = 25000;
        ToolTip.WinLeft = 0;
        ToolTip.WinTop = 0;
        ToolTip.WinWidth = 200;
        ToolTip.Masked = false;
        ToolTip.Alignment.AlignWidth = AOWAlignWidth.None;
        ToolTip.Alignment.AlignHeight = AOWAlignHeight.None;
        ToolTip.Alignment.HeightPercent = 0;
        ToolTip.Alignment.WidthPercent = 0;
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
        Manager.BackgroundTexture = DialogHosts.IntGfxMod.GenericT;
        Manager.ForegroundTexture = DialogHosts.IntGfxMod.GenericT;
        ToolTip.ILFrame = DialogHosts.IntGfxMod.GenericF;
        ToolTip.ILTexture = DialogHosts.IntGfxMod.GenericT;
        ToolTip.AOWFont = DialogHosts.FontModule.Age8;
        if (session.FireCreateEvents)
            LayoutOnCreate?.Invoke(this);
    }

    private void Layout_GeneralCreate_0()
    {
        GeneralCreate();
    }

    private void Layout_GeneralDestroy_1()
    {
        GeneralDestroy();
    }

    private void Layout_ManagerUpdate_2()
    {
        ManagerUpdate();
    }

    private void Layout_ManagerChangeCursor_3()
    {
        ManagerChangeCursor();
    }
}
