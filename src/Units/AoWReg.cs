// Translated from AoWReg.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Engine;

// { ot=1 }
// inherited TRegistry fields
public partial class AoWRegistry : Registry
{
    // AoWReg.pas:128
    public void AssociateAoWMap(string Executable)
    {
        /* TODO: Implement Registry.RootKey in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AoWReg.pas:141
    public void rwInteger(string Key, string Name, ref int Value, ReadWriteMode Mode)
    {
        if (OpenKey("\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
        {
            switch (Mode)
            {
                case ReadWriteMode.Read:
                    if (ValueExists(Name))
                    {
                        Value = ReadInteger(Name);
                    }

                    break;
                case ReadWriteMode.Write:
                    WriteInteger(Name, Value);
                    break;
            }

            CloseKey();
        }
    }

    // AoWReg.pas:153
    public void rwBool(string Key, string Name, ref bool Value, ReadWriteMode Mode)
    {
        if (OpenKey("\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
        {
            switch (Mode)
            {
                case ReadWriteMode.Read:
                    if (ValueExists(Name))
                    {
                        Value = ReadBool(Name);
                    }

                    break;
                case ReadWriteMode.Write:
                    WriteBool(Name, Value);
                    break;
            }

            CloseKey();
        }
    }

    // AoWReg.pas:165
    public void rwString(string Key, string Name, ref string Value, ReadWriteMode Mode)
    {
        if (OpenKey("\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
        {
            switch (Mode)
            {
                case ReadWriteMode.Read:
                    if (ValueExists(Name))
                    {
                        Value = ReadString(Name);
                    }

                    break;
                case ReadWriteMode.Write:
                    WriteString(Name, Value);
                    break;
            }

            CloseKey();
        }
    }

    // AoWReg.pas:177
    public void rwBinaryData(string Key, string Name, Span<byte> Value, int Size, ReadWriteMode Mode)
    {
        if (OpenKey("\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
        {
            switch (Mode)
            {
                case ReadWriteMode.Read:
                    if (ValueExists(Name))
                    {
                        ReadBinaryData(Name, Value, Size);
                    }

                    break;
                case ReadWriteMode.Write:
                    WriteBinaryData(Name, Value, Size);
                    break;
            }

            CloseKey();
        }
    }

    // AoWReg.pas:227
    public void rwRect(string Key, string Name, ref Rect Value, ReadWriteMode Mode)
    {
        string S;
        if (OpenKey("\\Software\\Triumph Studios\\Age of Wonders\\" + Key, true))
        {
            switch (Mode)
            {
                case ReadWriteMode.Read:
                    if (ValueExists(Name))
                    {
                        S = ReadString(Name);
                        Value = StringToRect(S);
                    }

                    break;
                case ReadWriteMode.Write:
                    S = RectToString(Value);
                    WriteString(Name, S);
                    break;
            }

            CloseKey();
        }
    }

    // AoWReg.pas:250
    public Rect GetRectangle(int Index)
    {
        Rect Result = default;
        if (Index >= 0)
        {
            Result = default;
            rwRect("General\\rectangles", IntToStr(Index), ref Result, ReadWriteMode.Read);
        }
        else
        {
            Result = default;
        }

        return Result;
    }

    // AoWReg.pas:260
    public void SetRectangle(int Index, Rect Value)
    {
        if (Index >= 0)
        {
            rwRect("General\\rectangles", IntToStr(Index), ref Value, ReadWriteMode.Write);
        }
    }

    // AoWReg.pas:266
    public bool GetBoolean(int Index)
    {
        bool Result = default;
        // { Original leaves Result uninitialized for a missing nonnegative index. }
        if (Index >= 0)
        {
            rwBool("General\\booleans", IntToStr(Index), ref Result, ReadWriteMode.Read);
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
            rwBool("General\\booleans", IntToStr(Index), ref Value, ReadWriteMode.Write);
        }
    }

    // AoWReg.pas:280
    public string GetString(int Index)
    {
        string Result = "";
        if (Index >= 0)
        {
            rwString("General\\strings", IntToStr(Index), ref Result, ReadWriteMode.Read);
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
            rwString("General\\strings", IntToStr(Index), ref Value, ReadWriteMode.Write);
        }
    }

    // AoWReg.pas:293
    public int GetInteger(int Index)
    {
        int Result = default;
        // { Original leaves Result uninitialized for a missing nonnegative index. }
        if (Index >= 0)
        {
            rwInteger("General\\integers", IntToStr(Index), ref Result, ReadWriteMode.Read);
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
            rwInteger("General\\integers", IntToStr(Index), ref Value, ReadWriteMode.Write);
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
        rwBool("General", "Skip Intro", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:335
    public void SetSkipIntro(bool Value)
    {
        rwBool("General", "Skip Intro", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:340
    public int GetDisplayMode()
    {
        int Result;
        Result = 1;
        rwInteger("Video", "Display Mode", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:346
    public void SetDisplayMode(int Value)
    {
        rwInteger("Video", "Display Mode", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:351
    public int GetDisplayWidth()
    {
        int Result;
        Result = 640;
        rwInteger("Video", "Display Width", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:357
    public void SetDisplayWidth(int Value)
    {
        rwInteger("Video", "Display Width", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:362
    public int GetDisplayHeight()
    {
        int Result;
        Result = 480;
        rwInteger("Video", "Display Height", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:368
    public void SetDisplayHeight(int Value)
    {
        rwInteger("Video", "Display Height", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:373
    public bool GetSurfaceInVMem()
    {
        bool Result;
        Result = false;
        rwBool("Video", "Surface In VMem", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:379
    public void SetSurfaceInVMem(bool Value)
    {
        rwBool("Video", "Surface In VMem", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:384
    public bool GetAllowResolutionSwitches()
    {
        bool Result;
        Result = false;
        rwBool("Video", "Allow Resolution Switches", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:390
    public void SetAllowResolutionSwitches(bool Value)
    {
        rwBool("Video", "Allow Resolution Switches", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:395
    public bool GetDebugMode()
    {
        bool Result;
        Result = false;
        rwBool("General", "Debug Mode", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:401
    public void SetDebugMode(bool Value)
    {
        rwBool("General", "Debug Mode", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:406
    public string GetRootDirectory()
    {
        string Result;
        Result = "";
        rwString("General", "Root Directory", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:412
    public void SetRootDirectory(string Value)
    {
        rwString("General", "Root Directory", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:428
    public void SetLanguage(AoWLanguage Value)
    {
        int I;
        I = (int)Value;
        rwInteger("General", "Language", ref I, ReadWriteMode.Write);
    }

    // AoWReg.pas:436
    public int GetSoundQuality()
    {
        int Result;
        Result = 3;
        rwInteger("Sound", "Sound Quality", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:442
    public void SetSoundQuality(int Value)
    {
        rwInteger("Sound", "Sound Quality", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:447
    public int GetMusicVolume()
    {
        int Result;
        Result = 50;
        rwInteger("Sound", "Music Volume", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:453
    public void SetMusicVolume(int Value)
    {
        rwInteger("Sound", "Music Volume", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:458
    public int GetSFXVolume()
    {
        int Result;
        Result = 50;
        rwInteger("Sound", "SFX Volume", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:464
    public void SetSFXVolume(int Value)
    {
        rwInteger("Sound", "SFX Volume", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:469
    public Rect GetMainWindowPosition()
    {
        Rect Result;
        Result = default;
        rwRect("Workspace", "Main Window Position", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:475
    public void SetMainWindowPosition(Rect Value)
    {
        rwRect("Workspace", "Main Window Position", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:480
    public WindowState GetMainWindowState()
    {
        int Value;
        Value = 0;
        rwInteger("Workspace", "Main Window State", ref Value, ReadWriteMode.Read);
        return (WindowState)Value;
    }

    // AoWReg.pas:489
    public void SetMainWindowState(WindowState Value)
    {
        int I;
        I = (int)Value;
        rwInteger("Workspace", "Main Window State", ref I, ReadWriteMode.Write);
    }

    // AoWReg.pas:497
    public Rect GetMapWindowPosition()
    {
        Rect Result;
        Result = default;
        rwRect("Workspace", "Map Window Position", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:503
    public void SetMapWindowPosition(Rect Value)
    {
        rwRect("Workspace", "Map Window Position", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:508
    public bool GetShadowsEnabled()
    {
        bool Result;
        Result = true;
        rwBool("Video", "Shadows Enabled", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:514
    public void SetShadowsEnabled(bool Value)
    {
        rwBool("Video", "Shadows Enabled", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:519
    public bool GetSFXEnabled()
    {
        bool Result;
        Result = true;
        rwBool("Sound", "Sound Effects Enabled", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:525
    public void SetSFXEnabled(bool Value)
    {
        rwBool("Sound", "Sound Effects Enabled", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:530
    public bool GetMusicEnabled()
    {
        bool Result;
        Result = true;
        rwBool("Sound", "Music Enabled", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:536
    public void SetMusicEnabled(bool Value)
    {
        rwBool("Sound", "Music Enabled", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:541
    public bool GetUseDirectSound()
    {
        bool Result;
        Result = false;
        rwBool("Sound", "Use DirectSound", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:547
    public void SetUseDirectSound(bool Value)
    {
        rwBool("Sound", "Use DirectSound", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:552
    public DetailLevel GetDetailLevel()
    {
        int Value;
        Value = 2;
        rwInteger("Workspace", "Detail Level", ref Value, ReadWriteMode.Read);
        return (DetailLevel)Value;
    }

    // AoWReg.pas:561
    public void SetDetailLevel(DetailLevel Value)
    {
        int I;
        I = (int)Value;
        rwInteger("Workspace", "Detail Level", ref I, ReadWriteMode.Write);
    }

    // AoWReg.pas:569
    public string GetStartupDirectory()
    {
        string Result;
        Result = "";
        rwString("General", "Startup Directory", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:575
    public void SetStartupDirectory(string Value)
    {
        rwString("General", "Startup Directory", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:580
    public bool GetLogTokenEvents()
    {
        bool Result;
        Result = false;
        rwBool("General", "Log Token Events", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:586
    public void SetLogTokenEvents(bool Value)
    {
        rwBool("General", "Log Token Events", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:591
    public bool GetAutoSave()
    {
        bool Result;
        Result = true;
        rwBool("General", "Auto Save", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:597
    public void SetAutoSave(bool Value)
    {
        rwBool("General", "Auto Save", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:602
    public bool GetAutoDebugSave()
    {
        bool Result;
        Result = false;
        rwBool("General", "Auto Debug Save", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:608
    public void SetAutoDebugSave(bool Value)
    {
        rwBool("General", "Auto Debug Save", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:613
    public bool GetFastMovementEnabled()
    {
        bool Result;
        Result = false;
        rwBool("General", "Fast Movement Enabled", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:619
    public void SetFastMovementEnabled(bool Value)
    {
        rwBool("General", "Fast Movement Enabled", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:646
    public CombatResolveMode GetCombatResolveMode()
    {
        CombatResolveMode Result;
        Result = CombatResolveMode.Choose;
        rwBinaryData("General", "Combat Resolve Mode", Bytes.Of(ref Result), 1, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:652
    public void SetCombatResolveMode(CombatResolveMode Value)
    {
        rwBinaryData("General", "Combat Resolve Mode", Bytes.Of(ref Value), 1, ReadWriteMode.Write);
    }

    // AoWReg.pas:657
    public MovementSpeed GetMovementSpeed()
    {
        MovementSpeed Result;
        Result = MovementSpeed.Normal;
        rwBinaryData("General", "Movement Speed", Bytes.Of(ref Result), 1, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:663
    public void SetMovementSpeed(MovementSpeed Value)
    {
        rwBinaryData("General", "Movement Speed", Bytes.Of(ref Value), 1, ReadWriteMode.Write);
    }

    // AoWReg.pas:668
    public string GetLocalEmailAddress()
    {
        string Result;
        Result = "";
        rwString("Email", "Local Email Address", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:674
    public void SetLocalEmailAddress(string Value)
    {
        rwString("Email", "Local Email address", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:679
    public string GetSMTPServer()
    {
        string Result;
        Result = "";
        rwString("Email", "SMTP Server", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:685
    public void SetSMTPServer(string Value)
    {
        rwString("Email", "SMTP Server", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:690
    public string GetPOP3Server()
    {
        string Result;
        Result = "";
        rwString("Email", "POP3 Server", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:696
    public void SetPOP3Server(string Value)
    {
        rwString("Email", "POP3 Server", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:710
    public string GetPOP3UserName()
    {
        string Result;
        Result = "";
        rwString("Email", "POP3 UserName", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:716
    public void SetPOP3UserName(string Value)
    {
        rwString("Email", "POP3 UserName", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:721
    public string GetPOP3Password()
    {
        string Result;
        Result = "";
        rwString("Email", "POP3 Password", ref Result, ReadWriteMode.Read);
        Result = TransformPassword(Result, true);
        return Result;
    }

    // AoWReg.pas:728
    public void SetPOP3Password(string Value)
    {
        Value = TransformPassword(Value, true);
        rwString("Email", "POP3 Password", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:734
    public string GetAttachmentDirectory()
    {
        string Result;
        Result = "";
        rwString("Email", "Attachment Directory", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:740
    public void SetAttachmentDirectory(string Value)
    {
        if (Value.Length > 0 && Value[Value.Length - 1] != 92)
        {
            Value = Value + "\\";
        }

        rwString("Email", "Attachment Directory", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:747
    public int GetCheckEmailFrequency()
    {
        int Result;
        Result = 1;
        rwInteger("Email", "Check Email Frequency", ref Result, ReadWriteMode.Read);
        return Result;
    }

    // AoWReg.pas:753
    public void SetCheckEmailFrequency(int Value)
    {
        rwInteger("Email", "Check Email Frequency", ref Value, ReadWriteMode.Write);
    }

    // AoWReg.pas:758
    public int GetEmailGamesCRC()
    {
        string S = "";
        int P;
        int Result = 0;
        rwString("General", "Serial number", ref S, ReadWriteMode.Read);
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
        rwString("General", "Serial number", ref S, ReadWriteMode.Write);
    }

    // AoWReg.pas:791
    public void RegisterDPlayLobbyServer()
    {
        /* TODO: Implement Registry.RootKey in the platform or editor service. */
        throw new NotImplementedException();
    }

