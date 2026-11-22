// Translated from AoWReg.pas.
using System.Globalization;
using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game;

// { ot=1 }
// inherited TRegistry fields
public sealed partial class GameSettings : DelphiRegistry
{
    // AoWReg.pas:128
    public void AssociateMapFiles(string executable)
        /* TODO: Implement Registry.RootKey in the platform or editor service. */
        => throw new NotImplementedException();

    // AoWReg.pas:141
    public void Int32(string key, string name, ref int value, ReadWriteMode mode)
    {
        if (OpenKey($"\\Software\\Triumph Studios\\Age of Wonders\\{key}", create: true))
        {
            switch (mode)
            {
                case ReadWriteMode.Read:
                    if (ValueExists(name))
                    {
                        value = ReadInteger(name);
                    }

                    break;
                case ReadWriteMode.Write:
                    WriteInteger(name, value);
                    break;
                default:
                    break;
            }

            CloseKey();
        }
    }

    // AoWReg.pas:153
    public void Boolean(string key, string name, ref bool value, ReadWriteMode mode)
    {
        if (OpenKey($"\\Software\\Triumph Studios\\Age of Wonders\\{key}", create: true))
        {
            switch (mode)
            {
                case ReadWriteMode.Read:
                    if (ValueExists(name))
                    {
                        value = ReadBool(name);
                    }

                    break;
                case ReadWriteMode.Write:
                    WriteBool(name, value);
                    break;
                default:
                    break;
            }

            CloseKey();
        }
    }

    // AoWReg.pas:165
    public void String(string key, string name, ref string value, ReadWriteMode mode)
    {
        if (OpenKey($"\\Software\\Triumph Studios\\Age of Wonders\\{key}", create: true))
        {
            switch (mode)
            {
                case ReadWriteMode.Read:
                    if (ValueExists(name))
                    {
                        value = ReadString(name);
                    }

                    break;
                case ReadWriteMode.Write:
                    WriteString(name, value);
                    break;
                default:
                    break;
            }

            CloseKey();
        }
    }

    // AoWReg.pas:177
    public void Bytes(string key, string name, Span<byte> value, int size, ReadWriteMode mode)
    {
        if (OpenKey($"\\Software\\Triumph Studios\\Age of Wonders\\{key}", create: true))
        {
            switch (mode)
            {
                case ReadWriteMode.Read:
                    if (ValueExists(name))
                    {
                        _ = ReadBinaryData(name, buffer: value, size);
                    }

                    break;
                case ReadWriteMode.Write:
                    WriteBinaryData(name, buffer: value, size);
                    break;
                default:
                    break;
            }

            CloseKey();
        }
    }

    // AoWReg.pas:227
    public void Rectangle(string key, string name, ref Rect value, ReadWriteMode mode)
    {
        if (OpenKey($"\\Software\\Triumph Studios\\Age of Wonders\\{key}", create: true))
        {
            string text;
            switch (mode)
            {
                case ReadWriteMode.Read:
                    if (ValueExists(name))
                    {
                        text = ReadString(name);
                        value = StringToRectangle(text);
                    }

                    break;
                case ReadWriteMode.Write:
                    text = RectangleToString(value);
                    WriteString(name, text);
                    break;
                default:
                    break;
            }

            CloseKey();
        }
    }

    // AoWReg.pas:250
    public Rect GetRectangle(int index)
    {
        Rect result;
        if (index >= 0)
        {
            result = default;
            Rectangle("General\\rectangles", index.ToString(CultureInfo.InvariantCulture), ref result, mode: ReadWriteMode.Read);
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
            Rectangle("General\\rectangles", index.ToString(CultureInfo.InvariantCulture), ref value, mode: ReadWriteMode.Write);
        }
    }

    // AoWReg.pas:266
    public bool GetBoolean(int index)
    {
        bool result = default;
        // { Original leaves Result uninitialized for a missing nonnegative index. }
        if (index >= 0)
        {
            Boolean("General\\booleans", name: index.ToString(CultureInfo.InvariantCulture), value: ref result, mode: ReadWriteMode.Read);
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
            Boolean("General\\booleans", name: index.ToString(CultureInfo.InvariantCulture), ref value, mode: ReadWriteMode.Write);
        }
    }

