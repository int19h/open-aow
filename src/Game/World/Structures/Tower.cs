// Translated from Tower.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// State, bit 0 razed
public partial class Tower : PlayerStructure
{
    public Tower(ECustomNode owner) : base(owner)
    { }

    // Tower.pas:55
    public new const int ClassId = 132158;

    public Set<StateElement> State;

    // Tower.pas:96
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        TransferSet(ref State, 1, 20, stream.Bytes, 0);
    }

    // Tower.pas:102
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (_resource.TerrainList.Terrain[message.HexNumber] >= 0 && message.MessageId == 4387 && message.Param[0].As<MapField>().FindNoneTransparentHexSprite(typeof(Tower)) == this && !GetBuilding() && !GetRazed())
        {
            message.Result = 2;
        }
    }

    // Tower.pas:85
    public override void SetRazed(bool value)
    {
        if (value)
        {
            SetPlayer(-1);
            State = State + Set<StateElement>.Of(StateElement.Razed);
        }
        else
        {
            State = State - Set<StateElement>.Of(StateElement.Razed);
        }

        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Tower.pas:82
    public override bool GetRazed() => State.Contains(StateElement.Razed);

    // Tower.pas:73
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        base.UpdateAITarget(info, ref data);
        if (!State.Contains(StateElement.Razed))
        {
            switch (info.Mode)
            {
                case AITargetMode.Fortify:
                    if (info.Player == Player)
                    {
                        data.Priority = 50;
                    }

                    break;
            }
        }
    }

    // Tower.pas:58
    public override int VisibilityRange()
    {
        int result;
        if (!GetBuilding())
        {
            result = 12;
            if (GetHexLevel() != 0 || Player > 0 && (((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments.Image) & 8) != 0)
            {
                result = (int)(((uint)(result + 1)) >> 1);
            }
        }
        else
        {
            result = 4;
        }

        return result;
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
            TerrainBound = true;
            Claimed = false;
            Razeable = true;
            RazeDefenderBudget = 50;
            RebuildGold = 25;
            RebuildDays = 2;
        }

        // Tower.pas:124
        public new const int ClassId = 132159;

        // Tower.pas:110

        // Tower.pas:127
        public override int ResourceUserClassId() => 132158;

        // Tower.pas:35

        // Tower.pas:121
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), GameText.TowerText);

    }

}
