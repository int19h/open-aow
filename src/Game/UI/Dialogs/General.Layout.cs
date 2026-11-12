// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;

namespace Aow.Game.UI.Dialogs;

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
        Manager = new WindowManager(this);
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
        Manager.ImageLibraryIndexTBack = 0;
        Manager.ImageLibraryIndexTFore = 1;
        Manager.OverlayPriority = 0;
        Manager.BorderLeft = 0;
        Manager.BorderRight = 0;
        Manager.BorderTop = 0;
        Manager.BorderBottom = 48;
        Manager.DrawOverlay = true;
        session.EndRead(Manager);
        ToolTip = new ToolTip(this);
        session.BeginRead(ToolTip);
        ToolTip.Name = "ToolTip";
        ToolTip.Visible = false;
        ToolTip.Priority = 25000;
        ToolTip.WinLeft = 0;
        ToolTip.WinTop = 0;
        ToolTip.WinWidth = 200;
        ToolTip.Masked = false;
        ToolTip.Alignment.AlignWidth = AlignWidth.None;
        ToolTip.Alignment.AlignHeight = AlignHeight.None;
        ToolTip.Alignment.HeightPercent = 0;
        ToolTip.Alignment.WidthPercent = 0;
        ToolTip.Alignment.LeftOffsetPercent = 0;
        ToolTip.Alignment.RightOffsetPercent = 0;
        ToolTip.Alignment.TopOffsetPercent = 0;
        ToolTip.Alignment.BottomOffsetPercent = 0;
        ToolTip.Alignment.LeftOffset = 0;
        ToolTip.Alignment.RightOffset = 0;
        ToolTip.Alignment.TopOffset = 0;
        ToolTip.Alignment.BottomOffset = 0;
        ToolTip.UseVideoMemory = false;
        ToolTip.BorderWidth = 10;
        ToolTip.BorderHeight = 3;
        ToolTip.OverlapTopCorners = false;
        ToolTip.Cursor = 0;
        ToolTip.DoubleBuffered = false;
        ToolTip.MaskColor = 0;
        ToolTip.FreeSurfaceOnHide = false;
        ToolTip.ImageLibraryIndexFrame = 24;
        ToolTip.ImageLibraryIndexTexture = 3;
        ToolTip.TextColor = 0;
        ToolTip.ToolTipStatic = false;
        ToolTip.ToolTipDelay = 25;
        ToolTip.ShowDuration = 250;
        ToolTip.ItemHeight = 12;
        session.EndRead(ToolTip);
        session.EndRead(this);
        ToolTip.WindowManager = Manager;
        Loaded();
        Manager.Loaded();
        ToolTip.Loaded();
        session.Resolve(this);
        Manager.BackgroundTexture = DialogHosts.UIGraphics.GenericTextures;
        Manager.ForegroundTexture = DialogHosts.UIGraphics.GenericTextures;
        ToolTip.ImageLibraryFrame = DialogHosts.UIGraphics.GenericFrames;
        ToolTip.ImageLibraryTexture = DialogHosts.UIGraphics.GenericTextures;
        ToolTip.Font = DialogHosts.UIFonts.Age8;
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
