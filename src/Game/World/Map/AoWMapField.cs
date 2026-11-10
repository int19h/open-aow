// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.Magic;

using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// flags, byte fog count, bit $02000000 owner query
public partial class AoWMapField : MapField
{
    public AoWMapField(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131090;

    public int FFieldFlags;

    // AoWHSMap.PAS:63
    // { Original tests the entire ECX register for a Boolean passed in CL. }
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwWord(ref Unsafe.As<int, LongRec>(ref FFieldFlags).Lo, 4);
    }

    // AoWHSMap.PAS:85
    public override void UpdateMapField()
    {
        FFieldFlags = FFieldFlags & -57345;
        base.UpdateMapField();
    }

    // AoWHSMap.PAS:68
    public void FogChanged()
    {
        MapFieldMsg Msg = default;
        Msg.MsgID = 4358;
        SendMapFieldMsg(ref Msg);
    }

    // AoWHSMap.PAS:75
    public void SetFogCount(byte Value)
    {
        if ((Value == 0 && Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref FFieldFlags).Hi).Lo != 0) || (Value != 0 && Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref FFieldFlags).Hi).Lo == 0))
        {
            Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref FFieldFlags).Hi).Lo = Value;
            FogChanged();
        }
        else
        {
            Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref FFieldFlags).Hi).Lo = Value;
        }
    }

    // AoWHSMap.PAS:90
    public sbyte GetPlayer()
    {
        sbyte Result;
        MapFieldMsg Msg = default;
        if ((FFieldFlags & 33554432) != 0)
        {
            Msg.MsgID = 4352;
            Msg.Result = -1;
            SendMapFieldMsg(ref Msg);
            Result = (sbyte)Msg.Result.Int;
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // AoWHSMap.PAS:103
    public int GetScannerIcon()
    {
        MapFieldMsg Msg = default;
        Msg.MsgID = 4364;
        Msg.Param[0] = 0;
        Msg.Result = 0;
        SendMapFieldMsg(ref Msg);
        return Msg.Param[0].Int;
    }

    // AoWHSMap.PAS:113
    public int GetCombatTerrainTag()
    {
        MapFieldMsg Msg = default;
        Msg.MsgID = 4386;
        Msg.Result = 0;
        SendMapFieldMsg(ref Msg);
        return Msg.Result.Int;
    }

    // AoWHSMap.PAS:122
    public bool GetConcealmentActive()
    {
        MapFieldMsg Msg = default;
        Msg.MsgID = 4392;
        Msg.Result = 0;
        Msg.Param[0] = 0;
        SendMapFieldMsg(ref Msg);
        return Msg.Param[0].Int != 0;
    }

    // AoWHSMap.PAS:132
    public bool GetMagicShieldActive()
    {
        MapFieldMsg Msg = default;
        Msg.MsgID = 4393;
        Msg.Result = 0;
        Msg.Param[0] = 0;
        SendMapFieldMsg(ref Msg);
        return Msg.Param[0].Int != 0;
    }

    // AoWHSMap.PAS:142
    public ProductionControl GetProductionControl()
    {
        MapFieldMsg Msg = default;
        Msg.MsgID = 4609;
        Msg.Result = 0;
        SendMapFieldMsg(ref Msg);
        return Msg.Result.As<ProductionControl>();
    }

    // AoWHSMap.PAS:165
    public MoveAction GetMoveAction(sbyte Player)
    {
        MapFieldMsg Msg = default;
        Msg.MsgID = 4353;
        Msg.Param[0] = Player;
        Msg.Param[1] = 0;
        SendMapFieldMsg(ref Msg);
        return (MoveAction)Msg.Param[1].Int;
    }

    // AoWHSMap.PAS:175
    public byte GetIndependentRelation()
    {
        MapFieldMsg Msg = default;
        Msg.MsgID = 4389;
        Msg.Result = -1;
        SendMapFieldMsg(ref Msg);
        return (byte)Msg.Result.Int;
    }

    // AoWHSMap.PAS:184
    public byte GetDefensePriority()
    {
        MapFieldMsg Msg = default;
        Msg.MsgID = 4390;
        Msg.Result = 0;
        SendMapFieldMsg(ref Msg);
        return (byte)Msg.Result.Int;
    }

    // AoWHSMap.PAS:204
    public WallType GetWallType()
    {
        MapFieldMsg Msg = default;
        Msg.MsgID = 4387;
        Msg.Param[0] = this;
        Msg.Result = 0;
        SendMapFieldMsg(ref Msg);
        return (WallType)Msg.Result.Int;
    }

    // AoWHSMap.PAS:214
    public ArmyHS GetArmyHS() => (ArmyHS)FindOwnedChild(131607);

    // AoWHSMap.PAS:218
    public void RestoreTerrain()
    {
        MapFieldMsg Msg = default;
        Msg.MsgID = 4402;
        Msg.Param[0] = this;
        Msg.Result = 0;
        SendMapFieldMsg(ref Msg);
    }

    public bool GetExplored(int player)
    {
        int count = player & 31;
        return count == 0 || ((uint)FFieldFlags & (1u << (count - 1))) != 0;
    }

    public void SetExplored(int player, bool value) => SetExploredRaw(player, value ? 1 : 0);

    // The original tests ECX, including the bits above the Boolean byte.
    public void SetExploredRaw(int player, int value)
    {
        int mask = 1 << ((player - 1) & 31);
        if (value != 0) FFieldFlags |= mask;
        else FFieldFlags &= ~mask;
    }

    internal static int IgnitionChance(AoWMapField MapField)
    {
        int Result;
        if (Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)MapField.TerrainType) && MapField.FindOwnedChild(132990) == null && !MapField.GetMagicShieldActive())
        {
            if (MapField.OverlayType == 1)
            {
                if (MapField.TerrainType == 3)
                {
                    Result = 40;
                }
                else
                {
                    if (MapField.TerrainType == 2)
                    {
                        Result = 80;
                    }
                    else
                    {
                        Result = 60;
                    }
                }
            }
            else
            {
                if (MapField.OverlayType == 3)
                {
                    if (MapField.TerrainType == 3)
                    {
                        Result = 10;
                    }
                    else
                    {
                        if (MapField.TerrainType == 2)
                        {
                            Result = 30;
                        }
                        else
                        {
                            Result = 20;
                        }
                    }
                }
                else
                {
                    if (MapField.TerrainType == 3)
                    {
                        Result = 1;
                    }
                    else
                    {
                        Result = 5;
                    }
                }
            }

            if (MapField.FindNoneTransparentHS(typeof(Structure)) != null)
            {
                Result += 5;
                if (AoWHSMap.Current != null && AoWHSMap.Current.FGlobalMagicControl.FActiveEnchantments.Contains(GlobalEnchantmentType.FireMastery))
                {
                    Result += 5;
                }
            }

            if (Result < 0)
            {
                Result = 0;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

}