    // AoWReg.pas:280
    public string GetString(int index)
    {
        string result = "";
        if (index >= 0)
        {
            String("General\\strings", index.ToString(CultureInfo.InvariantCulture), ref result, mode: ReadWriteMode.Read);
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
            String("General\\strings", index.ToString(CultureInfo.InvariantCulture), ref value, mode: ReadWriteMode.Write);
        }
    }

    // AoWReg.pas:293
    public int GetInteger(int index)
    {
        int result = default;
        // { Original leaves Result uninitialized for a missing nonnegative index. }
        if (index >= 0)
        {
            Int32("General\\integers", index.ToString(CultureInfo.InvariantCulture), ref result, mode: ReadWriteMode.Read);
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
            Int32("General\\integers", index.ToString(CultureInfo.InvariantCulture), ref value, mode: ReadWriteMode.Write);
        }
    }

    // AoWReg.pas:307
    public bool Developer => false;

    // AoWReg.pas:329
    public bool GetSkipIntro()
    {
        bool result = false;
        Boolean("General", name: "Skip Intro", value: ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:335
    public void SetSkipIntro(bool value) => Boolean("General", name: "Skip Intro", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:340
    public int GetDisplayMode()
    {
        int result = 1;
        Int32("Video", "Display Mode", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:346
    public void SetDisplayMode(int value) => Int32("Video", "Display Mode", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:351
    public int GetDisplayWidth()
    {
        int result = 640;
        Int32("Video", "Display Width", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:357
    public void SetDisplayWidth(int value) => Int32("Video", "Display Width", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:362
    public int GetDisplayHeight()
    {
        int result = 480;
        Int32("Video", "Display Height", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:368
    public void SetDisplayHeight(int value) => Int32("Video", "Display Height", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:373
    public bool GetSurfaceInVideoMemory()
    {
        bool result = false;
        Boolean("Video", name: "Surface In VMem", value: ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:379
    public void SetSurfaceInVideoMemory(bool value) => Boolean("Video", name: "Surface In VMem", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:384
    public bool GetAllowResolutionSwitches()
    {
        bool result = false;
        Boolean("Video", name: "Allow Resolution Switches", value: ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:390
    public void SetAllowResolutionSwitches(bool value) => Boolean("Video", name: "Allow Resolution Switches", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:395
    public bool GetDebugMode()
    {
        bool result = false;
        Boolean("General", name: "Debug Mode", value: ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:401
    public void SetDebugMode(bool value) => Boolean("General", name: "Debug Mode", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:406
    public string GetRootDirectory()
    {
        string result = "";
        String("General", "Root Directory", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:412
    public void SetRootDirectory(string value) => String("General", "Root Directory", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:428
    public void SetLanguage(Language value)
    {
        int i = (int)value;
        Int32("General", "Language", ref i, mode: ReadWriteMode.Write);
    }

    // AoWReg.pas:436
    public int GetSoundQuality()
    {
        int result = 3;
        Int32("Sound", "Sound Quality", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:442
    public void SetSoundQuality(int value) => Int32("Sound", "Sound Quality", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:447
    public int GetMusicVolume()
    {
        int result = 50;
        Int32("Sound", "Music Volume", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:453
    public void SetMusicVolume(int value) => Int32("Sound", "Music Volume", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:458
    public int GetSfxVolume()
    {
        int result = 50;
        Int32("Sound", "SFX Volume", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:464
    public void SetSfxVolume(int value) => Int32("Sound", "SFX Volume", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:469
    public Rect GetMainWindowPosition()
    {
        Rect result = default;
        Rectangle("Workspace", "Main Window Position", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:475
    public void SetMainWindowPosition(Rect value) => Rectangle("Workspace", "Main Window Position", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:480
    public WindowState GetMainWindowState()
    {
        int value = 0;
        Int32("Workspace", "Main Window State", ref value, mode: ReadWriteMode.Read);
        return (WindowState)value;
    }

    // AoWReg.pas:489
    public void SetMainWindowState(WindowState value)
    {
        int i = (int)value;
        Int32("Workspace", "Main Window State", ref i, mode: ReadWriteMode.Write);
    }

    // AoWReg.pas:497
    public Rect GetMapWindowPosition()
    {
        Rect result = default;
        Rectangle("Workspace", "Map Window Position", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:503
    public void SetMapWindowPosition(Rect value) => Rectangle("Workspace", "Map Window Position", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:508
    public bool GetShadowsEnabled()
    {
        bool result = true;
        Boolean("Video", name: "Shadows Enabled", value: ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:514
    public void SetShadowsEnabled(bool value) => Boolean("Video", name: "Shadows Enabled", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:519
    public bool GetSfxEnabled()
    {
        bool result = true;
        Boolean("Sound", name: "Sound Effects Enabled", value: ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:525
    public void SetSfxEnabled(bool value) => Boolean("Sound", name: "Sound Effects Enabled", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:530
    public bool GetMusicEnabled()
    {
        bool result = true;
        Boolean("Sound", name: "Music Enabled", value: ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:536
    public void SetMusicEnabled(bool value) => Boolean("Sound", name: "Music Enabled", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:541
    public bool GetUseDirectSound()
    {
        bool result = false;
        Boolean("Sound", name: "Use DirectSound", value: ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:547
    public void SetUseDirectSound(bool value) => Boolean("Sound", name: "Use DirectSound", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:552
    public DetailLevel GetDetailLevel()
    {
        int value = 2;
        Int32("Workspace", "Detail Level", ref value, mode: ReadWriteMode.Read);
        return (DetailLevel)value;
    }

    // AoWReg.pas:561
    public void SetDetailLevel(DetailLevel value)
    {
        int i = (int)value;
        Int32("Workspace", "Detail Level", ref i, mode: ReadWriteMode.Write);
    }

    // AoWReg.pas:569
    public string GetStartupDirectory()
    {
        string result = "";
        String("General", "Startup Directory", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:575
    public void SetStartupDirectory(string value) => String("General", "Startup Directory", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:580
    public bool GetLogTokenEvents()
    {
        bool result = false;
        Boolean("General", name: "Log Token Events", value: ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:586
    public void SetLogTokenEvents(bool value) => Boolean("General", name: "Log Token Events", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:591
    public bool GetAutoSave()
    {
        bool result = true;
        Boolean("General", name: "Auto Save", value: ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:597
    public void SetAutoSave(bool value) => Boolean("General", name: "Auto Save", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:602
    public bool GetAutoDebugSave()
    {
        bool result = false;
        Boolean("General", name: "Auto Debug Save", value: ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:608
    public void SetAutoDebugSave(bool value) => Boolean("General", name: "Auto Debug Save", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:613
    public bool GetFastMovementEnabled()
    {
        bool result = false;
        Boolean("General", name: "Fast Movement Enabled", value: ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:619
    public void SetFastMovementEnabled(bool value) => Boolean("General", name: "Fast Movement Enabled", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:646
    public CombatResolveMode GetCombatResolveMode()
    {
        CombatResolveMode result = CombatResolveMode.Choose;
        Bytes("General", "Combat Resolve Mode", Rtl.Bytes.Of(ref result), size: 1, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:652
    public void SetCombatResolveMode(CombatResolveMode value) => Bytes("General", "Combat Resolve Mode", Rtl.Bytes.Of(ref value), size: 1, mode: ReadWriteMode.Write);

    // AoWReg.pas:657
    public MovementSpeed GetMovementSpeed()
    {
        MovementSpeed result = MovementSpeed.Normal;
        Bytes("General", "Movement Speed", Rtl.Bytes.Of(ref result), size: 1, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:663
    public void SetMovementSpeed(MovementSpeed value) => Bytes("General", "Movement Speed", Rtl.Bytes.Of(ref value), size: 1, mode: ReadWriteMode.Write);

    // AoWReg.pas:668
    public string GetLocalEmailAddress()
    {
        string result = "";
        String("Email", "Local Email Address", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:674
    public void SetLocalEmailAddress(string value) => String("Email", "Local Email address", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:679
    public string GetSmtpServer()
    {
        string result = "";
        String("Email", "SMTP Server", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:685
    public void SetSmtpServer(string value) => String("Email", "SMTP Server", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:690
    public string GetPop3Server()
    {
        string result = "";
        String("Email", "POP3 Server", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:696
    public void SetPop3Server(string value) => String("Email", "POP3 Server", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:710
    public string GetPop3UserName()
    {
        string result = "";
        String("Email", "POP3 UserName", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:716
    public void SetPop3UserName(string value) => String("Email", "POP3 UserName", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:721
    public string GetPop3Password()
    {
        string result = "";
        String("Email", "POP3 Password", ref result, mode: ReadWriteMode.Read);
        result = TransformPassword(result);
        return result;
    }

    // AoWReg.pas:728
    public void SetPop3Password(string value)
    {
        value = TransformPassword(value);
        String("Email", "POP3 Password", ref value, mode: ReadWriteMode.Write);
    }

    // AoWReg.pas:734
    public string GetAttachmentDirectory()
    {
        string result = "";
        String("Email", "Attachment Directory", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:740
    public void SetAttachmentDirectory(string value)
    {
        if (value.Length > 0 && value[^1] != 92)
        {
            value += "\\";
        }

        String("Email", "Attachment Directory", ref value, mode: ReadWriteMode.Write);
    }

    // AoWReg.pas:747
    public int GetCheckEmailFrequency()
    {
        int result = 1;
        Int32("Email", "Check Email Frequency", ref result, mode: ReadWriteMode.Read);
        return result;
    }

    // AoWReg.pas:753
    public void SetCheckEmailFrequency(int value) => Int32("Email", "Check Email Frequency", ref value, mode: ReadWriteMode.Write);

    // AoWReg.pas:758
    public int GetEmailGamesCrc()
    {
        string s = "";
        String("General", "Serial number", ref s, mode: ReadWriteMode.Read);
        try
        {
            int p = Pos("-", haystack: s);
            // { Original tests against -1 although Pos returns 0 when absent. }
            if (p != -1)
            {
                s = Copy(s, index: p + 1, count: s.Length - p);
                p = Pos("-", haystack: s);
                if (p != -1)
                {
                    s = Copy(s, index: 1, count: p - 1);
                    if (!IsEmpty(s))
                    {
                        return HexToInt(s);
                    }
                }
            }
        }
        catch
        {
            return 0;
        }

        return 0;
    }

    // AoWReg.pas:783
    public void SetEmailGamesCrc(int value)
    {
        string s = $"4G5F6JHH-{unchecked((uint)value).ToString("X8", provider: CultureInfo.InvariantCulture)}-6AG66554-98LK0FG3";
        String("General", "Serial number", ref s, mode: ReadWriteMode.Write);
    }

    // AoWReg.pas:791
    public void RegisterDPlayLobbyServer()
        /* TODO: Implement Registry.RootKey in the platform or editor service. */
        => throw new NotImplementedException();

    // Automatic and English retain the shipped forcing behavior.
    public Language GetLanguage()
    {
        int value = 0;
        Int32("General", "Language", ref value, mode: ReadWriteMode.Read);
        return value is >= 2 and <= 5 ? (Language)value : Language.English;
    }

    private static Rect StringToRectangle(string S)
    {
        Rect result = default;
        int number = Pos(",", haystack: S);
        int number2 = 0;
        while (!IsEmpty(S) && number2 != 4)
        {
            string text;
            if (number != 0)
            {
                text = Copy(S, index: 1, count: number - 1);
                S = Delete(S, index: 1, count: number);
            }
            else
            {
                text = S;
                S = "";
            }
            int number3 = (!IsEmpty(text)) ? StrToInt(text) : 0;
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
                default:
                    break;
            }
            number = Pos(",", haystack: S);
            number2++;
        }
        return result;
    }

    private static string RectangleToString(Rect R)
    {
        return string.Concat(new string[7]
        {
            R.Left.ToString(CultureInfo.InvariantCulture),
            ",",
            R.Top.ToString(CultureInfo.InvariantCulture),
            ",",
            R.Right.ToString(CultureInfo.InvariantCulture),
            ",",
            R.Bottom.ToString(CultureInfo.InvariantCulture)
        });
    }

    private static string TransformPassword(string S)
    {
        string text = "";
        for (int i = 1; i <= S.Length; i++)
        {
            text += Cp1252.Text((byte)(((S.Length + i + 5) & 0xFF) ^ Cp1252.Byte(S[i - 1])));
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
            int b = Cp1252.Byte(S[number2 - i - 1]);
            int number3 = (b is not (>= 48 and <= 57)) ? ((b is not (>= 65 and <= 70)) ? (b is >= 97 and <= 102 ? (b - 97 + 10) : 0) : (b - 65 + 10)) : (b - 48);
            number |= number3 << (i * 4);
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
