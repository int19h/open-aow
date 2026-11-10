using System.Reflection;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Schema;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
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

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class ClassRegistryTests
{
    [Fact]
    public void EveryProductClassHasItsDescriptor()
    {
        var types = typeof(EObject).Assembly.GetTypes();
        var engineTypes = types.Where(t => t.IsClass && typeof(EObject).IsAssignableFrom(t)).OrderBy(t => t.FullName, StringComparer.Ordinal);
        Assert.Equal(engineTypes, EClass.All.Where(c => c.Type.Assembly == typeof(EObject).Assembly).Select(c => c.Type).OrderBy(t => t.FullName, StringComparer.Ordinal));
        foreach (var type in engineTypes)
        {
            var descriptor = EClass.Of(type);
            Assert.Equal(type, descriptor.Type);
            Assert.Equal((int)type.GetField("ClassID", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)!.GetRawConstantValue()!, descriptor.ClassID);
            Assert.Equal(type == typeof(EObject) ? null : type.BaseType, descriptor.Parent?.Type);
        }
        CheckFamily(typeof(IAIPlayerActionClass), types, AIPlayerActionClass.All.Select(c => c.Class.Type));
        CheckFamily(typeof(IAIGroupClass), types, AIGroupClass.All.Select(c => c.Class.Type));
        CheckFamily(typeof(IEObjectEditorClass), types, EditorClass.All.Select(c => c.Class.Type));
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
            var cls = schema.Find(name);
            Assert.True(cls is not null, "The format mapping names no schema class: " + name);
            Assert.NotEmpty(rules);
            foreach (var member in rules.SelectMany(r => r.Members))
                Assert.True(member == "Children" ? cls.KeepsChildren : cls.PropertyByName(member) is not null, $"{name} has no member {member}.");
        }
        foreach (var name in FormatVersions.FlatMappings)
        {
            Assert.NotNull(schema.Find(name));
            Assert.False(FormatMapping.All.ContainsKey(name), name + " keeps its flat mapping in version 1.");
        }
        // A schema class with a case table has its line, and its line gives the converter the
        // kind family's FromFields and ToFields.
        foreach (var cls in schema.Classes.Where(c => c.Mapping is not null))
        {
            Assert.Contains(cls.Name, FormatVersions.UnitMappings);
            Assert.Contains(FormatMapping.All[cls.Name], r => r is KindCases);
            Assert.Same(FormatMapping.All[cls.Name].OfType<KindCases>().Single(), CaseMappings.Find(cls.Name));
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
            Assert.Equal(c.Name, order.GetType().Name);
            var (id, parameter, groupId) = order.ToFields();
            Assert.Equal(c.Key, id);
            Assert.Equal(c.Fields.Any(f => f.Member == "Parameter"), parameter == 7);
            Assert.Equal(c.Fields.Any(f => f.Member == "GroupID"), groupId == 3);
        }
        var diplomacy = schema.Get("DiplomaticAction").Mapping!;
        Assert.Equal(Enumerable.Range(0, 256).Where(k => DiplomaticProposal.IsRegistered((byte)k)).Select(k => (long)k), diplomacy.Cases.Select(c => c.Key!.Value));
        foreach (var c in diplomacy.Cases)
        {
            var proposal = DiplomaticProposal.FromFields((byte)c.Key!.Value, 70, 30);
            Assert.Equal(c.Name, proposal.GetType().Name);
            var (kind, gold, mana) = proposal.ToFields();
            Assert.Equal(c.Key, kind);
            Assert.Equal(c.Fields.Any(f => f.Member == "Gold"), gold == 70);
            Assert.Equal(c.Fields.Any(f => f.Member == "Mana"), mana == 30);
        }
        Assert.IsType<UnknownOrder>(ProductionOrder.FromFields(99, 0, 0));
        Assert.IsType<UnknownProposal>(DiplomaticProposal.FromFields(12, 0, 0));
    }

    public static IEnumerable<object?[]> Classes => EClass.All
        .Where(c => c.Type.Assembly == typeof(EObject).Assembly).Select(c => new object?[] { c.Type, IncompleteCleanup.GetValueOrDefault(c.Type) });

    [Theory]
    [MemberData(nameof(Classes))]
    public void RegisteredCreationReturnsTheExactType(Type type, Type? cleanupError)
    {
        // These two entries require a linked owner even though the descriptor accepts null.
        if (type == typeof(DynamicIsometricHexagon) || type == typeof(FlagControl))
        {
            Assert.Throws<NullReferenceException>(() => EClass.Of(type).Create(null!));
            return;
        }
        var instance = EClass.Of(type).Create(null!);
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

    private static readonly Dictionary<Type, Type> IncompleteCleanup = new()
    {
        [typeof(AIGroupControl)] = typeof(NullReferenceException),
        [typeof(Army)] = typeof(NullReferenceException),
        [typeof(ArmyEventLog)] = typeof(NullReferenceException),
        [typeof(ArmyHS)] = typeof(NullReferenceException),
        [typeof(ArmyInfoEventLog)] = typeof(NullReferenceException),
        [typeof(ArmyView)] = typeof(NullReferenceException),
        [typeof(CombatParty)] = typeof(NullReferenceException),
        [typeof(FastCombatParty)] = typeof(NullReferenceException),
        [typeof(HuntAGC)] = typeof(NullReferenceException),
        [typeof(WorldFXHS)] = typeof(NullReferenceException),
        [typeof(CCorpseHS)] = typeof(NullReferenceException),
        [typeof(CItemHS)] = typeof(NullReferenceException),
        [typeof(TacticalCombatUnitHS)] = typeof(NullReferenceException),
        [typeof(TfxHS)] = typeof(NullReferenceException),
        [typeof(AbstractBehaviorAGC)] = typeof(NullReferenceException),
        [typeof(AggressorAGC)] = typeof(NullReferenceException),
        [typeof(DefenderAGC)] = typeof(NullReferenceException),
        [typeof(ExpanderAGC)] = typeof(NullReferenceException),
        [typeof(NormalAGC)] = typeof(NullReferenceException),
        [typeof(ScorcherAGC)] = typeof(NullReferenceException),
        [typeof(SkirmishAGC)] = typeof(NullReferenceException),
        [typeof(Crypt)] = typeof(NullReferenceException),
        [typeof(Dungeon)] = typeof(NullReferenceException),
        [typeof(PrisonersArmy)] = typeof(NullReferenceException),
        [typeof(ECustomNode)] = typeof(AbstractErrorException),
        [typeof(DefendersArmy)] = typeof(NullReferenceException),
        [typeof(ExplorationSite)] = typeof(NullReferenceException),
        [typeof(WatcherHS)] = typeof(NullReferenceException),
        [typeof(MapFieldTandO)] = typeof(AbstractErrorException),
        [typeof(LibraryHS)] = typeof(NullReferenceException),
        [typeof(GuardAGC)] = typeof(NullReferenceException),
        [typeof(GuardAreaAGC)] = typeof(NullReferenceException),
        [typeof(PatrolAGC)] = typeof(NullReferenceException),
        [typeof(RaidAGC)] = typeof(NullReferenceException),
        [typeof(RefugeAGC)] = typeof(NullReferenceException),
        [typeof(ScoutAGC)] = typeof(NullReferenceException),
        [typeof(ItemExplorationSite)] = typeof(NullReferenceException),
        [typeof(MagicSanctuary)] = typeof(NullReferenceException),
        [typeof(FortifyAGC)] = typeof(NullReferenceException),
        [typeof(PassiveAGC)] = typeof(NullReferenceException),
        [typeof(SuicidalAGC)] = typeof(NullReferenceException),
        [typeof(TransportAGC)] = typeof(NullReferenceException),
        [typeof(Monsterlair)] = typeof(NullReferenceException),
        [typeof(ENodeView)] = typeof(AbstractErrorException),
        [typeof(Pyramid)] = typeof(NullReferenceException),
        [typeof(Ruin)] = typeof(NullReferenceException),
        [typeof(ENodeAccess)] = typeof(AbstractErrorException),
    };

    private static void CheckFamily(Type contract, Type[] types, IEnumerable<Type> registered) =>
        Assert.Equal(types.Where(t => t.IsClass && contract.IsAssignableFrom(t)).OrderBy(t => t.FullName, StringComparer.Ordinal),
            registered.Where(t => t.Assembly == typeof(EObject).Assembly).OrderBy(t => t.FullName, StringComparer.Ordinal));
}
