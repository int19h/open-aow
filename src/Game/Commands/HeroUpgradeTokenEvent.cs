// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// FUnitID. FPlayer. statistic upgrades. FAbilities
public partial class HeroUpgradeTokenEvent : TokenEvent
{
    public new const int ClassID = 135965;

    public int FUnitID;
    public sbyte FPlayer;
    public sbyte FAttack;
    public sbyte FDefense;
    public sbyte FResistance;
    public sbyte FDamage;
    public sbyte FMoves;
    public sbyte FHits;
    public AbilityOwner FAbilities;

    // Hero.PAS:7
    public override void Dispose()
    {
        FAbilities?.Dispose();
        base.Dispose();
    }

    // Hero.PAS:13
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FAttack), 30);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDefense), 31);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FResistance), 32);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDamage), 33);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FMoves), 34);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FHits), 35);
        Stream.rwEObject(FAbilities, 36);
        Stream.rwInteger(ref FUnitID, 37);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 38);
    }

    // Hero.PAS:1
    public HeroUpgradeTokenEvent(ECustomNode owner) : base(owner)
    {
        FAbilities = new AbilityOwner(null);
    }

    // Hero.PAS:30
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        Hero Hero;
        ExecuteResult Result = base.Execute(TE);
        Hero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(FUnitID);
        if (Hero != null && Hero.GetHitPoints() != 0 && Hero.FPlayer == FPlayer)
        {
            Hero.SetUnitAttack((sbyte)(Hero.FUnitAttack + FAttack));
            Hero.SetUnitDefense((sbyte)(Hero.FUnitDefense + FDefense));
            Hero.SetUnitResistance((sbyte)(Hero.FUnitResistance + FResistance));
            Hero.SetUnitDamage((sbyte)(Hero.FUnitDamage + FDamage));
            Hero.SetUnitMoves((sbyte)(Hero.FUnitMoves + FMoves));
            Hero.SetUnitHits((sbyte)(Hero.FUnitHits + FHits));
            Hero.UpdateDefaultAbilities(FAbilities);
            Hero.FUpgradePending = false;
        }

        return Result;
    }

    // Hero.PAS:48
    public void Setup(Hero Hero)
    {
        FUnitID = Hero.FUnitID;
        FPlayer = Hero.FPlayer;
    }

    // intf_P2_06.inc:62

}
