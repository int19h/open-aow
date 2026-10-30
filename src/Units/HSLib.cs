// Translated from HSLib.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;

namespace Aow.Units.HSLib
{
    // { HSLib.THSLibrary (instance size 92): +$50 FList (the TLibraryHS it created), +$54 FHSEngine,
//     +$58 FEngine }
    public partial class THSLibrary : Aow.Units.ImageLib.TImageLibrary
    {
        internal Aow.Rtl.TList FList;
        internal Aow.Units.HSEngine.THSEngine FHSEngine;
        internal Aow.Units.HSEngine.THexagonSpriteEngine FEngine;
        // HSLib.pas:292
        internal void SetEngine(Aow.Units.HSEngine.THexagonSpriteEngine Value)
        {
            int I = default;
            if (!ComponentState.Contains(Aow.Rtl.TComponentState.csDesigning) && (Value != FEngine))
            {
                FEngine = Value;
                FHSEngine = FEngine.HSEngine;
                {
                    I = FList.Count - 1;
                    byte end__1 = 0;
                    for (; I >= end__1; I -= 1)
                    {
                        var call__2 = FList[I];
                        ((Aow.Units.HSLib.TLibraryHS)call__2).Remove();
                    }
                }
            }
        }

        // HSLib.pas:279
        // { THSLibrary }
        public THSLibrary(TComponent owner) : base(owner)
        {
            var call__3 = new Aow.Rtl.TList();
            FList = call__3;
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
            int I = default;
            {
                I = FList.Count - 1;
                byte end__4 = 0;
                for (; I >= end__4; I -= 1)
                {
                    var call__5 = FList[I];
                    ((Aow.Units.HSLib.TLibraryHS)call__5).Destroy();
                }
            }

            FList.Clear();
        }

        // HSLib.pas:312
        public void RemoveHS(Aow.Units.HSLib.TLibraryHS HS)
        {
            FList.Remove(HS);
        }

        // HSLib.pas:317
        public Aow.Units.HSLib.TLibraryHS CreateHS(int Index)
        {
            Aow.Units.HSLib.TLibraryHS Result = default;
            Aow.Units.HSLib.TLibraryHS HS = default;
            HS = new Aow.Units.HSLib.TLibraryHS(null);
            var call__6 = this[Index];
            HS.SetImage(call__6);
            HS.FLibrary = this;
            FList.Add(HS);
            Result = HS;
            return Result;
        }

        [Published]
        public Aow.Units.HSEngine.THexagonSpriteEngine Engine { get => FEngine; set => SetEngine(value); }
    }

    // { HSLib.TLibraryHS (instance size 44): a library image shown at a pixel position of the map.
//     +$18 FMapContainer, +$1C FLevel (the show level, 30 by default), +$20 FImage, +$24
//     FDestroyOnShow, +$28 FLibrary (the THSLibrary that created it). FXhp = Low(Integer) means not
//     placed. }
    public partial class TLibraryHS : Aow.Units.HSEngine.THPBasedHS
    {
        public TLibraryHS(TECustomNode owner) : base(owner)
        {
            FLevel = 30;
        }

        internal Aow.Units.HSEngine.TMapContainer FMapContainer;
        internal byte FLevel;
        internal Aow.Units.ImageLib.TLibraryImage FImage;
        internal bool FDestroyOnShow;
        internal Aow.Units.HSLib.THSLibrary FLibrary;
        // HSLib.pas:75
        // { TLibraryHS }


        // HSLib.pas:81
        public override void Destroy()
        {
            FLibrary.RemoveHS(this);
            base.Destroy();
        }

        // HSLib.pas:87
        public void GetBaseHex(ref sbyte Lhx)
        {
            Lhx = unchecked((sbyte)-1);
        }

        // HSLib.pas:92
        public override byte GetLevel()
        {
            byte Result = default;
            Result = unchecked((byte)FLevel);
            return Result;
        }

        // HSLib.pas:97
        public override int GetShowPriority()
        {
            int Result = default;
            Result = ((FLevel << 26) + (FYhp << 13)) + FXhp;
            return Result;
        }

        // HSLib.pas:102
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
        }

