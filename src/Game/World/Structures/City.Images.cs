#nullable disable warnings

using Aow.Game.Draw.Images;
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
        Production.Activate();
        StrategicMap? map = GetEngine().StrategicMap;
        if (Player is not null)
        {
            _ = map.Players.GetPlayers(Player).ProductionControls.Add(Production);
        }

        if (map.Status.Contains(HexSpriteMapStatuses.Editting) && map.Day == 0)
        {
            SetUpgradeLevel(UpgradeLevel);
        }

        UpdateImages();
    }

    // City.pas:1944 reads FWallType again through PChar(Self)[$4C].
    public void UpdateImages()
    {
        MapField? field = Owner;
        int terrain = field.TerrainNumber;
        int offset = terrain switch { 3 => 20, 5 or (>= 7 and <= 9) or 12 or 13 => 40, _ => 0 };
        CustomImageLibrary? images = Resource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        if (!GetRazed())
        {
            if (Race >= 0)
            {
                RaceResource? race = GetEngine().GamePack.RaceResources.GetRaceResource(Race);
                int size = GetResource<Resource>().Size;
                if (race is not null)
                {
                    CityImage = race.GetCityImage(size, terrain, level: field.HexLevel);
                }

                CityImage ??= images[images.ImageExist(offset) ? offset : 0];
            }
            else
            {
                int index = offset + 1;
                CityImage = images[images.ImageExist(index) ? index : 1];
            }
            if (WallType is not 0 and not 3)
            {
                int index = (WallType << 1) + offset;
                if (!images.ImageExist(index))
                {
                    index = WallType << 1;
                }

                WallFrontImage = images[index];
                WallBackImage = images[index + 1];
            }
            else
            {
                WallBackImage = null;
                WallFrontImage = null;
            }
        }
        else
        {
            CityImage = null;
            WallBackImage = null;
            WallFrontImage = null;
        }
        UpdateFlagId();
    }
}
