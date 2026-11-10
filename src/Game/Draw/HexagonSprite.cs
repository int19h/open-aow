// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// the base of everything placed on the map. Owner is the TMapField the sprite stands on (for
// the multi-hex sprites the field of the base hexagon)
public partial class HexagonSprite : EObject
{
    public HexagonSprite(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 65872;

    // HXSPRITE.PAS:6
    // { HXSPRITE.PAS: THexagonSprite, THPBasedHS, TSingleHS, TFixedVisibleHS, TCustomFixedVisibleHS
    //   (include file of HSEngine.pas; the original Asserts name D:\GEngine\HSENGINE\inc\HXSPRITE.PAS) }
    public override void Dispose()
    {
        MainRemove();
        base.Dispose();
    }

    // HXSPRITE.PAS:116
    public virtual sbyte GetTerrain(int HN) => -2;

    // HXSPRITE.PAS:121
    public virtual void SetTerrain(int HN, sbyte Value)
    {
    }

    // HXSPRITE.PAS:125
    public virtual int GetTerrainCount() => 0;

    // HXSPRITE.PAS:130
    public virtual sbyte GetOverlay(int HN) => -2;

    // HXSPRITE.PAS:135
    public virtual void SetOverlay(int HN, sbyte Value)
    {
    }

    // HXSPRITE.PAS:139
    public virtual int GetOverlayCount() => 0;

    // HXSPRITE.PAS:52
    public virtual byte GetLevel()
    {
        throw new InvalidOperationException("No HexagonSprite Level defined");
    }

    // HXSPRITE.PAS:43
    public virtual void SetVisible(bool Value)
    {
    }

    // HXSPRITE.PAS:47
    public virtual bool GetVisible() => false;

    // HXSPRITE.PAS:72
    public virtual bool GetSelected() => false;

    // HXSPRITE.PAS:87
    public virtual sbyte GetXhx() => ((MapField)Owner).FXhx;

    // HXSPRITE.PAS:92
    public virtual sbyte GetYhx() => ((MapField)Owner).FYhx;

    // HXSPRITE.PAS:97
    public virtual sbyte GetLhx() => (sbyte)((MapLevel)Owner.Owner).FLhx;

    // HXSPRITE.PAS:102
    public virtual int GetXYL() => GetXhx() + (GetYhx() << 8) + (GetLhx() << 16);

    // HXSPRITE.PAS:153
    public virtual bool GetEditMode() => false;

    // HXSPRITE.PAS:158
    public virtual void SetEditMode(bool Value)
    {
    }

    // HXSPRITE.PAS:162
    public virtual int GetShowPriority() => 2130706432;

    // HXSPRITE.PAS:189
    public virtual void Connect()
    {
    }

    // HXSPRITE.PAS:193
    public virtual void Disconnect()
    {
    }

    // HXSPRITE.PAS:167
    public virtual bool CanChangeTerrain(in HSSenderInfo Info) => true;

    // HXSPRITE.PAS:172
    public virtual void ChangeTerrain(in HSTerrainInfo Info)
    {
    }

    // HXSPRITE.PAS:176
    public virtual void TerrainChanged(in HSTerrainInfo Info)
    {
    }

    // HXSPRITE.PAS:180
    public virtual void NeighbourTerrainChanged(int HN, in HSTerrainInfo Info)
    {
    }

    // HSEngine.pas:881
    public virtual void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // HXSPRITE.PAS:197
    public virtual bool PlaceOnMap()
    {
        Connect();
        return true;
    }

    // HXSPRITE.PAS:203
    public virtual void RemoveFromMap()
    {
        Disconnect();
    }

    // HXSPRITE.PAS:573
    public virtual bool MainPlace(MapContainer Map, int Xhp, int Yhp, int Lhx)
    {
        bool Result = Place(Map, Xhp, Yhp, Lhx);
        if (Result)
        {
            try
            {
                Activate();
            }
            catch
            {
                Result = false;
                Deactivate();
            }
        }

        return Result;
    }

    // HXSPRITE.PAS:585
    public virtual void MainRemove()
    {
        Deactivate();
        Remove();
        Assert(!GetVisible(), "Hexagon Sprite Still Visible!", "HXSPRITE.PAS", 589);
    }

    // HXSPRITE.PAS:592
    public virtual bool MainLoaded() => Loaded();

    // HXSPRITE.PAS:184
    public virtual bool Place(MapContainer Map, int Xhp, int Yhp, int Lhx) => PlaceOnMap();

    // HXSPRITE.PAS:208
    public virtual void Remove()
    {
        RemoveFromMap();
    }

    // HXSPRITE.PAS:213
    public virtual bool Loaded()
    {
        Connect();
        return true;
    }

    // HXSPRITE.PAS:26
    public virtual void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
    }

