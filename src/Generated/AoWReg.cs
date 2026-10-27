// Generated from AoWReg.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
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

namespace Aow.Units.AoWReg
{
    // { ot=1 }
    // { TAoWRegistry: instance size $18, inherited TRegistry fields $00..$17;
//     no additional fields. VMT 55703984. }
    public partial class TAoWRegistry : Aow.Rtl.TRegistry
    {
        // AoWReg.pas:128
        public void AssociateAoWMap(Aow.Rtl.AnsiString Executable)
        {
            /* TODO(T-shim) Platform or editor boundary Registry.RootKey */
            throw new System.NotImplementedException();
        }

        // AoWReg.pas:141
        public void rwInteger(Aow.Rtl.AnsiString Key, Aow.Rtl.AnsiString Name, ref int Value, Aow.Units.AoWReg.TReadWriteMode Mode)
        {
            if (OpenKey(((AnsiString)"\\Software\\Triumph Studios\\Age of Wonders\\") + Key, true))
            {
                switch (Mode)
                {
                    case Aow.Units.AoWReg.TReadWriteMode.rwRead:
                        if (ValueExists(Name))
                        {
                            Value = ReadInteger(Name);
                        }

                        break;
                    case Aow.Units.AoWReg.TReadWriteMode.rwWrite:
                        WriteInteger(Name, Value);
                        break;
                }

                CloseKey();
            }
        }

        // AoWReg.pas:153
        public void rwBool(Aow.Rtl.AnsiString Key, Aow.Rtl.AnsiString Name, ref bool Value, Aow.Units.AoWReg.TReadWriteMode Mode)
        {
            if (OpenKey(((AnsiString)"\\Software\\Triumph Studios\\Age of Wonders\\") + Key, true))
            {
                switch (Mode)
                {
                    case Aow.Units.AoWReg.TReadWriteMode.rwRead:
                        if (ValueExists(Name))
                        {
                            Value = ReadBool(Name);
                        }

                        break;
                    case Aow.Units.AoWReg.TReadWriteMode.rwWrite:
                        WriteBool(Name, Value);
                        break;
                }

                CloseKey();
            }
        }

        // AoWReg.pas:165
        public void rwString(Aow.Rtl.AnsiString Key, Aow.Rtl.AnsiString Name, ref Aow.Rtl.AnsiString Value, Aow.Units.AoWReg.TReadWriteMode Mode)
        {
            if (OpenKey(((AnsiString)"\\Software\\Triumph Studios\\Age of Wonders\\") + Key, true))
            {
                switch (Mode)
                {
                    case Aow.Units.AoWReg.TReadWriteMode.rwRead:
                        if (ValueExists(Name))
                        {
                            Value = ReadString(Name);
                        }

                        break;
                    case Aow.Units.AoWReg.TReadWriteMode.rwWrite:
                        WriteString(Name, Value);
                        break;
                }

                CloseKey();
            }
        }

        // AoWReg.pas:177
        public void rwBinaryData(Aow.Rtl.AnsiString Key, Aow.Rtl.AnsiString Name, Span<byte> Value, int Size, Aow.Units.AoWReg.TReadWriteMode Mode)
        {
            if (OpenKey(((AnsiString)"\\Software\\Triumph Studios\\Age of Wonders\\") + Key, true))
            {
                switch (Mode)
                {
                    case Aow.Units.AoWReg.TReadWriteMode.rwRead:
                        if (ValueExists(Name))
                        {
                            ReadBinaryData(Name, Value, Size);
                        }

                        break;
                    case Aow.Units.AoWReg.TReadWriteMode.rwWrite:
                        WriteBinaryData(Name, Value, Size);
                        break;
                }

                CloseKey();
            }
        }

