// Translated from SmokeCloud.pas.
using Aow.Rtl;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngineUnit;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.SmokeCloud;

public enum TCloudEnchantmentOptionsElement : byte
{
    ce0 = 0,
    ce1 = 1,
    ce2 = 2,
    ce3 = 3,
    ce4 = 4,
    ce5 = 5,
    ce6 = 6,
    ce7 = 7
}

// FCloud
public partial class TSmokeCloudEnchantment : TGlobalEnchantment
{
    // SmokeCloud.pas:99
    public new const int ClassID = 135748;

    public TSmokeCloud FCloud;
    // SmokeCloud.pas:91
    public TSmokeCloudEnchantment(TECustomNode owner) : base(owner)
    {
        FEnchantmentMode = TSet<TGlobalEnchantmentModeElement>.FromMask((TSet<TCloudEnchantmentOptionsElement>.FromMask(FEnchantmentMode.Mask) - TSet<TCloudEnchantmentOptionsElement>.Of(TCloudEnchantmentOptionsElement.ce0, TCloudEnchantmentOptionsElement.ce1)).Mask);
        FDispelMana = 25;
    }

    // SmokeCloud.pas:102
    public override string GetName() => AoWHSSet.FSpellControl.GetSpell(74).FSpellName;

    // SmokeCloud.pas:156
    public override bool VisibleForPlayer(sbyte Player)
    {
        THNWalkData Walk = default;
        bool Result = FPlayerIndex == Player;
        ref var walk = ref Walk;
        var yhx = FCloud.Yhx;
        var xhx = FCloud.Xhx;
        InitHNWalk(ref Walk, xhx, yhx, 0);
        while (!Result && walk.Radius <= 3)
        {
            Result = AoWHSMap.VisibleForPlayer(walk.X, walk.Y, FCloud.Lhx, Player);
            WalkToNextHN(ref Walk);
        }

        return Result;
    }

    // The unused fourth byte in the original register result has no defined value.
    public override TXYL GetLocation() => new() { X = FCloud.GetXhx(), Y = FCloud.GetYhx(), L = FCloud.GetLhx() };
    public override bool Deactivate(byte player)
    {
        if ((FEnchantmentStatus.Image & 1) == 0) return false;
        bool result = base.Deactivate(player);
        if ((FCloud.FStatus.Image & 0x20) != 0) FCloud.Free();
        return result;
    }
}

// FPlayer, FTurnsRemaining, FDrifting, FDirection, FUpdated, FVisibleForSeatedPlayer,
// FEnchantment
public partial class TSmokeCloud : TMultiHexMO
{
    public TSmokeCloud(TECustomNode owner) : base(owner)
    {
        FEnchantment = new TSmokeCloudEnchantment(null);
        FEnchantment.FCloud = this;
        FVisibleForSeatedPlayer = false;
        FDrifting = false;
    }

    // SmokeCloud.pas:187
    public new const int ClassID = 135746;

    public sbyte FPlayer;
    public int FTurnsRemaining;
    public bool FDrifting;
    public byte FDirection;
    public bool FUpdated;
    public bool FVisibleForSeatedPlayer;
    public TSmokeCloudEnchantment FEnchantment;
    // SmokeCloud.pas:181
    public override void Destroy()
    {
        base.Destroy();
        FEnchantment.Release();
    }

