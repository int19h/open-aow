// Translated from AoWEReg.pas.
using static Aow.Units.ILTer.ILTerUnit;
using static Aow.Units.HxBridge.HxBridgeUnit;
using static Aow.Units.AoWHex.AoWHexUnit;
using static Aow.Units.AoWRoad.AoWRoadUnit;
using static Aow.Units.RndHex.RndHexUnit;
using static Aow.Units.AoWTer.AoWTerUnit;
using static Aow.Units.Mountain.MountainUnit;
using static Aow.Units.Spring.SpringUnit;
using static Aow.Units.Bridge.BridgeUnit;
using static Aow.Units.Sign.SignUnit;
using static Aow.Units.Teleport.TeleportUnit;
using static Aow.Units.CitySpells.CitySpellsUnit;
using static Aow.Units.Farm.FarmUnit;
using static Aow.Units.Cave.CaveUnit;
using static Aow.Units.Mine.MineUnit;
using static Aow.Units.ManaCrystals.ManaCrystalsUnit;
using static Aow.Units.FireNode.FireNodeUnit;
using static Aow.Units.EarthNode.EarthNodeUnit;
using static Aow.Units.DeathNode.DeathNodeUnit;
using static Aow.Units.LifeNode.LifeNodeUnit;
using static Aow.Units.WaterNode.WaterNodeUnit;
using static Aow.Units.AirNode.AirNodeUnit;
using static Aow.Units.MonsterL.MonsterLUnit;
using static Aow.Units.Tower.TowerUnit;
using static Aow.Units.PoisonP.PoisonPUnit;
using static Aow.Units.Gems.GemsUnit;
using static Aow.Units.Dungeon.DungeonUnit;
using static Aow.Units.Crypt.CryptUnit;
using static Aow.Units.Ruin.RuinUnit;
using static Aow.Units.Pyramid.PyramidUnit;
using static Aow.Units.Shipyard.ShipyardUnit;
using static Aow.Units.HolyG.HolyGUnit;
using static Aow.Units.UnHolyG.UnHolyGUnit;
using static Aow.Units.Merchant.MerchantUnit;
using static Aow.Units.PrdPlace.PrdPlaceUnit;
using static Aow.Units.BldGuild.BldGuildUnit;
using static Aow.Units.AltFire.AltFireUnit;
using static Aow.Units.AltIce.AltIceUnit;
using static Aow.Units.PowerNode.PowerNodeUnit;
using static Aow.Units.FlagEvents.FlagEventsUnit;
using static Aow.Units.AltLight.AltLightUnit;
using static Aow.Units.AltLife.AltLifeUnit;
using static Aow.Units.AltDeath.AltDeathUnit;
using static Aow.Units.AltPest.AltPestUnit;
using static Aow.Units.MoveOnEvents.MoveOnEventsUnit;
using static Aow.Units.OverlayObject.OverlayObjectUnit;
using static Aow.Units.MagicSanctuary.MagicSanctuaryUnit;
using static Aow.Units.ReflectingPool.ReflectingPoolUnit;
using static Aow.Units.WizardsTower.WizardsTowerUnit;
using static Aow.Units.Arena.ArenaUnit;
using static Aow.Units.Crops.CropsUnit;
using static Aow.Units.Pad.PadUnit;
using static Aow.Units.AoWSetup.AoWSetupUnit;
using static Aow.Units.ExploreS.ExploreSUnit;
using static Aow.Units.ItemExpS.ItemExpSUnit;
using static Aow.Units.BloodShrine.BloodShrineUnit;
using static Aow.Units.Storms.StormsUnit;
using static Aow.Units.PoisonC.PoisonCUnit;
using static Aow.Units.SummonSpells.SummonSpellsUnit;
using static Aow.Units.UnitSpells.UnitSpellsUnit;
using static Aow.Units.GlobalSpells.GlobalSpellsUnit;
using static Aow.Units.SmokeCloud.SmokeCloudUnit;
using static Aow.Units.GlobalTargetSpells.GlobalTargetSpellsUnit;
using static Aow.Units.GlobalEnchantmentSpells.GlobalEnchantmentSpellsUnit;
using static Aow.Units.CombatSpells.CombatSpellsUnit;
using static Aow.Units.PassiveAb.PassiveAbUnit;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.AoWEReg;

