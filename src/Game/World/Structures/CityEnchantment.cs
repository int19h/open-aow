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

// CityEnchantmentId, EnchantmentLocation
public partial class CityEnchantment : GlobalEnchantment
{
    // City.pas:2722
    public new const int ClassId = 131820;

    public CityEnchantmentId CityEnchantmentId;
    public MapCoordinate EnchantmentLocation;

    // The fourth byte of the packed location stored at id 21, 0 from every writer and in every
    // corpus file, kept so that a loaded value saves back unchanged.
    public byte UnknownBytes { get; set; }

    // City.pas:2727
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Bytes(Bytes.Of(ref CityEnchantmentId, 1), 1, 20);
        int location = StoredCoordinateImage.From(EnchantmentLocation, unknownBytes: UnknownBytes).ToInt32();
        stream.Int32(ref location, 21);
        (EnchantmentLocation, UnknownBytes) = StoredCoordinateImage.FromInt32(location);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        CityEnchantmentId = persister.Enum("CityEnchantmentId", value: CityEnchantmentId);
        int location = StoredCoordinateImage.From(EnchantmentLocation, unknownBytes: UnknownBytes).ToInt32();
        location = persister.Int32("EnchantmentLocation", value: location);
        (EnchantmentLocation, UnknownBytes) = StoredCoordinateImage.FromInt32(location);
    }

    // City.pas:2716
    public CityEnchantment(ECustomNode owner) : base(owner)
    {
        EnchantmentMode = EnchantmentMode - Set<GlobalEnchantmentModeElement>.Of(GlobalEnchantmentModeElement.TrueGlobal, GlobalEnchantmentModeElement.Owned);
    }

    // City.pas:2762
    public override string GetName() => AbilityRules.TranslateResourceString(GetEngine(), s_cityEnchantmentIdTexts[(int)CityEnchantmentId]);

    // City.pas:2739
    public override MapCoordinate GetLocation()
    {
        MapCoordinate result = default;
        result = new MapCoordinate(X: EnchantmentLocation.X, Y: EnchantmentLocation.Y, MapLevel: EnchantmentLocation.MapLevel);
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
            result = city.AICityEnchantmentUpkeepPriority(CityEnchantmentId);
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
