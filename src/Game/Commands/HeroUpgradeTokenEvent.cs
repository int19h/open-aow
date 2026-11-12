// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// UnitId. Player. statistic upgrades. Abilities
public partial class HeroUpgradeTokenEvent : TokenEvent
{
    public new const int ClassId = 135965;

    public int UnitId;
    public sbyte Player;
    public sbyte Attack;
    public sbyte Defense;
    public sbyte Resistance;
    public sbyte Damage;
    public sbyte Moves;
    public sbyte Hits;

    public AbilityOwner Abilities { get; }

    // Hero.PAS:7
    public override void Dispose()
    {
        Abilities?.Dispose();
        base.Dispose();
    }

    // Hero.PAS:13
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Attack), 30);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Defense), 31);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Resistance), 32);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Damage), 33);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Moves), 34);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Hits), 35);
        stream.rwEObject(Abilities, 36);
        stream.rwInteger(ref UnitId, 37);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Player), 38);
    }

    // Hero.PAS:1
    public HeroUpgradeTokenEvent(ECustomNode owner) : base(owner)
    {
        Abilities = new AbilityOwner(null);
    }

    // Hero.PAS:30
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        Hero hero;
        ExecuteResult result = base.Execute(tokenEvent);
        hero = AoWHSMap.Current.UnitControl.FindUnit<Hero>(UnitId);
        if (hero != null && hero.GetHitPoints() != 0 && hero.Player == Player)
        {
            hero.SetUnitAttack((sbyte)(hero.UnitAttack + Attack));
            hero.SetUnitDefense((sbyte)(hero.UnitDefense + Defense));
            hero.SetUnitResistance((sbyte)(hero.UnitResistance + Resistance));
            hero.SetUnitDamage((sbyte)(hero.UnitDamage + Damage));
            hero.SetUnitMoves((sbyte)(hero.UnitMoves + Moves));
            hero.SetUnitHits((sbyte)(hero.UnitHits + Hits));
            hero.UpdateDefaultAbilities(Abilities);
            hero.UpgradePending = false;
        }

        return result;
    }

    // Hero.PAS:48
    public void Setup(Hero hero)
    {
        UnitId = hero.UnitId;
        Player = hero.Player;
    }

    // intf_P2_06.inc:62

}
