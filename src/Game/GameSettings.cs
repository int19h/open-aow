// Translated from AoWReg.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game;

// { ot=1 }
// inherited TRegistry fields
public partial class GameSettings : Registry
{
    // AoWReg.pas:128
    public void AssociateMapFiles(string executable)
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
                        Value = StringToRectangle(S);
                    }

                    break;
                case ReadWriteMode.Write:
                    S = RectangleToString(Value);
                    WriteString(Name, S);
                    break;
            }

            CloseKey();
        }
    }

    // AoWReg.pas:250
    public Rect GetRectangle(int index)
    {
        Rect result = default;
        if (index >= 0)
        {
            result = default;
            rwRect("General\\rectangles", IntToStr(index), ref result, ReadWriteMode.Read);
        }
        else
        {
            result = default;
        }

        return result;
    }

    // AoWReg.pas:260
    public void SetRectangle(int index, Rect value)
    {
        if (index >= 0)
        {
            rwRect("General\\rectangles", IntToStr(index), ref value, ReadWriteMode.Write);
        }
    }

    // AoWReg.pas:266
    public bool GetBoolean(int index)
    {
        bool result = default;
        // { Original leaves Result uninitialized for a missing nonnegative index. }
        if (index >= 0)
        {
            rwBool("General\\booleans", IntToStr(index), ref result, ReadWriteMode.Read);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWReg.pas:274
    public void SetBoolean(int index, bool value)
    {
        if (index >= 0)
        {
            rwBool("General\\booleans", IntToStr(index), ref value, ReadWriteMode.Write);
        }
    }

    // AoWReg.pas:280
    public string GetString(int index)
    {
        string result = "";
        if (index >= 0)
        {
            rwString("General\\strings", IntToStr(index), ref result, ReadWriteMode.Read);
        }
        else
        {
            result = "";
        }

        return result;
    }

    // AoWReg.pas:287
    public void SetString(int index, string value)
    {
        if (index >= 0)
        {
            rwString("General\\strings", IntToStr(index), ref value, ReadWriteMode.Write);
        }
    }

    // AoWReg.pas:293
    public int GetInteger(int index)
    {
        int result = default;
        // { Original leaves Result uninitialized for a missing nonnegative index. }
        if (index >= 0)
        {
            rwInteger("General\\integers", IntToStr(index), ref result, ReadWriteMode.Read);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWReg.pas:301
    public void SetInteger(int index, int value)
    {
        if (index >= 0)
        {
            rwInteger("General\\integers", IntToStr(index), ref value, ReadWriteMode.Write);
        }
    }

    // AoWReg.pas:307
    public bool GetDeveloper() => false;

    // AoWReg.pas:312
    public void SetDeveloper(bool value)
    {
        /* TODO: Implement Registry.DeleteValue in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AoWReg.pas:329
    public bool GetSkipIntro()
    {
        bool result;
        result = false;
        rwBool("General", "Skip Intro", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:335
    public void SetSkipIntro(bool value)
    {
        rwBool("General", "Skip Intro", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:340
    public int GetDisplayMode()
    {
        int result;
        result = 1;
        rwInteger("Video", "Display Mode", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:346
    public void SetDisplayMode(int value)
    {
        rwInteger("Video", "Display Mode", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:351
    public int GetDisplayWidth()
    {
        int result;
        result = 640;
        rwInteger("Video", "Display Width", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:357
    public void SetDisplayWidth(int value)
    {
        rwInteger("Video", "Display Width", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:362
    public int GetDisplayHeight()
    {
        int result;
        result = 480;
        rwInteger("Video", "Display Height", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:368
    public void SetDisplayHeight(int value)
    {
        rwInteger("Video", "Display Height", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:373
    public bool GetSurfaceInVideoMemory()
    {
        bool result;
        result = false;
        rwBool("Video", "Surface In VMem", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:379
    public void SetSurfaceInVideoMemory(bool value)
    {
        rwBool("Video", "Surface In VMem", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:384
    public bool GetAllowResolutionSwitches()
    {
        bool result;
        result = false;
        rwBool("Video", "Allow Resolution Switches", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:390
    public void SetAllowResolutionSwitches(bool value)
    {
        rwBool("Video", "Allow Resolution Switches", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:395
    public bool GetDebugMode()
    {
        bool result;
        result = false;
        rwBool("General", "Debug Mode", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:401
    public void SetDebugMode(bool value)
    {
        rwBool("General", "Debug Mode", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:406
    public string GetRootDirectory()
    {
        string result;
        result = "";
        rwString("General", "Root Directory", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:412
    public void SetRootDirectory(string value)
    {
        rwString("General", "Root Directory", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:428
    public void SetLanguage(Language value)
    {
        int I;
        I = (int)value;
        rwInteger("General", "Language", ref I, ReadWriteMode.Write);
    }

    // AoWReg.pas:436
    public int GetSoundQuality()
    {
        int result;
        result = 3;
        rwInteger("Sound", "Sound Quality", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:442
    public void SetSoundQuality(int value)
    {
        rwInteger("Sound", "Sound Quality", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:447
    public int GetMusicVolume()
    {
        int result;
        result = 50;
        rwInteger("Sound", "Music Volume", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:453
    public void SetMusicVolume(int value)
    {
        rwInteger("Sound", "Music Volume", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:458
    public int GetSfxVolume()
    {
        int result;
        result = 50;
        rwInteger("Sound", "SFX Volume", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:464
    public void SetSfxVolume(int value)
    {
        rwInteger("Sound", "SFX Volume", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:469
    public Rect GetMainWindowPosition()
    {
        Rect result;
        result = default;
        rwRect("Workspace", "Main Window Position", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:475
    public void SetMainWindowPosition(Rect value)
    {
        rwRect("Workspace", "Main Window Position", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:480
    public WindowState GetMainWindowState()
    {
        int value;
        value = 0;
        rwInteger("Workspace", "Main Window State", ref value, ReadWriteMode.Read);
        return (WindowState)value;
    }

    // AoWReg.pas:489
    public void SetMainWindowState(WindowState value)
    {
        int I;
        I = (int)value;
        rwInteger("Workspace", "Main Window State", ref I, ReadWriteMode.Write);
    }

    // AoWReg.pas:497
    public Rect GetMapWindowPosition()
    {
        Rect result;
        result = default;
        rwRect("Workspace", "Map Window Position", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:503
    public void SetMapWindowPosition(Rect value)
    {
        rwRect("Workspace", "Map Window Position", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:508
    public bool GetShadowsEnabled()
    {
        bool result;
        result = true;
        rwBool("Video", "Shadows Enabled", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:514
    public void SetShadowsEnabled(bool value)
    {
        rwBool("Video", "Shadows Enabled", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:519
    public bool GetSfxEnabled()
    {
        bool result;
        result = true;
        rwBool("Sound", "Sound Effects Enabled", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:525
    public void SetSfxEnabled(bool value)
    {
        rwBool("Sound", "Sound Effects Enabled", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:530
    public bool GetMusicEnabled()
    {
        bool result;
        result = true;
        rwBool("Sound", "Music Enabled", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:536
    public void SetMusicEnabled(bool value)
    {
        rwBool("Sound", "Music Enabled", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:541
    public bool GetUseDirectSound()
    {
        bool result;
        result = false;
        rwBool("Sound", "Use DirectSound", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:547
    public void SetUseDirectSound(bool value)
    {
        rwBool("Sound", "Use DirectSound", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:552
    public DetailLevel GetDetailLevel()
    {
        int value;
        value = 2;
        rwInteger("Workspace", "Detail Level", ref value, ReadWriteMode.Read);
        return (DetailLevel)value;
    }

    // AoWReg.pas:561
    public void SetDetailLevel(DetailLevel value)
    {
        int I;
        I = (int)value;
        rwInteger("Workspace", "Detail Level", ref I, ReadWriteMode.Write);
    }

    // AoWReg.pas:569
    public string GetStartupDirectory()
    {
        string result;
        result = "";
        rwString("General", "Startup Directory", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:575
    public void SetStartupDirectory(string value)
    {
        rwString("General", "Startup Directory", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:580
    public bool GetLogTokenEvents()
    {
        bool result;
        result = false;
        rwBool("General", "Log Token Events", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:586
    public void SetLogTokenEvents(bool value)
    {
        rwBool("General", "Log Token Events", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:591
    public bool GetAutoSave()
    {
        bool result;
        result = true;
        rwBool("General", "Auto Save", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:597
    public void SetAutoSave(bool value)
    {
        rwBool("General", "Auto Save", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:602
    public bool GetAutoDebugSave()
    {
        bool result;
        result = false;
        rwBool("General", "Auto Debug Save", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:608
    public void SetAutoDebugSave(bool value)
    {
        rwBool("General", "Auto Debug Save", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:613
    public bool GetFastMovementEnabled()
    {
        bool result;
        result = false;
        rwBool("General", "Fast Movement Enabled", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:619
    public void SetFastMovementEnabled(bool value)
    {
        rwBool("General", "Fast Movement Enabled", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:646
    public CombatResolveMode GetCombatResolveMode()
    {
        CombatResolveMode result;
        result = CombatResolveMode.Choose;
        rwBinaryData("General", "Combat Resolve Mode", Bytes.Of(ref result), 1, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:652
    public void SetCombatResolveMode(CombatResolveMode value)
    {
        rwBinaryData("General", "Combat Resolve Mode", Bytes.Of(ref value), 1, ReadWriteMode.Write);
    }

    // AoWReg.pas:657
    public MovementSpeed GetMovementSpeed()
    {
        MovementSpeed result;
        result = MovementSpeed.Normal;
        rwBinaryData("General", "Movement Speed", Bytes.Of(ref result), 1, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:663
    public void SetMovementSpeed(MovementSpeed value)
    {
        rwBinaryData("General", "Movement Speed", Bytes.Of(ref value), 1, ReadWriteMode.Write);
    }

    // AoWReg.pas:668
    public string GetLocalEmailAddress()
    {
        string result;
        result = "";
        rwString("Email", "Local Email Address", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:674
    public void SetLocalEmailAddress(string value)
    {
        rwString("Email", "Local Email address", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:679
    public string GetSmtpServer()
    {
        string result;
        result = "";
        rwString("Email", "SMTP Server", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:685
    public void SetSmtpServer(string value)
    {
        rwString("Email", "SMTP Server", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:690
    public string GetPop3Server()
    {
        string result;
        result = "";
        rwString("Email", "POP3 Server", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:696
    public void SetPop3Server(string value)
    {
        rwString("Email", "POP3 Server", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:710
    public string GetPop3UserName()
    {
        string result;
        result = "";
        rwString("Email", "POP3 UserName", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:716
    public void SetPop3UserName(string value)
    {
        rwString("Email", "POP3 UserName", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:721
    public string GetPop3Password()
    {
        string result;
        result = "";
        rwString("Email", "POP3 Password", ref result, ReadWriteMode.Read);
        result = TransformPassword(result, true);
        return result;
    }

    // AoWReg.pas:728
    public void SetPop3Password(string value)
    {
        value = TransformPassword(value, true);
        rwString("Email", "POP3 Password", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:734
    public string GetAttachmentDirectory()
    {
        string result;
        result = "";
        rwString("Email", "Attachment Directory", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:740
    public void SetAttachmentDirectory(string value)
    {
        if (value.Length > 0 && value[value.Length - 1] != 92)
        {
            value = value + "\\";
        }

        rwString("Email", "Attachment Directory", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:747
    public int GetCheckEmailFrequency()
    {
        int result;
        result = 1;
        rwInteger("Email", "Check Email Frequency", ref result, ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:753
    public void SetCheckEmailFrequency(int value)
    {
        rwInteger("Email", "Check Email Frequency", ref value, ReadWriteMode.Write);
    }

    // AoWReg.pas:758
    public int GetEmailGamesCrc()
    {
        string S = "";
        int P;
        int result = 0;
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
                        result = HexToInt(S);
                    }
                }
            }
        }
        catch
        {
            result = 0;
        }

        return result;
    }

    // AoWReg.pas:783
    public void SetEmailGamesCrc(int value)
    {
        string S;
        S = "4G5F6JHH-" + IntToHex(value, 8) + "-6AG66554-98LK0FG3";
        rwString("General", "Serial number", ref S, ReadWriteMode.Write);
    }

    // AoWReg.pas:791
    public void RegisterDPlayLobbyServer()
    {
        /* TODO: Implement Registry.RootKey in the platform or editor service. */
        throw new NotImplementedException();
    }

    // Automatic and English retain the shipped forcing behavior.
    public Language GetLanguage()
    {
        int value = 0;
        rwInteger("General", "Language", ref value, ReadWriteMode.Read);
        return value is >= 2 and <= 5 ? (Language)value : Language.English;
    }

    private static Rect StringToRectangle(string S)
    {
        Rect result = default(Rect);
        int number = RtlUnit.Pos(",", S);
        int number2 = 0;
        while (!RtlUnit.IsEmpty(S) && number2 != 4)
        {
            string text;
            if (number != 0)
            {
                text = RtlUnit.Copy(S, 1, number - 1);
                S = RtlUnit.Delete(S, 1, number);
            }
            else
            {
                text = S;
                S = "";
            }
            int number3 = ((!RtlUnit.IsEmpty(text)) ? RtlUnit.StrToInt(text) : 0);
            switch (number2)
            {
                case 0:
                    result.Left = number3;
                    break;
                case 1:
                    result.Top = number3;
                    break;
                case 2:
                    result.Right = number3;
                    break;
                case 3:
                    result.Bottom = number3;
                    break;
            }
            number = RtlUnit.Pos(",", S);
            number2++;
        }
        return result;
    }

    private static string RectangleToString(Rect R)
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

    private static string TransformPassword(string S, bool mode)
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
        int number = 0;
        int number2 = S.Length;
        if (number2 > 8)
        {
            number2 = 8;
        }
        for (int i = 0; i <= number2 - 1; i++)
        {
            byte b = Cp1252.Byte(S[number2 - i - 1]);
            int number3 = ((!Set<byte>.WithBounds(0, 255).WithRange((byte)48, (byte)57).Contains(b)) ? ((!Set<byte>.WithBounds(0, 255).WithRange((byte)65, (byte)70).Contains(b)) ? (Set<byte>.WithBounds(0, 255).WithRange((byte)97, (byte)102).Contains(b) ? (b - 97 + 10) : 0) : (b - 65 + 10)) : (b - 48));
            number |= number3 << i * 4;
        }
        return number;
    }

    public enum ReadWriteMode : byte
    {
        Read = 0, Write = 1
    }

    public enum CombatResolveMode : byte
    {
        Choose = 0, AlwaysFastBattle = 1, AlwaysTacticalBattle = 2
    }

    public enum MovementSpeed : byte
    {
        Normal = 0, Fast = 1, Turbo = 2
    }

    public enum Language : byte
    {
        Auto = 0, English = 1, German = 2, French = 3, Spanish = 4, Italian = 5
    }
}
