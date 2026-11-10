// Translated from AoWEReg.pas.
using static Aow.Game.Draw.Animation.SpringUnit;
using static Aow.Game.Draw.Animation.StormsUnit;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Magic.Abilities.Passive.PassiveAbUnit;
using static Aow.Game.Magic.Spells.City.CitySpellsUnit;
using static Aow.Game.Magic.Spells.Combat.CombatSpellsUnit;
using static Aow.Game.Magic.Spells.Global.GlobalSpellsUnit;
using static Aow.Game.Magic.Spells.GlobalEnchantment.GlobalEnchantmentSpellsUnit;
using static Aow.Game.Magic.Spells.GlobalTarget.GlobalTargetSpellsUnit;
using static Aow.Game.Magic.Spells.MountainUnit;
using static Aow.Game.Magic.Spells.SmokeCloudUnit;
using static Aow.Game.Magic.Spells.Summon.SummonSpellsUnit;
using static Aow.Game.Magic.Spells.Unit.UnitSpellsUnit;
using static Aow.Game.Session.AoWSetupUnit;
using static Aow.Game.World.BridgeUnit;
using static Aow.Game.World.FlagEventsUnit;
using static Aow.Game.World.Hexes.AoWHexUnit;
using static Aow.Game.World.Hexes.AoWRoadUnit;
using static Aow.Game.World.Hexes.AoWTerUnit;
using static Aow.Game.World.Hexes.HxBridgeUnit;
using static Aow.Game.World.Hexes.RndHexUnit;
using static Aow.Game.World.Movement.MoveOnEventsUnit;
using static Aow.Game.World.PoisonCUnit;
using static Aow.Game.World.Structures.AirNodeUnit;
using static Aow.Game.World.Structures.AltDeathUnit;
using static Aow.Game.World.Structures.AltFireUnit;
using static Aow.Game.World.Structures.AltIceUnit;
using static Aow.Game.World.Structures.AltLifeUnit;
using static Aow.Game.World.Structures.AltLightUnit;
using static Aow.Game.World.Structures.AltPestUnit;
using static Aow.Game.World.Structures.ArenaUnit;
using static Aow.Game.World.Structures.BldGuildUnit;
using static Aow.Game.World.Structures.BloodShrineUnit;
using static Aow.Game.World.Structures.CryptUnit;
using static Aow.Game.World.Structures.DeathNodeUnit;
using static Aow.Game.World.Structures.DungeonUnit;
using static Aow.Game.World.Structures.EarthNodeUnit;
using static Aow.Game.World.Structures.ExploreSUnit;
using static Aow.Game.World.Structures.FarmUnit;
using static Aow.Game.World.Structures.FireNodeUnit;
using static Aow.Game.World.Structures.ItemExpSUnit;
using static Aow.Game.World.Structures.LifeNodeUnit;
using static Aow.Game.World.Structures.MagicSanctuaryUnit;
using static Aow.Game.World.Structures.MerchantUnit;
using static Aow.Game.World.Structures.MineUnit;
using static Aow.Game.World.Structures.MonsterLUnit;
using static Aow.Game.World.Structures.PowerNodeUnit;
using static Aow.Game.World.Structures.PrdPlaceUnit;
using static Aow.Game.World.Structures.PyramidUnit;
using static Aow.Game.World.Structures.ReflectingPoolUnit;
using static Aow.Game.World.Structures.RuinUnit;
using static Aow.Game.World.Structures.ShipyardUnit;
using static Aow.Game.World.Structures.SignUnit;
using static Aow.Game.World.Structures.TeleportUnit;
using static Aow.Game.World.Structures.TowerUnit;
using static Aow.Game.World.Structures.WaterNodeUnit;
using static Aow.Game.World.Structures.WizardsTowerUnit;
using static Aow.Game.World.Terrain.CaveUnit;
using static Aow.Game.World.Terrain.CropsUnit;
using static Aow.Game.World.Terrain.GemsUnit;
using static Aow.Game.World.Terrain.HolyGUnit;
using static Aow.Game.World.Terrain.ILTerUnit;
using static Aow.Game.World.Terrain.ManaCrystalsUnit;
using static Aow.Game.World.Terrain.OverlayObjectUnit;
using static Aow.Game.World.Terrain.PadUnit;
using static Aow.Game.World.Terrain.PoisonPUnit;
using static Aow.Game.World.Terrain.UnHolyGUnit;

namespace Aow.Game.Engine;

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
