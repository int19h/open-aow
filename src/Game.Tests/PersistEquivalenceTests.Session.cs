using System.Reflection;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;

using Aow.Rtl;

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
        var proposal = DiplomaticProposal.FromFields((byte)kind, gold: gold, mana: mana) with { Message = ["Hail", "Neighbor"] };
        var classValue = s_schema.Get("DiplomaticAction");
        var action = new DiplomaticAction(null!) { Holder = EngineFixture.Engine, Proposal = proposal, Source = 1, Target = 2, OtherPlayer = 3, Day = 9 };
        var document = DocumentBackend.Write(TreeWriter.Write(action, factory: Factory), classValue: classValue, schema: s_schema, engine: "game");
        Assert.IsType<JsonString>(document["$case"]);
        Check("DiplomaticAction", objectValue: action);
        using var loaded = new DiplomaticAction(null!) { Holder = EngineFixture.Engine };
        TreeReader.Read(DocumentBackend.Read(document, classValue: classValue, schema: s_schema, engine: "game"), objectValue: loaded, factory: Factory);
        Assert.Equal(proposal, loaded.Proposal);
    }

    // While the map is edited, Player.ReadWrite stores the chosen leader where the leader setup
    // goes. Otherwise it stores the leader setup. Both writes and both loads agree on each side.
    [Fact]
    public void AnEditedPlayerStoresItsLeaderAsTheLeaderSetupOnBothPaths()
    {
        WithDestroyingMap(map =>
        {
            var classValue = s_schema.Get("Player");
            var editing = map.Status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Editting);
            var plain = map.Status;
            map.Status = editing;
            var edited = NewPlayer(index: 1);
            var leader = new Leader(null!) { Holder = EngineFixture.Engine, HeroName = "Chosen" };
            edited.Leader = leader;
            var editedDocument = DocumentBackend.Write(TreeWriter.Write(edited, factory: Factory), classValue: classValue, schema: s_schema, engine: "game");
            Check("Player", objectValue: edited, converterRead: false);
            map.Status = plain;
            var stored = NewPlayer(index: 1);
            stored.Leader = leader;
            var storedDocument = DocumentBackend.Write(TreeWriter.Write(stored, factory: Factory), classValue: classValue, schema: s_schema, engine: "game");
            Check("Player", objectValue: stored, converterRead: false);
            Assert.NotEqual(JsonWriter.ToUtf8(editedDocument["LeaderSetup"]!), JsonWriter.ToUtf8(storedDocument["LeaderSetup"]!));
            leader.Dispose();
        });
    }

    // Player.ReadWrite stores the leader setup only for a player other than 0. A stored leader
    // setup of player 0 stays unread on both loads.
    [Fact]
    public void PlayerZeroLeavesAStoredLeaderSetupUnreadOnBothLoads()
    {
        WithDestroyingMap(_ =>
        {
            using var source = NewPlayer(index: 1);
            source.LeaderSetup.HeroName = "Setup";
            var document = PilotImage.Decode(Image(source), className: "Player");
            Assert.NotNull(document["LeaderSetup"]);
            document.Members.RemoveAll(member => member.Name == "PlayerIndex");
            var image = PilotImage.Encode(document, className: "Player");
            using var viaReadWrite = NewPlayer(index: -1);
            using var viaPersist = NewPlayer(index: -1);
            Load(viaReadWrite, image: image);
            TreeReader.Read(OriginalBackend.Read(image, classValue: s_schema.Get("Player"), schema: s_schema, engine: "game"), objectValue: viaPersist, factory: Factory);
            Assert.Equal(0, viaPersist.PlayerIndex);
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
            map.SeatedPlayer = 2;
            if (network) map.ConnectionType = PlayerConnectionType.Network;
            var player = NewPlayer(index: 2);
            player.BaseHeroJoinDay = 6;
            var document = DocumentBackend.Write(TreeWriter.Write(player, factory: Factory), classValue: s_schema.Get("Player"), schema: s_schema, engine: "game");
            Assert.Equal(network, document["ViewX"] is null);
            Assert.Equal(network, document["BaseHeroJoinDay"] is null);
            Check("Player", objectValue: player, converterRead: false);
        });
    }

    // A preview load leaves the stored event logbook unread on both paths.
    [Fact]
    public void APreviewLoadOfAPlayerLeavesItsLogbookUnreadOnBothPaths()
    {
        WithDestroyingMap(_ =>
        {
            using var source = NewPlayer(index: 1);
            var image = Image(source);
            using var viaReadWrite = NewPlayer(index: -1);
            using var viaPersist = NewPlayer(index: -1);
            LoadPreview(viaReadWrite, image: image);
            TreeReader.Read(OriginalBackend.Read(image, classValue: s_schema.Get("Player"), schema: s_schema, engine: "game"), objectValue: viaPersist, factory: Factory, preview: true);
            EqualBytes(Image(viaReadWrite), actual: Image(viaPersist), label: "Player preview read");
        });
    }

    // Player.ReadWrite reads a stored leader (property 19) only while the engine loads or saves a
    // map, and never writes one. Otherwise both loads leave it unread. A preview load while the
    // engine loads a map takes the leader on both paths without registering it.
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void APlayerTakesAStoredLeaderOnlyWhileTheEngineLoadsAMap(bool loadingMap)
    {
        WithDestroyingMap(_ =>
        {
            using var source = NewPlayer(index: 1);
            var document = PilotImage.Decode(Image(source), className: "Player");
            using (var leader = new Leader(null!) { Holder = EngineFixture.Engine, HeroName = "Stored" })
            {
                var leaderDocument = PilotImage.Decode(Image(leader), className: "Leader");
                leaderDocument.Members.Insert(0, item: new JsonMember("$class", value: new JsonString("Leader")));
                int at = document.Members.FindIndex(member => member.Name == "ViewX");
                document.Members.Insert(at < 0 ? document.Members.Count : at, item: new JsonMember("Leader", value: leaderDocument));
            }
            var image = PilotImage.Encode(document, className: "Player");
            using var viaReadWrite = NewPlayer(index: -1);
            using var viaPersist = NewPlayer(index: -1);
            WithEngineStatus(loadingMap ? Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm) : default, body: () =>
            {
                LoadPreview(viaReadWrite, image: image);
                TreeReader.Read(OriginalBackend.Read(image, classValue: s_schema.Get("Player"), schema: s_schema, engine: "game"), objectValue: viaPersist, factory: Factory, preview: true);
            });
            Assert.Equal(loadingMap, viaReadWrite.Leader is not null);
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
        var settings = new CampaignTransferSettings(null!) { Holder = EngineFixture.Engine };
        var leader = new Leader(null!) { Holder = EngineFixture.Engine, HeroName = "Carried" };
        settings.SetLeader(leader);
        leader.Release();
        Check("CampaignTransferSettings", objectValue: settings);
    }

    // Race.ReadWrite sets the enabled flag through SetEnabled on a load, and an empty default
    // relation list resets the defaults, so a load does not give back the converter's image.
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ARaceMatchesTheConverter(bool enabled)
    {
        var race = new Race(null!) { Holder = EngineFixture.Engine, RaceIndex = 3, Enabled = enabled };
        Check("Race", objectValue: race, converterRead: false);
    }

    // A campaign with no map stores no map member on either path, and its load gives each
    // scenario without an id a unique one.
    [Fact]
    public void ACampaignMatchesTheConverter()
    {
        var campaign = new Campaign(null!) { Holder = EngineFixture.Engine, CurrentScenarioId = 2, ScenarioStarted = true };
        campaign.CompletedScenarios.Add(1);
        Check("Campaign", objectValue: campaign);
    }

    // The event log list releases each loaded log once, on both loads.
    [Fact]
    public void AnEventLogListWithLogsMatchesTheConverter()
    {
        var list = new EventLogList(null!) { Holder = EngineFixture.Engine };
        new TextEventLog(null!) { Holder = EngineFixture.Engine, Text = "First" }.SetOwner(list);
        new TextEventLog(null!) { Holder = EngineFixture.Engine, Text = "Second" }.SetOwner(list);
        Check("EventLogList", objectValue: list);
    }

    // Every class of the family on the tree, blank: the tree write matches the converter's
    // document and the stream image, and a load of that image ends alike on both paths, with the
    // same state or the same error. The player's write reads the current map.
    [Theory]
    [MemberData(nameof(SessionClasses))]
    public void ASessionClassWritesAndLoadsAlikeOnBothPaths(string className)
    {
        var enlisted = EnlistSessionClasses();
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
        WithDestroyingMap(_ =>
        {
            var classValue = s_schema.Get(className);
            // The campaign classes belong to the campaign engine's table only.
            string engine = classValue.Engines.Count == 0 || classValue.Engines.Contains("game") ? "game" : classValue.Engines[0];
            var source = Blank(className);
            EngineObject? viaReadWrite = null;
            EngineObject? viaPersist = null;
            try
            {
                byte[]? image = null;
                ObjectNode? tree = null;
                AlikeOutcome(() => image = Image(source), viaPersist: () => tree = TreeWriter.Write(source, factory: Factory), label: className + " write");
                if (image is null || tree is null) return;
                EqualJson(PilotImage.Decode(image, className: className, engine: engine), actual: DocumentBackend.Write(tree, classValue: classValue, schema: s_schema, engine: engine), label: className + " write");
                EqualBytes(image, actual: OriginalBackend.Write(tree, classValue: classValue, schema: s_schema, engine: engine), label: className + " original");
                var stream = viaReadWrite = Blank(className);
                var persisted = viaPersist = Blank(className);
                bool loaded = AlikeOutcome(() => Load(stream, image: image), viaPersist: () => TreeReader.Read(OriginalBackend.Read(image, classValue: classValue, schema: s_schema, engine: engine), objectValue: persisted, factory: Factory), label: className + " load");
                if (loaded) EqualBytes(Image(stream), actual: Image(persisted), label: className + " read");
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
        var names = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var engineClass in EngineClass.All)
        {
            var type = engineClass.Type;
            if (type.Assembly != typeof(GamePack).Assembly || type.IsAbstract) continue;
            if (type.Namespace is not { } space) continue;
            if (type != typeof(EmailGame) && !s_sessionNamespaces.Any(prefix => space == prefix || space.StartsWith(prefix + ".", comparisonType: StringComparison.Ordinal))) continue;
            if (space.StartsWith("Aow.Game.Magic.Abilities", comparisonType: StringComparison.Ordinal)) continue;
            if (EngineObject.TreePersistenceRefusal(type) is not null) continue;
            if (FormatSchema.Current.Find(engineClass.StorageKey) is null) continue;
            names.Add(engineClass.StorageKey);
        }
        var data = new TheoryData<string>();
        foreach (var name in names) data.Add(name);
        return data;
    }

    // The game enlists the campaign pages and the other classes of these namespaces when its
    // screens start, and the fixture engine holds only the classes of the engine's start. Each
    // class id is enlisted once.
    private static EngineClass[] EnlistSessionClasses()
    {
        var table = EngineFixture.Engine.ClassTable;
        var added = new Dictionary<int, EngineClass>();
        foreach (var engineClass in EngineClass.All)
        {
            if (engineClass.Type.Assembly != typeof(GamePack).Assembly || engineClass.Type.Namespace is not { } space) continue;
            if (!s_sessionNamespaces.Any(prefix => space == prefix || space.StartsWith(prefix + ".", comparisonType: StringComparison.Ordinal))) continue;
            if (table.ContainsKey(engineClass.ClassId) || added.ContainsKey(engineClass.ClassId)) continue;
            added[engineClass.ClassId] = engineClass;
        }
        var classes = added.Values.ToArray();
        EngineFixture.Engine.RegisterEngineClasses(classes);
        return classes;
    }

    private static Player NewPlayer(int index) =>
        new(null!) { Holder = EngineFixture.Engine, PlayerIndex = (sbyte)index };

    // The partial storage reader, as a preview load reads a map.
    private static void LoadPreview(EngineObject objectValue, byte[] image)
    {
        using var memory = new MemoryStream(image);
        var reader = new Aow.Engine.Storage.StorageReader(memory, engine: EngineFixture.Engine, partial: true);
        try
        {
            reader.PersistObject(objectValue);
        }
        finally
        {
            reader.Dispose();
        }
    }

    // The engine status is private state of the engine's map loads. The test sets it around the
    // body and puts the previous status back.
    private static void WithEngineStatus(Set<HexSpriteEngineStatusElement> status, Action body)
    {
        var field = typeof(GameEngine).GetField("_status", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance)
            ?? throw new InvalidOperationException("GameEngine has no _status field.");
        var previous = field.GetValue(EngineFixture.Engine);
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
