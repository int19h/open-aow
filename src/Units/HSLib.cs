// Translated from HSLib.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;

namespace Aow.Units.HSLib
{
    // FList (the TLibraryHS it created), FHSEngine, FEngine
    public partial class THSLibrary : TImageLibrary
    {
        internal TList FList;
        internal THSEngine FHSEngine;
        internal THexagonSpriteEngine FEngine;
        // HSLib.pas:292
        internal void SetEngine(THexagonSpriteEngine Value)
        {
            if (!ComponentState.Contains(TComponentState.csDesigning) && Value != FEngine)
            {
                FEngine = Value;
                FHSEngine = FEngine.HSEngine;
                for (int I = FList.Count - 1; I >= 0; I--)
                {
                    ((TLibraryHS)FList[I]).Remove();
                }
            }
        }

        // HSLib.pas:279
        public THSLibrary(TComponent owner) : base(owner)
        {
            FList = new TList();
        }

        // HSLib.pas:285
        public override void Destroy()
        {
            ClearList();
            FList.Free();
            base.Destroy();
        }

        // HSLib.pas:304
        public void ClearList()
        {
            for (int I = FList.Count - 1; I >= 0; I--)
            {
                ((TLibraryHS)FList[I]).Destroy();
            }

            FList.Clear();
        }

        // HSLib.pas:312
        public void RemoveHS(TLibraryHS HS)
        {
            FList.Remove(HS);
        }

        // HSLib.pas:317
        public TLibraryHS CreateHS(int Index)
        {
            TLibraryHS HS;
            HS = new TLibraryHS(null);
            HS.SetImage(this[Index]);
            HS.FLibrary = this;
            FList.Add(HS);
            return HS;
        }

        [Published]
        public THexagonSpriteEngine Engine { get => FEngine; set => SetEngine(value); }
    }

    // a library image shown at a pixel position of the map. FMapContainer, FLevel (the show level,
    // 30 by default), FImage, FDestroyOnShow, FLibrary (the THSLibrary that created it). FXhp =
    // Low(Integer) means not placed
    public partial class TLibraryHS : THPBasedHS
    {
        public TLibraryHS(TECustomNode owner) : base(owner)
        {
            FLevel = 30;
        }

        internal TMapContainer FMapContainer;
        internal byte FLevel;
        internal ImageLib.TLibraryImage FImage;
        internal bool FDestroyOnShow;
        internal THSLibrary FLibrary;
        // HSLib.pas:75


        // HSLib.pas:81
        public override void Destroy()
        {
            FLibrary.RemoveHS(this);
            base.Destroy();
        }

        // HSLib.pas:87
        public void GetBaseHex(ref sbyte Lhx)
        {
            Lhx = -1;
        }

        // HSLib.pas:92
        public override byte GetLevel() => FLevel;

        // HSLib.pas:97
        public override int GetShowPriority() => (FLevel << 26) + (FYhp << 13) + FXhp;

        // HSLib.pas:102
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
        }

        // HSLib.pas:107
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp;
            Xhp = FXhp;
            if (Xhp != -2147483648)
            {
                FImage.Show(Xhp - XhpSurf, FYhp - YhpSurf, SurfaceDesc);
            }

