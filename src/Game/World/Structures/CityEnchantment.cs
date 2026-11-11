// Translated from City.pas.
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// FEnchantmentID, FEnchantmentLocation
public partial class CityEnchantment : GlobalEnchantment
{
    // City.pas:2722
    public new const int ClassID = 131820;

    public new CityEnchantmentId FEnchantmentID;
    public MapCoordinate FEnchantmentLocation;

    // City.pas:2727
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref FEnchantmentID, 1), 1, 20);
        Stream.rwInteger(ref FEnchantmentLocation.Packed, 21);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Enum("TCityEnchantment.EnchantmentID", ref FEnchantmentID);
        persister.Int("EnchantmentLocation", ref FEnchantmentLocation.Packed);
    }

    // City.pas:2716
    public CityEnchantment(ECustomNode owner) : base(owner)
    {
        FEnchantmentMode = FEnchantmentMode - Set<GlobalEnchantmentModeElement>.Of(GlobalEnchantmentModeElement.TrueGlobal, GlobalEnchantmentModeElement.Owned);
    }

    // City.pas:2762
    public override string GetName() => AbilityRules.TranslateRStr(TCityEnchantmentIDRStr[(int)FEnchantmentID]);

    // City.pas:2739
    public override MapCoordinate GetLocation()
    {
        MapCoordinate Result = default;
        Result.X = FEnchantmentLocation.X;
        Result.Y = FEnchantmentLocation.Y;
        Result.L = FEnchantmentLocation.L;
        return Result;
    }

    // City.pas:2767
    public override int GetAIUpkeepPriority()
    {
        int Result;
        City City;
        City = GetCity();
        if (City != null)
        {
            Result = City.AICityEnchantmentUpkeepPriority(FEnchantmentID);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // City.pas:2746
    public override bool Deactivate(byte Player)
    {
        bool Result;
        if (FEnchantmentStatus.Contains(GlobalEnchantmentStatusElement.Activated))
        {
            Result = base.Deactivate(Player);
            GetCity().DeactivateEnchantment(this);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // City.pas:2734
    public override bool VisibleForPlayer(sbyte Player) => GetCity().VisibleForPlayer(Player);

    // City.pas:2756
    public City GetCity() => (City)AoWHSMap.Current.MapContainer.GetField(FEnchantmentLocation.X, FEnchantmentLocation.Y, FEnchantmentLocation.L).FindNoneTransparentHS(typeof(City));

    private static StringArray4 TCityEnchantmentIDRStr = StringArray4.From(["Fountain Of Life", "Gold Rush", "Anti Magic Shell", "Warmonger"]);

}
