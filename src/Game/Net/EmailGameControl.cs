// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net;

// games
public partial class EmailGameControl : EObject
{
    // Diplomacy.pas:2489
    public EmailGameControl(ECustomNode owner) : base(owner)
    {
        FGames = new EmailGameList(null);
    }

    public EmailGameList FGames;

    // Diplomacy.pas:2495
    public override void Dispose()
    {
        FGames?.Dispose();
        base.Dispose();
    }

    // Diplomacy.pas:2498
    public int CreateUniqueLocalID()
    {
        int Result = 1;
        while (FGames.IndexOfUniqueLocalID(Result) != -1)
        {
            Result += 1;
        }

        return Result;
    }

    // Diplomacy.pas:2504
    public void Load()
    {
        string FileName;
        FGames.Clear();
        FileName = GameTables.AoWEngine.StartupDirectory + "EmailIn\\" + "egames.pfs";
        if (FileExists(FileName))
        {
            if (GetFileCRC32(FileName) == GameTables.AoWEngine.FRegistry.GetEmailGamesCRC())
            {
                GameTables.AoWEngine.ReadFromFile(FileName, FGames, null);
            }
        }
    }

    // Diplomacy.pas:2514
    public void Save()
    {
        string FileName;
        try
        {
            FileName = GameTables.AoWEngine.StartupDirectory + "EmailIn\\" + "egames.pfs";
            GameTables.AoWEngine.WriteToFile(GameTables.AoWEngine.StartupDirectory + "EmailIn\\" + "egames.pfs", FGames, null);
            var fileCRC = GetFileCRC32(FileName);
            GameTables.AoWEngine.FRegistry.SetEmailGamesCRC(fileCRC);
        }
        catch
        {
        }
    }

