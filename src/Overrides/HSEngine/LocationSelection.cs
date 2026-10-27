#nullable disable
using Aow.Rtl;
namespace Aow.Units.HSEngine
{
    public partial class THexagonSpriteSelection
    {
        public bool CanSelectLocation(int Xhp, int Yhp, int Lhp)
        {
            bool Result = default;
            int Xhx = default;
            int Yhx = default;
            Aow.Units.HSEngine.TMapField MapF = default;
            int I = default;
            Aow.Units.HSEngine.HSEngine.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
            if ((((Xhx >= 0) && (((Aow.Units.HSEngine.THSMap)Owner).FMapContainer.FWidth > Xhx)) && (((Aow.Units.HSEngine.THSMap)Owner).FMapContainer.FHeight > Yhx)) && (Yhx >= 0))
            {
                var call__226 = ((Aow.Units.HSEngine.THSMap)Owner).FMapContainer.GetField(Xhx, Yhx, Lhp);
                MapF = call__226;
                var call__227 = MapF.Count;
                I = call__227;
                Result = false;
                while (!Result && (I != 0))
                {
                    I -= 1;
                    var call__228 = MapF.FHSList[I].CanSelect(Xhp, Yhp);
                    Result = call__228;
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
            int N = default;
            int Xhx = default;
            int Yhx = default;
            Aow.Units.HSEngine.TMapField MapF = default;
            Aow.Units.HSEngine.THexagonSprite HS = default;
            int I = default;
            Aow.Units.HSEngine.HSEngine.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
            var call__229 = ((Aow.Units.HSEngine.THSMap)Owner).FMapContainer.GetField(Xhx, Yhx, Lhp);
            MapF = call__229;
            I = FindLocationHSIndex(MapF) - 1;
            ;
            HS = null;
            if (I < 0)
            {
                var call__230 = MapF.Count;
                I = call__230 - 1;
            }

            var call__231 = MapF.Count;
            N = call__231;
            while ((N != 0) && (HS == null))
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
                    var call__232 = MapF.Count;
                    I = call__232 - 1;
                }
            }

            if (HS != null)
            {
                var call__233 = FLocationHS.IndexOf(HS);
                if (call__233 == -1)
                {
                    UnselectLocation();
                    SelectLocationHS(HS);
                }
            }
        }
        public int FindLocationHSIndex(Aow.Units.HSEngine.TMapField MapF)
        {
            int Result = default;
            int I = default;
            int Xhp = default;
            int Yhp = default;
            Aow.Units.HSEngine.THexagonSprite HS = default;
            Result = -1;
            if (FLocationHS.Count != 0)
            {
                I = 0;
                Aow.Units.HSEngine.HSEngine.HXtoHP(MapF.FXhx, MapF.FYhx, ref Xhp, ref Yhp);
                while (true)
                {
                    bool bool__227 = (Result == -1);
                    if (bool__227)
                    {
                        var call__228 = MapF.Count;
                        bool__227 = (I != call__228);
                    }

                    if (!bool__227)
                        break;
                    {
                        HS = MapF.FHSList[I];
                        if (HS.CanSelect(Xhp, Yhp))
                        {
                            var call__229 = FLocationHS.IndexOf(HS);
                            if (call__229 != -1)
                            {
                                Result = I;
                            }
                        }

                        I += 1;
                    }
                }
            }

            return Result;
        }
    }
}
