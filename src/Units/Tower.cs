// Translated from Tower.pas.
using Aow.Game.Ai;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// FState, bit 0 razed
public partial class Tower : PlayerStructure
{
    public Tower(ECustomNode owner) : base(owner)
    { }

    // Tower.pas:55
    public new const int ClassID = 132158;

    public Set<StateElement> FState;

    // Tower.pas:96
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        RwSet(ref FState, 1, 20, Stream.rwData, 0);
    }

    // Tower.pas:102
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (FResource.TerrainList.Terrain[Msg.HN] >= 0 && Msg.MsgID == 4387 && Msg.Param[0].As<MapField>().FindNoneTransparentHS(typeof(Tower)) == this && !GetBuilding() && !GetRazed())
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
            FState = FState + Set<StateElement>.Of(StateElement.Razed);
        }
        else
        {
            FState = FState - Set<StateElement>.Of(StateElement.Razed);
        }

        FImageIndex = GetTerrainTypeImage(FTerrainType);
    }

    // Tower.pas:82
    public override bool GetRazed() => FState.Contains(StateElement.Razed);

    // Tower.pas:73
    public override void UpdateAITarget(in AITargetInfo Info, ref AITargetData Data)
    {
        base.UpdateAITarget(Info, ref Data);
        if (!FState.Contains(StateElement.Razed))
        {
            switch (Info.Mode)
            {
                case AITargetMode.Fortify:
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
            if (GetLhx() != 0 || FPlayer > 0 && (((byte)Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FEnemyEnchantments.Image) & 8) != 0)
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

    public enum StateElement : byte
    {
        Razed = 0
    }

    public new partial class Resource : PlayerStructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
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
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TowerRStr);

    }

}

public partial class TowerConstructionControl : StructureConstructionControl
{
    public TowerConstructionControl(ECustomNode owner) : base(owner)
    { }

    // Tower.pas:139
    public override void GetInfo(AbstractUnit AUnit, ref string Text, ref int Gold, ref int Days)
    {
        var tower = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TowerRStr);
        Text = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.BuildXRStr), [tower]);
        Gold = 50;
        Days = 2;
    }

    // Tower.pas:146
    public override bool CanConstruct(AbstractUnit AUnit, ref string Reason)
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
            var tower = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TowerRStr);
            Reason = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TowerConstructionControl.CannotBuildXInUndergroundRStr), [tower]);
        }

        return Result;
    }

    // Tower.pas:133
    public override global::Aow.Game.World.Structures.StructureResource StructureResource(AbstractUnit AUnit) => (global::Aow.Game.World.Structures.StructureResource)Aow.Game.Engine.AoWHSSet.Current.MapObjectResourceList.FindChild(132159);

    // Tower.pas:130
    public override int ID() => 3;

    private static readonly string CannotBuildXInUndergroundRStr = "Cannot build %s underground";

}
