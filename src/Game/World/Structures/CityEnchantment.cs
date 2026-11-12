// Translated from City.pas.
using System.Collections.Immutable;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// _enchantmentId, EnchantmentLocation
public partial class CityEnchantment : GlobalEnchantment
{
    // City.pas:2722
    public new const int ClassId = 131820;

    public new CityEnchantmentId _enchantmentId;
    public MapCoordinate EnchantmentLocation;

    // City.pas:2727
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Bytes(Bytes.Of(ref _enchantmentId, 1), 1, 20);
        stream.Int32(ref EnchantmentLocation.Packed, 21);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _enchantmentId = persister.Enum("TCityEnchantment.EnchantmentID", _enchantmentId);
        EnchantmentLocation.Packed = persister.Int32("EnchantmentLocation", EnchantmentLocation.Packed);
    }

    // City.pas:2716
    public CityEnchantment(ECustomNode owner) : base(owner)
    {
        EnchantmentMode = EnchantmentMode - Set<GlobalEnchantmentModeElement>.Of(GlobalEnchantmentModeElement.TrueGlobal, GlobalEnchantmentModeElement.Owned);
    }

    // City.pas:2762
    public override string GetName() => AbilityRules.TranslateResourceString(s_cityEnchantmentIdTexts[(int)_enchantmentId]);

    // City.pas:2739
    public override MapCoordinate GetLocation()
    {
        MapCoordinate result = default;
        result.X = EnchantmentLocation.X;
        result.Y = EnchantmentLocation.Y;
        result.MapLevel = EnchantmentLocation.MapLevel;
        return result;
    }

    // City.pas:2767
    public override int GetAIUpkeepPriority()
    {
        int result;
        City city;
        city = GetCity();
        if (city != null)
        {
            result = city.AICityEnchantmentUpkeepPriority(_enchantmentId);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // City.pas:2746
    public override bool Deactivate(byte player)
    {
        bool result;
        if (EnchantmentStatus.Contains(GlobalEnchantmentStatusElement.Activated))
        {
            result = base.Deactivate(player);
            GetCity().DeactivateEnchantment(this);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // City.pas:2734
    public override bool VisibleForPlayer(sbyte player) => GetCity().VisibleForPlayer(player);

    // City.pas:2756
    public City GetCity() => (City)StrategicMap.Current.MapContainer.GetField(EnchantmentLocation.X, EnchantmentLocation.Y, EnchantmentLocation.MapLevel).FindNoneTransparentHexSprite(typeof(City));

    private static readonly ImmutableArray<string> s_cityEnchantmentIdTexts = ["Fountain Of Life", "Gold Rush", "Anti Magic Shell", "Warmonger"];

}
