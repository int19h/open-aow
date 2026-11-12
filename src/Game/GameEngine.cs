// Translated from AoWE.pas.
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Events;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Game.World.Terrain;

using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game;

// { Interface declaration run owned by P2. }
// Version, Registry, GameMap, GameSet, CombatFont, FDebugMode, AnimationDetail, Demo,
// Campaign, EmailGames, Dictionary, Language
public partial class GameEngine : ENode, IChildList<GameEngineChild>
{
    public new const int ClassId = 131073;

    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only the kinds of GameEngineChild, so the read never fails.
    public GameEngineChild this[int index] => GameEngineChild.Of(GetChild(index));

    protected sealed override bool AcceptsChild(EngineObject child) => child is EControlStyleNode or GamePack or GameMap;

    int IChildSink<GameEngineChild>.AddChild(GameEngineChild child) => AddChild(child.Node);

    public int Version { get; private set; }
    public GameSettings Registry { get; set; }

    private StrategicMap _gameMap;

    public GamePack GameSet { get; private set; }
    public ImageLibraryFont CombatFont { get; set; }
    public bool Debug { get; set; }
    public byte AnimationDetail { get; private set; }
    public bool Demo { get; private set; }
    public Campaign Campaign { get; private set; }
    public EmailGameControl EmailGames { get; private set; }
    public AoWDictionary Dictionary { get; set; }
    public byte Language { get; private set; }

    // Army.PAS:5767-5786. A bare constructor leaves the flag clear and skips this body.
    private protected sealed override void InitializeBeforeInherited()
    {
        if (!s_fullConstruction) return;
        GameTables.GameEngine = this;
        SetVersion(1, 36, 53);
        Registry = new GameSettings();
        User = Set<EngineUserElement>.Of(EngineUserElement.Programmer, EngineUserElement.Designer, EngineUserElement.User);
        InitializeMovePoints();
        Debug = Registry.GetDebugMode();
        AnimationDetail = (byte)Registry.GetDetailLevel();
        EmailGames = new EmailGameControl(null);
        Dictionary = new AoWDictionary(null);
        Dictionary.SetDictionaryName("ResourceStringDictionary");
        // { Original selects English for every explicit language here. }
        switch (Registry.GetLanguage())
        {
            case GameSettings.Language.Auto:
                Dictionary.SetLanguage(-1);
                break;
            case GameSettings.Language.English:
                Dictionary.SetLanguage(1);
                break;
            case GameSettings.Language.German:
                Dictionary.SetLanguage(1);
                break;
            case GameSettings.Language.French:
                Dictionary.SetLanguage(1);
                break;
            case GameSettings.Language.Italian:
                Dictionary.SetLanguage(1);
                break;
            case GameSettings.Language.Spanish:
                Dictionary.SetLanguage(1);
                break;
        }

        Demo = false;
        Language = (byte)Registry.GetLanguage();
    }

    // Army.PAS:5748. A bare engine has no dictionary, so it returns the text unchanged.
    public virtual string TranslateText(string text) => Dictionary is null ? text : TranslateResourceString(text);

    // Army.PAS:5955
    public virtual void CreateHexSpriteSet()
    {
        _hexSpriteSet = GamePack.Create(this);
        GameSet = _hexSpriteSet;
        GamePack.Current = GameSet;
    }

    // Army.PAS:5973. The HSEngine body assigns the map, then the game body records an AoW map.
    public virtual void SetHexSpriteMap(GameMap value)
    {
        _hexSpriteMap = value;
        if (value is StrategicMap)
        {
            _gameMap = (StrategicMap)_hexSpriteMap;
            StrategicMap.Current = (StrategicMap)value;
        }
    }

    // Army.PAS:5962. The HSEngine body closes the map, then the game body clears the current map.
    public virtual void CloseHsm(Aow.Engine.ProgressEvent progress)
    {
        try
        {
            if (_hexSpriteMap != null)
            {
                _hexSpriteMap.OnProgress = progress;
                OnHsmCloseStore.Raise(_hexSpriteMap);
                _hexSpriteMap?.Dispose();
                SetHexSpriteMap(null);
            }

            _gameMap = null;
            StrategicMap.Current = null;
        }
        catch
        {
            throw new InvalidOperationException("exception occurred during CloseHSM");
        }
    }

    // Army.PAS:5648
    public void TranslateEngineObject(EngineObject objectValue, IvDictionary dictionary)
    {
        TranslateTextStream stream;
        stream = new TranslateTextStream(this);
        stream.FDictionary = dictionary;
        try
        {
            stream.Object(objectValue, 0);
        }
        finally
        {
            stream?.Dispose();
        }
    }

    // Army.PAS:5660
    public void TranslateEngineObjectFromFile(EngineObject objectValue, string fileName)
    {
        AoWDictionary dictionary;
        if (FileExists(fileName))
        {
            dictionary = new AoWDictionary(null);
            try
            {
                dictionary.FileName = fileName;
                dictionary.Open();
                switch (Registry.GetLanguage())
                {
                    case GameSettings.Language.Auto:
                        dictionary.SetLanguage(-1);
                        break;
                    case GameSettings.Language.English:
                        dictionary.SetLanguage(1);
                        break;
                    case GameSettings.Language.German:
                        dictionary.SetLanguage(2);
                        break;
                    case GameSettings.Language.French:
                        dictionary.SetLanguage(3);
                        break;
                    case GameSettings.Language.Italian:
                        dictionary.SetLanguage(4);
                        break;
                    case GameSettings.Language.Spanish:
                        dictionary.SetLanguage(5);
                        break;
                }

                TranslateEngineObject(objectValue, dictionary);
                dictionary.Close();
            }
            finally
            {
                dictionary?.Dispose();
            }
        }
    }

    // Army.PAS:5741
    public string TranslateResourceString(string S)
    {
        string result;
        if (Dictionary.Language > 0)
        {
            result = Dictionary.Translate(S);
        }
        else
        {
            if (CompareText(S, s_undeadPluralText) == 0)
            {
                result = s_undeadText;
            }
            else
            {
                result = S;
            }
        }

        return result;
    }

