// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Units;

// Race. HeroType. Name. Alignment. Hidden. base statistics. Abilities. FaceIndex
public partial class HeroResource : TroopResource
{
    public new const int ClassId = 131633;

    // Hero.PAS:2308
    public HeroResource(ECustomNode owner) : base(owner)
    {
        Abilities = new HeroResourceAbilityOwner(null);
        Attack = 3;
        Defense = 3;
        Damage = 3;
        Hits = 5;
        Moves = 28;
        Resistance = 2;
    }

    public string Name = "";
    public sbyte Race;
    public HeroType HeroType;
    public Alignment Alignment;
    public bool Hidden;
    public sbyte Attack;
    public sbyte Defense;
    public sbyte Damage;
    public sbyte Hits;
    public sbyte Moves;
    public sbyte Resistance;
    public AbilityOwner Abilities;
    public int FaceIndex;

    // Hero.PAS:2320
    public override void Dispose()
    {
        Abilities?.Dispose();
        base.Dispose();
    }

    // Hero.PAS:2341
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwString(ref Name, 10);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Race), 11);
        stream.rwData(Bytes.Of(ref HeroType, 1), 1, 12);
        stream.rwData(Bytes.Of(ref Alignment, 1), 1, 13);
        if (Alignment > Alignment.None)
        {
            Alignment = Alignment.Neutral;
        }

        stream.rwBool(ref Hidden, 14);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Attack), 15);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Defense), 16);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Damage), 17);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Hits), 18);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Moves), 19);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Resistance), 20);
        stream.rwEObject(Abilities, 29);
        stream.rwInteger(ref FaceIndex, 30);
    }

    // Hero.PAS:2326
    public override int ResourceUserClassId() => 131632;

    // Hero.PAS:2329
    public int HeroResourceIndex() => Owner.FindChildIndex(this);

    // Hero.PAS:2332
    public int UsedSkillPoints()
    {
        int result = Attack * 5 + Defense * 5 + Damage * 10 + Resistance * 5 + Hits * 5 + Moves * 2;
        for (int I = Abilities.GetAbCount() - 1; I >= 0; I--)
        {
            if (Abilities.GetAbilitySetAt(I))
            {
                result += AoWHSSet.Current.AbilityControl.GetAbility(I).GetSkillPoints(Abilities);
            }
        }

        return result;
    }

    // intf_P2_07.inc:343

}