    // HXSPRITE.PAS:30
    public virtual void CanMoveOn(in HSMoveOnInfo Info, ref HSMoveOnResult MoveResult)
    {
    }

    // HXSPRITE.PAS:34
    public virtual int CanMoveOver(in HSSenderInfo Info) => 1;

    // HXSPRITE.PAS:39
    public virtual void MoveExclusive(in HSSenderInfo Info)
    {
    }

    // HXSPRITE.PAS:63
    public virtual void Changed()
    {
        MapField MapF;
        MapF = (MapField)Owner;
        if (MapF != null)
        {
            ((GameEngine)((MapLevel)MapF.Owner).FEngine).FHSMap.FHSChangedEventsStore.Raise(this);
        }
    }

    // HXSPRITE.PAS:563
    public virtual void Activate()
    {
    }

    // HXSPRITE.PAS:567
    public virtual void Deactivate()
    {
        if (GetSelected())
        {
            Unselect();
        }
    }

    // HXSPRITE.PAS:22
    public virtual void UpdateMapField(in HSMapFieldInfo Info)
    {
    }

    // HXSPRITE.PAS:144
    public virtual void MakeVisible(VisibilityList VisibilityList)
    {
        if (!GetVisible())
        {
            SetVisible(true);
            VisibilityList.AddSortChild(this, GetShowPriority());
        }
    }

    // HXSPRITE.PAS:82
    public virtual void GetBaseHX(ref int XYL)
    {
        XYL = ((MapField)Owner).FXhx + (((MapField)Owner).FYhx << 8) + (((MapLevel)Owner.Owner).FLhx << 16);
    }

    // HXSPRITE.PAS:57
    public virtual bool CanSelect(int Xhp, int Yhp) => Owner != null && ((GameEngine)((MapLevel)Owner.Owner).FEngine).FHSMap.FStatus.Contains(HSMapStatusElement.Editting);

    // HXSPRITE.PAS:107
    public virtual void Select()
    {
    }

    // HXSPRITE.PAS:111
    public virtual void Unselect()
    {
        ((MapLevel)Owner.Owner).FMap.FSelection.Unselect(this);
    }

    // HXSPRITE.PAS:17
    public virtual string EditName() => "Hexagon Sprite";

