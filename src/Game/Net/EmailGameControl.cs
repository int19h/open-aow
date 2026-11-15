#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats;

using Aow.Game.Draw;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net;

// games
public partial class EmailGameControl : EngineObject
{
    // Diplomacy.pas:2489
    public EmailGameControl(CustomNode owner) : base(owner) => _games = new EmailGameList(null) { Holder = CreationHolder() };

    private readonly EmailGameList _games;

    // Diplomacy.pas:2495
    public override void Dispose()
    {
        _games?.Dispose();
        base.Dispose();
    }

    // Diplomacy.pas:2498
    public int CreateUniqueLocalId()
    {
        int result = 1;
        while (_games.IndexOfUniqueLocalId(result) != -1)
        {
            result += 1;
        }

        return result;
    }

    // Diplomacy.pas:2504
    public void Load()
    {
        string fileName;
        _games.Clear();
        fileName = GetEngine().StartupDirectory + "EmailIn\\" + "egames.pfs";
        if (FileExists(fileName))
        {
            if (GetFileCrc32(fileName) == GetEngine().Registry.GetEmailGamesCrc())
            {
                GetEngine().ReadFromFile(fileName, _games, null);
            }
        }
    }

    // Diplomacy.pas:2514
    public void Save()
    {
        string fileName;
        try
        {
            fileName = GetEngine().StartupDirectory + "EmailIn\\" + "egames.pfs";
            GetEngine().WriteToFile(GetEngine().StartupDirectory + "EmailIn\\" + "egames.pfs", _games, null);
            int fileCrc = GetFileCrc32(fileName);
            GetEngine().Registry.SetEmailGamesCrc(fileCrc);
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
        index = _games.IndexOfUniqueLocalId(game.UniqueLocalId);
        if (index != -1 && game.UniqueId != 0 && _games.GetEmailGames(index).UniqueId == game.UniqueId)
        {
            result = _games.GetEmailGames(index);
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
        EmailGame result = new(null) { Holder = CreationHolder() };
        result.EmailAddress = emailAddress;
        result.UniqueLocalId = CreateUniqueLocalId();
        result.UniqueId = Random(PixelFormats.GetElapsedMilliseconds() & 16777215) + 1;
        result.SecurityId = GenerateSecurityId();
        result.Name = name;
        result.SetOwner(_games);
        Save();
        return result;
    }

    // Diplomacy.pas:2576
    public void ValidateIncomingGame(string fileName, ref string destination)
    {
        StrategicMap map;
        StrategicMap previousMap;
        EmailGame localGame;
        bool overwrite;
        int i;
        string name = "";
        // { Addressability survives elimination of this assignment. }
        try
        {
            destination = "";
            overwrite = false;
            map = (StrategicMap)GetEngine().CreatePreviewHsm(fileName, GameProfile.WorldMap);
            bool condition = map != null && map.ConnectionType == PlayerConnectionType.Email && map.CurrentPlayer > 0;
            if (condition)
            {
                int count = map.Players.Count;
                condition = map.CurrentPlayer < count;
            }

            if (condition && map.Players.GetPlayers(map.CurrentPlayer).PbemSettings.Status.Contains(PbemPlayerStatusElement.NewTurn))
            {
                if (CompareText(b: GetEngine().Registry.GetLocalEmailAddress(), a: map.Players.GetPlayers(map.CurrentPlayer).PbemSettings.EmailGame.EmailAddress) == 0)
                {
                    localGame = FindEmailGame(map.Players.GetPlayers(map.CurrentPlayer).PbemSettings.EmailGame);
                    if (localGame != null)
                    {
                        if (map.Day > localGame.LastDay)
                        {
                            name = map.Players.GetPlayers(map.CurrentPlayer).PbemSettings.EmailGame.Name;
                            if (FileExists(GetEngine().StartupDirectory + "EmailIn\\" + name + ".asg"))
                            {
                                previousMap = (StrategicMap)GetEngine().CreatePreviewHsm(GetEngine().StartupDirectory + "EmailIn\\" + name + ".asg", GameProfile.WorldMap);
                                if (previousMap != null)
                                {
                                    // { Original bug: the previous map's player is checked against the new map's count. }
                                    bool test = previousMap.CurrentPlayer > 0;
                                    if (test)
                                    {
                                        int count = map.Players.Count;
                                        test = previousMap.CurrentPlayer < count;
                                    }

                                    if (test)
                                    {
                                        Player players = previousMap.Players.GetPlayers(previousMap.CurrentPlayer);
                                        Player fPlayersPlayers = map.Players.GetPlayers(map.CurrentPlayer);
                                        bool passed = players.PbemSettings.EmailGame.UniqueLocalId == fPlayersPlayers.PbemSettings.EmailGame.UniqueLocalId;
                                        if (passed)
                                        {
                                            Player players2 = previousMap.Players.GetPlayers(previousMap.CurrentPlayer);
                                            Player players3 = map.Players.GetPlayers(map.CurrentPlayer);
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
                        i = 0;
                        destination = GetEngine().StartupDirectory + "EmailIn\\" + name + ".asg";
                        while (!overwrite && FileExists(destination))
                        {
                            i += 1;
                            destination = GetEngine().StartupDirectory + "EmailIn\\" + name + IntToStr(i) + ".asg";
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
        MemoryStream value = new();
        try
        {
            Stream value2 = OpenFile(fileName, 0);
            try
            {
                StreamExtensions.LoadFromStream(value, value2);
                result = EngineServices.GetCrc32(size: (int)value.Length, buffer: (ReadOnlySpan<byte>)value.GetBuffer());
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
                value.Dispose();
            }
        }
        return result;
    }

    // intf_P5_04.inc:29

    // intf_P5_04.inc:29

}
