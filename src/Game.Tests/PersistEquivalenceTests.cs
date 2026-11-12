using System.Runtime.CompilerServices;
using System.Text;
using Aow.Engine;
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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
public sealed class PersistEquivalenceTests
{
    private static readonly FormatSchema s_schema = FormatSchema.Current;
    private static EnginePersistableFactory? s_factory;

    private static EnginePersistableFactory Factory => s_factory ??= new(EngineFixture.Engine);

    [Fact]
    public void EnchantmentMatchesTheConverter()
    {
        Check("Enchantment", new Enchantment(null!));
        var zero = new Enchantment(null!) { DispelResistance = 0, Source = 0, Upkeep = 0, EnchantmentId = 0 };
        Check("Enchantment", zero);
        var edge = new Enchantment(null!)
        {
            DispelResistance = -1,
            Source = 127,
            Upkeep = int.MinValue,
            EnchantmentId = int.MaxValue,
        };
        Check("Enchantment", edge);
        Check("Enchantment", new Enchantment(null!) { DispelResistance = -128, Source = 0, Upkeep = int.MaxValue, EnchantmentId = int.MinValue });
    }

    [Fact]
    public void EventLogMatchesTheConverter()
    {
        Check("EventLog", new EventLog(null!));
        Check("EventLog", new EventLog(null!) { _day = 0, _player = 0, Id = 0 });
        Check("EventLog", new EventLog(null!) { _day = int.MinValue, _player = -128, Id = int.MaxValue });
        Check("EventLog", new EventLog(null!) { _day = int.MaxValue, _player = 127, Id = int.MinValue });
    }

    [Fact]
    public void MessageEventLogMatchesTheConverter()
    {
        Check("MessageEventLog", new MessageEventLog(null!));
        var empty = new MessageEventLog(null!) { _day = 0, _player = 0, Id = 0, Title = "", Layout = 0, Buttons = 0 };
        Check("MessageEventLog", empty);
        var full = new MessageEventLog(null!)
        {
            _day = int.MaxValue,
            _player = -1,
            Id = int.MinValue,
            Title = "Jörg",
            Layout = MessageEventLogLayout.Header,
            Buttons = MessageEventLogButtons.YesNoCancel,
        };
        full.Message.Add("one");
        full.Message.Add("");
        full.Message.Add("two");
        Check("MessageEventLog", full);
        var edge = new MessageEventLog(null!) { Layout = (MessageEventLogLayout)255, Buttons = (MessageEventLogButtons)255, Title = "" };
        edge.Message.Add("line");
        Check("MessageEventLog", edge);
    }

    [Fact]
    public void TokenEventResourceEditSettingsMatchesTheConverter()
    {
        Check("ControlStyleObject", new ControlStyleObject(null!));
        var zeroStyle = new ControlStyleObject(null!) { _controlStyle = Set<ControlStyleElement>.FromMask(0) };
        Check("ControlStyleObject", zeroStyle);
        var bits = new ControlStyleObject(null!) { _controlStyle = Set<ControlStyleElement>.FromMask(0xA5) };
        Check("ControlStyleObject", bits);
        Check("ResourceEditSettings", new ResourceEditSettings(null!));
        var editors = new ResourceEditSettings(null!) { _controlStyle = Set<ControlStyleElement>.FromMask(0) };
        editors.Editors.AddObject("", Runtime.IntToObject(0));
        editors.Editors.AddObject("Gate", Runtime.IntToObject(4));
        Check("ResourceEditSettings", editors);
    }

    [Fact]
    public void ItemGraphicsMatchesTheConverter()
    {
        Check("EngineResource", new EngineResource(null!));
        Check("EngineResource", new EngineResource(null!) { _resourceId = int.MinValue, _editId = int.MaxValue, _editIndex = 0 });
        Check("ItemGraphics", new ItemGraphics(null!));
        var zero = new ItemGraphics(null!)
        {
            _resourceId = 0,
            _editId = 0,
            _editIndex = 0,
            ImageLibraryName = "",
            ImageIndex = 0,
            ItemType = 0,
            Hidden = false,
        };
        Check("ItemGraphics", zero);
        var full = new ItemGraphics(null!)
        {
            _resourceId = int.MaxValue,
            _editId = int.MinValue,
            _editIndex = 3,
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
        Check("AIBudget", new AIBudget(null!));
        Check("AISavingsBudget", new AISavingsBudget(null!));
        Check("AITransporterBudget", new AITransporterBudget(null!));
        Check("AICityFortifyBudget", new AICityFortifyBudget(null!));
        Check("AICityUpgradeBudget", new AICityUpgradeBudget(null!));
        Check("AICityResearchBudget", new AICityResearchBudget(null!));
        var edge = new AIBudget(null!)
        {
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
        var manager = new AIBudgetManager(null!);
        Check("AIBudgetManager", manager);
        var populated = new AIBudgetManager(null!)
        {
            Gold = int.MaxValue,
            Mana = int.MinValue,
            GoldUpkeep = 1,
            ManaUpkeep = 2,
            ProductionGoldUpkeep = 3,
            ProductionManaUpkeep = 4,
        };
        new AISavingsBudget(null!).SetOwner(populated.Budgets);
        new AITransporterBudget(null!).SetOwner(populated.Budgets);
        new AICityResearchBudget(null!).SetOwner(populated.Budgets);
        Check("AIBudgetManager", populated, converterRead: false);
    }

    [Fact]
    public void FrozenWaterHexSpriteMatchesTheConverter()
    {
        Check("FrozenWaterHexSprite", new FrozenWaterHexSprite(null!));
        Check("FrozenWaterHexSprite", new FrozenWaterHexSprite(null!) { DaysRemaining = 0, CasterPlayer = 0 });
        Check("FrozenWaterHexSprite", new FrozenWaterHexSprite(null!) { DaysRemaining = -128, CasterPlayer = 127 });
        Check("FrozenWaterHexSprite", new FrozenWaterHexSprite(null!) { DaysRemaining = 127, CasterPlayer = -1 });
    }

    // A level of 5 by 3 fields: field [0][0] is empty, the others hold up to three frozen water
    // sprites and nonzero field flags. The grid, the sprite sequence and the progress reports
    // match MainReadWrite on load and on save, through both backends.
    [Theory]
    [InlineData("MapLevel", "game")]
    [InlineData("CombatMapLevel", "tactical")]
    public void MapLevelMatchesTheConverter(string className, string engine)
    {
        var classValue = s_schema.Get(className);
        var document = LevelDocument(width: 5, height: 3);
        var image = PilotImage.Encode(document, className, engine);
        bool added = Register<FrozenWaterHexSprite>();
        var (viaReadWrite, streamMap) = NewLevel(className);
        var (fromImage, imageMap) = NewLevel(className);
        var (fromDocument, documentMap) = NewLevel(className);
        try
        {
            Load(viaReadWrite, image);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine), fromImage, Factory);
            TreeReader.Read(DocumentBackend.Read(document, classValue, s_schema, engine), fromDocument, Factory);
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
            var rewritten = Image(viaReadWrite);
            EqualBytes(rewritten, Image(fromImage), className + " original read");
            EqualBytes(rewritten, Image(fromDocument), className + " document read");
            int treeSaved = imageMap.Reports.Count;
            var tree = TreeWriter.Write(fromImage, Factory);
            Assert.Equal(streamMap.Reports[streamSaved..], imageMap.Reports[treeSaved..]);
            Assert.Equal((MapProgressType.Saving, 0), imageMap.Reports[treeSaved]);
            EqualJson(PilotImage.Decode(rewritten, className, engine), DocumentBackend.Write(tree, classValue, s_schema, engine), className + " write");
            EqualBytes(rewritten, OriginalBackend.Write(tree, classValue, s_schema, engine), className + " original");
        }
        finally
        {
            fromDocument.Dispose();
            fromImage.Dispose();
            viaReadWrite.Dispose();
            if (added) EngineFixture.Engine.UnregisterEngineClasses([EngineClass.Of<FrozenWaterHexSprite>()]);
        }
    }

