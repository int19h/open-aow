using System.Collections;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.ImageLib;
using Game = Aow.Units.AoWE.AoWE;

namespace Aow.Rtl.Tests;

public sealed class LeftoverConstructionTests
{
    private sealed class Owner : TECustomNode
    {
        internal readonly List<TEObject> Members = [];
        internal readonly List<string> Observations = [];
        internal int Removals;
        public override int AddChild(TEObject child)
        {
            Members.Add(child);
            Observations.Add(Snapshot(child));
            return Members.Count - 1;
        }
        public override int RemoveChild(TEObject child) { Removals++; return Members.Remove(child) ? 0 : -1; }
    }
    private sealed class Fixture : IDisposable
    {
        private readonly int previousSeed = Rtl.RandSeed;
        private readonly TAoWHSSet previousSet = Game.AoWHSSet;
        private readonly TAoWEngine previousEngine = Game.AoWEngine;
        private readonly TAoWHSMap previousMap = Game.AoWHSMap;
        internal readonly TImageLibraryManager Manager = TImageLibraryManager.Create(null!);
        internal readonly TMapObjectResourceList CombatResources = new TMapObjectResourceList(null!);
        internal Fixture()
        {
            Rtl.RandSeed = 123456789;
            var set = new TAoWHSSet { FCombatResources = CombatResources };
            typeof(THSSet).GetField("FImageLibraryManager", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(set, Manager);
            Game.AoWHSSet = set;
            Game.AoWEngine = new TAoWEngine { FDictionary = new TAoWDictionary(null!) };
            Game.AoWHSMap = null!;
        }
        public void Dispose()
        {
            Rtl.RandSeed = previousSeed;
            Game.AoWEngine.FDictionary.Destroy();
            Game.AoWHSSet = previousSet;
            Game.AoWEngine = previousEngine;
            Game.AoWHSMap = previousMap;
            CombatResources.Destroy();
            Manager.Destroy();
        }
    }
    private static string Snapshot(object value)
    {
        var visited = new Dictionary<object, string>(ReferenceEqualityComparer.Instance);
        object? State(object? item, string path)
        {
            if (item is null) return null;
            Type type = item.GetType();
            if (item is AnsiString text) return text.ToString();
            if (type.IsEnum) return Convert.ToInt64(item);
            if (type.IsPrimitive || item is string || item is decimal) return item;
            if (item is Type represented) return represented.FullName;
            if (item is TObject.Meta meta) return meta.Type.FullName;
            if (item is Delegate handler) return handler.GetInvocationList().Select(x => x.Method.DeclaringType!.FullName + "." + x.Method.Name).ToArray();
            if (!type.IsValueType)
            {
                if (visited.TryGetValue(item, out string? found)) return "$ref:" + found;
                visited.Add(item, path);
            }
            if (item is Array array) return array.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            if (item is IList list) return list.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            var fields = new SortedDictionary<string, object?> { ["$type"] = type.FullName };
            for (Type? current = type; current is not null && current != typeof(TObject) && current != typeof(object); current = current.BaseType)
                foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(x => x.Name))
                {
                    if (field.Name == "eclass") continue; // the descriptor cache is not constructor state
                    string key = current.Name + "." + field.Name;
                    object? child = field.GetValue(item);
                    if (field.Name == "FOwner") { fields[key] = child is null ? null : "$owner"; continue; }
                    fields[key] = State(child, path + "." + key);
                }
            return fields;
        }
        return JsonSerializer.Serialize(State(value, "$"));
    }


