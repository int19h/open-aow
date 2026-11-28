#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Structures;

// Size
public class Resource : PlayerCropStructureResource
{
    public Resource(CustomNode owner) : base(owner) => Razeable = true;

    // City.pas:2628
    public new const int ClassId = 131_337;

    public int Size { get; private set; }

    // City.pas:2669
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            ActivateMessage();
        }
    }

    // City.pas:2638
    public override int ResourceUserClassId() => City.ClassId;

    // City.pas:2643
    public void ShowCity(int X, int Y, in LockedSurface lockedSurface, City city)
    {
        int dX = ImageOffsetX + X;
        int dY = Y;
        dY += ImageOffsetY;
        LibraryImage image = city.WallBackImage;
        image?.Show(dX, dY, lockedSurface);

        if (city.CityImage is not null)
        {
            // { Original blinks the selected city from frame bits. }
            if (city.GetSelected() && (GetEngine().StrategicMap.Frame & 0b0110) == 0b0110)
            {
                city.CityImage.ShowTransparent(dX, dY, lockedSurface);
            }
            else
            {
                city.CityImage.Show(dX, dY, lockedSurface);
            }
        }

        image = city.WallFrontImage;
        image?.Show(dX, dY, lockedSurface);
    }

    // City.pas:2661
    public new void ActivateMessage()
    {
        Size = 0;
        for (int i = 0, last = TerrainList.Count - 1; i <= last; i++)
        {
            if (TerrainList.Terrain[i].Value is not null)
            {
                Size = (Size + 1).TruncateToByte();
            }
        }
    }

    // City.pas:2633
    public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_cityText);

    private static readonly string s_cityText = "City";

}
