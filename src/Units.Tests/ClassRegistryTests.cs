using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.World;
using Aow.Game.World.Structures;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed class ClassRegistryTests
{
    [Fact]
    public void EveryProductClassHasItsDescriptor()
    {
        var types = typeof(TEObject).Assembly.GetTypes();
        var engineTypes = types.Where(t => t.IsClass && typeof(TEObject).IsAssignableFrom(t)).OrderBy(t => t.FullName, StringComparer.Ordinal);
        Assert.Equal(engineTypes, EClass.All.Where(c => c.Type.Assembly == typeof(TEObject).Assembly).Select(c => c.Type).OrderBy(t => t.FullName, StringComparer.Ordinal));
        foreach (var type in engineTypes)
        {
            var descriptor = EClass.Of(type);
            Assert.Equal(type, descriptor.Type);
            Assert.Equal((int)type.GetField("ClassID", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy)!.GetRawConstantValue()!, descriptor.ClassID);
            Assert.Equal(type == typeof(TEObject) ? null : type.BaseType, descriptor.Parent?.Type);
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
        var schema = Aow.Formats.GameFiles.Schema;
        Assert.Equal(Aow.Formats.Schema.FormatVersions.UnitMappings.Order(StringComparer.Ordinal), FormatMapping.All.Keys.Order(StringComparer.Ordinal));
        foreach (var (name, rules) in FormatMapping.All)
        {
            var cls = schema.Find(name);
            Assert.True(cls is not null, "The format mapping names no schema class: " + name);
            Assert.NotEmpty(rules);
            foreach (var member in rules.SelectMany(r => r.Members))
                Assert.True(member == "Children" ? cls.KeepsChildren : cls.PropertyByName(member) is not null, $"{name} has no member {member}.");
        }
        foreach (var name in Aow.Formats.Schema.FormatVersions.FlatMappings)
        {
            Assert.NotNull(schema.Find(name));
            Assert.False(FormatMapping.All.ContainsKey(name), name + " keeps its flat mapping in version 1.");
        }
        // A schema class with a case table has its line, and its line gives the converter the
        // kind family's FromFields and ToFields.
        foreach (var cls in schema.Classes.Where(c => c.Mapping is not null))
        {
            Assert.Contains(cls.Name, Aow.Formats.Schema.FormatVersions.UnitMappings);
            Assert.Contains(FormatMapping.All[cls.Name], r => r is KindCases);
            Assert.Same(FormatMapping.All[cls.Name].OfType<KindCases>().Single(), Aow.Formats.CaseMappings.Find(cls.Name));
        }
    }

    // own-formats.md 2.2: the case tables name the cases by the keys the 9.6-6 and 9.6-7 kinds
    // register, and every case field is a value its kind writes back.
    [Fact]
    public void CaseTablesMatchTheRegisteredKinds()
    {
        var schema = Aow.Formats.GameFiles.Schema;
        var production = schema.Get("TProductionItem").Mapping!;
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
        var diplomacy = schema.Get("TDiplomaticAction").Mapping!;
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
        .Where(c => c.Type.Assembly == typeof(TEObject).Assembly).Select(c => new object?[] { c.Type, IncompleteCleanup.GetValueOrDefault(c.Type) });

    [Theory]
    [MemberData(nameof(Classes))]
    public void RegisteredCreationReturnsTheExactType(Type type, Type? cleanupError)
    {
        // These two entries require a linked owner even though the descriptor accepts null.
        if (type == typeof(Aow.Game.Draw.TDynamicIsometricHexagon) || type == typeof(TFlagControl))
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
        [typeof(Aow.Game.Ai.Groups.TAIGroupControl)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Units.TArmy)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Events.TArmyEventLog)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Draw.TArmyHS)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Events.TArmyInfoEventLog)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Units.TArmyView)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Combat.TCombatParty)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Combat.TFastCombatParty)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.THuntAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Draw.TWorldFXHS)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Draw.TCCorpseHS)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Draw.TCItemHS)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Draw.TTacticalCombatUnitHS)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Draw.TfxHS)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TAbstractBehaviorAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TAggressorAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TDefenderAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TExpanderAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TNormalAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TScorcherAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TSkirmishAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.World.Structures.TCrypt)] = typeof(NullReferenceException),
        [typeof(Aow.Game.World.Structures.TDungeon)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Units.TPrisonersArmy)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Engine.Nodes.TECustomNode)] = typeof(EAbstractError),
        [typeof(Aow.Game.World.Structures.TDefendersArmy)] = typeof(NullReferenceException),
        [typeof(Aow.Game.World.Structures.TExplorationSite)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Draw.TWatcherHS)] = typeof(NullReferenceException),
        [typeof(Aow.Game.World.Map.TMapFieldTandO)] = typeof(EAbstractError),
        [typeof(Aow.Game.Draw.TLibraryHS)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TGuardAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TGuardAreaAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TPatrolAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TRaidAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TRefugeAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TScoutAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.World.Structures.TItemExplorationSite)] = typeof(NullReferenceException),
        [typeof(Aow.Game.World.Structures.TMagicSanctuary)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TFortifyAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TPassiveAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TSuicidalAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Ai.Groups.TTransportAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Game.World.Structures.TMonsterlair)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Net.TENodeView)] = typeof(EAbstractError),
        [typeof(Aow.Game.World.Structures.TPyramid)] = typeof(NullReferenceException),
        [typeof(Aow.Game.World.Structures.TRuin)] = typeof(NullReferenceException),
        [typeof(Aow.Game.Audio.TENodeAccess)] = typeof(EAbstractError),
    };

    private static void CheckFamily(Type contract, Type[] types, IEnumerable<Type> registered) =>
        Assert.Equal(types.Where(t => t.IsClass && contract.IsAssignableFrom(t)).OrderBy(t => t.FullName, StringComparer.Ordinal),
            registered.Where(t => t.Assembly == typeof(TEObject).Assembly).OrderBy(t => t.FullName, StringComparer.Ordinal));
}
