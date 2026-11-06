// Translated from CombatILTer.pas.
using Aow.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngineUnit;
using Aow.Units.ILTer;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.CombatILTer;

public partial class TCombatILTerrainShadow : THexagonSprite
{
    public TCombatILTerrainShadow(TECustomNode owner) : base(owner)
    {}

    public TCombatILTerrain FTerrain;
    // CombatILTer.pas:82
    public override int GetShowPriority() => (FTerrain.GetShowPriority() & 67108863) - 1677721600;

    // CombatILTer.pas:96
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        FTerrain.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
    }

    // Engine.pas:524

}

public partial class TCombatILTerrain : TILTerrainMO
{
    public TCombatILTerrain(TECustomNode owner) : base(owner)
    {}

    // CombatILTer.pas:105
    public new const int ClassID = 2228534;

    public TCombatILTerrainShadow FShadow;
    // CombatILTer.pas:99
    public override void Destroy()
    {
        DestroyShadow();
        base.Destroy();
    }

    // CombatILTer.pas:166
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 135169)
        {
            if (FResource.TerrainList.GetTerrain(Msg.HN) >= 0)
            {
                if (Msg.Param[0].Int < ((TCombatILTerrainResource)FResource).FDefense)
                {
                    Msg.Param[0] = ((TCombatILTerrainResource)FResource).FDefense;
                }
            }
        }
    }

    // CombatILTer.pas:151
    public override void Activate()
    {
        base.Activate();
        CreateShadow();
    }

    // CombatILTer.pas:157
    public override void Deactivate()
    {
        DestroyShadow();
        base.Deactivate();
    }

    // CombatILTer.pas:141
    public override void MakeVisible(TVisibilityList VisibilityList)
    {
        if (!FStatus.Contains(TMapObjectStatusElement.moVisible))
        {
            base.MakeVisible(VisibilityList);
            if (FShadow != null)
            {
                var showPriority = FShadow.GetShowPriority();
                VisibilityList.AddSortChild(FShadow, showPriority);
            }
        }
    }

    // CombatILTer.pas:163
    public override TMapObjectResourceList GetResourceList(TEngine AEngine) => AoWHSSet.FCombatResources;

    // CombatILTer.pas:93
    public override bool CanPlaceOnHS(THexagonSprite HS) => base.CanPlaceOnHS(HS) || HS is TAbstractRoad;

    // CombatILTer.pas:108
    public virtual void ShowShadow(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 40, (uint)(((TMapField)Owner).Xhx + (((TMapField)Owner).Yhx << 4)));
    }

    // CombatILTer.pas:87
    public sbyte GetDefense() => ((TCombatILTerrainResource)FResource).FDefense;

    // CombatILTer.pas:90
    public sbyte GetBurnValue() => ((TCombatILTerrainResource)FResource).FBurnValue;

    // CombatILTer.pas:118
    public void DestroyShadow()
    {
        if (FShadow != null)
        {
            if (GetVisible())
            {
                FResource.Engine.HSMap.DynamicVisibleHS.RemoveChild(FShadow);
            }

            FShadow.Free();
            FShadow = null;
        }
    }

    // CombatILTer.pas:128
    public void CreateShadow()
    {
        if (FResource.ImageSequenceList.GetImageSequence(40) != null)
        {
            if (FShadow != null)
            {
                DestroyShadow();
            }

            if (AoWHSMap == null || AoWHSMap.FShowUnitShadows)
            {
                FShadow = new TCombatILTerrainShadow(null);
                FShadow.FTerrain = this;
            }
        }
    }

}

public partial class TCombatILTerrainResource : TILTerrainMOR
{
    public TCombatILTerrainResource(TECustomNode owner) : base(owner)
    {}

    // CombatILTer.pas:175
    public new const int ClassID = 2228535;

    public sbyte FBurnValue;
    public sbyte FDefense;
    // CombatILTer.pas:181
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDefense), 20);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FBurnValue), 21);
    }

    // CombatILTer.pas:188
    public override int ResourceUserClassID() => 2228534;

    // CombatILTer.pas:178
    public override string ResourceEditName() => "Combat ILTerrain";

}

public partial class TCombatFixedILTerrain : TCombatILTerrain
{
    public TCombatFixedILTerrain(TECustomNode owner) : base(owner)
    {}

    // CombatILTer.pas:191
    public new const int ClassID = 2228536;

    // CombatILTer.pas:197
    public override bool CanChangeTerrain(in THSSenderInfo Info)
    {
        bool Result;
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
        {
            bool validTerrainType = ValidTerrainType(Info.MapField.TerrainType);
            if (validTerrainType)
            {
                var terrain = FResource.TerrainList.GetTerrain(Info.HN);
                validTerrainType = GetMostFrequentMapFieldTerrainType((TMapLevel)Owner.Owner, FXhx, FYhx, terrain) == Info.MapField.TerrainType;
            }

            Result = validTerrainType;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // CombatILTer.pas:206
    public override void ChangeTerrain(in THSTerrainInfo Info)
    {
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
        {
            if (ValidTerrainType(Info.TerrainType))
            {
                FTerrainType = Info.TerrainType;
                FImageIndex = GetTerrainTypeImage(Info.TerrainType);
            }
        }
    }

    // CombatILTer.pas:216
    public override void TerrainChanged(in THSTerrainInfo Info)
    {
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
        {
            if (ValidTerrainType(Info.TerrainType))
            {
                FTerrainType = Info.TerrainType;
                FImageIndex = GetTerrainTypeImage(Info.TerrainType);
            }
            else
            {
                throw Exception.Create("TILFixedTerrain Failure");
            }
        }
    }

    // CombatILTer.pas:194
    public override sbyte ForceTerrainType(TMapLevel MapLevel, sbyte Xhx, sbyte Yhx) => -1;

}

public partial class TCombatFixedILTerrainResource : TCombatILTerrainResource
{
    public TCombatFixedILTerrainResource(TECustomNode owner) : base(owner)
    {}

    // CombatILTer.pas:227
    public new const int ClassID = 2228537;

    // CombatILTer.pas:233
    public override int ResourceUserClassID() => 2228536;

    // CombatILTer.pas:230
    public override string ResourceEditName() => "Combat FixedILTerrain";

}

public static partial class CombatILTerUnit
{
    // CombatILTer.pas:236
    public static void RegisterCombatILTerrain(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TCombatILTerrain>(), EClass.Of<TCombatILTerrainResource>(), EClass.Of<TCombatFixedILTerrain>(), EClass.Of<TCombatFixedILTerrainResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