    [Fact]
    public void AnAbsentGridLeavesTheLevelEmpty()
    {
        var classValue = s_schema.Get("MapLevel");
        var document = (JsonObject)JsonReader.Parse("""{"Width":2,"Height":1,"HexLevel":1}"""u8, "level.json");
        var image = PilotImage.Encode(document, "MapLevel");
        var (viaReadWrite, streamMap) = NewLevel("MapLevel");
        var (fromImage, imageMap) = NewLevel("MapLevel");
        try
        {
            Load(viaReadWrite, image);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), fromImage, Factory);
            Assert.Equal(streamMap.Reports, imageMap.Reports);
            Assert.Equal(2, fromImage.GetCount());
            Assert.Equal(0, fromImage.GetField(1, 0).GetCount());
            EqualBytes(Image(viaReadWrite), Image(fromImage), "absent grid");
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
        var classValue = s_schema.Get("MapLevel");
        var image = PilotImage.Encode(LevelDocument(width: 2, height: 1), "MapLevel");
        var descriptor = EngineClass.Of<FrozenWaterHexSprite>();
        bool added = Register<FrozenWaterHexSprite>();
        var previous = EngineClassCreation.Substitute(descriptor, next: static owner => new ThrowingFrozenWater(owner));
        try
        {
            var (continued, _) = NewLevel("MapLevel");
            var (continuedTree, _) = NewLevel("MapLevel");
            try
            {
                Load(continued, image);
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), continuedTree, Factory);
                Assert.Equal(0, continued.GetField(1, 0).GetCount());
                Assert.Equal(0, continuedTree.GetField(1, 0).GetCount());
                EqualBytes(Image(continued), Image(continuedTree), "dropped sprites");
            }
            finally
            {
                continuedTree.Dispose();
                continued.Dispose();
            }
            var (stopped, _) = NewLevel("MapLevel");
            var (stoppedTree, _) = NewLevel("MapLevel");
            try
            {
                var streamError = Assert.Throws<InvalidOperationException>(() =>
                    Load(stopped, image, static (StorageStream _, string _, ref bool cont) => cont = false));
                var treeError = Assert.Throws<InvalidOperationException>(() =>
                    TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), stoppedTree, Factory,
                        static (string _, out bool cont) => cont = false));
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
            EngineClassCreation.Substitute(descriptor, next: previous);
            if (added) EngineFixture.Engine.UnregisterEngineClasses([descriptor]);
        }
    }

    // The fixture engine enlists the classes its other tests create. A stored sprite is created by its class id.
    private static bool Register<T>() where T : EngineObject
    {
        var descriptor = EngineClass.Of<T>();
        if (EngineFixture.Engine.ClassTable.ContainsKey(descriptor.ClassId)) return false;
        EngineFixture.Engine.RegisterEngineClasses([descriptor]);
        return true;
    }

    // Field [y][x] holds (x + y) % 4 sprites with distinct values, and flags x * 100 + y + 1.
    private static JsonObject LevelDocument(int width, int height)
    {
        var rows = new JsonArray();
        for (int y = 0; y < height; y++)
        {
            var row = new JsonArray();
            for (int x = 0; x < width; x++)
            {
                var field = new JsonObject();
                if (x != 0 || y != 0)
                {
                    var sprites = new JsonArray();
                    for (int k = 0; k < (x + y) % 4; k++)
                        sprites.Add(new JsonObject()
                            .Add("$class", new JsonString("FrozenWaterHexSprite"))
                            .Add("DaysRemaining", new JsonInteger(k + 1))
                            .Add("CasterPlayer", new JsonInteger(x - y)));
                    if (sprites.Items.Count != 0) field.Add("HexSpriteList", sprites);
                    field.Add("FieldFlags", new JsonInteger(x * 100 + y + 1));
                }
                row.Add(field);
            }
            rows.Add(row);
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
        var map = new ProgressMap();
        var container = new MapContainer(map) { Map = map };
        MapLevel level = className == "CombatMapLevel" ? new CombatMapLevel(container) : new MapLevel(container);
        return (level, map);
    }

    private sealed class ProgressMap() : GameMap(null!)
    {
        public List<(MapProgressType Type, int Percent)> Reports { get; } = [];

        public override void NotifyProgress(in MapProgress progress) => Reports.Add((progress.ProgressType, progress.Percent));
    }

    private sealed class ThrowingFrozenWater : FrozenWaterHexSprite
    {
        public ThrowingFrozenWater(ECustomNode owner) : base(owner)
        {
        }

        public override void ReadWrite(StorageStream stream)
        {
            if (stream.StorageMode().Contains(StorageModeElement.Read)) throw new InvalidOperationException("frozen water failed");
            base.ReadWrite(stream);
        }

        public override void PersistFields(IPersister persister)
        {
            if (persister.Loading) throw new InvalidOperationException("frozen water failed");
            base.PersistFields(persister);
        }
    }

    [Fact]
    public void FloodEnchantmentMatchesTheConverter()
    {
        Check("GlobalEnchantment", new GlobalEnchantment(null!));
        var named = new GlobalEnchantment(null!)
        {
            _enchantmentId = int.MinValue,
            EnchantmentType = (GlobalEnchantmentType)255,
            PlayerIndex = -1,
            Name = "Jörg",
            EnchantmentManaUpkeep = int.MaxValue,
            DispelMana = 0,
        };
        Check("GlobalEnchantment", named);
        Check("FloodControl", new FloodControl(null!));
        var terrain = new FloodControl(null!) { Width = 2, Height = 1 };
        terrain.OriginalTerrain = [0, -1, 127, -128];
        Check("FloodControl", terrain);
        Check("FloodEnchantment", new FloodEnchantment(null!));
        var flood = new FloodEnchantment(null!)
        {
            _enchantmentId = 9,
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
        Check("FloodEnchantment", new FloodEnchantment(null!) { Removed = false, RestoreDays = -128, DispelPlayer = -128, TurnsActive = int.MinValue });
    }

    // The single stores its exact bits: the document holds a number, and $bits for negative zero, an
    // infinity and a NaN with its payload.
    [Fact]
    public void TheSingleOfTheAIMatchesTheConverter()
    {
        foreach (int bits in new[] { 0, 0x3DCCCCCD, 0x3F800000, 0x3F000000, 0x3727C5AC, 0x7F7FFFFF, int.MinValue, 0x7F800000, unchecked((int)0xFF800000), 0x7FC00001 })
            Check("AIPlayerControl", new AIPlayerControl(null!) { DefenseStrengthMultiplier = BitConverter.Int32BitsToSingle(bits) });
    }

    // The fixed objects of the controls persist in place at their slots. The empty controls store no member.
    [Fact]
    public void TheEmptyAbilityAndSpellControlsMatchTheConverter()
    {
        Check("AbilityControl", new AbilityControl(null!));
        Check("SpellControl", new SpellControl(null!));
    }

    [Fact]
    public void UnitEnchantmentMatchesTheConverter()
    {
        Check("UnitEnchantment", new UnitEnchantment(null!));
        Check("UnitEnchantment", new UnitEnchantment(null!) { EnchantmentAbilityId = int.MinValue, UnitId = int.MaxValue, Source = 0, DispelResistance = 0, Upkeep = 0 });
        Check("CosmeticSurgeryEnchantment", new CosmeticSurgeryEnchantment(null!) { FaceIndex = int.MaxValue });
        Check("CosmeticSurgeryEnchantment", new CosmeticSurgeryEnchantment(null!) { FaceIndex = int.MinValue, EnchantmentId = 0, Upkeep = 0, Source = 0, DispelResistance = 0 });
        Check("UnitEnchantmentAbilityData", new UnitEnchantmentAbilityData(null!));
        var absent = new UnitEnchantmentAbilityData(null!)
        {
            EnchantmentAbilityId = 0,
            EnchantmentPlayerIndex = -1,
            EnchantmentDuration = -1,
            Enchantment = null,
        };
        Check("UnitEnchantmentAbilityData", absent);
        var present = new UnitEnchantmentAbilityData(null!)
        {
            EnchantmentAbilityId = 65535,
            EnchantmentPlayerIndex = 0,
            EnchantmentDuration = -128,
            Enchantment = new CosmeticSurgeryEnchantment(null!) { FaceIndex = 3, UnitId = int.MinValue },
        };
        Check("UnitEnchantmentAbilityData", present);
        var edge = new UnitEnchantmentAbilityData(null!)
        {
            EnchantmentAbilityId = 1,
            EnchantmentPlayerIndex = 127,
            EnchantmentDuration = 127,
            Enchantment = new UnitEnchantment(null!) { EnchantmentAbilityId = int.MaxValue },
        };
        Check("UnitEnchantmentAbilityData", edge);
    }

    [Fact]
    public void DeclaredEnumMembersAndAnUndeclaredByteRoundTrip()
    {
        Each<DiplomaticRelation>(value => Check("DiplomaticRelationHistoryItem", new DiplomaticRelationHistoryItem(null!) { Day = 3, Relation = value }));
        Check("DiplomaticRelationHistoryItem", new DiplomaticRelationHistoryItem(null!) { Relation = (DiplomaticRelation)255, Day = int.MinValue });
        Each<ChangeGlobalMagicTokenEventMode>(value => Check("ChangeGlobalMagicTokenEvent", new ChangeGlobalMagicTokenEvent(null!) { Mode = value, PlayerIndex = 1, EnchantmentId = 2, Value = 3 }));
        Check("ChangeGlobalMagicTokenEvent", new ChangeGlobalMagicTokenEvent(null!) { Mode = (ChangeGlobalMagicTokenEventMode)255, PlayerIndex = 255, EnchantmentId = int.MaxValue, Value = int.MinValue });
        Each<GlobalEnchantmentType>(value => Check("GlobalEnchantment", new GlobalEnchantment(null!) { EnchantmentType = value }));
        Each<ItemType>(value => Check("ItemGraphics", new ItemGraphics(null!) { ItemType = value }));
        Each<MessageEventLogLayout>(value => Check("MessageEventLog", new MessageEventLog(null!) { Layout = value }));
        Each<MessageEventLogButtons>(value => Check("MessageEventLog", new MessageEventLog(null!) { Buttons = value }));
        Each<CityEnchantmentId>(value =>
        {
            var control = new GlobalMagicControl(null!);
            var city = new CityEnchantment(null!) { CityEnchantmentId = value };
            city.EnchantmentLocation.Packed = 9;
            city.SetOwner(control.Enchantments);
            Check("GlobalMagicControl", control, converterRead: false);
        });
    }

    [Fact]
    public void SetsCoverEmptyFullAndUnnamedBits()
    {
        Check("ControlStyleObject", new ControlStyleObject(null!) { _controlStyle = Set<ControlStyleElement>.FromMask(0xFF) });
        Check("SingleTargetCombatAction", new SingleTargetCombatAction(null!) { Mode = Set<SingleTargetModeElement>.FromMask(0), SourceId = 0, TargetId = 0 });
        Check("SingleTargetCombatAction", new SingleTargetCombatAction(null!) { Mode = Set<SingleTargetModeElement>.Of(SingleTargetModeElement.Defensive), SourceId = 255, TargetId = 1 });
        Check("SingleTargetCombatAction", new SingleTargetCombatAction(null!) { Mode = Set<SingleTargetModeElement>.FromMask(0xFF), SourceId = 4, TargetId = 5 });
    }

    [Fact]
    public void FixedArraysCoverZeroMixedAndBoundaryValues()
    {
        var zero = new UnitSpellCombatAction(null!);
        Check("UnitSpellCombatAction", zero);
        var mixed = new UnitSpellCombatAction(null!) { SpellId = 8, SourceId = 1, TargetId = 2, Mana = -4 };
        mixed.SpellData[0] = 7;
        mixed.SpellData[1] = -1;
        Check("UnitSpellCombatAction", mixed);
        var bounds = new UnitSpellCombatAction(null!) { SpellId = int.MinValue, Mana = int.MaxValue };
        bounds.SpellData[0] = int.MinValue;
        bounds.SpellData[1] = int.MaxValue;
        Check("UnitSpellCombatAction", bounds);
        var control = new GlobalMagicControl(null!);
        Check("GlobalMagicControl", control);
        var spheres = new GlobalMagicControl(null!) { NextEnchantmentId = int.MaxValue };
        for (int i = 0; i < 9; i++) spheres.DoubleSphereManaCount[i] = i == 8 ? int.MinValue : i;
        var hatred = new HatredEnchantment(null!) { DaysActive = 12 };
        hatred.SetOwner(spheres.Enchantments);
        var harmony = new HarmonyEnchantment(null!) { DaysActive = int.MinValue };
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
        Check("TutorialControl", new TutorialControl(null!));
    }

    [Fact]
    public void ByteRunListsMatchTheConverter()
    {
        Check("CommandAbilityData", Commanding(defaultValue: 0));
        Check("CommandAbilityData", Commanding(defaultValue: 0, [.. Enumerable.Repeat((byte)0, 255)]));
        Check("CommandAbilityData", Commanding(defaultValue: 0, [.. Enumerable.Repeat((byte)0, 256)]));
        Check("CommandAbilityData", Commanding(defaultValue: 0, [.. Enumerable.Repeat((byte)0, 300), 1, .. Enumerable.Repeat((byte)0, 600)]));
        Check("CommandAbilityData", Commanding(defaultValue: 255, [255, 254, 255, 255, 0]));
        Check("CommandAbilityData", Commanding(defaultValue: 7, [.. Enumerable.Repeat((byte)7, 511)]));
        // The relation list resets its default to Unknown after a load on both paths, so the
        // loaded image differs from the converter's encode of the written document.
        WithDestroyingMap(_ =>
        {
            var relations = new PlayerDiplomaticRelations(null!);
            relations.SetDiplomaticRelation(3, DiplomaticRelation.War);
            relations.SetDiplomaticRelation(1, DiplomaticRelation.Alliance);
            Check("PlayerDiplomaticRelations", relations, converterRead: false);
        });
    }

    [Fact]
    public void AByteRunOver255SplitsAsTheWriterSplitsIt()
    {
        using var source = Commanding(defaultValue: 0, [.. Enumerable.Repeat((byte)0, 300)]);
        var classValue = s_schema.Get("CommandAbilityData");
        var image = OriginalBackend.Write(TreeWriter.Write(source, Factory), classValue, s_schema, "game");
        Assert.Equal(Image(source), image);
        var stored = (JsonObject)PilotImage.Decode(image, "CommandAbilityData")["CommandedUnits"]!;
        Assert.Equal(300, ((JsonArray)stored["items"]!).Items.Count);
        // The default, the count 300, then a run of 255 and a run of 45.
        byte[] runs = [0, 44, 1, 0, 0, 0, 255, 0, 45];
        Assert.True(image.AsSpan().IndexOf(runs) >= 0);
    }

    [Fact]
    public void AnAbsentRunListLeavesTheListAsItWas()
    {
        var classValue = s_schema.Get("ItemExplorationSiteEventLog");
        var image = PilotImage.Encode(new JsonObject(), "ItemExplorationSiteEventLog");
        using var viaReadWrite = ExplorationLog(defaultValue: 5, 5, 6, 5);
        Load(viaReadWrite, image);
        using var viaImage = ExplorationLog(defaultValue: 5, 5, 6, 5);
        TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), viaImage, Factory);
        using var viaDocument = ExplorationLog(defaultValue: 5, 5, 6, 5);
        TreeReader.Read(DocumentBackend.Read(new JsonObject(), classValue, s_schema, "game"), viaDocument, Factory);
        Assert.Equal(5, viaReadWrite.ItemIds.Default);
        Assert.Equal(3, viaReadWrite.ItemIds.Count);
        EqualBytes(Image(viaReadWrite), Image(viaImage), "absent list original read");
        EqualBytes(Image(viaReadWrite), Image(viaDocument), "absent list document read");
    }

    [Fact]
    public void AUnitIndexListKeepsItsStoredDefault()
    {
        var classValue = s_schema.Get("ItemExplorationSiteEventLog");
        var source = new UnitIndexProbe();
        source.Units.Add(4);
        source.Units.Add(-1);
        source.Units.Add(-1);
        using var expected = ExplorationLog(defaultValue: -1, 4, -1, -1);
        var image = OriginalBackend.Write(TreeWriter.Write(source, Factory), classValue, s_schema, "game");
        EqualBytes(Image(expected), image, "unit index list original");
        var loaded = new UnitIndexProbe();
        loaded.Units.Default = 0;
        TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), loaded, Factory);
        Assert.Equal(-1, loaded.Units.Default);
        Assert.Equal([4, -1, -1], loaded.Units.List.AsSpan(0, loaded.Units.Count).ToArray());
        var other = new UnitIndexProbe();
        other.Units.Default = 3;
        other.Units.Add(3);
        var otherImage = OriginalBackend.Write(TreeWriter.Write(other, Factory), classValue, s_schema, "game");
        var reloaded = new UnitIndexProbe();
        TreeReader.Read(OriginalBackend.Read(otherImage, classValue, s_schema, "game"), reloaded, Factory);
        Assert.Equal(3, reloaded.Units.Default);
        var untouched = new UnitIndexProbe();
        untouched.Units.Add(8);
        TreeReader.Read(new ObjectNode(null, null), untouched, Factory);
        Assert.Equal(-1, untouched.Units.Default);
        Assert.Equal(1, untouched.Units.Count);
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
        using var source = AbilityItem(withData: true);
        var document = PilotImage.Decode(Image(source), "Item");
        // AbilityDataIds names slot 33 and an absent slot 99, and leaves the stored slot 47 unread.
        var ids = (JsonArray)((JsonObject)document["AbilityDataIds"]!)["items"]!;
        ids.Items.Clear();
        ids.Items.Add(new JsonInteger(33));
        ids.Items.Add(new JsonInteger(99));
        SameLoad("Item", document);
        var noBits = PilotImage.Decode(Image(source), "Item");
        // ReadWrite tests the ability bits (property 3), not the count.
        Assert.True(noBits.Remove("AbilityBits"));
        SameLoad("Item", noBits);
    }

    [Fact]
    public void ItemControlMatchesTheConverter()
    {
        Check("ItemControl", new ItemControl(null!));
        var control = new ItemControl(null!);
        control.RegisterItem(PlainItem(itemId: 5, name: "Sword"));
        control.RegisterItem(PlainItem(itemId: 9, name: "Shield"));
        control.RegisterItem(AbilityItem(withData: true));
        Check("ItemControl", control);
    }

    [Fact]
    public void ItemControlLoadsStopAtTheFirstMissingItem()
    {
        using var control = new ItemControl(null!);
        control.RegisterItem(PlainItem(itemId: 5, name: "Sword"));
        control.RegisterItem(PlainItem(itemId: 9, name: "Shield"));
        control.RegisterItem(PlainItem(itemId: 11, name: "Ring"));
        var document = PilotImage.Decode(Image(control), "ItemControl");
        var items = (JsonArray)document["Items"]!;
        // Slots 0 and 2 stay. Both loads stop at the gap at slot 1.
        items.Items.RemoveAt(1);
        SameLoad("ItemControl", document);
        var nil = PilotImage.Decode(Image(control), "ItemControl");
        var first = (JsonArray)nil["Items"]!;
        first.Items[0] = new JsonObject().Add("$class", new JsonNull()).Add("$slot", new JsonInteger(0));
        SameLoad("ItemControl", nil);
    }

    private static ItemExplorationSiteEventLog ExplorationLog(int defaultValue, params int[] items)
    {
        var log = new ItemExplorationSiteEventLog(null!);
        log.ItemIds.Default = defaultValue;
        foreach (int item in items) log.ItemIds.Add(item);
        return log;
    }

    private static CommandAbilityData Commanding(byte defaultValue, params byte[] items)
    {
        var data = new CommandAbilityData(null!) { AbilityId = 40 };
        data.CommandedUnits.Default = defaultValue;
        foreach (byte item in items) data.CommandedUnits.Add(item);
        return data;
    }

    private static Item PlainItem(int itemId, string name) => new(null!) { ItemId = itemId, Name = name, Attack = -2, LibraryId = 3 };

    // AddAbilityData puts each new item first, so the chain holds the reverse of this order.
    private static Item AbilityItem(bool withData)
    {
        var item = new Item(null!) { ItemId = 12, Name = "Staff" };
        item.SetAbilityCount(170);
        if (!withData) return item;
        item.AddAbilityData(new MultiLevelAbilityData(null!) { Level = 3, LevelAbilityId = 12 });
        item.AddAbilityData(Commanding(defaultValue: 0, 1, 0, 0, 2));
        item.AddAbilityData(new DurationAbilityData(null!) { AbilityId = 7, Duration = 2, PlayerIndex = 1, Source = -1 });
        item.AddAbilityData(new UnitEnchantmentAbilityData(null!) { EnchantmentAbilityId = 9, Enchantment = new UnitEnchantment(null!) { UnitId = 4 } });
        item.AddAbilityData(new LeadershipAbilityData(null!) { LevelAbilityId = 20, Level = 1, ExternalLevel = 1, ExternalSource = "banner" });
        item.AddAbilityData(new HealingAbilityData(null!) { Enabled = true });
        item.AddAbilityData(new DispelMagicAbilityData(null!) { Enabled = true, Level = 2 });
        item.AddAbilityData(new PossessedAbilityData(null!) { SourceExperience = 300 });
        item.AddAbilityData(new CommandedAbilityData(null!) { AbilityId = 33, CommandAbilityId = 40, SourceId = 2, OriginalCombatPlayer = 1 });
        return item;
    }

    // The engine load, the original backend load and the document load of one document write the same image.
    private static void SameLoad(string className, JsonObject document)
    {
        var classValue = s_schema.Get(className);
        var image = PilotImage.Encode(document, className);
        var viaReadWrite = Blank(className);
        var viaImage = Blank(className);
        var viaDocument = Blank(className);
        try
        {
            Load(viaReadWrite, image);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), viaImage, Factory);
            TreeReader.Read(DocumentBackend.Read(document, classValue, s_schema, "game"), viaDocument, Factory);
            var expected = Image(viaReadWrite);
            EqualBytes(expected, Image(viaImage), className + " original load");
            EqualBytes(expected, Image(viaDocument), className + " document load");
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
        var previous = StrategicMap.Current;
        var map = new StrategicMap(null!);
        map.Status = Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Destroying);
        StrategicMap.Current = map;
        try
        {
            Check("Army", new Army(null!));
            Each<GroupBehavior>(value =>
            {
                var army = new Army(null!) { OldAIBehavior = value, Player = -1 };
                army.Mode = Set<ArmyModeElement>.Of(ArmyModeElement.FixedBehavior);
                army.UserMode = Set<ArmyUserModeElement>.FromMask(0);
                Check("Army", army);
            });
            var full = new Army(null!);
            full.Mode = Set<ArmyModeElement>.FromMask(0xFF) - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
            full.UserMode = Set<ArmyUserModeElement>.FromMask(0xFF);
            Check("Army", full);
            var view = new ArmyView(null!);
            Check("ArmyView", view);
            var widths = new ArmyView(null!) { ConcealedForPlayers = ushort.MaxValue };
            for (int i = 0; i < 8; i++)
            {
                widths.UnitConcealment[i] = (ushort)(i == 0 ? 0 : ushort.MaxValue);
                widths.MoraleValues[i] = i % 2 == 0 ? int.MinValue : int.MaxValue;
            }
            Check("ArmyView", widths);
        }
        finally
        {
            StrategicMap.Current = previous!;
            map.Dispose();
        }
    }

    [Fact]
    public void AnOverlongSpellDataListIsRefusedOnExport()
    {
        using var spell = new UnitSpellCombatAction(null!);
        var classValue = s_schema.Get("UnitSpellCombatAction");
        var tree = TreeWriter.Write(spell, Factory);
        var list = (ListNode)tree.Find("SpellData")!;
        list.Items.Add(new IntegerNode(4, true, 1));
        var document = DocumentBackend.Write(tree, classValue, s_schema, "game");
        Assert.Equal(3, ((JsonArray)document["SpellData"]!).Items.Count);
        var error = Assert.Throws<ConversionException>(() => OriginalBackend.Write(tree, classValue, s_schema, "game"));
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
            var army = new Army(null!);
            army.DestinationX = 3;
            army.DestinationY = 4;
            army.DestinationMapLevel = 1;
            army.UserMode = Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
            var classValue = s_schema.Get("Army");
            var document = DocumentBackend.Write(TreeWriter.Write(army, Factory), classValue, s_schema, "game");
            Assert.Null(document.Find("DestinationX"));
            Assert.Null(document.Find("DestinationY"));
            Assert.Null(document.Find("DestinationMapLevel"));
            Assert.Null(document.Find("UserMode"));
            Check("Army", army);

            map.ConnectionType = PlayerConnectionType.Local;
            var source = new Army(null!) { DestinationX = 3, DestinationY = 4, DestinationMapLevel = 1 };
            source.UserMode = Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
            var image = Image(source);
            source.Dispose();
            map.ConnectionType = PlayerConnectionType.Network;
            var viaPersist = (Army)Factory.Create("Army");
            var viaReadWrite = (Army)Factory.Create("Army");
            try
            {
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), viaPersist, Factory);
                Load(viaReadWrite, image);
                Assert.Equal(3, viaPersist.DestinationX);
                Assert.Equal(4, viaPersist.DestinationY);
                Assert.Equal(1, viaPersist.DestinationMapLevel);
                Assert.True(viaPersist.UserMode.Contains(ArmyUserModeElement.Guard));
                Assert.Equal(3, viaReadWrite.DestinationX);
                Assert.Equal(4, viaReadWrite.DestinationY);
                Assert.Equal(1, viaReadWrite.DestinationMapLevel);
                Assert.True(viaReadWrite.UserMode.Contains(ArmyUserModeElement.Guard));
                EqualBytes(Image(viaPersist), Image(viaReadWrite), "Army network load");
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
            var source = new Army(null!);
            source.Mode = Set<ArmyModeElement>.Of(ArmyModeElement.Moving, ArmyModeElement.FixedBehavior);
            var image = Image(source);
            source.Dispose();
            var classValue = s_schema.Get("Army");
            var viaPersist = (Army)Factory.Create("Army");
            var viaReadWrite = (Army)Factory.Create("Army");
            try
            {
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), viaPersist, Factory);
                Load(viaReadWrite, image);
                Assert.False(viaPersist.Mode.Contains(ArmyModeElement.Moving));
                Assert.False(viaReadWrite.Mode.Contains(ArmyModeElement.Moving));
                Assert.True(viaPersist.Mode.Contains(ArmyModeElement.FixedBehavior));
                Assert.True(viaReadWrite.Mode.Contains(ArmyModeElement.FixedBehavior));
                EqualBytes(Image(viaPersist), Image(viaReadWrite), "Army moving");
            }
            finally
            {
                viaReadWrite.Dispose();
                viaPersist.Dispose();
            }
        });
    }

    [Fact]
    public void AnArmyWithAUnitRefusesThroughTUnit()
    {
        WithDestroyingMap(_ =>
        {
            using var army = new Army(null!);
            new Unit(null!).SetOwner(army);
            var error = Assert.Throws<InvalidOperationException>(() => TreeWriter.Write(army, Factory));
            Assert.Equal("Class Unit overrides ReadWrite and has no PersistFields.", error.Message);
        });
    }

    [Fact]
    public void AnimationMatchesTheConverter()
    {
        Check("Animation", new Animation(null!));
        var edge = new Animation(null!)
        {
            Dimensions = new Rect { Left = int.MinValue, Top = -1, Right = int.MaxValue, Bottom = 4 },
            _hexX = -1,
            _hexY = sbyte.MinValue,
            _hexLevel = sbyte.MaxValue,
            FrameCounter = int.MinValue,
        };
        Check("Animation", edge);
    }

    [Fact]
    public void DamageCombatActionMatchesTheConverter()
    {
        Check("DamageCombatAction", new DamageCombatAction(null!));
        var edge = new DamageCombatAction(null!)
        {
            Damage = 255,
            SourceId = 255,
            TargetId = 1,
            Effects = new PackedDamageTypes { Value = Set<DamageType>.FromMask((1UL << 9) | (1UL << 15)) },
            DamageTypes = new PackedDamageTypes { Value = Set<DamageType>.Of(DamageType.Fire, DamageType.None) },
        };
        Check("DamageCombatAction", edge);
    }

    [Fact]
    public void RemotePlayerMatchesTheConverter()
    {
        Check("RemotePlayer", new RemotePlayer(null!));
        var zero = new RemotePlayer(null!) { RemoteId = 0, PreferredRace = 0, MapValidation = MapValidationInfo.Unknown, Version = 0 };
        Check("RemotePlayer", zero);
        Each<MapValidationInfo>(value => Check("RemotePlayer", new RemotePlayer(null!)
        {
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
        Check("AIGroup", new AIGroup(null!));
        var zero = new AIGroup(null!)
        {
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
            var group = new AIGroup(null!)
            {
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
            };
            group.TargetMapCoordinate.Packed = int.MinValue;
            Check("AIGroup", group);
        });
    }

    [Fact]
    public void SetupSettingsMatchesTheConverter()
    {
        Check("SetupSettings", new SetupSettings(null!));
        var zero = new SetupSettings(null!)
        {
            PlayMode = 0,
            VictoryCondition = 0,
            IndependentAIMode = 0,
            ConnectionType = 0,
            TacticalBattleAvailability = 0,
            ExplorationEnabled = false,
            LeadersOnMap = false,
        };
        Check("SetupSettings", zero);
        var edge = new SetupSettings(null!)
        {
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
        Check("CustomAbilityList", new CustomAbilityList(null!));
        var emptyByte = new CustomAbilityList(null!);
        emptyByte.SetAbilityCount(8);
        Check("CustomAbilityList", emptyByte);
        var residue = new CustomAbilityList(null!);
        residue.SetAbilityCount(1);
        residue.AbilityBits[0] = 0x80;
        Check("CustomAbilityList", residue);
    }

    [Fact]
    public void RegisteredAbilityBitsSurviveTheCleanup()
    {
        var placed = new List<Ability>();
        try
        {
            placed.Add(PlaceAbility(0));
            placed.Add(PlaceAbility(1));
            placed.Add(PlaceAbility(3));
            var list = new CustomAbilityList(null!);
            list.SetAbilityCount(8);
            list.SetAbilitySet(0, 1);
            list.SetAbilitySet(1, 1);
            list.SetAbilitySet(3, 1);
            Check("CustomAbilityList", list);
        }
        finally
        {
            foreach (var ability in placed) ReleaseAbility(ability);
        }
    }

    [Fact]
    public void AMissingAbilityBitIsClearedAndResidueStays()
    {
        // Image writes the bits and then the cleanup clears the missing bit in memory. The tree
        // write runs after that bit is set again, so both stored images still hold it. Both loads clear it.
        Ability? placed = null;
        var list = new CustomAbilityList(null!);
        try
        {
            placed = PlaceAbility(0);
            list.SetAbilityCount(3);
            list.SetAbilitySet(0, 1);
            list.SetAbilitySet(1, 1);
            list.AbilityBits[0] |= 0xF0;
            var image = Image(list);
            list.SetAbilitySet(1, 1);
            var classValue = s_schema.Get("CustomAbilityList");
            var tree = TreeWriter.Write(list, Factory);
            var expected = PilotImage.Decode(image, "CustomAbilityList");
            EqualJson(expected, DocumentBackend.Write(tree, classValue, s_schema, "game"), "missing bit write");
            EqualBytes(image, OriginalBackend.Write(tree, classValue, s_schema, "game"), "missing bit original");
            EqualBytes(PilotImage.Encode(expected, "CustomAbilityList"), image, "missing bit converter");
            var viaReadWrite = (CustomAbilityList)Blank("CustomAbilityList");
            var viaTree = (CustomAbilityList)Blank("CustomAbilityList");
            var fromDocument = (CustomAbilityList)Blank("CustomAbilityList");
            try
            {
                Load(viaReadWrite, image);
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), viaTree, Factory);
                TreeReader.Read(DocumentBackend.Read(expected, classValue, s_schema, "game"), fromDocument, Factory);
                Assert.True(viaReadWrite.GetAbilitySetAt(0));
                Assert.False(viaReadWrite.GetAbilitySetAt(1));
                Assert.Equal(0xF0, viaReadWrite.AbilityBits[0] & 0xF0);
                Assert.True(viaTree.GetAbilitySetAt(0));
                Assert.False(viaTree.GetAbilitySetAt(1));
                Assert.Equal(0xF0, viaTree.AbilityBits[0] & 0xF0);
                EqualBytes(Image(viaReadWrite), Image(viaTree), "missing bit loads");
                EqualBytes(Image(viaReadWrite), Image(fromDocument), "missing bit document");
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
            if (placed is not null) ReleaseAbility(placed);
        }
    }

    [Fact]
    public void AChildWhosePersistThrowsIsDroppedUnlessTheHandlerStops()
    {
        var descriptor = EngineClass.Of<HatredEnchantment>();
        bool added = !EngineFixture.Engine.ClassTable.ContainsKey(descriptor.ClassId);
        if (added) EngineFixture.Engine.RegisterEngineClasses([descriptor]);
        var control = new GlobalMagicControl(null!);
        Func<ECustomNode, EngineObject>? previous = null;
        try
        {
            var hatred = new HatredEnchantment(null!) { DaysActive = 12 };
            hatred.SetOwner(control.Enchantments);
            var image = Image(control);
            previous = EngineClassCreation.Substitute(descriptor, next: static owner => new ThrowingHatred(owner));
            var classValue = s_schema.Get("GlobalMagicControl");
            var continued = (GlobalMagicControl)Blank("GlobalMagicControl");
            var continuedTree = (GlobalMagicControl)Blank("GlobalMagicControl");
            try
            {
                Load(continued, image);
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), continuedTree, Factory);
                Assert.Equal(0, continued.Enchantments.GetCount());
                Assert.Equal(0, continuedTree.Enchantments.GetCount());
                EqualBytes(Image(continued), Image(continuedTree), "dropped child");
            }
            finally
            {
                continuedTree.Dispose();
                continued.Dispose();
            }
            var stopped = (GlobalMagicControl)Blank("GlobalMagicControl");
            var stoppedTree = (GlobalMagicControl)Blank("GlobalMagicControl");
            try
            {
                var streamError = Assert.Throws<InvalidOperationException>(() =>
                    Load(stopped, image, static (StorageStream _, string _, ref bool cont) => cont = false));
                var treeError = Assert.Throws<InvalidOperationException>(() =>
                    TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), stoppedTree, Factory,
                        static (string _, out bool cont) => cont = false));
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
            if (previous is not null) EngineClassCreation.Substitute(descriptor, next: previous);
            control.Dispose();
            if (added) EngineFixture.Engine.UnregisterEngineClasses([descriptor]);
        }
    }

    [Fact]
    public void AFixedObjectWhosePersistThrowsStaysUnlessTheHandlerStops()
    {
        using var source = new GlobalMagicControl(null!);
        source.NextEnchantmentId = 9;
        source.DoubleSphereManaCount[0] = 3;
        var image = Image(source);
        var classValue = s_schema.Get("GlobalMagicControl");
        var continued = Faulted(4);
        var continuedTree = Faulted(4);
        var continuedList = continued.Enchantments;
        var continuedTreeList = continuedTree.Enchantments;
        try
        {
            Load(continued, image);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), continuedTree, Factory);
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
        var stopped = Faulted(4);
        var stoppedTree = Faulted(4);
        var stoppedList = stopped.Enchantments;
        var stoppedTreeList = stoppedTree.Enchantments;
        try
        {
            var streamError = Assert.Throws<InvalidOperationException>(() =>
                Load(stopped, image, static (StorageStream _, string _, ref bool cont) => cont = false));
            var treeError = Assert.Throws<InvalidOperationException>(() =>
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), stoppedTree, Factory,
                    static (string _, out bool cont) => cont = false));
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
        var control = new GlobalMagicControl(null!);
        control.Enchantments?.Dispose();
        control.Enchantments = new ThrowingEnchantments(null!);
        control.NextEnchantmentId = sentinel;
        return control;
    }

    [Fact]
    public void SetupPlayerBehaviorTransfersTheReservedBytes()
    {
        var probe = new BehaviorProbe
        {
            Behavior = new SetupPlayerBehavior
            {
                PlayerType = 2,
                Flags = 9,
                Reserved = Array3<byte>.From([1, 0, 2]),
            },
        };
        var classValue = s_schema.Get("PlayerSetupSettings");
        var tree = TreeWriter.Write(probe, Factory);
        var stored = (JsonObject)DocumentBackend.Write(tree, classValue, s_schema, "game")["Behavior"]!;
        Assert.Equal(2, ((JsonInteger)stored["PlayerType"]!).Value);
        Assert.Equal("010002", ((JsonString)stored["$reserved"]!).Value);
        Assert.Equal(9, ((JsonInteger)stored["Flags"]!).Value);
        var back = OriginalBackend.Read(OriginalBackend.Write(tree, classValue, s_schema, "game"), classValue, s_schema, "game");
        var loaded = new BehaviorProbe();
        TreeReader.Read(back, loaded, Factory);
        Assert.Equal(2, loaded.Behavior.PlayerType);
        Assert.Equal(9, loaded.Behavior.Flags);
        Assert.Equal(1, loaded.Behavior.Reserved[0]);
        Assert.Equal(0, loaded.Behavior.Reserved[1]);
        Assert.Equal(2, loaded.Behavior.Reserved[2]);
    }

    [Fact]
    public void AChampionByteOtherThanZeroOrOneRoundTripsThroughReadWrite()
    {
        using var source = new UnitResource(null!);
        Unsafe.As<bool, byte>(ref source.Champion) = 2;
        var image = Image(source);
        using var loaded = new UnitResource(null!);
        Load(loaded, image);
        Assert.Equal(2, Unsafe.As<bool, byte>(ref loaded.Champion));
        EqualBytes(image, Image(loaded), "champion 02");
    }

    [Fact]
    public void CsSaveLoadIsTheStoredNameOfTheAlias()
    {
        var info = EnumTypeInfo.Describe(typeof(ControlStyleElement));
        Assert.Equal("SaveLoad", info.ByValue[0]);
        Assert.False(info.ByName.ContainsKey("csStored"));
        Assert.Equal(0, info.ByName["SaveLoad"]);
    }

    private static void WithDestroyingMap(Action<StrategicMap> body)
    {
        var previous = StrategicMap.Current;
        var map = new StrategicMap(null!);
        map.Status = Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Destroying);
        StrategicMap.Current = map;
        try
        {
            body(map);
        }
        finally
        {
            StrategicMap.Current = previous!;
            map.Dispose();
        }
    }

    [Fact]
    public void EngineNodesMatchTheConverter()
    {
        Check("ECustomOwner", new ECustomOwner(null!));
        var owner = new ECustomOwner(null!);
        owner.SetControlStyle(Set<ControlStyleElement>.FromMask(0xA5));
        Check("ECustomOwner", owner);
        var resources = new EResourceList(null!);
        resources.SetControlStyle(Set<ControlStyleElement>.FromMask(0));
        Check("EResourceList", resources);
        Check("EControlStyleNode", new EControlStyleNode(null!));
        var styled = new EControlStyleNode(null!);
        styled.SetControlStyle(Set<ControlStyleElement>.FromMask(0x81));
        new AISavingsBudget(null!) { Priority = 7, Gold = -3 }.SetOwner(styled);
        new AITransporterBudget(null!).SetOwner(styled);
        Check("EControlStyleNode", styled);
        var plugins = new PluginControl(null!);
        plugins.UsedPlugins.Add("Abilities");
        Check("PluginControl", plugins);
        Check("PluginControl", new PluginControl(null!));
    }

    [Fact]
    public void ASortNodeRefusesBothWritesAlike()
    {
        foreach (var node in new ESortNode[] { new(null!), new EInverseSortNode(null!), new EControlStyleInverseSortNode(null!) })
        {
            try
            {
                var stream = Assert.Throws<NotSupportedException>(() => Image(node));
                var tree = Assert.Throws<NotSupportedException>(() => TreeWriter.Write(node, Factory));
                Assert.Equal(stream.Message, tree.Message);
            }
            finally
            {
                node.Dispose();
            }
        }
    }

    // A reference list stores the children with no owner or with the list as owner. The stream
    // write makes the list their owner while it writes and clears those owners after it, so the
    // child the list owned before the write ends with no owner. The tree write leaves every owner.
    [Fact]
    public void AReferenceListStoresItsUnownedChildrenAndTakesOneReferenceEach()
    {
        var list = new CombatActionList(null!);
        var other = new CombatActionList(null!);
        try
        {
            var free = new DamageCombatAction(null!) { Damage = 9, SourceId = 3, TargetId = 4 };
            list.AddChild(free);
            var owned = new DamageCombatAction(null!) { Damage = 1 };
            list.AddChild(owned);
            owned.SetOwnerWithoutNotify(list);
            var elsewhere = new DamageCombatAction(null!) { Damage = 2 };
            list.AddChild(elsewhere);
            elsewhere.SetOwnerWithoutNotify(other);
            var classValue = s_schema.Get("CombatActionList");
            var tree = TreeWriter.Write(list, Factory);
            Assert.Null(free.Owner);
            Assert.Same(list, owned.Owner);
            Assert.Same(other, elsewhere.Owner);
            var image = Image(list);
            Assert.Null(owned.Owner);
            Assert.Same(other, elsewhere.Owner);
            var expected = PilotImage.Decode(image, "CombatActionList");
            Assert.Equal(2, ((JsonArray)expected["Children"]!).Items.Count);
            EqualJson(expected, DocumentBackend.Write(tree, classValue, s_schema, "game"), "reference list write");
            EqualBytes(image, OriginalBackend.Write(tree, classValue, s_schema, "game"), "reference list original");
            var viaReadWrite = (CombatActionList)Blank("CombatActionList");
            var fromDocument = (CombatActionList)Blank("CombatActionList");
            var fromImage = (CombatActionList)Blank("CombatActionList");
            try
            {
                Load(viaReadWrite, image);
                TreeReader.Read(DocumentBackend.Read(expected, classValue, s_schema, "game"), fromDocument, Factory);
                TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), fromImage, Factory);
                foreach (var loaded in new[] { viaReadWrite, fromDocument, fromImage })
                {
                    Assert.Equal(2, loaded.Count);
                    for (int index = 0; index < loaded.Count; index++)
                    {
                        Assert.Null(loaded[index].Owner);
                        Assert.Equal(1, loaded[index].RefCount);
                    }
                }
                EqualBytes(Image(viaReadWrite), Image(fromDocument), "reference list document read");
                EqualBytes(Image(viaReadWrite), Image(fromImage), "reference list original read");
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
        var list = new CustomAbilityList(null!);
        list.SetAbilityCount(8);
        list.SetAbilitySet(5, 1);
        var classValue = s_schema.Get("CustomAbilityList");
        var image = Image(list);
        var preview = (CustomAbilityList)Blank("CustomAbilityList");
        var previewTree = (CustomAbilityList)Blank("CustomAbilityList");
        var full = (CustomAbilityList)Blank("CustomAbilityList");
        try
        {
            using (var memory = new MemoryStream(image))
            using (var reader = new StorageReader(memory, EngineFixture.Engine, true))
            {
                preview.MainReadWrite(reader);
            }
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), previewTree, Factory, preview: true);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), full, Factory);
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
        using var control = new ItemControl(null!);
        control.RegisterItem(PlainItem(itemId: 4, name: "Ring"));
        var tree = TreeWriter.Write(control, Factory);
        Assert.NotNull(tree.Find("Items"));
        Assert.Null(TreeWriter.Write(control, Factory, preview: true).Find("Items"));
        using var loaded = (ItemControl)Blank("ItemControl");
        TreeReader.Read(tree, loaded, Factory, preview: true);
        Assert.Equal(0, loaded.GetCount());
    }

    private static void Each<TEnum>(Action<TEnum> check) where TEnum : struct, Enum
    {
        foreach (var value in Enum.GetValues<TEnum>()) check(value);
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
            var classValue = s_schema.Get(className);
            var image = Image(objectValue);
            var expected = PilotImage.Decode(image, className);
            var tree = TreeWriter.Write(objectValue, Factory);
            var actual = DocumentBackend.Write(tree, classValue, s_schema, "game");
            EqualJson(expected, actual, className + " write");
            EqualBytes(image, OriginalBackend.Write(tree, classValue, s_schema, "game"), className + " original");
            viaReadWrite = Blank(className);
            Load(viaReadWrite, image);
            var read = Image(viaReadWrite);
            loaded = Blank(className);
            TreeReader.Read(DocumentBackend.Read(expected, classValue, s_schema, "game"), loaded, Factory);
            if (converterRead)
                EqualBytes(PilotImage.Encode(expected, className), Image(loaded), className + " converter read");
            EqualBytes(read, Image(loaded), className + " read");
            fromImage = Blank(className);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), fromImage, Factory);
            EqualBytes(read, Image(fromImage), className + " original read");
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
        foreach (var engineClass in EngineClass.All)
            if (engineClass.StorageKey == className) return engineClass.CreateInstance(null!);
        throw new InvalidOperationException("The engine has no class " + className + ".");
    }

    private static void Load(EngineObject objectValue, byte[] image, StorageStream.StorageStreamError? onError = null)
    {
        using var memory = new MemoryStream();
        memory.Write(image);
        memory.Position = 0;
        var reader = new StorageReader(memory, EngineFixture.Engine, false);
        if (onError is not null) reader.OnError += onError;
        try
        {
            objectValue.MainReadWrite(reader);
        }
        finally
        {
            reader.Dispose();
        }
    }

    private static Ability PlaceAbility(int id)
    {
        var ability = new Ability(null!);
        ability.AbilityId = id;
        var list = GamePack.Current.AbilityControl.Abilities;
        while (id >= list.Count) list.Add(null);
        list[id] = ability;
        return ability;
    }

    private static void ReleaseAbility(Ability ability)
    {
        var list = GamePack.Current.AbilityControl.Abilities;
        int id = ability.AbilityId;
        if ((uint)id < (uint)list.Count && ReferenceEquals(list[id], ability)) list[id] = null;
        ability.Dispose();
    }

    private sealed class ThrowingHatred : HatredEnchantment
    {
        public ThrowingHatred(ECustomNode owner) : base(owner)
        {
        }

        public override void ReadWrite(StorageStream stream) => throw new InvalidOperationException("hatred failed");

        public override void PersistFields(IPersister persister) => throw new InvalidOperationException("hatred failed");
    }

    private sealed class ThrowingEnchantments : GlobalEnchantmentList
    {
        public ThrowingEnchantments(ECustomNode owner) : base(owner)
        {
        }

        public override void ReadWrite(StorageStream stream) => throw new InvalidOperationException("enchantments failed");

        public override void PersistFields(IPersister persister) => throw new InvalidOperationException("enchantments failed");
    }

    private sealed class BehaviorProbe : IPersistable
    {
        public SetupPlayerBehavior Behavior { get; set; }

        public void Persist(IPersister persister) => Behavior = PersistSupport.Record(persister, "Behavior", Behavior);
    }

    // A unit index list stored under the integer list of ItemExplorationSiteEventLog.
    private sealed class UnitIndexProbe : IPersistable
    {
        public UnitIndexList Units { get; } = new();

        public void Persist(IPersister persister) => PersistSupport.UnitIndexList(persister, "ItemIds", Units);
    }

    private static byte[] Image(EngineObject objectValue)
    {
        using var memory = new MemoryStream();
        using (var writer = new StorageWriter(memory, EngineFixture.Engine, false))
        {
            objectValue.MainReadWrite(writer);
        }
        return memory.ToArray();
    }

    private static void EqualJson(JsonObject expected, JsonObject actual, string label)
    {
        var left = JsonWriter.ToUtf8(expected);
        var right = JsonWriter.ToUtf8(actual);
        if (left.AsSpan().SequenceEqual(right)) return;
        Assert.Fail(label + "\n" + Encoding.UTF8.GetString(right));
    }

    private static void EqualBytes(byte[] expected, byte[] actual, string label)
    {
        if (expected.AsSpan().SequenceEqual(actual)) return;
        Assert.Fail(label + " at " + expected.AsSpan().CommonPrefixLength(actual));
    }
}
