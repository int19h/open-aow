// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

// Size
public partial class Resource : PlayerCropStructureResource
{
    public Resource(ECustomNode owner) : base(owner)
    {
        Razeable = true;
    }

    // City.pas:2628
    public new const int ClassId = 131337;

    public byte Size { get; private set; }

    // City.pas:2622

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
    public override int ResourceUserClassId() => 131336;

    // City.pas:2643
    public void ShowCity(int X, int Y, in LockedSurface lockedSurface, City city)
    {
        int dX;
        int dY;
        LibraryImage image;
        dX = _imageOffsetX + X;
        dY = Y;
        dY += _imageOffsetY;
        image = city.WallBackImage;
        if (image != null)
        {
            image.Show(dX, dY, lockedSurface);
        }

        if (city.CityImage != null)
        {
            // { Original blinks the selected city from frame bits. }
            if (city.GetSelected() && (AoWHSMap.Current.Frame & 6) == 6)
            {
                city.CityImage.ShowTransparent(dX, dY, lockedSurface);
            }
            else
            {
                city.CityImage.Show(dX, dY, lockedSurface);
            }
        }

        image = city.WallFrontImage;
        if (image != null)
        {
            image.Show(dX, dY, lockedSurface);
        }
    }

    // City.pas:2661
    public new void ActivateMessage()
    {
        Size = 0;
        for (int I = 0, last = TerrainList.Count - 1; I <= last; I++)
        {
            if (TerrainList.Terrain[I] >= 0)
            {
                Size = (byte)(Size + 1);
            }
        }
    }

    // City.pas:262

    // City.pas:2633
    public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_cityText);

    private static readonly string s_cityText = "City";

}
