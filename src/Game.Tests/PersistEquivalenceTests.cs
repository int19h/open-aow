#nullable disable warnings
using System.Runtime.CompilerServices;
using System.Text;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

/// <summary>
/// Pilot objects persist through the tree to the same version 3 JSON the converter decodes from
/// the image, and a fresh object loaded from that JSON writes the image the converter encodes.
/// The original backend writes and reads that same image with no ReadWrite on its path.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed partial class PersistEquivalenceTests
{
    private static readonly FormatSchema s_schema = FormatSchema.Current;

    private static EnginePersistableFactory Factory => field ??= new(EngineFixture.Engine);

    [Fact]
    public void EnchantmentMatchesTheConverter()
    {
        Check("Enchantment", new Enchantment(null) { Holder = EngineFixture.Engine });
        Enchantment zero = new(null) { Holder = EngineFixture.Engine, DispelResistance = 0, Source = 0, Upkeep = 0, EnchantmentId = 0 };
        Check("Enchantment", zero);
        Enchantment edge = new(null)
        {
            Holder = EngineFixture.Engine,
            DispelResistance = -1,
            Source = 127,
            Upkeep = int.MinValue,
            EnchantmentId = int.MaxValue,
        };
        Check("Enchantment", edge);
        Check("Enchantment", new Enchantment(null) { Holder = EngineFixture.Engine, DispelResistance = -128, Source = 0, Upkeep = int.MaxValue, EnchantmentId = int.MinValue });
    }

    [Fact]
    public void EventLogMatchesTheConverter()
    {
        Check("EventLog", new EventLog(null) { Holder = EngineFixture.Engine });
        Check("EventLog", new EventLog(null) { Holder = EngineFixture.Engine, Day = 0, Player = 0, Id = 0 });
        Check("EventLog", new EventLog(null) { Holder = EngineFixture.Engine, Day = int.MinValue, Player = -128, Id = int.MaxValue });
        Check("EventLog", new EventLog(null) { Holder = EngineFixture.Engine, Day = int.MaxValue, Player = 127, Id = int.MinValue });
    }

    [Fact]
    public void MessageEventLogMatchesTheConverter()
    {
        Check("MessageEventLog", new MessageEventLog(null, holder: EngineFixture.Engine));
        MessageEventLog empty = new(null, holder: EngineFixture.Engine) { Day = 0, Player = 0, Id = 0, Title = "", Layout = 0, Buttons = 0 };
        Check("MessageEventLog", empty);
        MessageEventLog full = new(null, holder: EngineFixture.Engine)
        {
            Day = int.MaxValue,
            Player = -1,
            Id = int.MinValue,
            Title = "Jörg",
            Layout = MessageEventLogLayout.Header,
            Buttons = MessageEventLogButtons.YesNoCancel,
        };
        _ = full.Message.Add("one");
        _ = full.Message.Add("");
        _ = full.Message.Add("two");
        Check("MessageEventLog", full);
        MessageEventLog edge = new(null, holder: EngineFixture.Engine) { Layout = (MessageEventLogLayout)255, Buttons = (MessageEventLogButtons)255, Title = "" };
        _ = edge.Message.Add("line");
        Check("MessageEventLog", edge);
    }

    [Fact]
    public void TokenEventResourceEditSettingsMatchesTheConverter()
    {
        Check("ControlStyleObject", new ControlStyleObject(null) { Holder = EngineFixture.Engine });
        ControlStyleObject zeroStyle = new(null) { Holder = EngineFixture.Engine, ControlStyle = ControlStyles.None };
        Check("ControlStyleObject", zeroStyle);
        ControlStyleObject bits = new(null) { Holder = EngineFixture.Engine, ControlStyle = (ControlStyles)0xA5 };
        Check("ControlStyleObject", bits);
        Check("ResourceEditSettings", new ResourceEditSettings(null) { Holder = EngineFixture.Engine });
        ResourceEditSettings editors = new(null) { Holder = EngineFixture.Engine, ControlStyle = ControlStyles.None };
        _ = editors.Editors.AddObject("", item: IntegerObjects.IntToObject(0));
        _ = editors.Editors.AddObject("Gate", item: IntegerObjects.IntToObject(4));
        Check("ResourceEditSettings", editors);
    }

    [Fact]
    public void ItemGraphicsMatchesTheConverter()
    {
        Check("EngineResource", new EngineResource(null) { Holder = EngineFixture.Engine });
        Check("EngineResource", new EngineResource(null) { Holder = EngineFixture.Engine, ResourceId = int.MinValue, EditId = int.MaxValue, EditIndex = 0 });
        Check("ItemGraphics", new ItemGraphics(null) { Holder = EngineFixture.Engine });
        ItemGraphics zero = new(null)
        {
            Holder = EngineFixture.Engine,
            ResourceId = 0,
            EditId = 0,
            EditIndex = 0,
            ImageLibraryName = "",
            ImageIndex = 0,
            ItemType = 0,
            Hidden = false,
        };
        Check("ItemGraphics", zero);
        ItemGraphics full = new(null)
        {
            Holder = EngineFixture.Engine,
            ResourceId = int.MaxValue,
            EditId = int.MinValue,
            EditIndex = 3,
            ImageLibraryName = "Jörg",
            ImageIndex = int.MinValue,
            ItemType = (ItemType)255,
            Hidden = true,
        };
        Check("ItemGraphics", full);
    }

    [Fact]
    public void AIBudgetMatchesTheConverter()
    {
        Check("AIBudget", new AIBudget(null) { Holder = EngineFixture.Engine });
        Check("AISavingsBudget", new AISavingsBudget(null) { Holder = EngineFixture.Engine });
        Check("AITransporterBudget", new AITransporterBudget(null) { Holder = EngineFixture.Engine });
        Check("AICityFortifyBudget", new AICityFortifyBudget(null) { Holder = EngineFixture.Engine });
        Check("AICityUpgradeBudget", new AICityUpgradeBudget(null) { Holder = EngineFixture.Engine });
        Check("AICityResearchBudget", new AICityResearchBudget(null) { Holder = EngineFixture.Engine });
        AIBudget edge = new(null)
        {
            Holder = EngineFixture.Engine,
            Priority = int.MinValue,
            RequiredGold = int.MaxValue,
            RequiredMana = int.MinValue,
            GoldUpkeep = int.MaxValue,
            ManaUpkeep = 0,
            Gold = int.MinValue,
            Mana = int.MaxValue,
            Days = int.MinValue,
            DayGoldMax = int.MaxValue,
            DayManaMax = int.MinValue,
            DayGold = int.MaxValue,
            DayMana = int.MinValue,
        };
        Check("AIBudget", edge);
        AIBudgetManager manager = new(null) { Holder = EngineFixture.Engine };
        Check("AIBudgetManager", manager);
        AIBudgetManager populated = new(null)
        {
            Holder = EngineFixture.Engine,
            Gold = int.MaxValue,
            Mana = int.MinValue,
            GoldUpkeep = 1,
            ManaUpkeep = 2,
            ProductionGoldUpkeep = 3,
            ProductionManaUpkeep = 4,
        };
        new AISavingsBudget(null) { Holder = EngineFixture.Engine }.SetOwner(populated.Budgets);
        new AITransporterBudget(null) { Holder = EngineFixture.Engine }.SetOwner(populated.Budgets);
        new AICityResearchBudget(null) { Holder = EngineFixture.Engine }.SetOwner(populated.Budgets);
        Check("AIBudgetManager", populated, converterRead: false);
    }

    [Fact]
    public void FrozenWaterHexSpriteMatchesTheConverter()
    {
        Check("FrozenWaterHexSprite", new FrozenWaterHexSprite(null) { Holder = EngineFixture.Engine });
        Check("FrozenWaterHexSprite", new FrozenWaterHexSprite(null) { Holder = EngineFixture.Engine, DaysRemaining = 0, CasterPlayer = 0 });
        Check("FrozenWaterHexSprite", new FrozenWaterHexSprite(null) { Holder = EngineFixture.Engine, DaysRemaining = -128, CasterPlayer = 127 });
        Check("FrozenWaterHexSprite", new FrozenWaterHexSprite(null) { Holder = EngineFixture.Engine, DaysRemaining = 127, CasterPlayer = -1 });
    }

    // A level of 5 by 3 fields: field [0][0] is empty, the others hold up to three frozen water
    // sprites and nonzero field flags. The grid, the sprite sequence and the progress reports
    // match the main visit on load and on save, through both backends.
    [Theory]
    [InlineData("MapLevel", "game")]
    [InlineData("CombatMapLevel", "tactical")]
    public void MapLevelMatchesTheConverter(string className, string engine)
    {
        SchemaClass classValue = s_schema.Get(className);
        JsonObject document = LevelDocument(width: 5, height: 3);
        byte[] image = PilotImage.Encode(document, className, engine);
        bool added = Register<FrozenWaterHexSprite>();
        (MapLevel viaReadWrite, ProgressMap streamMap) = NewLevel(className);
        (MapLevel fromImage, ProgressMap imageMap) = NewLevel(className);
        (MapLevel fromDocument, ProgressMap documentMap) = NewLevel(className);
        try
        {
            Load(viaReadWrite, image);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine), objectValue: fromImage, Factory);
            TreeReader.Read(DocumentBackend.Read(document, classValue, s_schema, engine), objectValue: fromDocument, Factory);
            Assert.Equal(streamMap.Reports, imageMap.Reports);
            Assert.Equal(streamMap.Reports, documentMap.Reports);
            // 15 fields report one tenth each, after the report at 0.
            Assert.Equal(16, streamMap.Reports.Count);
            Assert.Equal((MapProgressType.Loading, 150), streamMap.Reports[^1]);
            Assert.Equal(15, fromImage.GetCount());
            Assert.Equal(3, fromImage.GetField(3, 0).GetCount());
            Assert.Same(fromImage.GetField(3, 0), fromImage.GetField(3, 0).GetChild(0).Owner);
            Assert.Same(fromImage, fromImage.GetField(1, 0).Owner);
            Assert.Same(EngineFixture.Engine, fromImage.GetEngine());
            int streamSaved = streamMap.Reports.Count;
            byte[] rewritten = Image(viaReadWrite);
            EqualBytes(rewritten, Image(fromImage), label: $"{className} original read");
            EqualBytes(rewritten, Image(fromDocument), label: $"{className} document read");
            int treeSaved = imageMap.Reports.Count;
            ObjectNode tree = TreeWriter.Write(fromImage, Factory);
            Assert.Equal(streamMap.Reports[streamSaved..], imageMap.Reports[treeSaved..]);
            Assert.Equal((MapProgressType.Saving, 0), imageMap.Reports[treeSaved]);
            EqualJson(PilotImage.Decode(rewritten, className, engine), DocumentBackend.Write(tree, classValue, s_schema, engine), label: $"{className} write");
            EqualBytes(rewritten, OriginalBackend.Write(tree, classValue, s_schema, engine), label: $"{className} original");
        }
        finally
        {
            fromDocument.Dispose();
            fromImage.Dispose();
            viaReadWrite.Dispose();
            if (added)
            {
                EngineFixture.Engine.UnregisterEngineClasses([EngineClass.Of<FrozenWaterHexSprite>()]);
            }
        }
    }

    [Fact]
    public void AnAbsentGridLeavesTheLevelEmpty()
    {
        SchemaClass classValue = s_schema.Get("MapLevel");
        byte[] image = PilotImage.Encode((JsonObject)JsonReader.Parse("""{"Width":2,"Height":1,"HexLevel":1}"""u8, file: "level.json"), className: "MapLevel");
        (MapLevel viaReadWrite, ProgressMap streamMap) = NewLevel("MapLevel");
        (MapLevel fromImage, ProgressMap imageMap) = NewLevel("MapLevel");
        try
        {
            Load(viaReadWrite, image);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: fromImage, Factory);
            Assert.Equal(streamMap.Reports, imageMap.Reports);
            Assert.Equal(2, fromImage.GetCount());
            Assert.Equal(0, fromImage.GetField(1, 0).GetCount());
            EqualBytes(Image(viaReadWrite), Image(fromImage), label: "absent grid");
        }
        finally
        {
            fromImage.Dispose();
            viaReadWrite.Dispose();
        }
    }

    [Fact]
    public void ASpriteWhosePersistThrowsIsDroppedUnlessTheHandlerStops()
    {
        SchemaClass classValue = s_schema.Get("MapLevel");
        byte[] image = PilotImage.Encode(LevelDocument(width: 2, height: 1), className: "MapLevel");
        EngineClass descriptor = EngineClass.Of<FrozenWaterHexSprite>();
        bool added = Register<FrozenWaterHexSprite>();
        Delegate previous = EngineClassCreation.Substitute(descriptor, next: (Func<CustomNode, EngineObject>)(static owner => new ThrowingFrozenWater(owner)));
        try
        {
            (MapLevel continued, _) = NewLevel("MapLevel");
            (MapLevel continuedTree, _) = NewLevel("MapLevel");
            try
            {
                Load(continued, image);
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: continuedTree, Factory);
                Assert.Equal(0, continued.GetField(1, 0).GetCount());
                Assert.Equal(0, continuedTree.GetField(1, 0).GetCount());
                EqualBytes(Image(continued), Image(continuedTree), label: "dropped sprites");
            }
            finally
            {
                continuedTree.Dispose();
                continued.Dispose();
            }
            (MapLevel stopped, _) = NewLevel("MapLevel");
            (MapLevel stoppedTree, _) = NewLevel("MapLevel");
            try
            {
                InvalidOperationException streamError = Assert.Throws<InvalidOperationException>(() =>
                    Load(stopped, image, static (_, out cont) => cont = false));
                InvalidOperationException treeError = Assert.Throws<InvalidOperationException>(() =>
                    TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: stoppedTree, Factory,
                        static (_, out cont) => cont = false));
                Assert.Equal("frozen water failed", streamError.Message);
                Assert.Equal("frozen water failed", treeError.Message);
            }
            finally
            {
                stoppedTree.Dispose();
                stopped.Dispose();
            }
        }
        finally
        {
            _ = EngineClassCreation.Substitute(descriptor, next: previous);
            if (added)
            {
                EngineFixture.Engine.UnregisterEngineClasses([descriptor]);
            }
        }
    }

    // The fixture engine enlists the classes its other tests create. A stored sprite is created by its class id.
    private static bool Register<T>() where T : EngineObject
    {
        EngineClass descriptor = EngineClass.Of<T>();
        if (EngineFixture.Engine.ClassTable.ContainsKey(descriptor.ClassId))
        {
            return false;
        }

        EngineFixture.Engine.RegisterEngineClasses([descriptor]);
        return true;
    }

    // Field [y][x] holds (x + y) % 4 sprites with distinct values, and flags x * 100 + y + 1.
    private static JsonObject LevelDocument(int width, int height)
    {
        JsonArray rows = new();
        for (int y = 0; y < height; y++)
        {
            JsonArray row = new();
            for (int x = 0; x < width; x++)
            {
                JsonObject field = new();
                if (x != 0 || y != 0)
                {
                    JsonArray sprites = new();
                    for (int k = 0; k < (x + y) % 4; k++)
                    {
                        _ = sprites.Add(new JsonObject()
                            .Add("$class", new JsonString("FrozenWaterHexSprite"))
                            .Add("DaysRemaining", new JsonInteger(k + 1))
                            .Add("CasterPlayer", new JsonInteger(x - y)));
                    }

                    if (sprites.Items.Count != 0)
                    {
                        _ = field.Add("HexSpriteList", sprites);
                    }

                    _ = field.Add("FieldFlags", new JsonInteger((x * 100) + y + 1));
                }
                _ = row.Add(field);
            }
            _ = rows.Add(row);
        }
        return new JsonObject()
            .Add("Width", new JsonInteger(width))
            .Add("Height", new JsonInteger(height))
            .Add("HexLevel", new JsonInteger(1))
            .Add("Fields", rows);
    }

    // A level in a container of a map that records its progress reports.
    private static (MapLevel Level, ProgressMap Map) NewLevel(string className)
    {
        ProgressMap map = new();
        MapContainer container = new(map) { Map = map };
        MapLevel level = className == "CombatMapLevel" ? new CombatMapLevel(container) : new MapLevel(container);
        return (level, map);
    }

    private sealed class ProgressMap() : GameMap(null!)
    {
        public List<(MapProgressType Type, int Percent)> Reports { get; } = [];

        public override void NotifyProgress(in MapProgress progress) => Reports.Add((progress.ProgressType, progress.Percent));
    }

    private sealed class ThrowingFrozenWater(CustomNode owner) : FrozenWaterHexSprite(owner)
    {
        public override void PersistFields(IPersister persister)
        {
            if (persister.Loading)
            {
                throw new InvalidOperationException("frozen water failed");
            }

            base.PersistFields(persister);
        }
    }

    [Fact]
    public void FloodEnchantmentMatchesTheConverter()
    {
        Check("GlobalEnchantment", new GlobalEnchantment(null) { Holder = EngineFixture.Engine });
        GlobalEnchantment named = new(null)
        {
            Holder = EngineFixture.Engine,
            EnchantmentId = int.MinValue,
            EnchantmentType = (GlobalEnchantmentType)255,
            PlayerIndex = -1,
            Name = "Jörg",
            EnchantmentManaUpkeep = int.MaxValue,
            DispelMana = 0,
        };
        Check("GlobalEnchantment", named);
        Check("FloodControl", new FloodControl(null) { Holder = EngineFixture.Engine });
        FloodControl terrain = new(null)
        {
            Holder = EngineFixture.Engine,
            Width = 2,
            Height = 1,
            OriginalTerrain = [0, -1, 127, -128]
        };
        Check("FloodControl", terrain);
        Check("FloodEnchantment", new FloodEnchantment(null, holder: EngineFixture.Engine));
        FloodEnchantment flood = new(null, holder: EngineFixture.Engine)
        {
            EnchantmentId = 9,
            EnchantmentType = (GlobalEnchantmentType)1,
            PlayerIndex = 4,
            Name = "",
            EnchantmentManaUpkeep = int.MinValue,
            DispelMana = int.MaxValue,
            TurnsActive = int.MaxValue,
            Removed = true,
            DispelPlayer = 127,
            RestoreDays = 0,
        };
        flood.FloodControl.Width = 1;
        flood.FloodControl.Height = 2;
        flood.FloodControl.OriginalTerrain = [1, 2, 3, 4];
        Check("FloodEnchantment", flood);
        Check("FloodEnchantment", new FloodEnchantment(null, holder: EngineFixture.Engine) { Removed = false, RestoreDays = -128, DispelPlayer = -128, TurnsActive = int.MinValue });
    }

    // The single stores its exact bits: the document holds a number, and $bits for negative zero, an
    // infinity and a NaN with its payload.
    [Fact]
    public void TheSingleOfTheAIMatchesTheConverter()
    {
        foreach (int bits in new[] { 0, 0x3DCCCCCD, 0x3F800000, 0x3F000000, 0x3727C5AC, 0x7F7FFFFF, int.MinValue, 0x7F800000, unchecked((int)0xFF800000), 0x7FC00001 })
        {
            Check("AIPlayerControl", new AIPlayerControl(null) { Holder = EngineFixture.Engine, DefenseStrengthMultiplier = BitConverter.Int32BitsToSingle(bits) });
        }
    }

    // The fixed objects of the controls persist in place at their slots. The empty controls store no member.
    [Fact]
    public void TheEmptyAbilityAndSpellControlsMatchTheConverter()
    {
        Check("AbilityControl", new AbilityControl(null) { Holder = EngineFixture.Engine });
        Check("SpellControl", new SpellControl(null) { Holder = EngineFixture.Engine });
    }

    [Fact]
    public void UnitEnchantmentMatchesTheConverter()
    {
        Check("UnitEnchantment", new UnitEnchantment(null) { Holder = EngineFixture.Engine });
        Check("UnitEnchantment", new UnitEnchantment(null) { Holder = EngineFixture.Engine, EnchantmentAbilityId = int.MinValue, UnitId = int.MaxValue, Source = 0, DispelResistance = 0, Upkeep = 0 });
        Check("CosmagicSurgeryEnchantment", new CosmagicSurgeryEnchantment(null) { Holder = EngineFixture.Engine, FaceIndex = int.MaxValue });
        Check("CosmagicSurgeryEnchantment", new CosmagicSurgeryEnchantment(null) { Holder = EngineFixture.Engine, FaceIndex = int.MinValue, EnchantmentId = 0, Upkeep = 0, Source = 0, DispelResistance = 0 });
        Check("UnitEnchantmentAbilityData", new UnitEnchantmentAbilityData(null) { Holder = EngineFixture.Engine });
        UnitEnchantmentAbilityData absent = new(null)
        {
            Holder = EngineFixture.Engine,
            EnchantmentAbilityId = 0,
            EnchantmentPlayerIndex = -1,
            EnchantmentDuration = -1,
            Enchantment = null,
        };
        Check("UnitEnchantmentAbilityData", absent);
        UnitEnchantmentAbilityData present = new(null)
        {
            Holder = EngineFixture.Engine,
            EnchantmentAbilityId = 65535,
            EnchantmentPlayerIndex = 0,
            EnchantmentDuration = -128,
            Enchantment = new CosmagicSurgeryEnchantment(null) { Holder = EngineFixture.Engine, FaceIndex = 3, UnitId = int.MinValue },
        };
        Check("UnitEnchantmentAbilityData", present);
        UnitEnchantmentAbilityData edge = new(null)
        {
            Holder = EngineFixture.Engine,
            EnchantmentAbilityId = 1,
            EnchantmentPlayerIndex = 127,
            EnchantmentDuration = 127,
            Enchantment = new UnitEnchantment(null) { Holder = EngineFixture.Engine, EnchantmentAbilityId = int.MaxValue },
        };
        Check("UnitEnchantmentAbilityData", edge);
    }

    [Fact]
    public void DeclaredEnumMembersAndAnUndeclaredByteRoundTrip()
    {
        Each<DiplomaticRelation>(value => Check("DiplomaticRelationHistoryItem", new DiplomaticRelationHistoryItem(null) { Holder = EngineFixture.Engine, Day = 3, Relation = value }));
        Check("DiplomaticRelationHistoryItem", new DiplomaticRelationHistoryItem(null) { Holder = EngineFixture.Engine, Relation = (DiplomaticRelation)255, Day = int.MinValue });
        Each<ChangeGlobalMagicTokenEventMode>(value => Check("ChangeGlobalMagicTokenEvent", new ChangeGlobalMagicTokenEvent(null) { Holder = EngineFixture.Engine, Mode = value, PlayerIndex = 1, EnchantmentId = 2, Value = 3 }));
        Check("ChangeGlobalMagicTokenEvent", new ChangeGlobalMagicTokenEvent(null) { Holder = EngineFixture.Engine, Mode = (ChangeGlobalMagicTokenEventMode)255, PlayerIndex = 255, EnchantmentId = int.MaxValue, Value = int.MinValue });
        Each<GlobalEnchantmentType>(value => Check("GlobalEnchantment", new GlobalEnchantment(null) { Holder = EngineFixture.Engine, EnchantmentType = value }));
        Each<ItemType>(value => Check("ItemGraphics", new ItemGraphics(null) { Holder = EngineFixture.Engine, ItemType = value }));
        Each<MessageEventLogLayout>(value => Check("MessageEventLog", new MessageEventLog(null, holder: EngineFixture.Engine) { Layout = value }));
        Each<MessageEventLogButtons>(value => Check("MessageEventLog", new MessageEventLog(null, holder: EngineFixture.Engine) { Buttons = value }));
        Each<CityEnchantmentId>(value =>
        {
            GlobalMagicControl control = new(null) { Holder = EngineFixture.Engine };
            CityEnchantment city = new(null)
            {
                Holder = EngineFixture.Engine,
                CityEnchantmentId = value,
                EnchantmentLocation = new MapCoordinate(9, 0, 0)
            };
            city.SetOwner(control.Enchantments);
            Check("GlobalMagicControl", control, converterRead: false);
        });
    }

    [Fact]
    public void SetsCoverEmptyFullAndUnnamedBits()
    {
        Check("ControlStyleObject", new ControlStyleObject(null) { Holder = EngineFixture.Engine, ControlStyle = (ControlStyles)0xFF });
        Check("SingleTargetCombatAction", new SingleTargetCombatAction(null) { Holder = EngineFixture.Engine, Mode = SingleTargetModes.None, SourceId = 0, TargetId = 0 });
        Check("SingleTargetCombatAction", new SingleTargetCombatAction(null) { Holder = EngineFixture.Engine, Mode = SingleTargetModes.Defensive, SourceId = 255, TargetId = 1 });
        Check("SingleTargetCombatAction", new SingleTargetCombatAction(null) { Holder = EngineFixture.Engine, Mode = (SingleTargetModes)0xFF, SourceId = 4, TargetId = 5 });
    }

    [Fact]
    public void FixedArraysCoverZeroMixedAndBoundaryValues()
    {
        UnitSpellCombatAction zero = new(null) { Holder = EngineFixture.Engine };
        Check("UnitSpellCombatAction", zero);
        UnitSpellCombatAction mixed = new(null) { Holder = EngineFixture.Engine, SpellId = 8, SourceId = 1, TargetId = 2, Mana = -4 };
        mixed.SpellData[0] = 7;
        mixed.SpellData[1] = -1;
        Check("UnitSpellCombatAction", mixed);
        UnitSpellCombatAction bounds = new(null) { Holder = EngineFixture.Engine, SpellId = int.MinValue, Mana = int.MaxValue };
        bounds.SpellData[0] = int.MinValue;
        bounds.SpellData[1] = int.MaxValue;
        Check("UnitSpellCombatAction", bounds);
        GlobalMagicControl control = new(null) { Holder = EngineFixture.Engine };
        Check("GlobalMagicControl", control);
        GlobalMagicControl spheres = new(null) { Holder = EngineFixture.Engine, NextEnchantmentId = int.MaxValue };
        for (int i = 0; i < 9; i++)
        {
            spheres.DoubleSphereManaCount[i] = i == 8 ? int.MinValue : i;
        }

        HatredEnchantment hatred = new(null, holder: EngineFixture.Engine) { DaysActive = 12 };
        hatred.SetOwner(spheres.Enchantments);
        HarmonyEnchantment harmony = new(null, holder: EngineFixture.Engine) { DaysActive = int.MinValue };
        harmony.SetOwner(spheres.Enchantments);
        Check("GlobalMagicControl", spheres, converterRead: false);
    }

    [Fact]
    public void IntegerRunListsMatchTheConverter()
    {
        Check("ItemExplorationSiteEventLog", ExplorationLog(defaultValue: 0));
        Check("ItemExplorationSiteEventLog", ExplorationLog(defaultValue: 0, 0, 0, 0));
        Check("ItemExplorationSiteEventLog", ExplorationLog(defaultValue: 0, 4, 0, 0, int.MinValue, int.MaxValue, 0, -1));
        Check("ItemExplorationSiteEventLog", ExplorationLog(defaultValue: -1, -1, -1, 9, -1, 0));
        Check("ItemExplorationSiteEventLog", ExplorationLog(defaultValue: int.MinValue, int.MinValue));
        Check("ItemExplorationSiteEventLog", ExplorationLog(defaultValue: 7));
        Check("TutorialControl", new TutorialControl(null) { Holder = EngineFixture.Engine });
    }

    [Fact]
    public void ByteRunListsMatchTheConverter()
    {
        Check("CommandAbilityData", Commanding(defaultValue: 0));
        Check("CommandAbilityData", Commanding(defaultValue: 0, items: [.. Enumerable.Repeat((byte)0, count: 255)]));
        Check("CommandAbilityData", Commanding(defaultValue: 0, items: [.. Enumerable.Repeat((byte)0, count: 256)]));
        Check("CommandAbilityData", Commanding(defaultValue: 0, items: [.. Enumerable.Repeat((byte)0, count: 300), 1, .. Enumerable.Repeat((byte)0, count: 600)]));
        Check("CommandAbilityData", Commanding(defaultValue: 255, items: [255, 254, 255, 255, 0]));
        Check("CommandAbilityData", Commanding(defaultValue: 7, items: [.. Enumerable.Repeat((byte)7, count: 511)]));
        // The relation list resets its default to Unknown after a load on both paths, so the
        // loaded image differs from the converter's encode of the written document.
        WithDestroyingMap(_ =>
        {
            PlayerDiplomaticRelations relations = new(null) { Holder = EngineFixture.Engine };
            relations.SetDiplomaticRelation(3, value: DiplomaticRelation.War);
            relations.SetDiplomaticRelation(1, value: DiplomaticRelation.Alliance);
            Check("PlayerDiplomaticRelations", relations, converterRead: false);
        });
    }

    [Fact]
    public void AByteRunOver255SplitsAsTheWriterSplitsIt()
    {
        using CommandAbilityData source = Commanding(defaultValue: 0, items: [.. Enumerable.Repeat((byte)0, count: 300)]);
        byte[] image = OriginalBackend.Write(classValue: s_schema.Get("CommandAbilityData"), node: TreeWriter.Write(source, Factory), schema: s_schema, engine: "game");
        Assert.Equal(Image(source), image);
        JsonObject? stored = (JsonObject?)PilotImage.Decode(image, className: "CommandAbilityData")["CommandedUnits"];
        Assert.Equal(300, ((JsonArray)stored["items"]).Items.Count);
        // The default, the count 300, then a run of 255 and a run of 45.
        byte[] runs = [0, 44, 1, 0, 0, 0, 255, 0, 45];
        Assert.True(image.AsSpan().IndexOf(runs) >= 0);
    }

    [Fact]
    public void AnAbsentRunListLeavesTheListAsItWas()
    {
        SchemaClass classValue = s_schema.Get("ItemExplorationSiteEventLog");
        byte[] image = PilotImage.Encode(new JsonObject(), className: "ItemExplorationSiteEventLog");
        using ItemExplorationSiteEventLog viaReadWrite = ExplorationLog(defaultValue: 5, 5, 6, 5);
        Load(viaReadWrite, image);
        using ItemExplorationSiteEventLog viaImage = ExplorationLog(defaultValue: 5, 5, 6, 5);
        TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: viaImage, Factory);
        using ItemExplorationSiteEventLog viaDocument = ExplorationLog(defaultValue: 5, 5, 6, 5);
        TreeReader.Read(DocumentBackend.Read(new JsonObject(), classValue, s_schema, engine: "game"), objectValue: viaDocument, Factory);
        Assert.Equal(5, viaReadWrite.ItemIds.Default);
        Assert.Equal(3, viaReadWrite.ItemIds.Count);
        EqualBytes(Image(viaReadWrite), Image(viaImage), label: "absent list original read");
        EqualBytes(Image(viaReadWrite), Image(viaDocument), label: "absent list document read");
    }

    [Fact]
    public void AUnitIndexListKeepsItsStoredDefault()
    {
        SchemaClass classValue = s_schema.Get("ItemExplorationSiteEventLog");
        UnitIndexProbe source = new();
        _ = source.Units.Add(4);
        _ = source.Units.Add(-1);
        _ = source.Units.Add(-1);
        using ItemExplorationSiteEventLog expected = ExplorationLog(defaultValue: -1, 4, -1, -1);
        byte[] image = OriginalBackend.Write(TreeWriter.Write(source, Factory), classValue, s_schema, engine: "game");
        EqualBytes(Image(expected), image, label: "unit index list original");
        UnitIndexProbe loaded = new();
        loaded.Units.Default = 0;
        TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: loaded, Factory);
        Assert.Equal(-1, loaded.Units.Default);
        Assert.Equal([4, -1, -1], loaded.Units.List.AsSpan(0, length: loaded.Units.Count).ToArray());
        UnitIndexProbe other = new();
        other.Units.Default = 3;
        _ = other.Units.Add(3);
        byte[] otherImage = OriginalBackend.Write(TreeWriter.Write(other, Factory), classValue, s_schema, engine: "game");
        UnitIndexProbe reloaded = new();
        TreeReader.Read(OriginalBackend.Read(otherImage, classValue, s_schema, engine: "game"), objectValue: reloaded, Factory);
        Assert.Equal(3, reloaded.Units.Default);
        UnitIndexProbe untouched = new();
        _ = untouched.Units.Add(8);
        TreeReader.Read(new ObjectNode(null, slot: null), objectValue: untouched, Factory);
        Assert.Equal(-1, untouched.Units.Default);
        _ = Assert.Single(untouched.Units);
    }

    [Fact]
    public void AbilityDataMatchesTheConverter()
    {
        Check("Item", AbilityItem(withData: false));
        Check("Item", AbilityItem(withData: true));
    }

    [Fact]
    public void AbilityDataLoadsFollowTheStoredIds()
    {
        using Item source = AbilityItem(withData: true);
        JsonObject document = PilotImage.Decode(Image(source), className: "Item");
        // AbilityDataIds names slot 33 and an absent slot 99, and leaves the stored slot 47 unread.
        JsonArray? ids = (JsonArray?)((JsonObject)document["AbilityDataIds"])["items"];
        ids.Items.Clear();
        ids.Items.Add(new JsonInteger(33));
        ids.Items.Add(new JsonInteger(99));
        SameLoad("Item", document);
        JsonObject noBits = PilotImage.Decode(Image(source), className: "Item");
        // ReadWrite tests the ability bits (property 3), not the count.
        Assert.True(noBits.Remove("AbilityBits"));
        SameLoad("Item", document: noBits);
    }

    [Fact]
    public void ItemControlMatchesTheConverter()
    {
        Check("ItemControl", new ItemControl(null) { Holder = EngineFixture.Engine });
        ItemControl control = new(null) { Holder = EngineFixture.Engine };
        control.RegisterItem(PlainItem(itemId: 5, name: "Sword"));
        control.RegisterItem(PlainItem(itemId: 9, name: "Shield"));
        control.RegisterItem(AbilityItem(withData: true));
        Check("ItemControl", control);
    }

    [Fact]
    public void ItemControlLoadsStopAtTheFirstMissingItem()
    {
        using ItemControl control = new(null) { Holder = EngineFixture.Engine };
        control.RegisterItem(PlainItem(itemId: 5, name: "Sword"));
        control.RegisterItem(PlainItem(itemId: 9, name: "Shield"));
        control.RegisterItem(PlainItem(itemId: 11, name: "Ring"));
        JsonObject document = PilotImage.Decode(Image(control), className: "ItemControl");
        JsonArray? items = (JsonArray?)document["Items"];
        // Slots 0 and 2 stay. Both loads stop at the gap at slot 1.
        items.Items.RemoveAt(1);
        SameLoad("ItemControl", document);
        JsonObject nil = PilotImage.Decode(Image(control), className: "ItemControl");
        JsonArray? first = (JsonArray?)nil["Items"];
        first.Items[0] = new JsonObject().Add("$class", new JsonNull()).Add("$slot", new JsonInteger(0));
        SameLoad("ItemControl", document: nil);
    }

    private static ItemExplorationSiteEventLog ExplorationLog(int defaultValue, params int[] items)
    {
        ItemExplorationSiteEventLog log = new(null) { Holder = EngineFixture.Engine };
        log.ItemIds.Default = defaultValue;
        foreach (int item in items)
        {
            _ = log.ItemIds.Add(item);
        }

        return log;
    }

    private static CommandAbilityData Commanding(byte defaultValue, params byte[] items)
    {
        CommandAbilityData data = new(null) { Holder = EngineFixture.Engine, AbilityId = 40 };
        data.CommandedUnits.Default = defaultValue;
        foreach (byte item in items)
        {
            _ = data.CommandedUnits.Add(item);
        }

        return data;
    }

    private static Item PlainItem(int itemId, string name) => new(null) { Holder = EngineFixture.Engine, ItemId = itemId, Name = name, Attack = -2, LibraryId = 3 };

    // AddAbilityData puts each new item first, so the chain holds the reverse of this order.
    private static Item AbilityItem(bool withData)
    {
        Item item = new(null) { Holder = EngineFixture.Engine, ItemId = 12, Name = "Staff" };
        item.SetAbilityCount(170);
        if (!withData)
        {
            return item;
        }

        item.AddAbilityData(new MultiLevelAbilityData(null) { Holder = EngineFixture.Engine, Level = 3, LevelAbilityId = 12 });
        item.AddAbilityData(Commanding(defaultValue: 0, 1, 0, 0, 2));
        item.AddAbilityData(new DurationAbilityData(null) { Holder = EngineFixture.Engine, AbilityId = 7, Duration = 2, PlayerIndex = 1, Source = -1 });
        item.AddAbilityData(new UnitEnchantmentAbilityData(null) { Holder = EngineFixture.Engine, EnchantmentAbilityId = 9, Enchantment = new UnitEnchantment(null) { Holder = EngineFixture.Engine, UnitId = 4 } });
        item.AddAbilityData(new LeadershipAbilityData(null) { Holder = EngineFixture.Engine, LevelAbilityId = 20, Level = 1, ExternalLevel = 1, ExternalSource = "banner" });
        item.AddAbilityData(new HealingAbilityData(null) { Holder = EngineFixture.Engine, Enabled = true });
        item.AddAbilityData(new DispelMagicAbilityData(null) { Holder = EngineFixture.Engine, Enabled = true, Level = 2 });
        item.AddAbilityData(new PossessedAbilityData(null) { Holder = EngineFixture.Engine, SourceExperience = 300 });
        item.AddAbilityData(new CommandedAbilityData(null) { Holder = EngineFixture.Engine, AbilityId = 33, CommandAbilityId = 40, SourceId = 2, OriginalCombatPlayer = 1 });
        return item;
    }

    // The engine load, the original backend load and the document load of one document write the same image.
    private static void SameLoad(string className, JsonObject document)
    {
        SchemaClass classValue = s_schema.Get(className);
        byte[] image = PilotImage.Encode(document, className);
        EngineObject viaReadWrite = Blank(className);
        EngineObject viaImage = Blank(className);
        EngineObject viaDocument = Blank(className);
        try
        {
            Load(viaReadWrite, image);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: viaImage, Factory);
            TreeReader.Read(DocumentBackend.Read(document, classValue, s_schema, engine: "game"), objectValue: viaDocument, Factory);
            byte[] expected = Image(viaReadWrite);
            EqualBytes(expected, Image(viaImage), label: $"{className} original load");
            EqualBytes(expected, Image(viaDocument), label: $"{className} document load");
        }
        finally
        {
            viaDocument.Dispose();
            viaImage.Dispose();
            viaReadWrite.Dispose();
        }
    }

    [Fact]
    public void ArmyViewCoversBothArrayWidths()
    {
        StrategicMap? previous = StrategicMap.Current;
        StrategicMap map = new(null, holder: EngineFixture.Engine)
        {
            Status = HexSpriteMapStatuses.Destroying
        };
        StrategicMap.Current = map;
        try
        {
            Check("Army", new Army(null) { Holder = EngineFixture.Engine });
            Each<GroupBehavior>(value =>
            {
                Army army = new(null)
                {
                    Holder = EngineFixture.Engine,
                    OldAIBehavior = value,
                    Player = -1,
                    Mode = ArmyModes.FixedBehavior,
                    UserMode = ArmyUserModes.None
                };
                Check("Army", army);
            });
            Army full = new(null)
            {
                Holder = EngineFixture.Engine,
                Mode = (ArmyModes)0xFF & ~ArmyModes.Moving,
                UserMode = (ArmyUserModes)0xFF
            };
            Check("Army", full);
            ArmyView view = new(null) { Holder = EngineFixture.Engine };
            Check("ArmyView", view);
            ArmyView widths = new(null) { Holder = EngineFixture.Engine, ConcealedForPlayers = ushort.MaxValue };
            for (int i = 0; i < 8; i++)
            {
                widths.UnitConcealment[i] = (ushort)(i == 0 ? 0 : ushort.MaxValue);
                widths.MoraleValues[i] = i % 2 == 0 ? int.MinValue : int.MaxValue;
            }
            Check("ArmyView", widths);
        }
        finally
        {
            StrategicMap.Current = previous;
            map.Dispose();
        }
    }

    [Fact]
    public void AnOverlongSpellDataListIsRefusedOnExport()
    {
        using UnitSpellCombatAction spell = new(null) { Holder = EngineFixture.Engine };
        SchemaClass classValue = s_schema.Get("UnitSpellCombatAction");
        ObjectNode tree = TreeWriter.Write(spell, Factory);
        ListNode? list = (ListNode?)tree.Find("SpellData");
        list.Items.Add(new IntegerNode(4, signed: true, value: 1));
        JsonObject document = DocumentBackend.Write(tree, classValue, s_schema, engine: "game");
        Assert.Equal(3, ((JsonArray)document["SpellData"]).Items.Count);
        ConversionException error = Assert.Throws<ConversionException>(() => OriginalBackend.Write(tree, classValue, s_schema, engine: "game"));
        Assert.Equal(Codes.Range, error.Diagnostic.Code);
        Assert.Equal("root.SpellData", error.Diagnostic.Path);
        Assert.Contains("holds at most 2", error.Diagnostic.Message);
        Assert.Contains("Export does not truncate", error.Diagnostic.Message);
    }

    [Fact]
    public void ANetworkArmyOmitsDestinationsAndStillReadsThem()
    {
        WithDestroyingMap(map =>
        {
            map.ConnectionType = PlayerConnectionType.Network;
            Army army = new(null)
            {
                Holder = EngineFixture.Engine,
                Destination = new MapCoordinate(3, 4, 1),
                UserMode = ArmyUserModes.Guard
            };
            SchemaClass classValue = s_schema.Get("Army");
            JsonObject document = DocumentBackend.Write(TreeWriter.Write(army, Factory), classValue, s_schema, engine: "game");
            Assert.Null(document.Find("Destination"));
            Assert.Null(document.Find("UserMode"));
            Check("Army", army);

            map.ConnectionType = PlayerConnectionType.Local;
            Army source = new(null)
            {
                Holder = EngineFixture.Engine,
                Destination = new MapCoordinate(3, 4, 1),
                UserMode = ArmyUserModes.Guard
            };
            byte[] image = Image(source);
            source.Dispose();
            map.ConnectionType = PlayerConnectionType.Network;
            Army viaPersist = (Army)Factory.Create("Army");
            Army viaReadWrite = (Army)Factory.Create("Army");
            try
            {
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: viaPersist, Factory);
                Load(viaReadWrite, image);
                Assert.Equal(3, viaPersist.Destination.X);
                Assert.Equal(4, viaPersist.Destination.Y);
                Assert.Equal(1, viaPersist.Destination.MapLevel);
                Assert.True(viaPersist.UserMode.Contains(ArmyUserModes.Guard));
                Assert.Equal(3, viaReadWrite.Destination.X);
                Assert.Equal(4, viaReadWrite.Destination.Y);
                Assert.Equal(1, viaReadWrite.Destination.MapLevel);
                Assert.True(viaReadWrite.UserMode.Contains(ArmyUserModes.Guard));
                EqualBytes(Image(viaPersist), Image(viaReadWrite), label: "Army network load");
            }
            finally
            {
                viaReadWrite.Dispose();
                viaPersist.Dispose();
            }
        });
    }

    [Fact]
    public void LoadingAnArmyClearsMoving()
    {
        WithDestroyingMap(_ =>
        {
            Army source = new(null)
            {
                Holder = EngineFixture.Engine,
                Mode = ArmyModes.Moving | ArmyModes.FixedBehavior
            };
            byte[] image = Image(source);
            source.Dispose();
            SchemaClass classValue = s_schema.Get("Army");
            Army viaPersist = (Army)Factory.Create("Army");
            Army viaReadWrite = (Army)Factory.Create("Army");
            try
            {
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: viaPersist, Factory);
                Load(viaReadWrite, image);
                Assert.False(viaPersist.Mode.Contains(ArmyModes.Moving));
                Assert.False(viaReadWrite.Mode.Contains(ArmyModes.Moving));
                Assert.True(viaPersist.Mode.Contains(ArmyModes.FixedBehavior));
                Assert.True(viaReadWrite.Mode.Contains(ArmyModes.FixedBehavior));
                EqualBytes(Image(viaPersist), Image(viaReadWrite), label: "Army moving");
            }
            finally
            {
                viaReadWrite.Dispose();
                viaPersist.Dispose();
            }
        });
    }

    [Fact]
    public void AnimationMatchesTheConverter()
    {
        Check("Animation", new Animation(null) { Holder = EngineFixture.Engine });
        Animation edge = new(null)
        {
            Holder = EngineFixture.Engine,
            Dimensions = new Rect { Left = int.MinValue, Top = -1, Right = int.MaxValue, Bottom = 4 },
            Location = new MapCoordinate(-1, sbyte.MinValue, sbyte.MaxValue),
            FrameCounter = int.MinValue
        };
        Check("Animation", edge);
    }

    [Fact]
    public void DamageCombatActionMatchesTheConverter()
    {
        Check("DamageCombatAction", new DamageCombatAction(null) { Holder = EngineFixture.Engine });
        DamageCombatAction edge = new(null)
        {
            Holder = EngineFixture.Engine,
            Damage = 255,
            SourceId = 255,
            TargetId = 1,
            Effects = (DamageTypes)((1UL << 9) | (1UL << 15)),
            DamageTypes = DamageTypes.Fire | DamageTypes.NonSpecific,
        };
        Check("DamageCombatAction", edge);
    }

    [Fact]
    public void RemotePlayerMatchesTheConverter()
    {
        Check("RemotePlayer", new RemotePlayer(null) { Holder = EngineFixture.Engine });
        RemotePlayer zero = new(null) { Holder = EngineFixture.Engine, RemoteId = 0, PreferredRace = 0, MapValidation = MapValidationInfo.Unknown, Version = 0 };
        Check("RemotePlayer", zero);
        Each<MapValidationInfo>(value => Check("RemotePlayer", new RemotePlayer(null)
        {
            Holder = EngineFixture.Engine,
            MapValidation = value,
            Name = "Jörg",
            RemoteId = int.MinValue,
            PreferredRace = 255,
            Version = int.MaxValue,
        }));
    }

    [Fact]
    public void AIGroupMatchesTheConverter()
    {
        Check("AIGroup", new AIGroup(null) { Holder = EngineFixture.Engine });
        AIGroup zero = new(null)
        {
            Holder = EngineFixture.Engine,
            GroupId = 0,
            TransporterId = 0,
            LeaderId = 0,
            Player = 0,
            IndependentRelation = 0,
            ProcessPriority = 0,
            RecruitmentPriority = 0,
            TargetMode = 0,
            TargetType = AIGroupTargetStatus.None,
        };
        Check("AIGroup", zero);
        Each<AIGroupTargetStatus>(value =>
        {
            AIGroup group = new(null)
            {
                Holder = EngineFixture.Engine,
                GroupId = int.MinValue,
                TransporterId = int.MaxValue,
                LeaderId = int.MinValue,
                Player = -1,
                IndependentRelation = sbyte.MinValue,
                ProcessPriority = 255,
                RecruitmentPriority = 1,
                GoldBudget = int.MaxValue,
                ManaBudget = int.MinValue,
                TargetMovePoints = int.MaxValue,
                TargetMode = 255,
                TargetType = value,
                TargetLocation = new MapCoordinate(40, 21, 1)
            };
            Check("AIGroup", group);
        });
    }

    [Fact]
    public void SetupSettingsMatchesTheConverter()
    {
        Check("SetupSettings", new SetupSettings(null) { Holder = EngineFixture.Engine });
        SetupSettings zero = new(null)
        {
            Holder = EngineFixture.Engine,
            PlayMode = 0,
            VictoryCondition = 0,
            IndependentAIMode = 0,
            ConnectionType = 0,
            TacticalBattleAvailability = 0,
            ExplorationEnabled = false,
            LeadersOnMap = false,
        };
        Check("SetupSettings", zero);
        SetupSettings edge = new(null)
        {
            Holder = EngineFixture.Engine,
            PlayMode = 255,
            VictoryCondition = 255,
            IndependentAIMode = 255,
            ConnectionType = 255,
            TacticalBattleAvailability = 255,
            ExplorationEnabled = true,
            LeadersOnMap = true,
            CustomizeLeaders = true,
            AlliedVictory = true,
            AlwaysViewTacticalBattle = true,
            FileName = "Jörg",
            MapName = "Gate",
            RandomSeed = int.MinValue,
            DayLimit = int.MaxValue,
            TurnTime = int.MinValue,
            HostRemoteId = int.MaxValue,
            MapDay = int.MinValue,
            MapCrc = int.MaxValue,
            SetCrc = int.MinValue,
        };
        Check("SetupSettings", edge);
    }

    [Fact]
    public void CustomAbilityListMatchesTheConverter()
    {
        Check("CustomAbilityList", new CustomAbilityList(null) { Holder = EngineFixture.Engine });
        CustomAbilityList emptyByte = new(null) { Holder = EngineFixture.Engine };
        emptyByte.SetAbilityCount(8);
        Check("CustomAbilityList", emptyByte);
        CustomAbilityList residue = new(null) { Holder = EngineFixture.Engine };
        residue.SetAbilityCount(1);
        byte[] abilityBits = residue.AbilityBits;
        abilityBits[0] = 0x80;
        residue.AbilityBits = abilityBits;
        Check("CustomAbilityList", residue);
    }

    [Fact]
    public void RegisteredAbilityBitsSurviveTheCleanup()
    {
        List<Ability> placed = [];
        try
        {
            placed.Add(PlaceAbility(0));
            placed.Add(PlaceAbility(1));
            placed.Add(PlaceAbility(3));
            CustomAbilityList list = new(null) { Holder = EngineFixture.Engine };
            list.SetAbilityCount(8);
            list.SetAbilitySet(0, value: 1);
            list.SetAbilitySet(1, value: 1);
            list.SetAbilitySet(3, value: 1);
            Check("CustomAbilityList", list);
        }
        finally
        {
            foreach (Ability ability in placed)
            {
                ReleaseAbility(ability);
            }
        }
    }

    [Fact]
    public void AMissingAbilityBitIsClearedAndResidueStays()
    {
        // Image writes the bits and then the cleanup clears the missing bit in memory. The tree
        // write runs after that bit is set again, so both stored images still hold it. Both loads clear it.
        Ability? placed = null;
        CustomAbilityList list = new(null) { Holder = EngineFixture.Engine };
        try
        {
            placed = PlaceAbility(0);
            list.SetAbilityCount(3);
            list.SetAbilitySet(0, value: 1);
            list.SetAbilitySet(1, value: 1);
            byte[] abilityBits = list.AbilityBits;
            abilityBits[0] |= 0xF0;
            list.AbilityBits = abilityBits;
            byte[] image = Image(list);
            list.SetAbilitySet(1, value: 1);
            SchemaClass classValue = s_schema.Get("CustomAbilityList");
            ObjectNode tree = TreeWriter.Write(list, Factory);
            JsonObject expected = PilotImage.Decode(image, className: "CustomAbilityList");
            EqualJson(expected, DocumentBackend.Write(tree, classValue, s_schema, engine: "game"), label: "missing bit write");
            EqualBytes(image, OriginalBackend.Write(tree, classValue, s_schema, engine: "game"), label: "missing bit original");
            EqualBytes(PilotImage.Encode(expected, className: "CustomAbilityList"), image, label: "missing bit converter");
            CustomAbilityList viaReadWrite = (CustomAbilityList)Blank("CustomAbilityList");
            CustomAbilityList viaTree = (CustomAbilityList)Blank("CustomAbilityList");
            CustomAbilityList fromDocument = (CustomAbilityList)Blank("CustomAbilityList");
            try
            {
                Load(viaReadWrite, image);
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: viaTree, Factory);
                TreeReader.Read(DocumentBackend.Read(expected, classValue, s_schema, engine: "game"), objectValue: fromDocument, Factory);
                Assert.True(viaReadWrite.GetAbilitySetAt(0));
                Assert.False(viaReadWrite.GetAbilitySetAt(1));
                Assert.Equal(0xF0, viaReadWrite.AbilityBits[0] & 0xF0);
                Assert.True(viaTree.GetAbilitySetAt(0));
                Assert.False(viaTree.GetAbilitySetAt(1));
                Assert.Equal(0xF0, viaTree.AbilityBits[0] & 0xF0);
                EqualBytes(Image(viaReadWrite), Image(viaTree), label: "missing bit loads");
                EqualBytes(Image(viaReadWrite), Image(fromDocument), label: "missing bit document");
            }
            finally
            {
                fromDocument.Dispose();
                viaTree.Dispose();
                viaReadWrite.Dispose();
            }
        }
        finally
        {
            list.Dispose();
            if (placed is not null)
            {
                ReleaseAbility(placed);
            }
        }
    }

    [Fact]
    public void AChildWhosePersistThrowsIsDroppedUnlessTheHandlerStops()
    {
        EngineClass descriptor = EngineClass.Of<HatredEnchantment>();
        bool added = !EngineFixture.Engine.ClassTable.ContainsKey(descriptor.ClassId);
        if (added)
        {
            EngineFixture.Engine.RegisterEngineClasses([descriptor]);
        }

        GlobalMagicControl control = new(null) { Holder = EngineFixture.Engine };
        Delegate? previous = null;
        try
        {
            HatredEnchantment hatred = new(null, holder: EngineFixture.Engine) { DaysActive = 12 };
            hatred.SetOwner(control.Enchantments);
            byte[] image = Image(control);
            previous = EngineClassCreation.Substitute(descriptor, next: (Func<CustomNode, EngineObject>)(static owner => new ThrowingHatred(owner, holder: EngineFixture.Engine)));
            SchemaClass classValue = s_schema.Get("GlobalMagicControl");
            GlobalMagicControl continued = (GlobalMagicControl)Blank("GlobalMagicControl");
            GlobalMagicControl continuedTree = (GlobalMagicControl)Blank("GlobalMagicControl");
            try
            {
                Load(continued, image);
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: continuedTree, Factory);
                Assert.Equal(0, continued.Enchantments.GetCount());
                Assert.Equal(0, continuedTree.Enchantments.GetCount());
                EqualBytes(Image(continued), Image(continuedTree), label: "dropped child");
            }
            finally
            {
                continuedTree.Dispose();
                continued.Dispose();
            }
            GlobalMagicControl stopped = (GlobalMagicControl)Blank("GlobalMagicControl");
            GlobalMagicControl stoppedTree = (GlobalMagicControl)Blank("GlobalMagicControl");
            try
            {
                InvalidOperationException streamError = Assert.Throws<InvalidOperationException>(() =>
                    Load(stopped, image, static (_, out cont) => cont = false));
                InvalidOperationException treeError = Assert.Throws<InvalidOperationException>(() =>
                    TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: stoppedTree, Factory,
                        static (_, out cont) => cont = false));
                Assert.Equal("hatred failed", streamError.Message);
                Assert.Equal("hatred failed", treeError.Message);
            }
            finally
            {
                stoppedTree.Dispose();
                stopped.Dispose();
            }
        }
        finally
        {
            if (previous is not null)
            {
                _ = EngineClassCreation.Substitute(descriptor, next: previous);
            }

            control.Dispose();
            if (added)
            {
                EngineFixture.Engine.UnregisterEngineClasses([descriptor]);
            }
        }
    }

    [Fact]
    public void AFixedObjectWhosePersistThrowsStaysUnlessTheHandlerStops()
    {
        using GlobalMagicControl source = new(null) { Holder = EngineFixture.Engine };
        source.NextEnchantmentId = 9;
        source.DoubleSphereManaCount[0] = 3;
        byte[] image = Image(source);
        SchemaClass classValue = s_schema.Get("GlobalMagicControl");
        GlobalMagicControl continued = Faulted(4);
        GlobalMagicControl continuedTree = Faulted(4);
        GlobalEnchantmentList continuedList = continued.Enchantments;
        GlobalEnchantmentList continuedTreeList = continuedTree.Enchantments;
        try
        {
            Load(continued, image);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: continuedTree, Factory);
            Assert.Same(continuedList, continued.Enchantments);
            Assert.Same(continuedTreeList, continuedTree.Enchantments);
            Assert.Equal(9, continued.NextEnchantmentId);
            Assert.Equal(9, continuedTree.NextEnchantmentId);
            Assert.Equal(3, continued.DoubleSphereManaCount[0]);
            Assert.Equal(3, continuedTree.DoubleSphereManaCount[0]);
            for (int i = 1; i < 9; i++)
            {
                Assert.Equal(0, continued.DoubleSphereManaCount[i]);
                Assert.Equal(continued.DoubleSphereManaCount[i], continuedTree.DoubleSphereManaCount[i]);
            }
        }
        finally
        {
            continuedTree.Dispose();
            continued.Dispose();
        }
        GlobalMagicControl stopped = Faulted(4);
        GlobalMagicControl stoppedTree = Faulted(4);
        GlobalEnchantmentList stoppedList = stopped.Enchantments;
        GlobalEnchantmentList stoppedTreeList = stoppedTree.Enchantments;
        try
        {
            InvalidOperationException streamError = Assert.Throws<InvalidOperationException>(() =>
                Load(stopped, image, static (_, out cont) => cont = false));
            InvalidOperationException treeError = Assert.Throws<InvalidOperationException>(() =>
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: stoppedTree, Factory,
                    static (_, out cont) => cont = false));
            Assert.Equal("enchantments failed", streamError.Message);
            Assert.Equal("enchantments failed", treeError.Message);
            Assert.Same(stoppedList, stopped.Enchantments);
            Assert.Same(stoppedTreeList, stoppedTree.Enchantments);
            Assert.Equal(4, stopped.NextEnchantmentId);
            Assert.Equal(4, stoppedTree.NextEnchantmentId);
        }
        finally
        {
            stoppedTree.Dispose();
            stopped.Dispose();
        }
    }

    private static GlobalMagicControl Faulted(int sentinel)
    {
        GlobalMagicControl control = new(null) { Holder = EngineFixture.Engine };
        control.Enchantments?.Dispose();
        control.Enchantments = new ThrowingEnchantments(null) { Holder = EngineFixture.Engine };
        control.NextEnchantmentId = sentinel;
        return control;
    }

    // The three stored bytes after PlayerType are declared padding on SetupPlayerBehaviorImage.
    // The tree, the original backend and the converter ignore them on read and write 0.
    [Fact]
    public void SetupPlayerBehaviorIgnoresPaddingBytes()
    {
        SetupPlayerBehavior behavior = new(PlayerType: 2, Flags: 9);
        BehaviorProbe probe = new() { Behavior = behavior };
        SchemaClass classValue = s_schema.Get("PlayerSetupSettings");
        ObjectNode tree = TreeWriter.Write(probe, Factory);
        JsonObject? stored = (JsonObject?)DocumentBackend.Write(tree, classValue, s_schema, engine: "game")["Behavior"];
        Assert.Equal(["PlayerType", "Flags"], stored.Members.Select(member => member.Name).ToList());
        Assert.Equal(2, ((JsonInteger)stored["PlayerType"]).Value);
        Assert.Equal(9, ((JsonInteger)stored["Flags"]).Value);
        byte[] imageBytes = OriginalBackend.Write(tree, classValue, s_schema, engine: "game");
        // Put nonzero padding bytes into the stored 8-byte behavior record and verify load ignores them.
        byte[] withPadding = [.. imageBytes];
        withPadding[3 + 1] = 0x01;
        withPadding[3 + 2] = 0x00;
        withPadding[3 + 3] = 0x02;
        ObjectNode back = OriginalBackend.Read(withPadding, classValue, s_schema, engine: "game");
        BehaviorProbe loaded = new();
        TreeReader.Read(back, objectValue: loaded, Factory);
        Assert.Equal(behavior, loaded.Behavior);
        Assert.Equal(imageBytes, OriginalBackend.Write(back, classValue, s_schema, engine: "game"));
        Assert.Equal(["PlayerType", "Flags"], ((JsonObject)PilotImage.Decode(withPadding, className: "PlayerSetupSettings")["Behavior"]).Members.Select(member => member.Name).ToList());

        using PlayerSetupSettings settings = new(null) { Holder = EngineFixture.Engine };
        settings.Behavior = behavior;
        settings.OriginalBehavior = behavior with { PlayerType = 5 };
        byte[] image = Image(settings);
        JsonObject document = PilotImage.Decode(image, className: "PlayerSetupSettings");
        EqualJson(new JsonObject().Add("Behavior", stored), new JsonObject().Add("Behavior", document["Behavior"]), label: "PlayerSetupSettings behavior");
        Assert.Equal(["PlayerType", "Flags"], ((JsonObject)document["OriginalBehavior"]).Members.Select(member => member.Name).ToList());
        using PlayerSetupSettings read = new(null) { Holder = EngineFixture.Engine };
        Load(read, image);
        Assert.Equal(settings.Behavior, read.Behavior);
        Assert.Equal(settings.OriginalBehavior, read.OriginalBehavior);
    }

    // Bits past AbilityCount inside the last stored byte are game state in 1.36: AssignAbility
    // leaves them on shrink (Ability.PAS:1702), CompareAbility compares whole bytes (:1147), and
    // growing the count revives them (:954-977, :1122-1126). Own JSON keeps them in AbilityBits.
    [Fact]
    public void AbilityBitsPastTheCountArePreservedAsState()
    {
        Ability? placed = null;
        try
        {
            placed = PlaceAbility(0);
            using AbilityOwner target = new(null) { Holder = EngineFixture.Engine };
            using AbilityOwner source = new(null) { Holder = EngineFixture.Engine };
            target.SetAbilitySet(0, value: 1);
            target.SetAbilitySet(7, value: 1);
            Assert.Equal(8, target.AbilityCount);
            source.SetAbilityCount(1);
            source.SetAbilitySet(0, value: 1);
            target.AssignAbility(source);
            Assert.Equal(1, target.AbilityCount);
            Assert.False(target.GetAbilitySetAt(7));
            Assert.Equal(0x81, target.AbilityBits[0]);
            Assert.False(target.CompareAbility(source));

            SchemaClass classValue = s_schema.Get("CustomAbilityList");
            JsonObject document = DocumentBackend.Write(TreeWriter.Write(target, Factory), classValue, s_schema, engine: "game");
            JsonArray? storedBits = (JsonArray?)document["AbilityBits"];
            Assert.Equal([0L, 7L], storedBits.Items.Select(item => ((JsonInteger)item).Value).ToList());

            using AbilityOwner reloaded = new(null) { Holder = EngineFixture.Engine };
            TreeReader.Read(DocumentBackend.Read(document, classValue, s_schema, engine: "game"), objectValue: reloaded, Factory);
            Assert.Equal(1, reloaded.AbilityCount);
            Assert.Equal(0x81, reloaded.AbilityBits[0]);
            Assert.True(reloaded.CompareAbility(target));
            Assert.False(reloaded.CompareAbility(source));
            // Growing the count inside the same byte revives bit 7 (Ability.PAS:954-977, :1122-1126).
            reloaded.SetAbilityCount(8);
            Assert.Equal(8, reloaded.AbilityCount);
            Assert.True(reloaded.GetAbilitySetAt(7));
        }
        finally
        {
            if (placed is not null)
            {
                ReleaseAbility(placed);
            }
        }
    }

    [Fact]
    public void AChampionByteOtherThanZeroOrOneRoundTripsThroughReadWrite()
    {
        using UnitResource source = new(null) { Holder = EngineFixture.Engine };
        bool champion = source.Champion;
        Unsafe.As<bool, byte>(ref champion) = 2;
        source.Champion = champion;
        byte[] image = Image(source);
        using UnitResource loaded = new(null) { Holder = EngineFixture.Engine };
        Load(loaded, image);
        bool champion2 = loaded.Champion;
        Assert.Equal(2, Unsafe.As<bool, byte>(ref champion2));
        loaded.Champion = champion2;
        EqualBytes(image, Image(loaded), label: "champion 02");
    }

    [Fact]
    public void CsSaveLoadIsTheStoredNameOfTheAlias()
    {
        EnumTypeInfo info = EnumTypeInfo.Describe(typeof(ControlStyles));
        Assert.Equal("SaveLoad", info.ByValue[0]);
        Assert.False(info.ByName.ContainsKey("csStored"));
        Assert.Equal(0, info.ByName["SaveLoad"]);
    }

    private static void WithDestroyingMap(Action<StrategicMap> body)
    {
        StrategicMap? previous = StrategicMap.Current;
        StrategicMap map = new(null, holder: EngineFixture.Engine)
        {
            Status = HexSpriteMapStatuses.Destroying
        };
        StrategicMap.Current = map;
        try
        {
            body(map);
        }
        finally
        {
            StrategicMap.Current = previous;
            map.Dispose();
        }
    }

    [Fact]
    public void EngineNodesMatchTheConverter()
    {
        Check("ECustomOwner", new EngineNode(null) { Holder = EngineFixture.Engine });
        EngineNode owner = new(null) { Holder = EngineFixture.Engine };
        owner.SetControlStyle((ControlStyles)0xA5);
        Check("ECustomOwner", owner);
        ResourceCollection resources = new(null) { Holder = EngineFixture.Engine };
        resources.SetControlStyle(ControlStyles.None);
        Check("EResourceList", resources);
        Check("EControlStyleNode", new ControlStyleNode(null) { Holder = EngineFixture.Engine });
        ControlStyleNode styled = new(null) { Holder = EngineFixture.Engine };
        styled.SetControlStyle((ControlStyles)0x81);
        new AISavingsBudget(null) { Holder = EngineFixture.Engine, Priority = 7, Gold = -3 }.SetOwner(styled);
        new AITransporterBudget(null) { Holder = EngineFixture.Engine }.SetOwner(styled);
        Check("EControlStyleNode", styled);
        PluginControl plugins = new(null) { Holder = EngineFixture.Engine };
        _ = plugins.UsedPlugins.Add("Abilities");
        Check("PluginControl", plugins);
        Check("PluginControl", new PluginControl(null) { Holder = EngineFixture.Engine });
    }

    [Fact]
    public void ASortNodeRefusesBothWritesAlike()
    {
        foreach (SortedNode node in new SortedNode[] { new(null) { Holder = EngineFixture.Engine }, new InverseSortedNode(null) { Holder = EngineFixture.Engine }, new ControlStyleInverseSortedNode(null) { Holder = EngineFixture.Engine } })
        {
            try
            {
                NotSupportedException stream = Assert.Throws<NotSupportedException>(() => Image(node));
                NotSupportedException tree = Assert.Throws<NotSupportedException>(() => TreeWriter.Write(node, Factory));
                Assert.Equal(stream.Message, tree.Message);
            }
            finally
            {
                node.Dispose();
            }
        }
    }

    // A reference list stores the children with no owner or with the list as owner. The write
    // leaves every owner as it was.
    [Fact]
    public void AReferenceListStoresItsUnownedChildrenAndTakesOneReferenceEach()
    {
        CombatActionList list = new(null) { Holder = EngineFixture.Engine };
        CombatActionList other = new(null) { Holder = EngineFixture.Engine };
        try
        {
            DamageCombatAction free = new(null) { Holder = EngineFixture.Engine, Damage = 9, SourceId = 3, TargetId = 4 };
            _ = list.AddChild(free);
            DamageCombatAction owned = new(null) { Holder = EngineFixture.Engine, Damage = 1 };
            _ = list.AddChild(owned);
            owned.SetOwnerWithoutNotify(list);
            DamageCombatAction elsewhere = new(null) { Holder = EngineFixture.Engine, Damage = 2 };
            _ = list.AddChild(elsewhere);
            elsewhere.SetOwnerWithoutNotify(other);
            SchemaClass classValue = s_schema.Get("CombatActionList");
            ObjectNode tree = TreeWriter.Write(list, Factory);
            Assert.Null(free.Owner);
            Assert.Same(list, owned.Owner);
            Assert.Same(other, elsewhere.Owner);
            byte[] image = Image(list);
            Assert.Same(list, owned.Owner);
            Assert.Same(other, elsewhere.Owner);
            JsonObject expected = PilotImage.Decode(image, className: "CombatActionList");
            Assert.Equal(2, ((JsonArray)expected["Children"]).Items.Count);
            EqualJson(expected, DocumentBackend.Write(tree, classValue, s_schema, engine: "game"), label: "reference list write");
            EqualBytes(image, OriginalBackend.Write(tree, classValue, s_schema, engine: "game"), label: "reference list original");
            CombatActionList viaReadWrite = (CombatActionList)Blank("CombatActionList");
            CombatActionList fromDocument = (CombatActionList)Blank("CombatActionList");
            CombatActionList fromImage = (CombatActionList)Blank("CombatActionList");
            try
            {
                Load(viaReadWrite, image);
                TreeReader.Read(DocumentBackend.Read(expected, classValue, s_schema, engine: "game"), objectValue: fromDocument, Factory);
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: fromImage, Factory);
                foreach (CombatActionList loaded in new[] { viaReadWrite, fromDocument, fromImage })
                {
                    Assert.Equal(2, loaded.Count);
                    for (int index = 0; index < loaded.Count; index++)
                    {
                        Assert.Null(loaded[index].Owner);
                        Assert.Equal(1, loaded[index].RefCount);
                    }
                }
                EqualBytes(Image(viaReadWrite), Image(fromDocument), label: "reference list document read");
                EqualBytes(Image(viaReadWrite), Image(fromImage), label: "reference list original read");
            }
            finally
            {
                fromImage.Dispose();
                fromDocument.Dispose();
                viaReadWrite.Dispose();
            }
        }
        finally
        {
            list.Dispose();
            other.Dispose();
        }
    }

    // A preview load keeps an ability bit whose ability is missing, as ReadWrite does with
    // smPreview, and a full load clears it.
    [Fact]
    public void APreviewLoadSkipsTheAbilityCleanup()
    {
        CustomAbilityList list = new(null) { Holder = EngineFixture.Engine };
        list.SetAbilityCount(8);
        list.SetAbilitySet(5, value: 1);
        SchemaClass classValue = s_schema.Get("CustomAbilityList");
        byte[] image = Image(list);
        CustomAbilityList preview = (CustomAbilityList)Blank("CustomAbilityList");
        CustomAbilityList previewTree = (CustomAbilityList)Blank("CustomAbilityList");
        CustomAbilityList full = (CustomAbilityList)Blank("CustomAbilityList");
        try
        {
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: preview, Factory, preview: true);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: previewTree, Factory, preview: true);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: full, Factory);
            Assert.True(preview.GetAbilitySetAt(5));
            Assert.True(previewTree.GetAbilitySetAt(5));
            Assert.False(full.GetAbilitySetAt(5));
        }
        finally
        {
            full.Dispose();
            previewTree.Dispose();
            preview.Dispose();
            list.Dispose();
        }
    }

    // A preview load and save of an item control moves no item, as ReadWrite does with smPreview.
    // The preview load leaves the stored items unread.
    [Fact]
    public void APreviewMovesNoItem()
    {
        using ItemControl control = new(null) { Holder = EngineFixture.Engine };
        control.RegisterItem(PlainItem(itemId: 4, name: "Ring"));
        ObjectNode tree = TreeWriter.Write(control, Factory);
        Assert.NotNull(tree.Find("Items"));
        Assert.Null(TreeWriter.Write(control, Factory, preview: true).Find("Items"));
        using ItemControl loaded = (ItemControl)Blank("ItemControl");
        TreeReader.Read(tree, objectValue: loaded, Factory, preview: true);
        Assert.Equal(0, loaded.GetCount());
    }

    private static void Each<TEnum>(Action<TEnum> check) where TEnum : struct, Enum
    {
        foreach (TEnum value in Enum.GetValues<TEnum>())
        {
            check(value);
        }
    }

    // Three constructed states own a child the live table does not enlist. Both loads drop that
    // child, and the converter document still holds it, so those states pass converterRead false.
    // The record pilots have no children, so their tree read also matches the converter encode.
    private static void Check(string className, EngineObject objectValue, bool converterRead = true)
    {
        EngineObject? loaded = null;
        EngineObject? fromImage = null;
        EngineObject? viaReadWrite = null;
        try
        {
            SchemaClass classValue = s_schema.Get(className);
            byte[] image = Image(objectValue);
            JsonObject expected = PilotImage.Decode(image, className);
            ObjectNode tree = TreeWriter.Write(objectValue, Factory);
            EqualJson(actual: DocumentBackend.Write(tree, classValue, s_schema, engine: "game"), expected: expected, label: $"{className} write");
            EqualBytes(image, OriginalBackend.Write(tree, classValue, s_schema, engine: "game"), label: $"{className} original");
            viaReadWrite = Blank(className);
            Load(viaReadWrite, image);
            byte[] read = Image(viaReadWrite);
            loaded = Blank(className);
            TreeReader.Read(DocumentBackend.Read(expected, classValue, s_schema, engine: "game"), objectValue: loaded, Factory);
            if (converterRead)
            {
                EqualBytes(PilotImage.Encode(expected, className), Image(loaded), label: $"{className} converter read");
            }

            EqualBytes(read, Image(loaded), label: $"{className} read");
            fromImage = Blank(className);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: fromImage, Factory);
            EqualBytes(read, Image(fromImage), label: $"{className} original read");
        }
        finally
        {
            fromImage?.Dispose();
            loaded?.Dispose();
            viaReadWrite?.Dispose();
            objectValue.Dispose();
        }
    }

    private static EngineObject Blank(string className)
    {
        foreach (EngineClass engineClass in EngineClass.All)
        {
            if (engineClass.StorageKey == className)
            {
                return engineClass.CreateInstance(null, holder: EngineFixture.Engine);
            }
        }

        throw new InvalidOperationException($"The engine has no class {className}.");
    }

    private static void Load(EngineObject objectValue, byte[] image, LoadErrorHandler? onError = null)
    {
        SchemaClass classValue = s_schema.Get(Factory.ClassName(objectValue));
        TreeReader.Read(OriginalBackend.Read(engine: GameBoundary.PacketClassTable(classValue), image: image, classValue: classValue, schema: s_schema), objectValue: objectValue, factory: Factory, onError: onError);
    }

    private static Ability PlaceAbility(int id)
    {
        Ability ability = new(null)
        {
            Holder = EngineFixture.Engine,
            AbilityId = id
        };
        List<Ability?> list = GamePack.Current.AbilityControl.Abilities;
        while (id >= list.Count)
        {
            list.Add(null);
        }

        list[id] = ability;
        return ability;
    }

    private static void ReleaseAbility(Ability ability)
    {
        List<Ability?> list = GamePack.Current.AbilityControl.Abilities;
        int id = ability.AbilityId;
        if ((uint)id < (uint)list.Count && ReferenceEquals(list[id], ability))
        {
            list[id] = null;
        }

        ability.Dispose();
    }

    private sealed class ThrowingHatred(CustomNode owner, EngineObject holder) : HatredEnchantment(owner, holder)
    {
        public override void PersistFields(IPersister persister) => throw new InvalidOperationException("hatred failed");
    }

    private sealed class ThrowingEnchantments(CustomNode? owner) : GlobalEnchantmentList(owner)
    {
        public override void PersistFields(IPersister persister) => throw new InvalidOperationException("enchantments failed");
    }

    private sealed class BehaviorProbe : IPersistable
    {
        public SetupPlayerBehavior Behavior { get; set; }

        public void Persist(IPersister persister) => Behavior = PersistSupport.Record<SetupPlayerBehaviorImage, SetupPlayerBehavior>(persister, "Behavior", Behavior);
    }

    // A unit index list stored under the integer list of ItemExplorationSiteEventLog.
    private sealed class UnitIndexProbe : IPersistable
    {
        public UnitIndexList Units { get; } = [];

        public void Persist(IPersister persister) => PersistSupport.UnitIndexList(persister, name: "ItemIds", list: Units);
    }

    private static byte[] Image(EngineObject? objectValue)
    {
        ObjectNode root = TreeWriter.Write(objectValue, factory: Factory);
        SchemaClass classValue = s_schema.Get(Factory.ClassName(objectValue));
        return OriginalBackend.Write(root, classValue: classValue, schema: s_schema, engine: GameBoundary.PacketClassTable(classValue));
    }

    private static void EqualJson(JsonObject expected, JsonObject actual, string label)
    {
        byte[] left = JsonWriter.ToUtf8(expected);
        byte[] right = JsonWriter.ToUtf8(actual);
        if (left.AsSpan().SequenceEqual(right))
        {
            return;
        }

        Assert.Fail($"{label}\n{Encoding.UTF8.GetString(right)}");
    }

    private static void EqualBytes(byte[]? expected, byte[]? actual, string label)
    {
        if (expected.AsSpan().SequenceEqual(actual))
        {
            return;
        }

        Assert.Fail($"{label} at {expected.AsSpan().CommonPrefixLength(actual)}");
    }
}
