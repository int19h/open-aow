namespace Aow.Units.AoWE;

public static partial class AoWE
{
    static AoWE()
    {
        TMagicSphereRStr[(int)TMagicSphere.msCosmos] = CosmosRStr;
        TMagicSphereRStr[(int)TMagicSphere.msLife] = LifeRStr;
        TMagicSphereRStr[(int)TMagicSphere.msDeath] = DeathRStr;
        TMagicSphereRStr[(int)TMagicSphere.msEarth] = EarthRStr;
        TMagicSphereRStr[(int)TMagicSphere.msAir] = AirRStr;
        TMagicSphereRStr[(int)TMagicSphere.msFire] = FireRStr;
        TMagicSphereRStr[(int)TMagicSphere.msWater] = WaterRStr;
        TMagicSphereRStr[(int)TMagicSphere.msSecret] = SecretRStr;
        TMagicSphereRStr[(int)TMagicSphere.msNone] = NoneRStr;
        TStatusRStr[(int)TStatus.sHostile] = HostileRStr;
        TStatusRStr[(int)TStatus.sWary] = WaryRStr;
        TStatusRStr[(int)TStatus.sNeutral] = NeutralRStr;
        TStatusRStr[(int)TStatus.sPolite] = PoliteRStr;
        TStatusRStr[(int)TStatus.sFriendly] = FriendlyRStr;
    }
}
