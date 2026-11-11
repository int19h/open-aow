// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic;

// (ID, source, upkeep, signed dispel resistance)
public partial class Enchantment : EngineObject
{
    // Spells.PAS:1692
    // ID, source, upkeep, dispel resistance
    public Enchantment(ECustomNode owner) : base(owner)
    {
        FDispelResistance = 10;
        FUpkeep = 1;
        FSource = -1;
        FEnchantmentID = 0;
    }

    public int FEnchantmentID;
    public sbyte FSource;
    public int FUpkeep;
    public sbyte FDispelResistance;

    // Spells.PAS:1726
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDispelResistance), 10);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FSource), 11);
        Stream.rwInteger(ref FUpkeep, 12);
        Stream.rwInteger(ref FEnchantmentID, 13);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.UnsignedByte(persister, "DispelResistance", ref FDispelResistance);
        PersistSupport.UnsignedByte(persister, "Source", ref FSource);
        persister.Int("Upkeep", ref FUpkeep);
        persister.Int("EnchantmentID", ref FEnchantmentID);
    }

    // Spells.PAS:1700
    public virtual string GetName() => AbilityRules.TranslateRStr(P4UndefinedRStr);

    // Spells.PAS:1702
    public virtual string GetBinding() => AbilityRules.TranslateRStr(GameTables.NoneRStr);

    // Spells.PAS:1734
    public virtual void SetUpkeep(int Value)
    {
        if (Value != FUpkeep)
        {
            FUpkeep = Value;
            if (FSource > -1)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FSource).FMagicControl.Update();
            }
        }
    }

    // Spells.PAS:1742
    public virtual void SetSource(sbyte Player)
    {
        if (Player != FSource)
        {
            if (FSource > -1)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FSource).FMagicControl.UnregisterEnchantment(this);
            }

            FSource = Player;
            if (FSource > -1)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FSource).FMagicControl.RegisterEnchantment(this);
            }
        }
    }

    // Spells.PAS:1759
    public virtual int GetAIUpkeepPriority() => 0;

    // Spells.PAS:1704
    public virtual void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = -1;
        Y = -1;
        L = -1;
    }

    // Spells.PAS:1706
    public virtual bool CenterToLocation()
    {
        bool Result;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        GetLocation(ref X, ref Y, ref L);
        if (X != -1)
        {
            if (AoWHSMap.Current.Visible(X, Y, L, 0))
            {
                AoWHSMap.Current.CenterViewTo(X, Y, L);
                AoWHSMap.Current.PlayCenterAnimation(X, Y, L);
            }
            else
            {
                var binding = GetBinding();
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XIsOutOfSightRStr), [binding]);
                AoWHSMap.Current.ShowMessage(format);
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:1751
    public virtual bool Dispel() => false;

    // Spells.PAS:1724
    public virtual void Select()
    {
        CenterToLocation();
    }

    // Spells.PAS:1753
    public int DispelChance(int Mana)
    {
        int Result = Mana - FDispelResistance * 10 + 50;
        if (Result < 10)
        {
            Result = 10;
        }

        if (Result > 90)
        {
            Result = 90;
        }

        return Result;
    }

    private static readonly string P4UndefinedRStr = "Undefined";

    // intf_P4_01.inc:28

    // intf_P4_01.inc:28

}
