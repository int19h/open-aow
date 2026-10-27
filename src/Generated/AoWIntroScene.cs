// Generated from AoWIntroScene.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
        // AoWIntroScene.pas:42
        // { $58 }
        public void InitializeSound()
        {
            /* TODO(T-shim) Platform or editor boundary ActiveX.CoInitialize */
            throw new System.NotImplementedException();
        }

        // AoWIntroScene.pas:77
        public void ReleaseSound()
        {
            if (FSoundBuffer != null)
            {
                FSoundBuffer.Stop();
                FSoundBuffer = null;
            }

            if (FDirectSound != null)
            {
                FDirectSound = null;
            }

            var call__1 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetSoundQuality();
            if (call__1 != 0)
            {
                Aow.Units.Sound.Sound.InitializeSoundSystem((AnsiString)"Galaxy", Aow.Units.AoWExeForms.AoWExeForms.SoundDlg.FSettings);
                var call__2 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetMusicVolume();
                Aow.Units.Sound.Sound.SetMusicVolume(call__2);
                var call__3 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetSFXVolume();
                Aow.Units.Sound.Sound.SetSFXVolume(call__3);
            }
        }

        // AoWIntroScene.pas:173
        public void Finish()
        {
            ReleaseSound();
            Window.Visible = false;
            Aow.Units.E2GameFlow.E2GameFlow.ShowTitleScene(Aow.Units.E2Globals.E2Globals.TitleScene, true);
        }

        // AoWIntroScene.pas:137
        public void Execute()
        {
            /* TODO(T-shim) Platform or editor boundary Video.Open */
            throw new System.NotImplementedException();
        }

        // AoWIntroScene.pas:93
        public void VideoPlayerStreamSound(Aow.Rtl.TStreamSoundMode Mode, object Data, int DataSize)
        {
            int Size1 = default;
            int Size2 = default;
            object Data1 = default;
            object Data2 = default;
            int HR = default;
            int Count = default;
            if (FSoundBuffer != null)
            {
                if (Mode == Aow.Rtl.TStreamSoundMode.ssmPlay)
                {
                    var call__4 = FSoundBuffer.Lock(0, DataSize, ref Data1, ref Size1, ref Data2, ref Size2, 0);
                    HR = call__4;
                    if (Aow.Rtl.Rtl.Succeeded(HR))
                    {
                        Aow.Rtl.Rtl.Move(Aow.Rtl.Rtl.BytePointer(Data).AsSpan(DataSize), Aow.Rtl.Rtl.BytePointer(Data1).AsSpan(DataSize), DataSize);
                        var call__5 = FSoundBuffer.Unlock(Data1, Size1, Data2, Size2);
                        HR = call__5;
                        if (Aow.Rtl.Rtl.Succeeded(HR))
                        {
                            var call__6 = FSoundBuffer.SetCurrentPosition(0);
                            HR = call__6;
                        }

                        if (Aow.Rtl.Rtl.Succeeded(HR))
                        {
                            var call__7 = FSoundBuffer.Play(0, 0, 1);
                            HR = call__7;
                        }

                        FStreamPosition = DataSize;
                        // { The original unlocks the same block a second time. }
                        if (Aow.Rtl.Rtl.Succeeded(HR))
                        {
                            FSoundBuffer.Unlock(Data1, Size1, Data2, Size2);
                        }
                    }
                }
                else
                {
                    if (Mode == Aow.Rtl.TStreamSoundMode.ssmStream)
                    {
                        var call__8 = FSoundBuffer.Lock(FStreamPosition % FBufferSize, DataSize, ref Data1, ref Size1, ref Data2, ref Size2, 0);
                        HR = call__8;
                        if (Aow.Rtl.Rtl.Succeeded(HR))
                        {
                            FStreamPosition += DataSize;
                            Count = DataSize;
                            if (DataSize > Size1)
                            {
                                Count = Size1;
                            }

                            Aow.Rtl.Rtl.Move(Aow.Rtl.Rtl.BytePointer(Data).AsSpan(Count), Aow.Rtl.Rtl.BytePointer(Data1).AsSpan(Count), Count);
                            DataSize -= Count;
                            if (DataSize > 0)
                            {
                                // { The wrapped block reuses Count instead of the remaining DataSize. }
                                if (Count > Size2)
                                {
                                    Count = Size2;
                                }

                                Aow.Rtl.Rtl.Move((Aow.Rtl.Rtl.BytePointer(Data) + Size1).AsSpan(Count), Aow.Rtl.Rtl.BytePointer(Data2).AsSpan(Count), Count);
                            }

                            if (Aow.Rtl.Rtl.Succeeded(HR))
                            {
                                FSoundBuffer.Unlock(Data1, Size1, Data2, Size2);
                            }
                        }
                    }
                    else
                    {
                        if (Mode == Aow.Rtl.TStreamSoundMode.ssmStop)
                        {
                            FSoundBuffer.Stop();
                        }
                    }
                }
            }
        }

        // AoWIntroScene.pas:154
        public void WindowDrawSurfaceEx(Aow.Units.GFXE.TSurface Surface, ref bool Handled)
        {
            /* TODO(T-shim) Platform or editor boundary Video.VideoLoaded */
            throw new System.NotImplementedException();
        }

        // AoWIntroScene.pas:180
        public void WindowHide()
        {
            /* TODO(T-shim) Platform or editor boundary Video.Close */
            throw new System.NotImplementedException();
        }

        // AoWIntroScene.pas:186
        public void WindowUpdate()
        {
            /* TODO(T-shim) Platform or editor boundary Video.Status */
            throw new System.NotImplementedException();
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
