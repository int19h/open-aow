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

    public new const int ClassId = 131090;

    public int FieldFlags;

    // AoWHSMap.PAS:63
    // { Original tests the entire ECX register for a Boolean passed in CL. }
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwWord(ref Unsafe.As<int, LongRec>(ref FieldFlags).Lo, 4);
    }

    // AoWHSMap.PAS:85
    public override void UpdateMapField()
    {
        FieldFlags = FieldFlags & -57345;
        base.UpdateMapField();
    }

    // AoWHSMap.PAS:68
    public void FogChanged()
    {
        MapFieldMessage message = default;
        message.MessageId = 4358;
        SendMapFieldMessage(ref message);
    }

    // AoWHSMap.PAS:75
    public void SetFogCount(byte value)
    {
        if ((value == 0 && Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref FieldFlags).Hi).Lo != 0) || (value != 0 && Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref FieldFlags).Hi).Lo == 0))
        {
            Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref FieldFlags).Hi).Lo = value;
            FogChanged();
        }
        else
        {
            Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref FieldFlags).Hi).Lo = value;
        }
    }

    // AoWHSMap.PAS:90
    public sbyte GetPlayer()
    {
        sbyte result;
        MapFieldMessage message = default;
        if ((FieldFlags & 33554432) != 0)
        {
            message.MessageId = 4352;
            message.Result = -1;
            SendMapFieldMessage(ref message);
            result = (sbyte)message.Result.Int;
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // AoWHSMap.PAS:103
    public int GetScannerIcon()
    {
        MapFieldMessage message = default;
        message.MessageId = 4364;
        message.Param[0] = 0;
        message.Result = 0;
        SendMapFieldMessage(ref message);
        return message.Param[0].Int;
    }

    // AoWHSMap.PAS:113
    public int GetCombatTerrainTag()
    {
        MapFieldMessage message = default;
        message.MessageId = 4386;
        message.Result = 0;
        SendMapFieldMessage(ref message);
        return message.Result.Int;
    }

    // AoWHSMap.PAS:122
    public bool GetConcealmentActive()
    {
        MapFieldMessage message = default;
        message.MessageId = 4392;
        message.Result = 0;
        message.Param[0] = 0;
        SendMapFieldMessage(ref message);
        return message.Param[0].Int != 0;
    }

    // AoWHSMap.PAS:132
    public bool GetMagicShieldActive()
    {
        MapFieldMessage message = default;
        message.MessageId = 4393;
        message.Result = 0;
        message.Param[0] = 0;
        SendMapFieldMessage(ref message);
        return message.Param[0].Int != 0;
    }

    // AoWHSMap.PAS:142
    public ProductionControl GetProductionControl()
    {
        MapFieldMessage message = default;
        message.MessageId = 4609;
        message.Result = 0;
        SendMapFieldMessage(ref message);
        return message.Result.As<ProductionControl>();
    }

    // AoWHSMap.PAS:165
    public MoveAction GetMoveAction(sbyte player)
    {
        MapFieldMessage message = default;
        message.MessageId = 4353;
        message.Param[0] = player;
        message.Param[1] = 0;
        SendMapFieldMessage(ref message);
        return (MoveAction)message.Param[1].Int;
    }

    // AoWHSMap.PAS:175
    public byte GetIndependentRelation()
    {
        MapFieldMessage message = default;
        message.MessageId = 4389;
        message.Result = -1;
        SendMapFieldMessage(ref message);
        return (byte)message.Result.Int;
    }

    // AoWHSMap.PAS:184
    public byte GetDefensePriority()
    {
        MapFieldMessage message = default;
        message.MessageId = 4390;
        message.Result = 0;
        SendMapFieldMessage(ref message);
        return (byte)message.Result.Int;
    }

    // AoWHSMap.PAS:204
    public WallType GetWallType()
    {
        MapFieldMessage message = default;
        message.MessageId = 4387;
        message.Param[0] = this;
        message.Result = 0;
        SendMapFieldMessage(ref message);
        return (WallType)message.Result.Int;
    }

    // AoWHSMap.PAS:214
    public ArmyHexSprite GetArmyHexSprite() => (ArmyHexSprite)FindOwnedChild(131607);

    // AoWHSMap.PAS:218
    public void RestoreTerrain()
    {
        MapFieldMessage message = default;
        message.MessageId = 4402;
        message.Param[0] = this;
        message.Result = 0;
        SendMapFieldMessage(ref message);
    }

    public bool GetExplored(int player)
    {
        int count = player & 31;
        return count == 0 || ((uint)FieldFlags & (1u << (count - 1))) != 0;
    }

    public void SetExplored(int player, bool value) => SetExploredRaw(player, value ? 1 : 0);

    // The original tests ECX, including the bits above the Boolean byte.
    public void SetExploredRaw(int player, int value)
    {
        int mask = 1 << ((player - 1) & 31);
        if (value != 0) FieldFlags |= mask;
        else FieldFlags &= ~mask;
    }

    internal static int IgnitionChance(AoWMapField mapField)
    {
        int result;
        if (Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)mapField.TerrainType) && mapField.FindOwnedChild(132990) == null && !mapField.GetMagicShieldActive())
        {
            if (mapField.OverlayType == 1)
            {
                if (mapField.TerrainType == 3)
                {
                    result = 40;
                }
                else
                {
                    if (mapField.TerrainType == 2)
                    {
                        result = 80;
                    }
                    else
                    {
                        result = 60;
                    }
                }
            }
            else
            {
                if (mapField.OverlayType == 3)
                {
                    if (mapField.TerrainType == 3)
                    {
                        result = 10;
                    }
                    else
                    {
                        if (mapField.TerrainType == 2)
                        {
                            result = 30;
                        }
                        else
                        {
                            result = 20;
                        }
                    }
                }
                else
                {
                    if (mapField.TerrainType == 3)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 5;
                    }
                }
            }

            if (mapField.FindNoneTransparentHexSprite(typeof(Structure)) != null)
            {
                result += 5;
                if (AoWHSMap.Current != null && AoWHSMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.FireMastery))
                {
                    result += 5;
                }
            }

            if (result < 0)
            {
                result = 0;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

}
