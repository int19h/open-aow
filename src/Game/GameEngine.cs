// Translated from AoWE.pas.
using System.Runtime.InteropServices;
using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

using Aow.Game.Ai.Groups;
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
// FVersion, FRegistry, FGameMap, FGameSet, FCombatFont, FDebugMode, FAnimationDetail, FDemo,
// FCampaign, FEmailGames, FDictionary, FLanguage
public partial class GameEngine : ENode
{
    public new const int ClassID = 131073;

    public int FVersion;
    public AoWRegistry FRegistry;
    public AoWHSMap FGameMap;
    public AoWHSSet FGameSet;
    public ImageLibraryFont FCombatFont;
    public bool FDebug;
    public byte FAnimationDetail;
    public bool FDemo;
    public AoWCampaign FCampaign;
    public EmailGameControl FEmailGames;
    public AoWDictionary FDictionary;
    public byte FLanguage;

    // Army.PAS:5767-5786. A bare constructor leaves the flag clear and skips this body.
    private protected sealed override void InitializeBeforeInherited()
    {
        if (!fullConstruction) return;
        GameTables.AoWEngine = this;
        SetVersion(1, 36, 53);
        FRegistry = new AoWRegistry();
        FUser = Set<EngineUserElement>.Of(EngineUserElement.Programmer, EngineUserElement.Designer, EngineUserElement.User);
        P2InitializeMovePoints();
        FDebug = FRegistry.GetDebugMode();
        FAnimationDetail = (byte)FRegistry.GetDetailLevel();
        FEmailGames = new EmailGameControl(null);
        FDictionary = new AoWDictionary(null);
        FDictionary.SetDictionaryName("ResourceStringDictionary");
        // { Original selects English for every explicit language here. }
        switch (FRegistry.GetLanguage())
        {
            case AoWRegistry.AoWLanguage.Auto:
                FDictionary.SetLanguage(-1);
                break;
            case AoWRegistry.AoWLanguage.English:
                FDictionary.SetLanguage(1);
                break;
            case AoWRegistry.AoWLanguage.German:
                FDictionary.SetLanguage(1);
                break;
            case AoWRegistry.AoWLanguage.French:
                FDictionary.SetLanguage(1);
                break;
            case AoWRegistry.AoWLanguage.Italian:
                FDictionary.SetLanguage(1);
                break;
            case AoWRegistry.AoWLanguage.Spanish:
                FDictionary.SetLanguage(1);
                break;
        }

        FDemo = false;
        FLanguage = (byte)FRegistry.GetLanguage();
    }

    // Army.PAS:5748. A bare engine has no dictionary, so it returns the text unchanged.
    public virtual string TranslateText(string Text) => FDictionary is null ? Text : TranslateRStr(Text);

    // Army.PAS:5955
    public virtual void CreateHSSet()
    {
        FHSSet = AoWHSSet.Create(this);
        FGameSet = (AoWHSSet)FHSSet;
        AoWHSSet.Current = FGameSet;
    }

    // Army.PAS:5973. The HSEngine body assigns the map, then the game body records an AoW map.
    public virtual void SetHSMap(HSMap Value)
    {
        FHSMap = Value;
        if (Value is AoWHSMap)
        {
            FGameMap = (AoWHSMap)FHSMap;
            AoWHSMap.Current = (AoWHSMap)Value;
        }
    }

    // Army.PAS:5962. The HSEngine body closes the map, then the game body clears the current map.
    public virtual void CloseHSM(Aow.Engine.ProgressEvent AProgress)
    {
        try
        {
            if (FHSMap != null)
            {
                FHSMap.FOnProgress = AProgress;
                FOnHSMCloseStore.Raise(FHSMap);
                FHSMap?.Dispose();
                SetHSMap(null);
            }

            FGameMap = null;
            AoWHSMap.Current = null;
        }
        catch
        {
            throw new InvalidOperationException("exception occurred during CloseHSM");
        }
    }

    // Army.PAS:5648
    public void TranslateEObject(EObject Obj, IvDictionary Dictionary)
    {
        TranslateTextStream Stream;
        Stream = new TranslateTextStream(this);
        Stream.FDictionary = Dictionary;
        try
        {
            Stream.rwEObject(Obj, 0);
        }
        finally
        {
            Stream?.Dispose();
        }
    }

    // Army.PAS:5660
    public void TranslateEObjectFromFile(EObject Obj, string FileName)
    {
        AoWDictionary Dictionary;
        if (FileExists(FileName))
        {
            Dictionary = new AoWDictionary(null);
            try
            {
                Dictionary.FFileName = FileName;
                Dictionary.Open();
                switch (FRegistry.GetLanguage())
                {
                    case AoWRegistry.AoWLanguage.Auto:
                        Dictionary.SetLanguage(-1);
                        break;
                    case AoWRegistry.AoWLanguage.English:
                        Dictionary.SetLanguage(1);
                        break;
                    case AoWRegistry.AoWLanguage.German:
                        Dictionary.SetLanguage(2);
                        break;
                    case AoWRegistry.AoWLanguage.French:
                        Dictionary.SetLanguage(3);
                        break;
                    case AoWRegistry.AoWLanguage.Italian:
                        Dictionary.SetLanguage(4);
                        break;
                    case AoWRegistry.AoWLanguage.Spanish:
                        Dictionary.SetLanguage(5);
                        break;
                }

                TranslateEObject(Obj, Dictionary);
                Dictionary.Close();
            }
            finally
            {
                Dictionary?.Dispose();
            }
        }
    }

    // Army.PAS:5725
    public void ExportTextToFile(string FileName, EObject Obj)
    {
        ExportTextStream Stream;
        Stream FileStream;
        FileStream = OpenFile(FileName, 65535);
        try
        {
            Stream = new ExportTextStream(FileStream, this);
            try
            {
                Stream.rwEObject(Obj, 0);
            }
            finally
            {
                Stream?.Dispose();
            }
        }
        finally
        {
            FileStream?.Dispose();
        }
    }

    // Army.PAS:5741
    public string TranslateRStr(string S)
    {
        string Result;
        if (FDictionary.FLanguage > 0)
        {
            Result = FDictionary.Translate(S);
        }
        else
        {
            if (CompareText(S, P2UndeadPluralRStr) == 0)
            {
                Result = P2UndeadRStr;
            }
            else
            {
                Result = S;
            }
        }

        return Result;
    }

