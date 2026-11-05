using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;

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

    public static IEnumerable<object?[]> Classes => EClass.All
        .Where(c => c.Type.Assembly == typeof(TEObject).Assembly).Select(c => new object?[] { c.Type, IncompleteCleanup.GetValueOrDefault(c.Type) });

    [Theory]
    [MemberData(nameof(Classes))]
    public void RegisteredCreationReturnsTheExactType(Type type, Type? cleanupError)
    {
        // These two entries require a linked owner even though the descriptor accepts null.
        if (type == typeof(Aow.Units.IsoHex.TDynamicIsometricHexagon) || type == typeof(TFlagControl))
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
            var error = Record.Exception(instance.Destroy);
            try
            {
                if (cleanupError is null) Assert.Null(error);
                else Assert.IsType(cleanupError, error);
            }
            finally { Runtime.ForgetReference(instance); }
        }
    }

    private static readonly Dictionary<Type, Type> IncompleteCleanup = new()
    {
        [typeof(Aow.Units.AoWE.TAIGroupControl)] = typeof(NullReferenceException),
        [typeof(Aow.Units.AoWE.TArmy)] = typeof(NullReferenceException),
        [typeof(Aow.Units.AoWE.TArmyEventLog)] = typeof(NullReferenceException),
        [typeof(Aow.Units.AoWE.TArmyHS)] = typeof(NullReferenceException),
        [typeof(Aow.Units.AoWE.TArmyInfoEventLog)] = typeof(NullReferenceException),
        [typeof(Aow.Units.AoWE.TArmyView)] = typeof(NullReferenceException),
        [typeof(Aow.Units.AoWE.TCombatParty)] = typeof(NullReferenceException),
        [typeof(Aow.Units.AoWE.TFastCombatParty)] = typeof(NullReferenceException),
        [typeof(Aow.Units.AoWE.THuntAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.AoWE.TWorldFXHS)] = typeof(NullReferenceException),
        [typeof(Aow.Units.AoWTC.TCCorpseHS)] = typeof(NullReferenceException),
        [typeof(Aow.Units.AoWTC.TCItemHS)] = typeof(NullReferenceException),
        [typeof(Aow.Units.AoWTC.TTacticalCombatUnitHS)] = typeof(NullReferenceException),
        [typeof(Aow.Units.AoWTC.TfxHS)] = typeof(NullReferenceException),
        [typeof(Aow.Units.BehaviorAG.TAbstractBehaviorAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.BehaviorAG.TAggressorAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.BehaviorAG.TDefenderAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.BehaviorAG.TExpanderAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.BehaviorAG.TNormalAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.BehaviorAG.TScorcherAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.BehaviorAG.TSkirmishAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.Crypt.TCrypt)] = typeof(NullReferenceException),
        [typeof(Aow.Units.Dungeon.TDungeon)] = typeof(NullReferenceException),
        [typeof(Aow.Units.Dungeon.TPrisonersArmy)] = typeof(NullReferenceException),
        [typeof(Aow.Units.Engine.TECustomNode)] = typeof(EAbstractError),
        [typeof(Aow.Units.ExploreS.TDefendersArmy)] = typeof(NullReferenceException),
        [typeof(Aow.Units.ExploreS.TExplorationSite)] = typeof(NullReferenceException),
        [typeof(Aow.Units.GlobalTargetSpells.TWatcherHS)] = typeof(NullReferenceException),
        [typeof(Aow.Units.HSEngine.TMapFieldTandO)] = typeof(EAbstractError),
        [typeof(Aow.Units.HSLib.TLibraryHS)] = typeof(NullReferenceException),
        [typeof(Aow.Units.IndependentAG.TGuardAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.IndependentAG.TGuardAreaAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.IndependentAG.TPatrolAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.IndependentAG.TRaidAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.IndependentAG.TRefugeAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.IndependentAG.TScoutAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.ItemExpS.TItemExplorationSite)] = typeof(NullReferenceException),
        [typeof(Aow.Units.MagicSanctuary.TMagicSanctuary)] = typeof(NullReferenceException),
        [typeof(Aow.Units.MainAG.TFortifyAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.MainAG.TPassiveAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.MainAG.TSuicidalAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.MainAG.TTransportAGC)] = typeof(NullReferenceException),
        [typeof(Aow.Units.MonsterL.TMonsterlair)] = typeof(NullReferenceException),
        [typeof(Aow.Units.NetworkE.TENodeView)] = typeof(EAbstractError),
        [typeof(Aow.Units.Pyramid.TPyramid)] = typeof(NullReferenceException),
        [typeof(Aow.Units.Ruin.TRuin)] = typeof(NullReferenceException),
        [typeof(Aow.Units.Sound.TENodeAccess)] = typeof(EAbstractError),
    };

    private static void CheckFamily(Type contract, Type[] types, IEnumerable<Type> registered) =>
        Assert.Equal(types.Where(t => t.IsClass && contract.IsAssignableFrom(t)).OrderBy(t => t.FullName, StringComparer.Ordinal),
            registered.Where(t => t.Assembly == typeof(TEObject).Assembly).OrderBy(t => t.FullName, StringComparer.Ordinal));
}
