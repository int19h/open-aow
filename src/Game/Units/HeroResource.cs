// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Units;

// FRace. FHeroType. FName. FAlignment. FHidden. base statistics. FAbilities. FFaceIndex
public partial class HeroResource : AbstractUnitResource
{
    public new const int ClassID = 131633;

    // Hero.PAS:2308
    public HeroResource(ECustomNode owner) : base(owner)
    {
        FAbilities = new HeroResourceAbilityOwner(null);
        FAttack = 3;
        FDefense = 3;
        FDamage = 3;
        FHits = 5;
        FMoves = 28;
        FResistance = 2;
    }

    public string FName = "";
    public sbyte FRace;
    public HeroType FHeroType;
    public Alignment FAlignment;
    public bool FHidden;
    public sbyte FAttack;
    public sbyte FDefense;
    public sbyte FDamage;
    public sbyte FHits;
    public sbyte FMoves;
    public sbyte FResistance;
    public AbilityOwner FAbilities;
    public int FFaceIndex;

    // Hero.PAS:2320
    public override void Dispose()
    {
        FAbilities?.Dispose();
        base.Dispose();
    }

    // Hero.PAS:2341
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwString(ref FName, 10);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FRace), 11);
        Stream.rwData(Bytes.Of(ref FHeroType, 1), 1, 12);
        Stream.rwData(Bytes.Of(ref FAlignment, 1), 1, 13);
        if (FAlignment > Alignment.None)
        {
            FAlignment = Alignment.Neutral;
        }

        Stream.rwBool(ref FHidden, 14);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FAttack), 15);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDefense), 16);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDamage), 17);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FHits), 18);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FMoves), 19);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FResistance), 20);
        Stream.rwEObject(FAbilities, 29);
        Stream.rwInteger(ref FFaceIndex, 30);
    }

    // Hero.PAS:2326
    public override int ResourceUserClassID() => 131632;

    // Hero.PAS:2329
    public int HeroResourceIndex() => Owner.FindChildIndex(this);

    // Hero.PAS:2332
    public int UsedSkillPoints()
    {
        int Result = FAttack * 5 + FDefense * 5 + FDamage * 10 + FResistance * 5 + FHits * 5 + FMoves * 2;
        for (int I = FAbilities.GetAbCount() - 1; I >= 0; I--)
        {
            if (FAbilities.GetAbSet(I))
            {
                Result += AoWHSSet.Current.FAbilityControl.GetAbility(I).GetSkillPoints(FAbilities);
            }
        }

        return Result;
    }

    // intf_P2_07.inc:343

}
