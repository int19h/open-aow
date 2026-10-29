// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWTitleScene
{
    public partial class TTitleScene
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "TitleScene";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_TitleSceneCreate_0;
            LayoutOnDestroy = Layout_TitleSceneDestroy_1;
            var created__TitleSceneIL = Aow.Units.ImageLib.TImageLibrary.CreateForLayout(this);
            created__TitleSceneIL.Name = "TitleSceneIL";
            created__TitleSceneIL.LoadOnDemand = true;
            created__TitleSceneIL.FileName = "Int\\Scenes\\Title.ILB";
            TitleSceneIL = created__TitleSceneIL;
            LayoutSession.EndRead(TitleSceneIL);
            Window = new Aow.Units.AowUserWindow.TAoWUserWindow(this)
            {
                Name = "Window",
                OnMouseDown = WindowMouseDown,
                OnMouseUp = WindowMouseUp,
                OnMouseMove = WindowMouseMove,
                OnUpdate = Layout_WindowUpdate_2,
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
                Masked = false,
                OnHide = Layout_WindowHide_3,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 0,
                BorderHeight = 0,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = false,
                MaskColor = 0,
                FreeSurfaceOnHide = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                DragHeight = 0,
                Draggable = false,
                Resizable = false,
                MinWidth = 0,
                MinHeight = 0,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = false,
                Modal = false,
                WinHeight = 480,
                Dockable = false,
                OnDrawSurfaceEx = WindowDrawSurfaceEx
            };
            LayoutSession.EndRead(Window);
            LegalNotice = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "LegalNotice",
                Priority = 0,
                WinLeft = 0,
                WinTop = 268,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 56,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 640,
                WinHeight = 120,
                LayoutItems = new LayoutStrings()
                {
                    "Age of Wonders(TM)  Triumph Studios and Epic Games, Inc.",
                    "All Rights Reserved",
                    "Copyright (c) 1996, 1997, 1998, 1999, Triumph Studios and Epic Games, Inc.",
                    "CONFIDENTIAL AND PROPRIETARY INFORMATION",
                    "DO NOT DISTRIBUTE"
                },
                BorderWidth = 0,
                BorderHeight = 0,
                TextColor = 255,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 13,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(LegalNotice);
            StrLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "StrLbl",
                Priority = 100,
                WinLeft = 145,
                WinTop = 220,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 46,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "This special demo version of Age of Wonders has been internally marked for:",
                TextColor = 255
            };
            LayoutSession.EndRead(StrLbl);
            NameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "NameLbl",
                Priority = 100,
                WinLeft = 293,
                WinTop = 235,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 49,
                    BottomOffPercent = 0,
                    LeftOffset = 130,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "######",
                TextColor = 255
            };
            LayoutSession.EndRead(NameLbl);
            Copyright = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Copyright",
                Priority = 0,
                WinLeft = 10,
                WinTop = 458,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 2,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 190,
                    BottomOffset = 10
                },
                Masked = true,
                Text = "Copyright (C) 1999 Epic MegaGames and Kolbitar Development",
                TextColor = 16777215
            };
            LayoutSession.EndRead(Copyright);
            Version = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Version",
                Priority = 0,
                WinLeft = 582,
                WinTop = 464,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = true,
                Text = "########",
                TextColor = 8421504
            };
            LayoutSession.EndRead(Version);
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
            Window.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            LegalNotice.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            StrLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            NameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age16s;
            Copyright.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            Version.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
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
}
