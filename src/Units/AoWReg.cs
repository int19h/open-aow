// Translated from AoWReg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using static Aow.Units.Engine.Engine;

namespace Aow.Units.AoWReg;

// { ot=1 }
// inherited TRegistry fields
public partial class TAoWRegistry : TRegistry
{
    // AoWReg.pas:128
    public void AssociateAoWMap(string Executable)
    {
        /* TODO: Implement Registry.RootKey in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AoWReg.pas:141
    public void rwInteger(string Key, string Name, ref int Value, TReadWriteMode Mode)
    {
        if (OpenKey("\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
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
    public void rwBool(string Key, string Name, ref bool Value, TReadWriteMode Mode)
    {
        if (OpenKey("\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
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
    public void rwString(string Key, string Name, ref string Value, TReadWriteMode Mode)
    {
        if (OpenKey("\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
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
    public void rwBinaryData(string Key, string Name, Span<byte> Value, int Size, TReadWriteMode Mode)
    {
        if (OpenKey("\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
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
    public void rwRect(string Key, string Name, ref TRect Value, TReadWriteMode Mode)
    {
        string S;
        if (OpenKey("\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
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
            rwRect("General\\rectangles", IntToStr(Index), ref Result, TReadWriteMode.rwRead);
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
            rwRect("General\\rectangles", IntToStr(Index), ref Value, TReadWriteMode.rwWrite);
        }
    }

    // AoWReg.pas:266
    public bool GetBoolean(int Index)
    {
        bool Result = default;
        // { Original leaves Result uninitialized for a missing nonnegative index. }
        if (Index >= 0)
        {
            rwBool("General\\booleans", IntToStr(Index), ref Result, TReadWriteMode.rwRead);
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
            rwBool("General\\booleans", IntToStr(Index), ref Value, TReadWriteMode.rwWrite);
        }
    }

    // AoWReg.pas:280
    public string GetString(int Index)
    {
        string Result = "";
        if (Index >= 0)
        {
            rwString("General\\strings", IntToStr(Index), ref Result, TReadWriteMode.rwRead);
        }
        else
        {
            Result = "";
        }

        return Result;
    }

    // AoWReg.pas:287
    public void SetString(int Index, string Value)
    {
        if (Index >= 0)
        {
            rwString("General\\strings", IntToStr(Index), ref Value, TReadWriteMode.rwWrite);
        }
    }

    // AoWReg.pas:293
    public int GetInteger(int Index)
    {
        int Result = default;
        // { Original leaves Result uninitialized for a missing nonnegative index. }
        if (Index >= 0)
        {
            rwInteger("General\\integers", IntToStr(Index), ref Result, TReadWriteMode.rwRead);
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
            rwInteger("General\\integers", IntToStr(Index), ref Value, TReadWriteMode.rwWrite);
        }
    }

    // AoWReg.pas:307
    public bool GetDeveloper() => false;

    // AoWReg.pas:312
    public void SetDeveloper(bool Value)
    {
        /* TODO: Implement Registry.DeleteValue in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AoWReg.pas:329
    public bool GetSkipIntro()
    {
        bool Result;
        Result = false;
        rwBool("General", "Skip Intro", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:335
    public void SetSkipIntro(bool Value)
    {
        rwBool("General", "Skip Intro", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:340
    public int GetDisplayMode()
    {
        int Result;
        Result = 1;
        rwInteger("Video", "Display Mode", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:346
    public void SetDisplayMode(int Value)
    {
        rwInteger("Video", "Display Mode", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:351
    public int GetDisplayWidth()
    {
        int Result;
        Result = 640;
        rwInteger("Video", "Display Width", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:357
    public void SetDisplayWidth(int Value)
    {
        rwInteger("Video", "Display Width", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:362
    public int GetDisplayHeight()
    {
        int Result;
        Result = 480;
        rwInteger("Video", "Display Height", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:368
    public void SetDisplayHeight(int Value)
    {
        rwInteger("Video", "Display Height", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:373
    public bool GetSurfaceInVMem()
    {
        bool Result;
        Result = false;
        rwBool("Video", "Surface In VMem", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:379
    public void SetSurfaceInVMem(bool Value)
    {
        rwBool("Video", "Surface In VMem", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:384
    public bool GetAllowResolutionSwitches()
    {
        bool Result;
        Result = false;
        rwBool("Video", "Allow Resolution Switches", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:390
    public void SetAllowResolutionSwitches(bool Value)
    {
        rwBool("Video", "Allow Resolution Switches", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:395
    public bool GetDebugMode()
    {
        bool Result;
        Result = false;
        rwBool("General", "Debug Mode", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:401
    public void SetDebugMode(bool Value)
    {
        rwBool("General", "Debug Mode", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:406
    public string GetRootDirectory()
    {
        string Result;
        Result = "";
        rwString("General", "Root Directory", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:412
    public void SetRootDirectory(string Value)
    {
        rwString("General", "Root Directory", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:428
    public void SetLanguage(TAoWLanguage Value)
    {
        int I;
        I = (int)Value;
        rwInteger("General", "Language", ref I, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:436
    public int GetSoundQuality()
    {
        int Result;
        Result = 3;
        rwInteger("Sound", "Sound Quality", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:442
    public void SetSoundQuality(int Value)
    {
        rwInteger("Sound", "Sound Quality", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:447
    public int GetMusicVolume()
    {
        int Result;
        Result = 50;
        rwInteger("Sound", "Music Volume", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:453
    public void SetMusicVolume(int Value)
    {
        rwInteger("Sound", "Music Volume", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:458
    public int GetSFXVolume()
    {
        int Result;
        Result = 50;
        rwInteger("Sound", "SFX Volume", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:464
    public void SetSFXVolume(int Value)
    {
        rwInteger("Sound", "SFX Volume", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:469
    public TRect GetMainWindowPosition()
    {
        TRect Result;
        Result = default;
        rwRect("Workspace", "Main Window Position", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:475
    public void SetMainWindowPosition(TRect Value)
    {
        rwRect("Workspace", "Main Window Position", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:480
    public TWindowState GetMainWindowState()
    {
        int Value;
        Value = 0;
        rwInteger("Workspace", "Main Window State", ref Value, TReadWriteMode.rwRead);
        return (TWindowState)Value;
    }

    // AoWReg.pas:489
    public void SetMainWindowState(TWindowState Value)
    {
        int I;
        I = (int)Value;
        rwInteger("Workspace", "Main Window State", ref I, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:497
    public TRect GetMapWindowPosition()
    {
        TRect Result;
        Result = default;
        rwRect("Workspace", "Map Window Position", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:503
    public void SetMapWindowPosition(TRect Value)
    {
        rwRect("Workspace", "Map Window Position", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:508
    public bool GetShadowsEnabled()
    {
        bool Result;
        Result = true;
        rwBool("Video", "Shadows Enabled", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:514
    public void SetShadowsEnabled(bool Value)
    {
        rwBool("Video", "Shadows Enabled", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:519
    public bool GetSFXEnabled()
    {
        bool Result;
        Result = true;
        rwBool("Sound", "Sound Effects Enabled", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:525
    public void SetSFXEnabled(bool Value)
    {
        rwBool("Sound", "Sound Effects Enabled", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:530
    public bool GetMusicEnabled()
    {
        bool Result;
        Result = true;
        rwBool("Sound", "Music Enabled", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:536
    public void SetMusicEnabled(bool Value)
    {
        rwBool("Sound", "Music Enabled", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:541
    public bool GetUseDirectSound()
    {
        bool Result;
        Result = false;
        rwBool("Sound", "Use DirectSound", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:547
    public void SetUseDirectSound(bool Value)
    {
        rwBool("Sound", "Use DirectSound", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:552
    public TDetailLevel GetDetailLevel()
    {
        int Value;
        Value = 2;
        rwInteger("Workspace", "Detail Level", ref Value, TReadWriteMode.rwRead);
        return (TDetailLevel)Value;
    }

    // AoWReg.pas:561
    public void SetDetailLevel(TDetailLevel Value)
    {
        int I;
        I = (int)Value;
        rwInteger("Workspace", "Detail Level", ref I, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:569
    public string GetStartupDirectory()
    {
        string Result;
        Result = "";
        rwString("General", "Startup Directory", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:575
    public void SetStartupDirectory(string Value)
    {
        rwString("General", "Startup Directory", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:580
    public bool GetLogTokenEvents()
    {
        bool Result;
        Result = false;
        rwBool("General", "Log Token Events", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:586
    public void SetLogTokenEvents(bool Value)
    {
        rwBool("General", "Log Token Events", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:591
    public bool GetAutoSave()
    {
        bool Result;
        Result = true;
        rwBool("General", "Auto Save", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:597
    public void SetAutoSave(bool Value)
    {
        rwBool("General", "Auto Save", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:602
    public bool GetAutoDebugSave()
    {
        bool Result;
        Result = false;
        rwBool("General", "Auto Debug Save", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:608
    public void SetAutoDebugSave(bool Value)
    {
        rwBool("General", "Auto Debug Save", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:613
    public bool GetFastMovementEnabled()
    {
        bool Result;
        Result = false;
        rwBool("General", "Fast Movement Enabled", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:619
    public void SetFastMovementEnabled(bool Value)
    {
        rwBool("General", "Fast Movement Enabled", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:646
    public TCombatResolveMode GetCombatResolveMode()
    {
        TCombatResolveMode Result;
        Result = TCombatResolveMode.crmChoose;
        rwBinaryData("General", "Combat Resolve Mode", Bytes.Of(ref Result), 1, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:652
    public void SetCombatResolveMode(TCombatResolveMode Value)
    {
        rwBinaryData("General", "Combat Resolve Mode", Bytes.Of(ref Value), 1, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:657
    public TMovementSpeed GetMovementSpeed()
    {
        TMovementSpeed Result;
        Result = TMovementSpeed.mmsNormal;
        rwBinaryData("General", "Movement Speed", Bytes.Of(ref Result), 1, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:663
    public void SetMovementSpeed(TMovementSpeed Value)
    {
        rwBinaryData("General", "Movement Speed", Bytes.Of(ref Value), 1, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:668
    public string GetLocalEmailAddress()
    {
        string Result;
        Result = "";
        rwString("Email", "Local Email Address", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:674
    public void SetLocalEmailAddress(string Value)
    {
        rwString("Email", "Local Email address", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:679
    public string GetSMTPServer()
    {
        string Result;
        Result = "";
        rwString("Email", "SMTP Server", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:685
    public void SetSMTPServer(string Value)
    {
        rwString("Email", "SMTP Server", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:690
    public string GetPOP3Server()
    {
        string Result;
        Result = "";
        rwString("Email", "POP3 Server", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:696
    public void SetPOP3Server(string Value)
    {
        rwString("Email", "POP3 Server", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:710
    public string GetPOP3UserName()
    {
        string Result;
        Result = "";
        rwString("Email", "POP3 UserName", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:716
    public void SetPOP3UserName(string Value)
    {
        rwString("Email", "POP3 UserName", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:721
    public string GetPOP3Password()
    {
        string Result;
        Result = "";
        rwString("Email", "POP3 Password", ref Result, TReadWriteMode.rwRead);
        Result = AoWReg.TransformPassword(Result, true);
        return Result;
    }

    // AoWReg.pas:728
    public void SetPOP3Password(string Value)
    {
        Value = AoWReg.TransformPassword(Value, true);
        rwString("Email", "POP3 Password", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:734
    public string GetAttachmentDirectory()
    {
        string Result;
        Result = "";
        rwString("Email", "Attachment Directory", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:740
    public void SetAttachmentDirectory(string Value)
    {
        if (Value.Length > 0 && Value[Value.Length - 1] != 92)
        {
            Value = Value + "\\";
        }

        rwString("Email", "Attachment Directory", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:747
    public int GetCheckEmailFrequency()
    {
        int Result;
        Result = 1;
        rwInteger("Email", "Check Email Frequency", ref Result, TReadWriteMode.rwRead);
        return Result;
    }

    // AoWReg.pas:753
    public void SetCheckEmailFrequency(int Value)
    {
        rwInteger("Email", "Check Email Frequency", ref Value, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:758
    public int GetEmailGamesCRC()
    {
        string S = "";
        int P;
        int Result = 0;
        rwString("General", "Serial number", ref S, TReadWriteMode.rwRead);
        try
        {
            P = Pos("-", S);
            // { Original tests against -1 although Pos returns 0 when absent. }
            if (P != -1)
            {
                S = Copy(S, P + 1, S.Length - P);
                P = Pos("-", S);
                if (P != -1)
                {
                    S = Copy(S, 1, P - 1);
                    if (!IsEmpty(S))
                    {
                        Result = HexToInt(S);
                    }
                }
            }
        }
        catch
        {
            Result = 0;
        }

        return Result;
    }

    // AoWReg.pas:783
    public void SetEmailGamesCRC(int Value)
    {
        string S;
        S = "4G5F6JHH-" + IntToHex(Value, 8) + "-6AG66554-98LK0FG3";
        rwString("General", "Serial number", ref S, TReadWriteMode.rwWrite);
    }

    // AoWReg.pas:791
    public void RegisterDPlayLobbyServer()
    {
        /* TODO: Implement Registry.RootKey in the platform or editor service. */
        throw new NotImplementedException();
    }

