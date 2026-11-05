// Translated from IsoHex.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngineUnit;

namespace Aow.Units.IsoHex;

// FDynamic, FOpenSides (bit HN-1: the neighbour HN is no hexagon of this kind), FSides (bits
// 0.5: the neighbour HN has another terrain, bits 6.7: the image variant)
public partial class TIsometricHexagon : TAbstractHexagon
{
    public TIsometricHexagon(TECustomNode owner) : base(owner)
    {
        FSides = (byte)(Random(4) << 6);
    }


    // IsoHex.pas:214
    public new const int ClassID = 131437;

    protected internal TDynamicIsometricHexagon FDynamic;
    protected internal byte FOpenSides;
    protected internal byte FSides;
    // IsoHex.pas:204
    public override void Destroy()
    {
        if (FDynamic != null)
        {
            FDynamic.Free();
            FDynamic = null;
        }

        base.Destroy();
    }

    // IsoHex.pas:198


    // IsoHex.pas:224
    public override void Disconnect()
    {
        if (FDynamic != null)
        {
            FDynamic.Free();
            FDynamic = null;
        }

        base.Disconnect();
    }

    // IsoHex.pas:234
    public override void NeighbourTerrainChanged(int HN, in THSTerrainInfo Info)
    {
        UpdateTransitions();
    }

    // IsoHex.pas:239
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        TCustomImageLibrary IL;
        int Xhp = default;
        int Yhp = default;
        HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
        Xhp = Xhp - XhpSurf - 8;
        Yhp = Yhp - YhpSurf;
        IL = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        IL[(int)(((uint)FSides) >> 6) + 40].Show(Xhp, Yhp, SurfaceDesc);
        Xhp -= 6;
        Yhp -= 9;
        if ((FSides & 1) != 0)
        {
            IL[31].Show(Xhp, Yhp, SurfaceDesc);
        }

        if ((FSides & 2) != 0)
        {
            IL[32].Show(Xhp, Yhp, SurfaceDesc);
        }

        if ((FSides & 32) != 0)
        {
            IL[30].Show(Xhp, Yhp, SurfaceDesc);
        }
    }

    // IsoHex.pas:256
    public virtual void ShowDynamic(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
    }

    // IsoHex.pas:50



}

// owned by its TIsometricHexagon (FOwner set directly, so it is not added as a child) and shown
// from the map field of that hexagon
public partial class TDynamicIsometricHexagon : THexagonSprite
{
    // IsoHex.pas:126
    public new const int ClassID = 131439;

    // IsoHex.pas:136
    public override TSet<TEControlStyleElement> GetControlStyle() => TSet<TEControlStyleElement>.Of();

    // IsoHex.pas:161
    public override void SetVisible(bool Value)
    {
    }

    // IsoHex.pas:165
    public override bool GetVisible() => false;

    // IsoHex.pas:131
    public override bool CanSelect(int Xhp, int Yhp) => false;

    // IsoHex.pas:141
    public override TSet<TControlModeElement> ControlMode() => TSet<TControlModeElement>.Of();

}

// FDynamicLevel (26)
public partial class TIsometricHexagonResource : TAbstractHexagonResource
{
    public TIsometricHexagonResource(TECustomNode owner) : base(owner)
    {
        FDynamicLevel = 26;
    }


    // IsoHex.pas:268
    public new const int ClassID = 131438;

    protected internal byte FDynamicLevel;
    // IsoHex.pas:262


    // IsoHex.pas:302
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 65621)
        {
            Msg.Result = 5;
        }
    }

    // IsoHex.pas:273
    public override TMapObject PlaceResourceUser(THSMap Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        TMapField MapField;
        MapField = Map.MapContainer.GetField(Xhx, Yhx, Lhx);
        TMapObject Result = (TMapObject)MapField.FindOwnedHS(typeof(TIsometricHexagon));
        if (Result != null)
        {
            if (Result.GetTerrain(0) != FTerrainList.GetTerrain(0))
            {
                Result.SetResource(this);
                MapField.UpdateTerrainType();
                ((TIsometricHexagon)Result).UpdateTransitions();
            }
            else
            {
                Result.SetResource(this);
            }

            Map.InvalidateStaticScene(Xhx, Yhx, Lhx);
        }
        else
        {
            Result = (TMapObject)CreateResourceUser();
            if (!Map.PlaceHX(Result, Xhx, Yhx, Lhx))
            {
                Result.Free();
                Result = null;
            }
        }

        return Result;
    }

    // IsoHex.pas:64


}

public partial class TUpperIsometricHexagon : TIsometricHexagon
{
    public TUpperIsometricHexagon(TECustomNode owner) : base(owner)
    {}


    public new const int ClassID = 131440;

    // IsoHex.pas:355
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        TCustomImageLibrary IL;
        int Xhp = default;
        int Yhp = default;
        if (FDynamic == null)
        {
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = Xhp - XhpSurf - 8;
            Yhp = Yhp - YhpSurf;
            IL = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
            IL[(int)(((uint)FSides) >> 6) + 40].Show(Xhp, Yhp, SurfaceDesc);
            Xhp -= 6;
            Yhp -= 9;
            if ((FSides & 1) != 0)
            {
                IL[31].Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((FSides & 2) != 0)
            {
                IL[32].Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((FSides & 32) != 0)
            {
                IL[30].Show(Xhp, Yhp, SurfaceDesc);
            }
        }
    }

}

public partial class TUpperIsometricHexagonResource : TIsometricHexagonResource
{
    public TUpperIsometricHexagonResource(TECustomNode owner) : base(owner)
    {}


    public new const int ClassID = 131441;

    // IsoHex.pas:443
    public override int ResourceUserClassID() => 131440;

    // IsoHex.pas:448
    public override string ResourceEditName() => UpperIsometricHexagonStr;

}

public partial class TLowerIsometricHexagon : TIsometricHexagon
{
    public TLowerIsometricHexagon(TECustomNode owner) : base(owner)
    {}


    public new const int ClassID = 131442;

}

public partial class TLowerIsometricHexagonResource : TIsometricHexagonResource
{
    public TLowerIsometricHexagonResource(TECustomNode owner) : base(owner)
    {}


    public new const int ClassID = 131443;

    // IsoHex.pas:581
    public override int ResourceUserClassID() => 131442;

    // IsoHex.pas:576
    public override string ResourceEditName() => LowerIsometricHexagonStr;

}

public static partial class IsoHexUnit
{
    // IsoHex.pas:586
    public static void RegisterIsometricHexagon(TEngine Engine)
    {
        Engine.RegisterEClasses([EClass.Of<TIsometricHexagon>(), EClass.Of<TIsometricHexagonResource>(), EClass.Of<TUpperIsometricHexagon>(), EClass.Of<TUpperIsometricHexagonResource>(), EClass.Of<TLowerIsometricHexagon>(), EClass.Of<TLowerIsometricHexagonResource>()]);
    }
}