    // Army.PAS:5915
    public int GetMajorVersion() => ((int)(((uint)Version) >> 24)) & 255;

    // Army.PAS:5918
    public int GetMinorVersion()
    {
        int result = ((int)(((uint)Version) >> 16)) & 255;
        if (result > 99)
        {
            result = 99;
        }

        return result;
    }

    // Army.PAS:5924
    public int GetBuildNumber()
    {
        int result = Version & 65535;
        if (result > 9999)
        {
            result = 9999;
        }

        return result;
    }

    // Army.PAS:5930
    public void SetVersion(int major, int minor, int build)
    {
        if (major < 0)
        {
            major = 0;
        }

        if (major > 99)
        {
            major = 99;
        }

        if (minor < 0)
        {
            minor = 0;
        }

        if (minor > 99)
        {
            minor = 99;
        }

        if (build < 0)
        {
            build = 0;
        }

        if (build > 9999)
        {
            build = 9999;
        }

        Version = (major << 24) + (minor << 16) + build;
    }

    // Army.PAS:5941
    public string GetVersionString()
    {
        int version;
        string result = IntToStr(GetMajorVersion()) + ".";
        version = GetMinorVersion();
        if (version < 10)
        {
            result = result + "0" + IntToStr(version) + ".";
        }
        else
        {
            result = result + IntToStr(version) + ".";
        }

        version = GetBuildNumber();
        if (version < 1000)
        {
            result = result + "0";
        }

        if (version < 100)
        {
            result = result + "0";
        }

        if (version < 10)
        {
            result = result + "0";
        }

        return result + IntToStr(version);
    }

    // Army.PAS:5983
    public bool RegisteredVersion()
    {
        bool result = default;
        bool registered = default;
        MemoryStream memory;
        Stream fileStream;
        string name = "";
        try
        {
            memory = new MemoryStream();
            // { Original leaks Mem if opening or loading the file raises. }
            fileStream = OpenFile(GameTables.GameEngine.StartupDirectory + "RELEASE\\GENERAL.PFS", 0 | 32);
            try
            {
                memory.LoadFromStream(fileStream);
            }
            finally
            {
                fileStream?.Dispose();
            }

            try
            {
                memory.Position = 0;
                memory.Read(Bytes.Of(ref registered, 1));
                StringFromStream(ref name, memory);
                result = registered;
            }
            finally
            {
                memory?.Dispose();
            }
        }
        catch
        {
            result = true;
        }

        return result;
    }

    // Army.PAS:6008
    public string RegisteredTo()
    {
        string result = "";
        bool registered = default;
        MemoryStream memory;
        string name = "";
        try
        {
            memory = new MemoryStream();
            try
            {
                memory.LoadFromFile(GameTables.GameEngine.StartupDirectory + "RELEASE\\GENERAL.PFS");
                memory.Read(Bytes.Of(ref registered, 1));
                StringFromStream(ref name, memory);
                result = AbilityRules.Encode(name, 0);
            }
            finally
            {
                memory?.Dispose();
            }
        }
        catch
        {
            result = "";
        }

        return result;
    }

    // Army.PAS:6026
    public bool RequestAccess(string name, string code)
    {
        string RegistrationCode(string text)
        {
            int value;
            string hex;
            string result = "";
            if (text.Length >= 4)
            {
                value = 0;
                for (int I = 1; I <= text.Length; I++)
                {
                    value += Cp1252.Byte(text[I - 1]);
                }

                for (int I = 1; I <= 4; I++)
                {
                    hex = IntToHex(Cp1252.Byte(text[I - 1]) * value + I, 6);
                    result = result + hex[hex.Length - 1].ToString();
                    value += Cp1252.Byte(hex[hex.Length - 1]) + Cp1252.Byte(hex[0]);
                }
            }

            return result;
        }

        return RegisteredTo() == name && !IsEmpty(name) && code == RegistrationCode(name);
    }

    // Army.PAS:6050
    public void CloseCampaign()
    {
        if (Campaign != null)
        {
            Campaign?.Dispose();
            Campaign = null;
        }
    }

    // Army.PAS:6059
    public void LoadCampaignMap(Aow.Engine.ProgressEvent progress)
    {
        if (Campaign != null && Campaign.Stream != null)
        {
            CloseHsm(null);
            GameTables.GameEngine.LoadHsmFromStream(Campaign.Stream, false, progress);
        }
    }

    // Army.PAS:6068
    // The preflight raises before the current campaign closes (own-formats.md 2.7).
    public void LoadCampaign(string fileName)
    {
        LoadCampaign(fileName, FileBoundary.LoadOrRaise(fileName, GameProfile.Campaign));
    }

    // Loads a campaign file whose preflight has passed, from its engine image.
    public void LoadCampaign(string fileName, byte[] image)
    {
        CloseCampaign();
        Campaign = (Campaign)RetrieveFromImage(image, null);
        TranslateEngineObjectFromFile(Campaign, GameTables.GameEngine.StartupDirectory + "Dict\\" + "Campaign" + ".mld");
        Campaign.FileName = fileName;
    }

    // Army.PAS:6076
    public void SaveCampaign(string fileName)
    {
        if (Campaign != null)
        {
            Campaign.Stream.Clear();
            if (_gameMap != null)
            {
                SaveHsmToStream(Campaign.Stream, null);
            }

            StoreInFile(fileName, Campaign, null, GameProfile.Campaign);
        }
    }

    // Army.PAS:6086
    public void NewCampaign(int classId)
    {
        CloseCampaign();
        Campaign = (Campaign)CreateEngineObject(classId, null);
    }

    // intf_P2_08.inc:29. The flag is set before construction and cleared when it returns.
    public static GameEngine Create(ECustomNode owner)
    {
        s_fullConstruction = true;
        try
        {
            return ConstructionContext.Create(owner, static (context, owner) => new GameEngine(context, owner));
        }
        finally
        {
            s_fullConstruction = false;
        }
    }

    private static ReadOnlySpan<byte> MoveBits => [1, 4, 2, 8, 16, 32, 64, 128];

