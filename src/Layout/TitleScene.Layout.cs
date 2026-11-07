// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Layout;
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWExeForms;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AowUserWindow;
using Aow.Units.AoWWinMan;
using Aow.Units.ImageLib;

namespace Aow.Units.AoWTitleScene;

public partial class TTitleScene : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "TitleScene";
        LayoutOnCreate = Layout_TitleSceneCreate_0;
        LayoutOnDestroy = Layout_TitleSceneDestroy_1;
        TitleSceneIL = new TImageLibrary(this);
        session.BeginRead(TitleSceneIL);
        TitleSceneIL.Name = "TitleSceneIL";
        TitleSceneIL.LoadOnDemand = true;
        TitleSceneIL.FileName = "Int\\Scenes\\Title.ILB";
        session.EndRead(TitleSceneIL);
        Window = new TAoWUserWindow(this);
        session.BeginRead(Window);
        Window.Name = "Window";
        Window.OnMouseDown = WindowMouseDown;
        Window.OnMouseUp = WindowMouseUp;
        Window.OnMouseMove = WindowMouseMove;
        Window.OnUpdate = Layout_WindowUpdate_2;
        Window.Visible = false;
        Window.Priority = 0;
        Window.WinLeft = 0;
        Window.WinTop = 0;
        Window.WinWidth = 640;
        Window.Masked = false;
        Window.OnHide = Layout_WindowHide_3;
        Window.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
        Window.Alignment.AlignHeight = TAOWAlignHeight.ahBoth;
        Window.Alignment.HeightPercent = 0;
        Window.Alignment.WidthPercent = 0;
        Window.Alignment.LeftOffPercent = 0;
        Window.Alignment.RightOffPercent = 0;
        Window.Alignment.TopOffPercent = 0;
        Window.Alignment.BottomOffPercent = 0;
        Window.Alignment.LeftOffset = 0;
        Window.Alignment.RightOffset = 0;
        Window.Alignment.TopOffset = 0;
        Window.Alignment.BottomOffset = 0;
        Window.UseVMem = false;
        Window.BorderWidth = 0;
        Window.BorderHeight = 0;
        Window.OverlapTopCorners = false;
        Window.Cursor = 0;
        Window.DoubleBuffered = false;
        Window.MaskColor = 0;
        Window.FreeSurfaceOnHide = false;
        Window.ILIndexFrame = 0;
        Window.ILIndexTexture = 0;
        Window.DragHeight = 0;
        Window.Draggable = false;
        Window.Resizable = false;
        Window.MinWidth = 0;
        Window.MinHeight = 0;
        Window.MaxWidth = 0;
        Window.MaxHeight = 0;
        Window.UseManagerBorders = false;
        Window.Modal = false;
        Window.WinHeight = 480;
        Window.Dockable = false;
        Window.OnDrawSurfaceEx = WindowDrawSurfaceEx;
        session.EndRead(Window);
        LegalNotice = new TAOWMemo(this);
        session.BeginRead(LegalNotice);
        LegalNotice.Name = "LegalNotice";
        LegalNotice.Priority = 0;
        LegalNotice.WinLeft = 0;
        LegalNotice.WinTop = 268;
        LegalNotice.CaptureInput = true;
        LegalNotice.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
        LegalNotice.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
        LegalNotice.Alignment.HeightPercent = 0;
        LegalNotice.Alignment.WidthPercent = 0;
        LegalNotice.Alignment.LeftOffPercent = 0;
        LegalNotice.Alignment.RightOffPercent = 0;
        LegalNotice.Alignment.TopOffPercent = 56;
        LegalNotice.Alignment.BottomOffPercent = 0;
        LegalNotice.Alignment.LeftOffset = 0;
        LegalNotice.Alignment.RightOffset = 0;
        LegalNotice.Alignment.TopOffset = 0;
        LegalNotice.Alignment.BottomOffset = 0;
        LegalNotice.Masked = true;
        LegalNotice.WinWidth = 640;
        LegalNotice.WinHeight = 120;
        LegalNotice.LayoutItems = new LayoutStrings()
        {
            "Age of Wonders(TM)  Triumph Studios and Epic Games, Inc.",
            "All Rights Reserved",
            "Copyright (c) 1996, 1997, 1998, 1999, Triumph Studios and Epic Games, Inc.",
            "CONFIDENTIAL AND PROPRIETARY INFORMATION",
            "DO NOT DISTRIBUTE"
        };
        LegalNotice.BorderWidth = 0;
        LegalNotice.BorderHeight = 0;
        LegalNotice.TextColor = 255;
        LegalNotice.CursorChar = 124;
        LegalNotice.BlinkDelay = 0;
        LegalNotice.Editable = false;
        LegalNotice.ILIndexFrame = 0;
        LegalNotice.ILIndexTexture = 0;
        LegalNotice.ItemHeight = 13;
        LegalNotice.ColorChangeChar = 0;
        LegalNotice.TextAlign = TaowTextAlign.taCenter;
        session.EndRead(LegalNotice);
        StrLbl = new TAOWLabel(this);
        session.BeginRead(StrLbl);
        StrLbl.Name = "StrLbl";
        StrLbl.Priority = 100;
        StrLbl.WinLeft = 145;
        StrLbl.WinTop = 220;
        StrLbl.CaptureInput = true;
        StrLbl.Alignment.AlignWidth = TAOWAlignWidth.awCenter;
        StrLbl.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
        StrLbl.Alignment.HeightPercent = 0;
        StrLbl.Alignment.WidthPercent = 0;
        StrLbl.Alignment.LeftOffPercent = 0;
        StrLbl.Alignment.RightOffPercent = 0;
        StrLbl.Alignment.TopOffPercent = 46;
        StrLbl.Alignment.BottomOffPercent = 0;
        StrLbl.Alignment.LeftOffset = 0;
        StrLbl.Alignment.RightOffset = 0;
        StrLbl.Alignment.TopOffset = 0;
        StrLbl.Alignment.BottomOffset = 0;
        StrLbl.Masked = true;
        StrLbl.Text = "This special demo version of Age of Wonders has been internally marked for:";
        StrLbl.TextColor = 255;
        session.EndRead(StrLbl);
        NameLbl = new TAOWLabel(this);
        session.BeginRead(NameLbl);
        NameLbl.Name = "NameLbl";
        NameLbl.Priority = 100;
        NameLbl.WinLeft = 293;
        NameLbl.WinTop = 235;
        NameLbl.CaptureInput = true;
        NameLbl.Alignment.AlignWidth = TAOWAlignWidth.awCenter;
        NameLbl.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
        NameLbl.Alignment.HeightPercent = 0;
        NameLbl.Alignment.WidthPercent = 0;
        NameLbl.Alignment.LeftOffPercent = 0;
        NameLbl.Alignment.RightOffPercent = 0;
        NameLbl.Alignment.TopOffPercent = 49;
        NameLbl.Alignment.BottomOffPercent = 0;
        NameLbl.Alignment.LeftOffset = 130;
        NameLbl.Alignment.RightOffset = 0;
        NameLbl.Alignment.TopOffset = 0;
        NameLbl.Alignment.BottomOffset = 0;
        NameLbl.Masked = true;
        NameLbl.Text = "######";
        NameLbl.TextColor = 255;
        session.EndRead(NameLbl);
        Copyright = new TAOWLabel(this);
        session.BeginRead(Copyright);
        Copyright.Name = "Copyright";
        Copyright.Priority = 0;
        Copyright.WinLeft = 10;
        Copyright.WinTop = 458;
        Copyright.CaptureInput = true;
        Copyright.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
        Copyright.Alignment.AlignHeight = TAOWAlignHeight.ahBottom;
        Copyright.Alignment.HeightPercent = 0;
        Copyright.Alignment.WidthPercent = 0;
        Copyright.Alignment.LeftOffPercent = 0;
        Copyright.Alignment.RightOffPercent = 0;
        Copyright.Alignment.TopOffPercent = 2;
        Copyright.Alignment.BottomOffPercent = 0;
        Copyright.Alignment.LeftOffset = 10;
        Copyright.Alignment.RightOffset = 0;
        Copyright.Alignment.TopOffset = 190;
        Copyright.Alignment.BottomOffset = 10;
        Copyright.Masked = true;
        Copyright.Text = "Copyright (C) 1999 Epic MegaGames and Kolbitar Development";
        Copyright.TextColor = 16777215;
        session.EndRead(Copyright);
        Version = new TAOWLabel(this);
        session.BeginRead(Version);
        Version.Name = "Version";
        Version.Priority = 0;
        Version.WinLeft = 582;
        Version.WinTop = 464;
        Version.CaptureInput = true;
        Version.Alignment.AlignWidth = TAOWAlignWidth.awRight;
        Version.Alignment.AlignHeight = TAOWAlignHeight.ahBottom;
        Version.Alignment.HeightPercent = 0;
        Version.Alignment.WidthPercent = 0;
        Version.Alignment.LeftOffPercent = 0;
        Version.Alignment.RightOffPercent = 0;
        Version.Alignment.TopOffPercent = 0;
        Version.Alignment.BottomOffPercent = 0;
        Version.Alignment.LeftOffset = 0;
        Version.Alignment.RightOffset = 10;
        Version.Alignment.TopOffset = 0;
        Version.Alignment.BottomOffset = 3;
        Version.Masked = true;
        Version.Text = "########";
        Version.TextColor = 8421504;
        session.EndRead(Version);
        session.EndRead(this);
        LegalNotice.AOWWindow = Window;
        StrLbl.AOWWindow = Window;
        NameLbl.AOWWindow = Window;
        Copyright.AOWWindow = Window;
        Version.AOWWindow = Window;
        Loaded();
        TitleSceneIL.Loaded();
        Window.Loaded();
        LegalNotice.Loaded();
        StrLbl.Loaded();
        NameLbl.Loaded();
        Copyright.Loaded();
        Version.Loaded();
        session.Resolve(this);
        Window.WinManager = AoWWinManUnit.General.Manager;
        LegalNotice.AOWFont = AoWExeFormsUnit.FontModule.Age8;
        StrLbl.AOWFont = AoWExeFormsUnit.FontModule.Age8;
        NameLbl.AOWFont = AoWExeFormsUnit.FontModule.Age16s;
        Copyright.AOWFont = AoWExeFormsUnit.FontModule.Age10s;
        Version.AOWFont = AoWExeFormsUnit.FontModule.Age8s;
        if (session.FireCreateEvents)
            LayoutOnCreate?.Invoke(this);
    }

    private void Layout_TitleSceneCreate_0(TObject p0)
    {
        TitleSceneCreate();
    }

    private void Layout_TitleSceneDestroy_1(TObject p0)
    {
        TitleSceneDestroy();
    }

    private void Layout_WindowUpdate_2(TObject p0)
    {
        WindowUpdate();
    }

    private void Layout_WindowHide_3(TObject p0)
    {
        WindowHide();
    }
}
