// Translated from City.pas.
using System.Collections.Immutable;

using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// CityEnchantmentId, EnchantmentLocation
public partial class CityEnchantment : GlobalEnchantment
{
    // City.pas:2722
    public new const int ClassId = 131820;

    public CityEnchantmentId CityEnchantmentId { get; set; }

    public MapCoordinate EnchantmentLocation { get; internal set; }

    // City.pas:2727
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        CityEnchantmentId = persister.Enum("CityEnchantmentId", value: CityEnchantmentId);
        int location = persister.Int32("EnchantmentLocation", value: StoredCoordinateImage.From(EnchantmentLocation).ToInt32());
        EnchantmentLocation = StoredCoordinateImage.FromInt32(location).Location;
    }

    // City.pas:2716
    public CityEnchantment(CustomNode owner) : base(owner) => EnchantmentMode -= Set<GlobalEnchantmentModeElement>.Of(GlobalEnchantmentModeElement.TrueGlobal, GlobalEnchantmentModeElement.Owned);

    // City.pas:2762
    public override string GetName() => AbilityRules.TranslateResourceString(GetEngine(), s_cityEnchantmentIdTexts[(int)CityEnchantmentId]);

    // City.pas:2739
    public override MapCoordinate GetLocation()
    {
        MapCoordinate result = new(X: EnchantmentLocation.X, Y: EnchantmentLocation.Y, MapLevel: EnchantmentLocation.MapLevel);
        return result;
    }

    // City.pas:2767
    public override int GetAIUpkeepPriority()
    {
        City city = GetCity();
        int result = city is not null ? city.AICityEnchantmentUpkeepPriority(CityEnchantmentId) : 0;

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
    public City GetCity() => StrategicMap.Current.MapContainer.GetField(EnchantmentLocation.X, EnchantmentLocation.Y, EnchantmentLocation.MapLevel).FindNoneTransparentHexSprite<City>();

    private static readonly ImmutableArray<string> s_cityEnchantmentIdTexts = ["Fountain Of Life", "Gold Rush", "Anti Magic Shell", "Warmonger"];

}