    // HXSPRITE.PAS:219
    public bool PlaceOnMapEx(MapContainer Map, int Xhp, int Yhp, int Lhx, int Width, int Height)
    {
        bool Result;
        int X = default;
        int Y = default;
        int Xmin = default;
        int YminOdd = default;
        int YminEven;
        int Xmax = default;
        int YmaxOdd = default;
        int YmaxEven;
        MapLevel MapL;
        HexGeometry.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
        HexGeometry.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
        if (!((Xmin & 1) != 0))
        {
            YminEven = YminOdd;
            YminOdd = Y;
        }
        else
        {
            YminEven = Y;
        }

        Y = Yhp + Height - 1;
        HexGeometry.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
        HexGeometry.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
        if (!((Xmax & 1) != 0))
        {
            YmaxEven = YmaxOdd;
            YmaxOdd = Y;
        }
        else
        {
            YmaxEven = Y;
        }

        if (Xmax >= 0 && Map.Width > Xmin && (YmaxEven >= 0 || YmaxOdd >= 0) && (Map.Height > YminEven || Map.Height > YminOdd))
        {
            if (Xmin < 0)
            {
                Xmin = 0;
            }

            if (Xmax >= Map.Width)
            {
                Xmax = Map.Width - 1;
            }

            if (YminEven < 0)
            {
                YminEven = 0;
                if (YmaxEven < 0)
                {
                    YmaxEven = 0;
                }
            }

            if (YmaxEven >= Map.Height)
            {
                YmaxEven = Map.Height - 1;
                if (YminEven >= Map.Height)
                {
                    YminEven = Map.Height - 1;
                }
            }

            if (YminOdd < 0)
            {
                YminOdd = 0;
                if (YmaxOdd < 0)
                {
                    YmaxOdd = 0;
                }
            }

            if (YmaxOdd >= Map.Height)
            {
                YmaxOdd = Map.Height - 1;
                if (YminOdd >= Map.Height)
                {
                    YminOdd = Map.Height - 1;
                }
            }

            MapL = Map.GetMapLevel(Lhx);
            X = (int)(((uint)(Xmin + Xmax)) >> 1);
            if (!((X & 1) != 0))
            {
                Y = (int)(((uint)(YminEven + YmaxEven)) >> 1);
            }
            else
            {
                Y = (int)(((uint)(YminOdd + YmaxOdd)) >> 1);
            }

            FOwner = MapL.GetField(X, Y);
            X = Xmin;
            int last = Xmax;
            for (; X <= last; X += 1)
            {
                if (!((X & 1) != 0))
                {
                    Y = YminEven;
                    for (; Y <= YmaxEven; Y += 1)
                    {
                        MapL.GetField(X, Y).AddHS(this, 1);
                    }
                }
                else
                {
                    Y = YminOdd;
                    int limit = YmaxOdd;
                    for (; Y <= limit; Y += 1)
                    {
                        MapL.GetField(X, Y).AddHS(this, 1);
                    }
                }
            }

            ((MapField)FOwner).SetHNofHS(this, 0);
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // HXSPRITE.PAS:295
    public void RemoveFromMapEx(int Xhp, int Yhp, int Width, int Height)
    {
        int X = default;
        int Y = default;
        int Xmin = default;
        int YminOdd = default;
        int YminEven;
        int Xmax = default;
        int YmaxOdd = default;
        int YmaxEven;
        MapLevel MapL;
        MapL = (MapLevel)FOwner.Owner;
        HexGeometry.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
        HexGeometry.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
        if (!((Xmin & 1) != 0))
        {
            YminEven = YminOdd;
            YminOdd = Y;
        }
        else
        {
            YminEven = Y;
        }

        Y = Yhp + Height - 1;
        HexGeometry.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
        HexGeometry.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
        if (!((Xmax & 1) != 0))
        {
            YmaxEven = YmaxOdd;
            YmaxOdd = Y;
        }
        else
        {
            YmaxEven = Y;
        }

        if (Xmax >= 0 && Xmin < MapL.Width && (YmaxEven >= 0 || YmaxOdd >= 0) && (MapL.Height > YminEven || MapL.Height > YminOdd))
        {
            if (Xmin < 0)
            {
                Xmin = 0;
            }

            if (Xmax >= MapL.Width)
            {
                Xmax = MapL.Width - 1;
            }

            if (YminEven < 0)
            {
                YminEven = 0;
                if (YmaxEven < 0)
                {
                    YmaxEven = 0;
                }
            }

            if (YmaxEven >= MapL.Height)
            {
                YmaxEven = MapL.Height - 1;
                if (YminEven >= MapL.Height)
                {
                    YminEven = MapL.Height - 1;
                }
            }

            if (YminOdd < 0)
            {
                YminOdd = 0;
                if (YmaxOdd < 0)
                {
                    YmaxOdd = 0;
                }
            }

            if (YmaxOdd >= MapL.Height)
            {
                YmaxOdd = MapL.Height - 1;
                if (YminOdd >= MapL.Height)
                {
                    YminOdd = MapL.Height - 1;
                }
            }

            X = Xmin;
            int last = Xmax;
            for (; X <= last; X += 1)
            {
                if (!((X & 1) != 0))
                {
                    Y = YminEven;
                    for (; Y <= YmaxEven; Y += 1)
                    {
                        MapL.GetField(X, Y).RemoveHS(this);
                    }
                }
                else
                {
                    Y = YminOdd;
                    int limit = YmaxOdd;
                    for (; Y <= limit; Y += 1)
                    {
                        MapL.GetField(X, Y).RemoveHS(this);
                    }
                }
            }
        }

        FOwner = null;
    }

    // HXSPRITE.PAS:362
    public bool PlaceUserOnMapEx(MapContainer Map, int Xhp, int Yhp, int Lhx, int Width, int Height)
    {
        bool Result;
        int X = default;
        int Y = default;
        int Xmin = default;
        int YminOdd = default;
        int YminEven;
        int Xmax = default;
        int YmaxOdd = default;
        int YmaxEven;
        MapLevel MapL;
        HexGeometry.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
        HexGeometry.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
        if (!((Xmin & 1) != 0))
        {
            YminEven = YminOdd;
            YminOdd = Y;
        }
        else
        {
            YminEven = Y;
        }

        Y = Yhp + Height - 1;
        HexGeometry.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
        HexGeometry.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
        if (!((Xmax & 1) != 0))
        {
            YmaxEven = YmaxOdd;
            YmaxOdd = Y;
        }
        else
        {
            YmaxEven = Y;
        }

        if (Xmax >= 0 && Map.Width > Xmin && (YmaxEven >= 0 || YmaxOdd >= 0) && (Map.Height > YminEven || Map.Height > YminOdd))
        {
            if (Xmin < 0)
            {
                Xmin = 0;
            }

            if (Xmax >= Map.Width)
            {
                Xmax = Map.Width - 1;
            }

            if (YminEven < 0)
            {
                YminEven = 0;
                if (YmaxEven < 0)
                {
                    YmaxEven = 0;
                }
            }

            if (YmaxEven >= Map.Height)
            {
                YmaxEven = Map.Height - 1;
                if (YminEven >= Map.Height)
                {
                    YminEven = Map.Height - 1;
                }
            }

            if (YminOdd < 0)
            {
                YminOdd = 0;
                if (YmaxOdd < 0)
                {
                    YmaxOdd = 0;
                }
            }

            if (YmaxOdd >= Map.Height)
            {
                YmaxOdd = Map.Height - 1;
                if (YminOdd >= Map.Height)
                {
                    YminOdd = Map.Height - 1;
                }
            }

            MapL = Map.GetMapLevel(Lhx);
            X = Xmin;
            int last = Xmax;
            for (; X <= last; X += 1)
            {
                if (!((X & 1) != 0))
                {
                    Y = YminEven;
                    for (; Y <= YmaxEven; Y += 1)
                    {
                        MapL.GetField(X, Y).AddHS(this, 1);
                    }
                }
                else
                {
                    Y = YminOdd;
                    int limit = YmaxOdd;
                    for (; Y <= limit; Y += 1)
                    {
                        MapL.GetField(X, Y).AddHS(this, 1);
                    }
                }
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // HXSPRITE.PAS:431
    public void RemoveUserFromMapEx(MapContainer Map, int Xhp, int Yhp, int Lhx, int Width, int Height)
    {
        int X = default;
        int Y = default;
        int Xmin = default;
        int YminOdd = default;
        int YminEven;
        int Xmax = default;
        int YmaxOdd = default;
        int YmaxEven;
        MapLevel MapL;
        MapL = Map.GetMapLevel(Lhx);
        HexGeometry.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
        HexGeometry.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
        if (!((Xmin & 1) != 0))
        {
            YminEven = YminOdd;
            YminOdd = Y;
        }
        else
        {
            YminEven = Y;
        }

        Y = Yhp + Height - 1;
        HexGeometry.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
        HexGeometry.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
        if (!((Xmax & 1) != 0))
        {
            YmaxEven = YmaxOdd;
            YmaxOdd = Y;
        }
        else
        {
            YmaxEven = Y;
        }

        if (Xmax >= 0 && Xmin < MapL.Width && (YmaxEven >= 0 || YmaxOdd >= 0) && (MapL.Height > YminEven || MapL.Height > YminOdd))
        {
            if (Xmin < 0)
            {
                Xmin = 0;
            }

            if (Xmax >= MapL.Width)
            {
                Xmax = MapL.Width - 1;
            }

            if (YminEven < 0)
            {
                YminEven = 0;
                if (YmaxEven < 0)
                {
                    YmaxEven = 0;
                }
            }

            if (YmaxEven >= MapL.Height)
            {
                YmaxEven = MapL.Height - 1;
                if (YminEven >= MapL.Height)
                {
                    YminEven = MapL.Height - 1;
                }
            }

            if (YminOdd < 0)
            {
                YminOdd = 0;
                if (YmaxOdd < 0)
                {
                    YmaxOdd = 0;
                }
            }

            if (YmaxOdd >= MapL.Height)
            {
                YmaxOdd = MapL.Height - 1;
                if (YminOdd >= MapL.Height)
                {
                    YminOdd = MapL.Height - 1;
                }
            }

            X = Xmin;
            int last = Xmax;
            for (; X <= last; X += 1)
            {
                if (!((X & 1) != 0))
                {
                    Y = YminEven;
                    for (; Y <= YmaxEven; Y += 1)
                    {
                        MapL.GetField(X, Y).RemoveHS(this);
                    }
                }
                else
                {
                    Y = YminOdd;
                    int limit = YmaxOdd;
                    for (; Y <= limit; Y += 1)
                    {
                        MapL.GetField(X, Y).RemoveHS(this);
                    }
                }
            }
        }
    }

    // HXSPRITE.PAS:497
    public void SortMapFieldEx(MapContainer Map, int Xhp, int Yhp, int Lhx, int Width, int Height)
    {
        int X = default;
        int Y = default;
        int Xmin = default;
        int YminOdd = default;
        int YminEven;
        int Xmax = default;
        int YmaxOdd = default;
        int YmaxEven;
        MapLevel MapL;
        MapL = Map.GetMapLevel(Lhx);
        HexGeometry.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
        HexGeometry.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
        if (!((Xmin & 1) != 0))
        {
            YminEven = YminOdd;
            YminOdd = Y;
        }
        else
        {
            YminEven = Y;
        }

        Y = Yhp + Height - 1;
        HexGeometry.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
        HexGeometry.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
        if (!((Xmax & 1) != 0))
        {
            YmaxEven = YmaxOdd;
            YmaxOdd = Y;
        }
        else
        {
            YmaxEven = Y;
        }

        if (Xmax >= 0 && Xmin < MapL.Width && (YmaxEven >= 0 || YmaxOdd >= 0) && (MapL.Height > YminEven || MapL.Height > YminOdd))
        {
            if (Xmin < 0)
            {
                Xmin = 0;
            }

            if (Xmax >= MapL.Width)
            {
                Xmax = MapL.Width - 1;
            }

            if (YminEven < 0)
            {
                YminEven = 0;
                if (YmaxEven < 0)
                {
                    YmaxEven = 0;
                }
            }

            if (YmaxEven >= MapL.Height)
            {
                YmaxEven = MapL.Height - 1;
                if (YminEven >= MapL.Height)
                {
                    YminEven = MapL.Height - 1;
                }
            }

            if (YminOdd < 0)
            {
                YminOdd = 0;
                if (YmaxOdd < 0)
                {
                    YmaxOdd = 0;
                }
            }

            if (YmaxOdd >= MapL.Height)
            {
                YmaxOdd = MapL.Height - 1;
                if (YminOdd >= MapL.Height)
                {
                    YminOdd = MapL.Height - 1;
                }
            }

            X = Xmin;
            int last = Xmax;
            for (; X <= last; X += 1)
            {
                if (!((X & 1) != 0))
                {
                    Y = YminEven;
                    for (; Y <= YmaxEven; Y += 1)
                    {
                        MapL.GetField(X, Y).Sort();
                    }
                }
                else
                {
                    Y = YminOdd;
                    int limit = YmaxOdd;
                    for (; Y <= limit; Y += 1)
                    {
                        MapL.GetField(X, Y).Sort();
                    }
                }
            }
        }
    }

    public sealed class TerrainAccessor(HexagonSprite target)
    {
        public sbyte this[int HN] { get => target.GetTerrain(HN); set => target.SetTerrain(HN, value); }
    }

    public TerrainAccessor Terrain => new TerrainAccessor(this);
    public int TerrainCount { get => GetTerrainCount(); }

    public sealed class OverlayAccessor(HexagonSprite target)
    {
        public sbyte this[int HN] { get => target.GetOverlay(HN); set => target.SetOverlay(HN, value); }
    }

    public OverlayAccessor Overlay => new OverlayAccessor(this);
    public int OverlayCount { get => GetOverlayCount(); }
    public byte Level { get => GetLevel(); }
    public bool Visible { get => GetVisible(); set => SetVisible(value); }
    public bool Selected { get => GetSelected(); }
    public sbyte Xhx { get => GetXhx(); }
    public sbyte Yhx { get => GetYhx(); }
    public sbyte Lhx { get => GetLhx(); }
    public int XYL { get => GetXYL(); }
    public bool EditMode { get => GetEditMode(); set => SetEditMode(value); }
    public int ShowPriority { get => GetShowPriority(); }

    // HXSPRITE.PAS:77
    public virtual Set<ControlModeElement> ControlMode() => Set<ControlModeElement>.Of();

}
