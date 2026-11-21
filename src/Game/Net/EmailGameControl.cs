#nullable disable warnings
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiStreams;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.Net;

// games
public sealed class EmailGameControl : EngineObject
{
    // Diplomacy.pas:2489
    public EmailGameControl(CustomNode? owner) : base(owner) => _games = new EmailGameList(null) { Holder = CreationHolder() };

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
        _games.Clear();
        string fileName = $"{GetEngine().StartupDirectory}EmailIn\\egames.pfs";
        if (RtlHost.Files.FileExists(fileName))
        {
            if (GetFileCrc32(fileName) == GetEngine().Registry.GetEmailGamesCrc())
            {
                GetEngine().ReadFromFile(fileName, _games, progress: null);
            }
        }
    }

    // Diplomacy.pas:2514
    public void Save()
    {
        try
        {
            string fileName = $"{GetEngine().StartupDirectory}EmailIn\\egames.pfs";
            GetEngine().WriteToFile($"{GetEngine().StartupDirectory}EmailIn\\egames.pfs", _games, progress: null);
            int fileCrc = GetFileCrc32(fileName);
            GetEngine().Registry.SetEmailGamesCrc(fileCrc);
        }
        catch
        {
        }
    }

    // Diplomacy.pas:2525
    public EmailGame? FindEmailGame(EmailGame game)
    {
        int index = _games.IndexOfUniqueLocalId(game.UniqueLocalId);
        return index != -1 && game.UniqueId != 0 && _games.GetEmailGames(index).UniqueId == game.UniqueId
            ? _games.GetEmailGames(index)
            : null;
    }

    // Diplomacy.pas:2535
    public bool EmailGameRestarted(EmailGame game)
    {
        if (game.SecurityId != 0)
        {
            EmailGame? localGame = FindEmailGame(game);
            return localGame is null || game.SecurityId != localGame.SecurityId;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:2546
    public int GenerateSecurityId() => Random(PixelFormats.ElapsedMilliseconds & 0xFF_FF_FF) + 1;

    // Diplomacy.pas:2549
    public bool UpdateGame(EmailGame game)
    {
        EmailGame? localGame = FindEmailGame(game);
        if (localGame is not null)
        {
            localGame.LastDay = game.LastDay;
            localGame.SecurityId = GenerateSecurityId();
            game.SecurityId = localGame.SecurityId;
            Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:2564
    public EmailGame CreateEmailGame(string name, string emailAddress)
    {
        EmailGame result = new(null)
        {
            Holder = CreationHolder(),
            EmailAddress = emailAddress,
            UniqueLocalId = CreateUniqueLocalId(),
            UniqueId = Random(PixelFormats.ElapsedMilliseconds & 0xFF_FF_FF) + 1,
            SecurityId = GenerateSecurityId(),
            Name = name
        };
        result.SetOwner(_games);
        Save();
        return result;
    }

    // Diplomacy.pas:2576
    public string ValidateIncomingGame(string fileName, string destination)
    {
        // { Addressability survives elimination of this assignment. }
        try
        {
            destination = "";
            StrategicMap map = GetEngine().CreatePreviewHsm<StrategicMap>(fileName, profile: GameProfile.WorldMap);
            bool condition = map is not null && map.ConnectionType == PlayerConnectionType.Email && map.CurrentPlayer > 0;
            if (condition)
            {
                int count = map.Players.Count;
                condition = map.CurrentPlayer < count;
            }

            if (condition && map.Players.GetPlayers(map.CurrentPlayer).PbemSettings.Status.Contains(PbemPlayerStatuses.NewTurn))
            {
                if (CompareText(b: GetEngine().Registry.GetLocalEmailAddress(), a: map.Players.GetPlayers(map.CurrentPlayer).PbemSettings.EmailGame.EmailAddress) == 0)
                {
                    EmailGame? localGame = FindEmailGame(map.Players.GetPlayers(map.CurrentPlayer).PbemSettings.EmailGame);
                    string name = "";
                    bool overwrite = false;
                    if (localGame is not null)
                    {
                        if (map.Day > localGame.LastDay)
                        {
                            name = map.Players.GetPlayers(map.CurrentPlayer).PbemSettings.EmailGame.Name;
                            if (RtlHost.Files.FileExists($"{GetEngine().StartupDirectory}EmailIn\\{name}.asg"))
                            {
                                StrategicMap previousMap = GetEngine().CreatePreviewHsm<StrategicMap>($"{GetEngine().StartupDirectory}EmailIn\\{name}.asg", profile: GameProfile.WorldMap);
                                if (previousMap is not null)
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
                                        Player? players = previousMap.Players.GetPlayers(previousMap.CurrentPlayer);
                                        Player? fPlayersPlayers = map.Players.GetPlayers(map.CurrentPlayer);
                                        bool passed = players.PbemSettings.EmailGame.UniqueLocalId == fPlayersPlayers.PbemSettings.EmailGame.UniqueLocalId;
                                        if (passed)
                                        {
                                            Player? players2 = previousMap.Players.GetPlayers(previousMap.CurrentPlayer);
                                            Player? players3 = map.Players.GetPlayers(map.CurrentPlayer);
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
                        int i = 0;
                        destination = $"{GetEngine().StartupDirectory}EmailIn\\{name}.asg";
                        while (!overwrite && RtlHost.Files.FileExists(destination))
                        {
                            i += 1;
                            destination = $"{GetEngine().StartupDirectory}EmailIn\\{name}{i.ToString(CultureInfo.InvariantCulture)}.asg";
                        }
                    }
                }
            }

            map?.Dispose();
        }
        catch
        {
        }
        return destination;
    }

    private static int GetFileCrc32(string fileName)
    {
        MemoryStream value = new();
        try
        {
            Stream value2 = OpenFile(fileName, mode: FileOpenMode.Read);
            try
            {
                StreamExtensions.LoadFromStream(value, source: value2);
                return EngineServices.GetCrc32(size: (int)value.Length, buffer: (ReadOnlySpan<byte>)value.GetBuffer());
            }
            finally
            {
                value2?.Dispose();
            }
        }
        finally
        {
            value?.Dispose();
        }
    }
}
