// Translated from HSEngine.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// an image of an image library shown at a pixel position. FStatus, FImage, FXhp, FYhp, FLhx,
// FShowPriority (the top six bits are the level, the rest the offset SetShowPriorityOffset
// adds). Persisted: 1 FStatus (rwData), 2 FShowPriority, 3 FXhp, 4 FYhp, 5 FLhx (rwByte)
public partial class LibraryImageHS : HexagonSprite
{
    public new const int ClassID = 65875;

    internal Set<StatusElement> FStatus;
    internal LibraryImage FImage;
    internal int FXhp;
    internal int FYhp;
    internal byte FLhx;
    internal int FShowPriority;

    // LibImgHS.PAS:111
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        RwSet(ref FStatus, 1, 1, Stream.rwData, 0);
        Stream.rwInteger(ref FShowPriority, 2);
        Stream.rwInteger(ref FXhp, 3);
        Stream.rwInteger(ref FYhp, 4);
        Stream.rwByte(ref FLhx, 5);
    }

    // LibImgHS.PAS:6
    // { LibImgHS.PAS: TLibraryImageHS (include file of HSEngine.pas; the original Asserts name
    //   D:\GEngine\HSENGINE\inc\LibImgHS.PAS) }
    public LibraryImageHS(ECustomNode owner) : base(owner)
    {
        FStatus = Set<StatusElement>.Of(StatusElement.MapIsOwner);
        SetLevel(26);
    }

    // LibImgHS.PAS:85
    public override byte GetLevel() => (byte)(int)(((uint)FShowPriority) >> 26);

    // LibImgHS.PAS:18
    public override void SetVisible(bool Value)
    {
        if (Value)
        {
            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.Visible);
        }
        else
        {
            FStatus = FStatus - Set<StatusElement>.Of(StatusElement.Visible);
        }
    }

    // LibImgHS.PAS:26
    public override bool GetVisible() => FStatus.Contains(StatusElement.Visible);

    // LibImgHS.PAS:40
    public override bool GetEditMode() => FStatus.Contains(StatusElement.EditMode);

    // LibImgHS.PAS:45
    public override void SetEditMode(bool Value)
    {
        if (Value)
        {
            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.EditMode);
        }
        else
        {
            FStatus = FStatus - Set<StatusElement>.Of(StatusElement.EditMode);
        }
    }

    // LibImgHS.PAS:90
    public override int GetShowPriority() => (FYhp << 13) + FXhp + FShowPriority;

    // LibImgHS.PAS:194
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        FImage.Show(FXhp - XhpSurf, FYhp - YhpSurf, SurfaceDesc);
        if (FStatus.Contains(StatusElement.DestroyOnShow))
        {
            Dispose();
        }
    }

    // LibImgHS.PAS:126
    public override bool Place(MapContainer Map, int Xhp, int Yhp, int Lhx)
    {
        bool Result;
        if (CanPlace(Map, Xhp, Yhp, Lhx))
        {
            FXhp = Xhp;
            FYhp = Yhp;
            FLhx = (byte)Lhx;
            if (FStatus.Contains(StatusElement.MapIsOwner))
            {
                Result = PlaceOnMapEx(Map, Xhp, Yhp, Lhx, FImage.Width, FImage.Height);
            }
            else
            {
                Result = PlaceUserOnMapEx(Map, Xhp, Yhp, Lhx, FImage.Width, FImage.Height);
            }

            if (Result)
            {
                FStatus = FStatus + Set<StatusElement>.Of(StatusElement.PlacedOnMap);
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // LibImgHS.PAS:152
    public override void Remove()
    {
        GameEngine Engine;
        if (FStatus.Contains(StatusElement.PlacedOnMap))
        {
            if (FStatus.Contains(StatusElement.MapIsOwner))
            {
                if (GetVisible())
                {
                    if (GetLevel() <= 15)
                    {
                        ((MapLevel)Owner.Owner).FMapContainer.FMap.FStaticVisibleHS.RemoveChild(this);
                        ((MapLevel)Owner.Owner).FMapContainer.FMap.FStaticSceneInvalid = true;
                    }
                    else
                    {
                        ((MapLevel)Owner.Owner).FMapContainer.FMap.FDynamicVisibleHS.RemoveChild(this);
                    }

                    SetVisible(false);
                }

                RemoveFromMapEx(FXhp, FYhp, FImage.Width, FImage.Height);
            }
            else
            {
                Engine = GetEngine();
                if (GetVisible())
                {
                    if (GetLevel() <= 15)
                    {
                        Engine.FHSMap.FStaticVisibleHS.RemoveChild(this);
                        Engine.FHSMap.FStaticSceneInvalid = true;
                    }
                    else
                    {
                        Engine.FHSMap.FDynamicVisibleHS.RemoveChild(this);
                    }

                    SetVisible(false);
                }

                RemoveUserFromMapEx(Engine.FHSMap.FMapContainer, FXhp, FYhp, FLhx, FImage.Width, FImage.Height);
            }

            FStatus = FStatus - Set<StatusElement>.Of(StatusElement.PlacedOnMap);
        }
    }

    // LibImgHS.PAS:31
    public override void MakeVisible(VisibilityList VisibilityList)
    {
        if (!FStatus.Contains(StatusElement.Visible))
        {
            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.Visible);
            VisibilityList.AddSortChild(this, GetShowPriority());
        }
    }

    // LibImgHS.PAS:101
    public virtual void SetImage(LibraryImage Value)
    {
        if (FStatus.Contains(StatusElement.PlacedOnMap))
        {
            if (Value == null || (FImage.Width != Value.Width || FImage.Height != Value.Height))
            {
                throw new InvalidOperationException("Cannot change image when placed on map (TLibraryImageHS)");
            }
        }

        FImage = Value;
        if (FImage != null)
        {
            SetShowPriorityOffset((int)(((uint)FImage.Width) >> 1), (int)(((uint)FImage.Height) >> 1));
        }
    }

    // LibImgHS.PAS:121
    public virtual bool CanPlace(MapContainer Map, int Xhp, int Yhp, int Lhx) => true;

    // LibImgHS.PAS:144
    public virtual bool PlaceHX(MapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
        return Place(Map, (int)(((uint)FImage.Width) >> 2) + Xhp + 16, (int)(((uint)FImage.Height) >> 2) + Yhp + 16, Lhx);
    }

    // LibImgHS.PAS:53
    public bool GetMapIsOwner() => FStatus.Contains(StatusElement.MapIsOwner);

    // LibImgHS.PAS:58
    public void SetMapIsOwner(bool Value)
    {
        if (FStatus.Contains(StatusElement.PlacedOnMap))
        {
            HexGeometry.OriginalAssert("Cannot change MapIsOwner when places on map (TLibraryImageHS)", "D:\\GEngine\\HSENGINE\\inc\\LibImgHS.PAS", 6626);
        }

        if (Value)
        {
            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.MapIsOwner);
        }
        else
        {
            FStatus = FStatus - Set<StatusElement>.Of(StatusElement.MapIsOwner);
        }
    }

    // LibImgHS.PAS:67
    public bool GetDestroyOnShow() => FStatus.Contains(StatusElement.DestroyOnShow);

    // LibImgHS.PAS:72
    public void SetDestroyOnShow(bool Value)
    {
        if (Value)
        {
            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.DestroyOnShow);
        }
        else
        {
            FStatus = FStatus - Set<StatusElement>.Of(StatusElement.DestroyOnShow);
        }
    }

    // LibImgHS.PAS:80
    public void SetLevel(byte Value)
    {
        FShowPriority = (FShowPriority & 67108863) + (Value << 26);
    }

    // LibImgHS.PAS:95
    public void SetShowPriorityOffset(int OffsetX, int OffsetY)
    {
        // { both offsets are shifted into the Y part (original) }
        FShowPriority = (FShowPriority & -67108864) + (OffsetY << 13) + (OffsetX << 13);
    }

    public LibraryImage Image { get => FImage; set => SetImage(value); }
    public bool MapIsOwner { get => GetMapIsOwner(); set => SetMapIsOwner(value); }
    public bool DestroyOnShow { get => GetDestroyOnShow(); set => SetDestroyOnShow(value); }
    public new byte Level { get => GetLevel(); set => SetLevel(value); }

    public enum StatusElement : byte
    {
        Visible = 0, EditMode = 1, DestroyOnShow = 2, MapIsOwner = 3, PlacedOnMap = 4
    }

}
