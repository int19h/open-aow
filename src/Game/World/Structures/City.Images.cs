
using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public partial class City
{
    public override void Activate()
    {
        RegisterEnchantments();
        base.Activate();
        UpdateVisibleForSeatedPlayer();
        Production.Activate();
        var map = AoWHSMap.Current;
        if (Player != -1) map.Players.GetPlayers(Player).ProductionControls.Add(Production);
        if (map.Status.Contains(HexSpriteMapStatusElement.Editting) && map.Day == 0) SetUpgradeLevel(UpgradeLevel);
        UpdateImages();
    }

    // City.pas:1944 reads FWallType again through PChar(Self)[$4C].
    public void UpdateImages()
    {
        var field = (MapField)Owner;
        int terrain = field.TerrainType;
        int offset = terrain switch { 3 => 20, 5 or >= 7 and <= 9 or 12 or 13 => 40, _ => 0 };
        var images = _resource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        if (!GetRazed())
        {
            if (Race >= 0)
            {
                var race = AoWHSSet.Current.RaceResources.GetRaceResource(Race);
                int size = ((Resource)_resource).Size;
                if (race != null) CityImage = race.GetCityImage(size, terrain, field.HexLevel);
                if (CityImage == null) CityImage = images[images.ImageExist(offset) ? offset : 0];
            }
            else
            {
                int index = offset + 1;
                CityImage = images[images.ImageExist(index) ? index : 1];
            }
            if (WallType != 0 && WallType != 3)
            {
                int index = (WallType << 1) + offset;
                if (!images.ImageExist(index)) index = WallType << 1;
                WallFrontImage = images[index];
                WallBackImage = images[index + 1];
            }
            else { WallBackImage = null; WallFrontImage = null; }
        }
        else { CityImage = null; WallBackImage = null; WallFrontImage = null; }
        UpdateFlagId();
    }
}