    // Automatic and English retain the shipped forcing behavior.
    public TAoWLanguage GetLanguage()
    {
        int value = 0;
        rwInteger("General", "Language", ref value, TReadWriteMode.rwRead);
        return value is >= 2 and <= 5 ? (TAoWLanguage)value : TAoWLanguage.alEnglish;
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
    public static TGUID ID_AOWAPP = "{3205D060-4FF5-11D3-811F-0050042E6D24}";
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
    public static bool GuaranteeAoWCD(string FirstMessage, string RetryMessage)
    {
        /* TODO: Implement Dialogs.MessageDlg in the platform or editor service. */
        throw new NotImplementedException();
    }

    public static readonly string AgeOfWondersMapStr = "Age Of Wonders Map";
    // AoWReg.pas:189
    public static TRect StringToRect(string S)
    {
        int P;
        int I;
        int Value;
        string Part;
        TRect Result = default;
        P = Pos(",", S);
        I = 0;
        while (!IsEmpty(S) && I != 4)
        {
            if (P != 0)
            {
                Part = Copy(S, 1, P - 1);
                S = Delete(S, 1, P);
            }
            else
            {
                Part = S;
                S = "";
            }

            if (!IsEmpty(Part))
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

            P = Pos(",", S);
            I += 1;
        }

        return Result;
    }

    // AoWReg.pas:221
    public static string RectToString(TRect R) => IntToStr(R.Left) + "," + IntToStr(R.Top) + "," + IntToStr(R.Right) + "," + IntToStr(R.Bottom);

    // AoWReg.pas:701
    public static string TransformPassword(string S, bool Mode)
    {
        string Result = "";
        for (int I = 1; I <= S.Length; I++)
        {
            Result = Result + Cp1252.Text((byte)(((S.Length + (I + 5)) & 255) ^ Cp1252.Byte(S[I - 1])));
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
