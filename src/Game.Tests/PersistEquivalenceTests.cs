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

    [Fact]
    public void UnitEnchantmentMatchesTheConverter()
    {
        Check("UnitEnchantment", new UnitEnchantment(null!));
        Check("UnitEnchantment", new UnitEnchantment(null!) { EnchantmentAbilityId = int.MinValue, UnitId = int.MaxValue, Source = 0, DispelResistance = 0, Upkeep = 0 });
        Check("CosmeticSurgeryEnchantment", new CosmeticSurgeryEnchantment(null!) { FaceIndex = int.MaxValue });
        Check("CosmeticSurgeryEnchantment", new CosmeticSurgeryEnchantment(null!) { FaceIndex = int.MinValue, EnchantmentId = 0, Upkeep = 0, Source = 0, DispelResistance = 0 });
        RefuseAbilityData(new UnitEnchantmentAbilityData(null!));
        var absent = new UnitEnchantmentAbilityData(null!)
        {
            EnchantmentAbilityId = 0,
            EnchantmentPlayerIndex = -1,
            EnchantmentDuration = -1,
            Enchantment = null,
        };
        RefuseAbilityData(absent);
        var present = new UnitEnchantmentAbilityData(null!)
        {
            EnchantmentAbilityId = 65535,
            EnchantmentPlayerIndex = 0,
            EnchantmentDuration = -128,
            Enchantment = new CosmeticSurgeryEnchantment(null!) { FaceIndex = 3, UnitId = int.MinValue },
        };
        RefuseAbilityData(present);
        var edge = new UnitEnchantmentAbilityData(null!)
        {
            EnchantmentAbilityId = 1,
            EnchantmentPlayerIndex = 127,
            EnchantmentDuration = 127,
            Enchantment = new UnitEnchantment(null!) { EnchantmentAbilityId = int.MaxValue },
        };
        RefuseAbilityData(edge);
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
            var city = new CityEnchantment(null!) { _enchantmentId = value };
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
            army.DestinationL = 1;
            army.UserMode = Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
            var classValue = s_schema.Get("Army");
            var document = DocumentBackend.Write(TreeWriter.Write(army, Factory), classValue, s_schema, "game");
            Assert.Null(document.Find("DestinationX"));
            Assert.Null(document.Find("DestinationY"));
            Assert.Null(document.Find("DestinationL"));
            Assert.Null(document.Find("UserMode"));
            Check("Army", army);

            map.ConnectionType = PlayerConnectionType.Local;
            var source = new Army(null!) { DestinationX = 3, DestinationY = 4, DestinationL = 1 };
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
                Assert.Equal(1, viaPersist.DestinationL);
                Assert.True(viaPersist.UserMode.Contains(ArmyUserModeElement.Guard));
                Assert.Equal(3, viaReadWrite.DestinationX);
                Assert.Equal(4, viaReadWrite.DestinationY);
                Assert.Equal(1, viaReadWrite.DestinationL);
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
            previous = descriptor.SubstituteCreate(static owner => new ThrowingHatred(owner));
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
                        static (string _, ref bool cont) => cont = false));
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
            if (previous is not null) descriptor.SubstituteCreate(previous);
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
                    static (string _, ref bool cont) => cont = false));
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

    private static void RefuseAbilityData(UnitEnchantmentAbilityData objectValue)
    {
        try
        {
            var error = Assert.Throws<InvalidOperationException>(() => TreeWriter.Write(objectValue, Factory));
            Assert.Equal("Class AbilityData overrides ReadWrite and has no PersistFields.", error.Message);
        }
        finally
        {
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
        if (onError is not null) reader.OnError = onError;
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
