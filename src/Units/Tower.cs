// Translated from Tower.pas.
using Aow.Game.Ai;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

public enum TTowerStateElement : byte
{
    tsRazed = 0
}

// FState, bit 0 razed
public partial class TTower : TPlayerStructure
{
    public TTower(TECustomNode owner) : base(owner)
    { }

    // Tower.pas:55
    public new const int ClassID = 132158;

    public TSet<TTowerStateElement> FState;

    // Tower.pas:96
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        RwSet(ref FState, 1, 20, Stream.rwData, 0);
    }

    // Tower.pas:102
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (FResource.TerrainList.Terrain[Msg.HN] >= 0 && Msg.MsgID == 4387 && Msg.Param[0].As<TMapField>().FindNoneTransparentHS(typeof(TTower)) == this && !GetBuilding() && !GetRazed())
        {
            Msg.Result = 2;
        }
    }

    // Tower.pas:85
    public override void SetRazed(bool Value)
    {
        if (Value)
        {
            SetPlayer(-1);
            FState = FState + TSet<TTowerStateElement>.Of(TTowerStateElement.tsRazed);
        }
        else
        {
            FState = FState - TSet<TTowerStateElement>.Of(TTowerStateElement.tsRazed);
        }

        FImageIndex = GetTerrainTypeImage(FTerrainType);
    }

    // Tower.pas:82
    public override bool GetRazed() => FState.Contains(TTowerStateElement.tsRazed);

    // Tower.pas:73
    public override void UpdateAITarget(in TAITargetInfo Info, ref TAITargetData Data)
    {
        base.UpdateAITarget(Info, ref Data);
        if (!FState.Contains(TTowerStateElement.tsRazed))
        {
            switch (Info.Mode)
            {
                case TAITargetMode.tmFortify:
                    if (Info.Player == FPlayer)
                    {
                        Data.Priority = 50;
                    }

                    break;
            }
        }
    }

    // Tower.pas:58
    public override int VisibilityRange()
    {
        int Result;
        if (!GetBuilding())
        {
            Result = 12;
            if (GetLhx() != 0 || FPlayer > 0 && (((byte)AoWHSMap.FPlayers.GetPlayers(FPlayer).FMagicControl.FEnemyEnchantments.Image) & 8) != 0)
            {
                Result = (int)(((uint)(Result + 1)) >> 1);
            }
        }
        else
        {
            Result = 4;
        }

        return Result;
    }

    // Tower.pas:70
    public override int GetDefaultAITargetPriority() => 1;

}

public partial class TTowerResource : TPlayerStructureResource
{
    public TTowerResource(TECustomNode owner) : base(owner)
    {
        FTerrainBound = true;
        FClaimed = false;
        FRazeable = true;
        FRazeDefenderBudget = 50;
        FRebuildGold = 25;
        FRebuildDays = 2;
    }

    // Tower.pas:124
    public new const int ClassID = 132159;

    // Tower.pas:110

    // Tower.pas:127
    public override int ResourceUserClassID() => 132158;

    // Tower.pas:35

    // Tower.pas:121
    public override string ResourceEditName() => TranslateRStr(TowerRStr);

}

public partial class TTowerConstructionControl : TStructureConstructionControl
{
    public TTowerConstructionControl(TECustomNode owner) : base(owner)
    { }

    // Tower.pas:139
    public override void GetInfo(TAbstractUnit AUnit, ref string Text, ref int Gold, ref int Days)
    {
        var tower = TranslateRStr(TowerRStr);
        Text = RtlUnit.Format(TranslateRStr(BuildXRStr), [tower]);
        Gold = 50;
        Days = 2;
    }

    // Tower.pas:146
    public override bool CanConstruct(TAbstractUnit AUnit, ref string Reason)
    {
        bool Result;
        sbyte Xhx = default;
        sbyte Yhx = default;
        sbyte Lhx = default;
        AUnit.GetLocation(ref Xhx, ref Yhx, ref Lhx);
        if (Lhx == 0)
        {
            Result = base.CanConstruct(AUnit, ref Reason);
        }
        else
        {
            Result = false;
            var tower = TranslateRStr(TowerRStr);
            Reason = RtlUnit.Format(TranslateRStr(CannotBuildXInUndergroundRStr), [tower]);
        }

        return Result;
    }

    // Tower.pas:133
    public override TStructureResource StructureResource(TAbstractUnit AUnit) => (TStructureResource)AoWHSSet.MapObjectResourceList.FindChild(132159);

    // Tower.pas:130
    public override int ID() => 3;

}

public static partial class TowerUnit
{
    // Tower.pas:159
    public static void RegisterTower(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TTower>(), EClass.Of<TTowerResource>()]);
        var towerConstructionControl = new TTowerConstructionControl(null);
        ((TConstructAbility)AoWHSSet.FAbilityControl.GetAbility(125)).RegisterConstructionControl(towerConstructionControl);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
