#nullable disable warnings
using System.Reflection;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game.Audio;
using Aow.Game.Commands;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// PE-3-d: the classes of Game/Session, Game/Events, Game/AI, Game/Magic, Game/UI and Game/Audio
// whose instances the installed corpus does not prove, and both sides of each branch that depends
// on game state.
public sealed partial class PersistEquivalenceTests
{
    // Every case of the diplomatic proposal mapping and a kind no case registers. The kind is one
    // stored byte, the document names the case it selects, and both loads give the same proposal.
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 100, 20)]
    [InlineData(2, 50, 0)]
    [InlineData(3, 0, 0)]
    [InlineData(4, 0, 0)]
    [InlineData(5, 0, 30)]
    [InlineData(6, 0, 0)]
    [InlineData(7, 0, 0)]
    [InlineData(8, 10, 10)]
    [InlineData(9, 70, 5)]
    [InlineData(77, 3, 4)]
    public void ADiplomaticActionStoresItsCaseOnBothPaths(int kind, int gold, int mana)
    {
        DiplomaticProposal proposal = DiplomaticProposal.FromFields(kind.TruncateToByte(), gold: gold, mana: mana) with { Message = ["Hail", "Neighbor"] };
        SchemaClass classValue = s_schema.Get("DiplomaticAction");
        DiplomaticAction action = new(null) { Holder = EngineFixture.Engine, Proposal = proposal, Source = PlayerId.Player1, Target = PlayerId.Player2, OtherPlayer = PlayerId.Player3, Day = 9 };
        JsonObject document = DocumentBackend.Write(TreeWriter.Write(action, factory: Factory), classValue: classValue, schema: s_schema, engine: "game");
        _ = Assert.IsType<JsonString>(document["$case"]);
        Check("DiplomaticAction", objectValue: action);
        using DiplomaticAction loaded = new(null) { Holder = EngineFixture.Engine };
        TreeReader.Read(DocumentBackend.Read(document, classValue: classValue, schema: s_schema, engine: "game"), objectValue: loaded, factory: Factory);
        Assert.Equal(proposal, loaded.Proposal);
    }

    // While the map is edited, the original Player.ReadWrite stores the chosen leader where the
    // leader setup goes. Otherwise it stores the leader setup. Both writes and both loads agree on
    // each side.
    [Fact]
    public void AnEditedPlayerStoresItsLeaderAsTheLeaderSetupOnBothPaths()
    {
        WithDestroyingMap(map =>
        {
            SchemaClass classValue = s_schema.Get("Player");
            HexSpriteMapStatuses editing = map.Status | HexSpriteMapStatuses.Editting;
            HexSpriteMapStatuses plain = map.Status;
            map.Status = editing;
            Player edited = NewPlayer(index: 1);
            Leader leader = new(null) { Holder = EngineFixture.Engine, HeroName = "Chosen" };
            edited.Leader = leader;
            JsonObject editedDocument = DocumentBackend.Write(TreeWriter.Write(edited, factory: Factory), classValue: classValue, schema: s_schema, engine: "game");
            Check("Player", objectValue: edited, converterRead: false);
            map.Status = plain;
            Player stored = NewPlayer(index: 1);
            stored.Leader = leader;
            JsonObject storedDocument = DocumentBackend.Write(TreeWriter.Write(stored, factory: Factory), classValue: classValue, schema: s_schema, engine: "game");
            Check("Player", objectValue: stored, converterRead: false);
            Assert.NotEqual(JsonWriter.ToUtf8(editedDocument["LeaderSetup"]), JsonWriter.ToUtf8(storedDocument["LeaderSetup"]));
            leader.Dispose();
        });
    }

    // Issue 396: the original's load of a short AIValidAttackPlayers takes the start of the
    // PbemSettings image into the bits past the stored ones. The port fixes that bug
    // (deviations.md): the game's loads from an image and from a document take the stored bits
    // only. A load that emulates the original takes the bytes from the image, and the owner rebuild
    // of PersistSupport.BitListOverRead gives a document load the same bytes. A node whose read
    // past differs from those members shows that an emulated image load takes the image's bytes.
    [Fact]
    public void APlayerLoadTakesTheAttackPlayersReadPastOnlyWhenItEmulatesTheOriginal()
    {
        WithDestroyingMap(_ =>
        {
            SchemaClass classValue = s_schema.Get("Player");
            EnginePersistableFactory emulating = new(EngineFixture.Engine) { EmulateOriginalBugs = true };
            using Player source = NewPlayer(index: 1);
            source.AIValidAttackPlayers[12] = true;
            Assert.Equal(13, source.AIValidAttackPlayers.Count);
            ObjectNode tree = OriginalBackend.Read(Image(source), classValue: classValue, schema: s_schema, engine: "game");
            byte[] readPast = ((BitListNode)tree.Find("AIValidAttackPlayers")).ReadPast.Value.ToArray();
            Assert.Equal(2, readPast.Length);
            Assert.NotEqual(new byte[2], readPast);
            ObjectNode documentTree = DocumentBackend.Read(DocumentBackend.Write(tree, classValue: classValue, schema: s_schema, engine: "game"), classValue: classValue, schema: s_schema, engine: "game");
            foreach (ObjectNode read in new[] { tree, documentTree })
            {
                using Player normal = NewPlayer(index: 1);
                TreeReader.Read(read, objectValue: normal, factory: Factory);
                Assert.Equal(32, normal.AIValidAttackPlayers.Count);
                Assert.Equal(new byte[2], normal.AIValidAttackPlayers.List.Span[2..4].ToArray());
                using Player emulated = NewPlayer(index: 1);
                TreeReader.Read(read, objectValue: emulated, factory: emulating);
                Assert.Equal(32, emulated.AIValidAttackPlayers.Count);
                Assert.Equal(readPast, emulated.AIValidAttackPlayers.List.Span[2..4].ToArray());
            }

            ObjectNode altered = new(tree.ClassName, slot: tree.Slot);
            foreach ((string name, TreeValue value) in tree.Members)
            {
                altered.Add(name, value: value is BitListNode node && name == "AIValidAttackPlayers"
                    ? new BitListNode(node.Default, node.Count, indices: [.. node.Indices]) { ReadPast = new byte[] { 0xAA, 0xBB } }
                    : value);
            }

            using Player viaAltered = NewPlayer(index: 1);
            TreeReader.Read(altered, objectValue: viaAltered, factory: emulating);
            Assert.Equal([0xAA, 0xBB], viaAltered.AIValidAttackPlayers.List.Span[2..4].ToArray());
        });
    }

    // The original Player.ReadWrite stores the leader setup only for a player other than 0. A
    // stored leader setup of player 0 stays unread on both loads.
    [Fact]
    public void PlayerZeroLeavesAStoredLeaderSetupUnreadOnBothLoads()
    {
        WithDestroyingMap(destroyingMap =>
        {
            using Player source = NewPlayer(index: 1);
            source.LeaderSetup.HeroName = "Setup";
            JsonObject document = PilotImage.Decode(Image(source), className: "Player");
            Assert.NotNull(document["LeaderSetup"]);
            _ = document.Members.RemoveAll(member => member.Name == "PlayerIndex");
            byte[] image = PilotImage.Encode(document, className: "Player");
            using Player viaReadWrite = NewPlayer(index: -1);
            using Player viaPersist = NewPlayer(index: -1);
            Load(viaReadWrite, image: image);
            TreeReader.Read(OriginalBackend.Read(image, classValue: s_schema.Get("Player"), schema: s_schema, engine: "game"), objectValue: viaPersist, factory: Factory);
            Assert.Equal(0, PlayerIdExtensions.ToOriginal(viaPersist.PlayerIndex));
            Assert.NotEqual("Setup", viaPersist.LeaderSetup.HeroName);
            EqualBytes(Image(viaReadWrite), actual: Image(viaPersist), label: "Player 0 read");
        });
    }

    // The seated player of a local map stores the map's view center. A network map leaves out
    // the view and the hero join day, and both loads read them when they are present.
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ASeatedPlayerStoresItsViewOnALocalMapOnBothPaths(bool network)
    {
        WithDestroyingMap(map =>
        {
            map.SeatedPlayer = PlayerId.Player2;
            if (network)
            {
                map.ConnectionType = PlayerConnectionType.Network;
            }

            Player player = NewPlayer(index: 2);
            player.BaseHeroJoinDay = 6;
            JsonObject document = DocumentBackend.Write(TreeWriter.Write(player, factory: Factory), classValue: s_schema.Get("Player"), schema: s_schema, engine: "game");
            Assert.Equal(network, actual: document["View"] is null);
            Assert.Equal(network, actual: document["BaseHeroJoinDay"] is null);
            Check("Player", objectValue: player, converterRead: false);
        });
    }

    // A preview load leaves the stored event logbook unread on both paths.
    [Fact]
    public void APreviewLoadOfAPlayerLeavesItsLogbookUnreadOnBothPaths()
    {
        WithDestroyingMap(_ =>
        {
            using Player source = NewPlayer(index: 1);
            byte[] image = Image(source);
            using Player viaReadWrite = NewPlayer(index: -1);
            using Player viaPersist = NewPlayer(index: -1);
            LoadPreview(viaReadWrite, image: image);
            TreeReader.Read(OriginalBackend.Read(image, classValue: s_schema.Get("Player"), schema: s_schema, engine: "game"), objectValue: viaPersist, factory: Factory, preview: true);
            EqualBytes(Image(viaReadWrite), actual: Image(viaPersist), label: "Player preview read");
        });
    }

    // The original Player.ReadWrite reads a stored leader (property 19) only while the engine loads
    // or saves a map, and never writes one. Otherwise both loads leave it unread. A preview load
    // while the engine loads a map takes the leader on both paths without registering it.
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void APlayerTakesAStoredLeaderOnlyWhileTheEngineLoadsAMap(bool loadingMap)
    {
        WithDestroyingMap(_ =>
        {
            using Player source = NewPlayer(index: 1);
            JsonObject document = PilotImage.Decode(Image(source), className: "Player");
            using (Leader leader = new(null) { Holder = EngineFixture.Engine, HeroName = "Stored" })
            {
                JsonObject leaderDocument = PilotImage.Decode(Image(leader), className: "Leader");
                leaderDocument.Members.Insert(0, item: new JsonMember("$class", value: new JsonString("Leader")));
                int at = document.Members.FindIndex(member => member.Name == "View");
                document.Members.Insert(at < 0 ? document.Members.Count : at, item: new JsonMember("Leader", value: leaderDocument));
            }
            byte[] image = PilotImage.Encode(document, className: "Player");
            using Player viaReadWrite = NewPlayer(index: -1);
            using Player viaPersist = NewPlayer(index: -1);
            WithEngineStatus(loadingMap ? HexSpriteEngineStatuses.LoadingHsm : default, body: () =>
            {
                LoadPreview(viaReadWrite, image: image);
                TreeReader.Read(OriginalBackend.Read(image, classValue: s_schema.Get("Player"), schema: s_schema, engine: "game"), objectValue: viaPersist, factory: Factory, preview: true);
            });
            Assert.Equal(loadingMap, actual: viaReadWrite.Leader is not null);
            Assert.Equal(viaReadWrite.Leader?.HeroName, viaPersist.Leader?.HeroName);
            EqualBytes(Image(viaReadWrite), actual: Image(viaPersist), label: "Player leader read");
            viaReadWrite.Leader?.Dispose();
            viaPersist.Leader?.Dispose();
        });
    }

    // On load CampaignTransferSettings hands the stored leader to SetLeader, which takes its own
    // reference, and releases the one the load made.
    [Fact]
    public void CampaignTransferSettingsWithALeaderMatchesTheConverter()
    {
        CampaignTransferSettings settings = new(null) { Holder = EngineFixture.Engine };
        Leader leader = new(null) { Holder = EngineFixture.Engine, HeroName = "Carried" };
        settings.SetLeader(leader);
        _ = leader.Release();
        Check("CampaignTransferSettings", objectValue: settings);
    }

    // The original Race.ReadWrite sets the enabled flag through SetEnabled on a load, and an empty
    // default relation list resets the defaults, so a load does not give back the converter's
    // image.
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ARaceMatchesTheConverter(bool enabled)
    {
        Race race = new(null) { Holder = EngineFixture.Engine, RaceIndex = 3, Enabled = enabled };
        Check("Race", objectValue: race, converterRead: false);
    }

    // A campaign with no map stores no map member on either path, and its load gives each
    // scenario without an id a unique one.
    [Fact]
    public void ACampaignMatchesTheConverter()
    {
        Campaign campaign = new(null) { Holder = EngineFixture.Engine, CurrentScenarioId = 2, ScenarioStarted = true };
        _ = campaign.CompletedScenarios.Add(1);
        Check("Campaign", objectValue: campaign);
    }

    // The event log list releases each loaded log once, on both loads.
    [Fact]
    public void AnEventLogListWithLogsMatchesTheConverter()
    {
        EventLogList list = new(null) { Holder = EngineFixture.Engine };
        new TextEventLog(null) { Holder = EngineFixture.Engine, Text = "First" }.SetOwner(list);
        new TextEventLog(null) { Holder = EngineFixture.Engine, Text = "Second" }.SetOwner(list);
        Check("EventLogList", objectValue: list);
    }

    // Every class of the family on the tree, blank: the tree write matches the converter's
    // document and the stream image, and a load of that image ends alike on both paths, with the
    // same state or the same error. The player's write reads the current map.
    [Theory]
    [MemberData(nameof(SessionClasses))]
    public void ASessionClassWritesAndLoadsAlikeOnBothPaths(string className)
    {
        EngineClass[] enlisted = EnlistSessionClasses();
        try
        {
            WriteAndLoadAlike(className);
        }
        finally
        {
            EngineFixture.Engine.UnregisterEngineClasses(enlisted);
        }
    }

    private static void WriteAndLoadAlike(string className)
    {
        WithDestroyingMap(destroyingMap =>
        {
            SchemaClass classValue = s_schema.Get(className);
            // The campaign classes belong to the campaign engine's table only.
            string engine = classValue.Engines.Count == 0 || classValue.Engines.Contains("game") ? "game" : classValue.Engines[0];
            EngineObject source = Blank(className);
            EngineObject? viaReadWrite = null;
            EngineObject? viaPersist = null;
            try
            {
                byte[]? image = null;
                ObjectNode? tree = null;
                _ = AlikeOutcome(() => image = Image(source), viaPersist: () => tree = TreeWriter.Write(source, factory: Factory), label: $"{className} write");
                if (image is null || tree is null)
                {
                    return;
                }

                EqualJson(PilotImage.Decode(image, className: className, engine: engine), actual: DocumentBackend.Write(tree, classValue: classValue, schema: s_schema, engine: engine), label: $"{className} write");
                EqualBytes(image, actual: OriginalBackend.Write(tree, classValue: classValue, schema: s_schema, engine: engine), label: $"{className} original");
                EngineObject stream = viaReadWrite = Blank(className);
                EngineObject persisted = viaPersist = Blank(className);
                bool loaded = AlikeOutcome(() => Load(stream, image: image), viaPersist: () => TreeReader.Read(OriginalBackend.Read(image, classValue: classValue, schema: s_schema, engine: engine), objectValue: persisted, factory: Factory), label: $"{className} load");
                if (loaded)
                {
                    EqualBytes(Image(stream), actual: Image(persisted), label: $"{className} read");
                }
            }
            finally
            {
                Discard(viaPersist);
                Discard(viaReadWrite);
                Discard(source);
            }
        });
    }

    private static readonly string[] s_sessionNamespaces =
        ["Aow.Game.Session", "Aow.Game.Events", "Aow.Game.AI", "Aow.Game.Magic", "Aow.Game.UI", "Aow.Game.Audio"];

    public static TheoryData<string> SessionClasses()
    {
        SortedSet<string> names = [with(StringComparer.Ordinal)];
        foreach (EngineClass engineClass in EngineClass.All)
        {
            Type type = engineClass.Type;
            if (type.Assembly != typeof(GamePack).Assembly || type.IsAbstract)
            {
                continue;
            }

            if (type.Namespace is not { } space)
            {
                continue;
            }

            if (type != typeof(EmailGame) && !s_sessionNamespaces.Any(prefix => space == prefix || space.StartsWith($"{prefix}.", comparisonType: StringComparison.Ordinal)))
            {
                continue;
            }

            if (space.StartsWith("Aow.Game.Magic.Abilities", comparisonType: StringComparison.Ordinal))
            {
                continue;
            }

            if (EngineObject.TreePersistenceRefusal(type) is not null)
            {
                continue;
            }

            if (FormatSchema.Current.Find(engineClass.StorageKey) is null)
            {
                continue;
            }

            _ = names.Add(engineClass.StorageKey);
        }
        TheoryData<string> data = [.. names];

        return data;
    }

    // The game enlists the campaign pages and the other classes of these namespaces when its
    // screens start, and the fixture engine holds only the classes of the engine's start. Each
    // class id is enlisted once.
    private static EngineClass[] EnlistSessionClasses()
    {
        Dictionary<int, EngineClass?> table = EngineFixture.Engine.ClassTable;
        Dictionary<int, EngineClass> added = [];
        foreach (EngineClass engineClass in EngineClass.All)
        {
            if (engineClass.Type.Assembly != typeof(GamePack).Assembly || engineClass.Type.Namespace is not { } space)
            {
                continue;
            }

            if (!s_sessionNamespaces.Any(prefix => space == prefix || space.StartsWith($"{prefix}.", comparisonType: StringComparison.Ordinal)))
            {
                continue;
            }

            if (table.ContainsKey(engineClass.ClassId) || added.ContainsKey(engineClass.ClassId))
            {
                continue;
            }

            added[engineClass.ClassId] = engineClass;
        }
        EngineClass[] classes = [.. added.Values];
        EngineFixture.Engine.RegisterEngineClasses(classes);
        return classes;
    }

    // The translate stream visits this family through its tree methods (issue 185 PE-3-i part 3),
    // under the conditions of the original ReadWrite: a player leaves out its settings and keeps
    // its view, an event log list visits its logs' texts inside BeginUpdate and EndUpdate and
    // keeps them, a diplomatic action builds its proposal with the translated message, a
    // campaign page leaves out its image library and a sound leaves out its file name.
    [Fact]
    public void TheTranslateStreamVisitsTheSessionStringsTheOriginalReadWriteVisited()
    {
        WithDestroyingMap(map =>
        {
            Dictionary<string, string> dictionary = new()
            {
                ["Lord"] = "Herr",
                ["Easy"] = "Leicht",
                ["First"] = "Erste",
                ["Hail"] = "Heil",
                ["Intro"] = "Einleitung",
                ["Once upon a time"] = "Es war einmal",
                ["pages.ilb"] = "seiten.ilb",
                ["bell.wav"] = "glocke.wav",
            };
            map.SeatedPlayer = PlayerId.Player2;
            using Player player = NewPlayer(index: 2);
            player.Settings = "Easy";
            player.View = player.View with { X = 7 };
            typeof(Player).GetField("_description", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance).SetValue(player, value: "Lord");
            using (DictionaryStream stream = new(dictionary))
            {
                stream.Object(player, id: 0);
                Assert.Contains("Lord", collection: stream.Visited);
                Assert.DoesNotContain("Easy", collection: stream.Visited);
                Assert.Contains("EventLogbook", collection: stream.Objects);
                Assert.Contains("DiplomaticActionList", collection: stream.Objects);
            }
            Assert.Equal("Easy", player.Settings);
            Assert.Equal(7, unchecked((byte)player.View.X));

            using EventLogList list = new(null) { Holder = EngineFixture.Engine };
            TextEventLog log = new(null) { Holder = EngineFixture.Engine, Text = "First" };
            log.SetOwner(list);
            int changes = 0;
            list.Changed += (_, _) => changes++;
            using (DictionaryStream stream = new(dictionary))
            {
                stream.Object(list, id: 0);
                Assert.Equal(["First"], stream.Visited);
                Assert.Equal(["EventLogList", "TextEventLog"], stream.Objects);
            }
            Assert.Equal("Erste", log.Text);
            Assert.Same(log, list.GetChild(0));
            Assert.Equal(1, changes);

            using DiplomaticAction action = new(null) { Holder = EngineFixture.Engine, Proposal = DiplomaticProposal.FromFields(1, gold: 100, mana: 20) with { Message = ["Hail"] } };
            using (DictionaryStream stream = new(dictionary))
            {
                stream.Object(action, id: 0);
                Assert.Equal(["lines 1"], stream.Visited);
            }
            Assert.Equal(DiplomaticProposal.FromFields(1, gold: 100, mana: 20) with { Message = ["Heil"] }, action.Proposal);

            using CampaignScenarioPage page = new(null) { Holder = EngineFixture.Engine };
            _ = page.Text.Add("Once upon a time");
            typeof(CampaignScenarioPage).GetProperty(nameof(CampaignScenarioPage.Heading)).SetValue(page, value: "Intro");
            typeof(CampaignScenarioPage).GetProperty(nameof(CampaignScenarioPage.ImageLibrary)).SetValue(page, value: "pages.ilb");
            using (DictionaryStream stream = new(dictionary))
            {
                stream.Object(page, id: 0);
                Assert.Equal(["lines 1", "Intro"], stream.Visited);
            }
            Assert.Equal("Es war einmal", page.Text[0]);
            Assert.Equal("Einleitung", page.Heading);
            Assert.Equal("pages.ilb", page.ImageLibrary);

            using SfxSequence sequence = new(null) { Holder = EngineFixture.Engine };
            Sfx sound = new(null) { Holder = EngineFixture.Engine };
            MemberNames.Field(typeof(Sfx), name: "FileName").SetValue(sound, value: "bell.wav");
            sound.SetOwner(sequence);
            using (DictionaryStream stream = new(dictionary))
            {
                stream.Object(sequence, id: 0);
                Assert.Empty(stream.Visited);
                Assert.Equal(["SfxSequence", "Sfx"], stream.Objects);
            }
            Assert.Equal("bell.wav", MemberNames.Field(typeof(Sfx), name: "FileName").GetValue(sound));
        });
    }

    // A setup message stores this family's objects through the storage streams (the setup package,
    // its settings, their remote players and the player setups). The streams store a retired
    // class through the tree and the original backend (persistence-tree.md 14), and no gate layer
    // sends a packet end to end, so the test sends a package as SetupControl.SendPackage stores it
    // and receives it as DataReceived reads it.
    [Fact]
    public void ASetupPackageWithItsSettingsRoundTripsAsAPacket()
    {
        EngineClass[] enlisted = EnlistSessionClasses();
        try
        {
            using SetupSettings settings = new(null) { Holder = EngineFixture.Engine, MapName = "Islands", FileName = "islands.hsm", RandomSeed = 77, DayLimit = 40 };
            new RemotePlayer(null) { Holder = EngineFixture.Engine, Name = "Host", RemoteId = 5, Version = 136 }.SetOwner(settings.RemotePlayers);
            new PlayerSetupSettings(null) { Holder = EngineFixture.Engine, PlayerId = PlayerId.Player1, Race = 3 }.SetOwner(settings);
            using SetupPackage package = new(null) { Holder = EngineFixture.Engine, MessageId = 5, Param1 = 2, Param2 = 9 };
            package.Data = settings.Copy(EngineFixture.Engine);
            using MemoryStream memory = new();
            EngineFixture.Engine.StoreEngineObject(memory, objectValue: package, progress: null);
            byte[] sent = memory.ToArray();
            memory.Position = 0;
            using SetupPackage received = Assert.IsType<SetupPackage>(EngineFixture.Engine.RetrieveEngineObject(memory, progress: null));
            Assert.Equal((5, 2, 9), (received.MessageId, received.Param1, received.Param2));
            SetupSettings data = Assert.IsType<SetupSettings>(received.Data);
            Assert.Equal(("Islands", "islands.hsm", 77, 40), (data.MapName, data.FileName, data.RandomSeed, data.DayLimit));
            Assert.Equal(("Host", 5, 136), (data.RemotePlayers[0].Name, data.RemotePlayers[0].RemoteId, data.RemotePlayers[0].Version));
            Assert.Equal((byte)1, (byte)PlayerIdExtensions.ToOriginal(((PlayerSetupSettings)data.GetChild(0)).PlayerId));
            Assert.True(package.Compare(received, engine: EngineFixture.Engine));
            using MemoryStream again = new();
            EngineFixture.Engine.StoreEngineObject(again, objectValue: received, progress: null);
            EqualBytes(sent, actual: again.ToArray(), label: "setup package resent");
        }
        finally
        {
            EngineFixture.Engine.UnregisterEngineClasses(enlisted);
        }
    }

    // A token event stores a diplomatic action as a created object. The token event still has
    // its ReadWrite (PE-4-a), and the action goes through the tree inside the token event's image.
    [Fact]
    public void ADiplomaticActionTokenEventRoundTripsAsAPacket()
    {
        EngineClass descriptor = EngineClass.Of<DiplomaticActionTokenEvent>();
        bool added = !EngineFixture.Engine.ClassTable.ContainsKey(descriptor.ClassId);
        if (added)
        {
            EngineFixture.Engine.RegisterEngineClasses([descriptor]);
        }

        try
        {
            using DiplomaticActionTokenEvent tokenEvent = new(null) { Holder = EngineFixture.Engine, Operation = 3 };
            tokenEvent.Action = new DiplomaticAction(null)
            {
                Holder = EngineFixture.Engine,
                Proposal = DiplomaticProposal.FromFields(1, gold: 100, mana: 20) with { Message = ["Hail", "Neighbor"] },
                Source = PlayerId.Player1,
                Target = PlayerId.Player2,
                Day = 9,
            };
            using MemoryStream memory = new();
            EngineFixture.Engine.StoreEngineObject(memory, objectValue: tokenEvent, progress: null);
            memory.Position = 0;
            using DiplomaticActionTokenEvent received = Assert.IsType<DiplomaticActionTokenEvent>(EngineFixture.Engine.RetrieveEngineObject(memory, progress: null));
            Assert.Equal(3, received.Operation);
            Assert.Equal(tokenEvent.Action.Proposal, received.Action.Proposal);
            Assert.Equal(((sbyte)1, (sbyte)2, 9), ((sbyte)PlayerIdExtensions.ToOriginal(received.Action.Source), (sbyte)PlayerIdExtensions.ToOriginal(received.Action.Target), received.Action.Day));
            EqualBytes(Image(tokenEvent.Action), actual: Image(received.Action), label: "diplomatic action received");
        }
        finally
        {
            if (added)
            {
                EngineFixture.Engine.UnregisterEngineClasses([descriptor]);
            }
        }
    }

    private static Player NewPlayer(int index)
        => new(null) { Holder = EngineFixture.Engine, PlayerIndex = PlayerId.FromOriginal((sbyte)index) };

    // A preview load, as a preview reads a map.
    private static void LoadPreview(EngineObject objectValue, byte[] image) => TreeReader.Read(node: OriginalBackend.Read(classValue: s_schema.Get(Factory.ClassName(objectValue)), image: image, schema: s_schema, engine: "game"), objectValue: objectValue, factory: Factory, preview: true);

    // The engine status is private state of the engine's map loads. The test sets it around the
    // body and puts the previous status back.
    private static void WithEngineStatus(HexSpriteEngineStatuses status, Action body)
    {
        FieldInfo field = MemberNames.Field(typeof(GameEngine), name: "Status")
            ?? throw new InvalidOperationException("GameEngine has no Status field.");
        object? previous = field.GetValue(EngineFixture.Engine);
        field.SetValue(EngineFixture.Engine, value: status);
        try
        {
            body();
        }
        finally
        {
            field.SetValue(EngineFixture.Engine, value: previous);
        }
    }
}
