// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic;

// (ID, source, upkeep, signed dispel resistance)
public partial class Enchantment : EngineObject
{
    public Enchantment(CustomNode owner) : base(owner)
    {
        DispelResistance = 10;
        Upkeep = 1;
        Source = -1;
        EnchantmentId = 0;
    }

    public int EnchantmentId { get; set; }
    public sbyte Source { get; set; }
    public int Upkeep { get; set; }
    public sbyte DispelResistance { get; set; }

    // Spells.PAS:1726
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        DispelResistance = PersistSupport.UnsignedByte(persister, name: "DispelResistance", value: DispelResistance);
        Source = PersistSupport.UnsignedByte(persister, name: "Source", value: Source);
        Upkeep = persister.Int32("Upkeep", value: Upkeep);
        EnchantmentId = persister.Int32("EnchantmentId", value: EnchantmentId);
    }

    // Spells.PAS:1700
    public virtual string GetName() => AbilityRules.TranslateResourceString(GetEngine(), s_undefinedText);

    // Spells.PAS:1702
    public virtual string GetBinding() => AbilityRules.TranslateResourceString(GetEngine(), GameTables.NoneText);

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
                _ = StrategicMap.Current.Players.GetPlayers(Source).MagicControl.UnregisterEnchantment(this);
            }

            Source = player;
            if (Source > -1)
            {
                _ = StrategicMap.Current.Players.GetPlayers(Source).MagicControl.RegisterEnchantment(this);
            }
        }
    }

    // Spells.PAS:1759
    public virtual int GetAIUpkeepPriority() => 0;

    // Spells.PAS:1704
    public virtual MapCoordinate GetLocation() => MapCoordinate.None;

    // Spells.PAS:1706
    public virtual bool CenterToLocation()
    {
        bool result;
        MapCoordinate coordinate22 = GetLocation();
        sbyte x = coordinate22.X;
        sbyte y = coordinate22.Y;
        sbyte l = coordinate22.MapLevel;
        if (x != -1)
        {
            if (StrategicMap.Current.Visible(x, y, l, 0))
            {
                StrategicMap.Current.CenterViewTo(x, y, l);
                StrategicMap.Current.PlayCenterAnimation(x, y, l);
            }
            else
            {
                string binding = GetBinding();
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.IsOutOfSightFormat), [binding]);
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
    public virtual void Select() => CenterToLocation();

    // Spells.PAS:1753
    public int DispelChance(int mana)
    {
        int result = mana - (DispelResistance * 10) + 50;
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
