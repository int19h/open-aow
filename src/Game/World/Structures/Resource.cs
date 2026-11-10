// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

// FSize
public partial class Resource : PlayerCropStructureResource
{
    public Resource(ECustomNode owner) : base(owner)
    {
        FRazeable = true;
    }

    // City.pas:2628
    public new const int ClassID = 131337;

    public byte FSize;

    // City.pas:2622

    // City.pas:2669
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            ActivateMsg();
        }
    }

    // City.pas:2638
    public override int ResourceUserClassID() => 131336;

    // City.pas:2643
    public void ShowCity(int X, int Y, in SurfaceDesc SurfaceDesc, City City)
    {
        int dX;
        int dY;
        LibraryImage Image;
        dX = FdXImage + X;
        dY = Y;
        dY += FdYImage;
        Image = City.FWallBackImage;
        if (Image != null)
        {
            Image.Show(dX, dY, SurfaceDesc);
        }

        if (City.FCityImage != null)
        {
            // { Original blinks the selected city from frame bits. }
            if (City.GetSelected() && (AoWHSMap.Current.Frame & 6) == 6)
            {
                City.FCityImage.ShowTransparent(dX, dY, SurfaceDesc);
            }
            else
            {
                City.FCityImage.Show(dX, dY, SurfaceDesc);
            }
        }

        Image = City.FWallFrontImage;
        if (Image != null)
        {
            Image.Show(dX, dY, SurfaceDesc);
        }
    }

    // City.pas:2661
    public new void ActivateMsg()
    {
        FSize = 0;
        for (int I = 0, last = FTerrainList.Count - 1; I <= last; I++)
        {
            if (FTerrainList.Terrain[I] >= 0)
            {
                FSize = (byte)(FSize + 1);
            }
        }
    }

    // City.pas:262

    // City.pas:2633
    public override string ResourceEditName() => AbilityRules.TranslateRStr(CityRStr);

    private static readonly string CityRStr = "City";

}