    // SmokeCloud.pas:206
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 15);
        Stream.rwInteger(ref FTurnsRemaining, 16);
        Stream.rwBool(ref FDrifting, 17);
        Stream.rwByte(ref FDirection, 18);
        Stream.rwEObject(FEnchantment, 19);
    }

    // SmokeCloud.pas:172


    // SmokeCloud.pas:373
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986 && Msg.Value == FPlayer && !FUpdated)
        {
            UpdateCloud();
            return;
        }

        if (Msg.MsgID == 537001987 && Msg.Value == FPlayer)
        {
            FUpdated = false;
        }
        else
        {
            if (Msg.MsgID == 537001993)
            {
                UpdateVisibleForSeatedPlayer();
            }
        }
    }

    // SmokeCloud.pas:190
    public override int GetShowPriority() => base.GetShowPriority() + 201326592;

    // SmokeCloud.pas:300
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        if (FVisibleForSeatedPlayer || AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }
    }

    // SmokeCloud.pas:266
    public override bool PlaceOnMap()
    {
        bool Result = base.PlaceOnMap();
        if (Result)
        {
            UpdateMapFields((TMapLevel)Owner.Owner, FXhx, FYhx);
        }

        return Result;
    }

    // SmokeCloud.pas:272
    public override void RemoveFromMap()
    {
        TMapLevel Level;
        Level = (TMapLevel)Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(Level, FXhx, FYhx);
    }

    // SmokeCloud.pas:245
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        if (Msg.MsgID != 16 || AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
        {
            base.MapFieldMsgProc(ref Msg);
        }

        if (FResource.TerrainList.Terrain[Msg.HN] >= -1)
        {
            if (Msg.MsgID == 4392 && FStatus.Contains(TMapObjectStatusElement.moActivated))
            {
                Msg.Param[0] = 1;
            }
            else
            {
                if (Msg.MsgID == 4365)
                {
                    UpdateVisibleForSeatedPlayer();
                }
            }
        }
    }

    // SmokeCloud.pas:281
    public override void Activate()
    {
        base.Activate();
        if (FDirection == 0)
        {
            FDirection = (byte)(AoWHSMap.Random(6) + 1);
        }

        var smokeCloudEnchantment = FEnchantment;
        if (smokeCloudEnchantment.FPlayerIndex > 0 && !smokeCloudEnchantment.FEnchantmentStatus.Contains(TGlobalEnchantmentStatusElement.getActivated))
        {
            smokeCloudEnchantment.Activate(smokeCloudEnchantment.FPlayerIndex);
        }

        var yhx = Yhx;
        UpdateConcealment(Xhx, yhx, Lhx);
        UpdateVisibleForSeatedPlayer();
    }

    // SmokeCloud.pas:292
    public override void Deactivate()
    {
        base.Deactivate();
        if (FEnchantment.FPlayerIndex > 0)
        {
            FEnchantment.Deactivate((byte)FEnchantment.FPlayerIndex);
        }

        var lhx = GetLhx();
        var yhx = GetYhx();
        UpdateConcealment(GetXhx(), yhx, lhx);
    }

    // SmokeCloud.pas:238
    public override void UpdateMapField(in THSMapFieldInfo Info)
    {
        if (Info.HN > 18 && Info.HN < 37)
        {
            ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 16777216;
        }
    }

    // SmokeCloud.pas:193
    public override bool CanSelect(int Xhp, int Yhp)
    {
        bool Result;
        int Xhx = default;
        int Yhx = default;
        int HN;
        if (FResource.Engine.HSMap.Status.Contains(THSMapStatusElement.msEditting))
        {
            HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
            HN = dHXtoHN(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, Xhx, Yhx);
            Result = HN < FResource.TerrainList.Count && FResource.TerrainList.Terrain[HN] >= -1;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // SmokeCloud.pas:216
    public void UpdateConcealment(sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        THNWalkData Walk = default;
        TMapLevel Level;
        TAoWMapField MapField;
        TArmyHS ArmyHS;
        Level = AoWHSMap.MapContainer.MapLevels[Lhx];
        InitHNWalk(ref Walk, Xhx, Yhx, 0);
        while (FResource.TerrainList.Count > Walk.HN)
        {
            if (FResource.TerrainList.Terrain[Walk.HN] >= -1 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Level.Height > Walk.Y)
            {
                MapField = (TAoWMapField)Level.GetField(Walk.X, Walk.Y);
                ArmyHS = MapField.GetArmyHS();
                if (ArmyHS != null)
                {
                    ArmyHS.FArmy.Update();
                }
            }

            WalkToNextHN(ref Walk);
        }
    }

    // SmokeCloud.pas:253
    public void UpdateVisibleForSeatedPlayer()
    {
        bool Visible;
        var lhx = GetLhx();
        var yhx = GetYhx();
        var xhx = GetXhx();
        Visible = AoWHSMap.Visible(xhx, yhx, lhx, 3) || FEnchantment.FPlayerIndex == AoWHSMap.FSeatedPlayer;
        if (FVisibleForSeatedPlayer ^ Visible)
        {
            FVisibleForSeatedPlayer = Visible;
            AoWHSMap.FGlobalMagicControl.TriggerChangedEvent();
        }
    }

    // SmokeCloud.pas:306
    public bool ValidMoveDir(int Direction)
    {
        int Xhx = default;
        int Yhx = default;
        TMapLevel Level;
        Level = (TMapLevel)Owner.Owner;
        var yhx = GetYhx();
        CenterHNtoHX(GetXhx(), yhx, Direction, ref Xhx, ref Yhx);
        CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
        return Xhx >= 10 && Yhx >= 10 && Level.Width >= Xhx + 10 && Level.Height >= Yhx + 10;
    }

    // SmokeCloud.pas:317
    public void UpdateCloud()
    {
        int Xhx = default;
        int Yhx = default;
        TByteList Directions;
        bool Valid;
        int Direction;
        int Tries;
        int Lhx;
        if (FTurnsRemaining != 1)
        {
            FUpdated = true;
            if (FTurnsRemaining != 0)
            {
                FTurnsRemaining -= 1;
            }

            if (FDrifting)
            {
                Direction = 0;
                Directions = new TByteList();
                Valid = false;
                Directions.Add(FDirection);
                if (FDirection != 1)
                {
                    Directions.Add((byte)(FDirection - 1));
                }
                else
                {
                    Directions.Add(6);
                }

                if (FDirection != 6)
                {
                    Directions.Add((byte)(FDirection + 1));
                }
                else
                {
                    Directions.Add(1);
                }

                while (Directions.Count != 0 && !Valid)
                {
                    Direction = Directions[AoWHSMap.Random(Directions.Count)];
                    Valid = ValidMoveDir(Direction);
                    if (!Valid)
                    {
                        Directions.Remove((byte)Direction);
                    }
                }

                Directions.Free();
                if (!Valid)
                {
                    Tries = 20;
                    while (!Valid && Tries != 0)
                    {
                        Direction = AoWHSMap.Random(6) + 1;
                        Valid = ValidMoveDir(Direction);
                        Tries -= 1;
                    }
                }

                if (Valid)
                {
                    FDirection = (byte)Direction;
                    Lhx = GetLhx();
                    var yhx = GetYhx();
                    CenterHNtoHX(GetXhx(), yhx, Direction, ref Xhx, ref Yhx);
                    CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
                    Disconnect();
                    UpdateConcealment(FXhx, FYhx, (sbyte)Lhx);
                    FXhx = (sbyte)Xhx;
                    FYhx = (sbyte)Yhx;
                    Owner = AoWHSMap.MapContainer.GetField(Xhx, Yhx, Lhx);
                    Connect();
                    UpdateConcealment(FXhx, FYhx, (sbyte)Lhx);
                }
            }
        }
        else
        {
            Destroy();
        }
    }

    // SmokeCloud.pas:43


}

public partial class TSmokeCloudResource : TMultiHexMOR
{
    public TSmokeCloudResource(TECustomNode owner) : base(owner)
    {}

    // SmokeCloud.pas:385
    public new const int ClassID = 135747;

    // SmokeCloud.pas:391
    public override int ResourceUserClassID() => 135746;

    // SmokeCloud.pas:388
    public override string ResourceEditName() => TranslateRStr(SmokeCloudRStr);

}

public struct TCloudSpellFields
{
    public FixedArray_byte_8 Reserved;
    private string name;
    public string Name { readonly get => name ?? ""; set => name = value; }
}

[StructLayout(LayoutKind.Explicit, Size = 166)]
public struct TCloudMapFields
{
    [FieldOffset(0)]
    public FixedArray_byte_165 Reserved;
    [FieldOffset(165)]
    public byte SeatedPlayer;
}

public static partial class SmokeCloudUnit
{
    // SmokeCloud.pas:394
    public static void RegisterSmokeCloud(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TSmokeCloud>(), EClass.Of<TSmokeCloudResource>(), EClass.Of<TSmokeCloudEnchantment>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
