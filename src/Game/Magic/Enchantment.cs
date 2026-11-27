#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic;

// (ID, source, upkeep, signed dispel resistance)
public class Enchantment(CustomNode owner) : EngineObject(owner)
{

    public int EnchantmentId { get; set; }
    public int Source { get; set; } = -1;
    public int Upkeep { get; set; } = 1;
    public int DispelResistance { get; set; } = 10;

    // Spells.PAS:1726
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        DispelResistance = persister.UnsignedByte(name: "DispelResistance", value: DispelResistance.TruncateToSByte());
        Source = persister.UnsignedByte(name: "Source", value: Source);
        Upkeep = persister.Int32("Upkeep", value: Upkeep);
        EnchantmentId = persister.Int32("EnchantmentId", value: EnchantmentId);
    }

    // Spells.PAS:1700
    public virtual string GetName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_undefinedText);

    // Spells.PAS:1702
    public virtual string GetBinding() => AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.NoneText);

    // Spells.PAS:1734
    public virtual void SetUpkeep(int value)
    {
        if (value != Upkeep)
        {
            Upkeep = value;
            if (Source > -1)
            {
                GetEngine().StrategicMap.Players.GetPlayers(Source).MagicControl.Update();
            }
        }
    }

    // Spells.PAS:1742
    public virtual void SetSource(PlayerId? player)
    {
        if (PlayerIdExtensions.ToOriginal(player) != Source)
        {
            if (Source > -1)
            {
                _ = GetEngine().StrategicMap.Players.GetPlayers(Source).MagicControl.UnregisterEnchantment(this);
            }

            Source = PlayerIdExtensions.ToOriginal(player);
            if (Source > -1)
            {
                _ = GetEngine().StrategicMap.Players.GetPlayers(Source).MagicControl.RegisterEnchantment(this);
            }
        }
    }

    // Spells.PAS:1759
    public virtual int AIUpkeepPriority => 0;

    // Spells.PAS:1704
    public virtual MapCoordinate GetLocation() => MapCoordinate.None;

    // Spells.PAS:1706
    public virtual bool CenterToLocation()
    {
        MapCoordinate coordinate22 = GetLocation();
        int x = coordinate22.X;
        int y = coordinate22.Y;
        int l = coordinate22.MapLevel;
        if (x != -1)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            if (strategicMap.Visible(x, y, l, radius: 0))
            {
                strategicMap.CenterViewTo(x, y, l);
                GetEngine().StrategicMap.PlayCenterAnimation(x, y, l);
            }
            else
            {
                string binding = GetBinding();
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.IsOutOfSightFormat), arguments: [binding]);
                GetEngine().StrategicMap.ShowMessage(format);
            }

            return true;
        }
        else
        {
            return false;
        }
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

        return result > 90 ? 90 : result;
    }

    private static readonly string s_undefinedText = "Undefined";
}