    // Automatic and English retain the shipped forcing behavior.
    public AoWLanguage GetLanguage()
    {
        int value = 0;
        rwInteger("General", "Language", ref value, ReadWriteMode.Read);
        return value is >= 2 and <= 5 ? (AoWLanguage)value : AoWLanguage.English;
    }

    private static Rect StringToRect(string S)
    {
        Rect result = default(Rect);
        int num = RtlUnit.Pos(",", S);
        int num2 = 0;
        while (!RtlUnit.IsEmpty(S) && num2 != 4)
        {
            string text;
            if (num != 0)
            {
                text = RtlUnit.Copy(S, 1, num - 1);
                S = RtlUnit.Delete(S, 1, num);
            }
            else
            {
                text = S;
                S = "";
            }
            int num3 = ((!RtlUnit.IsEmpty(text)) ? RtlUnit.StrToInt(text) : 0);
            switch (num2)
            {
                case 0:
                    result.Left = num3;
                    break;
                case 1:
                    result.Top = num3;
                    break;
                case 2:
                    result.Right = num3;
                    break;
                case 3:
                    result.Bottom = num3;
                    break;
            }
            num = RtlUnit.Pos(",", S);
            num2++;
        }
        return result;
    }

    private static string RectToString(Rect R)
    {
        return string.Concat(new string[7]
        {
            RtlUnit.IntToStr(R.Left),
            ",",
            RtlUnit.IntToStr(R.Top),
            ",",
            RtlUnit.IntToStr(R.Right),
            ",",
            RtlUnit.IntToStr(R.Bottom)
        });
    }

