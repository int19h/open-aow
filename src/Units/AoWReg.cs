// Translated from AoWReg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using static Aow.Units.Engine.Engine;

namespace Aow.Units.AoWReg
{
    // { ot=1 }
    // inherited TRegistry fields
    public partial class TAoWRegistry : TRegistry
    {
        // AoWReg.pas:128
        public void AssociateAoWMap(AnsiString Executable)
        {
            /* TODO: Implement Registry.RootKey in the platform or editor service. */
            throw new NotImplementedException();
        }

        // AoWReg.pas:141
        public void rwInteger(AnsiString Key, AnsiString Name, ref int Value, TReadWriteMode Mode)
        {
            if (OpenKey((AnsiString)"\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
            {
                switch (Mode)
                {
                    case TReadWriteMode.rwRead:
                        if (ValueExists(Name))
                        {
                            Value = ReadInteger(Name);
                        }

                        break;
                    case TReadWriteMode.rwWrite:
                        WriteInteger(Name, Value);
                        break;
                }

                CloseKey();
            }
        }

        // AoWReg.pas:153
        public void rwBool(AnsiString Key, AnsiString Name, ref bool Value, TReadWriteMode Mode)
        {
            if (OpenKey((AnsiString)"\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
            {
                switch (Mode)
                {
                    case TReadWriteMode.rwRead:
                        if (ValueExists(Name))
                        {
                            Value = ReadBool(Name);
                        }

                        break;
                    case TReadWriteMode.rwWrite:
                        WriteBool(Name, Value);
                        break;
                }

                CloseKey();
            }
        }

        // AoWReg.pas:165
        public void rwString(AnsiString Key, AnsiString Name, ref AnsiString Value, TReadWriteMode Mode)
        {
            if (OpenKey((AnsiString)"\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
            {
                switch (Mode)
                {
                    case TReadWriteMode.rwRead:
                        if (ValueExists(Name))
                        {
                            Value = ReadString(Name);
                        }

                        break;
                    case TReadWriteMode.rwWrite:
                        WriteString(Name, Value);
                        break;
                }

                CloseKey();
            }
        }

        // AoWReg.pas:177
        public void rwBinaryData(AnsiString Key, AnsiString Name, Span<byte> Value, int Size, TReadWriteMode Mode)
        {
            if (OpenKey((AnsiString)"\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
            {
                switch (Mode)
                {
                    case TReadWriteMode.rwRead:
                        if (ValueExists(Name))
                        {
                            ReadBinaryData(Name, Value, Size);
                        }

                        break;
                    case TReadWriteMode.rwWrite:
                        WriteBinaryData(Name, Value, Size);
                        break;
                }

                CloseKey();
            }
        }

        // AoWReg.pas:227
        public void rwRect(AnsiString Key, AnsiString Name, ref TRect Value, TReadWriteMode Mode)
        {
            AnsiString S = default;
            if (OpenKey((AnsiString)"\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
            {
                switch (Mode)
                {
                    case TReadWriteMode.rwRead:
                        if (ValueExists(Name))
                        {
                            S = ReadString(Name);
                            Value = AoWReg.StringToRect(S);
                        }

                        break;
                    case TReadWriteMode.rwWrite:
                        S = AoWReg.RectToString(Value);
                        WriteString(Name, S);
                        break;
                }

                CloseKey();
            }
        }

        // AoWReg.pas:250
        public TRect GetRectangle(int Index)
        {
            TRect Result = default;
            if (Index >= 0)
            {
                Result = default;
                rwRect((AnsiString)"General\\rectangles", IntToStr(Index), ref Result, TReadWriteMode.rwRead);
            }
            else
            {
                Result = default;
            }

            return Result;
        }

        // AoWReg.pas:260
        public void SetRectangle(int Index, TRect Value)
        {
            if (Index >= 0)
            {
                rwRect((AnsiString)"General\\rectangles", IntToStr(Index), ref Value, TReadWriteMode.rwWrite);
            }
        }

        // AoWReg.pas:266
        public bool GetBoolean(int Index)
        {
            bool Result = default;
            // { Original leaves Result uninitialized for a missing nonnegative index. }
            if (Index >= 0)
            {
                rwBool((AnsiString)"General\\booleans", IntToStr(Index), ref Result, TReadWriteMode.rwRead);
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
                rwBool((AnsiString)"General\\booleans", IntToStr(Index), ref Value, TReadWriteMode.rwWrite);
            }
        }

        // AoWReg.pas:280
        public AnsiString GetString(int Index)
        {
            AnsiString Result = default;
            if (Index >= 0)
            {
                rwString((AnsiString)"General\\strings", IntToStr(Index), ref Result, TReadWriteMode.rwRead);
            }
            else
            {
                Result = (AnsiString)"";
            }

            return Result;
        }

        // AoWReg.pas:287
        public void SetString(int Index, AnsiString Value)
        {
            if (Index >= 0)
            {
                rwString((AnsiString)"General\\strings", IntToStr(Index), ref Value, TReadWriteMode.rwWrite);
            }
        }

        // AoWReg.pas:293
        public int GetInteger(int Index)
        {
            int Result = default;
            // { Original leaves Result uninitialized for a missing nonnegative index. }
            if (Index >= 0)
            {
                rwInteger((AnsiString)"General\\integers", IntToStr(Index), ref Result, TReadWriteMode.rwRead);
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
                rwInteger((AnsiString)"General\\integers", IntToStr(Index), ref Value, TReadWriteMode.rwWrite);
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
            /* TODO: Implement Registry.DeleteValue in the platform or editor service. */
            throw new NotImplementedException();
        }

        // AoWReg.pas:329
        public bool GetSkipIntro()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"General", (AnsiString)"Skip Intro", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:335
        public void SetSkipIntro(bool Value)
        {
            rwBool((AnsiString)"General", (AnsiString)"Skip Intro", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:340
        public int GetDisplayMode()
        {
            int Result = default;
            Result = 1;
            rwInteger((AnsiString)"Video", (AnsiString)"Display Mode", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:346
        public void SetDisplayMode(int Value)
        {
            rwInteger((AnsiString)"Video", (AnsiString)"Display Mode", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:351
        public int GetDisplayWidth()
        {
            int Result = default;
            Result = 640;
            rwInteger((AnsiString)"Video", (AnsiString)"Display Width", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:357
        public void SetDisplayWidth(int Value)
        {
            rwInteger((AnsiString)"Video", (AnsiString)"Display Width", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:362
        public int GetDisplayHeight()
        {
            int Result = default;
            Result = 480;
            rwInteger((AnsiString)"Video", (AnsiString)"Display Height", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:368
        public void SetDisplayHeight(int Value)
        {
            rwInteger((AnsiString)"Video", (AnsiString)"Display Height", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:373
        public bool GetSurfaceInVMem()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"Video", (AnsiString)"Surface In VMem", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:379
        public void SetSurfaceInVMem(bool Value)
        {
            rwBool((AnsiString)"Video", (AnsiString)"Surface In VMem", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:384
        public bool GetAllowResolutionSwitches()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"Video", (AnsiString)"Allow Resolution Switches", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:390
        public void SetAllowResolutionSwitches(bool Value)
        {
            rwBool((AnsiString)"Video", (AnsiString)"Allow Resolution Switches", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:395
        public bool GetDebugMode()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"General", (AnsiString)"Debug Mode", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:401
        public void SetDebugMode(bool Value)
        {
            rwBool((AnsiString)"General", (AnsiString)"Debug Mode", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:406
        public AnsiString GetRootDirectory()
        {
            AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"General", (AnsiString)"Root Directory", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:412
        public void SetRootDirectory(AnsiString Value)
        {
            rwString((AnsiString)"General", (AnsiString)"Root Directory", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:428
        public void SetLanguage(TAoWLanguage Value)
        {
            int I = default;
            I = (int)Value;
            rwInteger((AnsiString)"General", (AnsiString)"Language", ref I, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:436
        public int GetSoundQuality()
        {
            int Result = default;
            Result = 3;
            rwInteger((AnsiString)"Sound", (AnsiString)"Sound Quality", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:442
        public void SetSoundQuality(int Value)
        {
            rwInteger((AnsiString)"Sound", (AnsiString)"Sound Quality", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:447
        public int GetMusicVolume()
        {
            int Result = default;
            Result = 50;
            rwInteger((AnsiString)"Sound", (AnsiString)"Music Volume", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:453
        public void SetMusicVolume(int Value)
        {
            rwInteger((AnsiString)"Sound", (AnsiString)"Music Volume", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:458
        public int GetSFXVolume()
        {
            int Result = default;
            Result = 50;
            rwInteger((AnsiString)"Sound", (AnsiString)"SFX Volume", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:464
        public void SetSFXVolume(int Value)
        {
            rwInteger((AnsiString)"Sound", (AnsiString)"SFX Volume", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:469
        public TRect GetMainWindowPosition()
        {
            TRect Result = default;
            Result = default;
            rwRect((AnsiString)"Workspace", (AnsiString)"Main Window Position", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:475
        public void SetMainWindowPosition(TRect Value)
        {
            rwRect((AnsiString)"Workspace", (AnsiString)"Main Window Position", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:480
        public TWindowState GetMainWindowState()
        {
            TWindowState Result = default;
            int Value = default;
            Value = 0;
            rwInteger((AnsiString)"Workspace", (AnsiString)"Main Window State", ref Value, TReadWriteMode.rwRead);
            Result = (TWindowState)Value;
            return Result;
        }

        // AoWReg.pas:489
        public void SetMainWindowState(TWindowState Value)
        {
            int I = default;
            I = (int)Value;
            rwInteger((AnsiString)"Workspace", (AnsiString)"Main Window State", ref I, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:497
        public TRect GetMapWindowPosition()
        {
            TRect Result = default;
            Result = default;
            rwRect((AnsiString)"Workspace", (AnsiString)"Map Window Position", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:503
        public void SetMapWindowPosition(TRect Value)
        {
            rwRect((AnsiString)"Workspace", (AnsiString)"Map Window Position", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:508
        public bool GetShadowsEnabled()
        {
            bool Result = default;
            Result = true;
            rwBool((AnsiString)"Video", (AnsiString)"Shadows Enabled", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:514
        public void SetShadowsEnabled(bool Value)
        {
            rwBool((AnsiString)"Video", (AnsiString)"Shadows Enabled", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:519
        public bool GetSFXEnabled()
        {
            bool Result = default;
            Result = true;
            rwBool((AnsiString)"Sound", (AnsiString)"Sound Effects Enabled", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:525
        public void SetSFXEnabled(bool Value)
        {
            rwBool((AnsiString)"Sound", (AnsiString)"Sound Effects Enabled", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:530
        public bool GetMusicEnabled()
        {
            bool Result = default;
            Result = true;
            rwBool((AnsiString)"Sound", (AnsiString)"Music Enabled", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:536
        public void SetMusicEnabled(bool Value)
        {
            rwBool((AnsiString)"Sound", (AnsiString)"Music Enabled", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:541
        public bool GetUseDirectSound()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"Sound", (AnsiString)"Use DirectSound", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:547
        public void SetUseDirectSound(bool Value)
        {
            rwBool((AnsiString)"Sound", (AnsiString)"Use DirectSound", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:552
        public TDetailLevel GetDetailLevel()
        {
            TDetailLevel Result = default;
            int Value = default;
            Value = 2;
            rwInteger((AnsiString)"Workspace", (AnsiString)"Detail Level", ref Value, TReadWriteMode.rwRead);
            Result = (TDetailLevel)Value;
            return Result;
        }

        // AoWReg.pas:561
        public void SetDetailLevel(TDetailLevel Value)
        {
            int I = default;
            I = (int)Value;
            rwInteger((AnsiString)"Workspace", (AnsiString)"Detail Level", ref I, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:569
        public AnsiString GetStartupDirectory()
        {
            AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"General", (AnsiString)"Startup Directory", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:575
        public void SetStartupDirectory(AnsiString Value)
        {
            rwString((AnsiString)"General", (AnsiString)"Startup Directory", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:580
        public bool GetLogTokenEvents()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"General", (AnsiString)"Log Token Events", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:586
        public void SetLogTokenEvents(bool Value)
        {
            rwBool((AnsiString)"General", (AnsiString)"Log Token Events", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:591
        public bool GetAutoSave()
        {
            bool Result = default;
            Result = true;
            rwBool((AnsiString)"General", (AnsiString)"Auto Save", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:597
        public void SetAutoSave(bool Value)
        {
            rwBool((AnsiString)"General", (AnsiString)"Auto Save", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:602
        public bool GetAutoDebugSave()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"General", (AnsiString)"Auto Debug Save", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:608
        public void SetAutoDebugSave(bool Value)
        {
            rwBool((AnsiString)"General", (AnsiString)"Auto Debug Save", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:613
        public bool GetFastMovementEnabled()
        {
            bool Result = default;
            Result = false;
            rwBool((AnsiString)"General", (AnsiString)"Fast Movement Enabled", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:619
        public void SetFastMovementEnabled(bool Value)
        {
            rwBool((AnsiString)"General", (AnsiString)"Fast Movement Enabled", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:646
        public TCombatResolveMode GetCombatResolveMode()
        {
            TCombatResolveMode Result = default;
            Result = TCombatResolveMode.crmChoose;
            rwBinaryData((AnsiString)"General", (AnsiString)"Combat Resolve Mode", Bytes.Of(ref Result), 1, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:652
        public void SetCombatResolveMode(TCombatResolveMode Value)
        {
            rwBinaryData((AnsiString)"General", (AnsiString)"Combat Resolve Mode", Bytes.Of(ref Value), 1, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:657
        public TMovementSpeed GetMovementSpeed()
        {
            TMovementSpeed Result = default;
            Result = TMovementSpeed.mmsNormal;
            rwBinaryData((AnsiString)"General", (AnsiString)"Movement Speed", Bytes.Of(ref Result), 1, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:663
        public void SetMovementSpeed(TMovementSpeed Value)
        {
            rwBinaryData((AnsiString)"General", (AnsiString)"Movement Speed", Bytes.Of(ref Value), 1, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:668
        public AnsiString GetLocalEmailAddress()
        {
            AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"Email", (AnsiString)"Local Email Address", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:674
        public void SetLocalEmailAddress(AnsiString Value)
        {
            rwString((AnsiString)"Email", (AnsiString)"Local Email address", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:679
        public AnsiString GetSMTPServer()
        {
            AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"Email", (AnsiString)"SMTP Server", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:685
        public void SetSMTPServer(AnsiString Value)
        {
            rwString((AnsiString)"Email", (AnsiString)"SMTP Server", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:690
        public AnsiString GetPOP3Server()
        {
            AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"Email", (AnsiString)"POP3 Server", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:696
        public void SetPOP3Server(AnsiString Value)
        {
            rwString((AnsiString)"Email", (AnsiString)"POP3 Server", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:710
        public AnsiString GetPOP3UserName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"Email", (AnsiString)"POP3 UserName", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:716
        public void SetPOP3UserName(AnsiString Value)
        {
            rwString((AnsiString)"Email", (AnsiString)"POP3 UserName", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:721
        public AnsiString GetPOP3Password()
        {
            AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"Email", (AnsiString)"POP3 Password", ref Result, TReadWriteMode.rwRead);
            Result = AoWReg.TransformPassword(Result, true);
            return Result;
        }

        // AoWReg.pas:728
        public void SetPOP3Password(AnsiString Value)
        {
            Value = AoWReg.TransformPassword(Value, true);
            rwString((AnsiString)"Email", (AnsiString)"POP3 Password", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:734
        public AnsiString GetAttachmentDirectory()
        {
            AnsiString Result = default;
            Result = (AnsiString)"";
            rwString((AnsiString)"Email", (AnsiString)"Attachment Directory", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:740
        public void SetAttachmentDirectory(AnsiString Value)
        {
            if (Value.Length > 0 && Value[Value.Length] != 92)
            {
                Value = Value + AnsiString.FromBytes(92);
            }

            rwString((AnsiString)"Email", (AnsiString)"Attachment Directory", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:747
        public int GetCheckEmailFrequency()
        {
            int Result = default;
            Result = 1;
            rwInteger((AnsiString)"Email", (AnsiString)"Check Email Frequency", ref Result, TReadWriteMode.rwRead);
            return Result;
        }

        // AoWReg.pas:753
        public void SetCheckEmailFrequency(int Value)
        {
            rwInteger((AnsiString)"Email", (AnsiString)"Check Email Frequency", ref Value, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:758
        public int GetEmailGamesCRC()
        {
            int Result = default;
            AnsiString S = default;
            int P = default;
            Result = 0;
            rwString((AnsiString)"General", (AnsiString)"Serial number", ref S, TReadWriteMode.rwRead);
            try
            {
                P = Pos(AnsiString.FromBytes(45), S);
                // { Original tests against -1 although Pos returns 0 when absent. }
                if (P != -1)
                {
                    S = Copy(S, P + 1, S.Length - P);
                    P = Pos(AnsiString.FromBytes(45), S);
                    if (P != -1)
                    {
                        S = Copy(S, 1, P - 1);
                        if (S != (AnsiString)"")
                        {
                            Result = HexToInt(S);
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
            AnsiString S = default;
            S = (AnsiString)"4G5F6JHH-" + IntToHex(Value, 8) + (AnsiString)"-6AG66554-98LK0FG3";
            rwString((AnsiString)"General", (AnsiString)"Serial number", ref S, TReadWriteMode.rwWrite);
        }

        // AoWReg.pas:791
        public void RegisterDPlayLobbyServer()
        {
            /* TODO: Implement Registry.RootKey in the platform or editor service. */
            throw new NotImplementedException();
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
        // AoWReg registry settings from AoWEPACK.dpl, recovered against Delphi 3.02
        public static TGUID ID_AOWAPP = (AnsiString)"{3205D060-4FF5-11D3-811F-0050042E6D24}";
        // AoWReg.pas:811
        public static bool ValidAoWCD(byte Drive)
        {
            /* TODO: Implement Windows.GetDriveType in the platform or editor service. */
            throw new NotImplementedException();
        }

        // AoWReg.pas:824
        public static bool AoWCDInserted() => true;

        // AoWReg.pas:894
        // { The shipped GOG binary patches the drive scan to True followed by 54 NOPs. }
        public static bool GuaranteeAoWCD(AnsiString FirstMessage, AnsiString RetryMessage)
        {
            /* TODO: Implement Dialogs.MessageDlg in the platform or editor service. */
            throw new NotImplementedException();
        }

        public static readonly AnsiString AgeOfWondersMapStr = (AnsiString)"Age Of Wonders Map";
        // AoWReg.pas:189
        public static TRect StringToRect(AnsiString S)
        {
            TRect Result = default;
            int P = default;
            int I = default;
            int Value = default;
            AnsiString Part = default;
            Result = default;
            P = Pos(AnsiString.FromBytes(44), S);
            I = 0;
            while (S != (AnsiString)"" && I != 4)
            {
                if (P != 0)
                {
                    Part = Copy(S, 1, P - 1);
                    S = Delete(S, 1, P);
                }
                else
                {
                    Part = S;
                    S = (AnsiString)"";
                }

                if (Part != (AnsiString)"")
                {
                    Value = StrToInt(Part);
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

                P = Pos(AnsiString.FromBytes(44), S);
                I += 1;
            }

            return Result;
        }

        // AoWReg.pas:221
        public static AnsiString RectToString(TRect R)
        {
            AnsiString Result = default;
            Result = IntToStr(R.Left) + AnsiString.FromBytes(44) + IntToStr(R.Top) + AnsiString.FromBytes(44) + IntToStr(R.Right) + AnsiString.FromBytes(44) + IntToStr(R.Bottom);
            return Result;
        }

        // AoWReg.pas:701
        public static AnsiString TransformPassword(AnsiString S, bool Mode)
        {
            AnsiString Result = default;
            int I = default;
            Result = (AnsiString)"";
            {
                I = 1;
                int end__1 = S.Length;
                for (; I <= end__1; I += 1)
                {
                    Result = Result + AnsiString.FromBytes((byte)(((S.Length + (I + 5)) & 255) ^ S[I]));
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
