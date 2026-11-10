using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public partial class City
{
    public override void Activate()
    {
        RegisterEnchantments();
        base.Activate();
        UpdateVisibleForSeatedPlayer();
        FProduction.Activate();
        var map = AoWHSMap.Current;
        if (FPlayer != -1) map.FPlayers.GetPlayers(FPlayer).FProductionControls.Add(FProduction);
        if (map.Status.Contains(HSMapStatusElement.Editting) && map.FDay == 0) SetUpgradeLevel(FUpgradeLevel);
        UpdateImages();
    }

    // City.pas:1944 reads FWallType again through PChar(Self)[$4C].
    public void UpdateImages()
    {
        var field = (MapField)Owner;
        int terrain = field.TerrainType;
        int offset = terrain switch { 3 => 20, 5 or >= 7 and <= 9 or 12 or 13 => 40, _ => 0 };
        var images = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        if (!GetRazed())
        {
            if (FRace >= 0)
            {
                var race = Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(FRace);
                int size = ((Resource)FResource).FSize;
                if (race != null) FCityImage = race.GetCityImage(size, terrain, field.Lhx);
                if (FCityImage == null) FCityImage = images[images.ImageExist(offset) ? offset : 0];
            }
            else
            {
                int index = offset + 1;
                FCityImage = images[images.ImageExist(index) ? index : 1];
            }
            if (FWallType != 0 && FWallType != 3)
            {
                int index = (FWallType << 1) + offset;
                if (!images.ImageExist(index)) index = FWallType << 1;
                FWallFrontImage = images[index];
                FWallBackImage = images[index + 1];
            }
            else { FWallBackImage = null; FWallFrontImage = null; }
        }
        else { FCityImage = null; FWallBackImage = null; FWallFrontImage = null; }
        UpdateFlagID();
    }
}