    private static readonly string[] Names =
    [
        "Aow.Units.AoWE.TAIGroup",
        "Aow.Units.AoWE.TAIGroupControl",
        "Aow.Units.AoWE.TAIGroupControlPlugin",
        "Aow.Units.AoWE.TAbility",
        "Aow.Units.AoWE.TAbilityOwner",
        "Aow.Units.AoWE.TAoWCampaign",
        "Aow.Units.AoWE.TAoWCampaignScenario",
        "Aow.Units.AoWE.TAoWHSMap",
        "Aow.Units.AoWE.TArmy",
        "Aow.Units.AoWE.TArmyCombatMoveTE",
        "Aow.Units.AoWE.TArmyCombineMoveTE",
        "Aow.Units.AoWE.TArmyDefaultMoveTE",
        "Aow.Units.AoWE.TArmyNegotiateMoveTE",
        "Aow.Units.AoWE.TChangeGlobalMagicTE",
        "Aow.Units.AoWE.TChangePlayerMagicTE",
        "Aow.Units.AoWE.TChangePlayerSettingsTE",
        "Aow.Units.AoWE.TCheatTE",
        "Aow.Units.AoWE.TCombat",
        "Aow.Units.AoWE.TCombatAction",
        "Aow.Units.AoWE.TCombatFlowControlTE",
        "Aow.Units.AoWE.TCombatObject",
        "Aow.Units.AoWE.TCombatUnit",
        "Aow.Units.AoWE.TConstructStructureTE",
        "Aow.Units.AoWE.TDiplomaticActionTE",
        "Aow.Units.AoWE.TDisbandUnitTE",
        "Aow.Units.AoWE.TDispelMagicAbilityTE",
        "Aow.Units.AoWE.TDispelMagicCA",
        "Aow.Units.AoWE.TDurationAbility",
        "Aow.Units.AoWE.TEndTurnTE",
        "Aow.Units.AoWE.TEnhancementAbility",
        "Aow.Units.AoWE.TFire",
        "Aow.Units.AoWE.TFireResource",
        "Aow.Units.AoWE.TFrozenWaterHS",
        "Aow.Units.AoWE.TGameOverTE",
        "Aow.Units.AoWE.TGlobalEnchantment",
        "Aow.Units.AoWE.THealingTE",
        "Aow.Units.AoWE.THeroUpdateTE",
        "Aow.Units.AoWE.THeroUpgradeTE",
        "Aow.Units.AoWE.TItemList",
        "Aow.Units.AoWE.TItemTE",
        "Aow.Units.AoWE.TLeaderSetupControl",
        "Aow.Units.AoWE.TMoveArmyTE",
        "Aow.Units.AoWE.TMoveControlSettings",
        "Aow.Units.AoWE.TMoveTE",
        "Aow.Units.AoWE.TMultiLevelAbility",
        "Aow.Units.AoWE.TMultiMoveTE",
        "Aow.Units.AoWE.TNegotiateTE",
        "Aow.Units.AoWE.TPlayerMagicControl",
        "Aow.Units.AoWE.TProductionControl",
        "Aow.Units.AoWE.TProductionTE",
        "Aow.Units.AoWE.TSaveGameTE",
        "Aow.Units.AoWE.TSpell",
        "Aow.Units.AoWE.TSpellCaster",
        "Aow.Units.AoWE.TSpellTECallViewP4",
        "Aow.Units.AoWE.TStartupSongList",
        "Aow.Units.AoWE.TStructureShadow",
        "Aow.Units.AoWE.TTerminatePlayerTE",
        "Aow.Units.AoWE.TUnitEnchantmentAbility",
        "Aow.Units.AoWE.TUnitShadowHS",
        "Aow.Units.AoWE.TUnitSpell",
        "Aow.Units.AoWE.TUnitSpellTE",
        "Aow.Units.AoWE.TWorldFXHS",
        "Aow.Units.CitySpells.TCityEnchantmentSpellTE",
        "Aow.Units.CitySpells.TCityTargetSpellTE",
        "Aow.Units.CitySpells.TTownGateTE",
        "Aow.Units.Engine.TEChangeNotifyEventList",
        "Aow.Units.Engine.TEChangeNotifyNode",
        "Aow.Units.Engine.TEControlStyleInverseSortNode",
        "Aow.Units.Engine.TEControlStyleNode",
        "Aow.Units.Engine.TECustomNode",
        "Aow.Units.Engine.TECustomOwner",
        "Aow.Units.Engine.TEInverseSortNode",
        "Aow.Units.Engine.TENode",
        "Aow.Units.Engine.TEObjectEditor",
        "Aow.Units.Engine.TEReferenceNode",
        "Aow.Units.Engine.TEResource",
        "Aow.Units.Engine.TEResourceList",
        "Aow.Units.Engine.TESortNode",
        "Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE",
        "Aow.Units.GlobalSpells.TBirdsViewHS",
        "Aow.Units.GlobalSpells.TConcealAreaHS",
        "Aow.Units.GlobalSpells.TDisjunctionTE",
        "Aow.Units.GlobalSpells.TWarpPartyTE",
        "Aow.Units.GlobalTargetSpells.TFireBarrierTE",
        "Aow.Units.GlobalTargetSpells.TFreezeWaterTE",
        "Aow.Units.GlobalTargetSpells.TGlobalTargetSpell",
        "Aow.Units.GlobalTargetSpells.TGlobalTargetSpellCalls",
        "Aow.Units.GlobalTargetSpells.THealingShowersTE",
        "Aow.Units.GlobalTargetSpells.TLevelTerrainTE",
        "Aow.Units.GlobalTargetSpells.TRejuvenateTE",
        "Aow.Units.GlobalTargetSpells.TTornadoTE",
        "Aow.Units.GlobalTargetSpells.TVortexTE",
        "Aow.Units.GlobalTargetSpells.TWatcherHS",
        "Aow.Units.GlobalTargetSpells.TWatcherTE",
        "Aow.Units.HSEngine.TFixedVisibleHS",
        "Aow.Units.HSEngine.THPBasedHS",
        "Aow.Units.HSEngine.THSSet",
        "Aow.Units.HSEngine.TMapField",
        "Aow.Units.HSEngine.TMapFieldTandO",
        "Aow.Units.HSEngine.TMapLevel",
        "Aow.Units.HSEngine.TMapObjectResource",
        "Aow.Units.HSEngine.TMultiHexMOR",
        "Aow.Units.ILTer.TILTerrainMO",
        "Aow.Units.NetworkE.TENodeView",
        "Aow.Units.NetworkE.TTokenEvent",
        "Aow.Units.Sound.TENodeAccess",
        "Aow.Units.Sound.TSonglist",
        "Aow.Units.SummonSpells.TSummonSpellTE",
    ];
    public static IEnumerable<object[]> Classes => Names.Select(name => new object[] { name });
    private static object? Invoke(Func<object?> action)
    {
        try { return action(); }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }
    private static TEObject Direct(Type type, TECustomNode owner)
    {
        var constructor = type.GetConstructor([typeof(TECustomNode)]);
        if (constructor is not null) return (TEObject)Invoke(() => constructor.Invoke([owner]))!;
        var factory = type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, [typeof(TECustomNode)]);
        if (factory is not null) return (TEObject)Invoke(() => factory.Invoke(null, [owner]))!;
        return EClass.Of(type).Create(owner);
    }
    [Theory]
    [MemberData(nameof(Classes))]
    public void StateOwnerObservationAndTeardownMatchTheOriginal(string name)
    {
        Type type = typeof(TSpell).Assembly.GetType(name)!;

        for (int owned = 0; owned < 2; owned++)
        for (int route = 0; route < 4; route++)
        {
            using var fixture = new Fixture();
            var owner = owned == 0 ? null : new Owner();
            TEObject? value = null;
            var states = new List<string>();
            try
            {
                value = route == 1 ? EClass.Of(type).Create(owner!)
                    : route == 0 ? Direct(type, owner!)
                    : route == 2 ? ((TEObject.Meta)MetaRegistry.Of(type)).Create(owner!)
                    : Saved(type, owner!);
                Assert.IsType(type, value);
                Assert.Same(owner, value.Owner);
                states.Add("success:" + Snapshot(value));
            }
            catch (System.Exception error) { states.Add("failure:" + error.GetType().FullName + ":" + error.Message); }
            finally
            {
                try { value?.Destroy(); }
                catch (System.Exception error) { states.Add("destroy:" + error.GetType().FullName + ":" + error.Message); }
            }
            states.Add("owner:" + (owner?.Members.Count ?? 0));
            if (owner is not null) states.Add(JsonSerializer.Serialize(owner.Observations));
            string actual = string.Join("\n", states);
            string key = name + ".owner" + owned + ".route" + route;
            string hash = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(actual)));
            {
                using var expected = JsonDocument.Parse(typeof(LeftoverConstructionTests).Assembly.GetManifestResourceStream("Aow.Rtl.Tests.LeftoverConstructionBaseline.json")!);
                Assert.True(expected.RootElement.GetProperty(key).GetString() == hash, key + " route " + route + "\n" + actual);
            }
        }
    }
    private static TEObject Saved(Type type, TECustomNode owner)
    {
        if (Aow.Units.Engine.Engine.Engines is null) Aow.Units.Engine.Engine.Initialize();
        var engine = new TEngine();
        engine.Create__init(null!);
        try
        {
            var descriptor = EClass.Of(type);
            engine.Options -= TSet<TEngineModeElement>.Of(Aow.Units.Engine.Engine.eoVerifyClasses);
            try { engine.UnregisterEClasses([engine.GetEClass(descriptor.ClassID)]); }
            catch (Aow.Rtl.Exception) { }
            engine.RegisterEClasses([descriptor]);
            return engine.CreateEObject(descriptor.ClassID, owner);
        }
        finally { engine.Destroy(); }
    }

    private sealed class RejectingOwner : TECustomNode
    {
        internal TAoWHSMap? Captured;
        public override int AddChild(TEObject child)
        {
            Captured = Assert.IsType<TAoWHSMap>(child);
            Assert.Equal(0, Captured.FMapChangedLockCount);
            throw new InvalidOperationException("reject after insertion");
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void WorldMapFailureRunsOneDestructorAndKeepsItsReplacementException(int route)
    {
        using var fixture = new Fixture();
        var owner = new RejectingOwner();
        Assert.Throws<NullReferenceException>(() => route switch
        {
            0 => TAoWHSMap.Create(owner),
            1 => EClass.Of<TAoWHSMap>().Create(owner),
            2 => ((TEObject.Meta)MetaRegistry.Of(typeof(TAoWHSMap))).Create(owner),
            _ => Saved(typeof(TAoWHSMap), owner)
        });
        var partial = Assert.IsType<TAoWHSMap>(owner.Captured);
        Assert.Equal(1, partial.FMapChangedLockCount);
        Assert.Same(owner, partial.Owner);
        Assert.Null(partial.FItemControl);
        Assert.Null(partial.FEffects);
        Assert.Empty(typeof(TAoWHSMap).GetConstructors());
    }

    [Fact]
    public void MapSettingsAndMouseKeepTheirInheritedInitialization()
    {
        var settings = new TAoWNewHSMapSettings();
        Assert.Equal((64, 64, 1), (settings.Width, settings.Height, settings.Levels));
        Assert.Equal(TAoWHSMap.ClassID, settings.HSMapClassID);
        Assert.Equal(TMapContainer.ClassID, settings.MapContainerClassID);
        Assert.Equal(TAoWMapLevel.ClassID, settings.MapLevelClassID);
        var mouse = new TAoWHSMapMouse();
        Assert.Equal((-1, -1, -1, -1), (mouse.FSelectionRectangle.Left, mouse.FSelectionRectangle.Top,
            mouse.FSelectionRectangle.Right, mouse.FSelectionRectangle.Bottom));
        Assert.NotNull(mouse.FRectangleSelectEvents);
        mouse.Destroy();
        settings.Destroy();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void HashTablesAllocateEachBucketOnceInOrder(bool strings)
    {
        int before = new TObject().InstanceId;
        TObject table;
        var buckets = new List<TObject>();
        if (strings)
        {
            var value = new TStringHashTable256();
            table = value;
            for (int i = 0; i < 256; i++) buckets.Add(value.FLists[i]);
        }
        else
        {
            var value = new TIntegerHashTable256();
            table = value;
            for (int i = 0; i < 256; i++) buckets.Add(value.FLists[i]);
        }
        Assert.Equal(before + 1, table.InstanceId);
        Assert.Equal(258, new TObject().InstanceId - before);
        for (int i = 0; i < buckets.Count; i++) Assert.Equal(before + i + 2, buckets[i].InstanceId);
        table.Destroy();
    }
}
