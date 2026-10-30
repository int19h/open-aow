// Translated from AoWIntroScene.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AowUserWindow;
using static Aow.Units.AowUserWindow.AowUserWindow;
using Aow.Units.E2DirectSound;
using static Aow.Units.E2DirectSound.E2DirectSound;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.E2GameFlow;
using static Aow.Units.E2GameFlow.E2GameFlow;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;

namespace Aow.Units.AoWIntroScene
{
    // { TIntroScene: size $5C; $4C sound buffer, $50 buffer size, $54 DirectSound, $58 stream position. VMT 00451F68. }
    public partial class TIntroScene : Aow.Rtl.TDataModule
    {
        public TIntroScene(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AowUserWindow.TAoWUserWindow Window;
        // { +0044 }
        public Aow.Rtl.TVideoPlayer VideoPlayer;
        // { +0048 }
        public Aow.Units.E2DirectSound.IDirectSoundBuffer FSoundBuffer;
        // { $4C }
        public int FBufferSize;
        // { $50 }
        public Aow.Units.E2DirectSound.IDirectSound FDirectSound;
        // { $54 }
        public int FStreamPosition;
        // AoWIntroScene.pas:173
        public void Finish()
        {
            ReleaseSound();
            Window.Visible = false;
            Aow.Units.E2GameFlow.E2GameFlow.ShowTitleScene(Aow.Units.E2Globals.E2Globals.TitleScene, true);
        }

        // AoWIntroScene.pas:192
        public void WindowMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Finish();
        }

        // AoWIntroScene.pas:197
        public void WindowKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if ((Key == 27) || (Key == 32))
            {
                Finish();
            }
        }
    }

    public static partial class AoWIntroScene
    {
    }
}