public static partial class AoWERegUnit
{
    public static void Initialize()
    {
        // { AoWEReg, from AoWEPACK.dpl: package initialization registers the game types. }
        if (ValidateAoWEngine())
        {
            RegisterAoWHexagon(AoWEngine);
            RegisterAoWTerrain(AoWEngine);
            RegisterRandomHexagon(AoWEngine);
            RegisterAoWRoad(AoWEngine);
            RegisterHexagonBridge(AoWEngine);
            RegisterMountain(AoWEngine);
            RegisterBridge(AoWEngine);
            RegisterSpringOfLife(AoWEngine);
            RegisterBloodShrine(AoWEngine);
            RegisterSign(AoWEngine);
            RegisterTeleport(AoWEngine);
            RegisterPad(AoWEngine);
            RegisterILTerrain(AoWEngine);
            RegisterCrops(AoWEngine);
            RegisterFarm(AoWEngine);
            RegisterCave(AoWEngine);
            RegisterMine(AoWEngine);
            RegisterTower(AoWEngine);
            RegisterPoisonPlant(AoWEngine);
            RegisterGems(AoWEngine);
            RegisterProductionPlace(AoWEngine);
            RegisterFireNode(AoWEngine);
            RegisterWaterNode(AoWEngine);
            RegisterDeathNode(AoWEngine);
            RegisterLifeNode(AoWEngine);
            RegisterEarthNode(AoWEngine);
            RegisterAirNode(AoWEngine);
            RegisterExplorationSite(AoWEngine);
            RegisterMonsterlair(AoWEngine);
            RegisterDungeon(AoWEngine);
            RegisterCrypt(AoWEngine);
            RegisterRuin(AoWEngine);
            RegisterPyramid(AoWEngine);
            RegisterShipyard(AoWEngine);
            RegisterHolyGround(AoWEngine);
            RegisterUnHolyGround(AoWEngine);
            RegisterMerchant(AoWEngine);
            RegisterAltarOfFire(AoWEngine);
            RegisterAltarOfIce(AoWEngine);
            RegisterAltarOfLightning(AoWEngine);
            RegisterAltarOfLife(AoWEngine);
            RegisterAltarOfDeath(AoWEngine);
            RegisterAltarOfPestilence(AoWEngine);
            RegisterBuildersGuild(AoWEngine);
            RegisterItemExplorationSite(AoWEngine);
            RegisterSetupClasses(AoWEngine);
            RegisterStorms(AoWEngine);
            RegisterPoisonCloud(AoWEngine);
            RegisterCitySpells(AoWEngine);
            RegisterPowerNode(AoWEngine);
            RegisterManaCrystals(AoWEngine);
            RegisterGlobalEnchantmentSpells(AoWEngine);
            RegisterGlobalSpells(AoWEngine);
            RegisterGlobalTargetSpells(AoWEngine);
            RegisterCombatSpells(AoWEngine);
            RegisterSummonSpells(AoWEngine);
            RegisterPassiveAbilities(AoWEngine.FGameSet.FAbilityControl);
            RegisterUnitSpells(AoWEngine);
            RegisterMoveOnEvents(AoWEngine);
            RegisterMagicSanctuary(AoWEngine);
            RegisterArena(AoWEngine);
            RegisterReflectingPool(AoWEngine);
            RegisterWizardsTower(AoWEngine);
            RegisterFlagEvents(AoWEngine);
            RegisterOverlayObject(AoWEngine);
            RegisterSmokeCloud(AoWEngine);
        }
    }

    public static void Finalize()
    {
    }
}
