// Generated from AoWWinGeneric.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.AOWWinManager;
using static Aow.Units.AOWWinManager.AOWWinManager;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWInterfaceImgLib;
using static Aow.Units.AOWInterfaceImgLib.AOWInterfaceImgLib;
using Aow.Units.AoWWinControl;
using static Aow.Units.AoWWinControl.AoWWinControl;
using Aow.Units.AoWBaseWin;
using static Aow.Units.AoWBaseWin.AoWBaseWin;

namespace Aow.Units.AoWWinGeneric
{
    // { The AoWWinGeneric unit of aowInt.dpl: TAOWGenericWindow, the base window class that publishes
//   the window manager's settings (the ancestor of TAOWWindow, TAOWToolTip and TAOWMenu). }
    // { AoWWinGeneric.TAOWGenericWindow (instance size 492): no fields of its own }
    public partial class TAOWGenericWindow : Aow.Units.AoWBaseWin.TAOWBaseWindow
    {
        // AoWWinGeneric.pas:39
        // { TAOWGenericWindow }
        public override void Destroy()
        {
            FBuffer.Free();
            base.Destroy();
        }

        // AoWWinGeneric.pas:45
        public override void SetOwner(Aow.Units.AoWComp.TAoWComponent Value)
        {
            base.SetOwner(Value);
            FWinManager = (Aow.Units.AOWWinManager.TAOWWinManager)FAOWOwner;
        }

        // AoWWinGeneric.pas:51
        public void SetCursor(int Value)
        {
            if (Value != FCursor)
            {
                FCursor = Value;
                FWindowCursor = Value;
                if (FOnChangeCursor != null)
                {
                    FOnChangeCursor(this);
                }
            }
        }

        // AoWWinGeneric.pas:62
        public void SetBuffer(bool Value)
        {
            if (Value != FDoubleBuffered)
            {
                FDrawn = false;
                FDoubleBuffered = Value;
                if (!FDoubleBuffered)
                {
                    if (FBackSurface != null)
                    {
                        FBackSurface.Free();
                        FBackSurface = null;
                    }
                }
            }
        }

        [Published]
        public Aow.Units.AOWWinManager.TAOWWinManager WinManager { get => FWinManager; set => SetManager(value); }

        [Published]
        public bool UseVMem { get => FUseVMem; set => FUseVMem = value; }

        [Published]
        public int BorderWidth { get => FBorderWidth; set => FBorderWidth = value; }

        [Published]
        public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

        [Published]
        public bool OverlapTopCorners { get => FOverlapTopCorners; set => SetOverlap(value); }

        [Published]
        public int Cursor { get => FCursor; set => SetCursor(value); }

        [Published]
        public bool DoubleBuffered { get => FDoubleBuffered; set => SetBuffer(value); }

        [Published]
        public Aow.Rtl.TNotifyEvent OnChangeCursor { get => FOnChangeCursor; set => FOnChangeCursor = value; }

        [Published]
        public int MaskColor { get => FMaskColor; set => FMaskColor = value; }

        [Published]
        public Aow.Units.AoWWinControl.TAoWOnDrawSurface OnDrawSurface { get => FOnDrawSurface; set => FOnDrawSurface = value; }

        [Published]
        public Aow.Units.AoWWinControl.TAoWOnDrawSurface OnPreDrawSurface { get => FOnPreDrawSurface; set => FOnPreDrawSurface = value; }

        [Published]
        public bool FreeSurfaceOnHide { get => FFreeSurfaceOnHide; set => FFreeSurfaceOnHide = value; }
    }

    public static partial class AoWWinGeneric
    {
    }
}