    // Diplomacy.pas:2525
    public EmailGame FindEmailGame(EmailGame Game)
    {
        EmailGame Result;
        int Index;
        Index = FGames.IndexOfUniqueLocalID(Game.FUniqueLocalID);
        if (Index != -1 && Game.FUniqueID != 0 && FGames.GetEmailGames(Index).FUniqueID == Game.FUniqueID)
        {
            Result = FGames.GetEmailGames(Index);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Diplomacy.pas:2535
    public bool EmailGameRestarted(EmailGame Game)
    {
        bool Result;
        EmailGame LocalGame;
        if (Game.FSecurityID != 0)
        {
            LocalGame = FindEmailGame(Game);
            Result = LocalGame == null || Game.FSecurityID != LocalGame.FSecurityID;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:2546
    public int GenerateSecurityID() => Random(PixelFormats.GetElapsedMilliSeconds() & 16777215) + 1;

    // Diplomacy.pas:2549
    public bool UpdateGame(EmailGame Game)
    {
        bool Result;
        EmailGame LocalGame;
        LocalGame = FindEmailGame(Game);
        if (LocalGame != null)
        {
            LocalGame.FLastDay = Game.FLastDay;
            LocalGame.FSecurityID = GenerateSecurityID();
            Game.FSecurityID = LocalGame.FSecurityID;
            Save();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:2564
    public EmailGame CreateEmailGame(string Name, string EmailAddress)
    {
        EmailGame Result = new EmailGame(null);
        Result.FEmailAddress = EmailAddress;
        Result.FUniqueLocalID = CreateUniqueLocalID();
        Result.FUniqueID = Random(PixelFormats.GetElapsedMilliSeconds() & 16777215) + 1;
        Result.FSecurityID = GenerateSecurityID();
        Result.FName = Name;
        Result.SetOwner(FGames);
        Save();
        return Result;
    }

    // Diplomacy.pas:2576
    public void ValidateIncommingGame(string FileName, ref string Destination)
    {
        AoWHSMap Map;
        AoWHSMap PreviousMap;
        EmailGame LocalGame;
        bool Overwrite;
        int I;
        string Name = "";
        // { Addressability survives elimination of this assignment. }
        try
        {
            Destination = "";
            Overwrite = false;
            Map = (AoWHSMap)GameTables.AoWEngine.CreatePreviewHSM(FileName, GameProfile.WorldMap);
            bool condition = Map != null && Map.FConnectionType == PlayerConnectionType.Email && Map.FCurrentPlayer > 0;
            if (condition)
            {
                var count = Map.FPlayers.Count;
                condition = Map.FCurrentPlayer < count;
            }

            if (condition && Map.FPlayers.GetPlayers(Map.FCurrentPlayer).FPBEMSettings.FStatus.Contains(PBEMPlayerStatusElement.NewTurn))
            {
                var localEmailAddress = GameTables.AoWEngine.FRegistry.GetLocalEmailAddress();
                if (CompareText(Map.FPlayers.GetPlayers(Map.FCurrentPlayer).FPBEMSettings.FEmailGame.FEmailAddress, localEmailAddress) == 0)
                {
                    LocalGame = FindEmailGame(Map.FPlayers.GetPlayers(Map.FCurrentPlayer).FPBEMSettings.FEmailGame);
                    if (LocalGame != null)
                    {
                        if (Map.FDay > LocalGame.FLastDay)
                        {
                            Name = Map.FPlayers.GetPlayers(Map.FCurrentPlayer).FPBEMSettings.FEmailGame.FName;
                            if (FileExists(GameTables.AoWEngine.StartupDirectory + "EmailIn\\" + Name + ".asg"))
                            {
                                PreviousMap = (AoWHSMap)GameTables.AoWEngine.CreatePreviewHSM(GameTables.AoWEngine.StartupDirectory + "EmailIn\\" + Name + ".asg", GameProfile.WorldMap);
                                if (PreviousMap != null)
                                {
                                    // { Original bug: the previous map's player is checked against the new map's count. }
                                    bool test = PreviousMap.FCurrentPlayer > 0;
                                    if (test)
                                    {
                                        var count = Map.FPlayers.Count;
                                        test = PreviousMap.FCurrentPlayer < count;
                                    }

                                    if (test)
                                    {
                                        var players = PreviousMap.FPlayers.GetPlayers(PreviousMap.FCurrentPlayer);
                                        var fPlayersPlayers = Map.FPlayers.GetPlayers(Map.FCurrentPlayer);
                                        bool passed = players.FPBEMSettings.FEmailGame.FUniqueLocalID == fPlayersPlayers.FPBEMSettings.FEmailGame.FUniqueLocalID;
                                        if (passed)
                                        {
                                            var players2 = PreviousMap.FPlayers.GetPlayers(PreviousMap.FCurrentPlayer);
                                            var players3 = Map.FPlayers.GetPlayers(Map.FCurrentPlayer);
                                            passed = players2.FPBEMSettings.FEmailGame.FUniqueID == players3.FPBEMSettings.FEmailGame.FUniqueID;
                                        }

                                        if (passed)
                                        {
                                            Overwrite = true;
                                        }
                                    }

                                    PreviousMap?.Dispose();
                                }
                            }
                        }
                    }
                    else
                    {
                        Name = Map.FPlayers.GetPlayers(Map.FCurrentPlayer).FPBEMSettings.FEmailGame.FName;
                    }

                    if (!IsEmpty(Name))
                    {
                        I = 0;
                        Destination = GameTables.AoWEngine.StartupDirectory + "EmailIn\\" + Name + ".asg";
                        while (!Overwrite && FileExists(Destination))
                        {
                            I += 1;
                            Destination = GameTables.AoWEngine.StartupDirectory + "EmailIn\\" + Name + IntToStr(I) + ".asg";
                        }
                    }
                }
            }

            Map?.Dispose();
        }
        catch
        {
        }
    }

    private static int GetFileCRC32(string FileName)
    {
        int result = 0;
        MemoryStream val = new MemoryStream();
        try
        {
            Stream val2 = RtlUnit.OpenFile(FileName, 0);
            try
            {
                StreamExtensions.LoadFromStream(val, val2);
                int size = (int)((Stream)val).Length;
                result = EngineServices.GetCRC32((ReadOnlySpan<byte>)(val.GetBuffer()), size);
            }
            finally
            {
                if (val2 != null)
                {
                    val2.Dispose();
                }
            }
        }
        finally
        {
            if (val != null)
            {
                ((Stream)val).Dispose();
            }
        }
        return result;
    }

    // intf_P5_04.inc:29

    // intf_P5_04.inc:29

}