        // AoWReg.pas:227
        public void rwRect(Aow.Rtl.AnsiString Key, Aow.Rtl.AnsiString Name, ref Aow.Rtl.TRect Value, Aow.Units.AoWReg.TReadWriteMode Mode)
        {
            Aow.Rtl.AnsiString S = default;
            if (OpenKey(((AnsiString)"\\Software\\Triumph Studios\\Age of Wonders\\") + Key, true))
            {
                switch (Mode)
                {
                    case Aow.Units.AoWReg.TReadWriteMode.rwRead:
                        if (ValueExists(Name))
                        {
                            S = ReadString(Name);
                            Value = Aow.Units.AoWReg.AoWReg.StringToRect(S);
                        }

                        break;
                    case Aow.Units.AoWReg.TReadWriteMode.rwWrite:
                        S = Aow.Units.AoWReg.AoWReg.RectToString(Value);
                        WriteString(Name, S);
                        break;
                }

                CloseKey();
            }
        }

        // AoWReg.pas:250
        public Aow.Rtl.TRect GetRectangle(int Index)
        {
            Aow.Rtl.TRect Result = default;
            if (Index >= 0)
            {
                Result = default;
                rwRect((AnsiString)"General\\rectangles", Aow.Rtl.Rtl.IntToStr(Index), ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            }
            else
            {
                Result = default;
            }

            return Result;
        }

        // AoWReg.pas:260
        public void SetRectangle(int Index, Aow.Rtl.TRect Value)
        {
            if (Index >= 0)
            {
                rwRect((AnsiString)"General\\rectangles", Aow.Rtl.Rtl.IntToStr(Index), ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
            }
        }

        // AoWReg.pas:266
        public bool GetBoolean(int Index)
        {
            bool Result = default;
            // { Original leaves Result uninitialized for a missing nonnegative index. }
            if (Index >= 0)
            {
                rwBool((AnsiString)"General\\booleans", Aow.Rtl.Rtl.IntToStr(Index), ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // AoWReg.pas:274
        public void SetBoolean(int Index, bool Value)
        {
            if (Index >= 0)
            {
                rwBool((AnsiString)"General\\booleans", Aow.Rtl.Rtl.IntToStr(Index), ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
            }
        }

        // AoWReg.pas:280
        public Aow.Rtl.AnsiString GetString(int Index)
        {
            Aow.Rtl.AnsiString Result = default;
            if (Index >= 0)
            {
                rwString((AnsiString)"General\\strings", Aow.Rtl.Rtl.IntToStr(Index), ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            }
            else
            {
                Result = (AnsiString)"";
            }

            return Result;
        }

        // AoWReg.pas:287
        public void SetString(int Index, Aow.Rtl.AnsiString Value)
        {
            if (Index >= 0)
            {
                rwString((AnsiString)"General\\strings", Aow.Rtl.Rtl.IntToStr(Index), ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
            }
        }

        // AoWReg.pas:293
        public int GetInteger(int Index)
        {
            int Result = default;
            // { Original leaves Result uninitialized for a missing nonnegative index. }
            if (Index >= 0)
            {
                rwInteger((AnsiString)"General\\integers", Aow.Rtl.Rtl.IntToStr(Index), ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // AoWReg.pas:301
        public void SetInteger(int Index, int Value)
        {
            if (Index >= 0)
            {
                rwInteger((AnsiString)"General\\integers", Aow.Rtl.Rtl.IntToStr(Index), ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
            }
        }

        // AoWReg.pas:307
        public bool GetDeveloper()
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // AoWReg.pas:312
        public void SetDeveloper(bool Value)
        {
            /* TODO(T-shim) Platform or editor boundary Registry.DeleteValue */
            throw new System.NotImplementedException();
        }

        // AoWReg.pas:329
        public bool GetSkipIntro()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"General", (AnsiString)"Skip Intro", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:335
        public void SetSkipIntro(bool Value)
        {
            rwBool((AnsiString)"General", (AnsiString)"Skip Intro", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:340
        public int GetDisplayMode()
        {
            int Result = default;
            Result = 1;
            rwInteger((AnsiString)"Video", (AnsiString)"Display Mode", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:346
        public void SetDisplayMode(int Value)
        {
            rwInteger((AnsiString)"Video", (AnsiString)"Display Mode", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:351
        public int GetDisplayWidth()
        {
            int Result = default;
            Result = 640;
            rwInteger((AnsiString)"Video", (AnsiString)"Display Width", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:357
        public void SetDisplayWidth(int Value)
        {
            rwInteger((AnsiString)"Video", (AnsiString)"Display Width", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:362
        public int GetDisplayHeight()
        {
            int Result = default;
            Result = 480;
            rwInteger((AnsiString)"Video", (AnsiString)"Display Height", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:368
        public void SetDisplayHeight(int Value)
        {
            rwInteger((AnsiString)"Video", (AnsiString)"Display Height", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:373
        public bool GetSurfaceInVMem()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"Video", (AnsiString)"Surface In VMem", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:379
        public void SetSurfaceInVMem(bool Value)
        {
            rwBool((AnsiString)"Video", (AnsiString)"Surface In VMem", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:384
        public bool GetAllowResolutionSwitches()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"Video", (AnsiString)"Allow Resolution Switches", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:390
        public void SetAllowResolutionSwitches(bool Value)
        {
            rwBool((AnsiString)"Video", (AnsiString)"Allow Resolution Switches", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:395
        public bool GetDebugMode()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"General", (AnsiString)"Debug Mode", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:401
        public void SetDebugMode(bool Value)
        {
            rwBool((AnsiString)"General", (AnsiString)"Debug Mode", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:406
        public Aow.Rtl.AnsiString GetRootDirectory()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"General", (AnsiString)"Root Directory", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:412
        public void SetRootDirectory(Aow.Rtl.AnsiString Value)
        {
            rwString((AnsiString)"General", (AnsiString)"Root Directory", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:428
        public void SetLanguage(Aow.Units.AoWReg.TAoWLanguage Value)
        {
            int I = default;
            I = (int)Value;
            rwInteger((AnsiString)"General", (AnsiString)"Language", ref I, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:436
        public int GetSoundQuality()
        {
            int Result = default;
            Result = 3;
            rwInteger((AnsiString)"Sound", (AnsiString)"Sound Quality", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:442
        public void SetSoundQuality(int Value)
        {
            rwInteger((AnsiString)"Sound", (AnsiString)"Sound Quality", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:447
        public int GetMusicVolume()
        {
            int Result = default;
            Result = 50;
            rwInteger((AnsiString)"Sound", (AnsiString)"Music Volume", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:453
        public void SetMusicVolume(int Value)
        {
            rwInteger((AnsiString)"Sound", (AnsiString)"Music Volume", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:458
        public int GetSFXVolume()
        {
            int Result = default;
            Result = 50;
            rwInteger((AnsiString)"Sound", (AnsiString)"SFX Volume", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:464
        public void SetSFXVolume(int Value)
        {
            rwInteger((AnsiString)"Sound", (AnsiString)"SFX Volume", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:469
        public Aow.Rtl.TRect GetMainWindowPosition()
        {
            Aow.Rtl.TRect Result = default;
            Result = default;
            rwRect((AnsiString)"Workspace", (AnsiString)"Main Window Position", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:475
        public void SetMainWindowPosition(Aow.Rtl.TRect Value)
        {
            rwRect((AnsiString)"Workspace", (AnsiString)"Main Window Position", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:480
        public Aow.Rtl.TWindowState GetMainWindowState()
        {
            Aow.Rtl.TWindowState Result = default;
            int Value = default;
            Value = 0;
            rwInteger((AnsiString)"Workspace", (AnsiString)"Main Window State", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            Result = (Aow.Rtl.TWindowState)Value;
            return Result;
        }

        // AoWReg.pas:489
        public void SetMainWindowState(Aow.Rtl.TWindowState Value)
        {
            int I = default;
            I = (int)Value;
            rwInteger((AnsiString)"Workspace", (AnsiString)"Main Window State", ref I, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:497
        public Aow.Rtl.TRect GetMapWindowPosition()
        {
            Aow.Rtl.TRect Result = default;
            Result = default;
            rwRect((AnsiString)"Workspace", (AnsiString)"Map Window Position", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:503
        public void SetMapWindowPosition(Aow.Rtl.TRect Value)
        {
            rwRect((AnsiString)"Workspace", (AnsiString)"Map Window Position", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:508
        public bool GetShadowsEnabled()
        {
            bool Result = default;
            Result = true;
            rwBool((AnsiString)"Video", (AnsiString)"Shadows Enabled", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:514
        public void SetShadowsEnabled(bool Value)
        {
            rwBool((AnsiString)"Video", (AnsiString)"Shadows Enabled", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:519
        public bool GetSFXEnabled()
        {
            bool Result = default;
            Result = true;
            rwBool((AnsiString)"Sound", (AnsiString)"Sound Effects Enabled", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:525
        public void SetSFXEnabled(bool Value)
        {
            rwBool((AnsiString)"Sound", (AnsiString)"Sound Effects Enabled", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:530
        public bool GetMusicEnabled()
        {
            bool Result = default;
            Result = true;
            rwBool((AnsiString)"Sound", (AnsiString)"Music Enabled", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:536
        public void SetMusicEnabled(bool Value)
        {
            rwBool((AnsiString)"Sound", (AnsiString)"Music Enabled", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:541
        public bool GetUseDirectSound()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"Sound", (AnsiString)"Use DirectSound", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:547
        public void SetUseDirectSound(bool Value)
        {
            rwBool((AnsiString)"Sound", (AnsiString)"Use DirectSound", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:552
        public Aow.Units.AoWReg.TDetailLevel GetDetailLevel()
        {
            Aow.Units.AoWReg.TDetailLevel Result = default;
            int Value = default;
            Value = 2;
            rwInteger((AnsiString)"Workspace", (AnsiString)"Detail Level", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            Result = (Aow.Units.AoWReg.TDetailLevel)Value;
            return Result;
        }

        // AoWReg.pas:561
        public void SetDetailLevel(Aow.Units.AoWReg.TDetailLevel Value)
        {
            int I = default;
            I = (int)Value;
            rwInteger((AnsiString)"Workspace", (AnsiString)"Detail Level", ref I, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:569
        public Aow.Rtl.AnsiString GetStartupDirectory()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"General", (AnsiString)"Startup Directory", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:575
        public void SetStartupDirectory(Aow.Rtl.AnsiString Value)
        {
            rwString((AnsiString)"General", (AnsiString)"Startup Directory", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:580
        public bool GetLogTokenEvents()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"General", (AnsiString)"Log Token Events", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:586
        public void SetLogTokenEvents(bool Value)
        {
            rwBool((AnsiString)"General", (AnsiString)"Log Token Events", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:591
        public bool GetAutoSave()
        {
            bool Result = default;
            Result = true;
            rwBool((AnsiString)"General", (AnsiString)"Auto Save", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:597
        public void SetAutoSave(bool Value)
        {
            rwBool((AnsiString)"General", (AnsiString)"Auto Save", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:602
        public bool GetAutoDebugSave()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"General", (AnsiString)"Auto Debug Save", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:608
        public void SetAutoDebugSave(bool Value)
        {
            rwBool((AnsiString)"General", (AnsiString)"Auto Debug Save", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:613
        public bool GetFastMovementEnabled()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"General", (AnsiString)"Fast Movement Enabled", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:619
        public void SetFastMovementEnabled(bool Value)
        {
            rwBool((AnsiString)"General", (AnsiString)"Fast Movement Enabled", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:630
        public void SetClassicWindowPopupEvents(TSet<Aow.Units.AoWReg.TWindowPopupEvent> Value)
        {
            /* TODO(T-5.8) No physical byte image for TWindowPopupEvents */
            throw new System.NotImplementedException();
        }

        // AoWReg.pas:635
        public TSet<Aow.Units.AoWReg.TWindowPopupEvent> GetSimultaneousWindowPopupEvents()
        {
            TSet<Aow.Units.AoWReg.TWindowPopupEvent> Result = default;
            Result = TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpCombat, Aow.Units.AoWReg.TWindowPopupEvent.wpDiplomacy, Aow.Units.AoWReg.TWindowPopupEvent.wpSpellResearched, Aow.Units.AoWReg.TWindowPopupEvent.wpSpellReady, Aow.Units.AoWReg.TWindowPopupEvent.wpPartyDamage, Aow.Units.AoWReg.TWindowPopupEvent.wpHeroUpgrade, Aow.Units.AoWReg.TWindowPopupEvent.wpHeroJoin);
            /* TODO(T-5.8) No physical byte image for TWindowPopupEvents */
            throw new System.NotImplementedException();
            return Result;
        }

        // AoWReg.pas:641
        public void SetSimultaneousWindowPopupEvents(TSet<Aow.Units.AoWReg.TWindowPopupEvent> Value)
        {
            /* TODO(T-5.8) No physical byte image for TWindowPopupEvents */
            throw new System.NotImplementedException();
        }

        // AoWReg.pas:646
        public Aow.Units.AoWReg.TCombatResolveMode GetCombatResolveMode()
        {
            Aow.Units.AoWReg.TCombatResolveMode Result = default;
            Result = Aow.Units.AoWReg.TCombatResolveMode.crmChoose;
            rwBinaryData((AnsiString)"General", (AnsiString)"Combat Resolve Mode", Aow.Rtl.Bytes.Of(ref Result), 1, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:652
        public void SetCombatResolveMode(Aow.Units.AoWReg.TCombatResolveMode Value)
        {
            rwBinaryData((AnsiString)"General", (AnsiString)"Combat Resolve Mode", Aow.Rtl.Bytes.Of(ref Value), 1, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:657
        public Aow.Units.AoWReg.TMovementSpeed GetMovementSpeed()
        {
            Aow.Units.AoWReg.TMovementSpeed Result = default;
            Result = Aow.Units.AoWReg.TMovementSpeed.mmsNormal;
            rwBinaryData((AnsiString)"General", (AnsiString)"Movement Speed", Aow.Rtl.Bytes.Of(ref Result), 1, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:663
        public void SetMovementSpeed(Aow.Units.AoWReg.TMovementSpeed Value)
        {
            rwBinaryData((AnsiString)"General", (AnsiString)"Movement Speed", Aow.Rtl.Bytes.Of(ref Value), 1, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:668
        public Aow.Rtl.AnsiString GetLocalEmailAddress()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"Email", (AnsiString)"Local Email Address", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:674
        public void SetLocalEmailAddress(Aow.Rtl.AnsiString Value)
        {
            rwString((AnsiString)"Email", (AnsiString)"Local Email address", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:679
        public Aow.Rtl.AnsiString GetSMTPServer()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"Email", (AnsiString)"SMTP Server", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:685
        public void SetSMTPServer(Aow.Rtl.AnsiString Value)
        {
            rwString((AnsiString)"Email", (AnsiString)"SMTP Server", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:690
        public Aow.Rtl.AnsiString GetPOP3Server()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"Email", (AnsiString)"POP3 Server", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:696
        public void SetPOP3Server(Aow.Rtl.AnsiString Value)
        {
            rwString((AnsiString)"Email", (AnsiString)"POP3 Server", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:710
        public Aow.Rtl.AnsiString GetPOP3UserName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"Email", (AnsiString)"POP3 UserName", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:716
        public void SetPOP3UserName(Aow.Rtl.AnsiString Value)
        {
            rwString((AnsiString)"Email", (AnsiString)"POP3 UserName", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:721
        public Aow.Rtl.AnsiString GetPOP3Password()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"Email", (AnsiString)"POP3 Password", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            Result = Aow.Units.AoWReg.AoWReg.TransformPassword(Result, true);
            return Result;
        }

        // AoWReg.pas:728
        public void SetPOP3Password(Aow.Rtl.AnsiString Value)
        {
            Value = Aow.Units.AoWReg.AoWReg.TransformPassword(Value, true);
            rwString((AnsiString)"Email", (AnsiString)"POP3 Password", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:734
        public Aow.Rtl.AnsiString GetAttachmentDirectory()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"Email", (AnsiString)"Attachment Directory", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:740
        public void SetAttachmentDirectory(Aow.Rtl.AnsiString Value)
        {
            if ((Value.Length > 0) && (Value[Value.Length] != 92))
            {
                Value = Value + AnsiString.FromBytes(92);
            }

            rwString((AnsiString)"Email", (AnsiString)"Attachment Directory", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:747
        public int GetCheckEmailFrequency()
        {
            int Result = default;
            Result = 1;
            rwInteger((AnsiString)"Email", (AnsiString)"Check Email Frequency", ref Result, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:753
        public void SetCheckEmailFrequency(int Value)
        {
            rwInteger((AnsiString)"Email", (AnsiString)"Check Email Frequency", ref Value, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:758
        public int GetEmailGamesCRC()
        {
            int Result = default;
            Aow.Rtl.AnsiString S = default;
            int P = default;
            Result = 0;
            rwString((AnsiString)"General", (AnsiString)"Serial number", ref S, Aow.Units.AoWReg.TReadWriteMode.rwRead);
            try
            {
                P = Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(45), S);
                // { Original tests against -1 although Pos returns 0 when absent. }
                if (P != -1)
                {
                    S = Aow.Rtl.Rtl.Copy(S, P + 1, S.Length - P);
                    P = Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(45), S);
                    if (P != -1)
                    {
                        S = Aow.Rtl.Rtl.Copy(S, 1, P - 1);
                        if (S != ((AnsiString)""))
                        {
                            Result = Aow.Units.Engine.Engine.HexToInt(S);
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                Result = 0;
            }

            return Result;
        }

        // AoWReg.pas:783
        public void SetEmailGamesCRC(int Value)
        {
            Aow.Rtl.AnsiString S = default;
            S = (((AnsiString)"4G5F6JHH-") + Aow.Rtl.Rtl.IntToHex(Value, 8)) + ((AnsiString)"-6AG66554-98LK0FG3");
            rwString((AnsiString)"General", (AnsiString)"Serial number", ref S, Aow.Units.AoWReg.TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:791
        public void RegisterDPlayLobbyServer()
        {
            /* TODO(T-shim) Platform or editor boundary Registry.RootKey */
            throw new System.NotImplementedException();
        }

        // Registry.pas:54
        public static Aow.Units.AoWReg.TAoWRegistry Create()
        {
            return new Aow.Units.AoWReg.TAoWRegistry();
        }
    }

    public enum TDetailLevel : byte
    {
        dtlLow = 0,
        dtlNormal = 1,
        dtlHigh = 2
    }

    public enum TReadWriteMode : byte
    {
        rwRead = 0,
        rwWrite = 1
    }

    public enum TWindowPopupEvent : byte
    {
        wpCombat = 0,
        wpDiplomacy = 1,
        wpSpellResearched = 2,
        wpSpellReady = 3,
        wpRebellion = 4,
        wpPartyDamage = 5,
        wpHeroUpgrade = 6,
        wpHeroJoin = 7
    }

    public enum TCombatResolveMode : byte
    {
        crmChoose = 0,
        crmAlwaysFC = 1,
        crmAlwaysTC = 2
    }

    public enum TMovementSpeed : byte
    {
        mmsNormal = 0,
        mmsFast = 1,
        mmsTurbo = 2
    }

    public enum TAoWLanguage : byte
    {
        alAuto = 0,
        alEnglish = 1,
        alGerman = 2,
        alFrench = 3,
        alSpanish = 4,
        alItalian = 5
    }

    public static partial class AoWReg
    {
        // { AoWReg registry settings from AoWEPACK.dpl, recovered against Delphi 3.02.
//   Remaining draft routines are marked below. }
        public static Aow.Rtl.TGUID ID_AOWAPP = (AnsiString)"{3205D060-4FF5-11D3-811F-0050042E6D24}";
        // AoWReg.pas:811
        public static bool ValidAoWCD(byte Drive)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetDriveType */
            throw new System.NotImplementedException();
        }

        // AoWReg.pas:824
        public static bool AoWCDInserted()
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // AoWReg.pas:894
        // { The shipped GOG binary patches the drive scan to True followed by 54 NOPs. }
        public static bool GuaranteeAoWCD(Aow.Rtl.AnsiString FirstMessage, Aow.Rtl.AnsiString RetryMessage)
        {
            /* TODO(T-shim) Platform or editor boundary Dialogs.MessageDlg */
            throw new System.NotImplementedException();
        }

        public static readonly Aow.Rtl.AnsiString AgeOfWondersMapStr = (AnsiString)"Age Of Wonders Map";
        // AoWReg.pas:189
        public static Aow.Rtl.TRect StringToRect(Aow.Rtl.AnsiString S)
        {
            Aow.Rtl.TRect Result = default;
            int P = default;
            int I = default;
            int Value = default;
            Aow.Rtl.AnsiString Part = default;
            Result = default;
            P = Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(44), S);
            I = 0;
            while ((S != ((AnsiString)"")) && (I != 4))
            {
                if (P != 0)
                {
                    Part = Aow.Rtl.Rtl.Copy(S, 1, P - 1);
                    S = Aow.Rtl.Rtl.Delete(S, 1, P);
                }
                else
                {
                    Part = S;
                    S = (AnsiString)"";
                }

                if (Part != ((AnsiString)""))
                {
                    Value = Aow.Rtl.Rtl.StrToInt(Part);
                }
                else
                {
                    Value = 0;
                }

                switch (I)
                {
                    case 0:
                        Result.Left = Value;
                        break;
                    case 1:
                        Result.Top = Value;
                        break;
                    case 2:
                        Result.Right = Value;
                        break;
                    case 3:
                        Result.Bottom = Value;
                        break;
                }

                P = Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(44), S);
                I += 1;
            }

            return Result;
        }

        // AoWReg.pas:221
        public static Aow.Rtl.AnsiString RectToString(Aow.Rtl.TRect R)
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (((((Aow.Rtl.Rtl.IntToStr(R.Left) + AnsiString.FromBytes(44)) + Aow.Rtl.Rtl.IntToStr(R.Top)) + AnsiString.FromBytes(44)) + Aow.Rtl.Rtl.IntToStr(R.Right)) + AnsiString.FromBytes(44)) + Aow.Rtl.Rtl.IntToStr(R.Bottom);
            return Result;
        }

        // AoWReg.pas:701
        public static Aow.Rtl.AnsiString TransformPassword(Aow.Rtl.AnsiString S, bool Mode)
        {
            Aow.Rtl.AnsiString Result = default;
            int I = default;
            Result = (AnsiString)"";
            {
                I = 1;
                int end__1 = S.Length;
                for (; I <= end__1; I += 1)
                {
                    Result = Result + AnsiString.FromBytes(unchecked((byte)(((S.Length + (I + 5)) & 255) ^ ((int)S[I]))));
                }
            }

            return Result;
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
