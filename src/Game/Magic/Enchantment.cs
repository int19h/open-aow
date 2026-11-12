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
        DispelResistance = 10;
        Upkeep = 1;
        Source = -1;
        EnchantmentId = 0;
    }

    public int EnchantmentId;
    public sbyte Source;
    public int Upkeep;
    public sbyte DispelResistance;

    // Spells.PAS:1726
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref DispelResistance), 10);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Source), 11);
        stream.rwInteger(ref Upkeep, 12);
        stream.rwInteger(ref EnchantmentId, 13);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.UnsignedByte(persister, "DispelResistance", ref DispelResistance);
        PersistSupport.UnsignedByte(persister, "Source", ref Source);
        persister.Int32("Upkeep", ref Upkeep);
        persister.Int32("EnchantmentID", ref EnchantmentId);
    }

    // Spells.PAS:1700
    public virtual string GetName() => AbilityRules.TranslateResourceString(s_undefinedText);

    // Spells.PAS:1702
    public virtual string GetBinding() => AbilityRules.TranslateResourceString(GameTables.NoneText);

    // Spells.PAS:1734
    public virtual void SetUpkeep(int value)
    {
        if (value != Upkeep)
        {
            Upkeep = value;
            if (Source > -1)
            {
                StrategicMap.Current.Players.GetPlayers(Source).MagicControl.Update();
            }
        }
    }

    // Spells.PAS:1742
    public virtual void SetSource(sbyte player)
    {
        if (player != Source)
        {
            if (Source > -1)
            {
                StrategicMap.Current.Players.GetPlayers(Source).MagicControl.UnregisterEnchantment(this);
            }

            Source = player;
            if (Source > -1)
            {
                StrategicMap.Current.Players.GetPlayers(Source).MagicControl.RegisterEnchantment(this);
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
        bool result;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        GetLocation(ref X, ref Y, ref L);
        if (X != -1)
        {
            if (StrategicMap.Current.Visible(X, Y, L, 0))
            {
                StrategicMap.Current.CenterViewTo(X, Y, L);
                StrategicMap.Current.PlayCenterAnimation(X, Y, L);
            }
            else
            {
                var binding = GetBinding();
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.IsOutOfSightFormat), [binding]);
                StrategicMap.Current.ShowMessage(format);
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:1751
    public virtual bool Dispel() => false;

    // Spells.PAS:1724
    public virtual void Select()
    {
        CenterToLocation();
    }

    // Spells.PAS:1753
    public int DispelChance(int mana)
    {
        int result = mana - DispelResistance * 10 + 50;
        if (result < 10)
        {
            result = 10;
        }

        if (result > 90)
        {
            result = 90;
        }

        return result;
    }

    private static readonly string s_undefinedText = "Undefined";

    // intf_P4_01.inc:28

    // intf_P4_01.inc:28

}
