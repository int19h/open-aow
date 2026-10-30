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

[Collection("Engine")]
[EngineState]
public sealed class SweepConstructionTests
{
    private sealed class Owner : TECustomNode
    {
        public Owner() : base(null!) { }

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
            var set = EngineFixture.Configure(TAoWHSSet.Create(null!), value => { value.FCombatResources = CombatResources; });
            typeof(THSSet).GetField("FImageLibraryManager", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(set, Manager);
            Game.AoWHSSet = set;
            Game.AoWEngine = EngineFixture.Engine;
            Game.AoWHSMap = null!;
            Game.AoWHSSet.FUnitGFXResources = null!;
            Rtl.RandSeed = 123456789;
        }
        public void Dispose()
        {
            Rtl.RandSeed = previousSeed;

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
            if (item is TAoWEngine) return "$engine";
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
        "Aow.Units.AirNode.TAirNode",
        "Aow.Units.AirNode.TAirNodeResource",
        "Aow.Units.AltDeath.TAltarOfDeath",
        "Aow.Units.AltDeath.TAltarOfDeathResource",
        "Aow.Units.AltFire.TAltarOfFire",
        "Aow.Units.AltFire.TAltarOfFireResource",
        "Aow.Units.AltIce.TAltarOfIce",
        "Aow.Units.AltIce.TAltarOfIceResource",
        "Aow.Units.AltLife.TAltarOfLife",
        "Aow.Units.AltLife.TAltarOfLifeResource",
        "Aow.Units.AltLight.TAltarOfLightning",
        "Aow.Units.AltLight.TAltarOfLightningResource",
        "Aow.Units.AltPest.TAltarOfPestilence",
        "Aow.Units.AltPest.TAltarOfPestilenceResource",
        "Aow.Units.Altar.TAltar",
        "Aow.Units.Altar.TAltarResource",
        "Aow.Units.Animation.TAnimation",
        "Aow.Units.Animation.TAnimationTE",
        "Aow.Units.Animation.TImageSequenceAnimation",
        "Aow.Units.Animation.TTwinkleAnimation",
        "Aow.Units.AoWHex.TAoWBorderHexagon",
        "Aow.Units.AoWHex.TAoWHexagon",
        "Aow.Units.AoWHex.TAoWHexagonResource",
        "Aow.Units.AoWHex.TAoWWaterHexagon",
        "Aow.Units.AoWHex.TAoWWaterHexagonResource",
        "Aow.Units.AoWRoad.TAoWRoad",
        "Aow.Units.AoWTC.TAoWCombatMapField",
        "Aow.Units.AoWTC.TAoWCombatMapLevel",
        "Aow.Units.AoWTC.TCBloodHS",
        "Aow.Units.AoWTC.TCCorpseHS",
        "Aow.Units.AoWTC.TCItemHS",
        "Aow.Units.AoWTC.TCOozeHS",
        "Aow.Units.AoWTC.TRangedPathHS",
        "Aow.Units.AoWTC.TTacticalCombatUnitHS",
        "Aow.Units.AoWTC.TUnitPositionHS",
        "Aow.Units.AoWTC.TfxHS",
        "Aow.Units.AoWTer.TAoWTerrain",
        "Aow.Units.AoWTer.TAoWTerrainResource",
        "Aow.Units.Arena.TArena",
        "Aow.Units.Arena.TArenaResource",
        "Aow.Units.Arena.TArenaTE",
        "Aow.Units.BldGuild.TBuildersGuild",
        "Aow.Units.BldGuild.TBuildersGuildResource",
        "Aow.Units.BloodShrine.TBloodShrine",
        "Aow.Units.BloodShrine.TBloodShrineAnimation",
        "Aow.Units.BloodShrine.TBloodShrineResource",
        "Aow.Units.BorderHx.TBorderHexagon",
        "Aow.Units.BorderHx.TBorderHexagonResource",
        "Aow.Units.Bridge.TBridge",
        "Aow.Units.Bridge.TBridgeResource",
        "Aow.Units.Cave.TCave",
        "Aow.Units.Cave.TCaveResource",
        "Aow.Units.CenterViewLoc.TCenterViewLocation",
        "Aow.Units.CenterViewLoc.TCenterViewLocationResource",
        "Aow.Units.City.TCity",
        "Aow.Units.City.TCityOverlayHS",
        "Aow.Units.City.TCityResource",
        "Aow.Units.CityWall.TCityWall",
        "Aow.Units.CityWall.TCityWallDoor",
        "Aow.Units.CityWall.TCityWallDoorResource",
        "Aow.Units.CityWall.TCityWallResource",
        "Aow.Units.CombatFire.TCombatFire",
        "Aow.Units.CombatFire.TCombatFireResource",
        "Aow.Units.CombatHex.TCombatHexagon",
        "Aow.Units.CombatHex.TCombatHexagonResource",
        "Aow.Units.CombatHex.TWaterCombatHexagon",
        "Aow.Units.CombatHex.TWaterCombatHexagonResource",
        "Aow.Units.CombatILTer.TCombatFixedILTerrain",
        "Aow.Units.CombatILTer.TCombatFixedILTerrainResource",
        "Aow.Units.CombatILTer.TCombatILTerrain",
        "Aow.Units.CombatILTer.TCombatILTerrainResource",
        "Aow.Units.CombatILTer.TCombatILTerrainShadow",
        "Aow.Units.CombatObstacle.TCombatObstacle",
        "Aow.Units.CombatObstacle.TCombatObstacleOverlay",
        "Aow.Units.CombatObstacle.TCombatObstacleResource",
        "Aow.Units.CombatObstacle.TCombatObstacleShadow",
        "Aow.Units.CombatRoad.TCombatRoad",
        "Aow.Units.CombatRoad.TCombatRoadResource",
        "Aow.Units.CombatStructure.TCombatStructure",
        "Aow.Units.CombatStructure.TCombatStructureResource",
        "Aow.Units.CombatTE.TCAbRangedTE",
        "Aow.Units.CombatTE.TCAbTouchMoveTE",
        "Aow.Units.CombatTE.TCEndCombatTE",
        "Aow.Units.CombatTE.TCEndTurnTE",
        "Aow.Units.CombatTE.TCMeleeMoveTE",
        "Aow.Units.CombatTE.TCombatMoveTE",
        "Aow.Units.CombatTE.TTerminateCombatTE",
        "Aow.Units.CombatTerrain.TCombatTerrain",
        "Aow.Units.CombatTerrain.TCombatTerrainResource",
        "Aow.Units.CombatTerrain.TCombatTerrainShadow",
        "Aow.Units.Crops.TCrop",
        "Aow.Units.Crops.TCropResource",
        "Aow.Units.Crops.TPlayerCropStructure",
        "Aow.Units.Crops.TPlayerCropStructureResource",
        "Aow.Units.Crypt.TCrypt",
        "Aow.Units.Crypt.TCryptResource",
        "Aow.Units.DeathNode.TDeathNode",
        "Aow.Units.DeathNode.TDeathNodeResource",
        "Aow.Units.Dungeon.TDungeon",
        "Aow.Units.Dungeon.TDungeonResource",
        "Aow.Units.EarthNode.TEarthNode",
        "Aow.Units.EarthNode.TEarthNodeResource",
        "Aow.Units.ExploreS.TExplorationSite",
        "Aow.Units.ExploreS.TExplorationSiteResource",
        "Aow.Units.Farm.TFarm",
        "Aow.Units.Farm.TFarmResource",
        "Aow.Units.FireNode.TFireNode",
        "Aow.Units.FireNode.TFireNodeResource",
        "Aow.Units.FlagEvents.TFlagEvent",
        "Aow.Units.FlagEvents.TFlagEventResource",
        "Aow.Units.FlagEvents.TFlagMessageEvent",
        "Aow.Units.FlagEvents.TFlagMessageEventResource",
        "Aow.Units.FlagEvents.TTutorialMessageEvent",
        "Aow.Units.FlagEvents.TTutorialMessageEventResource",
        "Aow.Units.FleeLoc.TFleeLocation",
        "Aow.Units.FleeLoc.TFleeLocationResource",
        "Aow.Units.FleeLoc.TMovementHS",
        "Aow.Units.Gems.TGems",
        "Aow.Units.Gems.TGemsResource",
        "Aow.Units.HSLib.TLibraryHS",
        "Aow.Units.Hexagon.THexagon",
        "Aow.Units.Hexagon.THexagonResource",
        "Aow.Units.Hill.THillMO",
        "Aow.Units.Hill.THillMOR",
        "Aow.Units.HolyG.THolyGround",
        "Aow.Units.HolyG.THolyGroundResource",
        "Aow.Units.HolyG.THolyGroundSpell",
        "Aow.Units.HolyG.THolyGroundSpellTE",
        "Aow.Units.House.THouse",
        "Aow.Units.House.THouseResource",
        "Aow.Units.HxBridge.THexagonBridge",
        "Aow.Units.HxBridge.THexagonBridgeResource",
        "Aow.Units.ILTer.TFixedILTerrainMO",
        "Aow.Units.ILTer.TFixedILTerrainMOR",
        "Aow.Units.ILTer.TILTerrainMO",
        "Aow.Units.ILTer.TILTerrainMOR",
        "Aow.Units.IsoCombatHex.TIsometricCombatHexagon",
        "Aow.Units.IsoCombatHex.TIsometricCombatHexagonResource",
        "Aow.Units.ItemExpS.TItemExplorationSite",
        "Aow.Units.ItemExpS.TItemExplorationSiteResource",
        "Aow.Units.LifeNode.TLifeNode",
        "Aow.Units.LifeNode.TLifeNodeResource",
        "Aow.Units.MagicSanctuary.TMagicSanctuary",
        "Aow.Units.MagicSanctuary.TMagicSanctuaryOverlayHS",
        "Aow.Units.MagicSanctuary.TMagicSanctuaryResource",
        "Aow.Units.MainStructure.TMainStructure",
        "Aow.Units.MainStructure.TMainStructureResource",
        "Aow.Units.ManaCrystals.TManaCrystals",
        "Aow.Units.ManaCrystals.TManaCrystalsResource",
        "Aow.Units.Merchant.TMerchant",
        "Aow.Units.Merchant.TMerchantResource",
        "Aow.Units.Mine.TMine",
        "Aow.Units.Mine.TMineResource",
        "Aow.Units.MonsterL.TMonsterlair",
        "Aow.Units.MonsterL.TMonsterlairResource",
        "Aow.Units.Mountain.TMountainMO",
        "Aow.Units.Mountain.TMountainMOR",
        "Aow.Units.Mountain.TRaiseTerrain",
        "Aow.Units.Mountain.TRaiseTerrainTE",
        "Aow.Units.Mountain.TRaisedMountain",
        "Aow.Units.Mountain.TRaisedMountainResource",
        "Aow.Units.MoveOnEvents.TChangeSongMoveOnEvent",
        "Aow.Units.MoveOnEvents.TChangeSongMoveOnEventResource",
        "Aow.Units.MoveOnEvents.TMoveOnEvent",
        "Aow.Units.MoveOnEvents.TMoveOnEventResource",
        "Aow.Units.MoveOnEvents.TPlaySampleMoveOnEvent",
        "Aow.Units.MoveOnEvents.TPlaySampleMoveOnEventResource",
        "Aow.Units.MoveOnEvents.TShowMessageMoveOnEvent",
        "Aow.Units.MoveOnEvents.TShowMessageMoveOnEventResource",
        "Aow.Units.OverlayObject.TOverlayObject",
        "Aow.Units.OverlayObject.TOverlayObjectResource",
        "Aow.Units.OverviewUnit.TOverviewUnitHS",
        "Aow.Units.Pad.TPad",
        "Aow.Units.Pad.TPadResource",
        "Aow.Units.PoisonC.TPoisonCloud",
        "Aow.Units.PoisonC.TPoisonCloudResource",
        "Aow.Units.PoisonP.TPoisonPlant",
        "Aow.Units.PoisonP.TPoisonPlantResource",
        "Aow.Units.PoisonP.TPoisonPlantSpell",
        "Aow.Units.PoisonP.TPoisonPlantSpellTE",
        "Aow.Units.PowerNode.TPowerNode",
        "Aow.Units.PowerNode.TPowerNodeResource",
        "Aow.Units.PrdPlace.TProductionPlace",
        "Aow.Units.PrdPlace.TProductionPlaceResource",
        "Aow.Units.Pyramid.TPyramid",
        "Aow.Units.Pyramid.TPyramidResource",
        "Aow.Units.RandomNode.TRandomNode",
        "Aow.Units.RandomNode.TRandomNodeResource",
        "Aow.Units.ReflectingPool.TReflectingPool",
        "Aow.Units.ReflectingPool.TReflectingPoolResource",
        "Aow.Units.RndHex.TRndHexagonResource",
        "Aow.Units.Road.TRoad",
        "Aow.Units.Road.TRoadResource",
        "Aow.Units.Ruin.TRuin",
        "Aow.Units.Ruin.TRuinResource",
        "Aow.Units.Shipyard.TShipyard",
        "Aow.Units.Shipyard.TShipyardResource",
        "Aow.Units.Sign.TSign",
        "Aow.Units.Sign.TSignResource",
        "Aow.Units.Sign.TUpdateSignTE",
        "Aow.Units.SingleCombatHex.TSingleCombatHexagon",
        "Aow.Units.SingleCombatHex.TSingleCombatHexagonResource",
        "Aow.Units.SmokeCloud.TSmokeCloud",
        "Aow.Units.SmokeCloud.TSmokeCloudResource",
        "Aow.Units.Spring.TSpringOfLife",
        "Aow.Units.Spring.TSpringOfLifeAnimation",
        "Aow.Units.Spring.TSpringOfLifeResource",
        "Aow.Units.Storms.TBlastStorm",
        "Aow.Units.Storms.TDeathStorm",
        "Aow.Units.Storms.TDivineStorm",
        "Aow.Units.Storms.TFireStorm",
        "Aow.Units.Storms.TIceStorm",
        "Aow.Units.Storms.TLightningStorm",
        "Aow.Units.Storms.TPestilenceStorm",
        "Aow.Units.Storms.TStorm",
        "Aow.Units.Storms.TStormSpellTE",
        "Aow.Units.Teleport.TTeleport",
        "Aow.Units.Teleport.TTeleportResource",
        "Aow.Units.Terrain.TTerrainMO",
        "Aow.Units.Terrain.TTerrainMOR",
        "Aow.Units.Tower.TTower",
        "Aow.Units.Tower.TTowerResource",
        "Aow.Units.Townhall.TTownhall",
        "Aow.Units.Townhall.TTownhallResource",
        "Aow.Units.UnHolyG.TUnHolyGround",
        "Aow.Units.UnHolyG.TUnHolyGroundResource",
        "Aow.Units.UnHolyG.TUnholyGroundSpell",
        "Aow.Units.UnHolyG.TUnholyGroundSpellTE",
        "Aow.Units.WaterNode.TWaterNode",
        "Aow.Units.WaterNode.TWaterNodeResource",
        "Aow.Units.WizardsTower.TWizardsTower",
        "Aow.Units.WizardsTower.TWizardsTowerResource",
        "Aow.Units.WorkShop.TWorkshop",
        "Aow.Units.WorkShop.TWorkshopResource",
        "Aow.Units.HSEngine.TMultiHexMO",
        "Aow.Units.HSEngine.TMultiHexMOR",
        "Aow.Units.HSEngine.THexagonSprite",
        "Aow.Units.HSEngine.TMapObject",
        "Aow.Units.HSEngine.TMapObjectResource",
        "Aow.Units.HSEngine.TSingleHS",
        "Aow.Units.HSEngine.TAbstractRoad",
        "Aow.Units.HSEngine.TAbstractRoadResource",
        "Aow.Units.HSEngine.TAbstractHexagonResource",
        "Aow.Units.AoWE.TPlayerStructure",
        "Aow.Units.AoWE.TPlayerStructureResource",
        "Aow.Units.AoWE.TStructure",
        "Aow.Units.AoWE.TStructureResource",
        "Aow.Units.NetworkE.TTokenEvent",
        "Aow.Units.AoWE.TUnitHS",
        "Aow.Units.AoWE.TItemHS",
        "Aow.Units.AoWE.TMoveTE",
        "Aow.Units.AoWE.TSpellTE",
        "Aow.Units.AoWE.TStructureTE",
        "Aow.Units.AoWE.TAoWMapField",
        "Aow.Units.AoWE.TAoWMapLevel",
        "Aow.Units.HSEngine.TAbstractBorderHexagonResource",
        "Aow.Units.IsoHex.TIsometricHexagon",
        "Aow.Units.IsoHex.TLowerIsometricHexagon",
        "Aow.Units.IsoHex.TUpperIsometricHexagon",
        "Aow.Units.IsoHex.TIsometricHexagonResource",
        "Aow.Units.IsoHex.TLowerIsometricHexagonResource",
        "Aow.Units.IsoHex.TUpperIsometricHexagonResource",
        "Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE",
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
        return (TEObject)Invoke(() => Activator.CreateInstance(type, [owner]))!;
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
            string key = name + ".owner" + owned;
            string hash = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(actual)));
            {
                using var expected = JsonDocument.Parse(typeof(SweepConstructionTests).Assembly.GetManifestResourceStream("Aow.Rtl.Tests.SweepConstructionBaseline.json")!);
                Assert.True(expected.RootElement.GetProperty(key).GetString() == hash, key + " route " + route + "\n" + actual);
            }
        }
    }
    private static TEObject Saved(Type type, TECustomNode owner)
    {
        if (Aow.Units.Engine.Engine.Engines is null) Aow.Units.Engine.Engine.Initialize();
        var engine = new TEngine(null!);
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
    [Theory]
    [InlineData("Aow.Units.AoWE.TAbstractAoWHSMapMouse")]
    [InlineData("Aow.Units.AoWTC.TAoWCombatMapMouse")]
    [InlineData("Aow.Units.AoWTC.TAoWNewCombatMapSettings")]
    [InlineData("Aow.Units.AoWTC.TCombatMoveControl")]
    public void HelperStateMatchesTheOriginalFactory(string name)
    {
        using var fixture = new Fixture();
        Type type = typeof(TSpell).Assembly.GetType(name)!;
        if (name.EndsWith(".TAoWNewCombatMapSettings", StringComparison.Ordinal))
            typeof(THSEngine).GetField("FHSSFileName", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(Game.AoWEngine, default(AnsiString));
        Type[] signature = name.EndsWith(".TCombatMoveControl", StringComparison.Ordinal) ? [typeof(THSMap)] : [];
        var value = (TObject)Invoke(() => type.GetConstructor(signature)!.Invoke(signature.Length == 0 ? [] : [null]))!;
        try
        {
            string hash = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(Snapshot(value))));
            using var expected = JsonDocument.Parse(typeof(SweepConstructionTests).Assembly.GetManifestResourceStream("Aow.Rtl.Tests.SweepConstructionBaseline.json")!);
            Assert.True(expected.RootElement.GetProperty(name).GetString() == hash, Snapshot(value));
        }
        finally { value.Destroy(); }
    }

}