    // Army.PAS:5915
    public int GetMajorVersion() => ((int)(((uint)FVersion) >> 24)) & 255;

    // Army.PAS:5918
    public int GetMinorVersion()
    {
        int Result = ((int)(((uint)FVersion) >> 16)) & 255;
        if (Result > 99)
        {
            Result = 99;
        }

        return Result;
    }

    // Army.PAS:5924
    public int GetBuildNumber()
    {
        int Result = FVersion & 65535;
        if (Result > 9999)
        {
            Result = 9999;
        }

        return Result;
    }

    // Army.PAS:5930
    public void SetVersion(int Major, int Minor, int Build)
    {
        if (Major < 0)
        {
            Major = 0;
        }

        if (Major > 99)
        {
            Major = 99;
        }

        if (Minor < 0)
        {
            Minor = 0;
        }

        if (Minor > 99)
        {
            Minor = 99;
        }

        if (Build < 0)
        {
            Build = 0;
        }

        if (Build > 9999)
        {
            Build = 9999;
        }

        FVersion = (Major << 24) + (Minor << 16) + Build;
    }

    // Army.PAS:5941
    public string GetVersionStr()
    {
        int Version;
        string Result = IntToStr(GetMajorVersion()) + ".";
        Version = GetMinorVersion();
        if (Version < 10)
        {
            Result = Result + "0" + IntToStr(Version) + ".";
        }
        else
        {
            Result = Result + IntToStr(Version) + ".";
        }

        Version = GetBuildNumber();
        if (Version < 1000)
        {
            Result = Result + "0";
        }

        if (Version < 100)
        {
            Result = Result + "0";
        }

        if (Version < 10)
        {
            Result = Result + "0";
        }

        return Result + IntToStr(Version);
    }

    // Army.PAS:5983
    public bool RegisteredVersion()
    {
        bool Result = default;
        bool Registered = default;
        MemoryStream Mem;
        Stream FileStream;
        string Name = "";
        try
        {
            Mem = new MemoryStream();
            // { Original leaks Mem if opening or loading the file raises. }
            FileStream = OpenFile(GameTables.AoWEngine.StartupDirectory + "RELEASE\\GENERAL.PFS", 0 | 32);
            try
            {
                Mem.LoadFromStream(FileStream);
            }
            finally
            {
                FileStream?.Dispose();
            }

            try
            {
                Mem.Position = 0;
                Mem.Read(Bytes.Of(ref Registered, 1));
                StringFromStream(ref Name, Mem);
                Result = Registered;
            }
            finally
            {
                Mem?.Dispose();
            }
        }
        catch
        {
            Result = true;
        }

        return Result;
    }

    // Army.PAS:6008
    public string RegisteredTo()
    {
        string Result = "";
        bool Registered = default;
        MemoryStream Mem;
        string Name = "";
        try
        {
            Mem = new MemoryStream();
            try
            {
                Mem.LoadFromFile(GameTables.AoWEngine.StartupDirectory + "RELEASE\\GENERAL.PFS");
                Mem.Read(Bytes.Of(ref Registered, 1));
                StringFromStream(ref Name, Mem);
                Result = AbilityRules.Encode(Name, 0);
            }
            finally
            {
                Mem?.Dispose();
            }
        }
        catch
        {
            Result = "";
        }

        return Result;
    }

    // Army.PAS:6026
    public bool RequestAccess(string Name, string Code)
    {
        string RegistrationCode(string Text)
        {
            int Value;
            string Hex;
            string Result = "";
            if (Text.Length >= 4)
            {
                Value = 0;
                for (int I = 1; I <= Text.Length; I++)
                {
                    Value += Cp1252.Byte(Text[I - 1]);
                }

                for (int I = 1; I <= 4; I++)
                {
                    Hex = IntToHex(Cp1252.Byte(Text[I - 1]) * Value + I, 6);
                    Result = Result + Hex[Hex.Length - 1].ToString();
                    Value += Cp1252.Byte(Hex[Hex.Length - 1]) + Cp1252.Byte(Hex[0]);
                }
            }

            return Result;
        }

        return RegisteredTo() == Name && !IsEmpty(Name) && Code == RegistrationCode(Name);
    }

    // Army.PAS:6050
    public void CloseCampaign()
    {
        if (FCampaign != null)
        {
            FCampaign?.Dispose();
            FCampaign = null;
        }
    }

    // Army.PAS:6059
    public void LoadCampaignMap(Aow.Engine.ProgressEvent AProgress)
    {
        if (FCampaign != null && FCampaign.FStream != null)
        {
            CloseHSM(null);
            GameTables.AoWEngine.LoadHSMfromStream(FCampaign.FStream, false, AProgress);
        }
    }

    // Army.PAS:6068
    // The preflight raises before the current campaign closes (own-formats.md 2.7).
    public void LoadCampaign(string FileName)
    {
        LoadCampaign(FileName, FileBoundary.LoadOrRaise(FileName, GameProfile.Campaign));
    }

    // Loads a campaign file whose preflight has passed, from its engine image.
    public void LoadCampaign(string FileName, byte[] Image)
    {
        CloseCampaign();
        FCampaign = (AoWCampaign)RetrieveFromImage(Image, null);
        TranslateEObjectFromFile(FCampaign, GameTables.AoWEngine.StartupDirectory + "Dict\\" + "Campaign" + ".mld");
        FCampaign.FFileName = FileName;
    }

    // Army.PAS:6076
    public void SaveCampaign(string FileName)
    {
        if (FCampaign != null)
        {
            FCampaign.FStream.Clear();
            if (FGameMap != null)
            {
                SaveHSMtoStream(FCampaign.FStream, null);
            }

            StoreInFile(FileName, FCampaign, null, GameProfile.Campaign);
        }
    }

    // Army.PAS:6086
    public void NewCampaign(int AClassID)
    {
        CloseCampaign();
        FCampaign = (AoWCampaign)CreateEObject(AClassID, null);
    }

    // intf_P2_08.inc:29. The flag is set before construction and cleared when it returns.
    public static GameEngine Create(ECustomNode owner)
    {
        fullConstruction = true;
        try
        {
            return ConstructionContext.Create(owner, static (context, owner) => new GameEngine(context, owner));
        }
        finally
        {
            fullConstruction = false;
        }
    }

    private static FixedArray_byte_8 P2InitializeMovePoints__MoveBits = FixedArray_byte_8.From([1, 4, 2, 8, 16, 32, 64, 128]);