    private static readonly string s_undeadPluralText = "Undead(plural)";

    private static readonly string s_undeadText = "Undead";

    private static readonly ImmutableArray<ImmutableArray<ImmutableArray<sbyte>>> s_movementTemplates = [[[-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, -1, -1, -1, 3, 3, -1, 3, -1, -1, -1, 8, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, -1, -1, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 6, -1, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, -1, -1, -1, -1, -1, -1, 3, -1, -1, -1, 8, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]], [[4, -1, -1, -1, 4, -1, -1, 4, 4, -1, 4, -1, 8, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, -1, -1, 4, -1, -1, 4, 4, -1, 4, -1, 8, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, -1, -1, 4, -1, -1, 4, 4, -1, 4, -1, 8, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]], [[4, -1, -1, -1, 4, -1, 4, 4, 4, 4, 4, -1, 4, 4, -1, -1], [4, 8, 4, 4, 4, 4, -1, 4, 4, 4, 4, -1, 4, 4, -1, -1], [4, 8, 4, 4, 4, 4, -1, 4, 4, 4, 4, -1, 4, 4, -1, -1], [4, 8, 4, 4, 4, 4, -1, 4, 4, 4, 4, -1, 4, 4, -1, -1], [4, 8, 4, 4, 4, 4, -1, 4, 4, 4, 4, -1, 4, 4, -1, -1], [4, 8, 4, 4, 4, 4, -1, 4, 4, 4, 4, -1, 4, 4, -1, -1], [4, -1, -1, -1, -1, 4, 4, -1, 4, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, -1, -1, 4, -1, 4, 4, 4, -1, 4, -1, 4, 4, -1, -1], [4, -1, -1, -1, 4, 4, 4, 4, 4, -1, 4, -1, 4, 4, -1, -1], [4, -1, -1, -1, 4, 4, -1, 4, 4, -1, 4, -1, 4, 4, -1, -1], [4, -1, 4, -1, 4, 4, -1, 4, 4, -1, 4, -1, 4, 4, -1, -1], [4, -1, -1, -1, -1, -1, -1, -1, 4, -1, -1, -1, -1, -1, -1, -1], [4, -1, -1, -1, 4, -1, 4, 4, 4, -1, 4, -1, 4, 4, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]], [[-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, 4, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 4, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 4, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 4, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 4, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, -1, -1, -1, 3, 3, -1, 3, -1, -1, -1, 8, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, -1, -1, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 4, -1, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, -1, -1, -1, -1, -1, -1, 3, -1, -1, -1, 8, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]], [[-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, -1, -1, -1, 3, 3, -1, 3, -1, -1, -1, 8, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, -1, -1, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1], [3, -1, 6, -1, 3, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, -1, -1, -1, -1, -1, -1, 3, -1, -1, -1, 8, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]], [[-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, 8, 6, 4, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1], [4, 8, 6, 4, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1], [4, 8, 6, 4, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1], [4, 8, 6, 4, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1], [4, 8, 6, 4, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, -1, -1, -1, 3, 3, -1, 3, -1, -1, -1, 8, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, -1, -1, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, 6, -1, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1], [4, -1, -1, -1, -1, -1, -1, -1, 3, -1, -1, -1, 8, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]], [[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]], [[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]];

    internal static int MagicDebugChannel { get; set; }

    private static void UnregisterEnhancementAbilities(AbilityControl control)
    {
        control.UnregisterAbilityId(120);
        control.UnregisterAbilityId(121);
        control.UnregisterAbilityId(122);
        control.UnregisterAbilityId(123);
        control.UnregisterAbilityId(124);
        control.UnregisterAbilityId(86);
        control.UnregisterAbilityId(87);
        control.UnregisterAbilityId(88);
        control.UnregisterAbilityId(89);
        control.UnregisterAbilityId(90);
        control.UnregisterAbilityId(24);
        control.UnregisterAbilityId(25);
        control.UnregisterAbilityId(30);
        control.UnregisterAbilityId(31);
        control.UnregisterAbilityId(44);
        control.UnregisterAbilityId(45);
        control.UnregisterAbilityId(55);
        control.UnregisterAbilityId(57);
        control.UnregisterAbilityId(58);
        control.UnregisterAbilityId(61);
        control.UnregisterAbilityId(62);
    }

    private static void UnregisterTouchAbilities(AbilityControl control)
    {
        control.UnregisterAbilityId(21);
        control.UnregisterAbilityId(118);
        control.UnregisterAbilityId(111);
        control.UnregisterAbilityId(112);
        control.UnregisterAbilityId(113);
        control.UnregisterAbilityId(115);
        control.UnregisterAbilityId(116);
        control.UnregisterAbilityId(119);
        control.UnregisterAbilityId(14);
        control.UnregisterAbilityId(15);
        control.UnregisterAbilityId(16);
        control.UnregisterAbilityId(17);
        control.UnregisterAbilityId(18);
        control.UnregisterAbilityId(19);
        control.UnregisterAbilityId(20);
    }

    private static void UnregisterCoreSpells(GameEngine engine)
    {
        SpellControl control;
        control = engine.GameSet.SpellControl;
        engine.UnregisterEngineClasses([EngineClass.Of<SpellTokenEvent>()]);
        engine.UnregisterEngineClasses([EngineClass.Of<SpellCastEventLog>()]);
        engine.UnregisterEngineClasses([EngineClass.Of<UnitSpellTokenEvent>(), EngineClass.Of<UnitSpellCombatAction>()]);
        control.UnregisterAll();
    }

    private static void UnregisterCoreAbilities(AbilityControl control)
    {
        GameTables.GameEngine.UnregisterEngineClasses([EngineClass.Of<CosmeticSurgeryEnchantment>(), EngineClass.Of<LeadershipAbilityData>()]);
        GameTables.GameEngine.UnregisterEngineClasses([EngineClass.Of<DurationAbilityData>(), EngineClass.Of<MultiLevelAbilityData>(), EngineClass.Of<RangedAttackCombatAction>(), EngineClass.Of<SelfDestructCombatAction>(), EngineClass.Of<StrikeCombatAction>(), EngineClass.Of<DispelMagicAbilityCombatAction>()]);
        GameTables.GameEngine.UnregisterEngineClasses([EngineClass.Of<UnitEnchantmentAbilityData>(), EngineClass.Of<UnitEnchantment>(), EngineClass.Of<DispelMagicAbilityTokenEvent>()]);
        GameTables.GameEngine.UnregisterEngineClasses([EngineClass.Of<CommandCombatAction>(), EngineClass.Of<CommandedAbilityData>(), EngineClass.Of<CommandAbilityData>(), EngineClass.Of<HealingTokenEvent>(), EngineClass.Of<DispelMagicAbilityData>()]);
        GameTables.GameEngine.UnregisterEngineClasses([EngineClass.Of<PossessCombatAction>(), EngineClass.Of<WebCombatAction>(), EngineClass.Of<HealingCombatAction>(), EngineClass.Of<EntangleCombatAction>(), EngineClass.Of<HealingAbilityData>(), EngineClass.Of<InvokeDeathCombatAction>()]);
        control.UnregisterAbilityId(86);
        control.UnregisterAbilityId(87);
        control.UnregisterAbilityId(88);
        control.UnregisterAbilityId(89);
        control.UnregisterAbilityId(90);
        control.UnregisterAbilityId(114);
        control.UnregisterAbilityId(40);
        control.UnregisterAbilityId(27);
        control.UnregisterAbilityId(65);
        control.UnregisterAbilityId(148);
        control.UnregisterAbilityId(149);
        control.UnregisterAbilityId(28);
        control.UnregisterAbilityId(150);
        control.UnregisterAbilityId(29);
        control.UnregisterAbilityId(48);
        control.UnregisterAbilityId(22);
        control.UnregisterAbilityId(47);
        control.UnregisterAbilityId(46);
        control.UnregisterAbilityId(52);
        control.UnregisterAbilityId(38);
        control.UnregisterAbilityId(35);
        control.UnregisterAbilityId(146);
        control.UnregisterAbilityId(147);
        control.UnregisterAbilityId(125);
        control.UnregisterAbilityId(60);
        control.UnregisterAbilityId(117);
        control.UnregisterAbilityId(50);
        control.UnregisterAbilityId(106);
        control.UnregisterAbilityId(107);
        UnregisterEnhancementAbilities(control);
        UnregisterTouchAbilities(control);
    }

    private static readonly string s_registerLifeStealingText = "Life Stealing";

    private static readonly string s_registerChargeText = "Charge";

    private static readonly string s_registerDragonSlayingText = "Dragon Slaying";

    private static readonly string s_registerParryText = "Parry";

    private static readonly string s_registerExtraStrikeText = "Extra Strike";

    private static readonly string s_registerFirstStrikeText = "First Strike";

    private static readonly string s_registerEntangleStrikeText = "Entangle Strike";

    private static readonly string s_registerFireStrikeText = "Fire Strike";

    private static readonly string s_registerColdStrikeText = "Cold Strike";

    private static readonly string s_registerLightningStrikeText = "Lightning Strike";

    private static readonly string s_registerPoisonStrikeText = "Poison Strike";

    private static readonly string s_registerDeathStrikeText = "Death Strike";

    private static readonly string s_registerHolyStrikeText = "Holy Strike";

    private static readonly string s_registerMagicStrikeText = "Magic Strike";

    private static readonly string s_registerBlackBoltsText = "Black Bolts";

    private static readonly string s_registerMagicBoltsText = "Magic Bolts";

    private static readonly string s_registerFrostBoltsText = "Frost Bolts";

    private static readonly string s_registerLightningBoltsText = "Lightning Bolts";

    private static readonly string s_registerHolyBoltsText = "Holy Bolts";

    private static readonly string s_registerFireBreathText = "Fire Breath";

    private static readonly string s_registerColdBreathText = "Cold Breath";

    private static readonly string s_registerBlackBreathText = "Black Breath";

    private static readonly string s_registerDivineBreathText = "Divine Breath";

    private static readonly string s_registerGasBreathText = "Gas Breath";

    private static readonly string s_registerShootJavelinText = "Shoot Javelin";

    private static readonly string s_registerShootBlackJavelinText = "Shoot Black Javelin";

    private static readonly string s_registerHurlStonesText = "Hurl Stones";

    private static readonly string s_registerFireMusketText = "Fire Musket";

    private static readonly string s_registerFireCannonText = "Fire Cannon";

    private static readonly string s_registerHurlBoulderText = "Hurl Boulder";

    private static readonly string s_registerVenomousSpitText = "Venomous Spit";

    private static readonly string s_registerCallFlamesText = "Call Flames";

    private static readonly string s_registerArcheryText = "Archery";

    private static readonly string s_registerPoisonDartsText = "Poison Darts";

    private static readonly string s_registerDoomGazeText = "Doom Gaze";

    private static RangedAttackAbility CreateRangedAttack(int id, string name, sbyte attack, sbyte damage, sbyte repeats, RangedAttackAbility.CombatRange range, Set<DamageType> damageTypes, int enchantment)
    {
        RangedAttackAbility ability;
        ability = new RangedAttackAbility(null);
        ability.AbilityId = id;
        ability.AbilityName = name;
        ability.RangeRangedAttack = range;
        ability.DamageTypesRangedAttack.Value = damageTypes;
        ability.AttackRangedAttack = attack;
        ability.DamageRangedAttack = damage;
        ability.AttackRepeatRangedAttack = repeats;
        // { Original ignores Enchantment. }
        return ability;
    }

    private static BoltsAbility CreateBolts(int id, string name, int attack, sbyte damage, DamageType damageType, int enchantment)
    {
        BoltsAbility ability;
        ability = new BoltsAbility(null);
        ability.AbilityId = id;
        ability.AbilityName = name;
        ability.DamageTypesRangedAttack.Value = Set<DamageType>.Of(damageType);
        ability.DamageRangedAttack = damage;
        ability.AttackRangedAttack = (sbyte)attack;
        // { Original ignores Enchantment. }
        return ability;
    }

    private static BreathAbility CreateBreath(int id, string name, DamageType damageType, int enchantment)
    {
        BreathAbility ability;
        ability = new BreathAbility(null);
        ability.AbilityId = id;
        ability.AbilityName = name;
        ability.DamageTypesRangedAttack.Value = Set<DamageType>.Of(damageType);
        // { Original ignores Enchantment. }
        return ability;
    }

    private static StrikeEnhancementAbility CreateStrikeEnhancement(int id, string name, Set<AbilitySelectionType> selection)
    {
        StrikeEnhancementAbility ability;
        ability = new StrikeEnhancementAbility(null);
        ability.SelectionTypes = selection;
        ability.AbilityId = id;
        ability.AbilityName = name;
        return ability;
    }

    private static void RegisterRangedGroup1(AbilityControl control)
    {
        control.RegisterAbility(CreateBolts(120, AbilityRules.TranslateResourceString(s_registerBlackBoltsText), 6, 3, DamageType.Death, 126));
        control.RegisterAbility(CreateBolts(121, AbilityRules.TranslateResourceString(s_registerMagicBoltsText), 7, 3, DamageType.Magic, 124));
        control.RegisterAbility(CreateBolts(122, AbilityRules.TranslateResourceString(s_registerFrostBoltsText), 6, 3, DamageType.Cold, 125));
        control.RegisterAbility(CreateBolts(123, AbilityRules.TranslateResourceString(s_registerLightningBoltsText), 6, 3, DamageType.Lightning, -1));
        control.RegisterAbility(CreateBolts(124, AbilityRules.TranslateResourceString(s_registerHolyBoltsText), 6, 3, DamageType.Holy, 125));
        control.RegisterAbility(CreateBreath(86, AbilityRules.TranslateResourceString(s_registerFireBreathText), DamageType.Fire, -1));
        control.RegisterAbility(CreateBreath(87, AbilityRules.TranslateResourceString(s_registerColdBreathText), DamageType.Cold, -1));
    }

    private static void RegisterRangedGroup2(AbilityControl control)
    {
        control.RegisterAbility(CreateBreath(88, AbilityRules.TranslateResourceString(s_registerBlackBreathText), DamageType.Death, -1));
        control.RegisterAbility(CreateBreath(89, AbilityRules.TranslateResourceString(s_registerDivineBreathText), DamageType.Holy, -1));
        control.RegisterAbility(CreateBreath(90, AbilityRules.TranslateResourceString(s_registerGasBreathText), DamageType.Poison, -1));
        control.RegisterAbility(new FlameThrowingAbility(null));
        control.RegisterAbility(CreateRangedAttack(57, AbilityRules.TranslateResourceString(s_registerShootJavelinText), 5, 5, 2, RangedAttackAbility.CombatRange.Long, Set<DamageType>.Of(DamageType.Physical), 122));
        control.RegisterAbility(CreateRangedAttack(58, AbilityRules.TranslateResourceString(s_registerShootBlackJavelinText), 5, 5, 2, RangedAttackAbility.CombatRange.Long, Set<DamageType>.Of(DamageType.Death, DamageType.Physical), 123));
        control.RegisterAbility(CreateRangedAttack(25, AbilityRules.TranslateResourceString(s_registerHurlStonesText), 3, 1, 4, RangedAttackAbility.CombatRange.Medium, Set<DamageType>.Of(DamageType.Physical), 111));
    }

    private static void RegisterRangedGroup3(AbilityControl control)
    {
        control.RegisterAbility(CreateRangedAttack(45, AbilityRules.TranslateResourceString(s_registerFireMusketText), 5, 7, 1, RangedAttackAbility.CombatRange.Long, Set<DamageType>.Of(DamageType.Physical), 113));
        control.RegisterAbility(CreateRangedAttack(44, AbilityRules.TranslateResourceString(s_registerFireCannonText), 5, 8, 1, RangedAttackAbility.CombatRange.Long, Set<DamageType>.Of(DamageType.Physical, DamageType.Wall), 112));
        control.RegisterAbility(CreateRangedAttack(24, AbilityRules.TranslateResourceString(s_registerHurlBoulderText), 3, 7, 1, RangedAttackAbility.CombatRange.Long, Set<DamageType>.Of(DamageType.Physical, DamageType.Wall), 110));
        control.RegisterAbility(CreateRangedAttack(62, AbilityRules.TranslateResourceString(s_registerVenomousSpitText), 5, 4, 1, RangedAttackAbility.CombatRange.Short, Set<DamageType>.Of(DamageType.Poison), -1));
        control.RegisterAbility(CreateRangedAttack(31, AbilityRules.TranslateResourceString(s_registerCallFlamesText), 5, 4, 1, RangedAttackAbility.CombatRange.Medium, Set<DamageType>.Of(DamageType.Fire), -1));
        control.RegisterAbility(CreateRangedAttack(22, AbilityRules.TranslateResourceString(s_registerArcheryText), 4, 2, 2, RangedAttackAbility.CombatRange.Medium, Set<DamageType>.Of(DamageType.Physical), -1));
    }

    private static void RegisterRangedGroup4(AbilityControl control)
    {
        control.RegisterAbility(CreateRangedAttack(61, AbilityRules.TranslateResourceString(s_registerPoisonDartsText), 4, 1, 3, RangedAttackAbility.CombatRange.Medium, Set<DamageType>.Of(DamageType.Poison, DamageType.Physical), 114));
        control.RegisterAbility(CreateRangedAttack(55, AbilityRules.TranslateResourceString(s_registerDoomGazeText), 6, 5, 1, RangedAttackAbility.CombatRange.Long, Set<DamageType>.Of(DamageType.Death), -1));
    }

    private static void RegisterStrikeGroup1(AbilityControl control)
    {
        control.RegisterAbility(new StrikeAbility(null));
        control.RegisterAbility(CreateStrikeEnhancement(118, AbilityRules.TranslateResourceString(s_registerLifeStealingText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem, AbilitySelectionType.Editor)));
        control.RegisterAbility(CreateStrikeEnhancement(111, AbilityRules.TranslateResourceString(s_registerChargeText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.Editor)));
        control.RegisterAbility(CreateStrikeEnhancement(112, AbilityRules.TranslateResourceString(s_registerDragonSlayingText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem, AbilitySelectionType.Editor)));
        control.RegisterAbility(CreateStrikeEnhancement(113, AbilityRules.TranslateResourceString(s_registerParryText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.Editor)));
        control.RegisterAbility(CreateStrikeEnhancement(115, AbilityRules.TranslateResourceString(s_registerExtraStrikeText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem, AbilitySelectionType.Editor)));
        control.RegisterAbility(CreateStrikeEnhancement(116, AbilityRules.TranslateResourceString(s_registerFirstStrikeText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem, AbilitySelectionType.Editor)));
    }

    private static void RegisterStrikeGroup2(AbilityControl control)
    {
        control.RegisterAbility(CreateStrikeEnhancement(119, AbilityRules.TranslateResourceString(s_registerEntangleStrikeText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit)));
        control.RegisterAbility(CreateStrikeEnhancement(14, AbilityRules.TranslateResourceString(s_registerFireStrikeText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
        control.RegisterAbility(CreateStrikeEnhancement(15, AbilityRules.TranslateResourceString(s_registerColdStrikeText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
        control.RegisterAbility(CreateStrikeEnhancement(16, AbilityRules.TranslateResourceString(s_registerLightningStrikeText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
        control.RegisterAbility(CreateStrikeEnhancement(17, AbilityRules.TranslateResourceString(s_registerPoisonStrikeText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
        control.RegisterAbility(CreateStrikeEnhancement(18, AbilityRules.TranslateResourceString(s_registerDeathStrikeText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
    }

    private static void RegisterStrikeGroup3(AbilityControl control)
    {
        control.RegisterAbility(CreateStrikeEnhancement(19, AbilityRules.TranslateResourceString(s_registerHolyStrikeText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
        control.RegisterAbility(CreateStrikeEnhancement(20, AbilityRules.TranslateResourceString(s_registerMagicStrikeText), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
    }

    private static void RegisterEnhancements(AbilityControl control)
    {
        RegisterRangedGroup1(control);
        RegisterRangedGroup2(control);
        RegisterRangedGroup3(control);
        // Original ranged-attack registration
        RegisterRangedGroup4(control);
    }

    private static void RegisterTouches(AbilityControl control)
    {
        RegisterStrikeGroup1(control);
        RegisterStrikeGroup2(control);
        // Original strike registration
        RegisterStrikeGroup3(control);
    }

    private static void InitializeMovePoints()
    {
        int points;
        FillBytes(MemoryMarshal.AsBytes(MovementTables.MovePointTables.AsSpan()), 255);
        for (int mask = 0; mask <= 255; mask++)
        {
            for (int overlay = -1; overlay <= 11; overlay++)
            {
                for (int terrain = 0; terrain <= 15; terrain++)
                {
                    points = 127;
                    for (int moveType = 0; moveType <= 7; moveType++)
                    {
                        if ((mask & MoveBits[moveType]) != 0 && s_movementTemplates[moveType][terrain][overlay - MovementTables.FirstOverlay] != -1 && s_movementTemplates[moveType][terrain][overlay - MovementTables.FirstOverlay] < points)
                        {
                            points = s_movementTemplates[moveType][terrain][overlay - MovementTables.FirstOverlay];
                        }
                    }

                    if (points != 127)
                    {
                        MovementTables.MovePointTables[mask][terrain][overlay - MovementTables.FirstOverlay] = (sbyte)points;
                    }
                }
            }
        }
    }

    private static void RegisterGroupControls()
    {
    }

    private static void RegisterMoveEvents(GameEngine engine)
    {
        engine.RegisterEngineClasses([EngineClass.Of<EndTurnTokenEvent>(), EngineClass.Of<GameOverTokenEvent>(), EngineClass.Of<MultiMoveTokenEvent>(), EngineClass.Of<CheatTokenEvent>(), EngineClass.Of<DisbandUnitTokenEvent>(), EngineClass.Of<ChangePlayerSettingsTokenEvent>(), EngineClass.Of<CombatFlowControlTokenEvent>(), EngineClass.Of<TerminatePlayerTokenEvent>()]);
    }

    private static void RegisterAbilities(AbilityControl control)
    {
        GameTables.GameEngine.RegisterEngineClasses([EngineClass.Of<CosmeticSurgeryEnchantment>(), EngineClass.Of<LeadershipAbilityData>(), EngineClass.Of<DispelMagicAbilityCombatAction>()]);
        GameTables.GameEngine.RegisterEngineClasses([EngineClass.Of<DurationAbilityData>(), EngineClass.Of<MultiLevelAbilityData>(), EngineClass.Of<RangedAttackCombatAction>(), EngineClass.Of<SelfDestructCombatAction>(), EngineClass.Of<StrikeCombatAction>()]);
        GameTables.GameEngine.RegisterEngineClasses([EngineClass.Of<UnitEnchantmentAbilityData>(), EngineClass.Of<UnitEnchantment>(), EngineClass.Of<DispelMagicAbilityTokenEvent>(), EngineClass.Of<PossessedAbilityData>()]);
        GameTables.GameEngine.RegisterEngineClasses([EngineClass.Of<CommandCombatAction>(), EngineClass.Of<CommandedAbilityData>(), EngineClass.Of<CommandAbilityData>(), EngineClass.Of<HealingTokenEvent>(), EngineClass.Of<DispelMagicAbilityData>()]);
        GameTables.GameEngine.RegisterEngineClasses([EngineClass.Of<PossessCombatAction>(), EngineClass.Of<WebCombatAction>(), EngineClass.Of<HealingCombatAction>(), EngineClass.Of<EntangleCombatAction>(), EngineClass.Of<HealingAbilityData>(), EngineClass.Of<TurnUndeadCombatAction>(), EngineClass.Of<InvokeDeathCombatAction>()]);
        control.RegisterAbility(new LeadershipAbility(null));
        control.RegisterAbility(new EntangleAbility(null));
        control.RegisterAbility(new WebAbility(null));
        control.RegisterAbility(new SelfDestructAbility(null));
        control.RegisterAbility(new RoundAttackAbility(null));
        control.RegisterAbility(new CharmAbility(null));
        control.RegisterAbility(new CharmedAbility(null));
        control.RegisterAbility(new DominateAbility(null));
        control.RegisterAbility(new DominatedAbility(null));
        control.RegisterAbility(new SeduceAbility(null));
        control.RegisterAbility(new SeducedAbility(null));
        control.RegisterAbility(new HealingAbility(null));
        control.RegisterAbility(new SpellCastingAbility(null));
        control.RegisterAbility(new TurnUndeadAbility(null));
        control.RegisterAbility(new InvokeDeathAbility(null));
        control.RegisterAbility(new ConstructAbility(null));
        control.RegisterAbility(new DispelMagicAbility(null));
        control.RegisterAbility(new WallCrushingAbility(null));
        control.RegisterAbility(new TransportAbility(null));
        control.RegisterAbility(new PossessAbility(null));
        control.RegisterAbility(new PossessedAbility(null));
        RegisterEnhancements(control);
        RegisterTouches(control);
    }

    private static void RegisterSpells(GameEngine engine)
    {
        engine.RegisterEngineClasses([EngineClass.Of<SpellTokenEvent>()]);
        engine.RegisterEngineClasses([EngineClass.Of<SpellCastEventLog>()]);
        engine.RegisterEngineClasses([EngineClass.Of<UnitSpellTokenEvent>(), EngineClass.Of<UnitSpellCombatAction>()]);
    }

    internal static void StringFromStream(ref string S, Stream stream)
    {
        int length = default;
        stream.ReadExactly(Bytes.Of(ref length, 4));
        if (length < 0) throw new ArgumentOutOfRangeException(paramName: null, "Negative string length");
        byte[] buffer = new byte[length];
        Cp1252.Encode(S)[..Math.Min(S.Length, length)].CopyTo(buffer);
        // A failed read leaves the resized value in the destination.
        S = Cp1252.Decode(buffer);
        int transferred = stream.Read(buffer);
        if (transferred != buffer.Length)
        {
            S = Cp1252.Decode(buffer);
            return;
        }

        S = Cp1252.Decode(buffer);
    }

    private static void RegisterEventLog(GameEngine engine)
    {
        engine.RegisterEngineClasses([EngineClass.Of<TextEventLog>(), EngineClass.Of<EndTurnEventLog>(), EngineClass.Of<GameOverEventLog>(), EngineClass.Of<LocationTextEventLog>()]);
    }

    private static void RegisterNetworkEngine(GameEngine engine)
    {
        NetworkSession.NetworkEngineDebugChannel = engine.RegisterDebugId("Network Engine");
        engine.RegisterEngineClasses([EngineClass.Of<TokenPackage>(), EngineClass.Of<PassTokenTokenEvent>(), EngineClass.Of<SyncTokenEvent>()]);
    }

    private static void RegisterSoundEngineClasses(GameEngine engine)
    {
        engine.RegisterEngineClasses([EngineClass.Of<SfxSequence>(), EngineClass.Of<SfxLibrary>(), EngineClass.Of<Sfx>()]);
    }

    internal static void UnregisterCitySpells(GameEngine engine)
    {
        engine.UnregisterEngineClasses([EngineClass.Of<CityEnchantmentSpellTokenEvent>(), EngineClass.Of<CityTargetSpellTokenEvent>(), EngineClass.Of<TownGateTokenEvent>()]);
        var spellControl = engine.GameSet.SpellControl;
        spellControl.UnregisterSpellId(72);
        // { Original leaves Town Quake ($47) registered. }
        spellControl.UnregisterSpellId(38);
        spellControl.UnregisterSpellId(52);
        spellControl.UnregisterSpellId(53);
        spellControl.UnregisterSpellId(54);
        spellControl.UnregisterSpellId(58);
        spellControl.UnregisterSpellId(76);
    }

    internal static void UnregisterCombatSpells(GameEngine engine)
    {
        SpellControl control;
        control = engine.GameSet.SpellControl;
        engine.UnregisterEngineClasses([EngineClass.Of<CombatSpellCombatAction>(), EngineClass.Of<FastBattleTerrorCombatAction>(), EngineClass.Of<TacticalBattleTerrorCombatAction>(), EngineClass.Of<RecallSpiritCombatAction>(), EngineClass.Of<AnimateDeadCombatAction>(), EngineClass.Of<FastBattleHighPrayerCombatAction>(), EngineClass.Of<TacticalBattleHighPrayerCombatAction>(), EngineClass.Of<EntangleSpellCombatAction>(), EngineClass.Of<MindDecayCombatAction>(), EngineClass.Of<TurnUndeadSpellCombatAction>(), EngineClass.Of<SlowCombatAction>()]);
        control.UnregisterSpellId(100);
        control.UnregisterSpellId(101);
        control.UnregisterSpellId(102);
        control.UnregisterSpellId(103);
        control.UnregisterSpellId(104);
        control.UnregisterSpellId(105);
        control.UnregisterSpellId(106);
        control.UnregisterSpellId(107);
        control.UnregisterSpellId(108);
        control.UnregisterSpellId(110);
        control.UnregisterSpellId(111);
        control.UnregisterSpellId(112);
        control.UnregisterSpellId(113);
        control.UnregisterSpellId(114);
        control.UnregisterSpellId(115);
        control.UnregisterSpellId(116);
        control.UnregisterSpellId(117);
        control.UnregisterSpellId(118);
        control.UnregisterSpellId(119);
        control.UnregisterSpellId(120);
        control.UnregisterSpellId(121);
        control.UnregisterSpellId(122);
        control.UnregisterSpellId(123);
        control.UnregisterSpellId(124);
        control.UnregisterSpellId(125);
        control.UnregisterSpellId(130);
        control.UnregisterSpellId(129);
        control.UnregisterSpellId(126);
        control.UnregisterSpellId(127);
        control.UnregisterSpellId(128);
    }

    internal static void UnregisterGlobalSpells(GameEngine engine)
    {
        SpellControl control;
        control = engine.GameSet.SpellControl;
        engine.UnregisterEngineClasses([EngineClass.Of<WarpPartyTokenEvent>(), EngineClass.Of<DisjunctionTokenEvent>()]);
        control.UnregisterSpellId(1);
        control.UnregisterSpellId(2);
        control.UnregisterSpellId(34);
        control.UnregisterSpellId(51);
        control.UnregisterSpellId(66);
        control.UnregisterSpellId(67);
        control.UnregisterSpellId(69);
        control.UnregisterSpellId(75);
    }

    internal static void UnregisterGlobalEnchantmentSpells(GameEngine engine)
    {
        SpellControl control;
        control = engine.GameSet.SpellControl;
        control.UnregisterSpellId(44);
        control.UnregisterSpellId(46);
        control.UnregisterSpellId(47);
        control.UnregisterSpellId(48);
        control.UnregisterSpellId(50);
        control.UnregisterSpellId(60);
        control.UnregisterSpellId(61);
        control.UnregisterSpellId(62);
        control.UnregisterSpellId(63);
        control.UnregisterSpellId(64);
        control.UnregisterSpellId(45);
        control.UnregisterSpellId(65);
        engine.UnregisterEngineClasses([EngineClass.Of<EnchantedRoadsEnchantment>(), EngineClass.Of<GlobalEnchantmentSpellTokenEvent>(), EngineClass.Of<GlobalEnchantmentEventLog>()]);
        engine.UnregisterEngineClasses([EngineClass.Of<WaterMasteryEnchantment>(), EngineClass.Of<HatredEnchantment>(), EngineClass.Of<HarmonyEnchantment>(), EngineClass.Of<PowerLeakEnchantment>(), EngineClass.Of<FloodEnchantment>(), EngineClass.Of<AirMasteryEnchantment>()]);
        engine.UnregisterEngineClasses([EngineClass.Of<LifeMasteryEnchantment>(), EngineClass.Of<DeathMasteryEnchantment>(), EngineClass.Of<FireMasteryEnchantment>(), EngineClass.Of<EarthMasteryEnchantment>(), EngineClass.Of<SpellWardEnchantment>()]);
    }

    internal static void UnregisterGlobalTargetSpells(GameEngine engine)
    {
        SpellControl control;
        control = engine.GameSet.SpellControl;
        control.UnregisterSpellId(10);
        control.UnregisterSpellId(11);
        control.UnregisterSpellId(18);
        control.UnregisterSpellId(41);
        control.UnregisterSpellId(57);
        control.UnregisterSpellId(68);
        control.UnregisterSpellId(70);
        control.UnregisterSpellId(73);
        control.UnregisterSpellId(74);
        engine.UnregisterEngineClasses([EngineClass.Of<FreezeWaterTokenEvent>(), EngineClass.Of<FrozenWaterHexSprite>(), EngineClass.Of<FireBarrierTokenEvent>(), EngineClass.Of<LevelTerrainTokenEvent>(), EngineClass.Of<RejuvenateTokenEvent>(), EngineClass.Of<WatcherTokenEvent>(), EngineClass.Of<WatcherHexSprite>(), EngineClass.Of<TornadoTokenEvent>(), EngineClass.Of<VortexTokenEvent>(), EngineClass.Of<HealingShowersTokenEvent>(), EngineClass.Of<GlobalTargetSpellTokenEvent>()]);
    }

    internal static void UnregisterSummonSpells(GameEngine engine)
    {
        SpellControl control;
        control = engine.GameSet.SpellControl;
        engine.UnregisterEngineClasses([EngineClass.Of<SummonSpellTokenEvent>()]);
        control.UnregisterSpellId(24);
        control.UnregisterSpellId(22);
        control.UnregisterSpellId(31);
        control.UnregisterSpellId(27);
        control.UnregisterSpellId(28);
        control.UnregisterSpellId(29);
        control.UnregisterSpellId(30);
        control.UnregisterSpellId(25);
        control.UnregisterSpellId(26);
        control.UnregisterSpellId(23);
        control.UnregisterSpellId(21);
        control.UnregisterSpellId(20);
        control.UnregisterSpellId(78);
    }

    internal static void UnregisterUnitSpells(GameEngine engine)
    {
        SpellControl control;
        control = engine.GameSet.SpellControl;
        control.UnregisterSpellId(8);
        control.UnregisterSpellId(15);
        control.UnregisterSpellId(17);
        control.UnregisterSpellId(16);
        control.UnregisterSpellId(33);
        control.UnregisterSpellId(32);
        control.UnregisterSpellId(35);
        control.UnregisterSpellId(37);
        control.UnregisterSpellId(36);
        control.UnregisterSpellId(39);
        control.UnregisterSpellId(40);
        control.UnregisterSpellId(42);
        control.UnregisterSpellId(43);
        control.UnregisterSpellId(49);
        control.UnregisterSpellId(55);
        control.UnregisterSpellId(56);
        control.UnregisterSpellId(59);
        control.UnregisterSpellId(77);
        engine.UnregisterEngineClasses([EngineClass.Of<DispelMagicSpellCombatAction>()]);
    }

    internal static void UnregisterPoisonPlant(GameEngine engine)
    {
        engine.UnregisterEngineClasses([EngineClass.Of<PoisonPlant>(), EngineClass.Of<PoisonPlant.Resource>(), EngineClass.Of<PoisonPlantSpellTokenEvent>()]);
        engine.GameSet.SpellControl.UnregisterSpellId(12);
    }

    internal static void UnregisterUnholyGround(GameEngine engine)
    {
        engine.UnregisterEngineClasses([EngineClass.Of<UnholyGround>(), EngineClass.Of<UnholyGround.Resource>(), EngineClass.Of<UnholyGroundSpellTokenEvent>()]);
        engine.GameSet.SpellControl.UnregisterSpellId(14);
    }

}