            if (FDestroyOnShow)
            {
                Destroy();
            }
        }

        // HSLib.pas:116
        public override sbyte GetXhx() => -1;

        // HSLib.pas:121
        public override sbyte GetYhx() => -1;

        // HSLib.pas:126
        public override sbyte GetLhx() => -1;

        // HSLib.pas:132
        // { Original bug: the right edge of the image is taken 8 pixels short for the second column. }
        public override void Remove()
        {
            int I = default;
            int J = default;
            int XMin = default;
            int OddYMin = default;
            int EvenYMin;
            int XMax = default;
            int OddYMax = default;
            int EvenYMax;
            if (FXhp != -2147483648)
            {
                HPtoHX(FXhp - 8, FYhp, ref XMin, ref OddYMin);
                HPtoHX(FXhp - 8 + 32, FYhp, ref I, ref J);
                if (!((XMin & 1) != 0))
                {
                    EvenYMin = OddYMin;
                    OddYMin = J;
                }
                else
                {
                    EvenYMin = J;
                }

                J = FImage.Height + FYhp - 1;
                HPtoHX(FImage.Width + FXhp + 8 - 1, J, ref XMax, ref OddYMax);
                HPtoHX(FImage.Width + FXhp - 1 + 32, J, ref I, ref J);
                if (!((XMax & 1) != 0))
                {
                    EvenYMax = OddYMax;
                    OddYMax = J;
                }
                else
                {
                    EvenYMax = J;
                }

                if (XMin < 0)
                {
                    XMin = 0;
                }

                if (XMax >= FMapContainer.Width)
                {
                    XMax = FMapContainer.Width - 1;
                }

                if (EvenYMin < 0)
                {
                    EvenYMin = 0;
                }

                if (EvenYMax >= FMapContainer.Height)
                {
                    EvenYMax = FMapContainer.Height - 1;
                }

                if (OddYMin < 0)
                {
                    OddYMin = 0;
                }

                if (OddYMax >= FMapContainer.Height)
                {
                    OddYMax = FMapContainer.Height - 1;
                }

                I = XMin;
                int last = XMax;
                for (; I <= last; I += 1)
                {
                    if (!((I & 1) != 0))
                    {
                        J = EvenYMin;
                        for (; J <= EvenYMax; J += 1)
                        {
                            FMapContainer.RemoveHS(I, J, FLhx, this);
                        }
                    }
                    else
                    {
                        J = OddYMin;
                        int limit = OddYMax;
                        for (; J <= limit; J += 1)
                        {
                            FMapContainer.RemoveHS(I, J, FLhx, this);
                        }
                    }
                }

                if (GetVisible())
                {
                    if (FLevel <= 15)
                    {
                        FMapContainer.Map.StaticVisibleHS.RemoveChild(this);
                        FMapContainer.Map.StaticSceneInvalid = true;
                    }
                    else
                    {
                        FMapContainer.Map.DynamicVisibleHS.RemoveChild(this);
                    }

                    SetVisible(false);
                }

                FXhp = -2147483648;
            }
        }

        // HSLib.pas:186
        public void SetImage(ImageLib.TLibraryImage Value)
        {
            int Xhp;
            if (FXhp != -2147483648)
            {
                Xhp = FXhp;
                Remove();
                FXhp = -2147483648;
            }
            else
            {
                Xhp = -2147483648;
            }

            FImage = Value;
            if (Value != null && Xhp != -2147483648)
            {
                Place(FMapContainer, Xhp, FYhp, FLhx);
            }
        }

        // HSLib.pas:203
        public override bool Place(TMapContainer MapContainer, int Xhp, int Yhp, int Lhx)
        {
            int I = default;
            int J = default;
            int XMin = default;
            int OddYMin = default;
            int EvenYMin;
            int XMax = default;
            int OddYMax = default;
            int EvenYMax;
            if (FXhp != -2147483648)
            {
                Remove();
            }

            FXhp = Xhp;
            FYhp = Yhp;
            FLhx = (sbyte)Lhx;
            FMapContainer = MapContainer;
            if (Xhp != -2147483648)
            {
                HPtoHX(FXhp - 8, FYhp, ref XMin, ref OddYMin);
                HPtoHX(FXhp - 8 + 32, FYhp, ref I, ref J);
                if (!((XMin & 1) != 0))
                {
                    EvenYMin = OddYMin;
                    OddYMin = J;
                }
                else
                {
                    EvenYMin = J;
                }

                J = FImage.Height + FYhp - 1;
                HPtoHX(FImage.Width + FXhp + 8 - 1, J, ref XMax, ref OddYMax);
                HPtoHX(FImage.Width + FXhp + 8 - 1 + 32, J, ref I, ref J);
                if (!((XMax & 1) != 0))
                {
                    EvenYMax = OddYMax;
                    OddYMax = J;
                }
                else
                {
                    EvenYMax = J;
                }

                if (XMin < 0)
                {
                    XMin = 0;
                }

                if (XMax >= MapContainer.Width)
                {
                    XMax = MapContainer.Width - 1;
                }

                if (EvenYMin < 0)
                {
                    EvenYMin = 0;
                }

                if (EvenYMax >= MapContainer.Height)
                {
                    EvenYMax = MapContainer.Height - 1;
                }

                if (OddYMin < 0)
                {
                    OddYMin = 0;
                }

                if (OddYMax >= MapContainer.Height)
                {
                    OddYMax = MapContainer.Height - 1;
                }

                I = XMin;
                int last = XMax;
                for (; I <= last; I += 1)
                {
                    if (!((I & 1) != 0))
                    {
                        J = EvenYMin;
                        for (; J <= EvenYMax; J += 1)
                        {
                            MapContainer.InsertHS(I, J, FLhx, this, 0);
                        }
                    }
                    else
                    {
                        J = OddYMin;
                        int limit = OddYMax;
                        for (; J <= limit; J += 1)
                        {
                            MapContainer.InsertHS(I, J, FLhx, this, 0);
                        }
                    }
                }
            }

            return true;
        }

        // HSLib.pas:251
        public bool PlaceEx(TMapContainer MapContainer, int Xhp, int Yhp, int Lhx, byte Level)
        {
            FLevel = Level;
            return Place(MapContainer, Xhp, Yhp, Lhx);
        }

        // HSLib.pas:257
        public bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            int Xhp = default;
            int Yhp = default;
            HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            return Place(MapContainer, Xhp - 8, Yhp, Lhx);
        }

        // HSLib.pas:265
        public bool PlaceHXEx(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx, sbyte Level, bool Center)
        {
            int Xhp = default;
            int Yhp = default;
            if (Level != -1)
            {
                FLevel = (byte)Level;
            }

            HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            Xhp = Xhp + 16 - (int)(((uint)FImage.Width) >> 1);
            Yhp = Yhp + 16 - (int)(((uint)FImage.Height) >> 1);
            return Place(MapContainer, Xhp, Yhp, Lhx);
        }

        public TMapContainer MapContainer { get => FMapContainer; }
        public new byte Level { get => FLevel; set => FLevel = value; }
        public ImageLib.TLibraryImage Image { get => FImage; set => SetImage(value); }
        public bool DestroyOnShow { get => FDestroyOnShow; set => FDestroyOnShow = value; }

        // HSLib.pas:28

    }

    public static partial class HSLib
    {
        // HSLib.pas:328
        public static void Register()
        {
            RegisterComponents("HS-Engine", [typeof(THSLibrary)]);
        }
    }
}
