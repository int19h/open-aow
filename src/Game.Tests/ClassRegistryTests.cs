using System.Reflection;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Schema;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Events;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class ClassRegistryTests
{
    [Fact]
    public void EveryProductClassHasItsDescriptor()
    {
        var types = typeof(EngineObject).Assembly.GetTypes();
        var engineTypes = types.Where(t => t.IsClass && typeof(EngineObject).IsAssignableFrom(t)).OrderBy(t => t.FullName, StringComparer.Ordinal);
        Assert.Equal(engineTypes, EngineClass.All.Where(c => c.Type.Assembly == typeof(EngineObject).Assembly).Select(c => c.Type).OrderBy(t => t.FullName, StringComparer.Ordinal));
        foreach (var type in engineTypes)
        {
            var descriptor = EngineClass.Of(type);
            Assert.Equal(type, descriptor.Type);
            Assert.Equal((int)type.GetField("ClassId", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)!.GetRawConstantValue()!, descriptor.ClassId);
            if (type != typeof(EngineObject)) Assert.Equal(type.BaseType, EngineClass.Of(type.BaseType!).Type);
        }
        CheckFamily(typeof(IAIPlayerActionClass), types, AIPlayerActionClass.All.Select(c => c.Class.Type));
        CheckFamily(typeof(IAIGroupClass), types, AIGroupClass.All.Select(c => c.Class.Type));
        CheckFamily(typeof(IEngineObjectEditorClass), types, EditorClass.All.Select(c => c.Class.Type));
        CheckFamily(typeof(ILibraryImageClass), types, LibraryImageClass.All.Select(c => c.Type));
        CheckFamily(typeof(IImageFileClass), types, ImageFileClass.All.Select(c => c.Type));
    }

    // own-formats.md 3.1: the third block registers one format mapping per persisted class that
    // needs unit code, exactly the classes versions.json names for the current version. Every
    // rule names members of its schema class. The kind classes keep their flat mapping until
    // 9.6-6 and 9.6-7 map their cases.
    [Fact]
    public void EverySchemaClassRequiringAUnitMappingHasItsLine()
    {
        var schema = GameFiles.Schema;
        Assert.Equal(FormatVersions.UnitMappings.Order(StringComparer.Ordinal), FormatMapping.All.Keys.Order(StringComparer.Ordinal));
        foreach (var (name, rules) in FormatMapping.All)
        {
            var classValue = schema.Find(name);
            Assert.True(classValue is not null, "The format mapping names no schema class: " + name);
            Assert.NotEmpty(rules);
            foreach (var member in rules.SelectMany(r => r.Members))
                Assert.True(member == "Children" ? classValue.KeepsChildren : classValue.PropertyByName(member) is not null, $"{name} has no member {member}.");
        }
        foreach (var name in FormatVersions.FlatMappings)
        {
            Assert.NotNull(schema.Find(name));
            Assert.False(FormatMapping.All.ContainsKey(name), name + " keeps its flat mapping in version 1.");
        }
        // A schema class with a case table has its line, and its line gives the converter the
        // kind family's FromFields and ToFields.
        foreach (var classValue in schema.Classes.Where(c => c.Mapping is not null))
        {
            Assert.Contains(classValue.Name, FormatVersions.UnitMappings);
            Assert.Contains(FormatMapping.All[classValue.Name], r => r is KindCases);
            Assert.Same(FormatMapping.All[classValue.Name].OfType<KindCases>().Single(), CaseMappings.Find(classValue.Name));
        }
    }

    // own-formats.md 2.2: the case tables name the cases by the keys the 9.6-6 and 9.6-7 kinds
    // register, and every case field is a value its kind writes back.
    [Fact]
    public void CaseTablesMatchTheRegisteredKinds()
    {
        var schema = GameFiles.Schema;
        var production = schema.Get("ProductionItem").Mapping!;
        Assert.Equal(Enumerable.Range(0, 256).Where(ProductionOrder.IsRegistered).Select(k => (long)k), production.Cases.Select(c => c.Key!.Value));
        foreach (var c in production.Cases)
        {
            var order = ProductionOrder.FromFields((int)c.Key!.Value, 7, 3);
            Assert.Equal(c.Name, NameTable.Delphi(order.GetType()));
            var (kind, parameter, groupId) = order.ToFields();
            Assert.Equal(c.Key, kind);
            Assert.Equal(c.Fields.Any(f => f.Member == "Parameter"), parameter == 7);
            Assert.Equal(c.Fields.Any(f => f.Member == "GroupId"), groupId == 3);
        }
        var diplomacy = schema.Get("DiplomaticAction").Mapping!;
        Assert.Equal(Enumerable.Range(0, 256).Where(k => DiplomaticProposal.IsRegistered((byte)k)).Select(k => (long)k), diplomacy.Cases.Select(c => c.Key!.Value));
        foreach (var c in diplomacy.Cases)
        {
            var proposal = DiplomaticProposal.FromFields((byte)c.Key!.Value, 70, 30);
            Assert.Equal(c.Name, NameTable.Delphi(proposal.GetType()));
            var (kind, gold, mana) = proposal.ToFields();
            Assert.Equal(c.Key, kind);
            Assert.Equal(c.Fields.Any(f => f.Member == "Gold"), gold == 70);
            Assert.Equal(c.Fields.Any(f => f.Member == "Mana"), mana == 30);
        }
        Assert.IsType<UnknownOrder>(ProductionOrder.FromFields(99, 0, 0));
        Assert.IsType<UnknownProposal>(DiplomaticProposal.FromFields(12, 0, 0));
    }

    public static IEnumerable<object?[]> Classes => EngineClass.All
        .Where(c => c.Type.Assembly == typeof(EngineObject).Assembly).Select(c => new object?[] { c.Type, s_incompleteCleanup.GetValueOrDefault(c.Type) });

    [Theory]
    [MemberData(nameof(Classes))]
    public void RegisteredCreationReturnsTheExactType(Type type, Type? cleanupError)
    {
        // These two entries require a linked owner even though the descriptor accepts null.
        if (type == typeof(DynamicIsometricHex) || type == typeof(FlagControl))
        {
            Assert.Throws<NullReferenceException>(() => EngineClass.Of(type).CreateInstance(null!));
            return;
        }
        var instance = EngineClass.Of(type).CreateInstance(null!);
        try { Assert.IsType(type, instance); }
        finally
        {
            // Construction alone leaves abstract nodes and world-linked objects incomplete.
            // Keep those existing teardown outcomes explicit, separate from factory identity.
            var error = Record.Exception(instance.Dispose);
            if (cleanupError is null) Assert.Null(error);
            else Assert.IsType(cleanupError, error);
        }
    }

    private static readonly Dictionary<Type, Type> s_incompleteCleanup = new()
    {
        [typeof(AIGroupControl)] = typeof(NullReferenceException),
        [typeof(Army)] = typeof(NullReferenceException),
        [typeof(ArmyEventLog)] = typeof(NullReferenceException),
        [typeof(ArmyHexSprite)] = typeof(NullReferenceException),
        [typeof(ArmyInfoEventLog)] = typeof(NullReferenceException),
        [typeof(ArmyView)] = typeof(NullReferenceException),
        [typeof(CombatParty)] = typeof(NullReferenceException),
        [typeof(HuntAIGroupControl)] = typeof(NullReferenceException),
        [typeof(WorldEffectHexSprite)] = typeof(NullReferenceException),
        [typeof(TacticalBattleCorpseHexSprite)] = typeof(NullReferenceException),
        [typeof(TacticalBattleItemHexSprite)] = typeof(NullReferenceException),
        [typeof(TacticalBattleUnitHexSprite)] = typeof(NullReferenceException),
        [typeof(BattleEffectSprite)] = typeof(NullReferenceException),
        [typeof(BehaviorAIGroupControl)] = typeof(NullReferenceException),
        [typeof(AggressorAIGroupControl)] = typeof(NullReferenceException),
        [typeof(DefenderAIGroupControl)] = typeof(NullReferenceException),
        [typeof(ExpanderAIGroupControl)] = typeof(NullReferenceException),
        [typeof(NormalAIGroupControl)] = typeof(NullReferenceException),
        [typeof(ScorcherAIGroupControl)] = typeof(NullReferenceException),
        [typeof(SkirmishAIGroupControl)] = typeof(NullReferenceException),
        [typeof(Crypt)] = typeof(NullReferenceException),
        [typeof(Dungeon)] = typeof(NullReferenceException),
        [typeof(PrisonersArmy)] = typeof(NullReferenceException),
        [typeof(ECustomNode)] = typeof(AbstractErrorException),
        [typeof(DefendersArmy)] = typeof(NullReferenceException),
        [typeof(ExplorationSite)] = typeof(NullReferenceException),
        [typeof(WatcherHexSprite)] = typeof(NullReferenceException),
        [typeof(GuardAIGroupControl)] = typeof(NullReferenceException),
        [typeof(GuardAreaAIGroupControl)] = typeof(NullReferenceException),
        [typeof(PatrolAIGroupControl)] = typeof(NullReferenceException),
        [typeof(RaidAIGroupControl)] = typeof(NullReferenceException),
        [typeof(RefugeAIGroupControl)] = typeof(NullReferenceException),
        [typeof(ScoutAIGroupControl)] = typeof(NullReferenceException),
        [typeof(ItemExplorationSite)] = typeof(NullReferenceException),
        [typeof(MagicSanctuary)] = typeof(NullReferenceException),
        [typeof(FortifyAIGroupControl)] = typeof(NullReferenceException),
        [typeof(PassiveAIGroupControl)] = typeof(NullReferenceException),
        [typeof(SuicidalAIGroupControl)] = typeof(NullReferenceException),
        [typeof(TransportAIGroupControl)] = typeof(NullReferenceException),
        [typeof(MonsterLair)] = typeof(NullReferenceException),
        [typeof(Pyramid)] = typeof(NullReferenceException),
        [typeof(Ruin)] = typeof(NullReferenceException),
    };

    private static void CheckFamily(Type contract, Type[] types, IEnumerable<Type> registered) =>
        Assert.Equal(types.Where(t => t.IsClass && contract.IsAssignableFrom(t)).OrderBy(t => t.FullName, StringComparer.Ordinal),
            registered.Where(t => t.Assembly == typeof(EngineObject).Assembly).OrderBy(t => t.FullName, StringComparer.Ordinal));
}