        // HSLib.pas:107
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
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
        public override sbyte GetXhx()
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-1);
            return Result;
        }

        // HSLib.pas:121
        public override sbyte GetYhx()
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-1);
            return Result;
        }

        // HSLib.pas:126
        public override sbyte GetLhx()
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-1);
            return Result;
        }

        // HSLib.pas:132
        // { Original bug: the right edge of the image is taken 8 pixels short for the second column. }
        public override void Remove()
        {
            int I = default;
            int J = default;
            int XMin = default;
            int OddYMin = default;
            int EvenYMin = default;
            int XMax = default;
            int OddYMax = default;
            int EvenYMax = default;
            if (FXhp != -2147483648)
            {
                Aow.Units.HSEngine.HSEngine.HPtoHX(FXhp - 8, FYhp, ref XMin, ref OddYMin);
                Aow.Units.HSEngine.HSEngine.HPtoHX((FXhp - 8) + 32, FYhp, ref I, ref J);
                if (!((XMin & 1) != 0))
                {
                    EvenYMin = OddYMin;
                    OddYMin = J;
                }
                else
                {
                    EvenYMin = J;
                }

                J = (FImage.Height + FYhp) - 1;
                Aow.Units.HSEngine.HSEngine.HPtoHX(((FImage.Width + FXhp) + 8) - 1, J, ref XMax, ref OddYMax);
                Aow.Units.HSEngine.HSEngine.HPtoHX(((FImage.Width + FXhp) - 1) + 32, J, ref I, ref J);
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

                {
                    I = XMin;
                    int end__7 = XMax;
                    for (; I <= end__7; I += 1)
                    {
                        if (!((I & 1) != 0))
                        {
                            {
                                J = EvenYMin;
                                int end__8 = EvenYMax;
                                for (; J <= end__8; J += 1)
                                {
                                    FMapContainer.RemoveHS(I, J, FLhx, this);
                                }
                            }
                        }
                        else
                        {
                            {
                                J = OddYMin;
                                int end__9 = OddYMax;
                                for (; J <= end__9; J += 1)
                                {
                                    FMapContainer.RemoveHS(I, J, FLhx, this);
                                }
                            }
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
        public void SetImage(Aow.Units.ImageLib.TLibraryImage Value)
        {
            int Xhp = default;
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
            if ((Value != null) && (Xhp != -2147483648))
            {
                Place(FMapContainer, Xhp, FYhp, FLhx);
            }
        }

        // HSLib.pas:203
        public override bool Place(Aow.Units.HSEngine.TMapContainer MapContainer, int Xhp, int Yhp, int Lhx)
        {
            bool Result = default;
            int I = default;
            int J = default;
            int XMin = default;
            int OddYMin = default;
            int EvenYMin = default;
            int XMax = default;
            int OddYMax = default;
            int EvenYMax = default;
            if (FXhp != -2147483648)
            {
                Remove();
            }

            FXhp = Xhp;
            FYhp = Yhp;
            FLhx = unchecked((sbyte)Lhx);
            FMapContainer = MapContainer;
            if (Xhp != -2147483648)
            {
                Aow.Units.HSEngine.HSEngine.HPtoHX(FXhp - 8, FYhp, ref XMin, ref OddYMin);
                Aow.Units.HSEngine.HSEngine.HPtoHX((FXhp - 8) + 32, FYhp, ref I, ref J);
                if (!((XMin & 1) != 0))
                {
                    EvenYMin = OddYMin;
                    OddYMin = J;
                }
                else
                {
                    EvenYMin = J;
                }

                J = (FImage.Height + FYhp) - 1;
                Aow.Units.HSEngine.HSEngine.HPtoHX(((FImage.Width + FXhp) + 8) - 1, J, ref XMax, ref OddYMax);
                Aow.Units.HSEngine.HSEngine.HPtoHX((((FImage.Width + FXhp) + 8) - 1) + 32, J, ref I, ref J);
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

                {
                    I = XMin;
                    int end__10 = XMax;
                    for (; I <= end__10; I += 1)
                    {
                        if (!((I & 1) != 0))
                        {
                            {
                                J = EvenYMin;
                                int end__11 = EvenYMax;
                                for (; J <= end__11; J += 1)
                                {
                                    MapContainer.InsertHS(I, J, FLhx, this, 0);
                                }
                            }
                        }
                        else
                        {
                            {
                                J = OddYMin;
                                int end__12 = OddYMax;
                                for (; J <= end__12; J += 1)
                                {
                                    MapContainer.InsertHS(I, J, FLhx, this, 0);
                                }
                            }
                        }
                    }
                }
            }

            Result = true;
            return Result;
        }

        // HSLib.pas:251
        public bool PlaceEx(Aow.Units.HSEngine.TMapContainer MapContainer, int Xhp, int Yhp, int Lhx, byte Level)
        {
            bool Result = default;
            FLevel = unchecked((byte)Level);
            Result = Place(MapContainer, Xhp, Yhp, Lhx);
            return Result;
        }

        // HSLib.pas:257
        public bool PlaceHX(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            int Xhp = default;
            int Yhp = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            Result = Place(MapContainer, Xhp - 8, Yhp, Lhx);
            return Result;
        }

        // HSLib.pas:265
        public bool PlaceHXEx(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx, sbyte Level, bool Center)
        {
            bool Result = default;
            int Xhp = default;
            int Yhp = default;
            if (Level != -1)
            {
                FLevel = unchecked((byte)Level);
            }

            Aow.Units.HSEngine.HSEngine.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            Xhp = (Xhp + 16) - ((int)(((uint)FImage.Width) >> 1));
            Yhp = (Yhp + 16) - ((int)(((uint)FImage.Height) >> 1));
            Result = Place(MapContainer, Xhp, Yhp, Lhx);
            return Result;
        }

        public Aow.Units.HSEngine.TMapContainer MapContainer { get => FMapContainer; }
        public new byte Level { get => FLevel; set => FLevel = value; }
        public Aow.Units.ImageLib.TLibraryImage Image { get => FImage; set => SetImage(value); }
        public bool DestroyOnShow { get => FDestroyOnShow; set => FDestroyOnShow = value; }

        // HSLib.pas:28

    }

    public static partial class HSLib
    {
        // HSLib.pas:328
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"HS-Engine", [typeof(Aow.Units.HSLib.THSLibrary)]);
        }
    }
}
