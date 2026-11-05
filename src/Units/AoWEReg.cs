// Translated from AoWEReg.pas.
using static Aow.Units.ILTer.ILTer;
using static Aow.Units.HxBridge.HxBridge;
using static Aow.Units.AoWHex.AoWHex;
using static Aow.Units.AoWRoad.AoWRoad;
using static Aow.Units.RndHex.RndHex;
using static Aow.Units.AoWTer.AoWTer;
using static Aow.Units.Mountain.Mountain;
using static Aow.Units.Spring.Spring;
using static Aow.Units.Bridge.Bridge;
using static Aow.Units.Sign.Sign;
using static Aow.Units.Teleport.Teleport;
using static Aow.Units.CitySpells.CitySpells;
using static Aow.Units.Farm.Farm;
using static Aow.Units.Cave.Cave;
using static Aow.Units.Mine.Mine;
using static Aow.Units.ManaCrystals.ManaCrystals;
using static Aow.Units.FireNode.FireNode;
using static Aow.Units.EarthNode.EarthNode;
using static Aow.Units.DeathNode.DeathNode;
using static Aow.Units.LifeNode.LifeNode;
using static Aow.Units.WaterNode.WaterNode;
using static Aow.Units.AirNode.AirNode;
using static Aow.Units.MonsterL.MonsterL;
using static Aow.Units.Tower.Tower;
using static Aow.Units.PoisonP.PoisonP;
using static Aow.Units.Gems.Gems;
using static Aow.Units.Dungeon.Dungeon;
using static Aow.Units.Crypt.Crypt;
using static Aow.Units.Ruin.Ruin;
using static Aow.Units.Pyramid.Pyramid;
using static Aow.Units.Shipyard.Shipyard;
using static Aow.Units.HolyG.HolyG;
using static Aow.Units.UnHolyG.UnHolyG;
using static Aow.Units.Merchant.Merchant;
using static Aow.Units.PrdPlace.PrdPlace;
using static Aow.Units.BldGuild.BldGuild;
using static Aow.Units.AltFire.AltFire;
using static Aow.Units.AltIce.AltIce;
using static Aow.Units.PowerNode.PowerNode;
using static Aow.Units.FlagEvents.FlagEvents;
using static Aow.Units.AltLight.AltLight;
using static Aow.Units.AltLife.AltLife;
using static Aow.Units.AltDeath.AltDeath;
using static Aow.Units.AltPest.AltPest;
using static Aow.Units.MoveOnEvents.MoveOnEvents;
using static Aow.Units.OverlayObject.OverlayObject;
using static Aow.Units.MagicSanctuary.MagicSanctuary;
using static Aow.Units.ReflectingPool.ReflectingPool;
using static Aow.Units.WizardsTower.WizardsTower;
using static Aow.Units.Arena.Arena;
using static Aow.Units.Crops.Crops;
using static Aow.Units.Pad.Pad;
using static Aow.Units.AoWSetup.AoWSetup;
using static Aow.Units.ExploreS.ExploreS;
using static Aow.Units.ItemExpS.ItemExpS;
using static Aow.Units.BloodShrine.BloodShrine;
using static Aow.Units.Storms.Storms;
using static Aow.Units.PoisonC.PoisonC;
using static Aow.Units.SummonSpells.SummonSpells;
using static Aow.Units.UnitSpells.UnitSpells;
using static Aow.Units.GlobalSpells.GlobalSpells;
using static Aow.Units.SmokeCloud.SmokeCloud;
using static Aow.Units.GlobalTargetSpells.GlobalTargetSpells;
using static Aow.Units.GlobalEnchantmentSpells.GlobalEnchantmentSpells;
using static Aow.Units.CombatSpells.CombatSpells;
using static Aow.Units.PassiveAb.PassiveAb;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AoWEReg;

public static partial class AoWEReg
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
