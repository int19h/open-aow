using Aow.Game.Draw;
#nullable disable
namespace Aow.Game.World.Map;

public partial class HexagonSpriteSelection
{
    public bool CanSelectLocation(int Xhp, int Yhp, int Lhp)
    {
        bool Result;
        int Xhx = default;
        int Yhx = default;
        MapField MapF;
        int I;
        Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
        if (Xhx >= 0 && ((HSMap)Owner).FMapContainer.FWidth > Xhx && ((HSMap)Owner).FMapContainer.FHeight > Yhx && Yhx >= 0)
        {
            MapF = ((HSMap)Owner).FMapContainer.GetField(Xhx, Yhx, Lhp);
            I = MapF.Count;
            Result = false;
            while (!Result && I != 0)
            {
                I -= 1;
                Result = MapF.FHSList[I].CanSelect(Xhp, Yhp);
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    public void SelectLocation(int Xhp, int Yhp, int Lhp)
    {
        int N;
        int Xhx = default;
        int Yhx = default;
        MapField MapF;
        HexagonSprite HS;
        int I;
        Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
        MapF = ((HSMap)Owner).FMapContainer.GetField(Xhx, Yhx, Lhp);
        I = FindLocationHSIndex(MapF) - 1;
        HS = null;
        if (I < 0)
        {
            I = MapF.Count - 1;
        }

        N = MapF.Count;
        while (N != 0 && HS == null)
        {
            HS = MapF.FHSList[I];
            if (!HS.CanSelect(Xhp, Yhp))
            {
                HS = null;
            }

            N -= 1;
            I -= 1;
            if (I < 0)
            {
                I = MapF.Count - 1;
            }
        }

        if (HS != null)
        {
            if (FLocationHS.IndexOf(HS) == -1)
            {
                UnselectLocation();
                SelectLocationHS(HS);
            }
        }
    }

    public int FindLocationHSIndex(MapField MapF)
    {
        int I;
        int Xhp = default;
        int Yhp = default;
        HexagonSprite HS;
        int Result = -1;
        if (FLocationHS.Count != 0)
        {
            I = 0;
            Aow.Game.World.Map.HexGeometry.HXtoHP(MapF.FXhx, MapF.FYhx, ref Xhp, ref Yhp);
            while (Result == -1 && I != MapF.Count)
            {
                HS = MapF.FHSList[I];
                if (HS.CanSelect(Xhp, Yhp))
                {
                    if (FLocationHS.IndexOf(HS) != -1)
                    {
                        Result = I;
                    }
                }

                I += 1;
            }
        }

        return Result;
    }
}
