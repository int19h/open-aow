// Translated from PoisonC.pas.
using System.Runtime.CompilerServices;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World;

// FPlayer, FTurnsRemaining, FDrifting, FDirection, FUpdated
public partial class PoisonCloud : MultiHexMO
{
    public PoisonCloud(ECustomNode owner) : base(owner)
    {
        FDrifting = false;
    }

    // PoisonC.pas:64
    public new const int ClassID = 135743;

    public sbyte FPlayer;
    public int FTurnsRemaining;
    public bool FDrifting;
    public byte FDirection;
    public bool FUpdated;

    // PoisonC.pas:149
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 15);
        Stream.rwInteger(ref FTurnsRemaining, 16);
        Stream.rwBool(ref FDrifting, 17);
        Stream.rwByte(ref FDirection, 18);
    }

    // PoisonC.pas:58

    // PoisonC.pas:275
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986 && Msg.Payload.Int == FPlayer && !FUpdated)
        {
            UpdateCloud();
            return;
        }

        if (Msg.MsgID == 537001987 && Msg.Payload.Int == FPlayer)
        {
            FUpdated = false;
        }
    }

    // PoisonC.pas:67
    public override int GetShowPriority() => base.GetShowPriority() + 201326592;

    // PoisonC.pas:89
    public override bool PlaceOnMap()
    {
        bool Result = base.PlaceOnMap();
        if (Result)
        {
            var yhx = GetYhx();
            var xhx = GetXhx();
            UpdateMapFields((MapLevel)Owner.Owner, xhx, yhx);
        }

        return Result;
    }

    // PoisonC.pas:111
    public override void RemoveFromMap()
    {
        MapLevel Level;
        Level = (MapLevel)Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(Level, FXhx, FYhx);
    }

    // PoisonC.pas:182
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        if (Msg.MsgID != 16 || AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
        {
            base.MapFieldMsgProc(ref Msg);
        }

        if (FResource.TerrainList.Terrain[Msg.HN] >= -1)
        {
            if (Msg.MsgID == 4418)
            {
                ArmyPlaced(Msg.Param[0].As<ArmyHS>());
            }
            else
            {
                if (Msg.MsgID == 4384)
                {
                    if (!Msg.Param[0].As<ArmyHS>().FArmy.ImmunityTypes(255).Contains(DamageType.Poison))
                    {
                        Msg.Result = 1;
                    }
                }
                else
                {
                    if (Msg.MsgID == 4432)
                    {
                        if ((byte)Msg.Param[0].Int == 1 && Msg.Param[1].Int == 0 && Msg.HN == 0)
                        {
                            Dispose();
                        }
                    }
                }
            }
        }
    }

    // PoisonC.pas:119
    public override int CanMoveOver(in HSSenderInfo Info)
    {
        int Result;
        if (FResource.TerrainList.Terrain[Info.HN] >= -1)
        {
            if (!CanDoDamage(((MoveControl)Info.Sender).FSettings.FUnits))
            {
                Result = 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            Result = 1;
        }

        return Result;
    }

    // PoisonC.pas:198
    public override void Activate()
    {
        base.Activate();
        if (FDirection == 0)
        {
            FDirection = (byte)(AoWHSMap.Current.Random(6) + 1);
        }
    }

    // PoisonC.pas:83
    public override void UpdateMapField(in HSMapFieldInfo Info)
    {
        if (FResource.TerrainList.Terrain[Info.HN] >= -1)
        {
            ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 8192;
        }
    }

    // PoisonC.pas:70
    public override bool CanSelect(int Xhp, int Yhp)
    {
        bool Result;
        int Xhx = default;
        int Yhx = default;
        int HN;
        if (FResource.Engine.HSMap.Status.Contains(HSMapStatusElement.Editting))
        {
            HexGeometry.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
            HN = HexGeometry.dHXtoHN(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, Xhx, Yhx);
            Result = HN < FResource.TerrainList.Count && FResource.TerrainList.Terrain[HN] >= -1;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // PoisonC.pas:95
    public new void UpdateMapFields(MapLevel Level, int Xhx, int Yhx)
    {
        HNWalkData Walk = default;
        int Count;
        Count = FResource.TerrainList.Count;
        HNWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
        while (Walk.HN != Count)
        {
            if (GetTerrain(Walk.HN) >= -1 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Level.GetField(Walk.X, Walk.Y).UpdateMapField();
            }

            HNWalkData.WalkToNextHN(ref Walk);
        }
    }

    // PoisonC.pas:129
    public bool CanDoDamage(UnitList Units)
    {
        int I;
        AbstractUnit AUnit;
        bool Result = false;
        I = Units.GetCount();
        while (!Result && I != 0)
        {
            I -= 1;
            AUnit = Units.GetUnit(I);
            Result = !AUnit.GetImmunityTypes().Contains(DamageType.Poison) && !AUnit.GetAbilityEnabled(1);
        }

        return Result;
    }

    // PoisonC.pas:142
    public void ArmyPlaced(ArmyHS ArmyHS)
    {
        if (!AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting) && !((AoWMapField)ArmyHS.Owner).GetMagicShieldActive())
        {
            ArmyHS.FArmy.IncommingStorm(StormType.Pestilence, 0, ArmyHS.FArmy.FPlayer);
        }
    }

    // PoisonC.pas:158
    public void TriggerDamage()
    {
        MapFieldMsg Msg = default;
        HNWalkData Walk = default;
        MapLevel Level;
        Level = (MapLevel)Owner.Owner;
        var yhx = GetYhx();
        var xhx = GetXhx();
        HNWalkData.InitHNWalk(ref Walk, xhx, yhx, 0);
        Msg.MsgID = 4432;
        Msg.Param[0] = (int)StormType.Pestilence;
        Msg.Param[2] = FPlayer;
        ref var walk = ref Walk;
        while (FResource.TerrainList.Count > walk.HN)
        {
            if (FResource.TerrainList.GetTerrain(walk.HN) >= -1 && walk.X >= 0 && walk.Y >= 0 && walk.X < Level.Width && Level.Height > walk.Y)
            {
                Msg.Param[1] = walk.Radius;
                Level.GetField(walk.X, walk.Y).SendMapFieldMsg(ref Msg);
            }

            HNWalkData.WalkToNextHN(ref Walk);
        }
    }

    // PoisonC.pas:204
    public bool ValidMoveDir(int Direction)
    {
        int Xhx = default;
        int Yhx = default;
        MapLevel Level;
        Level = (MapLevel)Owner.Owner;
        var yhx = GetYhx();
        HexGeometry.CenterHNtoHX(GetXhx(), yhx, Direction, ref Xhx, ref Yhx);
        HexGeometry.CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
        return Xhx >= 10 && Yhx >= 10 && Xhx + 10 <= Level.Width && Yhx + 10 <= Level.Height;
    }

    // PoisonC.pas:216
    public void UpdateCloud()
    {
        int Xhx = default;
        int Yhx = default;
        ByteList Directions;
        bool Valid;
        int Direction;
        int Tries;
        int Lhx;
        int OldXhx;
        int OldYhx;
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
                Directions = new ByteList();
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
                    Direction = Directions[AoWHSMap.Current.Random(Directions.Count)];
                    Valid = ValidMoveDir(Direction);
                    if (!Valid)
                    {
                        Directions.Remove((byte)Direction);
                    }
                }

                Directions?.Dispose();
                if (!Valid)
                {
                    Tries = 20;
                    while (!Valid && Tries != 0)
                    {
                        Direction = AoWHSMap.Current.Random(6) + 1;
                        Valid = ValidMoveDir(Direction);
                        Tries -= 1;
                    }
                }

                if (Valid)
                {
                    OldXhx = GetXhx();
                    OldYhx = GetYhx();
                    FDirection = (byte)Direction;
                    Lhx = GetLhx();
                    var yhx = GetYhx();
                    HexGeometry.CenterHNtoHX(GetXhx(), yhx, Direction, ref Xhx, ref Yhx);
                    HexGeometry.CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
                    Disconnect();
                    UpdateMapFields(AoWHSMap.Current.MapContainer.GetMapLevel(Lhx), OldXhx, OldYhx);
                    FXhx = (sbyte)Xhx;
                    FYhx = (sbyte)Yhx;
                    Owner = AoWHSMap.Current.MapContainer.GetField(Xhx, Yhx, Lhx);
                    Connect();
                    UpdateMapFields(AoWHSMap.Current.MapContainer.GetMapLevel(Lhx), Xhx, Yhx);
                }
            }

            TriggerDamage();
        }
        else
        {
            Dispose();
        }
    }

    public new partial class Resource : MultiHexMOR
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // PoisonC.pas:286
        public new const int ClassID = 135744;

        // PoisonC.pas:292
        public override int ResourceUserClassID() => 135743;

        // PoisonC.pas:289
        public override string ResourceEditName() => AbilityRules.TranslateRStr(PoisonCloudRStr);

        private static readonly string PoisonCloudRStr = "Poison Cloud";

    }

    // PoisonC.pas:27

}
