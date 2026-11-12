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
public partial class EmailGameControl : EngineObject
{
    // Diplomacy.pas:2489
    public EmailGameControl(ECustomNode owner) : base(owner)
    {
        Games = new EmailGameList(null);
    }

    public EmailGameList Games;

    // Diplomacy.pas:2495
    public override void Dispose()
    {
        Games?.Dispose();
        base.Dispose();
    }

    // Diplomacy.pas:2498
    public int CreateUniqueLocalId()
    {
        int result = 1;
        while (Games.IndexOfUniqueLocalId(result) != -1)
        {
            result += 1;
        }

        return result;
    }

    // Diplomacy.pas:2504
    public void Load()
    {
        string fileName;
        Games.Clear();
        fileName = GameTables.GameEngine.StartupDirectory + "EmailIn\\" + "egames.pfs";
        if (FileExists(fileName))
        {
            if (GetFileCrc32(fileName) == GameTables.GameEngine.Registry.GetEmailGamesCrc())
            {
                GameTables.GameEngine.ReadFromFile(fileName, Games, null);
            }
        }
    }

    // Diplomacy.pas:2514
    public void Save()
    {
        string fileName;
        try
        {
            fileName = GameTables.GameEngine.StartupDirectory + "EmailIn\\" + "egames.pfs";
            GameTables.GameEngine.WriteToFile(GameTables.GameEngine.StartupDirectory + "EmailIn\\" + "egames.pfs", Games, null);
            var fileCrc = GetFileCrc32(fileName);
            GameTables.GameEngine.Registry.SetEmailGamesCrc(fileCrc);
        }
        catch
        {
        }
    }

    // Diplomacy.pas:2525
    public EmailGame FindEmailGame(EmailGame game)
    {
        EmailGame result;
        int index;
        index = Games.IndexOfUniqueLocalId(game.UniqueLocalId);
        if (index != -1 && game.UniqueId != 0 && Games.GetEmailGames(index).UniqueId == game.UniqueId)
        {
            result = Games.GetEmailGames(index);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Diplomacy.pas:2535
    public bool EmailGameRestarted(EmailGame game)
    {
        bool result;
        EmailGame localGame;
        if (game.SecurityId != 0)
        {
            localGame = FindEmailGame(game);
            result = localGame == null || game.SecurityId != localGame.SecurityId;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:2546
    public int GenerateSecurityId() => Random(PixelFormats.GetElapsedMilliseconds() & 16777215) + 1;

    // Diplomacy.pas:2549
    public bool UpdateGame(EmailGame game)
    {
        bool result;
        EmailGame localGame;
        localGame = FindEmailGame(game);
        if (localGame != null)
        {
            localGame.LastDay = game.LastDay;
            localGame.SecurityId = GenerateSecurityId();
            game.SecurityId = localGame.SecurityId;
            Save();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:2564
    public EmailGame CreateEmailGame(string name, string emailAddress)
    {
        EmailGame result = new EmailGame(null);
        result.EmailAddress = emailAddress;
        result.UniqueLocalId = CreateUniqueLocalId();
        result.UniqueId = Random(PixelFormats.GetElapsedMilliseconds() & 16777215) + 1;
        result.SecurityId = GenerateSecurityId();
        result.Name = name;
        result.SetOwner(Games);
        Save();
        return result;
    }

    // Diplomacy.pas:2576
    public void ValidateIncomingGame(string fileName, ref string destination)
    {
        AoWHSMap map;
        AoWHSMap previousMap;
        EmailGame localGame;
        bool overwrite;
        int I;
        string name = "";
        // { Addressability survives elimination of this assignment. }
        try
        {
            destination = "";
            overwrite = false;
            map = (AoWHSMap)GameTables.GameEngine.CreatePreviewHsm(fileName, GameProfile.WorldMap);
            bool condition = map != null && map.ConnectionType == PlayerConnectionType.Email && map.CurrentPlayer > 0;
            if (condition)
            {
                var count = map.Players.Count;
                condition = map.CurrentPlayer < count;
            }

            if (condition && map.Players.GetPlayers(map.CurrentPlayer).PbemSettings.Status.Contains(PbemPlayerStatusElement.NewTurn))
            {
                var localEmailAddress = GameTables.GameEngine.Registry.GetLocalEmailAddress();
                if (CompareText(map.Players.GetPlayers(map.CurrentPlayer).PbemSettings.EmailGame.EmailAddress, localEmailAddress) == 0)
                {
                    localGame = FindEmailGame(map.Players.GetPlayers(map.CurrentPlayer).PbemSettings.EmailGame);
                    if (localGame != null)
                    {
                        if (map.Day > localGame.LastDay)
                        {
                            name = map.Players.GetPlayers(map.CurrentPlayer).PbemSettings.EmailGame.Name;
                            if (FileExists(GameTables.GameEngine.StartupDirectory + "EmailIn\\" + name + ".asg"))
                            {
                                previousMap = (AoWHSMap)GameTables.GameEngine.CreatePreviewHsm(GameTables.GameEngine.StartupDirectory + "EmailIn\\" + name + ".asg", GameProfile.WorldMap);
                                if (previousMap != null)
                                {
                                    // { Original bug: the previous map's player is checked against the new map's count. }
                                    bool test = previousMap.CurrentPlayer > 0;
                                    if (test)
                                    {
                                        var count = map.Players.Count;
                                        test = previousMap.CurrentPlayer < count;
                                    }

                                    if (test)
                                    {
                                        var players = previousMap.Players.GetPlayers(previousMap.CurrentPlayer);
                                        var fPlayersPlayers = map.Players.GetPlayers(map.CurrentPlayer);
                                        bool passed = players.PbemSettings.EmailGame.UniqueLocalId == fPlayersPlayers.PbemSettings.EmailGame.UniqueLocalId;
                                        if (passed)
                                        {
                                            var players2 = previousMap.Players.GetPlayers(previousMap.CurrentPlayer);
                                            var players3 = map.Players.GetPlayers(map.CurrentPlayer);
                                            passed = players2.PbemSettings.EmailGame.UniqueId == players3.PbemSettings.EmailGame.UniqueId;
                                        }

                                        if (passed)
                                        {
                                            overwrite = true;
                                        }
                                    }

                                    previousMap?.Dispose();
                                }
                            }
                        }
                    }
                    else
                    {
                        name = map.Players.GetPlayers(map.CurrentPlayer).PbemSettings.EmailGame.Name;
                    }

                    if (!IsEmpty(name))
                    {
                        I = 0;
                        destination = GameTables.GameEngine.StartupDirectory + "EmailIn\\" + name + ".asg";
                        while (!overwrite && FileExists(destination))
                        {
                            I += 1;
                            destination = GameTables.GameEngine.StartupDirectory + "EmailIn\\" + name + IntToStr(I) + ".asg";
                        }
                    }
                }
            }

            map?.Dispose();
        }
        catch
        {
        }
    }

    private static int GetFileCrc32(string fileName)
    {
        int result = 0;
        MemoryStream value = new MemoryStream();
        try
        {
            Stream value2 = RtlUnit.OpenFile(fileName, 0);
            try
            {
                StreamExtensions.LoadFromStream(value, value2);
                int size = (int)((Stream)value).Length;
                result = EngineServices.GetCrc32((ReadOnlySpan<byte>)(value.GetBuffer()), size);
            }
            finally
            {
                if (value2 != null)
                {
                    value2.Dispose();
                }
            }
        }
        finally
        {
            if (value != null)
            {
                ((Stream)value).Dispose();
            }
        }
        return result;
    }

    // intf_P5_04.inc:29

    // intf_P5_04.inc:29

}
