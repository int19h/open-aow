using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// PE-3-b: the classes of Game/Units and Game/Magic/Abilities whose instances the installed corpus
// does not hold, and both sides of each branch that depends on game state.
public sealed partial class PersistEquivalenceTests
{
    [Fact]
    public void AnArmyWithAUnitMatchesTheConverter()
    {
        WithUnitResource(resource => WithDestroyingMap(_ =>
        {
            Army army = new(null) { Holder = EngineFixture.Engine };
            Unit unit = new(null) { Holder = EngineFixture.Engine, UnitResource = resource, UnitId = 7, GroupId = 2, MoraleValue = -3, Player = 1 };
            unit.SetOwner(army);
            Check("Army", army);
        }));
    }

    // A unit with no resource stores -1, and a load of that index refuses on both paths with the
    // message of ReadWrite.
    [Fact]
    public void AUnitWithoutAResourceStoresMinusOneAndBothLoadsRefuseIt()
    {
        using Unit unit = new(null!) { Holder = EngineFixture.Engine };
        SchemaClass classValue = s_schema.Get("Unit");
        byte[] image = Image(unit);
        ObjectNode tree = TreeWriter.Write(unit, Factory);
        EqualBytes(image, OriginalBackend.Write(tree, classValue, s_schema, "game"), "Unit original");
        JsonObject document = DocumentBackend.Write(tree, classValue, s_schema, "game");
        Assert.Equal(ushort.MaxValue, ((JsonInteger)document["UnitResourceIndex"]!).Value);
        EqualJson(PilotImage.Decode(image, "Unit"), document, "Unit write");
        using Unit viaReadWrite = new(null!) { Holder = EngineFixture.Engine };
        using Unit viaPersist = new(null!) { Holder = EngineFixture.Engine };
        KeyNotFoundException streamError = Assert.Throws<KeyNotFoundException>(() => Load(viaReadWrite, image));
        KeyNotFoundException treeError = Assert.Throws<KeyNotFoundException>(() => TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), viaPersist, Factory));
        Assert.Equal("unit resource missing (-1)", streamError.Message);
        Assert.Equal(streamError.Message, treeError.Message);
    }

    // Hero.ReadWrite skips the inventory and the worn items and keeps the player it had while the
    // hero updates from its library. The write of such a hero and both loads into one agree.
    [Fact]
    public void AHeroUpdatingFromItsLibraryKeepsItsPlayerAndSkipsItsItems()
    {
        WithDestroyingMap(_ =>
        {
            SchemaClass classValue = s_schema.Get("Hero");
            Check("Hero", new Hero(null) { Holder = EngineFixture.Engine, HeroName = "Plain", Player = 2, LibraryId = 4 });
            Hero updating = new(null) { Holder = EngineFixture.Engine, HeroName = "Updating", Player = 2 };
            updating.Mode = Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
            JsonObject written = DocumentBackend.Write(TreeWriter.Write(updating, Factory), classValue, s_schema, "game");
            Assert.Null(written.Find("Inventory"));
            Assert.Null(written.Find("HeroItems"));
            // A blank load is not updating, so it writes the items the converter's image lacks.
            Check("Hero", updating, converterRead: false);

            using Hero source = new(null) { Holder = EngineFixture.Engine, HeroName = "Source", Player = 2 };
            new Item(null) { Holder = EngineFixture.Engine, ItemId = 9, Name = "Ring" }.SetOwner(source.Inventory);
            byte[] image = Image(source);
            using Hero viaReadWrite = Updating(player: 5);
            using Hero viaPersist = Updating(player: 5);
            Load(viaReadWrite, image);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), viaPersist, Factory);
            Assert.Equal(5, viaReadWrite.Player);
            Assert.Equal(5, viaPersist.Player);
            Assert.Equal("Source", viaPersist.HeroName);
            Assert.Null(viaReadWrite.Inventory.Items.GetItem(0));
            Assert.Null(viaPersist.Inventory.Items.GetItem(0));
            EqualBytes(Image(viaReadWrite), Image(viaPersist), "Hero updating load");

            using Hero plain = new(null) { Holder = EngineFixture.Engine, Player = 5 };
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), plain, Factory);
            Assert.Equal(2, plain.Player);
            Assert.Equal(9, plain.Inventory.Items.GetItem(0).ItemId);
        });

        static Hero Updating(sbyte player)
        {
            Hero hero = new(null) { Holder = EngineFixture.Engine, Player = player };
            hero.Mode = Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
            return hero;
        }
    }

    // A hero of an older document stores no experience and its old experience under property 9.
    // Both loads derive the experience from it, and neither write stores it again.
    [Fact]
    public void AnOldHeroDerivesItsExperienceOnBothLoads()
    {
        WithDestroyingMap(_ =>
        {
            SchemaClass classValue = s_schema.Get("Hero");
            using Hero source = new(null) { Holder = EngineFixture.Engine, HeroName = "Old", Experience = 0 };
            JsonObject document = PilotImage.Decode(Image(source), "Hero");
            int at = document.Members.FindIndex(member => member.Name == "HeroName");
            document.Members.Insert(at, new JsonMember("OldExperience", new JsonInteger(3 << 5)));
            byte[] image = PilotImage.Encode(document, "Hero");
            using Hero viaReadWrite = new(null) { Holder = EngineFixture.Engine };
            using Hero viaPersist = new(null) { Holder = EngineFixture.Engine };
            Load(viaReadWrite, image);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), viaPersist, Factory);
            Assert.Equal(source.LevelToExperience(4), viaReadWrite.Experience);
            Assert.Equal(viaReadWrite.Experience, viaPersist.Experience);
            EqualBytes(Image(viaReadWrite), Image(viaPersist), "Hero old experience");
            Assert.Null(DocumentBackend.Write(TreeWriter.Write(viaPersist, Factory), classValue, s_schema, "game").Find("OldExperience"));
        });
    }

    [Fact]
    public void ArmyIdMatchesTheConverter()
    {
        Check("ArmyId", new ArmyId(null) { Holder = EngineFixture.Engine });
        ArmyId full = new(null) { Holder = EngineFixture.Engine, Player = -1, Location = new MapCoordinate(sbyte.MinValue, sbyte.MaxValue, 1) };
        full.UnitIds.Add(int.MinValue);
        full.UnitIds.Add(0);
        full.UnitIds.Add(int.MaxValue);
        Check("ArmyId", full);
    }

    [Fact]
    public void ItemLibraryMatchesTheConverter()
    {
        Check("ItemLibrary", new ItemLibrary(null) { Holder = EngineFixture.Engine });
        ItemLibrary library = new(null) { Holder = EngineFixture.Engine, Name = "Mine" };
        PlainItem(itemId: 1, name: "Sword").SetOwner(library);
        PlainItem(itemId: 2, name: "Shield").SetOwner(library);
        Check("ItemLibrary", library);
    }

    // A unit graphics resource with its five sound libraries. The sound libraries hold no
    // sequence, because SfxSequence belongs to a later family. GameSetTests and the image
    // sequence tests of PE-2c-5 cover the sequences.
    [Fact]
    public void UnitGraphicsResourceMatchesTheConverter()
    {
        Check("UnitGraphicsResource", new UnitGraphicsResource(null) { Holder = EngineFixture.Engine });
        UnitGraphicsResource full = new(null) { Holder = EngineFixture.Engine, Name = "Archer", MoveAnimationSpeed = 255 };
        Check("UnitGraphicsResource", full);
    }

    // The library managers store nothing of their own: ReadWrite and PersistFields read and write
    // the system library as Heroes.PFS and ITEMS.PFS beside the scenario set, here in the
    // fixture's temporary root.
    [Fact]
    public void TheLibraryManagersReadAndWriteTheirSystemLibraryAlike()
    {
        string heroes = Path.Combine(EngineFixture.Root, "Heroes.PFS");
        string items = Path.Combine(EngineFixture.Root, "ITEMS.PFS");
        try
        {
            HeroLibraryManager heroManager = new(null) { Holder = EngineFixture.Engine };
            heroManager.SystemLibrary.LibraryName = "System";
            Check("HeroLibraryManager", heroManager);
            byte[] heroBytes = File.ReadAllBytes(heroes);
            using (HeroLibraryManager viaPersist = new(null) { Holder = EngineFixture.Engine })
            {
                TreeReader.Read(DocumentBackend.Read(new JsonObject(), s_schema.Get("HeroLibraryManager"), s_schema, "game"), viaPersist, Factory);
                Assert.Equal("System", viaPersist.SystemLibrary.LibraryName);
            }
            using (HeroLibraryManager writer = new(null) { Holder = EngineFixture.Engine })
            {
                writer.SystemLibrary.LibraryName = "System";
                TreeWriter.Write(writer, Factory);
                Assert.Equal(heroBytes, File.ReadAllBytes(heroes));
            }

            ItemLibraryManager itemManager = new(null) { Holder = EngineFixture.Engine };
            itemManager.SystemLibrary.Name = "System";
            PlainItem(itemId: 5, name: "Staff").SetOwner(itemManager.SystemLibrary);
            Check("ItemLibraryManager", itemManager);
            using (ItemLibraryManager viaPersist = new(null) { Holder = EngineFixture.Engine })
            {
                TreeReader.Read(DocumentBackend.Read(new JsonObject(), s_schema.Get("ItemLibraryManager"), s_schema, "game"), viaPersist, Factory);
                Assert.Equal("System", viaPersist.SystemLibrary.Name);
                Assert.Equal("Staff", viaPersist.SystemLibrary.Items[0].Name);
            }
        }
        finally
        {
            File.Delete(heroes);
            File.Delete(items);
        }
    }

    // GameEngine.TranslateEngineObject runs the translate stream over a loaded map. The family's
    // classes have no ReadWrite since PE-3-i, so the stream visits them through their tree methods
    // and visits what the original ReadWrite visited under its mode (read, text only, translate):
    // a hero's name and nickname but not its library name, the owner name, name and description of
    // an item outside a library, and nothing of a library item outside an item library.
    [Fact]
    public void TheTranslateStreamVisitsTheStringsTheOriginalReadWriteVisited()
    {
        WithDestroyingMap(_ =>
        {
            using Army army = new(null) { Holder = EngineFixture.Engine };
            Hero hero = new(null) { Holder = EngineFixture.Engine, HeroName = "Archer", LibraryName = "Mine", Player = 1 };
            hero.SetNickname("the Swift");
            hero.SetOwner(army);
            Item sword = new(null) { Holder = EngineFixture.Engine, ItemId = 4, OwnerName = "Lair", Name = "Sword", LibraryId = -1 };
            sword.SetOwner(hero.Inventory);
            using ItemList lair = new(null) { Holder = EngineFixture.Engine };
            Item loot = new(null) { Holder = EngineFixture.Engine, ItemId = 5, OwnerName = "Lair", Name = "Sword", LibraryId = -1 };
            loot.SetOwner(lair);
            Item ring = new(null) { Holder = EngineFixture.Engine, ItemId = 6, OwnerName = "Shop", Name = "Ring", LibraryId = 3 };
            ring.SetOwner(lair);
            ObjectNode before = TreeWriter.Write(army, Factory);

            Dictionary<string, string> dictionary = new()
            {
                ["Archer"] = "Bogenschütze",
                ["the Swift"] = "der Schnelle",
                ["Lair"] = "Hort",
                ["Sword"] = "Schwert",
                ["Shop"] = "Laden",
                ["Ring"] = "Reif",
                ["Mine"] = "Meins",
            };
            using (DictionaryStream stream = new(dictionary))
            {
                stream.Object(army, 0);
                Assert.Equal(["Archer", "the Swift", "Lair", "Sword", "lines 0"], stream.Visited);
            }
            using (DictionaryStream stream = new(dictionary))
            {
                stream.Object(lair, 0);
                Assert.Equal(["Lair", "Sword", "lines 0"], stream.Visited);
            }

            Assert.Equal("Bogenschütze", hero.HeroName);
            Assert.Equal("der Schnelle", hero.Nickname);
            Assert.Equal("Mine", hero.LibraryName);
            Assert.Equal("Hort", sword.OwnerName);
            Assert.Equal("Schwert", sword.Name);
            Assert.Equal(("Hort", "Schwert"), (loot.OwnerName, loot.Name));
            Assert.Equal(("Shop", "Ring"), (ring.OwnerName, ring.Name));
            Assert.Same(hero, army.GetChild(0));
            Assert.Same(sword, hero.Inventory.Items.GetItem(0));
            Assert.Equal(2, lair.Count);
            // Apart from the translated strings the army writes the tree it wrote before.
            hero.HeroName = "Archer";
            hero.SetNickname("the Swift");
            sword.OwnerName = "Lair";
            sword.Name = "Sword";
            Assert.True(before.ValueEquals(TreeWriter.Write(army, Factory)));
        });
    }

    // The translate stream of TranslateTextStream with a small dictionary in place of the game's,
    // recording what it visits.
    private sealed class DictionaryStream(Dictionary<string, string> dictionary) : TranslateVisit(EngineFixture.Engine)
    {
        public List<string> Visited { get; } = [];

        // The classes of the objects the stream visits, in order.
        public List<string> Objects { get; } = [];

        public override void Object(EngineObject objectValue, int id)
        {
            Objects.Add(objectValue.GetType().Name);
            base.Object(objectValue, id);
        }

        public override void TranslateString(ref string value)
        {
            Visited.Add(value);
            value = dictionary.GetValueOrDefault(value, value);
        }

        public override void TranslateStrings(ref StringCollection lines)
        {
            Visited.Add("lines " + lines.Count);
            for (int index = 0; index < lines.Count; index++)
            {
                lines[index] = dictionary.GetValueOrDefault(lines[index], lines[index]);
            }
        }
    }

    // A unit loads its resource from the game set by index, so the test gives the game set a
    // list of one resource and puts the fixture's list back after.
    private static void WithUnitResource(Action<UnitResource> body)
    {
        GamePack gameSet = GamePack.Current;
        UnitResourceList previous = gameSet.UnitResources;
        UnitResourceList resources = new(null) { Holder = EngineFixture.Engine };
        UnitResource resource = new(resources);
        gameSet.UnitResources = resources;
        try
        {
            Assert.Equal(0, resource.UnitResourceIndex());
            body(resource);
        }
        finally
        {
            gameSet.UnitResources = previous;
            resources.Dispose();
        }
    }
}