    private static string TransformPassword(string S, bool Mode)
    {
        string text = "";
        for (int i = 1; i <= S.Length; i++)
        {
            text += Cp1252.Text((byte)(((S.Length + (i + 5)) & 0xFF) ^ Cp1252.Byte(S[i - 1])));
        }
        return text;
    }

    private static int HexToInt(string S)
    {
        int num = 0;
        int num2 = S.Length;
        if (num2 > 8)
        {
            num2 = 8;
        }
        for (int i = 0; i <= num2 - 1; i++)
        {
            byte b = Cp1252.Byte(S[num2 - i - 1]);
            int num3 = ((!Set<byte>.WithBounds(0, 255).WithRange((byte)48, (byte)57).Contains(b)) ? ((!Set<byte>.WithBounds(0, 255).WithRange((byte)65, (byte)70).Contains(b)) ? (Set<byte>.WithBounds(0, 255).WithRange((byte)97, (byte)102).Contains(b) ? (b - 97 + 10) : 0) : (b - 65 + 10)) : (b - 48));
            num |= num3 << i * 4;
        }
        return num;
    }

    public enum ReadWriteMode : byte
    {
        Read = 0, Write = 1
    }

    public enum CombatResolveMode : byte
    {
        Choose = 0, AlwaysFC = 1, AlwaysTC = 2
    }

    public enum MovementSpeed : byte
    {
        Normal = 0, Fast = 1, Turbo = 2
    }

    public enum AoWLanguage : byte
    {
        Auto = 0, English = 1, German = 2, French = 3, Spanish = 4, Italian = 5
    }
}

public enum DetailLevel : byte
{
    Low = 0,
    Normal = 1,
    High = 2
}

public enum WindowPopupEvent : byte
{
    Combat = 0,
    Diplomacy = 1,
    SpellResearched = 2,
    SpellReady = 3,
    Rebellion = 4,
    PartyDamage = 5,
    HeroUpgrade = 6,
    HeroJoin = 7
}