    private static readonly string P2UndeadPluralRStr = "Undead(plural)";

    private static readonly string P2UndeadRStr = "Undead";

    private static FixedArray_Aow_Units_FixedArray_Aow_Units_FixedArray_sbyte_16_16_8 P2MovementTemplates = FixedArray_Aow_Units_FixedArray_Aow_Units_FixedArray_sbyte_16_16_8.From([FixedArray_Aow_Units_FixedArray_sbyte_16_16.From([FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, 3, 3, -1, 3, -1, -1, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 6, -1, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, -1, -1, -1, 3, -1, -1, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1])]), FixedArray_Aow_Units_FixedArray_sbyte_16_16.From([FixedArray_sbyte_16.From([4, -1, -1, -1, 4, -1, -1, 4, 4, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, 4, -1, -1, 4, 4, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, 4, -1, -1, 4, 4, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1])]), FixedArray_Aow_Units_FixedArray_sbyte_16_16.From([FixedArray_sbyte_16.From([4, -1, -1, -1, 4, -1, 4, 4, 4, 4, 4, -1, 4, 4, -1, -1]), FixedArray_sbyte_16.From([4, 8, 4, 4, 4, 4, -1, 4, 4, 4, 4, -1, 4, 4, -1, -1]), FixedArray_sbyte_16.From([4, 8, 4, 4, 4, 4, -1, 4, 4, 4, 4, -1, 4, 4, -1, -1]), FixedArray_sbyte_16.From([4, 8, 4, 4, 4, 4, -1, 4, 4, 4, 4, -1, 4, 4, -1, -1]), FixedArray_sbyte_16.From([4, 8, 4, 4, 4, 4, -1, 4, 4, 4, 4, -1, 4, 4, -1, -1]), FixedArray_sbyte_16.From([4, 8, 4, 4, 4, 4, -1, 4, 4, 4, 4, -1, 4, 4, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, 4, 4, -1, 4, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, 4, -1, 4, 4, 4, -1, 4, -1, 4, 4, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, 4, 4, 4, 4, 4, -1, 4, -1, 4, 4, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, 4, 4, -1, 4, 4, -1, 4, -1, 4, 4, -1, -1]), FixedArray_sbyte_16.From([4, -1, 4, -1, 4, 4, -1, 4, 4, -1, 4, -1, 4, 4, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, -1, -1, -1, 4, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, 4, -1, 4, 4, 4, -1, 4, -1, 4, 4, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1])]), FixedArray_Aow_Units_FixedArray_sbyte_16_16.From([FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 4, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 4, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 4, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 4, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 4, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, 3, 3, -1, 3, -1, -1, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 4, -1, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, -1, -1, -1, 3, -1, -1, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1])]), FixedArray_Aow_Units_FixedArray_sbyte_16_16.From([FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 6, 6, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, 3, 3, -1, 3, -1, -1, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, 4, 3, -1, 6, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([3, -1, 6, -1, 3, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, -1, -1, -1, 3, -1, -1, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1])]), FixedArray_Aow_Units_FixedArray_sbyte_16_16.From([FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, 8, 6, 4, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, 8, 6, 4, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, 8, 6, 4, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, 8, 6, 4, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, 8, 6, 4, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, 3, 3, -1, 3, -1, -1, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, 6, -1, 4, 3, -1, 4, 3, -1, 4, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, -1, -1, -1, 3, -1, -1, -1, 8, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1])]), FixedArray_Aow_Units_FixedArray_sbyte_16_16.From([FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1])]), FixedArray_Aow_Units_FixedArray_sbyte_16_16.From([FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]), FixedArray_sbyte_16.From([-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1])])]);

    internal static int MAGICDID;

    private static void UnregisterEnhancementAbilities(AbilityControl Control)
    {
        Control.UnregisterAbilityID(120);
        Control.UnregisterAbilityID(121);
        Control.UnregisterAbilityID(122);
        Control.UnregisterAbilityID(123);
        Control.UnregisterAbilityID(124);
        Control.UnregisterAbilityID(86);
        Control.UnregisterAbilityID(87);
        Control.UnregisterAbilityID(88);
        Control.UnregisterAbilityID(89);
        Control.UnregisterAbilityID(90);
        Control.UnregisterAbilityID(24);
        Control.UnregisterAbilityID(25);
        Control.UnregisterAbilityID(30);
        Control.UnregisterAbilityID(31);
        Control.UnregisterAbilityID(44);
        Control.UnregisterAbilityID(45);
        Control.UnregisterAbilityID(55);
        Control.UnregisterAbilityID(57);
        Control.UnregisterAbilityID(58);
        Control.UnregisterAbilityID(61);
        Control.UnregisterAbilityID(62);
    }

    private static void UnregisterTouchAbilities(AbilityControl Control)
    {
        Control.UnregisterAbilityID(21);
        Control.UnregisterAbilityID(118);
        Control.UnregisterAbilityID(111);
        Control.UnregisterAbilityID(112);
        Control.UnregisterAbilityID(113);
        Control.UnregisterAbilityID(115);
        Control.UnregisterAbilityID(116);
        Control.UnregisterAbilityID(119);
        Control.UnregisterAbilityID(14);
        Control.UnregisterAbilityID(15);
        Control.UnregisterAbilityID(16);
        Control.UnregisterAbilityID(17);
        Control.UnregisterAbilityID(18);
        Control.UnregisterAbilityID(19);
        Control.UnregisterAbilityID(20);
    }

    private static void UnregisterCoreSpells(GameEngine AEngine)
    {
        SpellControl Control;
        Control = AEngine.FGameSet.FSpellControl;
        AEngine.UnregisterEClasses([EClass.Of<SpellTE>()]);
        AEngine.UnregisterEClasses([EClass.Of<SpellCastEventLog>()]);
        AEngine.UnregisterEClasses([EClass.Of<UnitSpellTE>(), EClass.Of<UnitSpellCA>()]);
        Control.UnregisterAll();
    }

    private static void UnregisterCoreAbilities(AbilityControl Control)
    {
        GameTables.AoWEngine.UnregisterEClasses([EClass.Of<CosmeticSurgeryEnchantment>(), EClass.Of<LeadershipAbilityData>()]);
        GameTables.AoWEngine.UnregisterEClasses([EClass.Of<DurationAbilityData>(), EClass.Of<MultiLevelAbilityData>(), EClass.Of<RangedAttackCA>(), EClass.Of<SelfDestructCA>(), EClass.Of<StrikeCA>(), EClass.Of<DispelMagicAbilityCA>()]);
        GameTables.AoWEngine.UnregisterEClasses([EClass.Of<UnitEnchantmentAbilityData>(), EClass.Of<UnitEnchantment>(), EClass.Of<DispelMagicAbilityTE>()]);
        GameTables.AoWEngine.UnregisterEClasses([EClass.Of<CommandCA>(), EClass.Of<CommandedAbilityData>(), EClass.Of<CommandAbilityData>(), EClass.Of<HealingTE>(), EClass.Of<DispelMagicAbilityData>()]);
        GameTables.AoWEngine.UnregisterEClasses([EClass.Of<PossessCA>(), EClass.Of<WebCA>(), EClass.Of<HealingCA>(), EClass.Of<EntangleCA>(), EClass.Of<HealingAbilityData>(), EClass.Of<InvokeDeathCA>()]);
        Control.UnregisterAbilityID(86);
        Control.UnregisterAbilityID(87);
        Control.UnregisterAbilityID(88);
        Control.UnregisterAbilityID(89);
        Control.UnregisterAbilityID(90);
        Control.UnregisterAbilityID(114);
        Control.UnregisterAbilityID(40);
        Control.UnregisterAbilityID(27);
        Control.UnregisterAbilityID(65);
        Control.UnregisterAbilityID(148);
        Control.UnregisterAbilityID(149);
        Control.UnregisterAbilityID(28);
        Control.UnregisterAbilityID(150);
        Control.UnregisterAbilityID(29);
        Control.UnregisterAbilityID(48);
        Control.UnregisterAbilityID(22);
        Control.UnregisterAbilityID(47);
        Control.UnregisterAbilityID(46);
        Control.UnregisterAbilityID(52);
        Control.UnregisterAbilityID(38);
        Control.UnregisterAbilityID(35);
        Control.UnregisterAbilityID(146);
        Control.UnregisterAbilityID(147);
        Control.UnregisterAbilityID(125);
        Control.UnregisterAbilityID(60);
        Control.UnregisterAbilityID(117);
        Control.UnregisterAbilityID(50);
        Control.UnregisterAbilityID(106);
        Control.UnregisterAbilityID(107);
        UnregisterEnhancementAbilities(Control);
        UnregisterTouchAbilities(Control);
    }

    private static readonly string P2RegisterLifeStealingRStr = "Life Stealing";

    private static readonly string P2RegisterChargeRStr = "Charge";

    private static readonly string P2RegisterDragonSlayingRStr = "Dragon Slaying";

    private static readonly string P2RegisterParryRStr = "Parry";

    private static readonly string P2RegisterExtraStrikeRStr = "Extra Strike";

    private static readonly string P2RegisterFirstStrikeRStr = "First Strike";

    private static readonly string P2RegisterEntangleStrikeRStr = "Entangle Strike";

    private static readonly string P2RegisterFireStrikeRStr = "Fire Strike";

    private static readonly string P2RegisterColdStrikeRStr = "Cold Strike";

    private static readonly string P2RegisterLightningStrikeRStr = "Lightning Strike";

    private static readonly string P2RegisterPoisonStrikeRStr = "Poison Strike";

    private static readonly string P2RegisterDeathStrikeRStr = "Death Strike";

    private static readonly string P2RegisterHolyStrikeRStr = "Holy Strike";

    private static readonly string P2RegisterMagicStrikeRStr = "Magic Strike";

    private static readonly string P2RegisterBlackBoltsRStr = "Black Bolts";

    private static readonly string P2RegisterMagicBoltsRStr = "Magic Bolts";

    private static readonly string P2RegisterFrostBoltsRStr = "Frost Bolts";

    private static readonly string P2RegisterLightningBoltsRStr = "Lightning Bolts";

    private static readonly string P2RegisterHolyBoltsRStr = "Holy Bolts";

    private static readonly string P2RegisterFireBreathRStr = "Fire Breath";

    private static readonly string P2RegisterColdBreathRStr = "Cold Breath";

    private static readonly string P2RegisterBlackBreathRStr = "Black Breath";

    private static readonly string P2RegisterDivineBreathRStr = "Divine Breath";

    private static readonly string P2RegisterGasBreathRStr = "Gas Breath";

    private static readonly string P2RegisterShootJavelinRStr = "Shoot Javelin";

    private static readonly string P2RegisterShootBlackJavelinRStr = "Shoot Black Javelin";

    private static readonly string P2RegisterHurlStonesRStr = "Hurl Stones";

    private static readonly string P2RegisterFireMusketRStr = "Fire Musket";

    private static readonly string P2RegisterFireCannonRStr = "Fire Cannon";

    private static readonly string P2RegisterHurlBoulderRStr = "Hurl Boulder";

    private static readonly string P2RegisterVenomousSpitRStr = "Venomous Spit";

    private static readonly string P2RegisterCallFlamesRStr = "Call Flames";

    private static readonly string P2RegisterArcheryRStr = "Archery";

    private static readonly string P2RegisterPoisonDartsRStr = "Poison Darts";

    private static readonly string P2RegisterDoomGazeRStr = "Doom Gaze";

    private static RangedAttackAbility P2CreateRangedAttack(int ID, string Name, sbyte Attack, sbyte Damage, sbyte Repeats, RangedAttackAbility.CombatRange Range, Set<DamageType> DamageTypes, int Enchantment)
    {
        RangedAttackAbility Ability;
        Ability = new RangedAttackAbility(null);
        Ability.FAbilityID = ID;
        Ability.FAbilityName = Name;
        Ability.FRangeRA = Range;
        Ability.FDamageTypesRA.Value = DamageTypes;
        Ability.FAttackRA = Attack;
        Ability.FDamageRA = Damage;
        Ability.FAttackRepeatRA = Repeats;
        // { Original ignores Enchantment. }
        return Ability;
    }

    private static BoltsAbility P2CreateBolts(int ID, string Name, int Attack, sbyte Damage, DamageType DamageType, int Enchantment)
    {
        BoltsAbility Ability;
        Ability = new BoltsAbility(null);
        Ability.FAbilityID = ID;
        Ability.FAbilityName = Name;
        Ability.FDamageTypesRA.Value = Set<DamageType>.Of(DamageType);
        Ability.FDamageRA = Damage;
        Ability.FAttackRA = (sbyte)Attack;
        // { Original ignores Enchantment. }
        return Ability;
    }

    private static BreathAbility P2CreateBreath(int ID, string Name, DamageType DamageType, int Enchantment)
    {
        BreathAbility Ability;
        Ability = new BreathAbility(null);
        Ability.FAbilityID = ID;
        Ability.FAbilityName = Name;
        Ability.FDamageTypesRA.Value = Set<DamageType>.Of(DamageType);
        // { Original ignores Enchantment. }
        return Ability;
    }

    private static StrikeEnhancementAbility P2CreateStrikeEnhancement(int ID, string Name, Set<AbilitySelectionType> Selection)
    {
        StrikeEnhancementAbility Ability;
        Ability = new StrikeEnhancementAbility(null);
        Ability.FSelectionTypes = Selection;
        Ability.FAbilityID = ID;
        Ability.FAbilityName = Name;
        return Ability;
    }

    private static void P2RegisterRangedGroup1(AbilityControl Control)
    {
        Control.RegisterAbility(P2CreateBolts(120, AbilityRules.TranslateRStr(P2RegisterBlackBoltsRStr), 6, 3, DamageType.Death, 126));
        Control.RegisterAbility(P2CreateBolts(121, AbilityRules.TranslateRStr(P2RegisterMagicBoltsRStr), 7, 3, DamageType.Magic, 124));
        Control.RegisterAbility(P2CreateBolts(122, AbilityRules.TranslateRStr(P2RegisterFrostBoltsRStr), 6, 3, DamageType.Cold, 125));
        Control.RegisterAbility(P2CreateBolts(123, AbilityRules.TranslateRStr(P2RegisterLightningBoltsRStr), 6, 3, DamageType.Lightning, -1));
        Control.RegisterAbility(P2CreateBolts(124, AbilityRules.TranslateRStr(P2RegisterHolyBoltsRStr), 6, 3, DamageType.Holy, 125));
        Control.RegisterAbility(P2CreateBreath(86, AbilityRules.TranslateRStr(P2RegisterFireBreathRStr), DamageType.Fire, -1));
        Control.RegisterAbility(P2CreateBreath(87, AbilityRules.TranslateRStr(P2RegisterColdBreathRStr), DamageType.Cold, -1));
    }

    private static void P2RegisterRangedGroup2(AbilityControl Control)
    {
        Control.RegisterAbility(P2CreateBreath(88, AbilityRules.TranslateRStr(P2RegisterBlackBreathRStr), DamageType.Death, -1));
        Control.RegisterAbility(P2CreateBreath(89, AbilityRules.TranslateRStr(P2RegisterDivineBreathRStr), DamageType.Holy, -1));
        Control.RegisterAbility(P2CreateBreath(90, AbilityRules.TranslateRStr(P2RegisterGasBreathRStr), DamageType.Poison, -1));
        Control.RegisterAbility(new FlameThrowingAbility(null));
        Control.RegisterAbility(P2CreateRangedAttack(57, AbilityRules.TranslateRStr(P2RegisterShootJavelinRStr), 5, 5, 2, RangedAttackAbility.CombatRange.Long, Set<DamageType>.Of(DamageType.Physical), 122));
        Control.RegisterAbility(P2CreateRangedAttack(58, AbilityRules.TranslateRStr(P2RegisterShootBlackJavelinRStr), 5, 5, 2, RangedAttackAbility.CombatRange.Long, Set<DamageType>.Of(DamageType.Death, DamageType.Physical), 123));
        Control.RegisterAbility(P2CreateRangedAttack(25, AbilityRules.TranslateRStr(P2RegisterHurlStonesRStr), 3, 1, 4, RangedAttackAbility.CombatRange.Medium, Set<DamageType>.Of(DamageType.Physical), 111));
    }

    private static void P2RegisterRangedGroup3(AbilityControl Control)
    {
        Control.RegisterAbility(P2CreateRangedAttack(45, AbilityRules.TranslateRStr(P2RegisterFireMusketRStr), 5, 7, 1, RangedAttackAbility.CombatRange.Long, Set<DamageType>.Of(DamageType.Physical), 113));
        Control.RegisterAbility(P2CreateRangedAttack(44, AbilityRules.TranslateRStr(P2RegisterFireCannonRStr), 5, 8, 1, RangedAttackAbility.CombatRange.Long, Set<DamageType>.Of(DamageType.Physical, DamageType.Wall), 112));
        Control.RegisterAbility(P2CreateRangedAttack(24, AbilityRules.TranslateRStr(P2RegisterHurlBoulderRStr), 3, 7, 1, RangedAttackAbility.CombatRange.Long, Set<DamageType>.Of(DamageType.Physical, DamageType.Wall), 110));
        Control.RegisterAbility(P2CreateRangedAttack(62, AbilityRules.TranslateRStr(P2RegisterVenomousSpitRStr), 5, 4, 1, RangedAttackAbility.CombatRange.Short, Set<DamageType>.Of(DamageType.Poison), -1));
        Control.RegisterAbility(P2CreateRangedAttack(31, AbilityRules.TranslateRStr(P2RegisterCallFlamesRStr), 5, 4, 1, RangedAttackAbility.CombatRange.Medium, Set<DamageType>.Of(DamageType.Fire), -1));
        Control.RegisterAbility(P2CreateRangedAttack(22, AbilityRules.TranslateRStr(P2RegisterArcheryRStr), 4, 2, 2, RangedAttackAbility.CombatRange.Medium, Set<DamageType>.Of(DamageType.Physical), -1));
    }

    private static void P2RegisterRangedGroup4(AbilityControl Control)
    {
        Control.RegisterAbility(P2CreateRangedAttack(61, AbilityRules.TranslateRStr(P2RegisterPoisonDartsRStr), 4, 1, 3, RangedAttackAbility.CombatRange.Medium, Set<DamageType>.Of(DamageType.Poison, DamageType.Physical), 114));
        Control.RegisterAbility(P2CreateRangedAttack(55, AbilityRules.TranslateRStr(P2RegisterDoomGazeRStr), 6, 5, 1, RangedAttackAbility.CombatRange.Long, Set<DamageType>.Of(DamageType.Death), -1));
    }

    private static void P2RegisterStrikeGroup1(AbilityControl Control)
    {
        Control.RegisterAbility(new StrikeAbility(null));
        Control.RegisterAbility(P2CreateStrikeEnhancement(118, AbilityRules.TranslateRStr(P2RegisterLifeStealingRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem, AbilitySelectionType.Editor)));
        Control.RegisterAbility(P2CreateStrikeEnhancement(111, AbilityRules.TranslateRStr(P2RegisterChargeRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.Editor)));
        Control.RegisterAbility(P2CreateStrikeEnhancement(112, AbilityRules.TranslateRStr(P2RegisterDragonSlayingRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem, AbilitySelectionType.Editor)));
        Control.RegisterAbility(P2CreateStrikeEnhancement(113, AbilityRules.TranslateRStr(P2RegisterParryRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.Editor)));
        Control.RegisterAbility(P2CreateStrikeEnhancement(115, AbilityRules.TranslateRStr(P2RegisterExtraStrikeRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem, AbilitySelectionType.Editor)));
        Control.RegisterAbility(P2CreateStrikeEnhancement(116, AbilityRules.TranslateRStr(P2RegisterFirstStrikeRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem, AbilitySelectionType.Editor)));
    }

    private static void P2RegisterStrikeGroup2(AbilityControl Control)
    {
        Control.RegisterAbility(P2CreateStrikeEnhancement(119, AbilityRules.TranslateRStr(P2RegisterEntangleStrikeRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit)));
        Control.RegisterAbility(P2CreateStrikeEnhancement(14, AbilityRules.TranslateRStr(P2RegisterFireStrikeRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
        Control.RegisterAbility(P2CreateStrikeEnhancement(15, AbilityRules.TranslateRStr(P2RegisterColdStrikeRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
        Control.RegisterAbility(P2CreateStrikeEnhancement(16, AbilityRules.TranslateRStr(P2RegisterLightningStrikeRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
        Control.RegisterAbility(P2CreateStrikeEnhancement(17, AbilityRules.TranslateRStr(P2RegisterPoisonStrikeRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
        Control.RegisterAbility(P2CreateStrikeEnhancement(18, AbilityRules.TranslateRStr(P2RegisterDeathStrikeRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
    }

    private static void P2RegisterStrikeGroup3(AbilityControl Control)
    {
        Control.RegisterAbility(P2CreateStrikeEnhancement(19, AbilityRules.TranslateRStr(P2RegisterHolyStrikeRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
        Control.RegisterAbility(P2CreateStrikeEnhancement(20, AbilityRules.TranslateRStr(P2RegisterMagicStrikeRStr), Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem)));
    }

    private static void P2RegisterEnhancements(AbilityControl Control)
    {
        P2RegisterRangedGroup1(Control);
        P2RegisterRangedGroup2(Control);
        P2RegisterRangedGroup3(Control);
        // Original ranged-attack registration
        P2RegisterRangedGroup4(Control);
    }

    private static void P2RegisterTouches(AbilityControl Control)
    {
        P2RegisterStrikeGroup1(Control);
        P2RegisterStrikeGroup2(Control);
        // Original strike registration
        P2RegisterStrikeGroup3(Control);
    }

    private static void P2InitializeMovePoints()
    {
        int Points;
        FillBytes(MemoryMarshal.AsBytes((Span<FixedArray_Aow_Units_FixedArray_sbyte_16_16>)MovementTables.MovePointTables).Slice(0, 65536), 255);
        for (int Mask = 0; Mask <= 255; Mask++)
        {
            for (int Overlay = -1; Overlay <= 11; Overlay++)
            {
                for (int Terrain = 0; Terrain <= 15; Terrain++)
                {
                    Points = 127;
                    for (int MoveType = 0; MoveType <= 7; MoveType++)
                    {
                        if ((Mask & P2InitializeMovePoints__MoveBits[MoveType]) != 0 && P2MovementTemplates[MoveType][Terrain][Overlay - -1] != -1 && P2MovementTemplates[MoveType][Terrain][Overlay - -1] < Points)
                        {
                            Points = P2MovementTemplates[MoveType][Terrain][Overlay - -1];
                        }
                    }

                    if (Points != 127)
                    {
                        MovementTables.MovePointTables[Mask][Terrain][Overlay - -1] = (sbyte)Points;
                    }
                }
            }
        }
    }

    private static void P2RegisterGroupControls()
    {
    }

    private static void P2RegisterMoveEvents(GameEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<EndTurnTE>(), EClass.Of<GameOverTE>(), EClass.Of<MultiMoveTE>(), EClass.Of<CheatTE>(), EClass.Of<DisbandUnitTE>(), EClass.Of<ChangePlayerSettingsTE>(), EClass.Of<CombatFlowControlTE>(), EClass.Of<TerminatePlayerTE>()]);
    }

    private static void P2RegisterAbilities(AbilityControl Control)
    {
        GameTables.AoWEngine.RegisterEClasses([EClass.Of<CosmeticSurgeryEnchantment>(), EClass.Of<LeadershipAbilityData>(), EClass.Of<DispelMagicAbilityCA>()]);
        GameTables.AoWEngine.RegisterEClasses([EClass.Of<DurationAbilityData>(), EClass.Of<MultiLevelAbilityData>(), EClass.Of<RangedAttackCA>(), EClass.Of<SelfDestructCA>(), EClass.Of<StrikeCA>()]);
        GameTables.AoWEngine.RegisterEClasses([EClass.Of<UnitEnchantmentAbilityData>(), EClass.Of<UnitEnchantment>(), EClass.Of<DispelMagicAbilityTE>(), EClass.Of<PossessedAbilityData>()]);
        GameTables.AoWEngine.RegisterEClasses([EClass.Of<CommandCA>(), EClass.Of<CommandedAbilityData>(), EClass.Of<CommandAbilityData>(), EClass.Of<HealingTE>(), EClass.Of<DispelMagicAbilityData>()]);
        GameTables.AoWEngine.RegisterEClasses([EClass.Of<PossessCA>(), EClass.Of<WebCA>(), EClass.Of<HealingCA>(), EClass.Of<EntangleCA>(), EClass.Of<HealingAbilityData>(), EClass.Of<TurnUndeadCA>(), EClass.Of<InvokeDeathCA>()]);
        Control.RegisterAbility(new LeadershipAbility(null));
        Control.RegisterAbility(new EntangleAbility(null));
        Control.RegisterAbility(new WebAbility(null));
        Control.RegisterAbility(new SelfDestructAbility(null));
        Control.RegisterAbility(new RoundAttackAbility(null));
        Control.RegisterAbility(new CharmAbility(null));
        Control.RegisterAbility(new CharmedAbility(null));
        Control.RegisterAbility(new DominateAbility(null));
        Control.RegisterAbility(new DominatedAbility(null));
        Control.RegisterAbility(new SeduceAbility(null));
        Control.RegisterAbility(new SeducedAbility(null));
        Control.RegisterAbility(new HealingAbility(null));
        Control.RegisterAbility(new SpellCastingAbility(null));
        Control.RegisterAbility(new TurnUndeadAbility(null));
        Control.RegisterAbility(new InvokeDeathAbility(null));
        Control.RegisterAbility(new ConstructAbility(null));
        Control.RegisterAbility(new DispelMagicAbility(null));
        Control.RegisterAbility(new WallCrushingAbility(null));
        Control.RegisterAbility(new TransportAbility(null));
        Control.RegisterAbility(new PossessAbility(null));
        Control.RegisterAbility(new PossessedAbility(null));
        P2RegisterEnhancements(Control);
        P2RegisterTouches(Control);
    }

    private static void P2RegisterSpells(GameEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<SpellTE>()]);
        AEngine.RegisterEClasses([EClass.Of<SpellCastEventLog>()]);
        AEngine.RegisterEClasses([EClass.Of<UnitSpellTE>(), EClass.Of<UnitSpellCA>()]);
    }

    private static void LoadExternalPlugIns(string Directory)
    {
        string Dir;
        void LoadFrom(string Dir)
        {
            SearchRec SR = default;
            StringList Dirs;
            bool Found;
            int H = default;
            Found = FindFirst(Dir + "*.DPL", 63 - 16, ref SR) == 0;
            while (Found)
            {
                try
                {
                    /* TODO: Implement the native Delphi package loader. */
                    throw new NotImplementedException();
                    EngineHost.Packages.AddObject(Dir + SR.Name, IntToObject(H));
                }
                catch
                {
                    throw new IOException("Unable to load PlugIn: " + Dir + SR.Name);
                }

                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            Dirs = new StringList();
            Found = FindFirst(Dir + "*.*", 16, ref SR) == 0;
            while (Found)
            {
                if ((SR.Attr & 16) != 0 && SR.Name != "." && SR.Name != "..")
                {
                    Dirs.Add(Dir + SR.Name + "\\");
                }

                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            for (int I = 0, last = Dirs.Count - 1; I <= last; I++)
            {
                LoadFrom(Dirs[I]);
            }

            Dirs?.Dispose();
        }

        Dir = ExpandFileName(Directory);
        // { the original tests the character of Directory, not of Dir }
        if (!IsEmpty(Dir) && Directory[Dir.Length - 1] != 92)
        {
            Dir = Dir + "\\";
        }

        LoadFrom(Dir);
    }

    internal static void StringFromStream(ref string S, Stream Stream)
    {
        int Len = default;
        Stream.ReadBytes(Bytes.Of(ref Len, 4));
        if (Len < 0) throw new ArgumentOutOfRangeException(paramName: null, "Negative string length");
        byte[] buffer = new byte[Len];
        Cp1252.Encode(S)[..Math.Min(S.Length, Len)].CopyTo(buffer);
        // A failed read leaves the resized value in the destination.
        S = Cp1252.Decode(buffer);
        Stream.ReadBytes(buffer);
        S = Cp1252.Decode(buffer);
    }

    private static void RegisterEventLog(GameEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TextEventLog>(), EClass.Of<EndTurnEventLog>(), EClass.Of<GameOverEventLog>(), EClass.Of<LocationTextEventLog>()]);
    }

    private static void RegisterNetworkEngine(GameEngine AEngine)
    {
        NetworkSession.NetworkEngineDID = AEngine.RegisterDebugID("Network Engine");
        AEngine.RegisterEClasses([EClass.Of<TokenPackage>(), EClass.Of<PassTokenTE>(), EClass.Of<SyncTE>()]);
    }

    private static void RegisterSoundEngineClasses(GameEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<SFXSequence>(), EClass.Of<SFXLibrary>(), EClass.Of<SFX>()]);
    }

    internal static void UnregisterCitySpells(GameEngine Engine)
    {
        Engine.UnregisterEClasses([EClass.Of<CityEnchantmentSpellTE>(), EClass.Of<CityTargetSpellTE>(), EClass.Of<TownGateTE>()]);
        var spellControl = Engine.FGameSet.FSpellControl;
        spellControl.UnregisterSpellID(72);
        // { Original leaves Town Quake ($47) registered. }
        spellControl.UnregisterSpellID(38);
        spellControl.UnregisterSpellID(52);
        spellControl.UnregisterSpellID(53);
        spellControl.UnregisterSpellID(54);
        spellControl.UnregisterSpellID(58);
        spellControl.UnregisterSpellID(76);
    }

    internal static void UnregisterCombatSpells(GameEngine Engine)
    {
        SpellControl Control;
        Control = Engine.FGameSet.FSpellControl;
        Engine.UnregisterEClasses([EClass.Of<CombatSpellCA>(), EClass.Of<FastCombatTerrorCA>(), EClass.Of<TacticalCombatTerrorCA>(), EClass.Of<RecallSpiritCA>(), EClass.Of<AnimateDeadCA>(), EClass.Of<FastCombatHighPrayerCA>(), EClass.Of<TacticalCombatHighPrayerCA>(), EClass.Of<EntangleSpellCA>(), EClass.Of<MindDecayCA>(), EClass.Of<TurnUndeadSpellCA>(), EClass.Of<SlowCA>()]);
        Control.UnregisterSpellID(100);
        Control.UnregisterSpellID(101);
        Control.UnregisterSpellID(102);
        Control.UnregisterSpellID(103);
        Control.UnregisterSpellID(104);
        Control.UnregisterSpellID(105);
        Control.UnregisterSpellID(106);
        Control.UnregisterSpellID(107);
        Control.UnregisterSpellID(108);
        Control.UnregisterSpellID(110);
        Control.UnregisterSpellID(111);
        Control.UnregisterSpellID(112);
        Control.UnregisterSpellID(113);
        Control.UnregisterSpellID(114);
        Control.UnregisterSpellID(115);
        Control.UnregisterSpellID(116);
        Control.UnregisterSpellID(117);
        Control.UnregisterSpellID(118);
        Control.UnregisterSpellID(119);
        Control.UnregisterSpellID(120);
        Control.UnregisterSpellID(121);
        Control.UnregisterSpellID(122);
        Control.UnregisterSpellID(123);
        Control.UnregisterSpellID(124);
        Control.UnregisterSpellID(125);
        Control.UnregisterSpellID(130);
        Control.UnregisterSpellID(129);
        Control.UnregisterSpellID(126);
        Control.UnregisterSpellID(127);
        Control.UnregisterSpellID(128);
    }

    internal static void UnregisterGlobalSpells(GameEngine Engine)
    {
        SpellControl Control;
        Control = Engine.FGameSet.FSpellControl;
        Engine.UnregisterEClasses([EClass.Of<WarpPartyTE>(), EClass.Of<DisjunctionTE>()]);
        Control.UnregisterSpellID(1);
        Control.UnregisterSpellID(2);
        Control.UnregisterSpellID(34);
        Control.UnregisterSpellID(51);
        Control.UnregisterSpellID(66);
        Control.UnregisterSpellID(67);
        Control.UnregisterSpellID(69);
        Control.UnregisterSpellID(75);
    }

    internal static void UnregisterGlobalEnchantmentSpells(GameEngine AEngine)
    {
        SpellControl Control;
        Control = AEngine.FGameSet.FSpellControl;
        Control.UnregisterSpellID(44);
        Control.UnregisterSpellID(46);
        Control.UnregisterSpellID(47);
        Control.UnregisterSpellID(48);
        Control.UnregisterSpellID(50);
        Control.UnregisterSpellID(60);
        Control.UnregisterSpellID(61);
        Control.UnregisterSpellID(62);
        Control.UnregisterSpellID(63);
        Control.UnregisterSpellID(64);
        Control.UnregisterSpellID(45);
        Control.UnregisterSpellID(65);
        AEngine.UnregisterEClasses([EClass.Of<EnchantedRoadsEnchantment>(), EClass.Of<GlobalEnchantmentSpellTE>(), EClass.Of<GlobalEnchantmentEventLog>()]);
        AEngine.UnregisterEClasses([EClass.Of<WaterMasteryEnchantment>(), EClass.Of<HatredEnchantment>(), EClass.Of<HarmonyEnchantment>(), EClass.Of<PowerLeakEnchantment>(), EClass.Of<FloodEnchantment>(), EClass.Of<AirMasteryEnchantment>()]);
        AEngine.UnregisterEClasses([EClass.Of<LifeMasteryEnchantment>(), EClass.Of<DeathMasteryEnchantment>(), EClass.Of<FireMasteryEnchantment>(), EClass.Of<EarthMasteryEnchantment>(), EClass.Of<SpellWardEnchantment>()]);
    }

    internal static void UnregisterGlobalTargetSpells(GameEngine Engine)
    {
        SpellControl Control;
        Control = Engine.FGameSet.FSpellControl;
        Control.UnregisterSpellID(10);
        Control.UnregisterSpellID(11);
        Control.UnregisterSpellID(18);
        Control.UnregisterSpellID(41);
        Control.UnregisterSpellID(57);
        Control.UnregisterSpellID(68);
        Control.UnregisterSpellID(70);
        Control.UnregisterSpellID(73);
        Control.UnregisterSpellID(74);
        Engine.UnregisterEClasses([EClass.Of<FreezeWaterTE>(), EClass.Of<FrozenWaterHS>(), EClass.Of<FireBarrierTE>(), EClass.Of<LevelTerrainTE>(), EClass.Of<RejuvenateTE>(), EClass.Of<WatcherTE>(), EClass.Of<WatcherHS>(), EClass.Of<TornadoTE>(), EClass.Of<VortexTE>(), EClass.Of<HealingShowersTE>(), EClass.Of<GlobalTargetSpellTE>()]);
    }

    internal static void UnregisterSummonSpells(GameEngine AEngine)
    {
        SpellControl Control;
        Control = AEngine.FGameSet.FSpellControl;
        AEngine.UnregisterEClasses([EClass.Of<SummonSpellTE>()]);
        Control.UnregisterSpellID(24);
        Control.UnregisterSpellID(22);
        Control.UnregisterSpellID(31);
        Control.UnregisterSpellID(27);
        Control.UnregisterSpellID(28);
        Control.UnregisterSpellID(29);
        Control.UnregisterSpellID(30);
        Control.UnregisterSpellID(25);
        Control.UnregisterSpellID(26);
        Control.UnregisterSpellID(23);
        Control.UnregisterSpellID(21);
        Control.UnregisterSpellID(20);
        Control.UnregisterSpellID(78);
    }

    internal static void UnregisterUnitSpells(GameEngine Engine)
    {
        SpellControl Control;
        Control = Engine.FGameSet.FSpellControl;
        Control.UnregisterSpellID(8);
        Control.UnregisterSpellID(15);
        Control.UnregisterSpellID(17);
        Control.UnregisterSpellID(16);
        Control.UnregisterSpellID(33);
        Control.UnregisterSpellID(32);
        Control.UnregisterSpellID(35);
        Control.UnregisterSpellID(37);
        Control.UnregisterSpellID(36);
        Control.UnregisterSpellID(39);
        Control.UnregisterSpellID(40);
        Control.UnregisterSpellID(42);
        Control.UnregisterSpellID(43);
        Control.UnregisterSpellID(49);
        Control.UnregisterSpellID(55);
        Control.UnregisterSpellID(56);
        Control.UnregisterSpellID(59);
        Control.UnregisterSpellID(77);
        Engine.UnregisterEClasses([EClass.Of<DispelMagicSpellCA>()]);
    }

    internal static void UnRegisterPoisonPlant(GameEngine AEngine)
    {
        AEngine.UnregisterEClasses([EClass.Of<PoisonPlant>(), EClass.Of<PoisonPlant.Resource>(), EClass.Of<PoisonPlantSpellTE>()]);
        AEngine.FGameSet.FSpellControl.UnregisterSpellID(12);
    }

    internal static void UnRegisterUnholyGround(GameEngine AEngine)
    {
        AEngine.UnregisterEClasses([EClass.Of<UnHolyGround>(), EClass.Of<UnHolyGround.Resource>(), EClass.Of<UnholyGroundSpellTE>()]);
        AEngine.FGameSet.FSpellControl.UnregisterSpellID(14);
    }

}
